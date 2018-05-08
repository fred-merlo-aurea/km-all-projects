using System.Collections.Generic;
using System.Data;
using System.Linq;
using FrameworkUAS.DataAccess;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.FrameworkUAS.DataAccess.Common;

using ShimKMCommonDataFunctiones = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkUAS.DataAccess
{
    public partial class ClientMethodsTest
    {
        private const string SamplePublishCode = "pub code1";
        private const string SampleGlobalUserKey = "user002";
        private const string SampleGroupId = "group 2";
        private const string SampleIsRecent = "no";
        private const string SampleAddDate = "2018-01-02";
        private const string SampleDropDate = "2019-01-03";

        [Test]
        public void Northstar_Relational_AddGroup_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = new List<string>
            {
                Consts.TagNorthStar,
                Consts.TagPublishCode,
                Consts.TagGlobalUserKey,
                Consts.TagGroupId,
                Consts.TagIsRecent,
                Consts.TagAddDate,
                Consts.TagDropDate
            };
            var columns = new List<string>
            {
                Consts.TagPublishCode,
                Consts.TagGlobalUserKey,
                Consts.TagGroupId,
                Consts.TagIsRecent,
                Consts.TagAddDate,
                Consts.TagDropDate
            };
            object[] values =
            {
                SamplePublishCode,
                SampleGlobalUserKey,
                SampleGroupId,
                SampleIsRecent,
                SampleAddDate,
                SampleDropDate
            };
            table.Columns.AddRange(columns.Select(s => new DataColumn(s)).ToArray());
            table.Rows.Add(values);
            string commandText = null;
            string xml = null;
            ShimKMCommonDataFunctiones.ExecuteNonQuerySqlCommandString = (command, _) =>
            {
                commandText = command.CommandText;
                xml = command.Parameters[0].Value as string;
                command.Dispose();
                return true;
            };

            // Act
            ClientMethods.Northstar_Relational_AddGroup(table);

            // Assert
            commandText.ShouldBe(Consts.ProcedureNorthstarRelationalAddGroup);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string) elementValue));
        }
    }
}
