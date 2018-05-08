using System;
using System.Web.UI.WebControls;
using ECN_Framework_Entities.Accounts;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests.Engines
{
    public partial class SubscriptionManagementTest
    {
        private const string UnsubscribedCheckedChangedMethodName = "rbUnsubscribed_CheckedChanged";
        private const string SubscribedCheckedChangedMethodName = "rbSubscribed_CheckedChanged";

        [TestCase(UnsubscribedCheckedChangedMethodName)]
        [TestCase(SubscribedCheckedChangedMethodName)]
        public void RbUnsubscribedCheckedChanged_HiddenfieldValueBlank_ControlVisibilitySet(string methodName)
        {
            // Arrange
            InitializeControlsForCheckedChanged();
            SetShimForCheckedChanged(null);

            // Act
            var result = _subscriptionManagementPrivateObject.Invoke(
                methodName, 
                new object[] { _rbSubscribed, EventArgs.Empty });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _ddlReason.Visible.ShouldBeFalse(),
                () => _txtReason.Visible.ShouldBeFalse(),
                () => _lblReasonMessage.Visible.ShouldBeFalse()
            );
        }

        [TestCase(UnsubscribedCheckedChangedMethodName)]
        [TestCase(SubscribedCheckedChangedMethodName)]
        public void RbUnsubscribedCheckedChanged_SubscriptionManagementNull_ControlVisibilitySet(string methodName)
        {
            // Arrange
            InitializeControlsForCheckedChanged();
            _hiddenField.Value = ValueS;
            SetShimForCheckedChanged(null);

            // Act
            var result = _subscriptionManagementPrivateObject.Invoke(
                methodName,
                new object[] { _rbSubscribed, EventArgs.Empty });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _ddlReason.Visible.ShouldBeFalse(),
                () => _txtReason.Visible.ShouldBeFalse(),
                () => _lblReasonMessage.Visible.ShouldBeFalse()
            );
        }

        [TestCase(UnsubscribedCheckedChangedMethodName)]
        [TestCase(SubscribedCheckedChangedMethodName)]
        public void RbUnsubscribedCheckedChanged_ReasonVisibleTrue_ControlVisibilitySet(string methodName)
        {
            // Arrange
            InitializeControlsForCheckedChanged();
            _hiddenField.Value = ValueS;
            var subscriptionManagement = new SubscriptionManagement
            {
                ReasonVisible = true
            };
            SetShimForCheckedChanged(subscriptionManagement);

            // Act
            var result = _subscriptionManagementPrivateObject.Invoke(
                methodName,
                new object[] { _rbSubscribed, EventArgs.Empty });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _ddlReason.Visible.ShouldBeFalse(),
                () => _txtReason.Visible.ShouldBeTrue(),
                () => _lblReasonMessage.Visible.ShouldBeTrue()
            );
        }

        [TestCase(UnsubscribedCheckedChangedMethodName)]
        [TestCase(SubscribedCheckedChangedMethodName)]
        public void RbUnsubscribedCheckedChanged_UseReasonDropDownTrue_ControlVisibilitySet(string methodName)
        {
            // Arrange
            InitializeControlsForCheckedChanged();
            _hiddenField.Value = ValueS;
            var subscriptionManagement = new SubscriptionManagement
            {
                ReasonVisible = true,
                UseReasonDropDown = true
            };
            SetShimForCheckedChanged(subscriptionManagement);

            // Act
            var result = _subscriptionManagementPrivateObject.Invoke(
                methodName,
                new object[] { _rbSubscribed, EventArgs.Empty });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _ddlReason.Visible.ShouldBeTrue(),
                () => _lblReasonMessage.Visible.ShouldBeTrue(),
                () => _txtReason.Visible.ShouldBeFalse(),
                () => _txtReason.Text.ShouldBeEmpty()
            );
        }

        [TestCase(UnsubscribedCheckedChangedMethodName)]
        [TestCase(SubscribedCheckedChangedMethodName)]
        public void RbUnsubscribedCheckedChanged_DropdownReasonValueOther_ControlVisibilitySet(string methodName)
        {
            // Arrange
            InitializeControlsForCheckedChanged();
            _hiddenField.Value = ValueS;
            var subscriptionManagement = new SubscriptionManagement
            {
                ReasonVisible = true,
                UseReasonDropDown = true
            };
            _ddlReason.Items.Add(new ListItem(ValueOther, ValueOther));
            _ddlReason.SelectedIndex = 0;
            SetShimForCheckedChanged(subscriptionManagement);

            // Act
            var result = _subscriptionManagementPrivateObject.Invoke(
                methodName,
                new object[] { _rbSubscribed, EventArgs.Empty });

            // Assert
            result.ShouldSatisfyAllConditions(
                () => _ddlReason.Visible.ShouldBeTrue(),
                () => _lblReasonMessage.Visible.ShouldBeTrue(),
                () => _txtReason.Visible.ShouldBeTrue()
            );
        }
    }
}