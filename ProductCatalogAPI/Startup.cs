using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductCatalogAPI.Data;

namespace ProductCatalogAPI
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
            services.Configure<CatalogSettings>(Configuration);


            //Build Connection string
            var databaseServer = Configuration["DatabaseServer"];
            var databaseName = Configuration["DatabaseName"];
            var databaseUser = Configuration["DatabaseUser"];
            var databasePassword = Configuration["DatabaseUserPassword"];
            var connectionString = String.Format("Data Source={0};Initial Catalog={1};Integrated Security=False;User ID={2};Password={3};MultipleActiveResultSets=True;", databaseServer, databaseName, databaseUser, databasePassword);

            if (databaseServer != null)
            {
                services.AddDbContext<CatalogContext>
                    (options =>
                     options.UseSqlServer(connectionString));
            }
            else
            {
                services.AddDbContext<CatalogContext>
                    (options =>
                     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "E-Commerce Docker API",
                    Version = "v1",
                    Description = "The product Catalog Microservice HTTPS API is a CRUD api.",
                    TermsOfService = "Terms of Service"
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "ProductCatalogAPI V1");
                });


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
