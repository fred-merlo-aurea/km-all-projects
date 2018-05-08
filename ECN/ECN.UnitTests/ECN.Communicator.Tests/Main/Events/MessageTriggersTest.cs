using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI.WebControls;
using ecn.communicator.main.events;
using ecn.communicator.main.events.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Events
{
    /// <summary>
	///     Unit tests for <see cref="ecn.communicator.main.events.MessageTriggers"/>
	/// </summary>
	[TestFixture, ExcludeFromCodeCoverage]
    public partial class MessageTriggersTest
    {
        private IDisposable _shimContext;
        private PrivateObject _messageTriggersPrivateObject;
        private MessageTriggers _messageTriggersInstance;
        private ShimMessageTriggers _shimMessageTriggers;
        private int _layoutPlanSaveMethodCallCount;
        private int _triggerPlanSaveMethodCallCount;
        private int _triggerPlanDeleteMethodCallCount;
        private Label _lblErrorMessage;
        private PlaceHolder _phError;

        [SetUp]
        public void Setup()
        {
            _layoutPlanSaveMethodCallCount = 0;
            _triggerPlanSaveMethodCallCount= 0;
            _triggerPlanDeleteMethodCallCount=0;
            _shimContext = ShimsContext.Create();
            _messageTriggersInstance = new MessageTriggers();
            _shimMessageTriggers = new ShimMessageTriggers(_messageTriggersInstance);
            _messageTriggersPrivateObject = new PrivateObject(_messageTriggersInstance);
            InitShims();
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext.Dispose();
        }

        private void InitShims()
        {
            ShimLayoutPlans.SaveLayoutPlansUser = (l, user) => _layoutPlanSaveMethodCallCount++;
            ShimTriggerPlans.DeleteInt32User = (t, u) => _triggerPlanDeleteMethodCallCount++;
            _lblErrorMessage = new Label();
            _phError = new PlaceHolder();
            _phError.Visible = false;
            _messageTriggersPrivateObject.SetField("lblErrorMessage", BindingFlags.Instance | BindingFlags.NonPublic, _lblErrorMessage);
            _messageTriggersPrivateObject.SetField("phError", BindingFlags.Instance | BindingFlags.NonPublic, _phError);
        }
    }
}
