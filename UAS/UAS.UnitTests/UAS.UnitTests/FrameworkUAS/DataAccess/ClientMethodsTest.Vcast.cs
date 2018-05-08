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
        private const string SampleSubscribeNumber = "number 1";
        private const string SampleSub02Ans = "sub 02 ans";
        private const string SampleSub03Ans = "sub 03 ans";
        private const string ProcedureVcastProcessFileMxBooks = "ccp_Vcast_Process_File_MX_Books";
        private const string ProcedureVcastProcessFileMxElan = "ccp_Vcast_Process_File_MX_Elan";

        [Test]
        public void Vcast_Process_File_MX_Books_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = GetVcastTags();
            var columns = GetVcastDataColumns();
            var values = GetVcastDataValues();
            table.Columns.AddRange(columns);
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
            ClientMethods.Vcast_Process_File_MX_Books(table);

            // Assert
            commandText.ShouldBe(ProcedureVcastProcessFileMxBooks);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string) elementValue));
        }

        [Test]
        public void Vcast_Process_File_MX_Elan_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = GetVcastTags();
            var columns = GetVcastDataColumns();
            var values = GetVcastDataValues();
            table.Columns.AddRange(columns);
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
            ClientMethods.Vcast_Process_File_MX_Elan(table);

            // Assert
            commandText.ShouldBe(ProcedureVcastProcessFileMxElan);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string) elementValue));
        }

        private static IEnumerable<string> GetVcastTags()
        {
            return new List<string>
            {
                Consts.TagWatt,
                Consts.TagSubscribeNumber,
                Consts.TagSub02Ans,
                Consts.TagSub03Ans
            };
        }

        private static DataColumn[] GetVcastDataColumns()
        {
            return new List<string>
                {
                    Consts.TagSubscribeNumber,
                    Consts.TagSub02Ans,
                    Consts.TagSub03Ans
                }
                .Select(s => new DataColumn(s)).ToArray();
        }

        private static object[] GetVcastDataValues()
        {
            return new object[]
            {
                SampleSubscribeNumber,
                SampleSub02Ans,
                SampleSub03Ans
            };
        }
    }
}
