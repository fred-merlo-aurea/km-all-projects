using System;
using System.IO;
using System.Net;
using System.Text;
using ECN_Framework_Entities.Salesforce.Interfaces;
using Moq;

namespace ECN_Framework_EntitiesTests.Salesforce.Tests
{
    public class SalesForceTestBase
    {
        protected Mock<WebResponse> CreateWebResponse(Exception exception)
        {
            var webResponse = new Mock<WebResponse>();
            webResponse.Setup(x => x.GetResponseStream()).Throws(exception);
            return webResponse;
        }

        protected Mock<WebResponse> CreateWebResponse(string lastObjectProperty)
        {
            var firstResponse = new StringBuilder();
            firstResponse.AppendLine(lastObjectProperty)
                .AppendLine(".done\" :false")
                .AppendLine(".nextRecordsUrl\" :Dummy Url");

            var firstResponseStream = new MemoryStream(Encoding.UTF8.GetBytes(firstResponse.ToString()));

            var webResponse = new Mock<WebResponse>();
            webResponse.Setup(x => x.GetResponseStream()).Returns(firstResponseStream);

            return webResponse;
        }

        protected Mock<ISFUtilities> CreateSFUtilities(string lastProperty)
        {
            var secondResponse = new StringBuilder();
            secondResponse.AppendLine(lastProperty)
                .AppendLine(".done\" :true");

            var secondResponseStream = new MemoryStream(Encoding.UTF8.GetBytes(secondResponse.ToString()));

            var queryParameter = string.Empty;
            var sfUtilities = new Mock<ISFUtilities>();
            sfUtilities.Setup(x => x.GetNextURL(It.IsAny<string>())).Returns(new StreamReader(secondResponseStream));

            return sfUtilities;
        }

        protected Mock<WebRequest> CreateWebRequest(Mock<WebResponse> webResponse)
        {
            var webRequest = new Mock<WebRequest>();
            webRequest.Setup(x => x.GetResponse()).Returns(webResponse.Object);
            return webRequest;
        }
    }
}
