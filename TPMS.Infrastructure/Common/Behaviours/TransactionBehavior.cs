using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;
using TPMS.Infrastructure.Persistence;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Infrastructure.Common.Behaviours
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly TPMSDBContext _db;

        public TransactionBehavior(TPMSDBContext db) => _db = db;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var strategy = _db.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
                var response = await next();
                await tx.CommitAsync(cancellationToken);
                return response;
            });
        }
    }
}
