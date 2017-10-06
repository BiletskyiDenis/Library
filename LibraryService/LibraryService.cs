using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Library.Data.Models;
using Library.Data;
using System.Web;
using System.IO;
using System.Web.Hosting;
using System.Text.RegularExpressions;

namespace Library.Service
{
    public class LibraryService : ILibraryService
    {
        private LibraryContext context;
        private readonly string imageStorePath = @"~/img/books/";
        private readonly string imageSmallStorePath = @"~/img/books_small/";

        public LibraryService(LibraryContext context)
        {
            this.context = context;
        }
        public bool DeleteBook(int id)
        {
            var book = context.Books.Find(id);
            if (book == null)
            {
                return false;
            }

            DeleteImage(book);
            context.Books.Remove(book);

            return true;
        }

        public Book GetBook(int? id)
        {
            return context.Books.Find(id);
        }

        public IEnumerable<Book> GetBooks()
        {
            return context.Books.ToList();
        }

        public void InsertBook(Book book, HttpPostedFileBase file)
        {
            try
            {
                book.ImageUrl = UploadImage(book,file);
            }
            catch (Exception)
            {
                book.ImageUrl = "none";
            }

            context.Books.Add(book);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateBook(Book book, HttpPostedFileBase file)
        {
            if (file != null && file.FileName != string.Empty)
            {
                DeleteImage(book);
                book.ImageUrl = UploadImage(book,file);
            }
            context.Entry(book).State = EntityState.Modified;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public string UploadImage(Book book, HttpPostedFileBase file)
        {
            var fileName = GetImageFileName(book);
            byte[] data = new byte[] { };
            if (file != null)
            {
                fileName += Path.GetExtension(file.FileName);
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    data = binaryReader.ReadBytes(file.ContentLength);
                }

                using (var stream = new MemoryStream(data))
                {
                    var pathBig = HostingEnvironment.MapPath(imageStorePath + fileName);
                    var pathSmall = HostingEnvironment.MapPath(imageSmallStorePath + fileName);
                    new ImageHandler(stream, 250).ImgSource.Save(pathBig);
                    new ImageHandler(stream, 50).ImgSource.Save(pathSmall);
                }
            }
            return fileName;

        }
        public void DeleteImage(Book book)
        {
            var pathBig = HostingEnvironment.MapPath(imageStorePath + book.ImageUrl);
            var pathSmall = HostingEnvironment.MapPath(imageSmallStorePath + book.ImageUrl);
            if (book.ImageUrl == "none")
                return;

            if (File.Exists(pathBig))
                File.Delete(pathBig);

            if (File.Exists(pathSmall))
                File.Delete(pathSmall);

            book.ImageUrl = "none";
        }

        public string GetImageFileName(Book book)
        {
            var dest = string.Empty;

            var raw = new string[]
            {
                book.Title,
                book.Publisher,
                book.Pages.ToString(),
                book.Year.ToString(),
            };
            foreach (var item in raw)
            {
                var tmpItem = Regex.Replace(item, @"[^a-zA-z0-9]+", String.Empty);
                if (tmpItem.Length > 40)
                {
                    tmpItem = tmpItem.Substring(0, 40);
                }
                dest += tmpItem;
            }
            return dest;
        }
    }
}
