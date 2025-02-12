using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace TestPackageMinimizer
{
    public class Launcher
    {
        private const string UnpackedDataDirectoryPath = @"C:\ForTools\packages";
        private const string TemplateFilesDirectoryPath = @"C:\Users\lkhimiak\source\repos\Ideas_D2L\Tools\TestPackageMinimizer\BlankFiles";
        internal const string MoodleManifestFileName = "moodle_backup.xml";
        private static readonly ImmutableHashSet<string> Files = new HashSet<string>( StringComparer.OrdinalIgnoreCase ) {
            "announcement.xml",
            "assignment.xml",
            "attachment.xml",
            "basiclti.xml",
            "calendar.xml",
            "chat.xml",
            "content.xml",
            "email.xml",
            "lessonbuilder.xml",
            "melete.xml",
            "messageforum.xml",
            "poll.xml",
            "samigo.xml",
            "site.xml",
            "syllabus.xml",
            "user.xml",
            "web.xml",
            "wiki.xml",
            "gradebook2.xml"
        }.ToImmutableHashSet();


        private static readonly Dictionary<string, string> ExtensionFileMap = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase ) {
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
            foreach(var folder in Directory.EnumerateFileSystemEntries( UnpackedDataDirectoryPath, "*", SearchOption.TopDirectoryOnly ))
            {
                ProccesPackage( folder );

            }
        }

        private static void ProccesPackage( string folder )
        {
            switch(DefineVendor( folder ))
            {
                case Vendors.Sakai:
                    {
                        WriteLineWithColor( "Minimization of the package for Sakai vendor", ConsoleColor.Blue );
                        SakaiHelper.ProccesSakaiPackages( UnpackedDataDirectoryPath );
                        ReplaceFiles();
                        SakaiHelper.RemoveExtensionsSakaiPackages( UnpackedDataDirectoryPath );
                        break;
                    }
                case Vendors.Moodle:
                    {
                        WriteLineWithColor( "Minimization of the package for Moodle vendor", ConsoleColor.Blue );
                        MoodleHelper.ProccesMoodlePackages( UnpackedDataDirectoryPath );
                        ReplaceFiles();
                        MoodleHelper.RemoveExtensionsMoodlePackages( UnpackedDataDirectoryPath );
                        break;
                    }
                case Vendors.Other:
                    {
                        WriteLineWithColor( "Minimization of the package for undefined vendor", ConsoleColor.Blue );
                        ReplaceFiles();
                        break;
                    }
                default:
                    WriteLineWithColor( "Please, press any key to continue", ConsoleColor.DarkGray );
                    Console.ReadLine();
                    break;
            }
        }

        private static void WriteLineWithColor( string text, ConsoleColor color )
        {
            Console.BackgroundColor = color;
            Console.WriteLine( text );
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void ReplaceFiles()
        {
            long size = 0;
            foreach(var fileName in Directory.EnumerateFileSystemEntries( UnpackedDataDirectoryPath, "*", SearchOption.AllDirectories ))
            {
                var extension = Path.GetExtension( fileName );

                if(extension == null)
                    continue;

                string emptyFileName;
                if(!ExtensionFileMap.TryGetValue( extension, out emptyFileName ))
                    continue;
                emptyFileName = Path.Combine( TemplateFilesDirectoryPath, emptyFileName );

                var fileToReplace = new FileInfo( fileName );
                var emptyFile = new FileInfo( emptyFileName );

                if(!fileToReplace.Exists)
                    continue;
                //if (fileToReplace.Length <= emptyFile.Length)
                //    continue;

                Console.WriteLine( fileName );

                size += fileToReplace.Length - emptyFile.Length;

                fileToReplace.Delete();
                emptyFile.CopyTo( fileName );
            }
            WriteLineWithColor( "Saved bytes: " + size, ConsoleColor.Blue );
            WriteLineWithColor( "Please, press any key to continue", ConsoleColor.DarkGray );
            Console.ReadLine();
        }

        private enum Vendors
        {
            Moodle,
            Sakai,
            Other,
            Undefined
        }

        private static Vendors DefineVendor( string folder )
        {
            if(Path.HasExtension( folder ))
            {
                WriteLineWithColor( "Please, unzipe folder", ConsoleColor.Red );
                return Vendors.Undefined;
            }
            if(File.Exists( Path.Combine( folder, MoodleManifestFileName ) ))
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
            Files.Contains( Path.GetFileName( path ) ) &&
            Regex.IsMatch( File.ReadAllText( path ), "<archive[\\d\\D]*system*=*\"[Sakai][\\d\\D]*\"" );
    }
}
