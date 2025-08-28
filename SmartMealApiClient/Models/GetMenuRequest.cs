namespace SmartMealApiClient.Models
{
    public class GetMenuRequest : Request
    {
        private const string _command = "GetMenu";

        public GetMenuRequest(GetMenuParameters getMenuParameters) : base(_command)
        {
            CommandParameters = getMenuParameters;
        }

        public GetMenuRequest(bool withPrice) : base(_command)
        {
            CommandParameters = new GetMenuParameters(withPrice);
        }

        public GetMenuParameters CommandParameters { get; set; }
    }
}
