using System;
using System.IO;

namespace TestPackageMinimizer
{
    internal static class FileReplacer
    {
        public static void ReplaceFiles()
        {
            long size = 0;
            foreach(var fileName in Directory.EnumerateFileSystemEntries( Constants.UnpackedDataDirectoryPath, "*", SearchOption.AllDirectories ))
            {
                var extension = Path.GetExtension( fileName );

                if(extension == null)
                    continue;

                if(!Constants.ExtensionFileMap.TryGetValue( extension, out string emptyFileName ))
                    continue;
                emptyFileName = Path.Combine( Constants.TemplateFilesDirectoryPath, emptyFileName );

                var fileToReplace = new FileInfo( fileName );
                var emptyFile = new FileInfo( emptyFileName );

                if(!fileToReplace.Exists)
                    continue;

                Console.WriteLine( fileName );

                size += fileToReplace.Length - emptyFile.Length;

                fileToReplace.Delete();
                emptyFile.CopyTo( fileName );
            }
            ConsoleHelper.WriteLineWithColor( "Saved bytes: " + size, ConsoleColor.Blue );
            ConsoleHelper.WriteLineWithColor( "Please, press any key to continue", ConsoleColor.DarkGray );
            Console.ReadLine();
        }
    }
}
