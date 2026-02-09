# Quick Admin Setup Script

## Generate Admin Password Hash

Run this code in a C# Interactive window or create a simple console app:

```csharp
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

string HashPassword(string password)
{
    byte[] salt = new byte[128 / 8];
    using (var rng = RandomNumberGenerator.Create())
    {
        rng.GetBytes(salt);
    }

    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: password,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 100000,
        numBytesRequested: 256 / 8));

    return $"{Convert.ToBase64String(salt)}.{hashed}";
}

// Generate hash for admin password
string hash = HashPassword("admin123");
Console.WriteLine("Copy this hash:");
Console.WriteLine(hash);
```

## Complete SQL Script

Once you have the hash, run this SQL:

```sql
-- Step 1: Delete everything
DELETE FROM Reviews;
DELETE FROM Bookings;
DELETE FROM Inquiries;
DELETE FROM Members;
DBCC CHECKIDENT ('Members', RESEED, 0);

-- Step 2: Add admin with hashed password
INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES (
    'Administrator',
    'admin@culturalcouncil.org',
    'PASTE_YOUR_GENERATED_HASH_HERE',
    NULL,
    'Admin'
);

-- Verify
SELECT MemberId, FullName, Email, Role, 
       LEFT(Password, 20) + '...' AS PasswordHash
FROM Members;
```

## Test Login

- **Email:** admin@culturalcouncil.org
- **Password:** admin123

The password will be verified using the hash!
