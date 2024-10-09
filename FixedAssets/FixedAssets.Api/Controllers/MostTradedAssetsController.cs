using FixedAssets.Application.Interfaces;
using FixedAssets.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MostTradedAssetsController : ControllerBase
    {
        private readonly IMostTradedAssetService _mostTradedAssetService;

        public MostTradedAssetsController(IMostTradedAssetService mostTradedAssetService)
        {
            _mostTradedAssetService = mostTradedAssetService;
        }

        /// <summary>
        /// Retorna os 5 ativos mais negociados.
        /// </summary>
        /// <returns>Uma lista dos 5 ativos mais negociados.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Retorna os 5 ativos mais negociados", Description = "Obtém uma lista dos 5 ativos mais negociados, ordenados pelo número de transações.")]
        [SwaggerResponse(200, "Ativos mais negociados retornados com sucesso.", typeof(List<MostTradedAsset>))]
        [SwaggerResponse(500, "Erro interno no servidor.")]
        public async Task<IActionResult> GetTop5MostTradedAssets()
        {
            try
            {
                var assets = await _mostTradedAssetService.GetTop5MostTradedAssetsAsync();
                return Ok(assets);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um ativo existente.
        /// </summary>
        /// <param name="asset">Os detalhes do ativo a ser atualizado.</param>
        /// <returns>Resultado da atualização.</returns>
        [HttpPut]
        [SwaggerOperation(Summary = "Atualiza um ativo", Description = "Atualiza as informações de um ativo mais negociado.")]
        [SwaggerResponse(200, "Ativo atualizado com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos fornecidos.")]
        [SwaggerResponse(500, "Erro interno no servidor.")]
        public async Task<IActionResult> UpdateMostTradedAsset([FromBody] MostTradedAsset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                await _mostTradedAssetService.UpdateMostTradedAssetAsync(asset);
                return Ok(new { message = "Ativo atualizado com sucesso." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Adiciona um novo ativo mais negociado.
        /// </summary>
        /// <param name="asset">Os detalhes do ativo a ser adicionado.</param>
        /// <returns>Resultado da operação.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Adiciona um novo ativo", Description = "Adiciona um novo ativo à lista de ativos mais negociados.")]
        [SwaggerResponse(201, "Ativo adicionado com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos fornecidos.")]
        [SwaggerResponse(500, "Erro interno no servidor.")]
        public async Task<IActionResult> AddMostTradedAsset([FromBody] MostTradedAsset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                await _mostTradedAssetService.AddMostTradedAssetAsync(asset);
                return StatusCode(201, "Ativo adicionado com sucesso.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }
    }
}
