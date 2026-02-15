using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Event_Management_and_Ticketing_System.Data;
using Smart_Event_Management_and_Ticketing_System.Helpers;
using Smart_Event_Management_and_Ticketing_System.Models;
using System.Diagnostics;

namespace Smart_Event_Management_and_Ticketing_System.Controllers
{
    /// <summary>
    /// Home Controller - Handles the main landing page and general information
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Display home page with upcoming events and cultural council information
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Get upcoming events (next 6 events) with bookings to calculate available seats
            var upcomingEvents = await _context.Events
                .Include(e => e.Bookings)
                .Where(e => e.EventDate >= DateTime.Now)
                .OrderBy(e => e.EventDate)
                .Take(6)
                .ToListAsync();

            // Pass member info to view for conditional display
            ViewBag.IsLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            ViewBag.IsAdmin = SessionHelper.IsAdmin(HttpContext.Session);

            return View(upcomingEvents);
        }

        /// <summary>
        /// Display About/Privacy page
        /// </summary>
        public IActionResult Privacy()
        {
            ViewBag.IsLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
            ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
            ViewBag.IsAdmin = SessionHelper.IsAdmin(HttpContext.Session);
            return View();
        }

        /// <summary>
        /// UTILITY: Reset database and add admin user
        /// Access at: /Home/ResetDatabase
        /// </summary>
        public async Task<IActionResult> ResetDatabase()
        {
            try
            {
                // Delete all data
                _context.Reviews.RemoveRange(_context.Reviews);
                _context.Bookings.RemoveRange(_context.Bookings);
                _context.Inquiries.RemoveRange(_context.Inquiries);
                _context.Members.RemoveRange(_context.Members);
                await _context.SaveChangesAsync();

                // Add admin with plain text password
                var admin = new Member
                {
                    FullName = "Administrator",
                    Email = "admin@culturalcouncil.org",
                    Password = "admin123",
                    Role = "Admin",
                    PreferredCategory = null
                };
                _context.Members.Add(admin);
                await _context.SaveChangesAsync();

                ViewBag.Success = true;
                ViewBag.Message = "Database reset successfully! Admin user created.";
                ViewBag.Email = "admin@culturalcouncil.org";
                ViewBag.Password = "admin123";
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.Message = $"Error: {ex.Message}";
            }

            return View();
        }

        /// <summary>
        /// Error handling page
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
