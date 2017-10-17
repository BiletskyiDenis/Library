using System.Collections.Generic;
using System.Web;
using Library.Data.Models;

namespace Library.Service
{
    public interface ILibraryService
    {
        void AddAsset(LibraryAsset asset, HttpPostedFileBase file);
        bool DeleteAsset(int id);
        void DeleteImage(LibraryAsset asset);
        void Dispose();
        IEnumerable<LibraryAsset> GetAll();
        IEnumerable<string> GetAllTypes();
        string GetAuthor(int id);
        IEnumerable<Book> GetBooks();
        IEnumerable<Brochure> GetBrochures();
        LibraryAsset GetById(int? id);
        string GetFrequency(int id);
        string GetImageFileName(LibraryAsset asset);
        string GetISBN(int id);
        IEnumerable<Journal> GetJournals();
        int GetPages(int id);
        AssetType GetType(int? id);
        void Save();
        void UpdateAsset(LibraryAsset asset, HttpPostedFileBase file);
        bool UploadData(HttpPostedFileBase file);
        string UploadImage(LibraryAsset asset, HttpPostedFileBase file);
    }
}