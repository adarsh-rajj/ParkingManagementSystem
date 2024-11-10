using AutoMapper;
using BAL.ViewModel;
using DAL.Entities;
using System;

namespace BAL.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<VehicleRegistration, VehicleRegistrationViewModel>().ReverseMap()
                    .ForMember(d => d.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(d => d.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

            Mapper.CreateMap<VehicleRegistration, VehicleRegistrationCreateModel>().ReverseMap();

            Mapper.CreateMap<VehicleRegistrationCreateModel, VehicleRegistration>()
                    .ForMember(d => d.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));
                    //.ForMember(d => d.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));


            Mapper.CreateMap<ParkingAllotment, ParkingAllocationViewModel>().ReverseMap()
                    .ForMember(d => d.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(d => d.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

            Mapper.CreateMap<ParkingAllotment, ParkingCreateViewModel>().ReverseMap();

            Mapper.CreateMap<ParkingCreateViewModel, ParkingAllotment>()
                    .ForMember(d => d.AllocationId, opt => opt.Ignore())
                    .ForMember(d => d.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));
                    //.ForMember(d => d.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

            Mapper.CreateMap<ParkingBlock, BlockViewModel>().ReverseMap();
        }
    }
}
