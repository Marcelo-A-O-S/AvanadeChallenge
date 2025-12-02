using System.Linq.Expressions;
namespace AuthService.Domain.Interfaces
{
    public interface IGenerics<T> where T : class
    {
        Task Save(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<T> GetById(long Id);
        Task<List<T>> List();
        Task<List<T>> List(int page = 1, int itemsPage = 10);
    }
}