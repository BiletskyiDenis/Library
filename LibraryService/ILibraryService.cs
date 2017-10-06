using System.Collections.Generic;
using System.Web;
using Library.Data.Models;

namespace Library.Service
{
    public interface ILibraryService
    {
        bool DeleteBook(int id);
        void DeleteImage(Book book);
        void Dispose();
        Book GetBook(int? id);
        IEnumerable<Book> GetBooks();
        string GetImageFileName(Book book);
        void InsertBook(Book book, HttpPostedFileBase file);
        void Save();
        void UpdateBook(Book book, HttpPostedFileBase file);
        string UploadImage(Book book, HttpPostedFileBase file);
    }
}