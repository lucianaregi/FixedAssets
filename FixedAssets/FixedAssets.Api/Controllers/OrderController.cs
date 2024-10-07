using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using FixedAssets.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FixedAssets.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public OrderController(IOrderService orderService, IOrderItemService orderItemService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        [HttpPost("order")]
        public async Task<IActionResult> ProcessOrder([FromBody] OrderDto orderDto)
        {
            
            if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
                return BadRequest("Dados inválidos.");

            foreach (var item in orderDto.OrderItems)
            {
                if (item.Quantity <= 0)
                    return BadRequest($"Quantidade inválida para o produto {item.ProductId}.");
            }
           
            var result = await _orderService.ProcessOrder(orderDto);

            if (!result)
                return BadRequest("Erro ao processar a compra.");

            return Ok("Compra realizada com sucesso.");
        }


        [HttpGet("{orderId}/items")]
        public async Task<IActionResult> GetOrderItems(int orderId)
        {
            var items = await _orderItemService.GetOrderItemsByOrderId(orderId);
            if (items == null || !items.Any())
                return NotFound("Nenhum item encontrado para esse pedido.");

            return Ok(items);
        }

    }

}
