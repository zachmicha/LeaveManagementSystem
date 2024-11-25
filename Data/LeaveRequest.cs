namespace LeaveManagementSystem.Data
{
    public class LeaveRequest : BaseEntity
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public LeaveType? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        public LeaveRequestStatus? LeaveRequestStatus { get; set; }
        public int LeaveRequestStatusId { get; set; }
        public ApplicationUser? Employee { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationUser? Reviewer { get; set; }
        public string? ReviewerId { get; set; }
        public string? RequestComments { get; set; }

    }
}