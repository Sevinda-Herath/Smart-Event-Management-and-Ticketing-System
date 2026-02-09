# ? Updated Guest Search - Partial Access Feature

## Feature: Guests Can Use Basic Filters

Guests can now use **basic filters** (Category, Date, Location) but **advanced features** (Price, Keyword Search) remain restricted to members only.

---

## ?? Guest Access Levels

### ? **Guests CAN Use:**
1. **Category Filter** - Browse by event type (Music, Theater, Art, etc.)
2. **Date Filter** - Find events on specific dates
3. **Location Filter** - Search by venue name
4. **Filter Button** - Submit their basic search

### ? **Guests CANNOT Use (Members Only):**
1. **Max Price Filter** - ?? Disabled, greyed out
2. **Advanced Search** - ?? Disabled, greyed out (keyword search)

---

## ?? Visual Design

### Alert Message (Updated)
```
?? Limited Guest Access

You can browse by Category, Date, and Location.
To access price filtering and advanced search, please register or login.

[Register Now] [Login]
```

### Form Fields

#### Enabled Fields (Guests Can Use)
```
Category:    [Dropdown ?]           ? Active
Date:        [Date Picker]          ? Active
Location:    [Text Input]           ? Active
```

#### Disabled Fields (Members Only)
```
Max Price:   [?? Members only]      ? Greyed out
             Register to filter by price

Advanced:    [?? Register to search by keywords]  ? Greyed out
Search:      Members can search event names and descriptions
```

### Button
```
[Filter] - Active for guests (submits basic filters)
```

---

## ?? Technical Implementation

### Enabled Fields (Functional)
```razor
<!-- Category - Active -->
<select name="category" id="category" class="form-control">
    <option value="">All Categories</option>
    @foreach (var cat in ViewBag.Categories)
    {
        <option value="@cat">@cat</option>
    }
</select>

<!-- Date - Active -->
<input type="date" name="date" id="date" class="form-control" 
       value="@ViewBag.SelectedDate" />

<!-- Location - Active -->
<input type="text" name="location" id="location" class="form-control" 
       placeholder="Venue name" value="@ViewBag.SelectedLocation" />
```

### Disabled Fields (Restricted)
```razor
<!-- Max Price - Disabled -->
<label for="maxPrice" class="form-label">
    Max Price <i class="fas fa-lock text-warning"></i>
</label>
<input type="number" name="maxPrice" id="maxPrice" class="form-control" 
       placeholder="Members only" disabled 
       style="opacity: 0.5; cursor: not-allowed;" />
<small class="text-muted">Register to filter by price</small>

<!-- Search Term - Disabled -->
<label for="searchTerm" class="form-label">
    Advanced Search <i class="fas fa-lock text-warning"></i>
</label>
<input type="text" name="searchTerm" id="searchTerm" class="form-control" 
       placeholder="Register to search by keywords" disabled 
       style="opacity: 0.5; cursor: not-allowed;" />
<small class="text-muted">Members can search event names and descriptions</small>
```

---

## ?? User Experience Flow

### Guest Filtering Flow
```
1. Guest visits Browse Events
2. Sees info alert about limited access
3. Selects category from dropdown (? Works)
4. Picks a date (? Works)
5. Types venue name (? Works)
6. Tries to enter price ? Disabled ?
7. Tries to search keywords ? Disabled ?
8. Clicks "Filter" button
9. Page reloads with filtered results
10. Guest sees filtered events
```

### Conversion Flow
```
Guest sees value in basic filters
    ?
Wants price filtering
    ?
Sees lock icon ?? "Members only"
    ?
Reads "Register to filter by price"
    ?
Clicks "Register Now"
    ?
Creates account
    ?
Returns to Browse Events
    ?
All filters now unlocked! ?
```

---

## ?? Feature Comparison

| Filter Feature | Guest | Member |
|----------------|-------|--------|
| **Category** | ? Active | ? Active |
| **Date** | ? Active | ? Active |
| **Location** | ? Active | ? Active |
| **Max Price** | ? Locked ?? | ? Active |
| **Keyword Search** | ? Locked ?? | ? Active |
| **Filter Button** | ? Active (basic) | ? Active (full) |

---

