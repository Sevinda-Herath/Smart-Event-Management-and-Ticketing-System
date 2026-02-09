-- Insert Admin User
-- Run this script in SQL Server Object Explorer to add an admin user

-- Insert Admin User
INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES ('Admin User', 'admin@culturalcouncil.org', 'admin123', NULL, 'Admin');

-- Verify insertion
SELECT * FROM Members WHERE Role = 'Admin';

GO

-- Admin Login Credentials:
-- Email: admin@culturalcouncil.org
-- Password: admin123
