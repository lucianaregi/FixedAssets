using Microsoft.AspNetCore.Mvc;
using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;
using FixedAssets.Application.Services;

namespace FixedAssets.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IToroAccountService _toroAccountService;

        public UserController(IUserService userService, IProductService productService, IOrderService orderService, IToroAccountService toroAccountService)
        {
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
            _toroAccountService = toroAccountService;
        }

        /// <summary>
        /// Autentica um usuário com base no e-mail e senha fornecidos.
        /// </summary>
        /// <param name="loginRequest">Dados de autenticação.</param>
        /// <returns>Detalhes do usuário ou erro de autenticação.</returns>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Autentica um usuário", Description = "Autentica um usuário com base no e-mail e senha fornecidos.")]
        [SwaggerResponse(200, "Login bem-sucedido.", typeof(UserDto))]
        [SwaggerResponse(401, "Falha na autenticação.")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _userService.LoginAsync(loginRequest.Email, loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

            user.PasswordHash = null; // Remover o hash de senha da resposta

            return Ok(user);
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
        /// Obtém o saldo da conta Toro de um usuário específico.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Saldo da conta Toro do usuário</returns>
        [HttpGet("user/{id}/balance")]
        [SwaggerOperation(Summary = "Obtém o saldo da conta Toro de um usuário", Description = "Retorna o saldo da conta Toro para o usuário fornecido.")]
        [SwaggerResponse(200, "Saldo do usuário retornado com sucesso.", typeof(decimal))]
        [SwaggerResponse(404, "Usuário ou conta Toro não encontrado.")]
        [SwaggerResponse(500, "Erro interno no servidor.")]
        public async Task<IActionResult> GetUserBalance(int id)
        {
            try
            {
                // Busca o usuário pelo ID
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                // Busca a conta Toro associada ao usuário
                var toroAccount = await _toroAccountService.GetAccountByUserIdAsync(id);
                if (toroAccount == null)
                {
                    return NotFound("Conta Toro não encontrada.");
                }

                // Retorna o nome do usuário e o saldo da conta Toro
                return Ok(new
                {
                    name = user.Name,
                    balance = toroAccount.Balance
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
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
