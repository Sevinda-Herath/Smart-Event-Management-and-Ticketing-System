-- =====================================================
-- Smart Event Management System - Complete Database Setup
-- =====================================================
-- This script creates all tables, relationships, and initial data
-- Run this script when setting up the application for the first time
-- =====================================================

USE master;
GO

-- =====================================================
-- SECTION 1: Database Creation
-- =====================================================
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'EventManagementDB')
BEGIN
    ALTER DATABASE EventManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EventManagementDB;
    PRINT 'Existing database dropped.';
END
GO

CREATE DATABASE EventManagementDB;
GO

PRINT 'Database EventManagementDB created successfully.';
GO

USE EventManagementDB;
GO

-- =====================================================
-- SECTION 2: Table Creation
-- =====================================================

-- Table 1: Members
-- Stores user accounts (both regular members and admins)
CREATE TABLE Members (
    MemberId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL, -- Stores plain text password for coursework
    PreferredCategory NVARCHAR(50) NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Member', -- 'Member' or 'Admin'
    CONSTRAINT CK_Members_Email CHECK (Email LIKE '%@%'),
    CONSTRAINT CK_Members_Role CHECK (Role IN ('Member', 'Admin'))
);
GO

PRINT 'Table Members created.';
GO

-- Table 2: Events
-- Stores cultural events
CREATE TABLE Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(200) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    EventDate DATETIME NOT NULL,
    Venue NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Price DECIMAL(10, 2) NOT NULL,
    TotalSeats INT NOT NULL,
    CONSTRAINT CK_Events_Price CHECK (Price >= 0),
    CONSTRAINT CK_Events_TotalSeats CHECK (TotalSeats > 0),
    CONSTRAINT CK_Events_Category CHECK (Category IN ('Music', 'Theater', 'Dance', 'Art', 'Workshop', 'Festival', 'Conference'))
);
GO

PRINT 'Table Events created.';
GO

-- Table 3: Bookings
-- Stores ticket bookings
CREATE TABLE Bookings (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    EventId INT NOT NULL,
    Quantity INT NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    SeatType NVARCHAR(50) NOT NULL DEFAULT 'Standard',
    CONSTRAINT FK_Bookings_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId) ON DELETE CASCADE,
    CONSTRAINT FK_Bookings_Events FOREIGN KEY (EventId) REFERENCES Events(EventId) ON DELETE CASCADE,
    CONSTRAINT CK_Bookings_Quantity CHECK (Quantity > 0)
);
GO

PRINT 'Table Bookings created.';
GO

-- Table 4: Reviews
-- Stores event reviews and ratings
CREATE TABLE Reviews (
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    MemberId INT NOT NULL,
    EventId INT NOT NULL,
    Rating INT NOT NULL,
    Comment NVARCHAR(500) NOT NULL,
    ReviewDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Reviews_Members FOREIGN KEY (MemberId) REFERENCES Members(MemberId) ON DELETE CASCADE,
    CONSTRAINT FK_Reviews_Events FOREIGN KEY (EventId) REFERENCES Events(EventId) ON DELETE CASCADE,
    CONSTRAINT CK_Reviews_Rating CHECK (Rating BETWEEN 1 AND 5),
    CONSTRAINT UQ_Reviews_MemberEvent UNIQUE (MemberId, EventId) -- One review per member per event
);
GO

PRINT 'Table Reviews created.';
GO

-- Table 5: Inquiries
-- Stores contact form submissions
CREATE TABLE Inquiries (
    InquiryId INT IDENTITY(1,1) PRIMARY KEY,
    SenderName NVARCHAR(100) NOT NULL,
    SenderEmail NVARCHAR(100) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    InquiryDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CK_Inquiries_Email CHECK (SenderEmail LIKE '%@%')
);
GO

PRINT 'Table Inquiries created.';
GO

-- =====================================================
-- SECTION 4: Indexes for Performance
-- =====================================================

-- Index on Member Email for faster login queries
CREATE NONCLUSTERED INDEX IX_Members_Email ON EVENT_MGMT.Members(Email);
GO

-- Index on Event Date for event listing queries
CREATE NONCLUSTERED INDEX IX_Events_EventDate ON EVENT_MGMT.Events(EventDate);
GO

-- Index on Booking Date for booking history queries
CREATE NONCLUSTERED INDEX IX_Bookings_BookingDate ON EVENT_MGMT.Bookings(BookingDate DESC);
GO

-- Index on Review Date for recent reviews
CREATE NONCLUSTERED INDEX IX_Reviews_ReviewDate ON EVENT_MGMT.Reviews(ReviewDate DESC);
GO

-- Composite index for member bookings
CREATE NONCLUSTERED INDEX IX_Bookings_MemberId_EventId ON EVENT_MGMT.Bookings(MemberId, EventId);
GO

