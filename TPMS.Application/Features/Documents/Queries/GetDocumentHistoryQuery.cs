using Amazon.Runtime.Internal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries
{
    public class GetDocumentHistoryQuery : IRequest<List<DocumentHistoryDto>>
    {
        public string OwnerType { get; set; } = string.Empty;
        public int OwnerID { get; set; }
        public string? DocumentTypeName { get; set; }

    }
}
