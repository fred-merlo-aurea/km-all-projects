using System.Data;
using System.Web.UI.WebControls;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Reports
{
    public partial class BlastComparisonReportTest
    {
        private const string ChartDataGridName = "gvChartData";
        private const string PanelExport = "PanelExport";

        [Test]
        [TestCase(ActivationCodeOpen, ColumnOpens, ColumnOpensPerc)]
        [TestCase(ActivationCodeClick, ColumnClicks, ColumnClicksPerc)]
        [TestCase(ActivationCodeBounce, ColumnBounces, ColumnBouncesPerc)]
        [TestCase(ActivationCodeSubscribe, ColumnOptOuts, ColumnOptOutsPerc)]
        [TestCase(ActivationCodeComplaint, ColumnComplaints, ColumnComplaintsPerc)]
        public void CreateGridDataTable_WithBlastComparisionList_ReturnsDataTable(
            string activationTypeCode,
            string columnName,
            string percColumnName)
        {
            // Arrange
            const int percValue = 1;
            var bcList = GetBlastComparisonList();
            bcList[0].ActionTypeCode = activationTypeCode;

            _privateTestObject.SetFieldOrProperty(BlastComparelistFieldName, bcList);

            // Act
            var dataTable = _testEntity.createGridDataTable();
            var grid = Get<GridView>(_privateTestObject, ChartDataGridName);
            var panel = Get<Panel>(_privateTestObject, PanelExport);

            // Assert
            dataTable.ShouldSatisfyAllConditions(
                () => grid.ShouldNotBeNull(),
                () => 
                     {
                         var dataSource = grid.DataSource.ShouldBeOfType<DataTable>();
                         dataSource.Rows.Count.ShouldBe(1);
                         dataSource.Rows[0][ColumnBlastID].ShouldBe(bcList[0].BlastID);
                     },
                () => panel.Visible.ShouldBeTrue(),
                () => dataTable.ShouldNotBeNull(),
                () => dataTable.Rows.Count.ShouldBe(1),
                () => dataTable.Rows[0][ColumnBlastID].ShouldBe(bcList[0].BlastID),
                () => dataTable.Rows[0][ColumnEmailSubject].ShouldBe(bcList[0].EmailSubject),
                () => dataTable.Rows[0][ColumnTotalSent].ShouldBe($"Sent {bcList[0].TotalSent} on "),
                () => dataTable.Rows[0][ColumnSendTime].ToString().ShouldContain(bcList[0].SendTime.Date.ToString()),
                () => dataTable.Rows[0][columnName].ShouldBe($"{percValue}"),
                () => dataTable.Rows[0][percColumnName].ShouldBe($"{percValue}%"));
        }
    }
}
