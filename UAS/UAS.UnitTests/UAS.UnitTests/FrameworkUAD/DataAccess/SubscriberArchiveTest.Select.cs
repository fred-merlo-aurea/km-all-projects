using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FrameworkUAD.DataAccess;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Entity;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="SubscriberArchive"/>
    /// </summary>
    public partial class SubscriberArchiveTest
    {
        private const string ProcessCode = "process-code";
        private const int SourceFileId = 2;
        private const string Xml = "xml";
        private const string ProcSelect = "e_SubscriberArchive_Select";
        private const string ProcSelectForFileAudit = "e_SubscriberArchive_SelectForFileAudit";
        private const string ProcSaveBulkInsert = "e_SubscriberArchive_SaveBulkInsert";
        private const string MethodGet = "Get";
        private const string MethodGetList = "GetList";
        private static readonly ClientConnections Client = new ClientConnections();

        private DataTable _dataTable;
        private IList<Entity.SubscriberArchive> _list;
        private Entity.SubscriberArchive _objWithRandomValues;
        private Entity.SubscriberArchive _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberArchive.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Select_WhenCalledWithProcessCode_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberArchive.Select(ProcessCode, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectForFileAudit_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            // Act
            var result = SubscriberArchive.SelectForFileAudit(ProcessCode, SourceFileId, startDate, endDate, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@ProcessCode"].Value.ShouldBe(ProcessCode),
                () => _sqlCommand.Parameters["@SourceFileID"].Value.ShouldBe(SourceFileId),
                () => _sqlCommand.Parameters["@StartDate"].Value.ShouldBe(startDate),
                () => _sqlCommand.Parameters["@EndDate"].Value.ShouldBe(endDate),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectForFileAudit),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(SubscriberArchive).CallMethod(MethodGet, new object[] { new SqlCommand() }) as Entity.SubscriberArchive;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.ShouldBe(_objWithDefaultValues));
        }

        [Test]
        public void GetList_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(SubscriberArchive).CallMethod(MethodGetList, new object[] { new SqlCommand() }) as List<Entity.SubscriberArchive>;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue());
        }

        [Test]
        public void SaveBulkInsert_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = SubscriberArchive.SaveBulkInsert(Xml, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(Xml),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSaveBulkInsert),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }
    }
}