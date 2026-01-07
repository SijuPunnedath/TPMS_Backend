using AutoMapper;
using TPMS.Domain.Entities;
using TPMS.Application.Features.Leases.DTOs;

public class LeaseProfile : Profile
{
    public LeaseProfile()
    {
        CreateMap<Lease, LeaseDto>().ReverseMap();
        CreateMap<LeaseDto, Lease>().ReverseMap();
        
        CreateMap<LeaseCreateDto, Lease>()
            .ForMember(d => d.LeaseType,
                opt 
                    => opt.MapFrom(s => (int)s.LeaseType)).ReverseMap();

            
        CreateMap<LeaseUpdateDto, Lease>().ReverseMap();

        // If you have a separate update DTO
        CreateMap<Lease, LeaseUpdateDto>().ReverseMap();
    }
}