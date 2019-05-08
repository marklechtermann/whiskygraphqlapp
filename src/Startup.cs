using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Whisky.GraphQL;
using WhiskyApp.DataAccess;
using WhiskyApp.DataAccess.Models;

namespace Whisky
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Database
            services.AddDbContext<WhiskyDbContext>(o => o.UseInMemoryDatabase("WhiskyDB"));

            // Repositories
            services.AddScoped<IWhiskyRepository, WhiskyRepository>();
            services.AddScoped<IDestilleryRepository, DestilleryRepository>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // For GraphQL
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddScoped<WhiskyType>();
            services.AddScoped<DestilleryType>();
            services.AddScoped<WhiskyRegionEnumType>();

            services.AddScoped<WhiskyInputType>();

            services.AddScoped<WhiskyQuery>();
            services.AddScoped<WhiskyMutation>();
            services.AddScoped<ISchema, WhiskyAppSchema>();

            services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
            services.AddSingleton<DataLoaderDocumentListener>();

            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = true;
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(f =>
                {
                    var actionContext = f.GetService<IActionContextAccessor>().ActionContext;
                    return new UrlHelper(actionContext);
                }
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            FillTheBarrels(services.GetService<WhiskyDbContext>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });


            // Register GraphQL middleware            
            app.UseMiddleware<GraphQLMiddleware>(new GraphQLSettings() { });

            //app.UseGraphiQLServer(new GraphiQLOptions());
            //app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            //app.UseGraphQLVoyager(new GraphQLVoyagerOptions());

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    ///spa.UseAngularCliServer(npmScript: "start");
                }
                else
                {
                    spa.Options.SourcePath = "ClientApp";
                }
            });
        }

        private static void FillTheBarrels(WhiskyDbContext context)
        {
            var ardbeg = context.Distilleries.Add(new DestilleryEntity()
            {
                Id = "1",
                Name = "Ardbeg",
                Capacity = 1000000,
                WashStills = 1,
                SpiritStills = 1,
                Region = WhiskyRegion.Islay,
                Owner = "Moët Hennessy Louis Vuitton",
                Description = @"The distillery Ardbeg has arisen from an illegal distillery which has been in production since 1794. Ardbeg has been officially founded by John McDougall 1815. The distillery is located at the south side of Islay in the neighbourhood of Port Ellen, Lagavulin and Laphroaig. 
Ardbeg has been closed from 1981 to 1989 and from 1996 to 1997. Especially the eight years in the 80s have a very negative influence to the Ardbeg products nowadays. Because of the interruption in the production, there are no casks from the eighties available. These vintage bottlings are missing in product palette. 
As can be notified throughout a lot of distilleries, there is a wide range of bottlings without an age notice. This gives the distilleries a chance to mix older casks with pretty new ones. Ardbeg is one of the distilleries which drives this way of marketing to the top. All bottlings in last years have come out without an age declaration.
Ardbeg is furthermore one of the distilleries with a very aggressive marketing. Every year comes a new bottling to the market and there is a lot of commercials and advertising for that. The release of the new bottling is celebrated at the so called ,Ardbeg Day‘. 
(https://www.whiskybase.com/whiskies/distillery/6/about)",

                Whiskys = new List<WhiskyEntity>
                    {
                        new WhiskyEntity() {Id ="1", Name = "Ardbeg TEN", Age = 10, Size = 700, Strength = 46.0 },
                        new WhiskyEntity() {Id ="2", Name = "Ardbeg Uigeadail", Age = 0, Size = 700, Strength = 54.2 },
                        new WhiskyEntity() {Id ="3", Name = "Ardbeg An Oa", Age = 0, Size = 700, Strength = 46.6 },
                        new WhiskyEntity() {Id ="4", Name = "Ardbeg Corryvreckan", Age = 0, Size = 700, Strength = 57.1 }
                    }
            });

            var laphroaig = context.Distilleries.Add(new DestilleryEntity()
            {
                Id = "2",
                Name = "Laphroaig",
                Capacity = 2700000,
                WashStills = 3,
                SpiritStills = 3,
                Region = WhiskyRegion.Islay,
                Owner = "Beam Suntory",
                Description = @"Laphroaig is one of the few distilleries with its own malting floor. The produce of this floor is heated in their own kiln, first on a very low temperature over smoky peat to bring the peat aroma in the malt. Then the malt is heated a second time, now at high temperature, to stop it germinating. The peat comes from Laphroaig's own moorlands.
This homemade malt is only about 10% of the total amount of malt that the distillery uses.
For the wash process Laproaig uses liquid yeast. As our spokesman said, that is to free the employees of carrying heavy bags of yeast around.
(https://www.whiskybase.com/whiskies/distillery/49/about)",
                Whiskys = new List<WhiskyEntity>
                    {
                        new WhiskyEntity() {Id ="5", Name = "Laphroaig 10", Age = 10, Size = 700, Strength = 40.0  },
                        new WhiskyEntity() {Id ="6", Name = "Laphroaig 15", Age = 15, Size = 700, Strength = 43.0  },
                        new WhiskyEntity() {Id ="7", Name = "Laphroaig Quarter Cask", Age = 0, Size = 700, Strength = 48.0  },
                        new WhiskyEntity() {Id ="8", Name = "Laphroaig Triple Wood", Age = 0, Size = 700, Strength = 48.0  }
                    }
            });

            context.SaveChanges();

        }
    }
}
