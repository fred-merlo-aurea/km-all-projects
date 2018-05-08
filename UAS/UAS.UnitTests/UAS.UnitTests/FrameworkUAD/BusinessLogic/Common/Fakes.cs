﻿using System;
using System.IO;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

using FrameworkUAD.BusinessLogic;

namespace UAS.UnitTests.FrameworkUAD.BusinessLogic.Common
{
    public class Fakes
    {
        protected IDisposable shimObject;
        protected PrivateObject privateObject;

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
        protected static readonly string ValueFile1 = "1";
        protected static readonly string ValueFile2 = "2";
        protected static readonly string ValueFile3 = "3";
        protected static readonly string ValueFile4 = "4";
        protected static readonly string ValueFile5 = "5";
        protected static readonly string ValueFile6 = "6";

        protected static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;

        protected static readonly string SampleFolderPath = Path.Combine(BasePath, "ImportFile\\sample\\");
        protected static readonly string FileName1 = Path.Combine(SampleFolderPath, "1.txt");
        protected static readonly FileInfo FileInfo1 = new FileInfo(FileName1);

        [SetUp]
        public void Setup()
        {
            shimObject = ShimsContext.Create();
            privateObject = new PrivateObject(typeof(ImportFile));

            Directory.CreateDirectory(SampleFolderPath);
            using (var writer = File.CreateText(FileName1));
        }

        [TearDown]
        public void DisposeContext()
        {
            File.Delete(FileName1);

            shimObject.Dispose();
        }

        protected static void FillFileCol2RowCol2()
        {
            using (var writer = File.CreateText(FileName1))
            {
                writer.WriteLine(string.Format("{0}, {1}", FieldFileCol1Name, FieldFileCol2Name));
                writer.WriteLine(string.Format("{0}, {1}", ValueFile1, ValueFile2));
            }
        }

        protected static void FillFileCol2RowCol1()
        {
            using (var writer = File.CreateText(FileName1))
            {
                writer.WriteLine(string.Format("{0}, {1}", FieldFileCol1Name, FieldFileCol2Name));
                writer.WriteLine(string.Format("{0}", ValueFile1));
            }
        }

        protected static void FillFileCol2Row2()
        {
            using (var writer = File.CreateText(FileName1))
            {
                writer.WriteLine(string.Format("{0}, {1}", FieldFileCol1Name, FieldFileCol2Name));
                writer.WriteLine(string.Format("{0}, {1}", ValueFile1, ValueFile2));
                writer.WriteLine(string.Format("{0}, {1}", ValueFile3, ValueFile4));
            }
        }
    }
}
