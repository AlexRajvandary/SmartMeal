namespace WpfSmsTestClient.Services
{
    public interface IMessageBoxService
    {
        void ShowError(string message);
        void ShowNotification(string message);
    }
}