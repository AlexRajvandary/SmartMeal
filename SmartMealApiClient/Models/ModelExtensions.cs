namespace SmartMealApiClient.Models
{
    public static class ModelExtensions
    {
        public static MenuItem ToMenuItem (this Sms.Test.MenuItem protoMenuItem)
        {
            var menuItem = new MenuItem()
            {
                Id = protoMenuItem.Id,
                Article = protoMenuItem.Article,
                Name = protoMenuItem.Name,
                Price = protoMenuItem.Price,
                IsWeighted = protoMenuItem.IsWeighted,
                FullPath = protoMenuItem.FullPath,
                Barcodes = protoMenuItem.Barcodes.ToArray()
            };

            return menuItem;
        }

        public static Sms.Test.OrderItem ToProto(this OrderItem orderItem)
        {
            var protoOrderItem = new Sms.Test.OrderItem()
            {
                Id = orderItem.Id,
                Quantity = orderItem.Quantity
            };

            return protoOrderItem;
        }
    }
}