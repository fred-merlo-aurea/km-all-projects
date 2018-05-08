using System.Data;
using System.Reflection;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Common.Objects;
using ECN.Tests.Helpers;
using ECNEntities = ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.Framework.BusinessLayer.Tests.Communicator
{
    public partial class ContentTest
    {
        private const string EcnContentWithId = "<a href=\"http://www.google.com\" &l=\"Test\" ecn_id=\"85c25d23-8ea5-4803-92bf-e88030047475\">Test</a> \r\n <a href=\"http://www.google.com\" &l=\"Test\" ecn_id=\"85c25d23-8ea5-4803-92bf-e88030047475\">Test</a>";
        private const string EcnContent = "<ECN href=\"http://www.google.com\" &l=\"Test\" ecn_id=\"\">Test</a>";
        private const string TransLine = "<transnippet><transnippet_detail>##firstName##,##firstName##</transnippet_detail></transnippet>";
        private const string EcnId = "85c25d23-8ea5-4803-92bf-e88030047475";
        private const string firstNameColumn = "firstName";
        private const string MethodGetTransDetail = "GetTransDetail";
        private const string MethodCreateUniqueLinkIds = "CreateUniqueLinkIDs";
        private const string MethodReadyContent = "ReadyContent";
        private const string MethodModifyHTML = "ModifyHTML";

        [Test]
        public void ModifyHTML_WhenTransOpenIsAtZeroIndex_HtmlIsModified()
        {
            // Arrange
            var htmlSnippet = "<transnippet sort='firstName' filter_value='DummyString' filter_field='firstName'> <transnippet_detail>##firstName##,##firstName##</transnippet_detail></transnippet>";
            var methodArgs = new object[] { htmlSnippet, GetEmailProfileTable() };

            // Act	
            var result = (string)ReflectionHelper.CallMethod(typeof(Content), MethodModifyHTML, methodArgs, new Content());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () => result.Contains(DummyString).ShouldBeTrue());
        }

        [Test]
        public void ModifyHTML_WhenTransOpenIndexIsNegative_HtmlIsNotModified()
        {
            // Arrange
            var htmlSnippet = "<html> <transnippet_detail>##firstName##,##firstName##</transnippet_detail></transnippet>";
            var methodArgs = new object[] { htmlSnippet, GetEmailProfileTable() };

            // Act	
            var result = (string)ReflectionHelper.CallMethod(typeof(Content), MethodModifyHTML, methodArgs, new Content());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () => result.ShouldBe(htmlSnippet));
        }

        [Test]
        public void ModifyHTML_WhenTransOpenIndexIsGreaterThanZero_HtmlIsModified()
        {
            // Arrange
            var htmlSnippet = "sampleString<transnippet sort='firstName' filter_value='DummyString' filter_field='firstName'> <transnippet_detail>##firstName##,##firstName##</transnippet_detail></transnippet>";
            var methodArgs = new object[] { htmlSnippet, GetEmailProfileTable() };

            // Act	
            var result = (string)ReflectionHelper.CallMethod(typeof(Content), MethodModifyHTML, methodArgs, new Content());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () => result.Contains(DummyString).ShouldBeTrue());
        }

        [Test]
        public void ReadyContent_Success_TableTagsAreRemoved()
        {
            // Arrange
            var tbodyTag = "<tbody>";
            var contentObject = GetContentObject();
            var methodArgs = new object[] { contentObject, false };

            // Act	
            ReflectionHelper.CallMethod(typeof(Content), MethodReadyContent, methodArgs, new Content());

            // Assert
            contentObject.ShouldSatisfyAllConditions(
                () => contentObject.ShouldNotBeNull(),
                () => contentObject.ContentSource.ShouldNotBeNullOrWhiteSpace(),
                () => contentObject.ContentMobile.ShouldNotBeNullOrWhiteSpace(),
                () => contentObject.ContentSource.Contains(tbodyTag).ShouldBeFalse(),
                () => contentObject.ContentMobile.Contains(tbodyTag).ShouldBeFalse());
        }

        [TestCase(EcnContent, "")]
        [TestCase(EcnContentWithId, EcnId)]
        public void CreateUniqueLinkIds_Success_ReturnsContentString(string content, string ecnId)
        {
            // Arrange
            var methodArgs = new object[] { content };

            // Act	
            var result = (string)ReflectionHelper.CallMethod(typeof(Content), MethodCreateUniqueLinkIds, methodArgs, new Content());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () => result.Contains(ecnId).ShouldBeTrue());
        }

        [TestCase(EcnContent)]
        [TestCase(EcnContentWithId)]
        public void CreateUniqueLinkIds_Failure_ThrowsException(string content)
        {
            // Arrange
            ShimContent.CheckURLForHardCodedString = (x) => false;
            var methodArgs = new object[] { content };

            // Act //Assert
            var exception = Should.Throw<TargetInvocationException>(() =>
            {
                ReflectionHelper.CallMethod(typeof(Content), MethodCreateUniqueLinkIds, methodArgs, new Content());
            });
            exception.InnerException.ShouldBeOfType<ECNException>();
        }

        [Test]
        public void GetTransDetail_Success_ReturnsTransDetails()
        {
            // Arrange
            var methodArgs = new object[] { TransLine, GetEmailProfileTable(), firstNameColumn, DummyString, firstNameColumn };

            // Act	
            var result = (string)ReflectionHelper.CallMethod(typeof(Content), MethodGetTransDetail, methodArgs, new Content());

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrWhiteSpace(),
                () => result.Contains(DummyString).ShouldBeTrue());
        }

        private DataTable GetEmailProfileTable()
        {
            var emailProfileTable = new DataTable();
            emailProfileTable.Columns.Add(firstNameColumn);
            emailProfileTable.Rows.Add(DummyString);
            return emailProfileTable;
        }

        private ECNEntities.Content GetContentObject()
        {
            var contentObject = (ECNEntities.Content)ReflectionHelper.CreateInstance(typeof(ECNEntities.Content));
            var content = "<tbody>dummyString</tbody>";
            contentObject.ContentSource = content;
            contentObject.ContentMobile = content;
            contentObject.ContentText = content;
            return contentObject;
        }
    }
}