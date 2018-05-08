namespace ECN.Communicator.Tests.Setup.Interfaces
{
    public interface IDirectory
    {
        string[] GetFiles(string path, string pattern);
    }
}
