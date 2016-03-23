namespace RealMusic.Controllers
{
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Boilerplate.Web.Mvc;
    using Boilerplate.Web.Mvc.Filters;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.OptionsModel;
    using RealMusic.Constants;
    using RealMusic.Services;
    using RealMusic.Settings;

    public class HomeController : Controller
    {
        #region Fields

        private readonly IOptions<AppSettings> appSettings;
        private readonly IRobotsService robotsService;
        private readonly ISitemapService sitemapService;

        #endregion

        #region Constructors

        public HomeController(
            IRobotsService robotsService,
            ISitemapService sitemapService,
            IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings;
            this.robotsService = robotsService;
            this.sitemapService = sitemapService;
        }

        #endregion

        [HttpGet("", Name = HomeControllerRoute.GetIndex)]
        public IActionResult Index()
        {
            return this.View(HomeControllerAction.Index);
        }

        /// <summary>
        /// Tells search engines (or robots) how to index your site. 
        /// The reason for dynamically generating this code is to enable generation of the full absolute sitemap URL
        /// and also to give you added flexibility in case you want to disallow search engines from certain paths. The 
        /// sitemap is cached for one day, adjust this time to whatever you require. See
        /// http://rehansaeed.com/dynamically-generating-robots-txt-using-asp-net-mvc/
        /// </summary>
        /// <returns>The robots text for the current site.</returns>
        [NoTrailingSlash]
        [ResponseCache(CacheProfileName = CacheProfileName.RobotsText)]
        [Route("robots.txt", Name = HomeControllerRoute.GetRobotsText)]
        public IActionResult RobotsText()
        {
            string content = this.robotsService.GetRobotsText();
            return this.Content(content, ContentType.Text, Encoding.UTF8);
        }

        /// <summary>
        /// Gets the sitemap XML for the current site. You can customize the contents of this XML from the 
        /// <see cref="SitemapService"/>. The sitemap is cached for one day, adjust this time to whatever you require.
        /// http://www.sitemaps.org/protocol.html
        /// </summary>
        /// <param name="index">The index of the sitemap to retrieve. <c>null</c> if you want to retrieve the root 
        /// sitemap file, which may be a sitemap index file.</param>
        /// <returns>The sitemap XML for the current site.</returns>
        [NoTrailingSlash]
        [Route("sitemap.xml", Name = HomeControllerRoute.GetSitemapXml)]
        public async Task<IActionResult> SitemapXml(int? index = null)
        {
            string content = await this.sitemapService.GetSitemapXml(index);

            if (content == null)
            {
                return this.HttpBadRequest("Sitemap index is out of range.");
            }

            return this.Content(content, ContentType.Xml, Encoding.UTF8);
        }
    }
}
