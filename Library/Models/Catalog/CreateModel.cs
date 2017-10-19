using Library.Data.Models;

namespace Library.Models.Catalog
{
    public class CreateModel
    {
        public LibraryAsset Asset { get; set; }
        public string Type { get; set; }
    }
}