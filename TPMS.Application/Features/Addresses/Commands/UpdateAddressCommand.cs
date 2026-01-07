using MediatR;
using TPMS.Application.Features.Addresses.DTOs;

namespace TPMS.Application.Features.Addresses.Commands;

public class UpdateAddressCommand : IRequest<bool>
{
    public AddressDto Address { get; set; }
    public UpdateAddressCommand(AddressDto address) => Address = address;
}