using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ecn.communicator.MasterPages.Fakes;
using ECN.Communicator.Tests.Helpers;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using PageHelper = ECN.Tests.Helpers.PageHelper;
using NUnit.Framework;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using static KMPlatform.Enums;

namespace ECN.Communicator.Tests.Main.Blasts
{
    /// <summary>
    /// Generic base class for Blasts UT classes
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public abstract class BaseBlastsTest<T1> : PageHelper where T1 : IDisposable, new()
    {
        protected IDisposable context;
        protected internal const string CurrentUser = "CurrentUser";
        protected internal const string CustomerID = "CustomerID";
        protected internal const string BaseChannelID = "BaseChannelID";
        protected internal const string UDFName = "UDFName";
        protected internal const string UDFNameValue = "UDFNameValue";
        protected internal const string UDFdata = "UDFdata";
        protected internal const string UDFdataValue = "UDFdataValue";
        protected internal const string PageLoad = "Page_Load";
        protected internal const string ClickThroughRatioReport = "Click Through Ratio Report";
        protected internal const string HelpContentClickThrough = "<p><b>Click-Through</b><br />Lists unique recepients who clicked on the URL links in your email Blast<br />Displays the time clicked, the URL link clicked.<br />Click on the email address to view the profile of that email address.";
        protected internal const string SendsReport = "Sends Report";
        protected internal const string ResendReport = "Resend Reports";
        protected internal const string UnopenedEmailReports = "Unopened Email Reports";
        protected internal const string BlastManager = "Blast Manager";
        protected MenuCode currentMenuCode;
        protected string heading;
        protected string helpContent;
        protected string helpTitle;
        protected string subMenu;
        protected bool roleAccessExceptionOccured;
        protected T1 testObject;
        protected PrivateObject privateObject;

        [SetUp]
        public virtual void SetUp()
        {
            context = ShimsContext.Create();
            SetECNCurrentSession();
            SetShimsCommunicator();
            currentMenuCode = MenuCode.BASECHANNEL;
            heading = string.Empty;
            helpContent = string.Empty;
            helpTitle = string.Empty;
            subMenu = string.Empty;
            roleAccessExceptionOccured = false;
            QueryString = new System.Collections.Specialized.NameValueCollection();
            InitTestObject();
        }

        [TearDown]
        public virtual void TearDown()
        {
            context.Dispose();
            testObject.Dispose();
            QueryString = null;
        }

        protected void InitTestObject()
        {
            testObject = new T1();
            privateObject = new PrivateObject(testObject);
        }

        protected static void SetECNCurrentSession()
        {
            ShimECNSession.CurrentSession = () =>
            {
                var user = ReflectionHelper.CreateInstance(typeof(KMPlatform.Entity.User));
                user.UserID = 1;
                ECNSession ecnSession = ReflectionHelper.CreateInstance(typeof(ECNSession));
                ReflectionHelper.SetField(ecnSession, CurrentUser, user);
                ReflectionHelper.SetField(ecnSession, CustomerID, 1);
                ReflectionHelper.SetField(ecnSession, BaseChannelID, 1);
                return ecnSession;
            };
        }

        protected void SetShimsCommunicator()
        {
            SetECNCurrentSession();
            ShimPage.AllInstances.MasterGet = (obj) => new ShimCommunicator();
            ShimCommunicator.AllInstances.UserSessionGet = (obj) => new ShimECNSession().Instance;
            ShimCommunicator.AllInstances.SetValuesEnumsMenuCodeStringStringStringString = (obj, value1, value2, value3, value4, value5)
                =>
            {
                currentMenuCode = value1;
                subMenu = value2;
                heading = value3;
                helpContent = value4;
                helpTitle = value5;
            };
        }

        protected static void SetShimKMPlatformUserAccess(Services services, ServiceFeatures serviceFeatures,
            Dictionary<Access, bool> access)
        {
            KM.Platform.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (param1, param2, param3, param4) =>
            {
                if(param2 == services &&
                param3 == serviceFeatures)
                {
                    if (access.ContainsKey(param4))
                    {
                        return access[param4];
                    }
                }
                return false;
            };
        }

        protected static void SetShimBusinessLogicUserAccess(Services services, ServiceFeatures serviceFeatures,
            Dictionary<Access, bool> access)
        {
            KMPlatform.BusinessLogic.Fakes.ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess =
                (param1, param2, param3, param4) =>
                {
                    if (param2 == services &&
                    param3 == serviceFeatures)
                    {
                        if (access.ContainsKey(param4))
                        {
                            return access[param4];
                        }
                    }
                    return false;
                };
        }
    }
}
