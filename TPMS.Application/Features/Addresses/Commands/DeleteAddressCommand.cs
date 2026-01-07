using MediatR;

namespace TPMS.Application.Features.Addresses.Commands;

public record DeleteAddressCommand(int AddressID) : IRequest<bool>;