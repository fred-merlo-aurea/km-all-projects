using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_BusinessLayer.Communicator;
using KMPlatform.Entity;
using NUnit.Framework;
using ECN_Framework_Common.Objects;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using KM.Platform.Fakes;
using Shouldly;
using ECNEntities = ECN_Framework_Entities.Communicator;
using KMPlatformFakes = KMPlatform.BusinessLogic.Fakes;
using ECN_Framework_Common.Functions.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class NewFileTest
    {
        private const string DummyString = "DummyString";
        private IDisposable _shimsObject;
        private readonly NameValueCollection _appSettings = new NameValueCollection();

        [SetUp]
        public void SetUp()
        {
            _appSettings.Add("Image_DomainPath", DummyString);
            _appSettings.Add("Communicator_VirtualPath", DummyString);
            _shimsObject = ShimsContext.Create();
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
        }

        [TearDown]
        public void CleanUp()
        {
            _appSettings.Clear();
            _shimsObject.Dispose();
        }
    }
}
