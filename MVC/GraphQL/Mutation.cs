using HotChocolate;
using HotChocolate.Data;
using MVC.Respository;
using System.Threading.Tasks;
using MVC.GraphQL.Platforms;
using MVC.Models;
using MVC.GraphQL.Commands;
using HotChocolate.Subscriptions;
using System.Threading;

namespace MVC.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(PostgresDBContext))]
        public async Task<AddPlatformPayload> AddPlatformAsync(
            AddPlatformInput input,
            [ScopedService] PostgresDBContext context,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken
            )
        {
            var _platform = new Platform
            {
                Name = input.Name
            };
            context.Platforms.Add(_platform);
            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(nameof(Subscription.OnPlatformAdded), _platform, cancellationToken);

            return new AddPlatformPayload
            {
                platform = _platform
            };
        }

        [UseDbContext(typeof(PostgresDBContext))]
        public async Task<AddCommandPayload> AddCommandAsync(AddCommandInput input, [ScopedService] PostgresDBContext context)
        {
            var _command = new Command
            {
                HowTo = input.HowTo,
                CommandLine = input.CommandLine,
                PlatformId = input.PlatformId
            };
            context.Commands.Add(_command);
            await context.SaveChangesAsync();
            return new AddCommandPayload
            {
                command = _command
            };
        }
    }
}