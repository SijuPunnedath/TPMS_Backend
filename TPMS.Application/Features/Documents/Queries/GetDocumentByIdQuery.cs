using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries
{
    public class GetDocumentByIdQuery : IRequest<DocumentDto?>
    {
        public int DocumentID { get; set; }

        public GetDocumentByIdQuery(int documentId)
        {
            DocumentID = documentId;
        }
    }
}
