using System;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiniBankSystemProjectOverview
{
    class Program
    {
        // _______Constants_________
        const double MinimumBalance = 100.0;
        

        // ____Global lists (parallel)______
        static List<int> accountNumbers = new List<int>();
        static List<string> accountNames = new List<string>();
        static List<double> balances = new List<double>();

        //_______Account number generator_____
        static int lastAccountNumber;

        static Queue<string> createAccountRequests = new Queue<string>(); // format: "Name|NationalID"
        static void Main(string[] args)
        {
            //LoadAccountsInformationFromFile();
            //LoadReviews();

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
                    Console.WriteLine("0. Exit the system");
                    Console.Write(" Please Select Your Option:\n ");
                    string mainChoice = Console.ReadLine();

                    switch (mainChoice)
                    {
                        case "1": UserMenu(); break;
                        case "2": AdminMenu(); break;
                        case "0":
                            //SaveAccountsInformationToFile();
                            //SaveReviews();
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
            //const int MaxLoginAttempts = 5;
            while (inUserMenu)
            {
                Console.Clear();
                Console.WriteLine("\n------ User Menu ------");
                Console.WriteLine("1. Request Account Creation");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. View Balance");
                Console.WriteLine("5. Submit Review/Complaint");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select option: ");
                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1": RequestAccountCreation(); break;
                    case "2": Deposit(); break;
                    case "3": Withdraw(); break;
                    //case "4": ViewBalance(); break;
                    //case "5": SubmitReview(); break;
                    case "0": inUserMenu = false; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        // ==========  Addition The  Function Types Of The  Admin Menu==========

        static void AdminMenu()
        {
            bool inAdminMenu = true;

            while (inAdminMenu)
            {
                Console.Clear();
                Console.WriteLine("\n------ Admin Menu ------");
                Console.WriteLine("1. Process Next Account Request");
                Console.WriteLine("2. View Submitted Reviews");
                Console.WriteLine("3. View All Accounts");
                Console.WriteLine("4. View Pending Account Requests");
                Console.WriteLine("0. Return to Main Menu");
                Console.Write("Select option: ");
                string adminChoice = Console.ReadLine();

                switch (adminChoice)
                {
                    case "1": ProcessNextAccountRequest(); break;
                    //case "2": ViewReviews(); break;
                    //case "3": ViewAllAccounts(); break;
                    //case "4": ViewPendingRequests(); break;
                    case "0": inAdminMenu = false; break; // this will Eixt the  loop and return
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }
        // ========== User features function ==========
        //_______________Request account creation (1) ______
        static void RequestAccountCreation()
        {
            //  ____Adding  Error Handling____
            try
            {
                Console.Write("Enter Your Full name: ");
                string name = Console.ReadLine();
                // valid the Name
                string ValidName = stringOnlyLetterValidation(name);

                Console.Write("Enter Your National ID: ");
                string nationalID = Console.ReadLine();
                // valid the national ID
                string ValidID = stringOnlyLetterValidation(nationalID);

                string request = ValidName + "|" + ValidID;

                //Queue<string> queue = new Queue<string>();
                //queue.Enqueue(request);// we do not add a queue heer , because added in the internal calsses for if need used in a multiy bald function 

                //createAccountRequests.Enqueue((name, nationalID));
                createAccountRequests.Enqueue(request);

                Console.WriteLine("Your account request has been submitted.");
            }
            catch
            {
                Console.WriteLine("Warring.. Request is not submited!");
            }
        }
        //_____________Process Next Acount Request(2)____________
        static void ProcessNextAccountRequest()
        {
            //check if the list of account requests is empty or not. If it is empty, it prints a message "There are no pending account requests", then exits the function directly using return.
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No pending account requests.");
                return;
            }

            //var (name, nationalID) = createAccountRequests.Dequeue();
            string request = createAccountRequests.Dequeue();//This request is a string that takes the first request in the createAccountRequests list, dequeues it, and stores it in the request variable.
           //Here, the string is split into two parts using the | symbol as a separator.
            string[] parts = request.Split('|');
           // The resulting array will be parts, where parts[0] = the name and parts[1] = the national ID number.
            string name = parts[0];
            string nationalID = parts[1];

            int newAccountNumber = lastAccountNumber + 1;//This creates a new account number by adding 1 to the last registered account number.

            accountNumbers.Add(newAccountNumber);//to Adds new account to several lists
            accountNames.Add($"{name} ");
            balances.Add(0.0);

            lastAccountNumber = newAccountNumber;//The last account number (lastAccountNumber) will be the new account number that was created.

            Console.WriteLine($"Account created for {name} with Account Number: {newAccountNumber}");
        }
        //_________Deposit (3)________
        static void Deposit()
        {
            int index = GetAccountIndex();//To get the account index so that we can arrange the lists based on the account number entered by the user
            if (index == -1) return;//If the function returns -1 (meaning the calculation does not exist), terminate the function immediately using return.

            try
            {
                //Prints a message to the user asking him to enter the deposit amount.
                Console.Write("Enter deposit amount: ");
                double amount = Convert.ToDouble(Console.ReadLine());//Here it reads the text entered by the user (Console.ReadLine()), then converts it to a decimal number (double) using Convert.ToDouble.

                if (amount <= 0)//Checks that the amount is greater than zero (positive), if the user enters a zero or negative amount, prints an error message and terminates the function immediately.
                {
                    Console.WriteLine("Amount must be positive.");
                    return;
                }

                balances[index] += amount;//Here the amount entered by the user is added to the account balance in the balances list in the place opposite index.
                Console.WriteLine("Deposit successful.");
            }
            catch
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        //________withdraw (4)_________
        static void Withdraw()
        {
            int index = GetAccountIndex();//Here we call the GetAccountIndex() function to ask the user to enter the account number, then we search for its location in the lists and then we reorder the lists.
            if (index == -1) return;//If the function returns -1 (i.e., the calculation does not exist or an input error), it exits the Withdraw function directly and does not continue.

            try
            {
                Console.Write("Enter withdrawal amount: ");
                double amount = Convert.ToDouble(Console.ReadLine());

                if (amount <= 0)
                {
                    Console.WriteLine("Amount must be positive.");
                    return;
                }

                if (balances[index] - amount >= MinimumBalance)//Checks if the current balance - withdrawal amount - is still greater than or equal to the Minimum Balance (the lowest balance allowed in the account). The Minimum Balance may be, for example, 100 riyals, or according to the bank’s policy.
                {
                    //If the condition is met: the amount is deducted from the account balance (balances[index] -= amount) and then the message “Withdrawal Successful” is printed.
                    balances[index] -= amount;
                    Console.WriteLine("Withdrawal successful.");
                }
                else
                { //If the condition is not met (i.e. the debit will be lowered to the account than the minimum allowed), the message “Insufficient balance after deducting the minimum” will be printed.
                    Console.WriteLine("Insufficient balance after minimum limit.");
                }
            }
            catch
            {
                Console.WriteLine("Invalid amount.");
            }
        }


























        //_________Get Account Index____________
        static int GetAccountIndex()
        {
            Console.Write("Enter account number: ");
            try //Starts a try block to attempt to execute the code normally with the possibility of catching errors.
            {
                int accNum = Convert.ToInt32(Console.ReadLine());
                int index = accountNumbers.IndexOf(accNum); //Finds the index of this account number accNum within the list accountNumbers.If it finds it, it returns its position(e.g., 0, 1, or 2).If it does not find it, it returns -1.
                //If index returns -1 (meaning the account number is not in the list), it prints the message "Account not found", and returns -1 to tell other functions that the account does not exist.
                if (index == -1)
                {
                    Console.WriteLine("Account not found.");
                    return -1;
                }

                return index;
            }
            catch //If an error occurs (e.g. the user types text instead of a number), the error is caught and "Invalid input" is printed and -1 is returned as an indication that the input failed.
            {
                Console.WriteLine("Invalid input.");
                return -1;
            }
        }


        //========== valadition ==========

        // ________string validation__________________ 
        public static string stringOnlyLetterValidation(string word)
        {
            bool IsValid = true;
            string ValidWord = "";
            if (string.IsNullOrEmpty(word) && word.All(char.IsLetter))
            {
                Console.WriteLine("Input is just empty!");
                IsValid = false;

            }
            else
            {
                IsValid = true;
            }

            if (Regex.IsMatch(word, @"^[a-zA-Z]+$"))
            {
                Console.WriteLine("Valid: only letters.");
                IsValid = true;
            }
            else
            {
                Console.WriteLine("Invalid: contains non-letter characters.");
                IsValid = false;
            }

            if (IsValid)
            {
                ValidWord = word;
            }
            else
            {
                Console.WriteLine("word unsaved");
            }
            return ValidWord;
        }
        // _________validate numeric strting_________
        public static string StringWithNumberValidation(string word)
        {
            bool IsValid = true;
            string ValidWord = "";
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.WriteLine("Input is just spaces or empty!");
                IsValid = false;

            }
            else
            {
                IsValid = true;
            }
            if (Regex.IsMatch(word, @"^\d+$"))
            {
                Console.WriteLine("Valid: only numbers.");
                IsValid = true;

            }
            else
            {
                Console.WriteLine("Invalid: contains non-numeric characters.");
                IsValid = false;
            }
            if (IsValid)
            {
                ValidWord = word;
            }
            else
            {
                Console.WriteLine("word unsaved! try agine");
            }
            return ValidWord;
        }










    }
}

