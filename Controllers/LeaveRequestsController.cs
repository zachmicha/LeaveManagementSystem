using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class LeaveRequestsController(ILeaveTypesService _leaveTypesService, ILeaveRequestsService _leaveRequestsService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await _leaveRequestsService.GetEmployeeLeaveRequests();
            return View(model);
        }
        public async Task<IActionResult> Create(int? leaveTypeId)
        {

            var leaveTypes = await _leaveTypesService.GetAllLeaveTypes();
            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name", leaveTypeId);
            var model = new LeaveRequestCreateVM
            {
                LeaveTypes = leaveTypesList,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(1)),

            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model)
        {
            //validate  that the days dont exceed the allocation
            if (await _leaveRequestsService.RequestDatesExceedAllocation(model))
            {
                ModelState.AddModelError(string.Empty, "You have exceeded your allocation");
                ModelState.AddModelError(nameof(model.EndDate), "The number of days requested i invalid");
            }
            if (ModelState.IsValid)
            {
                await _leaveRequestsService.CreateLeaveRequest(model);
                return RedirectToAction(nameof(Index));
            }

            var leaveTypes = await _leaveTypesService.GetAllLeaveTypes();
            model.LeaveTypes = new SelectList(leaveTypes, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Cancel(int id)
        {
            await _leaveRequestsService.CancelLeaveRequest(id);
            return RedirectToAction(nameof(Index));
        }
        // admin and sups review requests
        [Authorize(Policy = "AdminOrSupervisorOnly")]
        public async Task<IActionResult> ListRequests()
        {
            var model = await _leaveRequestsService.AdminGetAllLeaveRequests();
            return View(model);
        }
        [Authorize(Policy = "AdminOrSupervisorOnly")]

        public async Task<IActionResult> Review(int id)
        {
            var model = await _leaveRequestsService.GetLeaveRequestForReview(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Policy = "AdminOrSupervisorOnly")]

        public async Task<IActionResult> Review(int id, bool approved)
        {
            await _leaveRequestsService.ReviewLeaveRequest(id, approved);
            return RedirectToAction(nameof(ListRequests));
        }
    }
}
