using HotChocolate;
using System.Linq;
using MVC.GraphQL.Data;
using MVC.Respository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVC.GraphQL.Extensions;

namespace MVC.GraphQL
{
    public class Query
    {
        [UseApplicationDbContext]
        // public IQueryable<Speaker> GetSpeakers([Service] PostgresDBContext context) => context.Speakers;
        public Task<List<Speaker>> GetSpeakers([ScopedService] PostgresDBContext context) => context.Speakers.ToListAsync();
    }
}