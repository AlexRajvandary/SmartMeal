namespace SmartMealApiClient.Models
{
    public class GetMenuParameters
    {
        public GetMenuParameters(bool withPrice) 
        {
            WithPrice = withPrice;
        }

        public bool WithPrice { get; set; }
    }
}