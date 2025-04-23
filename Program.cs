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
        static List<string> Passward = new List<string>();


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
                    Console.WriteLine("0. Exit the system");
                    Console.Write(" Please Select Your Option: ");
                    string mainChoice = Console.ReadLine();

                    switch (mainChoice)
                    {
                        case "1": UserMenu(); break;
                        case "2": AdminMenu(); break;
                        case "0":
                            SaveAccountsInformationToFile();
                            SaveReviews();
                            running = false;
                            break;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }

                Console.WriteLine("Do you want another operation ? y / n");
                choice = Console.ReadKey().KeyChar;

            } while (choice == 'y' || choice == 'Y');
            Console.WriteLine("Thank you for using Bank System!");
        }






















    }
}

