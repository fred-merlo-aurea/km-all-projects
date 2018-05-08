using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Web;
using System.Web.Fakes;
using System.Diagnostics.CodeAnalysis;
using ecn.webservice;
using ECN_Framework_Common.Functions.Fakes;
using ECN_Framework_DataLayer.Fakes;
using ECN_Framework_DataLayer.DomainTracker.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using EntitiesDomainTracker = ECN_Framework_Entities.DomainTracker;
using EntityFakes = KM.Common.Entity.Fakes;
using PlatformFakes = KM.Platform.Fakes;
using WebserviceFakes = ecn.webservice.Fakes;

namespace ECN.Webservice.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DomainTrackerTest
    {
        private DomainTracker _domainTracker;
        private DataTable _dataTable;
        private IDisposable _shims;
        private const string TrakerKey = "key";
        private const string UriString = "http://www.example.com";
        private const string Domain = "www.example.com";
        private const string StringId = "10";
        private const string UserAgent = "Windows 98 chrome";
        private const string MethodGetOperatingSystem = "GetOperatingSystem";
        private const int Id = 10;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _shims?.Dispose();
            _dataTable?.Dispose();
        }

        [Test]
        public void GetDomainTrackerFields_ForDataTable_ReturnList()
        {
            // Arrange
            _domainTracker = new DomainTracker();
            InitializeDomainTracker(true);

            // Act
            var result = _domainTracker.GetDomainTrackerFields(TrakerKey) as List<DomainTrackerFieldSource>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result[0].DomainTrackerFieldsID.ShouldBe(StringId),
                () => result.Count.ShouldBe(3));
        }

        [Test]
        public void GetDomainTrackerFields_ForNullDataTable_ReturnList()
        {
            // Arrange
            _domainTracker = new DomainTracker();
            InitializeDomainTracker(false);

            // Act
            var result = _domainTracker.GetDomainTrackerFields(TrakerKey) as List<DomainTrackerFieldSource>;

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public void UpdateDomainTrackerActivity_ForDomainTrackerFieldSource_SaveValues()
        {
            // Arrange
            _domainTracker = new DomainTracker();
            InitializeUpdateTracker();
            var domainTrackerFieldCollection = new List<DomainTrackerFieldSource>
            {
                new DomainTrackerFieldSource()
                {
                    FieldValue = string.Empty,
                    DomainTrackerFieldsID = StringId
                }
            };

            // Act
            _domainTracker.UpdateDomainTrackerActivity(domainTrackerFieldCollection, TrakerKey, string.Empty,
                StringId, Domain, Domain);

            // Assert
            HttpContext.Current.Request.UserAgent.ShouldBe(UserAgent);
            HttpContext.Current.Request.UserHostAddress.ShouldBe(Domain);
        }

        [Test]
        [TestCase("Windows 98")]
        [TestCase("Windows NT 5.0")]
        [TestCase("Windows NT 5.1")]
        [TestCase("Windows NT 6.0")]
        [TestCase("Windows NT 6.1")]
        [TestCase("Windows NT 6.2")]
        [TestCase("Windows NT 6.3")]
        [TestCase("Windows")]
        [TestCase("Android")]
        [TestCase("Linux")]
        [TestCase("iPhone")]
        [TestCase("iPad")]
        [TestCase("Macintosh")]
        [TestCase("Macintosh")]
        [TestCase("Unknown OS")]
        public void GetOperatingSystem_ForDifferentValues_ReturnSystemNames(string param)
        {
            // Arrange
            _domainTracker = new DomainTracker();
            var privateObject = new PrivateObject(_domainTracker);

            // Act
            var result = privateObject.Invoke(MethodGetOperatingSystem, new object[] { param });

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetUDFSources_ForDomainTrackerFieldSource_ReturnGroupUDFList()
        {
            // Arrange
            _domainTracker = new DomainTracker();
            InitializeUdfSource();

            // Act
            var result = _domainTracker.GetUDFSources(TrakerKey) as List<GroupUDF>;

            // Arrange
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result[0].GroupID.ShouldBe(StringId));
        }

        [Test]
        public void UpdateDataJSON_ForGroupDataFieldSource_UpdateTracker()
        {
            // Arrange
            _domainTracker = new DomainTracker();
            var groupData = new List<GroupUDF>
            {
                new GroupUDF()
                {
                    GroupDataFields = new List<GroupDataFieldSource>
                    {
                        new GroupDataFieldSource()
                        {
                            GroupDataFieldName = Domain,
                            GroupDataFieldsID = StringId,
                            GroupDataFieldValue = Domain
                        }
                    }
                }
            };
            InitializeUpdateTracker();

            // Act
            _domainTracker.UpdateDataJSON(groupData, TrakerKey, string.Empty);

            // Assert
            HttpContext.Current.Request.UserAgent.ShouldBe(UserAgent);
            HttpContext.Current.Request.UserHostAddress.ShouldBe(Domain);
        }

        private void InitializeUdfSource()
        {
            ShimDomainTracker.GetSqlCommand = (x) => new EntitiesDomainTracker.DomainTracker()
            {
                DomainTrackerID = Id
            };
            WebserviceFakes.ShimDomainTracker.AllInstances.GetDomainTrackerFieldsString = 
                (x, y) => new List<DomainTrackerFieldSource>
                {
                    new DomainTrackerFieldSource()
                    {
                        Source = Domain,
                        SourceID = Domain,
                        DomainTrackerFieldsID = Domain
                    }
                };
        }

        private void InitializeUpdateTracker()
        {
            InitializeVerify();
            ShimDomainTrackerUserProfile.GetSqlCommand = (x) => null;
            ShimDataFunctions.ExecuteScalarSqlCommandString = (x, y) => Id;
        }

        private void InitializeDomainTracker(bool param)
        {
            InitializeVerify();
            _dataTable = new DataTable()
            {
                Columns = { "DomainTrackerFieldsID", "Source", "SourceID" },
                Rows = { Id, Id, Id }
            };
            if (param)
            {
                ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => _dataTable;
            }
            else
            {
                ShimDataFunctions.GetDataTableSqlCommandString = (x, y) => new DataTable();
            }
        }

        private void InitializeVerify()
        {
            var uri = new Uri(UriString);
            var httpRequest = new ShimHttpRequest
            {
                UrlReferrerGet = () => uri,
                UserAgentGet = () => UserAgent,
                UserHostAddressGet = () => Domain
            };
            var shimHttp = new ShimHttpContext
            {
                RequestGet = () => httpRequest
            };
            ShimHttpContext.CurrentGet = () => shimHttp;
            ShimCacheHelper.IsCacheEnabled = () => true;
            var domainTracker = new EntitiesDomainTracker.DomainTracker()
            {
                Domain = Domain,
                DomainTrackerID = Id,
                BaseChannelID = Id
            };
            ShimCacheHelper.GetCurrentCacheString = (cacheKey) =>
            {
                var result = new Object();
                if (cacheKey.Contains(TrakerKey))
                {
                    return result = domainTracker;
                }
                else if (cacheKey.Contains(Id.ToString()))
                {
                    return result = null;
                }
                return result;
            };
            ShimCacheHelper.AddToCacheStringObject = (x, y) => new ShimCacheHelper();
            ShimUser.GetUsersByChannelIDInt32 = (x) => new List<User>
            {
                new User()
                {
                    DefaultClientGroupID = Id,
                    DefaultClientID = Id,
                    UserID = Id
                }
            };
            ShimUser.AllInstances.ECN_SetAuthorizedUserObjectsUserInt32Int32 = (x, y, z, q) => new User();
            PlatformFakes.ShimUser.IsChannelAdministratorUser = (x) => true;
            PlatformFakes.ShimUser.IsAdministratorUser = (x) => true;
        }
    }
}
