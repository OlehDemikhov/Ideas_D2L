using System;
using Vendors = TestPackageMinimizer.VendorDefiner.Vendors;

namespace TestPackageMinimizer
{
    internal static class PackageProccesor
    {
        public static void ProccesPackage( string folder )
        {
            switch(VendorDefiner.DefineVendor( folder ))
            {
                case Vendors.Sakai:
                    {
                        ConsoleHelper.WriteLineWithColor( "Minimization of the package for Sakai vendor: \n", ConsoleColor.Blue );
                        SakaiHelper.ProccesSakaiPackages( Constants.UnpackedDataDirectoryPath );
                        FileReplacer.ReplaceFiles( folder );
                        SakaiHelper.RemoveExtensionsSakaiPackages( Constants.UnpackedDataDirectoryPath );
                        break;
                    }
                case Vendors.Moodle:
                    {
                        ConsoleHelper.WriteLineWithColor( "Minimization of the package for Moodle vendor: \n", ConsoleColor.Blue );
                        Console.Write( "Folder name: " + folder );
                        MoodleHelper.ProccesMoodlePackages( Constants.UnpackedDataDirectoryPath );
                        FileReplacer.ReplaceFiles( Constants.UnpackedDataDirectoryPath );
                        MoodleHelper.RemoveExtensionsMoodlePackages( Constants.UnpackedDataDirectoryPath );
                        break;
                    }
                case Vendors.Other:
                    {
                        ConsoleHelper.WriteLineWithColor( "Minimization of the package for undefined vendor: \n", ConsoleColor.Blue );
                        FileReplacer.ReplaceFiles( folder );
                        break;
                    }
                default:
                    ConsoleHelper.WriteLineWithColor( "Please, press any key to continue", ConsoleColor.DarkGray );
                    Console.ReadLine();
                    break;
            }
        }
    }
}
