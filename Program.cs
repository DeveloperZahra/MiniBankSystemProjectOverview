using System;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace MiniBankSystemProjectOverview
{
    internal class Program

    {

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
                    Console.Write(" Please Select Your Option: ");
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
                    //case "2": Deposit(); break;
                    //case "3": Withdraw(); break;
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
                    //case "1": ProcessNextAccountRequest(); break;
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
            //  Adding  Error Handling
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

        static void ProcessNextAccountRequest()
        {
            if (createAccountRequests.Count == 0)
            {
                Console.WriteLine("No pending account requests.");
                return;
            }

            //var (name, nationalID) = createAccountRequests.Dequeue();
            string request = createAccountRequests.Dequeue();
            string[] parts = request.Split('|');
            string name = parts[0];
            string nationalID = parts[1];

            int newAccountNumber = lastAccountNumber + 1;

            accountNumbers.Add(newAccountNumber);
            accountNames.Add($"{name} ");
            balances.Add(0.0);

            lastAccountNumber = newAccountNumber;

            Console.WriteLine($"Account created for {name} with Account Number: {newAccountNumber}");
        }

        static void Deposit()
        {
            int index = GetAccountIndex();
            if (index == -1) return;

            try
            {
                Console.Write("Enter deposit amount: ");
                double amount = Convert.ToDouble(Console.ReadLine());

                if (amount <= 0)
                {
                    Console.WriteLine("Amount must be positive.");
                    return;
                }

                balances[index] += amount;
                Console.WriteLine("Deposit successful.");
            }
            catch
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        static void Withdraw()
        {
            int index = GetAccountIndex();
            if (index == -1) return;

            try
            {
                Console.Write("Enter withdrawal amount: ");
                double amount = Convert.ToDouble(Console.ReadLine());

                if (amount <= 0)
                {
                    Console.WriteLine("Amount must be positive.");
                    return;
                }

                if (balances[index] - amount >= MinimumBalance)
                {
                    balances[index] -= amount;
                    Console.WriteLine("Withdrawal successful.");
                }
                else
                {
                    Console.WriteLine("Insufficient balance after minimum limit.");
                }
            }
            catch
            {
                Console.WriteLine("Invalid amount.");
            }
        }
        static void ViewBalance()
        {
            int index = GetAccountIndex();
            if (index == -1) return;

            Console.WriteLine($"Account Number: {accountNumbers[index]}");
            Console.WriteLine($"Holder Name: {accountNames[index]}");
            Console.WriteLine($"Current Balance: {balances[index]}");
        }

        static void SaveAccountsInformationToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AccountsFilePath))//Through it we create the object to write to the file
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        string dataLine = $"{accountNumbers[i]},{accountNames[i]},{balances[i]}";
                        writer.WriteLine(dataLine);
                    }
                }
                Console.WriteLine("Accounts saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }
        }




        }


















        //========== valadition ==========

        // string validation__________________ 
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
        // validate numeric strting
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

