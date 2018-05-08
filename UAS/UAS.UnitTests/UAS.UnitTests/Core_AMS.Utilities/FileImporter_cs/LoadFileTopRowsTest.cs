using System;
using Core_AMS.Utilities;
using KM.Common.Import;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities.FileImporter_cs
{
    [TestFixture]
    public class LoadFileTopRows : Fakes
    {
        [Test]
        public void LoadFileTopRows_EmptyFile_NullReference()
        {
            // Arrange, Act, Assert
            Should.Throw<NullReferenceException>(() => FileImporter.LoadFileTopRows(FileInfo1, null));
        }

        [Test]
        public void LoadFileTopRows_Row2Top1_DataRow1()
        {
            // Arrange
            FillFileCol2Row2();

            // Act
            var dataTable = FileImporter.LoadFileTopRows(FileInfo1, null);

            // Assert
            dataTable.ShouldNotBeNull();
            
            dataTable.Columns.Count.ShouldBe(2);
            dataTable.Columns[0].ColumnName.ShouldBe(FieldFileCol1Name);
            dataTable.Columns[1].ColumnName.ShouldBe(FieldFileCol2Name);

            dataTable.Rows.Count.ShouldBe(1);
        }

        [Test]
        public void LoadFileTopRows_Row2Top2_DataRow2()
        {
            // Arrange
            FillFileCol2Row2();

            // Act
            var dataTable = FileImporter.LoadFileTopRows(FileInfo1, null, 2);

            // Assert
            dataTable.ShouldNotBeNull();
            dataTable.Columns.Count.ShouldBe(2);
            dataTable.Columns[0].ColumnName.ShouldBe(FieldFileCol1Name);
            dataTable.Columns[1].ColumnName.ShouldBe(FieldFileCol2Name);
            dataTable.Rows.Count.ShouldBe(2);
            dataTable.Rows[0][FieldFileCol1Name].ShouldBe(ValueFile1);
            dataTable.Rows[0][FieldFileCol2Name].ShouldBe(ValueFile2);
            dataTable.Rows[1][FieldFileCol1Name].ShouldBe(ValueFile3);
            dataTable.Rows[1][FieldFileCol2Name].ShouldBe(ValueFile4);
        }
    }
}
