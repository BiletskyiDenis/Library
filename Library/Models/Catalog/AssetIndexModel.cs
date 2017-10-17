using System.Collections.Generic;

namespace Library.Models.Catalog
{
    public class AssetIndexModel
    {
        public IEnumerable<AssetIndexListingModel> Assets { get; set; }
        public IEnumerable<string> Types { get; set; }
    }
}