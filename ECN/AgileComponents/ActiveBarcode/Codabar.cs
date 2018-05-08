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
    /// An encoder that generates Codabar barcodes.
    /// </summary>
    /// <remarks>Codabar is a barcode symbology commonly used in blood banks, photo labs, etc.  It can encode
    /// the digits 0-9, the letters A, B, C, and D, which act as mandatory start and stop characters, and the symbols
    /// dash (-), dollar ($), colon (:), slash (/), dot (.), and plus (+).</remarks>
    /// <seealso cref="Barcodes.CodabarGenerator"/>
    /// <seealso cref="Barcodes.CodabarSizer"/>
    public class CodabarEncoder : BarcodeEncoder {
        /// <summary>
        /// Not supported.
        /// </summary>
        /// <remarks>Always throws a <see cref="System.NotSupportedException"/> exception.</remarks>
        public override byte[] Data {
            get {
                throw new NotSupportedException("Codabar cannot encode binary data.");
            }
            set {
                throw new NotSupportedException("Codabar cannot encode binary data.");
            }
        }

        private void CheckAndEncodeText(string value, out byte[] data) {
            data = null;
            if (value == null)
                return;

            if (value.Length < 2)
                throw new ArgumentException("The specified string is too short; it must have at least two symbols (a start and a stop symbol).", "Length");

            value = value.ToUpper();
            if ("ABCD".IndexOf(value[0]) == -1)
                throw new ArgumentException("The first symbol was not a start symbol (A, B, C, or D).", "Start");
            if ("ABCD".IndexOf(value[value.Length - 1]) == -1)
                throw new ArgumentException("The last symbol was not a stop symbol (A, B, C, or D).", "Stop");

            int i = 1;
            for (; i < value.Length - 1; i++)
                if ("0123456789-$:/.+".IndexOf(value[i]) == -1)
                    throw new ArgumentException("A symbol contained in the middle of the string was not valid.", "Symbol");

            data = new byte[value.Length];

            for (i = 0; i < data.Length; i++)
                data[i] = (byte)"0123456789-$:/.+ABCD".IndexOf(value[i]);
        }

        private string text;

        /// <summary>
        /// Gets or sets the text to be encoded into a barcode.
        /// </summary>
        /// <value>A string of the data to be encoded.</value>
        /// <remarks>A valid Codabar barcode must begin with an A, B, C, or D, and must end with an A, B, C, 
        /// or D, though the start and end do not have to be the same.  A, B, C, and D can only appear as the first
        /// and last characters.  The symbols between the start and stop symbols, can only be the following: 0-9,
        /// dash (-), dollar ($), colon (:), slash (/), dot (.), and plus (+).  The string is converted to uppercase
        /// when it is encoded, but the string returned by this property will have the same case as the string passed
        /// in.</remarks>
        /// <exception cref="System.NotSupportedException">The encoder does not support setting the data
        /// to be encoded by string.</exception>
        /// <exception cref="System.ArgumentException">The data to be encoded is invalid, either by length
        /// or content.</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                byte[] encoding;
                CheckAndEncodeText(value, out encoding);
                text = value;
                GeneratorInstance.Data = encoding;
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
        /// <value>Always returns <c>"0123456789-$:/.+ABCDabcd"</c>.</value>
        /// <remarks>The encoder can only encode these symbols.  Passing in a symbol that is not in this string
        /// will cause the encoder to fail.  A, B, C, and D are start and stop symbols.  They can and must appear only at
        /// the beginning and end of the data to be encoded..</remarks>
        public override string TextSymbols {
            get {
                return "0123456789-$:/.+ABCDabcd";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.CodabarEncoder"/> class.
        /// </summary>
        public CodabarEncoder() : base(new CodabarGenerator()) { }
    }

    /// <summary>
    /// That class that controls Codabar barcode sizing.
    /// </summary>
    /// <seealso cref="Barcodes.CodabarGenerator"/>
    /// <seealso cref="Barcodes.CodabarEncoder"/>
    public class CodabarSizer : BarcodeSizer, IBarcodeModularSizer {
        private int moduleWidth = 1;
        /// <summary>
        /// The module width of the barcode, in pixels.
        /// </summary>
        public int ModuleWidth { get { return moduleWidth; } set { moduleWidth = value; } }

        private int guardWidth = 10;
        /// <summary>
        /// The guard (quiet) zone width, in pixels.
        /// </summary>
        public int GuardWidth { 
            get {
                if ((Mode & BarcodeRenderMode.Guarded) != 0)
                    return guardWidth;
                else
                    return 0;
            } 
            set { guardWidth = value; } 
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

        private int moduleLength=0;
        /// <summary>
        /// Gets and sets the total number of modules used by the generator.
        /// </summary>
        /// <value>The total number of modules in the barcode.</value>
        /// <remarks>This property is used by <see cref="Barcodes.CodabarGenerator"/> to inform it
        /// of the size of the encoded barcode, since the module length of individual symbols varies
        /// depending on the symbol.  It should not be set directly.</remarks>
        public int ModuleLength {
            get { return moduleLength; }
            set { moduleLength = value; }
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
        /// The total width of the barcode, excluding any guard (quiet) zones, in pixels.
        /// </summary>
        public int WidthWithoutGuards {
            get { return moduleLength * moduleWidth; } 
        }

        /// <summary>
        /// The total width of the barcode, in pixels, given the current DPI and module width.
        /// </summary>
        public override int Width {
            get { return WidthWithoutGuards + (((Mode & BarcodeRenderMode.Guarded) != 0) ? 2 * guardWidth : 0); }
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
        /// The minimum height of the barcode, in pixels.
        /// </summary>
        public override int Height {
            get { return 1+FontHeightAddon; }
        }


        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>The height of all items added to the barcode, in pixels.</value>
        /// <remarks><para>For Codabar barcodes, the only extra height comes from the text at the bottom of the barcode.</para>
        /// <para>This property is meant to be used to generate a desired barcode height.  To calculate the height
        /// of the size to pass to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, multiply the desired height
        /// (in inches) by the DPI, and add this number to it.</para><para>This property's value is potentially impacted by the value of <see cref="Barcodes.IBarcodeSizer.Mode"/>.
        /// The barcode render mode flags should be set before using this value.</para></remarks>
        public override int ExtraHeight {
            get {
                return FontHeightAddon;
            }
        }

        private float module=10f;
        /// <summary>
        /// Gets or sets the desired module width, in mils.
        /// </summary>
        /// <value>The current desired module width, in mils (1 mil is 1/1000th of an inch, .0254 mm).  The default value is 10 mils (.25 mm).</value>
        /// <remarks>The barcode generator will generate a barcode with a module width that is as close as
        /// possible to, but not smaller than, the specified module width, within the limits of the 
        /// specified DPI.  It cannot be set lower than the value returned by <see cref="Barcodes.CodabarSizer.MinimumModule"/>.</remarks>
        /// <exception cref="System.ArgumentException">The specified module width is smaller than 
        /// <see cref="Barcodes.CodabarSizer.MinimumModule"/>.</exception>
        public float Module {
            get {
                return module;
            }
            set {
                if (value < 7.5f)
                    throw new ArgumentException("The specified module width is smaller than the minimum width.");
                module = value;
                CalculateSizes();
            }
        }

        /// <summary>
        /// Gets the minimum module width, in mils.
        /// </summary>
        /// <value>The minimum module width, in mils. (1 mil is 1/1000th of an inch, or .025mm). Always returns 7.5 mils (.19 mm).</value>
        public float MinimumModule { get { return 7.5f; } }

        /// <summary>
        /// Checks to see if a specified size is valid.
        /// </summary>
        /// <param name="size">A size to test for validity.</param>
        /// <returns>True if this size may be passed to <see cref="Barcodes.CodabarGenerator.GenerateBarcode"/>, false otherwise.</returns>
        public override bool IsValidSize(Size size) {
            return (size.Width == Width && size.Height >= size.Height);
        }

        /// <summary>
        /// Given a size, returns the largest valid size contained by that size.
        /// </summary>
        /// <param name="size">A maximum size, from which to find a valid size.</param>
        /// <returns>A valid size which may be passed to <see cref="Barcodes.CodabarGenerator.GenerateBarcode"/>.</returns>
        /// <exception cref="System.ArgumentException">The specified size is smaller than the minimum size in one or both dimensions.</exception>

        public override Size GetValidSize(Size size) {
            if (size.Width < Width || size.Height < Height)
                throw new ArgumentException("The specified size is smaller than the minumum size.");
            return new Size(Width, size.Height);
        }
    }

    /// <summary>
    /// A generator that generates Codabar barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.CodabarEncoder"/>
    /// <seealso cref="Barcodes.CodabarSizer"/>
    public class CodabarGenerator : BarcodeGenerator {
        private byte[] data=null;
        private BitArray encodedData = null;

        #region Encoding Table
        private bool[,] encodings ={
            {true,false,true,false,true,false,false,true,true,false},    //0
            {true,false,true,false,true,true,false,false,true,false},    //1
            {true,false,true,false,false,true,false,true,true,false},    //2
            {true,true,false,false,true,false,true,false,true,false},    //3
            {true,false,true,true,false,true,false,false,true,false},    //4
            {true,true,false,true,false,true,false,false,true,false},    //5
            {true,false,false,true,false,true,false,true,true,false},    //6
            {true,false,false,true,false,true,true,false,true,false},    //7
            {true,false,false,true,true,false,true,false,true,false},    //8
            {true,true,false,true,false,false,true,false,true,false},    //9
            {true,false,true,false,false,true,true,false,true,false},    //10 -
            {true,false,true,true,false,false,true,false,true,false},    //11 $
            {true,true,false,true,false,true,true,false,true,true},      //12 :
            {true,true,false,true,true,false,true,false,true,true},      //13 /
            {true,true,false,true,true,false,true,true,false,true},      //14 .
            {true,false,true,true,false,true,true,false,true,true},      //15 +
            {true,false,true,true,false,false,true,false,false,true},    //16 A
            {true,false,true,false,false,true,false,false,true,true},    //17 B
            {true,false,false,true,false,false,true,false,true,true},    //18 C
            {true,false,true,false,false,true,true,false,false,true}     //19 D
        };
        #endregion

        private void EncodeData() {
            if (encodedData!=null) return;

            List<bool> bits = new List<bool>();

            foreach (byte b in data) {
                for (int i = 0; i < 10; i++) {
                    if (i == 9 && !encodings[b, i])
                        continue; //Codes end in a true, but the table must be of a fixed size.
                    bits.Add(encodings[b, i]);
                }
                bits.Add(false);
            }

            bits.RemoveAt(bits.Count - 1);//Ignore the last space.
            encodedData = new BitArray(bits.ToArray());
            ((CodabarSizer)Sizer).ModuleLength = encodedData.Count;
        }

        private void CheckData(byte[] value) {
            //Ensure the data is well formed.
            if (value == null)
                return;

            if (value.Length < 2)
                throw new ArgumentException("The data is not long enough; it must be at least two bytes (a start and a stop code).");

            if (value[0] > 19 || value[0] < 16)
                throw new ArgumentException("The data does not start with a valid start symbol.");
            if (value[value.Length - 1] > 19 || value[value.Length - 1] < 16)
                throw new ArgumentException("The data does not end with a valid stop symbol.");

            for (int i = 1; i < value.Length - 1; i++)
                if (value[i] > 16)
                    throw new ArgumentException("The data contains start/stop characters in the middle of the data.");
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
                CheckData(value);
                data = value;
                encodedData = null;
                if (data == null)
                    ((CodabarSizer)Sizer).ModuleLength = 0;
                else
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
        /// Generates a Codabar barcode of a specified size, using the data that has been set previously by its corresponding
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
            g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);

            int module, guard;
            module = ((CodabarSizer)Sizer).ModuleWidth;
            guard = ((CodabarSizer)Sizer).GuardWidth;

            int pos = guard;
            for (int i = 0; i < encodedData.Length; i++) {
                if (encodedData[i])
                    g.FillRectangle(Brushes.Black, pos, 0, module, size.Height);
                pos += module;
            }


            if (((CodabarSizer)Sizer).FontHeight != 0) {
                string text = "";
                foreach (byte b in data)
                    text += "0123456789-$:/.+ABCD"[b];

                Rectangle textArea = Rectangle.FromLTRB(0, size.Height - ((CodabarSizer)Sizer).FontHeightAddon, size.Width, size.Height);
                g.FillRectangle(Brushes.White, textArea);
                g.DrawString(text, FontHolder.GenerateFont(((CodabarSizer)Sizer).FontHeight), Brushes.Black, textArea, FontHolder.CenterJustify);
            }

            g.Dispose();
            return bm;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.CodabarGenerator"/> class.
        /// </summary>
        public CodabarGenerator() : base(new CodabarSizer()) { }
    }
}