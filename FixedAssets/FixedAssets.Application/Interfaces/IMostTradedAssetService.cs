using FixedAssets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Application.Interfaces
{
    public interface IMostTradedAssetService
    {
        Task<List<MostTradedAsset>> GetTop5MostTradedAssetsAsync();
        Task UpdateMostTradedAssetAsync(MostTradedAsset asset);
        Task AddMostTradedAssetAsync(MostTradedAsset asset);
    }
}
