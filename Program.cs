﻿using System;
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
        const string AccountsFilePath = "accounts.txt";
        const string ReviewsFilePath = "reviews.txt";

        // ____Global lists (parallel)______
        static List<int> accountNumbers = new List<int>();
        static List<string> accountNames = new List<string>();
        static List<double> balances = new List<double>();


        //_____Stacks & Queues_____________

        static Stack<string> reviewsStack = new Stack<string>();

        //_______Account number generator_____
        static int lastAccountNumber;

        static Queue<string> createAccountRequests = new Queue<string>(); // format: "Name|NationalID"
        static void Main(string[] args)
        {
           LoadAccountsInformationFromFile();//This function makes the program retrieve all previous calculations from a text file and prepare them for operation while it runs.
               
            LoadReviews();//Read reviews from the file and refill them in the stack.

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
                            SaveAccountsInformationToFile();// This saves all account data to a text file in an organized and secure manner.
                            SaveReviews();//All revisions in a stack are saved in a text file.
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
                    case "4": ViewBalance(); break;
                    case "5": SubmitReview(); break;
                    case "0": inUserMenu = false; break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
                Console.WriteLine("press any key");
                Console.ReadLine();
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
                    case "2": ViewReviews(); break;
                    case "3": ViewAllAccounts(); break;
                    case "4": ViewPendingRequests(); break;
                    case "0": inAdminMenu = false; break; // this will Eixt the  loop and return
                    default: Console.WriteLine("Invalid choice."); break;
                }
                Console.WriteLine("press any key");
                Console.ReadLine();
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

        //________view balance (5)___________
        static void ViewBalance()
        {
            int index = GetAccountIndex();
            if (index == -1) return;//If the account is not found (index == -1), the function terminates immediately without showing anything.

            Console.WriteLine($"Account Number: {accountNumbers[index]}");//This prints the account number corresponding to the indicator entered by the user.
            Console.WriteLine($"Holder Name: {accountNames[index]}");//This prints the name of the account holder corresponding to this account.
            Console.WriteLine($"Current Balance: {balances[index]}");//This prints the current balance of the account selected by the user.
        }
        //___________Save Accounts Information To File (6)_______
        static void SaveAccountsInformationToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(AccountsFilePath))//Open a file using StreamWriter, the file path is specified via a variable named AccountsFilePath and using here ensures that the file will be closed automatically after completion.
                {
                    for (int i = 0; i < accountNumbers.Count; i++)//Here starts the for loop to iterate over all the accounts stored in the lists.
                    {
                        string dataLine = $"{accountNumbers[i]},{accountNames[i]},{balances[i]}";
                        writer.WriteLine(dataLine);//The dataLine we prepared is written inside the file as a new line.
                    }
                }
                Console.WriteLine("Accounts saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }
        }
        //__________ Load Accounts Information From File (7)______
        static void LoadAccountsInformationFromFile()
        {
            try
            {
                if (!File.Exists(AccountsFilePath))//First, it checks whether the file specified in AccountsFilePath exists. If it doesn't, it prints a message "No saved data found" and terminates the function (return)
                {
                    Console.WriteLine("No saved data found.");
                    return;
                }
                //Before loading new data, it flushes the lists: deleting all accounts, names, and balances in memory (to prevent duplicate data when reloading)
                accountNumbers.Clear();
                accountNames.Clear();
                balances.Clear();
                //transactions.Clear();

                using (StreamReader reader = new StreamReader(AccountsFilePath))//Opens the specified file for reading using the StreamReader. When using is used, ensures that the file is automatically closed when finished.
                {
                    //Starts reading the file line by line.In each iteration: reads a new line until it reaches the end of the file(null)
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(','); //The line is divided into parts based on the comma (,) Each line is assumed to contain: the account number, then the customer name, then the balance
                        int accNum = Convert.ToInt32(parts[0]);
                        accountNumbers.Add(accNum);
                        accountNames.Add(parts[1]);
                        balances.Add(Convert.ToDouble(parts[2]));//Converts the first part (the account number) from string to an integer (int)

                        //Verifies: If the current account number is greater than the last account number (lastAccountNumber), the lastAccountNumber is updated to be equal to it. This is useful for correctly assigning new account numbers when creating a new account.
                        if (accNum > lastAccountNumber)
                            lastAccountNumber = accNum;
                    }
                }

                Console.WriteLine("Accounts loaded successfully.");
            }
            catch
            {
                Console.WriteLine("Error loading file.");
            }

        }
        //_____________view all accounts(8)_____
        static void ViewAllAccounts()//To display data 
        {
            Console.WriteLine("\n--- All Accounts ---");//Prints an address to show the user that all accounts will be displayed.
            for (int i = 0; i < accountNumbers.Count; i++)//Starts a for loop to iterate over all the accounts in the list accountNumbers where i represents the current index in the lists.
            {
                Console.WriteLine($"{accountNumbers[i]} - {accountNames[i]} - Balance: {balances[i]}");
            }
        }
        //_______________View Pending Requests (9)________
        static void ViewPendingRequests()
        {
            Console.WriteLine("\n--- Pending Account Requests ---");
            if (createAccountRequests.Count == 0)//Checks whether there are any pending account creation requests. createAccountRequests is a list or queue containing requests.
            {
                Console.WriteLine("No pending requests.");//If no requests exist: Prints "No requests are pending." Terminates the function immediately using return
                return;
            }

            foreach (string request in createAccountRequests)//If requests are found, iterates over them one by one using a foreach loop. Each element in createAccountRequests is a string representing a single request.
            {
                string[] parts = request.Split('|'); //The request is divided into parts using the separator | and the request is expected to be written in this format: "Customer Name|ID Number"
                Console.WriteLine($"Name: {parts[0]}, National ID: {parts[1]}");
            }
        }

        //_________Get Account Index(10)____________
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

        // ===== Reviews & Complaints (Stack) =====
        //____________Submit Review (1)________________


        static void SubmitReview()
        {
            Console.Write("Enter your review or complaint: ");//There user is asked to enter his opinion or complaint.
            string review = Console.ReadLine();//to reads the text typed by the user and saves it in a variable named review.
            reviewsStack.Push(review);//Push pushes text onto the stack (reviewStack). Stacks operate on a last-in, first-out (LIFO) basis.
            Console.WriteLine("Thank you! Your feedback has been recorded.");
        }
        //__________view reviews (2)___________
        static void ViewReviews()
        {
            if (reviewsStack.Count == 0)//Checks if there are any existing reviews/complaints, reviewsStack.Count is the number of items inside the stack
            {
                Console.WriteLine("No reviews or complaints submitted yet.");//If the number of reviews = zero: Prints a message to the user that there are no reviews or complaints and terminates the function immediately (return).
                return;
            }

            Console.WriteLine("Recent Reviews/Complaints (most recent first):");//If reviews are found, a caption will be printed indicating that what will be displayed is from newest to oldest.
            foreach (string r in reviewsStack)//The iteration starts over all the reviews stored in reviewsStack. In each iteration, the variable r contains the text of a single review or complaint.
            {
                Console.WriteLine("- " + r);
            }
        }

        //__________save reviews (3)____________
        static void SaveReviews()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ReviewsFilePath))//We create a StreamWriter object to write text to a file, the file is specified via the ReviewsFilePath variable (it must be predefined, e.g. its value could be "reviews.txt").
                {
                    foreach (var review in reviewsStack)//We start a foreach loop that goes through all the reviews inside reviewsStack, where each review is stored in the review variable.
                    {
                        writer.WriteLine(review);//We write each review as a separate line within the file using WriteLine ،That is, each review is placed on its own line.
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error saving reviews.");
            }
        }
        //____________load reviews (4)_______
        static void LoadReviews()
        {
            try//We start a try block to safely load revisions and if anything goes wrong (such as the file not existing or the read failed), we move to a catch block.
            {
                if (!File.Exists(ReviewsFilePath)) return;//We check: If the file in the path ReviewsFilePath does not exist, we exit the function directly (we do not try to read).

                using (StreamReader reader = new StreamReader(ReviewsFilePath))//We create a StreamReader object to open the file for reading.
                {
                    string line;//We define a line variable to read each line of the file one by one.
                    while ((line = reader.ReadLine()) != null)//While loop: As long as there is a new line in the file (we haven't reached the end), we read it،ReadLine() reads one line at a time.
                    {
                        reviewsStack.Push(line); //We take the read line and push it to the reviewsStack stack using push, meaning we add it as a new item at the top.
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error loading reviews.");
            }
        }



        //========== valadition ==========

        // ________string validation__________________ 
        //The purpose of this function is to check the validity of a text string (word) that contains only alphabetic characters (either lowercase or uppercase), and if the string contains anything else (such as numbers or symbols), it is considered invalid.
        public static string stringOnlyLetterValidation(string word)
        {//IsValid: A variable of type bool used to determine whether a string is valid or not, ValidWord: A variable of type string in which the valid word will be stored if it contains only letters.
            bool IsValid = true;//A logical variable to determine whether a word is valid.
            string ValidWord = "";//If the word is valid, it will be saved here.
            if (string.IsNullOrEmpty(word) && word.All(char.IsLetter))
            {
                Console.WriteLine("Input is just empty!");
                IsValid = false;

            }
            else
            {
                IsValid = true;
            }

            if (Regex.IsMatch(word, @"^[a-zA-Z]+$"))//A regular expression (Regex) is used to ensure that a word contains only English letters (upper and lower case).
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

