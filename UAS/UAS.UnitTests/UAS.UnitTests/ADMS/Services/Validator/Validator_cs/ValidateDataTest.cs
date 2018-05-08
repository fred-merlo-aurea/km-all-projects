using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADMS.Services.Fakes;
using ADMS.Services.Validator.Fakes;
using Core.ADMS.Events;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using FrameworkUAD_Lookup.BusinessLogic.Fakes;
using FrameworkUAD_Lookup.Entity;
using FrameworkUAS.BusinessLogic.Fakes;
using FrameworkUAS.Entity;
using FrameworkUAS.Object;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Shouldly;
using ADMS_Validator = ADMS.Services.Validator.Validator;
using Enums = FrameworkUAD_Lookup.Enums;
using ShimEnums = KMPlatform.BusinessLogic.Fakes.ShimEnums;

namespace UAS.UnitTests.ADMS.Services.Validator.Validator_cs
{
    [TestFixture()]
    public class ValidateDataTest
    {
        private const string TestFieldName = "TEST";
        private const int TestPubId = 1;

        private static readonly List<TransformSplitInfo> TransformSplitInfoList = new List<TransformSplitInfo>()
        {
            new TransformSplitInfo()
            {
                MAFField = TestFieldName,
                Delimiter = "colon",
                PubID = TestPubId
            },
            new TransformSplitInfo()
            {
                MAFField = TestFieldName,
                Delimiter = "comma",
                PubID = TestPubId
            },
            new TransformSplitInfo()
            {
                MAFField = TestFieldName,
                Delimiter = "semicolon",
                PubID = TestPubId
            },
            new TransformSplitInfo()
            {
                MAFField = TestFieldName,
                Delimiter = "tab",
                PubID = TestPubId
            },
            new TransformSplitInfo()
            {
                MAFField = TestFieldName,
                Delimiter = "tild",
                PubID = TestPubId
            },
            new TransformSplitInfo()
            {
                MAFField = TestFieldName,
                Delimiter = "pipe",
                PubID = TestPubId
            }
        };

        [Test]
        public void ValidateData_DelimitersCheck_ReturnsImportedFile()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var privateObject = new PrivateObject(new ADMS_Validator());
                var dataIV = new ImportFile();
                var eventMessage = new FileMoved()
                {
                    AdmsLog = new AdmsLog(),
                    SourceFile = new SourceFile()
                    {
                        IsDQMReady = true
                    },
                    Client = new Client()
                };
                var feature = new ServiceFeature();
                var dft = Enums.FileTypes.Web_Forms;

                ArrangeShims();

                List<SubscriberTransformed> listToAssert = null;
                ShimSubscriberTransformed.AllInstances
                        .SaveBulkSqlInsertListOfSubscriberTransformedClientConnectionsBoolean =
                    (transformed, list, arg3, arg4) =>
                    {
                        listToAssert = list;
                        return true;
                    };

                // Act
                var result =
                    privateObject.Invoke("ValidateData", new object[] {dataIV, eventMessage, feature, dft}) as
                        ImportFile;

                // Assert
                result.ShouldNotBeNull();
                result.ImportedRowCount.ShouldBe(1);
                listToAssert.ShouldNotBeNull();
                listToAssert.Count.ShouldBe(1);
            }
        }

        private static void ArrangeShims()
        {
            ShimCode.AllInstances.SelectCodeIdInt32 = (_, __) => new Code();
            ShimCode.AllInstances.SelectEnumsCodeType = (_, __) => new List<Code>()
            {
                new Code()
                {
                    CodeName = Enums.TransformationTypes.Split_Into_Rows.ToString().Replace("_", " ")
                }
            };

            ShimEnums.GetUADFeatureString = _ => KMPlatform.BusinessLogic.Enums.UADFeatures.AdHoc_Dimensions;
            ShimAdmsLog.AllInstances
                    .UpdateStringEnumsFileStatusTypeEnumsADMS_StepTypeEnumsProcessingStatusTypeEnumsExecutionPointTypeInt32StringBooleanInt32Boolean
                =
                (log, s, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => true;

            ShimValidator.AllInstances.InsertOriginalSubscribersImportFileFileMovedServiceFeatureEnumsFileTypes =
                (validator, file, arg3, arg4, arg5) =>
                    new HashSet<SubscriberOriginal>()
                    {
                        new SubscriberOriginal()
                    };
            ShimValidator.AllInstances.DeDupeTransformedImportFileHashSetOfSubscriberTransformedStringClient =
                (_, __, list, ___, _4) => list;

            ShimServiceFeature.AllInstances.SelectServiceFeatureInt32 = (_, __) => null;
            ShimTransformation.AllInstances.SelectInt32Int32Boolean = (_, __, ___, ____) =>
                new List<Transformation>();
            ShimTransformSplit.AllInstances.SelectSourceFileIDInt32 =
                (_, __) => new List<TransformSplit>();
            ShimAdHocDimensionGroup.AllInstances.SelectInt32Boolean =
                (_, __, ___) => new List<AdHocDimensionGroup>();

            ShimEmailStatus.AllInstances.SelectClientConnections = (_, __) =>
                new List<EmailStatus>()
                {
                    new EmailStatus()
                };

            ShimValidator.AllInstances
                    .HasQualifiedProfileHashSetOfSubscriberTransformedImportFileEnumsFileTypesAdmsLogSourceFile =
                (validator, set, arg3, arg4, arg5, arg6) =>
                    new HashSet<SubscriberTransformed>()
                    {
                        new SubscriberTransformed()
                        {
                            DemographicTransformedList = new HashSet<SubscriberDemographicTransformed>()
                            {
                                new SubscriberDemographicTransformed()
                                {
                                    MAFField = TestFieldName
                                }
                            }
                        }
                    };
            ShimTransformSplit.AllInstances.SelectObjectInt32 = (_, __) =>
                TransformSplitInfoList;
            ShimServiceBase.AllInstances.clientPubCodesGet = _ =>
                new Dictionary<int, string>()
                {
                    {TestPubId, ""}
                };

            ShimResponseGroup.AllInstances.SelectInt32ClientConnections = (_, __, ___) =>
                new List<ResponseGroup>()
                {
                    new ResponseGroup()
                };

            ShimCodeSheet.AllInstances.SelectInt32ClientConnections = (_, __, ___) =>
                new List<CodeSheet>();

            ShimAdmsLog.AllInstances.UpdateFailedCountsStringInt32Int32Int32Int32BooleanInt32 =
                (log, s, arg3, arg4, arg5, arg6, arg7, arg8) => true;

            ShimFieldMapping.AllInstances.SelectInt32Boolean = (mapping, i, arg3) => null;
            ShimTransformationFieldMap.AllInstances.Select = map => null;

            ShimAdmsLog.AllInstances.UpdateTransformedCountsStringInt32Int32Int32Int32BooleanInt32 =
                (log, s, arg3, arg4, arg5, arg6, arg7, arg8) => true;

            ShimAdmsLog.AllInstances.UpdateDuplicateCountsStringInt32Int32Int32Int32BooleanInt32 =
                (log, s, arg3, arg4, arg5, arg6, arg7, arg8) => true;

            ShimValidator.AllInstances
                    .CreateDashboardReportsImportFileFileMovedHashSetOfSubscriberTransformedHashSetOfSubscriberInvalidHashSetOfSubscriberTransformedListOfFieldMappingListOfTransformationFieldMapListOfTransformSplit
                = (validator, file, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => { };
        }
    }
}
