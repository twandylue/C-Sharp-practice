using HotChocolate;
using HotChocolate.Data;
using System.Linq;
using MVC.GraphQL.Data;
using MVC.Respository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.GraphQL
{
    [GraphQLDescription("Represents the queries available.")]
    public class Query
    {
        [UseDbContext(typeof(PostgresDBContext))]
        [GraphQLDescription("Gets the queryable platform.")]
        // public IQueryable<Speaker> GetSpeakers([Service] PostgresDBContext context) => context.Speakers;
        public Task<List<Platform>> GetPlatforms([ScopedService] PostgresDBContext context)
        {
            return context.Platforms.ToListAsync();
        }

        [UseDbContext(typeof(PostgresDBContext))]
        [GraphQLDescription("Gets the queryable command.")]
        public Task<List<Command>> GetCommands([ScopedService] PostgresDBContext context)
        {
            return context.Commands.ToListAsync();
        }
    }
}