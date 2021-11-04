using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC.Models;
using MVC.Respository;
using MVC.GraphQL;
using MVC.GraphQL.Platforms;
using MVC.GraphQL.Commands;
using HotChocolate;
using GraphQL.Server.Ui.Voyager;

namespace MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews();
            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddType<PlatformType>()
                .AddType<CommandType>()
                .AddFiltering()
                .AddSorting();
            // .AddMutationType<Mutation>();

            // services.AddScoped<IAccountData, DBAccountsData>(); // DI // ! 待釐清跟生命週期的關係 (AddPooledDbContextFactory)
            // services.AddScoped<ISSOAccount, DBSSOAccountData>(); // DI
            services.AddPooledDbContextFactory<PostgresDBContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("PostgresConnectionString"));
                options.EnableSensitiveDataLogging();
            });

            // ! 待釐清跟生命週期的關係
            services.AddDbContextPool<PostgresDBContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("PostgresConnectionString"));
                options.EnableSensitiveDataLogging();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapGraphQL(); // * enable graphql
                // ! 待釐清跟生命週期的關係
            });

            app.UseGraphQLVoyager(new GraphQLVoyagerOptions()
            {
                GraphQLEndPoint = "/graphql",
                Path = "/graphql-voyager"
            });
        }
    }
}
