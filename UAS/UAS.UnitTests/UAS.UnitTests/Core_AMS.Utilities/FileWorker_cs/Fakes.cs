using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using NUnit.Framework;
using Shouldly;

namespace UAS.UnitTests.Core_AMS.Utilities.FileWorker_cs
{
    [ExcludeFromCodeCoverage]
    public class Fakes
    {
        protected static readonly string TableDataName = "Data";
        protected static readonly string TableErrorsName = "Errors";
        protected static readonly string TableCountsName = "Counts";

        protected static readonly string FieldErrorsClientMessage = "ClientMessage";
        protected static readonly string FieldFormattedError = "FormattedError";
        protected static readonly string FieldCountsTotalRows = "TotalRows";
        protected static readonly string FieldCountsRowImportCount = "RowImportCount";
        protected static readonly string FieldCountsRowErrorCount = "RowErrorCount";

        protected static readonly string ErrorImportFile = "An error has been detected with your import file:";
        protected static readonly string ErrorExpectedColumnCount = "Expected column count is 2 but this row column count is  1";

        protected static readonly string FieldFileCol1Name = "col1";
        protected static readonly string FieldFileCol2Name = "col2";
        protected static readonly string FieldFileCol1UnicodeName = "столбец 1";
        protected static readonly string FieldFileCol2UnicodeName = "столбец 2";
        protected static readonly string ValueFile1 = "1";
        protected static readonly string ValueFile2 = "2";
        
        protected static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
        protected static readonly string SampleFolderPath = Path.Combine(BasePath, "FileWorker\\sample\\");
        protected static readonly string FileName1 = Path.Combine(SampleFolderPath, "1.txt");
        protected static readonly FileInfo FileInfo1 = new FileInfo(FileName1);

        [SetUp]
        public void Setup()
        {
            Directory.CreateDirectory(SampleFolderPath);
            using (var writer = File.CreateText(FileName1));
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(FileName1);
        }

        protected static void FillFileHeaderCol2NonUnicodeRowCol2()
        {
            using (var writer = File.CreateText(FileName1))
            {
                writer.WriteLine(string.Format("{0}, {1}", FieldFileCol1Name, FieldFileCol2Name));
                writer.WriteLine(string.Format("{0}, {1}", ValueFile1, ValueFile2));
            }
        }

        protected static void FillFileHeaderCol2UnicodeRowCol2()
        {
            using (var writer = File.CreateText(FileName1))
            {
                writer.WriteLine(string.Format("{0}, {1}", FieldFileCol1UnicodeName, FieldFileCol2UnicodeName));
                writer.WriteLine(string.Format("{0}, {1}", ValueFile1, ValueFile2));
            }
        }

        protected static void FillFileRow1Col2Semicolon()
        {
            using (var writer = File.CreateText(FileName1))
            {
                writer.WriteLine(string.Format("{0};{1}", ValueFile1, ValueFile2));
            }
        }

        protected void AssertDatasetTables(DataSet dataSet, out DataTable dtData, out DataTable dtErrors, out DataTable dtCounts)
        {
            dataSet.ShouldNotBeNull();

            dtData = dataSet.Tables[TableDataName];
            dtData.ShouldNotBeNull();

            dtErrors = dataSet.Tables[TableErrorsName];
            dtErrors.ShouldNotBeNull();

            dtCounts = dataSet.Tables[TableCountsName];
            dtCounts.ShouldNotBeNull();
        }
    }
}
