using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ActiveUp.WebControls {
    /// <summary>
    /// A class that encodes data into Interleaved 2 of 5 barcodes.
    /// </summary>
    /// <remarks>Interleaved 2 of 5 is a symbology that is commonly used in industry.  It is related to
    /// Standard 2 of 5 (See <see cref="Barcodes.S2of5Encoder"/>), except instead of encoding each digit
    /// as a series of bars of varying widths, Interleaved 2 of 5 encodes pairs of digits as a series of
    /// bars and spaces.  It is also the basis for ITF14.  Interleaved 2 of 5 can only encode digit strings
    /// of even length.</remarks>
    /// <seealso cref="Barcodes.I2of5Generator"/>
    /// <seealso cref="Barcodes.I2of5Sizer"/>
    public class I2of5Encoder : BarcodeEncoder {
        private void CheckAndEncodeText(string value, out byte[] data) {
            data = null;
            if (value == null)
                return;

            if ((value.Length & 1) == 1) {
                //Odd; generate a check digit.
                data = new byte[value.Length + 1];
            } else {
                data = new byte[value.Length];
            }

            int i = 0;
            for (; i < value.Length; i++) {
                int val = "0123456789".IndexOf(value[i]);
                if (val == -1)
                    throw new ArgumentException("The specified string contains non-digit symbols.", "Digit");
                data[i] = (byte)val;
            }

            if (i != data.Length)
                data[i] = CalculateChecksum(data, i);
        }

        private byte CalculateChecksum(byte[] data, int length) {
            int checkSum = 0;
            int weight = 3;

            for (int i = length - 1; i >= 0; i--) {
                checkSum += weight * data[i];
                weight = 4 - weight;
            }
            return (byte)((10 - checkSum % 10) % 10);
        }

        private string text;
        /// <summary>
        /// Gets or sets the data to be encoded in Interleaved 2 of 5, as a text string.
        /// </summary>
        /// <value>The text to be encoded.</value>
        /// <remarks>Interleaved 2 of 5 can only encode even-length digit strings. However, this encoder will accept
        /// odd-length digit strings.  If the digit string's length is odd, the encoder will add a mod-10 check digit
        /// the end, to make the completed string even-length.  The checksum is calculated by multiplying the rightmost
        /// digit by 3, the next digit by 1, the third digit by 3, and so on. The digits are then summed; the check
        /// digit is the value that must be added to reach the next multiple of ten.</remarks>
        /// <exception cref="System.ArgumentException">The data supplied to the encoder contains non-digit symbols.
        /// The <see cref="System.ArgumentException.ParamName"/> property will be set to "Digit".</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                byte[] encoded;
                CheckAndEncodeText(value, out encoded);
                text = value;
                GeneratorInstance.Data = encoded;
            }
        }

        /// <summary>
        /// Gets flags that represent the encoder's capabilities.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.Text"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.Text; }
        }

        /// <summary>
        /// Symbols that are encodable by this encoder.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>This encoder can only encode digits.  If any non-digit symbols are passed in, the encoder will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.I2of5Encoder"/> class.
        /// </summary>
        public I2of5Encoder() : base(new I2of5Generator()) { }
    }

    /// <summary>
    /// A class that encodes data into Standard (Industrial) 2 of 5 barcodes.
    /// </summary>
    /// <remarks>Standard 2 of 5 is a symbology that is commonly used in industry.  It encodes digits in
    /// patterns consisting of five bars per digit, with two of the five bars thicker than the other three
    /// (hence, 2 of 5).  While it is larger than Interleaved 2 of 5, it can encode an odd number of digits.</remarks>
    /// <seealso cref="Barcodes.S2of5Generator"/>
    /// <seealso cref="Barcodes.S2of5Sizer"/>
    public class S2of5Encoder : BarcodeEncoder {
        private bool useChecksum = false;
        /// <summary>
        /// Gets or sets whether a checksum digit should be appended to the barcode.
        /// </summary>
        /// <value>True if a checksum digit should be appended, false otherwise.</value>
        /// <remarks><para>The checksum digit is calculated by weighting the digits in
        /// odd positions (rightmost, third to rightmost, etc.) with a weight of three,
        /// and even-positioned digits a weight of one.  The digits are then summed, and
        /// the check digit is the value that must be added to reach the next multiple of
        /// ten.</para><para>If the value of this property is changed after the data has
        /// been set, the data will be re-encoded to include or exclude the check digit.
        /// </para></remarks>
        public bool UseChecksum {
            get { return useChecksum; }
            set { 
                useChecksum = value;
                byte[] encoded;
                CheckAndEncodeText(text, out encoded);
                GeneratorInstance.Data = encoded;
            }
        }

        private byte CalculateChecksum(byte[] data, int length) {
            int checkSum = 0;
            int weight = 3;

            for (int i = length - 1; i >= 0; i--) {
                checkSum += weight * data[i];
                weight = 4 - weight;
            }
            return (byte)((10 - checkSum % 10) % 10);
        }

        private void CheckAndEncodeText(string value, out byte[] data) {
            data = null;
            if (value == null)
                return;

            if (useChecksum)
                data = new byte[value.Length + 1];
            else
                data = new byte[value.Length];

            int i;
            for (i = 0; i < value.Length; i++) {
                int val = "0123456789".IndexOf(value[i]);
                if (val == -1)
                    throw new ArgumentException("The specified string contains non-digit characters.", "Digit");
                data[i] = (byte)val;
            }

            if (useChecksum)
                data[i] = CalculateChecksum(data, i);

            return;
        }

        private string text;
        /// <summary>
        /// Gets or sets the data to be encoded in Standard (Industrial) 2 of 5, as a text string.
        /// </summary>
        /// <value>The text to be encoded.</value>
        /// <remarks>Standard 2 of 5 can only encode digits. If <see cref="Barcodes.S2of5Encoder.UseChecksum"/>
        /// is true, a checksum digit will be appended.  The checksum is calculated by multiplying the rightmost
        /// digit by 3, the next digit by 1, the third digit by 3, and so on. The digits are then summed; the check
        /// digit is the value that must be added to reach the next multiple of ten.</remarks>
        /// <exception cref="System.ArgumentException">The data supplied to the encoder contains non-digit symbols.
        /// The <see cref="System.ArgumentException.ParamName"/> property will be set to "Digit".</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                byte[] encoded;
                CheckAndEncodeText(value, out encoded);
                text = value;
                GeneratorInstance.Data = encoded;
            }
        }

        /// <summary>
        /// Gets flags that represent the encoder's capabilities.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.Text"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.Text; }
        }

        /// <summary>
        /// Symbols that are encodable by this encoder.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>The encoder can only encode digits.  If any non-digit symbols are passed in, the encoder will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.S2of5Encoder"/> class.
        /// </summary>
        public S2of5Encoder() : base(new S2of5Generator()) { }
    }

    /// <summary>
    /// The base sizing class for the two 2 of 5 symbologies.
    /// </summary>
    /// <remarks>The two 2 of 5 symbologies' sizing requirements are virtually identical, with
    /// the only difference being the size of each digit's encoding.</remarks>
    public abstract class Base2of5Sizer : BarcodeSizer, IBarcodeModularSizer {
        private int moduleWidth=1;
        /// <summary>
        /// The module width of the barcode, in pixels.
        /// </summary>
        public int ModuleWidth {
            get { return moduleWidth; }
            set { moduleWidth = value; }
        }

        private int guardWidth=10;
        /// <summary>
        /// The guard (quiet) zone width, in pixels.
        /// </summary>
        /// <remarks>If the <see cref="BarcodeRenderMode.Boxed"/> flag is set, this width also includes
        /// a 5 module wide black stripe, for the left and right sides of the box.</remarks>
        public int GuardWidth {
            get {
                if ((Mode & BarcodeRenderMode.Boxed) != 0) {
                    return guardWidth + 6 * moduleWidth;
                } else if ((Mode & BarcodeRenderMode.Guarded) != 0)
                    return guardWidth;
                else
                    return 0;
            }
            set {guardWidth=value;}
        }

        private int length;
        /// <summary>
        /// The length of the encoded data, in digits.
        /// </summary>
        /// <remarks>This is used by the derived classes in their implementation of 
        /// <see cref="Barcodes.Base2of5Sizer.WidthWithoutGuards"/>.</remarks>
        public int Length {
            get { return length; }
            set { length = value; }
        }

        private void CalculateSizes() {
            if (DPI == 0) {
                moduleWidth = 1;
                guardWidth = 10;
            } else {
                float tenthMilsPerDot = 10000 / DPI;
                moduleWidth = (int)Math.Ceiling(module * 10 / tenthMilsPerDot);
                guardWidth = 10 * moduleWidth;
            }
        }

        /// <summary>
        /// The total width of the barcode, excluding any guard (quiet) zones, in pixels.
        /// </summary>
        public abstract int WidthWithoutGuards { get;}

        /// <summary>
        /// The total width of the barcode, in pixels, given the current DPI and module width.
        /// </summary>
        public override int Width {
            get { 
                int width=WidthWithoutGuards + GuardWidth * 2;
            return width;
            }
        }

        /// <summary>
        /// Gets the font height used for numbering the barcode.
        /// </summary>
        /// <value>The font height, in pixels.</value>
        public float FontHeight {
            get {
                if ((Mode & BarcodeRenderMode.Numbered) == 0)
                    return 0;
                if (DPI != 0)
                    return (float)Math.Ceiling(Math.Max(1250f, (1250 * module / 10f)) / (10000 / DPI));
                else
                    return 9f;
            }
        }

        /// <summary>
        /// The extra space added to the bottom of the barcode, for space to number the barcode.
        /// </summary>
        /// <value>The extra space, in pixels, at the bottom of the barcode.</value>
        public int FontHeightAddon {
            get {
                if ((Mode & BarcodeRenderMode.Numbered) != 0)
                    return (int)FontHeight + ModuleWidth;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gets the width of the top bracer bar, if present, in pixels.
        /// </summary>
        public int TopWidth {
            get {
                if ((Mode & (BarcodeRenderMode.Braced | BarcodeRenderMode.Boxed)) != 0)
                    return 5 * moduleWidth;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gets the width of the bottom bracer bar, if present, and the numbering, in pixels.
        /// </summary>
        public int BottomWidth {
            get {
                int height;
                if ((Mode & (BarcodeRenderMode.Braced | BarcodeRenderMode.Boxed)) != 0)
                    height = 5 * moduleWidth;
                else
                    height = 0;
                return FontHeightAddon + height;
            }
        }

        /// <summary>
        /// Gets the width of the left and right side bracing bars, in pixels.
        /// </summary>
        public int SideBrace {
            get {
                if ((Mode & BarcodeRenderMode.Boxed) != 0)
                    return 5 * moduleWidth;
                else
                    return 0;
            }
        }

        /// <summary>
        /// The minimum height of the barcode, in pixels.
        /// </summary>
        public override int Height {
            get {
                if ((Mode & (BarcodeRenderMode.Boxed | BarcodeRenderMode.Braced)) != 0)
                    return (FontHeightAddon + 1 + 10) * moduleWidth;
                else
                    return FontHeightAddon + 1;
            }
        }

        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>The height of all items added to the barcode, in pixels.</value>
        /// <remarks><para>For 2 of 5 barcodes, the extra height comes from text at the bottom of the barcode, 
        /// and the optional bracing or boxing of the barcode symbol.</para>
        /// <para>This property is meant to be used to generate a desired barcode height.  To calculate the height
        /// of the size to pass to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, multiply the desired height
        /// (in inches) by the DPI, and add this number to it.  </para><para>This property's value is potentially impacted by the value of <see cref="Barcodes.IBarcodeSizer.Mode"/>.
        /// The barcode render mode flags should be set before using this value.</para></remarks>
        public override int ExtraHeight {
            get { return Height - moduleWidth; }
        }

        /// <summary>
        /// Checks to see if a specified size is valid.
        /// </summary>
        /// <param name="size">A size to test for validity.</param>
        /// <returns>True if this size may be passed to <see cref="Barcodes.BarcodeGenerator.GenerateBarcode"/>, false otherwise.</returns>
        public override bool IsValidSize(Size size) {
            return (size.Width == Width && size.Height >= Height);
        }

        /// <summary>
        /// Given a size, returns the largest valid size contained by that size.
        /// </summary>
        /// <param name="size">A maximum size, from which to find a valid size.</param>
        /// <returns>A valid size which may be passed to the <see cref="Barcodes.BarcodeGenerator.GenerateBarcode"/>.</returns>
        /// <exception cref="System.ArgumentException">The specified size is smaller than the minimum size in one or both dimensions.</exception>
        public override Size GetValidSize(Size size) {
            if (size.Width < Width || size.Height < Height)
                throw new ArgumentException("The specified size is smaller than the minimum size.");
            return new Size(Width, size.Height);
        }

        /// <summary>
        /// The current DPI (dots-per-inch) of the barcode.
        /// </summary>
        /// <value>The current DPI (dots-per-inch) of the barcode.</value>
        /// <remarks>
        /// This value represents the DPI that the barcode will be printed at.  This value defaults to zero, which represents "logical" mode.
        /// In "logical" mode, the generated barcode represents the relative sizing and positioning of barcode elements, but they are not
        /// necessarily in the proper size for printing.
        /// 
        /// If the value is greater than zero, the barcode generator will generate a barcode using the specified DPI.
        /// </remarks>
        /// <exception cref="System.ArgumentException">When setting the DPI, the value specified was less than zero.</exception>
        public override float DPI {
            get {
                return base.DPI;
            }
            set {
                base.DPI = value;
                CalculateSizes();
            }
        }


        /// <summary>
        /// The rendering mode flags, which control the way the barcode is rendered.
        /// </summary>
        /// <value>The flags which control which aspects of the barcode are rendered.</value>
        /// <remarks>The 2 of 5 encoders recognize all rendering flags except <see cref="Barcodes.BarcodeRenderMode.Notched"/>.
        /// Setting the <see cref="Barcodes.BarcodeRenderMode.Boxed"/> flag automatically sets the
        /// <see cref="Barcodes.BarcodeRenderMode.Guarded"/> flag.</remarks>
        public override BarcodeRenderMode Mode {
            get {
                return base.Mode;
            }
            set {
                if ((value & BarcodeRenderMode.Boxed) != 0)
                    base.Mode = value | BarcodeRenderMode.Guarded;
                else
                    base.Mode = value;
            }
        }

        #region IBarcodeModularSizer Members

        private float module = 10f;
        /// <summary>
        /// Gets and sets module width of the barcode (in mils).
        /// </summary>
        /// <value>The module width of the barcode, in mils (1 mil is 1/1000 of an inch). The default value is 10 mils (.254 mm).</value>
        /// <remarks>The module width is the width of the narrowest bar.  The widths of spaces,
        /// wide bars, wide spaces, quiet zones, etc. are multiples of this module width.  Changing the module width of a barcode
        /// changes the total width of the barcode, as returned by <see cref="Barcodes.Base2of5Sizer.Width"/>.</remarks>
        /// <exception cref="System.ArgumentException">The specified module width is less than <see cref="Barcodes.Base2of5Sizer.MinimumModule"/>.</exception>
        public float Module {
            get {
                return module;
            }
            set {
                if (value < 7.5f)
                    throw new ArgumentException("The specified module width is smaller than the minimum module width.");
                module = value;
                CalculateSizes();
            }
        }

        /// <summary>
        /// Gets the minimum module width, in mils.
        /// </summary>
        /// <value>The minimum module width, in mils. (1 mil is 1/1000th of an inch, or .025mm). Always returns 7.5 mils (.19 mm).</value>
        public float MinimumModule {
            get { return 7.5f; }
        }

        #endregion
    }

    /// <summary>
    /// Controls the sizing of Interleaved 2 of 5 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.I2of5Encoder"/>
    /// <seealso cref="Barcodes.I2of5Generator"/>
    public class I2of5Sizer : Base2of5Sizer {
        /// <summary>
        /// The total width of the barcode, excluding any guard (quiet) zones, in pixels.
        /// </summary>
        public override int WidthWithoutGuards {
            get { return (9 * Length + 9) * ModuleWidth; }
        }
    }

    /// <summary>
    /// Controls the sizing of Standard (Industrial) 2 of 5 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.S2of5Encoder"/>
    /// <seealso cref="Barcodes.S2of5Generator"/>
    public class S2of5Sizer : Base2of5Sizer {
        /// <summary>
        /// The total width of the barcode, excluding any guard (quiet) zones, in pixels.
        /// </summary>
        public override int WidthWithoutGuards {
            get { return (14 * Length + 15) * ModuleWidth; }
        }
    }

    /// <summary>
    /// Class which generates Interleaved 2 of 5 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.I2of5Encoder"/>
    /// <seealso cref="Barcodes.I2of5Sizer"/>
    public class I2of5Generator : BarcodeGenerator {
        private byte[] data=null;
        private BitArray encodedData = null;

        #region Encoding Table
        private bool[,] encodings ={
            {false,false,true,true,false},
            {true,false,false,false,true},
            {false,true,false,false,true},
            {true,true,false,false,false},
            {false,false,true,false,true},
            {true,false,true,false,false},
            {false,true,true,false,false},
            {false,false,false,true,true},
            {true,false,false,true,false},
            {false,true,false,true,false}
        };

        #endregion

        private void EncodeData() {
            if (encodedData != null)
                return;

            encodedData = new BitArray(9 * data.Length + 9);

            int pos=0;
            encodedData[pos++] = true;
            encodedData[pos++] = false;
            encodedData[pos++] = true;
            encodedData[pos++] = false;

            for (int i = 0; i < data.Length; i += 2) {
                byte bar = data[i];
                byte space = data[i + 1];
                for (int j = 0; j < 5; j++) {
                    encodedData[pos++] = true;
                    if (encodings[bar, j]) {
                        encodedData[pos++] = true;
                        encodedData[pos++] = true;
                    }

                    encodedData[pos++] = false;
                    if (encodings[space, j]) {
                        encodedData[pos++] = false;
                        encodedData[pos++] = false;
                    }
                }
            }

            encodedData[pos++] = true;
            encodedData[pos++] = true;
            encodedData[pos++] = true;
            encodedData[pos++] = false;
            encodedData[pos++] = true;

            ((Base2of5Sizer)Sizer).Length = data.Length;
        }

        /// <summary>
        /// Gets or sets the byte array of encoded data used by the generator to generate the barcode.
        /// </summary>
        /// <value>The byte array of encoded data.</value>
        /// <remarks>This data only has very basic validation to ensure that it does not cause exceptions
        /// when used (such as array overruns). It is not checked for validity.  The user should not set this
        /// directly, instead, it should be set by an encoder class.  This property does <b>not</b> make a
        /// copy of the byte array that is supplied to it, it only keeps a reference.  The generator's behavior
        /// if the underlying byre array is modified, is undefined.</remarks>
        /// <exception cref="System.ArgumentException">The specified byte array is invalid, in either length or content.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                if (data!=null && ((data.Length & 1) == 1))
                    throw new ArgumentException("The specified data is not encodable; it must be of even length.");
                data = value;
                encodedData = null;
                if (value != null)
                    EncodeData();
                else
                    ((Base2of5Sizer)Sizer).Length = 0;
            }
        }

        /// <summary>
        /// Gets the barcode generator capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/>|<see cref="Barcodes.BarcodeGeneratorFlags.VariableLength"/>.</value>
        public override BarcodeGeneratorFlags Flags {
            get { return BarcodeGeneratorFlags.VariableLength | BarcodeGeneratorFlags.Linear; }
        }

        /// <summary>
        /// Generates an Inteleaved 2 of 5 barcode of a specified size, using the data that has been set previously by its corresponding
        /// encoder.
        /// </summary>
        /// <param name="size">The size of the barcode to return.</param>
        /// <returns>A bitmap of the barcode, of the specified size.</returns>
        /// <exception cref="System.ArgumentException">The specified size is invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The data that is to be encoded has not been set yet.</exception>
        public override Bitmap GenerateBarcode(Size size) {
            if (data == null)
                throw new InvalidOperationException("The data for the generator has not been set.");
            if (!Sizer.IsValidSize(size))
                throw new ArgumentException("The specified size is not valid.");
            EncodeData();

            Bitmap bm = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppRgb);
            Graphics g = Graphics.FromImage(bm);

            int module, guard;
            module = ((I2of5Sizer)Sizer).ModuleWidth;
            guard = ((I2of5Sizer)Sizer).GuardWidth;

            I2of5Sizer iSizer = (I2of5Sizer)Sizer;
            if (iSizer.TopWidth!=0)
                g.FillRectangle(Brushes.White,Rectangle.FromLTRB(iSizer.SideBrace,iSizer.TopWidth,size.Width-iSizer.SideBrace,size.Height-iSizer.BottomWidth));
            else 
                g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);

            int pos = guard;
            for (int i = 0; i < encodedData.Length; i++) {
                if (encodedData[i])
                    g.FillRectangle(Brushes.Black, pos, 0, module, size.Height);
                pos += module;
            }

            if (iSizer.FontHeight != 0) {
                string text = "";
                foreach (byte b in data)
                    text += b.ToString();

                g.FillRectangle(Brushes.White, Rectangle.FromLTRB(0, size.Height - iSizer.FontHeightAddon, size.Width, size.Height));
                g.DrawString(text, FontHolder.GenerateFont(iSizer.FontHeight), Brushes.Black, Rectangle.FromLTRB(0, size.Height - iSizer.FontHeightAddon, size.Width, size.Height), FontHolder.CenterJustify);

            }

            g.Dispose();
            return bm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.I2of5Generator"/> class.
        /// </summary>
        public I2of5Generator() : base(new I2of5Sizer()) { }
    }

    /// <summary>
    /// Class which generates Standard (Industrial) 2 of 5 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.S2of5Encoder"/>
    /// <seealso cref="Barcodes.S2of5Sizer"/>
    public class S2of5Generator : BarcodeGenerator {
        private byte[] data=null;
        private BitArray encodedData = null;

        #region Encoding Table
        private bool[,] encodings ={
            {true,false,true,false,true,true,true,false,true,true,true,false,true,false},
            {true,true,true,false,true,false,true,false,true,false,true,true,true,false},
            {true,false,true,true,true,false,true,false,true,false,true,true,true,false},
            {true,true,true,false,true,true,true,false,true,false,true,false,true,false},
            {true,false,true,false,true,true,true,false,true,false,true,true,true,false},
            {true,true,true,false,true,false,true,true,true,false,true,false,true,false},
            {true,false,true,true,true,false,true,true,true,false,true,false,true,false},
            {true,false,true,false,true,false,true,true,true,false,true,true,true,false},
            {true,true,true,false,true,false,true,false,true,true,true,false,true,false},
            {true,false,true,true,true,false,true,false,true,true,true,false,true,false}
        };
        #endregion

        private void EncodeData() {
            if (encodedData != null)
                return;

            encodedData = new BitArray(14 * data.Length + 15);

            int pos=0;
            encodedData[pos++] = true;
            encodedData[pos++] = true;
            encodedData[pos++] = false;
            encodedData[pos++] = true;
            encodedData[pos++] = true;
            encodedData[pos++] = false;
            encodedData[pos++] = true;
            encodedData[pos++] = false;

            foreach (byte b in data) {
                for (int i = 0; i < 14; i++)
                    encodedData[pos++] = encodings[b, i];
            }

            encodedData[pos++] = true;
            encodedData[pos++] = true;
            encodedData[pos++] = false;
            encodedData[pos++] = true;
            encodedData[pos++] = false;
            encodedData[pos++] = true;
            encodedData[pos++] = true;

            ((Base2of5Sizer)Sizer).Length = data.Length;
        }

        /// <summary>
        /// Gets or sets the byte array of encoded data used by the generator to generate the barcode.
        /// </summary>
        /// <value>The byte array of encoded data.</value>
        /// <remarks>This data only has very basic validation to ensure that it does not cause exceptions
        /// when used (such as array overruns). It is not checked for validity.  The user should not set this
        /// directly, instead, it should be set by an encoder class.  This property does <b>not</b> make a
        /// copy of the byte array that is supplied to it, it only keeps a reference.  The generator's behavior
        /// if the underlying byre array is modified, is undefined.</remarks>
        /// <exception cref="System.ArgumentException">The specified byte array is invalid, in either length or content.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                data = value;
                encodedData = null;
                if (value != null)
                    EncodeData();
                else
                    ((Base2of5Sizer)Sizer).Length = 0;
            }
        }

        /// <summary>
        /// Gets the barcode generator capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/>|<see cref="Barcodes.BarcodeGeneratorFlags.VariableLength"/>.</value>
        public override BarcodeGeneratorFlags Flags {
            get { return BarcodeGeneratorFlags.Linear | BarcodeGeneratorFlags.VariableLength; }
        }

        /// <summary>
        /// Generates a Standard (Industrial) 2 of 5 barcode of a specified size, using the data that has been set previously by its corresponding
        /// encoder.
        /// </summary>
        /// <param name="size">The size of the barcode to return.</param>
        /// <returns>A bitmap of the barcode, of the specified size.</returns>
        /// <exception cref="System.ArgumentException">The specified size is invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The data that is to be encoded has not been set yet.</exception>
        public override Bitmap GenerateBarcode(Size size) {
            if (data == null)
                throw new InvalidOperationException("The data for the generator has not been set.");
            if (!Sizer.IsValidSize(size))
                throw new ArgumentException("The specified size is not valid.");
            EncodeData();

            Bitmap bm = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppRgb);
            Graphics g = Graphics.FromImage(bm);

            int module, guard;
            module = ((S2of5Sizer)Sizer).ModuleWidth;
            guard = ((S2of5Sizer)Sizer).GuardWidth;

            S2of5Sizer sSizer = (S2of5Sizer)Sizer;
            if (sSizer.TopWidth != 0)
                g.FillRectangle(Brushes.White, Rectangle.FromLTRB(sSizer.SideBrace, sSizer.TopWidth, size.Width - sSizer.SideBrace, size.Height - sSizer.BottomWidth));
            else
                g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);

            int pos = guard;
            for (int i = 0; i < encodedData.Length; i++) {
                if (encodedData[i])
                    g.FillRectangle(Brushes.Black, pos, 0, module, size.Height);
                pos += module;
            }

            if (sSizer.FontHeight != 0) {
                string text = "";
                foreach (byte b in data)
                    text += b.ToString();

                g.FillRectangle(Brushes.White, Rectangle.FromLTRB(0, size.Height - sSizer.FontHeightAddon, size.Width, size.Height));
                g.DrawString(text, FontHolder.GenerateFont(sSizer.FontHeight), Brushes.Black, Rectangle.FromLTRB(0, size.Height - sSizer.FontHeightAddon, size.Width, size.Height), FontHolder.CenterJustify);

            }

            g.Dispose();
            return bm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.S2of5Generator"/> class.
        /// </summary>
        public S2of5Generator() : base(new S2of5Sizer()) { }
    }
}