using System.IO;
using System.IO.Abstractions;
using System.Xml.Linq;

namespace TestPackageMinimizer.VendorHelpers
{
    internal static class Common
    {
        public static bool TryLoadFile( string unpackedDataDirectoryPath, string folder, string contentFile, out string folderPath, out XDocument contentXDoc )
        {
            folderPath = Path.Combine( unpackedDataDirectoryPath, folder );
            string contentDocPath = Path.Combine( folderPath, contentFile );

            XDocumentLoader xDocumentLoader = new XDocumentLoader( new FileSystem() );

            if(!xDocumentLoader.TryLoadFile( contentDocPath, out contentXDoc ))
            {
                return false;
            }

            return true;
        }

        public static void MoveFile( string sourceFileNmae, string destFileName )
        {
            if(File.Exists( sourceFileNmae ))
            {
                File.Move( sourceFileNmae, destFileName );
            }
        }
    }
}
