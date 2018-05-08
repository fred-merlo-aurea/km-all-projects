using System.IO;

namespace Ecn.Communicator.Mvc.Interfaces
{
    public interface IFileSystem
    {
        DirectoryInfo CreateDirectory(string path);
        bool DirectoryExists(string directoryPath);
        bool FileExists(string filePath);
        void CloseTextWriter(TextWriter textWriter);
        TextWriter FileAppendText(string outfileName);
        void FileDelete(string path);
    }
}
