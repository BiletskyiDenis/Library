using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Service
{
    public abstract class FileDataBase
    {
        public abstract byte[] GetTXTFile<T>(T obj) where T : LibraryAsset;
        public abstract byte[] GetXmlFile<T>(T obj) where T : LibraryAsset;
        public abstract byte[] GetTXTListFile<T>(IEnumerable<T> obj) where T : LibraryAsset;
        public abstract byte[] GetXmlListFile<T>(IEnumerable<T> obj) where T : LibraryAsset;
        public abstract LibraryAsset RestoreAssetFromTxt(string doc);
        public abstract LibraryAsset RestoreAssetFromXml(XDocument doc);
        public abstract IEnumerable<LibraryAsset> RestoreAssetsListFromTxt(string doc);
        public abstract IEnumerable<LibraryAsset> RestoreAssetsListFromXml(XDocument doc);
    }
}
