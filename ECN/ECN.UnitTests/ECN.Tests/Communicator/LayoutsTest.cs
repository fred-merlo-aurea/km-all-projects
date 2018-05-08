using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.common.classes.Fakes;
using ecn.communicator.classes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Tests.Communicator
{
    [TestFixture]
    public class LayoutsTest
    {
        private IDisposable _shimObject;
        private DataTable _dataTable;
        private const string Expected = "<table TableOptionsValue width='100%'><tr><td>Source1</td></tr></table>";
        private const string ExpectedMobileTrue = "<table TableOptionsValue width='100%'><tr><td>Source1True</td></tr></table>";
        private const string ExpectedMobileFalse = "<table TableOptionsValue width='100%'><tr><td>Source1False</td></tr></table>";
        private const string ColumnTemplateSource = "TemplateSource";
        private const string ColuumnTableOptions = "TableOptions";
        private const string ColumnContentSlot1 = "ContentSlot1";
        private const string ColumnContentSlot2 = "ContentSlot2";
        private const string ColumnContentSlot3 = "ContentSlot3";
        private const string ColumnContentSlot4 = "ContentSlot4";
        private const string ColumnContentSlot5 = "ContentSlot5";
        private const string ColumnContentSlot6 = "ContentSlot6";
        private const string ColumnContentSlot7 = "ContentSlot7";
        private const string ColumnContentSlot8 = "ContentSlot8";
        private const string ColumnContentSlot9 = "ContentSlot9";
        private const string TemplateSourceValue = "Source%%slot1%%";
        private const string TableOptionsValue = "TableOptionsValue";
        private const int SlotId = 1;

        [SetUp]
        public void Setup()
        {
            _shimObject = ShimsContext.Create();
            SetupShims();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void GetHTMLPreview_WithSuccess()
        {
            // Arrange
            var layouts = new Layouts();

            // Act
            var result = layouts.GetHTMLPreview();

            // Assert
            result.ShouldBe(Expected);
        }

        [Test]
        public void GetHTMLPreview_IsMobile_WithSuccess()
        {
            // Arrange
            var layouts = new Layouts();

            // Act
            var result = layouts.GetHTMLPreview(true);

            // Assert
            result.ShouldBe(ExpectedMobileTrue);
        }

        [Test]
        public void GetHTMLPreview_IsMobileFalse_WithSuccess()
        {
            // Arrange
            var layouts = new Layouts();

            // Act
            var result = layouts.GetHTMLPreview(false);

            // Assert
            result.ShouldBe(ExpectedMobileFalse);
        }

        private void SetupShims()
        {
            _dataTable = new DataTable();
            _dataTable.Columns.Add(ColumnTemplateSource);
            _dataTable.Columns.Add(ColuumnTableOptions);
            _dataTable.Columns.Add(ColumnContentSlot1, typeof(int));
            _dataTable.Columns.Add(ColumnContentSlot2, typeof(int));
            _dataTable.Columns.Add(ColumnContentSlot3, typeof(int));
            _dataTable.Columns.Add(ColumnContentSlot4, typeof(int));
            _dataTable.Columns.Add(ColumnContentSlot5, typeof(int));
            _dataTable.Columns.Add(ColumnContentSlot6, typeof(int));
            _dataTable.Columns.Add(ColumnContentSlot7, typeof(int));
            _dataTable.Columns.Add(ColumnContentSlot8, typeof(int));
            _dataTable.Columns.Add(ColumnContentSlot9, typeof(int));

            _dataTable.Rows.Add(TemplateSourceValue, TableOptionsValue, SlotId, SlotId, SlotId, SlotId, SlotId, SlotId, SlotId, SlotId, SlotId);

            ShimDataFunctions.GetDataTableString = (_) => _dataTable;

            ShimDataFunctions.GetContentInt32 = (val) => val.ToString();
            ShimDataFunctions.GetContentInt32Boolean = (val, isMobile) => val.ToString() + isMobile.ToString();
        }
    }
}
