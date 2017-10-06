using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Data.Models
{
    public class Book:LibraryAsset
    {
        [Required]
        public long ISBN { get; set; }
        [Required]
        public string Author { get; set; }

        public string Publisher { get; set; }
        public int Pages { get; set; }
    }
}