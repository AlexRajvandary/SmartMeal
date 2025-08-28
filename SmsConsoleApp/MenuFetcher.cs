using SmartMealApiClient.Models;
using SmartMealApiClient.Services;

namespace SmsConsoleApp
{
    public class MenuFetcher
    {
        private readonly IHttpApiClient? _apiClient;
        private readonly IGrpcApiClient? _grpcApiClient;
        private readonly bool _useGrpc;

        private readonly IPresenter _presenter;

        public MenuFetcher(IHttpApiClient apiClient, IPresenter presenter)
        {
            _apiClient = apiClient;
            _useGrpc = false;
            _presenter = presenter;
        }

        public MenuFetcher(IGrpcApiClient grpcApiClient, IPresenter presenter)
        {
            _grpcApiClient = grpcApiClient;
            _useGrpc = true;
            _presenter = presenter;
        }

        public async Task<List<MenuItem>> GetMenu()
        {
            var menuItems = _useGrpc ?
                                (await _grpcApiClient!.GetMenuAsync(true)).Select(protoMenuItem => protoMenuItem.ToMenuItem()).ToList()
                                : await _apiClient!.GetMenuAsync(true);

            if (menuItems == null || menuItems.Count == 0)
            {
                _presenter.WriteLine("Ошибка при получении меню с сервера.");
                Environment.Exit(1);
            }

            return menuItems;
        }
    }
}