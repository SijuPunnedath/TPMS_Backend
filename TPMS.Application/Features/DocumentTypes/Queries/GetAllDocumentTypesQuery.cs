using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.DocumentTypes.DTOs;

namespace TPMS.Application.Features.DocumentTypes.Queries
{
    public record GetAllDocumentTypesQuery() : IRequest<List<DocumentTypeDto>>;

}
