using Library.Data;
using Library.Data.Models;
using Library.Models.Catalog;
using Library.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ILibraryService assets;
        public CatalogController()
        {
            this.assets = new LibraryService(new LibraryContext("name=LibraryDB"));
        }
        // GET: Catalog
        public ActionResult Index()
        {
            var assetModels = assets.GetAll();
            var listingResult = assetModels.Select(
                result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    Title = result.Title,
                    Author = assets.GetAuthor(result.Id),
                    ImageUrl = result.ImageUrl,
                    Price = result.Price,
                    NumberOfCopies = result.NumbersOfCopies,
                    Type = assets.GetType(result.Id),
                    Publisher = result.Publisher
                });

            var model = new AssetIndexModel
            {
                Assets = listingResult,
                Types = assets.GetAllTypes()
            };

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            var asset = assets.GetById(id);
            var model = new DetailModel
            {
                Id = asset.Id,
                Title = asset.Title,
                Author = assets.GetAuthor(asset.Id),
                Country = asset.Country,
                Language = asset.Language,
                Description = asset.Description,
                Frequency = assets.GetFrequency(asset.Id),
                GenreOrCategory = asset.GenreOrCategory,
                ImageUrl = asset.ImageUrl,
                ISBN = assets.GetISBN(asset.Id),
                NumberOfCopies = asset.NumbersOfCopies,
                Pages = assets.GetPages(asset.Id),
                Publisher = asset.Publisher,
                Type = assets.GetType(asset.Id),
                Year = asset.Year,
                Price = asset.Price

            };

            return View(model);
        }

        public ActionResult AddAsset(string type)
        {
            AssetType assetType;
            try
            {
                assetType = (AssetType)Enum.Parse(typeof(AssetType), type, true);
            }
            catch (Exception)
            {

                return HttpNotFound();
            }

            if (assetType == AssetType.Book)
                return View("AddAssets/AddBook");

            if (assetType == AssetType.Brochure)
                return View("AddAssets/AddBrochure");

            if (assetType == AssetType.Journal)
                return View("AddAssets/AddJournal");

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBook(Book book, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                AddAsset(book, formCollection);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddJournal(Journal journal, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                AddAsset(journal, formCollection);
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBrochure(Brochure brochure, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                AddAsset(brochure, formCollection);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var asset = assets.GetById(id);
            if (asset == null)
            {
                return HttpNotFound();
            }

            var assetType = assets.GetType(id);

            if (assetType == AssetType.Book)
                return View("EditAssets/EditBook", asset);

            if (assetType == AssetType.Brochure)
                return View("EditAssets/EditBrochure", asset);

            if (assetType == AssetType.Journal)
                return View("EditAssets/EditJournal", asset);

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook(Book book, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                EditAsset(book, formCollection);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJournal(Journal journal, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                EditAsset(journal, formCollection);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBrochure(Brochure brochure, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                EditAsset(brochure, formCollection);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var asset = assets.GetById(id);
            if (asset == null)
            {
                return HttpNotFound();
            }

            var model = new AssetDeleteConfirmModel
            {
                Id = asset.Id,
                Author = assets.GetAuthor(asset.Id),
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                Type = assets.GetType(asset.Id).ToString()
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            assets.DeleteAsset(id);
            assets.Save();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [ChildActionOnly]
        private void AddAsset(LibraryAsset asset, FormCollection formCollection)
        {
            HttpPostedFileBase file = Request.Files["file"];
            assets.AddAsset(asset, file);
            assets.Save();
        }

        [ChildActionOnly]
        private void EditAsset(LibraryAsset asset, FormCollection formCollection)
        {
            HttpPostedFileBase file = Request.Files["file"];
            assets.UpdateAsset(asset, file);
            assets.Save();
        }

        public ActionResult DownloadData(string type, int? id)
        {
            var asset = assets.GetById(id);
            if (id == null || asset == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var fileData = new FileDataHandler();
            byte[] downFile = new byte[0];

            if (type == "xml")
            {
                downFile = fileData.GetXml(asset);
            }

            if (type == "txt")
            {
                downFile = fileData.GetTXT(asset);
            }

            var filename = Regex.Replace(asset.Title, @"[^a-zA-z0-9]+", String.Empty) + "." + type;

            return File(downFile, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        public ActionResult UploadData(FormCollection formCollection)
        {
            HttpPostedFileBase file = Request.Files["upFile"];
            if (!assets.UploadData(file))
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            };
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            assets.Dispose();
        }
    }
}