using Microsoft.AspNetCore.Mvc;
using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
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

        /// <summary>
        /// Obtém os detalhes de um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Detalhes do usuário.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtém os detalhes de um usuário", Description = "Retorna os detalhes do usuário com base no ID informado.")]
        [SwaggerResponse(200, "Detalhes do usuário retornados com sucesso.", typeof(UserDto))]
        [SwaggerResponse(404, "Usuário não encontrado.")]
        [SwaggerResponse(500, "Erro interno no servidor.")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Obtém os detalhes das ordens de compra de um usuário.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Detalhes das ordens de compra do usuário.</returns>
        [HttpGet("{id}/orders")]
        [SwaggerOperation(Summary = "Obtém as ordens de compra de um usuário", Description = "Retorna os detalhes das ordens de compra feitas pelo usuário com base no ID informado.")]
        [SwaggerResponse(200, "Detalhes das ordens de compra retornados com sucesso.")]
        [SwaggerResponse(404, "Nenhuma compra encontrada.")]
        [SwaggerResponse(500, "Erro interno no servidor.")]
        public async Task<IActionResult> GetUserOrders(int id)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(id);

            if (orders == null || !orders.Any())
                return NotFound("Nenhuma compra encontrada.");

            var orderDetails = orders.SelectMany(o => o.OrderItems.Select(oi => new
            {
                ProductName = oi.ProductName, 
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                TotalPrice = oi.Quantity * oi.UnitPrice
            })).ToList();


            return Ok(orderDetails);
        }
    }
}
