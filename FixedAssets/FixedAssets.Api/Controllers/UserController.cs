using Microsoft.AspNetCore.Mvc;
using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using System.Threading.Tasks;

namespace FixedAssets.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public UserController(IUserService userService, IProductService productService, IOrderService orderService)
        {
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        
        [HttpGet("{id}/orders")]
        public async Task<IActionResult> GetUserOrders(int id)
        {
            var orders = await _orderService.GetOrdersByUserId(id);

            if (orders == null || !orders.Any())
                return NotFound("Nenhuma compra encontrada.");

            var orderDetails = orders.SelectMany(o => o.OrderItems.Select(oi => new
            {
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.Product.UnitPrice,
                TotalPrice = oi.Quantity * oi.Product.UnitPrice
            })).ToList();

            return Ok(orderDetails);
        }


    }
}
