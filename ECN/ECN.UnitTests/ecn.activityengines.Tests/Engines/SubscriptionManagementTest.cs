using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.Fakes;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.activityengines.engines;
using ecn.activityengines.engines.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using BusinessAccountsFakes = ECN_Framework_BusinessLayer.Accounts.Fakes;
using EntityAccounts = ECN_Framework_Entities.Accounts;

namespace ecn.activityengines.Tests.Engines
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.activityengines.engines.SubscriptionManagement"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class SubscriptionManagementTest
    {
        private const string AppSettingsEngineAccessKey = "ECNEngineAccessKey";
        private const string AppSettingsEngineAccessKeyValue = "ECNEngineAccessKeyValue";
        private const string GVCurrentRBSubId = "rbSubscribed";
        private const string GVAvailableChkSubId = "chkSubscribe";
        private const string GV_CurrentHFInitialId = "hfInitial";
        private const string DropDownReasonId = "ddlReason";
        private const string LabelReasonErrorId = "lblReasonError";
        private const string LabelReasonMessageId = "lblReasonMessage";
        private const string TextReasonId = "txtReason";
        private const string CheckBoxSendResponseId = "chkSendResponse";
        private const string ThankYouLabel = "thankYouLable";
        private const string PanelContentId = "pnlContent";
        private const string PanelThankYouId = "pnlThankYou";
        private const string LabelErrorMessageId = "lblErrorMessage";
        private const string PlaceHolderErrorId = "phError";
        private const string GridViewCurrentId = "gvCurrent";
        private const string GridViewAvailableId = "gvAvailable";
        private const string LabelThankYouHeadingId = "lblThankYouHeading";
        private const string UnsubscribedRadioButtonId = "rbUnsubscribed";
        private const string SubscribedRadioButtonId = "rbSubscribed";
        private const string InitialHiddenFieldId = "hfInitial";
        private const string ValueS = "S";
        private const string ValueOther = "other";

        private Panel _panelContent;
        private Panel _panelThankYou;
        private GridView _gvCurrent;
        private GridView _gvAvailable;
        private Label _lblReasonError;
        private Label _lblThankYouHeading;
        private CheckBox _gvCheckboxAvailableSubscribe;
        private CheckBox _chkBoxSendResponse;
        private HiddenField _gvCurrentHFInitialId;
        private DropDownList _ddlReason;
        private TextBox _txtReason;
        private RadioButton _gvCurrentRBSubscribe;
        private IDisposable _shimContext;
        private PrivateObject _subscriptionManagementPrivateObject;
        private SubscriptionManagement _subscriptionManagementInstance;
        private ShimSubscriptionManagement _shimSubscriptionManagement;
        private PlaceHolder _phError;
        private Label _lblErrorMessage;
        private Literal _lblReasonMessage;
        private RadioButton _rbSubscribed;
        private RadioButton _rbUnsubscribed;
        private HiddenField _hiddenField;
        private GridViewRow _gridViewRow;
        private int _btc_EmailDirectSaveMethodCallCount;
        private bool _btc_ResponseRedirectCalled;
        private int _logNonCriticalErrorMethodCallCount;
        private int _logCriticalErrorMethodCallCount;

        [SetUp]
        public void Setup()
        {
            _logNonCriticalErrorMethodCallCount = 0;
            _logCriticalErrorMethodCallCount = 0;
            _btc_ResponseRedirectCalled = false;
            _btc_EmailDirectSaveMethodCallCount = 0;
            _shimContext = ShimsContext.Create();
            _subscriptionManagementInstance = new SubscriptionManagement();
            _shimSubscriptionManagement = new ShimSubscriptionManagement(_subscriptionManagementInstance);
            _subscriptionManagementPrivateObject = new PrivateObject(_subscriptionManagementInstance);
            InitCommonControls();
        }

        [TearDown]
        public void TearDown()
        {
            _phError?.Dispose();
            _lblErrorMessage?.Dispose();
            _panelContent?.Dispose();
            _panelThankYou?.Dispose();
            _gvCurrent?.Dispose();
            _gvAvailable?.Dispose();
            _lblThankYouHeading?.Dispose();
            _gvCurrentRBSubscribe?.Dispose();
            _gvCheckboxAvailableSubscribe?.Dispose();
            _gvCurrentHFInitialId?.Dispose();
            _ddlReason?.Dispose();
            _txtReason?.Dispose();
            _lblReasonError?.Dispose();
            _lblReasonMessage?.Dispose();
            _rbSubscribed?.Dispose();
            _rbUnsubscribed?.Dispose();
            _hiddenField?.Dispose();
            _gridViewRow?.Dispose();
            _shimContext.Dispose();
        }

        private void InitCommonControls()
        {
            _lblErrorMessage = InitField<Label>(LabelErrorMessageId);
            _phError = InitField<PlaceHolder>(PlaceHolderErrorId);
            if (_phError != null)
            {
                _phError.Visible = false;
            }
        }

        private T Get<T>(string fieldName)
        {
            var val = (T)_subscriptionManagementPrivateObject.GetFieldOrProperty(fieldName);
            return val;
        }

        private void Set(string fieldName, object fieldValue)
        {
            _subscriptionManagementPrivateObject.SetFieldOrProperty(fieldName, fieldValue);
        }

        private T InitField<T>(string fieldName, object fieldValue = null, bool createInstance = true) where T : new()
        {
            var obj = createInstance
                ? new T()
                : fieldValue;
            Set(fieldName, obj);
            return Get<T>(fieldName);
        }

        private List<T> CopyList<T>(List<T> source)
        {
            if (source == null)
            {
                return new List<T>();
            }
            var arr = new T[source.Count];
            source.CopyTo(arr);
            return arr.ToList();
        }

        private void InitializeAllControls(object page)
        {
            var fields = page.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(page) == null)
                {
                    var constructor = field.FieldType.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var obj = constructor.Invoke(new object[0]);
                        if (obj != null)
                        {
                            field.SetValue(page, obj);
                            TryLinkFieldWithPage(obj, page);
                        }
                    }
                }
            }
        }

        private void TryLinkFieldWithPage(object field, object page)
        {
            if (page is Page)
            {
                var fieldType = field.GetType().GetField("_page", BindingFlags.Public |
                                                                  BindingFlags.NonPublic |
                                                                  BindingFlags.Static |
                                                                  BindingFlags.Instance);
                if (fieldType != null)
                {
                    try
                    {
                        fieldType.SetValue(field, page);
                    }
                    catch (Exception ex)
                    {
                        // ignored
                        Trace.TraceError($"Unable to set value as :{ex}");
                    }
                }
            }
        }

        private void InitializeControlsForCheckedChanged()
        {
            _rbSubscribed = new RadioButton();
            _rbUnsubscribed = new RadioButton();
            _hiddenField = new HiddenField();
            _gridViewRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
            _gvCurrent = InitField<GridView>(GridViewCurrentId);
            _ddlReason = InitField<DropDownList>(DropDownReasonId);
            _lblReasonMessage = InitField<Literal>(LabelReasonMessageId);
            _txtReason = InitField<TextBox>(TextReasonId);
        }

        private void SetShimForCheckedChanged(EntityAccounts.SubscriptionManagement subscriptionManagement)
        {
            ShimControl.AllInstances.ParentGet = control => _gridViewRow;

            var rbUnsubscribedCounter = 0;
            var rbSubscribedCounter = 0;
            ShimControl.AllInstances.FindControlString = (control, id) =>
            {
                if (control is GridViewRow)
                {
                    if (id == UnsubscribedRadioButtonId)
                    {
                        if (rbUnsubscribedCounter == 1 || rbSubscribedCounter == 1)
                        {
                            _rbUnsubscribed.Checked = true;
                        }

                        rbUnsubscribedCounter++;
                        return _rbUnsubscribed;
                    }
                    if (id == SubscribedRadioButtonId)
                    {
                        rbSubscribedCounter++;
                        return _rbUnsubscribed;
                    }
                    else if (id == InitialHiddenFieldId)
                    {
                        return _hiddenField;
                    }
                }
                return ShimsContext.ExecuteWithoutShims(() => control.FindControl(id));
            };

            var arrayList = new ArrayList { _gridViewRow };
            var rowCollection = new GridViewRowCollection(arrayList);
            ShimGridView.AllInstances.RowsGet = view => rowCollection;

            BusinessAccountsFakes.ShimSubscriptionManagement.GetBySubscriptionManagementIDInt32 =
                i => subscriptionManagement;
        }
    }
}