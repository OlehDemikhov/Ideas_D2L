using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Xml.Linq;

namespace TestPackageMinimizer
{
    public static class SakaiHelper
    {
        private static readonly string СontentFile = "content.xml";

        public static void ProccesSakaiPackages(string unpackedDataDirectoryPath)
        {
            foreach (var folder in Directory.EnumerateFileSystemEntries(unpackedDataDirectoryPath, "*", SearchOption.TopDirectoryOnly))
            {
                string folderPath = Path.Combine(unpackedDataDirectoryPath, folder);
                string contentDocPath = Path.Combine(folderPath, СontentFile);

                XDocumentLoader s = new XDocumentLoader(new FileSystem());

                if (!s.TryLoadFile(contentDocPath, out XDocument contentXDoc))
                {
                    continue;
                }

                foreach (XElement fileElement in GetFileElements(contentXDoc))
                {

                    string id = fileElement.Attribute("id").Value;
                    string bodyLocation = fileElement.Attribute("body-location").Value;

                    string extension = Path.GetExtension(id);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        var sourceFile = Path.Combine(folderPath, bodyLocation);
                        if (File.Exists(sourceFile))
                        {
                            File.Move(sourceFile, sourceFile + extension);
                        }
                    }
                }
            }
        }

        public static void RemoveExtensionsSakaiPackages(string unpackedDataDirectoryPath)
        {
            foreach (var folder in Directory.EnumerateFileSystemEntries(unpackedDataDirectoryPath, "*", SearchOption.TopDirectoryOnly))
            {
                string folderPath = Path.Combine(unpackedDataDirectoryPath, folder);
                string contentDocPath = Path.Combine(folderPath, СontentFile);

                XDocumentLoader xDocumentLoader = new XDocumentLoader(new FileSystem());

                if (!xDocumentLoader.TryLoadFile(contentDocPath, out XDocument contentXDoc))
                {
                    continue;
                }

                foreach (XElement fileElement in GetFileElements(contentXDoc))
                {

                    string id = fileElement.Attribute("id").Value;
                    string bodyLocation = fileElement.Attribute("body-location").Value;

                    string extension = Path.GetExtension(id);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        var sourceFile = Path.Combine(folderPath, bodyLocation);
                        if (File.Exists(sourceFile + extension))
                        {
                            File.Move(sourceFile + extension, sourceFile);
                        }
                    }
                }
            }
        }

        private static IEnumerable<XElement> GetFileElements(XDocument xDoc) => xDoc.Descendants("resource");
    }
}
