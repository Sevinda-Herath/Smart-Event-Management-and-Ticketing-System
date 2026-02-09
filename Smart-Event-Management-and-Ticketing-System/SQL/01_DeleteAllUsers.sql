-- =====================================================
-- SQL Script: Delete All Users and Re-add Admin with Hashed Password
-- =====================================================

-- STEP 1: Delete all related data first (foreign key constraints)
PRINT 'Deleting all reviews...'
DELETE FROM Reviews;

PRINT 'Deleting all bookings...'
DELETE FROM Bookings;

PRINT 'Deleting all inquiries (if they have MemberId)...'
-- Note: Inquiries might not have foreign key to Members, adjust if needed
-- DELETE FROM Inquiries WHERE MemberId IS NOT NULL;

-- STEP 2: Delete all members including admin
PRINT 'Deleting all members...'
DELETE FROM Members;

-- STEP 3: Reset identity seed (optional, to start MemberId from 1)
PRINT 'Resetting identity seed...'
DBCC CHECKIDENT ('Members', RESEED, 0);

PRINT 'All users deleted successfully!'
PRINT ''
PRINT 'Database is now clean. Run the INSERT script next.'
