using Sms.Test;

namespace SmartMealApiClient.Services
{
    public interface IGrpcApiClient
    {
        Task<List<MenuItem>> GetMenuAsync(bool withPrice);
        Task<bool> SendOrderAsync(string orderId, List<OrderItem> orderItems);
    }
}