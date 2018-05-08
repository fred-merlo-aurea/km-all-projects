using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using ecn.communicator.mvc.Controllers;
using Ecn.Communicator.Mvc.Interfaces;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.MVC.Tests
{
    [TestFixture]
    public partial class GroupControllerTest
    {
        private const int ChannelIdValue = 1;
        private const string CSVContentType = "text/csv";
        private const string EmailAddressPropertyName = "EmailAddress";
        private const string DateAddedPropertyName = "DateAdded";
        private const string FirstEmail = "first@domain.com";
        private const string SecondEmail = "second@domain.com";
        private static string NoThresholdEmailCSVFileName = "_NoThreshold_Emails.CSV";
        private static string MasterSuppressedEmailCSVFileName = "_MasterSuppressed_Emails.CSV";
        private DateTime MinDateTime = DateTime.MinValue;
        private DateTime MaxDateTime = DateTime.MaxValue;
        private Mock<IFileSystem> _file;
        private Mock<HttpServerUtilityBase> _server;
        private Mock<IChannelNoThresholdList> _channelNoThresholdList;
        private Mock<HttpResponseBase> _response;
        private StringWriter _stringWriter;
        private GroupController _controller;

        [SetUp]
        public void SetUp()
        {
            _stringWriter = new StringWriter();

            _file = new Mock<IFileSystem>();
            _file.Setup(x => x.CloseTextWriter(It.IsAny<TextWriter>()));
            _file.Setup(x => x.CreateDirectory(It.IsAny<string>()));
            _file.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(false);
            _file.Setup(x => x.FileAppendText(It.IsAny<string>())).Returns(_stringWriter);
            _file.Setup(x => x.FileDelete(It.IsAny<string>()));
            _file.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            _server = new Mock<HttpServerUtilityBase>();
            _server.Setup(x => x.MapPath(It.IsAny<string>())).Returns(string.Empty);

            var noThresholdChannels = new List<ChannelNoThresholdList>();
            noThresholdChannels.Add(new ChannelNoThresholdList()
            {
                EmailAddress = FirstEmail,
                CreatedDate = MinDateTime
            });
            noThresholdChannels.Add(new ChannelNoThresholdList()
            {
                EmailAddress = SecondEmail,
                CreatedDate = MaxDateTime
            });
            _channelNoThresholdList = new Mock<IChannelNoThresholdList>();
            _channelNoThresholdList
                .Setup(x => x.GetByEmailAddress(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<User>()))
                .Returns(noThresholdChannels);

            _response = new Mock<HttpResponseBase>();

            var baseChannel = new BaseChannel()
            {
                BaseChannelID = ChannelIdValue
            };

            var masterSuppressionChannels = new List<ChannelMasterSuppressionList>();
            masterSuppressionChannels.Add(new ChannelMasterSuppressionList()
            {
                EmailAddress = FirstEmail,
                CreatedDate = MinDateTime
            });
            masterSuppressionChannels.Add(new ChannelMasterSuppressionList()
            {
                EmailAddress = SecondEmail,
                CreatedDate = MaxDateTime
            });
            var channelMasterSuppressionList = new Mock<IChannelMasterSuppressionList>();
            channelMasterSuppressionList
                .Setup(x => x.GetByEmailAddress(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<User>()))
                .Returns(masterSuppressionChannels);

            _controller = new GroupController(
                _file.Object, 
                _server.Object, 
                _channelNoThresholdList.Object,
                _response.Object, 
                baseChannel, 
                new User(), 
                channelMasterSuppressionList.Object);
        }

        [Test]
        public void ExportNoThresholdSuppresion_EmptySearchString_ReturnsNull()
        {
            // Arrange, Act
            var actual = _controller.ExportNoThresholdSuppresion(string.Empty);

            // Assert
            actual.ShouldBeNull();
            AssertMethodsCalled(NoThresholdEmailCSVFileName);
        }

        [Test]
        public void ExportChannelSuppresion_EmptySearchString_ReturnsNull()
        {
            // Arrange, Act
            var actual = _controller.ExportChannelSuppresion(string.Empty);

            // Assert
            actual.ShouldBeNull();
            AssertMethodsCalled(MasterSuppressedEmailCSVFileName);
        }

        private void AssertMethodsCalled(string csvFileName)
        {
            var serverPath = string.Format("{0}/customers/{1}/downloads/", ConfigurationManager.AppSettings["Images_VirtualPath"], ChannelIdValue);
            _server.Verify(x => x.MapPath(serverPath), Times.Once());
            _file.Verify(x => x.DirectoryExists(string.Empty), Times.Once());
            _file.Verify(x => x.CreateDirectory(string.Empty), Times.Once());

            var outFileName = string.Format("{0}{1}", ChannelIdValue, csvFileName);
            _file.Verify(x => x.FileExists(outFileName), Times.Once());
            _file.Verify(x => x.FileDelete(outFileName), Times.Once());

            var stringWriterContents = new StringBuilder();
            stringWriterContents.AppendLine(string.Format("{0}, {1}", EmailAddressPropertyName, DateAddedPropertyName));
            stringWriterContents.AppendLine(string.Format("{0}, {1}", FirstEmail, MinDateTime));
            stringWriterContents.AppendLine(string.Format("{0}, {1}", SecondEmail, MaxDateTime));
            _stringWriter.ToString().ShouldBe(stringWriterContents.ToString());

            _response.VerifySet(x => x.ContentType = CSVContentType, Times.Once());
            var header = string.Format("attachment; filename={0}", outFileName);
            _response.Verify(x => x.AddHeader("content-disposition", header), Times.Once());

            _response.Verify(x => x.WriteFile(outFileName), Times.Once());
            _response.Verify(x => x.Flush(), Times.Once());
            _response.Verify(x => x.End(), Times.Once());
        }
    }
}
