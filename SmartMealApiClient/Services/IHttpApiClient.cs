using SmartMealApiClient.Models;

namespace SmartMealApiClient.Services
{
    public interface IHttpApiClient
    {
        Task<List<MenuItem>> GetMenuAsync(bool withPrice);
        Task<bool> SendOrderAsync(string orderId, List<OrderItem> orderItems);
    }
}