
using AutoMapper;
using TPMS.Domain.Entities;
using TPMS.Application.Features.Leases.DTOs;


public class LeaseProfile : Profile
{
    public LeaseProfile()
    {
        //  Entity → DTO
        CreateMap<Lease, LeaseDto>()

            // Property mapping
            .ForMember(dest => dest.PropertyNumber,
                opt => opt.MapFrom(src =>
                    src.Property != null ? src.Property.PropertyNumber : null))

            // Tenant mapping
            .ForMember(dest => dest.TenantName,
                opt => opt.MapFrom(src =>
                    src.Tenant != null ? src.Tenant.Name : null))

            // Landlord mapping
            .ForMember(dest => dest.LandlordName,
                opt => opt.MapFrom(src =>
                    src.Landlord != null ? src.Landlord.Name : null))

            // Deposit mapping (if navigation exists)
            .ForMember(dest => dest.DepositMaster,
                opt => opt.MapFrom(src => src.DepositMaster))

            .ReverseMap();


        // ✅ Create DTO → Entity
        CreateMap<LeaseCreateDto, Lease>()
            .ForMember(d => d.LeaseType,
                opt => opt.MapFrom(s => s.LeaseType));

        // ✅ Update DTO
        CreateMap<LeaseUpdateDto, Lease>().ReverseMap();
    }
}
/*
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
}*/