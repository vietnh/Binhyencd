using BTDB.KVDBLayer;
using RealMusic.Db;

namespace RealMusic
{
    using Microsoft.Extensions.DependencyInjection;
    using RealMusic.Services;

    public partial class Startup
    {
        /// <summary>
        /// Configures custom services to add to the ASP.NET MVC 6 Injection of Control (IoC) container.
        /// </summary>
        /// <param name="services">The services collection or IoC container.</param>
        private static void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddSingleton<IKeyValueDB>(keyValueDb => new KeyValueDB(new OnDiskFileCollection("data")));
            services.AddScoped<IRobotsService, RobotsService>();
            services.AddScoped<ISitemapService, SitemapService>();
            services.AddScoped<IMapStore, MapStore>();
            services.AddSingleton<IAllocator, BufferedAllocator>();

            // Add your own custom services here e.g.

            // Singleton - Only one instance is ever created and returned.
            // services.AddSingleton<IExampleService, ExampleService>();

            // Scoped - A new instance is created and returned for each request/response cycle.
            // services.AddScoped<IExampleService, ExampleService>();

            // Transient - A new instance is created and returned each time.
            // services.AddTransient<IExampleService, ExampleService>();
        }
    }
}
