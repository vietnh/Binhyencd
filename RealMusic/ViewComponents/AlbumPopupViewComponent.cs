using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;
using RealMusic.Utils;

namespace RealMusic.ViewComponents
{
    public class AlbumPopupViewComponent : BaseViewComponent
    {
        public AlbumPopupViewComponent(IMapStore Data) : base(Data)
        {
        }

        public IViewComponentResult Invoke(ulong itemId, Album album)
        {
            var albumDto = CreateAlbumDto(new KeyValuePair<ulong, Album>(itemId, album));
            return View(albumDto);
        }

        public async Task<IViewComponentResult> InvokeAsync(ulong itemId, Album album)
        {
            var albumDto = await Task.FromResult(CreateAlbumDto(new KeyValuePair<ulong, Album>(itemId, album)));
            return View(albumDto);
        }

        AlbumDto CreateAlbumDto(KeyValuePair<ulong, Album> album)
        {
            var discTypes = new List<SelectListItemDto>
            {
                new SelectListItemDto {Id = (int) DiscType.Cd, Name = "CD"},
                new SelectListItemDto {Id = (int) DiscType.Dvd, Name = "DVD"},
                new SelectListItemDto {Id = (int) DiscType.HdDvd, Name = "HD-DVD"},
                new SelectListItemDto {Id = (int) DiscType.Blueray, Name = "Blueray"},
                new SelectListItemDto {Id = (int) DiscType.Cassette, Name = "Băng cối"},
                new SelectListItemDto {Id = (int) DiscType.Vinyl, Name = "Đĩa than"}
            };

            return new AlbumDto
            {
                ItemId = album.Key,
                Name = album.Value.Name,
                Description = album.Value.Description,
                //ImagePath = FileHelper.GetRelativePath(album.Value.ImagePath),
                ImagePath = album.Value.ImagePath,
                //BackImagePath = FileHelper.GetRelativePath(album.Value.BackImagePath),
                BackImagePath = album.Value.BackImagePath,
                Categories = GetCategories(album.Value.CategoryIds),
                DiscType = album.Value.DiscType,
                Price = album.Value.Price,
                DiscTypes = discTypes,
                SingerOrActors = string.Join(", ", album.Value.SingerOrActors),
                Director = string.Join(", ", album.Value.Director)
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
