using System.Linq.Expressions;
namespace SaleService.Application.Interfaces
{
    public interface IServices<T> where T : class
    {
        Task Save(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<List<T>> FindAllBy(Expression<Func<T, bool>> predicate);
        Task<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<T> GetById(int Id);
        Task<List<T>> List();
        Task<List<T>> List(int page);
    }
}