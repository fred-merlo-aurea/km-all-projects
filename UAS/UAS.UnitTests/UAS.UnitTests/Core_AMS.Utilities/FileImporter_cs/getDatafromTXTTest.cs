using Core_AMS.Utilities;
using KM.Common.Import;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities.FileImporter_cs
{
    [TestFixture]
    public class getDatafromTXTTest : Fakes
    {
        [Test]
        public void getDatafromTXTTest_FileCol2RowCol2Start0Take1_Table2Cols1Row()
        {
            // Arrange
            FillFileCol2RowCol2();

            // Act
            var dataSet = FileImporter.getDatafromTXT(SampleFolderPath, FileName1, 0, 1, ",");

            // Assert
            dataSet.ShouldNotBeNull();
            dataSet.Tables.ShouldNotBeNull();

            var dataTable = dataSet.Tables[0];
            dataTable.Columns.Count.ShouldBe(2);
            dataTable.Columns[0].ColumnName.ShouldBe(FieldFileCol0Name);
            dataTable.Columns[1].ColumnName.ShouldBe(FieldFileCol1Name);

            dataTable.Rows.Count.ShouldBe(1);
            dataTable.Rows[0][FieldFileCol0Name].ShouldBe(FieldFileCol1Name);
            dataTable.Rows[0][FieldFileCol1Name].ShouldBe(string.Format(" {0}", FieldFileCol2Name));
        }
    }
}
