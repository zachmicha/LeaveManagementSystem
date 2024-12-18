namespace LeaveManagementSystem.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var data = new TestViewModel
            {
                Name = "Mike",
                DateOfBirth = DateTime.Now,
            };
            return View(data);
        }
    }
}
