
using AutoMapper;
using LeaveManagementSystem.Models.LeaveAllocations;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            var leaveTypes= await _context.LeaveTypes.ToListAsync();

            var currentDate= DateTime.Now;
            var period = await _context.Periods.SingleAsync(q=>q.EndDate.Year==currentDate.Year);
            var monthsRemaining = period.EndDate.Month - currentDate.Month;
            foreach (var leaveType in leaveTypes) 
            {
                var accuralRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId=period.Id,
                    NumberOfDays= (int)Math.Ceiling(accuralRate * monthsRemaining),
                };
                _context.Add(leaveAllocation);
            }
              await _context.SaveChangesAsync();

        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocation(string? userId)
        {
            var user=string.IsNullOrEmpty(userId)
                ? await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User)
                : await _userManager.FindByIdAsync(userId);
            var allocations = await GetAllocations(user.Id);
            var allocationVmList= _mapper.Map<List<LeaveAllocation>,List<LeaveAllocationVM>>(allocations);


            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList
            };

            return employeeVm;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.Employee);
            var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users.ToList());
            return employees;
        }
        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {
            //string employeeId= string.Empty;
            //if (!string.IsNullOrEmpty(userId))
            //{
            //    employeeId = userId;
            //}
            //else
            //{
            //var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            //    employeeId = user.Id;
            //}

            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var monthsRemaining = period.EndDate.Month - currentDate.Month;
            var leaveAllocations = await _context.LeaveAllocation
                .Include(l => l.LeaveType)
                .Include(l => l.Period)
                .Where(q=>q.EmployeeId== userId && q.PeriodId==period.Id)
                .ToListAsync();
            return leaveAllocations;
        }
    }
}
