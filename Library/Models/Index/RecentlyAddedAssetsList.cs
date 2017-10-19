using System.Collections.Generic;

namespace Library.Models.Index
{
    public class RecentlyAddedAssetsLists
    {
        public IEnumerable<RecentlyAddedAsset> RecentlyAddedBooks { get; set; }
        public IEnumerable<RecentlyAddedAsset> RecentlyAddedJournals{ get; set; }
        public IEnumerable<RecentlyAddedAsset> RecentlyAddedBrochures { get; set; }
    }
}