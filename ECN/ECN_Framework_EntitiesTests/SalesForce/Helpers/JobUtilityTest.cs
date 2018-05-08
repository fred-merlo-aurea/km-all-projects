using System;
using System.Net;
using Moq;
using NUnit.Framework;
using Shouldly;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Helpers;
using ECN_Framework_Entities.Salesforce.Interfaces;

namespace ECN_Framework_EntitiesTests.SalesForce.Helpers
{
    [TestFixture]
    public class JobUtilityTest
    {
        private const string AccessToken = "accessToken";
        private const string Operation = "operation";
        private const string JobId = "jobId";
        private const string BatchId = "batchId";
        private const SF_Utilities.SFObject SfObject = SF_Utilities.SFObject.Account;
        private const string RequestUrl = "http://requestUrl/";
        private const string Xml = "xml";

        [Test]
        public void Create_ResponseWithWebException_ReturnsEmptyString()
        {
            // Arrange
            var sfUtilityMock = BuildUtilityWithWebException();
            var request = BuildWebRequest(RequestUrl);
            sfUtilityMock.Setup(x => x.CreateNewJob(AccessToken, SfObject, Operation)).Returns(request);
            JobUtility.InitUtilities(sfUtilityMock.Object);

            // Act
            var result = JobUtility.Create(AccessToken, Operation, SfObject);

            // Asert
            result.ShouldBeEmpty();
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.Is<string>(r => r == request.RequestUri.ToString())), Times.Once);
        }

        [Test]
        public void Create_ResponseWithCorrectXml_ReturnsIdValue()
        {
            // Arrange
            const string expectedValue = "tag";
            const string xml = "<root><id>tag</id></root>";
            var sfUtilityMock = new Mock<ISFUtilities>();
            sfUtilityMock.Setup(x => x.ProceedJobRequest(It.IsAny<WebRequest>())).Returns(xml);
            JobUtility.InitUtilities(sfUtilityMock.Object);

            // Act
            var result = JobUtility.Create(AccessToken, Operation, SfObject);

            // Asert
            result.ShouldBe(expectedValue);
            sfUtilityMock.Verify(x => x.WriteToLog(It.IsAny<string>()), Times.Once);
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Close_ResponseWithWebException_ReturnsFalse()
        {
            // Arrange
            var request = BuildWebRequest(RequestUrl);
            var sfUtilityMock = BuildUtilityWithWebException();
            sfUtilityMock.Setup(x => x.CloseJob(AccessToken, JobId)).Returns(request);
            JobUtility.InitUtilities(sfUtilityMock.Object);
            // Act
            var result = JobUtility.Close(AccessToken, JobId);

            // Asert
            result.ShouldBeFalse();
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.Is<string>(r => r == request.RequestUri.ToString())), Times.Once);
        }

        [Test]
        public void Close_ResponseWithCloseState_ReturnsTrue()
        {
            // Arrange
            const string xml = "<root><state>CLOSED</state></root>";
            var sfUtilityMock = new Mock<ISFUtilities>();
            sfUtilityMock.Setup(x => x.ProceedJobRequest(It.IsAny<WebRequest>())).Returns(xml);
            JobUtility.InitUtilities(sfUtilityMock.Object);
            // Act
            var result = JobUtility.Close(AccessToken, JobId);

            // Asert
            result.ShouldBeTrue();
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Never);
            sfUtilityMock.Verify(x => x.WriteToLog(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void AddBatch_ResponseWithWebException_ReturnsEmptyString()
        {
            // Arrange
            var sfUtilityMock = BuildUtilityWithWebException();
            var request = BuildWebRequest(RequestUrl);
            sfUtilityMock.Setup(x => x.AddBatchToJob(AccessToken, JobId, Xml)).Returns(request);
            JobUtility.InitUtilities(sfUtilityMock.Object);

            // Act
            var result = JobUtility.AddBatch(AccessToken, JobId, Xml);

            // Asert
            result.ShouldBeEmpty();
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.Is<string>(r => r == request.RequestUri.ToString())), Times.Once);
        }

        [Test]
        public void AddBatch_ResponseWithCorrectXml_ReturnsIdValue()
        {
            // Arrange
            const string expectedValue = "tag";
            const string xml = "<root><id>tag</id></root>";
            var sfUtilityMock = new Mock<ISFUtilities>();
            sfUtilityMock.Setup(x => x.ProceedJobRequest(It.IsAny<WebRequest>())).Returns(xml);
            JobUtility.InitUtilities(sfUtilityMock.Object);

            // Act
            var result = JobUtility.AddBatch(AccessToken, Operation, xml);

            // Asert
            result.ShouldBe(expectedValue);
            sfUtilityMock.Verify(x => x.WriteToLog(It.IsAny<string>()), Times.Once);
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void GetBatchState_ResponseWithWebException_ReturnsFalse()
        {
            // Arrange
            var request = BuildWebRequest(RequestUrl);
            var sfUtilityMock = BuildUtilityWithWebException();
            sfUtilityMock.Setup(x => x.GetBatchState(AccessToken, JobId, BatchId)).Returns(request);
            JobUtility.InitUtilities(sfUtilityMock.Object);
            // Act
            var result = JobUtility.GetBatchState(AccessToken, JobId, BatchId);

            // Asert
            result.ShouldBeFalse();
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.Is<string>(r => r == request.RequestUri.ToString())), Times.Once);
        }

        [TestCase("Completed")]
        [TestCase("Failed request")]
        public void GetBatchState_ResponseWithValidStates_ReturnsTrue(string state)
        {
            // Arrange
            var xml = $"<root><state>{state}</state></root>";
            var sfUtilityMock = new Mock<ISFUtilities>();
            sfUtilityMock.Setup(x => x.ProceedJobRequest(It.IsAny<WebRequest>())).Returns(xml);
            JobUtility.InitUtilities(sfUtilityMock.Object);
            // Act
            var result = JobUtility.GetBatchState(AccessToken, JobId, BatchId);

            // Asert
            result.ShouldBeTrue();
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Never);
            sfUtilityMock.Verify(x => x.WriteToLog(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void GetBatchResults_ResponseWithWebException_ReturnsNull()
        {
            // Arrange
            var request = BuildWebRequest(RequestUrl);
            var sfUtilityMock = BuildUtilityWithWebException();
            sfUtilityMock.Setup(x => x.GetBatchResults(AccessToken, JobId, BatchId)).Returns(request);
            JobUtility.InitUtilities(sfUtilityMock.Object);
            // Act
            var result = JobUtility.GetBatchResults(AccessToken, JobId, BatchId);

            // Asert
            result.ShouldBeNull();
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.Is<string>(r => r == request.RequestUri.ToString())), Times.Once);
        }

        [Test]
        public void GetBatchState_ResponseWithValidXml_ReturnsValidDictionary()
        {
            // Arrange
            const string successKey = "success";
            const string failKey = "fail";
            const string xml = "<root><success>true</success><success>false</success></root>";
            var sfUtilityMock = new Mock<ISFUtilities>();
            sfUtilityMock.Setup(x => x.ProceedJobRequest(It.IsAny<WebRequest>())).Returns(xml);
            JobUtility.InitUtilities(sfUtilityMock.Object);
            // Act
            var result = JobUtility.GetBatchResults(AccessToken, JobId, BatchId);

            // Asert
            result.ShouldContainKeyAndValue(successKey, 1);
            result.ShouldContainKeyAndValue(failKey, 1);
            sfUtilityMock.Verify(x => x.LogWebException(It.IsAny<WebException>(), It.IsAny<string>()), Times.Never);
        }

        private Mock<ISFUtilities> BuildUtilityWithWebException()
        {
            var sfUtilityMock = new Mock<ISFUtilities>();
            sfUtilityMock.Setup(x => x.ProceedJobRequest(It.IsAny<WebRequest>())).Throws<WebException>();
            return sfUtilityMock;
        }

        private WebRequest BuildWebRequest(string url)
        {
            var request = new Mock<WebRequest>();
            request.SetupGet(x => x.RequestUri).Returns(new Uri(url));
            return request.Object;
        }
    }
}
