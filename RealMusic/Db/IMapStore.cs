using System;
using System.Collections.Generic;

namespace RealMusic.Db
{
    public interface IMapStore : IDisposable
    {
        void Commit();
        void RefreshTransaction();

        #region Category
        IEnumerable<KeyValuePair<ulong, Category>> GetCategories();

        bool TryGetCategory(ulong itemId, out Category category);

        void StoreCategory(ulong itemId, Category category);

        void RemoveCategory(ulong itemId);
        #endregion

        #region Album
        IEnumerable<KeyValuePair<ulong, Album>> GetAlbums();

        bool TryGetAlbum(ulong itemId, out Album album);

        void StoreAlbum(ulong itemId, Album album);

        void RemoveAlbum(ulong itemId);
        #endregion

        #region Order
        IEnumerable<KeyValuePair<ulong, Order>> GetOrders();

        bool TryGetOrder(ulong itemId, out Order order);

        int CreateOrderNumber();

        void StoreOrder(ulong itemId, Order order);

        void RemoveOrder(ulong itemId);
        #endregion

        #region OrderItem
        IEnumerable<KeyValuePair<ulong, OrderItem>> GetOrderItems();

        bool TryGetOrderItem(ulong itemId, out OrderItem orderItem);

        void StoreOrderItem(ulong itemId, OrderItem orderItem);

        void RemoveOrderItem(ulong itemId);
        #endregion
    }
}
