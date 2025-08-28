using Npgsql;
using SmartMealApiClient.Models;

namespace SmsConsoleApp
{
    public class MenuSaver
    {
        private readonly string _connectionString;
        private readonly IPresenter _presenter;

        public MenuSaver(string connectionString, IPresenter presenter)
        {
            _connectionString = connectionString;
            _presenter = presenter;
        }

        public void SaveMenuToDatabase(List<MenuItem> menuItems)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            foreach (var item in menuItems)
            {
                var command = new NpgsqlCommand("INSERT INTO MenuItems (Name, Article, Price) VALUES (@name, @article, @price)", connection);
                command.Parameters.AddWithValue("name", item.Name);
                command.Parameters.AddWithValue("article", item.Article);
                command.Parameters.AddWithValue("price", item.Price);
                command.ExecuteNonQuery();
            }
        }

        public void DisplayMenu(List<MenuItem> menuItems)
        {
            foreach (var item in menuItems)
            {
                _presenter.WriteLine($"{item.Name} - {item.Article} - {item.Price} руб.");
            }
        }
    }
}