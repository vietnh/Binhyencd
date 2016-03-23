using System.Linq;
using Microsoft.AspNet.Mvc;
using RealMusic.ApiControllers;
using RealMusic.Db;
using RealMusic.Dto;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RealMusic.Controllers
{
    [Route("[controller]")]
    public class CategoryController : BaseController
    {
        readonly IAllocator _allocator;
        public CategoryController(IMapStore data, IAllocator allocator) : base(data)
        {
            _allocator = allocator;
        }

        [Route("[action]")]
        public IActionResult List()
        {
            var categoryListDto = GetCategories(Data);
            return View(categoryListDto);
        }

        [Route("[action]/{itemId}")]
        public IActionResult Edit(ulong itemId)
        {
            Category category;
            if (Data.TryGetCategory(itemId, out category))
            {
                
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Store([FromBody]CategoryDto categoryDto)
        {
            if (categoryDto.ItemId == 0)
            {
                var category = new Category
                {
                    Name = categoryDto.Name,
                    CategoryType = categoryDto.CategoryType
                };
                var itemId = _allocator.AllocateId();
                Data.StoreCategory(itemId, category);
            }
            else
            {
                Category category;
                if (!Data.TryGetCategory(categoryDto.ItemId, out category))
                    return HttpBadRequest();

                category.Name = categoryDto.Name;
                Data.StoreCategory(categoryDto.ItemId, category);
            }

            return Ok();
        }

        [HttpPost]
        [Route("[action]/{itemId}")]
        public IActionResult Delete(ulong itemId)
        {
            Category category;
            if (Data.TryGetCategory(itemId, out category))
            {
                Data.RemoveCategory(itemId);

                return Ok();
            }

            return HttpBadRequest();
        }

        public static CategoryListDto GetCategories(IMapStore data)
        {
            var categories = data.GetCategories()
                .OrderBy(c => c.Value.Name)
                .Select(c => new CategoryDto { ItemId = c.Key, Name = c.Value.Name, CategoryType = c.Value.CategoryType })
                .ToList();

            return new CategoryListDto
            {
                Categories = categories
            };
        }
    }
}
