using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Fakes;
using ecn.communicator.CommonControls;
using ecn.communicator.CommonControls.Fakes;
using ECN_Framework_Common.Objects;
using ECN.Tests.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.CommonControls
{
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class ErrorViewerTests: PageHelper
    {
        private IDisposable _shimObject;
        private object[] _methodParams;
        private const string _expectedValue = "ErrorMessage";
        private string _returnedString;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _methodParams = new object[] { };
            _returnedString = string.Empty;
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }

        [Test]
        public void Error_Test()
        {
            //Arrange
            InitializeShims();            

            var viewer = new ErrorViewer();
            var sender = new object();
            var e = new EventArgs();
            
            var privateObject = new PrivateObject(viewer);
            _methodParams = new object[] { sender, e };

            //Act
            privateObject.Invoke("Page_Load", _methodParams);

            //Assert
            _returnedString.ShouldBe(_expectedValue);
        }

        private void InitializeShims()
        {
            InitializeShimErrorViewerConstructor();
            InitializeShimDataBind();
        }

        private void InitializeShimDataBind()
        {
            ShimBaseDataBoundControl.AllInstances.DataBind = (instance) =>
            {
                try
                {
                    _returnedString = (instance as BulletedList).DataValueField;
                }
                catch (Exception ex)
                {
                    var result = new StringBuilder();
                    _returnedString = result.Append("Exception thrown. ").Append(ex.StackTrace).ToString();
                }
            };
        }

        private void InitializeShimErrorViewerConstructor()
        {
            ShimErrorViewer.Constructor = (instance) =>
            {
                var source = new List<ECNError>();
                var lblMessage = new Label();
                var phError = new PlaceHolder();

                var bulletList = new BulletedList()
                {
                    DataSource = source,
                    DataTextField = "",
                    DataValueField = ""
                };

                ReflectionHelper.SetField(instance, "BulletRunTime", bulletList);
                ReflectionHelper.SetField(instance, "lblErrorMessage", lblMessage);
                ReflectionHelper.SetField(instance, "phError", phError);
            };
        }
    }
}