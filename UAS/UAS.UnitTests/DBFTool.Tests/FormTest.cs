using DBFtoUAD_Circ_Migration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Shouldly;
using System.Windows.Forms;

namespace DBFTool.Tests
{
    /// <summary>
    /// Unit test for <see cref="Form1"/> class.
    /// </summary>
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class FormTest
    {
        private const string CbClient = "cbClient";
        private const string Label1 = "label1";
        private const string CbPub = "cbPub";
        private const string Name = "Form1";
        private const string ProductName = "DBFtoUAD_Circ_Migration";
        private const string Text = "DBF Import";
        private Form1 _form1;
        private PrivateObject _privateObject;

        [SetUp]
        public void Setup()
        {
            _form1 = new Form1();
            _privateObject = new PrivateObject(_form1);
        }

        [TearDown]
        public void DisposeContext()
        {
            _form1.Dispose();
        }

        [Test]
        public void InitializeComponent_LoadDefaultControl_PageControlLoadedSuccessfully()
        {
            // Arrange
            // Act
            var result = _privateObject;

            // Assert
            _form1.ShouldSatisfyAllConditions(
                () => _form1.ShouldNotBeNull(),
                () => _form1.AcceptButton.ShouldBeNull(),
                () => _form1.AccessibilityObject.ShouldNotBeNull(),
                () => _form1.ActiveControl.ShouldBeNull(),
                () => _form1.ActiveMdiChild.ShouldBeNull(),
                () => _form1.AllowDrop.ShouldBeFalse(),
                () => _form1.AllowTransparency.ShouldBeFalse(),
                () => _form1.AutoScroll.ShouldBeFalse(),
                () => _form1.ControlBox.ShouldBeTrue(),
                () => _form1.Created.ShouldBeFalse(),
                () => _form1.HelpButton.ShouldBeFalse(),
                () => _form1.IsAccessible.ShouldBeFalse(),
                () => _form1.IsDisposed.ShouldBeFalse(),
                () => _form1.IsMdiChild.ShouldBeFalse(),
                () => _form1.IsMdiContainer.ShouldBeFalse(),
                () => _form1.IsMirrored.ShouldBeFalse(),
                () => _form1.IsRestrictedWindow.ShouldBeFalse(),
                () => _form1.KeyPreview.ShouldBeFalse(),
                () => _form1.IsHandleCreated.ShouldBeTrue(),
                () => _form1.TopLevel.ShouldBeTrue(),
                () => _form1.Name.ShouldBe(Name),
                () => _form1.ProductName.ShouldBe(ProductName),
                () => _form1.Text.ShouldBe(Text)
            );
            var cbClient = Get<ComboBox>(CbClient);
            cbClient.ShouldSatisfyAllConditions(
                () => cbClient.ShouldNotBeNull(),
                () => cbClient.DisplayMember.ShouldBe("clientName"),
                () => cbClient.Name.ShouldBe("cbClient"),
                () => cbClient.ValueMember.ShouldBe("clientID")
            );

            var label1 = Get<Label>(Label1);
            label1.ShouldSatisfyAllConditions(
                () => label1.ShouldNotBeNull(),
                () => label1.Name.ShouldBe("label1"),
                () => label1.TabIndex.ShouldBe(1),
                () => label1.Text.ShouldBe("Client"),
                () => label1.AutoSize.ShouldBeTrue()
            );

            var cbPub = Get<ComboBox>(CbPub);
            cbPub.ShouldSatisfyAllConditions(
                () => cbPub.ShouldNotBeNull(),
                () => cbPub.DisplayMember.ShouldBe("PubName"),
                () => cbPub.Name.ShouldBe("cbPub"),
                () => cbPub.ValueMember.ShouldBe("PubCode"),
                () => cbPub.TabIndex.ShouldBe(2)
            );
            _privateObject.GetFieldOrProperty("sourceFileID").ShouldBe(-921);
        }
        private T Get<T>(string propName) where T : class, new()
        {
            return _privateObject.GetFieldOrProperty(propName) as T;
        }
    }
}
