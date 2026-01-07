using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Commands
{
    public class UploadDocumentCommand : IRequest<DocumentDto>
    {
        public UploadDocumentDto Document { get; }

        public UploadDocumentCommand(UploadDocumentDto document)
        {
            Document = document;
        }
    }
}
