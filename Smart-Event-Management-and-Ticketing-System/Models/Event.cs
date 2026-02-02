using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Event_Management_and_Ticketing_System.Models
{
    /// <summary>
    /// Represents a cultural event organized by the Metropolitan Cultural Council
    /// </summary>
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(200)]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Event date is required")]
        [Display(Name = "Event Date")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Venue is required")]
        [StringLength(200)]
        public string Venue { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Total seats is required")]
        [Display(Name = "Total Seats")]
        [Range(1, 10000, ErrorMessage = "Total seats must be between 1 and 10000")]
        public int TotalSeats { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Review>? Reviews { get; set; }

        /// <summary>
        /// Calculates the number of booked seats for this event
        /// </summary>
        [NotMapped]
        public int BookedSeats
        {
            get
            {
                return Bookings?.Sum(b => b.Quantity) ?? 0;
            }
        }

        /// <summary>
        /// Calculates the number of available seats
        /// </summary>
        [NotMapped]
        public int AvailableSeats
        {
            get
            {
                return TotalSeats - BookedSeats;
            }
        }

        /// <summary>
        /// Returns whether the event is fully booked
        /// </summary>
        [NotMapped]
        public bool IsFull
        {
            get
            {
                return AvailableSeats <= 0;
            }
        }
    }
}
