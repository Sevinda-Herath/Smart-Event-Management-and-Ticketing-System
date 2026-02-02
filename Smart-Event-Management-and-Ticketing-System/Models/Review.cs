using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Event_Management_and_Ticketing_System.Models
{
    /// <summary>
    /// Represents a review submitted by a member for an event they attended
    /// </summary>
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        [Required]
        [Display(Name = "Review Date")]
        public DateTime ReviewDate { get; set; }

        // Navigation properties
        [ForeignKey("MemberId")]
        public Member? Member { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }
    }
}
