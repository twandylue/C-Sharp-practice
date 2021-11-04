using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using MVC.Models;
using MVC.Respository;

namespace MVC.GraphQL.Platforms
{
    public class PlatformType : ObjectType<Platform>
    {
        protected override void Configure(IObjectTypeDescriptor<Platform> descriptor)
        {
            descriptor.Description("Represents any software or service that has a command line interface.");

            descriptor
                .Field(p => p.LicenseKey).Ignore();
            descriptor
                .Field(p => p.Commands)
                .ResolveWith<Resolvers>(p => p.GetCommands(default!, default!))
                .UseDbContext<PostgresDBContext>()
                .Description("This is the list of available commands for this platform.");
        }

        private class Resolvers
        {
            // * [Parent] -- HotChocolate v12 才有的功能
            public IQueryable<Command> GetCommands([Parent] Platform platform, [ScopedService] PostgresDBContext context)
            {
                return context.Commands.Where(p => p.PlatformId == platform.Id);
            }
        }
    }
}