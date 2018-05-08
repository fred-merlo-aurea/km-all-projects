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
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using StringAssert = Microsoft.VisualStudio.TestTools.UnitTesting.StringAssert;

namespace ECN.BlastEngine.Tests
{
    public partial class BlastEngineTest
    {
        private const string METHOD_VerifyForeignKey = "VerifyForeignKeysExist";

        [Test]
        public void VerifyForeignKeysExist_OnException_ReturnEmptyString()
        {
            // Arrange
            var blastId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "Exception while checking key data";

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            Assert.AreEqual(exceptionMsg, actualResult);
        }

        [Test]
        public void VerifyForeignKeysExist_BlastIsNull_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "Blast ID: ";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                return null;
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_BlastGroupIsNull_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "Group ID: ";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                return blast;
            };
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) =>
            {
                return null;
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_CodeExist_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "Code ID: ";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.CodeID = 1;
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

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_RefBlastExist_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "Ref Blast ID(s): ";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.RefBlastID = "1,2";
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

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutNotExist_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "Layout ID: ";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                return null;
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistContentSlot1Exists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                layout.ContentSlot1 = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                return "ContentExist";
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistContentSlot2Exists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                layout.ContentSlot2 = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                if (id == 1)
                {
                    return "ContentExist";
                }
                return "";
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistContentSlot3Exists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                layout.ContentSlot3 = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                if (id == 1)
                {
                    return "ContentExist";
                }
                return "";
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistContentSlot4Exists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                layout.ContentSlot4 = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                if (id == 1)
                {
                    return "ContentExist";
                }
                return "";
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistContentSlot5Exists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                layout.ContentSlot5 = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                if (id == 1)
                {
                    return "ContentExist";
                }
                return "";
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistContentSlot6Exists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                layout.ContentSlot6 = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                if (id == 1)
                {
                    return "ContentExist";
                }
                return "";
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistContentSlot7Exists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                layout.ContentSlot7 = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                if (id == 1)
                {
                    return "ContentExist";
                }
                return "";
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistContentSlot8Exists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                layout.ContentSlot8 = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                if (id == 1)
                {
                    return "ContentExist";
                }
                return "";
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistContentSlot9Exists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
                layout.ContentSlot9 = 1;
                return layout;
            };
            ShimECNBlastEngine.AllInstances.ContentExistsInt32 = (eng, id) =>
            {
                if (id == 1)
                {
                    return "ContentExist";
                }
                return "";
            };

            // Act
            var actualResult = typeof(ECNBlastEngine).CallMethod(METHOD_VerifyForeignKey,
                new object[] { blastId }, blastEngine).ToString();

            // Assert
            StringAssert.Contains(actualResult, exceptionMsg);
        }

        [Test]
        public void VerifyForeignKeysExist_NotChampionLayoutExistTemplateIDExists_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "ContentExist";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
        public void VerifyForeignKeysExist_NotChampionLayoutExistTemplateIDExistsWithException_ReturnString()
        {
            // Arrange
            var blastId = 1;
            var blastGroupId = 1;
            var blastEngine = new ECNBlastEngine();
            var exceptionMsg = "UDF(s):";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (id, child) =>
            {
                var blast = new BlastRegular();
                blast.BlastID = blastId;
                blast.GroupID = blastGroupId;
                blast.BlastType = BlastType.HTML.ToString();
                blast.LayoutID = 1;
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
