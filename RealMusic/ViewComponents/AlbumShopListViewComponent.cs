using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;
using RealMusic.Dto;

namespace RealMusic.ViewComponents
{
    public class AlbumShopListViewComponent : BaseViewComponent
    {
        const int RecordsPerPage = 15;
        public AlbumShopListViewComponent(IMapStore Data) : base(Data)
        {
        }

        public IViewComponentResult Invoke(ulong categoryId, int page)
        {
            var albumList = GetAlbums(categoryId, page);

            return View(albumList);
        }

        public async Task<IViewComponentResult> InvokeAsync(ulong categoryId, int page)
        {
            var albumList = await Task.FromResult(GetAlbums(categoryId, page));
            return View(albumList);
        }

        AlbumInfoListDto GetAlbums(ulong categoryId, int page)
        {
            var selectedAlbums = Data.GetAlbums()
                .Where(a => a.Value.CategoryIds.Contains(categoryId))
                .OrderByDescending(a => a.Key)
                .Skip(RecordsPerPage * (page - 1))
                .Take(RecordsPerPage)
                .Select(CreateAlbumInfoDto).ToList();

            return new AlbumInfoListDto
            {
                Albums = selectedAlbums,
                SelectedCategoryId = categoryId,
                SelectedPage = page
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

        IList<int> GetPages(int total, int selectedPage)
        {
            var pages = new List<int>();
            var numberOfPages = (int)Math.Ceiling((double)total / RecordsPerPage);
            if (numberOfPages == 0)
                pages.Add(1);
            else if (numberOfPages <= 5)
                for (var i = 1; i <= numberOfPages; i++)
                    pages.Add(i);
            else
            {
                var space = selectedPage%4;
                for (var i = selectedPage - space; i < selectedPage - space + 4; i++)
                    pages.Add(i);
            }

            return pages;
        }
    }
}
