using MongoDB.Driver;
using ProjectionWorker.Abstractions;
using System.Linq.Expressions;

public interface IMongoRepository<TDocument>
    where TDocument : IDocument
{
    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    Task InsertOneAsync(TDocument document);

    Task UpdateOneAsync(
        Expression<Func<TDocument, bool>> filter,
        UpdateDefinition<TDocument> update,
        bool isUpsert = false
    );

    Task UpdateManyAsync(
        Expression<Func<TDocument, bool>> filter,
        UpdateDefinition<TDocument> update,
        ArrayFilterDefinition[]? arrayFilters = null
    );

    Task DeleteOneAsync(
        Expression<Func<TDocument, bool>> filter
    );
}
