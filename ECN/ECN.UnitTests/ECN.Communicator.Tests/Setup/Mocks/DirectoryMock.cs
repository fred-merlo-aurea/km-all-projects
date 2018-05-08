using System.Diagnostics.CodeAnalysis;
using ECN.Communicator.Tests.Setup.Interfaces;
using Moq;
using Shim = System.IO.Fakes.ShimDirectory;

namespace ECN.Communicator.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class DirectoryMock : Mock<IDirectory>
    {
        public DirectoryMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetFilesStringString = GetFiles;
        }

        private string[] GetFiles(string path, string pattern)
        {
            return Object.GetFiles(path, pattern);
        }
    }
}
