using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Documents.Queries;

public record GetFullDocumentTreeQuery 
    : IRequest<List<DocumentTreeOwnerTypeDto>>;
