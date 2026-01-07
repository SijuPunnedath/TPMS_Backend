using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries
{
    public class GetDocumentAccessHistoryQuery : IRequest<List<DocumentAccessLogDto>>
    {
        public int? DocumentID { get; set; }
        public string? AccessedBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
