using System;
using System.Collections.Generic;
using BTDB.ODBLayer;
using RealMusic.Dto;

namespace RealMusic.Db
{
    [StoredInline]
    public class Album
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<string> Director { get; set; }
        public IList<string> SingerOrActors { get; set; }
        public CategoryType CategoryType { get; set; }
        public DiscType DiscType { get; set; }
        public string Duration { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public string BackImagePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SoldNumber { get; set; }
        public IList<ulong> CategoryIds { get; set; }
        public ulong RegionId { get; set; }
    }

    public class AlbumMap
    {
        public IOrderedDictionary<ulong, Album> Items { get; set; }
    }

    public enum DiscType
    {
        Cd,
        Dvd,
        HdDvd,
        Blueray,
        Vinyl,
        Cassette
    }

    public static class EnumExtensions
    {
        public static string GetStringValue(this DiscType discType)
        {
            switch (discType)
            {
                case DiscType.Cassette:
                    return "Băng cối";
                case DiscType.Cd:
                    return "CD";
                case DiscType.Dvd:
                    return "DVD";
                case DiscType.HdDvd:
                    return "HD-DVD";
                case DiscType.Blueray:
                    return "Blueray";
                case DiscType.Vinyl:
                    return "Đĩa than";
                default:
                    return "CD";
            };
        }
    }
}
