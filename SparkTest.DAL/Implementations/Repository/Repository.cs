using MongoDB.Bson;
using MongoDB.Driver;
using SparkTest.DAL.Domain.Interfaces;
using SparkTest.DAL.Interfaces.DataContext;
using SparkTest.DAL.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SparkTest.DAL.Implementations.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IEntity<string>
    {
        private IDataContext _context;
        private IMongoCollection<T> _entities;

        public Repository(IDataContext context)
        {
            _context = context;
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities.AsQueryable();
            }
        }

        public async Task DeleteById(string id)
        {
            try
            {
                await Entities.DeleteOneAsync(new BsonDocument(Constants.ID, new ObjectId(id)));
            }
            catch (Exception dbEx)
            {
                throw;
            }
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await Entities.Find(predicate).SingleAsync();
            }
            catch (Exception dbEx)
            {
                throw;
            }
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await Entities.Find(predicate).ToListAsync();
            }
            catch (Exception dbEx)
            {
                throw;
            }
        }

        public async Task<List<T>> GetAll()
        {
            try
            {
                return await Entities.Find(x => true).ToListAsync();
            }
            catch (Exception dbEx)
            {
                throw;
            }
        }

        public async Task<T> GetById(string id)
        {
            try
            {
                return await Entities.Find(new BsonDocument(Constants.ID, new ObjectId(id))).FirstOrDefaultAsync();
            }
            catch (Exception dbEx)
            {
                throw;
            }
        }

        public async Task Insert(T entity)
        {
            try
            {
                ThrowIfEntityIsNull(entity);
                await Entities.InsertOneAsync(entity);
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }

        public async Task Update(T entity)
        {
            try
            {
                ThrowIfEntityIsNull(entity);
                await Entities.ReplaceOneAsync(new BsonDocument(Constants.ID, new ObjectId(entity.Id)), entity);
            }
            catch (Exception dbEx)
            {
                throw;
            }
        }

        public async Task UpdateMany(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            try
            {
                await Entities.UpdateManyAsync(filter, update);
            }
            catch (Exception dbEx)
            {
                throw;
            }
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return Entities.Find(predicate).CountDocuments() > 0;
        }

        public async Task InsertMany(IEnumerable<T> entity)
        {
            try
            {
                if (!entity.Any())
                    throw new ArgumentNullException("entity");

                await Entities.InsertManyAsync(entity);
            }
            catch (Exception dbEx)
            {
                throw;
            }
        }

        public async Task<(List<T>, long)> Pagination(FilterDefinition<T> filter, string sort, int? skip = null, int? limit = null)
        {
            try
            {
                var result = Entities.Find(filter);

                var count = result.CountDocuments();

                return (await result.Skip(skip).Limit(limit).Sort(sort).ToListAsync(), count);
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }

        public async Task<(List<T>, long)> Pagination(Expression<Func<T, bool>> predicate, string sort, int? skip = null, int? limit = null)
        {
            try
            {
                var result = Entities.Find(predicate);

                var count = result.CountDocuments();

                return (await result.Skip(skip).Limit(limit).Sort(sort).ToListAsync(), count);
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }

        private IMongoCollection<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Get<T>(); ;
                }

                return _entities;
            }
        }

        private void ThrowIfEntityIsNull(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
        }
    }
}
