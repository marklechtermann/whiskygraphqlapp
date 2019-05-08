using System.Collections.Generic;
using System.Threading.Tasks;
using WhiskyApp.DataAccess.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace WhiskyApp.DataAccess
{
    public class WhiskyRepository : IWhiskyRepository
    {
        private readonly ILogger logger;

        private readonly WhiskyDbContext context;

        public WhiskyRepository(WhiskyDbContext context, ILogger<WhiskyRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<WhiskyEntity> AddAsync(WhiskyEntity entity)
        {
            var entityEntry = await this.context.Whiskys.AddAsync(entity);
            return entityEntry.Entity;
        }

        public async Task<int> CountAsync()
        {
            logger.LogWarning(nameof(CountAsync));
            return await this.context.Whiskys.CountAsync();
        }

        public async Task DeleteAsync(string id)
        {
            logger.LogWarning(nameof(DeleteAsync));

            var whisky = await this.context.Whiskys.FirstOrDefaultAsync(w => w.Id == id);
            if (whisky != null)
            {
                this.context.Whiskys.Remove(whisky);
            }
        }

        public async Task<IEnumerable<WhiskyEntity>> GetAllAsync()
        {
            logger.LogWarning(nameof(GetAllAsync));
            return await this.context.Whiskys.ToListAsync();
        }

        public async Task<IEnumerable<WhiskyEntity>> GetAllAsync(int skip, int take)
        {
            logger.LogWarning($"{nameof(GetAllByDestilleryIdAsync)} {skip} {take}");
            return await this.context.Whiskys.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<WhiskyEntity>> GetAllByDestilleryIdAsync(string id)
        {
            logger.LogWarning($"{nameof(GetAllByDestilleryIdAsync)} {id}");
            return await this.context.Whiskys.Where(d => d.DestilleryEntity.Id == id).ToListAsync();
        }

        public async Task<WhiskyEntity> GetByIdAsync(string id)
        {
            logger.LogWarning($"{nameof(GetByIdAsync)} {id}");
            return await this.context.Whiskys.FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}