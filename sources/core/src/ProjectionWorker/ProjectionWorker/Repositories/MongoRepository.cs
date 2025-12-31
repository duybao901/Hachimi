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

    // --------------------
    // READ (Query side)
    // --------------------

    public IQueryable<TDocument> AsQueryable(Expression<Func<TDocument, bool>>? filterExpression = null)
        => filterExpression is null
            ? _collection.AsQueryable()
            : _collection.AsQueryable().Where(filterExpression);

    public async Task<TDocument?> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        => await _collection
            .Find(filterExpression)
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyList<TDocument>> FindAllAsync(Expression<Func<TDocument, bool>>? filterExpression = null)
        => filterExpression is null
            ? await _collection.Find(FilterDefinition<TDocument>.Empty).ToListAsync()
            : await _collection.Find(filterExpression).ToListAsync();

    // --------------------
    // WRITE (Projection-safe)
    // --------------------

    public async Task InsertOneAsync(TDocument document)
        => await _collection.InsertOneAsync(document);

    public async Task UpdateOneAsync(
        Expression<Func<TDocument, bool>> filterExpression,
        UpdateDefinition<TDocument> updateDefinition,
        bool isUpsert = false)
    {
        await _collection.UpdateOneAsync(
            filterExpression,
            updateDefinition,
            new UpdateOptions { IsUpsert = isUpsert });
    }

    public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        => await _collection.DeleteOneAsync(filterExpression);

    public Task<IEnumerable<TDocument>> FindAll(Expression<Func<TDocument, bool>> filterExpression)
    {
        throw new NotImplementedException();
    }
}
