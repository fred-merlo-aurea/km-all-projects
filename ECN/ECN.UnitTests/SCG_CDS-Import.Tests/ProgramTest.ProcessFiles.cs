using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Fakes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace SCG_CDS_Import.Tests
{
    public partial class ProgramTest
    {
        private const int ImportFileGroupId = 10;
        private const string ImportFilesFieldName = "importFiles";
        private const string FilesExtension = "*.txt";
        private const string FileName = "FileName";
        private const string FakePath = "Dir/SubDir/" + FileName;
        private const string XlsExtension = ".xls";
        private const string XlsFilePath = FakePath + XlsExtension;
        private const string XlxsExtension = ".xlxs";
        private const string XlxsFilePath = FakePath + XlxsExtension;
        private const string XmlExtension = ".xml";
        private const string XmlFilePath = FakePath + XmlExtension;
        private const string OtherFileExtension = ".SomeDummyExtension";
        private const string OtherFilePath = FakePath + OtherFileExtension;

        [Test]
        public void ProcessFiles_WorkAsExpected()
        {
            //Arrange
            var filePath = GetUniqueString();
            var logPath = GetUniqueString();
            var fileArchive = GetUniqueString();
            _fakedAppSettings.Add("FilePath", filePath);
            _fakedAppSettings.Add("LogPath", logPath);
            _fakedAppSettings.Add("FileArchive", fileArchive);
            var expectedLogPath = $"SCG_CDS_Import_{FileName}_{_now.ToString("MM-dd-yyyy")}.log";
            var passedFilePath = false;
            var getGroupDataFieldsCalled = false;
            var passedLogPath = false;
            var fileMoveCallTimes = 0;
            var filesNames = new Dictionary<FileSystemInfo, string>();
            ShimDirectoryInfo.ConstructorString = (directoryInfo, path) =>
            {
                passedFilePath = path == filePath;
            };
            ShimFileInfo.ConstructorString = (fileInfo, path) =>
            {
                filesNames[fileInfo] = path;
            };
            ShimFileInfo.AllInstances.NameGet = fileInfo => Path.GetFileName(filesNames[fileInfo]);
            ShimFileSystemInfo.AllInstances.ExtensionGet = fileInfo => Path.GetExtension(filesNames[fileInfo]);
            ShimFileSystemInfo.AllInstances.FullNameGet = fileInfo => filesNames[fileInfo];
            ShimDirectoryInfo.AllInstances.GetFilesString = (directoryInfo, searchPattern) =>
            {
                if (searchPattern == FilesExtension)
                {
                    return new FileInfo[]
                    {
                        new FileInfo(XlsFilePath),
                        new FileInfo(XlxsFilePath),
                        new FileInfo(XmlFilePath),
                        new FileInfo(OtherFilePath)
                    };
                }
                else
                {
                    return new FileInfo[0];
                }
            };
            ShimFile.MoveStringString = (oldPath, newPath) =>
            {
                ++fileMoveCallTimes;
            };
            ShimFileStream.ConstructorStringFileMode = (fileStream, path, fileMode) =>
            {
                passedLogPath = expectedLogPath == path;
            };

            ShimStreamWriter.ConstructorStream = (streamWriter, stream) => { };
            var importFiles = new ImportFiles
            {
                Files = new List<ImportFile>
                {
                    new ImportFile
                    {
                        FileName = FileName,
                        GroupID = ImportFileGroupId
                    }
                }
            };
            Fakes.ShimProgram.GetGroupDataFieldsInt32 = groupId =>
            {
                getGroupDataFieldsCalled = groupId == ImportFileGroupId;
            };
            Fakes.ShimProgram.ProcessFlatFileFileInfoImportFile = (fileInfo, importFile) => { };
            SetField(ImportFilesFieldName, importFiles);
            var date = $"_{_now.Month}-{_now.Day}-{_now.Year}";
            var expectedLog = $"Moved file to {fileArchive}{FileName}{date}.txt ";

            //Act
            CallProcessFiles();

            //Assert
            passedFilePath.ShouldBeTrue();
            getGroupDataFieldsCalled.ShouldBeTrue();
            fileMoveCallTimes.ShouldBe(filesNames.Count);
            _programTestContext.Logs.ShouldContain(log => log.Contains($"Starting file : {FileName}{XlsExtension} "));
            _programTestContext.Logs.ShouldContain(log => log.Contains($"Starting file : {FileName}{XlxsExtension} "));
            _programTestContext.Logs.ShouldContain(log => log.Contains($"Starting file : {FileName}{XmlExtension} "));
            _programTestContext.Logs
                .ShouldContain(log => log.Contains($"Starting file : {FileName}{OtherFileExtension} "));
            _programTestContext.Logs.ShouldContain(expectedLog);
            _programTestContext.ImportFileLogs.ShouldContain(expectedLog);
        }

        [Test]
        public void ProcessFiles_ImportFileNull_NotifyAdminAndWiteToImportFileLog()
        {
            //Arrange

            var filePath = GetUniqueString();
            var logPath = GetUniqueString();
            var fileArchive = GetUniqueString();
            _fakedAppSettings.Add("FilePath", filePath);
            _fakedAppSettings.Add("LogPath", logPath);
            _fakedAppSettings.Add("FileArchive", fileArchive);
            var filesNames = new Dictionary<FileSystemInfo, string>();
            ShimDirectoryInfo.ConstructorString = (directoryInfo, path) => { };
            ShimFileInfo.ConstructorString = (fileInfo, path) =>
            {
                filesNames[fileInfo] = path;
            };
            ShimFileInfo.AllInstances.NameGet = fileInfo => Path.GetFileName(filesNames[fileInfo]);
            ShimFileSystemInfo.AllInstances.ExtensionGet = fileInfo => Path.GetExtension(filesNames[fileInfo]);
            ShimFileSystemInfo.AllInstances.FullNameGet = fileInfo => filesNames[fileInfo];
            ShimDirectoryInfo.AllInstances.GetFilesString = (directoryInfo, searchPattern) =>
            {
                if (searchPattern == FilesExtension)
                {
                    return new FileInfo[]
                    {
                        new FileInfo(XlsFilePath),
                    };
                }
                else
                {
                    return new FileInfo[0];
                }
            };
            ShimFile.MoveStringString = (oldPath, newPath) => { };
            ShimFileStream.ConstructorStringFileMode = (fileStream, path, fileMode) => { };

            ShimStreamWriter.ConstructorStream = (streamWriter, stream) => { };
            var importFiles = new ImportFiles
            {
                Files = new List<ImportFile>()
            };
            SetField(ImportFilesFieldName, importFiles);
            Fakes.ShimProgram.GetGroupDataFieldsInt32 = groupId => { };
            Fakes.ShimProgram.ProcessFlatFileFileInfoImportFile = (fileInfo, importFile) => { };

            //Act
            CallProcessFiles();

            //Assert
            var expectedNotifyAdminSubject = "SCG_CDS-Import - ProcessFiles";
            var expectedNotifyAdminMessge = $"{_now} - No matching import file name in xml mapping file for " +
                $"{FileName}{XlsExtension}";
            var expectedImportLog = $"{_now} - SCG_CDS-Import - ProcessFiles : No matching import file name in xml " +
                $"mapping file for {FileName}{XlsExtension}";
            _programTestContext.EmailFunctionsNotifyAdminSubject.ShouldBe(expectedNotifyAdminSubject);
            _programTestContext.EmailFunctionsNotifyAdminTextMessage.ShouldBe(expectedNotifyAdminMessge);
            _programTestContext.ImportFileLogs.ShouldContain(expectedImportLog);
        }

        [Test]
        public void ProcessFiles_ExceptionThrown_LogException()
        {
            //Arrange
            var filePath = GetUniqueString();
            var logPath = GetUniqueString();
            var fileArchive = GetUniqueString();
            _fakedAppSettings.Add("FilePath", filePath);
            _fakedAppSettings.Add("LogPath", logPath);
            _fakedAppSettings.Add("FileArchive", fileArchive);
            var filesNames = new Dictionary<FileSystemInfo, string>();
            ShimDirectoryInfo.ConstructorString = (directoryInfo, path) => { };
            ShimFileInfo.ConstructorString = (fileInfo, path) =>
            {
                filesNames[fileInfo] = path;
            };
            ShimFileInfo.AllInstances.NameGet = fileInfo => Path.GetFileName(filesNames[fileInfo]);
            ShimFileSystemInfo.AllInstances.ExtensionGet = fileInfo => Path.GetExtension(filesNames[fileInfo]);
            ShimFileSystemInfo.AllInstances.FullNameGet = fileInfo => filesNames[fileInfo];
            ShimDirectoryInfo.AllInstances.GetFilesString = (directoryInfo, searchPattern) =>
            {
                return new FileInfo[]
                {
                        new FileInfo(XlsFilePath),
                };
            };
            ShimFileStream.ConstructorStringFileMode = (fileStream, path, fileMode) => { };
            var exceptionMessage = GetUniqueString();
            var innerExceptionMessage = GetUniqueString();
            var innerException = new Exception(innerExceptionMessage);
            var exception = new Exception(exceptionMessage, innerException);
            ShimFile.MoveStringString = (oldPath, newPath) => { };

            ShimStreamWriter.ConstructorStream = (streamWriter, stream) =>
            {
                throw exception;
            };
            var importFiles = new ImportFiles
            {
                Files = new List<ImportFile>()
            };
            SetField(ImportFilesFieldName, importFiles);
            var expectedNotifyAdminSubject = "SCG_CDS-Import";
            var expectedSourceMethod = "SCG_CDS-Import.Program.ProcessFiles";
            var expecteNote = "SCG_CDS-Import: Unhandled Exception";
            var expectedLog = new StringBuilder();
            expectedLog.AppendLine("**********************");
            expectedLog.AppendLine($"Exception - {_now}");
            expectedLog.AppendLine("-- Message --");
            expectedLog.AppendLine(exceptionMessage);
            expectedLog.AppendLine("-- InnerException --");

            //Act
            CallProcessFiles();

            //Assert
            _programTestContext.EmailFunctionsNotifyAdminSubject.ShouldBe(expectedNotifyAdminSubject);
            _programTestContext.EmailFunctionsNotifyAdminTextMessage.ShouldContain(expectedLog.ToString());
            _programTestContext.LogCriticalErrorSourceMethod.ShouldBe(expectedSourceMethod);
            _programTestContext.LogCriticalErrorNote.ShouldBe(expecteNote);
            _programTestContext.LogCriticalErrorException.ShouldBe(exception);
            _programTestContext.Logs.ShouldContain(log => log.Contains(expectedLog.ToString()));
        }

        private void CallProcessFiles()
        {
            const string processFilesMethodName = "ProcessFiles";
            var caller = new PrivateObject(typeof(Program));
            caller.Invoke(processFilesMethodName, BindingFlags.NonPublic | BindingFlags.Static);
        }
    }
}
