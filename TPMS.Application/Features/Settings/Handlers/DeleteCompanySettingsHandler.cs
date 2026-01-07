using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Settings.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Settings.Handlers;

public class DeleteCompanySettingsHandler : IRequestHandler<DeleteCompanySettingsCommand, bool>
{
    private readonly TPMSDBContext _db;
    public DeleteCompanySettingsHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(DeleteCompanySettingsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.CompanySettings.FindAsync(new object?[] { request.CompanyID }, cancellationToken);
        if (entity == null) return false;

        _db.CompanySettings.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }   
}