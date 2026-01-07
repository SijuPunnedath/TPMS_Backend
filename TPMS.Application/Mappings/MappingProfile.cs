using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Deposit.DTOs;
using TPMS.Application.Features.Landlords.DTOs;
using TPMS.Application.Features.LeaseAlert.DTOs;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Properties.DTOs;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Application.Features.Tenants.DTOs;
using TPMS.Domain.Entities;

namespace TPMS.Application.Mappings
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            //  Lease and related DTOs
           // CreateMap<Lease, LeaseDto>().ReverseMap();
           CreateMap<DepositMaster, DepositMasterDto>().ReverseMap();
           CreateMap<DepositTransaction, DepositTransactionDto>().ReverseMap();
           
          // CreateMap<LeaseCreateDto, Lease>();
           //CreateMap<LeaseUpdateDto, Lease>();
         
           CreateMap<RentSchedule, RentScheduleDto>().ReverseMap();
            CreateMap<LeaseAlert, LeaseAlertDto>().ReverseMap();

            //  Landlord
            CreateMap<Landlord, LandlordDto>().ReverseMap();

            //  Tenant
            CreateMap<Tenant, TenantDto>().ReverseMap();

            //  Property
            CreateMap<Property, PropertyDto>().ReverseMap();

        }

    }
}
