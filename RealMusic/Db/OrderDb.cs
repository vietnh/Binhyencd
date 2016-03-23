using System;
using System.Collections.Generic;
using BTDB.ODBLayer;

namespace RealMusic.Db
{
    [StoredInline]
    public class Order
    {
        public int OrderNumber { get; set; }
        public IList<ulong> OrderItemIds { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class OrderMap
    {
        public IOrderedDictionary<ulong, Order> Items { get; set; }
    }

    [StoredInline]
    public class OrderItem
    {
        public ulong AlbumId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderItemMap
    {
        public IOrderedDictionary<ulong, OrderItem> Items { get; set; }
    }

    public enum OrderStatus
    {
        Requested,
        Finished,
        Cancelled
    }
}