-- Composite index for member reviews
CREATE NONCLUSTERED INDEX IX_Reviews_MemberId_EventId ON EVENT_MGMT.Reviews(MemberId, EventId);
GO

PRINT 'Indexes created successfully.';
GO

-- =====================================================
-- SECTION 5: Initial Data - Admin User
-- =====================================================

-- Add admin user with plain text password (for coursework simplicity)
INSERT INTO EVENT_MGMT.Members (FullName, Email, Password, PreferredCategory, Role)
VALUES (
    'Administrator',
    'admin@culturalcouncil.org',
    'admin123',
    NULL,
    'Admin'
);

PRINT 'Admin user created successfully with plain text password.';
PRINT 'Email: admin@culturalcouncil.org';
PRINT 'Password: admin123';
GO

-- =====================================================
-- SECTION 5: Sample Data (Optional)
-- =====================================================

-- Sample Events
-- Uncomment to add sample events for testing


INSERT INTO Events (EventName, Category, EventDate, Venue, Description, Price, TotalSeats)
VALUES 
    ('Summer Music Festival', 'Music', '2025-07-15 19:00:00', 'Central Park Amphitheater', 'A celebration of summer with live music from local and international artists.', 45.00, 500),
    ('Shakespeare in the Park', 'Theater', '2025-08-05 20:00:00', 'Riverside Theater', 'Classic performance of A Midsummer Night''s Dream under the stars.', 35.00, 300),
    ('Contemporary Dance Showcase', 'Dance', '2025-07-20 18:30:00', 'Metropolitan Arts Center', 'Innovative choreography by emerging dance companies.', 30.00, 200),
    ('Local Artists Exhibition', 'Art', '2025-07-10 10:00:00', 'City Gallery', 'Featuring works by talented local artists in various mediums.', 15.00, 150),
    ('Creative Writing Workshop', 'Workshop', '2025-08-01 14:00:00', 'Community Center', 'Interactive workshop on storytelling and creative writing techniques.', 25.00, 50),
    ('Annual Cultural Festival', 'Festival', '2025-09-15 12:00:00', 'Downtown Square', 'A day-long celebration of diverse cultures with food, music, and performances.', 20.00, 1000),
    ('Tech Innovation Conference', 'Conference', '2025-08-25 09:00:00', 'Convention Center Hall A', 'Leading experts discuss the future of technology and innovation.', 75.00, 400);

PRINT 'Sample events inserted.';
GO


-- =====================================================
-- SECTION 6: Verification Queries
-- =====================================================

-- Display table structure and row counts
PRINT '';
PRINT '=====================================================';
PRINT 'DATABASE SETUP VERIFICATION';
PRINT '=====================================================';
PRINT '';
PRINT 'Tables Created:';
GO

SELECT 
    t.name AS TableName,
    SUM(p.rows) AS RowCount
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE t.type = 'U' AND p.index_id IN (0,1)
GROUP BY t.name
ORDER BY t.name;
GO

PRINT '';
PRINT 'Foreign Key Relationships:';
GO

