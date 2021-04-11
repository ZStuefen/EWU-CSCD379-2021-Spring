using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Web.ViewModels
{
    public class GiftViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name="Title")]
        public string Title { get; set; } = "";
        
        [Display(Name="Description")]
        public string? Description { get; set; } = "";

        [Display(Name="URL")]
        public string? URL { get; set; } = "";
        [Required]
        [Display(Name="Priority")]
        public int Priority { get; set; } = 1;
        [Required]
        [Display(Name="User")]
        public int UserId { get; set; } = 1;
    }
}