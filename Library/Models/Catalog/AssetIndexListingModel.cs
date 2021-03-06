﻿using Library.Data.Models;

namespace Library.Models.Catalog
{
    public class AssetIndexListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public AssetType Type { get; set; }
        public string Publisher { get; set; }
        public int  NumberOfCopies { get; set; }
        public decimal Price { get; set; }
    }
}