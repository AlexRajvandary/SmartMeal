using SmartMealApiClient.Models;

namespace SmsConsoleApp
{
    public class OrderInputHandler
    {
        private readonly List<MenuItem> _menuItems;
        private readonly IPresenter _presenter;

        public OrderInputHandler(List<MenuItem> menuItems, IPresenter presenter)
        {
            _menuItems = menuItems;
            _presenter = presenter;
        }

        public List<OrderItem> GetOrderFromUser()
        {
            var orderItems = new List<OrderItem>();

            while (true)
            {
                _presenter.WriteLine("Введите список блюд (например: A1001:2;A1002:3):");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    _presenter.WriteLine("Ошибка ввода. Повторите ввод.");
                    orderItems.Clear();
                    break;
                }

                string[] items = input!.Split(';');

                foreach (var item in items)
                {
                    var parts = item.Split(':');
                    string code = parts[0];
                    int quantity = Convert.ToInt32(parts[1]);

                    if (_menuItems.Any(m => m.Article == code) && quantity > 0)
                    {
                        var menuItem = _menuItems.First(m => m.Article == code);
                        orderItems.Add(new OrderItem { Id = menuItem.Id, Quantity = quantity });
                    }
                    else
                    {
                        _presenter.WriteLine("Ошибка ввода. Повторите ввод.");
                        orderItems.Clear();
                        break;
                    }
                }

                if (orderItems.Count > 0)
                    break;
            }

            return orderItems;
        }
    }
}