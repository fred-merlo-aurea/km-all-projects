using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using APIModel = EmailMarketing.API.Models.FormsCustomer;


namespace EmailMarketing.API.Specs.Forms
{
    [Binding]
    public class FormsSteps
    {
        public readonly TestHelper Helper;
        public FormsSteps(TestHelper helper)
        {
            Helper = helper;
        }

        public APIModel GenerateNewContextObject()
        {
            throw new NotImplementedException("POST is not supported for User API");
        }

        [Then(@"The HTTP Response Content should be a list of Forms Customer objects")]
        public void ThenTheHTTPResponseContentShouldBeAListOfFormsCustomerObjects()
        {
            Helper.ResponseContentShouldImplementInterface<IEnumerable<APIModel>>();
        }
    }
}
