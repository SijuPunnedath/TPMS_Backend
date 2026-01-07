using MediatR;
using System.Collections.Generic;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries
{
    public class GetDocumentVersionHistoryQuery : IRequest<List<DocumentDto>>
    {
        public string OwnerType { get; set; } = string.Empty;
        public int OwnerID { get; set; }
        public string DocType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
