using Microsoft.AspNet.Mvc;
using RealMusic.ApiControllers;
using RealMusic.Constants;
using RealMusic.Db;
using RealMusic.Dto;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RealMusic.Controllers
{
    [Route("[controller]")]
    public class ShopController : BaseController
    {
        public ShopController(IMapStore data) : base(data)
        {
        }

        [Route("[action]/{id}/{page?}", Name = ShopControllerRoute.GetIndex)]
        public IActionResult Category(ulong id, int page = 1)
        {
            Category category;
            if (!Data.TryGetCategory(id, out category))
            {
                return RedirectToAction("Index", "Home");
            }

            var categoryInfoDto = new CategoryInfoDto
            {
                ItemId = id,
                Name = category.Name,
                Page = page
            };

            return View(categoryInfoDto);
        }
    }
}
