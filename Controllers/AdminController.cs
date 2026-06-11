using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarLeasingSystem.Data;

namespace CarLeasingSystem.Controllers
{
    // This attribute ensures ONLY users with the "Admin" role can access this controller
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch all bookings and include the Car details
            var allBookings = await _context.Bookings
                .Include(b => b.Car)
                .ToListAsync();

            return View(allBookings);
        }
    }
}
