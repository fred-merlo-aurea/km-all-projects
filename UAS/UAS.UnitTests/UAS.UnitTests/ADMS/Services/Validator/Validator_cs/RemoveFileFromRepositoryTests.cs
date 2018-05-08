using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Fakes;
using Core.ADMS;
using FrameworkUAD.Object;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;
using ADMS_Validator = ADMS.Services.Validator.Validator;
using ClientEntity = KMPlatform.Entity.Client;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RemoveFileFromRepositoryTests
    {
        private const string DefaultDateTimeFormat = "yyyyMMdd_HHmmss";
        private const string InvalidPath = "\\Invalid\\";
        private const string Underline = "_";

        private PrivateObject _validatorPrivateObject;
        private AdmsLog _admsLog;
        private ClientEntity _clientEntity;
        private ImportFile _importFile;
        private IDisposable _context;

        [SetUp]
        public void Initialize()
        {
            _context = ShimsContext.Create();

            _validatorPrivateObject = new PrivateObject(typeof(ADMS_Validator));
            _admsLog = new AdmsLog();
            _clientEntity = new ClientEntity();
            _importFile = new ImportFile();
        }

        [TearDown]
        public void DisposeContext()
        {
            _context.Dispose();
        }

        [Test]
        public void RemoveFileFromRepository_WhenDataIVIsNull_ThrowsException()
        {
            // Arrange
            _importFile = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(RemoveFileFromRepositoryMethod, _importFile, _admsLog, _clientEntity));
        }

        [Test]
        public void RemoveFileFromRepository_WhenAdmsLogIsNull_ThrowsException()
        {
            // Arrange
            _admsLog = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(RemoveFileFromRepositoryMethod, _importFile, _admsLog, _clientEntity));
        }

        [Test]
        public void RemoveFileFromRepository_WhenClientEntityIsNull_ThrowsException()
        {
            // Arrange
            _clientEntity = null;

            // Act, Assert
            Should.Throw<ArgumentNullException>(() =>
                _validatorPrivateObject.Invoke(RemoveFileFromRepositoryMethod, _importFile, _admsLog, _clientEntity));
        }

        [Test]
        public void RemoveFileFromRepository_WhenFileExists_ShouldMoveFile()
        {
            // Arrange
            _importFile = CreateImportFileForTest();

            var resultOldPath = string.Empty;
            var resultNewPath = CreateFilePathForNewFile(_importFile, _clientEntity);

            ShimFile.ExistsString = _ => true;
            ShimFile.MoveStringString = (oldPath, newPath) =>
            {
                resultOldPath = oldPath;
                resultNewPath = newPath;
            };
            ShimAdmsLog.AllInstances.UpdateFileEndStringDateTimeInt32Int32 = (_, processCode, fileEnd, userId, sourceId) => true;

            // Act
            _validatorPrivateObject.Invoke(RemoveFileFromRepositoryMethod, _importFile, _admsLog, _clientEntity);

            // Assert
            resultOldPath.ShouldSatisfyAllConditions(
                () => resultOldPath.ShouldBe(_importFile.ProcessFile.FullName),
                () => resultNewPath.ShouldBe(resultNewPath));
        }

        private static ImportFile CreateImportFileForTest()
        {
            return new ImportFile
            {
                ProcessFile = new FileInfo(@"J:\Sample\Sample.txt")
            };
        }

        private static string CreateFilePathForNewFile(ImportFile importFile, ClientEntity clientEntity)
        {
            var clientRepo = $"{BaseDirs.createDirectory(BaseDirs.getClientArchiveDir(), clientEntity.FtpFolder)}{InvalidPath}";
            var dateForFile = DateTime.Now.ToString(DefaultDateTimeFormat);
            var replacedName = importFile.ProcessFile.Name.Replace(importFile.ProcessFile.Extension, $"{Underline}{dateForFile}");

            return $"{clientRepo}{replacedName}{importFile.ProcessFile.Extension}";
        }
    }
}
