using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Library.Helpers
{
    public static class LibraryHelpers
    {
        public static MvcHtmlString ImageHelper(this HtmlHelper html, string Path, string fileName, string defaultFileName)
        {
            var fileNamePath = Path+fileName;
            var checkPath = HostingEnvironment.MapPath(fileNamePath);
            var imgTag = new TagBuilder("img");
            var attribustes = new Dictionary<string, string>();

            if (fileName!="none"&& File.Exists(checkPath))
            {
                imgTag.MergeAttribute("src", fileNamePath);
                imgTag.MergeAttribute("alt", fileName.Split('.')[0]);
            }
            else
            {
                imgTag.MergeAttribute("src", defaultFileName);
            }

            return new MvcHtmlString(imgTag.ToString());  
        }


    }
}