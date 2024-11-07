namespace LeaveManagementSystem.Data
{
    public class Period : BaseEntity
    {
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
