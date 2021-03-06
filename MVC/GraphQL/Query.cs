using HotChocolate;
using HotChocolate.Data;
using System.Linq;
using MVC.Respository;
using MVC.Models;

namespace MVC.GraphQL
{
    [GraphQLDescription("Represents the queries available.")]
    public class Query
    {
        [UseDbContext(typeof(PostgresDBContext))]
        [GraphQLDescription("Gets the queryable platform.")]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Platform> GetPlatforms([ScopedService] PostgresDBContext context)
        {
            return context.Platforms;
        }

        [UseDbContext(typeof(PostgresDBContext))]
        [GraphQLDescription("Gets the queryable command.")]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Command> GetCommands([ScopedService] PostgresDBContext context)
        {
            return context.Commands;
        }
    }
}