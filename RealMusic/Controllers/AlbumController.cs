using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Boilerplate.Web.Mvc.Filters;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using RealMusic.ApiControllers;
using RealMusic.Constants;
using RealMusic.Db;
using RealMusic.Dto;
using RealMusic.Utils;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RealMusic.Controllers
{
    [Route("[controller]")]
    public class AlbumController : BaseController
    {
        readonly IHostingEnvironment _environment;
        readonly IAllocator _allocator;
        const string ImageDirectory = "uploads";
        const int RecordsPerPage = 20;

        public AlbumController(IMapStore data, IHostingEnvironment environment, IAllocator allocator) : base(data)
        {
            _environment = environment;
            _allocator = allocator;
        }

        [Route("[action]")]
        public IActionResult List()
        {
            var albumInfoDtos = Data.GetAlbums().OrderByDescending(a => a.Key).Select(CreateAlbumInfoDto).ToList();
            return View(new AlbumInfoListDto { Albums = albumInfoDtos });
        }

        [NoTrailingSlash]
        [HttpGet]
        [Route("[action]/{itemId?}")]
        public IActionResult Create(ulong? itemId)
        {
            var musicCategories = Data.GetCategories().Where(c => c.Value.CategoryType == CategoryType.Music);
            var filmCategories = Data.GetCategories().Where(c => c.Value.CategoryType == CategoryType.Film);
            var discTypes = new List<SelectListItemDto>
            {
                new SelectListItemDto {Id = (int) DiscType.Cd, Name = "CD"},
                new SelectListItemDto {Id = (int) DiscType.Dvd, Name = "DVD"},
                new SelectListItemDto {Id = (int) DiscType.HdDvd, Name = "HD-DVD"},
                new SelectListItemDto {Id = (int) DiscType.Blueray, Name = "Blueray"},
                new SelectListItemDto {Id = (int) DiscType.Cassette, Name = "Băng cối"},
                new SelectListItemDto {Id = (int) DiscType.Vinyl, Name = "Đĩa than"}
            };
            var categoryTypes = new List<SelectListItemDto>
            {
                new SelectListItemDto {Id = (int) CategoryType.Music, Name = "Đĩa nhạc"},
                new SelectListItemDto {Id = (int) CategoryType.Film, Name = "Đĩa phim"}
            };

            var albumDto = new AlbumDto
            {
                MusicCategories = musicCategories.Select(c => new CheckBoxListItemDto { Id = c.Key, Name = c.Value.Name, IsChecked = false }).ToList(),
                FilmCategories = filmCategories.Select(c => new CheckBoxListItemDto { Id = c.Key, Name = c.Value.Name, IsChecked = false }).ToList(),
                DiscTypes = discTypes,
                CategoryTypes = categoryTypes
            };

            if (itemId.HasValue && itemId != 0)
            {
                Album album;
                if (Data.TryGetAlbum(itemId.Value, out album))
                {
                    albumDto.ItemId = itemId.Value;
                    albumDto.Name = album.Name;
                    albumDto.Description = album.Description;
                    albumDto.Price = album.Price;
                    albumDto.CategoryType = album.CategoryType;
                    albumDto.ImagePath = album.ImagePath;
                    albumDto.BackImagePath = album.BackImagePath;
                    albumDto.SingerOrActors = string.Join(", ", album.SingerOrActors);
                    albumDto.Director = string.Join(", ", album.Director);
                    albumDto.DiscType = album.DiscType;

                    albumDto.MusicCategories.Where(c => album.CategoryIds.Contains(c.Id)).ToList().ForEach(c => c.IsChecked = true);
                    albumDto.FilmCategories.Where(c => album.CategoryIds.Contains(c.Id)).ToList().ForEach(c => c.IsChecked = true);
                }
            }

            return View(albumDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AlbumDto albumDto, ICollection<IFormFile> files, ICollection<IFormFile> backFiles)
        {
            var filePath = await TrySaveFile(files);
            var backFilePath = await TrySaveFile(backFiles);

            var selectMusicCategoryIds = albumDto.MusicCategories.Where(c => c.IsChecked).Select(c => c.Id).ToList();
            var selectFilmCategoryIds = albumDto.FilmCategories.Where(c => c.IsChecked).Select(c => c.Id).ToList();

            Album album;
            if (albumDto.ItemId == 0 || !Data.TryGetAlbum(albumDto.ItemId, out album))
                album = new Album();

            album.Name = albumDto.Name;
            album.Description = albumDto.Description;
            album.CategoryType = albumDto.CategoryType;
            album.DiscType = albumDto.DiscType;
            album.Price = albumDto.Price ?? 0;
            album.SoldNumber = 0;
            album.CategoryIds = albumDto.CategoryType == CategoryType.Music ? selectMusicCategoryIds : selectFilmCategoryIds;
            album.CreatedDate = DateTime.UtcNow;
            album.Director = albumDto.Director?.Split(',').Select(d => d.Trim()).ToList() ?? new List<string>();
            album.SingerOrActors = albumDto.SingerOrActors?.Split(',').Select(d => d.Trim()).ToList() ?? new List<string>();

            if (!string.IsNullOrEmpty(filePath))
                album.ImagePath = filePath;
            if (!string.IsNullOrEmpty(backFilePath))
                album.BackImagePath = backFilePath;

            var itemId = albumDto.ItemId != 0 ? albumDto.ItemId : _allocator.AllocateId();
            Data.StoreAlbum(itemId, album);

            return RedirectToAction("List");
        }

        [Route("[action]/{id}")]
        public async Task<IActionResult> Details(ulong id)
        {
            Album album;
            if (Data.TryGetAlbum(id, out album))
            {
                return ViewComponent("AlbumPopup", id, album);
            }

            return HttpBadRequest();
        }

        [HttpGet]
        [Route("[action]/{searchType}/{query}/{page?}", Name = AlbumControllerRoute.GetSearch)]
        public IActionResult Search(SearchType searchType, string query, int page = 1)
        {
            var albums = new List<KeyValuePair<ulong, Album>>();
            var normalizedQuery = query.RemoveDiacriticsAndLowerCase();
            switch (searchType)
            {
                case SearchType.Album:
                    albums = Data.GetAlbums().Where(a => a.Value.Name.RemoveDiacriticsAndLowerCase().Contains(normalizedQuery)).Skip(RecordsPerPage * (page - 1)).Take(RecordsPerPage).ToList();
                    break;
                case SearchType.SingerOrActor:
                    albums = Data.GetAlbums().Where(a => string.Join(", ", a.Value.SingerOrActors).RemoveDiacriticsAndLowerCase().Contains(normalizedQuery)).Skip(RecordsPerPage * (page - 1)).Take(RecordsPerPage).ToList();
                    break;
                case SearchType.Director:
                    albums = Data.GetAlbums().Where(a => string.Join(", ", a.Value.Director).RemoveDiacriticsAndLowerCase().Contains(normalizedQuery)).Skip(RecordsPerPage * (page - 1)).Take(RecordsPerPage).ToList();
                    break;
            }

            var searchResultDto = new SearchResultDto
            {
                Items = albums.OrderByDescending(a => a.Value.CreatedDate).Select(CreateAlbumInfoDto).ToList(),
                Page = page
            };
            return View(searchResultDto);
        }

        async Task<string> TrySaveFile(ICollection<IFormFile> files)
        {
            if (!files.Any())
                return string.Empty;

            var file = files.First();
            if (file.Length <= 0)
                return string.Empty;

            var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var fileName = Guid.NewGuid() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var filePath = Path.Combine(uploadFolder, fileName);

            await file.SaveAsAsync(filePath);
            return "/" + ImageDirectory + "/" + fileName;
        }

        [HttpPost]
        [Route("[action]/{itemId}")]
        public IActionResult Delete(ulong itemId)
        {
            Album album;
            if (Data.TryGetAlbum(itemId, out album))
            {
                Data.RemoveAlbum(itemId);

                return Ok();
            }

            return HttpBadRequest();
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
