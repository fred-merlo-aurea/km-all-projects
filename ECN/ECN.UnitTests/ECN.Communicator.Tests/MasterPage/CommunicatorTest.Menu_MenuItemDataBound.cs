using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Reflection;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.MasterPages.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KM.Platform.Fakes;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Communicator.Tests.Master
{
    public partial class CommunicatorTest
    {
        private const string MID_VirtaulPathSettingKey = "Accounts_VirtualPath";
        private const string MID_VirtaulPathSettingValue = "VirtaulPathSettingValue";
        private const string MID_SiteMapCurrentNodeURL = "SiteMapCurrentNodeUrl";
        private const string MID_CommunicatorPageName = "communicator.mvc";
        private const string MID_HomeStartingNodeUrl = "~/ecn.accounts/main/";
        private const string MID_ItemParentNavigateUrl = "ItemParentNavigateUrl";
        private string _removedMenuItemText;
        private SiteMapDataSource _siteMapSecondLevel;
        private Menu _menu;

        [TestCase("admin")]
        [TestCase("basechannel")]
        [TestCase("customer")]
        public void Menu_MenuItemDataBound_NotSelectableItems_ItemSelectableDisabled(string itemText)
        {
            // Arrange
            InitTest_Menu_MenuItemDataBound(menuItem: out MenuItem menuItem, currentMenuCode: MenuCode.INDEX, itemText: itemText);
            // Act
            _communicatorPrivateObject.Invoke("Menu_MenuItemDataBound", new object[] { null, new MenuEventArgs(menuItem) });

            // Assert
            menuItem.Selectable.ShouldBeFalse();
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Menu_MenuItemDataBound_HasCurrentNode_NodeUrlInitialized(bool itemSelected, bool hasParent)
        {
            // Arrange
            InitTest_Menu_MenuItemDataBound(menuItemWithParent: hasParent, hasSiteMapNode: true, menuItem: out MenuItem menuItem, currentMenuCode: MenuCode.INDEX, itemText: "HOME");
            var expectedNodeUrl = MID_HomeStartingNodeUrl;
            if (itemSelected && hasParent)
            {
                expectedNodeUrl = MID_ItemParentNavigateUrl;
            }
            else if (itemSelected && !hasParent)
            {
                expectedNodeUrl = MID_SiteMapCurrentNodeURL;
            }
            menuItem.Selected = itemSelected;

            // Act
            _communicatorPrivateObject.Invoke("Menu_MenuItemDataBound", new object[] { null, new MenuEventArgs(menuItem) });

            // Assert
            _siteMapSecondLevel.ShouldSatisfyAllConditions(
                () => _siteMapSecondLevel.StartingNodeUrl.ShouldBe(expectedNodeUrl),
                () => menuItem.NavigateUrl.ShouldBe($"{MID_VirtaulPathSettingValue}/main/"));
        }

        [TestCase(MenuCode.GROUPS, "groups")]
        [TestCase(MenuCode.CONTENT, "content/messages")]
        [TestCase(MenuCode.BLASTS, "blasts/reporting")]
        [TestCase(MenuCode.EVENTS, "folders")]
        [TestCase(MenuCode.SALESFORCE, "salesforce")]
        [TestCase(MenuCode.OMNITURE, "omniture")]
        [TestCase(MenuCode.INDEX, "HOME")]
        [TestCase(MenuCode.PAGEKNOWTICE, "HOME")]
        [TestCase(MenuCode.REPORTS, "HOME")]
        public void Menu_MenuItemDataBound_NoCurrentNode_NodeUrlInitialized(MenuCode menuCode, string itemText)
        {
            // Arrange
            InitTest_Menu_MenuItemDataBound(
                hasSiteMapNode: false,
                menuItem: out MenuItem menuItem,
                currentMenuCode: menuCode,
                itemText: itemText,
                menuItemNavigateUrl: MID_CommunicatorPageName);
            var expectedUrl = string.Empty;
            switch (menuCode)
            {
                case MenuCode.GROUPS: expectedUrl = "/ecn.communicator/main/lists/"; break;
                case MenuCode.CONTENT: expectedUrl = "/ecn.communicator/main/content/"; break;
                case MenuCode.BLASTS: expectedUrl = "/ecn.communicator/main/ecnwizard/"; break;
                case MenuCode.EVENTS: expectedUrl = "/ecn.communicator/main/events/messagetriggers.aspx"; break;
                case MenuCode.INDEX: expectedUrl = "~/ecn.accounts/main/"; break;
                case MenuCode.PAGEKNOWTICE: expectedUrl = "/ecn.communicator/main/PageWatch/PageWatchEditor.aspx"; break;
                case MenuCode.REPORTS: expectedUrl = "/ecn.communicator/main/Reports/SentCampaignsReport.aspx#"; break;
                case MenuCode.SALESFORCE: expectedUrl = "/ecn.communicator/main/SalesForce/ECN_SF_Integration.aspx"; break;
                case MenuCode.OMNITURE: expectedUrl = "/ecn.communicator/main/Omniture/OmnitureCustomerSetup.aspx"; break;
            }

            // Act
            _communicatorPrivateObject.Invoke("Menu_MenuItemDataBound", new object[] { null, new MenuEventArgs(menuItem) });

            // Assert
            _siteMapSecondLevel.StartingNodeUrl.ShouldBe(expectedUrl);
        }

        [TestCase("groups", true, true, false)]
        [TestCase("groups", true, false, true)]
        [TestCase("content/messages", false, false, false)]
        [TestCase("blasts/reporting", false, false, false)]
        [TestCase("basechannel", false, false, false)]
        [TestCase("update email", false, false, false)]
        [TestCase("admin", false, false, false)]
        [TestCase("page knowtice", false, false, false)]
        [TestCase("events", false, false, false)]
        [TestCase("add group", false, false, false)]
        [TestCase("add emails", false, false, false)]
        [TestCase("import data", false, false, false)]
        [TestCase("clean emails", false, false, false)]
        [TestCase("create content", false, false, false)]
        [TestCase("create message", false, false, false)]
        [TestCase("manage images/storage", false, false, false)]
        [TestCase("link source", false, false, false)]
        [TestCase("message types", true, true, true)]
        [TestCase("message types", true, false, false)]
        [TestCase("message type priority", true, true, true)]
        [TestCase("message type priority", true, false, false)]
        [TestCase("message thresholds", true, true, true)]
        [TestCase("message thresholds", true, false, false)]
        [TestCase("blast status", false, false, false)]
        [TestCase("setup blast", false, false, false)]
        [TestCase("link report", false, false, false)]
        [TestCase("auto-responder", false, false, false)]
        [TestCase("active customer blasts", false, false, false)]
        [TestCase("view blast links", false, false, false)]
        [TestCase("email search", false, false, false)]
        public void Menu_MenuItemDataBound_RemovableItemSupplied_ItemRemoved(string itemText, bool isAdmin, bool isChannelAdmin, bool hasServiceFeature)
        {
            // Arrange
            _removedMenuItemText = string.Empty;
            InitTest_Menu_MenuItemDataBound(
                hasSiteMapNode: false,
                menuItem: out MenuItem menuItem,
                currentMenuCode: MenuCode.INDEX,
                itemText: itemText,
                menuItemNavigateUrl: MID_CommunicatorPageName,
                isChannelAdmin: isChannelAdmin,
                hasServiceFeature: hasServiceFeature,
                isAdmin: isAdmin);

            // Act
            _communicatorPrivateObject.Invoke("Menu_MenuItemDataBound", new object[] { null, new MenuEventArgs(menuItem) });

            //Assert 
            _removedMenuItemText.ShouldBe(isChannelAdmin || itemText == "groups" ? string.Empty : itemText);
        }

        private void InitTest_Menu_MenuItemDataBound(out MenuItem menuItem, MenuCode currentMenuCode, string itemText, bool hasServiceFeature = false, bool isChannelAdmin = false, bool hasSiteMapNode = false, string menuItemNavigateUrl = "", bool menuItemWithParent = true, bool isAdmin = false)
        {
            SetPageProperties_Menu_MenuItemDataBound();
            SetPageControls_Menu_MenuItemDataBoun();
            ShimUser.HasAccessUserEnumsServicesEnumsServiceFeaturesEnumsAccess = (u, service, feature, access) => false;
            ShimUser.IsChannelAdministratorUser = (u) => isChannelAdmin;
            ShimUser.IsAdministratorUser = (u) => isAdmin;
            ShimUser.IsSystemAdministratorUser = (u) => false;
            KMPlatform.BusinessLogic.Fakes.ShimClient.HasServiceFeatureInt32EnumsServicesEnumsServiceFeatures = (user, service, feature) => hasServiceFeature;
            ShimMenuItem.AllInstances.ParentGet = (mi) => menuItemWithParent ? new MenuItem() { Selectable = true, NavigateUrl = MID_ItemParentNavigateUrl } : null;
            menuItem = new MenuItem();
            menuItem.Text = itemText;
            menuItem.Value = itemText;
            menuItem.NavigateUrl = menuItemNavigateUrl;
            ShimSiteMapNode.AllInstances.UrlGet = (siteMap) => MID_SiteMapCurrentNodeURL;
            ShimSiteMap.CurrentNodeGet = () => hasSiteMapNode ? new ShimSiteMapNode() : null;
            ShimCommunicator.AllInstances.CurrentMenuCodeGet = (c) => currentMenuCode;
        }

        private void SetPageProperties_Menu_MenuItemDataBound()
        {
            var shimECNSession = new ShimECNSession();
            ShimCommunicator.AllInstances.UserSessionGet = (c) => shimECNSession;
            ShimConfigurationManager.AppSettingsGet = () =>
                {
                    var settingsCollection = new NameValueCollection();
                    settingsCollection.Add(MID_VirtaulPathSettingKey, MID_VirtaulPathSettingValue);
                    return settingsCollection;
                };
            ShimMenuItemCollection.AllInstances.RemoveMenuItem = (collection, menuItem) =>
            {
                _removedMenuItemText = menuItem?.Text;
            };
        }

        private void SetPageControls_Menu_MenuItemDataBoun()
        {
            _siteMapSecondLevel = new SiteMapDataSource();
            _communicatorPrivateObject.SetField("SiteMapSecondLevel", BindingFlags.Instance | BindingFlags.NonPublic, _siteMapSecondLevel);
            _menu = new Menu();
            _communicatorPrivateObject.SetField("Menu", BindingFlags.Instance | BindingFlags.NonPublic, _menu);
        }
    }
}
