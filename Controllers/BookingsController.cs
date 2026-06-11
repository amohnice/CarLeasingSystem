using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarLeasingSystem.Data;
using CarLeasingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarLeasingSystem.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: Bookings/SearchResults
        [HttpPost]
        public async Task<IActionResult> SearchResults(DateTime startDate, DateTime endDate)
        {
            // Logic: Find cars that have NO booking overlapping with the selected dates
            var availableCars = await _context.Cars
                .Where(c => !_context.Bookings.Any(b => 
                    b.CarId == c.Id && 
                    b.StartDate < endDate && 
                    b.EndDate > startDate)) // The "Overlap" logic
                .ToListAsync();

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View(availableCars);
        }
        // GET: Bookings/Create
        public IActionResult Create(int carId, DateTime start, DateTime end)
        {
            var car = _context.Cars.Find(carId);
            if (car == null) return NotFound();

            // Create a new booking object to pass to the view
            var booking = new Booking 
            { 
                CarId = carId, 
                Car = car, 
                StartDate = start, 
                EndDate = end 
            };
    
            return View(booking);
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,StartDate,EndDate")] Booking booking)
        {
            ModelState.Remove("Car");

            booking.CustomerName = User.Identity.Name;
    
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
        
                TempData["Message"] = "Booking confirmed successfully!";
                return RedirectToAction("Index", "Home"); 
            }

            booking.Car = await _context.Cars.FindAsync(booking.CarId);
            return View(booking);
        }
        
        public async Task<IActionResult> MyBookings()
        {
            var userName = User.Identity.Name;
            var myBookings = await _context.Bookings
                .Include(b => b.Car)
                .Where(b => b.CustomerName == userName)
                .ToListAsync();

            return View(myBookings);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            // Security: Only allow the owner to cancel
            if (booking == null || booking.CustomerName != User.Identity.Name)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Booking cancelled successfully.";
            return RedirectToAction("MyBookings");
        }
    }
}
