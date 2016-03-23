using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;
using RealMusic.Utils;

namespace RealMusic.ViewComponents
{
    public class AlbumTabsViewComponent : BaseViewComponent
    {
        public AlbumTabsViewComponent(IMapStore Data) : base(Data)
        {
        }

        public IViewComponentResult Invoke(int max)
        {
            var albumTabs = GetAlbumTabs(max);

            return View(albumTabs);
        }

        public async Task<IViewComponentResult> InvokeAsync(int max)
        {
            var albumTabs = await Task.FromResult(GetAlbumTabs(max));
            return View(albumTabs);
        }

        AlbumTabDto GetAlbumTabs(int max)
        {
            var newAlbums = Data.GetAlbums().OrderByDescending(a => a.Key).Take(max);
            var bestSoldAlbums = Data.GetAlbums().OrderByDescending(a => a.Value.SoldNumber).Take(max);

            return new AlbumTabDto
            {
                New = new AlbumInfoListDto { Albums = newAlbums.Select(CreateAlbumInfoDto).ToList() },
                BestSeller = new AlbumInfoListDto { Albums = bestSoldAlbums.Select(CreateAlbumInfoDto).ToList() }
            };
        }

        AlbumInfoDto CreateAlbumInfoDto(KeyValuePair<ulong, Album> album)
        {
            return new AlbumInfoDto
            {
                ItemId = album.Key,
                Name = album.Value.Name,
                //ImagePath = FileHelper.GetRelativePath(album.Value.ImagePath),
                ImagePath = album.Value.ImagePath,
                Categories = GetCategories(album.Value.CategoryIds),
                DiscType = album.Value.DiscType,
                Price = album.Value.Price
            };
        }

        IList<CategoryInfoDto> GetCategories(IList<ulong> categoryIds)
        {
            var result = new List<CategoryInfoDto>();

            if (!categoryIds.Any()) return result;

            foreach (var categoryId in categoryIds)
            {
                Category category;
                if (Data.TryGetCategory(categoryId, out category))
                    result.Add(new CategoryInfoDto
                    {
                        ItemId = categoryId,
                        Name = category.Name
                    });
            }

            return result;
        }
    }
}
