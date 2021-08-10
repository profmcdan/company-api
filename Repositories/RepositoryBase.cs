using System;
using System.Linq;
using System.Linq.Expressions;
using CompanyEmployee.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployee.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly RepositoryContext _repository;

        public RepositoryBase(RepositoryContext repository)
        {
            _repository = repository;
        }

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? _repository.Set<T>().AsNoTracking() : _repository.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => !trackChanges
            ? _repository.Set<T>().Where(expression).AsNoTracking()
            : _repository.Set<T>().Where(expression);

        public void Create(T entity) => 
            _repository.Set<T>().Add(entity);

        public void Update(T entity) => 
            _repository.Set<T>().Update(entity);

        public void Delete(T entity) => 
            _repository.Set<T>().Remove(entity);
    }
}