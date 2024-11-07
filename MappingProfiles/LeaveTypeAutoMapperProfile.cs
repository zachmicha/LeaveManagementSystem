﻿using AutoMapper;
using LeaveManagementSystem.Models.LeaveTypes;

namespace LeaveManagementSystem.MappingProfiles
{
    public class LeaveTypeAutoMapperProfile : Profile
    {
        public LeaveTypeAutoMapperProfile()
        {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();
            CreateMap<LeaveTypeReadOnlyVM, LeaveType>();
            CreateMap<LeaveTypeEditVM,LeaveType>().ReverseMap();

        }
    }
}