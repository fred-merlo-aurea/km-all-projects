using System.Collections.Generic;
using System.IO;
using System.IO.Fakes;
using Core_AMS.Utilities.Fakes;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Object;
using FrameworkUAS.Object.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic
{
    [TestFixture]
    public class ImportErrorSummaryTest
    {
        private const string TestPubCode = "TST";
        private readonly List<global::FrameworkUAD.Object.ImportErrorSummary> sampleSummary = new List<global::FrameworkUAD.Object.ImportErrorSummary>()
        {
            new global::FrameworkUAD.Object.ImportErrorSummary()
            {
                ClientMessage = "<test></test>",
                MAFField = string.Empty,
                PubCode = TestPubCode,
                Value = ":"
            },
            new global::FrameworkUAD.Object.ImportErrorSummary()
            {
                ClientMessage = "<test></test>",
                MAFField = string.Empty,
                PubCode = TestPubCode,
                Value = ";"
            },
            new global::FrameworkUAD.Object.ImportErrorSummary()
            {
                ClientMessage = "<test></test>",
                MAFField = string.Empty,
                PubCode = TestPubCode,
                Value = ","
            },
            new global::FrameworkUAD.Object.ImportErrorSummary()
            {
                ClientMessage = "<test></test>",
                MAFField = string.Empty,
                PubCode = TestPubCode,
                Value = "~"
            },
            new global::FrameworkUAD.Object.ImportErrorSummary()
            {
                ClientMessage = "<test></test>",
                MAFField = string.Empty,
                PubCode = TestPubCode,
                Value = "|"
            },
        };
        private readonly List<TransformSplitInfo> sampleSplits = new List<TransformSplitInfo>()
        {
            new TransformSplitInfo()
            {
                MAFField = string.Empty,
                PubID = 1,
                Delimiter = "comma"
            },
            new TransformSplitInfo()
            {
                MAFField = string.Empty,
                PubID = 1,
                Delimiter = "semicolon"
            },
            new TransformSplitInfo()
            {
                MAFField = string.Empty,
                PubID = 1,
                Delimiter = "colon"
            },
            new TransformSplitInfo()
            {
                MAFField = string.Empty,
                PubID = 1,
                Delimiter = "tild"
            },
            new TransformSplitInfo()
            {
                MAFField = string.Empty,
                PubID = 1,
                Delimiter = "pipe"
            }
        };

        [Test]
        public void CreateDimensionErrorsSummaryReport_DelimitersCoverage_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var testObject = new ImportErrorSummary();
                ShimImportErrorSummary.AllInstances.SelectInt32StringClientConnections = (_, __, ___, _4) =>
                    sampleSummary;

                ShimTransformSplit.AllInstances.SelectObjectInt32 = (_, __) => sampleSplits;
                ShimDBWorker.GetPubIDAndCodesByClientClientConnections = connections => new Dictionary<int, string>()
                {
                    {1, TestPubCode}
                };
                ShimDirectory.CreateDirectoryString = _ => new DirectoryInfo("test");
                ShimStringFunctions.CleanProcessCodeForFileNameString = s => s;
                var rowsCount = -1;
                ShimExcelFunctions.AllInstances.GetWorkbookDataTableString = (_, table, ___) =>
                {
                    rowsCount = table.Rows.Count;
                    return null;
                };
                ShimFileStream.ConstructorStringFileMode = (_, fileName, mode) => { };
                ShimFileStream.AllInstances.DisposeBoolean = (_, __) => { };
                ShimXlsxFormatProvider.AllInstances.ExportOverrideWorkbookStream = (_, __, ___) => { };

                // Act
                var result = testObject.CreateDimensionErrorsSummaryReport(0, null, null, new ClientConnections(), null);

                // Assert 
                result.ShouldBe("\\\\Reports\\_DimensionErrorsSummaryReport.xlsx");
                rowsCount.ShouldBe(0);
            }
        } 
    }
}
