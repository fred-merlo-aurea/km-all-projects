using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Web;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Data;
using System.Runtime.InteropServices;
using ActiveUp.WebControls;

namespace ActiveUp.WebControls
{
	[
	ToolboxData("<{0}:Barcode runat=server></{0}:Calendar>"),
	Designer(typeof(BarcodeControlDesigner)),
    Editor(typeof(BarcodeComponentEditor), typeof(ComponentEditor)),
    ToolboxBitmap(typeof(Barcode), "ToolBoxBitmap.Barecode.bmp"),
    ComVisibleAttribute(true),
    Serializable
	]
	public class Barcode : System.Web.UI.WebControls.WebControl
    {
        static LicenseContext _lastDesignContext = null;
        static bool _licenseChecked = false;

        #region Fields

        private string _tmpFileName = null;
        private SecondaryData _secondaryData = null;
        
        #endregion

        #region Constructors

        public Barcode()
        {
            _secondaryData = new SecondaryData();

            _licenseChecked = true;
        }

        #endregion

        #region Properties

        #region Appearance

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }

            set
            {
                base.BackColor = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string CssClass
        {
            get
            {
                return base.CssClass;
            }

            set
            {
                base.CssClass = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new FontInfo Font
        {
            get
            {
                return base.Font;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }

            set
            {
                base.ForeColor = value;
            }
        }

        #endregion

        #region Barcode

        [
        Bindable(false),
        Category("Barcode"),
        Description("N/A"),
        DefaultValue(typeof(Unit),"200px")
        ]
        public Unit Dpi
        {
            get
            {
                object dpi = ViewState["Dpi"];

                if (dpi != null)
                    return (Unit)dpi;

                return Unit.Parse("200px");
            }

            set
            {
                ViewState["Dpi"] = value;
            }
        }

        [
        Bindable(false),
        Category("Barcode"),
        Description("N/A"),
        DefaultValue(typeof(Mode),"Standard")
        ]
        public Mode Mode
        {
            get
            {
                object mode = ViewState["Mode"];

                if (mode != null)
                    return (Mode)mode;

                return Mode.Standard;
            }

            set
            {
                ViewState["Mode"] = value;
            }
        }    

        #endregion

        #region Standard

        [
        Bindable(false),
        Category("Standard"),
        Description("N/A"),
        DefaultValue("1234567890")
        ]
        public string Data
        {
            get
            {
                object data = ViewState["Data"];

                if (data != null)
                    return (string)data;

                return "1234567890";
            }

            set
            {
                ViewState["Data"] = value;
            }
        }

        [
          Bindable(false),
          Category("Standard"),
          Description("N/A"),
          DefaultValue(typeof(ActiveUp.WebControls.BarcodeType), "Code128")
          ]
        public ActiveUp.WebControls.BarcodeType BarcodeType
        {
            get
            {
                object type = ViewState["BarcodeType"];

                if (type != null)
                    return (ActiveUp.WebControls.BarcodeType)type;

                return ActiveUp.WebControls.BarcodeType.Code128;
            }

            set
            {
                ViewState["BarcodeType"] = value;
            }
        }

        [
          Bindable(false),
          Category("Standard"),
          Description("N/A"),
          DefaultValue(false)
        ]
        public bool Guarded
        {
            get
            {
                object guarded = ViewState["Guarded"];

                if (guarded != null)
                    return (bool)guarded;

                return false;
            }

            set
            {
                ViewState["Guarded"] = value;
            }
        }

        [
          Bindable(false),
          Category("Standard"),
          Description("N/A"),
          DefaultValue(false)
        ]
        public bool Notched
        {
            get
            {
                object notched = ViewState["Notched"];

                if (notched != null)
                    return (bool)notched;

                return false;
            }

            set
            {
                ViewState["Notched"] = value;
            }
        }

        [
          Bindable(false),
          Category("Standard"),
          Description("N/A"),
          DefaultValue(false)
        ]
        public bool Numbered
        {
            get
            {
                object numbered = ViewState["Numbered"];

                if (numbered != null)
                    return (bool)numbered;

                return false;
            }

            set
            {
                ViewState["Numbered"] = value;
            }
        }

        [
          Bindable(false),
          Category("Standard"),
          Description("N/A"),
          DefaultValue(false)
        ]
        public bool Braced
        {
            get
            {
                object braced = ViewState["Braced"];

                if (braced != null)
                    return (bool)braced;

                return false;
            }

            set
            {
                ViewState["Braced"] = value;
            }
        }
        
        [
          Bindable(false),
          Category("Standard"),
          Description("N/A"),
          DefaultValue(false)
        ]
        public bool Boxed
        {
            get
            {
                object boxed = ViewState["Boxed"];

                if (boxed != null)
                    return (bool)boxed;

                return false;
            }

            set
            {
                ViewState["Boxed"] = value;
            }
        }

        #endregion

        #region HIBC

        [
          Bindable(false),
          Category("HIBC"),
          Description("N/A"),
          DefaultValue("")
        ]
        public string LicGs1
        {
            get
            {
                object licGs1 = ViewState["LicGs1"];

                if (licGs1 != null)
                    return (string)licGs1;

                return string.Empty;
            }

            set
            {
                ViewState["LicGs1"] = value;
            }
        }

        [
          Bindable(false),
          Category("HIBC"),
          Description("N/A"),
          DefaultValue("")
        ]
        public string Pcn
        {
            get
            {
                object pcn = ViewState["Pcn"];

                if (pcn != null)
                    return (string)pcn;

                return string.Empty;
            }

            set
            {
                ViewState["Pcn"] = value;
            }
        }

        [
          Bindable(false),
          Category("HIBC"),
          Description("N/A"),
          DefaultValue(0)
        ]
        public int UnitOfMeasure
        {
            get
            {
                object unitOfMeasure = ViewState["UnitOfMeasure"];

                if (unitOfMeasure != null)
                    return (int)unitOfMeasure;

                return 0;
            }

            set
            {
                ViewState["UnitOfMeasure"] = value;
            }
        }

        [
          Bindable(false),
          Category("HIBC"),
          Description("N/A"),
          DefaultValue(typeof(PrimaryEncodingMode),"Code128")
        ]
        public PrimaryEncodingMode PrimaryEncoding
        {
            get
            {
                object primaryEncoding = ViewState["PrimaryEncoding"];

                if (primaryEncoding != null)
                    return (PrimaryEncodingMode)primaryEncoding;

                return PrimaryEncodingMode.Code128;
            }

            set
            {
                ViewState["UnitOfMeasure"] = value;
            }
        }

        [
           Bindable(false),
           Category("HIBC"),
           Description("N/A"),
           DefaultValue(typeof(Level),"Primary")
         ]
        public Level Level
        {
            get
            {
                object level = ViewState["Level"];

                if (level != null)
                    return (Level)level;

                return Level.Primary;
            }

            set
            {
                ViewState["Level"] = value;
            }
        }

        [
           Bindable(false),
           Category("HIBC"),
           Description("N/A"),
           DefaultValue(typeof(HIBCType),"LIC")
         ]
        public HIBCType HIBCType
        {
            get
            {
                object hibcType = ViewState["HIBCType"];

                if (hibcType != null)
                    return (HIBCType)hibcType;

                return HIBCType.LIC;
            }

            set
            {
                ViewState["HIBCType"] = value;
            }
        }

        [
           Bindable(false),
           Category("HIBC"),
           Description("N/A"),
           DefaultValue(""),
           DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
         ]
        public SecondaryData SecondaryData
        {
            get
            {
                return _secondaryData;
            }

            set
            {
                _secondaryData = value;
            }
        }

        #endregion

        #endregion

        #region Render

        protected override void Render(HtmlTextWriter output)
        {
            string toolTip = string.Empty;

            SetTmpFileName();
            Bitmap bitmapBarCode = GenerateBarcode();

            bool useBorder = false;

            if (BorderStyle != BorderStyle.NotSet && BorderWidth.Value > 0)
            {
                useBorder = true;
            }

            if (useBorder)
            {
                string style = string.Empty;
                style += "style=\"";
                style += string.Format("background-color:{0};",Color2Hex(this.BorderColor));
                style += string.Format("border-width:{0};", this.BorderWidth.ToString());
                style += string.Format("border-style:{0};", this.BorderStyle.ToString());
                style += "\"";

                output.Write("<table cellpadding=0 cellspacing = 0 {0}><tr><td>",style);
            }

            IEnumerator enumerator = this.Style.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                output.AddStyleAttribute((string)enumerator.Current, this.Style[(string)enumerator.Current]);
            }

            if (this.ToolTip != string.Empty)
            {
                output.AddAttribute("alt", this.ToolTip);
            }

            if ((HttpContext.Current != null && Page.Request.Browser.Browser.ToUpper() == "IE") || HttpContext.Current == null)
            {
                bitmapBarCode.Save(_tmpFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                output.AddAttribute("src", string.Format("file://{0}", _tmpFileName).Replace('\\', '/'));
            }
            else
            {
                output.AddAttribute("src",string.Format("data:image/gif;base64,{0}", ImageToBase64.GetBase64StringFromImage(bitmapBarCode)));
            }

            output.RenderBeginTag(HtmlTextWriterTag.Img);
            output.RenderEndTag();

            if (useBorder)
            {
                output.Write("</td></tr></table>");
            }
        }

        public string Color2Hex(System.Drawing.Color color)
        {
            if (color.IsEmpty)
                return "#FFFFFF";
            else
                return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        #endregion

        #region Generation

        private void SetTmpFileName()
        {
            if (_tmpFileName == null)
            {
                _tmpFileName = Path.GetTempFileName();
            }
            else
            {
                try
                {
                    System.IO.File.Delete(_tmpFileName);
                }
                catch
                {
                    _tmpFileName = Path.GetTempFileName();
                }
            }

           // _tmpFileName = @"c:\a.jpg";
        }

        private Bitmap GenerateBarcode()
        {
            if (this.Mode == Mode.Standard)
            {
                return GenerateBarcodeStandard();
            }
            else
            {
                return GenerateBarcodeHIBC();
            }
        }

        private Bitmap GenerateBarcodeStandard()
        {
            Bitmap result = null;
            Size currentSize = new Size(0, 0);

            IBarcodeEncoder enc = GetEncoder(this.BarcodeType);

            if (enc == null)
            {
                throw new Exception("That encoder is not implemented yet.");
            }

            try
            {
                enc.Text = this.Data;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }

            BarcodeRenderMode mode = BarcodeRenderMode.None;
            if (this.Guarded)
                mode |= BarcodeRenderMode.Guarded;
            if (this.Notched)
                mode |= BarcodeRenderMode.Notched;
            if (this.Numbered)
                mode |= BarcodeRenderMode.Numbered;
            if (this.Braced)
                mode |= BarcodeRenderMode.Braced;
            if (this.Boxed)
                mode |= BarcodeRenderMode.Boxed;

            enc.Sizer.Mode = mode;
            enc.Sizer.DPI = (float)this.Dpi.Value;

            bool calculResult = CalculateSize(ref currentSize);
            if (calculResult == false)
            {
                throw new Exception("Error calculating the size.");
            }

            if (!enc.Sizer.IsValidSize(currentSize))
            {
                throw new Exception("Invalid size.");
            }

            IBarcodeGenerator gen = enc.Generator;
            result = gen.GenerateBarcode(currentSize);

            return result;
        }

        private IBarcodeEncoder GetEncoder(BarcodeType type)
        {
            switch (type)
            {
                case BarcodeType.EAN13:
                    return new EAN13Encoder();
                case BarcodeType.EAN8:
                    return new EAN8Encoder();
                case BarcodeType.UPCA:
                    return new UPCAEncoder();
                case BarcodeType.UPCE:
                    return new UPCEEncoder();
                case BarcodeType.UPC2_5:
                    return new UPC25Encoder();
                case BarcodeType.ISBN:
                    return new ISBNEncoder();
                case BarcodeType.ISBN13:
                    return new ISBN13Encoder();
                case BarcodeType.ISSN:
                    return new ISSNEncoder();
                case BarcodeType.Postnet:
                    return new PostnetEncoder();
                case BarcodeType.Planet:
                    return new PlanetEncoder();
                case BarcodeType.Code11:
                    return new Code11Encoder();
                case BarcodeType.Code39:
                    return new Code39Encoder();
                case BarcodeType.Code93:
                    return new Code93Encoder();
                case BarcodeType.Code128:
                    return new Code128Encoder();
                case BarcodeType.MSI:
                    return new MSIEncoder();
                case BarcodeType.S2of5:
                    return new S2of5Encoder();
                case BarcodeType.I2of5:
                    return new I2of5Encoder();
                case BarcodeType.Codabar:
                    return new CodabarEncoder();
                case BarcodeType.EAN_Composite:
                    return new EANCompositeEncoder();
                case BarcodeType.ITF14:
                    return new ITF14Encoder();
                case BarcodeType.HIBC:
                    return null;
                case BarcodeType.EAN128:
                    return new ActiveUp.WebControls.EAN128Encoder();
                default:
                    return null;
            }
        }

        private bool CalculateSize(ref Size currentSize)
        {
            IBarcodeEncoder enc = GetEncoder(this.BarcodeType);
            if (enc == null)
                throw new Exception("That encoder is not implemented yet.");

            IBarcodeSizer sizer = enc.Sizer;
            IBarcodeModularSizer mSizer = sizer as IBarcodeModularSizer;

            try
            {
                enc.Text = this.Data;

                sizer = enc.Sizer;
                mSizer = sizer as IBarcodeModularSizer;

                sizer.DPI = (float)this.Dpi.Value;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }

            BarcodeRenderMode mode = BarcodeRenderMode.None;
            if (this.Guarded)
                mode |= BarcodeRenderMode.Guarded;
            if (this.Notched)
                mode |= BarcodeRenderMode.Notched;
            if (this.Numbered)
                mode |= BarcodeRenderMode.Numbered;
            if (this.Braced)
                mode |= BarcodeRenderMode.Braced;
            if (this.Boxed)
                mode |= BarcodeRenderMode.Boxed;

            sizer.Mode = mode;

            if (this.Height.Value < sizer.Height)
                this.Height = new Unit(sizer.Height);

            Size actualSize;
            if (sizer.DPI == 0 || mSizer == null)
            {
                //Logical mode, or simple fixed size.
                actualSize = sizer.GetValidSize(new Size((int)sizer.Width, (int)Height.Value));
                SetSizeDisplay(actualSize,ref currentSize);
            }
            else
            {
                actualSize = sizer.GetValidSize(new Size((int)sizer.Width, (int)Height.Value));
                SetSizeDisplay(actualSize, ref currentSize);
            }

            return true;
        }

        private void SetSizeDisplay(Size p, ref Size currentSize)
        {
            currentSize = p;
        }

        private Bitmap GenerateBarcodeHIBC()
        {
            ActiveUp.WebControls.SecondaryDataHibc secondData = new ActiveUp.WebControls.SecondaryDataHibc();
            if (SecondaryData.Date != "")
            {
                try
                {
                    secondData.Date = DateTime.Parse(SecondaryData.Date);
                }
                catch (Exception)
                {
                    throw new Exception("Invalid date format in the Date box.");
                }
            }

            if (SecondaryData.Quantity != "")
            {
                try
                {
                    secondData.Quantity = int.Parse(SecondaryData.Quantity);
                }
                catch (Exception)
                {
                    throw new Exception("The value in the quantity box must be a number between 0 and 99999.");
                }
            }

            if (SecondaryData.Serial != "")
                secondData.Lot = SecondaryData.Serial;

            secondData.DateFormat = this.SecondaryData.DateFormat;
            
            Regex licCheck = new Regex("[A-Z][A-Z0-9]{3}", RegexOptions.IgnoreCase);
            Regex pcnCheck = new Regex("[A-Z0-9]{1,13}", RegexOptions.IgnoreCase);
            Regex gs1Check = new Regex("[0-9]{12}");

            if (this.HIBCType == HIBCType.LIC)
            {
                //Check the LIC and PCN.
                if (this.LicGs1 == "" || !licCheck.IsMatch(this.LicGs1))
                {
                    throw new Exception("The LIC is not formatted correctly.");
                }

                if (this.Pcn != "" && !pcnCheck.IsMatch(this.Pcn))
                {
                    throw new Exception("The PCN is not formatted correctly.");
                }
            }
            else
            {
                //GS1
                if (this.LicGs1 == "" || !gs1Check.IsMatch(this.LicGs1))
                {
                    throw new Exception("The GS1 is not formatted correctly.");
                }

                if (this.Pcn != "" && !pcnCheck.IsMatch(this.Pcn))
                {
                    throw new Exception("The PCN is not formatted correctly.");
                }
            }

            byte uom;
            try
            {
                uom = (byte)this.UnitOfMeasure;
            }
            catch (Exception)
            {
                throw new Exception("The Unit of Measurement is not set.");
            }

            IBarcodeEncoder[] encoders;
            ActiveUp.WebControls.PrimaryEncodingMode pMode;
            ActiveUp.WebControls.SecondaryEncodingMode sMode;


            try
            {
                pMode = this.PrimaryEncoding;
            }
            catch (Exception)
            {
                throw new Exception("An error occured in parsing the primary encoding mode.");
            }

            try
            {
                sMode = this.SecondaryData.SecondaryEncoding;
            }
            catch (Exception)
            {
                throw new Exception("An error occured in parsing the secondary encoding mode.");
            }

            try
            {
                if (this.HIBCType == HIBCType.LIC)
                {
                    encoders = ActiveUp.WebControls.HIBCEncoder.EncodeHIBC(this.LicGs1, this.Pcn, uom, secondData, pMode, sMode);
                }
                else
                {
                    encoders = ActiveUp.WebControls.HIBCEncoder.EncodeGS1(uom.ToString() + this.LicGs1, (this.Pcn == "") ? null : this.Pcn, secondData, pMode, sMode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured:\n" + ex.Message);
            }

            encoders[0].Sizer.DPI = (float)this.Dpi.Value;
            if (encoders.Length == 2)
                encoders[1].Sizer.DPI = (float)this.Dpi.Value;

            encoders[0].Sizer.Mode = BarcodeRenderMode.Guarded | BarcodeRenderMode.Numbered;
            if (encoders.Length == 2)
                encoders[1].Sizer.Mode = BarcodeRenderMode.Guarded | BarcodeRenderMode.Numbered;

            Bitmap bmpPrimary = null, bmpSecondary = null;

            bmpPrimary = encoders[0].Generator.GenerateBarcode(encoders[0].Sizer.Size);
            if (encoders.Length == 2)
                bmpSecondary = encoders[1].Generator.GenerateBarcode(encoders[1].Sizer.Size);

            if (this.Level == Level.Primary)
                return bmpPrimary;
            else
                return bmpSecondary;
        }

        #endregion
    }
}
