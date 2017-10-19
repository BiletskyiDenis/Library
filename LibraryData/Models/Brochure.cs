using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public class Brochure: LibraryAsset
    {
        [Required]
        [Display(Name = "Categories")]
        public override string GenreOrCategory { get; set; }
    }
}
