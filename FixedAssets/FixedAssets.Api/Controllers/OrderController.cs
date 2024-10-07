using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// Processa uma nova ordem de compra.
        /// </summary>
        /// <param name="orderDto">Dados do pedido.</param>
        /// <returns>Resultado da transação.</returns>
        [HttpPost("order")]
        [SwaggerOperation(Summary = "Processa uma nova ordem de compra", Description = "Recebe os dados da ordem e realiza o processamento da compra.")]
        [SwaggerResponse(200, "Compra realizada com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos ou erro ao processar a compra.")]
        [SwaggerResponse(500, "Erro interno no servidor.")]
        public async Task<IActionResult> ProcessOrder([FromBody] OrderDto orderDto)
        {
            // Validações de entrada
            if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
                return BadRequest("Dados inválidos.");

            foreach (var item in orderDto.OrderItems)
            {
                if (item.Quantity <= 0)
                    return BadRequest($"Quantidade inválida para o produto {item.ProductId}.");
            }

            // Processa a ordem
            var result = await _orderService.ProcessOrderAsync(orderDto);
            if (!result)
                return BadRequest("Erro ao processar a compra.");

            return Ok("Compra realizada com sucesso.");
        }

        /// <summary>
        /// Busca os itens de uma ordem específica.
        /// </summary>
        /// <param name="orderId">ID da ordem.</param>
        /// <returns>Lista de itens da ordem.</returns>
        [HttpGet("{orderId}/items")]
        [SwaggerOperation(Summary = "Obtém os itens de uma ordem", Description = "Retorna a lista de itens de uma ordem específica.")]
        [SwaggerResponse(200, "Lista de itens retornada com sucesso.", typeof(OrderItemDto))]
        [SwaggerResponse(404, "Nenhum item encontrado para esse pedido.")]
        [SwaggerResponse(500, "Erro interno no servidor.")]
        public async Task<IActionResult> GetOrderItems(int orderId)
        {
            var items = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);
            if (items == null || !items.Any())
                return NotFound("Nenhum item encontrado para esse pedido.");

            return Ok(items);
        }
    }
}
