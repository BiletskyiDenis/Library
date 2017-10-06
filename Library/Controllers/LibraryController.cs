
using Library.Data;
using Library.Data.Models;
using Library.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class LibraryController : Controller
    {
        private ILibraryService libraryService;
        public LibraryController()
        {
            this.libraryService = new LibraryService(new LibraryContext("name=LibraryDB"));
        }

        // GET: Book
        public ActionResult Index()
        {
            return View(libraryService.GetBooks());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = libraryService.GetBook(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["file1"];
                libraryService.InsertBook(book, file);
                libraryService.Save();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = libraryService.GetBook(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["file1"];
                libraryService.UpdateBook(book, file);
                libraryService.Save();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: UserDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = libraryService.GetBook(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: UserDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            libraryService.DeleteBook(id);
            libraryService.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            libraryService.Dispose();
        }
    }
}