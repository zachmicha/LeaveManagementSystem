using LeaveManagementSystem.Models.LeaveAllocations;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocation(string? id);
        Task<List<EmployeeListVM>> GetEmployees();
    }
}
