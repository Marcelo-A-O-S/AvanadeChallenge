using Microsoft.EntityFrameworkCore;
using SaleService.Domain.Interfaces;
using SaleService.Infrastructure.Context;
using System.Linq.Expressions;
namespace SaleService.Infrastructure.Repositories
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

        public async Task<List<T>> FindAllBy(Expression<Func<T, bool>> predicate)
        {
            return await this.context.Set<T>().Where(predicate).ToListAsync();
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

        public async Task<List<T>> List(int page = 1, int itemsPage = 10)
        {
            var query = this.context.Set<T>().AsQueryable();
            var items = await query.Skip((page - 1) * itemsPage)
            .Take(itemsPage)
            .ToListAsync();
            return items;
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