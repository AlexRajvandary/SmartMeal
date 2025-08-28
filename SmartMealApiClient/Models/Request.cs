namespace SmartMealApiClient.Models
{
    public abstract class Request
    {
        public Request(string command)
        {
            Command = command;
        }

        public string Command { get; set; }
    }
}