using System;
using System.Web;
using System.Web.Fakes;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ECN_Framework_Entities.Communicator;
using KMDbManagers.Fakes;
using KMEntities;
using KMManagers.Tests.Helpers;
using KMManagers.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace KMManagers.Tests
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class PrepopulatorTest
    {
        private IDisposable _shimObject;
        private PrivateObject _prepopulator;
        private Form _form;
        private HttpRequest _httpRequest;
        private List<GroupDataFields> _groupDataFields;
        private List<string> _statesValues;
        private HttpCookieCollection _httpRequestCookies;
        private bool _applicationErrorWasLogged;
        private static string _emailTypeValue = ((int)KMEnums.EmailType.Confirmation).ToString();
        private static string _databaseTypeValue = ((int)KMEnums.PopulationType.Database).ToString();
        private const string PrivateFieldValue= "form";
        private const string DummyGuidStr = "8096f77e-42d7-40f0-9a19-1ddd8ba43414";
        private const string BaseRequestUri = "http://localhost:8080/api/nodes?child=true";
        private const string BaseRequestUriNoChild = "http://localhost:8080/api/nodes?child=false";
        private const string BaseRequestUriNoQuery = "http://localhost:8080/api/nodes";
        private const string RequestQueryWithChild = "child=true";
        private const string FieldLabelValue = "fieldLabelValue";
        private const string CountryName = "Brazil";        
        private const string RegionName = "Rio de Janeiro";        
        private const string RegionCode = "RJA";
        private const string SerilalizedValueWrongly = "{\"GroupID\":\"11\',\'SubscribeTypeCode\':\'dummyStringValue\',\'Password\':\'PasssowrdDummyValue\'}";
        private const string SerilalizedValueCorrectly = "{\'GroupID\':\'11\',\'SubscribeTypeCode\':\'dummyStringValue\',\'Voice\':\'dummyStringValue\',\'password\':\'dummyStringValue\',\'State\':\'RJA\'}";       
        private const int NewsLetterType = (int)KMEnums.ControlType.NewsLetter;
        private const int TextAreaType = (int)KMEnums.ControlType.TextArea;
        private const string GroupFieldShortNamePhone = "Phone";
        private const int GroupFieldIdPhone = 12;
        private const string GroupFieldShortNamePassword = "Password";
        private const int GroupFieldIdPassword = 13;
        private const string GroupFieldShortNameState = "State";
        private const int GroupFieldIdState = 14;
        private const string GroupFieldShortNameInvalid = "Invalid";
        private const int GroupFieldIdInvalid = 15;
        private const int TypeSeqIDForMailingPassword = 103;
        private const int ControlID = 1;
        private const int ControlPropertySeqID = 71;       

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();

            KMManagerTestsHelper.CreateAppSettingsShim();

            KMManagerTestsHelper.CreateWebAppSettingsShim();

            _groupDataFields = new List<GroupDataFields>();
            KMManagerTestsHelper.CreateFormManagerShim(_groupDataFields);

            ShimFormDbManager.Constructor =
                (formDbManager) =>
                {
                    var shimDbManager = new ShimFormDbManager(formDbManager)
                    {
                        GetChildByTokenUIDString = (tokenUid) => _form,
                        GetByTokenUIDString = (tokenUid)=> _form
                    };
                };

            _httpRequestCookies = new HttpCookieCollection
            {
                new HttpCookie(HTMLGenerator.HiddenIDForToken, DummyGuidStr)
            };

            ShimHttpRequest.ConstructorStringStringString =
                (request, filename, uriValue, queryvalue) =>
                { 
                    var shimRequest = new ShimHttpRequest(request)
                    {
                        UrlReferrerGet = () => new Uri(uriValue),
                        CookiesGet = () => _httpRequestCookies,
                        UserAgentGet = () => FieldLabelValue
                    };
                };
            KMManagerTestsHelper.CreateEmailGroupShim();

            KMManagerTestsHelper.CreateMXValidateShim();

            KMManagerTestsHelper.CreateControlPropertyManagerShim(ControlPropertySeqID);

            _statesValues = new List<string>();
            ShimControlManager.Constructor =
                (controlManger) =>
                {
                    var shim = new ShimControlManager(controlManger);
                    shim.GetStatesAll = () => _statesValues;
                    shim.GetStateCodeInt32 = (code) => RegionCode;
                    shim.GetCountryNameInt32 = (code) => CountryName;
                    shim.GetStateNameString = (code) => RegionName;
                };

            _httpRequest = new HttpRequest(FieldLabelValue, BaseRequestUri, RequestQueryWithChild);

            _form = KMManagerTestsHelper.GetForm();

            _applicationErrorWasLogged = false;

            var prepopulator = new Prepopulator();
            _prepopulator = new PrivateObject(prepopulator);
            _prepopulator.SetFieldOrProperty(PrivateFieldValue, _form);
                        
            CreateApplicationLogShim();

            CreateAPIRunnerBaseShim(SerilalizedValueCorrectly);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private void CreateAPIRunnerBaseShim(string objSerilized)
        {
            ShimAPIRunnerBase.AllInstances.CheckEmailByNewsLetterStringInt32Int32String =
                (apiRunner, accessKey, customerID, groupId, email) => objSerilized;
        }
        public void CreateApplicationLogShim()
        {
            KM.Common.Entity.Fakes.ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationId, note, charityId, customerId) => { _applicationErrorWasLogged = true; };
        }

    }
}
