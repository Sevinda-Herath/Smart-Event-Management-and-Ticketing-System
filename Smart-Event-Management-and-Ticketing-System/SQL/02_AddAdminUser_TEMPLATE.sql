-- =====================================================
-- SQL Script: Add Admin User with Hashed Password
-- =====================================================
-- 
-- IMPORTANT: The password 'admin123' has been hashed using PBKDF2
-- 
-- Original Password: admin123
-- Hashed Password: (generated using PasswordHasher.HashPassword("admin123"))
-- 
-- This password was generated using:
-- - Algorithm: PBKDF2 with HMACSHA256
-- - Iterations: 100,000
-- - Salt: Random 128-bit
-- - Hash: 256-bit
--
-- To generate a new hashed password:
-- 1. Run the application
-- 2. Use PasswordHasher.HashPassword("your_password")
-- 3. Copy the result here
-- =====================================================

-- For now, you need to generate the hash programmatically
-- Run this C# code to generate the hash:
-- var hash = Smart_Event_Management_and_Ticketing_System.Helpers.PasswordHasher.HashPassword("admin123");
-- Console.WriteLine(hash);

-- Then update this INSERT statement with the generated hash

INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES (
    'Administrator',
    'admin@culturalcouncil.org',
    'PASTE_HASHED_PASSWORD_HERE',  -- Replace this with the generated hash
    NULL,
    'Admin'
);

PRINT 'Admin user created successfully!'
PRINT 'Email: admin@culturalcouncil.org'
PRINT 'Password: admin123 (hashed)'

-- To verify the admin was created:
SELECT MemberId, FullName, Email, Role, 
       'Password is hashed' AS PasswordStatus
FROM Members 
WHERE Role = 'Admin';
