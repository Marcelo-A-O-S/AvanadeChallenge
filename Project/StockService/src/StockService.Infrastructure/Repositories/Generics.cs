using Microsoft.EntityFrameworkCore;
using StockService.Domain.Interfaces;
using StockService.Infrastructure.Context;
using System.Linq.Expressions;
namespace StockService.Infrastructure.Repositories
{
    public class Generics<T> : IGenerics<T> where T : class
    {
        private readonly DBContext context;
        public Generics(DBContext _context)
        {
            this.context = _context;
        }
        public async Task Delete(T entity)
        {
            this.context.Set<T>().Remove(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return await this.context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> GetById(long Id)
        {
            return await this.context.Set<T>().FindAsync(Id);
        }

        public async Task<List<T>> List()
        {
            return await this.context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> List(int? page)
        {
            var query = this.context.Set<T>().AsQueryable();
            var itemsPage = 10;
            if (page != null)
            {
                query = query.Skip(((int)page) * 10).Take(itemsPage);
            }
            return await query.ToListAsync();
        }

        public async Task Save(T entity)
        {
            await this.context.Set<T>().AddAsync(entity);
            await this.context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            this.context.Set<T>().Update(entity);
            await this.context.SaveChangesAsync();
        }
    }
}