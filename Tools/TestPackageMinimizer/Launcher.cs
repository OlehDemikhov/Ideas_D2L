using System;
using System.Collections.Generic;
using System.IO;

namespace TestPackageMinimizer
{
    public class Launcher
    {
        private const string UnpackedDataDirectoryPath = @"C:\ForTools\packages";
        private const string TemplateFilesDirectoryPath = @"C:\Users\lkhimiak\source\repos\Ideas_D2L\Tools\TestPackageMinimizer\BlankFiles";


        private static readonly Dictionary<string, string> ExtensionFileMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
            { ".docx", "empty.docx" },
            { ".doc", "empty.doc" },
            { ".gif", "empty.gif" },
            { ".jpg", "empty.jpg" },
            { ".pdf", "empty.pdf" },
            { ".bmp", "empty.bmp" },
            { ".png", "empty.png" },
            { ".mp4", "short.mp4" },
            { ".pptx", "empty.pptx"},
            { ".mov", "short.mov" },
            { ".mp3", "short.mp3" },
            { ".xls", "empty.xls" },
            { ".xlsx", "short.xlsx" },
            { ".zip", "empty.zip" },
        };

        public static void Main()
        {
            long size = 0;

            SakaiHelper.ProccesSakaiPackages(UnpackedDataDirectoryPath);
            MoodleHelper.ProccesMoodlePackages(UnpackedDataDirectoryPath);

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

                if (!fileToReplace.Exists)
                    continue;
                //if (fileToReplace.Length <= emptyFile.Length)
                //    continue;

                Console.WriteLine(fileName);

                size += fileToReplace.Length - emptyFile.Length;

                fileToReplace.Delete();
                emptyFile.CopyTo(fileName);
            }

            SakaiHelper.RemoveExtensionsSakaiPackages(UnpackedDataDirectoryPath);
            MoodleHelper.RemoveExtensionsMoodlePackages(UnpackedDataDirectoryPath);

            Console.WriteLine("Saved bytes: " + size);
            Console.ReadLine();
        }
    }
}
