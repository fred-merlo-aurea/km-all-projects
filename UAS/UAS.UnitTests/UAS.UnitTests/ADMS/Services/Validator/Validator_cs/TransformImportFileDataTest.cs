using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrameworkUAD.Object;
using FrameworkUAS.Entity;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.ADMS.Services.Validator.Common;
using UAS.UnitTests.Helpers;
using ADMS_Validator = ADMS.Services.Validator.Validator;
using static UAS.UnitTests.ADMS.Services.Validator.Common.Constants;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    /// <summary>
    ///     Unit Tests for <see cref="ADMS_Validator"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class TransformImportFileDataTest
    {
        private TestEntity _testEntity;

        [SetUp]
        public void Setup()
        {
            _testEntity = new TestEntity();
            SetupFakes(_testEntity.Mocks);
            Initialize();
        }

        [Test, TestCaseSource(nameof(GetSetupTransformationDataTableTestCases))]
        public void TransformImportFileData_SetupTransformationDataTable_VerifyNewColumnsAreAdded(
            string mafField,
            string incomingField,
            string multiMafField,
            StringDictionary dataOriginal,
            StringDictionary headerOriginals,
            StringDictionary headerTransformed)
        {
            // Arrange
            SetupFieldMapping(mafField, incomingField, multiMafField);

            // Act
            importFile = (ImportFile) typeof(ADMS_Validator).CallMethod(
                TransformImportFileData,
                new object[] {importFile, feature, dbFileType, _testEntity.SourceFile, admsLog, dft},
                _testEntity.Validator);

            // Assert
            CollectionAssert.AreEquivalent(dataOriginal, importFile.DataOriginal[TransformDataMappingId]);
            CollectionAssert.AreEquivalent(dataOriginal, importFile.DataTransformed[TransformDataMappingId]);
            CollectionAssert.AreEquivalent(headerOriginals, importFile.HeadersOriginal);
            CollectionAssert.AreEquivalent(headerTransformed, importFile.HeadersTransformed);
        }

        [Test]
        public void TransformImportFileData_AssignTransformIfNotHasPubID_VerifyNewRowsAreAdded()
        {
            // Arrange
            SetupFieldMapping();
            string newRowValue = "new-value";

            SetupForAssignTransform(newRowValue);

            // Act
            importFile = (ImportFile) typeof(ADMS_Validator).CallMethod(
                TransformImportFileData,
                new object[] {importFile, feature, dbFileType, _testEntity.SourceFile, admsLog, dft},
                _testEntity.Validator);

            // Assert
            importFile.DataOriginal[TransformDataMappingId][IncomingField].ShouldBe(newRowValue);
            importFile.DataTransformed[TransformDataMappingId][IncomingField].ShouldBe(newRowValue);
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
            SetupFieldMapping();
            SetupForSplitTransformation(delimiter, matchType, rowData, desiredData);

            // Act
            importFile = (ImportFile) typeof(ADMS_Validator).CallMethod(
                TransformImportFileData,
                new object[] {importFile, feature, dbFileType, _testEntity.SourceFile, admsLog, dft},
                _testEntity.Validator);

            // Assert
            int index = 0;
            foreach (StringDictionary dictionary in importFile.DataTransformed.Values)
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
            SetupFieldMapping();
            SetupForDataMapTransformation(matchType, rowData, desiredData);

            // Act
            importFile = (ImportFile) typeof(ADMS_Validator).CallMethod(TransformImportFileData,
                new object[] {importFile, feature, dbFileType, _testEntity.SourceFile, admsLog, dft},
                _testEntity.Validator);

            // Assert
            importFile.DataTransformed[TransformDataMappingId][IncomingField].ShouldBe(expectedOutput);
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
            SetupForTransformByPubCode(delimiter, matchType, rowData, desiredData);

            // Act
            importFile = (ImportFile) typeof(ADMS_Validator).CallMethod(TransformImportFileData,
                new object[] {importFile, feature, dbFileType, _testEntity.SourceFile, admsLog, dft},
                _testEntity.Validator);

            // Assert
            int index = 0;
            foreach (StringDictionary dictionary in importFile.DataTransformed.Values)
            {
                CollectionAssert.AreEquivalent(headerValues[index], dictionary);
                index++;
            }
        }

        [Test, TestCaseSource(nameof(GetJoinTransformTestCases))]
        public void TransformImportFileData_JoinTranform_VerifyNewRowAreAdded(
            string matchType,
            List<StringDictionary> headerValues)
        {
            // Arrange
            SetupForJoinTransform(matchType);

            // Act
            importFile = (ImportFile) typeof(ADMS_Validator).CallMethod(TransformImportFileData,
                new object[] {importFile, feature, dbFileType, _testEntity.SourceFile, admsLog, dft},
                _testEntity.Validator);

            // Assert
            int index = 0;
            foreach (StringDictionary dictionary in importFile.DataTransformed.Values)
            {
                CollectionAssert.AreEquivalent(headerValues[index], dictionary);
                index++;
            }
        }

        [Test, TestCaseSource(nameof(GetJoinTransformTestCases))]
        public void TransformImportFileData_JoinMultipleTranforms_VerifyNewRowAreAdded(
            string matchType,
            List<StringDictionary> headerValues)
        {
            // Arrange
            SetupForJoinTransform(matchType);

            var transformation = allTransformations.Single(t => t.TransformationID == TransformSplitId);
            transformation.FieldMap.Add(new TransformationFieldMap()
            {
                SourceFileID = importFile.SourceFileId,
                FieldMappingID = 10
            });

            // Act
            importFile =  typeof(ADMS_Validator).CallMethod(TransformImportFileData,
                new object[] { importFile, feature, dbFileType, _testEntity.SourceFile, admsLog, dft },
                _testEntity.Validator) as ImportFile;

            // Assert
            importFile.ShouldNotBeNull();

            int index = 0;
            foreach (var dictionary in importFile.DataTransformed.Values)
            {
                CollectionAssert.AreEquivalent(headerValues[index % 2], dictionary);
                index++;
            }
        }
    }
}