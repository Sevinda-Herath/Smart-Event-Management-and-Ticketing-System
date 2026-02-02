# Quick Start Guide - Smart Event Management System

## Running the Application

### Method 1: Visual Studio
1. Open the solution in Visual Studio
2. Press **F5** to run
3. Browser will open automatically

### Method 2: Command Line
```bash
cd Smart-Event-Management-and-Ticketing-System
dotnet run
```
Then open: https://localhost:5001

---

## Test the System

### 1. Register a New Member
- Click "Register" in navigation
- Fill in the form:
  - Full Name: John Doe
  - Email: john@example.com
  - Password: password123
  - Preferred Category: Music
- Click "Register"

### 2. Login
- Click "Login"
- Enter email: john@example.com
- Enter password: password123
- Click "Login"

### 3. Browse Events
- Click "Browse Events" in navigation
- Use filters to search by:
  - Category (Music, Art, Theater, etc.)
  - Date
  - Location
  - Maximum Price

### 4. Book Tickets
- Click on any event's "View Details"
- Click "Book Tickets"
- Select Seat Type (Standard or VIP)
- Enter quantity (1-10)
- Click "Confirm Booking"

### 5. View Your Bookings
- Click "My Bookings" in navigation
- See all your ticket bookings
- Click "View Details" or "Cancel Booking"

### 6. Submit a Review
- After booking an event, go to event details
- Click "Write a Review"
- Select rating (1-5 stars)
- Write your comment
- Click "Submit Review"

### 7. Send an Inquiry (Works for Guests too!)
- Click "Contact Us" in navigation
- Fill in name, email, and message
- Click "Send Message"

---

## Key URLs

| Feature | URL |
|---------|-----|
| Home Page | /Home/Index |
| Browse Events | /Events/Index |
| Event Details | /Events/Details/{id} |
| Register | /Account/Register |
| Login | /Account/Login |
| My Bookings | /Bookings/Index |
| Contact Us | /Inquiries/Create |

---

## Database Connection

**Connection String:** `Server=(localdb)\\mssqllocaldb;Database=SmartEventManagementDB;Trusted_Connection=true`

**Location:** appsettings.json

---

## Pre-Seeded Events

| ID | Event Name | Category | Date | Price | Seats |
|----|------------|----------|------|-------|-------|
| 1 | Metropolitan Orchestra: Symphony Night | Music | Jun 15, 2025 | $45.00 | 500 |
| 2 | Contemporary Art Exhibition | Art | May 20, 2025 | $15.00 | 200 |
| 3 | Shakespeare's Hamlet | Theater | Jul 10, 2025 | $35.00 | 350 |
| 4 | Jazz Night Live | Music | May 30, 2025 | $25.00 | 150 |
| 5 | Cultural Dance Festival | Dance | Aug 5, 2025 | $30.00 | 400 |
| 6 | Photography Workshop | Workshop | Jun 1, 2025 | $50.00 | 30 |

---

## Important Features to Demonstrate

? **Guest vs Member Views:**
- Logout and browse events as a guest (limited info)
- Login and see full details with pricing

? **Authorization:**
- Try accessing /Bookings/Index without logging in
- You'll be redirected to login page

? **Validation:**
- Try registering with an existing email
- Try booking more tickets than available

? **Search & Filter:**
- Filter events by category "Music"
- Search for "Shakespeare"
- Set max price to $30

? **Reviews:**
- Only members who booked an event can review
- Each member can only review once per event

---

## Common Commands

### Reset Database
```bash
dotnet ef database drop -f
dotnet ef database update
```

### View Database in Visual Studio
1. View ? SQL Server Object Explorer
2. Expand (localdb)\\mssqllocaldb
3. Expand Databases ? SmartEventManagementDB
4. Explore tables: Members, Events, Bookings, Reviews, Inquiries

### Check Database Tables
```sql
SELECT * FROM Members
SELECT * FROM Events
SELECT * FROM Bookings
SELECT * FROM Reviews
SELECT * FROM Inquiries
```

---

## Project Highlights for Demonstration

1. **Clean MVC Architecture:** Models, Views, Controllers properly separated
2. **Entity Framework Code-First:** Database created from C# classes
3. **Relationships:** One-to-Many relationships properly implemented
4. **Session-Based Auth:** Custom authentication without Identity
5. **Bootstrap UI:** Responsive, professional design
6. **Data Validation:** Both client and server-side
7. **Seed Data:** Sample events pre-loaded
8. **Business Logic:** Seat availability checking, review restrictions
9. **User Experience:** Success/error messages, intuitive navigation
10. **Well-Documented Code:** Comments explaining functionality

---

## Tips for Coursework Presentation

?? **Demo Flow:**
1. Show home page as guest
2. Register new member
3. Show difference in event details (guest vs member)
4. Book tickets
5. Show "My Bookings"
6. Submit a review
7. Show contact form

?? **Code to Highlight:**
- Models with Data Annotations (Models/Event.cs)
- DbContext with relationships (Data/ApplicationDbContext.cs)
- Authorization filter (Filters/MemberAuthorizationAttribute.cs)
- Session helper (Helpers/SessionHelper.cs)

?? **Database to Show:**
- Open SQL Server Object Explorer
- Show tables and relationships
- Show seed data in Events table

---

## Troubleshooting

**Database not created?**
```bash
dotnet ef database update
```

**Port already in use?**
- Edit Properties/launchSettings.json
- Change port numbers

**Migration errors?**
```bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

**Ready to run! Press F5 in Visual Studio!** ??
