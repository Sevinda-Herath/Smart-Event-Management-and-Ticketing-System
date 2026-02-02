using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Smart_Event_Management_and_Ticketing_System.Helpers;

namespace Smart_Event_Management_and_Ticketing_System.Filters
{
    /// <summary>
    /// Custom authorization filter to restrict access to logged-in members only
    /// Redirects to login page if user is not authenticated
    /// </summary>
    public class MemberAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if member is logged in using session
            if (!SessionHelper.IsLoggedIn(context.HttpContext.Session))
            {
                // Redirect to login page if not logged in
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
            }

            base.OnActionExecuting(context);
        }
    }
}
