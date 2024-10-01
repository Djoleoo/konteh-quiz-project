using System.Linq.Expressions;

namespace Konteh.Infrastructure.Repositories;
public interface IRepository<T>
{
    Task<List<T>> GetAll();
    Task<T?> GetById(int id);
    void Add(T entity);
    void Delete(T entity);
    Task SaveChanges();
    IQueryable<T> Search(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetPaged(int page, int pagesize);
    IEnumerable<T> GetByIds(List<int> ids);
}
