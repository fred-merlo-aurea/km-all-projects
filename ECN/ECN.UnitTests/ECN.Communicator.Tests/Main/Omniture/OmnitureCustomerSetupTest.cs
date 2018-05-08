using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using AjaxControlToolkit;
using AjaxControlToolkit.Fakes;
using ECN.Communicator.Tests.Helpers;
using ecn.communicator.main.Omniture.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Application;
using ECN_Framework_Common.Objects;
using ECNBusiness = ECN_Framework_BusinessLayer.Application;
using ECNEntities = ECN_Framework_Entities.Communicator;
using ECNMasterPages = ecn.communicator.MasterPages;
using OmnitureCustomerSetup = ecn.communicator.main.Omniture.OmnitureCustomerSetup;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.Omniture
{
	/// <summary>
	///     Unit Tests for <see cref="ecn.communicator.main.Omniture.OmnitureCustomerSetup"/>
	/// </summary>
	[TestFixture]
	public class OmnitureCustomerSetupTest
	{
		private IDisposable _context;
		private Button _btnSaveSettings;
		private EventArgs _eventArgs;
		private OmnitureCustomerSetup _omnitureCstStp;
		private Type _omnitureCstStpType;
		private TextBox _txtQueryName;
		private TextBox _txtDelimiter;
		private Label _lblErrorMessage;
		private PlaceHolder _phError;
		private Label _lblTemplateMessage;
		private CheckBox _chkboxOverride;
		private Label _lblOmniture1;
		private RadioButtonList _rblReqOmni1;
		private DropDownList _ddlOmniDefault1;
		private RadioButtonList _rblCustomOmni1;

		[SetUp]
		public void SetUp()
		{
			_context = ShimsContext.Create();
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

		[TestCase("", "", "Please enter a character for the delimiter and a query string")]
		[TestCase("", "testDelimiter", "Please enter a query string")]
		[TestCase("testQuery", "", "Please enter a character for the delimiter")]
		[TestCase(null, null, "Please enter a character for the delimiter and a query string")]
		[TestCase(null, "testDelimiter", "Please enter a query string")]
		[TestCase("testQuery", null, "Please enter a character for the delimiter")]
		public void btnSaveSettings_Click_WhenQueryNameOrDelimiterAreEmptyOR_DisplaysErrorMessage(string queryTxt, string delimeterTxt, string errMsg)
		{
			// Arrange
			Initialize();
			_txtQueryName.Text = queryTxt;
			_txtDelimiter.Text = delimeterTxt;

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			_phError.Visible.ShouldBeTrue();
			_lblErrorMessage.Text.ShouldNotBeNullOrWhiteSpace();
			_lblErrorMessage.Text.ShouldBe(errMsg);

		}

		[Test]
		public void btnSaveSettings_Click_WhenSettingsAreNotChanged_DisplaysTemplateMessage()
		{
			// Arrange
			Initialize();
			_btnSaveSettings.ID = "not_btnconfirmtemplate";
			_lblTemplateMessage.Text = "";
			var templateName = "new template";
			var messageShown = false;
			ShimOmnitureCustomerSetup.AllInstances.CheckSettingsChange = item =>
			{
				return false;
			};
			ShimCampaignItemTemplate.GetTemplatesBySetupLevelInt32NullableOfInt32Boolean = (x, y, z) =>
			{
				List<ECNEntities.CampaignItemTemplate> citList = new List<ECNEntities.CampaignItemTemplate>()
				{
					new ECNEntities.CampaignItemTemplate(){ TemplateName = templateName }
				};
				return citList;
			};
			ShimCampaignItemTemplate.ClearOutOmniDataBySetupLevelInt32NullableOfInt32BooleanInt32 = (a, b, c, d) => { };
			ShimModalPopupExtender.AllInstances.Show = (item) => { messageShown = true; };

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			_lblTemplateMessage.Text.ShouldNotBeNullOrWhiteSpace();
			_lblTemplateMessage.Text.ShouldContain("values that will be deleted");
			_lblTemplateMessage.Text.ShouldContain(templateName);
			messageShown.ShouldBeTrue();
		}

		[Test]
		public void btnSaveSettings_Click_WhenBtnConfirmTemplateClicked_HideTemplateMessage()
		{
			// Arrange
			Initialize();
			_btnSaveSettings.ID = "btnconfirmtemplate";
			var messageHidden = false;
			ShimCampaignItemTemplate.ClearOutOmniDataBySetupLevelInt32NullableOfInt32BooleanInt32 = (a, b, c, d) => { };
			ShimModalPopupExtender.AllInstances.Hide = (item) =>
			{
				messageHidden = true;
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			messageHidden.ShouldBeTrue();
		}

		[Test]
		public void btnSaveSettings_Click_WhenCurrentCustomerHasLtsAndQueryIsValid_LtsIsUpdated()
		{
			// Arrange
			Initialize();
			_txtQueryName.Text = "testQuery";
			_btnSaveSettings.ID = "btnconfirmtemplate";
			var ltsUpdated = false;
			ShimLinkTrackingSettings.UpdateLinkTrackingSettings = (x) =>
			{
				ltsUpdated = true;
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			ltsUpdated.ShouldBeTrue();
		}
		[Test]
		public void btnSaveSettings_Click_WhenCurrentCustomerHasLtsAndQueryIsInvalid_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			_txtQueryName.Text = "bid";
			_lblErrorMessage.Text = "";
			_phError.Visible = false;
			var errorMsg = "Please enter a different query string";

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			_lblErrorMessage.Text.ShouldNotBeNullOrWhiteSpace();
			_lblErrorMessage.Text.ShouldBe(errorMsg);
			_phError.Visible.ShouldBeTrue();
		}

		[Test]
		public void btnSaveSettings_Click_WhenCurrentCustomerHasNotLtsAndQueryIsValid_LtsWithCustomerIdIsInserted()
		{
			// Arrange
			Initialize();
			var customerLtsInserted = false;
			ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) =>
			{
				return null;
			};
			ShimLinkTrackingSettings.InsertLinkTrackingSettings = (x) =>
			{
				customerLtsInserted = true;
				return 0;
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			customerLtsInserted.ShouldBeTrue();
		}

		[Test]
		public void btnSaveSettings_Click_WhenCurrentCustomerHasNotLtsAndQueryIsValidAndLtsInsertionThrowsException_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			var customerLtsInserted = false;
			var errorMsg = "problem adding LTS";
			ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) =>
			{
				return null;
			};
			ShimLinkTrackingSettings.InsertLinkTrackingSettings = (x) =>
			{
				List<ECNError> ecnErrorList = new List<ECNError>()
				{
					new ECNError(Enums.Entity.BlastPlans,Enums.Method.Save, errorMsg)
				};
				throw new ECNException(ecnErrorList);
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			customerLtsInserted.ShouldBeFalse();
			_lblErrorMessage.Text.ShouldNotBeNullOrWhiteSpace();
			_lblErrorMessage.Text.ShouldContain(errorMsg);
			_phError.Visible.ShouldBeTrue();
		}

		[Test]
		public void btnSaveSettings_Click_WhenCurrentCustomerHasNotLtsAndQueryIsInvalid_LtsWithCustomerIdIsInserted()
		{
			// Arrange
			Initialize();
			_txtQueryName.Text = "bid";
			_lblErrorMessage.Text = "";
			_phError.Visible = false;
			var errorMsg = "Please enter a different query string";
			ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) =>
			{
				return null;
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			_lblErrorMessage.Text.ShouldNotBeNullOrWhiteSpace();
			_lblErrorMessage.Text.ShouldBe(errorMsg);
			_phError.Visible.ShouldBeTrue();
		}

		[Test]
		public void btnSaveSettings_Click_WhenLtsCannotBeUpdatedForValidQuery_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			_txtQueryName.Text = "testQuery";
			var errorMsg = "test error message";
			ShimLinkTrackingSettings.UpdateLinkTrackingSettings = (x) =>
			{
				List<ECNError> ecnErrorList = new List<ECNError>()
				{
					new ECNError(Enums.Entity.BlastPlans,Enums.Method.Save, errorMsg)
				};
				throw new ECNException(ecnErrorList);
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			_lblErrorMessage.Text.ShouldNotBeNullOrWhiteSpace();
			_lblErrorMessage.Text.ShouldContain(errorMsg);
			_phError.Visible.ShouldBeTrue();
		}

		[TestCase("omniture1")]
		[TestCase("omniture2")]
		[TestCase("omniture3")]
		[TestCase("omniture4")]
		[TestCase("omniture5")]
		[TestCase("omniture6")]
		[TestCase("omniture7")]
		[TestCase("omniture8")]
		[TestCase("omniture9")]
		[TestCase("omniture10")]
		public void btnSaveSettings_Click_WhenLtpListContainsLtpSettingsForCustomerForSpecifiedOmniture_LtpSettingsForSpecifiedOmnitureAreUpdated(string specifiedOmniture)
		{
			// Arrange
			Initialize();
			var omniture = "";
			var LtpSettingsUpdated = false;
			ShimLinkTrackingParamSettings.UpdateLinkTrackingParamSettings = x =>
			{
				omniture = specifiedOmniture;
				LtpSettingsUpdated = true;
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			omniture.ShouldBe(specifiedOmniture);
			LtpSettingsUpdated.ShouldBeTrue();
		}

		[TestCase("omniture1")]
		[TestCase("omniture2")]
		[TestCase("omniture3")]
		[TestCase("omniture4")]
		[TestCase("omniture5")]
		[TestCase("omniture6")]
		[TestCase("omniture7")]
		[TestCase("omniture8")]
		[TestCase("omniture9")]
		[TestCase("omniture10")]
		public void btnSaveSettings_Click_WhenLtpListContainsLtpSettingsForCustomerForSpecifiedOmniture_LtpSettingsForSpecifiedOmnitureAreInserted(string specifiedOmniture)
		{
			// Arrange
			Initialize();
			var omniture = "";
			var LtpSettingsInserted = false;
			ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (x, y) =>
			{
				return null;
			};
			ShimLinkTrackingParamSettings.InsertLinkTrackingParamSettings = x =>
			{
				omniture = specifiedOmniture;
				LtpSettingsInserted = true;
				return 0;
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			omniture.ShouldBe(specifiedOmniture);
			LtpSettingsInserted.ShouldBeTrue();
		}

		[TestCase("ddlOmniDefault1", "2")]
		[TestCase("ddlOmniDefault2", "16")]
		[TestCase("ddlOmniDefault3", "12")]
		[TestCase("ddlOmniDefault4", "11")]
		[TestCase("ddlOmniDefault5", "23")]
		[TestCase("ddlOmniDefault6", "6")]
		[TestCase("ddlOmniDefault7", "55")]
		[TestCase("ddlOmniDefault8", "22")]
		[TestCase("ddlOmniDefault9", "1")]
		[TestCase("ddlOmniDefault10", "9")]
		public void btnSaveSettings_Click_WhenSomeOptionIsSelectedFromOmniDefaultDropDownList_LtpOptionForSpecifiedValueIsRetreivedAndUpdated(string dropDownListName, string choosenValue)
		{
			// Arrange
			Initialize();
			var ltParamOptionUpdated = false;
			var ltParamOptionRetrieved = false;
			var ddlOmniName = "";
			var value = "";
			var _ddlOmniDefault = new DropDownList();
			_ddlOmniDefault.Items.Insert(0, new ListItem()
			{
				Selected = true,
				Text = "dropDownName",
				Enabled = true,
				Value = choosenValue
			});
			SetField(_omnitureCstStp, dropDownListName, _ddlOmniDefault);
			ShimLinkTrackingParamOption.GetByLTPOIDInt32 = (x) =>
			{
				ltParamOptionRetrieved = true;
				ddlOmniName = dropDownListName;
				value = choosenValue;
				return new ECNEntities.LinkTrackingParamOption();
			};
			ShimLinkTrackingParamOption.UpdateLinkTrackingParamOption = (x) =>
			{
				ltParamOptionUpdated = true;
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			ddlOmniName.ShouldBe(dropDownListName);
			ltParamOptionRetrieved.ShouldBeTrue();
			ltParamOptionUpdated.ShouldBeTrue();
			value.ShouldBe(choosenValue);
		}

		[TestCase("ddlOmniDefault1", "-1")]
		[TestCase("ddlOmniDefault2", "-1")]
		[TestCase("ddlOmniDefault3", "-1")]
		[TestCase("ddlOmniDefault4", "-1")]
		[TestCase("ddlOmniDefault5", "-1")]
		[TestCase("ddlOmniDefault6", "-1")]
		[TestCase("ddlOmniDefault7", "-1")]
		[TestCase("ddlOmniDefault8", "-1")]
		[TestCase("ddlOmniDefault9", "-1")]
		[TestCase("ddlOmniDefault10", "-1")]
		public void btnSaveSettings_Click_WhenNoOptionIsSelectedFromOmniDefaultDropDownList_LtpOptionIsResetToDefault(string dropDownListName, string defValue)
		{
			// Arrange
			Initialize();
			var ltParamOptionReset = false;
			var ddlOmniName = "";
			var _ddlOmniDefault = new DropDownList();
			_ddlOmniDefault.Items.Insert(0, new ListItem()
			{
				Selected = true,
				Text = "dropDownName",
				Enabled = true,
				Value = defValue
			});
			SetField(_omnitureCstStp, dropDownListName, _ddlOmniDefault);
			ShimLinkTrackingParamOption.ResetCustDefaultInt32Int32 = (x, y) =>
			{
				ltParamOptionReset = true;
				ddlOmniName = dropDownListName;
			};
			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			ddlOmniName.ShouldBe(dropDownListName);
			ltParamOptionReset.ShouldBeTrue();
		}

		[Test]
		public void btnSaveSettings_Click_WhenExceptionIsThrown_ErrorMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			var errMsg = "test Error";
			var errMessageDisplayed = false;
			ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (x, y) =>
			{
				List<ECNError> ecnErrorList = new List<ECNError>()
				{
					new ECNError(Enums.Entity.Blast, Enums.Method.Get, errMsg)
				};
				throw new ECNException(ecnErrorList) { };
			};
			ShimOmnitureCustomerSetup.AllInstances.ShowMessageStringStringMessageMessage_Icon = (a, b, c, d) =>
			{
				errMessageDisplayed = true;
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			_lblErrorMessage.Text.ShouldNotBeNullOrWhiteSpace();
			_lblErrorMessage.Text.ShouldContain(errMsg);
			_phError.Visible.ShouldBeTrue();
			errMessageDisplayed.ShouldBeTrue();
		}

		[Test]
		public void btnSaveSettings_Click_WhenExecutedSuccessfully_SuccesMessageIsDisplayed()
		{
			// Arrange
			Initialize();
			var successMessageDisplayed = false;
			ShimOmnitureCustomerSetup.AllInstances.ShowMessageStringStringMessageMessage_Icon = (a, b, c, d) =>
			{
				successMessageDisplayed = true;
			};

			// Act
			CallMethod(
				_omnitureCstStpType,
				"btnSaveSettings_Click",
				new object[] { _btnSaveSettings, _eventArgs },
				_omnitureCstStp);

			// Assert
			successMessageDisplayed.ShouldBeTrue();
		}

		private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
		{
			ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
		}

		private dynamic CreateInstance(Type type)
		{
			return ReflectionHelper.CreateInstance(type);
		}

		private void CreateShims()
		{
			ShimAuthenticationTicket.getTicket = () =>
			{
				AuthenticationTicket authTkt = CreateInstance(typeof(AuthenticationTicket));
				SetField(authTkt, "CustomerID", 1);
				return authTkt;
			};
			ShimECNSession.AllInstances.RefreshSession = (item) => { };
			ShimECNSession.AllInstances.ClearSession = (itme) => { };
			ShimECNSession.CurrentSession = () =>
			{
				ECNBusiness.ECNSession ecnSession = CreateInstance(typeof(ECNBusiness.ECNSession));
				SetField(ecnSession, "CustomerID", 1);
				SetField(ecnSession, "BaseChannelID", 1);
				return ecnSession;
			};
			ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) =>
			{
				ECNEntities.LinkTrackingSettings lts = CreateInstance(typeof(ECNEntities.LinkTrackingSettings));
				return lts;
			};
			ShimOmnitureCustomerSetup.AllInstances.CheckSettingsChange = item =>
			{
				return false;
			};
			ShimOmnitureCustomerSetup.AllInstances.MasterGet = item =>
			{
				return CreateInstance(typeof(ECNMasterPages.Communicator));//new ecn.communicator.MasterPages.Communicator() { };
			};
			ShimCampaignItemTemplate.GetTemplatesBySetupLevelInt32NullableOfInt32Boolean = (x, y, z) =>
			{
				var cit = CreateInstance(typeof(ECNEntities.CampaignItemTemplate));
				SetField(cit, "TemplateName", "newTemplate");
				List<ECNEntities.CampaignItemTemplate> citList = new List<ECNEntities.CampaignItemTemplate>()
				{
					cit
				};
				return citList;
			};
			ShimCampaignItemTemplate.ClearOutOmniDataBySetupLevelInt32NullableOfInt32BooleanInt32 = (a, b, c, d) => { };
			ShimModalPopupExtender.AllInstances.Show = (item) => { };
			ShimModalPopupExtender.AllInstances.Hide = (item) => { };
			ShimLinkTrackingSettings.UpdateLinkTrackingSettings = x => { };
			ShimLinkTrackingSettings.InsertLinkTrackingSettings = (x) => { return 0; };
			ShimLinkTrackingParam.GetByLinkTrackingIDInt32 = x =>
			{
				List<ECNEntities.LinkTrackingParam> listLTP = new List<ECNEntities.LinkTrackingParam>()
				{
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture1",
						LTPID =1
					},
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture2",
						LTPID =1
					},
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture3",
						LTPID =1
					},
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture4",
						LTPID =1
					},
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture5",
						LTPID =1
					},
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture6",
						LTPID =1
					},
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture7",
						LTPID =1
					},
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture8",
						LTPID =1
					},
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture9",
						LTPID =1
					},
					new ECNEntities.LinkTrackingParam()
					{
						DisplayName ="omniture10",
						LTPID =1
					}
				};
				return listLTP;
			};
			ShimLinkTrackingParamSettings.Get_LTPID_CustomerIDInt32Int32 = (x, y) =>
			{
				var ltps = CreateInstance(typeof(ECNEntities.LinkTrackingParamSettings));
				SetField(ltps, "LTPSID", 1);
				return ltps;
			};
			ShimLinkTrackingParamSettings.UpdateLinkTrackingParamSettings = x => { };
			ShimLinkTrackingParamSettings.InsertLinkTrackingParamSettings = x => { return 0; };
			ShimLinkTrackingParamOption.GetByLTPOIDInt32 = (x) =>
			{
				return CreateInstance(typeof(ECNEntities.LinkTrackingParamOption));
			};
			ShimLinkTrackingParamOption.UpdateLinkTrackingParamOption = (x) => { };
			ShimLinkTrackingParamOption.ResetCustDefaultInt32Int32 = (x, y) => { };
			ShimOmnitureCustomerSetup.AllInstances.ShowMessageStringStringMessageMessage_Icon = (a, b, c, d) => { };
		}

		private void Initialize()
		{
			CreateShims();
			_btnSaveSettings = new Button();
			_eventArgs = new EventArgs();
			_txtDelimiter = new TextBox();
			_txtQueryName = new TextBox();
			_lblErrorMessage = new Label();
			_phError = new PlaceHolder();
			_omnitureCstStpType = Type.GetType("ecn.communicator.main.Omniture.OmnitureCustomerSetup,ecn.communicator");
			_omnitureCstStp = ReflectionHelper.CreateInstance(_omnitureCstStpType);
			_lblTemplateMessage = new Label();
			_chkboxOverride = new CheckBox();

			_lblOmniture1 = new Label();
			_rblReqOmni1 = new RadioButtonList()
			{
				SelectedValue = "1"
			};

			_rblCustomOmni1 = new RadioButtonList()
			{
				SelectedValue = "1"
			};
			_ddlOmniDefault1 = new DropDownList();
			_ddlOmniDefault1.Items.Insert(0, new ListItem()
			{
				Selected = true,
				Text = "1",
				Enabled = true,
				Value = "1"
			});
			SetDefaults();
		}

		private void SetDefaults()
		{
			_btnSaveSettings.ID = "btnconfirmtemplate";
			_txtQueryName.Text = "testQuery";
			_txtDelimiter.Text = ",";
			SetField(_omnitureCstStp, "txtDelimiter", _txtDelimiter);
			SetField(_omnitureCstStp, "txtQueryName", _txtQueryName);
			SetField(_omnitureCstStp, "lblErrorMessage", _lblErrorMessage);
			SetField(_omnitureCstStp, "phError", _phError);
			SetField(_omnitureCstStp, "lblTemplateMessage", _lblTemplateMessage);
			SetField(_omnitureCstStp, "mpeTemplateNotif", new ModalPopupExtender());
			SetField(_omnitureCstStp, "chkboxOverride", _chkboxOverride);
			for (int i = 1; i < 11; i++)
			{
				var fieldName = "rblReqOmni" + i.ToString();
				SetField(_omnitureCstStp, fieldName, _rblReqOmni1);
				fieldName = "lblOmniture" + i.ToString();
				SetField(_omnitureCstStp, fieldName, _lblOmniture1);
				fieldName = "rblCustomOmni" + i.ToString();
				SetField(_omnitureCstStp, fieldName, _rblCustomOmni1);
				fieldName = "ddlOmniDefault" + i.ToString();
				SetField(_omnitureCstStp, fieldName, _ddlOmniDefault1);
			}
		}

		private void SetField(dynamic obj, string fieldName, dynamic value)
		{
			ReflectionHelper.SetField(obj, fieldName, value);
		}
	}
}
