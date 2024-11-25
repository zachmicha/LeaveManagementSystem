using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Services.LeaveAllocations;
using LeaveManagementSystem.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService, ILeaveTypesService _leaveTypesService) : Controller
    {
        public async Task<IActionResult> Details(string? userId)
        {
            
          var employeeVm= await _leaveAllocationsService.GetEmployeeAllocation(userId);
            return View(employeeVm);
        }

        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {

            var employeeVm = await _leaveAllocationsService.GetEmployees();
            return View(employeeVm);
        }
        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AllocationLeave(string? Id)
        {

              await _leaveAllocationsService.AllocateLeave(Id);
            return RedirectToAction(nameof(Details), new {userId = Id});
        }

        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var allocation = await _leaveAllocationsService.GetEmployeeAllocation(id.Value);
            if (allocation==null)
            {
                return NotFound();
            }
            return View(allocation);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocationEditVm)
        {
            if (await _leaveTypesService.DaysExceedMaximum(allocationEditVm.LeaveType.Id, allocationEditVm.NumberOfDays))
            {
                ModelState.AddModelError("NumberOfDays", "Alloaction exceeds maximum leave type value");
            }
            if (ModelState.IsValid)
            {
            await _leaveAllocationsService.EditAllocation(allocationEditVm);
            return RedirectToAction(nameof(Details), new { userId = allocationEditVm.Employee.Id});
            }
            var days = allocationEditVm.NumberOfDays;
            allocationEditVm = await _leaveAllocationsService.GetEmployeeAllocation(allocationEditVm.Id);
            allocationEditVm.NumberOfDays = days;
            return View(allocationEditVm);

        }
    }

   
}
