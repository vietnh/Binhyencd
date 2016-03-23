using System;
using System.Collections.Generic;
using RealMusic.Db;

namespace RealMusic.Dto
{
    public class OrderDto
    {
        public OrderDto()
        {
            Items = new List<OrderItemDto>();
            Status = OrderStatus.Requested;
        }

        public ulong OrderId { get; set; }
        public int OrderNumber { get; set; }
        public IList<OrderItemDto> Items { get; set; } 
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class OrderItemDto
    {
        public ulong ItemId { get; set; }
        public ulong AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string AlbumImagePath { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
