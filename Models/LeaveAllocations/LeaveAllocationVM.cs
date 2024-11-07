using LeaveManagementSystem.Models.LeaveTypes;
using LeaveManagementSystem.Models.Periods;

namespace LeaveManagementSystem.Models.LeaveAllocations
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }

        [Display(Name = "Number of days")]
        public int NumberOfDays { get; set; }

        [Display(Name = "Allocation period")]
        public PeriodVM Period { get; set; } = new();
        public LeaveTypeReadOnlyVM LeaveType { get; set; } = new();
    }
}
