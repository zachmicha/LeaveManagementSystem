using LeaveManagementSystem.Models.LeaveAllocations;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId);
        Task EditAllocation(LeaveAllocationEditVM allocationEditVm);
        Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocation(string? id);
        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId);
        Task<List<EmployeeListVM>> GetEmployees();
    }
}
