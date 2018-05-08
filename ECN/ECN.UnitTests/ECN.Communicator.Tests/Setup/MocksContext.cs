using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using ECN.Communicator.Tests.Setup.Mocks;
using Microsoft.QualityTools.Testing.Fakes;

namespace ECN.Communicator.Tests.Setup
{
    [ExcludeFromCodeCoverage]
    public class MocksContext
    {
        public MocksContext()
        {
            ShimAppSettings();
        }

        public NameValueCollection AppSettings { get; } = new NameValueCollection();

        public DirectoryMock Directory { get; } = new DirectoryMock();

        public FileInfoMock FileInfo { get; } = new FileInfoMock();

        public ServerMock Server { get; } = new ServerMock();

        private void ShimAppSettings()
        {
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var originalAppSettings = ShimsContext.ExecuteWithoutShims(() => ConfigurationManager.AppSettings);
                var result = new NameValueCollection(originalAppSettings);
                result.Add(AppSettings);
                return result;
            };
        }
    }
}
