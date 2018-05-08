using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Core.ADMS.Fakes;
using Core_AMS.Utilities;
using Core_AMS.Utilities.Fakes;
using DQM.Helpers.Validation;
using DQM.Helpers.Validation.Fakes;
using FrameworkServices.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using UAS.UnitTests.DQM.Helpers.Validation.Common;
using UAS_WS.Interface;
using UADWSInterface = UAD_WS.Interface;
using FrameworkUASService = FrameworkUAS.Service;
using UAD_WS.Interface.Fakes;
using UADWSInterfaceFake = UAS_WS.Interface.Fakes;
using NUnit.Framework;
using System.Threading;
using KM.Common.Import;
using FrameworkUADBusinessLogic = FrameworkUAD.BusinessLogic;

namespace UAS.UnitTests.DQM.Helpers.Validation.Validation_cs
{
    /// <summary>
    /// Unit Tests for <see cref="FileValidator"/> class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class ValidateFileAsObjectTest : Fakes
    {
        private FileValidator fileValidator;
        private const string fileName = "ValidateFileAsObjectTest.csv";
        private const string qDateFormat = "MDDYY";
        private const string dQmTestFilePath = "DQM\\test.txt";
        private static string path = AppDomain.CurrentDomain.BaseDirectory;

        [SetUp]
        public void Setup()
        {

            fileValidator = new FileValidator();
            SetupFakes();
            ShimBaseDirs.getAppsDir = () =>
            {
                var dQMfilePath = Path.Combine(path, dQmTestFilePath);
                CreateSettings(dQMfilePath, string.Empty);
                return path;
            };
        }

        [Test]
        public void ValidateFileAsObject_AcceptableFileTypeIsTrue_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, fileName);
            var content = CreateFileContent();
            CreateSettings(filePath, content);
            var myCheckFile = new FileInfo(filePath);
            var myClient = new KMPlatform.Entity.Client();
            var mySourceFile = CreateSourceFile(true);

            // Act
            var result = fileValidator.ValidateFileAsObject(myCheckFile, myClient, mySourceFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            foreach (var item in result)
            {
                Assert.IsTrue(File.Exists(item.Value));
            }
        }


