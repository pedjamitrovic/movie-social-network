using System.ComponentModel.DataAnnotations;

namespace MovieSocialNetworkApi.Models
{
    public class ChangeDescriptionCommand
    {
        [Required]
        public string Description { get; set; }
    }
}
