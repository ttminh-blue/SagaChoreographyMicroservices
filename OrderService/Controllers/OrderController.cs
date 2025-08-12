using Microsoft.AspNetCore.Mvc;
using OrderService.Models.Dtos;
using OrderService.Services.Interfaces;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null || request.OrderItems == null || !request.OrderItems.Any())
                return BadRequest("Order items are required.");

            var requestCreated = await _orderService.CreateOrder(request);
            return Ok(requestCreated);
        }

        [HttpGet("GetAllOrders")]
        public async Task<List<CreateOrderRequest>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return orders;
        }

        [HttpGet("GetOrder/{orderID}")]
        public async Task<CreateOrderRequest> GetOrder(Guid orderID)
        {
            var order = await _orderService.GetOrder(orderID);
            return order;
        }

    }
}
