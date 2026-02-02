using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Event_Management_and_Ticketing_System.Data;
using Smart_Event_Management_and_Ticketing_System.Models;
using Smart_Event_Management_and_Ticketing_System.Helpers;

namespace Smart_Event_Management_and_Ticketing_System.Controllers
{
    /// <summary>
    /// Account Controller - Handles member registration, login, and logout
    /// Uses simple session-based authentication (no ASP.NET Identity)
    /// </summary>
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        /// <summary>
        /// Display registration form
        /// </summary>
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        /// <summary>
        /// Process member registration
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Member member)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingMember = await _context.Members
                    .FirstOrDefaultAsync(m => m.Email == member.Email);

                if (existingMember != null)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(member);
                }

                // In a real application, you should hash the password
                // For coursework simplicity, we'll store it directly (NOT recommended for production)
                _context.Members.Add(member);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }

            return View(member);
        }

        // GET: Account/Login
        /// <summary>
        /// Display login form
        /// </summary>
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        /// <summary>
        /// Process member login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            // Find member by email and password
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Email == email && m.Password == password);

            if (member == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            // Set session data for authenticated member
            SessionHelper.SetMemberSession(HttpContext.Session, member.MemberId, member.FullName, member.Email);

            TempData["SuccessMessage"] = $"Welcome back, {member.FullName}!";

            // Redirect to return URL or home page
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Logout
        /// <summary>
        /// Process member logout
        /// </summary>
        public IActionResult Logout()
        {
            // Clear session data
            SessionHelper.ClearSession(HttpContext.Session);

            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}
