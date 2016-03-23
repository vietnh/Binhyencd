using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;

namespace RealMusic.ViewComponents
{
    public class MenuCategoryViewComponent : BaseViewComponent
    {
        public MenuCategoryViewComponent(IMapStore Data) : base(Data)
        {
        }

        public IViewComponentResult Invoke()
        {
            var categoryList = GetCategories();

            return View(categoryList);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryList = await Task.FromResult(GetCategories());
            return View(categoryList);
        }

        MenuCategoryListDto GetCategories()
        {
            var categories = Data.GetCategories().ToList();

            return new MenuCategoryListDto
            {
                Musics = categories
                .Where(c => c.Value.CategoryType == CategoryType.Music).OrderBy(c => c.Value.ParentItemId)
                .ThenBy(c => c.Value.Name)
                .Select(c => new CategoryInfoDto { ItemId = c.Key, Name = c.Value.Name })
                .ToList(),
                Films = categories
                .Where(c => c.Value.CategoryType == CategoryType.Film).OrderBy(c => c.Value.ParentItemId)
                .ThenBy(c => c.Value.Name)
                .Select(c => new CategoryInfoDto { ItemId = c.Key, Name = c.Value.Name })
                .ToList()
            };
        }
    }
}

