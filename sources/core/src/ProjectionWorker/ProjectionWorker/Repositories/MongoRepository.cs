using MongoDB.Bson;
using MongoDB.Driver;
using ProjectionWorker.Abstractions;
using ProjectionWorker.Abstractions.Options;
using System.Linq.Expressions;

namespace ProjectionWorker.Repositories;

public class MongoRepository<TDocument> : IMongoRepository<TDocument>
    where TDocument : IDocument
{
    private readonly IMongoCollection<TDocument> _collection;

    public MongoRepository(IMongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _collection = database.GetCollection<TDocument>(
            GetCollectionName(typeof(TDocument)));
    }

    private static string GetCollectionName(Type documentType)
    {
        return documentType
                   .GetCustomAttributes(typeof(CollectionNameAttribute), true)
                   .FirstOrDefault() is CollectionNameAttribute attr
            ? attr.Name
            : throw new InvalidOperationException(
                $"Missing [CollectionName] attribute on {documentType.Name}");
    }

    public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
    }


    public async Task InsertOneAsync(TDocument document)
    {
        await _collection.InsertOneAsync(document);
    }

    public async Task UpdateOneAsync(
        Expression<Func<TDocument, bool>> filter,
        UpdateDefinition<TDocument> update,
        bool isUpsert = false
    )
    {
        await _collection.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions
            {
                IsUpsert = isUpsert
            }
        );
    }

    public async Task UpdateManyAsync(
        Expression<Func<TDocument, bool>> filter,
        UpdateDefinition<TDocument> update,
        ArrayFilterDefinition[]? arrayFilters = null
    )
    {
        var options = new UpdateOptions
        {
            ArrayFilters = arrayFilters
        };

        await _collection.UpdateManyAsync(
            filter,
            update,
            options
        );
    }

    public async Task DeleteOneAsync(
        Expression<Func<TDocument, bool>> filter
    )
    {
        await _collection.DeleteOneAsync(filter);
    }
}
