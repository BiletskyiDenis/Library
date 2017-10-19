using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models
{
    public class Book:LibraryAsset
    {
        [Required]
        public string Author { get; set; }
        public string ISBN { get; set; }

        public int Pages { get; set; }

        [Required]
        [Display (Name ="Genre")]
        public override string GenreOrCategory { get; set; }
    }
}