using MongoDB.Driver;

namespace SparkTest.DAL.Interfaces.DataContext
{
    public interface IDataContext
    {
        IMongoCollection<TEntity> Get<TEntity>() where TEntity : class;
    }
}
