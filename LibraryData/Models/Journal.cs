using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public class Journal : LibraryAsset
    {
        [Required]
        public string Frequency { get; set; }

        [Required]
        [Display(Name = "Categories")]
        public override string GenreOrCategory { get; set; }
    }
}
