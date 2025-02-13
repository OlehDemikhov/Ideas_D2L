using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestPackageMinimizer
{
    internal static class VendorDefiner
    {
        public enum Vendors
        {
            Moodle,
            Sakai,
            Other,
            Undefined
        }

        public static Vendors DefineVendor( string folder )
        {
            if(Path.HasExtension( folder ))
            {
                ConsoleHelper.WriteLineWithColor( "Please, unzipe folder", ConsoleColor.Red );
                return Vendors.Undefined;
            }
            if(File.Exists( Path.Combine( folder, Constants.MoodleManifestFileName ) ))
            {
                return Vendors.Moodle;
            }
            if(Directory.EnumerateFiles( folder, "*.xml", SearchOption.AllDirectories ).Any( IsSakaiFile ))
            {
                return Vendors.Sakai;
            }
            return Vendors.Other;
        }

        private static bool IsSakaiFile( string path ) =>
           Constants.Files.Contains( Path.GetFileName( path ) ) &&
           Regex.IsMatch( File.ReadAllText( path ), "<archive[\\d\\D]*system*=*\"[Sakai][\\d\\D]*\"" );
    }
}
