using System.Collections.Generic;
using System.Threading.Tasks;
using WhiskyApp.DataAccess.Models;

namespace WhiskyApp.DataAccess
{
    public interface IWhiskyRepository : IRepository<WhiskyEntity>
    {
        Task<IEnumerable<WhiskyEntity>> GetAllByDestilleryIdAsync(string id);

        Task<WhiskyEntity> AddAsync(WhiskyEntity entity);

        Task DeleteAsync(string id);
    }
}