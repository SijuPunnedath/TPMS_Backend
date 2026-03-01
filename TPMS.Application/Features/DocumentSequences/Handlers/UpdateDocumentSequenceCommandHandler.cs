using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Common.Exceptions;
using TPMS.Application.Features.DocumentSequences.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.DocumentSequences.Handlers;


    public class UpdateDocumentSequenceCommandHandler 
        : IRequestHandler<UpdateDocumentSequenceCommand, Unit>
    {
        private readonly TPMSDBContext _context;

        public UpdateDocumentSequenceCommandHandler(TPMSDBContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(
            UpdateDocumentSequenceCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _context.DocumentSequences
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
                throw new NotFoundException(nameof(DocumentSequence), request.Id);

            entity.Prefix = request.Prefix.ToUpper();
            entity.NumberLength = request.NumberLength;
            entity.ResetEveryYear = request.ResetEveryYear;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
