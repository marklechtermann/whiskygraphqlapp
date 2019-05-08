using System.Collections.Generic;
using System.Threading.Tasks;

namespace WhiskyApp.DataAccess
{
    public interface IRepository<TEntity> where TEntity : IEntity, new()
    {
        Task<TEntity> GetByIdAsync(string id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllAsync(int skip, int take);

        Task<int> CountAsync();
    }
}