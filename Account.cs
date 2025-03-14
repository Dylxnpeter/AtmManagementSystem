public class Account
{
    public string AccountNumber { get; }
    public string Pin { get; }
    public decimal Balance { get; private set; } // Allow internal modification

    public Account(string accountNumber, string pin, decimal balance)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number cannot be empty", nameof(accountNumber));

        if (string.IsNullOrWhiteSpace(pin))
            throw new ArgumentException("PIN cannot be empty", nameof(pin));

        if (balance < 0)
            throw new ArgumentException("Balance cannot be negative", nameof(balance));

        AccountNumber = accountNumber;
        Pin = pin;
        Balance = balance;
    }

    // Method to update balance (encapsulation)
    public void UpdateBalance(decimal amount)
    {
        Balance += amount;
    }

    // Method to validate PIN
    public bool ValidatePin(string? inputPin)
    {
        if (string.IsNullOrWhiteSpace(inputPin))
        {
            Console.WriteLine("Invalid PIN input");
            return false;
        }

        return inputPin == Pin;
    }
}
