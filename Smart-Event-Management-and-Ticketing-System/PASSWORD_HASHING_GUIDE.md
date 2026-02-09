# ?? Password Hashing Implementation Guide

## ? What Was Implemented

### 1. Password Hashing System
- ? **PasswordHasher Helper** - PBKDF2 with HMACSHA256
- ? **100,000 iterations** - Industry standard
- ? **Random salt** - Unique for each password
- ? **Secure storage** - Salt + Hash combined

### 2. Updated Controllers
- ? **Registration** - Hashes passwords before saving
- ? **Login** - Verifies password against hash
- ? **Admin Edit Member** - Optional password change with hashing

### 3. Updated Views
- ? **EditMember** - New password field (optional)
- ? **Hidden field** - Preserves existing hash
- ? **Clear instructions** - Leave empty to keep current password

---

## ?? How to Delete All Users and Re-add Admin

### Method 1: Using SQL Scripts (Recommended)

#### Step 1: Generate Admin Password Hash

**Option A: Use the Password Generator Program**
```bash
# Navigate to project directory
cd Smart-Event-Management-and-Ticketing-System

# Run the generator (you may need to build first)
dotnet build
cd Utilities
# Copy the hash generator code and run it manually
```

**Option B: Generate Hash Programmatically**

Add this temporary code to your `Program.cs` or create a test endpoint:

```csharp
using Smart_Event_Management_and_Ticketing_System.Helpers;

// Generate hash for admin password
var hash = PasswordHasher.HashPassword("admin123");
Console.WriteLine("Admin Password Hash:");
Console.WriteLine(hash);
```

Run the application and copy the output hash.

**Option C: Use SQL Server Management Studio**

1. Open SQL Server Management Studio
2. Connect to your database
3. Open a new query window
4. Run this C# code externally to get the hash, then use in SQL

#### Step 2: Delete All Users

```sql
-- Connect to your database
USE [YourDatabaseName];
GO

-- Delete all related data first
DELETE FROM Reviews;
DELETE FROM Bookings;
DELETE FROM Members;

-- Reset identity
DBCC CHECKIDENT ('Members', RESEED, 0);
```

Or run the provided script:
```bash
SQL/01_DeleteAllUsers.sql
```

#### Step 3: Add Admin with Hashed Password

```sql
USE [YourDatabaseName];
GO

INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
VALUES (
    'Administrator',
    'admin@culturalcouncil.org',
    'YOUR_GENERATED_HASH_HERE',  -- Replace with actual hash
    NULL,
    'Admin'
);
```

---

### Method 2: Using Application Endpoint (Quick Method)

Create a temporary endpoint in your application:

1. Add this to `AccountController.cs`:

```csharp
// TEMPORARY - Remove after use
[HttpGet]
public async Task<IActionResult> ResetDatabase()
{
    // Delete all data
    _context.Reviews.RemoveRange(_context.Reviews);
    _context.Bookings.RemoveRange(_context.Bookings);
    _context.Members.RemoveRange(_context.Members);
    await _context.SaveChangesAsync();

    // Add admin with hashed password
    var admin = new Member
    {
        FullName = "Administrator",
        Email = "admin@culturalcouncil.org",
        Password = PasswordHasher.HashPassword("admin123"),
        Role = "Admin"
    };
    _context.Members.Add(admin);
    await _context.SaveChangesAsync();

    return Content("Database reset! Admin created with hashed password.");
}
```

2. Navigate to: `https://localhost:7227/Account/ResetDatabase`
3. **IMPORTANT:** Remove this method after use!

---

### Method 3: Using Package Manager Console

1. Open Package Manager Console in Visual Studio
2. Run these commands:

```powershell
# Delete all data
Invoke-Sqlcmd -Query "DELETE FROM Reviews; DELETE FROM Bookings; DELETE FROM Members;" -ServerInstance "YourServer" -Database "YourDatabase"

# Add admin (after generating hash)
Invoke-Sqlcmd -Query "INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role) VALUES ('Administrator', 'admin@culturalcouncil.org', 'YOUR_HASH_HERE', NULL, 'Admin');" -ServerInstance "YourServer" -Database "YourDatabase"
```

---

## ?? Password Hashing Technical Details

### Algorithm: PBKDF2 (Password-Based Key Derivation Function 2)

