using CommonModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Events;
using OrderService.Events.Interfaces;
using OrderService.Models;
using OrderService.Models.Context;
using OrderService.Models.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IPublisher _publisher;
        private OrderDb _db;
        public OrderController(IPublisher publisher, OrderDb db)
        {
            _publisher = publisher;
            _db = db;
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null || request.OrderItems == null || !request.OrderItems.Any())
                return BadRequest("Order items are required.");

            var orderId = Guid.NewGuid();

            double orderAmount = request.OrderItems.Sum(item => item.Units * item.UnitPrice);

            var order = new Orders
            {
                Id = orderId,
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                OrderAmount = orderAmount,
                OrderItems = request.OrderItems.Select(x => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    ProductId = x.ProductId,
                    UnitPrice = x.UnitPrice,
                    Units = x.Units
                }).ToList()
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            _publisher.Publish(new OrderEvent
            {
                CustomerId = request.CustomerId,
                OrderAmount = (int)orderAmount,
                OrderId = orderId
            });

            return Ok(order.Id);
        }
    }
}
