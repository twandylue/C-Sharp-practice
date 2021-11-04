using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using MVC.Models;
using MVC.Respository;

namespace MVC.GraphQL.Commands
{
    public class CommandType : ObjectType<Command>
    {
        protected override void Configure(IObjectTypeDescriptor<Command> descriptor)
        {
            descriptor.Description("Respresents any executable command.");

            descriptor
                .Field(c => c.Platform)
                .ResolveWith<Resolvers>(c => c.GetPlatform(default!, default!))
                .UseDbContext<PostgresDBContext>()
                .Description("This is the platform to which the command belongs.");
        }

        private class Resolvers
        {
            public Platform GetPlatform([Parent] Command command, [ScopedService] PostgresDBContext context)
            {
                return context.Platforms.FirstOrDefault(p => p.Id == command.PlatformId);
            }
        }
    }
}