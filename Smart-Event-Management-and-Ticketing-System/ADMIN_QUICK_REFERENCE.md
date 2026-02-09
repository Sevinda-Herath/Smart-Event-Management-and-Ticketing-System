# ?? Quick Admin Reference Card

## ?? Login Credentials
```
Email: admin@culturalcouncil.org
Password: admin123
```

## ?? Admin URLs
- Dashboard: `/Admin/Index`
- Manage Events: `/Admin/Events`
- Create Event: `/Admin/CreateEvent`
- All Bookings: `/Admin/Bookings`
- All Inquiries: `/Admin/Inquiries`
- All Members: `/Admin/Members`

## ? Quick Test Steps

### 1. Test Admin Login
1. Go to login page
2. Enter: `admin@culturalcouncil.org` / `admin123`
3. ? Should redirect to Admin Dashboard

### 2. Test Create Event
1. Click "Create New Event"
2. Fill form:
   - Name: "Test Concert"
   - Category: Music
   - Date: Any future date
   - Venue: "Test Hall"
   - Price: 25.00
   - Seats: 100
3. ? Event created successfully

### 3. Test View Bookings with Member Details
1. Register as member: `test@test.com` / `test123`
2. Book a ticket
3. Logout and login as admin
4. Click "All Bookings"
5. ? See booking with member name and email

### 4. Test Edit Event
1. Go to "Manage Events"
2. Click edit icon on any event
3. Change price to 50.00
4. ? Event updated

### 5. Test Delete Event
1. Go to "Manage Events"
2. Click delete icon
3. Confirm deletion
4. ? Event removed

### 6. Test Filter Bookings
1. Go to "All Bookings"
2. Select an event from dropdown
3. Click "Filter"
4. ? Shows only bookings for that event

## ?? Visual Indicators
- Admin users have **"ADMIN"** badge in navbar
- Navigation shows admin-specific menu items
- Dashboard has color-coded statistic cards

## ?? Access Control
- Only users with Role = "Admin" can access `/Admin/*` URLs
- Non-admins redirected to home page
- Non-logged-in users redirected to login page

## ?? What Admin Can See

### In Bookings List:
- ? Booking ID
- ? Member Name (Full Name)
- ? Member Email
- ? Event Name
- ? Event Date
- ? Seat Type
- ? Quantity
- ? Booking Date

### In Dashboard:
- ? Total Events
- ? Total Bookings
- ? Total Members
- ? Total Inquiries
- ? Total Reviews

## ?? Key Features
1. **Full CRUD** for events (Create, Read, Update, Delete)
2. **View all bookings** with complete member information
3. **Filter bookings** by event or member
4. **View all inquiries** from contact form
5. **View all members** with activity statistics

## ?? For Presentation
- Show admin login and automatic redirect
- Demonstrate event creation
- Highlight member details in booking list
- Show filtering functionality
- Point out admin badge in navbar

---

**Everything is ready! Press F5 to run.** ??
