using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Models
{
    [Serializable]
    public class Journal : LibraryAsset
    {
        [Required]
        public string Frequency { get; set; }

        [Required]
        [Display(Name = "Categories")]
        public override string GenreOrCategory { get; set; }
    }
}
