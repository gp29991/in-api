using in_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_API.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly FinancialDataContext _context;

        public Repository(FinancialDataContext context)
        {
            _context = context;
        }

        public async Task<T> AddEntity(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllEntities()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntity(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> EditEntity(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteEntity(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public void MarkEntitiesForDeletion(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void MarkEntitiesForUpdate(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public async Task SaveEntities()
        {
            await _context.SaveChangesAsync();
        }
    }
}
