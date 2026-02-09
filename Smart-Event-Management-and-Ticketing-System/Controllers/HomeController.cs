using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Event_Management_and_Ticketing_System.Data;
using Smart_Event_Management_and_Ticketing_System.Models;
using Smart_Event_Management_and_Ticketing_System.Helpers;

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
            // Get upcoming events (next 6 events)
            var upcomingEvents = await _context.Events
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
        /// Error handling page
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
