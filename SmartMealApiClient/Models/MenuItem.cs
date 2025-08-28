namespace SmartMealApiClient.Models
{
    public class MenuItem
    {
        public string Id { get; set; } = null!;
        public string Article { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public bool IsWeighted { get; set; }
        public string FullPath { get; set; } = null!;
        public string[] Barcodes { get; set; } = null!;
    }
}