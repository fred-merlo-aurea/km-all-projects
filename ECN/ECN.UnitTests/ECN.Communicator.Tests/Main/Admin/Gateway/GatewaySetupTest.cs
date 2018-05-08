using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI.WebControls;
using ecn.communicator.main.admin.Gateway;
using ecn.communicator.main.admin.Gateway.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Admin.Gateway
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.admin.Gateway.GatewaySetup"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class GatewaySetupTest
    {
        private IDisposable _shimContext;
        private PrivateObject _gatewaySetupPrivateObject;
        private GatewaySetup _gatewaySetupInstance;
        private ShimGatewaySetup _shimGatewaySetup;
        private int _gatewayValueWipeOutValuesMethodCallCount;
        private int _gatewayValueDeleteByIDMethodCallCount;
        private Label _lblErrorMessage;
        private PlaceHolder _phError;

        [SetUp]
        public void Setup()
        {
            _gatewayValueWipeOutValuesMethodCallCount = 0;
            _gatewayValueDeleteByIDMethodCallCount = 0;
            _shimContext = ShimsContext.Create();
            _gatewaySetupInstance = new GatewaySetup();
            _shimGatewaySetup = new ShimGatewaySetup(_gatewaySetupInstance);
            _gatewaySetupPrivateObject = new PrivateObject(_gatewaySetupInstance);
            InitShims();
        }

        [TearDown]
        public void TearDown()
        {
            _lblErrorMessage.Dispose();
            _phError.Dispose();
            _shimContext.Dispose();
        }

        private void InitShims()
        {
            _lblErrorMessage = new Label();
            _phError = new PlaceHolder();
            _phError.Visible = false;
            _gatewaySetupPrivateObject.SetField("lblErrorMessage", BindingFlags.Instance | BindingFlags.NonPublic, _lblErrorMessage);
            _gatewaySetupPrivateObject.SetField("phError", BindingFlags.Instance | BindingFlags.NonPublic, _phError);
        }
    }
}

