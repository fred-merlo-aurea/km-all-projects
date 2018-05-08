using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web.UI.WebControls;
using Castle.Core.Logging;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using KMPS.MD.Controls;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using DownloadPanelBase = KMPS.MD.Controls.DownloadPanelBase;

namespace KMPS.MD.Tests.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DownloadPanelBaseTests : BaseControlTests
    {
        private const string TestDB = "TestDB";
        private const string LiveDB = "LiveDB";
        private DownloadPanelBase _testEntity;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _testEntity = new DownloadPanel_CLV();
            InitializeUserControl(_testEntity);
            InitializeAllControls(_testEntity);
        }
        
        [Test]
        public void UserSession_Get_SameSession()
        {
            // Arrange
            ShimECNSession.AllInstances.RefreshSession = _ => { };
            var session = ReflectionHelper.CreateInstance<ECNSession>();
            ShimECNSession.CurrentSession = () => session;

            // Act
            var result = _testEntity.UserSession;

            // Assert
            result.ShouldBe(session);
        }

        [Test]
        public void clientconnections_Get_ReturnsClientConnection()
        {
            // Arrange
            ShimECNSession.AllInstances.RefreshSession = _ => { };
            var session = ReflectionHelper.CreateInstance<ECNSession>();
            ShimECNSession.CurrentSession = () => session;
            ShimClient.AllInstances.SelectInt32Boolean = (_, __, ___) => new Client
            {
                ClientLiveDBConnectionString = LiveDB, 
                ClientTestDBConnectionString = TestDB
            };

            // Act
            var result = _testEntity.clientconnections;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ClientLiveDBConnectionString = LiveDB,
                () => result.ClientTestDBConnectionString = TestDB);
        }
    }
}
