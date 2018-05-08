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
        private const string SampleProductCode = "product code1";
        private const string SampleFoxColumnName = "fox column name A";
        private const string SampleCodeSheetValue = "code sheet value A";
        private const string SampleWattGroupId = "WattGroup1";
        private const string SampleWattPubCode = "WattPubCode2";
        private const string TagWattGroupId = "GROUPID";
        private const string TagWattPubCode = "PUBCODE";
        private const string ProcedureWattProcessEcnGroupIdPubCode = "ccp_WATT_Process_ECN_GROUPID_PUBCODE";

        [Test]
        public void WATT_Relational_Process_MacMic_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = new List<string> { Consts.TagWatt, Consts.TagFoxColumnName, Consts.TagCodeSheetValue };
            var columns = new List<string> { Consts.ColumnProductCode, Consts.ColumnFoxColumnName, Consts.TagCodeSheetValue };
            object[] values = { SampleProductCode, SampleFoxColumnName, SampleCodeSheetValue };
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
            ClientMethods.WATT_Relational_Process_MacMic(table);

            // Assert
            commandText.ShouldBe(Consts.ProcedureWattProcessMacMic);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string)elementValue));
        }

        [Test]
        public void WATT_Relational_Process_ECN_GROUPID_PUBCODE_DataTableIsNotNull_UpdatesTable()
        {
            // Arrange
            var table = new DataTable();
            var tags = new List<string> { Consts.TagWatt, TagWattGroupId, TagWattPubCode };
            var columns = new List<string> { TagWattGroupId, TagWattPubCode };
            object[] values = { SampleWattGroupId, SampleWattPubCode };
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
            ClientMethods.WATT_Relational_Process_ECN_GROUPID_PUBCODE(table);

            // Assert
            commandText.ShouldBe(ProcedureWattProcessEcnGroupIdPubCode);
            tags.ShouldAllBe(tag => xml.Contains(tag));
            values.ShouldAllBe(elementValue => xml.Contains((string)elementValue));
        }
    }
}
