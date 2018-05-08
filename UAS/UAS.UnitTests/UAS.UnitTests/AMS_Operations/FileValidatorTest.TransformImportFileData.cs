using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using AMS_Operations;
using Core.ADMS;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;

namespace UAS.UnitTests.AMS_Operations
{
    /// <summary>
    ///     Unit Tests for <see cref="FileValidator.TransformImportFileData"/>
    /// </summary>
    public partial class FileValidatorTest
    {
        [Test, TestCaseSource(nameof(GetSetupTransformationDataTableTestCases))]
        public void TransformImportFileData_SetupTransformationData_VerifyNewColumnsAreAdded(
            string mafField,
            string incomingField,
            string multiMafField,
            StringDictionary dataOriginal,
            StringDictionary dataTransformed,
            StringDictionary headerOriginals,
            StringDictionary headerTransformed)
        {
            // Arrange
            Initialize();
            SetupFieldMapping(mafField, incomingField, multiMafField);
            SetPrivateFields();
            var validateErrorMethodInfo = typeof(FileValidator).GetMethod("ValidationError", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var validationErrorFunc = Delegate.CreateDelegate(typeof(ValidationErrorDelegate), validator, validateErrorMethodInfo) as ValidationErrorDelegate;

            var logExceptionMethodInfo = typeof(FileValidator).GetMethod("LogException", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var logExceptionFunc = Delegate.CreateDelegate(typeof(LogExceptionDelegate), validator, logExceptionMethodInfo) as LogExceptionDelegate;

            var errorMessages = validator.GetFieldValue("ErrorMessages") as List<string>;
            var codeWorker = validator.GetFieldValue("codeWorker") as FrameworkUAD_Lookup.BusinessLogic.Code;

            dataIV = (ImportFile)validator.GetFieldValue(nameof(dataIV));

            var worker = new TransformationWorker(
                dataIV,
                validationErrorFunc,
                errorMessages,
                client,
                sourceFile,
                codeWorker,
                logExceptionFunc,
                serviceFeature,
                clientPubCodes);

            // Act
            worker.TransformImportFileData();
            dataIV = (ImportFile) validator.GetFieldValue(nameof(dataIV));

            // Assert
            CollectionAssert.AreEquivalent(dataOriginal, dataIV.DataOriginal[TransformDataMappingId]);
            CollectionAssert.AreEquivalent(dataTransformed, dataIV.DataTransformed[TransformDataMappingId]);
            CollectionAssert.AreEquivalent(headerOriginals, dataIV.HeadersOriginal);
            CollectionAssert.AreEquivalent(headerTransformed, dataIV.HeadersTransformed);
        }

        [Test]
        public void TransformImportFileData_AssignTransformIfNotHasPubID_VerifyNewRowsAreAdded()
        {
            // Arrange
            Initialize();
            SetupFieldMapping();
            string newRowValue = "new-value";

            SetupForAssignTransform(newRowValue);
            SetPrivateFields();

            var validateErrorMethodInfo = typeof(FileValidator).GetMethod("ValidationError", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var validationErrorFunc = Delegate.CreateDelegate(typeof(ValidationErrorDelegate), validator, validateErrorMethodInfo) as ValidationErrorDelegate;

            var logExceptionMethodInfo = typeof(FileValidator).GetMethod("LogException", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var logExceptionFunc = Delegate.CreateDelegate(typeof(LogExceptionDelegate), validator, logExceptionMethodInfo) as LogExceptionDelegate;

            var errorMessages = validator.GetFieldValue("ErrorMessages") as List<string>;
            var codeWorker = validator.GetFieldValue("codeWorker") as FrameworkUAD_Lookup.BusinessLogic.Code;

            dataIV = (ImportFile)validator.GetFieldValue(nameof(dataIV));

            var worker = new TransformationWorker(
                dataIV,
                validationErrorFunc,
                errorMessages,
                client,
                sourceFile,
                codeWorker,
                logExceptionFunc,
                serviceFeature,
                clientPubCodes);

            // Act
            worker.TransformImportFileData();
            dataIV = (ImportFile) validator.GetFieldValue(nameof(dataIV));

            // Assert
            dataIV.DataOriginal[TransformDataMappingId][IncomingField].ShouldBe(newRowValue);
            dataIV.DataTransformed[TransformDataMappingId][IncomingField].ShouldBe(newRowValue);
        }

        [Test, TestCaseSource(nameof(GetSplitTransformTestCases))]
        public void TransformImportFileData_SplitTranform_VerifyNewRowAreAdded(
            string delimiter,
            string matchType,
            string rowData,
            string desiredData,
            List<StringDictionary> headerValues)
        {
            // Arrange
            Initialize();
            SetupFieldMapping();
            SetupForSplitTransformation(delimiter, matchType, rowData, desiredData);
            SetPrivateFields();
            var validateErrorMethodInfo = typeof(FileValidator).GetMethod("ValidationError", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var validationErrorFunc = Delegate.CreateDelegate(typeof(ValidationErrorDelegate), validator, validateErrorMethodInfo) as ValidationErrorDelegate;

            var logExceptionMethodInfo = typeof(FileValidator).GetMethod("LogException", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var logExceptionFunc = Delegate.CreateDelegate(typeof(LogExceptionDelegate), validator, logExceptionMethodInfo) as LogExceptionDelegate;

            var errorMessages = validator.GetFieldValue("ErrorMessages") as List<string>;
            var codeWorker = validator.GetFieldValue("codeWorker") as FrameworkUAD_Lookup.BusinessLogic.Code;

            dataIV = (ImportFile)validator.GetFieldValue(nameof(dataIV));

            var worker = new TransformationWorker(
                dataIV,
                validationErrorFunc,
                errorMessages,
                client,
                sourceFile,
                codeWorker,
                logExceptionFunc,
                serviceFeature,
                clientPubCodes);

            // Act
            worker.TransformImportFileData();
            dataIV = (ImportFile) validator.GetFieldValue(nameof(dataIV));

            // Assert
            int index = 0;
            foreach (StringDictionary dictionary in dataIV.DataTransformed.Values)
            {
                CollectionAssert.AreEquivalent(headerValues[index], dictionary);
                index++;
            }
        }

        [Test, TestCaseSource(nameof(GetDataMapTransformTestCases))]
        public void TransformImportFileData_DataMapTransform_VerifyNewRowAreAdded(
            string matchType,
            string rowData,
            string desiredData,
            string expectedOutput)
        {
            // Arrange
            Initialize();
            SetupFieldMapping();
            SetupForDataMapTransformation(matchType, rowData, desiredData);
            SetPrivateFields();

            var validateErrorMethodInfo = typeof(FileValidator).GetMethod("ValidationError", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var validationErrorFunc = Delegate.CreateDelegate(typeof(ValidationErrorDelegate), validator, validateErrorMethodInfo) as ValidationErrorDelegate;

            var logExceptionMethodInfo = typeof(FileValidator).GetMethod("LogException", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var logExceptionFunc = Delegate.CreateDelegate(typeof(LogExceptionDelegate), validator, logExceptionMethodInfo) as LogExceptionDelegate;

            var errorMessages = validator.GetFieldValue("ErrorMessages") as List<string>;
            var codeWorker = validator.GetFieldValue("codeWorker") as FrameworkUAD_Lookup.BusinessLogic.Code;

            dataIV = (ImportFile)validator.GetFieldValue(nameof(dataIV));
            var worker = new TransformationWorker(
                dataIV,
                validationErrorFunc,
                errorMessages,
                client,
                sourceFile,
                codeWorker,
                logExceptionFunc,
                serviceFeature,
                clientPubCodes);

            // Act
            worker.TransformImportFileData();
            dataIV = (ImportFile) validator.GetFieldValue(nameof(dataIV));

            // Assert
            dataIV.DataTransformed[TransformDataMappingId][IncomingField].ShouldBe(expectedOutput);
        }

        [Test, TestCaseSource(nameof(GetTransformByPubCodeTestCases))]
        public void TransformImportFileData_TranformByPubCode_VerifyNewRowAreAdded(
            string delimiter,
            string matchType,
            string rowData,
            string desiredData,
            List<StringDictionary> headerValues)
        {
            // Arrange
            Initialize();
            SetupForTransformByPubCode(delimiter, matchType, rowData, desiredData);
            SetPrivateFields();
            var validateErrorMethodInfo = typeof(FileValidator).GetMethod("ValidationError", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var validationErrorFunc = Delegate.CreateDelegate(typeof(ValidationErrorDelegate), validator, validateErrorMethodInfo) as ValidationErrorDelegate;

            var logExceptionMethodInfo = typeof(FileValidator).GetMethod("LogException", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var logExceptionFunc = Delegate.CreateDelegate(typeof(LogExceptionDelegate), validator, logExceptionMethodInfo) as LogExceptionDelegate;

            var errorMessages = validator.GetFieldValue("ErrorMessages") as List<string>;
            var codeWorker = validator.GetFieldValue("codeWorker") as FrameworkUAD_Lookup.BusinessLogic.Code;

            dataIV = (ImportFile)validator.GetFieldValue(nameof(dataIV));

            var worker = new TransformationWorker(
                dataIV,
                validationErrorFunc,
                errorMessages,
                client,
                sourceFile,
                codeWorker,
                logExceptionFunc,
                serviceFeature,
                clientPubCodes);

            // Act
            worker.TransformImportFileData();
            dataIV = (ImportFile) validator.GetFieldValue(nameof(dataIV));

            // Assert
            int index = 0;
            foreach (StringDictionary dictionary in dataIV.DataTransformed.Values)
            {
                CollectionAssert.AreEquivalent(headerValues[index], dictionary);
                index++;
            }
        }

        [Test, TestCaseSource(nameof(GetJoinTransformTestCases))]
        public void TransformImportFileData_JoinTransform_VerifyNewRowAreAdded(
            string matchType,
            List<StringDictionary> headerValues)
        {
            // Arrange
            Initialize();
            SetupForJoinTransform(matchType);
            SetPrivateFields();
            var validateErrorMethodInfo = typeof(FileValidator).GetMethod("ValidationError", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var validationErrorFunc = Delegate.CreateDelegate(typeof(ValidationErrorDelegate), validator, validateErrorMethodInfo) as ValidationErrorDelegate;

            var logExceptionMethodInfo = typeof(FileValidator).GetMethod("LogException", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var logExceptionFunc = Delegate.CreateDelegate(typeof(LogExceptionDelegate), validator, logExceptionMethodInfo) as LogExceptionDelegate;

            var errorMessages = validator.GetFieldValue("ErrorMessages") as List<string>;
            var codeWorker = validator.GetFieldValue("codeWorker") as FrameworkUAD_Lookup.BusinessLogic.Code;

            dataIV = (ImportFile)validator.GetFieldValue(nameof(dataIV));

            var worker = new TransformationWorker(
                dataIV,
                validationErrorFunc,
                errorMessages,
                client,
                sourceFile,
                codeWorker,
                logExceptionFunc,
                serviceFeature,
                clientPubCodes);

            // Act
            worker.TransformImportFileData();
            dataIV = (ImportFile) validator.GetFieldValue(nameof(dataIV));

            // Assert
            int index = 0;
            foreach (StringDictionary dictionary in dataIV.DataTransformed.Values)
            {
                CollectionAssert.AreEquivalent(headerValues[index], dictionary);
                index++;
            }
        }
    }
}
