using System.Collections.Generic;
using System.Data;
using System.Linq;
using FrameworkUAS.DataAccess;
using FrameworkUAS.DataAccess.Fakes;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.FrameworkUAS.DataAccess.Common;

using ShimKMCommonDataFunctions = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkUAS.DataAccess
{
    public partial class ClientMethodsTest
    {
        private const string ProcedureBriefMediaInsertAccess = "ccp_BriefMedia_BMWU_Insert_Access";
        private const string ColumnUserId = "UserID";
        private const string SampleEmail = "a@b.com";
        private const string SampleTopicCode = "topic code A";
        private const string SampleTopicCodeText = "code text";
        private const string SampleUserId = "002";
        private const string SampleFirstName = "Alice";
        private const string SampleLastName = "Last";
        private const string SampleAccessId = "123456";

        [Test]
        public void BriefMedia_Relational_Update_TaxBehavior_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = new List<string> { Consts.TagAccessId, Consts.TagTopicCode };
            var columns = new List<string> { Consts.TagAccessId, Consts.TagTaxonomyId };
            object[] values = { "accessId1", "taxonomyId1" };
            table.Columns.AddRange(columns.Select(s => new DataColumn(s)).ToArray());
            table.Rows.Add(values);
            string commandText = null;
            string xml = null;
            ShimKMCommonDataFunctions.ExecuteNonQuerySqlCommand = command =>
            {
                commandText = command.CommandText;
                xml = command.Parameters[0].Value as string;
                command.Connection.Dispose();
                command.Dispose();
                return true;
            };

            // Act
            ClientMethods.BriefMedia_Relational_Update_TaxBehavior(new Client(), table);

            // Assert
            commandText.ShouldBe(Consts.ProcedureBriefMediaUpdateTaxBehavior);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string)elementValue));
        }

        [Test]
        public void BriefMedia_Relational_Update_PageBehavior_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = new List<string> { Consts.TagAccessId, Consts.TagPageId };
            var columns = new List<string> { Consts.TagAccessId, Consts.TagPageUnderscoreId };
            object[] values = { "accessId1", "pageId1" };
            table.Columns.AddRange(columns.Select(s => new DataColumn(s)).ToArray());
            table.Rows.Add(values);
            string commandText = null;
            string xml = null;
            ShimKMCommonDataFunctions.ExecuteNonQuerySqlCommand = command =>
            {
                commandText = command.CommandText;
                xml = command.Parameters[0].Value as string;
                command.Connection.Dispose();
                command.Dispose();
                return true;
            };

            // Act
            ClientMethods.BriefMedia_Relational_Update_PageBehavior(new Client(), table);

            // Assert
            commandText.ShouldBe(Consts.ProcedureBriefMediaUpdatePageBehavior);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string)elementValue));
        }

        [Test]
        public void BriefMedia_Relational_Update_TopicCode_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = new List<string> { Consts.TagEmailAddress, Consts.TagTopicId, Consts.TagSearchTerms };
            var columns = new List<string> { Consts.TagEmailAddress, Consts.ColumnTopicCode, Consts.ColumnTopicCodeText };
            object[] values = { SampleEmail, SampleTopicCode, SampleTopicCodeText };
            table.Columns.AddRange(columns.Select(s => new DataColumn(s)).ToArray());
            table.Rows.Add(values);
            string commandText = null;
            string xml = null;
            ShimKMCommonDataFunctions.ExecuteNonQuerySqlCommand = command =>
            {
                commandText = command.CommandText;
                xml = command.Parameters[0].Value as string;
                command.Connection.Dispose();
                command.Dispose();
                return true;
            };

            // Act
            ClientMethods.BriefMedia_Relational_Update_TopicCode(new Client(), table);

            // Assert
            commandText.ShouldBe(Consts.ProcedureBriefMediaUpdateTopicCode);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string)elementValue));
        }

        [Test]
        public void BriefMedia_Relational_Update_Users_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = new List<string>
            {
                Consts.TagUser,
                Consts.TagDrupalId,
                Consts.TagFirstName,
                Consts.TagLastName,
                Consts.TagEmail
            };
            var columns = new List<string>
            {
                Consts.ColumnDrupalUserId,
                Consts.ColumnFirstName,
                Consts.ColumnLastName,
                Consts.TagEmail
            };
            object[] values =
            {
                SampleUserId,
                SampleFirstName,
                SampleLastName,
                SampleEmail
            };
            table.Columns.AddRange(columns.Select(s => new DataColumn(s)).ToArray());
            table.Rows.Add(values);
            string commandText = null;
            string xml = null;
            ShimKMCommonDataFunctions.ExecuteNonQuerySqlCommand = command =>
            {
                commandText = command.CommandText;
                xml = command.Parameters[0].Value as string;
                command.Connection.Dispose();
                command.Dispose();
                return true;
            };

            // Act
            ClientMethods.BriefMedia_Relational_Update_Users(new Client(), table);

            // Assert
            commandText.ShouldBe(Consts.ProcedureBriefMediaUpdateUsers);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string)elementValue));
        }

        [Test]
        public void BriefMedia_Relational_Insert_Access_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = new List<string> { Consts.TagUser, Consts.TagAccessId, Consts.TagDrupalId };
            var columns = new List<string> { Consts.TagAccessId, ColumnUserId };
            object[] values = { SampleAccessId, SampleUserId };
            table.Columns.AddRange(columns.Select(s => new DataColumn(s)).ToArray());
            table.Rows.Add(values);
            string commandText = null;
            string xml = null;
            ShimKMCommonDataFunctions.ExecuteNonQuerySqlCommand = command =>
            {
                commandText = command.CommandText;
                xml = command.Parameters[0].Value as string;
                command.Connection.Dispose();
                command.Dispose();
                return true;
            };

            // Act
            ClientMethods.BriefMedia_Relational_Insert_Access(new Client(), table);

            // Assert
            commandText.ShouldBe(ProcedureBriefMediaInsertAccess);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string)elementValue));
        }
    }
}
