using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using TestPackageMinimizer.VendorHelpers;

namespace TestPackageMinimizer
{
    public static class SakaiHelper
    {
        private static readonly string СontentFile = "content.xml";

        public static void ProccesSakaiPackages( string unpackedDataDirectoryPath )
        {
            foreach(var folder in Directory.EnumerateFileSystemEntries( unpackedDataDirectoryPath, "*", SearchOption.TopDirectoryOnly ))
            {
                if(!Common.TryLoadFile( unpackedDataDirectoryPath, folder, СontentFile, out string folderPath, out XDocument contentXDoc ))
                {
                    continue;
                }

                MoveFiles( folderPath, contentXDoc );
            }
        }

        public static void RemoveExtensionsSakaiPackages( string unpackedDataDirectoryPath )
        {
            foreach(var folder in Directory.EnumerateFileSystemEntries( unpackedDataDirectoryPath, "*", SearchOption.TopDirectoryOnly ))
            {
                if(!Common.TryLoadFile( unpackedDataDirectoryPath, folder, СontentFile, out string folderPath, out XDocument contentXDoc ))
                {
                    continue;
                }

                foreach(XElement fileElement in GetFileElements( contentXDoc ))
                {

                    string bodyLocation = GetBodyLocation( fileElement );
                    string extension = GetExtension( fileElement );
                    if(!string.IsNullOrEmpty( extension ))
                    {
                        var sourceFile = Path.Combine( folderPath, bodyLocation );
                        Common.MoveFile( sourceFile + extension, sourceFile );
                    }
                }
            }
        }

        private static void MoveFiles( string folderPath, XDocument contentXDoc )
        {
            foreach(XElement fileElement in GetFileElements( contentXDoc ))
            {
                string bodyLocation = GetBodyLocation( fileElement );
                string extension = GetExtension( fileElement );
                if(!string.IsNullOrEmpty( extension ))
                {
                    var sourceFile = Path.Combine( folderPath, bodyLocation );
                    Common.MoveFile( sourceFile, sourceFile + extension );
                }
            }
        }

        private static string GetExtension( XElement fileElement )
        {
            string id = fileElement.Attribute( "id" ).Value;
            return Path.GetExtension( id );

        }

        private static string GetBodyLocation( XElement fileElement )
        {
            return fileElement.Attribute( "body-location" ).Value;
        }

        private static IEnumerable<XElement> GetFileElements( XDocument xDoc ) => xDoc.Descendants( "resource" );
    }
}
