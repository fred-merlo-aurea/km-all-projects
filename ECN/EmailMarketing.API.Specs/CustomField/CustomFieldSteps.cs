using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using TechTalk.SpecFlow;
using Should;

using APIModel = EmailMarketing.API.Models.CustomField;

namespace EmailMarketing.API.Specs
{
    [Binding]
    public class CustomFieldSteps //: ControllerTest<APIModel>
    {
        public readonly TestHelper Helper;
        public CustomFieldSteps(TestHelper helper)
        {
            Helper = helper;
        }

        public APIModel GenerateNewContextObject()
        {
            return new APIModel()
            {
                ShortName = Helper.GenerateRandomString("RandomCustomField", 50),//shortname max is 50
                LongName = Helper.GenerateRandomString("Random Custom Field Description", 255),//longname max is 255
                GroupID = 49195,
                IsPublic = "Y",
            };
        }

        #region GET
        [Given(@"I have an existing GroupDataFieldsID of (.*)")]
        public void GivenIHaveAnExistingGroupDataFieldsIDOf(int p0)
        {
            Helper.SetRecordId(p0);
        }
        
        [Then(@"The API Model object should have a GroupDataFieldsID property matching the given GroupDataFieldsID")]
        public void ThenTheAPIModelObjectShouldHaveAGroupDataFieldsIDPropertyMatchingTheGivenGroupDataFieldsID()
        {
            Helper.GetContextObject<APIModel>().GroupDataFieldsID.ShouldEqual(Helper.RecordID);
        }

        [Then(@"The HTTP Response CustomField should be a valid API Model object")]
        public void ThenTheHTTPResponseCustomFieldShouldBeAValidAPIModelObject()
        {
            Helper.ContextObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            Helper.ContextObject.ShouldNotBeNull();
            Helper.ContextObject.ShouldBeType<APIModel>();
        }
        #endregion

        #region POST
        [Given(@"I have a new CustomField Object")]
        public void GivenIHaveANewCustomFieldObject()
        {
            Helper.ContextObject = GenerateNewContextObject();
        }

        [Then(@"the Location Header should end with GroupDataFieldsID")]
        public void ThenTheLocationHeaderShouldEndWithGroupDataFieldsID()
        {
            Helper.Response.Headers.Location.ToString().EndsWith(Helper.RecordID.ToString());
        }

        [Then(@"HttpResponseContent should have ShortName ending with the RandomString")]
        public void ThenHttpResponseContentShouldHaveShortNameEndingWithTheRandomString()
        {
            var responseObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            responseObject.ShortName.EndsWith(Helper.RandomString);
        }
        #endregion

        #region NonExistant CustomField
        [Given(@"I have an nonexistent GroupDataFieldsID of (.*)")]
        public void GivenIHaveAnNonexistentGroupDataFieldsIDOf(int p0)
        {
            GivenIHaveAnExistingGroupDataFieldsIDOf(p0);
        }
        #endregion

        #region Invalid data
        [Given(@"I append invalid character '(.*)' to the ShortName")]
        public void GivenIAppendInvalidCharacterToTheShortName(string p0)
        {
            Helper.GetContextObject<APIModel>().ShortName += p0;
        }
        #endregion
    }
}
