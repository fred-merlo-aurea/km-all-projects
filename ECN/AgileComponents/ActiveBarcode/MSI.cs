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
    /// The checksumming mode for the MSI encoder to use.
    /// </summary>
    /// <remarks><para>The mod-10 checksum is calculated by separating the number to be encoded into two
    /// numbers, consisting of the digits in odd positions and the digits in even positions (for instance,
    /// "12345" becomes "135" and "24").  The number containing the rightmost digit is multiplied by two,
    /// and the digits from this number and the number containing the second-rightmost digit. (135 becomes 270,
    /// and the digits are added: 2+7+0+2+4=15.)  The check digit is this number subtracted from the next
    /// highest multiple of 10.</para>
    /// <para>The mod-11 checksum is calculated by weighting the digits of the number to be encoded, starting
    /// with a weight of 2 at the rightmost digit, and increasing by 1 for each digit to the left. Once the weight
    /// goes over 7, it is reset back to 2, so the 7th digit will have a weight of 2, the 8th, a weight of 3, etc.
    /// The weighted digits are summed, and the check digit is this value mod 11.</para></remarks>
    public enum MSIChecksum {
        /// <summary>
        /// No checksum is appended.
        /// </summary>
        None,
        /// <summary>
        /// A mod-10 checksum digit is added.
        /// </summary>
        Mod10,
        /// <summary>
        /// A mod-11 checksum digit is added.
        /// </summary>
        Mod11,
        /// <summary>
        /// A mod-10 checksum digit is added, followed by a second mod-10 checksum digit.
        /// </summary>
        Mod1010,
        /// <summary>
        /// A mod-11 checksum digit is added, followed by a mod-10 checksum digit.
        /// </summary>
        Mod1110
    }

    /// <summary>
    /// An encoder that produces MSI barcodes.
    /// </summary>
    public class MSIEncoder : BarcodeEncoder {
        private byte CalculateMod10(byte[] data, int len) {
            int checkSum=0;
            int carry=0, product;
            bool odd=true;

            for (int i = len - 1; i >= 0; i--, odd = !odd) {
                if (odd) {
                    product = data[i] * 2 + carry;
                    carry = product / 10;
                    checkSum += product % 10;
                } else {
                    checkSum += data[i];
                }
            }

            checkSum += carry;
            return (byte)(checkSum % 10);
        }

        private byte CalculateMod11(byte[] data, int len) {
            int checkSum=0;
            int weight = 2;

            for (int i = len - 1; i >= 0; i--) {
                checkSum += weight * data[i];
                weight++;
                if (weight > 7)
                    weight = 2;
            }

            return (byte)(checkSum%11);
        }

        private void CheckAndEncodeText(string value, out byte[] data) {
            data = null;
            if (value == null)
                return;

            switch (checkMode) {
                case MSIChecksum.None:
                    data = new byte[value.Length];
                    break;
                case MSIChecksum.Mod10:
                case MSIChecksum.Mod11:
                    data = new byte[value.Length + 1];
                    break;
                case MSIChecksum.Mod1010:
                case MSIChecksum.Mod1110:
                    data = new byte[value.Length + 2];
                    break;
            }

            int i;
            for (i = 0; i < value.Length; i++) {
                int val="0123456789".IndexOf(value[i]);
                if (val == -1)
                    throw new ArgumentException("The specified data contains non-digit characters.", "Digit");
                data[i] = (byte)val;
            }

            switch (checkMode) {
                case MSIChecksum.Mod10:
                    data[i] = CalculateMod10(data, i);
                    break;
                case MSIChecksum.Mod11:
                    data[i] = CalculateMod11(data, i);
                    break;
                case MSIChecksum.Mod1010:
                    data[i] = CalculateMod10(data, i);
                    data[i + 1] = CalculateMod10(data, i + 1);
                    break;
                case MSIChecksum.Mod1110:
                    data[i] = CalculateMod11(data, i);
                    data[i + 1] = CalculateMod10(data, i + 1);
                    break;
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <remarks>Always throws an <see cref="System.NotSupportedException"/> exception.</remarks>
        public override byte[] Data {
            get {
                throw new NotSupportedException("MSI cannot encode binary data.");
            }
            set {
                throw new NotSupportedException("MSI cannot encode binary data.");
            }
        }

        private string text;
        /// <summary>
        /// Gets or sets the data to be encoded by the encoder.
        /// </summary>
        /// <value>The data that is encoded into the barcode.</value>
        /// <exception cref="System.ArgumentException">The data specified could not be encoded into an MSI barcode because
        /// it contained non-digit characters.  The <see cref="System.ArgumentException.ParamName"/> property is set to
        /// "Digit".</exception>
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
        /// Gets the barcode encoder capabilities.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.Text"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.Text; }
        }

        /// <summary>
        /// Gets the symbols that can be encoded into MSI.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>The encoder can only encode digits.  If any non-digit symbol is passed to the encoder, it will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        private MSIChecksum checkMode;
        /// <summary>
        /// Gets the checksum mode that the encoder is using.
        /// </summary>
        /// <value>The checksum mode.</value>
        /// <remarks>The checksum mode can only be set when the encoder is constructed.</remarks>
        public MSIChecksum ChecksumMode {
            get {return checkMode;}
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.MSIEncoder"/> class, with no checksum.
        /// </summary>
        public MSIEncoder() : base(new MSIGenerator()) {
            checkMode=MSIChecksum.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.MSIEncoder"/> class, with the specified checksum mode.
        /// </summary>
        /// <param name="mode">The checksum mode to use.</param>
        public MSIEncoder(MSIChecksum mode) : base(new MSIGenerator()) {
            checkMode=mode;
        }
    }

    /// <summary>
    /// A sizer that controls the size of MSI barcodes.
    /// </summary>
    public class MSISizer : BarcodeSizer, IBarcodeModularSizer {
        private int moduleWidth;
        /// <summary>
        /// Returns the current module width, in pixels.
        /// </summary>
        public int ModuleWidth { get { return moduleWidth; } }
        private int guardWidth;
        /// <summary>
        /// Returns the current guard zone width, in pixels.
        /// </summary>
        public int GuardWidth {
            get {
                if ((Mode & BarcodeRenderMode.Guarded) != 0)
                    return guardWidth;
                else
                    return 0;
            }
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
        /// Gets or sets the current DPI (dots-per-inch) of the barcode.
        /// </summary>
        /// <value>The current DPI (dots-per-inch) of the barcode.</value>
        /// <remarks>
        /// This value represents the DPI that the barcode will be printed at.  This value defaults to zero, which represents "logical" mode.
        /// In "logical" mode, the generated barcode represents the relative sizing and positioning of barcode elements, but they are not
        /// necessarily in the proper size for printing.
        /// 
        /// If the value is greater than zero, the barcode generator will generate a barcode using the specified DPI.
        /// </remarks>
        /// <exception cref="System.ArgumentException">The value specified was less than zero.</exception>
        public override float DPI {
            get {
                return base.DPI;
            }
            set {
                base.DPI = value;
                CalculateSizes();
            }
        }

        private int length;
        /// <summary>
        /// Gets or sets the number of digits to be encoded into the barcode.  Set by the generator.
        /// </summary>
        /// <value>The number of digits to be encoded.</value>
        public int Length {
            get { return length; }
            set { length = value; }
        }

        /// <summary>
        /// Gets the height of the font to use to number the barcode, in pixels.
        /// </summary>
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
        /// Gets the amount of white space to add to the bottom of the barcode, in pixels, for numbering.
        /// </summary>
        public int FontHeightAddon {
            get {
                if ((Mode & BarcodeRenderMode.Numbered) != 0)
                    return (int)FontHeight + ModuleWidth;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gets the total width of the barcode, in pixels, without the guard zones.
        /// </summary>
        public int WidthWithoutGuard {
            get { return (length * 12 + 7)*moduleWidth; }
        }

        /// <summary>
        /// Gets the total width of the barcode, in pixels.
        /// </summary>
        public override int Width {
            get { return WidthWithoutGuard + GuardWidth * 2; }
        }

        /// <summary>
        /// Gets the minimum height of the barcode, in pixels.
        /// </summary>
        public override int Height {
            get { return 1+FontHeightAddon; }
        }

        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>The height of all items added to the barcode, in pixels.</value>
        /// <remarks><para>For MSI barcodes, the only extra height comes from the text at the bottom of the barcode.</para>
        /// <para>This property is meant to be used to generate a desired barcode height.  To calculate the height
        /// of the size to pass to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, multiply the desired height
        /// (in inches) by the DPI, and add this number to it.</para><para>This property's value is potentially impacted by the value of <see cref="Barcodes.IBarcodeSizer.Mode"/>.
        /// The barcode render mode flags should be set before using this value.</para></remarks>
        public override int ExtraHeight {
            get { return FontHeightAddon; }
        }

        /// <summary>
        /// Checks to see if a specified size is valid.
        /// </summary>
        /// <param name="size">The size to check for validity.</param>
        /// <returns>True if the size is valid, false otherwise.</returns>
        public override bool IsValidSize(Size size) {
            return (size.Width==Width && size.Height>=Height);
        }

        /// <summary>
        /// Returns the largest valid size that is contained within the specified size.
        /// </summary>
        /// <param name="size">The maximum size to use.</param>
        /// <returns>The largest valid size that is less than or equal to the specified size.</returns>
        /// <exception cref="System.ArgumentException">The specified size is smaller than the minimum size,
        /// in one or both dimensions.</exception>
        public override Size GetValidSize(Size size) {
            if (size.Width < Width || size.Height < Height)
                throw new ArgumentException("The specified size is smaller than the minimum size.");
            return new Size(Width, size.Height);
        }

        /// <summary>
        /// Gets the minimum module width, in mils.
        /// </summary>
        /// <value>Always returns 7.5 mils (.19 mm). (1 mil is 1/1000th of an inch, or .0254 mm).</value>
        /// <remarks>This minimum module size was not derived from a specification, so it might not be correct.</remarks>
        public float MinimumModule {
            get { return 7.5f; }
        }

        private float module=10f;
        /// <summary>
        /// Gets and sets module width of the barcode (in mils).
        /// </summary>
        /// <value>The module width of the barcode, in mils (1 mil is 1/1000 of an inch). The default value is 10 mils (.254 mm).</value>
        /// <remarks>The module width is the width of the narrowest bar.  The widths of spaces,
        /// wide bars, wide spaces, quiet zones, etc. are multiples of this module width.  Changing the module width of a barcode
        /// changes the total width of the barcode, as returned by <see cref="Barcodes.IBarcodeSizer.Width"/>.</remarks>
        /// <exception cref="System.ArgumentException">The specified module width is less than <see cref="Barcodes.MSISizer.MinimumModule"/>.</exception>
        public float Module {
            get { return module; }
            set {
                if (module < 7.5f)
                    throw new ArgumentException("The specified module width is smaller than the minimum."); 
                module = value;
                CalculateSizes();
            }
        }
    }

    /// <summary>
    /// A generator that generates MSI barcodes.
    /// </summary>
    public class MSIGenerator : BarcodeGenerator {
        private byte[] data=null;
        private BitArray encodedData = null;

        #region Encoding Table
        private bool[,] encodings ={
            {true,false,false,true,false,false,true,false,false,true,false,false}, //0
            {true,false,false,true,false,false,true,false,false,true,true,false},
            {true,false,false,true,false,false,true,true,false,true,false,false},
            {true,false,false,true,false,false,true,true,false,true,true,false},
            {true,false,false,true,true,false,true,false,false,true,false,false},
            {true,false,false,true,true,false,true,false,false,true,true,false},
            {true,false,false,true,true,false,true,true,false,true,false,false},
            {true,false,false,true,true,false,true,true,false,true,true,false},
            {true,true,false,true,false,false,true,false,false,true,false,false},
            {true,true,false,true,false,false,true,false,false,true,true,false},
            {true,true,false,true,false,false,true,true,false,true,false,false},   //10
            {true,true,false,true,false,false,true,true,false,true,true,false},
            {true,true,false,true,true,false,true,false,false,true,false,false},
            {true,true,false,true,true,false,true,false,false,true,true,false},
            {true,true,false,true,true,false,true,true,false,true,false,false},
            {true,true,false,true,true,false,true,true,false,true,true,false}      //15
        }
            ;

        #endregion

        private void EncodeData() {
            if (encodedData != null)
                return;

            encodedData = new BitArray(data.Length * 12 + 7);

            int i;
            int pos = 0;

            encodedData[pos++] = true;
            encodedData[pos++] = true;
            encodedData[pos++] = false;

            foreach (byte b in data)
                for (i = 0; i < 12; i++)
                    encodedData[pos++] = encodings[b, i];

            encodedData[pos++] = true;
            encodedData[pos++] = false;
            encodedData[pos++] = false;
            encodedData[pos++] = true;

            ((MSISizer)Sizer).Length = data.Length;
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
                if (value != null)
                    foreach (byte b in value)
                        if (b > 15)
                            throw new ArgumentException("The specified data contains invalid values.");

                data = value;
                encodedData = null;
                if (value == null)
                    return;
                EncodeData();
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
        /// Generates an MSI barcode of the specified size, using the data that has been set previously by <see cref="Barcodes.MSIEncoder"/>.
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
            g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);

            int module, guard;
            module = ((MSISizer)Sizer).ModuleWidth;
            guard = ((MSISizer)Sizer).GuardWidth;

            int pos = guard;
            for (int i = 0; i < encodedData.Length; i++) {
                if (encodedData[i])
                    g.FillRectangle(Brushes.Black, pos, 0, module, size.Height);
                pos += module;
            }

            if (((MSISizer)Sizer).FontHeight != 0) {
                string number = "";
                foreach (byte b in data)
                    number += "0123456789ABCDEF"[b];

                Rectangle textArea = Rectangle.FromLTRB(0, size.Height - ((MSISizer)Sizer).FontHeightAddon, size.Width, size.Height);
                g.FillRectangle(Brushes.White, textArea);
                g.DrawString(number, FontHolder.GenerateFont(((MSISizer)Sizer).FontHeight), Brushes.Black, textArea, FontHolder.CenterJustify);
            }

            g.Dispose();
            return bm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.MSIGenerator"/> class.
        /// </summary>
        public MSIGenerator() : base(new MSISizer()) { }
    }
}