## ?? Testing Scenarios

### Test 1: Guest Basic Filtering
1. **Logout** or open incognito
2. Go to **Browse Events**
3. ? Select "Music" from category
4. ? Pick tomorrow's date
5. ? Type "Hall" in location
6. ? Click "Filter"
7. ? See filtered results

### Test 2: Guest Restricted Fields
1. As guest, try to click **Max Price** field
2. ? Field is disabled, can't type
3. ? Cursor shows "not-allowed" icon
4. ? See lock icon ?? and "Members only" text
5. Try to click **Advanced Search** field
6. ? Field is disabled, can't type
7. ? See help text below field

### Test 3: Member Full Access
1. **Login** as member
2. Go to **Browse Events**
3. ? All filters active and normal
4. ? No disabled fields
5. ? No lock icons
6. ? Can use price filter
7. ? Can use keyword search

---

## ?? Benefits of Partial Access

### 1. **Better Guest Experience**
- Guests can actually use the site
- Not completely blocked from filtering
- See immediate value

### 2. **Clear Value Proposition**
- Guests see what they're missing
- Lock icons clearly indicate premium features
- Helper text explains benefits

### 3. **Conversion Optimization**
- Guests get hooked with basic features
- Want more ? Register
- Soft sell approach

### 4. **Reduced Friction**
- Not forcing registration immediately
- Let users explore first
- Build trust before asking for signup

---

## ?? Visual Elements

### Lock Icons
```
Max Price ??
Advanced Search ??
```
- Gold/Yellow color (`text-warning`)
- Next to label text
- Universal "locked" symbol

### Helper Text
```css
<small class="text-muted">Register to filter by price</small>
<small class="text-muted">Members can search event names and descriptions</small>
```
- Grey, small text
- Below disabled fields
- Explains how to unlock

### Disabled Styling
```css
style="opacity: 0.5; cursor: not-allowed;"
```
- 50% opacity (greyed out)
- Cursor changes to "not-allowed" symbol
- Visually distinct from enabled fields

---

## ?? Backend Behavior

The backend controller already handles partial queries:
- If `maxPrice` is null ? Ignores price filter
- If `searchTerm` is null ? Ignores keyword search
- Guest filters still work with category, date, location

No controller changes needed! ?

---

## ?? Responsive Design

Works on all screen sizes:
- **Desktop:** 4-column layout
- **Tablet:** Fields stack nicely
- **Mobile:** Full-width fields

Disabled fields remain disabled on all devices.

---

## ?? For Coursework Presentation

### Demo Points:

1. **Show Guest View**
   - "Guests can use basic filters"
   - Select category, date, location
   - Show results update

2. **Point Out Restrictions**
   - "But advanced features are locked"
   - Show lock icons
   - Read helper text

3. **Explain Strategy**
   - "This encourages registration"
   - "Guests see value before committing"
   - "Soft conversion approach"

4. **Show Member View**
   - Login as member
   - "Now all features unlocked"
   - Use price filter and search

### Comparison Demo:
```
Guest ? Can filter: Music + Tomorrow + "Hall" ? See events
      ?
      Try price filter ? Locked ??
      ?
      Register
      ?
Member ? Same filters + $50 max + "concert" keyword ? Refined results
```

---

## ? Feature Complete

? **Basic filters active** for guests (Category, Date, Location)  
? **Advanced features locked** (Price, Keyword Search)  
? **Clear visual indicators** (lock icons, greyed out)  
? **Helper text** explains restrictions  
? **Functional filter button** for guests  
? **Conversion prompts** (Register/Login buttons)  
? **Members get full access** unchanged  
? **Build successful**  

---

## ?? Key Improvements Over Full Block

| Aspect | Full Block | Partial Access |
|--------|-----------|----------------|
| Guest Usability | ? Nothing works | ? Basic works |
| Value Demonstration | ? Can't try | ? Can try basics |
| Conversion Approach | ?? Hard sell | ? Soft sell |
| User Experience | ?? Frustrating | ?? Helpful |
| Registration Incentive | ?? Forced | ? Earned |

---

**Ready to test! Press F5 and try filtering as a guest.** ??

Now guests can meaningfully browse events while still seeing the value of membership! ??
