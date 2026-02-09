using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Event_Management_and_Ticketing_System.Data;
using Smart_Event_Management_and_Ticketing_System.Models;
using Smart_Event_Management_and_Ticketing_System.Helpers;
using Smart_Event_Management_and_Ticketing_System.Filters;

namespace Smart_Event_Management_and_Ticketing_System.Controllers
{
    /// <summary>
    /// Reviews Controller - Handles review submissions
    /// Only accessible to logged-in members who have booked the event
    /// </summary>
    [MemberAuthorization]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews/Create?eventId=5
        /// <summary>
        /// Display review form for a specific event
        /// </summary>
        public async Task<IActionResult> Create(int? eventId)
        {
            if (eventId == null)
            {
                return NotFound();
            }

            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            if (memberId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if member has booked this event
            var hasBooked = await _context.Bookings
                .AnyAsync(b => b.MemberId == memberId && b.EventId == eventId);

            if (!hasBooked)
            {
                TempData["ErrorMessage"] = "You can only review events you have booked.";
                return RedirectToAction("Details", "Events", new { id = eventId });
            }

            // Check if member has already reviewed this event
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.MemberId == memberId && r.EventId == eventId);

            if (existingReview != null)
            {
                TempData["ErrorMessage"] = "You have already reviewed this event.";
                return RedirectToAction("Details", "Events", new { id = eventId });
            }

            var eventItem = await _context.Events.FindAsync(eventId);
            if (eventItem == null)
            {
                return NotFound();
            }

            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            ViewBag.Event = eventItem;

            var review = new Review
            {
                EventId = eventId.Value,
                Rating = 5
            };

            return View(review);
        }

        // POST: Reviews/Create
        /// <summary>
        /// Process review submission
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            if (memberId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Set member ID and review date
            review.MemberId = memberId.Value;
            review.ReviewDate = DateTime.Now;

            // Verify member has booked the event
            var hasBooked = await _context.Bookings
                .AnyAsync(b => b.MemberId == memberId && b.EventId == review.EventId);

            if (!hasBooked)
            {
                TempData["ErrorMessage"] = "You can only review events you have booked.";
                return RedirectToAction("Details", "Events", new { id = review.EventId });
            }

            // Check for existing review
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.MemberId == memberId && r.EventId == review.EventId);

            if (existingReview != null)
            {
                TempData["ErrorMessage"] = "You have already reviewed this event.";
                return RedirectToAction("Details", "Events", new { id = review.EventId });
            }

            if (ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you for your review!";
                return RedirectToAction("Details", "Events", new { id = review.EventId });
            }

            var eventItem = await _context.Events.FindAsync(review.EventId);
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            ViewBag.Event = eventItem;
            return View(review);
        }

        // GET: Reviews/Edit/5
        /// <summary>
        /// Display form to edit an existing review
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            var review = await _context.Reviews
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.ReviewId == id && r.MemberId == memberId);

            if (review == null)
            {
                return NotFound();
            }

            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            ViewBag.Event = review.Event;
            return View(review);
        }

        // POST: Reviews/Edit/5
        /// <summary>
        /// Process review edit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            review.MemberId = memberId.Value;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Review updated successfully!";
                    return RedirectToAction("Details", "Events", new { id = review.EventId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Reviews.AnyAsync(r => r.ReviewId == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            var eventItem = await _context.Events.FindAsync(review.EventId);
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            ViewBag.Event = eventItem;
            return View(review);
        }

        // GET: Reviews/Delete/5
        /// <summary>
        /// Display confirmation page for review deletion
        /// Only the review author can delete their review
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            var review = await _context.Reviews
                .Include(r => r.Event)
                .Include(r => r.Member)
                .FirstOrDefaultAsync(r => r.ReviewId == id && r.MemberId == memberId);

            if (review == null)
            {
                TempData["ErrorMessage"] = "Review not found or you don't have permission to delete it.";
                return RedirectToAction("Index", "Events");
            }

            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            return View(review);
        }

        // POST: Reviews/Delete/5
        /// <summary>
        /// Process review deletion
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberId = SessionHelper.GetMemberId(HttpContext.Session);
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == id && r.MemberId == memberId);

            if (review == null)
            {
                TempData["ErrorMessage"] = "Review not found or you don't have permission to delete it.";
                return RedirectToAction("Index", "Events");
            }

            var eventId = review.EventId;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your review has been deleted successfully.";
            return RedirectToAction("Details", "Events", new { id = eventId });
        }
    }
}
