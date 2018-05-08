using System.Collections.Generic;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAS.BusinessLogic.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UAD.DataCompare.Web.Models;
using Shouldly;
using static FrameworkUAD_Lookup.Enums;
using UADEntities = FrameworkUAD_Lookup.Entity;
using UASEntities = FrameworkUAS.Entity;

namespace UAD.DataCompare.Web.Tests.Controllers.UAD
{
    public partial class DatacompareControllerTest
    {
        private const string SamplePreviewColumn = "SamplePreviewColumn";
        private const string SampleSourceColumn = "SampleSourceColumn";
        private const string MappedColumnTypeAdd = "Add";
        private const string MappedColumnTypeIgnore = "Ignore";
        private const string MappedColumnTypeKmTransform = "kmTransform";
        private const string MappedColumnTypeDelete = "Delete";
        private const string SaveFieldMappingNewMethodName = "SaveFieldMappingNew";
        private bool _isFieldMappingSaved;
        private bool _isFieldMappingDeleted;
        private UASEntities.FieldMapping _savedFieldMapping;
        private int _deletedFieldMappingID;

        [Test]
        public void SaveFieldMappingNew_WhenMappedColumnIsNotNull_FieldMappingIsSaved()
        {
            // Arrange
            const int sourceFileId = 1;
            var columnMapList = new List<ColumnMap>
            {
                new ColumnMap
                {
                    FieldMapID = 1,
                    SourceFileID = 1,
                    MappedColumn = MappedColumnTypeAdd,
                    SourceColumn = SampleSourceColumn,
                    PreviewDataColumn = SamplePreviewColumn
                },
                new ColumnMap
                {
                    FieldMapID = 2,
                    SourceFileID = 1,
                    MappedColumn = MappedColumnTypeIgnore,
                },
            };
            _controllerPrivate = new PrivateObject(_controller);
            SetFakesForSaveFieldMappingNew();

            // Act 
            var isResultSaved = _controllerPrivate.Invoke(SaveFieldMappingNewMethodName, columnMapList, sourceFileId);

            // Assert
            isResultSaved.ShouldSatisfyAllConditions(
                () => isResultSaved.ShouldBeOfType<bool>().ShouldBeTrue(),
                () => _isFieldMappingDeleted.ShouldBeFalse(),
                () => _deletedFieldMappingID.ShouldBe(0),
                () => _isFieldMappingSaved.ShouldBeTrue(),
                () => _savedFieldMapping.ShouldNotBeNull(),
                () => _savedFieldMapping.IncomingField.ShouldBe(SampleSourceColumn),
                () => _savedFieldMapping.MAFField.ShouldBe(MappedColumnTypeAdd),
                () => _savedFieldMapping.PubNumber.ShouldBe(0),
                () => _savedFieldMapping.FieldMappingID.ShouldBe(columnMapList[0].FieldMapID));

        }

        [Test]
        public void SaveFieldMappingNew_WhenMappedColumnDelete_FieldMappingIsDeletedAndSaved()
        {
            // Arrange
            const int sourceFileId = 1;
            var columnMapList = new List<ColumnMap>
            {
                new ColumnMap
                {
                    FieldMapID = 1,
                    SourceFileID = 1,
                    MappedColumn = MappedColumnTypeKmTransform,
                    SourceColumn = SampleSourceColumn,
                    PreviewDataColumn = new string('P',1001),
                },
                new ColumnMap
                {
                    FieldMapID = 2,
                    SourceFileID = 1,
                    MappedColumn = MappedColumnTypeDelete,
                    SourceColumn = SampleSourceColumn,
                },
            };
            _controllerPrivate = new PrivateObject(_controller);
            SetFakesForSaveFieldMappingNew();

            // Act 
            var isResultSaved = _controllerPrivate.Invoke(SaveFieldMappingNewMethodName, columnMapList, sourceFileId);

            // Assert
            isResultSaved.ShouldSatisfyAllConditions(
                () => isResultSaved.ShouldBeOfType<bool>().ShouldBeTrue(),
                () => _isFieldMappingDeleted.ShouldBeTrue(),
                () => _deletedFieldMappingID.ShouldBe(columnMapList[1].FieldMapID),
                () => _isFieldMappingSaved.ShouldBeTrue(),
                () => _savedFieldMapping.ShouldNotBeNull(),
                () => _savedFieldMapping.IncomingField.ShouldBe(SampleSourceColumn),
                () => _savedFieldMapping.MAFField.ShouldBe(MappedColumnTypeKmTransform),
                () => _savedFieldMapping.PubNumber.ShouldBe(0),
                () => _savedFieldMapping.FieldMappingID.ShouldBe(columnMapList[0].FieldMapID));
        }

        [Test]
        public void SaveFieldMappingNew_WhenMappedColumnIsDuplicated_NothingIsSaved()
        {
            // Arrange
            const int sourceFileId = 1;
            var columnMapList = new List<ColumnMap>
            {
                new ColumnMap
                {
                    FieldMapID = 1,
                    SourceFileID = 1,
                    MappedColumn = MappedColumnTypeKmTransform,
                    SourceColumn = SampleSourceColumn,
                    PreviewDataColumn = new string('P',1001),
                },
                new ColumnMap
                {
                    FieldMapID = 2,
                    SourceFileID = 1,
                    MappedColumn = MappedColumnTypeKmTransform,
                    SourceColumn = SampleSourceColumn,
                },
            };
            _controllerPrivate = new PrivateObject(_controller);
            SetFakesForSaveFieldMappingNew();

            // Act 
            var isResultSaved = _controllerPrivate.Invoke(SaveFieldMappingNewMethodName, columnMapList, sourceFileId);

            // Assert
            isResultSaved.ShouldBeOfType<bool>().ShouldBeFalse();

        }

        private void SetFakesForSaveFieldMappingNew()
        {
            _isFieldMappingSaved = false;
            _savedFieldMapping = null;
            _isFieldMappingDeleted = false;
            _deletedFieldMappingID = 0;
            InitializeSessionFakes();
            ShimCode.AllInstances.SelectEnumsCodeType = (c, e) => new List<UADEntities.Code>
            {
                new UADEntities.Code { CodeId = 1, CodeName = FieldMappingTypes.Ignored.ToString() },
                new UADEntities.Code { CodeId = 1, CodeName = FieldMappingTypes.kmTransform.ToString() },
                new UADEntities.Code { CodeId = 1, CodeName = FieldMappingTypes.Standard.ToString() },
                new UADEntities.Code { CodeId = 1, CodeName = FieldMappingTypes.Demographic.ToString() },
                new UADEntities.Code { CodeId = 1, CodeName = FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' ') },
            };

            ShimFieldMapping.AllInstances.SaveFieldMapping = (fm, fieldMapping) => 
            {
                _savedFieldMapping = fieldMapping;
                _isFieldMappingSaved = true;
                return _savedFieldMapping.SourceFileID;
            };
            ShimFieldMapping.AllInstances.DeleteMappingInt32 = (fm, fmId) =>
            {
                _deletedFieldMappingID = fmId;
                _isFieldMappingDeleted = true;
                return _deletedFieldMappingID;
            };
        }
    }
}
