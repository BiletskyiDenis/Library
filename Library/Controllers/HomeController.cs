using Library.Data;
using Library.Models.Index;
using Library.Service;
using System.Linq;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        private ILibraryService libraryService;
        public HomeController()
        {
            this.libraryService = new LibraryService(new LibraryContext("name=LibraryDB"));
        }
        public ActionResult Index()
        {
            var books = libraryService
                  .GetBooks()
                  .OrderByDescending(b => b.Id)
                  .Take(3)
                  .Select(b => new RecentlyAddedAsset
                  {
                      Id=b.Id,
                      Title = b.Title,
                      ImageUrl = b.ImageUrl,
                      Description = b.Description
                  });

            var journals = libraryService
                  .GetJournals()
                  .OrderByDescending(b => b.Id)
                  .Take(3)
                  .Select(b => new RecentlyAddedAsset
                  {
                      Id = b.Id,
                      Title = b.Title,
                      ImageUrl = b.ImageUrl,
                      Description = b.Description
                  });

            var broshures = libraryService
                  .GetBrochures()
                  .OrderByDescending(b => b.Id)
                  .Take(3)
                  .Select(b => new RecentlyAddedAsset
                  {
                      Id = b.Id,
                      Title = b.Title,
                      ImageUrl = b.ImageUrl,
                      Description = b.Description
                  });

            return View(new RecentlyAddedAssetsLists
            {
                RecentlyAddedBooks= books,
                RecentlyAddedJournals=journals,
                RecentlyAddedBrochures=broshures
            });
        }

        protected override void Dispose(bool disposing)
        {
            libraryService.Dispose();
        }
    }
}