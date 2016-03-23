using System.Collections.Generic;
using RealMusic.Db;
using RealMusic.Infrastructure;

namespace RealMusic.Dto
{
    public class CategoryDto : ErrorInfo
    {
        public ulong ItemId { get; set; }
        public string Name { get; set; }
        public CategoryType CategoryType { get; set; }
    }

    public class CategoryListDto
    {
        public IList<CategoryDto> Categories { get; set; }
    }

    public class CategoryInfoDto
    {
        public ulong ItemId { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
    }

    public class CategoryInfoListDto
    {
        public IList<CategoryInfoDto> Categories { get; set; }
        public ulong SelectedItemId { get; set; }
    }

    public class MenuCategoryListDto
    {
        public IList<CategoryInfoDto> Musics { get; set; }
        public IList<CategoryInfoDto> Films { get; set; }
    }
}
