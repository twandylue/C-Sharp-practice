namespace MVC.GraphQL.Commands
{
    public class AddCommandInput
    {
        public string HowTo { get; set; }
        public string CommandLine { get; set; }
        public int PlatformId { get; set; }
    }
}