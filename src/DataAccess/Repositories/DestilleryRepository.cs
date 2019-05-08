using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WhiskyApp.DataAccess;
using WhiskyApp.DataAccess.Models;

namespace WhiskyApp.DataAccess
{
    public class DestilleryRepository : IDestilleryRepository
    {
        private readonly WhiskyDbContext context;
        private readonly ILogger<DestilleryRepository> logger;

        public DestilleryRepository(WhiskyDbContext context, ILogger<DestilleryRepository> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<int> CountAsync()
        {
            logger.LogWarning(nameof(CountAsync));
            return await this.context.Distilleries.CountAsync();
        }

        public async Task<IEnumerable<DestilleryEntity>> GetAllAsync()
        {
            logger.LogWarning(nameof(GetAllAsync));
            return await this.context.Distilleries.ToListAsync();
        }

        public async Task<IEnumerable<DestilleryEntity>> GetAllAsync(int skip, int take)
        {
            logger.LogWarning($"{nameof(GetAllAsync)} {skip} {take}");
            return await this.context.Distilleries.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<DestilleryEntity> GetByIdAsync(string id)
        {
            logger.LogWarning($"{nameof(GetByIdAsync)} {id}");
            return await this.context.Distilleries.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<DestilleryEntity> GetByWhiskyIdAsync(string id)
        {
            logger.LogWarning($"{nameof(GetByWhiskyIdAsync)} {id}");
            return await this.context.Distilleries.FirstOrDefaultAsync(d => d.Whiskys.Any(w => w.Id == id));
        }

        public async Task<IDictionary<string, DestilleryEntity>> GetByWhiskyIdsAsync(IEnumerable<string> whiskyIds)
        {
            logger.LogWarning($"{nameof(GetByWhiskyIdsAsync)} {string.Join(" ", whiskyIds)}");

            var whiskys = this.context.Whiskys.Include(x => x.DestilleryEntity).Where(w => whiskyIds.Contains(w.Id));
            return await whiskys.ToDictionaryAsync<WhiskyEntity, string, DestilleryEntity>(ks => ks.Id, es => es.DestilleryEntity);
        }
    }
}