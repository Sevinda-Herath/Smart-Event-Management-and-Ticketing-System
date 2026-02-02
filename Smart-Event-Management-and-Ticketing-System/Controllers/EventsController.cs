using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Event_Management_and_Ticketing_System.Data;
using Smart_Event_Management_and_Ticketing_System.Helpers;

namespace Smart_Event_Management_and_Ticketing_System.Controllers
{
    /// <summary>
    /// Events Controller - Handles event browsing and searching
    /// Accessible to both guests and members (with different views)
    /// </summary>
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        /// <summary>
        /// Display list of events with search/filter options
        /// </summary>
        public async Task<IActionResult> Index(string? category, DateTime? date, string? location, decimal? maxPrice, string? searchTerm)
        {
            var isLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
            ViewBag.IsLoggedIn = isLoggedIn;
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);

            // Start with all events
            var eventsQuery = _context.Events
                .Include(e => e.Bookings)
                .Include(e => e.Reviews)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(category))
            {
                eventsQuery = eventsQuery.Where(e => e.Category == category);
                ViewBag.SelectedCategory = category;
            }

            if (date.HasValue)
            {
                eventsQuery = eventsQuery.Where(e => e.EventDate.Date == date.Value.Date);
                ViewBag.SelectedDate = date.Value.ToString("yyyy-MM-dd");
            }

            if (!string.IsNullOrEmpty(location))
            {
                eventsQuery = eventsQuery.Where(e => e.Venue.Contains(location));
                ViewBag.SelectedLocation = location;
            }

            if (maxPrice.HasValue)
            {
                eventsQuery = eventsQuery.Where(e => e.Price <= maxPrice.Value);
                ViewBag.MaxPrice = maxPrice.Value;
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                eventsQuery = eventsQuery.Where(e => e.EventName.Contains(searchTerm) || e.Description.Contains(searchTerm));
                ViewBag.SearchTerm = searchTerm;
            }

            // Get unique categories for filter dropdown
            ViewBag.Categories = await _context.Events
                .Select(e => e.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            var events = await eventsQuery
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        // GET: Events/Details/5
        /// <summary>
        /// Display detailed information about a specific event
        /// Members see full details, guests see limited information
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Bookings)
                .Include(e => e.Reviews)
                    .ThenInclude(r => r.Member)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            var isLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
            ViewBag.IsLoggedIn = isLoggedIn;
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);

            // Check if current member has booked this event (for review eligibility)
            if (isLoggedIn)
            {
                var memberId = SessionHelper.GetMemberId(HttpContext.Session);
                var hasBooked = await _context.Bookings
                    .AnyAsync(b => b.MemberId == memberId && b.EventId == id);
                ViewBag.HasBooked = hasBooked;

                // Check if member has already reviewed this event
                var hasReviewed = await _context.Reviews
                    .AnyAsync(r => r.MemberId == memberId && r.EventId == id);
                ViewBag.HasReviewed = hasReviewed;
            }

            return View(eventItem);
        }
    }
}
