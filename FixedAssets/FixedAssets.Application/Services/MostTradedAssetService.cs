using FixedAssets.Domain.Entities;
using FixedAssets.Application.Interfaces;
using FixedAssets.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Application.Services
{
    public class MostTradedAssetService : IMostTradedAssetService
    {
        private readonly IMostTradedAssetRepository _mostTradedAssetRepository;

        public MostTradedAssetService(IMostTradedAssetRepository mostTradedAssetRepository)
        {
            _mostTradedAssetRepository = mostTradedAssetRepository;
        }

        // Retorna os 5 ativos mais negociados
        public async Task<List<MostTradedAsset>> GetTop5MostTradedAssetsAsync()
        {
            return await _mostTradedAssetRepository.GetTop5MostTradedAssetsAsync();
        }

        // Atualiza um ativo já existente
        public async Task UpdateMostTradedAssetAsync(MostTradedAsset asset)
        {
            await _mostTradedAssetRepository.UpdateMostTradedAssetAsync(asset);
        }

        // Adiciona um novo ativo
        public async Task AddMostTradedAssetAsync(MostTradedAsset asset)
        {
            await _mostTradedAssetRepository.AddMostTradedAssetAsync(asset);
        }
    }
}
