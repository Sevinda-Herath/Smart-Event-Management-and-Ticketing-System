# Smart Event Management System - Complete Documentation

## Table of Contents
1. [System Overview](#system-overview)
2. [Admin Guide](#admin-guide)
3. [Features Implemented](#features-implemented)
4. [Security & Password Hashing](#security--password-hashing)
5. [User Management](#user-management)
6. [Reviews & Ratings](#reviews--ratings)
7. [Guest vs Member Access](#guest-vs-member-access)
8. [Technical Details](#technical-details)
9. [Setup & Configuration](#setup--configuration)
10. [Bug Fixes](#bug-fixes)

---

## System Overview

**Metropolitan Cultural Council Event Management System**

A comprehensive ASP.NET Core MVC application for managing cultural events, bookings, and member reviews.

### Key Technologies
- **.NET 10**
- **ASP.NET Core MVC** (Not Razor Pages)
- **Entity Framework Core**
- **SQL Server**
- **Bootstrap 5**
- **Font Awesome**
- **Session-based Authentication**

### User Roles
- **Admin** - Full system management
- **Member** - Book events, write reviews
- **Guest** - Browse events (limited)

---

## Admin Guide

### Admin Credentials
**Email:** admin@culturalcouncil.org  
**Password:** admin123  
*(Hashed with PBKDF2)*

### Admin Dashboard
Access: `/Admin/Index`

**Statistics Displayed:**
- Total Events
- Total Bookings
- Total Members
- Total Inquiries
- Total Reviews
- Upcoming Events List

### Admin Features

#### 1. Manage Events (`/Admin/Events`)
- ? Create new events
- ? Edit existing events
- ? Delete events (cascade deletes bookings & reviews)
- ? View booking & review counts

#### 2. All Bookings (`/Admin/Bookings`)
- ? View all bookings system-wide
- ? Filter by event or member
- ? See member details (name, email)
- ? View statistics (total tickets, unique members)

#### 3. Inquiries (`/Admin/Inquiries`)
- ? View all contact form submissions
- ? See sender details and messages

#### 4. Manage Members (`/Admin/Members`)
- ? View all registered members
- ? Edit member information
- ? Change member passwords (hashed)
- ? Delete member accounts (cascade delete)
- ? View member statistics (bookings, reviews)
- ? Protection: Cannot edit/delete admin users

### Admin Navigation Bar
```
Home | Dashboard | Manage Events | All Bookings | Inquiries | Members | [ADMIN Badge] | Logout
```

---

## Features Implemented

### Core Features

#### Event Management
- Create, edit, delete events
- Categories: Music, Theater, Dance, Art, Workshop, Festival, Conference
- Track available seats and bookings
- Full event details (date, time, venue, price, description)

#### Booking System
- Members can book multiple tickets
- Seat availability tracking
- Booking history for members
- Admin can view all bookings
- Cancel bookings functionality

#### Review System
- Members can review events they've attended
- 1-5 star ratings
- Text comments (max 500 characters)
- One review per member per event
- **Edit own reviews**
- **Delete own reviews**
- **Filter reviews by star rating**
- **Search reviews by keywords**
- Average rating display

#### Member Management
- Registration with email validation
- Login/Logout with sessions
- Profile preferences (preferred category)
- Admin can edit member details
- Admin can reset member passwords
- Admin can delete members

#### Inquiry System
- Contact form for guests and members
- Admin can view all inquiries
- Captures name, email, message, and date

---

## Security & Password Hashing

### Implementation
**Algorithm:** PBKDF2 with HMACSHA256  
**Iterations:** 100,000  
**Salt:** Random 128-bit per password  
**Hash Size:** 256-bit  

### Storage Format
```
{Base64Salt}.{Base64Hash}
Example: kR9Xp2vN8FqY1zLmT3wJ5g==.xH4KpL8+mN9vQr2sT7uVwYz1aB3cD6eF
```

### Security Features
? Never store plain text passwords  
? Unique salt per password  
? One-way hashing (cannot reverse)  
? Industry-standard algorithm  
? High iteration count slows brute force  

### Password Operations

#### Registration
```
User Password ? Generate Salt ? Hash with PBKDF2 ? Store Salt+Hash
```

#### Login
```
User Password ? Extract Salt ? Hash with Same Salt ? Compare Hashes ? Grant/Deny Access
```

#### Admin Password Reset
- Admin enters new password in "New Password" field
- Leave empty to keep current password
- New password is automatically hashed before storage
- Member can immediately login with new password

### Setup Database with Hashed Passwords

**Method 1: Automatic (Easiest)**
1. Navigate to: `https://localhost:7227/Home/ResetDatabase`
2. Automatically deletes all data and creates admin with hashed password
3. Login with: admin@culturalcouncil.org / admin123

**Method 2: Manual SQL**
```sql
-- Delete all data
DELETE FROM Reviews;
DELETE FROM Bookings;
DELETE FROM Inquiries;
DELETE FROM Members;
DBCC CHECKIDENT ('Members', RESEED, 0);

-- Add admin (get hash from /Home/GenerateHash)
INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES ('Administrator', 'admin@culturalcouncil.org', 
        'YOUR_GENERATED_HASH', NULL, 'Admin');
```

---

## User Management

### Member Registration
- Required: Full Name, Email, Password
- Optional: Preferred Category
- Password automatically hashed
- Email must be unique
- Role automatically set to "Member"

### Admin Edit Member
**Features:**
- Edit name, email, preferred category
- Optional password change (leave empty to keep current)
- New passwords are hashed automatically
- Cannot edit admin users
- Cannot change member to admin role (security)

### Admin Delete Member
**Cascade Deletion:**
- Deletes member account
- Deletes all member's bookings
- Deletes all member's reviews
- Shows confirmation with impact details
- Cannot delete admin users

### Member Capabilities
| Action | Member | Admin |
|--------|--------|-------|
| Browse Events | ? Full details | ? |
| Book Tickets | ? | ? |
| Cancel Bookings | ? Own only | ? |
| Write Reviews | ? After booking | ? |
| Edit Own Reviews | ? | ? |
| Delete Own Reviews | ? | ? |
| View Own Bookings | ? | ? |

---

## Reviews & Ratings

### Review Features

#### Write Review
- Only for events you've booked
- One review per member per event
- 1-5 star rating
- Text comment (max 500 characters)
- Can edit later
- Can delete later

#### Edit Own Review
- "You" badge shown on your reviews
- Edit button appears only on your reviews
- Update rating or comment
- Pre-filled with current data
- Changes take effect immediately

#### Delete Own Review
- Delete button on your reviews
- Confirmation page shows review details
- Permanent deletion warning
- Cannot be undone

#### Search & Filter Reviews
**Filter by Stars:**
- All Ratings
- 5 Stars
- 4 Stars
- 3 Stars
- 2 Stars
- 1 Star

**Search by Keywords:**
- Search in review comments
- Search in reviewer names
- Case-insensitive
- Partial matching

**Combined Filtering:**
- Use both star filter and keyword search
- Example: "4-star reviews mentioning 'performance'"

**Display Features:**
- Average rating shown in header
- Filtered count: "Showing 8 of 15 reviews"
- Clear filters button
- No results message

---

## Guest vs Member Access

### Guest Access (Not Logged In)

**Can Do:**
- Browse events (limited info)
- Filter by: Category, Date, Location
- See event name, date, venue, availability status
- View contact form
- Register for account
- Login to existing account

**Cannot Do:**
- See event prices ?
- See available seat count ?
- Filter by price ?
- Search events by keywords ?
- Book tickets ?
- Write reviews ?
- View member bookings ?

**Visual Indicators:**
- "Members Only" badge on search filters
- Lock icons (??) on restricted features
- "Register to see full details" messages
- Disabled/greyed out premium filters

### Member Access (Logged In)

**Additional Features:**
- ? See full event details (price, seats)
- ? Use all search filters (including price)
- ? Keyword search events
- ? Book tickets
- ? View own bookings
- ? Cancel bookings
- ? Write reviews (after attending)
- ? Edit/delete own reviews
- ? Filter and search reviews

### Admin Access

**Everything Members Can + Additional:**
- ? Admin dashboard with statistics
- ? Create/edit/delete events
- ? View all bookings (with member details)
- ? View all inquiries
- ? Manage members (edit/delete)
- ? Reset member passwords
- ? Cannot be edited/deleted by other admins

---

## Technical Details

### Architecture

**Pattern:** ASP.NET Core MVC  
**Database:** SQL Server with Entity Framework Core  
**Authentication:** Custom session-based (no Identity)  

**Key Components:**
- Controllers: Handle requests and business logic
- Models: Entity Framework entities
- Views: Razor views (.cshtml)
- Helpers: Utility classes (SessionHelper, PasswordHasher)
- Filters: Custom authorization attributes

### Database Schema

**Tables:**
1. **Members** - User accounts (members and admins)
2. **Events** - Cultural events
3. **Bookings** - Ticket bookings
4. **Reviews** - Event reviews and ratings
5. **Inquiries** - Contact form submissions

**Key Relationships:**
- Member ? Bookings (One-to-Many)
- Member ? Reviews (One-to-Many)
- Event ? Bookings (One-to-Many)
- Event ? Reviews (One-to-Many)

### Session Management

**Session Keys:**
- `MemberId` - User ID
- `MemberName` - Full name
- `MemberEmail` - Email address
- `MemberRole` - "Admin" or "Member"

**SessionHelper Methods:**
```csharp
SetMemberSession() - Store user data
IsLoggedIn() - Check if user logged in
IsAdmin() - Check if user is admin
GetMemberId() - Retrieve user ID
GetMemberName() - Retrieve user name
ClearSession() - Logout user
```

### Authorization

**Custom Filters:**
1. **MemberAuthorizationAttribute** - Requires any logged-in user
2. **AdminAuthorizationAttribute** - Requires admin role

**Applied To:**
- BookingsController ? [MemberAuthorization]
- ReviewsController ? [MemberAuthorization]
- AdminController ? [AdminAuthorization]

### Computed Properties

**Event Model:**
```csharp
AvailableSeats - TotalSeats - Booked seats
IsFull - True if no available seats
```

---

## Setup & Configuration

### Initial Setup

#### 1. Database Creation
```sql
CREATE DATABASE EventManagementDB;
```

#### 2. Run Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### 3. Create Admin User

**Option A: Automatic (Recommended)**
```
Navigate to: https://localhost:7227/Home/ResetDatabase
```

**Option B: Manual SQL**
```sql
-- Generate hash first at /Home/GenerateHash
INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES ('Administrator', 'admin@culturalcouncil.org', 
        'GENERATED_HASH_HERE', NULL, 'Admin');
```

### Configuration Files

**appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=EventManagementDB;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

**Program.cs:**
- Session configuration
- DbContext registration
- MVC routing setup

### Running the Application

```bash
# Build
dotnet build

# Run
dotnet run

# Access
https://localhost:7227
```

### Test Data

**Create Sample Events:**
1. Login as admin
2. Go to Manage Events
3. Click "Create New Event"
4. Fill in details
5. Save

**Register Test Member:**
1. Go to Register
2. Create account: test@test.com / test123
3. Password will be hashed automatically

---

## Bug Fixes

### 1. Fixed: Sticky Footer
**Problem:** Footer appeared in middle of page on short content pages

**Solution:**
- Added Flexbox layout to body
- `d-flex flex-column min-vh-100` on body
- `flex-fill` on main content
- `mt-auto` on footer

**CSS:**
```css
body {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}
.flex-fill {
  flex: 1 0 auto;
}
footer {
  margin-top: auto;
}
```

### 2. Fixed: Navbar Shows Wrong Buttons for Admin in Bookings
**Problem:** Admin saw "Login/Register" in bookings section instead of "Logout"

**Solution:**
- Added `SetViewBagData()` helper to BookingsController
- Sets `ViewBag.IsLoggedIn` and `ViewBag.IsAdmin`
- Called in all booking actions

**Code:**
```csharp
private void SetViewBagData()
{
    ViewBag.IsLoggedIn = SessionHelper.IsLoggedIn(HttpContext.Session);
    ViewBag.MemberName = SessionHelper.GetMemberName(HttpContext.Session);
    ViewBag.IsAdmin = SessionHelper.IsAdmin(HttpContext.Session);
}
```

### 3. Fixed: Star Emoji Icons Show as Question Marks
**Problem:** Star emojis (?) in dropdowns showed as "???"

**Solution:**
- Removed emoji stars from rating dropdowns
- Used text: "5 Stars - Excellent" instead
- Font Awesome icons still work in reviews display

**Changed:**
```razor
<!-- Before -->
<option value="5">????? Excellent</option>

<!-- After -->
<option value="5">5 Stars - Excellent</option>
```

---

## Quick Reference

### URLs

| Page | URL | Access |
|------|-----|--------|
| Home | `/` | Everyone |
| Browse Events | `/Events/Index` | Everyone |
| Event Details | `/Events/Details/{id}` | Everyone |
| Register | `/Account/Register` | Guest only |
| Login | `/Account/Login` | Guest only |
| My Bookings | `/Bookings/Index` | Members |
| Book Tickets | `/Bookings/Create?eventId={id}` | Members |
| Write Review | `/Reviews/Create?eventId={id}` | Members |
| Edit Review | `/Reviews/Edit/{id}` | Review author |
| Delete Review | `/Reviews/Delete/{id}` | Review author |
| Admin Dashboard | `/Admin/Index` | Admin only |
| Manage Events | `/Admin/Events` | Admin only |
| All Bookings | `/Admin/Bookings` | Admin only |
| Inquiries | `/Admin/Inquiries` | Admin only |
| Manage Members | `/Admin/Members` | Admin only |
| Edit Member | `/Admin/EditMember/{id}` | Admin only |
| Delete Member | `/Admin/DeleteMember/{id}` | Admin only |

### Default Credentials

**Admin:**
- Email: admin@culturalcouncil.org
- Password: admin123 (hashed)

### Event Categories

1. Music
2. Theater
3. Dance
4. Art
5. Workshop
6. Festival
7. Conference

### Seat Types

1. Standard
2. VIP
3. Premium
4. General Admission

---

## Features Summary

### ? Completed Features

#### User Management
- ? Registration with validation
- ? Login/Logout with sessions
- ? Password hashing (PBKDF2)
- ? Admin vs Member roles
- ? Admin can edit members
- ? Admin can delete members
- ? Admin can reset passwords (hashed)

#### Event Management
- ? Create events (admin)
- ? Edit events (admin)
- ? Delete events with cascade (admin)
- ? Browse events (all users)
- ? Search & filter events
- ? Guest vs member views (different info)
- ? Category-based browsing

#### Booking System
- ? Book tickets (members)
- ? Multiple ticket quantity
- ? Seat availability tracking
- ? View own bookings (members)
- ? Cancel bookings (members)
- ? View all bookings (admin)
- ? Filter bookings by event/member

#### Review System
- ? Write reviews (members, after booking)
- ? 1-5 star ratings
- ? Text comments
- ? One review per member per event
- ? Edit own reviews
- ? Delete own reviews
- ? Filter reviews by stars
- ? Search reviews by keywords
- ? Average rating display
- ? "You" badge on own reviews

#### Admin Dashboard
- ? Statistics display
- ? Quick action buttons
- ? Upcoming events list
- ? System-wide overview

#### Guest Access Controls
- ? Limited event information
- ? Partial search filters (category, date, location)
- ? Disabled premium filters (price, keywords)
- ? Registration prompts
- ? Lock icons on restricted features

#### UI/UX
- ? Responsive design (Bootstrap 5)
- ? Font Awesome icons
- ? Sticky footer (always at bottom)
- ? Success/Error messages (TempData)
- ? Confirmation dialogs
- ? Admin navigation menu
- ? Member navigation menu
- ? Guest navigation menu

#### Security
- ? Password hashing (100,000 iterations)
- ? Session-based authentication
- ? Role-based authorization
- ? Admin protection (can't edit/delete admins)
- ? Review ownership verification
- ? Booking ownership verification

---

## Testing Guide

### Test Scenarios

#### 1. Guest User Flow
```
1. Visit home page
2. Browse events (limited info shown)
3. Try to filter by price ? Disabled
4. Try to search keywords ? Disabled
5. Filter by category ? Works
6. Click event details ? See limited info
7. Try to book ? Redirected to login
8. Click register ? Create account
```

#### 2. Member User Flow
```
1. Register account
2. Login successfully
3. Browse events (full details)
4. Use all search filters
5. Book tickets for event
6. View "My Bookings"
7. Write review for booked event
8. Edit own review
9. Delete own review
10. Logout
```

#### 3. Admin User Flow
```
1. Login as admin
2. View dashboard statistics
3. Create new event
4. Edit existing event
5. View all bookings
6. Filter bookings by event
7. View all inquiries
8. View all members
9. Edit member details
10. Change member password
11. Delete member (with confirmation)
12. Delete event (cascade delete)
13. Logout
```

#### 4. Password Hashing Test
```
1. Register new user with password "test123"
2. Check database: SELECT Password FROM Members WHERE Email = 'test@test.com'
3. Verify: Password is long hash string (not "test123")
4. Login with "test123" ? Success
5. Try wrong password ? Fail
6. Admin changes password to "newpass"
7. Check database: Password is new hash
8. Login with "newpass" ? Success
```

#### 5. Review Features Test
```
1. Login as member
2. Book event ticket
3. Write review with 5 stars
4. View event details ? See your review with "You" badge
5. Click Edit ? Change to 4 stars
6. Save ? Review updated
7. Filter reviews by 4 stars ? Your review appears
8. Search reviews by keyword ? Test search
9. Click Delete ? Confirm deletion
10. Review removed from list
```

---

## Troubleshooting

### Common Issues

**Can't login after enabling password hashing?**
- Old passwords are plain text
- New system expects hashed passwords
- Solution: Run `/Home/ResetDatabase` to recreate users

**Footer not at bottom?**
- Clear browser cache
- Verify CSS loads: `/css/site.css`
- Check body has classes: `d-flex flex-column min-vh-100`

**Admin sees Login/Register in some pages?**
- Controller missing `SetViewBagData()` call
- Add ViewBag settings to all actions

**Star emojis show as "???"?**
- Use text instead: "5 Stars"
- Font Awesome icons work fine: `<i class="fas fa-star">`

**Can't edit/delete own reviews?**
- Must be logged in
- Must be the review author
- Check `currentMemberId` matches `review.MemberId`

**Cascade deletion not working?**
- Check Entity Framework relationships
- Include `.Include()` in queries
- Use `RemoveRange()` for collections

---

## Production Considerations

?? **This is a coursework project. For production:**

1. **Security:**
   - Keep password hashing (already implemented)
   - Add HTTPS enforcement
   - Implement CSRF protection (already using [ValidateAntiForgeryToken])
   - Add rate limiting
   - Implement email verification
   - Add account lockout after failed attempts

2. **Remove Temporary Utilities:**
   - Delete `/Home/ResetDatabase` endpoint
   - Delete `/Home/GenerateHash` endpoint
   - Remove these views

3. **Performance:**
   - Add caching (Redis)
   - Implement pagination for large lists
   - Add database indexes
   - Optimize queries

4. **Features:**
   - Add password recovery (forgot password)
   - Add email notifications
   - Add payment integration
   - Add event images
   - Add event capacity alerts

5. **Monitoring:**
   - Add logging (Serilog)
   - Add error tracking
   - Add analytics
   - Add health checks

---

## Credits & License

**Project:** Smart Event Management and Ticketing System  
**Purpose:** Academic Coursework  
**Technology:** ASP.NET Core MVC (.NET 10)  
**Author:** [Your Name]  
**Repository:** https://github.com/Sevinda-Herath/Smart-Event-Management-and-Ticketing-System  

---

## Appendix

### Database Connection String Template
```json
"Server=(localdb)\\mssqllocaldb;Database=EventManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
```

### Common SQL Queries

**View all members with their stats:**
```sql
SELECT 
    m.MemberId,
    m.FullName,
    m.Email,
    m.Role,
    COUNT(DISTINCT b.BookingId) AS TotalBookings,
    COUNT(DISTINCT r.ReviewId) AS TotalReviews
FROM Members m
LEFT JOIN Bookings b ON m.MemberId = b.MemberId
LEFT JOIN Reviews r ON m.MemberId = r.MemberId
GROUP BY m.MemberId, m.FullName, m.Email, m.Role
ORDER BY m.FullName;
```

**View event statistics:**
```sql
SELECT 
    e.EventId,
    e.EventName,
    e.EventDate,
    e.TotalSeats,
    COALESCE(SUM(b.Quantity), 0) AS BookedSeats,
    e.TotalSeats - COALESCE(SUM(b.Quantity), 0) AS AvailableSeats,
    COUNT(DISTINCT r.ReviewId) AS ReviewCount,
    AVG(CAST(r.Rating AS FLOAT)) AS AverageRating
FROM Events e
LEFT JOIN Bookings b ON e.EventId = b.EventId
LEFT JOIN Reviews r ON e.EventId = r.EventId
GROUP BY e.EventId, e.EventName, e.EventDate, e.TotalSeats
ORDER BY e.EventDate;
```

---

**End of Documentation**

*Last Updated: 2025*  
*Version: 1.0*  
*Status: Complete*
