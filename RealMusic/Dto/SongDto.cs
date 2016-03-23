using System.Collections.Generic;
using RealMusic.Infrastructure;

namespace RealMusic.Dto
{
    public class SongDto : ErrorInfo
    {
        public ulong ItemId { get; set; }
        public string Name { get; set; }
        public string Musician { get; set; }
        public IList<string> Singers { get; set; }
        public string Duration { get; set; }
    }

    public class SongListDto
    {
        public IList<SongDto> Songs { get; set; }
    }

    public class SongInfoDto
    {
        public ulong ItemId { get; set; }
        public string Name { get; set; }
    }
}
