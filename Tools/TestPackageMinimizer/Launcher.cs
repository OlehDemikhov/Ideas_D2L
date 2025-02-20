using System;
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
            ConsoleHelper.WriteLineWithColor( "Please, press any key to continue", ConsoleColor.DarkGray );
            Console.ReadLine();
        }
    }
}
