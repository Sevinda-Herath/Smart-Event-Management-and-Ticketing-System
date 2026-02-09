# Database Setup Guide

## Complete Database Setup Script

The `CompleteSetup.sql` script contains all database operations needed for first-time setup of the Smart Event Management System.

---

## What's Included in the Script

### 1. Database Creation
- Drops existing database (if exists)
- Creates new `EventManagementDB` database

### 2. Table Creation
- **Members** - User accounts (members and admins)
- **Events** - Cultural events
- **Bookings** - Ticket bookings
- **Reviews** - Event reviews and ratings
- **Inquiries** - Contact form submissions

### 3. Relationships & Constraints
- Foreign key relationships with CASCADE delete
- Check constraints for data validation
- Unique constraints (email, one review per member per event)
- Default values for common fields

### 4. Performance Indexes
- Email lookup index
- Date-based indexes
- Composite indexes for common queries

### 5. Views
- `vw_MemberStatistics` - Member activity statistics
- `vw_EventStatistics` - Event booking and review statistics

### 6. Stored Procedures
- `sp_ResetDatabase` - Reset database for testing

---

## How to Run the Script

### Option 1: SQL Server Management Studio (SSMS)

1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Click **File** ? **Open** ? **File**
4. Navigate to `Database/CompleteSetup.sql`
5. Click **Execute** (F5)
6. Check messages pane for success confirmation

### Option 2: Visual Studio

1. Open **SQL Server Object Explorer**
2. Right-click your server ? **New Query**
3. Copy and paste the contents of `CompleteSetup.sql`
4. Click **Execute**

### Option 3: Command Line (sqlcmd)

```bash
sqlcmd -S (localdb)\mssqllocaldb -i "Database\CompleteSetup.sql"
```

### Option 4: Azure Data Studio

1. Open Azure Data Studio
2. Connect to your server
3. File ? Open File ? Select `CompleteSetup.sql`
4. Click Run

---

## After Running the Script

### Step 1: Verify Database Creation

Run this query to verify:
```sql
USE EventManagementDB;
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
```

Expected output: 5 tables (Members, Events, Bookings, Reviews, Inquiries)

### Step 2: Create Admin User

The admin user requires a hashed password. Choose one method:

**Method A: Automatic (Recommended)**
1. Start the application (F5 in Visual Studio)
2. Navigate to: `https://localhost:7227/Home/ResetDatabase`
3. Admin user will be created automatically
4. Login with: `admin@culturalcouncil.org` / `admin123`

**Method B: Manual**
1. Navigate to: `https://localhost:7227/Home/GenerateHash`
2. Copy the generated hash
3. Run this SQL (replace with your hash):
```sql
USE EventManagementDB;
INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES ('Administrator', 'admin@culturalcouncil.org', 'YOUR_HASH_HERE', NULL, 'Admin');
```

### Step 3: Update Connection String

Update `appsettings.json` in your project:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EventManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Step 4: Test the Application

1. Run the application
2. Login as admin
3. Create a sample event
4. Register a test member
5. Book tickets and write reviews

---

## Database Schema Overview

```
???????????????
?   Members   ?
? (MemberId)  ?
???????????????
       ?
       ????????????????
       ?              ?
???????????????  ??????????????
?  Bookings   ?  ?  Reviews   ?
? (BookingId) ?  ? (ReviewId) ?
???????????????  ??????????????
       ?             ?
       ?    ??????????
       ?    ?
????????????????
?    Events    ?
?  (EventId)   ?
????????????????

???????????????
?  Inquiries  ?
? (InquiryId) ?
? (Standalone)?
???????????????
```

---

## Table Details

### Members Table
```sql
MemberId (PK, Identity)
FullName (NVARCHAR(100), NOT NULL)
Email (NVARCHAR(100), NOT NULL, UNIQUE)
Password (NVARCHAR(500), NOT NULL) -- Hashed
PreferredCategory (NVARCHAR(50), NULL)
Role (NVARCHAR(20), NOT NULL, DEFAULT 'Member')
```

### Events Table
```sql
EventId (PK, Identity)
EventName (NVARCHAR(200), NOT NULL)
Category (NVARCHAR(50), NOT NULL)
EventDate (DATETIME, NOT NULL)
Venue (NVARCHAR(200), NOT NULL)
Description (NVARCHAR(MAX), NULL)
Price (DECIMAL(10, 2), NOT NULL)
TotalSeats (INT, NOT NULL)
```

