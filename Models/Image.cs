using System.ComponentModel.DataAnnotations;

namespace Imagegallery_ui.Models
{
    public class Image
    {
        public int imageID { get; set; }
        [Required]
        public string imageData { get; set; }
        [Required]
        public string imageDescription { get; set; }
        [Required]
        public string imageLocation { get; set; }
    }
}
