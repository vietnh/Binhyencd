using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;

namespace RealMusic.ViewComponents
{
    public class SidebarViewComponent : BaseViewComponent
    {
        public SidebarViewComponent(IMapStore Data) : base(Data)
        {
        }

        public IViewComponentResult Invoke(ulong itemId)
        {
            var categoryList = GetCategories(itemId);

            return View(categoryList);
        }

        public async Task<IViewComponentResult> InvokeAsync(ulong itemId)
        {
            var categoryList = await Task.FromResult(GetCategories(itemId));
            return View(categoryList);
        }

        CategoryInfoListDto GetCategories(ulong itemId)
        {
            var categories = Data.GetCategories()
                .OrderBy(c => c.Value.ParentItemId)
                .ThenBy(c => c.Value.Name)
                .Select(c => new CategoryInfoDto { ItemId = c.Key, Name = c.Value.Name })
                .ToList();

            return new CategoryInfoListDto
            {
                Categories = categories,
                SelectedItemId = itemId
            };
        }
    }
}
