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

        public UserController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/order
        [HttpPost("order")]
        public async Task<IActionResult> ProcessOrder([FromBody] OrderDto order)
        {
            // Validação dos dados
            if (order == null || order.Quantity <= 0) return BadRequest("Dados inválidos.");

            // Processar a compra
            var result = await _userService.ProcessPurchaseAsync(order.UserId, order.ProductId, order.Quantity);
            if (!result) return BadRequest("Erro ao processar a compra.");

            return Ok("Compra realizada com sucesso.");
        }

        //[HttpGet("{id}/orders")]
        //public async Task<IActionResult> GetUserOrders(int id)
        //{
        //    var orders = await _orderService.GetOrdersByUserId(id);
        //    if (orders == null || !orders.Any()) return NotFound("Nenhuma compra encontrada.");

        //    return Ok(orders.Select(o => new {
        //        o.Product.Name,
        //        o.Quantity,
        //        TotalPrice = o.Quantity * o.Product.Price
        //    }));
        //}

    }
}
