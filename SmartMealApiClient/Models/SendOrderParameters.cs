namespace SmartMealApiClient.Models
{
    public class SendOrderParameters
    {
        public SendOrderParameters(string orderId, List<OrderItem> orderItems)
        {
            OrderId = orderId;
            MenuItems = orderItems;
        }

        public string OrderId { get; set; }
        public List<OrderItem> MenuItems { get; set; }
    }
}