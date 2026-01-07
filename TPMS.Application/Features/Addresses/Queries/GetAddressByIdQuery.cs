using MediatR;
using TPMS.Application.Features.Addresses.DTOs;

namespace TPMS.Application.Features.Addresses.Queries;

public record GetAddressByIdQuery(int AddressID) : IRequest<AddressDto?>;