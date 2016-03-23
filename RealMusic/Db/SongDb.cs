using System.Collections.Generic;
using BTDB.ODBLayer;

namespace RealMusic.Db
{
    [StoredInline]
    public class Song
    {
        public string Name { get; set; }
        public string Musician { get; set; }
        public IList<string> Singers { get; set; }
        public string Duration { get; set; }
    }

    public class SongMap
    {
        public IOrderedDictionary<ulong, Category> Items { get; set; }
    }
}
