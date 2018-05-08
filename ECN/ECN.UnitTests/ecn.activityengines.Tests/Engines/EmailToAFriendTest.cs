using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI.WebControls;
using ecn.activityengines.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_Entities.Accounts;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ecn.activityengines.Tests.Engines
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.activityengines.emailtoafriend"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class EmailToAFriendTest
    {
        private IDisposable _shimContext;
        private PrivateObject _emailToAFriendPrivateObject;
        private emailtoafriend _emailToAFriendInstance;
        private Shimemailtoafriend _shimEmailToAFriend;
        private PlaceHolder _phError;
        private Label _lblErrorMessage;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            ShimBaseChannel.GetByBaseChannelIDInt32 = (id) => new BaseChannel() { BounceDomain = "BounceDomain", ChannelURL = "ChannelURL" };
            _emailToAFriendInstance = new emailtoafriend();
            _shimEmailToAFriend = new Shimemailtoafriend(_emailToAFriendInstance);
            _emailToAFriendPrivateObject = new PrivateObject(_emailToAFriendInstance);
            InitShims();
        }

        [TearDown]
        public void TearDown()
        {
            _phError.Dispose();
            _lblErrorMessage.Dispose();
            _shimContext.Dispose();
        }

        private void InitShims()
        {
            _lblErrorMessage = new Label();
            _phError = new PlaceHolder();
            _phError.Visible = false;
            _emailToAFriendPrivateObject.SetField("lblErrorMessage", BindingFlags.Instance | BindingFlags.NonPublic, _lblErrorMessage);
            _emailToAFriendPrivateObject.SetField("phError", BindingFlags.Instance | BindingFlags.NonPublic, _phError);
        }
    }
}
