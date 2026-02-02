using Microsoft.AspNetCore.Mvc;
using Smart_Event_Management_and_Ticketing_System.Data;
using Smart_Event_Management_and_Ticketing_System.Models;
using Smart_Event_Management_and_Ticketing_System.Helpers;

namespace Smart_Event_Management_and_Ticketing_System.Controllers
{
    /// <summary>
    /// Inquiries Controller - Handles contact/inquiry form submissions
    /// Accessible to both guests and members
    /// </summary>
    public class InquiriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InquiriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inquiries/Create
        /// <summary>
        /// Display inquiry/contact form
        /// </summary>
        public IActionResult Create()
        {
            var isLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
            ViewBag.IsLoggedIn = isLoggedIn;
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);

            // Pre-fill form if member is logged in
            var inquiry = new Inquiry();
            if (isLoggedIn)
            {
                inquiry.Name = SessionHelper.GetMemberName(HttpContext.Session);
                inquiry.Email = SessionHelper.GetMemberEmail(HttpContext.Session);
            }

            return View(inquiry);
        }

        // POST: Inquiries/Create
        /// <summary>
        /// Process inquiry submission
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inquiry inquiry)
        {
            // Set inquiry date
            inquiry.InquiryDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Inquiries.Add(inquiry);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you for your inquiry! We will get back to you soon.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.IsLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            return View(inquiry);
        }

        // GET: Inquiries/ThankYou
        /// <summary>
        /// Display thank you page after inquiry submission
        /// </summary>
        public IActionResult ThankYou()
        {
            ViewBag.IsLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            return View();
        }
    }
}
