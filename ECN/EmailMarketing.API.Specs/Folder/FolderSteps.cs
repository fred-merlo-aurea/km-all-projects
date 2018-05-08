using System;
using System.Net.Http;
using TechTalk.SpecFlow;

using Should;
using APIModel = EmailMarketing.API.Models.Folder;


namespace EmailMarketing.API.Specs.Folder
{
    [Binding]
    public class FolderSteps
    {
        public readonly TestHelper Helper;
        public FolderSteps(TestHelper helper)
        {
            Helper = helper;
            TestHelper.StoreObject(111, new APIModel()  //375005
            {
                FolderID = 111,
                FolderName = @"MyFolderNewFolderName",
                FolderDescription = @"MyFolderDescriptionNewFolderDescription",
                ParentID = 0,
                FolderType = "CNT"
            });
        }

        public APIModel GenerateNewContextObject()
        {
            return new APIModel()
            {
                //FolderID = 111,
                FolderName = Helper.GenerateRandomString(@"MyFolderNewFolderName", 255),//content title max is 255,
                FolderDescription = Helper.GenerateRandomString(@"MyFolderDescriptionNewFolderDescription", 255),//content title max is 255,
                ParentID = 0,
                FolderType = "CNT"
            };
        }

        #region GetExisting

        [Then(@"The HTTP Response Content should be a valid Folder Object")]
        public void ThenTheHTTPResponseContentShouldBeAValidFolderObject()
        {
            Helper.ContextObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            Helper.ContextObject.ShouldNotBeNull();
            Helper.ContextObject.ShouldBeType<APIModel>();
        }


        [Given(@"I have an existing Folder ID of (.*)")]
        public void GivenIHaveAnExistingFolderIDOf(int p0)
        {
            Helper.SetRecordId(p0);
        }
        
        [Then(@"The API Model object should have a FolderID property matching the given Folder ID")]
        public void ThenTheAPIModelObjectShouldHaveAFolderIDPropertyMatchingTheGivenFolderID()
        {
            Helper.GetContextObject<APIModel>().FolderID.ShouldEqual(Helper.RecordID);
        }
        #endregion GetExisting

        #region New

        [When(@"I invoke Folder DELETE")]
        public void WhenIInvokeFolderDELETE()
        {
            Helper.InvokeMethod(HttpMethod.Delete);
        }


        [Then(@"I can store the Folder object to test delete")]
        public void ThenICanStoreTheFolderObjectToTestDelete()
        {
            var responseObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            TestHelper.DeletableObjectId = responseObject.FolderID;
            TestHelper.StoreObject(TestHelper.DeletableObjectId.Value, responseObject);
        }


        [Given(@"I have an existing Folder Object with a FolderID of (.*)")]
        public void GivenIHaveAnExistingFolderObjectWithAFolderIDOf(int p0)
        {
            Helper.SetRecordId(p0);
            Helper.ContextObject = TestHelper.RetreiveObject(p0);
        }

        [Given(@"I generate a RandomString to append to the FolderDescription")]
        public void GivenIGenerateARandomStringToAppendToTheFolderDescription()
        {
            Helper.GetContextObject<APIModel>().FolderDescription =
                Helper.GenerateRandomString(Helper.GetContextObject<APIModel>().FolderDescription + "Generated Text Content - Updated", 8000);
        }

        [Then(@"the Location Header should end with FolderID")]
        public void ThenTheLocationHeaderShouldEndWithFolderID()
        {
            Helper.Response.Headers.Location.ToString().EndsWith(Helper.RecordID.ToString());
        }

        [Then(@"HttpResponseContent should have FolderDescription ending with the RandomString")]
        public void ThenHttpResponseContentShouldHaveFolderDescriptionEndingWithTheRandomString()
        {
            var responseObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            responseObject.FolderDescription.EndsWith(Helper.RandomString);
        }

        [Given(@"I have a new Folder Object")]
        public void GivenIHaveANewFolderObject()
        {
            Helper.ContextObject = GenerateNewContextObject();
        }


        


        [Given(@"I have a deletable Folder Object")]
        public void GivenIHaveADeletableFolderObject()
        {
            uint waitCount = 10;
            while (false == TestHelper.DeletableObjectId.HasValue && waitCount-- > 0)
            {
                System.Threading.Thread.Sleep(new TimeSpan(0, 0, 5));
            }
            if (false == TestHelper.DeletableObjectId.HasValue)
            {
                //throw new ApplicationException("timeout waiting for a deletable object; did POST succeed?");
                ScenarioContext.Current.Pending();
            }
            else
            {
                Helper.RecordID = TestHelper.DeletableObjectId.Value;
            }
        }

        [Given(@"I have an nonexistent Folder ID of (.*)")]
        public void GivenIHaveAnNonexistentFolderIDOf(int p0)
        {
            GivenIHaveAnExistingFolderIDOf(p0); // no test-side validation
        }

        [Given(@"I append invalid character '(.*)' to the FolderName")]
        public void GivenIAppendInvalidCharacterToTheFolderName(string p0)
        {
            Helper.GetContextObject<APIModel>().FolderName += p0;
        }

        #endregion New
    }
}
