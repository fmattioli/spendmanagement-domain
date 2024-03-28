using Domain.Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Serilog;
using System.Linq.Expressions;

namespace Data.Persistence.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> collection;
        private readonly ILogger _logger;
        private static readonly object registrationLock = new();

        public BaseRepository(IMongoDatabase mongoDb, string collectionName, ILogger logger)
        {
            MapClasses();
            this.collection = mongoDb.GetCollection<TEntity>(collectionName);
            _logger = logger;
        }

        public async Task AddOneAsync(TEntity entity)
        {
            await this.collection.InsertOneAsync(entity);
            _logger.Information("Document createed with successfully {@entity}", entity);
        }

        public async Task ReplaceOneAsync(Expression<Func<TEntity, bool>> filterExpression, TEntity entity)
        {
            await this.collection.ReplaceOneAsync(filterExpression, entity);
            _logger.Information("Document updated with successfully {@entity}", entity);
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            await this.collection.DeleteOneAsync(filterExpression);

            var body = filterExpression.Body.ToString();
            _logger.Information($"Category deleted with sucessfully on database {body}");
        }

        private static void MapClasses()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
            {
                lock (registrationLock)
                {
                    if (!BsonClassMap.IsClassMapRegistered(typeof(TEntity)))
                    {
                        BsonClassMap.RegisterClassMap<TEntity>(cm =>
                        {
                            cm.AutoMap();
                            cm.SetIgnoreExtraElements(true);
                        });
                    }
                }
            }
        }
    }
}
