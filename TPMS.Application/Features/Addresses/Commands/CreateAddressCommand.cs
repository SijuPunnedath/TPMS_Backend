using MediatR;
using TPMS.Application.Features.Addresses.DTOs;

namespace TPMS.Application.Features.Addresses.Commands;

public class CreateAddressCommand : IRequest<int>
{
    public AddressDto Address { get; set; }
    public CreateAddressCommand(AddressDto address) => Address = address;
}