using LeaveManagementSystem.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
    {
        public async Task<IActionResult> Details(string? userId)
        {
            
          var employeeVm= await _leaveAllocationsService.GetEmployeeAllocation(userId);
            return View(employeeVm);
        }

        [Authorize(Roles.Administrator)]
        public async Task<IActionResult> Index()
        {

            var employeeVm = await _leaveAllocationsService.GetEmployees();
            return View(employeeVm);
        }

    }

   
}
