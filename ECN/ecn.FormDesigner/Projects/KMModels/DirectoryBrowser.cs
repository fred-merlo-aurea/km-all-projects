using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace KMModels
{
    public class DirectoryBrowser
    {
        public IEnumerable<FileBrowserEntry> GetContent(string path, string filter)
        {
            return GetFiles(path, filter).Concat(GetDirectories(path));
        }

        private IEnumerable<FileBrowserEntry> GetFiles(string path, string filter)
        {
            var directory = new DirectoryInfo(Server.MapPath(path));

            var extensions = (filter ?? "*").Split(",|;".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

            return extensions.SelectMany(directory.GetFiles)
                .Select(file => new FileBrowserEntry
                {
                    Name = file.Name,
                    Size = file.Length,
                    Type = EntryType.File
                });
        }

        private IEnumerable<FileBrowserEntry> GetDirectories(string path)
        {
            var directory = new DirectoryInfo(Server.MapPath(path));

            return directory.GetDirectories()
                .Select(subDirectory => new FileBrowserEntry
                {
                    Name = subDirectory.Name,
                    Type = EntryType.Directory
                });
        }

        public System.Web.HttpServerUtilityBase Server { get; set; }
    }
    public class FileBrowserEntry
    {
        public string Name { get; set; }
        public EntryType Type { get; set; }
        public long Size { get; set; }
    }

    public enum EntryType
    {
        File = 0,
        Directory
    }
}
