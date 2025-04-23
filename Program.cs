using System;

namespace MiniBankSystemProjectOverview
{
    internal class Program

    {
        // ______Inserting constants that do not change________
        const double MinimumBalance = 100.0;
        const int MaxLoginAttempts = 5;
        const string AccountsFilePath = "accounts.txt";
        const string ReviewsFilePath = "reviews.txt";


        // ______Global lists (parallel)_______
        static List<int> accountNumbers = new List<int>();
        static List<string> accountNames = new List<string>();
        static List<double> balances = new List<double>();
        


        // _____Queues and Stacks______

        static Queue<string> createAccountRequests = new Queue<string>(); // format: "Name|NationalID"
        static Stack<string> reviewsStack = new Stack<string>();// To show the data 


        // _____Account number generator_____
        static int lastAccountNumber;

        static void Main(string[] args)
        {
            LoadAccountsInformationFromFile();
            LoadReviews();

            // _______main menu for system bank to store user choice in avriable _______
            bool running = true;
            char choice;
            do
            {
                while (running)
                {
                    Console.Clear();//____just to clear the screen____
                    Console.WriteLine("\n====== Welcome To Bank System ======");
                    Console.WriteLine("1. User Menu");
                    Console.WriteLine("2. Admin Menu");
                    Console.WriteLine("3. Employee Menu");
                    Console.WriteLine("0. Exit the system");
                    Console.Write(" Please Select Your Option: ");
                    string mainChoice = Console.ReadLine();

                    switch (mainChoice)
                    {
                        case "1": UserMenu(); break;
                        case "2": AdminMenu(); break;
                        case "3": EmployeeMenu(); break;
                        case "0": ExitTheSystem(); break;

                            SaveAccountsInformationToFile();
                            SaveReviews();
                            running = false; //____Keep running  false to repeat the loop____
                            break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }

                Console.WriteLine("Do You Want Another Option  ? y / n");
                choice = Console.ReadKey().KeyChar;

            } while (choice == 'y' || choice == 'Y');
            Console.WriteLine("Thank you for using Bank System!");
        }

        // ==========  Addition The  Function Types Of The  User Menu==========
        static void UserMenu()
        {
            bool inUserMenu = true;

            while (inUserMenu)
            {
                Console.Clear();
                Console.WriteLine("\n------ User Menu ------");
                Console.WriteLine("1. Request Account Creation");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. View Balance");
                Console.WriteLine("5. Submit Review/Complaint");
                Console.WriteLine("6. Manage personal details");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select option: ");
                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1": RequestAccountCreation(); break;
                    case "2": Deposit(); break;
                    case "3": Withdraw(); break;
                    case "4": ViewBalance(); break;
                    case "5": SubmitReview(); break;
                    case "6": Managepersonaldetails(); break;
                    case "0": inUserMenu = false; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }





















    }
}

