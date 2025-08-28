using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Sms.Test;

namespace SmartMealApiClient.Services;

public class GrpcApiClient : IGrpcApiClient
{
    private readonly GrpcChannel _channel;
    private readonly SmsTestService.SmsTestServiceClient _client;

    public GrpcApiClient(string url)
    {
        _channel = GrpcChannel.ForAddress(url);
        _client = new SmsTestService.SmsTestServiceClient(_channel);
    }

    public async Task<List<MenuItem>> GetMenuAsync(bool withPrice)
    {
        var request = new BoolValue { Value = withPrice };
        var response = await _client.GetMenuAsync(request);

        if (!response.Success)
            throw new Exception($"Ошибка: {response.ErrorMessage}");

        return response.MenuItems.ToList();
    }

    public async Task<bool> SendOrderAsync(string orderId, List<OrderItem> orderItems)
    {
        var order = new Order
        {
            Id = orderId,
            OrderItems = { orderItems }
        };

        var response = await _client.SendOrderAsync(order);

        if (!response.Success)
            throw new Exception($"Ошибка: {response.ErrorMessage}");

        return response.Success;
    }
}