using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Documents.DTOs;

namespace TPMS.Application.Features.Landlords.Queries;

public class GetLandlordDocumentsQuery : IRequest<List<DocumentDto>>
{
    public int LandlordId { get; set; }

    public GetLandlordDocumentsQuery(int landlordId)
    {
        LandlordId = landlordId;
    }
}