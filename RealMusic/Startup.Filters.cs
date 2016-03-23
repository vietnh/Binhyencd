namespace RealMusic
{
    using System.Collections.Generic;
    using Boilerplate.Web.Mvc.Filters;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Mvc.Filters;
    using Microsoft.AspNet.Hosting;
    using Microsoft.AspNet.Routing;
    using RealMusic.Constants;

    public partial class Startup
    {
        /// <summary>
        /// Adds filters which help improve search engine optimization (SEO).
        /// </summary>
        private static void ConfigureSearchEngineOptimizationFilters(ICollection<IFilterMetadata> filters, RouteOptions routeOptions)
        {
            filters.Add(new RedirectToCanonicalUrlAttribute(
                 appendTrailingSlash: routeOptions.AppendTrailingSlash,
                 lowercaseUrls: routeOptions.LowercaseUrls));
        }

        /// <summary>
        /// Adds filters which help improve security.
        /// </summary>
        /// <param name="environment">The environment the application is running under. This can be Development, 
        /// Staging or Production by default.</param>
        private static void ConfigureSecurityFilters(IHostingEnvironment environment, ICollection<IFilterMetadata> filters)
        {
        }
    }
}
