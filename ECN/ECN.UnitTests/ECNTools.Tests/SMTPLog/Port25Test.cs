using System;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using ECNTools.SMTPLog;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECNTools.Tests.SMTPLog
{
    /// <summary>
    ///     Unit tests for <see cref="ECNTools.SMTPLog.Port25"/>
    /// </summary>
    [TestFixture]
	[ExcludeFromCodeCoverage]
	public partial class Port25Test
	{
		private const string ConnectionString = "data source=127.0.0.1;Integrated Security=SSPI;";
		private Port25 _port25Object;
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

		[Test]
		public void InitializeComponent_LoadDefaultValue_ValidatePageControlLoadSuccessfully()
		{
			// Arrange
			CreateClassObject();

			// Act
			_privateObject.Invoke("InitializeComponent", null);

			// Assert
			AssertMethodResult(_port25Object);
		}

		[Test]
		public void InitializeComponent_LoadDefaultValue_ValidatePageNameIsAssignedSuccessfully()
		{
			// Arrange
			CreateClassObject();

			// Act
			_privateObject.Invoke("InitializeComponent", null);

			// Assert
			var port25Name = (string)_privateObject.GetFieldOrProperty("Name");
			var port25Text = (string)_privateObject.GetFieldOrProperty("Text");
			port25Name.ShouldSatisfyAllConditions(
				() => port25Name.ShouldNotBeNullOrWhiteSpace(),
				() => port25Text.ShouldNotBeNullOrWhiteSpace(),
				() => port25Name.ShouldBe("Port25"),
				() => port25Name.ShouldBe("Port25"));
		}

		private object GetTextbox(string tbName)
		{
			var txt = "";
			var textBox = (TextBox)_privateObject.GetFieldOrProperty(tbName);
			if (textBox != null)
			{
				txt = textBox.Text;
			}
			return txt;
		}

		private object GetLabel(string tbName)
		{
			var txt = "";
			var label = (Label)_privateObject.GetFieldOrProperty(tbName);
			if (label != null)
			{
				txt = label.Text;
			}
			return txt;
		}

		private object GetButton(string tbName)
		{
			var txt = "";
			var btn = (Button)_privateObject.GetFieldOrProperty(tbName);
			if (btn != null)
			{
				txt = btn.Text;
			}
			return txt;
		}

		private object GetRadioButton(string tbName)
		{
			var txt = "";
			var radioBtn = (RadioButton)_privateObject.GetFieldOrProperty(tbName);
			if (radioBtn != null)
			{
				txt = radioBtn.Text;
			}
			return txt;
		}

		private object GetCheckbox(string tbName)
		{
			var txt = "";
			var ckBox = (CheckBox)_privateObject.GetFieldOrProperty(tbName);
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
			_port25Object = new Port25();
			_privateObject = new PrivateObject(_port25Object);
		}

		private void AssertMethodResult(Port25 result)
		{
			result.ShouldSatisfyAllConditions(
				() => GetLabel(CommonHelper.Label3).ShouldBe("From:"),
				() => GetLabel(CommonHelper.Label4).ShouldBe("To:"),
				() => GetLabel(CommonHelper.Label5).ShouldBe("BlastIDs:"),
				() => GetLabel(CommonHelper.Label2).ShouldBe("Group ID:"),
				() => GetLabel(CommonHelper.Label6).ShouldBe("Save File To:"),
				() => GetRadioButton(CommonHelper.RadioButtonGroup).ShouldBe("Group ID"),
				() => GetRadioButton(CommonHelper.RadioButtonBlastID).ShouldBe("Blast IDs"),
				() => GetButton(CommonHelper.ButtonFileLocation).ShouldBe("Select Folder"),
				() => GetButton(CommonHelper.ButtonDigitalSplit).ShouldBe("Select File"),
				() => GetButton(CommonHelper.ButtonCancel).ShouldBe("Cancel"),
				() => GetButton(CommonHelper.ButtonClose).ShouldBe("Close"),
				() => GetCheckbox(CommonHelper.CheckBoxDigitalSplit).ShouldBe("Import Digital Split to:")
			);
		}
	}
}
