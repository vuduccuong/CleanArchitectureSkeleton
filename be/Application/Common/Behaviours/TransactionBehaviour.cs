using Application.Common.Interfaces;
using Common.Extentions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly IApplicationDbContext _dbContext;

        public TransactionBehaviour(ILogger<TransactionBehaviour<TRequest, TResponse>> logger, IApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeRequestName = request.GetGenericTypeName();
            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next().ConfigureAwait(false);
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await _dbContext.BeginTransactionAsync().ConfigureAwait(false))
                    using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                    {
                        _logger.LogInformation($"----Begin Transaction: {transaction.TransactionId} - {typeRequestName} request: {request}");

                        response = await next().ConfigureAwait(false);

                        await _dbContext.CommitAsync(transaction).ConfigureAwait(false);

                        _logger.LogInformation($"----Commited Transaction: {transaction.TransactionId}");

                        _ = transaction.TransactionId;
                    }
                }).ConfigureAwait(false);

                return response;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error Transaction", typeRequestName, request);
                throw;
            }
        }
    }
}
