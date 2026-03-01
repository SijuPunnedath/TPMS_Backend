using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.DocumentSequences.DTOs;

namespace TPMS.Application.Features.DocumentSequences.Queries;

public class GetDocumentSequencesQuery : IRequest<List<DocumentSequenceDto>>
{
   
}
