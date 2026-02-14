# ? Custom Database Schema Implementation

## Changes Made: Default Schema "EVENT_MGMT"

All database tables now use a custom schema `EVENT_MGMT` instead of the default `dbo` schema.

---

## What Changed

### 1. ApplicationDbContext.cs ?
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Set default schema for all tables
    modelBuilder.HasDefaultSchema("EVENT_MGMT");

    // ... rest of configuration
}
```

**Result:** All Entity Framework tables will be created in the `EVENT_MGMT` schema.

---

### 2. CompleteSetup.sql ?

#### Schema Creation
```sql
-- Create custom schema
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'EVENT_MGMT')
BEGIN
    EXEC('CREATE SCHEMA EVENT_MGMT');
    PRINT 'Schema EVENT_MGMT created.';
END
GO
```

#### All Tables Updated
```sql
-- Before: CREATE TABLE Members (...)
-- After:
CREATE TABLE EVENT_MGMT.Members (...)
CREATE TABLE EVENT_MGMT.Events (...)
CREATE TABLE EVENT_MGMT.Bookings (...)
CREATE TABLE EVENT_MGMT.Reviews (...)
CREATE TABLE EVENT_MGMT.Inquiries (...)
```

#### All Indexes Updated
```sql
CREATE INDEX IX_Members_Email ON EVENT_MGMT.Members(Email);
CREATE INDEX IX_Events_EventDate ON EVENT_MGMT.Events(EventDate);
-- ... all 6 indexes
```

#### Views Updated
```sql
CREATE VIEW EVENT_MGMT.vw_MemberStatistics AS
SELECT ... FROM EVENT_MGMT.Members m
LEFT JOIN EVENT_MGMT.Bookings b ON ...
LEFT JOIN EVENT_MGMT.Reviews r ON ...
```

#### Stored Procedure Updated
```sql
CREATE PROCEDURE EVENT_MGMT.sp_ResetDatabase AS
BEGIN
    DELETE FROM EVENT_MGMT.Reviews;
    DELETE FROM EVENT_MGMT.Bookings;
    -- ... etc
END
```

---

## Database Structure

### Before (Default dbo schema)
```
EventManagementDB
??? dbo (schema)
    ??? Members
    ??? Events
    ??? Bookings
    ??? Reviews
    ??? Inquiries
```

### After (Custom EVENT_MGMT schema)
```
EventManagementDB
??? dbo (schema) - empty
??? EVENT_MGMT (schema) ?
    ??? Members
    ??? Events
    ??? Bookings
    ??? Reviews
    ??? Inquiries
    ??? vw_MemberStatistics
    ??? vw_EventStatistics
    ??? sp_ResetDatabase
```

---

## Benefits of Custom Schema

### 1. Organization
- Clear separation from system tables
- Easy to identify application-specific objects
- Better database management

### 2. Security
- Can grant permissions at schema level
- Easier access control
- Isolate application data

### 3. Multi-Tenancy
- Can add more schemas for different modules
- Example: `EVENT_MGMT`, `REPORTING`, `AUDIT`

### 4. Portability
- Schema can be moved independently
- Easier to backup/restore specific modules
- Clean separation of concerns

---

## SQL Queries with Schema

### Old Way (dbo)
```sql
SELECT * FROM Members;
SELECT * FROM Events;
EXEC sp_ResetDatabase;
```

### New Way (EVENT_MGMT)
```sql
SELECT * FROM EVENT_MGMT.Members;
SELECT * FROM EVENT_MGMT.Events;
EXEC EVENT_MGMT.sp_ResetDatabase;
```

**Note:** If you set the default schema for your database user, you can omit the schema prefix.

---

## Setting Default Schema for User

To avoid typing `EVENT_MGMT.` every time:

```sql
-- Set default schema for your user
ALTER USER [YourUsername] WITH DEFAULT_SCHEMA = EVENT_MGMT;

-- Now these work without prefix:
SELECT * FROM Members;  -- Looks in EVENT_MGMT.Members
SELECT * FROM Events;   -- Looks in EVENT_MGMT.Events
```

---

## Entity Framework Compatibility

? **Fully Compatible**

Entity Framework Core will automatically:
- Create tables in `EVENT_MGMT` schema
- Generate correct SQL with schema prefix
- Handle all queries properly

Example generated SQL:
```sql
SELECT [m].[MemberId], [m].[FullName], [m].[Email]
FROM [EVENT_MGMT].[Members] AS [m]
WHERE [m].[Email] = @p0
```

---

## Testing the Schema

### Step 1: Run the SQL Script
```sql
-- Execute CompleteSetup.sql
-- Will create EVENT_MGMT schema and all objects
```

### Step 2: Verify Schema Exists
```sql
SELECT * FROM sys.schemas WHERE name = 'EVENT_MGMT';
-- Should return 1 row
```

### Step 3: List Tables in Schema
```sql
SELECT TABLE_SCHEMA, TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'EVENT_MGMT'
ORDER BY TABLE_NAME;

