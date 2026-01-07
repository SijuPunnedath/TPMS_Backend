using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Documents.Commands
{
    public class SoftDeleteDocumentCommand : IRequest<bool>
    {
        public int DocumentID { get; set; }
    }
}
