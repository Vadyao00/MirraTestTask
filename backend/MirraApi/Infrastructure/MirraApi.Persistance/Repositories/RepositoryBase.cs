using System.Linq.Expressions;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MirraApi.Persistance.Repositories;

public class RepositoryBase<T>(AppDbContext dbContext) : IRepositoryBase<T> where T : class
{
    public void Create(T entity) => dbContext.Set<T>().Add(entity);

    public void Delete(T entity) => dbContext.Set<T>().Remove(entity);
    public void Attach(T entity) => dbContext.Set<T>().Attach(entity);

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
            dbContext.Set<T>().AsNoTracking() :
            dbContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges ?
            dbContext.Set<T>().Where(expression).AsNoTracking() :
            dbContext.Set<T>().Where(expression);

    public void Update(T entity) => dbContext.Set<T>().Update(entity);
}