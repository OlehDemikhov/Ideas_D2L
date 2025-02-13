using System.IO;


namespace TestPackageMinimizer
{
    public class Launcher
    {
        public static void Main()
        {
            foreach(var folder in Directory.EnumerateFileSystemEntries( Constants.UnpackedDataDirectoryPath, "*", SearchOption.TopDirectoryOnly ))
            {
                PackageProccesor.ProccesPackage( folder );

            }
        }
    }
}
