using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Event_Management_and_Ticketing_System.Data;
using Smart_Event_Management_and_Ticketing_System.Models;
using Smart_Event_Management_and_Ticketing_System.Helpers;
using Smart_Event_Management_and_Ticketing_System.Filters;

namespace Smart_Event_Management_and_Ticketing_System.Controllers
{
    /// <summary>
    /// Admin Controller - Handles admin dashboard and event management
    /// Only accessible to users with Admin role
    /// </summary>
    [AdminAuthorization]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Helper method to set ViewBag values for all admin views
        /// </summary>
        private void SetViewBagData()
        {
            ViewBag.IsLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            ViewBag.IsAdmin = SessionHelper.IsAdmin(HttpContext.Session);
        }

        // GET: Admin
        /// <summary>
        /// Admin dashboard showing statistics and quick links
        /// </summary>
        public async Task<IActionResult> Index()
        {
            SetViewBagData();

            // Get statistics
            ViewBag.TotalEvents = await _context.Events.CountAsync();
            ViewBag.TotalMembers = await _context.Members.Where(m => m.Role == "Member").CountAsync();
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();
            ViewBag.TotalInquiries = await _context.Inquiries.CountAsync();
            ViewBag.TotalReviews = await _context.Reviews.CountAsync();

            // Get upcoming events
            var upcomingEvents = await _context.Events
                .Where(e => e.EventDate >= DateTime.Now)
                .OrderBy(e => e.EventDate)
                .Take(5)
                .ToListAsync();

            return View(upcomingEvents);
        }

        // GET: Admin/Events
        /// <summary>
        /// Display all events for admin management
        /// </summary>
        public async Task<IActionResult> Events()
        {
            SetViewBagData();

            var events = await _context.Events
                .Include(e => e.Bookings)
                .Include(e => e.Reviews)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        // GET: Admin/CreateEvent
        /// <summary>
        /// Display form to create a new event
        /// </summary>
        public IActionResult CreateEvent()
        {
            SetViewBagData();
            return View();
        }

        // POST: Admin/CreateEvent
        /// <summary>
        /// Process new event creation
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(Event eventItem)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(eventItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Event '{eventItem.EventName}' created successfully!";
                return RedirectToAction(nameof(Events));
            }

            SetViewBagData();
            return View(eventItem);
        }

