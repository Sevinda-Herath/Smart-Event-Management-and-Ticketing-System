using System.ComponentModel.DataAnnotations;

namespace Smart_Event_Management_and_Ticketing_System.Models
{
    /// <summary>
    /// Represents an inquiry submitted by a guest or member
    /// </summary>
    public class Inquiry
    {
        [Key]
        public int InquiryId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        [Required]
        [Display(Name = "Inquiry Date")]
        public DateTime InquiryDate { get; set; }
    }
}
