using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Addresses.DTOs;

namespace TPMS.Application.Features.Addresses.Queries;

public record GetAddressesByOwnerQuery(int OwnerTypeID, int OwnerID) : IRequest<List<AddressDto>>;