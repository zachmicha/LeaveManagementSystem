
using AutoMapper;
using LeaveManagementSystem.Models.LeaveAllocations;
using LeaveManagementSystem.Services.Periods;
using LeaveManagementSystem.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, IUserService _userSerivce, IMapper _mapper, IPeriodsService _periodsService) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            var leaveTypes= await _context.LeaveTypes.Where(q=> !q.LeaveAllocations.Any(x=> x.EmployeeId == employeeId)).ToListAsync();

            var period = await _periodsService.GetCurrentPeriod();
            var monthsRemaining = period.EndDate.Month - DateTime.Now.Month;

           
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
            var user = string.IsNullOrEmpty(userId)
                ? await _userSerivce.GetLoggedInUser()
                : await _userSerivce.GetUserById(userId);
            var allocations = await GetAllocations(user.Id);
            var allocationVmList= _mapper.Map<List<LeaveAllocation>,List<LeaveAllocationVM>>(allocations);
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();

            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList,
                IsCompletedAllocation = leaveTypesCount == allocations.Count,
            };

            return employeeVm;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var users = await _userSerivce.GetEmployees();
            var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users.ToList());
            return employees;
        }
        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
        {
            var allocation = await _context.LeaveAllocation
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == allocationId);
            var model = _mapper.Map<LeaveAllocationEditVM>(allocation);
            return model;
        }
        public async Task EditAllocation(LeaveAllocationEditVM allocationEditVm)
        {
            //var leaveAllocation = await GetEmployeeAllocation(allocationEditVm.Id);
            //if (leaveAllocation==null)
            //{
            //    throw new Exception("Leave allocation record does not exists");
            //}
            //leaveAllocation.NumberOfDays = allocationEditVm.NumberOfDays;
            //_context.Update(leaveAllocation);

            await _context.LeaveAllocation.Where(q=>q.Id == allocationEditVm.Id).ExecuteUpdateAsync(s=>s.SetProperty(e=>e.NumberOfDays, allocationEditVm.NumberOfDays));
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

            var period = await _periodsService.GetCurrentPeriod();
            
            var leaveAllocations = await _context.LeaveAllocation
                .Include(l => l.LeaveType)
                .Include(l => l.Period)
                .Where(q=>q.EmployeeId== userId && q.PeriodId==period.Id)
                .ToListAsync();
            return leaveAllocations;
        }
        public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
        {
            var period = await _periodsService.GetCurrentPeriod();
            var allocation = await _context.LeaveAllocation.FirstAsync(q=>q.LeaveTypeId==leaveTypeId && q.EmployeeId==employeeId && q.PeriodId==period.Id);
            return allocation;
        }
        private async Task<bool> AllocationExisits(string userId, int periodId, int leaveTypeId)
        {
            var exist = await _context.LeaveAllocation.AnyAsync(q => 
            q.EmployeeId == userId
            && q.LeaveTypeId == leaveTypeId
            && q.PeriodId==periodId);
            return exist;
        }

    }
}
