using System.ComponentModel;

namespace LeaveManagementSystem.Models.LeaveRequests
{
    public class ReviewLeaveRequestVM : LeaveRequestReadOnlyVM
    {
        public EmployeeListVM Employee { get; set; } = new();
        [DisplayName("Additional information")]
        public string RequestComments { get; set; }
    }
}