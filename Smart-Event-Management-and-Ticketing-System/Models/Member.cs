using System.ComponentModel.DataAnnotations;

namespace Smart_Event_Management_and_Ticketing_System.Models
{
    /// <summary>
    /// Represents a registered member who can book tickets and submit reviews
    /// </summary>
    public class Member
    {
        [Key]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(50)]
        [Display(Name = "Preferred Category")]
        public string? PreferredCategory { get; set; }

        // Navigation properties
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
