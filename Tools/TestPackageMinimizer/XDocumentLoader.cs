using System.Collections.Immutable;
using System.IO;
using System.IO.Abstractions;
using System.Xml.Linq;

namespace TestPackageMinimizer
{
    public sealed class XDocumentLoader
    {
        private readonly IFileSystem m_fileSystem;

        public XDocumentLoader( IFileSystem fileSystem )
        {
            m_fileSystem = fileSystem;
        }

        public bool TryLoadFile( string path, out XDocument document )
        {
            document = null;

            if(string.IsNullOrEmpty( path ))
            {
                return false;
            }

            path = path.Replace( '\\', Path.DirectorySeparatorChar );
            if(m_fileSystem.File.Exists( path ))
            {
                return TryLoadFromFileSystem( path, out document );
            }

            return false;
        }


        private bool TryLoadFromFileSystem( string path, out XDocument document )
        {

            document = null;
            bool result = false;

            using(Stream stream = m_fileSystem.File.Open( path, FileMode.Open, FileAccess.Read, FileShare.Read ))
            {
                using(StreamReader reader = new StreamReader( stream ))
                {
                    string xml = reader.ReadToEnd();
                    foreach(var t in Replacements ) {
                        xml = xml.Replace( t.Item1, t.Item2 );
                    }
                    document = XDocument.Parse( xml );

                    result = true;
                }
            }

            return result;
        }

        private static readonly ImmutableArray<(string, string)> Replacements = ImmutableArray.Create(
            ("dt:dt", "dt"),
            ("sakai:", string.Empty),
            ("<CHEF:", "<"),
            ("</CHEF:", "</"),
            ("<DAV:", "<"),
            ("</DAV:", "</")
        );
    }
}
