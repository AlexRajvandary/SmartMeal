namespace SmartMealApiClient.Models
{
    public class GetMenuItemsServerResponse
    {
        public string Command { get; set; } = null!;
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public List<MenuItem>? Data { get; set; }
    }
}