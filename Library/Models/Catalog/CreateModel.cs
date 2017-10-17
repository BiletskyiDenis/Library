using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models.Catalog
{
    public class CreateModel
    {
        public LibraryAsset Asset { get; set; }
        public string Type { get; set; }
    }
}