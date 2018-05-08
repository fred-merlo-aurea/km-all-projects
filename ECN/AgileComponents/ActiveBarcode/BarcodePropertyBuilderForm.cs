using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ActiveUp.WebControls
{
    public partial class BarcodePropertyBuilderForm : Form
    {
        private Barcode _barcode = null;
        private Size _currentSize;
        private Bitmap _bmpPrimary = null;
        private Bitmap _bmpSecondary = null;

        public BarcodePropertyBuilderForm(Barcode barcode)
        {
            InitializeComponent();

            _barcode = barcode;

            _comboType.SelectedIndex = 0;
            _comboUnitOfMeasurement.SelectedIndex = 0;
            _comboPrimaryMode.SelectedIndex = 0;
            _comboDateFormat.SelectedIndex = 0;
            _comboSecondaryMode.SelectedIndex = 0;

            LoadBarcodeUI();

        }

        private void LoadBarcodeUI()
        {
         
            if (_barcode.Mode == Mode.Standard)
                _rbStandard.Checked = true;
            else
                _rbHIBC.Checked = true;

            _tbData.Text = _barcode.Data;
            _nudHeight.Value = (decimal)_barcode.Height.Value;
            SelectComboValue(_comboType, _barcode.BarcodeType.ToString());
            _cbGuarded.Checked = _barcode.Guarded;
            _cbNotched.Checked = _barcode.Notched;
            _cbNumbered.Checked = _barcode.Numbered;
            _cbBraced.Checked = _barcode.Braced;
            _cbBoxed.Checked = _barcode.Boxed;

            _tbLICorGS1.Text = string.Empty;
            _tbPCN.Text = string.Empty;
            SelectComboValue(_comboUnitOfMeasurement, _barcode.UnitOfMeasure.ToString());
            if (_barcode.Level == Level.Primary)
                 _rbPrimary.Checked = true;
            else
                _rbSecondary.Checked = true;

            SelectComboValue(_comboPrimaryMode, _barcode.PrimaryEncoding.ToString());
            if (_barcode.HIBCType == HIBCType.LIC)
                _rbHIBCLIC.Checked = true;
            else
                _rbHIBCGS1.Checked = true;

            _tbDate.Text = _barcode.SecondaryData.Date;
            
            _tbQuantity.Text = _barcode.SecondaryData.Quantity;
            _tbSerial.Text = _barcode.SecondaryData.Serial;
            SelectComboValue(_comboDateFormat, _barcode.SecondaryData.DateFormat.ToString());
            SelectComboValue(_comboSecondaryMode, _barcode.SecondaryData.SecondaryEncoding.ToString());
        }

        private void SelectComboValue(ComboBox comboBox, string value)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if ((string)comboBox.Items[i] == value)
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SelectCorrectFormat()
        {
            if (_rbStandard.Checked)
            {
                _gbHIBC.Visible = false;
                _gbStandard.Visible = true;

                this.Height = 435;
            }

            else
            {
                _gbHIBC.Visible = true;
                _gbStandard.Visible = false;

                _gbHIBC.Location = new Point(12, 175);

                this.Height = 445;
            }
        }

        private void _rbStandard_CheckedChanged(object sender, EventArgs e)
        {
            SelectCorrectFormat();
        }

        private void _rbHIBC_CheckedChanged(object sender, EventArgs e)
        {
            SelectCorrectFormat();
        }

        private void BarecodePropertyBuilderForm_Load(object sender, EventArgs e)
        {
            this.Height = 435;
        }

        private bool GenerateBarecode()
        {
            // --- STANDARD ---
            if (_rbStandard.Checked)
            {
                IBarcodeEncoder enc = GetEncoder();

                if (enc == null)
                {
                    MessageBox.Show("That encoder is not implemented yet.");
                    return false;
                }

                try
                {
                    enc.Text = _tbData.Text;
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }

                BarcodeRenderMode mode = BarcodeRenderMode.None;
                if (_cbGuarded.Checked)
                    mode |= BarcodeRenderMode.Guarded;
                if (_cbNotched.Checked)
                    mode |= BarcodeRenderMode.Notched;
                if (_cbNumbered.Checked)
                    mode |= BarcodeRenderMode.Numbered;
                if (_cbBraced.Checked)
                    mode |= BarcodeRenderMode.Braced;
                if (_cbBoxed.Checked)
                    mode |= BarcodeRenderMode.Boxed;

                enc.Sizer.Mode = mode;
                enc.Sizer.DPI = (int)_nupDpi.Value;

                IBarcodeModularSizer mSizer = enc.Sizer as IBarcodeModularSizer;
                if (mSizer != null)
                {
                    if (_nudModuleWidth.Value != 0)
                        mSizer.Module = (float)_nudModuleWidth.Value;
                }

                bool calculResult = CalculateSize();
                if (calculResult == false)
                {
                    return false;
                }

                if (!enc.Sizer.IsValidSize(_currentSize))
                {
                    MessageBox.Show("Invalid size.");
                    return false;
                }

                IBarcodeGenerator gen = enc.Generator;
                _bmpPrimary = gen.GenerateBarcode(_currentSize);
                _pbBarcode.Image = _bmpPrimary;
            }

            // --- HIBC ---
            else
            {
                ActiveUp.WebControls.SecondaryDataHibc secondData = new ActiveUp.WebControls.SecondaryDataHibc();
                if (_tbDate.Text != "")
                {
                    try
                    {
                        secondData.Date = DateTime.Parse(_tbDate.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Invalid date format in the Date box.");
                        return false;
                    }
                }

                if (_tbQuantity.Text != "")
                {
                    try
                    {
                        secondData.Quantity = int.Parse(_tbQuantity.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("The value in the quantity box must be a number between 0 and 99999.");
                        return false;
                    }
                }

                if (_tbSerial.Text != "")
                    secondData.Lot = _tbSerial.Text;

                if (_comboDateFormat.SelectedItem != null)
                {
                    try
                    {
                        secondData.DateFormat = (ActiveUp.WebControls.HibcDateFormat)Enum.Parse(typeof(ActiveUp.WebControls.HibcDateFormat), (string)_comboDateFormat.SelectedItem);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("An error occured in parsing the date format.");
                        return false;
                    }
                }

                Regex licCheck = new Regex("[A-Z][A-Z0-9]{3}", RegexOptions.IgnoreCase);
                Regex pcnCheck = new Regex("[A-Z0-9]{1,13}", RegexOptions.IgnoreCase);
                Regex gs1Check = new Regex("[0-9]{12}");

                if (_rbHIBCLIC.Checked)
                {
                    //Check the LIC and PCN.
                    if (_tbLICorGS1.Text == "" || !licCheck.IsMatch(_tbLICorGS1.Text))
                    {
                        MessageBox.Show("The LIC is not formatted correctly.");
                        return false;
                    }

                    if (_tbPCN.Text != "" && !pcnCheck.IsMatch(_tbPCN.Text))
                    {
                        MessageBox.Show("The PCN is not formatted correctly.");
                        return false;
                    }
                }
                else
                {
                    //GS1
                    if (_tbLICorGS1.Text == "" || !gs1Check.IsMatch(_tbLICorGS1.Text))
                    {
                        MessageBox.Show("The GS1 is not formatted correctly.");
                        return false;
                    }

                    if (_tbPCN.Text != "" && !pcnCheck.IsMatch(_tbPCN.Text))
                    {
                        MessageBox.Show("The PCN is not formatted correctly.");
                        return false;
                    }
                }

                byte uom;
                try
                {
                    uom = byte.Parse((string)_comboUnitOfMeasurement.SelectedItem);
                }
                catch (Exception)
                {
                    MessageBox.Show("The Unit of Measurement is not set.");
                    return false;
                }

                IBarcodeEncoder[] encoders;
                ActiveUp.WebControls.PrimaryEncodingMode pMode;
                ActiveUp.WebControls.SecondaryEncodingMode sMode;


                try
                {
                    pMode = (ActiveUp.WebControls.PrimaryEncodingMode)Enum.Parse(typeof(ActiveUp.WebControls.PrimaryEncodingMode), (string)_comboPrimaryMode.SelectedItem);
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured in parsing the primary encoding mode.");
                    return false;
                }

                try
                {
                    sMode = (ActiveUp.WebControls.SecondaryEncodingMode)Enum.Parse(typeof(ActiveUp.WebControls.SecondaryEncodingMode), (string)_comboSecondaryMode.SelectedItem);
                }
                catch (Exception)
                {
                    MessageBox.Show("An error occured in parsing the secondary encoding mode.");
                    return false;
                }

                try
                {
                    if (_rbHIBCLIC.Checked)
                    {
                        encoders = ActiveUp.WebControls.HIBCEncoder.EncodeHIBC(_tbLICorGS1.Text, _tbPCN.Text, uom, secondData, pMode, sMode);
                    }
                    else
                    {
                        encoders = ActiveUp.WebControls.HIBCEncoder.EncodeGS1(uom.ToString() + _tbLICorGS1.Text, (_tbPCN.Text == "") ? null : _tbPCN.Text, secondData, pMode, sMode);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An exception occured:\n" + ex.Message);
                    return false;
                }

                encoders[0].Sizer.DPI = (float)_nupDpi.Value;
                if (encoders.Length == 2)
                    encoders[1].Sizer.DPI = (float)_nupDpi.Value;

                encoders[0].Sizer.Mode = BarcodeRenderMode.Guarded | BarcodeRenderMode.Numbered;
                if (encoders.Length == 2)
                    encoders[1].Sizer.Mode = BarcodeRenderMode.Guarded | BarcodeRenderMode.Numbered;

                _bmpPrimary = encoders[0].Generator.GenerateBarcode(encoders[0].Sizer.Size);
                if (encoders.Length == 2)
                    _bmpSecondary = encoders[1].Generator.GenerateBarcode(encoders[1].Sizer.Size);

                if (_rbPrimary.Checked)
                    _pbBarcode.Image = _bmpPrimary;
                else
                    _pbBarcode.Image = _bmpSecondary;
            }

            return true;
        }

        private void _bGenerate_Click(object sender, EventArgs e)
        {
            if (GenerateBarecode() == false)
            {
                _pbBarcode.Image = null;
            }
        }

        private IBarcodeEncoder GetEncoder()
        {
            switch (_comboType.SelectedItem as string)
            {
                case "EAN13":
                    return new EAN13Encoder();
                case "EAN8":
                    return new EAN8Encoder();
                case "UPCA":
                    return new UPCAEncoder();
                case "UPCE":
                    return new UPCEEncoder();
                case "UPC2/5":
                    return new UPC25Encoder();
                case "ISBN":
                    return new ISBNEncoder();
                case "ISBN13":
                    return new ISBN13Encoder();
                case "ISSN":
                    return new ISSNEncoder();
                case "Postnet":
                    return new PostnetEncoder();
                case "Planet":
                    return new PlanetEncoder();
                case "Code11":
                    return new Code11Encoder();
                case "Code39":
                    return new Code39Encoder();
                case "Code93":
                    return new Code93Encoder();
                case "Code128":
                    return new Code128Encoder();
                case "MSI":
                    return new MSIEncoder();
                case "S2of5":
                    return new S2of5Encoder();
                case "I2of5":
                    return new I2of5Encoder();
                case "Codabar":
                    return new CodabarEncoder();
                case "EAN Composite":
                    return new EANCompositeEncoder();
                case "ITF14":
                    return new ITF14Encoder();
                case "HIBC":
                    return null;
                case "EAN128":
                    return new ActiveUp.WebControls.EAN128Encoder();
                default:
                    return null;
            }
        }

        private bool CalculateSize() {
            IBarcodeEncoder enc = GetEncoder();
            if (enc == null)
                return false;

            IBarcodeSizer sizer = enc.Sizer;
            IBarcodeModularSizer mSizer = sizer as IBarcodeModularSizer;
            
            try {
                //if ((enc.Flags&BarcodeEncoderFlags.Composite)!=0 || 
                //    (enc.Generator!=null && (enc.Generator.Flags & BarcodeGeneratorFlags.VariableLength)!=0))
                    enc.Text = _tbData.Text;

                //if ((enc.Flags & BarcodeEncoderFlags.Composite) != 0) {
                    sizer = enc.Sizer;
                    mSizer = sizer as IBarcodeModularSizer;
                //}

                sizer.DPI = (int)_nupDpi.Value;
            } catch (ArgumentException ex) {
                MessageBox.Show(ex.Message);
                return false;
            }

            BarcodeRenderMode mode = BarcodeRenderMode.None;
            if (_cbGuarded.Checked)
                mode |= BarcodeRenderMode.Guarded;
            if (_cbNotched.Checked)
                mode |= BarcodeRenderMode.Notched;
            if (_cbNumbered.Checked)
                mode |= BarcodeRenderMode.Numbered;
            if (_cbBraced.Checked)
                mode |= BarcodeRenderMode.Braced;
            if (_cbBoxed.Checked)
                mode |= BarcodeRenderMode.Boxed;

            sizer.Mode = mode;
            //Ok, now what we do depends on the options that are set.

            if (_nudHeight.Value < sizer.Height)
                _nudHeight.Value = sizer.Height;

            Size actualSize;
            if (sizer.DPI == 0 || mSizer==null) {
                //Logical mode, or simple fixed size.
                actualSize = sizer.GetValidSize(new Size(sizer.Width,(int)_nudHeight.Value));
                SetSizeDisplay(actualSize);
            } else if (_rbDesiredWidth.Checked) {
                //They have given a desired width.
                if (_nudDesiredWidth.Value == 0) {
                    actualSize = sizer.GetValidSize(new Size(sizer.Width, (int)_nudHeight.Value));
                    SetSizeDisplay(actualSize);
                } else {
                    float mils = BarcodeUtilities.CalculateModuleWidth(mSizer, (int)(mSizer.DPI * (float)_nudDesiredWidth.Value));
                    _nudModuleWidth.Value = (decimal)mils;
                    mSizer.Module = mils;
                    if (_nudHeight.Value < sizer.Height)
                        _nudHeight.Value = sizer.Height;
                    actualSize = sizer.GetValidSize(new Size(sizer.Width, (int)_nudHeight.Value));
                    SetSizeDisplay(actualSize);
                }
            } else {
                //rbModule checked.
                mSizer.Module = (float)_nudModuleWidth.Value;
                if (_nudHeight.Value < sizer.Height)
                    _nudHeight.Value = sizer.Height;
                actualSize = sizer.GetValidSize(new Size(sizer.Width, (int)_nudHeight.Value));
                SetSizeDisplay(actualSize);
            }

            return true;
        }

        private void SetSizeDisplay(Size p)
        {
            _currentSize = p;
        }

        private void _rbDesiredWidth_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbDesiredWidth.Checked)
            {
                _nudDesiredWidth.ReadOnly = false;
                _nudModuleWidth.ReadOnly = true;
            }
            else
            {
                _nudDesiredWidth.ReadOnly = true;
                _nudModuleWidth.ReadOnly = false;
            }
        }

        private void _rbPrimary_CheckedChanged(object sender, EventArgs e)
        {
            _pbBarcode.Image = _bmpPrimary;
        }

        private void _rbSecondary_CheckedChanged(object sender, EventArgs e)
        {
            _pbBarcode.Image = _bmpSecondary;
        }

        private void _bApply_Click(object sender, EventArgs e)
        {
            if (GenerateBarecode() == true)
            {
                SetValue();
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void SetValue()
        {
            _barcode.Dpi = (int)_nupDpi.Value;

            if (_rbStandard.Checked)
            {
                ResetHIBC();

                _barcode.Mode = Mode.Standard;
                _barcode.Data = _tbData.Text;
                _barcode.Height = new System.Web.UI.WebControls.Unit((int)_nudHeight.Value);
                _barcode.BarcodeType = (BarcodeType)Enum.Parse(typeof(BarcodeType), _comboType.Text, true);
                _barcode.Guarded = _cbGuarded.Checked;
                _barcode.Notched = _cbNotched.Checked;
                _barcode.Numbered = _cbNumbered.Checked;
                _barcode.Braced = _cbBraced.Checked;
                _barcode.Boxed = _cbBoxed.Checked;
            }

            else
            {
                ResetStandard();

                _barcode.Mode = Mode.HIBC;

                _barcode.LicGs1 = _tbLICorGS1.Text;
                _barcode.Pcn = _tbPCN.Text;
                _barcode.UnitOfMeasure = int.Parse(_comboUnitOfMeasurement.Text);
                if (_rbPrimary.Checked)
                    _barcode.Level = Level.Primary;
                else
                    _barcode.Level = Level.Secondary;
                _barcode.PrimaryEncoding = (PrimaryEncodingMode)Enum.Parse(typeof(PrimaryEncodingMode), _comboPrimaryMode.Text, true);
                if (_rbHIBCLIC.Checked)
                    _barcode.HIBCType = HIBCType.LIC;
                else
                    _barcode.HIBCType = HIBCType.GS1;

                _barcode.SecondaryData = new SecondaryData();
                _barcode.SecondaryData.Date = _tbDate.Text;
                _barcode.SecondaryData.Quantity = _tbQuantity.Text;
                _barcode.SecondaryData.Serial = _tbSerial.Text;
                _barcode.SecondaryData.SecondaryEncoding = (SecondaryEncodingMode)Enum.Parse(typeof(SecondaryEncodingMode), _comboSecondaryMode.Text, true);
            }
        }

        private void ResetStandard()
        {
            _barcode.Data = string.Empty;
            _barcode.BarcodeType = BarcodeType.Codabar;
            _barcode.Guarded = false;
            _barcode.Notched = false;
            _barcode.Numbered = false;
            _barcode.Braced = false;
            _barcode.Boxed = false;

        }

        private void ResetHIBC()
        {
            _barcode.LicGs1 = string.Empty;
            _barcode.Pcn = string.Empty;
            _barcode.UnitOfMeasure = 0;
            _barcode.PrimaryEncoding = PrimaryEncodingMode.Code128;
            _barcode.Level = Level.Primary;
            _barcode.HIBCType = HIBCType.LIC;
            _barcode.SecondaryData = new SecondaryData();
        }
    }
}