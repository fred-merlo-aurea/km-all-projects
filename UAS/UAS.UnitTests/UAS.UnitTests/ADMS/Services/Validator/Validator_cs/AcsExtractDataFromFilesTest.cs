using System;
using System.IO;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using Core.ADMS.Events;
using Core.ADMS.Fakes;
using KMPlatform.BusinessLogic.Fakes;
using System.Linq;
using Core_AMS.Utilities.Fakes;
using System.Collections.Generic;
using ADMS.Services.Fakes;
using FrameworkUAD.BusinessLogic.Fakes;
using Shouldly;
using SourceFile = FrameworkUAS.Entity.SourceFile;
using AdmsLog = FrameworkUAS.Entity.AdmsLog;
using KM.Common.Utilities.Email.Fakes;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    /// Unit test for <see cref="ADMS_Validator"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class AcsExtractDataFromFilesTest : Fakes
    {
        private TestEntity testEntity;
        private const string acsExtractDataFromFiles = "ACS_ExtractDataFromFiles";
        private const string sampleFolderPath = "FtpFolder\\sample\\";
        private const string ftpFolderPath = "FtpFolder\\sample.txt";
        private const string sampleCbFileName = "sampleCB.txt";
        private const string sampleSnFileName = "sampleSN.txt";
        private const string sampleSdFileName = "sampleSD.txt";
        private const string sampleFileName = "sample1.txt";
        private const string ftpFolder = "FtpFolder";
        private const string sampleFolderExtendedPath = "FtpFolder\\sample\\sample1\\";
        private const string ftpFolderExtendedPath = "FtpFolder\\sample\\sample1.txt";
        private const string ftpFolderExtended = "FtpFolder\\sample";
        private static string path = AppDomain.CurrentDomain.BaseDirectory;
        private bool userExist = false;
        private bool saveAcsShippingDetailClient = false;
        private bool sendMail = false;
        private List<string> emailToList = new List<string>();

        [SetUp]
        public void Setup()
        {
            testEntity = new TestEntity();
            SetupFakes(testEntity.Mocks);
            ShimBaseDirs.GetFulfillmentZipDir = () =>
            {
                return path;
            };
            ShimServiceBase.AllInstances.clientGet = (x) =>
            {
                return new KMPlatform.Entity.Client();
            };
            ShimUser.AllInstances.SelectInt32StringBoolean =
                (sender, clientID, securityGroupName, includeObjects) =>
                {
                    userExist = true;
                    var user = new KMPlatform.Entity.User
                    {
                        FullName = "Unit Test"
                    };
                    return Enumerable.Repeat(user, 10).ToList();
                };
            ShimAcsShippingDetail.AllInstances.SaveAcsShippingDetailClientConnections = (sender, acsShippingDetail, client) =>
            {
                saveAcsShippingDetailClient = true;
                return 1;
            };
            ShimEmailService.AllInstances.SendEmailEmailMessageString = (instance, message, mailServer) => 
            {
                sendMail = true;
                emailToList = message.To.ToList();
            };
        }

        [Test]
        public void AcsExtractDataFromFiles_FileWithDefaultData_SaveDetailtoAcsFileHeaderAndAcsFileDetail()
        {
            // Arrange
            var filePath = Path.Combine(path, sampleFolderExtendedPath);
            var parameters = CreateParameter(sampleFileName, true, false, true);

            // Act
            ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                acsExtractDataFromFiles,
                parameters,
                testEntity.Validator);

            // Assert
            File.Exists(string.Concat(filePath, sampleFileName)).ShouldBeFalse();
        }

        [Test]
        public void AcsExtractDataFromFiles_FileEndWithCb_DeleteFileFromPhysicalLocationAndDoNothing()
        {
            // Arrange
            var filePath = Path.Combine(path, sampleFolderPath);
            var parameters = CreateParameter(sampleCbFileName, false, false, true);

            // Act
            ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                acsExtractDataFromFiles,
                parameters,
                testEntity.Validator);

            // Assert
            File.Exists(string.Concat(filePath, sampleCbFileName)).ShouldBeFalse();
        }


        [Test]
        public void AcsExtractDataFromFiles_FileEndWithSn_SendMailToUsers()
        {
            // Arrange
            var filePath = Path.Combine(path, sampleFolderPath);
            var parameters = CreateParameter(sampleSnFileName, false, false);

            // Act
            ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                acsExtractDataFromFiles,
                parameters,
                testEntity.Validator);

            // Assert
            userExist.ShouldBeTrue();
            sendMail.ShouldBeTrue();
            emailToList.Any().ShouldBeTrue();
            emailToList.Count.ShouldBe(4);
        }

        [Test]
        public void AcsExtractDataFromFiles_FileEndWithSd_SaveDetailtoAcsShippingDetail()
        {
            // Arrange
            var filePath = Path.Combine(path, sampleFolderPath);
            var parameters = CreateParameter(sampleSdFileName, true, true);

            // Act
            ReflectionHelper.CallMethod(
                testEntity.Validator.GetType(),
                acsExtractDataFromFiles,
                parameters,
                testEntity.Validator);

            // Assert
            saveAcsShippingDetailClient.ShouldBeTrue();
            File.Exists(string.Concat(filePath, sampleCbFileName)).ShouldBeFalse();
        }

        /// <summary>
        /// This method used to create setting file on run time.
        /// </summary>
        /// <param name="path">The BasePath.</param>
        /// <param name="content">The content.</param>
        private void CreateSettings(string path, string content, string fileName, bool isExtendedPath = false)
        {
            var filePath = string.Empty;
            if (isExtendedPath)
            {
                filePath = Path.Combine(path, string.Concat(sampleFolderExtendedPath, fileName));
            }
            else
            {
                filePath = Path.Combine(path, string.Concat(sampleFolderPath, fileName));
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var file = new FileInfo(filePath);
            file.Directory.Create();
            File.WriteAllText(filePath, content, Encoding.ASCII);
        }

        private string CreateErrorString(bool includeHeader = true)
        {
            var content = new StringBuilder(string.Empty);
            if (includeHeader)
            {
                content.AppendLine(string.Format("{0},  {1} , {2} , {3} , {4}., {5} , {6} , {7} , {8} , {9} , {10} , {11}",
                    "Detail", "CustomerNumber", "AcsDate", "ShipmentNumber", "MailerId", "Title", "ProductCode", "Description", "Quantity", "UnitCost", "TotalCost", "Error"));
            }
            content.AppendLine(string.Format("{0},  {1} , {2} , {3} , {4}., {5} , {6} , {7} , {8} , {9} , {10}",
                "DetailD", "1", DateTime.Now.ToString(), "4448899", "555", "Unit test", "7342345", "Unit test", "12", "22.00", "100.00"));
            content.AppendLine(string.Format("{0},  {1} , {2} , {3} , {4}., {5} , {6} , {7} , {8} , {9} , {10}",
                "DetailD", "1", DateTime.Now.ToString(), "4448899", "555", "Unit test", "7342345", "Unit test", "12", "22.00", "100.00"));
            content.AppendLine(string.Format("{0},  {1} , {2} , {3} , {4}., {5} , {6} , {7} , {8} , {9} , {10}",
                "DetailD", "1", DateTime.Now.ToString(), "4448899", "555", "Unit test", "7342345", "Unit test", "12", "22.00", "100.00"));
            content.AppendLine(string.Format("{0},  {1} , {2} , {3} , {4}., {5} , {6} , {7} , {8} , {9} , {10}",
            "DetailD", "1", DateTime.Now.ToString(), "4448899", "555", "Unit test", "7342345", "Unit test", "12", "22.00", "100.00"));
            content.AppendLine(string.Format("{0},  {1} , {2} , {3} , {4}., {5} , {6} , {7} , {8} , {9} , {10}",
           "D", "1", DateTime.Now.ToString(), "4448899", "555", "Unit test", "7342345", "Unit test", "12", "22.00", "100.00"));
            return content.ToString();
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

        private SourceFile CreateSourceFile(bool isTextQualifier)
        {
            return new SourceFile
            {
                IsTextQualifier = isTextQualifier,
                IsBillable = true,
                Delimiter = ",",
                FileName = "sample"
            };
        }

        private object[] CreateParameter(string fileName, bool isErrorContent, bool includeHeaderError, bool isExtendedPath = false)
        {
            var content = string.Empty;
            if (isErrorContent)
            {
                content = CreateErrorString(includeHeaderError);
            }
            else
            {
                content = CreateFileContent();
            }
            CreateSettings(path, content, fileName, isExtendedPath);
            var importFilePath = string.Empty;
            if (isExtendedPath)
            {
                importFilePath = Path.Combine(path, ftpFolderExtendedPath);
            }
            else
            {
                importFilePath = Path.Combine(path, ftpFolderPath);
            }
            var myCheckFile = new FileInfo(importFilePath);
            var myClient = new KMPlatform.Entity.Client
            {
                FtpFolder = isExtendedPath ? ftpFolderExtended : ftpFolder
            };
            var mySourceFile = CreateSourceFile(true);
            var admsLog = new AdmsLog();
            var isKnownCustomerFileName = true;
            var threadId = 1110;
            var eventMessage = new FileMoved(
                myCheckFile,
                myClient,
                mySourceFile,
                admsLog,
                isKnownCustomerFileName,
                threadId);
            var parameters = new object[] { eventMessage };
            return parameters;
        }
    }
}