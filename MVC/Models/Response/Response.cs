namespace MVC.Models.Response
{
    public class SuccessResponse
    {
        public string message { get; set; }
        public string redirect_Uri { get; set; }
    }
    public class FailedResponse
    {
        public string message { get; set; }
    }
}