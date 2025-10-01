using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Prague_Parking_V1
{
    internal class Program
    {
        //A list to hold the parking spots as strings(100 max,1 max for car, 2 max for motorcycle)
        public static List<string>[] parkingSpots = new List<string>[100];


        static void Main(string[] args)
        {
            //Initialize the parking spots
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                parkingSpots[i] = new List<string>();
            }

            //while loop to keep the program running until the user decides to exit
            bool running = true;
            while (running)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('=', 100));
                Console.ResetColor();

                Console.WriteLine("\n\nWelcome to Prague Parking.\n\nWhat would you like to do?");
                Console.WriteLine("\n\t1: Register vehicle\n\t2: Check out vehicle\n\t3: Move vehicle to another spot\n\t4: Check parking spot availability\n\t5: Check status of all parking spots\n\t0: Exit");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Registervehicle();
                        break;
                    case "2":
                        CheckOutVehicle();
                        break;
                    case "3":
                        MoveVehicle();
                        break;
                    case "4":
                        CheckSpotAvailability();
                        break;
                    case "5":
                        CheckParkingStatus();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("\n\nExiting Prague Parking.\nGoodbye!");
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Please select a valid option.");
                        break;
                }

            }

        }

        //Method to register a vehicle
        public static void Registervehicle()
        {
            //take the vehicle type and license plate as input
            Console.WriteLine("\nSelect vehicle type:\n\t1: Car\n\t2: Motorcycle");
            string vehicleType = Console.ReadLine();
            string type = vehicleType == "2" ? "MC" : "CAR";


            Console.WriteLine("\nEnter vehicle license plate:");
            string licensePlate = Console.ReadLine().ToUpper();



            //check for null or empty license plate
            if (string.IsNullOrEmpty(licensePlate))
            {
                Console.WriteLine("\nLicense plate cannot be empty.");
                return;
            }

            //regex pattern match to validate the license plate
            string pattern = @"^[A-Za-zÅÄÖØÆåäöøæ]{3}[0-9]{4}$";
            if (!Regex.IsMatch(licensePlate, pattern))
            {
                Console.WriteLine("\nInvalid license plate format. Please use the format ABC1234.");
                return;
            }

            Console.WriteLine("\nEnter the desired parking spot (1-100)");
            int spotNumber;
            if (!int.TryParse(Console.ReadLine(), out spotNumber) || spotNumber < 1 || spotNumber > 100 || !IsSpotAvailable(type, spotNumber))
            {
                Console.WriteLine("\nInvalid parking spot number. Please enter a number between 1 and 100.");
                return;
            }
            //register parking spot
            parkingSpots[spotNumber - 1].Add(type + "#" + licensePlate);
            Console.WriteLine($"\n\nVehicle {licensePlate} of type {type} registered at spot {spotNumber}.\n\n");

        }

        //Check if spot is available
        public static bool IsSpotAvailable(string type, int spotNumber)
        {

            var vehicles = parkingSpots[spotNumber - 1];
            if (type == "CAR")
            {
                //spot must be empty for CAR
                return vehicles.Count == 0;
            }
            else // MC
            { //spot can have 0 or 1 MC
                return vehicles.Count < 2 && (vehicles.Count == 0 || vehicles[0].StartsWith("MC"));
            }
        }

        //New method to check spot availability
        public static void CheckSpotAvailability()
        {
            Console.WriteLine("\nSelect vehicle type to check:\n\t1:Car\n\t2:Motorcycle");
            string vehicleType = Console.ReadLine();
            string type = vehicleType == "2" ? "MC" : "CAR";

            Console.WriteLine("\nEnter parking spot number to check (1-100):");
            int spotNumber;
            if (!int.TryParse(Console.ReadLine(), out spotNumber) || spotNumber < 1 || spotNumber > 100)
            {
                Console.WriteLine("\nInvalid parking spot number. Please enter a number between 1 and 100.");
                return;
            }
            if (IsSpotAvailable(type, spotNumber))
            {
                Console.WriteLine($"\nParking spot {spotNumber} is available for a {type}.");
            }
            else
            {
                Console.WriteLine($"\nParking spot {spotNumber} is NOT available for a {type}.");
            }

        }

        //Method to check the status of all parking spots
        public static void CheckParkingStatus()
        {
            //int fullSpots = 0;
            //int freeSpots = 0;
            //List <int> fullSpotNumbers = new List<int>();
            //List <int> mcAvailableSpots = new List<int>();

            //for (int i = 0; i < parkingSpots.Length; i++)
            //{
            //    var spot = parkingSpots[i];
            //    // Full: 1 car or 2 motorcycles
            //    if (spot.Count == 1 && spot[0].StartsWith("CAR"))
            //    {
            //        fullSpots++;
            //        fullSpotNumbers.Add(i + 1);
            //    }
            //    else if (spot.Count == 2 && spot[0].StartsWith("MC") && spot[1].StartsWith("MC"))
            //    {
            //        fullSpots++;
            //        fullSpotNumbers.Add(i + 1);
            //    }
            //    else if (spot.Count == 1 && spot[0].StartsWith("MC"))
            //    {
            //        // Spot has one MC, available for another MC
            //        mcAvailableSpots.Add(i + 1);
            //        freeSpots++;
            //    }
            //    else
            //    {
            //        freeSpots++;
            //    }
            //}
            //Console.WriteLine($"Total full spots: {fullSpots}");
            //Console.WriteLine($"Total free spots: {freeSpots}");
            //if (fullSpotNumbers.Count > 0)
            //{
            //    Console.WriteLine("Full spots: " + string.Join(", ", fullSpotNumbers));
            //}
            //else
            //{
            //    Console.WriteLine("No full spots.");
            //}
            //if (mcAvailableSpots.Count > 0)
            //{
            //    Console.WriteLine("Spots with one MC (available for another MC): " + string.Join(", ", mcAvailableSpots));

            //}

            int fullSpots = 0;
            int freeSpots = 0;
            List<int> fullSpotNumbers = new List<int>();
            List<int> mcAvailableSpots = new List<int>();

            Console.WriteLine("\nParking Spot Status:");
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                var spot = parkingSpots[i];
                string spotInfo = $"Spot {i + 1}: ";

                if (spot.Count == 0)
                {
                    spotInfo += "Empty";
                    freeSpots++;
                }
                else
                {
                    // Show all vehicles in the spot
                    var vehicles = spot.Select(v =>
                    {
                        var parts = v.Split('#');
                        return $"{parts[0]} ({parts[1]})";
                    });
                    spotInfo += string.Join(", ", vehicles);

                    if (spot.Count == 1 && spot[0].StartsWith("CAR"))
                    {
                        fullSpots++;
                        fullSpotNumbers.Add(i + 1);
                    }
                    else if (spot.Count == 2 && spot[0].StartsWith("MC") && spot[1].StartsWith("MC"))
                    {
                        fullSpots++;
                        fullSpotNumbers.Add(i + 1);
                    }
                    else if (spot.Count == 1 && spot[0].StartsWith("MC"))
                    {
                        mcAvailableSpots.Add(i + 1);
                        freeSpots++;
                    }
                    else
                    {
                        freeSpots++;
                    }
                }
                Console.WriteLine(spotInfo);
            }
            Console.WriteLine($"\nTotal full spots: {fullSpots}");
            Console.WriteLine($"Total free spots: {freeSpots}");
            if (fullSpotNumbers.Count > 0)
            {
                Console.WriteLine("Parking Spots occupied: " + string.Join(", ", fullSpotNumbers));
            }
            else
            {
                Console.WriteLine("No full spots.");
            }
            if (mcAvailableSpots.Count > 0)
            {
                Console.WriteLine("Spots with one MC (available for another MC): " + string.Join(", ", mcAvailableSpots));
            }
        }

        //Method to check out a vehicle
        public static void CheckOutVehicle()
        {
            Console.WriteLine("\nWould you like to check out by:");
            Console.WriteLine("\t1: Parking spot number");
            Console.WriteLine("\t2: License plate");
            string option = Console.ReadLine();

            if (option == "1")
            {
                Console.WriteLine("\nEnter the parking spot number to check out from (1-100):");
                int spotNumber;
                if (!int.TryParse(Console.ReadLine(), out spotNumber) || spotNumber < 1 || spotNumber > 100)
                {
                    Console.WriteLine("\nInvalid parking spot number. Please enter a number between 1 and 100.");
                    return;
                }

                var spot = parkingSpots[spotNumber - 1];
                if (spot.Count == 0)
                {
                    Console.WriteLine($"\nParking spot {spotNumber} is already empty.");
                    return;
                }

                Console.WriteLine("\nEnter the license plate of the vehicle to check out:");
                string licensePlate = Console.ReadLine().ToUpper();

                int index = spot.FindIndex(v => v.EndsWith("#" + licensePlate));
                if (index == -1)
                {
                    Console.WriteLine($"\nVehicle with license plate {licensePlate} not found in spot {spotNumber}.");
                    return;
                }

                spot.RemoveAt(index);
                Console.WriteLine($"\nVehicle {licensePlate} has been checked out from spot {spotNumber}.");
            }
            else if (option == "2")
            {
                Console.WriteLine("\nEnter the license plate of the vehicle to check out:");
                string licensePlate = Console.ReadLine().ToUpper();
                bool found = false;

                for (int i = 0; i < parkingSpots.Length; i++)
                {
                    var spot = parkingSpots[i];
                    int index = spot.FindIndex(v => v.EndsWith("#" + licensePlate));
                    if (index != -1)
                    {
                        spot.RemoveAt(index);
                        Console.WriteLine($"\nVehicle {licensePlate} has been checked out from spot {i + 1}.");
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"\nVehicle with license plate {licensePlate} not found in any spot.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid option. Please select 1 or 2.");
            }
        }

        //Method to move a vehicle to another spot
        /*
         * check if this method works as intended
         */
        public static void MoveVehicle()
        {
            Console.WriteLine("\nEnter the license plate of the vehicle to move:");
            string licensePlate = Console.ReadLine().ToUpper();
            int fromSpot = -1;
            string vehicleType = null;
            string vehicleString = null;

            // Find the vehicle and its current spot
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                var spot = parkingSpots[i];
                int index = spot.FindIndex(v => v.EndsWith("#" + licensePlate));
                if (index != -1)
                {
                    fromSpot = i;
                    vehicleString = spot[index];
                    vehicleType = vehicleString.StartsWith("MC") ? "MC" : "CAR";
                    break;
                }
            }

            if (fromSpot == -1)
            {
                Console.WriteLine($"\nVehicle with license plate {licensePlate} not found in any spot.");
                return;
            }

            Console.WriteLine($"\nVehicle found in spot {fromSpot + 1}.");
            Console.WriteLine("\nEnter the destination parking spot (1-100):");
            int toSpot;
            if (!int.TryParse(Console.ReadLine(), out toSpot) || toSpot < 1 || toSpot > 100)
            {
                Console.WriteLine("\nInvalid parking spot number. Please enter a number between 1 and 100.");
                return;
            }

            // Check if destination spot is available for this vehicle type
            if (!IsSpotAvailable(vehicleType, toSpot))
            {
                Console.WriteLine($"\nDestination spot {toSpot} is not available for a {vehicleType}.");
                return;
            }

            // Move the vehicle
            parkingSpots[fromSpot].Remove(vehicleString);
            parkingSpots[toSpot - 1].Add(vehicleString);
            Console.WriteLine($"\nVehicle {licensePlate} moved from spot {fromSpot + 1} to {toSpot}.");
        }
    }
}