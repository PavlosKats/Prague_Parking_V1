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
                //Console.ForegroundColor = ConsoleColor.Green;
                //Console.WriteLine(new string('=', 100));
                //Console.ResetColor();

                //Console.WriteLine("\n\nWelcome to Prague Parking.\n\nWhat would you like to do?");
                //Console.WriteLine("\n\t1: Register vehicle\n\t2: Check out vehicle\n\t3: Move vehicle to another spot\n\t4: Check parking spot availability\n\t5: Check status of all parking spots\n\t0: Exit");
                string[] menuLines = new[]
                {
                    "",
                    "Welcome to Prague Parking.",
                    "",
                    "What would you like to do?",
                    "",
                    "  1: Register vehicle",
                    "  2: Check out vehicle",
                    "  3: Move vehicle to another spot",
                    "  4: Check parking spot number availability",
                    "  5: Check status of all parking spots",
                    "  6: Optimize motorcycle parking",
                    "  0: Exit",
                    ""
                };

                // Calculate box width with LINQ .Max method, l is each line in the menuLines array
                int boxWidth = menuLines.Max(l => l.Length) + 20; // Padding 

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('=', boxWidth));
                foreach (var line in menuLines)
                {
                    Console.WriteLine($"| {line.PadRight(boxWidth - 3)} |");
                }
                Console.WriteLine(new string('=', boxWidth));
                Console.ResetColor();



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
                    case "6":
                        OptimizeMotorcycleParking();
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
            //show date and time of registration
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            parkingSpots[spotNumber - 1].Add(type + "#" + licensePlate + "#" + timestamp);
            Console.WriteLine($"\n\nVehicle {licensePlate} of type {type} registered at spot {spotNumber} on {timestamp}.\n\n");

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

        public static void CheckParkingStatus()
        {
            int columns = 10;
            int rows = 10;
            int boxWidth = 16; // Adjust for license plate length

            int fullSpots = 0;
            int freeSpots = 0;
            List<int> fullSpotNumbers = new List<int>();
            List<int> mcAvailableSpots = new List<int>();

            Console.WriteLine("\nParking Plan:\n");

            for (int row = 0; row < rows; row++)
            {
                // Top border of boxes
                for (int col = 0; col < columns; col++)
                {
                    Console.Write("+" + new string('-', boxWidth - 1));
                }
                Console.WriteLine("+");

                // Spot number row
                for (int col = 0; col < columns; col++)
                {
                    int spotIndex = row * columns + col;
                    string spotNum = (spotIndex + 1).ToString().PadLeft(3, ' ');
                    Console.Write($"|{spotNum.PadRight(boxWidth - 1)}");
                }
                Console.WriteLine("|");

                // License plate row
                for (int col = 0; col < columns; col++)
                {
                    int spotIndex = row * columns + col;
                    var vehicles = parkingSpots[spotIndex];

                    string cellContent = "";
                    if (vehicles.Count == 0)
                    {
                        cellContent = "".PadRight(boxWidth - 1);
                        Console.Write($"|{cellContent}");
                    }
                    else
                    {
                        // Color-coding for each vehicle in the spot
                        Console.Write("|");
                        int usedWidth = 0;
                        foreach (var v in vehicles)
                        {
                            string[] parts = v.Split('#');
                            string type = parts[0];
                            string plate = parts[1];
                            string timestamp = parts.Length > 2 ? parts[2] : "";

                            // Set color based on type
                            if (type == "CAR")
                                Console.ForegroundColor = ConsoleColor.Red;
                            else if (type == "MC")
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            else
                                Console.ResetColor();

                            string display = plate;
                            // Truncate if too long
                            if (display.Length > boxWidth -1)
                                display = display.Substring(0, boxWidth - 1);

                            Console.Write(display);
                            usedWidth += display.Length;

                            // Separator for multiple vehicles
                            if (v != vehicles.Last())
                            {
                                Console.ResetColor();
                                Console.Write(",");
                                usedWidth += 1;
                            }
                        }
                        Console.ResetColor();
                        // Pad the rest of the cell
                        Console.Write(new string(' ', boxWidth - 1 - usedWidth));
                    }
                }
                Console.WriteLine("|");
            }

            // Bottom border of boxes
            for (int col = 0; col < columns; col++)
            {
                Console.Write("+" + new string('-', boxWidth - 1));
            }
            Console.WriteLine("+");

            // Summary calculation 
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                var spot = parkingSpots[i];
                if (spot.Count == 0)
                {
                    freeSpots++;
                }
                else
                {
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
            }

            // Display summary
            Console.WriteLine($"\nTotal full spots: {fullSpots}");
            Console.WriteLine($"Total free spots: {freeSpots}");
            if (fullSpotNumbers.Count > 0)
            {
                Console.WriteLine("Parking Spot number occupied: " + string.Join(", ", fullSpotNumbers));
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
                int index = spot.FindIndex(v => v.Split("#")[1] == licensePlate);
                if (index == -1)
                {
                    Console.WriteLine($"\nVehicle with license plate {licensePlate} not found in spot {spotNumber}.");
                    return;
                }

                string[] parts = spot[index].Split('#');
                string timestamp = parts.Length > 2 ? parts[2] : "N/A";
                Console.WriteLine($"\nVehicle {licensePlate} was registered on {timestamp}.");

                //calculate parking duration
                if(DateTime.TryParse(timestamp, out DateTime registeredTime))
                {
                    TimeSpan duration = DateTime.Now - registeredTime;
                    string durationStr = $"{(int)duration.Hours} hours,{duration.Minutes} minutes, {duration.Seconds} seconds";
                    Console.WriteLine($"Vehicle was parked for {durationStr}");
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
                    int index = spot.FindIndex(v => v.Split('#')[1] == licensePlate);
                    if (index != -1)
                    {
                        string[] parts = spot[index].Split('#');
                        string timestamp = parts.Length > 2 ? parts[2] : "N/A";
                        Console.WriteLine($"\nVehicle {licensePlate} was registered on {timestamp}.");

                        // Calculate and display duration
                        if (DateTime.TryParse(timestamp, out DateTime registeredTime))
                        {
                            TimeSpan duration = DateTime.Now - registeredTime;
                            string durationStr = $"{(int)duration.Hours} hours, {duration.Minutes} minutes, {duration.Seconds} seconds";
                            Console.WriteLine($"Vehicle was parked for: {durationStr}");
                        }

                        //remove vehicle from spot
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
                int index = spot.FindIndex(v => v.Split('#')[1] == licensePlate);
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

        public static void OptimizeMotorcycleParking()
        {
            // Find all spots with exactly one MC
            var singleMCSpots = new List<int>();
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                var spot = parkingSpots[i];
                if (spot.Count == 1 && spot[0].StartsWith("MC"))
                {
                    singleMCSpots.Add(i);
                }
            }

            // Try to pair up single MCs
            var moved = new List<(string licensePlate, int fromSpot, int toSpot)>();
            var used = new HashSet<int>();

            for (int i = 0; i < singleMCSpots.Count; i++)
            {
                if (used.Contains(singleMCSpots[i])) continue;
                for (int j = i + 1; j < singleMCSpots.Count; j++)
                {
                    if (used.Contains(singleMCSpots[j])) continue;

                    // Move MC from spot j to spot i
                    var fromSpot = singleMCSpots[j];
                    var toSpot = singleMCSpots[i];
                    var mcVehicle = parkingSpots[fromSpot][0];
                    parkingSpots[toSpot].Add(mcVehicle);
                    parkingSpots[fromSpot].RemoveAt(0);

                    string licensePlate = mcVehicle.Split('#')[1];
                    moved.Add((licensePlate, fromSpot + 1, toSpot + 1));
                    used.Add(fromSpot);
                    used.Add(toSpot);
                    break; // Only pair once
                }
            }

            // Display transactions
            if (moved.Count == 0)
            {
                Console.WriteLine("\nNo optimization moves were made. All single MCs are already paired or no pairs available.");
            }
            else
            {
                Console.WriteLine("\nMotorcycle parking optimization transactions:");
                foreach (var move in moved)
                {
                    Console.WriteLine($"MC {move.licensePlate} moved from spot {move.fromSpot} to spot {move.toSpot}.");
                }
            }
        }
    }
}