using BTDB.ODBLayer;

namespace RealMusic.Db
{
    [StoredInline]
    public class Category
    {
        public string Name { get; set; }
        public CategoryType CategoryType { get; set; }
        public ulong ParentItemId { get; set; }
    }

    public class CategoryMap
    {
        public IOrderedDictionary<ulong, Category> Items { get; set; }
    }

    public enum CategoryType
    {
        Music,
        Film
    }
}
