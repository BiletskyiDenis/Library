using Library.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryData
{
    public static class DbInitFactory
    {
        public static DropCreateDatabaseAlways<LibraryContext> DropCreateAlways()
        {
            return new DropCreateDatabaseAlways<LibraryContext>();
        }

        public static DropCreateDatabaseIfModelChanges<LibraryContext> DropCreateIfModelChanges()
        {
            return new DropCreateDatabaseIfModelChanges<LibraryContext>();
        }
    }
}
