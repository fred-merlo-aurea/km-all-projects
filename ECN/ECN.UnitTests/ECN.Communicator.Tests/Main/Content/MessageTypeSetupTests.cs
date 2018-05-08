using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECN.Tests.Helpers;
using ecn.communicator.contentmanager;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using MasterPage = ecn.communicator.MasterPages;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Content
{
    [TestFixture]
    public class MessageTypeSetupTests : BasePageTests
    {
        private const string MasterPageIdName = "_master";
        private const string ViewStateName = "_viewState";
        private const string ChannelIdName = "channelID";
        private const string ItemInsertingMethodName = "dvMessageTypes_ItemInserting";
        private const string ItemUpdatingMethodName = "dvMessageTypes_ItemUpdating";

        private MessageTypeSetup _page;
        private DetailsView _dvMessageTypes;
        private IDisposable _shimsContext;
        private FakeHttpContext.FakeHttpContext _fakeHttpContext;

        [SetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(DefaultCulture);
            _shimsContext = ShimsContext.Create();
            _fakeHttpContext = new FakeHttpContext.FakeHttpContext();

            _page = new MessageTypeSetup();

            InitializePage(_page);
            RetrievePageControls();
            SetCurrentUser();

            ReflectionHelper.SetValue(_page, typeof(Page), MasterPageIdName, new MasterPage.Communicator());
            ReflectionHelper.SetValue(_page, typeof(MessageTypeSetup), ChannelIdName, 1);            
        }

        [TearDown]
        public void TearDown()
        {
            ShimsContext.Reset();
            _shimsContext.Dispose();
            _fakeHttpContext.Dispose();
        }

        [Test]
        public void DvMessageTypesItemInserting_WithPriority_SavesMessageType()
        {
            // Arrange
            var messageType = CreateMessageType(true);
            var messageTypeList = new List<MessageType> {messageType};
            MessageType actualResult = null;
            var getMaxSortOrderCalled = false;

            InitDvMessageTypes(messageTypeList);
            InitContext(messageType, messageTypeList);
            ShimMessageType.SaveMessageTypeUser = (type, user) =>
            {
                actualResult = type;
                return 1;
            };
            ShimMessageType.GetMaxSortOrderInt32 = i =>
            {
                getMaxSortOrderCalled = true;
                return 1;
            };

            // Act
            ReflectionHelper.ExecuteMethod(_page, ItemInsertingMethodName, new object[] { _dvMessageTypes, null});

            // Assert
            getMaxSortOrderCalled.ShouldBe(true);

            actualResult.ShouldSatisfyAllConditions(
              () => actualResult.Name.ShouldBe(messageType.Name),
              () => actualResult.Description.ShouldBe(messageType.Description),
              () => actualResult.IsActive.ShouldBe(messageType.IsActive),
              () => actualResult.Threshold.ShouldBe(messageType.Threshold),
              () => actualResult.Priority.ShouldBe(messageType.Priority)
            );
        }

        [Test]
        public void DvMessageTypesItemInserting_WithoutPriority_SavesMessageType()
        {
            // Arrange
            var messageType = CreateMessageType(false);
            MessageType actualResult = null;
            var messageTypeList = new List<MessageType> { messageType };
            
            InitDvMessageTypes(messageTypeList);
            InitContext(messageType, messageTypeList);
            ShimMessageType.SaveMessageTypeUser = (type, user) =>
            {
                actualResult = type;
                return 1;
            };

            // Act
            ReflectionHelper.ExecuteMethod(_page, ItemInsertingMethodName, new object[] { _dvMessageTypes, null });

            // Assert
            actualResult.ShouldSatisfyAllConditions(
             () => actualResult.Name.ShouldBe(messageType.Name),
             () => actualResult.Description.ShouldBe(messageType.Description),
             () => actualResult.IsActive.ShouldBe(messageType.IsActive),
             () => actualResult.Threshold.ShouldBe(messageType.Threshold),
             () => actualResult.Priority.ShouldBe(messageType.Priority)
           );
        }

        [Test]
        public void DvMessageTypesItemUpdating_WithPriority_SavesMessageType()
        {
            // Arrange
            var messageType = CreateMessageType(true);
            var messageTypeList = new List<MessageType> { messageType };
            MessageType actualResult = null;
            var getMaxSortOrderCalled = false;

            InitDvMessageTypes(messageTypeList);
            InitContext(messageType, messageTypeList);
            ShimMessageType.SaveMessageTypeUser = (type, user) =>
            {
                actualResult = type;
                return 1;
            };
            ShimMessageType.GetMaxSortOrderInt32 = i =>
            {
                getMaxSortOrderCalled = true;
                return 1;
            };

            // Act
            ReflectionHelper.ExecuteMethod(_page, ItemUpdatingMethodName, new object[] { _dvMessageTypes, null });

            // Assert
            getMaxSortOrderCalled.ShouldBe(true);

            actualResult.ShouldSatisfyAllConditions(
             () => actualResult.Name.ShouldBe(messageType.Name),
             () => actualResult.Description.ShouldBe(messageType.Description),
             () => actualResult.IsActive.ShouldBe(messageType.IsActive),
             () => actualResult.Threshold.ShouldBe(messageType.Threshold),
             () => actualResult.Priority.ShouldBe(messageType.Priority)
           );
        }

        [Test]
        public void DvMessageTypesItemUpdating_WithoutPriority_SavesMessageType()
        {
            // Arrange
            var messageType = CreateMessageType(false);
            MessageType actualResult = null;
            var messageTypeList = new List<MessageType> { messageType };

            InitDvMessageTypes(messageTypeList);
            InitContext(messageType, messageTypeList);
            ShimMessageType.SaveMessageTypeUser = (type, user) =>
            {
                actualResult = type;
                return 1;
            };

            // Act
            ReflectionHelper.ExecuteMethod(_page, ItemUpdatingMethodName, new object[] { _dvMessageTypes, null });

            // Assert
            actualResult.ShouldSatisfyAllConditions(
             () => actualResult.Name.ShouldBe(messageType.Name),
             () => actualResult.Description.ShouldBe(messageType.Description),
             () => actualResult.IsActive.ShouldBe(messageType.IsActive),
             () => actualResult.Threshold.ShouldBe(messageType.Threshold),
             () => actualResult.Priority.ShouldBe(messageType.Priority)
           );
        }

        private void RetrievePageControls()
        {
            const string dvMessageTypesControlName = "dvMessageTypes";
            _dvMessageTypes = ReflectionHelper.GetValue<DetailsView>(_page, dvMessageTypesControlName);
        }

        private void SetCurrentUser()
        {
            const string authData = "1,1,1,1,0D5D37DB-ED27-456A-8C0B-062F9F37983B";

            ShimECNSession.AllInstances.RefreshSession = session => { session.CurrentUser = CurrentUserMock; };

            var formsAuthenticationTicket = new FormsAuthenticationTicket(1, CurrentUserMock.UserID.ToString(),
                DateTime.Now, DateTime.MaxValue, true, authData);
            var formsIdentity = new FormsIdentity(formsAuthenticationTicket);

            HttpContext.Current.User = new GenericPrincipal(formsIdentity, null);
        }

        private static MessageType CreateMessageType(bool priority)
        {
            const string name = "name1";
            const string description = "description1";

            return new MessageType()
            {
                Name = name,
                Description = description,
                IsActive = true,
                Threshold = true,
                Priority = priority,
                MessageTypeID = 1
            };
        }

        private void InitDvMessageTypes(IList<MessageType> dataSource)
        {
            const string dvMessagesTypeTxtName = "txtName";
            const string dvMessagesTypeTxtNameBindingName = "Name";
            const string dvMessagesTypeTxtDescription = "txtDescription";
            const string dvMessagesTypeTxtDescriptionBindingName = "Description";
            const string dvMessagesTypeDdlThreshold = "ddlThreshold";
            const string dvMessagesTypeDdlThresholdBindingName = "Threshold";
            const string dvMessagesTypeDdlPriority = "ddlPriority";
            const string dvMessagesTypeDdlPriorityBindingName = "Priority";
            const string dvMessagesTypeDdlIsActive = "ddlIsActive";
            const string dvMessagesTypeDdlIsActiveBindingName = "IsActive";

            _dvMessageTypes.ChangeMode(DetailsViewMode.Edit);
            
            var txtName = CreateTextBox(dvMessagesTypeTxtName, dvMessagesTypeTxtNameBindingName);
            var txtNameField = CreateTemplateField(txtName);
            _dvMessageTypes.Fields.Add(txtNameField);

            var txtDescription = CreateTextBox(dvMessagesTypeTxtDescription, dvMessagesTypeTxtDescriptionBindingName);
            var txtDescriptionField = CreateTemplateField(txtDescription);
            _dvMessageTypes.Fields.Add(txtDescriptionField);

            var dropDownDataSource = new List<bool>
            {
                false,
                true
            };
            
            var ddlThreshold = CreateDropdownList(dvMessagesTypeDdlThreshold, 
                dvMessagesTypeDdlThresholdBindingName, dropDownDataSource);
            var ddlThresholdField = CreateTemplateField(ddlThreshold);
            _dvMessageTypes.Fields.Add(ddlThresholdField);
            
            var ddlPriority = CreateDropdownList(dvMessagesTypeDdlPriority, 
                dvMessagesTypeDdlPriorityBindingName, dropDownDataSource);
            var ddlPriorityField = CreateTemplateField(ddlPriority);
            _dvMessageTypes.Fields.Add(ddlPriorityField);

            
            var ddlIsActive = CreateDropdownList(dvMessagesTypeDdlIsActive, 
                dvMessagesTypeDdlIsActiveBindingName, dropDownDataSource);
            var ddlIsActiveField = CreateTemplateField(ddlIsActive);
            _dvMessageTypes.Fields.Add(ddlIsActiveField);

            _dvMessageTypes.DataKeyNames = new[] { nameof(MessageType.MessageTypeID) };
            _dvMessageTypes.DataSource = dataSource.ToList();
            _dvMessageTypes.DataBind();
        }

        private TemplateField CreateTemplateField(Control control)
        {
            var templateView = new TemplateView(container =>
            {
                container.Controls.Add(control);
            });

            var templateField = new TemplateField();
            templateField.EditItemTemplate = templateView;

            return templateField;
        }

        private TextBox CreateTextBox(string name, string dataSourceBindingName)
        {
            var textBox = new TextBox();
            textBox.ID = name;
            textBox.DataBinding += delegate (object sender, EventArgs e)
            {
                var control = sender as TextBox;
                if (control == null)
                {
                    return;
                }
                var controlContainer = control.NamingContainer as DetailsView;
                if (controlContainer != null && controlContainer.DataItem != null)
                {
                    control.Text = DataBinder.Eval(controlContainer.DataItem, dataSourceBindingName).ToString();
                }
            };

            return textBox;
        }

        private DropDownList CreateDropdownList(string name, string dataSourceBindingName, IList<bool> dataSource)
        {
            var dropdown = new DropDownList();
            dropdown.ID = name;
            dropdown.DataBinding += delegate (object sender, EventArgs e)
            {
                var control = sender as DropDownList;
                if (control == null)
                {
                    return;
                }

                var controlContainer = control.NamingContainer as DetailsView;
                if (controlContainer != null && controlContainer.DataItem != null)
                {
                    control.SelectedValue = DataBinder.Eval(controlContainer.DataItem, dataSourceBindingName).ToString();
                }
            };
            foreach (var dataItem in dataSource)
            {
                dropdown.Items.Add(new ListItem(dataItem.ToString(), dataItem.ToString()));
            }

            return dropdown;
        }

        private void InitContext(MessageType messageType, IList<MessageType> messageTypeList)
        {
            const string sortFieldName = "mtSortField";
            const string sortFieldValue = "description";
            const string sortDirectionFieldName = "mtSortDirection";
            const string sortDirectionFieldValue = "DESC";

            ShimMessageType.GetByBaseChannelIDInt32User = (i, user) => messageTypeList.ToList();
            ShimMessageType.GetByMessageTypeIDInt32User = (i, user) => messageType;
            ShimLayout.MessageTypeUsedInLayoutInt32 = i => false;
            var stateBag = new StateBag
            {
                {
                    sortFieldName,
                    sortFieldValue
                },
                {
                    sortDirectionFieldName,
                    sortDirectionFieldValue
                }
            };
            ReflectionHelper.SetValue(_page, typeof(Control), ViewStateName, stateBag);
        }
    }
}
