using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using KMPlatform.Entity;
using KMPlatform.Object;
using KMPlatform.Object.Fakes;
using KMPS.MD.MasterPages.Fakes;
using KMPS.MD.Objects;
using KMPS.MD.Objects.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using TestCommonHelpers;
using MdSite = KMPS.MD.MasterPages.Site;

namespace KMPS.MD.Tests
{
    [ExcludeFromCodeCoverage]
    public class BasePageTests : BaseControlTests
    {
        protected const string PropertyCustomerId = "CustomerID";
        protected const string PropertyUserId = "UserID";

        protected PrivateObject PrivatePage { get;set; }
        protected Page PortalPage { get; set; }
        protected MdSite MasterPage { get; set; }
        protected int CustomerId { get; set; }
        protected int UserId { get; set; }
        protected ECNSession EcnSession { get; set; }
        protected CheckBox CheckBoxControl { get; set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            
            UserId = 1;
            
            ShimForEcnSession();
            ShimForMasterPage();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            CheckBoxControl?.Dispose();
            PortalPage?.Dispose();
        }

        protected virtual void InitializePage(Page page)
        {
            PortalPage = page ?? throw new ArgumentNullException(nameof(page));
            PrivatePage = new PrivateObject(PortalPage);
            PrivateType = new PrivateType(page.GetType());
            InitializeAllControls(PortalPage);
        }

        protected override void InitializeAllControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                try
                {
                    if (field.GetValue(page) == null)
                    {
                        var constructor = field.FieldType.GetConstructor(new Type[0]);
                        if (constructor != null)
                        {
                            var control = constructor.Invoke(new object[0]);
                            if (control != null)
                            {
                                field.SetValue(page, control);
                                TryLinkFieldWithPage(control, page);

                                if (control is UserControl)
                                {
                                    InitializeAllControls(control);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //Ignore exceptions related to UI controls initialization
                }
            }
        }

        private static void TryLinkFieldWithPage(object field, object page)
        {
            if (page is Page)
            {
                var fieldType = field.GetType().GetField("_page",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        protected virtual void ShimForMasterPage()
        {
            MasterPage = new MdSite();
            ShimPage.AllInstances.MasterGet = _ => MasterPage;
            ShimSite.AllInstances.clientconnectionsGet = _ => new ShimClientConnections();
            ShimSite.AllInstances.LoggedInUserGet = _ => UserId;
        }

        protected virtual void ShimForEcnSession()
        {
            ShimECNSession.AllInstances.RefreshSession = _ => { };
            EcnSession = ReflectionHelper.CreateInstance<ECNSession>();
            EcnSession.CurrentUser.CurrentClient = new Client();
            ReflectionHelper.SetPropertyValue(EcnSession, PropertyCustomerId, CustomerId);
            ReflectionHelper.SetPropertyValue(EcnSession, PropertyUserId, UserId);
            ShimECNSession.CurrentSession = () => EcnSession;
        }

        protected override T GetField<T>(string fieldName)
        {
            var field = PrivatePage.GetField(fieldName) as T;

            field.ShouldNotBeNull($"The field {field} of type {typeof(T).Name} cannot be null");

            return field;
        }
    }
}
