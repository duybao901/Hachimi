using AuthorizationAPI.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationAPI.Behaviors;
public sealed class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;  // SQL-SERVER-STRATEGY-2
    private readonly ApplicationDbContext _applicationDbContext;   // SQL-SERVER-STRATEGY-1 !Break Clean Architecture Rule (Application ref to Persistence)
    private readonly ILogger<TRequest> _logger;

    public TransactionPipelineBehavior(IUnitOfWork unitOfWork, ApplicationDbContext applocationDbContext, ILogger<TRequest> logger)
    {
        _unitOfWork = unitOfWork;
        _applicationDbContext = applocationDbContext;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!IsCommand())
        {
            return await next();
        }

        #region ============== SQL-SERVER-STRATEGY-1 ==============
        // Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        // https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency
        var strategy = _applicationDbContext.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Starting transaction...");
            await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();
            {
                var response = await next();
                await _applicationDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("Transaction committed successfully.");
                return response;
            }
        });
        #endregion ============== SQL-SERVER-STRATEGY-1 ==============

        #region ============== SQL-SERVER-STRATEGY-2 ==============

        ////IMPORTANT: passing "TransactionScopeAsyncFlowOption.Enabled" to the TransactionScope constructor. This is necessary to be able to use it with async/await.
        //using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //{
        //    var response = await next();
        //    await _unitOfWork.SaveChangesAsync(cancellationToken);
        //    transaction.Complete();
        //    await _unitOfWork.DisposeAsync();
        //    return response;
        //}

        #endregion ============== SQL-SERVER-STRATEGY-2 ==============
    }

    private bool IsCommand()
        => typeof(TRequest).Name.EndsWith("Command");
}
