using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models.Index
{
    public class RecentlyAddedAsset
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}