**Configuration:**
- **PRF:** HMACSHA256 (Pseudorandom Function)
- **Iterations:** 100,000
- **Salt Size:** 128 bits (16 bytes)
- **Hash Size:** 256 bits (32 bytes)

### Storage Format

Password stored in database as: `{Base64Salt}.{Base64Hash}`

Example:
```
kR9Xp2vN8FqY1zLmT3wJ5g==.xH4KpL8+mN9vQr2sT7uVwYz1aB3cD6eF
         ?                           ?
       Salt                       Hash
```

### Security Benefits

? **Salt prevents rainbow table attacks**  
? **High iteration count slows brute force**  
? **Industry-standard algorithm (NIST approved)**  
? **One-way function (cannot reverse)**  
? **Unique hash for each password**  

---

## ?? Testing Password Hashing

### Test 1: Register New User
1. Go to registration page
2. Register with email: `test@test.com`, password: `test123`
3. Check database:
   ```sql
   SELECT Password FROM Members WHERE Email = 'test@test.com';
   ```
4. ? Password should be a long hash string (not "test123")

### Test 2: Login with Hashed Password
1. Login with the test account
2. ? Should login successfully

### Test 3: Edit Member Password
1. Login as admin
2. Go to Members ? Edit a member
3. Enter new password in "New Password" field
4. Save
5. Logout and login as that member with new password
6. ? Should work

### Test 4: Edit Member Without Password Change
1. Login as admin
2. Edit a member but leave password field empty
3. Save
4. ? Member should still be able to login with old password

---

## ?? Quick Start Guide

### To Reset Database and Add Admin:

1. **Generate Admin Hash:**
   ```csharp
   var hash = PasswordHasher.HashPassword("admin123");
   // Copy the output
   ```

2. **Run SQL:**
   ```sql
   DELETE FROM Reviews;
   DELETE FROM Bookings;
   DELETE FROM Members;
   DBCC CHECKIDENT ('Members', RESEED, 0);
   
   INSERT INTO Members (FullName, Email, Password, PreferredCategory, Role)
   VALUES ('Administrator', 'admin@culturalcouncil.org', 
           'PASTE_HASH_HERE', NULL, 'Admin');
   ```

3. **Test:**
   - Login: `admin@culturalcouncil.org`
   - Password: `admin123`
   - ? Should work!

---

## ?? Security Best Practices Implemented

? **Never store plain text passwords**  
? **Use strong hashing algorithm (PBKDF2)**  
? **Unique salt per password**  
? **High iteration count (100,000)**  
? **No password visible in edit forms**  
? **Optional password updates**  
? **Admin can reset passwords securely**  

---

## ?? Files Created/Modified

### Created:
1. `Helpers/PasswordHasher.cs` - Password hashing utilities
2. `SQL/01_DeleteAllUsers.sql` - Clean database script
3. `SQL/02_AddAdminUser_TEMPLATE.sql` - Admin creation template
4. `Utilities/GeneratePasswordHash.cs` - Hash generator utility

### Modified:
5. `Controllers/AccountController.cs` - Registration and login with hashing
6. `Controllers/AdminController.cs` - Edit member with optional password change
7. `Views/Admin/EditMember.cshtml` - New password field instead of visible password

---

## ?? Important Notes

1. **Existing Users Cannot Login:**
   - Old users have plain text passwords
   - New system expects hashed passwords
   - Must delete all users and re-register OR migrate passwords

2. **Migration Script (If Needed):**
   ```csharp
   // Run this once to hash all existing passwords
   var members = await _context.Members.ToListAsync();
   foreach (var member in members)
   {
       if (!member.Password.Contains(".")) // Not already hashed
       {
           member.Password = PasswordHasher.HashPassword(member.Password);
       }
   }
   await _context.SaveChangesAsync();
   ```

3. **Admin Accounts:**
   - You can edit admin password in database directly
   - Or temporarily remove admin check in EditMember

---

## ? Verification Checklist

After implementing:
- [ ] Build successful
- [ ] Can register new user
- [ ] Can login with new user
- [ ] Password in database is hashed (long string with ".")
- [ ] Cannot login with wrong password
- [ ] Admin can edit member without changing password
- [ ] Admin can change member password
- [ ] Old users deleted
- [ ] New admin created with hashed password
- [ ] Can login as admin with "admin123"

---

**Implementation Complete! Password hashing is now active.** ???
