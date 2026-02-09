# Quick SQL Reference - First Time Setup

## ?? Quick Start (3 Steps)

### Step 1: Run the SQL Script
```sql
-- Open CompleteSetup.sql in SSMS or Visual Studio
-- Execute the entire script (F5)
```

### Step 2: Create Admin User
```
Navigate to: https://localhost:7227/Home/ResetDatabase
```

### Step 3: Login and Test
```
Email: admin@culturalcouncil.org
Password: admin123
```

---

## ?? What the Script Does

### Database Operations
```
? DROP DATABASE EventManagementDB (if exists)
? CREATE DATABASE EventManagementDB
? USE EventManagementDB
```

### Tables Created (5)
```
1. Members        - User accounts
2. Events         - Cultural events
3. Bookings       - Ticket reservations
4. Reviews        - Event reviews (1-5 stars)
5. Inquiries      - Contact messages
```

### Relationships Established
```
Members ???? Bookings (1:Many)
         ??? Reviews (1:Many)

Events ???? Bookings (1:Many)
        ??? Reviews (1:Many)

Inquiries (Standalone, no FK)
```

### Constraints Added
```
? Primary Keys (All tables)
? Foreign Keys (Bookings, Reviews)
? Unique Constraints (Email, One review per member per event)
? Check Constraints (Rating 1-5, Price >= 0, etc.)
? Default Values (Role='Member', BookingDate=NOW())
```

### Indexes Created (6)
```
IX_Members_Email
IX_Events_EventDate
IX_Bookings_BookingDate
IX_Reviews_ReviewDate
IX_Bookings_MemberId_EventId
IX_Reviews_MemberId_EventId
```

### Views Created (2)
```
vw_MemberStatistics  - Member activity stats
vw_EventStatistics   - Event booking/review stats
```

### Stored Procedures (1)
```
sp_ResetDatabase - Reset all data (for testing)
```

---

## ?? Verification Queries

### Check All Tables
```sql
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';
```

### Check Row Counts
```sql
SELECT 
    'Members' AS TableName, COUNT(*) AS Rows FROM Members
UNION ALL SELECT 'Events', COUNT(*) FROM Events
UNION ALL SELECT 'Bookings', COUNT(*) FROM Bookings
UNION ALL SELECT 'Reviews', COUNT(*) FROM Reviews
UNION ALL SELECT 'Inquiries', COUNT(*) FROM Inquiries;
```

### View Admin User
```sql
SELECT * FROM Members WHERE Role = 'Admin';
```

---

## ??? Useful Commands

### Reset Everything
```sql
EXEC sp_ResetDatabase;
```

### View Statistics
```sql
-- Member stats
SELECT * FROM vw_MemberStatistics;

-- Event stats
SELECT * FROM vw_EventStatistics;
```

### Manual Admin Insert (after getting hash)
```sql
INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES ('Administrator', 'admin@culturalcouncil.org', 'YOUR_HASH', NULL, 'Admin');
```

---

## ?? Sample Data Queries

### Add Sample Event
```sql
INSERT INTO Events (EventName, Category, EventDate, Venue, Description, Price, TotalSeats)
VALUES ('Summer Music Festival', 'Music', '2025-07-15 19:00', 
        'Central Park', 'Live music event', 45.00, 500);
```

### Add Sample Member
```sql
-- Note: Password must be hashed! Use the application to register instead.
```

### View All Events with Availability
```sql
SELECT 
    EventName,
    EventDate,
    TotalSeats,
    BookedSeats,
    AvailableSeats,
    IsFull,
    AverageRating
FROM vw_EventStatistics
ORDER BY EventDate;
```

---

## ?? Important Notes

1. **Admin Password**: Must be hashed using the application
2. **Cascade Delete**: Deleting a member deletes their bookings and reviews
3. **One Review Rule**: Members can only write one review per event
4. **Identity Columns**: All IDs auto-increment from 1

---

## ?? Connection String

Add to `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EventManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

---

## ?? Quick Commands Reference

| Command | Action |
|---------|--------|
| `F5` in SSMS | Execute script |
| `EXEC sp_ResetDatabase` | Clear all data |
| `SELECT * FROM vw_MemberStatistics` | View member stats |
| `SELECT * FROM vw_EventStatistics` | View event stats |

---

**Script Location**: `/Database/CompleteSetup.sql`  
**Documentation**: `/Database/README.md`  
**Complete Guide**: `/COMPLETE_DOCUMENTATION.md`
