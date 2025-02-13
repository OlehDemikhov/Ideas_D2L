using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using TestPackageMinimizer.VendorHelpers;

namespace TestPackageMinimizer
{
    public static class MoodleHelper
    {
        private static readonly string СontentFile = "files.xml";

        public static void ProccesMoodlePackages(string unpackedDataDirectoryPath)
        {

            foreach (var folder in Directory.EnumerateFileSystemEntries(unpackedDataDirectoryPath, "*", SearchOption.TopDirectoryOnly))
            {

                if (!Common.TryLoadFile( unpackedDataDirectoryPath, folder, СontentFile, out string folderPath, out XDocument contentXDoc))
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
                        Common.MoveFile( sourceFile, sourceFile + extension );
                    }
                }
            }
        }

        public static void RemoveExtensionsMoodlePackages(string unpackedDataDirectoryPath)
        {
            foreach (var folder in Directory.EnumerateFileSystemEntries(unpackedDataDirectoryPath, "*", SearchOption.TopDirectoryOnly))
            {
                if(!Common.TryLoadFile( unpackedDataDirectoryPath, folder, СontentFile, out string folderPath, out XDocument contentXDoc ))
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

                        Common.MoveFile( sourceFile, sourceFileWithoutExtension );
                    }
                }
            }
        }

        private static IEnumerable<XElement> GetFileElements(XDocument xDoc) => xDoc.XPathSelectElements("files/file");
    }
}
