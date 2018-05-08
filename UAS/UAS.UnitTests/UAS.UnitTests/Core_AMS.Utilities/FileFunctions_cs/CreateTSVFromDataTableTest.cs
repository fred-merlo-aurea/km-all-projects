using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core_AMS.Utilities;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities.FileFunctions_cs
{
    [TestFixture]
    public class CreateTSVFromDataTableTest
    {
        [Test]
        public void CreateTSVFromDataTable_IterateDelimiters_ReturnsVoid()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var resultList = new List<string>();
                var testObject = new FileFunctions();
                var testTable = new DataTable();
                testTable.Columns.Add("Test1");
                testTable.Columns.Add("Test2");

                testTable.Rows.Add("testValue1", "testValue2");

                ShimFile.WriteAllLinesStringIEnumerableOfString = (_, lines) =>
                {
                    resultList.AddRange(lines);
                };
                ShimFile.AppendAllLinesStringIEnumerableOfString = (_, lines) =>
                {
                    resultList.AddRange(lines);
                };

                // Act
                testObject.CreateTSVFromDataTable(testTable, "sampleFile", false);

                // Assert
                resultList.Count.ShouldBe(2);
                resultList[0].ShouldBe("Test1\tTest2");
                resultList[1].ShouldBe("testValue1\ttestValue2");
            }
        }
    }
}
