using MongoDB.Driver;
using ProjectionWorker.Abstractions;
using System.Linq.Expressions;

public interface IMongoRepository<TDocument>
    where TDocument : IDocument
{
    // READ
    IQueryable<TDocument> AsQueryable(Expression<Func<TDocument, bool>> filterExpression);

    Task<IEnumerable<TDocument>> FindAll(Expression<Func<TDocument, bool>> filterExpression);

    Task<TDocument?> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    // WRITE (Projection-safe)
    Task UpdateOneAsync(
        Expression<Func<TDocument, bool>> filterExpression,
        UpdateDefinition<TDocument> updateDefinition,
        bool isUpsert = false
    );

    Task InsertOneAsync(TDocument document);

    Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
}
