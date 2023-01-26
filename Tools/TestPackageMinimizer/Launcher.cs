using System;
using System.Collections.Generic;
using System.IO;

namespace TestPackageMinimizer
{
    public class Launcher
    {
        private const string UnpackedDataDirectoryPath = @"C:\ForTools\_packages";
        private const string TemplateFilesDirectoryPath = @"C:\ForTools\__Knowledge\_github\Ideas_D2L\Tools\TestPackageMinimizer\BlankFiles";

        private static readonly Dictionary<string, string> ExtensionFileMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
            { ".docx", "empty.docx" },
            { ".doc", "empty.doc" },
            { ".gif", "empty.gif" },
            { ".jpg", "empty.jpg" },
            { ".pdf", "empty.pdf" },
            { ".bmp", "empty.bmp" },
            { ".png", "empty.png" },
            { ".mp4", "short.mp4" },
            { ".pptx", "empty.pptx"}
        };

        public static void Main()
        {
            long size = 0;

            foreach (var fileName in Directory.EnumerateFileSystemEntries(UnpackedDataDirectoryPath, "*", SearchOption.AllDirectories))
            {
                var extension = Path.GetExtension(fileName);

                if (extension == null)
                    continue;

                string emptyFileName;
                if (!ExtensionFileMap.TryGetValue(extension, out emptyFileName))
                    continue;
                emptyFileName = Path.Combine(TemplateFilesDirectoryPath, emptyFileName);

                var fileToReplace = new FileInfo(fileName);
                var emptyFile = new FileInfo(emptyFileName);

                if (fileToReplace.Length == emptyFile.Length)
                    continue;
                if (fileToReplace.Length < emptyFile.Length)
                    throw new Exception("Luck: smaller file was found " + fileName);

                Console.WriteLine(fileName);

                size += fileToReplace.Length - emptyFile.Length;

                fileToReplace.Delete();
                emptyFile.CopyTo(fileName);
            }

            Console.WriteLine("Saved bytes: " + size);
            Console.ReadLine();
        }
    }
}
