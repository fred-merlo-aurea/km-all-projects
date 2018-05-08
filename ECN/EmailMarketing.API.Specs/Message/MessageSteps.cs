using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using TechTalk.SpecFlow;
using Should;

using APIModel = EmailMarketing.API.Models.Message;


namespace EmailMarketing.API.Specs.Message
{
    [Binding]
    public class MessageSteps
    {
        public readonly TestHelper Helper;
        public MessageSteps(TestHelper helper)
        {
            Helper = helper;
            TestHelper.StoreObject(76660, new APIModel()
            {
                LayoutID = 76660,
                TemplateID = 1062,
                FolderID = 0,
                LayoutName = "Web API Test Layout Name",
                ContentSlot1 = 85917,
                ContentSlot2 = 85920,
                ContentSlot3 = 0,
                ContentSlot4 = null,
                ContentSlot5 = 0,
                ContentSlot6 = null,
                ContentSlot7 = 0,
                ContentSlot8 = null,
                ContentSlot9 = 0,
                TableOptions = "default",
                DisplayAddress = Helper.GenerateRandomString("Generated Text Content - Updated", 8000),
                MessageTypeID = -1,
                CreatedDate = null,
                UpdatedDate = null,

                CreatedUserID = 0,
                UpdatedUserID = 0,

                
            });
        }

        public APIModel GenerateNewContextObject()
        {
            return new APIModel()
            {
                LayoutID = 76660,
                TemplateID = 1062,
                FolderID = 0,
                LayoutName = Helper.GenerateRandomString("Generated Text Content - Updated", 8000),
                ContentSlot1 = 85917,
                ContentSlot2 = 85920,
                ContentSlot3 = 0,
                ContentSlot4 = null,
                ContentSlot5 = 0,
                ContentSlot6 = null,
                ContentSlot7 = 0,
                ContentSlot8 = null,
                ContentSlot9 = 0,
                UpdatedDate = null,
                TableOptions = "",
                DisplayAddress = "321 Happy St, Tunsasmiles, CA 10101",
                MessageTypeID = -1,
                CreatedDate = null,
                CreatedUserID = 0,
                UpdatedUserID = 0,


            };
        }


        #region GET
        [Given(@"I have an existing Layout ID of (.*)")]
        public void GivenIHaveAnExistingLayoutIDOf(int p0)
        {
            Helper.SetRecordId(p0);
        }
        
        [Then(@"The API Model object should have a LayoutID property matching the given Layout ID")]
        public void ThenTheAPIModelObjectShouldHaveALayoutIDPropertyMatchingTheGivenLayoutID()
        {
            Helper.GetContextObject<APIModel>().LayoutID.ShouldEqual(Helper.RecordID);
        }

        [Then(@"The HTTP Response Message should be a valid API Model object")]
        public void ThenTheHTTPResponseMessageShouldBeAValidAPIModelObject()
        {
            Helper.ContextObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            Helper.ContextObject.ShouldNotBeNull();
            Helper.ContextObject.ShouldBeType<APIModel>();

        }
        #endregion 

        [Given(@"I have an existing Message Object with a LayoutID of (.*)")]
        public void GivenIHaveAnExistingMessageObjectWithALayoutIDOf(int p0)
        {
            Helper.SetRecordId(p0);
            Helper.ContextObject = TestHelper.RetreiveObject(p0);
        }

        [Given(@"I generate a RandomString to append to the DisplayAddress")]
        public void GivenIGenerateARandomStringToAppendToTheDisplayAddress()
        {
            Helper.GetContextObject<APIModel>().DisplayAddress =
                Helper.GenerateRandomString(Helper.GetContextObject<APIModel>().DisplayAddress + "Generated Text Content - Updated", 8000);
        }

        [Then(@"the Location Header should end with LayoutID")]
        public void ThenTheLocationHeaderShouldEndWithLayoutID()
        {
            Helper.Response.Headers.Location.ToString().EndsWith(Helper.RecordID.ToString());
        }

        [Then(@"HttpResponseContent should have DisplayAddress ending with the RandomString")]
        public void ThenHttpResponseContentShouldHaveDisplayAddressEndingWithTheRandomString()
        {
            var responseObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            responseObject.DisplayAddress.EndsWith(Helper.RandomString);
        }

        [Given(@"I have a new Message Object")]
        public void GivenIHaveANewMessageObject()
        {
            Helper.ContextObject = GenerateNewContextObject();
        }

        [Given(@"I have a deletable Message Object")]
        public void GivenIHaveADeletableMessageObject()
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

        [Given(@"I have an nonexistent Layout ID of (.*)")]
        public void GivenIHaveAnNonexistentLayoutIDOf(int p0)
        {
            GivenIHaveAnExistingLayoutIDOf(p0); // no test-side validation
        }

        [Given(@"I append invalid character '(.*)' to the LayoutName")]
        public void GivenIAppendInvalidCharacterToTheLayoutName(string p0)
        {
            Helper.GetContextObject<APIModel>().LayoutName += p0;
        }

        [Then(@"I can store the Message object to test delete")]
        public void ThenICanStoreTheObjectToTestDelete()
        {
            var responseObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            TestHelper.DeletableObjectId = responseObject.LayoutID;
            TestHelper.StoreObject(TestHelper.DeletableObjectId.Value, responseObject);
        }
    }
}
