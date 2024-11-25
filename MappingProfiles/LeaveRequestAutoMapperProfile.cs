using AutoMapper;
using LeaveManagementSystem.Models.LeaveRequests;

namespace LeaveManagementSystem.MappingProfiles
{
    public class LeaveRequestAutoMapperProfile : Profile
    {
        public LeaveRequestAutoMapperProfile()
        {
            CreateMap<LeaveRequestCreateVM, LeaveRequest >();

        }
    }
}
