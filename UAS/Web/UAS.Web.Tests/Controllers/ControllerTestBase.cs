using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KM.Platform.Fakes;
using KMPlatform.Entity;
using KMPlatform.Object;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using EnumsAccess = KMPlatform.Enums.Access;
using EnumsService = KMPlatform.Enums.Services;
using EnumsServiceFeature = KMPlatform.Enums.ServiceFeatures;

namespace UAS.Web.Tests.Controllers
{
    public abstract class ControllerTestBase
    {
        protected const string RouteAction = "action";
        protected const string RouteController = "controller";
        protected const string RouteErrorType = "errorType";
        protected const string RouteValueError = "Error";
        protected const string ErrorUnAuthorized = "UnAuthorized";
        protected const string PropertyCustomerId = "CustomerID";
        protected const string PropertyUserId = "UserID";
        protected const string KeyCurrentClient = "BaseControlller_CurrentClient";

        protected Controller Controller { get; set; }
        protected PrivateObject ControllerObject { get; set; }
        protected PrivateType ControllerType { get; set; }
        protected IDisposable ShimContext { get; private set; }
        protected NameValueCollection QueryString { get; } = new NameValueCollection();
        protected Mock<HttpContextBase> HttpContextMock { get; set; } = new Mock<HttpContextBase>();
        protected Mock<HttpRequestBase> RequestMock { get; set; } = new Mock<HttpRequestBase>();
        protected Mock<HttpSessionStateBase> SessionMock { get;set; } = new Mock<HttpSessionStateBase>();
        protected Mock<HttpServerUtilityBase> ServerMock { get;set; } = new Mock<HttpServerUtilityBase>();
        protected RouteData RouteData { get; set; }
        protected ECNSession EcnSession { get; set; }
        protected int CustomerId { get; set; } = 1;
        protected int UserId { get; set; } = 1;

        protected List<EnumsService> UserServices { get; } = new List<EnumsService>();
        protected List<EnumsServiceFeature> UserFeatures { get; } = new List<EnumsServiceFeature>();
        protected List<EnumsAccess> UserAccesses { get; } = new List<EnumsAccess>();

        [SetUp]
        public virtual void SetUp()
        {
            ShimContext = ShimsContext.Create();
            ShimForEcnSession();
            ShimForServiceAndAccess();
            UserServices.Clear();
            UserFeatures.Clear();
            UserAccesses.Clear();
        }

        [TearDown]
        public virtual void TearDown()
        {
            ShimContext?.Dispose();
            Controller?.Dispose();
        }

        protected virtual void Initialize(Controller controller)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            ControllerObject = new PrivateObject(controller);
            ControllerType = new PrivateType(controller.GetType());

            SessionMock.Setup(s => s[KeyCurrentClient]).Returns(EcnSession.CurrentUser.CurrentClient);
            MockControllerContext();
        }

        protected virtual void MockControllerContext()
        {
            if (Controller == null)
            {
                throw new InvalidOperationException("Invoke 'Initialize' method first.");
            }

            var request = RequestMock ?? new Mock<HttpRequestBase>();
            var session = SessionMock ?? new Mock<HttpSessionStateBase>();
            var context = HttpContextMock ?? new Mock<HttpContextBase>();
            var server = ServerMock ?? new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.MapPath(It.IsAny<string>())).Returns<string>(path => path);
            context.Setup(c => c.Request).Returns(request.Object);
            context.Setup(c => c.Session).Returns(session.Object);
            context.Setup(c => c.Server).Returns(server.Object);

            Controller.ControllerContext = new ControllerContext(
                context.Object,
                RouteData ?? new RouteData(),
                Controller);
        }

        protected virtual void ShimForEcnSession()
        {
            ShimECNSession.AllInstances.RefreshSession = _ => { };
            EcnSession = ReflectionHelper.CreateInstance<ECNSession>();
            EcnSession.CurrentUser.UserID = UserId;
            EcnSession.CurrentUser.CurrentClient = new Client();

            var privateObject = new PrivateObject(EcnSession);
            privateObject.SetFieldOrProperty(PropertyCustomerId, CustomerId);
            privateObject.SetFieldOrProperty(PropertyUserId, UserId);
            ShimECNSession.CurrentSession = () => EcnSession;
        }

        protected void ShimForServiceAndAccess()
        {
            ShimUser.HasServiceUserEnumsServices = (user, services) => UserServices.Contains(services);
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (user, services, feature, access) =>
                UserServices.Contains(services)
                && UserFeatures.Contains(feature)
                && UserAccesses.Contains(access);
        }

        protected void VerifyRedirectToError(
            RedirectToRouteResult result,
            string errorType = ErrorUnAuthorized,
            string action = RouteValueError,
            string controller = RouteValueError)
        {
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.RouteValues[RouteAction].ShouldBe(action),
                () => result.RouteValues[RouteController].ShouldBe(controller),
                () => result.RouteValues[RouteErrorType].ShouldBe(errorType));
        }
    }
}
