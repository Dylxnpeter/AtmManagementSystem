using System;

namespace ATMManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the ATM System");

            Console.Write("Enter Account Number: ");
            string? inputAccountNumber = Console.ReadLine();
            string accountNumber = inputAccountNumber ?? string.Empty;

            Console.Write("Enter PIN: ");
            string? inputPin = Console.ReadLine();
            string pin = inputPin ?? string.Empty;

            // ✅ Updated with correct PostgreSQL details
            var databaseManager = new DatabaseManager("localhost", "Atm_Management", "postgres", "20408007013");
            var account = databaseManager.GetAccount(accountNumber);

            if (account != null && account.ValidatePin(pin))
            {
                Console.WriteLine("\nLogin successful!\n");
                ShowMenu(account, databaseManager);
            }
            else
            {
                Console.WriteLine("\nInvalid account number or PIN. Please try again.\n");
            }
        }

        static void ShowMenu(Account account, DatabaseManager databaseManager)
        {
            while (true)
            {
                Console.WriteLine("\n--- ATM Menu ---");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                string? inputOption = Console.ReadLine();
                string option = inputOption ?? string.Empty;

                switch (option)
                {
                    case "1":
                        Console.WriteLine($"\nYour current balance is: {account.Balance:C}");
                        break;

                    case "2":
                        Console.Write("\nEnter deposit amount: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount) && depositAmount > 0)
                        {
                            account.UpdateBalance(depositAmount); 
                            databaseManager.UpdateBalance(account.AccountNumber, account.Balance);
                            Console.WriteLine($"\nDeposited {depositAmount:C}. New balance: {account.Balance:C}");
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid amount. Please try again.");
                        }
                        break;

                    case "3":
                        Console.Write("\nEnter withdrawal amount: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount) && withdrawAmount > 0)
                        {
                            if (withdrawAmount <= account.Balance)
                            {
                                account.UpdateBalance(-withdrawAmount); 
                                databaseManager.UpdateBalance(account.AccountNumber, account.Balance);
                                Console.WriteLine($"\nWithdrew {withdrawAmount:C}. New balance: {account.Balance:C}");
                            }
                            else
                            {
                                Console.WriteLine("\nInsufficient funds.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid amount. Please try again.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("\nThank you for using the ATM. Goodbye!");
                        return;

                    default:
                        Console.WriteLine("\nInvalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
