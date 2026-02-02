using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Event_Management_and_Ticketing_System.Models
{
    /// <summary>
    /// Represents a ticket booking made by a member
    /// </summary>
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Seat type is required")]
        [StringLength(20)]
        [Display(Name = "Seat Type")]
        public string SeatType { get; set; } // Standard or VIP

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }

        // Navigation properties
        [ForeignKey("MemberId")]
        public Member? Member { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }
    }
}
