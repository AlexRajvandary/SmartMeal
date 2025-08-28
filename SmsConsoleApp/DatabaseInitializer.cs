using Npgsql;

namespace SmsConsoleApp
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InitializeDatabase()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var createDbCommand = "CREATE DATABASE IF NOT EXISTS MenuOrders;";
            using var command = new NpgsqlCommand(createDbCommand, connection);
            command.ExecuteNonQuery();

            var createTableCommand = @"
            CREATE TABLE IF NOT EXISTS MenuItems (
                Id SERIAL PRIMARY KEY,
                Name TEXT NOT NULL,
                Article TEXT NOT NULL,
                Price DECIMAL(10, 2) NOT NULL
            );";
            using var tableCommand = new NpgsqlCommand(createTableCommand, connection);
            tableCommand.ExecuteNonQuery();
        }
    }
}
