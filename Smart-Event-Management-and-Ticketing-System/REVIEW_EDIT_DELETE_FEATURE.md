# ? Review Edit & Delete Feature Added

## Feature: Users Can Edit and Delete Their Own Reviews

Logged-in members can now **edit** or **delete** reviews they have written for events they've attended.

---

## ?? Features Implemented

### 1. **Edit Own Review**
- ? Edit rating (1-5 stars)
- ? Edit comment text
- ? Form pre-filled with current review
- ? Only review author can edit
- ? Success message after update

### 2. **Delete Own Review**
- ? Confirmation page before deletion
- ? Shows full review details
- ? Only review author can delete
- ? Permanent deletion warning
- ? Success message after deletion

### 3. **Visual Indicators**
- ? **"You" badge** on own reviews
- ? **Edit/Delete buttons** appear only on own reviews
- ? Buttons styled with icons
- ? Grouped button design

### 4. **Security**
- ? Only logged-in users can edit/delete
- ? Users can only modify their own reviews
- ? Backend verification of ownership
- ? Authorization checks

---

## ?? Where to Find It

**Location:** Event Details Page ? Reviews Section

When viewing reviews on any event, if you're logged in and see your own review, you'll see:
- A **"You"** badge next to your name
- **Edit** and **Delete** buttons

---

## ?? Visual Design

### Your Own Review (With Actions)
```
??????????????????????????????????????????
? John Doe [You]                     Edit?
? ?????                          Delete?
? Dec 15, 2025                           ?
?                                        ?
? This event was amazing! Great         ?
? performance and wonderful venue.       ?
??????????????????????????????????????????
```

### Other User's Review (No Actions)
```
??????????????????????????????????????????
? Jane Smith                             ?
? ????                                 ?
? Dec 14, 2025                           ?
?                                        ?
? Really enjoyed the event!              ?
??????????????????????????????????????????
```

---

## ?? Technical Implementation

### Controller Actions (`ReviewsController.cs`)

#### Delete (GET)
```csharp
public async Task<IActionResult> Delete(int? id)
{
    var memberId = SessionHelper.GetMemberId(HttpContext.Session);
    var review = await _context.Reviews
        .Include(r => r.Event)
        .Include(r => r.Member)
        .FirstOrDefaultAsync(r => r.ReviewId == id && r.MemberId == memberId);
    
    // Only author can access
    if (review == null)
    {
        TempData["ErrorMessage"] = "Review not found or you don't have permission.";
        return RedirectToAction("Index", "Events");
    }
    
    return View(review);
}
```

#### Delete (POST)
```csharp
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var memberId = SessionHelper.GetMemberId(HttpContext.Session);
    var review = await _context.Reviews
        .FirstOrDefaultAsync(r => r.ReviewId == id && r.MemberId == memberId);
    
    if (review == null)
    {
        TempData["ErrorMessage"] = "Review not found or you don't have permission.";
        return RedirectToAction("Index", "Events");
    }
    
    _context.Reviews.Remove(review);
    await _context.SaveChangesAsync();
    
    TempData["SuccessMessage"] = "Your review has been deleted successfully.";
    return RedirectToAction("Details", "Events", new { id = eventId });
}
```

### View Changes (`Events/Details.cshtml`)

#### Ownership Check
```razor
var currentMemberId = ViewBag.IsLoggedIn == true ? 
    SessionHelper.GetMemberId(Context.Session) : null;
```

#### Display Edit/Delete Buttons
```razor
@if (currentMemberId.HasValue && review.MemberId == currentMemberId.Value)
{
    <span class="badge bg-primary ms-1">You</span>
    
    <div class="btn-group btn-group-sm mt-1">
        <a asp-controller="Reviews" asp-action="Edit" asp-route-id="@review.ReviewId" 
           class="btn btn-sm btn-outline-primary">
            <i class="fas fa-edit"></i>
        </a>
        <a asp-controller="Reviews" asp-action="Delete" asp-route-id="@review.ReviewId" 
           class="btn btn-sm btn-outline-danger">
            <i class="fas fa-trash"></i>
        </a>
    </div>
}
```

---

## ?? Testing Scenarios

### Test 1: Edit Your Own Review
1. **Login** as a member who has reviewed an event
2. Go to the **event details** page
3. ? See "You" badge on your review
4. ? See Edit and Delete buttons
5. Click **Edit** button
6. Modify rating or comment
7. Click **"Update Review"**
8. ? Success message: "Review updated successfully!"
9. ? See updated review on event page

### Test 2: Delete Your Own Review
1. Login and find your review
2. Click **Delete** button
3. ? See confirmation page with review details
4. ? See warning about permanent deletion
5. Click **"Yes, Delete My Review"**
6. ? Success message: "Your review has been deleted successfully."
7. ? Review no longer appears on event page

### Test 3: Security - Can't Edit Others' Reviews
1. Login as Member A
2. Try to access: `/Reviews/Edit/{reviewIdByMemberB}`
3. ? Shows 404 Not Found or redirects
4. ? Cannot edit another user's review

### Test 4: Security - Can't Delete Others' Reviews
1. Login as Member A
2. Try to access: `/Reviews/Delete/{reviewIdByMemberB}`
3. ? Error message: "Review not found or you don't have permission"
4. ? Redirected to events page

### Test 5: Guest Can't See Action Buttons
1. **Logout** (become guest)
2. View event with reviews
3. ? No "You" badges shown
4. ? No Edit/Delete buttons visible
5. ? Only view reviews

