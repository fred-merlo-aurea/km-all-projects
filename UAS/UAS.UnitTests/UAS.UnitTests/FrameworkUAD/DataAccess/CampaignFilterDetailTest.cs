using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="CampaignFilterDetail"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CampaignFilterDetailTest
    {
        private const int CampaignFilterId = 1;
        private const string XmlSubscriber = "xml-subscriber";
        private const string ProcSaveCampainDetails = "sp_saveCampaignDetails";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private IList<Entity.CampaignFilterDetail> _list;
        private Entity.CampaignFilterDetail _objWithRandomValues;
        private Entity.CampaignFilterDetail _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();

            _objWithRandomValues = typeof(Entity.CampaignFilterDetail).CreateInstance();
            _objWithDefaultValues = typeof(Entity.CampaignFilterDetail).CreateInstance(true);

            _list = new List<Entity.CampaignFilterDetail>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = CampaignFilterDetail.Get(Client);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void SaveCampaignDetails_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            CampaignFilterDetail.saveCampaignDetails(Client, CampaignFilterId, XmlSubscriber);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@CampaignFilterID"].Value.ShouldBe(CampaignFilterId),
                () => _sqlCommand.Parameters["@xmlSubscriber"].Value.ShouldBe(XmlSubscriber),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveCampainDetails),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private void SetupFakes()
        {
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => new ShimSqlConnection().Instance;
            ShimDataFunctions.ExecuteSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return -1;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => new ShimSqlConnection().Instance;
            ShimSqlCommand.AllInstances.ExecuteReader = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}
