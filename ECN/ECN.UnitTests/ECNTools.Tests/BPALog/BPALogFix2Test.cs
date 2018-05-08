using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO.Fakes;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Fakes;
using ECN.Tests.Helpers;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Activity;
using ECN_Framework_Entities.Communicator;
using ECNTools.BPALog;
using ECNTools.Fakes;
using ECNTools.Tests.SMTPLog;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECNTools.Tests.BPALog
{
    [TestClass]
    [ExcludeFromCodeCoverage]
	public class BPALogFix2Test
	{
		private const string ConnectionString = "data source=127.0.0.1;Integrated Security=SSPI;";
        private const string DummyFilePath = "1_dummyFilePath.xls";
        private const string DummyEmailAddress = "test";
        private const string MethodReadDataFromBPAFile = "ReadDatafromBPAFile";
        private const string MethodInitializeComponent = "InitializeComponent";
        private const string BtnCloseClickMethodName = "btnClose_Click";
        private const string BtnFileLocationClickMethodName = "btnFileLocation_Click";
        private const string BpaLogFixFormClosingMethodName = "BPALogFix_FormClosing";
        private const string TextFolderLocationFieldName = "txtFolderLocation";
        private const string TestPath = "c:\bla\\mytestpath";
        private BPALogFix2 _bpaLogFix2Object;
		private PrivateObject _privateObject;
		protected IDisposable _shimObject;

		[SetUp]
		public void Setup()
		{
			_shimObject = ShimsContext.Create();
		}

		[TearDown]
		public void DisposeContext()
		{
			_shimObject.Dispose();
		}

        [TestCase("MSGID ISSUE [12][10] TO: test")]
        [TestCase("MSGID ISSUE [12][10] 250TO: test")]
        public void ReadDatafromBPAFile_WhenCalled_ReadsDataFromFile(string fileText)
        {
            // Arrange
            CreateClassObject();
            CreateShims();
            var dataRead = false;
            var dummyTextLine = fileText;
            var lineCount = 0;
            ShimStreamReader.AllInstances.ReadLine = (isntance) =>
            {
                lineCount++;
                return lineCount > 5 
                ? null 
                : dummyTextLine;
            };
            ShimStreamWriter.AllInstances.Close = (x) => 
            {
                dataRead = true;
            };

            // Act
            _privateObject.Invoke(MethodReadDataFromBPAFile, null);

            // Assert
            dataRead.ShouldBeTrue();
        }

        [Test]
		public void InitializeComponent_LoadDefaultValue_ValidatePageControlLoadSuccessfully()
		{
			// Arrange
			CreateClassObject();

			// Act
			_privateObject.Invoke(MethodInitializeComponent, null);

			// Assert
			AssertMethodResult(_bpaLogFix2Object);
		}

		[Test]
		public void InitializeComponent_LoadDefaultValue_ValidatePageNameIsAssignedSuccessfully()
		{
			// Arrange
			CreateClassObject();

			// Act
			_privateObject.Invoke(MethodInitializeComponent, null);

			// Assert
			var bpaLogName = (string)_privateObject.GetFieldOrProperty("Name");
			var bpaLogText = (string)_privateObject.GetFieldOrProperty("Text");
			bpaLogName.ShouldSatisfyAllConditions(
				() => bpaLogName.ShouldNotBeNullOrWhiteSpace(),
				() => bpaLogText.ShouldNotBeNullOrWhiteSpace(),
				() => bpaLogName.ShouldBe(BPAHelper.BpaName),
				() => bpaLogName.ShouldBe(BPAHelper.BpaName));
		}

        [Test]
        public void BtnCloseClick_CorrectParameters_MenusToggled()
        {
            // Arrange
            var actualFormCloseCalled = false;
            var actualToggleMenusCalled = false;

            var bpaLogFix2Object = new BPALogFix2 { MdiParent = new Main2() };

            ShimMain2.AllInstances.ToggleMenusBoolean = (main, show) =>
            {
                actualToggleMenusCalled = show;
            };

            ShimForm.AllInstances.Close = form => { actualFormCloseCalled = true; };

            // Act
            bpaLogFix2Object.GetType()
                .CallMethod(BtnCloseClickMethodName, new object[] { null, null }, bpaLogFix2Object);

            // Assert
            actualToggleMenusCalled.ShouldSatisfyAllConditions(
                () => actualToggleMenusCalled.ShouldBeTrue(),
                () => actualFormCloseCalled.ShouldBeTrue());
        }

        [Test]
        public void BtnFileLocationClick_DialogResultOk_FolderTextSet()
        {
            // Arrange
            var bpaLogFix2Object = new BPALogFix2 { MdiParent = new Main2() };
            ShimCommonDialog.AllInstances.ShowDialog = dialog => DialogResult.OK;
            ShimFolderBrowserDialog.AllInstances.SelectedPathGet = dialog => TestPath;

            // Act
            bpaLogFix2Object.GetType()
                .CallMethod(BtnFileLocationClickMethodName, new object[] { null, null }, bpaLogFix2Object);

            var txtFolderLocation = ((TextBox)bpaLogFix2Object.GetType()
                                            .GetField(TextFolderLocationFieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                                            ?.GetValue(bpaLogFix2Object))?.Text;

            // Assert
            txtFolderLocation.ShouldSatisfyAllConditions(
                () => txtFolderLocation.ShouldBe(TestPath));
        }
        
        [Test]
        public void BtnFileLocationClick_DialogResultCancel_FolderTextSet()
        {
            // Arrange
            var bpaLogFix2Object = new BPALogFix2 { MdiParent = new Main2() };
            ShimCommonDialog.AllInstances.ShowDialog = dialog => DialogResult.Cancel;
            ShimFolderBrowserDialog.AllInstances.SelectedPathGet = dialog => TestPath;

            // Act
            bpaLogFix2Object.GetType()
                .CallMethod(BtnFileLocationClickMethodName, new object[] { null, null }, bpaLogFix2Object);

            var txtFolderLocation = ((TextBox)bpaLogFix2Object.GetType()
                                            .GetField(TextFolderLocationFieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                                            ?.GetValue(bpaLogFix2Object))?.Text;

            // Assert
            txtFolderLocation.ShouldSatisfyAllConditions(
                () => txtFolderLocation.ShouldBe(string.Empty));
        }

        [Test]
        public void BPALogFixFormClosing_Called_ClosesForm()
        {
            // Arrange
            var actualToggleMenusCalled = false;

            var bpaLogFix2Object = new BPALogFix2 { MdiParent = new Main2() };

            ShimMain2.AllInstances.ToggleMenusBoolean = (main, show) =>
            {
                actualToggleMenusCalled = show;
            };

            ShimForm.AllInstances.Close = form => throw new ArgumentOutOfRangeException();

            // Act
            bpaLogFix2Object.GetType()
                .CallMethod(BpaLogFixFormClosingMethodName, new object[] { null, null }, bpaLogFix2Object);

            // Assert
            actualToggleMenusCalled.ShouldSatisfyAllConditions(
                () => actualToggleMenusCalled.ShouldBeTrue());
        }

		private object GetTextbox(string tbName)
		{
			var txt = string.Empty;;
			var textBox = _privateObject.GetFieldOrProperty(tbName) as TextBox;
			if (textBox != null)
			{
				txt = textBox.Text;
			}
			return txt;
		}

		private object GetLabel(string tbName)
		{
			var txt = string.Empty;;
			var label = _privateObject.GetFieldOrProperty(tbName) as Label;
			if (label != null)
			{
				txt = label.Text;
			}
			return txt;
		}

		private object GetButton(string tbName)
		{
			var txt = string.Empty;;
			var btn = _privateObject.GetFieldOrProperty(tbName) as Button;
			if (btn != null)
			{
				txt = btn.Text;
			}
			return txt;
		}

		private object GetRadioButton(string tbName)
		{
			var txt = string.Empty;;
			var radioBtn = _privateObject.GetFieldOrProperty(tbName) as RadioButton;
			if (radioBtn != null)
			{
				txt = radioBtn.Text;
			}
			return txt;
		}

		private object GetCheckbox(string tbName)
		{
			var txt = string.Empty;;
			var ckBox = _privateObject.GetFieldOrProperty(tbName) as CheckBox;
			if (ckBox != null)
			{
				txt = ckBox.Text;
			}
			return txt;
		}

		private T Get<T>(string propName)
		{
			var val = (T)_privateObject.GetFieldOrProperty(propName);
			return val;
		}

		private void CreateClassObject()
		{
			ShimConfigurationManager.ConnectionStringsGet = () =>
			{
				var sampleConfigSettingCollection = new ConnectionStringSettingsCollection();
				var dummyConnectionString = new ConnectionStringSettings("ECNCommunicator", ConnectionString);
				sampleConfigSettingCollection.Add(dummyConnectionString);
				return sampleConfigSettingCollection;
			};
			_bpaLogFix2Object = new BPALogFix2();
			_privateObject = new PrivateObject(_bpaLogFix2Object);
		}

		private void AssertMethodResult(BPALogFix2 result)
		{
			result.ShouldSatisfyAllConditions(
				() => GetLabel(BPAHelper.Label1).ShouldBe(BPAHelper.Label1Text),
				() => GetLabel(BPAHelper.Label2).ShouldBe(BPAHelper.Label2Text),
				() => GetLabel(BPAHelper.Label3).ShouldBe(BPAHelper.Label3Text),
				() => GetButton(BPAHelper.ButtonChooseDialog).ShouldBe(BPAHelper.ButtonChooseDialogText),
				() => GetButton(BPAHelper.ButtonFileLocation).ShouldBe(BPAHelper.ButtonFileLocationText),
				() => GetButton(BPAHelper.ButtonCancel).ShouldBe(BPAHelper.ButtonCancelText),
				() => GetButton(BPAHelper.ButtonClose).ShouldBe(BPAHelper.ButtonCloseText)
			);
		}

        private void CreateShims()
        {
            ShimPath.GetFileNameString = (x) => DummyFilePath;
            var blast = CreateInstance(typeof(BlastRegular));
            blast.SendTime = DateTime.Now;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => blast;
            var blastActivityBounces = CreateInstance(typeof(BlastActivityBounces));
            var blastActivityBouncesList = new List<BlastActivityBounces>();
            blastActivityBouncesList.Add(blastActivityBounces);
            ShimBlastActivityBounces.GetByBlastIDInt32 = (x) => blastActivityBouncesList;
            var blastActivitySends = CreateInstance(typeof(BlastActivitySends));
            blastActivitySends.EmailAddress = DummyEmailAddress;
            var blastActivitySendsList = new List<BlastActivitySends>();
            blastActivitySendsList.Add(blastActivitySends);
            ShimBlastActivitySends.GetByBlastIDInt32 = (x) => blastActivitySendsList;
            ShimEmail.GetByEmailID_NoAccessCheckInt32 = (x) => CreateInstance(typeof(Email));
            ShimFile.DeleteString = (x) => { };
            ShimFile.ExistsString = (x) => true;
            ShimStreamReader.ConstructorString = (x, y) => { };
            ShimStreamReader.AllInstances.ReadToEnd = (isntance) => string.Empty;
            ShimStreamWriter.ConstructorString = (x, y) => { };
            ShimTextWriter.AllInstances.WriteLineString = (x, y) => { };
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private void CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
        {
            ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
        }

        private void SetField(dynamic obj, string fieldName, dynamic fieldValue)
        {
            ReflectionHelper.SetField(obj, fieldName, fieldValue);
        }

        private dynamic GetField(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetFieldValue(obj, fieldName);
        }

        private void SetProperty(dynamic instance, string propertyName, dynamic value)
        {
            ReflectionHelper.SetProperty(instance, propertyName, value);
        }

        private dynamic GetProperty(dynamic instance, string propertyName)
        {
            return ReflectionHelper.GetPropertyValue(instance, propertyName);
        }
    }
}

