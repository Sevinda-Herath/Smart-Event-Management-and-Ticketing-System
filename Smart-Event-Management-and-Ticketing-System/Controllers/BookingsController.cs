using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Event_Management_and_Ticketing_System.Data;
using Smart_Event_Management_and_Ticketing_System.Models;
using Smart_Event_Management_and_Ticketing_System.Helpers;
using Smart_Event_Management_and_Ticketing_System.Filters;

namespace Smart_Event_Management_and_Ticketing_System.Controllers
{
    /// <summary>
    /// Bookings Controller - Handles ticket booking operations
    /// Only accessible to logged-in members
    /// </summary>
    [MemberAuthorization]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Helper method to set ViewBag values for all booking views
        /// </summary>
        private void SetViewBagData()
        {
            ViewBag.IsLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            ViewBag.IsAdmin = SessionHelper.IsAdmin(HttpContext.Session);
        }

        // GET: Bookings
        /// <summary>
        /// Display all bookings for the logged-in member
        /// </summary>
        public async Task<IActionResult> Index()
        {
            SetViewBagData();
            var memberId = SessionHelper.GetMemberId(HttpContext.Session);

            var bookings = await _context.Bookings
                .Include(b => b.Event)
                .Where(b => b.MemberId == memberId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Bookings/Create?eventId=5
        /// <summary>
        /// Display booking form for a specific event
        /// </summary>
        public async Task<IActionResult> Create(int? eventId)
        {
            if (eventId == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (eventItem == null)
            {
                return NotFound();
            }

            SetViewBagData();
            ViewBag.Event = eventItem;
            ViewBag.AvailableSeats = eventItem.AvailableSeats;

            // Create a new booking object with default values
            var booking = new Booking
            {
                EventId = eventItem.EventId,
                Quantity = 1,
                SeatType = "Standard"
            };

            return View(booking);
        }

        // POST: Bookings/Create
        /// <summary>
        /// Process ticket booking
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            if (memberId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Set member ID and booking date
            booking.MemberId = memberId.Value;
            booking.BookingDate = DateTime.Now;

            // Get event to check availability
            var eventItem = await _context.Events
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(e => e.EventId == booking.EventId);

            if (eventItem == null)
            {
                return NotFound();
            }

            // Validate seat availability
            if (eventItem.AvailableSeats < booking.Quantity)
            {
                ModelState.AddModelError("Quantity", $"Only {eventItem.AvailableSeats} seats available.");
                SetViewBagData();
                ViewBag.Event = eventItem;
                ViewBag.AvailableSeats = eventItem.AvailableSeats;
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Successfully booked {booking.Quantity} ticket(s) for {eventItem.EventName}!";
                return RedirectToAction(nameof(Index));
            }

            SetViewBagData();
            ViewBag.Event = eventItem;
            ViewBag.AvailableSeats = eventItem.AvailableSeats;
            return View(booking);
        }

        // GET: Bookings/Details/5
        /// <summary>
        /// Display details of a specific booking
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.MemberId == memberId);

            if (booking == null)
            {
                return NotFound();
            }

            SetViewBagData();
            return View(booking);
        }

        // GET: Bookings/Delete/5
        /// <summary>
        /// Display confirmation page for canceling a booking
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.MemberId == memberId);

            if (booking == null)
            {
                return NotFound();
            }

            SetViewBagData();
            return View(booking);
        }

        // POST: Bookings/Delete/5
        /// <summary>
        /// Process booking cancellation
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            var booking = await _context.Bookings
                .Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.MemberId == memberId);

            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking canceled successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
