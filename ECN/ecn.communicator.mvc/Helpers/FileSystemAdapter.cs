using System;
using System.IO;
using Ecn.Communicator.Mvc.Interfaces;

namespace Ecn.Communicator.Mvc.Helpers
{
    public class FileSystemAdapter : IFileSystem
    {
        public void CloseTextWriter(TextWriter textWriter)
        {
            textWriter.Close();
        }

        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public TextWriter FileAppendText(string path)
        {
            return File.AppendText(path);
        }

        public void FileDelete(string path)
        {
            File.Delete(path);
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
