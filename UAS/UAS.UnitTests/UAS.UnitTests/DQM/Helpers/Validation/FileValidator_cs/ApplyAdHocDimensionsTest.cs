using System;
using System.Threading;
using NUnit.Framework;
using DQM.Helpers.Validation;
using UAS.UnitTests.Helpers;
using UAS_WS.Interface;
using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Generic;
using FrameworkUAS.Entity;
using FrameworkUAS.Service;
using System.ServiceModel.Fakes;
using FrameworkServices.Fakes;
using Moq;
using UAD_WS.Interface;
using KMPlatform.Object;
using FrameworkUAD.Entity;
using FrameworkUAD.Object;
using System.Collections.Specialized;
using Shouldly;

namespace UAS.UnitTests.DQM.Helpers.Validation.FileValidator_cs
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ApplyAdHocDimensionsTest
    {
        private const string ResponseGroup = "100";
        private const string CreatedDimension = "101";
        private const string ResponseGroupValue = "200";
        private const string DimensionValue = "180";
        private const string Method = "ApplyAdHocDimensions";

        private IDisposable _shimsContext;
        private FileValidator fileValidator;
        private ImportFile _importFile;
        private StringDictionary _stringDictionary;

        [SetUp]
        public void SetUp()
        {
            _importFile = new ImportFile();
            _stringDictionary = new StringDictionary();

            _importFile.HeadersTransformed.Add(ResponseGroup, ResponseGroupValue);
            _importFile.DataTransformed.Add(1, _stringDictionary);

            _stringDictionary.Add(ResponseGroup, ResponseGroupValue);
            _shimsContext = ShimsContext.Create();

            fileValidator = new FileValidator();
            fileValidator.SetField("accessKey", new Guid());
            fileValidator.SetField("client", new KMPlatform.Entity.Client());
            fileValidator.SetField("dataIV", _importFile);
            fileValidator.SetField("clientPubCodes", new Dictionary<int, string>()
            {
                {1,"value"}
            });

            MockClientBase();
        }


        [TearDown]
        public void TearDown()
        {
            _shimsContext.Dispose();

        }


        [Test]
        public void ApplyAdHocDimensions_emptyPubCodeSheet_SetAdHoc()
        {

            //Arrange
            MockServiceClient(MockIAdHocDimensionGroup(new AdHocDimensionGroup
            {
                IsActive = true,
                StandardField = ResponseGroup,
                IsPubcodeSpecific = true,
                CreatedDimension = CreatedDimension,
                DimensionGroupPubcodeMappings = new List<AdHocDimensionGroupPubcodeMap>
                {
                    new AdHocDimensionGroupPubcodeMap
                    {
                        IsActive=true,
                        Pubcode=string.Empty
                    }
                }
            }));
            MockServiceClient(MockICodeSheet(new CodeSheet
            {
                IsActive = true,
                PubID = 0,
                ResponseGroup = ResponseGroup,

            }));
            MockServiceClient(MockIAdHocDimension(new AdHocDimension
            {
                IsActive = true,
                MatchValue = "1",
                Operator = "2"

            }));

            //Act
            typeof(FileValidator).CallMethod(Method, null, fileValidator);

            //Assert
            _stringDictionary.Count.ShouldBe(2);
            _stringDictionary[CreatedDimension].ShouldBe(string.Empty);
            _stringDictionary[ResponseGroup].ShouldBe(ResponseGroupValue);

        }

        [Test]
        public void ApplyAdHocDimensions_ValidResponseValue_SetAdHoc()
        {

            //Arrange
            MockServiceClient(MockIAdHocDimensionGroup(new AdHocDimensionGroup
            {
                IsActive = true,
                StandardField = ResponseGroup,
                IsPubcodeSpecific = true,
                CreatedDimension = ResponseGroup,
                DimensionGroupPubcodeMappings = new List<AdHocDimensionGroupPubcodeMap>
                {

                    new AdHocDimensionGroupPubcodeMap
                    {
                        IsActive=true,
                        Pubcode=string.Empty
                    }
                }
            }));
            MockServiceClient(MockICodeSheet(new CodeSheet
            {
                IsActive = true,
                PubID = 0,
                ResponseGroup = ResponseGroup,

            }));
            MockServiceClient(MockIAdHocDimension(new AdHocDimension
            {
                IsActive = true,
                MatchValue = "1",
                Operator = "greater_than",
                DimensionValue = DimensionValue

            }));

            //Act
            typeof(FileValidator).CallMethod(Method, null, fileValidator);

            //Assert
            _stringDictionary.Count.ShouldBe(1);
            _stringDictionary[ResponseGroup].ShouldBe(DimensionValue);

        }

        [Test]
        public void ApplyAdHocDimensions_NotSpecificPubCode_SetAdHoc()
        {

            //Arrange
            MockServiceClient(MockIAdHocDimensionGroup(new AdHocDimensionGroup
            {
                IsActive = true,
                StandardField = ResponseGroup,
                IsPubcodeSpecific = false,
                CreatedDimension = CreatedDimension,
                DimensionGroupPubcodeMappings = new List<AdHocDimensionGroupPubcodeMap>
                {
                    new AdHocDimensionGroupPubcodeMap
                    {
                        IsActive=true,
                        Pubcode=string.Empty
                    }
                }
            }));
            MockServiceClient(MockICodeSheet(new CodeSheet
            {
                IsActive = true,
                PubID = 0,
                ResponseGroup = ResponseGroup,

            }));
            MockServiceClient(MockIAdHocDimension(new AdHocDimension
            {
                IsActive = true,
                MatchValue = "1",
                Operator = "2"

            }));

            //Act
            typeof(FileValidator).CallMethod(Method, null, fileValidator);

            //Assert
            _stringDictionary.Count.ShouldBe(2);
            _stringDictionary[CreatedDimension].ShouldBe(string.Empty);
            _stringDictionary[ResponseGroup].ShouldBe(ResponseGroupValue);

        }


        private void MockClientBase()
        {
            ShimClientBase<IAdHocDimensionGroup>.ConstructorString = (clientBase, endPoint) => { };
            ShimClientBase<IAdHocDimension>.ConstructorString = (clientBase, endPoint) => { };
            ShimClientBase<ICodeSheet>.ConstructorString = (clientBase, endPoint) => { };
        }



        private void MockServiceClient<T>(T t) where T : class
        {
            ShimServiceClient<T>.AllInstances.ProxyGet = (obj) => t;

        }


        private IAdHocDimensionGroup MockIAdHocDimensionGroup(params AdHocDimensionGroup[] adHocDimensionGroups)
        {
            var channelMock = new Mock<IAdHocDimensionGroup>();
            var responses = new Response<List<AdHocDimensionGroup>>
             {
                Result = new List<AdHocDimensionGroup>(adHocDimensionGroups),
                Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success
            };
            channelMock.Setup(channel => channel.Select(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(responses);

            return channelMock.Object;
        }


        private ICodeSheet MockICodeSheet(params CodeSheet[] codeSheets)
        {
            var channelMock = new Mock<ICodeSheet>();
            var responses = new Response<List<CodeSheet>>
            {
                Result = new List<CodeSheet>(codeSheets),
                Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success
            };
            channelMock.Setup(channel => channel.Select(It.IsAny<Guid>(), It.IsAny<ClientConnections>())).Returns(responses);

            return channelMock.Object;
        }


        private IAdHocDimension MockIAdHocDimension(params AdHocDimension[] adHocDimensions)
        {
            var channelMock = new Mock<IAdHocDimension>();
            var responses = new Response<List<AdHocDimension>>
            {
                Result = new List<AdHocDimension>(adHocDimensions),
                Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success
            };

            channelMock.Setup(channel => channel.Select(It.IsAny<Guid>(), It.IsAny<int>())).Returns(responses);

            return channelMock.Object;
        }


    }
}
