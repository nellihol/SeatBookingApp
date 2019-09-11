using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace SeatBookingApp
{
    class Program
    {

        static void Main(string[] args)
        {
            bool endProgram = false;
            bool isValid = false;
            bool isStaff = false;
            int userErrors = 0;

            WriteLine("IFN501 Assignment 2 - Deliverable 2. Nelli Holopainen n10309152");
            WriteLine("---------------------------------------------------------------");

            // 1. Asking the user type
            while (isValid != true)
            {
                WriteLine("--- Welcome to the Seat booking systecm ---");
                WriteLine("Who are you?  \n" +
                        "(1) Airline staff \n" +
                        "(2) Passenger\n");
                string userInput2 = ReadLine();

                switch (userInput2)
                {
                    case "1":
                        isStaff = true;
                        isValid = true;
                        break;
                    case "2":
                        isStaff = false;
                        isValid = true;
                        break;
                    default:
                        WriteLine("Try again: 1 or 2");
                        userErrors++;
                        if (userErrors > 4) {
                            WriteLine("Too many errors, exiting...");
                            ReadLine();// Delay to see the message
                            System.Environment.Exit(0);
                        }
                        userInput2 = ReadLine();

                        break;
                }
            }

            // reset
            userErrors = 0;
            isValid = false;

            // 2. Asking for flight details

            WriteLine("---- Please input the flight details ----");

            WriteLine("Enter Destination:");
            string destination = ReadLine();

            if (destination.Length <= 2)
            {
                // if destination is less than 2 chars
                // add extra char for not getting errors when generating flight number
                destination += " ";
            }

            WriteLine("Departure time and date");
            WriteLine("Leave today? Y/N");
            string userInput = ReadLine();
            string departureTime = "";

            switch (userInput)
            {
                case "Y":
                case "y":
                    departureTime = DateTime.Now.AddHours(4).ToString(); // Adding 4 hours for convenience
                    break;
                case "N":
                case "n":
                    // Validating the date input 
                    while (!isValid)
                    {
                        WriteLine("Enter departure date and time in following format \n MM/DD/YYY HH:MM (eg 12/08/2019 18:30)");
                        DateTime userDateTime;
                        if (DateTime.TryParse(Console.ReadLine(), out userDateTime))
                        {
                            departureTime = "" + userDateTime;
                            isValid = true;
                        }
                        else
                        {
                            Console.WriteLine("You have entered an incorrect value. Try again");
                            userErrors++;
                        }
                    }
                    break;
                default:
                    WriteLine("I take that as yes");
                    departureTime = DateTime.Now.AddHours(4).ToString();  // Adding 4 hours for convenience
                    break;
            }

            WriteLine("--Thank you!--");

            // Now that the flight is selected, let's generate the flight number and gate number

            Random flightRandom = new Random();
            // Two first letters of destination + random number = flightnumber
            string flightNumber = destination.Substring(0, 2).ToUpper() + flightRandom.Next(100, 999).ToString();
            WriteLine("\nThe flight number " + flightNumber + " to " + destination + " selected");


            Random gateRandom = new Random();
            int gateNumber = gateRandom.Next(1, 45);
            string gate = gateNumber.ToString();

            if (gateNumber % 2 == 0)
            {
                gate += "a";
            }
            else if (gateNumber > 35)
            {
                gate += "b";
            }

            WriteLine("The flight departs " + departureTime + " at gate " + gate + "\n");

            bool isFlightFull = Passenger.IsFlightFull();
            userErrors = 0;

            // 3. Start booking seats for the chosen flight, end the loop if flight is full
            while (endProgram != true)
            {
                // Check what's the user role and show correct text 
                if (isStaff)
                {
                    WriteLine("--- What would you like to do now? ---  \n" +
                                       "(1) Input customer details & Book seats \n" +
                                       "(2) Show available seats\n" +
                                       "(3) Show all passengers. \n" +
                                       "Press Q to exit.");
                }
                else
                {
                    WriteLine("--- What would you like to do now? --- \n" +
                               "(1) Input customer details & Book seats  \n" +
                               "(2) Show available seats\n" +
                               "(3) Show boarding passes \n" +
                               "Press Q to exit.");
                }

                string userInput3 = ReadLine();

                //Starting the main loop
                switch (userInput3)
                {
                    case "q":
                    case "Q":
                        endProgram = true;
                        break;

                    case "1":
                        Passenger.BookASeat(destination, departureTime, flightNumber, gate);
                        break;
                    case "2":
                        // Show the Available seats
                        WriteLine("Available seat numbers: " + Passenger.AvailableSeats());
                        break;
                    case "3":
                        // Show list of all passengers if the user is staff
                        if (isStaff)
                        {
                            if (Passenger.ShowPassengerList() != "")
                            {
                                // Print passengers  if there is passengers
                                WriteLine("Passenger list:");
                                WriteLine(Passenger.ShowPassengerList());
                            }
                            else
                            {
                                WriteLine("No pssengers yet. Select option 1 to add.");
                            }
                        }
                        if (!isStaff)
                        {
                            // Show the booked boarding passes if user is a passenger
                            Passenger.ShowAllBoardingPasses();
                            WriteLine("Please select option 1 to enter client details");
                        }
                        break;
                    // Default: ask for new input
                    default:
                        WriteLine("Try again.");
                        userErrors++;
                        break;
                }

                if (userErrors > 4) {
                    WriteLine("Too many errors. Exiting...");
                    endProgram = true;
                }
                // Check every time if the flight is full
                isFlightFull = Passenger.IsFlightFull();
                if (isFlightFull)
                {
                    endProgram = true;
                }
            }

            if (isFlightFull)
            {
                WriteLine("\n\n---Checking closed - the flight is full--");

                if (isStaff)
                {
                    WriteLine("Showing all customers ...");
                    Passenger.ShowPassengerList();
                }
                else
                {
                    WriteLine("Showing all boarding passes booked in this session...");
                    Passenger.ShowAllBoardingPasses();
                }
            }

            ReadLine();
        }
    }
}
