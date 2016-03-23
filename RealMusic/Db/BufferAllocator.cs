using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RealMusic.Db
{
    public interface IAllocator
    {
        ulong AllocateId();
    }


    public class BufferedAllocator : IAllocator
    {
        long _lastUsed;

        public BufferedAllocator(IKeyValueDB kvDb)
        {
            var db = new ObjectDB();
            db.Open(kvDb, false);
            var tr = db.StartTransaction();

            //var index = 0;
            //foreach (var key in categories.Keys)
            //{
            //    tr.Singleton<CategoryMap>().Items[key] = defaultCategories[index];
            //    index++;
            //}

            //tr.Singleton<OrderMap>().Items.Clear();
            //tr.Singleton<OrderItemMap>().Items.Clear();
            //tr.Singleton<CategoryMap>().Items.Clear();
            //tr.Commit();

            var categories = tr.Singleton<CategoryMap>();
            var test = tr.Singleton<CategoryMap>().Items.Any();

            var maxCategory = tr.Singleton<CategoryMap>().Items.Any() ? tr.Singleton<CategoryMap>().Items.Max(i => i.Key) : 0;
            var maxAlbum = tr.Singleton<AlbumMap>().Items.Any() ? tr.Singleton<AlbumMap>().Items.Max(i => i.Key) : 0;
            var maxOrder = tr.Singleton<OrderMap>().Items.Any() ? tr.Singleton<OrderMap>().Items.Max(i => i.Key) : 0;
            var maxOrderItem = tr.Singleton<OrderItemMap>().Items.Any() ? tr.Singleton<OrderItemMap>().Items.Max(i => i.Key) : 0;
            _lastUsed = (long)(new List<ulong> {maxCategory, maxAlbum, maxOrder, maxOrderItem}).Max();
        }

        public ulong AllocateId()
        {
            long oldValue, newValue;
            do
            {
                oldValue = _lastUsed;
                newValue = oldValue + 1;
            } while (Interlocked.CompareExchange(ref _lastUsed, newValue, oldValue) != oldValue);
            return (ulong)newValue;
        }
    }
}