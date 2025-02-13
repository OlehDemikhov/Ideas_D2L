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
                        ConsoleHelper.WriteLineWithColor( "Minimization of the package for Sakai vendor", ConsoleColor.Blue );
                        SakaiHelper.ProccesSakaiPackages( Constants.UnpackedDataDirectoryPath );
                        FileReplacer.ReplaceFiles();
                        SakaiHelper.RemoveExtensionsSakaiPackages( Constants.UnpackedDataDirectoryPath );
                        break;
                    }
                case Vendors.Moodle:
                    {
                        ConsoleHelper.WriteLineWithColor( "Minimization of the package for Moodle vendor", ConsoleColor.Blue );
                        MoodleHelper.ProccesMoodlePackages( Constants.UnpackedDataDirectoryPath );
                        FileReplacer.ReplaceFiles();
                        MoodleHelper.RemoveExtensionsMoodlePackages( Constants.UnpackedDataDirectoryPath );
                        break;
                    }
                case Vendors.Other:
                    {
                        ConsoleHelper.WriteLineWithColor( "Minimization of the package for undefined vendor", ConsoleColor.Blue );
                        FileReplacer.ReplaceFiles();
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
