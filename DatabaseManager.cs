using System;
using Npgsql;

namespace ATMManagementSystem
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        // ✅ Constructor to initialize connection to PostgreSQL
        public DatabaseManager(string host, string dbName, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(dbName) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Database connection parameters cannot be empty");
            }

            _connectionString = $"Host={host};Database={dbName};Username={username};Password={password}";
        }

        // ✅ Method to retrieve account details based on account number
        public Account? GetAccount(string? accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                Console.WriteLine("Invalid account number");
                return null;
            }

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT accountnumber, pin, balance FROM bankaccounts WHERE accountnumber = @AccountNumber";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string accountNum = reader.GetString(0);
                                string pin = reader.GetString(1);
                                decimal balance = reader.GetDecimal(2);

                                return new Account(accountNum, pin, balance);
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"\n❌ Database error: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error fetching account: {ex.Message}\n");
            }

            return null;
        }

        // ✅ Method to update account balance
        public void UpdateBalance(string? accountNumber, decimal newBalance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                Console.WriteLine("Invalid account number");
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "UPDATE bankaccounts SET balance = @Balance WHERE accountnumber = @AccountNumber";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Balance", newBalance);
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("\n⚠️ No account found with the specified account number\n");
                        }
                        else
                        {
                            Console.WriteLine("\n✅ Balance updated successfully\n");
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"\n❌ Database error: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error updating balance: {ex.Message}\n");
            }
        }
    }
}
