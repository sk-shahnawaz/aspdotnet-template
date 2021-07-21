using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

using ASP.NET.Core.WebAPI.Utilities;
using ASP.NET.Core.WebAPI.Models.UtilityModels;
using ASP.NET.Core.WebAPI.Infrastructure.EFCore;
using ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;
using ASP.NET.Core.WebAPI.Helpers.Middlewares;

namespace ASP.NET.Core.WebAPI
{
    /// <summary>
    /// Startup type used by the web host to configure services and HTTP request processing pipeline.
    /// </summary>
    public class Startup
    {

        #region --- Global Variables ---

        #endregion --- Global Variables ---

        #region --- Properties ---

        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion --- Properties ---

        #region --- Constructors ---

        /// <summary>
        /// Type constructor to inject application configuration properties.
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion --- Constructors ---

        #region --- Methods ---

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">An abstration representing application services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            /* 
             * Try not to clutter the method definition by service provider implementations here.
             *  If need to register a service which requires multi-line provider implementation, 
             *  then move provider logic in separate class (Single Responsibilty principle). 
            */

            services.AddOptions<EnvironmentConfiguration>(Configuration, out EnvironmentConfiguration environmentConfiguration);
            services.AddOptions<ApplicationConfiguration>(Configuration, out ApplicationConfiguration applicationConfiguration);
            services.AddLogging(environmentConfiguration);
            services.AddCors(environmentConfiguration);
            services.AddRoutingConfigurations();
            services.AddDependencies(environmentConfiguration, applicationConfiguration);
            services.AddVersioning();               // Must come before AddControllers call.
            services.AddWebApi(applicationConfiguration);
            services.AddOpenApi();
            services.AddHealthChecks();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request processing pipeline.
        /// </summary>
        /// <param name="app">An abstraction which provides mechanisms to configure the HTTP request processing pipeline.</param>
        /// <param name="env">An abstraction which provides information about the web hosting environment within which this application is running.</param>
        /// <param name="provider">An abstraction representing OpenAPI specification configuration</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            /*
                While configuring ASP.NET Core pipeline please follow:
                https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0#middleware-order
                Accessed: 2021-06-01
             */

            ApplicationConfiguration applicationConfiguration = app.ApplicationServices.CreateScope().ServiceProvider.GetService<Microsoft.Extensions.Options.IOptions<ApplicationConfiguration>>().Value;
            if (applicationConfiguration?.UseInMemoryDatabase ?? false)
            {
                InMemoryDbDataSeeder.SeedTestData(app);
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(AppResources.CorsPolicyName);
            app.UseMiddleware<RequestResponseLogger>();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
			{
				if (applicationConfiguration?.EnableOData ?? false)
				{
					endpoints.EnableDependencyInjection();
					endpoints.Select().Expand().Filter();
				}
				endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
            app.UseSwagger(provider);
        }

        #endregion --- Methods ---
    }
}