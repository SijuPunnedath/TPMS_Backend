using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries
{
    public class DownloadDocumentQuery : IRequest<DownloadDocumentDto>
    {
        public int? DocumentID { get; set; }
        public string? OwnerType { get; set; }
        public int? OwnerID { get; set; }
        public string? DocumentTypeName { get; set; }
        public string? Version { get; set; }
    }
}
