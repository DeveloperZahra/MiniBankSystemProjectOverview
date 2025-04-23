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






















    }
}

