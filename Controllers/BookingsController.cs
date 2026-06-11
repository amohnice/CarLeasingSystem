using Microsoft.AspNetCore.Mvc;
using CarLeasingSystem.Data;
using CarLeasingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarLeasingSystem.Controllers
{
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
        public async Task<IActionResult> Create([Bind("CarId,StartDate,EndDate,CustomerName")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
        
                TempData["Message"] = "Booking confirmed successfully!";
                return RedirectToAction("Index", "Home"); 
            }

            // If validation fails, we still need to load the Car for the view to render correctly
            booking.Car = _context.Cars.Find(booking.CarId);
            return View(booking);
        }
    }
}
