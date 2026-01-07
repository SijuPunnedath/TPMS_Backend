using System;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Disputes.Commands;
using TPMS.Domain.Entities;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;
using TPMS.Infrastructure.Services;

namespace TPMS.Application.Features.Disputes.Handlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Common.Models;
using TPMS.Application.Common.Interfaces;

public class AddDisputeAttachmentCommandHandler
    : IRequestHandler<AddDisputeAttachmentCommand, ApiResponse<int>>
{
    private readonly TPMSDBContext _context;
    private readonly ICurrentUserService _currentUser;

    public AddDisputeAttachmentCommandHandler(
        TPMSDBContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<int>> Handle(
        AddDisputeAttachmentCommand request,
        CancellationToken cancellationToken)
    {
        //  Validate Dispute
        var dispute = await _context.Disputes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DisputeId == request.DisputeId, cancellationToken);

        if (dispute == null)
            return ApiResponse<int>.Failure("Dispute not found");

        if (dispute.Status == DisputeStatus.Closed)
            return ApiResponse<int>.Failure("Cannot add attachment to a closed dispute");

        //  Validate Document
        var documentExists = await _context.Documents
            .AnyAsync(x => x.DocumentID == request.DocumentId, cancellationToken);

        if (!documentExists)
            return ApiResponse<int>.Failure("Document not found");

        // 3 Create Attachment
        var attachment = new DisputeAttachment
        {
            //DisputeAttachmentId = Guid.NewGuid(),
            DisputeId = request.DisputeId,
            DocumentId = request.DocumentId,
            FileName = request.FileName,
            UploadedAt = DateTime.UtcNow
        };

        _context.DisputeAttachments.Add(attachment);
        await _context.SaveChangesAsync(cancellationToken);

        // 4️⃣ Return AttachmentId
        return ApiResponse<int>.Success(
            attachment.DisputeAttachmentId,
            "Attachment added successfully");
    }
}
