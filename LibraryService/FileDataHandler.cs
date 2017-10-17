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

namespace Library.Service
{
    public class FileDataHandler
    {
        public byte[] GetXml<T>(T serializableObject) where T:LibraryAsset
        {
            if (serializableObject == null) { return null; }
            try
            {
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    return stream.ToArray();
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public byte[] GetTXT<T>(T obj) where T : LibraryAsset
        {
            var type = obj.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var builder = new StringBuilder();

            builder.Append($"[{type.Name}]");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            foreach (var item in fields)
            {
                if (item.Name=="Id")
                {
                    continue;
                }
                builder.Append($"[{item.Name}]{Environment.NewLine}{item.GetValue(obj)}");
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }

        public LibraryAsset RestoreFromTxt(Stream file)
        {
            TextReader tr = new StreamReader(file);
            var data = tr.ReadToEnd();
            var assetData = data.Split('[', ']').Select(s => s.Replace(Environment.NewLine, string.Empty)).ToArray();
            var asset = GetAssetFromType(assetData[1]);

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

        public LibraryAsset RestoreFromXml(Stream file)
        {
            LibraryAsset objectOut = null;
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(file);

                var asset = GetAssetFromType(xmlDocument.DocumentElement.Name);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = asset.GetType();

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (LibraryAsset)Convert.ChangeType(serializer.Deserialize(reader), asset.GetType());
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception)
            {

            }

            return objectOut;
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
