using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TestPackageMinimizer
{
    public static class MoodleHelper
    {
        private static readonly string СontentFile = "files.xml";

        public static void ProccesMoodlePackages(string unpackedDataDirectoryPath)
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

                var filesWithoutExtension = Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)
                    .Where(f => Path.HasExtension(f) == false);

                foreach (XElement fileElement in GetFileElements(contentXDoc))
                {
                    string filename = fileElement.Element("filename").Value;

                    string extension = Path.GetExtension(filename);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        string contenthash = fileElement.Element("contenthash").Value;
                        var sourceFile = filesWithoutExtension.FirstOrDefault(x => x.Contains(contenthash));

                        if (File.Exists(sourceFile))
                        {
                            File.Move(sourceFile, sourceFile + extension);
                        }
                    }
                }
            }
        }

        public static void RemoveExtensionsMoodlePackages(string unpackedDataDirectoryPath)
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

                var filesWithExtension = Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories);

                foreach (XElement fileElement in GetFileElements(contentXDoc))
                {
                    string filename = fileElement.Element("filename").Value;

                    string extension = Path.GetExtension(filename);
                    if (!string.IsNullOrEmpty(extension))
                    {
                        string contenthash = fileElement.Element("contenthash").Value;
                        var sourceFile = filesWithExtension.FirstOrDefault(x => x.Contains(contenthash));

                        string sourceFileWithoutExtension = sourceFile.Replace(extension, "");

                        if (File.Exists(sourceFile))
                        {
                            File.Move(sourceFile, sourceFileWithoutExtension);
                        }
                    }
                }
            }
        }

        private static IEnumerable<XElement> GetFileElements(XDocument xDoc) => xDoc.XPathSelectElements("files/file");
    }
}
