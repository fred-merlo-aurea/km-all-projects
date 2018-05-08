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
    /// An encoder for Postnet barcodes.
    /// </summary>
    /// <remarks>Postnet is used by the US Postal Service to encode ZIP codes and destination codes.  It 
    /// can encode digit sequences of exactly 5, 9, or 11 digits (not including check digit).  The barcode includes a check digit
    /// which may be precalculated and appended before it is passed to the encoder; this check digit will
    /// be verified by the encoder. It has strictly defined dimensional requirements.</remarks>
    /// <seealso cref="Barcodes.PostnetGenerator"/>
    /// <seealso cref="Barcodes.PostnetSizer"/>
    public class PostnetEncoder : BarcodeEncoder {
        private void CheckAndEncodeText(string text, out byte[] data) {
            data = null;
            if (text == null)
                return;

            switch (text.Length) {
                case 5:
                case 9:
                case 11:
                    data = new byte[text.Length + 1];
                    break;
                case 6:
                case 10:
                case 12:
                    data = new byte[text.Length];
                    break;
                default:
                    throw new ArgumentException("The specified string is an invalid length.", "Length");
            }

            byte checkSum = 0;
            for (int i = 0; i < text.Length; i++) {
                int val = "0123456789".IndexOf(text[i]);
                if (val == -1)
                    throw new ArgumentException("The specified string contains non-digit characters.", "Digit");
                data[i] = (byte)val;
                checkSum += data[i];
            }


            switch (text.Length) {
                case 5:
                case 9:
                case 11:
                    data[text.Length] = (byte)((10 - checkSum % 10) % 10);
                    break;
                case 6:
                case 10:
                case 12:
                    if ((checkSum % 10) != 0)
                        throw new ArgumentException("The specified string contains a check digit, and it does not match the calculated check digit.", "Check");
                    break;
            }
        }

        private string text;

        /// <summary>
        /// Gets or sets the data to be encoded into Postnet.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>Postnet encodes 5, 9, or 11 digit ZIP codes and delivery codes.  The encoder will accept strings
        /// of 5, 9, or 11 digits, and will automatically calculate and append the check digit.  The
        /// encoder will also accept strings of 6, 10, and 12 digits, if the check digit is already known.  The
        /// encoder will check this digit, and throw an exception if it is invalid.</remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null, did not have a valid number of digits,
        /// was not composed of digits 0-9, or included an invalid check digit.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will be "Length", "Digit", or "Check", respectively, depending on the error.</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                byte[] data;
                CheckAndEncodeText(value, out data);
                text = value;
                GeneratorInstance.Data = data;
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
        /// Gets the symbols that can be encoded.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>The encoder can only encode digits.  If any non-digit symbol is passed to the encoder, it will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.PostnetEncoder"/> class.
        /// </summary>
        public PostnetEncoder() : base(new PostnetGenerator(false)) { }
    }


    /// <summary>
    /// An encoder for Pplanet barcodes.
    /// </summary>
    /// <remarks>Pplanet is used by the US Postal Service to encode tracking codes.  It 
    /// can encode digit sequences of exactly 11 or 13 digits (not including check digit).  The barcode includes a check digit
    /// which may be precalculated and appended before it is passed to the encoder; this check digit will
    /// be verified by the encoder. It has strictly defined dimensional requirements.</remarks>
    /// <seealso cref="Barcodes.PostnetGenerator"/>
    /// <seealso cref="Barcodes.PostnetSizer"/>
    public class PlanetEncoder : BarcodeEncoder {
        private void CheckAndEncodeText(string text, out byte[] data) {
            data = null;
            if (text == null)
                return;

            switch (text.Length) {
                case 11:
                case 13:
                    data = new byte[text.Length + 1];
                    break;
                case 12:
                case 14:
                    data = new byte[text.Length];
                    break;
                default:
                    throw new ArgumentException("The specified string is an invalid length.", "Length");
            }

            int checkSum = 0;
            for (int i = 0; i < text.Length; i++) {
                int val = "0123456789".IndexOf(text[i]);
                if (val == -1)
                    throw new ArgumentException("The specified string contains non-digit characters.", "Digit");
                data[i] = (byte)val;
                checkSum += val;
            }


            switch (text.Length) {
                case 11:
                case 13:
                    data[text.Length] = (byte)((10 - checkSum % 10) % 10);
                    break;
                case 12:
                case 14:
                    if ((checkSum % 10) != 0)
                        throw new ArgumentException("The specified string includes a check digit, and it does not match the calculated check digit.", "Check");
                    break;
            }
        }

        string text;
        /// <summary>
        /// Gets or sets the data to be encoded into Planet.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>Postnet encodes 11 or 13 digit tracking codes.  The encoder will accept strings
        /// of 11 or 13 digits, and will automatically calculate and append the check digit.  The
        /// encoder will also accept strings of 12 or 14 digits, if the check digit is already known.  The
        /// encoder will check this digit, and throw an exception if it is invalid.</remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null, did not have a valid number of digits,
        /// was not composed of digits 0-9, or included an invalid check digit.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will be "Length", "Digit", or "Check", respectively, depending on the error.</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                byte[] data;
                CheckAndEncodeText(value, out data);
                text = value;
                GeneratorInstance.Data = data;
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
        /// Gets the symbols that can be encoded.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>The encoder can only encode digits.  If any non-digit symbol is passed to the encoder, it will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.PlanetEncoder"/> class.
        /// </summary>
        public PlanetEncoder() : base(new PostnetGenerator(true)) { }
    }

    /// <summary>
    /// The sizer that controls the sizing of Postnet and Planet barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.PostnetGenerator"/>
    public class PostnetSizer : BarcodeSizer {
        private static readonly int minimumShortHeight = 400;
        private static readonly int maximumShortHeight = 600;
        private static readonly int minimumLongHeight = 1150;
        private static readonly int maximumLongHeight = 1350;
        private static readonly int minimumBarWidth = 150;
        private static readonly int maximumBarWidth = 250;
        private static readonly int minimumGapWidth = 120;
        private static readonly int maximumGapWidth = 400;
        private static readonly int minimumUnitWidth = 417;
        private static readonly int maximumUnitWidth = 500;

        private static readonly int minimumEdgeToEdge5 = 12450;
        private static readonly int maximumEdgeToEdge5 = 16250;
        private static readonly int minimumEdgeToEdge9 = 20750;
        private static readonly int maximumEdgeToEdge9 = 26250;
        private static readonly int minimumEdgeToEdgeB = 24950;
        private static readonly int maximumEdgeToEdgeB = 31250;
        private static readonly int minimumEdgeToEdgeD = 29150;
        private static readonly int maximumEdgeToEdgeD = 36250;

        private int length;
        private int bars = 0;

        /// <summary>
        /// Gets or sets the number of digits to be encoded in the barcode.
        /// </summary>
        /// <value>The number of digits to encode in the barcode, not including the check digit.</value>
        public int Length {
            get { return length; }
            set {
                length = value;
                switch (length) {
                    case 5:
                        bars = 32;
                        break;
                    case 9:
                        bars = 52;
                        break;
                    case 11:
                        bars = 62;
                        break;
                    case 13:
                        bars = 72;
                        break;
                    default:
                        throw new ArgumentException("The specified number of digits is invalid.");
                }
                CalculateDimensions();
            }
        }

        private void CalculateDimensions() {
            if (length == 0 || DPI == 0)
                return;
            float tenthMilsPerDot = 10000f / DPI;

            int minimumShortHeightDots = (int)Math.Ceiling(minimumShortHeight / tenthMilsPerDot);
            int maximumShortHeightDots = (int)Math.Floor(maximumShortHeight / tenthMilsPerDot);
            int minimumLongHeightDots = (int)Math.Ceiling(minimumLongHeight / tenthMilsPerDot);
            int maximumLongHeightDots = (int)Math.Floor(maximumLongHeight / tenthMilsPerDot);
            int minimumBarWidthDots = (int)Math.Ceiling(minimumBarWidth / tenthMilsPerDot);
            int maximumBarWidthDots = (int)Math.Floor(maximumBarWidth / tenthMilsPerDot);
            int minimumGapWidthDots = (int)Math.Ceiling(minimumGapWidth / tenthMilsPerDot);
            int maximumGapWidthDots = (int)Math.Floor(maximumGapWidth / tenthMilsPerDot);
            int minimumUnitWidthDots = (int)Math.Ceiling(minimumUnitWidth / tenthMilsPerDot);
            int maximumUnitWidthDots = (int)Math.Floor(maximumUnitWidth / tenthMilsPerDot);

            int minEtE = 0, maxEtE = 0;

            //Constraints:
            // minimumShortHeightDots <= barShortHeight    <= maximumShortHeightDots
            // minimumLongHeightDots  <= barLongHeight     <= maximumLongHeightDots
            // minimumBarWidthDots    <= barWidth          <= maximumBarWidthDots
            // minimumGapWidthDots    <= gapWidth          <= maximumGapWidthDots
            // minimumUnitWidthDots   <= barWidth+gapWidth <= maximumUnitWidthDots

            if (maximumBarWidthDots < minimumBarWidthDots ||
                maximumGapWidthDots < minimumGapWidthDots ||
                maximumUnitWidthDots < minimumUnitWidthDots ||
                maximumLongHeightDots < minimumLongHeightDots ||
                maximumShortHeightDots < minimumShortHeightDots)
                //The DPI is too small; anything we print will automatically fall out of spec.
                throw new ArgumentException("The specified DPI is not large enough to conform with Postnet/Planet specifications.");


            //Figure out the minimum and maximum total lengths, given the number of digits encoded.
            switch (Length) {
                case 5:
                    minEtE = (int)Math.Ceiling(minimumEdgeToEdge5 / tenthMilsPerDot);
                    maxEtE = (int)Math.Floor(maximumEdgeToEdge5 / tenthMilsPerDot);
                    break;
                case 9:
                    minEtE = (int)Math.Ceiling(minimumEdgeToEdge9 / tenthMilsPerDot);
                    maxEtE = (int)Math.Floor(maximumEdgeToEdge9 / tenthMilsPerDot);
                    break;
                case 11:
                    minEtE = (int)Math.Ceiling(minimumEdgeToEdgeB / tenthMilsPerDot);
                    maxEtE = (int)Math.Floor(maximumEdgeToEdgeB / tenthMilsPerDot);
                    break;
                case 13:
                    minEtE = (int)Math.Ceiling(minimumEdgeToEdgeD / tenthMilsPerDot);
                    maxEtE = (int)Math.Floor(maximumEdgeToEdgeD / tenthMilsPerDot);
                    break;
            }

            //First try for a middle position. This will almost always work.
            barWidth = (minimumBarWidthDots + maximumBarWidthDots) / 2;
            gapWidth = (minimumGapWidthDots + maximumGapWidthDots) / 2;
            barLongHeight = (minimumLongHeightDots + maximumLongHeightDots) / 2;
            barShortHeight = (minimumShortHeightDots + maximumShortHeightDots) / 2;

            int minEtETest = (barWidth + gapWidth) * (bars - 1);
            int maxEtETest = (barWidth + gapWidth) * bars - gapWidth;

            if (minEtETest >= minEtE &&
                maxEtETest <= maxEtE &&
                (barWidth + gapWidth) >= minimumUnitWidthDots &&
                (barWidth + gapWidth) <= maximumUnitWidthDots)
                return;

            //If not, then try every possible combination of bar width and gap width, to see if one of them works.
            for (barWidth = minimumBarWidthDots; barWidth <= maximumUnitWidthDots; barWidth++) {
                for (gapWidth = minimumGapWidthDots; gapWidth <= maximumGapWidthDots; gapWidth++) {
                    minEtETest = (barWidth + gapWidth) * (bars - 1);
                    maxEtETest = (barWidth + gapWidth) * bars - gapWidth;

                    if (minEtETest >= minEtE &&
                        maxEtETest <= maxEtE &&
                        (barWidth + gapWidth) >= minimumUnitWidthDots &&
                        (barWidth + gapWidth) <= maximumUnitWidthDots)
                        return;
                }
            }

            //Nothing was found.
            throw new ArgumentException("The specified DPI is not capable of conforming with Postnet/Planet specifications.");
        }

        private int barWidth = 1;
        private int gapWidth = 1;
        private int barLongHeight = 2;
        private int barShortHeight = 1;

        /// <summary>
        /// Gets the only valid width of the barcode.
        /// </summary>
        /// <value>The width of the barcode, in pixels.</value>
        public override int Width {
            get {
                if (length == 0)
                    return 0;
                return (barWidth + gapWidth) * bars - gapWidth;
            }
        }

        /// <summary>
        /// Gets the only valid height of the barcode.
        /// </summary>
        /// <value>The height of the barcode, in pixels.</value>
        public override int Height {
            get {
                if (length == 0)
                    return 0;
                return barLongHeight;
            }
        }

        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>Since the barcode's dimensions are fixed, this always returns 0.</value>
        public override int ExtraHeight {
            get { return 0; }
        }

        /// <summary>
        /// Gets or sets the dots-per-inch (DPI) of the barcode.
        /// </summary>
        /// <value>The current DPI of the barcode, or zero for "logical mode".</value>
        /// <remarks>If the DPI is set to zero, the generator operates in "logical mode," which produces
        /// barcodes with bars of the proper relative size and positioning, but not suitable for printing.
        /// If the barcodes are to be printed, the DPI must be set.  Due to the strict nature of the dimensional
        /// requirements, the barcode cannot be rendered at certain DPIs.  If the barcode cannot be rendered at
        /// the specified DPI, it will throw an exception when that DPI is set.
        /// </remarks>
        /// <exception cref="System.ArgumentException">The specified DPI is less than zero, or the barcode
        /// cannot be rendered properly at the specified DPI.</exception>
        public override float DPI {
            get {
                return base.DPI;
            }
            set {
                base.DPI = value;
                if (base.DPI != 0) {
                    CalculateDimensions();
                } else {
                    barWidth = gapWidth = barShortHeight = 1;
                    barLongHeight = 2;
                }
            }
        }

        /// <summary>
        /// Checks to see if a specified size is valid.
        /// </summary>
        /// <param name="size">A size to test for validity.</param>
        /// <returns>True if this size may be passed to <see cref="Barcodes.PostnetGenerator.GenerateBarcode"/>, false otherwise.</returns>
        /// <remarks>Because this barcode has fixed dimensions, the only valid size is <see cref="Barcodes.BarcodeSizer.Size"/>.</remarks>
        public override bool IsValidSize(Size size) {
            return size.Width == Width && size.Height == Height;
        }


        /// <summary>
        /// Given a size, returns the largest valid size contained by that size.
        /// </summary>
        /// <param name="size">A maximum size, from which to find a valid size.</param>
        /// <returns>A valid size which may be passed to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>.</returns>
        /// <exception cref="System.ArgumentException">The specified size is smaller than the minimum size in one or both dimensions.</exception>
        public override Size GetValidSize(Size size) {
            if (size.Width < Width || size.Height < Height)
                throw new ArgumentException("The specified size is below the minimum size.");
            return Size;
        }

        /// <summary>
        /// Gets the current long bar height, in pixels.
        /// </summary>
        public int LongBarHeight {
            get {
                return barLongHeight;
            }
        }

        /// <summary>
        /// Gets the current short bar height, in pixels.
        /// </summary>
        public int ShortBarHeight {
            get {
                return barShortHeight;
            }
        }

        /// <summary>
        /// Gets the current bar width, in pixels.
        /// </summary>
        public int BarWidth {
            get {
                return barWidth;
            }
        }

        /// <summary>
        /// Gets the current gap width, in pixels.
        /// </summary>
        public int GapWidth {
            get {
                return gapWidth;
            }
        }
    }

    /// <summary>
    /// A generator that produces Postnet and Planet barcodes.
    /// </summary>
    public class PostnetGenerator : BarcodeGenerator {
        private byte[] data;
        private BitArray encodedData;

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
                if (value != null) {
                    if ((!isPlanet && value.Length != 6 && value.Length != 10 && value.Length != 12) ||
                        (isPlanet && value.Length != 12 && value.Length != 14))
                        throw new ArgumentException((isPlanet ? "Planet" : "Postnet") + " data is an invalid length.");
                    foreach (byte b in value)
                        if (b >= 10)
                            throw new ArgumentException((isPlanet ? "Planet" : "Postnet") + " data contains invalid codes.");
                }
                data = value;
                encodedData = null;
                if (value == null)
                    return;
                ((PostnetSizer)Sizer).Length = data.Length - 1;
            }
        }

        /// <summary>
        /// Gets the barcode generator capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/>|<see cref="Barcodes.BarcodeGeneratorFlags.VariableLength"/>|<see cref="Barcodes.BarcodeGeneratorFlags.FixedDimensions"/>.</value>
        public override BarcodeGeneratorFlags Flags {
            get {
                return BarcodeGeneratorFlags.Linear |
              BarcodeGeneratorFlags.VariableLength |
              BarcodeGeneratorFlags.FixedDimensions;
            }
        }

        #region Encoding Table
        private static readonly bool[,] encodings ={
            {true,true,false,false,false},
            {false,false,false,true,true},
            {false,false,true,false,true},
            {false,false,true,true,false},
            {false,true,false,false,true},
            {false,true,false,true,false},
            {false,true,true,false,false},
            {true,false,false,false,true},
            {true,false,false,true,false},
            {true,false,true,false,false}
        };
        #endregion

        private void EncodeData() {
            if (encodedData != null)
                return;

            encodedData = new BitArray(data.Length * 5 + 2);
            int pos = 0;
            encodedData[pos++] = true;
            int i;
            for (i = 0; i < data.Length; i++) {
                encodedData[pos++] = encodings[data[i], 0];
                encodedData[pos++] = encodings[data[i], 1];
                encodedData[pos++] = encodings[data[i], 2];
                encodedData[pos++] = encodings[data[i], 3];
                encodedData[pos++] = encodings[data[i], 4];
            }
            encodedData[pos++] = true;
            Debug.Assert(pos == encodedData.Length);
        }

        /// <summary>
        /// Generates a bitmap of the barcode, of the specified size and data.
        /// </summary>
        /// <param name="size">The size of barcode to generate.</param>
        /// <returns>A bitmap of the generated barcode.</returns>
        /// <remarks>As the dimensions of the Postnet/Planet barcode are strictly defined, the only valid size is
        /// the size returned by <see cref="Barcodes.BarcodeSizer.Size"/>.</remarks>
        /// <exception cref="System.ArgumentException">The specified size is invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The data that is to be encoded has not been set yet.</exception>
        public override Bitmap GenerateBarcode(Size size) {
            if (data == null)
                throw new InvalidOperationException("The data to encode has not been set.");
            if (!Sizer.IsValidSize(size))
                throw new ArgumentException("The specified size is invalid.");
            EncodeData();

            PostnetSizer postnetSizer = (PostnetSizer)Sizer;
            int longBar = postnetSizer.LongBarHeight;
            int shortBar = postnetSizer.ShortBarHeight;
            int barWidth = postnetSizer.BarWidth;
            int moduleWidth = barWidth + postnetSizer.GapWidth;

            int bHeight = size.Height;
            Bitmap bm = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppRgb);
            Graphics g = Graphics.FromImage(bm);

            g.FillRectangle(Brushes.White, 0, 0, bm.Width, bm.Height);

            int i;
            for (i = 0; i < encodedData.Length; i++) {
                int length = (encodedData[i] ^ isPlanet) ? longBar : shortBar; //PLANET exchanges short and long bars.
                if (i == 0 || i == encodedData.Length - 1)
                    length = longBar; //The guard bars are always long.
                g.FillRectangle(Brushes.Black, moduleWidth * i, bHeight - length, barWidth, length);
            }

            g.Dispose();
            return bm;
        }

        private bool isPlanet;
        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.PostnetGenerator"/> class.
        /// </summary>
        /// <param name="isPlanet">Specifies whether to use Planet encoding (true) or Postnet encoding(false).</param>
        public PostnetGenerator(bool isPlanet)
            : base(new PostnetSizer()) {
            this.isPlanet = isPlanet;
        }
    }
}
