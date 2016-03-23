using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using RealMusic.ApiControllers;
using RealMusic.Constants;
using RealMusic.Db;
using RealMusic.Dto;
using RealMusic.Utils;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RealMusic.Controllers
{
    [Route("[controller]")]
    public class OrderController : BaseController
    {
        readonly IAllocator _allocator;
        public OrderController(IMapStore data, IAllocator allocator) : base(data)
        {
            _allocator = allocator;
        }

        [HttpGet]
        [Route("[action]/{albumId}/{quantity}", Name = OrderControllerRoute.GetCreate)]
        public IActionResult Create(ulong albumId, int quantity)
        {
            if (albumId == 0 || quantity == 0)
                return RedirectToAction("Index", "Home");

            Album album;
            if (!Data.TryGetAlbum(albumId, out album))
                return RedirectToAction("Index", "Home");

            var orderItemDto = new OrderItemDto
            {
                AlbumId = albumId,
                AlbumName = album.Name,
                AlbumImagePath = album.ImagePath,
                Quantity = quantity,
                Price = album.Price
            };

            var orderDto = new OrderDto
            {
                Items = new List<OrderItemDto> { orderItemDto },
                TotalPrice = orderItemDto.Price * orderItemDto.Quantity,
                Status = OrderStatus.Requested
            };

            return View(orderDto);
        }

        [HttpGet]
        [Route("[action]", Name = OrderControllerRoute.GetCheckout)]
        public IActionResult Checkout()
        {
            var cartSession = HttpContext.Session.GetObjectFromJson<OrderDto>("Cart");
            if (cartSession == null || !cartSession.Items.Any())
                return RedirectToAction("Index", "Home");

            return View("Create", cartSession);
        }

        [HttpPost]
        public IActionResult Create(OrderDto orderDto)
        {
            // TODO: need validation

            // calculate total price should be in backend while saving
            var order = new Order
            {
                OrderItemIds = new List<ulong>(),
                OrderNumber = Data.CreateOrderNumber(),
                TotalPrice = orderDto.Items.Sum(o => o.Price * o.Quantity),
                Address = orderDto.Address,
                Phone = orderDto.Phone,
                Note = orderDto.Note,
                Status = OrderStatus.Requested,
                CreatedDate = DateTime.UtcNow
            };

            foreach (var orderItemDto in orderDto.Items)
            {
                Album album;
                if (!Data.TryGetAlbum(orderItemDto.AlbumId, out album))
                    continue;

                var orderItem = new OrderItem
                {
                    AlbumId = orderItemDto.AlbumId,
                    Price = orderItemDto.Price,
                    Quantity = orderItemDto.Quantity
                };

                var itemId = _allocator.AllocateId();
                Data.StoreOrderItem(itemId, orderItem);

                album.SoldNumber += orderItemDto.Quantity;
                Data.StoreAlbum(orderItemDto.AlbumId, album);

                order.OrderItemIds.Add(itemId);
            }

            var orderId = _allocator.AllocateId();
            Data.StoreOrder(orderId, order);

            EmailSender.CreateOrderEmail(order, Data);
            HttpContext.Session.SetObjectAsJson("Cart", null);
            return View("Success");
        }

        [HttpPost]
        [Route("[action]/{albumId}/{quantity}", Name = OrderControllerRoute.GetAddToCart)]
        public IActionResult AddToCart(ulong albumId, int quantity)
        {
            var cartSession = HttpContext.Session.GetObjectFromJson<OrderDto>("Cart");
            if (cartSession == null)
                cartSession = new OrderDto();

            Album album;
            if (!Data.TryGetAlbum(albumId, out album))
                return new BadRequestResult();

            var existedCartItem = cartSession.Items.SingleOrDefault(c => c.AlbumId == albumId);
            if (existedCartItem != null)
            {
                existedCartItem.Quantity += quantity;
                cartSession.TotalPrice += album.Price * quantity;
            }
            else
            {
                var orderItemDto = new OrderItemDto
                {
                    AlbumId = albumId,
                    AlbumName = album.Name,
                    AlbumImagePath = album.ImagePath,
                    Quantity = quantity,
                    Price = album.Price
                };

                cartSession.Items.Add(orderItemDto);
                cartSession.TotalPrice += album.Price * quantity;
            }

            HttpContext.Session.SetObjectAsJson("Cart", cartSession);
            return ViewComponent("MyCart", cartSession);
        }

        [HttpPost]
        [Route("[action]/{albumId}", Name = OrderControllerRoute.GetRemoveFromCart)]
        public IActionResult RemoveFromCart(ulong albumId)
        {
            var cartSession = HttpContext.Session.GetObjectFromJson<OrderDto>("Cart");
            if (cartSession == null)
                return new BadRequestResult();

            Album album;
            if (!Data.TryGetAlbum(albumId, out album))
                return new BadRequestResult();

            var removedCartItem = cartSession.Items.SingleOrDefault(c => c.AlbumId == albumId);
            if (removedCartItem == null)
                return new BadRequestResult();

            cartSession.Items.Remove(removedCartItem);
            cartSession.TotalPrice -= removedCartItem.Price*removedCartItem.Quantity;

            HttpContext.Session.SetObjectAsJson("Cart", cartSession);
            return ViewComponent("MyCart", cartSession);
        }
    }
}