---

## ?? User Flow

### Edit Review Flow
```
User views event details
    ?
Sees own review with [You] badge
    ?
Clicks "Edit" button (??)
    ?
Edit form appears (pre-filled)
    ?
Changes rating or comment
    ?
Clicks "Update Review"
    ?
Success message ? Back to event page
    ?
Updated review visible
```

### Delete Review Flow
```
User views event details
    ?
Sees own review with [You] badge
    ?
Clicks "Delete" button (???)
    ?
Confirmation page shows
    ?
Reviews deletion details
    ?
Clicks "Yes, Delete My Review"
    ?
Review deleted from database
    ?
Success message ? Back to event page
    ?
Review no longer visible
```

---

## ?? Button Styles

### Edit Button
```html
<a class="btn btn-sm btn-outline-primary">
    <i class="fas fa-edit"></i>
</a>
```
- **Color:** Blue outline
- **Icon:** Pencil/edit icon
- **Size:** Small
- **Style:** Outline (not filled)

### Delete Button
```html
<a class="btn btn-sm btn-outline-danger">
    <i class="fas fa-trash"></i>
</a>
```
- **Color:** Red outline
- **Icon:** Trash can
- **Size:** Small
- **Style:** Outline (not filled)

### Button Group
```html
<div class="btn-group btn-group-sm">
    <!-- Buttons grouped together -->
</div>
```
- Buttons appear side-by-side
- Visually connected
- Compact layout

---

## ?? Security Features

### 1. **Ownership Verification**
```csharp
.FirstOrDefaultAsync(r => r.ReviewId == id && r.MemberId == memberId)
```
- SQL query includes ownership check
- Only fetches review if user owns it
- Database-level security

### 2. **Authorization Attribute**
```csharp
[MemberAuthorization]
public class ReviewsController : Controller
```
- Entire controller requires login
- Guests redirected to login page

### 3. **Session Validation**
```csharp
var memberId = SessionHelper.GetMemberId(HttpContext.Session);
if (memberId == null)
{
    return RedirectToAction("Login", "Account");
}
```
- Checks session on every action
- Validates user is logged in

### 4. **Not Found Protection**
```csharp
if (review == null)
{
    TempData["ErrorMessage"] = "Review not found or you don't have permission.";
    return RedirectToAction("Index", "Events");
}
```
- Handles non-existent reviews
- Handles unauthorized access attempts
- User-friendly error messages

---

## ?? Delete Confirmation Page

### Layout
```
???????????????????????????????????????????
? ?? Delete Review                        ?
???????????????????????????????????????????
? ?? Warning: This action cannot be undone?
?                                         ?
? Event: Summer Music Festival            ?
? Reviewer: John Doe                      ?
? Rating: ????? (5 out of 5)          ?
? Comment: "This event was amazing..."    ?
? Submitted: December 15, 2025            ?
?                                         ?
? [Yes, Delete My Review]                 ?
? [No, Keep My Review]                    ?
???????????????????????????????????????????
```

### Features
- Shows complete review details
- Warning about permanent deletion
- Clear action buttons
- Red danger theme
- Cancel option prominent

---

## ?? User Benefits

### For Members:
1. **Fix Mistakes** - Correct typos or errors
2. **Update Opinion** - Change rating after reflection
3. **Remove Reviews** - Delete if no longer relevant
4. **Full Control** - Manage own content

### For Event Quality:
1. **Accurate Feedback** - Members can update reviews
2. **Current Opinions** - Reviews stay up-to-date
3. **User Engagement** - More interaction with platform

---

## ?? For Coursework Presentation

### Demo Flow:

1. **Show Event with Reviews**
   - "Multiple reviews from different users"

2. **Login as Review Author**
   - "When I login, I see my own review"
   - Point out **"You"** badge

3. **Show Action Buttons**
   - "Notice Edit and Delete buttons appear"
   - "Only on MY review, not others'"

4. **Edit Review**
   - Click Edit
   - Change rating from 5 to 4 stars
   - Update comment
   - Save
   - "Review updated successfully!"

5. **Show Security**
   - "Can't edit other users' reviews"
   - "Buttons don't appear on their reviews"

6. **Delete Review**
   - Click Delete
   - Show confirmation page
   - "Warning about permanent deletion"
   - Confirm
   - "Review deleted!"

### Key Points:
- ? **User control** over own content
- ? **Security** - can only edit/delete own reviews
- ? **Confirmation** prevents accidental deletion
- ? **Visual feedback** - "You" badge, buttons
- ? **Professional UX** - clear, intuitive

---

## ? Feature Complete

? **Edit own review** functionality  
? **Delete own review** functionality  
? **"You" badge** on own reviews  
? **Edit/Delete buttons** (author only)  
? **Confirmation page** for deletion  
? **Security checks** (ownership verification)  
? **Success messages** for all actions  
? **Visual indicators** (icons, badges)  
? **Build successful**  

---

## ?? Files Modified/Created

### Modified Files:
1. **Controllers/ReviewsController.cs**
   - Added `Delete` (GET) action
   - Added `DeleteConfirmed` (POST) action

2. **Views/Events/Details.cshtml**
   - Added ownership check
   - Added "You" badge
   - Added Edit/Delete buttons

### New Files:
3. **Views/Reviews/Delete.cshtml**
   - Delete confirmation page

---

**Ready to test! Login as a member who has written a review and try editing or deleting it.** ??

Now members have full control over their own reviews! ??????
