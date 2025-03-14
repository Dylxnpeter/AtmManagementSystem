using System;

namespace ATMManagementSystem
{
    public class ATM
    {
        private readonly DatabaseManager dbManager;

        // Constructor to initialize DatabaseManager
        public ATM(DatabaseManager databaseManager)
        {
            dbManager = databaseManager ?? throw new ArgumentNullException(nameof(databaseManager));
        }

        // Method to handle user login
        public void Login()
        {
            Console.Write("Enter Account Number: ");
            string? inputAccountNumber = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(inputAccountNumber))
            {
                Console.WriteLine("Invalid account number! Please try again.");
                return;
            }

            // Fetch account details from the database
            Account? account = dbManager.GetAccount(inputAccountNumber);

            if (account != null)
            {
                Console.Write("Enter PIN: ");
                string? inputPin = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(inputPin))
                {
                    Console.WriteLine("Invalid PIN! Please try again.");
                    return;
                }

                if (account.ValidatePin(inputPin))
                {
                    Console.WriteLine("\nLogin successful!");
                    HandleTransaction(account);
                }
                else
                {
                    Console.WriteLine("Invalid PIN! Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Account not found! Please try again.");
            }
        }

        // Method to handle transactions
        private void HandleTransaction(Account account)
        {
            while (true)
            {
                Console.WriteLine("\nSelect Transaction:");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Exit");
                Console.Write("Enter choice: ");
                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"\nCurrent Balance: {account.Balance:C}");
                        break;

                    case "2":
                        Console.Write("\nEnter deposit amount: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount) && depositAmount > 0)
                        {
                            account.UpdateBalance(depositAmount);
                            dbManager.UpdateBalance(account.AccountNumber, account.Balance);
                            Console.WriteLine("Deposit successful!");
                        }
                        else
                        {
                            Console.WriteLine("Invalid deposit amount!");
                        }
                        break;

                    case "3":
                        Console.Write("\nEnter withdrawal amount: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawalAmount) && withdrawalAmount > 0)
                        {
                            if (withdrawalAmount > account.Balance)
                            {
                                Console.WriteLine("Insufficient funds!");
                            }
                            else
                            {
                                account.UpdateBalance(-withdrawalAmount);
                                dbManager.UpdateBalance(account.AccountNumber, account.Balance);
                                Console.WriteLine("Withdrawal successful!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid withdrawal amount!");
                        }
                        break;

                    case "4":
                        Console.WriteLine("\nThank you for using the ATM!");
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}
