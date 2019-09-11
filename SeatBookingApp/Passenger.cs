using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace SeatBookingApp
{
    class Passenger
    {
        // The data attributes for Passenger
        public int seatnumber { get; set; }
        public string passengerName { get; set; }
        private int securityNumber { get; set; }
        public string destination { get; set; }
        public string flightNumber { get; set; }
        public string departureTime { get; set; }
        public string departureGate { get; set; }


        // Max seats is 40 + 1 to get the numbers work correctly
        private static int MAXSeats = 41;

        // Array for storing passengers
        private static Passenger[] allPassengers = new Passenger[MAXSeats];

        public Passenger()
        {
            // Set the security number between 3000 and 99999
            Random random = new Random();
            securityNumber = random.Next(30000, 999999);
        }

        // Return a message 
        public new string ToString()
        {
            // Printing seat, name and security
            return ("- Seat: " + seatnumber + ", Name: " + passengerName + ", Security number: " + securityNumber);
        }

        // Method for going through the passenger array 
        public static string AvailableSeats()
        {
            string result = "";
            int index = 0;

            for (int i = 1; i < (allPassengers.Length); i++)
            {
                if (allPassengers[i] == null)
                {
                    result += i + ", ";
                }
                else
                {
                    index++;
                }
            }

            return result;
        }

        public static bool IsFlightFull()
        {
            // Boolean that goes through the Passenger array and counts them
            // If it's equal to maximum the flight is full.
            int index = 0;

            for (int i = 0; i < (allPassengers.Length); i++)
            {
                if (allPassengers[i] != null)
                {
                    index++;
                }
            }

            if (index == MAXSeats)
            {
                return true;
            }

            return false;
        }

        // Method for checking if the seat is taken
        public static bool isSeatTaken(int seatToCheck)
        {
            if (allPassengers[0] == null)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < allPassengers.Length; i++)
                {
                    if (allPassengers[i] == null)
                    {
                        return false;
                    }
                    else if (allPassengers[i].seatnumber == seatToCheck)
                    {
                        WriteLine("Seat number {0} is taken", allPassengers[i].seatnumber);
                        return true;
                    }
                }
            }

            return false;
        }

        // Method for showing the passenger list
        public static string ShowPassengerList()
        {
            string passengerlist = "";

            for (int i = 1; i < allPassengers.Length; i++)
            {
                if (allPassengers[i] != null)
                {
                    passengerlist += allPassengers[i].ToString() + "\n";
                }
            }
            return passengerlist;
        }

        // Method for booking seats
        public static void BookASeat(string destination, string date, string flightNumber, string gate)
        {
            bool bookingsession = true;

            // Looping the passenger input for this flight until the user says "n"
            while (bookingsession)
            {
                Passenger psngr = new Passenger();
                // Adding flight details
                psngr.destination = destination;
                psngr.flightNumber = flightNumber;
                psngr.departureGate = gate;
                psngr.departureTime = date;

                // First Name
                WriteLine("Enter first name:");
                string nameInput = ReadLine();
                if (nameInput.Length > 5)
                {
                    nameInput = nameInput.Remove(5);
                    psngr.passengerName = nameInput;
                }
                else
                {
                    psngr.passengerName = nameInput;
                }

                // Last name
                WriteLine("Enter last name:");
                string nameInput2 = ReadLine();
                if (nameInput2.Length > 5)
                {
                    nameInput2 = nameInput2.Remove(5);
                    psngr.passengerName += "/" + nameInput2;
                }
                else
                {
                    psngr.passengerName += "/" + nameInput2;
                }

                WriteLine("Enter desired seat number, available seats: " + AvailableSeats());

                // Seat numbers
                int AllocateSeat;
                int.TryParse(ReadLine(), out AllocateSeat);

                int userErrors = 0;

                while (AllocateSeat <= 0 || AllocateSeat > (MAXSeats - 1) || isSeatTaken(AllocateSeat))
                {
                    WriteLine("Invalid input. Try again");
                    userErrors++;
                    int.TryParse(ReadLine(), out AllocateSeat);
                    if (userErrors > 4)
                    {
                        WriteLine("Too many errors");
                        return;
                    }
                }

                psngr.seatnumber = AllocateSeat;

                allPassengers[psngr.seatnumber] = psngr;
                allPassengers[0] = psngr;

                psngr.ShowBoardingPass();

                WriteLine("Book another seat? Y/N");
                string userInput = ReadLine();

                if (userInput == "y" || userInput == "Y")
                {
                    // Just redo the loop if Y, or warning if flight is full
                    if (IsFlightFull())
                    {
                        WriteLine("Flight is full! no more seats available");
                        bookingsession = false;
                    }
                }
                else if (userInput == "n" || userInput == "N")
                {
                    WriteLine("Thanks for booking!");
                    bookingsession = false;
                }
                else
                {
                    WriteLine("I take that as no");
                    bookingsession = false;
                }
            }

            //Date and Time should be printed when passenger entries are completed. 
            DateTime currTimeAndDate = DateTime.Now;
            WriteLine("\nThe time and date now is {0}\n", currTimeAndDate);
        }

        public void ShowBoardingPass()
        {
            // Printing boarding pass in tabular format 
            WriteLine("-------------------------------------------------");
            WriteLine("* * * * * * B O A R D I N G   P A S S * * * * * *");
            WriteLine("{0,11} | {1,4} | {2,15}", "fName/lName", "Seat", "security number");
            WriteLine("{0,11} | {1,4} | {2,15}", passengerName, seatnumber, securityNumber);
            WriteLine("\n{0,11} | {1,6} | {2,4} | {3,9}", "Destination", "Flight", "Gate", "Date/Time");
            WriteLine("{0,11} | {1,6} | {2,4} | {3,9}", destination, flightNumber, departureGate, departureTime);
            WriteLine("-------------------------------------------------");
        }

        // Method for showing the passenger list
        public static void ShowAllBoardingPasses()
        {
            for (int i = 1; i < allPassengers.Length; i++)
            {
                if (allPassengers[i] != null)
                {
                    allPassengers[i].ShowBoardingPass();

                }
            }

        }
    }
}
