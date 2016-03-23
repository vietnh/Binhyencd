using System.Collections.Generic;

namespace RealMusic.Dto
{
    public class SearchDto
    {
        public string Query { get; set; }
        public SearchType SearchType { get; set; }
    }

    public class SearchResultDto
    {
        public IList<AlbumInfoDto> Items { get; set; }
        public int Page { get; set; }
    }

    public enum SearchType
    {
        Album,
        SingerOrActor,
        Director
    }
}
