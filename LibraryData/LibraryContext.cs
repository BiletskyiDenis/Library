using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Library.Data.Models;
using LibraryData;


namespace Library.Data
{
    public class LibraryContext:DbContext
    {
        public LibraryContext(string connectionString):base(connectionString)
        {
           // Database.SetInitializer(DbInitFactory.DropCreateIfModelChanges());
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Brochure> Brochures { get; set; }
        public DbSet<LibraryAsset> LibraryAssets { get; set; }
    }
}
