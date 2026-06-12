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
        public IActionResult Create(int carId, DateTime? start, DateTime? end)
        {
            var car = _context.Cars.Find(carId);
            if (car == null) return NotFound();

            // Create a new booking object to pass to the view
            var booking = new Booking 
            { 
                CarId = carId, 
                Car = car, 
                StartDate = start ?? DateTime.Today, 
                EndDate = end ?? DateTime.Today.AddDays(1) 
            };
    
            return View(booking);
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,StartDate,EndDate")] Booking booking)
        {
            // 1. Verify availability one last time (Prevents double-booking)
            bool isAlreadyBooked = await _context.Bookings.AnyAsync(b => 
                b.CarId == booking.CarId && 
                b.StartDate < booking.EndDate && 
                b.EndDate > booking.StartDate);

            if (isAlreadyBooked)
            {
                ModelState.AddModelError("", "Sorry, this car was just booked by someone else for these dates.");
            }

            // 2. Perform additional logic (e.g., check if start date is in the past)
            if (booking.StartDate < DateTime.Today)
            {
                ModelState.AddModelError("StartDate", "You cannot book a car in the past.");
            }

            // 3. Only proceed if the custom logic AND the model are valid
            if (ModelState.IsValid)
            {
                booking.CustomerName = User.Identity.Name;
                _context.Add(booking);
                await _context.SaveChangesAsync();
        
                TempData["Message"] = "Booking confirmed successfully!";
                return RedirectToAction("MyBookings"); 
            }

            // 4. If we reach here, there was an error. 
            // We must re-fetch the Car so the view doesn't crash.
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
        
        // GET: Bookings/Reschedule/5
        public async Task<IActionResult> Reschedule(int id)
        {
            var booking = await _context.Bookings.Include(b => b.Car).FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null || booking.CustomerName != User.Identity.Name) return NotFound();

            return View(booking);
        }

        // POST: Bookings/Reschedule/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reschedule(int id, Booking booking)
        {
            var existingBooking = await _context.Bookings.FindAsync(id);
            if (existingBooking == null || existingBooking.CustomerName != User.Identity.Name) return NotFound();

            // 1. Check for conflicts, EXCLUDING the current booking being edited
            bool isConflict = await _context.Bookings.AnyAsync(b => 
                b.CarId == existingBooking.CarId && 
                b.Id != id && // <--- This is the key: Ignore this record!
                b.StartDate < booking.EndDate && 
                b.EndDate > booking.StartDate);

            if (isConflict)
            {
                ModelState.AddModelError("", "This car is already booked for these dates.");
            }

            if (ModelState.IsValid && !isConflict)
            {
                existingBooking.StartDate = booking.StartDate;
                existingBooking.EndDate = booking.EndDate;
        
                _context.Update(existingBooking);
                await _context.SaveChangesAsync();
        
                TempData["Message"] = "Booking updated successfully!";
                return RedirectToAction("MyBookings");
            }

            // Reload the view with errors
            booking.Car = await _context.Cars.FindAsync(existingBooking.CarId);
            return View(booking);
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
