using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Prague_Parking_V1._1
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
            //ternary operator to set the vehicle type
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

            //check if the vehicle is already registered or user entered a duplicate license plate
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
            //spot number is 1-100, array index is 0-99
            var vehicles = parkingSpots[spotNumber - 1];
            //check if spot is available based on vehicle type
            if (type == "CAR")
            {
                //spot can have only 0 or 1 car
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
            //take vehicle type and spot number as input
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
            //Counters for full and free spots
            int fullSpots = 0;
            int freeSpots = 0;
            List<int> fullSpotNumbers = new List<int>();
            List<int> mcAvailableSpots = new List<int>();

            Console.WriteLine("\nParking Spot Status:");

            // Iterate through all parking spots
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                // Get the current spot
                var spot = parkingSpots[i];
                string spotInfo = $"Spot {i + 1}: ";

                // Check if the spot is empty
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
                    spotInfo += string.Join("| ", vehicles);

                    // Determine if the spot is full and count accordingly
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

            //Summary of parking status
            Console.WriteLine($"\nTotal full spots: {fullSpots}");
            Console.WriteLine($"Total free spots: {freeSpots}");
            if (fullSpotNumbers.Count > 0)
            {
                Console.WriteLine("Parking Spots occupied: " + string.Join(", ", fullSpotNumbers));
            }
            else
            {
                Console.WriteLine("No occupied spots.");
            }
            if (mcAvailableSpots.Count > 0)
            {
                Console.WriteLine("Spot number with one MC (available for another MC): " + string.Join(", ", mcAvailableSpots));
            }
        }

        //Method to check out a vehicle
        public static void CheckOutVehicle()
        {
            //ask user if they want to check out by spot number or license plate
            Console.WriteLine("\nWould you like to check out by:");
            Console.WriteLine("\t1: Parking spot number");
            Console.WriteLine("\t2: License plate");
            string option = Console.ReadLine();

            //check out by spot number
            if (option == "1")
            {
                Console.WriteLine("\nEnter the parking spot number to check out from (1-100):");
                int spotNumber;
                if (!int.TryParse(Console.ReadLine(), out spotNumber) || spotNumber < 1 || spotNumber > 100)
                {
                    Console.WriteLine("\nInvalid parking spot number. Please enter a number between 1 and 100.");
                    return;
                }

                //check if the spot is empty
                var spot = parkingSpots[spotNumber - 1];
                if (spot.Count == 0)
                {
                    Console.WriteLine($"\nParking spot {spotNumber} is already empty.");
                    return;
                }

                
                Console.WriteLine("\nEnter the license plate of the vehicle to check out:");
                string licensePlate = Console.ReadLine().ToUpper();

                //find the vehicle in the spot
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

                //search all spots for the vehicle
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
        public static void MoveVehicle()
        {
            //take license plate and destination spot as input
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
            // If vehicle not found
            if (fromSpot == -1)
            {
                Console.WriteLine($"\nVehicle with license plate {licensePlate} not found in any spot.");
                return;
            }

            // Display current spot and ask for destination
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