-- Expected output:
-- EVENT_MGMT | Bookings
-- EVENT_MGMT | Events
-- EVENT_MGMT | Inquiries
-- EVENT_MGMT | Members
-- EVENT_MGMT | Reviews
```

### Step 4: Test Stored Procedure
```sql
EXEC EVENT_MGMT.sp_ResetDatabase;
-- Should execute successfully
```

### Step 5: Test Views
```sql
SELECT * FROM EVENT_MGMT.vw_MemberStatistics;
SELECT * FROM EVENT_MGMT.vw_EventStatistics;
-- Should return results (empty initially)
```

---

## Migration from dbo to EVENT_MGMT

If you already have tables in `dbo` schema and want to move them:

```sql
-- Transfer objects from dbo to EVENT_MGMT
ALTER SCHEMA EVENT_MGMT TRANSFER dbo.Members;
ALTER SCHEMA EVENT_MGMT TRANSFER dbo.Events;
ALTER SCHEMA EVENT_MGMT TRANSFER dbo.Bookings;
ALTER SCHEMA EVENT_MGMT TRANSFER dbo.Reviews;
ALTER SCHEMA EVENT_MGMT TRANSFER dbo.Inquiries;
```

**Or just run the CompleteSetup.sql script to start fresh.**

---

## Connection String

No changes needed! Schema is handled by Entity Framework:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=EventManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

---

## SQL Server Management Studio (SSMS)

In SSMS, you'll see:
```
EventManagementDB
??? Security
?   ??? Schemas
?       ??? dbo
?       ??? EVENT_MGMT ?
??? Tables
?   ??? dbo.sysdiagrams (if any)
?   ??? EVENT_MGMT
?       ??? Members
?       ??? Events
?       ??? Bookings
?       ??? Reviews
?       ??? Inquiries
??? Views
?   ??? EVENT_MGMT
?       ??? vw_MemberStatistics
?       ??? vw_EventStatistics
??? Programmability
    ??? Stored Procedures
        ??? EVENT_MGMT
            ??? sp_ResetDatabase
```

---

## Updated Commands

### View Data
```sql
SELECT * FROM EVENT_MGMT.Members;
SELECT * FROM EVENT_MGMT.Events;
SELECT * FROM EVENT_MGMT.Bookings;
SELECT * FROM EVENT_MGMT.Reviews;
SELECT * FROM EVENT_MGMT.Inquiries;
```

### Use Views
```sql
SELECT * FROM EVENT_MGMT.vw_MemberStatistics;
SELECT * FROM EVENT_MGMT.vw_EventStatistics;
```

### Reset Database
```sql
EXEC EVENT_MGMT.sp_ResetDatabase;
```

### Insert Admin (after generating hash)
```sql
INSERT INTO EVENT_MGMT.Members (FullName, Email, Password, PreferredCategory, Role)
VALUES ('Administrator', 'admin@culturalcouncil.org', 'HASH_HERE', NULL, 'Admin');
```

---

## Troubleshooting

### Error: Invalid object name 'Members'
**Problem:** Query doesn't include schema
```sql
-- Wrong
SELECT * FROM Members;

-- Correct
SELECT * FROM EVENT_MGMT.Members;
```

**Solution:** Always prefix with schema or set default schema for user.

### Error: Schema already exists
**Problem:** Running script multiple times
```sql
-- Script already handles this with:
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'EVENT_MGMT')
```

**Solution:** Script is safe to run multiple times.

### Entity Framework Can't Find Tables
**Problem:** Old migrations reference `dbo` schema

**Solution:** Delete old migrations and recreate:
```bash
dotnet ef migrations remove
dotnet ef migrations add InitialWithSchema
dotnet ef database update
```

Or use the SQL script directly (no migrations needed).

---

## Summary

? **ApplicationDbContext** - Default schema set to EVENT_MGMT  
? **SQL Script** - All objects in EVENT_MGMT schema  
? **Tables** - 5 tables in EVENT_MGMT  
? **Indexes** - 6 indexes in EVENT_MGMT  
? **Views** - 2 views in EVENT_MGMT  
? **Stored Procedure** - 1 procedure in EVENT_MGMT  
? **Build** - Successful  
? **Compatibility** - Entity Framework Core ready  

---

## Next Steps

1. ? Run `Database/CompleteSetup.sql`
2. ? Verify schema created
3. ? Start application
4. ? Navigate to `/Home/ResetDatabase`
5. ? Create admin user
6. ? Test the application

**Everything is ready to use with the EVENT_MGMT schema!** ??
