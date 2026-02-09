using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Smart_Event_Management_and_Ticketing_System.Helpers;

namespace Smart_Event_Management_and_Ticketing_System.Filters
{
    /// <summary>
    /// Custom authorization filter to restrict access to admin users only
    /// Redirects to home page if user is not an admin
    /// </summary>
    public class AdminAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if user is logged in
            if (!SessionHelper.IsLoggedIn(context.HttpContext.Session))
            {
                // Redirect to login page if not logged in
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Check if user is an admin
            if (!SessionHelper.IsAdmin(context.HttpContext.Session))
            {
                // Redirect to home page if not an admin
                context.HttpContext.Session.SetString("ErrorMessage", "Access denied. Admin privileges required.");
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
