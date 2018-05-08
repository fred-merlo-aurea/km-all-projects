using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using TechTalk.SpecFlow;
using Should;

using APIModel = EmailMarketing.API.Models.Image;

namespace EmailMarketing.API.Specs.Image
{
    [Binding]
    public class ImageSteps
    {

        public readonly TestHelper Helper;
        public ImageSteps(TestHelper helper)
        {
            Helper = helper;
            TestHelper.StoreObject(0, new APIModel()
            {
                FolderName = "",
                FolderID = -1,
                ImageData = new byte[] { 64, 62, 61 },
                ImageID = 0,
                ImageName = "Name",
                ImageURL = "",
            });
        }

        public APIModel GenerateNewContextObject()
        {
            return new APIModel()
            {
                FolderName = "",
                FolderID = -1,
                ImageData = new byte[] { 64, 62, 61 },
                ImageID = 0,
                ImageName = "Name",
                ImageURL = "",
            };
        }
        [Given(@"I have a new Image Object")]
        public void GivenIHaveANewImageObject()
        {
            Helper.ContextObject = GenerateNewContextObject();
        }
        
        [Given(@"I have a deletable Image Object")]
        public void GivenIHaveADeletableImageObject()
        {
            uint waitCount = 10;
            while (false == TestHelper.DeletableObjectId.HasValue && waitCount-- > 0)
            {
                System.Threading.Thread.Sleep(new TimeSpan(0, 0, 5));
            }
            if (false == TestHelper.DeletableObjectId.HasValue)
            {
                ScenarioContext.Current.Pending();
            }
            else
            {
                Helper.RecordID = TestHelper.DeletableObjectId.Value;
            }
        }
        
        [Given(@"I append invalid character '(.*)' to the ImageName")]
        public void GivenIAppendInvalidCharacterToTheImageName(string p0)
        {
            Helper.GetContextObject<APIModel>().ImageName += p0;
        }
        [Then(@"the Location Header should end with ImageID")]
        public void ThenTheLocationHeaderShouldEndWithImageID()
        {
            Helper.Response.Headers.Location.ToString().EndsWith(Helper.RecordID.ToString());
        }
      
        [Then(@"HttpResponseContent should have ImageName ending with the RandomString")]
        public void ThenHttpResponseContentShouldHaveContentTextEndingWithTheRandomString()
        {
            var responseObject = Helper.Response.Content.ReadAsAsync<APIModel>().Result;
            responseObject.ImageName.EndsWith(Helper.RandomString);
        }
    }
}
