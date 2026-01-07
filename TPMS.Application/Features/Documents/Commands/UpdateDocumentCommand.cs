using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Documents.Commands
{
    public class UpdateDocumentCommand : IRequest<bool>
    {
        public int DocumentID { get; set; }
        public string? DocType { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
    }
}
