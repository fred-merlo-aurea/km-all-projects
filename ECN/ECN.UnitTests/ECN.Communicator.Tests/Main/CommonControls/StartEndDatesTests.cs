using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.UI.WebControls;
using ecn.communicator.CommonControls;
using ecn.communicator.CommonControls.Fakes;
using ECN.Communicator.Tests.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.CommonControls
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class StartEndDatesTests
    {
        private IDisposable _shimObject;
        private object[] _methodParams;
        private TextBox _textBoxStartDate;
        private TextBox _textBoxEndDate;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _methodParams = new object[] { };
            _textBoxStartDate = new TextBox();
            _textBoxEndDate = new TextBox();
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        [TestCase("", "11/20/2020", "formfield" , "formfield")]
        [TestCase("01/22/207", "11/20/2020", "errorClass", "formfield")]
        [TestCase("01/22/2007", "11/20/220", "formfield", "formfield errorClass")]
        [TestCase("01/22/2007", "11/20/2200", "errorClass", "formfield")]
        [TestCase("01/22/2017", "11/20/2011", "formfield errorClass", "formfield")]
        [TestCase("01/22/2017", "11/20/2200", "formfield", "formfield")]
        public void ValidateDatesTest(string startDateText, string endDateText, string expectedStartBox, string expectedEndBox)
        {
            //Arrange            
            _textBoxStartDate.Text = startDateText;
            _textBoxEndDate.Text = endDateText;

            InitializeFakes();
            
            //Act
            var privateObject = new PrivateObject(new StartEndDates());
            var sender = new object();
            var e = new EventArgs();

            _methodParams = new object[] { sender, e };
            privateObject.Invoke("btnCheckDates_Click", _methodParams);

            //Assert
            _textBoxStartDate.CssClass.ShouldBe(expectedStartBox);
            _textBoxEndDate.CssClass.ShouldBe(expectedEndBox);
        }

        private void InitializeFakes()
        {
            ShimStartEndDates.AllInstances.throwECNExceptionString = (StartEndDates instance, string value_string) =>
            {
                //Do nothing
            };
            
            ShimStartEndDates.Constructor = (instance) =>
            {                
                ReflectionHelper.SetField(instance, "txtStartDate", _textBoxStartDate);
                ReflectionHelper.SetField(instance, "txtEndDate", _textBoxEndDate);
            };

            ShimStartEndDates.AllInstances.clearECNError = (instance) =>
            {
                //Do nothing
            };
        }
    }
}