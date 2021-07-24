using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SparkTest.DAL.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }

        Task<List<T>> GetAll();

        Task<List<T>> Get(Expression<Func<T, bool>> predicate);

        Task<(List<T>, long)> Pagination(FilterDefinition<T> filter, string sort, int? skip, int? limit);

        Task<(List<T>, long)> Pagination(Expression<Func<T, bool>> predicate, string sort, int? skip, int? limit);

        bool Any(Expression<Func<T, bool>> predicate);

        Task<T> Find(Expression<Func<T, bool>> predicate);

        Task<T> GetById(string id);

        Task InsertAsync(T entity);

        Task InsertManyAsync(IEnumerable<T> entity);

        Task UpdateAsync(T entity);

        Task UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);

        Task DeleteById(string id);
    }
}
