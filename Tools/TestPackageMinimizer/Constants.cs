using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TestPackageMinimizer
{
    internal static class Constants
    {
        public static readonly string UnpackedDataDirectoryPath = @"C:\ForTools\packages";
        public static readonly string TemplateFilesDirectoryPath = @"C:\Users\lkhimiak\source\repos\Ideas_D2L\Tools\TestPackageMinimizer\BlankFiles";
        public static readonly string MoodleManifestFileName = "moodle_backup.xml";
        public static readonly ImmutableHashSet<string> Files = new HashSet<string>( StringComparer.OrdinalIgnoreCase ) {
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
        public static readonly Dictionary<string, string> ExtensionFileMap = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase ) {
            { ".docx", "empty.docx" },
            { ".doc", "empty.doc" },
            { ".gif", "empty.gif" },
            { ".jpg", "empty.jpg" },
            { ".pdf", "empty.pdf" },
            { ".bmp", "empty.bmp" },
            { ".png", "empty.png" },
            { ".mp4", "empty.mp4" },
            { ".pptx", "empty.pptx"},
            { ".mov", "empty.mov" },
            { ".mp3", "empty.mp3" },
            { ".xls", "empty.xls" },
            { ".xlsx", "empty.xlsx" },
            { ".zip", "empty.zip" },
            {".ppt", "empty.ppt"},
            {".swf", "empty.swf" },
            {".js", "empty.js" },
            {".webm", "empty.webm" },
            {".mus", "empty.mus" },
            {".jpeg", "empty.jpeg" },
            {".xsd", "empty.xsd" },
            {".wmv", "empty.wmv" },
            {".jcw", "empty.jcw" },
            {".svg", "empty.svg" },
            {".gsp", "empty.gsp" },
            {".jqz", "empty.jqz" },
            {".ppsx", "empty.ppsx" },
            {".dat", "empty.dat" },
            {".pps", "empty.pps" },
        };
    }
}
