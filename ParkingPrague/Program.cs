using System;
using System.Globalization;
namespace ParkingPrague
{
    class Program
    {
        static DateTime[] parkedAtTime = new DateTime[201];
        static string[] parkedTime = new string[201];
        static string[] pSpots = new string[101];
        static void Main(string[] args)
        {


            CultureInfo culture = CultureInfo.CurrentCulture;
            Console.WriteLine("--------Welcome to Prague Parking App--------");
            Console.ReadKey();
            string menuVal;
            do //Menu is created
            {
                Console.Clear();
                Console.WriteLine("\n[A]dd a vehicle");
                Console.WriteLine("[M]ove a vehicle");
                Console.WriteLine("[R]eturn a vehicle");
                Console.WriteLine("[P]arkinglot overview");
                Console.WriteLine("[S]earch for a vehicle");
                Console.WriteLine("[E]xit program");
                Console.Write("\nEnter the letter inside --> [] ");
                menuVal = Console.ReadLine().ToUpper();

                //String hourMinute = DateTime.Now.ToString("HH:mm");                
                string now = DateTime.Now.ToString("HH:mm");

                if (now.Contains("00:0"))
                {
                    Console.WriteLine("Times up!");
                    Console.WriteLine("All vehicles will explode..");
                    Console.WriteLine("Or just moved to another location where you will have to pay half you salary to get your vehicle back!");
                    Console.ReadLine();
                    pSpots = null;
                    break;
                }

                Console.Clear();

                switch (menuVal) //Takes the user to the option that is choosen.
                {
                    case "A"://Adds a vehicle
                        {
                            AddToParkingSpace();
                            break;
                        }
                    case "M": //Moves a vehicle from its spot to user input
                        {
                            MoveVehicle();
                            break;
                        }
                    case "R"://Removes a vehicle from the parkingspot
                        {
                            ReturnVehicle();
                            ; break;
                        }
                    case "P"://Displays the parkinglot and the parked vehicles
                        {
                            ParkingLotOverview();
                            Console.WriteLine("\n\nPress any key to go back");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                    case "S":
                        {
                            int output = Search();
                            if (output == 0)
                            {
                                Console.WriteLine("There is no vehicle with that platenumber here.");
                            }
                            else
                            {
                                Console.Clear();
                                ParkingLotOverview();
                                Console.WriteLine("That Vehicle is parked at spot number " + output);
                            }

                            Console.ReadKey();
                            break;
                        }
                    case "E":// Shuts down the program
                        {
                            Console.Clear();
                            Console.WriteLine("\n--------Thanks for Using Prague Parking--------");
                            Console.ReadLine();
                            break;
                        }
                    default://If the user enters wrong input
                        {
                            Console.WriteLine("You entered the wrong letters. Please only use the letters inside --> [ ] ");
                            Console.ReadKey();

                            break;
                        }
                }// Takes the user to the option that is choosen.
            } while (menuVal != "E");//Shutting down..
        }
        static string CarOrMc()
        {
            //ParkingLotOverview();

            Console.Write("\nIs it a Car or Mc: "); string input = Console.ReadLine().ToUpper();
            switch (input)
            {
                case "MC": return "MC";
                case "CAR": return "CAR";
                default:
                    Console.Clear();
                    Console.WriteLine("Sorry wrong input, use either \"CAR\" or \"MC\"");
                    Console.ReadLine();
                    Console.Clear();
                    return CarOrMc();
            }

        }      
        static void AddToParkingSpace()
        {
            string vehicle = CarOrMc();
            Console.Write("Enter the Regnumber: "); string regNr = Console.ReadLine().ToUpper();                   
            
            bool checkRegNr = CheckRegInput(regNr); 
            if (checkRegNr == false)
            {
                InputError();
            }
            regNr = AddCarOrMc(regNr, vehicle);
            bool checkSpot = CheckSpotTaken(regNr);
            if (checkRegNr && checkSpot)
            {
                for (int i = 1; i < pSpots.Length; i++)
                {
                    if (vehicle == "MC" && pSpots[i] == null)//if mc and empty spot
                    {
                        pSpots[i] = regNr;
                        Console.Clear();
                        ParkingLotOverview();                      
                        Ticket(regNr, i);
                        Console.Clear();
                        break;
                    }
                    else if (vehicle == "MC" && pSpots[i].Contains("MC") && pSpots[i].Length < 14)//if mc and spot already has 1 mc
                    {
                        pSpots[i] = pSpots[i] + '|' + regNr;
                        Console.Clear();
                        ParkingLotOverview();          
                        Ticket(regNr, i);                      
                        Console.Clear();
                        break;
                    }
                    else if (vehicle == "CAR")//if it's a car
                    {
                        if (pSpots[i] == null)
                        {
                            pSpots[i] = regNr;
                            Console.Clear();
                            ParkingLotOverview();                           
                            Ticket(regNr, i);                          
                            Console.Clear();
                            break;
                        }
                    }
                    else if (pSpots[i] != null)// IF FULL
                    {
                        bool full1 = pSpots[i].Contains("MC");
                        bool full2 = pSpots[i].Contains("CAR");

                        if (full1 && full2)
                        {
                            Console.Clear();
                            Console.WriteLine("The Parking Lot is Full");
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
            }
            else if (checkSpot == false)
            {
                Console.WriteLine("There is already a vehicle with that license plate parked here..");
                Console.WriteLine("Calling 911...");
                Console.ReadKey();
            }
            for (int i = 1; i < parkedTime.Length; i++) // Adds parked timed to vehicle
            {
                if (parkedTime[i] == null)
                {
                    DateTime now = DateTime.Now;
                    parkedTime[i] = regNr;
                    parkedAtTime[i] = now;
                    break;
                }
            }
        }
        static void ParkingLotOverview()
        {
            const int column = 6;
            int x = 1;

            for (int i = 1; i < pSpots.Length; i++)
            {

                if (x >= column && x % column == 0)
                {
                    Console.WriteLine();
                    x = 1;
                }
                if (pSpots[i] == null)//If spot is Empty
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(i + ": ");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Empty \t");
                    Console.ResetColor();
                    x++;
                }
                else if (pSpots[i].Contains("MC") && pSpots[i].Length < 14 && !pSpots[i].Contains('|'))//Adds Yellow color if half full with mc
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(i + ": ");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(pSpots[i] + "  Empty ");
                    Console.ResetColor();
                    x++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(i + ": ");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(pSpots[i] + "\t");
                    Console.ResetColor();
                    x++;
                }
            }



        }
        static void MoveVehicle()
        {
            string regNr = SearchForMove();
            if (regNr == "0")
            {
                Console.WriteLine("There is no vehicle like that at this parkinglot ");
                Console.WriteLine("\n\nPress any key to continue");
                Console.ReadKey();
                MoveVehicle();
            }
            if (regNr.Contains("-MC") || regNr.Contains("-CAR"))
            {
                Console.Write("To which parking spot do you wish to move it: "); string input = Console.ReadLine();
                int moveTo = CheckNumInput(input);
                if (moveTo == 0)
                {
                    Console.WriteLine("Wrong input, you can only use 1-100.");
                    Console.ReadKey();
                    MoveVehicle();
                }
                else if (moveTo == -1)
                {
                    Console.WriteLine("The parking-lot only has 100 parking spots..\n");
                    Console.ReadKey();
                    MoveVehicle();
                }


                for (int i = 1; i < pSpots.Length; i++)//Removes vehicle from current spot
                {
                    if (pSpots[i] != null)
                    {
                        bool test = pSpots[i].Contains(regNr);
                        if (test == true)
                        {
                            if (pSpots[i] != null && pSpots[i].Contains('|'))
                            {
                                string[] spliten = pSpots[i].Split('|');
                                if (spliten[0].Contains(regNr))
                                {
                                    pSpots[i] = spliten[1];
                                }
                                else
                                {
                                    pSpots[i] = spliten[0];
                                }
                                break;
                            }
                            else
                            {
                                pSpots[i] = null;
                                Console.Clear();
                                Console.WriteLine(regNr + " is now moved to spot nr: " + moveTo);
                                Console.ReadKey();
                                break;
                            }
                        }
                    }
                }
                if (pSpots[moveTo] == null) // Adds to new spot
                {
                    bool tryBool = regNr.Contains("-MC");
                    if (tryBool)
                    {
                        pSpots[moveTo] = regNr;
                    }
                    else
                    {
                        pSpots[moveTo] = regNr;
                    }
                }
                else if (pSpots[moveTo] != null && pSpots[moveTo].Contains("-MC") && pSpots[moveTo].Length < 14 && pSpots[moveTo].Length > 5 && !regNr.Contains("-CAR"))
                {
                    //pSpots[moveTo] += string.Join('|', regNr);
                    pSpots[moveTo] += '|' + regNr;
                    Console.Clear();
                    Console.WriteLine(regNr + " is now moved to spot nr: " + moveTo);
                    Console.ReadKey();
                }
            }
            Console.Clear();
            ParkingLotOverview();
            Console.WriteLine("\n\nPress any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
        static void ReturnVehicle()
        {
            ParkingLotOverview();
            Console.Write("\n\nEnter the regnumber of the vehicle you wish to remove from the lot: "); string regNr = Console.ReadLine().ToUpper();
            bool checkRegNr = CheckRegInput(regNr);
            if (checkRegNr == false)
            {
                InputError();
            }
            int counter = 0;
            int regSpotNr = 0;
            for (int i = 1; i < pSpots.Length; i++)
            {
                if (pSpots[i] != null)
                {
                    bool isMC = pSpots[i].Contains(regNr + "-MC");
                    bool isCar = pSpots[i].Contains(regNr + "-CAR");
                    if (isMC)//removes if MC
                    {
                        if (pSpots[i] != null && pSpots[i].Contains('|'))
                        {
                            string[] spliten = pSpots[i].Split('|');
                            if (spliten[0].Contains(regNr))
                            {
                                pSpots[i] = spliten[1];
                                regSpotNr = i;
                                break;
                            }
                            else
                            {
                                pSpots[i] = spliten[0] + '|';
                                regSpotNr = i;
                                break;
                            }
                        }
                        else
                        {
                            pSpots[i] = null;
                            regSpotNr = i;
                            break;
                        }
                    }
                    else if (isCar)//removes if Car
                    {
                        pSpots[i] = null;
                        regSpotNr = i;
                        break;
                    }
                    else if (pSpots[i] != regNr && pSpots[i] != null && counter == 100)//if there is nothing
                    {
                        Console.WriteLine("There is no vehicle with that reg number");
                        Console.ReadLine();
                        regSpotNr = -1;
                        break;
                    }
                }
                counter++;
            }
            Console.Clear();
            ParkingLotOverview();
            Console.WriteLine("\n" + regNr + " has been removed from the parking lot.");
            ReturnTime(regNr);
            Console.ReadLine();
        }
        static string SearchForMove()
        {
            Console.Clear();
            ParkingLotOverview();
            string vehicle = CarOrMc();
            Console.Write("Enter the regnumber of the vehicle you are moving: "); string regNr = Console.ReadLine().ToUpper();
            bool checkRegNr = CheckRegInput(regNr);          
            if (checkRegNr == false)
            {
                InputError();
            }
            regNr = AddCarOrMc(regNr, vehicle);
            if (regNr == null)
            {
                Console.WriteLine("No vehicle was found..");
                Console.ReadKey();
                SearchForMove();
            }

            for (int i = 1; i < pSpots.Length; i++)
            {
                if (pSpots[i] != null)
                {
                    bool test = pSpots[i].Contains(regNr);
                    if (test)
                    {
                        return regNr;
                    }
                }
            }
            for (int i = 1; i < pSpots.Length; i++)
            {
                if (pSpots[i] != null && pSpots[i] != regNr)
                {
                    regNr = "0";
                    return regNr;
                }
            }
            regNr = "0";
            return regNr;
        }
        static int Search()
        {
            Console.Write("Enter the Regnumber: "); string regNr = Console.ReadLine().ToUpper();
            bool checkRegNr = CheckRegInput(regNr);
            if (checkRegNr == false)
            {
                InputError();
            }
            for (int i = 1; i < pSpots.Length; i++)
            {
                if (pSpots[i] != null)
                {
                    // bool isTrue = pSpots[i].Contains(plateNum);
                    if (pSpots[i] == regNr + "-MC" || pSpots[i] == regNr + "-CAR")
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
        static void ReturnTime(string regNr)
        {
            for (int i = 1; i < parkedTime.Length; i++)
            {
                if (parkedTime[i] != null)
                {
                    if (parkedTime[i].Contains(regNr))
                    {
                        DateTime now = DateTime.Now;
                        DateTime before = parkedAtTime[i];
                        TimeSpan diff = now.Subtract(before);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\n\"{0}\" were at the parking-lot for {1} hours and {2} minutes ", parkedTime[i], diff.Hours, diff.Minutes);
                        int sum = diff.Minutes * 10;
                        if (diff.Hours > 0)
                        {
                            sum += diff.Hours * 100;
                        }
                        Console.WriteLine("\tYou will have to pay {0} Koruna at checkout.", sum);
                        Console.WriteLine("\n\t\tThanks and Welcome back again!");
                        Console.ResetColor();
                        Console.ReadLine();
                        parkedTime[i] = null;
                        break;

                    }
                }
            }
        }
        static bool CheckSpotTaken(string regNr)
        {
            for (int i = 1; i < pSpots.Length; i++)
            {
                if (pSpots[i] != null)
                {
                    if (pSpots[i].Contains(regNr))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static string AddCarOrMc(string regNr, string vehicle)
        {
            if (vehicle == "CAR" && regNr.Length > 2 && regNr.Length <= 10)//Adds -car and -mc to find them in the array
            {
                regNr += "-CAR";
                return regNr;
            }
            else if (vehicle == "MC" && regNr.Length > 2 && regNr.Length <= 10)
            {
                regNr += "-MC";
                return regNr;
            }
            else
            {
                Console.WriteLine("Please only use Car/mc and no less than 3 and no more than 10 letters..");

                Console.ReadLine();
                Console.Clear();
                regNr = null;
                return regNr;
            }

        }
        static int CheckNumInput(string input)
        {
            int moveTo = 0;
            bool parseResult;
            int resultat;

            if (parseResult = int.TryParse(input, out resultat))
            {
                moveTo = Convert.ToInt32(input);
                return moveTo;
            }
            else if (moveTo >= 101 || moveTo <= 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        } 
        static bool CheckRegInput(string regNr) 
        {
            if (regNr.Contains(" ") || regNr.Contains("¤") || regNr.Contains("/") || regNr.Contains("@") ||
                regNr.Contains("|") || regNr.Contains("%") || regNr.Contains("(") || regNr.Contains(">") ||
                regNr.Contains("?")|| regNr.Contains("&") || regNr.Contains(")") || regNr.Contains("<") ||
                regNr.Contains("!") || regNr.Contains("`") || regNr.Contains("+") || regNr.Contains("|") ||
                regNr.Contains("?") || regNr.Contains("´") || regNr.Contains("}") ||
                regNr.Contains("½") || regNr.Contains("=") || regNr.Contains("]") ||
                regNr.Contains("\"") || regNr.Contains(",") || regNr.Contains("[") ||
                regNr.Contains("'") || regNr.Contains(".") || regNr.Contains("{") ||
                regNr.Contains("^") || regNr.Contains("-") || regNr.Contains("€") ||
                regNr.Contains("#") || regNr.Contains("*") || regNr.Contains("$") ||
                regNr == null
                )
            {
                return false;
            }          
            return true;
        }//Titta inte här ...
        static void InputError()
        {
            Console.WriteLine("Wrong input, Please don't use any blankspaces or anything like \"#!?(/%-*'\" ");
            Console.WriteLine("Press any key to try again!");
            Console.ReadKey();
            Console.Clear();
            AddToParkingSpace();
        }
        static void Ticket(string regNr, int i)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n\n\t\t\t\tTHIS IS YOUR P-TICKET");   
            Console.WriteLine("\t\tYou parked {0} at spot nr {1} at the time {2} ",regNr,i, DateTime.Now.ToString("HH:mm"));
            Console.WriteLine("\tBe sure to check it out before 23:59 or it will be moved and extra charged");
            Random rnd = new Random();
            int randomNumber = rnd.Next(100, 100000);
            Console.Write("This is your personal number: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(randomNumber);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" show it to the valet for retrival of your vehicle");
            Console.ResetColor();
            Console.WriteLine("\n\nPress any key to print out ticket..");
            Console.ReadKey();
            
        }
    }
}

