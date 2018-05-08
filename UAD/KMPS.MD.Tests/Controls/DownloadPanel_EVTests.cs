using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web.UI.WebControls;
using Castle.Core.Logging;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using DownloadPanel_EV = KMPS.MD.Controls.DownloadPanel_EV;

namespace KMPS.MD.Tests.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class DownloadPanel_EVTests : BaseControlTests
    {
        private const string TestDB = "TestDB";
        private const string LiveDB = "LiveDB";
        private const string DownloadCountTextBox = "txtDownloadCount";
        private const string DownloadUniqueCountTextBox = "txtDownloadUniqueCount";
        private DownloadPanel_EV _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new DownloadPanel_EV();
            InitializeUserControl(_testEntity);
            InitializeAllControls(_testEntity);
        }

        [Test]
        [TestCase("BrandID", 1)]
        [TestCase("BrandID", 2)]
        [TestCase("ViewType", Enums.ViewType.ProductView)]
        [TestCase("ViewType", Enums.ViewType.ConsensusView)]
        [TestCase("ShowHeaderCheckBox", true)]
        [TestCase("ShowHeaderCheckBox", false)]
        [TestCase("dcRunID", 1)]
        [TestCase("dcRunID", 2)]
        [TestCase("filterCombination", "Test1")]
        [TestCase("filterCombination", "Test2")]
        [TestCase("downloadCount", 1)]
        [TestCase("downloadCount", 2)]
        [TestCase("dcTypeCodeID", 1)]
        [TestCase("dcTypeCodeID", 2)]
        [TestCase("dcTargetCodeID", 1)]
        [TestCase("dcTargetCodeID", 2)]
        [TestCase("matchedRecordsCount", 1)]
        [TestCase("matchedRecordsCount", 2)]
        [TestCase("nonMatchedRecordsCount", 1)]
        [TestCase("nonMatchedRecordsCount", 2)]
        [TestCase("TotalFileRecords", 1)]
        [TestCase("TotalFileRecords", 2)]
        [TestCase("TotalUADRecordCount", 1)]
        [TestCase("TotalUADRecordCount", 2)]
        [TestCase("HeaderText", "Test1")]
        [TestCase("HeaderText", "Test2")]
        [TestCase("VisibleCbIsRecentData", false)]
        [TestCase("VisibleCbIsRecentData", true)]
        public void Property_Set_GetSameValue(string propertyName, object value)
        {
            // Arrange, Act
            PrivateControl.SetProperty(propertyName, value);
            var result = PrivateControl.GetProperty(propertyName);
            
            // Assert
            result.ShouldBe(value);
        }

        [Test]
        [TestCase("BrandID", 0)]
        [TestCase("ViewType", Enums.ViewType.None)]
        [TestCase("ShowHeaderCheckBox", false)]
        [TestCase("dcRunID", 0)]
        [TestCase("filterCombination", null)]
        [TestCase("downloadCount", 0)]
        [TestCase("dcTypeCodeID", 0)]
        [TestCase("dcTargetCodeID", 0)]
        [TestCase("matchedRecordsCount", 0)]
        [TestCase("nonMatchedRecordsCount", 0)]
        [TestCase("TotalFileRecords", 0)]
        [TestCase("TotalUADRecordCount", 0)]
        [TestCase("PubIDs", null)]
        [TestCase("SubscribersQueries", null)]
        [TestCase("HeaderText", null)]
        [TestCase("VisibleCbIsRecentData", false)]
        public void Property_Get_DefaultValue(string propertyName, object defaultValue)
        {
            // Arrange, Act
            var result = PrivateControl.GetProperty(propertyName);
            
            // Assert
            result.ShouldBe(defaultValue);
        }

        [Test]
        public void PubIDs_Set_GetSameValue()
        {
            // Arrange
            var value = new List<int> {1, 2};

            // Act
            _testEntity.PubIDs = value;
            var result = _testEntity.PubIDs;
            
            // Assert
            result.ShouldBe(value);
        }

        [Test]
        public void SubscribersQueries_Set_GetSameValue()
        {
            // Arrange
            var value = new StringBuilder(Guid.NewGuid().ToString());

            // Act
            _testEntity.SubscribersQueries = value;
            var result = _testEntity.SubscribersQueries;
            
            // Assert
            result.ShouldBe(value);
        }

        [Test]
        public void SubscriptionID_Set_GetSameValue()
        {
            // Arrange
            var value = new List<int> {1, 2};
            ShimSubscriber.GetUniqueEmailsCountClientConnectionsStringBuilder = (_, __) => 2;
            ShimECNSession.AllInstances.RefreshSession = _ => { };
            var session = ReflectionHelper.CreateInstance<ECNSession>();
            ShimECNSession.CurrentSession = () => session;
            ShimClient.AllInstances.SelectInt32Boolean = (_, __, ___) => new Client
            {
                ClientLiveDBConnectionString = LiveDB, 
                ClientTestDBConnectionString = TestDB
            };
            _testEntity.SubscribersQueries = new StringBuilder();

            // Act
            _testEntity.SubscriptionID = value;
            var result = _testEntity.SubscriptionID;
            
            // Assert
            result.ShouldBe(value);
            GetField<TextBox>(DownloadCountTextBox).Text.ShouldBe(value.Count.ToString());
            GetField<TextBox>(DownloadUniqueCountTextBox).Text.ShouldBe(value.Count.ToString());
        }
    }
}
