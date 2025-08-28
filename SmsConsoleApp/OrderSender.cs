using SmartMealApiClient.Models;
using SmartMealApiClient.Services;

namespace SmsConsoleApp
{
    public class OrderSender
    {
        private readonly IHttpApiClient? _httpApiClient;
        private readonly IGrpcApiClient? _grpcApiClient;
        private readonly bool _useGrpc;

        private readonly IPresenter _presenter;

        public OrderSender(IHttpApiClient httpApiClient, IPresenter presenter)
        {
            _httpApiClient = httpApiClient;
            _presenter = presenter;
        }

        public OrderSender(IGrpcApiClient grpcApiClient)
        {
            _grpcApiClient = grpcApiClient;
        }

        public async Task SendOrder(List<OrderItem> orderItems)
        {
            var orderId = Guid.NewGuid().ToString();

            var response = _useGrpc 
                ? await _grpcApiClient!.SendOrderAsync(orderId, orderItems.Select(item => item.ToProto()).ToList())
                : await _httpApiClient!.SendOrderAsync(orderId, orderItems);

            if (response)
            {
                _presenter.WriteLine("УСПЕХ");
            }
            else
            {
                _presenter.WriteLine("Ошибка при отправке заказа.");
            }
        }
    }
}