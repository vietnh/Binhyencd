using System;
using System.Collections.Generic;
using System.Linq;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;

namespace RealMusic.Db
{
    public class MapStore : IMapStore
    {
        IKeyValueDB _kvDb;
        IObjectDB _db;
        IObjectDBTransaction _tr;
        readonly IAllocator _allocator;

        public MapStore(IAllocator allocator, IKeyValueDB kvDb)
        {
            _kvDb = kvDb;
            _db = new ObjectDB();
            _db.Open(_kvDb, false);
            _tr = _db.StartTransaction();
            _allocator = allocator;
        }

        public void Dispose()
        {
            _tr.Dispose();
            _db.Dispose();
        }

        public void RefreshTransaction()
        {
            _tr = _db.StartTransaction();
        }

        public void Commit()
        {
            _tr.Commit();
        }

        #region Category

        IOrderedDictionary<ulong, Category> _categories;

        IOrderedDictionary<ulong, Category> Categories
        {
            get { return _categories = _categories ?? _tr.Singleton<CategoryMap>().Items; }
        }


        public IEnumerable<KeyValuePair<ulong, Category>> GetCategories()
        {
            return Categories;
        }

        public bool TryGetCategory(ulong itemId, out Category category)
        {
            return Categories.TryGetValue(itemId, out category);
        }

        public void StoreCategory(ulong itemId, Category category)
        {
            if (itemId == 0)
                itemId = _allocator.AllocateId();

            using (var tr = _db.StartTransaction())
            {
                tr.Singleton<CategoryMap>().Items[itemId] = category;
                tr.Commit();
            }
        }

        public void RemoveCategory(ulong itemId)
        {
            Categories.Remove(itemId);
            _tr.Commit();
        }

        #endregion

        #region Album

        IOrderedDictionary<ulong, Album> _albums;

        IOrderedDictionary<ulong, Album> Albums
        {
            get { return _albums = _albums ?? _tr.Singleton<AlbumMap>().Items; }
        }

        public IEnumerable<KeyValuePair<ulong, Album>> GetAlbums()
        {
            return Albums;
        }

        public bool TryGetAlbum(ulong itemId, out Album album)
        {
            return Albums.TryGetValue(itemId, out album);
        }

        public void StoreAlbum(ulong itemId, Album album)
        {
            if (itemId == 0)
                itemId = _allocator.AllocateId();

            using (var tr = _db.StartTransaction())
            {
                tr.Singleton<AlbumMap>().Items[itemId] = album;
                tr.Commit();
            }
        }

        public void RemoveAlbum(ulong itemId)
        {
            Albums.Remove(itemId);
            _tr.Commit();
        }

        #endregion

        #region Order

        IOrderedDictionary<ulong, Order> _orders;

        IOrderedDictionary<ulong, Order> Orders
        {
            get { return _orders = _orders ?? _tr.Singleton<OrderMap>().Items; }
        }

        public IEnumerable<KeyValuePair<ulong, Order>> GetOrders()
        {
            return Orders;
        }

        public bool TryGetOrder(ulong itemId, out Order order)
        {
            return Orders.TryGetValue(itemId, out order);
        }

        public int CreateOrderNumber()
        {
            return Orders.Any() ? Orders.Max(o => o.Value.OrderNumber) + 1 : 1;
        }

        public void StoreOrder(ulong itemId, Order order)
        {
            if (itemId == 0)
                itemId = _allocator.AllocateId();

            using (var tr = _db.StartTransaction())
            {
                tr.Singleton<OrderMap>().Items[itemId] = order;
                tr.Commit();
            }
        }

        public void RemoveOrder(ulong itemId)
        {
            Orders.Remove(itemId);
            _tr.Commit();
        }

        #endregion

        #region OrderItem

        IOrderedDictionary<ulong, OrderItem> _orderItems;

        IOrderedDictionary<ulong, OrderItem> OrderItems
        {
            get { return _orderItems = _orderItems ?? _tr.Singleton<OrderItemMap>().Items; }
        }

        public IEnumerable<KeyValuePair<ulong, OrderItem>> GetOrderItems()
        {
            return OrderItems;
        }

        public bool TryGetOrderItem(ulong itemId, out OrderItem orderItem)
        {
            return OrderItems.TryGetValue(itemId, out orderItem);
        }

        public void StoreOrderItem(ulong itemId, OrderItem orderItem)
        {
            if (itemId == 0)
                itemId = _allocator.AllocateId();

            using (var tr = _db.StartTransaction())
            {
                tr.Singleton<OrderItemMap>().Items[itemId] = orderItem;
                tr.Commit();
            }
        }

        public void RemoveOrderItem(ulong itemId)
        {
            OrderItems.Remove(itemId);
            _tr.Commit();
        }

        #endregion
    }
}

