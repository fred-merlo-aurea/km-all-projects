using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.UI.WebControls;
using ecn.communicator.main.lists.reports;
using ecn.communicator.main.lists.reports.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Communicator.Tests.Main.Lists.reports
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.communicator.main.lists.reports.DataDumpReport"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class DataDumpReportTest
    {
        private IDisposable _shimContext;
        private PrivateObject _dataDumpReportPrivateObject;
        private DataDumpReport _dataDumpReportInstance;
        private ShimDataDumpReport _shimDataDumpReport;
        private PlaceHolder _phError;
        private Label _lblErrorMessage;

        [SetUp]
        public void Setup()
        {
            _shimContext = ShimsContext.Create();
            _dataDumpReportInstance = new DataDumpReport();
            _shimDataDumpReport = new ShimDataDumpReport(_dataDumpReportInstance);
            _dataDumpReportPrivateObject = new PrivateObject(_dataDumpReportInstance);
            InitShims();
        }

        [TearDown]
        public void TearDown()
        {
            _shimContext.Dispose();
            _phError.Dispose();
            _lblErrorMessage.Dispose();
        }

        private void InitShims()
        {
            _lblErrorMessage = new Label();
            _phError = new PlaceHolder();
            _phError.Visible = false;
            _dataDumpReportPrivateObject.SetField("lblErrorMessage", BindingFlags.Instance | BindingFlags.NonPublic, _lblErrorMessage);
            _dataDumpReportPrivateObject.SetField("phError", BindingFlags.Instance | BindingFlags.NonPublic, _phError);
        }
    }
}
