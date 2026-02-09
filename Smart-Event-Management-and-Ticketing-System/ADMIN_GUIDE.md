# Admin Functionality Guide

## Admin User Created

An admin user has been successfully added to the system with the following credentials:

**Email:** admin@culturalcouncil.org  
**Password:** admin123

---

## Admin Features

### 1. Admin Dashboard
- View statistics (total events, bookings, members, inquiries, reviews)
- Quick action buttons
- View upcoming events

**URL:** `/Admin/Index`

### 2. Manage Events
- **Create new events** with full details
- **Edit existing events** (name, category, date, venue, price, seats, description)
- **Delete events** (with confirmation - also removes associated bookings and reviews)
- View all events with booking and review counts

**URL:** `/Admin/Events`

### 3. View All Bookings
- See all bookings across the system
- View member details (name and email) for each booking
- Filter bookings by:
  - Specific event
  - Specific member
- View summary statistics (total bookings, total tickets, unique members)

**URL:** `/Admin/Bookings`

### 4. View All Inquiries
- See all contact form submissions
- View sender name, email, message, and submission date

**URL:** `/Admin/Inquiries`

### 5. Manage Members
- See all registered members
- View member statistics (number of bookings and reviews)
- **Edit member information** (name, email, password, preferred category)
- **Delete member accounts** (also removes their bookings and reviews)
- Quick link to view member's bookings
- **Protection:** Cannot edit or delete admin users

**URL:** `/Admin/Members`

---

## How to Access Admin Panel

### Method 1: Direct Login
1. Go to the login page
2. Enter admin credentials:
   - **Email:** admin@culturalcouncil.org
   - **Password:** admin123
3. You'll be automatically redirected to the Admin Dashboard

### Method 2: SQL Manual Insert
If you need to create additional admin users, run this SQL:

```sql
INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES ('Your Name', 'youremail@example.com', 'yourpassword', NULL, 'Admin');
```

---

## Admin Navigation

When logged in as admin, the navigation menu shows:
- **Dashboard** - Admin home page with statistics
- **Manage Events** - Create, edit, delete events
- **All Bookings** - View all bookings with member details
- **Inquiries** - View all contact messages

An **"ADMIN" badge** is displayed next to your name in the navbar.

---

## Authorization

### Admin-Only Access
The following pages require admin role:
- `/Admin/*` (all admin pages)

### Protected by AdminAuthorizationAttribute
- Non-admin users are redirected to home page
- Non-logged-in users are redirected to login page

---

## Admin Capabilities

| Feature | Admin Can |
|---------|-----------|
| **Events** | Create, Edit, Delete any event |
| **Bookings** | View all bookings with member details (name & email) |
| **Members** | View all member information, Edit members, Delete members |
| **Inquiries** | View all contact form submissions |
| **Statistics** | View system-wide statistics |
| **Events** | Create, Edit, Delete any event |
| **Bookings** | View all bookings with member details (name & email) |
| **Members** | View all member information and their activity |
| **Inquiries** | View all contact form submissions |
| **Statistics** | View system-wide statistics |

---

## Regular Member vs Admin

