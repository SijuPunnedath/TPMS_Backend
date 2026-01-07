using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Commands
{
    public record CompleteDocumentUploadCommand(CompleteUploadDto UploadDto) : IRequest<string>;

}
