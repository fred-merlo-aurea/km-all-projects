using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

using System.Net.Http;  //for ReadAsync request content extension method

using Should;
using System.Text.RegularExpressions;

namespace EmailMarketing.API.Specs.EmailGroup
{
    [Binding]
    public class ManageSubscriberSteps
    {
        TestHelper Helper; // DI
        ISubscriberTestDataProvider DataProvider ;//= ManageSubscriberTestDataProvider.Factory(); // Singleton

        public ManageSubscriberSteps(TestHelper helper)
        {
            Helper = helper;
            if(null == Helper.TestDataProvider)
            {
                Helper.TestDataProvider = new SubscriberProfileTestDataProvider();
            }
            DataProvider = Helper.TestDataProvider;
        }

        [Given(@"I set the API Access Key with the Test Data Provider")]
        public void GivenISetTheAPIAccessKeyWithTheTestDataProvider()
        {
            DataProvider.ApiAccessKey = Helper.APIAccessKey;
        }

        [Given(@"I set ActionName to ""(.*)""")]
        public void GivenISetActionNameTo(string p0)
        {
            Helper.ActionName = p0;
        }

        [Given(@"I have an Email List")]
        public void GivenIHaveAnEmailList(Table table)
        {
            Helper.RandomString = Helper.GenerateRandomString("randomized-",25);
            Helper.ContextObject = DataProvider.TransformEmailList(table,Helper);
        }

        [Then(@"HttpRequestContent should contain an object")]
        public void ThenHttpRequestContentShouldContainAnObject()
        {
            IEnumerable<Models.EmailGroup.SubscriptionResult> responseObject = 
                Helper.Response.Content.ReadAsAsync<IEnumerable<Models.EmailGroup.SubscriptionResult>>().Result;
            
            responseObject.ShouldNotBeNull();
            
            ScenarioContext.Current.Set<IEnumerable<Models.EmailGroup.SubscriptionResult>>(responseObject);
            Helper.ContextObject = responseObject;
        }

        [Then(@"the object should be an Enumeration of SubscriptionResult")]
        public void ThenTheObjectShouldBeAnEnumerationOfSubscriptionResult()
        {
            Helper.ContextObject.ShouldImplement<IEnumerable<Models.EmailGroup.SubscriptionResult>>();
        }


        [Then(@"the Enumeration of SubscriptionResult should validate as")]
        public void ThenTheEnumerationOfSubscriptionResultShouldValidateAs(Table table)
        {
            IEnumerable<Models.EmailGroup.SubscriptionResult> results =
                DataProvider.TransformResultList(
                    ScenarioContext.Current.Get<IEnumerable<Models.EmailGroup.SubscriptionResult>>());

            table.CompareToSet<Models.EmailGroup.SubscriptionResult>(@results);
        }

        [Then(@"I can cleanup EmailGroup Test Records from the DataBase")]
        public void ThenICanCleanupEmailGroupTestRecordsFromTheDataBase()
        {
            // TODO
            //ScenarioContext.Current.Pending();
        }

    }
}
