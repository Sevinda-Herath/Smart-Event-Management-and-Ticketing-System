# ?? Fix: Admin Logout Button Missing

## Problem
The logout button was not showing in the navigation bar for admin users on some pages.

## Root Cause
The admin controller was setting `ViewBag.IsAdmin` and `ViewBag.MemberName`, but **NOT** setting `ViewBag.IsLoggedIn`.

In the `_Layout.cshtml`, the logout button only displays when:
```razor
@if (ViewBag.IsLoggedIn == true)
```

Since `ViewBag.IsLoggedIn` was not being set in admin actions, it defaulted to `null` or `false`, causing the logout button to not appear.

## Solution
Created a helper method in `AdminController` to consistently set all ViewBag values:

```csharp
private void SetViewBagData()
{
    ViewBag.IsLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
    ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
    ViewBag.IsAdmin = SessionHelper.IsAdmin(HttpContext.Session);
}
```

Then replaced all individual ViewBag assignments with calls to this method in all admin actions:
- ? Index
- ? Events
- ? CreateEvent (GET & POST)
- ? EditEvent (GET & POST)
- ? DeleteEvent (GET)
- ? Bookings
- ? Inquiries
- ? Members

## Result
? Logout button now appears correctly for admin users on all pages  
? Admin badge still displays correctly  
? All navigation items work properly  
? Code is cleaner and more maintainable  

## Testing
1. Login as admin: `admin@culturalcouncil.org` / `admin123`
2. Navigate to each admin page:
   - Dashboard
   - Manage Events
   - Create Event
   - Edit Event
   - All Bookings
   - Inquiries
   - Members
3. ? Verify logout button appears on all pages
4. ? Verify "ADMIN" badge shows next to username
5. Click logout to confirm it works

## Files Modified
- `Controllers/AdminController.cs` - Added SetViewBagData() helper and updated all actions

---

**Fix applied successfully! Build successful.** ?