        [Test]
        public void ValidateFileAsObject_AcceptableFileTypeIsTrueAndIsTextQualifierIsFalse_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, fileName);
            var content = CreateFileContent();
            CreateSettings(filePath, content);
            var myCheckFile = new FileInfo(filePath);
            var myClient = new KMPlatform.Entity.Client();
            var mySourceFile = CreateSourceFile(false);

            ShimFileWorker.AllInstances.GetFirstCharacterFileInfo = (x, y) => { return '"'; };
            ShimFileWorker.AllInstances.AcceptableFileTypeFileInfo = (x, y) => { return false; };

            // Act
            var result = fileValidator.ValidateFileAsObject(myCheckFile, myClient, mySourceFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            foreach (var item in result)
            {
                Assert.IsTrue(File.Exists(item.Value));
            }
        }

        [Test]
        public void ValidateFileAsObject_AcceptableFileIsEmpty_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, fileName);
            CreateSettings(filePath, string.Empty);
            var myCheckFile = new FileInfo(filePath);
            var myClient = new KMPlatform.Entity.Client();
            var mySourceFile = CreateSourceFile(true);

            // Act
            var result = fileValidator.ValidateFileAsObject(myCheckFile, myClient, mySourceFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            foreach (var item in result)
            {
                Assert.IsTrue(File.Exists(item.Value));
            }
        }

        [Test]
        public void ValidateFileAsObject_PubCodeIsMissiong_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, fileName);
            var content = CreateFileContent();
            CreateSettings(filePath, content);
            var myCheckFile = new FileInfo(filePath);
            var myClient = new KMPlatform.Entity.Client();
            var mySourceFile = CreateSourceFile(true);
            CreateUasSourceFileClient();
            // Act
            var result = fileValidator.ValidateFileAsObject(myCheckFile, myClient, mySourceFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            foreach (var item in result)
            {
                Assert.IsTrue(File.Exists(item.Value));
            }
        }

        [Test]
        public void ValidateFileAsObject_TransformImportFileDataThrowException_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, fileName);
            var content = CreateFileContent();
            CreateSettings(filePath, content);
            var myCheckFile = new FileInfo(filePath);
            var myClient = new KMPlatform.Entity.Client();
            var mySourceFile = CreateSourceFile(true);
            ShimFileValidator.AllInstances.TransformImportFileData = (x) => { throw new Exception(); };
            var isImportError = false;
            ShimServiceClient.UAD_ImportErrorSummaryClient = () =>
            {
                return new ShimServiceClient<UADWSInterface.IImportErrorSummary>
                {
                    ProxyGet = () =>
                    {
                        return new StubIImportErrorSummary()
                        {
                            SelectGuidInt32StringClientConnections = (x, y, z, c) =>
                            {
                                isImportError = true;
                                throw new Exception();
                            }
                        };
                    }
                };
            };
            var isQSourceValidation = false;
            ShimServiceClient.UAD_OperationsClient = () =>
            {
                return new ShimServiceClient<UADWSInterface.IOperations>
                {
                    ProxyGet = () =>
                    {
                        return new StubIOperations()
                        {
                            QSourceValidationGuidInt32StringClientConnections = (x, y, z, m) =>
                            {
                                isQSourceValidation = true;
                                throw new Exception();
                            }
                        };
                    }
                };
            };
            var isCodeSheetValidation = false;
            ShimServiceClient.UAD_CodeSheetClient = () =>
            {
                return new ShimServiceClient<UADWSInterface.ICodeSheet>
                {
                    ProxyGet = () =>
                    {
                        return new StubICodeSheet()
                        {
                            CodeSheetValidationGuidInt32StringClientConnections = (x, y, z, n) =>
                            {
                                isCodeSheetValidation = true;
                                throw new Exception();
                            }
                        };
                    }
                };
            };

            // Act
            var result = fileValidator.ValidateFileAsObject(myCheckFile, myClient, mySourceFile);

            // Assert
            Assert.IsTrue(isImportError);
            Assert.IsTrue(isCodeSheetValidation);
            Assert.IsTrue(isQSourceValidation);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            foreach (var item in result)
            {
                Assert.IsTrue(File.Exists(item.Value));
            }
        }

        [Test]
        public void ValidateFileAsObject_DataTransformedObjectHaveValue_ReturnsOutputFilePathInObject()
        {
            // Arrange
            var filePath = Path.Combine(path, fileName);
            string content = CreateFileContent();
            CreateSettings(filePath, content);
            var myCheckFile = new FileInfo(filePath);
            var myClient = new KMPlatform.Entity.Client();
            var mySourceFile = CreateSourceFile(true);
            var fileConfig = new FileConfiguration()
            {
                FileColumnDelimiter = ",",
                IsQuoteEncapsulated = true,
            };
            var ifWorker = new FrameworkUADBusinessLogic.ImportFile();
            var dataIV = ifWorker.GetImportFile(myCheckFile, fileConfig);
            ShimImportFile.AllInstances.GetImportFileFileInfoFileConfiguration = (x, y, z) =>
            {
                dataIV.DataTransformed = dataIV.DataOriginal;
                dataIV.HeadersTransformed = dataIV.HeadersOriginal;
                return dataIV;
            };

            // Act
            var result = fileValidator.ValidateFileAsObject(myCheckFile, myClient, mySourceFile);

            // Assert
            Assert.IsNotNull(result);
            foreach (var item in result)
            {
                Assert.IsTrue(File.Exists(item.Value));
            }
        }

        private void CreateUasSourceFileClient()
        {
            ShimServiceClient.UAS_SourceFileClient = () =>
            {
                return new ShimServiceClient<ISourceFile>
                {
                    ProxyGet = () =>
                    {
                        return new UADWSInterfaceFake.StubISourceFile()
                        {
                            SelectForSourceFileGuidInt32Boolean = (x, y, z) =>
                            {
                                return new FrameworkUASService.Response<SourceFile>
                                {
                                    Result = new SourceFile
                                    {
                                        IsSpecialFile = true,
                                        IsTextQualifier = false,
                                        QDateFormat = qDateFormat,
                                        FieldMappings = new HashSet<FieldMapping>
                                        {
                                            new FieldMapping
                                            {
                                                IsNonFileColumn = true,
                                                MAFField="b",
                                                ColumnOrder=1,
                                                IncomingField="b",
                                                DataType=string.Empty,
                                                PreviewData=string.Empty,
                                                FieldMappingTypeID=3
                                            },
                                            new FieldMapping
                                            {
                                                IsNonFileColumn = true,
                                                MAFField=Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                                                ColumnOrder=1,
                                                IncomingField=Enums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                                                DataType=string.Empty,
                                                PreviewData=string.Empty,
                                                FieldMappingTypeID=4
                                            },
                                             new FieldMapping
                                            {
                                                IsNonFileColumn = true,
                                                MAFField=Enums.MAFFieldStandardFields.COMPANY.ToString(),
                                                ColumnOrder=1,
                                                IncomingField=Enums.MAFFieldStandardFields.COMPANY.ToString(),
                                                DataType=string.Empty,
                                                PreviewData=string.Empty,
                                                FieldMappingTypeID=5
                                            },
                                              new FieldMapping
                                            {
                                                IsNonFileColumn = true,
                                                MAFField=Enums.MAFFieldStandardFields.COPIES.ToString(),
                                                ColumnOrder=1,
                                                IncomingField=Enums.MAFFieldStandardFields.COPIES.ToString(),
                                                DataType=string.Empty,
                                                PreviewData=string.Empty,
                                                FieldMappingTypeID=5
                                            }
                                        }
                                    }
                                };
                            }
                        };
                    }
                };
            };
        }

        /// <summary>
        /// This method used to create setting file on run time.
        /// </summary>
        /// <param name="path">The BasePath.</param>
        /// <param name="content">The content.</param>
        private void CreateSettings(string path, string content)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            var file = new FileInfo(path);
            file.Directory.Create();
            File.WriteAllText(path, content, Encoding.ASCII);
        }

        private string CreateFileContent()
        {
            var content = new StringBuilder(string.Empty);
            content.AppendLine(string.Format("{0},  {1} , {2} , {3} , {4}., {5} , {6} , {7} , {8} ", "a", "b", "c", "d", "PubCode", "Qualification", "Test", "Test1", "QUALIFICATIONDATE"));
            content.AppendLine(string.Format("{0} , {1} , {2} , {3} , {4} , {5} , {6} , {7} , {8} ", "1", "2", "3", "4", "5", "6", "7", "8", "9918"));
            content.AppendLine(string.Format("{0} , {1} , {2} , {3} , {4} , {5} , {6} , {7} , {8} ", "1", "2", "3", "4", "5", "6", "7", "8", DateTime.Now.ToString()));
            content.AppendLine(string.Format("{0} , {1} , {2} , {3} , {4} , {5} , {6} , {7} , {8} ", "1", "2", "3", "4", "5", "6", "7", "8", DateTime.UtcNow.ToString()));
            content.AppendLine(string.Format("{0} , {1} , {2} , {3} , {4} , {5} , {6} , {7} , {8} ", "1", "2", "3", "4", "5", "6", "7", "8", "9"));
            return content.ToString();
        }

        private static SourceFile CreateSourceFile(bool isTextQualifier)
        {
            return new SourceFile
            {
                IsTextQualifier = isTextQualifier,
                IsBillable = true,
                Delimiter = ","
            };
        }
    }
}