        // GET: Admin/EditEvent/5
        /// <summary>
        /// Display form to edit an existing event
        /// </summary>
        public async Task<IActionResult> EditEvent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            SetViewBagData();
            return View(eventItem);
        }

        // POST: Admin/EditEvent/5
        /// <summary>
        /// Process event edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(int id, Event eventItem)
        {
            if (id != eventItem.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Event '{eventItem.EventName}' updated successfully!";
                    return RedirectToAction(nameof(Events));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Events.AnyAsync(e => e.EventId == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            SetViewBagData();
            return View(eventItem);
        }

        // GET: Admin/DeleteEvent/5
        /// <summary>
        /// Display confirmation page for event deletion
        /// </summary>
        public async Task<IActionResult> DeleteEvent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Bookings)
                .Include(e => e.Reviews)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            SetViewBagData();
            return View(eventItem);
        }

        // POST: Admin/DeleteEvent/5
        /// <summary>
        /// Process event deletion
        /// </summary>
        [HttpPost, ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEventConfirmed(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Bookings)
                .Include(e => e.Reviews)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem != null)
            {
                // Delete related bookings and reviews first
                _context.Bookings.RemoveRange(eventItem.Bookings);
                _context.Reviews.RemoveRange(eventItem.Reviews);
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Event '{eventItem.EventName}' deleted successfully!";
            }

            return RedirectToAction(nameof(Events));
        }

        // GET: Admin/Bookings
        /// <summary>
        /// Display all bookings with member details
        /// </summary>
        public async Task<IActionResult> Bookings(int? eventId, int? memberId)
        {
            SetViewBagData();

            var bookingsQuery = _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Event)
                .AsQueryable();

            // Filter by event if specified
            if (eventId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.EventId == eventId.Value);
                ViewBag.FilteredEventName = (await _context.Events.FindAsync(eventId.Value))?.EventName;
            }

            // Filter by member if specified
            if (memberId.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.MemberId == memberId.Value);
                ViewBag.FilteredMemberName = (await _context.Members.FindAsync(memberId.Value))?.FullName;
            }

            var bookings = await bookingsQuery
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            // Get events and members for filter dropdowns
            ViewBag.Events = await _context.Events.OrderBy(e => e.EventName).ToListAsync();
            ViewBag.Members = await _context.Members.Where(m => m.Role == "Member").OrderBy(m => m.FullName).ToListAsync();

            return View(bookings);
        }

        // GET: Admin/Inquiries
        /// <summary>
        /// Display all inquiries
        /// </summary>
        public async Task<IActionResult> Inquiries()
        {
            SetViewBagData();

            var inquiries = await _context.Inquiries
                .OrderByDescending(i => i.InquiryDate)
                .ToListAsync();

            return View(inquiries);
        }

        // GET: Admin/Members
        /// <summary>
        /// Display all members
        /// </summary>
        public async Task<IActionResult> Members()
        {
            SetViewBagData();

            var members = await _context.Members
                .Include(m => m.Bookings)
                .Include(m => m.Reviews)
                .Where(m => m.Role == "Member")
                .OrderBy(m => m.FullName)
                .ToListAsync();

            return View(members);
        }

        // GET: Admin/EditMember/5
        /// <summary>
        /// Display form to edit a member's information
        /// </summary>
        public async Task<IActionResult> EditMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            // Prevent editing admin users
            if (member.Role == "Admin")
            {
                TempData["ErrorMessage"] = "Cannot edit admin users.";
                return RedirectToAction(nameof(Members));
            }

            SetViewBagData();
            return View(member);
        }

        // POST: Admin/EditMember/5
        /// <summary>
        /// Process member edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMember(int id, Member member, string? NewPassword)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            // Ensure role stays as Member (prevent privilege escalation)
            member.Role = "Member";

            // If a new password is provided, use it (plain text)
            if (!string.IsNullOrEmpty(NewPassword))
            {
                if (NewPassword.Length < 6)
                {
                    ModelState.AddModelError("NewPassword", "Password must be at least 6 characters.");
                    SetViewBagData();
                    return View(member);
                }
                member.Password = NewPassword;
            }
            // Otherwise, keep the existing password (already in member.Password from hidden field)

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Member '{member.FullName}' updated successfully!";
                    return RedirectToAction(nameof(Members));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Members.AnyAsync(m => m.MemberId == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            SetViewBagData();
            return View(member);
        }

        // GET: Admin/DeleteMember/5
        /// <summary>
        /// Display confirmation page for member deletion
        /// </summary>
        public async Task<IActionResult> DeleteMember(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Bookings)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                return NotFound();
            }

            // Prevent deleting admin users
            if (member.Role == "Admin")
            {
                TempData["ErrorMessage"] = "Cannot delete admin users.";
                return RedirectToAction(nameof(Members));
            }

            SetViewBagData();
            return View(member);
        }

        // POST: Admin/DeleteMember/5
        /// <summary>
        /// Process member deletion (also removes their bookings and reviews)
        /// </summary>
        [HttpPost, ActionName("DeleteMember")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMemberConfirmed(int id)
        {
            var member = await _context.Members
                .Include(m => m.Bookings)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member != null)
            {
                // Prevent deleting admin users
                if (member.Role == "Admin")
                {
                    TempData["ErrorMessage"] = "Cannot delete admin users.";
                    return RedirectToAction(nameof(Members));
                }

                // Delete related bookings and reviews first
                _context.Bookings.RemoveRange(member.Bookings);
                _context.Reviews.RemoveRange(member.Reviews);
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Member '{member.FullName}' deleted successfully!";
            }

            return RedirectToAction(nameof(Members));
        }
    }
}
