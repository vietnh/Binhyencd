using System;
using System.Collections.Generic;
using RealMusic.Db;
using RealMusic.Infrastructure;

namespace RealMusic.Dto
{
    public class AlbumDto : ErrorInfo
    {
        public ulong ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string SingerOrActors { get; set; }
        public CategoryType CategoryType { get; set; }
        public DiscType DiscType { get; set; }
        public decimal? Price { get; set; }
        public string ImagePath { get; set; }
        public string BackImagePath { get; set; }
        public IList<CategoryInfoDto> Categories { get; set; }
        public IList<CheckBoxListItemDto> MusicCategories { get; set; }
        public IList<CheckBoxListItemDto> FilmCategories { get; set; }
        public IList<SelectListItemDto> DiscTypes { get; set; }
        public IList<SelectListItemDto> CategoryTypes { get; set; }
    }

    public class AlbumListDto
    {
        public IList<AlbumDto> Albums { get; set; }
    }

    public class AlbumInfoDto : ErrorInfo
    {
        public ulong ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DiscType DiscType { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public IList<CategoryInfoDto> Categories { get; set; }
    }

    public class AlbumInfoListDto
    {
        public IList<AlbumInfoDto> Albums { get; set; }
        public ulong SelectedCategoryId { get; set; }
        public int SelectedPage { get; set; }
    }

    public class AlbumTabDto
    {
        public AlbumInfoListDto New { get; set; }
        public AlbumInfoListDto BestSeller { get; set; }
    }
}
