using System.Threading;
using System.Threading.Tasks;

namespace TPMS.Application.Features.DocumentSequences.Services;

public interface IDocumentNumberService
{
    Task<string> GenerateAsync(
        string moduleName,
        CancellationToken cancellationToken = default);
} 