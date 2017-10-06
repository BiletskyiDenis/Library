using Library.Data;
using Library.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            return View(libraryService.GetBooks());
        }

        protected override void Dispose(bool disposing)
        {
            libraryService.Dispose();
        }
    }
}