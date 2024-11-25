
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Services.Periods
{
    public class PeriodsService(ApplicationDbContext _context) : IPeriodsService
    {
        public async Task<Period> GetCurrentPeriod()
        {
            var currentDate= DateTime.Now;
            var period = await _context.Periods.SingleAsync( q=> q.EndDate.Year == currentDate.Year );
            return period;
        }
    }
}
