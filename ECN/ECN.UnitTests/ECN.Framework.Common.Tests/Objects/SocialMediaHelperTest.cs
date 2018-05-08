using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Fakes;
using System.Text;
using System.Web.Script.Serialization;
using ECN.Tests.Helpers;
using ECN_Framework_Common.Objects;
using KM.Common.Entity;
using KM.Common.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using ShimCommonEncryption = KM.Common.Fakes.ShimEncryption;
using static ECN_Framework_Common.Objects.SocialMediaHelper;
using NUnit.Framework;
using Shouldly;

namespace ECN_Framework_Common.Tests.Objects
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    class SocialMediaHelperTest
    {
        private const string DummyString = "dummyString";
        private const string One = "1";
        private const string Likes = "Likes";
        private const string UniqueClicks = "Unique_Clicks";
        private const string TotalComments = "Total_Comments";
        private const string ActivityDomainPath = "Activity_DomainPath";
        private const string SocialPreview = "SocialPreview";
        private const string KMCommonApplication = "KMCommon_Application";
        private const string Success = "success";
        private const string MethodGetAccountsFromJSON = "GetAccountsFromJSON";
        private IDisposable _context;
        private SocialMediaHelper _testObject;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();
            _testObject = ReflectionHelper.CreateInstance(typeof(SocialMediaHelper));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void GetFBPostData_Success_ReturnsPostDateDictionary()
        {
            // Arrange
            var response = new ShimHttpWebResponse();
            var dummyData = Encoding.ASCII.GetBytes(GetJsonString());
            response.GetResponseStream = () => new MemoryStream(dummyData);
            ShimHttpWebRequest.AllInstances.GetResponse = (x) => response;

            // Act
            var postData = SocialMediaHelper.GetFBPostData(DummyString, DummyString);

            // Assert
            postData.ShouldSatisfyAllConditions(
                () => postData.ShouldNotBeNull(),
                () => postData[Likes].ShouldBe(4),
                () => postData[UniqueClicks].ShouldBe(1),
                () => postData[TotalComments].ShouldBe(4),
                () => postData.Count.ShouldBe(8));
        }

        [Test]
        public void PostToLI_Success_ReturnsSuccessfulResponse()
        {
            // Arrange
            SetupForMethodPostToLI();

            // Act
            var liResponse = SocialMediaHelper.PostToLI(DummyString, DummyString, DummyString, DummyString, true, DummyString, One, 1, 1, 1);

            // Assert
            liResponse.ShouldSatisfyAllConditions(
                () => liResponse.ShouldNotBeNull(),
                () => liResponse[Success].ShouldBe(bool.TrueString.ToLower()),
                () => liResponse.Count.ShouldBe(3));
        }

        [Test]
        public void PostToLI_Failure_ReturnsFailureResponse()
        {
            // Arrange
            SetupForMethodPostToLI();
            ShimHttpWebRequest.AllInstances.GetResponse = (x) => throw new Exception();

            // Act
            var liResponse = SocialMediaHelper.PostToLI(DummyString, DummyString, DummyString, DummyString, true, DummyString, One, 1, 1, 1);

            // Assert
            liResponse.ShouldSatisfyAllConditions(
                () => liResponse.ShouldNotBeNull(),
                () => liResponse[Success].ShouldBe(bool.FalseString.ToLower()),
                () => liResponse.Count.ShouldBe(2));
        }

        [Test]
        public void PostToLI_WhenEncryptionFails_ReturnsFailureResponse()
        {
            // Arrange
            SetupForMethodPostToLI();
            var requestFailed = false;
            ShimHttpWebRequest.AllInstances.GetRequestStream = (x) => throw new Exception();
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 = (a, b, c, d, e, f) => 
            {
                requestFailed = true;
                return 0;
            };

            // Act
            var liResponse = SocialMediaHelper.PostToLI(DummyString, DummyString, DummyString, DummyString, true, DummyString, One, 1, 1, 1);

            // Assert
            liResponse.ShouldSatisfyAllConditions(
                () => liResponse.ShouldNotBeNull(),
                () => liResponse.Count.ShouldBe(0),
                () => requestFailed.ShouldBeTrue());
        }

        [TestCase("id")]
        [TestCase("name")]
        [TestCase("link")]
        [TestCase("picture")]
        [TestCase("access_token")]
        public void GetAccountsFromJSON_WhenJsonIsNotPerms_ReturnsEmptyList(string propertyName)
        {
            // Arrange
            var methodArgs = new object[] { GetJsonAccountsString(propertyName) };

            // Act
            var accounts = ReflectionHelper.CallMethod(typeof(SocialMediaHelper), MethodGetAccountsFromJSON, methodArgs, _testObject) as List<FBAccount>;

            //Assert
            accounts.ShouldSatisfyAllConditions(
                () => accounts.ShouldNotBeNull(),
                () => accounts.Count.ShouldBe(0));
        }

        private void SetupForMethodPostToLI()
        {
            var response = new ShimHttpWebResponse();
            var dummyData = Encoding.ASCII.GetBytes(GetJsonString());
            response.GetResponseStream = () => new MemoryStream(dummyData);
            response.StatusCodeGet = () => HttpStatusCode.Created;
            ShimHttpWebRequest.AllInstances.GetResponse = (x) => response;
            ShimHttpWebResponse.AllInstances.GetResponseStream = (x) => new MemoryStream(dummyData);
            ShimEncryption.GetCurrentByApplicationIDInt32 = (x) => new Encryption();
            ShimCommonEncryption.EncryptStringEncryption = (x, y) => DummyString;
            ConfigurationManager.AppSettings[ActivityDomainPath] = DummyString;
            ConfigurationManager.AppSettings[SocialPreview] = DummyString;
            ConfigurationManager.AppSettings[KMCommonApplication] = One;
        }

        private object GetValueObject(string nameParameter)
        {
            return new
            {
                from = new
                {
                    id = 1
                },
                name = nameParameter,
                values = new
                {
                    value = 1
                }
            };
        }

        private string GetJsonString()
        {
            var jsonObject = new
            {
                shares = new
                {
                    count = new
                    {
                        test = 1
                    }
                },
                data = new[]
                {
                    GetValueObject("post_impressions"),
                    GetValueObject("post_impressions_unique"),
                    GetValueObject("post_consumptions"),
                    GetValueObject("post_consumptions_unique"),
                }
            };
            return new JavaScriptSerializer().Serialize(jsonObject);
        }

        private string GetJsonAccountsString(string type)
        {
            var jsonObject = new object();
            switch (type)
            {
                case "picture":
                    jsonObject = new
                    {
                        picture = new
                        {
                            http = new
                            {
                                test = 1
                            }
                        }
                    };
                    break;
                case "name":
                    jsonObject = new
                    {
                        name = type
                    };
                    break;
                case "id":
                    jsonObject = new
                    {
                        id = type
                    };
                    break;
                case "link":
                    jsonObject = new
                    {
                        link = new
                        {
                            Value = DummyString
                        }
                    };
                    break;
                case "access_token":
                    jsonObject = new
                    {
                        access_token = type
                    };
                    break;
            }
            return ((new JavaScriptSerializer().Serialize(jsonObject)).Replace("{", "")).Replace("}","");
        }
    }
}