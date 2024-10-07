using Microsoft.AspNetCore.Mvc;
using FixedAssets.Application.Interfaces;
using System.Threading.Tasks;
using FixedAssets.Application.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace FixedAssets.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Obtém todos os produtos disponíveis.
        /// </summary>
        /// <returns>Uma lista de produtos.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Obter todos os produtos", Description = "Retorna a lista de produtos de renda fixa disponíveis.")]
        [SwaggerResponse(200, "Lista de produtos retornada com sucesso", typeof(List<ProductDto>))]
        [SwaggerResponse(500, "Erro interno no servidor")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Obtém os detalhes de um produto específico pelo ID.
        /// </summary>
        /// <param name="id">ID do produto.</param>
        /// <returns>Os detalhes do produto.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter detalhes do produto", Description = "Retorna os detalhes de um produto específico.")]
        [SwaggerResponse(200, "Produto encontrado", typeof(ProductDto))]
        [SwaggerResponse(404, "Produto não encontrado")]
        [SwaggerResponse(500, "Erro interno no servidor")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound("Produto não encontrado.");
            return Ok(product);
        }
    }
}
