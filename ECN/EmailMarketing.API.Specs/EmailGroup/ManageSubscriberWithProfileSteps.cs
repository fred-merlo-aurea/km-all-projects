using System;
using TechTalk.SpecFlow;

namespace EmailMarketing.API.Specs.EmailGroup
{
    [Binding]
    public class ManageSubscriberWithProfileSteps
    {
        TestHelper Helper; // DI
        SubscriberProfileTestDataProvider DataProvider ;//= SubscriberProfileTestDataProvider.Factory(); // Singleton

        public ManageSubscriberWithProfileSteps(TestHelper helper)
        {
            DataProvider = new SubscriberProfileTestDataProvider();
            helper.TestDataProvider = DataProvider;
            Helper = helper;
        }

        [Given(@"I set Test Data Provider GroupID to (.*)")]
        public void GivenISetTestDataProviderGroupIDTo(int p0)
        {
            Helper.TestDataProvider.GroupID = p0;
        }


        [Given(@"I initialize a Subscriber Profile Test-Data Provider")]
        public void GivenIInitializeASubscriberProfileTest_DataProvider()
        {
            // noop: this is handled in the constructor, so calling this just ensures that this class is instantiated at the beginning of the run
            //DataProvider = new SubscriberProfileTestDataProvider();
        }

        [Given(@"I have an Email Profile List")]
        public void GivenIHaveAnEmailProfileList(Table table)
        {
            Helper.RandomString = Helper.GenerateRandomString("randomized-", 25);
            Helper.ContextObject = DataProvider.TransformProfileList(table, Helper);
        }

    }
}
