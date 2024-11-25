public class EmployeeListVM
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;
    [Display(Name = "Email adress")]
    public string Email { get; set; } = string.Empty;
}