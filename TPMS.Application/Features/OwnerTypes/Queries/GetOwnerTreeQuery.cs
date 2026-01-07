using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.OwnerTypes.DTOs;

namespace TPMS.Application.Features.OwnerTypes.Queries;

public class GetOwnerTreeQuery : IRequest<List<OwnerTreeDto>>
{
    
}