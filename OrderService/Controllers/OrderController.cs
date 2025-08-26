using Microsoft.AspNetCore.Mvc;
using OrderService.Models.Dtos;
using OrderService.Repositories.IRepositories;
using OrderService.Services.Interfaces;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IElasticsearchOrderRepository _elasticsearchOrderRepository;
        public OrderController(IOrderService orderService, IElasticsearchOrderRepository elasticsearchOrderRepository)
        {
            _orderService = orderService;
            _elasticsearchOrderRepository = elasticsearchOrderRepository;
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

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Keyword is required");

            var results = await _elasticsearchOrderRepository.SearchOrdersAsync(keyword);

            return Ok(results);
        }
    }
}