| Feature | Member | Admin |
|---------|--------|-------|
| View Events | ? Full details | ? Full details |
| Book Tickets | ? | ? (Admin doesn't book) |
| Submit Reviews | ? | ? |
| Create Events | ? | ? |
| Edit Events | ? | ? |
| Delete Events | ? | ? |
| View All Bookings | ? | ? (with member details) |
| View All Members | ? | ? |
| View Inquiries | ? | ? |
| Dashboard | ? | ? |

---

## Important Notes

### Security
- Admin credentials are stored in plain text for coursework simplicity
- In production, implement proper password hashing
- Consider adding email verification for admin accounts

### Role Field
- Default role for new registrations: "Member"
- Admin role must be manually assigned via database
- Role is stored in session after login

### Event Management
- Deleting an event also deletes associated:
  - Bookings
  - Reviews
- Confirmation is required before deletion

### Member Information
- Admins can see member names and emails in booking list
- This helps admins contact members about their bookings
- Useful for event management and customer support

---

## Testing Admin Features

### 1. Login as Admin
```
Email: admin@culturalcouncil.org
Password: admin123
```

### 2. Create a New Event
1. Click "Manage Events" ? "Create New Event"
2. Fill in all fields
3. Click "Create Event"

### 3. View Bookings with Member Details
1. First, register as a regular member and book a ticket
2. Logout and login as admin
3. Click "All Bookings"
4. See member name and email for each booking

### 4. Edit an Event
1. Go to "Manage Events"
2. Click edit icon on any event
3. Modify details
4. Click "Update Event"

### 5. Delete an Event
1. Go to "Manage Events"
2. Click delete icon on any event
3. Review warning message
4. Confirm deletion

### 6. View Inquiries
1. First, use Contact Us form as guest
2. Login as admin
3. Click "Inquiries"
4. See all submitted messages

---

## Database Schema Changes

### Members Table
New column added:
```sql
ALTER TABLE Members ADD Role nvarchar(20) NOT NULL DEFAULT 'Member';
```

Values:
- `"Member"` - Regular user (default)
- `"Admin"` - Administrator

---

## Troubleshooting

### Can't Access Admin Pages
- Verify you're logged in with admin credentials
- Check that Role = 'Admin' in database:
  ```sql
  SELECT * FROM Members WHERE Role = 'Admin';
  ```

### Admin User Not Found
Run the insert script:
```sql
INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES ('Admin User', 'admin@culturalcouncil.org', 'admin123', NULL, 'Admin');
```

### Navigation Doesn't Show Admin Menu
- Ensure ViewBag.IsAdmin is set in all controller actions
- Check SessionHelper.IsAdmin() is working correctly

---

## Files Added/Modified

### New Files
- `Filters/AdminAuthorizationAttribute.cs` - Admin authorization filter
- `Controllers/AdminController.cs` - Admin controller with all admin actions
- `Views/Admin/Index.cshtml` - Admin dashboard
- `Views/Admin/Events.cshtml` - Event management list
- `Views/Admin/CreateEvent.cshtml` - Create event form
- `Views/Admin/EditEvent.cshtml` - Edit event form
- `Views/Admin/DeleteEvent.cshtml` - Delete event confirmation
- `Views/Admin/Bookings.cshtml` - All bookings with member details
- `Views/Admin/Inquiries.cshtml` - All inquiries
- `Views/Admin/Members.cshtml` - All members list
- `Scripts/InsertAdminUser.sql` - SQL script to create admin user

### Modified Files
- `Models/Member.cs` - Added Role field
- `Helpers/SessionHelper.cs` - Added GetMemberRole() and IsAdmin()
- `Controllers/AccountController.cs` - Updated login to store role and redirect admin
- `Views/Shared/_Layout.cshtml` - Added admin menu items
- `Controllers/HomeController.cs` - Added IsAdmin to ViewBag
- `Data/ApplicationDbContext.cs` - Updated for role field

---

## Demo Flow for Presentation

1. **Show Admin Login**
   - Login with admin@culturalcouncil.org / admin123
   - Show admin badge in navbar

2. **Show Admin Dashboard**
   - Point out statistics cards
   - Show quick action buttons

3. **Create New Event**
   - Click "Create New Event"
   - Fill form and submit
   - Show success message

4. **View All Bookings with Member Details**
   - Show member names and emails
   - Demonstrate filtering by event
   - Show summary statistics

5. **Edit Event**
   - Edit an existing event
   - Show changes saved

6. **View Inquiries**
   - Show all contact messages

7. **View Members**
   - Show member list with activity counts

8. **Delete Event**
   - Show warning message
   - Demonstrate deletion

---

**Admin panel is fully functional and ready for testing!** ??
