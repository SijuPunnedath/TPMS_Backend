using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Penaltyploicy.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Penaltyploicy.Handlers;

public class CreatePenaltyPolicyHandler : IRequestHandler<CreatePenaltyPolicyCommand, int>
{
    private readonly TPMSDBContext _db;
    public CreatePenaltyPolicyHandler(TPMSDBContext db) => _db = db;

    public async Task<int> Handle(CreatePenaltyPolicyCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Policy;

        var entity = new PenaltyPolicy
        {
            Name = dto.Name,
            Description = dto.Description,
            FixedAmount = dto.FixedAmount,
            PercentageOfRent = dto.PercentageOfRent,
            GracePeriodDays = dto.GracePeriodDays,
            IsActive = dto.IsActive
        };

        _db.PenaltyPolicies.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity.PenaltyPolicyID;
    } 
}