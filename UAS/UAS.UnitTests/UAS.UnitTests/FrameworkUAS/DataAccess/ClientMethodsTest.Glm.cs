using System.Collections.Generic;
using System.Data;
using System.Linq;
using FrameworkUAS.DataAccess;
using NUnit.Framework;
using Shouldly;

using ShimKMCommonDataFunctiones = KM.Common.Fakes.ShimDataFunctions;

namespace UAS.UnitTests.FrameworkUAS.DataAccess
{
    public partial class ClientMethodsTest
    {
        private const string ProcedureGlmRelationalInsertData = "ccp_GLM_Relational_InsertData";
        private const string TagGlm = "GLM";
        private const string TagGlmEmail = "EMAIL";
        private const string TagLeadsSent = "LEADSSENT";
        private const string TagLikes = "LIKES";
        private const string TagBoardFollows = "BOARDFOLLOWS";
        private const string TagExhibitorFollows = "EXHIBITORFOLLOWS";
        private const string ColumnGlmEmail = "E-mail";
        private const string ColumnLeadsSent = "Leads Sent";
        private const string ColumnLikes = "Likes";
        private const string ColumnBoardFollows = "Board Follows";
        private const string ColumnExhibitorFollows = "Exhibitor Follows";
        private const string SampleLeadsSent = "SampleA";
        private const string SampleLikes = "SampleB";
        private const string SampleBoardFollows = "SampleC";
        private const string SampleExhibitorFollows = "SampleD";

        [Test]
        public void GLM_Relational_InsertData_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = new List<string>
            {
                TagGlm,
                TagGlmEmail,
                TagLeadsSent,
                TagLikes,
                TagBoardFollows,
                TagExhibitorFollows
            };
            var columns = new List<string>
            {
                ColumnGlmEmail,
                ColumnLeadsSent,
                ColumnLikes,
                ColumnBoardFollows,
                ColumnExhibitorFollows
            };
            object[] values =
            {
                SampleEmail,
                SampleLeadsSent,
                SampleLikes,
                SampleBoardFollows,
                SampleExhibitorFollows
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
            var result = ClientMethods.GLM_Relational_InsertData(table);

            // Assert
            result.ShouldBeTrue();
            commandText.ShouldBe(ProcedureGlmRelationalInsertData);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string)elementValue));
        }
    }
}
