using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Core_AMS.Utilities.Fakes;
using DQM.Helpers.Validation;
using FrameworkServices.Fakes;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup.Entity;
using NUnit.Framework;
using Shouldly;
using UAD_Lookup_WS.Interface;
using UAD_Lookup_WS.Interface.Fakes;
using UAS.UnitTests.DQM.Helpers.Validation.Common;
using UAS.UnitTests.Helpers;
using FrameworkUASService = FrameworkUAS.Service;
using Core_AMSUtilitiesEnums = Core_AMS.Utilities.Enums;

namespace UAS.UnitTests.DQM.Helpers.Validation.FileValidator_cs
{
    /// <summary>
    /// Unit Tests for <see cref="FileValidator"/>class.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class ScrantonCompanySurveyTest : Fakes
    {
        private FileValidator fileValidator;
        private const string scrantonCompanySurvey = "Scranton_CompanySurvey";
        private const string companyNameUsa = "KMAllUAS";
        private const string companyNameDqm = "KMAllDQM";
        private const string codeWorker = "codeWorker";

        [SetUp]
        public void Setup()
        {
            fileValidator = new FileValidator();
            SetupFakes();
            ShimFuzzySearch.AllInstances.SearchStringString = (x, companyName, z) =>
            {
                if (companyName == companyNameUsa)
                {
                    return 90;
                }
                return 0;
            };
        }

        [Test]
        public void ScrantonCompanySurvey_ClientPubCodesIsNotNull_ReturnObjectValue()
        {
            // Arrange
            var subscriberTransformed = CreateSubscriberTransformedObject();
            var clientPubCodes = new Dictionary<int, string> { { 1, "11xx22" } };
            var clientId = 1100;
            var parameters = new object[] { subscriberTransformed, clientPubCodes, clientId };
            var listCode = CreateCodeList();
            FrameworkServices.ServiceClient<ICode> code = CreateServiceClient(listCode);
            ReflectionHelper.SetField(fileValidator, codeWorker, code);
            // Act
            var result = (List<SubscriberTransformed>)ReflectionHelper.CallMethod(
                 fileValidator.GetType(),
                 scrantonCompanySurvey,
                 parameters,
                 fileValidator);

            // Assert
            result.ShouldNotBeNull();
            result.Any().ShouldBeTrue();
            result.Count().ShouldBe(2);
        }

        private List<SubscriberTransformed> CreateSubscriberTransformedObject()
        {
            return new List<SubscriberTransformed>
            {
                new SubscriberTransformed
                {
                    PubCode = "11xx22",
                    Company = companyNameDqm,
                    Email = "test@unittest.com",
                    CreatedByUserID = 1,
                    SORecordIdentifier = Guid.NewGuid(),
                    STRecordIdentifier = Guid.NewGuid(),
                    DemographicTransformedList = new HashSet<SubscriberDemographicTransformed>(),
                },
                new SubscriberTransformed
                {
                    PubCode = "11xx22",
                    Company = companyNameUsa,
                    Email = "admin.com@unittest.com",
                    CreatedByUserID = 1,
                    SORecordIdentifier = Guid.NewGuid(),
                    STRecordIdentifier = Guid.NewGuid(),
                    DemographicTransformedList = new HashSet<SubscriberDemographicTransformed>(),
                }
            };
        }

        private FrameworkServices.ServiceClient<ICode> CreateServiceClient(List<Code> listCode)
        {
            return new ShimServiceClient<ICode>
            {
                ProxyGet = () =>
                {
                    return new StubICode()
                    {
                        SelectGuidEnumsCodeType = (accessKey, codeType) =>
                        {
                            return new FrameworkUASService.Response<List<Code>>
                            {
                                Result = listCode
                            };
                        },
                        SelectCodeIdGuidInt32 = (accessKey, databaseFileTypeId) =>
                        {
                            return new FrameworkUASService.Response<Code>
                            {
                                Result = new Code
                                {
                                    CodeId = 1,
                                    CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString(),
                                }
                            };
                        },
                    };
                },
            };
        }

        private static List<Code> CreateCodeList()
        {
            var listCode = new List<Code>();
            listCode.Add(new Code
            {
                CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Standard.ToString(),
                CodeId = 1
            });
            listCode.Add(new Code
            {
                CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic.ToString(),
                CodeId = 2
            });
            listCode.Add(new Code
            {
                CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Ignored.ToString(),
                CodeId = 3
            });
            listCode.Add(new Code
            {
                CodeName = FrameworkUAD_Lookup.Enums.FieldMappingTypes.Demographic_Other.ToString().Replace('_', ' '),
                CodeId = 4
            });
            listCode.Add(new Code
            {
                CodeName = Core_AMSUtilitiesEnums.MAFFieldStandardFields.QUALIFICATIONDATE.ToString(),
                CodeId = 5
            });
            listCode.Add(new Code
            {
                CodeName = FrameworkUAD_Lookup.Enums.DemographicUpdate.Replace.ToString(),
                CodeId = 6
            });
            return listCode;
        }
    }
}
