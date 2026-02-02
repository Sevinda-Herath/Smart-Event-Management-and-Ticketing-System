# Smart Event Management and Ticketing System

**University Coursework Project**  
**Course:** Web Development / Software Engineering  
**Technology Stack:** ASP.NET Core MVC (.NET 10), Entity Framework Core, SQL Server LocalDB

---

## Project Overview

The **Smart Event Management and Ticketing System** is a web application designed for the **Metropolitan Cultural Council** to manage cultural events and ticket bookings. The system allows:

- **Guests** to browse events (limited information), view reviews, and send inquiries
- **Members** to register, login, browse events (full details), book tickets, and submit reviews

---

## Features Implemented

### For Guests (Non-Logged In Users)
? Browse events with limited information (Event Name, Category, Date, Venue)  
? View event availability as "Available" or "Full"  
? View reviews submitted by members  
? Register as a new member  
? Send inquiries/contact messages  

### For Members (Logged In Users)
? Member registration with email validation  
? Login/Logout with session-based authentication  
? Browse all events with full details (including price and exact seat availability)  
? Search and filter events by Category, Date, Location, Price  
? Book tickets with seat type selection (Standard/VIP)  
? View all personal bookings  
? Cancel bookings  
? Submit reviews for attended events (Rating 1-5 + Comment)  
? Edit submitted reviews  

---

## Setup Instructions

### Prerequisites
- Visual Studio 2026 (or Visual Studio 2022+)
- .NET 10 SDK (or .NET 8+ SDK)
- SQL Server Express LocalDB

### Installation Steps

1. **Open the Project in Visual Studio**

2. **Restore NuGet Packages**
   - Automatically restored on build, or run:
   ```bash
   dotnet restore
   ```

3. **Database is Already Created!**
   - The database migration has already been applied
   - Sample event data is seeded

4. **Run the Application**
   - Press `F5` in Visual Studio or run:
   ```bash
   dotnet run
   ```

5. **Access the Application**
   - Open browser: `https://localhost:5001` or the URL shown in console

---

## How to Use

### As a Guest (Not Logged In)
1. Browse events from the home page (limited information)
2. View event details and reviews
3. Send inquiries via "Contact Us"
4. Register to become a member

### As a Member (Logged In)
1. Click "Register" and create an account
2. Login with your credentials
3. Browse events with full details and pricing
4. Book tickets for any available event
5. View and manage your bookings under "My Bookings"
6. Submit reviews for events you've attended

---

## Project Structure

```
Controllers/    - Handle HTTP requests and business logic
Models/         - Database entities (Member, Event, Booking, Review, Inquiry)
Views/          - Razor views for UI
Data/           - ApplicationDbContext with EF Core
Helpers/        - Session management utilities
Filters/        - Custom authorization attribute
```

---

## Sample Seed Data

6 pre-loaded events including:
- Metropolitan Orchestra: Symphony Night
- Contemporary Art Exhibition
- Shakespeare's Hamlet
- Jazz Night Live
- Cultural Dance Festival
- Photography Workshop

---

## Technologies Used

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core (Code-First)
- SQL Server LocalDB
- Bootstrap 5
- Font Awesome Icons
- Session-based Authentication

---

## Important Notes

?? **For Educational Purposes:** This project uses simplified authentication without password hashing for coursework demonstration.

? **Well-Documented Code:** All classes and methods include comments explaining functionality.

---

## Troubleshooting

If you encounter database issues:
```bash
dotnet ef database drop
dotnet ef database update
```

---

**Created for University Coursework - 2025/2026**
