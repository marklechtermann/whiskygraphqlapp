using System.Collections.Generic;
using System.Threading.Tasks;
using WhiskyApp.DataAccess;
using WhiskyApp.DataAccess.Models;

namespace WhiskyApp.DataAccess
{
    public interface IDestilleryRepository : IRepository<DestilleryEntity>
    {

        Task<DestilleryEntity> GetByWhiskyIdAsync(string id);

        Task<IDictionary<string, DestilleryEntity>> GetByWhiskyIdsAsync(IEnumerable<string> whiskyIds);
    }
}