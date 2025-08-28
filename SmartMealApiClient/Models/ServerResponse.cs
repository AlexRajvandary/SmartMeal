namespace SmartMealApiClient.Models
{
    public class ServerResponse
    {
        public string Command { get; set; } = null!;
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}