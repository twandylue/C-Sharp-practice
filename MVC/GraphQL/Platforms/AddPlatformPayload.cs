using MVC.Models;

namespace MVC.GraphQL.Platforms
{
    // public record AddPlatformPayload(Platform platform);

    public class AddPlatformPayload
    {
        public Platform platform { get; set; }
    }
}