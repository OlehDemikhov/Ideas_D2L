using System;
using System.IO;

namespace TestPackageMinimizer
{
    internal static class FileReplacer
    {
        public static void ReplaceFiles( string folder )
        {
            long bytes = 0;
            foreach(var fileName in Directory.EnumerateFileSystemEntries( folder, "*", SearchOption.AllDirectories ))
            {
                var extension = Path.GetExtension( fileName );

                if(extension == null)
                    continue;

                if(!Constants.ExtensionFileMap.TryGetValue( extension, out string emptyFileName )) {
                    if(!extension.Contains("xml") || !extension.Contains( "html") || !extension.Contains( "htm") || !Path.HasExtension( fileName ))
                    {
                        ConsoleHelper.WriteLineWithColor( "Undefined file extention " + extension, ConsoleColor.Red );
                    }
                    
                    continue;
                }
                emptyFileName = Path.Combine( Constants.TemplateFilesDirectoryPath, emptyFileName );

                var fileToReplace = new FileInfo( fileName );
                var emptyFile = new FileInfo( emptyFileName );

                if(!fileToReplace.Exists)
                    continue;

                Console.WriteLine(" File \"" + fileName + "\" replaced with empty" );

                bytes += fileToReplace.Length - emptyFile.Length;

                fileToReplace.Delete();
                emptyFile.CopyTo( fileName );
            }
            double megabytes = bytes / (1024.0 * 1024.0);
            ConsoleHelper.WriteLineWithColor( "\nSaved MBs: " + megabytes.ToString("N2"), ConsoleColor.Blue );
        }
    }
}
