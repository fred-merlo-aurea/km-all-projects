using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using EmailMarketing.API.Controllers;
using EmailMarketing.API.Controllers.Fakes;
using EmailMarketing.API.Models;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;


namespace EmailMarketing.API.Tests.Controllers
{
    /// <summary>
    ///     Unit tests for <see cref="EmailMarketing.API.Controllers.MessageController"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class MessageControllerTest
    {
        private IDisposable _shimObject;
        private HttpStatusCode _resultCode;
        private string _resultRoute;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        private MessageController CreateController()
        {
            var testObject = new MessageController();
            testObject.Request = new HttpRequestMessage();
            return testObject;
        }

        private Message CreateModel()
        {
            return new Message
            {
                ContentSlot1 = 1,
                ContentSlot2 = 2,
                ContentSlot3 = 3,
                ContentSlot4 = 4,
                ContentSlot5 = 5,
                ContentSlot6 = 6,
                ContentSlot7 = 7,
                ContentSlot8 = 8,
                ContentSlot9 = 9,
                MessageTypeID = 1
            };
        }

        private Message CreateModelWithEmptySlots()
        {
            return new Message
            {
                ContentSlot1 = 0,
                ContentSlot2 = 0,
                ContentSlot3 = 0,
                ContentSlot4 = 0,
                ContentSlot5 = 0,
                ContentSlot6 = 0,
                ContentSlot7 = 0,
                ContentSlot8 = 0,
                ContentSlot9 = 0
            };
        }

        private void InitilizeFakes()
        {
            ShimClient.AllInstances.ECN_SelectInt32Boolean = (instance, id, ignore) => new KMPlatform.Entity.Client();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, service, feature, access) => true;
            ShimGroup.GetByGroupID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Group
            {
                CustomerID = 1
            };
            ShimFilter.GetByFilterID_NoAccessCheckInt32 = (id) => new ECN_Framework_Entities.Communicator.Filter
            {
                GroupID = 1,
                FilterID = 1,
                CustomerID = 1
            };

            ShimContent.GetByContentID_NoAccessCheckInt32Boolean = (id, child) =>
                new ECN_Framework_Entities.Communicator.Content { IsValidated = true };
            ShimHttpRequestMessage.AllInstances.PropertiesGet = (instance) =>
            new Dictionary<string, object>
            {
                { Strings.Headers.CustomerIdHeader, 1 },
                { Strings.Headers.APIAccessKeyHeader, string.Empty},
                { Strings.Properties.APIUserStashKey, new KMPlatform.Entity.User { } },
                { Strings.Properties.APICustomerStashKey, new ECN_Framework_Entities.Accounts.Customer { CustomerID = 1 } },
                { Strings.Properties.APIBaseChannelStashKey, new ECN_Framework_Entities.Accounts.BaseChannel()}
            };
            ShimAuthenticatedUserControllerBase.AllInstances.CreateResponseWithLocationOf1HttpStatusCodeM0Int32String(
                (AuthenticatedUserControllerBase instance, HttpStatusCode statusCode, Message model, int id, string routeName) =>
                {
                    _resultCode = statusCode;
                    _resultRoute = routeName;
                    return null;
                });

            ShimLayout.SaveLayoutUser = (model, user) => { };
            ShimLayout.GetByLayoutIDInt32UserBoolean = (id, user, ignore) => new Layout { };
        }
    }
}
