using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;

namespace RealMusic.ViewComponents
{
    public class MenuAlbumViewComponent : BaseViewComponent
    {
        public MenuAlbumViewComponent(IMapStore Data) : base(Data)
        {
        }

        public IViewComponentResult Invoke(int max)
        {
            var albums = GetAlbums(max);

            return View(albums);
        }

        public async Task<IViewComponentResult> InvokeAsync(int max)
        {
            var albums = await Task.FromResult(GetAlbums(max));
            return View(albums);
        }

        AlbumInfoListDto GetAlbums(int max)
        {
            var bestSoldAlbums = Data.GetAlbums().OrderByDescending(a => a.Value.SoldNumber).Take(max);

            return new AlbumInfoListDto
            {
                Albums = bestSoldAlbums.Select(CreateAlbumInfoDto).ToList()
            };
        }

        AlbumInfoDto CreateAlbumInfoDto(KeyValuePair<ulong, Album> album)
        {
            return new AlbumInfoDto
            {
                ItemId = album.Key,
                Name = album.Value.Name,
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
