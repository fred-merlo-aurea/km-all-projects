using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EmailMarketing.Site.Infrastructure.Concrete;
using EmailMarketing.Site.Infrastructure.Abstract;

using EcnUser = KMPlatform.Entity.User;
using EcnCustomer = ECN_Framework_Entities.Accounts.Customer;
using EcnUserManager = ECN_Framework_BusinessLayer.Accounts.User;
using EcnCustomerManager = ECN_Framework_BusinessLayer.Accounts.Customer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using System.Web.Security;

namespace EmailMarketing.Site.Infrastructure.Concrete.Tests
{
    [TestClass()]
    public class AuthenticationFormsProviderTests
    {
        ////EcnUser systemAdminUser = new EcnUser { IsSysAdmin = true };
        //static readonly EcnCustomer stubCustomer = new EcnCustomer 
        //{ 
        //    CommunicatorChannelID = 1, 
        //    CollectorChannelID = 1,
        //    CreatorChannelID = 1, 
        //    PublisherChannelID = 1, 
        //    CharityChannelID = 1
        //};

        //[TestMethod()]
        //public void AuthenticationFormsProviderTest_Create()
        //{
        //    AuthenticationFormsProvider p = new AuthenticationFormsProvider(Mock.Of<IWebAuthenticationWrapper>(), Mock.Of<IAccountProvider>());
        //    Assert.IsNotNull(p);
        //}

        //[TestMethod()]
        //public void Deauthenticate_Should_Call_SignOut()
        //{
        //    // arrange: mock dependencies outside the scope of this test
        //    var authWrapper = new Mock<IWebAuthenticationWrapper>();
        //    var authProvider = new AuthenticationFormsProvider(authWrapper.Object, Mock.Of<IAccountProvider>());

        //    // act
        //    authProvider.Deauthenticate((c) => { });

        //    // assert: should call FormsAuthentication.SignOut()
        //    authWrapper.Verify((aw) => aw.SignOut(), Times.Once());
        //}

        //[TestMethod()]
        //public void Authenticate_Objects()
        //{
        //    // assemble: mock dependencies outside the scope of this test
        //    var authWrapper = new Mock<IWebAuthenticationWrapper>();
        //    var authProvider = new AuthenticationFormsProvider(authWrapper.Object, Mock.Of<IAccountProvider>());

        //    // assemble: setup forms encryption method to return the unencrypted UserData, to be used as ECN cookie's value
        //    authWrapper.Setup((aw) => aw.Encrypt(It.IsAny<FormsAuthenticationTicket>())).Returns((FormsAuthenticationTicket t) => t.UserData);

        //    var masterId = 123;
        //    var persist = true;
        //    var userId = 123;
        //    HttpCookie cookie = null;

        //    // assemble: use our set-cookie handler to capture to generated ECN cookie
        //    Action<HttpCookie> setCookie = (c) => cookie = c;

        //    // act
        //    bool success = authProvider.Authenticate(setCookie, new KMPlatform.Entity.User { UserID = userId }, stubCustomer, persist, masterId);

            
        //    // assert: should call FormsAuthentication.SignOut()
        //    authWrapper.Verify((aw) => aw.Initialize(), Times.Once(), 
        //        "FormsAuthentication.Initialize() not called exactly once");

        //    // assert: should call FormsAuthentication.SetAuthCookie()
        //    authWrapper.Verify((aw) => aw.SetAuthCookie(userId.ToString(),persist), Times.Once(),
        //        "FormsAuthentication.SetAuthCookie() not called exactly once");

        //    Assert.IsNotNull(cookie, "ecn cookie not set");

        //    Assert.IsTrue(cookie.Value.EndsWith(userId.ToString()), "explicit master-id not set correctly");

        //    Assert.IsTrue(success, "authentication did not succeed");
        //}

        //[TestMethod()]
        //[Ignore] // not implemented
        //public void AuthenticateTest_UsernameAndPassword()
        //{
        //    // not implemented
        //    //Assert.Fail();        
        //}
    }
}