SELECT 
    OBJECT_NAME(f.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME(f.referenced_object_id) AS ReferencedTable,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumn
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
ORDER BY TableName;
GO

PRINT '';
PRINT 'Indexes Created:';
GO

SELECT 
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType
FROM sys.indexes i
WHERE OBJECT_NAME(i.object_id) IN ('Members', 'Events', 'Bookings', 'Reviews', 'Inquiries')
  AND i.name IS NOT NULL
  AND OBJECT_SCHEMA_NAME(i.object_id) = 'EVENT_MGMT'
ORDER BY TableName, IndexName;
GO

-- =====================================================
-- SECTION 7: Cleanup Procedures (For Development)
-- =====================================================

-- Stored procedure to reset database (useful for testing)
CREATE PROCEDURE EVENT_MGMT.sp_ResetDatabase
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Disable foreign key constraints explicitly
        ALTER TABLE EVENT_MGMT.Reviews NOCHECK CONSTRAINT ALL;
        ALTER TABLE EVENT_MGMT.Bookings NOCHECK CONSTRAINT ALL;
        
        -- Delete all data in proper order (respecting dependencies)
        DELETE FROM EVENT_MGMT.Reviews;
        DELETE FROM EVENT_MGMT.Bookings;
        DELETE FROM EVENT_MGMT.Inquiries;
        DELETE FROM EVENT_MGMT.Events;
        DELETE FROM EVENT_MGMT.Members;
        
        -- Reset identity seeds
        DBCC CHECKIDENT ('EVENT_MGMT.Reviews', RESEED, 0);
        DBCC CHECKIDENT ('EVENT_MGMT.Bookings', RESEED, 0);
        DBCC CHECKIDENT ('EVENT_MGMT.Inquiries', RESEED, 0);
        DBCC CHECKIDENT ('EVENT_MGMT.Events', RESEED, 0);
        DBCC CHECKIDENT ('EVENT_MGMT.Members', RESEED, 0);
        
        -- Re-enable foreign key constraints
        ALTER TABLE EVENT_MGMT.Reviews WITH CHECK CHECK CONSTRAINT ALL;
        ALTER TABLE EVENT_MGMT.Bookings WITH CHECK CHECK CONSTRAINT ALL;
        
        COMMIT TRANSACTION;
        
        PRINT 'Database reset successfully. All data deleted and identity seeds reset.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO

PRINT 'Stored procedure EVENT_MGMT.sp_ResetDatabase created.';
GO

-- =====================================================
-- SECTION 8: Useful Queries for Monitoring
-- =====================================================

-- Create view for member statistics
CREATE VIEW EVENT_MGMT.vw_MemberStatistics AS
SELECT 
    m.MemberId,
    m.FullName,
    m.Email,
    m.Role,
    COUNT(DISTINCT b.BookingId) AS TotalBookings,
    ISNULL(SUM(b.Quantity), 0) AS TotalTickets,
    COUNT(DISTINCT r.ReviewId) AS TotalReviews,
    AVG(CAST(r.Rating AS FLOAT)) AS AverageRatingGiven
FROM EVENT_MGMT.Members m
LEFT JOIN EVENT_MGMT.Bookings b ON m.MemberId = b.MemberId
LEFT JOIN EVENT_MGMT.Reviews r ON m.MemberId = r.MemberId
GROUP BY m.MemberId, m.FullName, m.Email, m.Role;
GO

PRINT 'View EVENT_MGMT.vw_MemberStatistics created.';
GO

-- Create view for event statistics
CREATE VIEW EVENT_MGMT.vw_EventStatistics AS
SELECT 
    e.EventId,
    e.EventName,
    e.Category,
    e.EventDate,
    e.Venue,
    e.Price,
    e.TotalSeats,
    ISNULL(SUM(b.Quantity), 0) AS BookedSeats,
    e.TotalSeats - ISNULL(SUM(b.Quantity), 0) AS AvailableSeats,
    CASE 
        WHEN e.TotalSeats - ISNULL(SUM(b.Quantity), 0) <= 0 THEN 1 
        ELSE 0 
    END AS IsFull,
    COUNT(DISTINCT b.BookingId) AS TotalBookings,
    COUNT(DISTINCT r.ReviewId) AS TotalReviews,
    AVG(CAST(r.Rating AS FLOAT)) AS AverageRating
FROM EVENT_MGMT.Events e
LEFT JOIN EVENT_MGMT.Bookings b ON e.EventId = b.EventId
LEFT JOIN EVENT_MGMT.Reviews r ON e.EventId = r.EventId
GROUP BY e.EventId, e.EventName, e.Category, e.EventDate, e.Venue, e.Price, e.TotalSeats;
GO

PRINT 'View EVENT_MGMT.vw_EventStatistics created.';
GO

-- =====================================================
-- SECTION 9: Security and Best Practices
-- =====================================================

-- Grant appropriate permissions (adjust based on your application user)
-- Example: GRANT SELECT, INSERT, UPDATE, DELETE ON DATABASE::EventManagementDB TO [YourAppUser];

-- Enable row-level security if needed (advanced feature)
-- Example for multi-tenant scenarios

-- =====================================================
-- FINAL STATUS
-- =====================================================

PRINT '';
PRINT '=====================================================';
PRINT 'DATABASE SETUP COMPLETE';
PRINT '=====================================================';
PRINT '';
PRINT 'Summary:';
PRINT '  - Database: EventManagementDB';
PRINT '  - Schema: EVENT_MGMT';
PRINT '  - Tables Created: 5 (Members, Events, Bookings, Reviews, Inquiries)';
PRINT '  - Indexes Created: 6';
PRINT '  - Views Created: 2 (vw_MemberStatistics, vw_EventStatistics)';
PRINT '  - Stored Procedures: 1 (sp_ResetDatabase)';
PRINT '';
PRINT 'Next Steps:';
PRINT '  1. Start the application';
PRINT '  2. Navigate to: https://localhost:7227/Home/ResetDatabase';
PRINT '  3. Create admin user with hashed password';
PRINT '  4. Login with: admin@culturalcouncil.org / admin123';
PRINT '  5. (Optional) Add sample events through the admin interface';
PRINT '';
PRINT 'Testing Procedures:';
PRINT '  - To view member stats: SELECT * FROM EVENT_MGMT.vw_MemberStatistics;';
PRINT '  - To view event stats: SELECT * FROM EVENT_MGMT.vw_EventStatistics;';
PRINT '  - To reset database: EXEC EVENT_MGMT.sp_ResetDatabase;';
PRINT '';
PRINT 'Connection String for appsettings.json:';
PRINT '  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=EventManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"';
PRINT '';
PRINT '=====================================================';
PRINT 'Setup completed successfully! ?';
PRINT '=====================================================';
GO
