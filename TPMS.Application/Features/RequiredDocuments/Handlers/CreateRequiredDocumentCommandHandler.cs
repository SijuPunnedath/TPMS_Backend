using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.RequiredDocuments.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.RequiredDocuments.Handlers;

public class CreateRequiredDocumentCommandHandler 
    : IRequestHandler<CreateRequiredDocumentCommand, int>
{
    private readonly TPMSDBContext _context;

    public CreateRequiredDocumentCommandHandler(TPMSDBContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(
        CreateRequiredDocumentCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {

       
        // Prevent duplicate configuration
        var exists = await _context.RequiredDocuments
            .AnyAsync(x => x.OwnerTypeID == request.OwnerTypeID &&
                           x.DocumentTypeID == request.DocumentTypeID,
                cancellationToken);

        if (exists)
            
            throw new Exception("This required document configuration already exists.");
        var entity = new RequiredDocument
        {
            OwnerTypeID = request.OwnerTypeID,
            DocumentTypeID = request.DocumentTypeID,
            IsMandatory = request.IsMandatory,
            IsActive = true
        };

        _context.RequiredDocuments.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.RequiredDocumentID;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
