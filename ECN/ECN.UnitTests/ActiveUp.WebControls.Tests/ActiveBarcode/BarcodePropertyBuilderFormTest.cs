using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using webControl = System.Web.UI.WebControls;

namespace ActiveUp.WebControls.Tests.ActiveBarcode
{
    /// <summary>
    /// Unit test for <see cref="BarcodePropertyBuilderForm"/> class.
    /// </summary>
    [TestFixture, Apartment(ApartmentState.STA)]
    [ExcludeFromCodeCoverage]
    public class BarcodePropertyBuilderFormTest
    {
        private const string Barcode = "_barcode";
        private const string CurrentSize = "_currentSize";
        private const string Data = "1234567890";
        private const string PropertyBuilderText = "Barecode Property Builder";
        private const string RbStandard = "_rbStandard";
        private const string CbGuarded = "_cbGuarded";
        private const string CbNotched = "_cbNotched";
        private const string CbNumbered = "_cbNumbered";
        private const string CbBraced = "_cbBraced";
        private const string CbBoxed = "_cbBoxed";
        private const string RbPrimary = "_rbPrimary";
        private const string RbHibclic = "_rbHIBCLIC";
        private const string TbDate = "_tbDate";
        private const string TbQuantity = "_tbQuantity";
        private const string TbSerial = "_tbSerial";
        private BarcodePropertyBuilderForm _barcodePropertyBuilderForm;
        private PrivateObject _privateObject;
        private Barcode _barcode;

        [SetUp]
        public void Setup()
        {
            CreateBarcodeObject();
        }

        [TearDown]
        public void DisposeContext()
        {
            _barcodePropertyBuilderForm.Dispose();
            _barcode.Dispose();
        }

        [Test]
        public void InitializeComponent_LoadBarcodePropertyBuilderForm_ValidatePageControlLoadSuccessfully()
        {
            // Arrange
            // Act
            CreateBarcodePropertyBuilderFormObject(_barcode);

            // Assert
            _barcodePropertyBuilderForm.ShouldSatisfyAllConditions(
                () => _barcodePropertyBuilderForm.ShouldNotBeNull(),
                () => _barcodePropertyBuilderForm.Visible.ShouldBeFalse(),
                () => _barcodePropertyBuilderForm.Text.ShouldBe(PropertyBuilderText),
                () => _barcodePropertyBuilderForm.Visible.ShouldBeFalse(),
                () => _barcodePropertyBuilderForm.CausesValidation.ShouldBeTrue(),
                () => _barcodePropertyBuilderForm.ControlBox.ShouldBeTrue(),
                () => _barcodePropertyBuilderForm.Created.ShouldBeFalse(),
                () => _barcodePropertyBuilderForm.Modal.ShouldBeFalse()
            );

            var barcode = _privateObject.GetFieldOrProperty(Barcode) as Barcode;
            barcode.ShouldSatisfyAllConditions(
                () => barcode.ShouldNotBeNull(),
                () => barcode.Data.ShouldBe(Data),
                () => barcode.Enabled.ShouldBeTrue(),
                () => barcode.PrimaryEncoding.ShouldBe(PrimaryEncodingMode.Code128)
            );
            var currentSize = _privateObject.GetFieldOrProperty(CurrentSize) as Size?;
            currentSize.ShouldSatisfyAllConditions(
                () => currentSize.Value.ShouldNotBeNull(),
                () => currentSize.Value.Height.ShouldBe(0),
                () => currentSize.Value.Width.ShouldBe(0),
                () => currentSize.Value.IsEmpty.ShouldBeTrue()
            );

            Get<RadioButton>(RbStandard).Checked.ShouldBeTrue();
            Get<CheckBox>(CbGuarded).Checked.ShouldBeFalse();
            Get<CheckBox>(CbNotched).Checked.ShouldBeFalse();
            Get<CheckBox>(CbNumbered).Checked.ShouldBeFalse();
            Get<CheckBox>(CbBraced).Checked.ShouldBeFalse();
            Get<RadioButton>(RbPrimary).Checked.ShouldBeTrue();
            Get<RadioButton>(RbHibclic).Checked.ShouldBeTrue();
            Get<TextBox>(TbSerial).Text.ShouldBeEmpty();
            Get<TextBox>(TbQuantity).Text.ShouldBe("0");
        }

        private void CreateBarcodeObject()
        {
            _barcode = new Barcode();
            _barcode.BackColor = Color.Red;
            _barcode.CssClass = string.Empty;
            _barcode.ForeColor = Color.Gray;
            _barcode.Dpi = new webControl.Unit();
            _barcode.Mode = Mode.Standard;
        }
        private void CreateBarcodePropertyBuilderFormObject(Barcode barcode)
        {
            _barcodePropertyBuilderForm = new BarcodePropertyBuilderForm(barcode);
            _privateObject = new PrivateObject(_barcodePropertyBuilderForm);
        }

        private T Get<T>(string propName) where T : class, new()
        {
            return _privateObject.GetFieldOrProperty(propName) as T;
        }
    }
}
