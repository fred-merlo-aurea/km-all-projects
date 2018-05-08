using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using TechTalk.SpecFlow;
using Should;

using APIModel = EmailMarketing.API.Models.Group;

namespace EmailMarketing.API.Specs
{
    [Binding]
    public class GroupSteps //: ControllerTest<APIModel>
    {
        public readonly TestHelper Helper;
        public GroupSteps(TestHelper helper)
        {
            Helper = helper;
        }

        public APIModel GenerateNewContextObject()
        {
            return new APIModel()
            {
                FolderID = 4091,
                GroupName = Helper.GenerateRandomString("API Test Generated Group", 50),//group name max is 255,,
                GroupDescription = Helper.GenerateRandomString("Random Custom Field Description", 500),//group description max is 500
            };
        }

        #region GET
        [Given(@"I have an existing GroupID of (.*)")]
        public void GivenIHaveAnExistingGroupIDOf(int p0)
        {
            Helper.SetRecordId(p0);
        }

        [Then(@"The API Model object should have a GroupID property matching the given GroupID")]
        public void ThenTheAPIModelObjectShouldHaveAGroupIDPropertyMatchingTheGivenGroupID()
        {
            Helper.GetContextObject<APIModel>().GroupID.ShouldEqual(Helper.RecordID);
        }

        [Then(@"The HTTP Response Group should be a valid API Model object")]
        public void ThenTheHTTPResponseGroupShouldBeAValidAPIModelObject()
        {
            Helper.ContextObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            Helper.ContextObject.ShouldNotBeNull();
            Helper.ContextObject.ShouldBeType<APIModel>();
        }
        #endregion

        #region POST
        [Given(@"I have a new Group Object")]
        public void GivenIHaveANewGroupObject()
        {
            Helper.ContextObject = GenerateNewContextObject();
        }

        [Then(@"the Location Header should end with GroupID")]
        public void ThenTheLocationHeaderShouldEndWithGroupID()
        {
            Helper.Response.Headers.Location.ToString().EndsWith(Helper.RecordID.ToString());
        }
        #endregion

        #region NonExistant Group
        [Given(@"I have an nonexistent GroupID of (.*)")]
        public void GivenIHaveAnNonexistentGroupIDOf(int p0)
        {
            GivenIHaveAnExistingGroupIDOf(p0);
        }
        #endregion

        #region Invalid data
        [Given(@"I append invalid character '(.*)' to the GroupName")]
        public void GivenIAppendInvalidCharacterToTheGroupName(string p0)
        {
            Helper.GetContextObject<APIModel>().GroupName += p0;
        }
        #endregion
        
    }
}
