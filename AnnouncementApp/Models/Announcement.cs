using System.ComponentModel.DataAnnotations;

namespace AnnouncementApp.Models
{
    public class Announcement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description must be at most 500 characters.")]
        public string Description { get; set; }

        [Display(Name = "Announcement Picture")]
        public string PictureURL { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
