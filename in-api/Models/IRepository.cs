using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_API.Models
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddEntity(T entity);
        Task<IEnumerable<T>> GetAllEntities();
        Task<T> GetEntity(int id);
        Task<T> EditEntity(T entity);
        Task DeleteEntity(T entity);
        void MarkEntitiesForDeletion(IEnumerable<T> entities);
        void MarkEntitiesForUpdate(IEnumerable<T> entities);
        Task SaveEntities();
    }
}