### Bookings Table
```sql
BookingId (PK, Identity)
MemberId (FK ? Members)
EventId (FK ? Events)
Quantity (INT, NOT NULL)
BookingDate (DATETIME, DEFAULT GETDATE())
SeatType (NVARCHAR(50), DEFAULT 'Standard')
```

### Reviews Table
```sql
ReviewId (PK, Identity)
MemberId (FK ? Members)
EventId (FK ? Events)
Rating (INT, 1-5, NOT NULL)
Comment (NVARCHAR(500), NOT NULL)
ReviewDate (DATETIME, DEFAULT GETDATE())
UNIQUE(MemberId, EventId) -- One review per member per event
```

### Inquiries Table
```sql
InquiryId (PK, Identity)
SenderName (NVARCHAR(100), NOT NULL)
SenderEmail (NVARCHAR(100), NOT NULL)
Message (NVARCHAR(MAX), NOT NULL)
InquiryDate (DATETIME, DEFAULT GETDATE())
```

---

## Useful Queries

### View All Members with Statistics
```sql
SELECT * FROM vw_MemberStatistics;
```

### View All Events with Availability
```sql
SELECT * FROM vw_EventStatistics;
```

### Check Admin User
```sql
SELECT MemberId, FullName, Email, Role 
FROM Members 
WHERE Role = 'Admin';
```

### Reset Database (For Testing)
```sql
EXEC sp_ResetDatabase;
```

### View Recent Bookings
```sql
SELECT TOP 10 
    b.BookingId,
    m.FullName,
    e.EventName,
    b.Quantity,
    b.BookingDate
FROM Bookings b
INNER JOIN Members m ON b.MemberId = m.MemberId
INNER JOIN Events e ON b.EventId = e.EventId
ORDER BY b.BookingDate DESC;
```

### View Reviews with Member and Event Names
```sql
SELECT 
    r.ReviewId,
    m.FullName AS Reviewer,
    e.EventName,
    r.Rating,
    r.Comment,
    r.ReviewDate
FROM Reviews r
INNER JOIN Members m ON r.MemberId = m.MemberId
INNER JOIN Events e ON r.EventId = e.EventId
ORDER BY r.ReviewDate DESC;
```

---

## Troubleshooting

### Error: Database already exists
- The script automatically drops and recreates the database
- Make sure no other connections are using the database
- Close Visual Studio and SSMS connections before running

### Error: Cannot drop database in use
Run this first:
```sql
USE master;
ALTER DATABASE EventManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE EventManagementDB;
```

### Error: Permission denied
- Make sure you have `sysadmin` or `db_owner` rights
- Run SSMS as Administrator
- Check SQL Server authentication settings

### Entity Framework Migrations Conflict
If you've already run EF migrations:
```bash
# Delete existing migrations
dotnet ef migrations remove

# Create new migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

Or simply let the SQL script handle everything (no migrations needed).

---

## Maintenance

### Backup Database
```sql
BACKUP DATABASE EventManagementDB 
TO DISK = 'C:\Backups\EventManagementDB.bak'
WITH FORMAT, INIT, NAME = 'Full Backup';
```

### Restore Database
```sql
USE master;
RESTORE DATABASE EventManagementDB 
FROM DISK = 'C:\Backups\EventManagementDB.bak'
WITH REPLACE;
```

### Check Database Size
```sql
EXEC sp_spaceused;
```

### Rebuild Indexes
```sql
ALTER INDEX ALL ON Members REBUILD;
ALTER INDEX ALL ON Events REBUILD;
ALTER INDEX ALL ON Bookings REBUILD;
ALTER INDEX ALL ON Reviews REBUILD;
```

---

## Notes

1. **Password Security**: The admin password must be hashed using PBKDF2. Never store plain text passwords in production.

2. **Cascade Deletion**: When a member is deleted, all their bookings and reviews are automatically deleted. Same for events.

3. **Identity Seeds**: All primary keys use IDENTITY(1,1) for auto-increment.

4. **Indexes**: The script creates indexes on frequently queried columns for better performance.

5. **Views**: Use the provided views for reporting and statistics rather than writing complex joins repeatedly.

6. **Stored Procedure**: The `sp_ResetDatabase` procedure is for development/testing only. Remove it in production.

---

## Support

For issues or questions:
1. Check the `COMPLETE_DOCUMENTATION.md` file
2. Review the error messages in SQL output
3. Verify your SQL Server version (compatible with SQL Server 2016+)
4. Ensure .NET 10 SDK is installed for the application

---

**Database setup complete! You're ready to run the application.** ?
