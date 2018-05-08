using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECN.Editor.Tests.Setup.Mocks;
using Microsoft.QualityTools.Testing.Fakes;

namespace ECN.Editor.Tests.Setup
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
        
        public SecurityCheckMock SecurityCheck { get; } = new SecurityCheckMock();

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
