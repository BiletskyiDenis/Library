using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Data.Models
{
    [Serializable]
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