using System;
using System.Collections.Generic;
using System.Data;
using System.Fakes;
using System.IO;
using System.IO.Fakes;
using System.Text;
using KM.Common.Entity;
using KM.Common.Entity.Fakes;
using MAF.NorthStarExport.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;

namespace MAF.NorthStarExport.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        private const string CreateNonXmlMethodName = "CreateNonXML";
        private const string CreateWSR_NonXMLMethodName = "CreateWSR_NonXML";
        private const string CreateFileMethodName = "CreateFile";
        private const string ExportFiles = "ExportFileTest";
        private const string Folder = "Folder";
        private const string SampleFile = "sample.txt";
        private const string CustomerFTPFolder = "CustomerFTPFolder";
        private const string TestExceptionMessage = "Test Exception";
        private const string InnerExceptionMessage = "Inner Exception";
        private const string CurrentCustomerFieldName = "CurrentCustomer";
        private const string FileInfoTestString = "FileInfoTestString";
        private const string TestBrandCode = "TestBrandCode";
        public readonly string ExpectedException = "**********************" + Environment.NewLine +
                                                   "-- Data --" + Environment.NewLine +
                                                   "-- Message --" + Environment.NewLine +
                                                   "Test Exception" + Environment.NewLine +
                                                   "-- InnerException --" + Environment.NewLine +
                                                   "System.Exception: Inner Exception" + Environment.NewLine +
                                                   "**********************" + Environment.NewLine;

        private static readonly DateTime TestDateTimeNow = new DateTime(2015, 06, 30);
        private static readonly Dictionary<ColumnDelimiter, string> DelimiterToString = new Dictionary<ColumnDelimiter, string>()
        {
            [ColumnDelimiter.colon] = ":",
            [ColumnDelimiter.comma] = ",",
            [ColumnDelimiter.semicolon] = ";",
            [ColumnDelimiter.tab] = "\t",
            [ColumnDelimiter.tild] = "~"
        };

        private readonly Program _target = new Program();
        private readonly Customer _customer = new Customer();
        private IDisposable _shims;

        [SetUp]
        public void Setup()
        {
            _shims = ShimsContext.Create();
            _customer.WebsiteSubscriberRequest_FTPFolder = CustomerFTPFolder;
            ReflectionHelper.SetValue(_target, "CurrentCustomer", _customer);
        }

        [TearDown]
        public void TearDown()
        {
            if (_shims != null)
            {
                _shims.Dispose();
            }
        }

        [Test]
        public void UploadBrandFile_DirectoryDoesntExists_ReturnsTrueAndCallsUpload()
        {
            // Arrange
            var fileInfo = new FileInfo(SampleFile);
            var brand = new Brand
            {
                FtpFolder = Folder
            };

            var directoryName = string.Empty;
            var remoteFile = string.Empty;
            var localFile = string.Empty;

            KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.CreateDirectoryString = (functions, newDirectory) => { directoryName = newDirectory; };
            KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.UploadStringStringBoolean = (functions, remote, local,_) =>
            {
                remoteFile = remote;
                localFile = local;
                return true;
            };
            
            // Act
            var result = (bool)ReflectionHelper.CallMethod(_target, "UploadBrandFile", fileInfo, brand);

            // Assert
            result.ShouldBeTrue();
            directoryName.ShouldBe(string.Format("{0}/", Folder));
            remoteFile.ShouldBe(string.Format("{0}/{1}", Folder, SampleFile));
            localFile.ShouldEndWith(SampleFile);
        }

        [Test]
        public void Upload_WSR_File_DirectoryDoesntExists_ReturnsTrueAndCallsUpload()
        {
            // Arrange
            var fileInfo = new FileInfo(SampleFile);
            var directoryName = string.Empty;
            var remoteFile = string.Empty;
            var localFile = string.Empty;

            KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.CreateDirectoryString = (functions, newDirectory) => { directoryName = newDirectory; };
            KM.Common.Functions.Fakes.ShimFtpFunctions.AllInstances.UploadStringStringBoolean = (functions, remote, local,_) =>
            {
                remoteFile = remote;
                localFile = local;
                return true;
            };
            
            // Act
            var result = (bool)ReflectionHelper.CallMethod(_target, "Upload_WSR_File", fileInfo);

            // Assert
            result.ShouldBeTrue();
            directoryName.ShouldBe(string.Format("{0}/", CustomerFTPFolder));
            remoteFile.ShouldBe(string.Format("{0}/{1}", CustomerFTPFolder, SampleFile));
            localFile.ShouldEndWith(SampleFile);
        }

        [Test]
        public void LogCustomerExeception_ExceptionWithInnerException_LogCorrectString()
        {
            // Arrange
            var innerException = new Exception(InnerExceptionMessage);
            var exception = new Exception(TestExceptionMessage, innerException);
            var expectedLog = string.Empty;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.CustomerLogWriteString = (_, s) => { expectedLog = s; };

            // Act
            _target.LogCustomerExeception(exception, string.Empty);

            // Assert
            expectedLog.ShouldBe(ExpectedException);
        }

        [Test]
        public void LogMainExeception_ExceptionWithInnerException_LogCorrectString()
        {
            // Arrange
            var innerException = new Exception(InnerExceptionMessage);
            var exception = new Exception(TestExceptionMessage, innerException);
            var expectedLog = string.Empty;

            ShimApplicationLog.SaveApplicationLogRef = (ref ApplicationLog log) => true;
            ShimProgram.AllInstances.MainLogWriteString = (_, s) => { expectedLog = s; };

            // Act
            _target.LogMainExeception(exception, string.Empty);

            // Assert
            expectedLog.ShouldBe(ExpectedException);
        }

        [Test]
        public void CreateFile_DirectoryExistsFileNot_FileWritten()
        {
            // Arrange
            var program = CreateProgram();
            var actualPath = string.Empty;
            var actualContent = string.Empty;

            ShimDateTime.NowGet = () => TestDateTimeNow;

            ShimDirectory.ExistsString = _ => true;
            ShimFile.ExistsString = _ => false;
            ShimFile.WriteAllTextStringString = (path, content) =>
            {
                actualPath = path;
                actualContent = content;
            };

            // Act
            var fileInfo = CallCreateFile(program, CreateBrand(), new StringBuilder());

            // Assert
            fileInfo.ShouldSatisfyAllConditions(
                () => fileInfo.ShouldNotBeNull(),
                () => actualPath.ShouldBe("ExportFileTest\\Welcome_TestBrandCode_20150630.xls"),
                () => actualContent.ShouldBe(string.Empty));
        }
        
        [Test]
        public void CreateFile_DirectoryExistsFileExists_FileWritten()
        {
            // Arrange
            var program = CreateProgram();
            var actualPath = string.Empty;
            var actualContent = string.Empty;
            var fileDeleteCalled = 0;

            ShimDateTime.NowGet = () => TestDateTimeNow;

            ShimDirectory.ExistsString = _ => true;
            ShimFile.ExistsString = _ => true;
            ShimFile.DeleteString = file => fileDeleteCalled++;
            
            ShimFile.WriteAllTextStringString = (path, content) =>
            {
                actualPath = path;
                actualContent = content;
            };

            // Act
            var fileInfo = CallCreateFile(program, CreateBrand(), new StringBuilder());

            // Assert
            fileInfo.ShouldSatisfyAllConditions(
                () => fileDeleteCalled.ShouldBe(1),
                () => fileInfo.ShouldNotBeNull(),
                () => actualPath.ShouldBe("ExportFileTest\\Welcome_TestBrandCode_20150630.xls"),
                () => actualContent.ShouldBe(string.Empty));
        } 
        
        [Test]
        public void CreateFile_DirectoryDoesNotExistFileExists_FileWritten()
        {
            // Arrange
            var program = CreateProgram();
            var actualPath = string.Empty;
            var actualCreateDirectoryPath = string.Empty;
            var actualContent = string.Empty;
            var fileDeleteCalled = 0;

            ShimDateTime.NowGet = () => TestDateTimeNow;

            ShimDirectory.ExistsString = _ => false;
            ShimDirectory.CreateDirectoryString = path =>
            {
                actualCreateDirectoryPath = path;
                return new DirectoryInfo(path);
            };

            ShimFile.ExistsString = _ => true;
            ShimFile.DeleteString = file => fileDeleteCalled++;
            
            ShimFile.WriteAllTextStringString = (path, content) =>
            {
                actualPath = path;
                actualContent = content;
            };

            // Act
            var fileInfo = CallCreateFile(program, CreateBrand(), new StringBuilder());

            // Assert
            fileInfo.ShouldSatisfyAllConditions(
                () => fileDeleteCalled.ShouldBe(1),
                () => fileInfo.ShouldNotBeNull(),
                () => actualPath.ShouldBe("ExportFileTest\\Welcome_TestBrandCode_20150630.xls"),
                () => actualCreateDirectoryPath.ShouldBe("ExportFileTest\\"),
                () => actualContent.ShouldBe(string.Empty));
        }

        [Test]
        public void CreateNonXml_EmptyDataTableGiven_ContentEmpty()
        {
            // Arrange
            var program = CreateProgram();
            var actualContent = string.Empty;

            ShimProgram.AllInstances.CreateFileBrandStringBuilder = (_, brand, builder) =>
            {
                actualContent = builder.ToString();
                return new FileInfo(FileInfoTestString);
            };
            
            // Act
            var fileInfo = CallCreateNonXml(program, new DataTable(), CreateBrand());

            // Assert
            fileInfo.ShouldSatisfyAllConditions(
                () => fileInfo.Name.ShouldBe(FileInfoTestString),
                () => actualContent.ShouldBe("\r\n"));
        } 
        
        [Test]
        public void CreateNonXml_DataTableGiven_ContentContainsColumns()
        {
            // Arrange
            var program = CreateProgram();
            var actualContent = string.Empty;

            ShimProgram.AllInstances.CreateFileBrandStringBuilder = (_, brand, builder) =>
            {
                actualContent = builder.ToString();
                return new FileInfo(FileInfoTestString);
            };
            
            // Act
            var fileInfo = CallCreateNonXml(program, CreateTestDataTable(), CreateBrand());

            // Assert
            fileInfo.ShouldSatisfyAllConditions(
                () => fileInfo.Name.ShouldBe(FileInfoTestString),
                () => actualContent.ShouldBe(":Column1\tCol:umn2\tColumn3:\r\nRecall\tHauser\tMars\r\nZeds\tDead\tBaby\r\n"));
        }
        
        [Test]
        public void CreateNonXml_DataTableGivenQuoteEnabled_ContentContainsColumns()
        {
            // Arrange
            var program = CreateProgram();
            var brand = new Brand { FileExtension = ".txt", IsQuoteEncapsulated = true };
            var actualContent = string.Empty;

            ShimProgram.AllInstances.CreateFileBrandStringBuilder = (_, __, builder) =>
            {
                actualContent = builder.ToString();
                return new FileInfo(FileInfoTestString);
            };
            
            // Act
            var fileInfo = CallCreateNonXml(program, CreateTestDataTable(), brand);

            // Assert
            fileInfo.ShouldSatisfyAllConditions(
                () => fileInfo.Name.ShouldBe(FileInfoTestString),
                () => actualContent.ShouldBe("\":Column1\",\"Col:umn2\",\"Column3:\"\r\n\"Recall\",\"Hauser\",\"Mars\"\r\n\"Zeds\",\"Dead\",\"Baby\"\r\n"));
        }
        
        [Test]
        [TestCase(ColumnDelimiter.colon)]
        [TestCase(ColumnDelimiter.comma)]
        [TestCase(ColumnDelimiter.semicolon)]
        [TestCase(ColumnDelimiter.tab)]
        [TestCase(ColumnDelimiter.tild)]
        public void CreateNonXml_DataTableGivenColumnDelimiter_ContentContainsColumns(ColumnDelimiter columnDelimiter)
        {
            // Arrange
            var program = CreateProgram();
            var brand = new Brand
            {
                FileExtension = ".txt", 
                IsQuoteEncapsulated = true,
                ColumnDelimiter = columnDelimiter.ToString()
            };

            var actualContent = string.Empty;
            var delimiter = DelimiterToString[columnDelimiter];

            ShimProgram.AllInstances.CreateFileBrandStringBuilder = (_, __, builder) =>
            {
                actualContent = builder.ToString();
                return new FileInfo(FileInfoTestString);
            };
            
            // Act
            var fileInfo = CallCreateNonXml(program, CreateTestDataTable(columnDelimiter), brand);

            // Assert
            fileInfo.ShouldSatisfyAllConditions(
                () => fileInfo.Name.ShouldBe(FileInfoTestString),
                () => actualContent.ShouldBe($"\"{delimiter}Column1\"{delimiter}\"Col{delimiter}umn2\"{delimiter}\"Column3{delimiter}\"\r\n\"Recall\"{delimiter}\"Hauser\"{delimiter}\"Mars\"\r\n\"Zeds\"{delimiter}\"Dead\"{delimiter}\"Baby\"\r\n"));
        } 
        
        [Test]
        [TestCase(ColumnDelimiter.colon)]
        [TestCase(ColumnDelimiter.comma)]
        [TestCase(ColumnDelimiter.semicolon)]
        [TestCase(ColumnDelimiter.tab)]
        [TestCase(ColumnDelimiter.tild)]
        public void CreateWSRNonXML_FileExists_ContentContainsColumns(ColumnDelimiter columnDelimiter)
        {
            // Arrange
            var program = CreateProgram();
            var customer = new Customer
            {
                WebsiteSubscriberRequest_ColumnDelimiter = columnDelimiter.ToString(),
                WebsiteSubscriberRequest_FileExtension = ".txt",
                WebsiteSubscriberRequest_IsQuoteEncapsulated = true
            };
            program.SetField(CurrentCustomerFieldName, customer);
            var actualAppendFileName = string.Empty;

            ShimDateTime.NowGet = () => TestDateTimeNow;
            ShimFile.ExistsString = fileName => true;

            ShimFile.AppendTextString = fileName =>
            {
                actualAppendFileName = fileName;
                return new ShimStreamWriter().Instance;
            };

            var actualContent = string.Empty;
            var delimiter = DelimiterToString[columnDelimiter];

            ShimTextWriter.AllInstances.WriteLineString = (writer, content) =>
            {
                actualContent = content;
            };
            
            // Act
            var fileInfo = CallCreateWSR_NonXML(program, CreateTestDataTable(columnDelimiter));

            // Assert
            fileInfo.ShouldSatisfyAllConditions(
                () => actualAppendFileName.ShouldBe("\\20150630..txt"),
                () => fileInfo.Name.ShouldBe("20150630..txt"),
                () => actualContent.ShouldBe($"\"Recall\"{delimiter}\"Hauser\"{delimiter}\"Mars\"\r\n\"Zeds\"{delimiter}\"Dead\"{delimiter}\"Baby\""));
        }
        
        [Test]
        [TestCase(ColumnDelimiter.colon)]
        [TestCase(ColumnDelimiter.comma)]
        [TestCase(ColumnDelimiter.semicolon)]
        [TestCase(ColumnDelimiter.tab)]
        [TestCase(ColumnDelimiter.tild)]
        public void CreateWSRNonXML_FileDoesNotExists_ContentContainsColumns(ColumnDelimiter columnDelimiter)
        {
            // Arrange
            var program = CreateProgram();
            var customer = new Customer
            {
                WebsiteSubscriberRequest_ColumnDelimiter = columnDelimiter.ToString(),
                WebsiteSubscriberRequest_FileExtension = ".txt",
                WebsiteSubscriberRequest_IsQuoteEncapsulated = true
            };
            program.SetField(CurrentCustomerFieldName, customer);
            var actualCreateFileName = string.Empty;

            ShimDateTime.NowGet = () => TestDateTimeNow;
            ShimFile.ExistsString = fileName => false;

            ShimFile.CreateTextString = fileName =>
            {
                actualCreateFileName = fileName;
                return new ShimStreamWriter().Instance;
            };

            var actualContent = string.Empty;
            var delimiter = DelimiterToString[columnDelimiter];

            ShimTextWriter.AllInstances.WriteLineString = (writer, content) =>
            {
                actualContent = content;
            };
            
            // Act
            var fileInfo = CallCreateWSR_NonXML(program, CreateTestDataTable(columnDelimiter));

            // Assert
            fileInfo.ShouldSatisfyAllConditions(
                () => actualCreateFileName.ShouldBe("\\20150630..txt"),
                () => fileInfo.Name.ShouldBe("20150630..txt"),
                () => actualContent.ShouldBe($"\"{delimiter}Column1\"{delimiter}\"Col{delimiter}umn2\"{delimiter}\"Column3{delimiter}\"\r\n\"Recall\"{delimiter}\"Hauser\"{delimiter}\"Mars\"\r\n\"Zeds\"{delimiter}\"Dead\"{delimiter}\"Baby\""));
        }
        
        private static Program CreateProgram()
        {
            var customer = new Customer { ExportedFiles = ExportFiles };
            
            var program = new Program();
            program.SetField(CurrentCustomerFieldName, customer);

            return program;
        }

        private static Brand CreateBrand()
        {
            return new Brand
                       {
                           BrandCode = TestBrandCode,
                           FileExtension = "xls",
                           IsQuoteEncapsulated = false
                       };
        }

        private static DataTable CreateTestDataTable(ColumnDelimiter columnDelimiter = ColumnDelimiter.colon)
        {
            var dataTable = new DataTable("TestTableName");

            dataTable.Columns.Add($"{DelimiterToString[columnDelimiter]}Column1");
            dataTable.Columns.Add($"Col{DelimiterToString[columnDelimiter]}umn2");
            dataTable.Columns.Add($"Column3{DelimiterToString[columnDelimiter]}");

            dataTable.Rows.Add("Recall", "Hauser", "Mars");
            dataTable.Rows.Add("Zeds", "Dead", "Baby");
            return dataTable;
        }

        private static FileInfo CallCreateFile(Program program, Brand brand, StringBuilder builder)
        {
            return (FileInfo)typeof(Program).CallMethod(
                CreateFileMethodName,
                new object[] { brand, builder },
                program);
        }

        private static FileInfo CallCreateNonXml(Program program, DataTable dataTable, Brand brand)
        {
            return (FileInfo)typeof(Program).CallMethod(
                CreateNonXmlMethodName,
                new object[] { dataTable, brand },
                program);
        }
        
        private static FileInfo CallCreateWSR_NonXML(Program program, DataTable dataTable)
        {
            return (FileInfo)typeof(Program).CallMethod(
                CreateWSR_NonXMLMethodName,
                new object[] { dataTable },
                program);
        }
    }
}
