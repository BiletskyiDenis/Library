using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public abstract class LibraryAsset
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }

        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }

        [Required]
        [DisplayName("Price ($)")]
        public decimal Price { get; set; }

        [Required]
        public virtual string GenreOrCategory{ get; set; }

        [Required]
        [Display(Name ="Count")]
        public int NumbersOfCopies { get; set; }
    }
}