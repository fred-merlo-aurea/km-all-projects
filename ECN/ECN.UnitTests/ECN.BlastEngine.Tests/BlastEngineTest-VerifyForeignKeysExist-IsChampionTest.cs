using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Reflection;
using System.Data;
using System.Collections.Generic;
using ecn.blastengine;
using ECN.TestHelpers;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ecn.common.classes.Fakes;
using ecn.blastengine.Fakes;
using ECN_Framework_Common.Objects;
using NUnit.Framework;
using StringAssert = Microsoft.VisualStudio.TestTools.UnitTesting.StringAssert;

namespace ECN.BlastEngine.Tests
{
    public partial class BlastEngineTest
    {
        [Test]
        public void VerifyForeignKeysExist_IsChampionBlastIsNull_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "Blast ID:";
            var firsTimeCall = true;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                if (firsTimeCall)
                {
                    var blast = new BlastRegular();
                    blast.BlastID = blastId;
                    blast.GroupID = blastGroupId;
                    blast.BlastType = BlastType.Champion.ToString();
                    blast.LayoutID = 1;
                    firsTimeCall = false;
                    return blast;
                }
                else
                {
                    return null;
                }
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) =>
            {
                var group = new Group();
                return group;
            };
            ShimDataFunctions.GetDataTableSqlCommand = (cmd) =>
            {
                return new DataTable();
            };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var layout = new Layout();
                layout.ContentSlot1 = 0;
                layout.TemplateID = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                return "ContentExist";
            };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (id) =>
            {
                var template = new Template
                {
                    TemplateSource = "TemplateSource",
                    TemplateText = "TemplateText"
                };
                return template;
            };
            ShimGroup.ValidateDynamicStringsForTemplateListOfStringInt32User = (list, groupId, user) => { };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_IsChampionSampleIDExist_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "Blast ID:";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.Champion.ToString();
                blast.LayoutID = 1;
                blast.SampleID = 1;
                return blast;
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) =>
            {
                var group = new Group();
                return group;
            };
            ShimDataFunctions.GetDataTableSqlCommand = (cmd) =>
            {
                return new DataTable();
            };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var layout = new Layout();
                layout.ContentSlot1 = 0;
                layout.TemplateID = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                return "ContentExist";
            };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (id) =>
            {
                var template = new Template
                {
                    TemplateSource = "TemplateSource",
                    TemplateText = "TemplateText"
                };
                return template;
            };
            ShimGroup.ValidateDynamicStringsForTemplateListOfStringInt32User = (list, groupId, user) => { };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_IsChampionSampleIDExistLayoutNull_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "Layout ID:";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.Champion.ToString();
                blast.LayoutID = 1;
                blast.SampleID = 1;
                return blast;
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) =>
            {
                var group = new Group();
                return group;
            };
            ShimDataFunctions.GetDataTableSqlCommand = (cmd) =>
            {
                var table = new DataTable();
                table.Columns.Add("LayoutID", typeof(string));
                var row = table.NewRow();
                row[0] = "1";
                table.Rows.Add(row);
                var row2 = table.NewRow();
                row2[0] = "1";
                table.Rows.Add(row2);
                return table;
            };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return null;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                return "ContentExist";
            };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (id) =>
            {
                var template = new Template
                {
                    TemplateSource = "TemplateSource",
                    TemplateText = "TemplateText"
                };
                return template;
            };
            ShimGroup.ValidateDynamicStringsForTemplateListOfStringInt32User = (list, groupId, user) => { };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_IsChampionSampleIDExistLayoutExist_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = string.Empty;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.Champion.ToString();
                blast.LayoutID = 1;
                blast.SampleID = 1;
                return blast;
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) =>
            {
                var group = new Group();
                return group;
            };
            ShimDataFunctions.GetDataTableSqlCommand = (cmd) =>
            {
                var table = new DataTable();
                table.Columns.Add("LayoutID", typeof(string));
                var row = table.NewRow();
                row[0] = "1";
                table.Rows.Add(row);
                var row2 = table.NewRow();
                row2[0] = "1";
                table.Rows.Add(row2);
                return table;
            };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var layout = new Layout();
                layout.ContentSlot1 = 0;
                layout.TemplateID = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                return "ContentExist";
            };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (id) =>
            {
                var template = new Template
                {
                    TemplateSource = "TemplateSource",
                    TemplateText = "TemplateText"
                };
                return template;
            };
            ShimGroup.ValidateDynamicStringsForTemplateListOfStringInt32User = (list, groupId, user) => { };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_IsChampionSampleIDExistLayoutExistWithException_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "UDF(s): ";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.Champion.ToString();
                blast.LayoutID = 1;
                blast.SampleID = 1;
                return blast;
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) =>
            {
                var group = new Group();
                return group;
            };
            ShimDataFunctions.GetDataTableSqlCommand = (cmd) =>
            {
                var table = new DataTable();
                table.Columns.Add("LayoutID", typeof(string));
                var row = table.NewRow();
                row[0] = "1";
                table.Rows.Add(row);
                var row2 = table.NewRow();
                row2[0] = "1";
                table.Rows.Add(row2);
                return table;
            };
            ShimLayout.GetByLayoutID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var layout = new Layout();
                layout.ContentSlot1 = 0;
                layout.TemplateID = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                return "ContentExist";
            };
            ShimTemplate.GetByTemplateID_NoAccessCheckInt32 = (id) =>
            {
                var template = new Template
                {
                    TemplateSource = "TemplateSource",
                    TemplateText = "TemplateText"
                };
                return template;
            };
            ShimGroup.ValidateDynamicStringsForTemplateListOfStringInt32User = (list, groupId, user) =>
            {
                throw new ECNException(new List<ECNError>()
                {
                   new ECNError
                   {
                       ErrorMessage = "Exception from Template subject line. error text",
                   }
                });
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }
    }
}
