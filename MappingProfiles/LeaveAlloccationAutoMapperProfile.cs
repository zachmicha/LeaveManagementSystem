using AutoMapper;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Models.LeaveTypes;
using LeaveManagementSystem.Models.Periods;

namespace LeaveManagementSystem.MappingProfiles
{
    public class LeaveAlloccationAutoMapperProfile : Profile
    {
        public LeaveAlloccationAutoMapperProfile()
        {
            CreateMap<LeaveAllocation, LeaveAllocationVM>();
           // CreateMap<LeaveAllocation, LeaveAllocationEditVM>();
            CreateMap<ApplicationUser, EmployeeListVM>();
            CreateMap<Period, PeriodVM>();

        }
    }
}
