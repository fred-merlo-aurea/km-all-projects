using System;
using System.Diagnostics.CodeAnalysis;
using System.Data;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using ecn.common.classes.Fakes;

namespace ECN.Tests.Common
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class BlastContentTest
    {
        private IDisposable _shimObject;
        private PrivateType _testedClass;
        private DataTable _dataTableContent;
        private const string AssemblyName = "ECN";
        private const string ClassName = "ecn.common.classes.BlastContent";
        private const string DataTableShortNameColumnName = "shortname";
        private const string DataTableContentColumnName = "Content";
        private const string DataTableOtherShortNameColumnName = "dummyShortName";
        private const string DataTableContentRegexMathcingValue = "%%";
        private const string DataTableContentRegexMathcingValueTwice = "%%Content%%";
        private const string DataTableContentRegexMathcingValueMoreThanOnce = "%%Content%%Wrong%%%%";

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _testedClass = new PrivateType(AssemblyName, ClassName);

            _dataTableContent = new DataTable();
            _dataTableContent.Columns.Add(DataTableShortNameColumnName);
            _dataTableContent.Columns.Add(DataTableContentColumnName);
            _dataTableContent.Rows.Add(DataTableShortNameColumnName, DataTableContentColumnName);

            ShimDataFunctions.GetDataTableStringString =
                (sql, connString) =>
                {
                    return _dataTableContent;
                };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}
