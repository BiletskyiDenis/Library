using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using Library.Data.Models;
using System.Web;
using System.Xml.Linq;
using System.Globalization;

namespace Library.Service
{
    public class FileDataHandler
    {
        private XElement GetXmlElement<T>(T obj) where T : LibraryAsset
        {
            var type = obj.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var asset = new XElement(type.Name);

            foreach (var item in fields)
            {
                if (item.Name == "Id")
                {
                    continue;
                }
                asset.Add(new XElement(item.Name, item.GetValue(obj)));
            }
            return asset;
        }

        private string GetTXT<T>(T obj) where T : LibraryAsset
        {
            var type = obj.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var builder = new StringBuilder();

            builder.Append($"#[{type.Name}]");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            foreach (var item in fields)
            {
                if (item.Name == "Id")
                {
                    continue;
                }
                builder.Append($"[{item.Name}]{Environment.NewLine}{item.GetValue(obj)}");
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();
        }

        public byte[] GetXmlFile<T>(T obj) where T : LibraryAsset
        {
            var doc = new XDocument();
            doc.Add(GetXmlElement(obj));
            return Encoding.UTF8.GetBytes(doc.ToString());
        }

        public byte[] GetXmlListFile<T>(IEnumerable<T> obj) where T : LibraryAsset
        {
            var doc = new XDocument();
            var list = new XElement("List");
            foreach (var item in obj)
            {
                list.Add(GetXmlElement(item));
            }
            doc.Add(list);
            return Encoding.UTF8.GetBytes(doc.ToString());
        }

        public byte[] GetTXTListFile<T>(IEnumerable<T> obj) where T : LibraryAsset
        {
            var doc = new StringBuilder();
            doc.Append("[List]");
            doc.Append(Environment.NewLine);

            foreach (var item in obj)
            {
                doc.Append(GetTXT(item));
            }

            return Encoding.UTF8.GetBytes(doc.ToString());
        }

        public byte[] GetTXTFile<T>(T obj) where T : LibraryAsset
        {
            return Encoding.UTF8.GetBytes(GetTXT(obj));
        }

        private LibraryAsset RestoreFromTxt(string data)
        {
            var assetData = data.Split('[', ']').Select(s => s.Replace(Environment.NewLine, string.Empty)).ToArray();
            var asset = GetAssetFromType(assetData[1]);

            if (asset==null)
            {
                return null;
            }

            var type = asset.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                for (int i = 0; i < assetData.Length; i++)
                {
                    if (field.Name != "Id" && field.Name == assetData[i])
                    {
                        field.SetValue(asset, Convert.ChangeType(assetData[i + 1], field.PropertyType));
                        break;
                    }
                }
            }

            return asset;

        }

        private LibraryAsset RestoreFromXml(XElement doc)
        {
            if (doc.Name == null)
            { return null; }

            var asset = GetAssetFromType(doc.Name.ToString());

            if (asset==null)
            { return null; }

            var type = asset.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.Name == "Id")
                    continue;

                foreach (var item in doc.Elements())
                {
                    if (field.Name == item.Name)
                    {
                        field.SetValue(asset, Convert.ChangeType(item.Value, field.PropertyType,CultureInfo.InvariantCulture));
                        break;
                    }
                }
            }

            return asset;
        }

        public LibraryAsset RestoreAssetFromXml(XDocument doc)
        {
            return RestoreFromXml(doc.Elements().FirstOrDefault());
        }

        public LibraryAsset RestoreAssetFromTxt(string doc)
        {
            return RestoreFromTxt(doc);
        }

        public IEnumerable<LibraryAsset> RestoreAssetsListFromXml(XDocument doc)
        {
            if (doc.Elements().FirstOrDefault().Name == null
                && doc.Elements().FirstOrDefault().Name!="List")
            { return null; }

            return doc.Elements().Elements().Select(s => RestoreFromXml(s));
        }

        public IEnumerable<LibraryAsset> RestoreAssetsListFromTxt(string doc)
        {
            var data = doc.Split('#').Where((s,i)=>i>0).ToArray();
            return data.Select(s=> RestoreFromTxt(s));
        }

        public LibraryAsset GetAssetFromType(string type)
        {
            AssetType assetType;
            try
            {
                assetType = (AssetType)Enum.Parse(typeof(AssetType), type);
            }
            catch (Exception)
            {
                return null;
            }

            if (assetType == AssetType.Book)
                return new Book();

            if (assetType == AssetType.Brochure)
                return new Brochure();

            if (assetType == AssetType.Journal)
                return new Journal();

            return null;
        }

    }
}
