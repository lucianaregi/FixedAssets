using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;
using FixedAssets.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Repositories
{
    public class MostTradedAssetRepository : IMostTradedAssetRepository
    {
        private readonly ApplicationDbContext _context;

        public MostTradedAssetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MostTradedAsset>> GetTop5MostTradedAssetsAsync()
        {
            return await _context.MostTradedAssets
                .OrderByDescending(a => a.TotalTrades)
                .Take(5)
                .ToListAsync();
        }

        public async Task UpdateMostTradedAssetAsync(MostTradedAsset asset)
        {
            _context.MostTradedAssets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task AddMostTradedAssetAsync(MostTradedAsset asset)
        {
            await _context.MostTradedAssets.AddAsync(asset);
            await _context.SaveChangesAsync();
        }
    }

}
