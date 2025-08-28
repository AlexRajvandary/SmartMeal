namespace SmartMealApiClient.Models
{
    public class SendOrderRequest : Request
    {
        private const string _command = "SendOrder";

        public SendOrderRequest(SendOrderParameters sendOrderParameters) : base(_command)
        {
            CommandParameters = sendOrderParameters;
        }

        public SendOrderRequest(string orderId, List<OrderItem> orderItems) : base(_command)
        {
            CommandParameters = new SendOrderParameters(orderId, orderItems);
        }

        public SendOrderParameters CommandParameters { get; set; }
    }
}