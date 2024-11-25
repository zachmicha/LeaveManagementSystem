using AutoMapper;
using LeaveManagementSystem.Data;
using LeaveManagementSystem.Models.LeaveRequests;
using LeaveManagementSystem.Services.LeaveAllocations;
using LeaveManagementSystem.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.LeaveRequests
{
    public class LeaveRequestsService(IMapper _mapper, IUserService _userService, ApplicationDbContext _context, ILeaveAllocationsService _leaveAllocationService) : ILeaveRequestsService
    {
        public async Task CancelLeaveRequest(int leaveaRequestId)
        {
            var leaveRequest= await _context.LeaveRequests.FindAsync(leaveaRequestId);
            leaveRequest.LeaveRequestStatusId=(int)LeaveRequestStatusEnum.Canceled;

            await UpdateAllocationDays(leaveRequest,false);
            await _context.SaveChangesAsync();
        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);
            var user = await _userService.GetLoggedInUser();
            leaveRequest.EmployeeId = user.Id;
            leaveRequest.LeaveRequestStatusId= (int)LeaveRequestStatusEnum.Pending;
            _context.Add(leaveRequest);

            await UpdateAllocationDays(leaveRequest, true);

            await _context.SaveChangesAsync();

        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userService.GetLoggedInUser();
            var leaveRequests = await _context.LeaveRequests
                .Include(q=>q.LeaveType)
                .Where(q=>q.EmployeeId==user.Id).ToListAsync();
            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM 
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType=q.LeaveType.Name,
                LeaveRequestStatus=(LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays=q.EndDate.DayNumber-q.StartDate.DayNumber,
            }).ToList();
            return model;
        }

        public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
           .Include(q => q.LeaveType).ToListAsync();
            var leaveRequestsModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,
            }).ToList();
            var model = new EmployeeLeaveRequestListVM
            {
                ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved),
                PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending),
                DeclinedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined),
                TotalRequests = leaveRequests.Count,
                LeaveRequests = leaveRequestsModels
            };
            return model;
        }

        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var user = await _userService.GetLoggedInUser();

            var currentDate = DateTime.Now;
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocation = await _context.LeaveAllocation.FirstAsync(q => q.LeaveTypeId == model.LeaveTypeId 
            && q.EmployeeId == user.Id
            && q.PeriodId == period.Id);
            bool isIt = allocation.NumberOfDays < numberOfDays;
            return isIt;
        }

        public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            var user = await _userService.GetLoggedInUser();
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            leaveRequest.LeaveRequestStatusId= approved
                ? (int)LeaveRequestStatusEnum.Approved 
                : (int)LeaveRequestStatusEnum.Declined;
            leaveRequest.ReviewerId = user.Id;
            if (!approved)
            {
                await UpdateAllocationDays(leaveRequest, false);
            }
            await _context.SaveChangesAsync();
        }

        //public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        //{
        //    var leaveRequest = await  _context.LeaveRequests.Include(q=>q.Id==id).FirstAsync(q=>q.Id == id);
        //    var user = await _userManager.FindByIdAsync(leaveRequest.EmployeeId);
        //    var model = new ReviewLeaveRequestVM
        //    {
        //        StartDate = leaveRequest.StartDate,
        //        EndDate = leaveRequest.EndDate,
        //        Id = leaveRequest.Id,
        //        LeaveType = leaveRequest.LeaveType.Name,
        //        NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
        //        LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
        //        RequestComments = leaveRequest.RequestComments,
        //        Employee=new EmployeeListVM
        //        {
        //            Id = leaveRequest.EmployeeId,
        //            Email=user.Email,
        //            FirstName=user.FirstName,
        //            LastName=user.LastName,
        //        },
        //    };
        //    return model;
        //}
        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstAsync(q => q.Id == id);
            var user = await _userService.GetUserById(leaveRequest.EmployeeId);

            var model = new ReviewLeaveRequestVM
            {
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
                Id = leaveRequest.Id,
                LeaveType = leaveRequest.LeaveType.Name,
                RequestComments = leaveRequest.RequestComments,
                Employee = new EmployeeListVM
                {
                    Id = leaveRequest.EmployeeId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                }
            };

            return model;
        }

        private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _leaveAllocationService.GetCurrentAllocation(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
            var numberOfDays = CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);
            if (deductDays)
            {
                allocation.NumberOfDays -= numberOfDays;
            }
            else {
                allocation.NumberOfDays += numberOfDays;
            }
            _context.Entry(allocation).State = EntityState.Modified;
        }

        private int CalculateDays(DateOnly startDate, DateOnly endDate)
        {
            return endDate.DayNumber - startDate.DayNumber;
        }
    }
}
