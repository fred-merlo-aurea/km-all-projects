using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.IO;
using Shouldly;
using UAS.UnitTests.Helpers;
using FrameworkUAS.Entity;
using FrameworkUAD.Object;
using System.Threading;
using DQM.Helpers.Validation;
using NUnit.Framework;
using KMPlatform.BusinessLogic.Fakes;
using UAS.UnitTests.DQM.Helpers.Validation.Common;

namespace UAS.UnitTests.DQM.Helpers.Validation.FileValidator_cs
{
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class GetReportBodyForSingleEmailTest : Fakes
    {
        private FileValidator _fileValidator;
        private static string Path = AppDomain.CurrentDomain.BaseDirectory;
        private const string FileName = "GetReportBodyForSingleEmail.csv";
        private const string CheckFile = "checkFile";
        private const string SourceFile = "sourceFile";
        private const string ProcessCode = "processCode";
        private const string ErrorMessage = "An unrecognized error has occurred";
        private const string SuccessMessage = "Import details for file";

        [SetUp]
        public void Setup()
        {
            _fileValidator = new FileValidator();
            SetupFakes();
        }

        [Test]
        public void GetReportBodyForSingleEmail_FileInfoIsNotNull_ReturnsObjectValue()
        {
            // Arrange
            var filePath = System.IO.Path.Combine(Path, FileName);
            var content = string.Empty;
            CreateSettings(filePath, content);
            var myCheckFile = new FileInfo(filePath);

            var isKnownCustomerFileName = false;
            var isValidFileType = false;
            var isFileSchemaValid = false;
            var validationResult = CreateValidationResult(myCheckFile);
            var mySourceFile = CreateSourceFile(true);
            ReflectionHelper.SetField(_fileValidator, ProcessCode, Guid.NewGuid().ToString());
            ReflectionHelper.SetField(_fileValidator, CheckFile, myCheckFile);
            ReflectionHelper.SetField(_fileValidator, SourceFile, mySourceFile);

            // Act
            var result = _fileValidator.GetReportBodyForSingleEmail(
                isKnownCustomerFileName,
                isValidFileType,
                isFileSchemaValid,
                validationResult);

            // Assert
            result.ShouldNotBeNull();
            result.ToString().Contains(SuccessMessage).ShouldBeTrue();
        }

        [Test]
        public void GetReportBodyForSingleEmail_ImportFileIsNull_ReturnsObjectValue()
        {
            // Arrange
            var filePath = System.IO.Path.Combine(Path, FileName);
            var content = string.Empty;
            CreateSettings(filePath, content);
            var myCheckFile = new FileInfo(filePath);
            var isKnownCustomerFileName = false;
            var isValidFileType = false;
            var isFileSchemaValid = false;
            var validationResult = new ValidationResult
            {
                ImportFile = null
            };
            ReflectionHelper.SetField(_fileValidator, CheckFile, myCheckFile);
            ReflectionHelper.SetField(_fileValidator, ProcessCode, Guid.NewGuid().ToString());

            // Act
            var result = _fileValidator.GetReportBodyForSingleEmail(
                isKnownCustomerFileName,
                isValidFileType,
                isFileSchemaValid,
                validationResult);

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public void GetReportBodyForSingleEmail_ProcessCodeIsNull_ReturnsObjectValue()
        {
            // Arrange
            var filePath = System.IO.Path.Combine(Path, FileName);
            var content = string.Empty;
            CreateSettings(filePath, content);
            var myCheckFile = new FileInfo(filePath);
            var isKnownCustomerFileName = false;
            var isValidFileType = false;
            var isFileSchemaValid = false;
            var validationResult = CreateValidationResult(myCheckFile);
            var mySourceFile = CreateSourceFile(true);
            ReflectionHelper.SetField(_fileValidator, CheckFile, myCheckFile);
            ReflectionHelper.SetField(_fileValidator, SourceFile, mySourceFile);
            var logLogCriticalErrorString = false;
            ShimApplicationLog.AllInstances.LogCriticalErrorStringStringEnumsApplicationsStringInt32String =
            (sender, ex, sourceMethod, application, note, clientId, subject) =>
            {
                logLogCriticalErrorString = true;
                return 1;
            };

            // Act
            var result = _fileValidator.GetReportBodyForSingleEmail(
                isKnownCustomerFileName,
                isValidFileType,
                isFileSchemaValid,
                validationResult);

            // Assert
            logLogCriticalErrorString.ShouldBeTrue();
            result.ShouldNotBeNull();
        }

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

        private SourceFile CreateSourceFile(bool isTextQualifier)
        {
            return new SourceFile
            {
                IsTextQualifier = isTextQualifier,
                IsBillable = true,
                Delimiter = ",",
                SourceFileID = 1
            };
        }

        private ValidationResult CreateValidationResult(FileInfo myCheckFile)
        {
            return new ValidationResult
            {
                UnexpectedColumns = new HashSet<string> { "UnexpectedColumns" },
                NotFoundColumns = new HashSet<string> { "NotFoundColumns" },
                DuplicateColumns = new HashSet<string> { "DuplicateColumns" },
                ImportFile = myCheckFile,
                DimensionImportErrorCount = 12,
                TotalRowCount = 10,
                OriginalRowCount = 10,
                TransformedRowCount = 10,
                ImportedRowCount = 10,
                HasError = true,
                RecordImportErrorCount = 10
            };
        }
    }
}
