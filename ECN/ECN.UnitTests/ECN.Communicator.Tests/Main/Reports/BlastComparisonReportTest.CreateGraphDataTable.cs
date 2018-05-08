using System;
using System.Collections.Generic;
using NUnit.Framework;
using ECN_Framework_Entities.Activity.Report;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Reports
{
    public partial class BlastComparisonReportTest
    {
        private const string ActivationCodeOpen = "open";
        private const string ActivationCodeClick = "click";
        private const string ActivationCodeBounce = "bounce";
        private const string ActivationCodeSubscribe = "subscribe";
        private const string ActivationCodeComplaint = "complaint";
        private const string ColumnBlastID = "BlastID";
        private const string ColumnEmailSubject = "EmailSubject";
        private const string ColumnSendTime = "SendTime";
        private const string ColumnTotalSent = "TotalSent";
        private const string ColumnOpens = "Opens";
        private const string ColumnClicks = "Clicks";
        private const string ColumnBounces = "Bounces";
        private const string ColumnOptOuts = "OptOuts";
        private const string ColumnComplaints = "Complaints";
        private const string ColumnOpensPerc = "OpensPerc";
        private const string ColumnClicksPerc = "ClicksPerc";
        private const string ColumnBouncesPerc = "BouncesPerc";
        private const string ColumnOptOutsPerc = "OptOutsPerc";
        private const string ColumnComplaintsPerc = "ComplaintsPerc";
        private const string SampleSubject = "SampleSubject"; 
        private const string BlastComparelistFieldName = "bclist";

        [Test]
        [TestCase(ActivationCodeOpen, ColumnOpensPerc)]
        [TestCase(ActivationCodeClick, ColumnClicksPerc)]
        [TestCase(ActivationCodeBounce, ColumnBouncesPerc)]
        [TestCase(ActivationCodeSubscribe, ColumnOptOutsPerc)]
        [TestCase(ActivationCodeComplaint, ColumnComplaintsPerc)]
        public void CreateGraphDataTable_WithBlastComparisionList_ReturnsDataTable(string activationTypeCode, string columnName)
        {
            // Arrange
            const int percValue = 1;
            var bcList = GetBlastComparisonList();
            bcList[0].ActionTypeCode = activationTypeCode;

            _privateTestObject.SetFieldOrProperty(BlastComparelistFieldName, bcList);

            // Act
            var dataTable = _testEntity.createGraphDataTable();

            // Assert
            dataTable.ShouldSatisfyAllConditions(
                () => dataTable.ShouldNotBeNull(),
                () => dataTable.Rows.Count.ShouldBe(1),
                () => dataTable.Rows[0][columnName].ShouldBe(percValue));
        }

        private List<BlastComparision> GetBlastComparisonList()
        {
            return new List<BlastComparision>
            {
                new BlastComparision
                {
                    BlastID = "1",
                    TotalSent = 1,
                    TotalCount = 1,
                    EmailSubject = SampleSubject,
                    SendTime = DateTime.UtcNow.Date,
                    ActionTypeCode = ActivationCodeOpen,
                    DistinctCount = 1,
                    Perc = 1.0f
                },
            };
        }
    }
}
