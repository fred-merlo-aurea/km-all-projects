using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using KMPS.MD.Controls;
using KMPS.MD.Controls.Fakes;
using KMPS.MD.Helpers;
using KMPS.MD.Main;
using KMPS.MD.Main.Fakes;
using NUnit.Framework;
using Shouldly;

namespace KMPS.MD.Tests.Helpers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class JsonHelperTests
    {
        private const string IdColumn = "Id";
        private const string NameColumn = "Name";

        [Test]
        public void GetJsonStringFromDataTable_DataTable_CorrectJson()
        {
            // Arrange
            var dataTable = new DataTable("Results");
            dataTable.Columns.Add(IdColumn);
            dataTable.Columns.Add(NameColumn);

            var row = dataTable.NewRow();
            row[IdColumn] = 1;
            row[NameColumn] = "Test1";
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row[IdColumn] = 2;
            row[NameColumn] = "Test2";
            dataTable.Rows.Add(row);

            // Act
            var result = JsonHelper.GetJsonStringFromDataTable(dataTable);

            // Assert
            result.ShouldBe("{\"Results\":[{\"Id\":\"1\",\"Name\":\"Test1\"},{\"Id\":\"2\",\"Name\":\"Test2\"}]}");
        }
    }
}
