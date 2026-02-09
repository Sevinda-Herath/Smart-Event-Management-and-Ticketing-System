# ?? Review Filter Quick Reference

## How to Use

### Location
**Event Details Page** ? Right side panel ? **Reviews Section**

---

## Filter Options

### 1. Filter by Stars ?
```
[All Ratings ?]
  ?? All Ratings (default)
  ?? ????? 5 Stars
  ?? ???? 4 Stars
  ?? ??? 3 Stars
  ?? ?? 2 Stars
  ?? ? 1 Star
```

### 2. Search by Keyword ??
```
[Search by keyword or name....]
```
- Searches in review comments
- Searches in reviewer names
- Case-insensitive
- Partial matching

---

## Quick Examples

### Example 1: Find All 5-Star Reviews
```
1. Select: "????? 5 Stars"
2. Click: [Filter]
3. Result: Only 5-star reviews shown
```

### Example 2: Search for Keyword
```
1. Type: "amazing"
2. Click: [Filter]
3. Result: Reviews containing "amazing"
```

### Example 3: Combined Search
```
1. Select: "4 Stars"
2. Type: "performance"
3. Click: [Filter]
4. Result: 4-star reviews about performance
```

### Example 4: Clear All Filters
```
1. Click: [Clear Filters]
2. Result: All reviews shown
```

---

## Visual Indicators

### Average Rating
```
? Reviews (15)
Average Rating: 4.2 ?
```

### Filter Active
```
?? Showing 8 of 15 reviews
```
- Shows when filters are applied
- Displays filtered count vs total

### No Results
```
?? No reviews match your filter criteria.
```

---

## URL Pattern

**No filter:**
```
/Events/Details/5
```

**Star filter:**
```
/Events/Details/5?starFilter=5
```

**Keyword search:**
```
/Events/Details/5?reviewKeyword=great
```

**Combined:**
```
/Events/Details/5?starFilter=4&reviewKeyword=great
```

---

## Testing Checklist

- [ ] Can select different star ratings
- [ ] Can type in search box
- [ ] Filter button works
- [ ] Clear button appears when filtered
- [ ] Clear button resets filters
- [ ] Filtered count shows correctly
- [ ] Average rating displays
- [ ] No results message appears when appropriate
- [ ] Can combine star + keyword
- [ ] Search is case-insensitive
- [ ] Search finds reviewer names
- [ ] Page remembers filter after reload

---

## Common Use Cases

| Goal | Action |
|------|--------|
| See best reviews | Filter by 5 stars |
| Find problems | Filter by 1-2 stars |
| Check specific topic | Search keyword (e.g., "parking") |
| Find friend's review | Search friend's name |
| See recent positive | Filter 5 stars (already sorted by date) |
| Quality check | Search specific words |

---

## Button Guide

| Button | Appearance | Action |
|--------|-----------|--------|
| **Filter** | Blue, Primary | Apply filters |
| **Clear Filters** | Grey, Outline | Reset all filters |

---

**Quick Tip:** Filters preserve state when you reload the page! ??
