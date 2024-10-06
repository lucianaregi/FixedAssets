using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FixedAssets.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("order")]
        public async Task<IActionResult> ProcessOrder([FromBody] OrderDto order)
        {
            if (order == null || order.Quantity <= 0)
                return BadRequest("Dados inválidos.");

            var result = await _orderService.ProcessOrder(order);

            if (!result)
                return BadRequest("Erro ao processar a compra.");

            return Ok("Compra realizada com sucesso.");
        }
    }

}
