using HotChocolate;
using HotChocolate.Types;
using MVC.Models;

namespace MVC.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic]
        public Platform OnPlatformAdded([EventMessage] Platform platform) => platform;
    }
}