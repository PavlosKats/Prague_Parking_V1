# Prague Parking v1.0

Prague Parking v1.0 is a simple console application for managing a parking lot with 100 spots. It allows users to register vehicles (cars and motorcycles), check out vehicles, move vehicles between spots, and view the status of all parking spots.

## Features

- Register a car (1 car per spot) or motorcycle (up to 2 motorcycles per spot)
- Check out vehicles by spot number or license plate
- Move vehicles to another spot
- Check availability of a parking spot for a specific vehicle type
- View the status of all parking spots

---

# Prague Parking v1.1 - Changes and Upgrades

## New Features & Improvements

- **Grid Parking Plan:** Parking status now displays a visual grid map, showing spot numbers and license plates for better readability.
- **Color Coding:** Cars and motorcycles are color-coded in the grid for quick identification.
- **Registration Timestamp:** Vehicles are registered with date and time, shown when checking out.
- **Parking Duration:** When a vehicle is checked out, the time it was parked is displayed.
- **Motorcycle Optimization:** Added an option to automatically pair single parked motorcycles into shared spots, optimizing space.
- **Menu Improvements:** Enhanced menu layout and added option for motorcycle optimization.

## Bug Fixes & Adjustments

- Improved license plate matching to support timestamped vehicle records.
- Fixed grid alignment and padding issues for consistent display.
- Prevented negative padding errors in grid rendering.

---

## Usage

Run the application and follow the menu prompts to manage parking operations.