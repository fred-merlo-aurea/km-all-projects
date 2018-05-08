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
    /// An encoder for EAN13 barcodes.
    /// </summary>
    public class EAN13Encoder : BarcodeEncoder {
        private void CheckAndEncodeText(string t, out byte[] data) {
            data = null;
            if (t == null)
                return;
            if (t.Length != 12 && t.Length != 13)
                throw new ArgumentException("The length of the specified data must be 12 or 13 digits.","Length");

            data = new byte[13];
            int checkSum = 0;
            int weight = 1;
            for (int i = 0; i < 12; i++) {
                int num = "0123456789".IndexOf(t[i]);
                if (num == -1)
                    throw new ArgumentException("The specified data must be of digits 0-9.","Digit");
                checkSum += weight*num;
                weight = 4 - weight;
                data[i] = (byte)num;
            }
            data[12] = (byte)((10 - checkSum % 10) % 10);

            if (t.Length == 13 && data[12] != (byte)"0123456789".IndexOf(t[12]))
                throw new ArgumentException("The specified data included a check digit, and the check digit does not match the calculated check digit.", "Check");
        }

        private string text = null;
        /// <summary>
        /// Gets or sets the data to be encoded into EAN13.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>This property takes a string of exactly 12 or 13 digits, without spaces, dashes, or 
        /// other separators. If the string is 12 digits long, the check digit will be calculated and 
        /// appended automatically.  If the string is 13 digits long, the check digit will be calculated
        /// and compared with the 13th digit.  If they do not match, an <see cref="System.ArgumentException"/> will be 
        /// thrown.  The property may also be set to null.</remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null, did not have 12 or 13 digits,
        /// was not composed of digits 0-9, or included an invalid check digit.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will be "Length", "Digit", or "Check", respectively, depending on the error.</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                var dataBytes = default(byte[]);
                SetTextPropertyValue(
                    value,
                    out text,
                    ref dataBytes,
                    (string str, out byte[] data, ref byte[] data1) =>
                        CheckAndEncodeText(str, out data));
            }
        }

        /// <summary>
        /// Returns the encoder capability flags./>
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.Text"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.Text; }
        }

        /// <summary>
        /// Returns the symbols that can be encoded.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>The encoder can only encode digits.  If any non-digit symbol is passed to the encoder, it will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        /// <summary>
        /// Default constructor to construct a standard EAN13Encoder.
        /// </summary>
        public EAN13Encoder() : base(new EAN13Generator()) { }

        /// <summary>
        /// Special constructor for use by <see cref="Barcodes.UPCAEncoder"/>.
        /// </summary>
        /// <param name="upc">If true, the generator will generate a UPCA barcode, which is very similar to an EAN13
        /// barcode, but with the numbers placed in a slightly different manner, and only showing 12 digits instead
        /// of 13.</param>
        protected EAN13Encoder(bool upc) : base(new EAN13Generator(upc)) { }
    }

    /// <summary>
    /// An encoder for UPCA barcodes.
    /// </summary>
    /// <remarks>This encoder is functionally equivilent to <see cref="Barcodes.EAN13Encoder"/>, with the exception
    /// of using a special constructor and always prepending "0" to any UPC specified before passing it to
    /// the EAN13 encoder.</remarks>
    public class UPCAEncoder : EAN13Encoder {
         /// <summary>
        /// Gets or sets the data to be encoded into UPCA.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>This property takes a string of exactly 11 or 12 digits, without spaces, dashes, or 
        /// other separators. If the string is 11 digits long, the check digit will be calculated and 
        /// appended automatically.  If the string is 12 digits long, the check digit will be calculated
        /// and compared with the 12th digit.  If they do not match, an <see cref="System.ArgumentException"/> will be 
        /// thrown.  The property may also be set to null.</remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null, did not have 11 or 12 digits,
        /// was not composed of digits 0-9, or included an invalid check digit.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will be "Length", "Digit", or "Check", respectively, depending on the error.</exception>
        public override string Text {
            get {
                return base.Text.Substring(1);
            }
            set {
                try {
                    base.Text = "0" + value;
                } catch (ArgumentException ex) {
                    if (ex.ParamName == "Length")
                        throw new ArgumentException("The length of the specified data must be 12 or 13 digits.", "Length");
                    else
                        throw ex;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.UPCAEncoder"/> class.
        /// </summary>
        public UPCAEncoder() : base(true) { }
    }

    /// <summary>
    /// An encoder for ISBN barcodes.
    /// </summary>
    /// <remarks>This encoder is built on top of <see cref="Barcodes.EAN13Encoder"/>, checking the
    /// ISBN check digit and always prepending "978" to the ISBN before passing it to
    /// the EAN13 encoder.</remarks>
    public class ISBNEncoder : EAN13Encoder {
        private string isbn = null;

        private void CheckISBN(string value, out string decoded) {
            int digitCount = 0;
            int weight = 0;
            decoded = "";
            value=value.ToUpper();
            foreach (char c in value) {
                int val = "0123456789X".IndexOf(c);
                if (val == -1)
                    continue;
                decoded += c;
                weight +=  val * (10 - digitCount);
                if (val == 10 && digitCount != 9)
                    throw new ArgumentException("The ISBN contains an X digit that is not in the check digit position.", "X");
                digitCount++;
            }
            if (digitCount != 10)
                throw new ArgumentException("The ISBN is not 10 digits long.", "Length");

            if ((weight % 11) != 0)
                throw new ArgumentException("The ISBN has an invalid check digit.", "Check");
        }

        /// <summary>
        /// Returns the symbols that will be recognized by the encoder.
        /// </summary>
        /// <value>Always returns <c>"0123456789xX"</c>.</value>
        /// <remarks>The encoder will ignore any symbol other than those listed above; however they will be passed
        /// on to the generator and appear in the ISBN number above the barcode.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789xX";
            }
        }

        /// <summary>
        /// Gets or sets the ISBN to be encoded.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>This property takes an ISBN string.  The ISBN is extracted by dropping all characters other than
        /// 0-9 and 'X' (case insensitive). The ISBN is then checked, and if it is valid, the underlying encoder is
        /// set.  The upper text area of the barcode generator is set to include both the ISBN10 and ISBN13 numbers,
        /// if the barcode <see cref="Barcodes.IBarcodeSizer.Mode"/> property includes the <see cref="Barcodes.BarcodeRenderMode.Numbered"/> flag.
        /// The property may also be set to null.</remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null and did not contain a 10 digit ISBN,
        /// had an 'X' in any position other than the check digit position, or the check digit was invalid.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will be "Length", "X", or "Check", respectively, depending on the error.</exception>
        public override string Text {
            get {
                return isbn;
            }
            set {
                string encoded;
                CheckISBN(value, out encoded);
                isbn = value;
                base.Text = "978" + encoded.Substring(0,9);
                ((EAN13Generator)base.Generator).UpperText = "ISBN-10 " + value+"\nISBN-13 978-"+value;
            }
        }
    }

    /// <summary>
    /// An encoder for ISBN13 barcodes.
    /// </summary>
    /// <remarks>This encoder is built on top of <see cref="Barcodes.EAN13Encoder"/>, checking the
    /// ISBN check digit before passing it to the EAN13 encoder.</remarks>
    public class ISBN13Encoder : EAN13Encoder {
        private string isbn = null;
        private void CheckISBN13(string value,out string encoded) {
            int digitCount = 0;
            int firstThree = 0;
            encoded = "";

            int weight = 1;
            int checkSum = 0;
            foreach (char c in value.ToUpper()) {
                int val = "0123456789".IndexOf(c);
                if (val == -1)
                    continue;
                encoded += c;
                digitCount++;
                if (digitCount < 4)
                    firstThree = firstThree * 10 + val;
                checkSum += val*weight;
                weight = 4 - weight;
            }
            if (digitCount != 13)
                throw new ArgumentException("The specified ISBN is not 13 digits long.", "Length");
            if (firstThree != 978 && firstThree != 979)
                throw new ArgumentException("The specified ISBN does not start with 978 or 979.", "Start");
            if ((checkSum % 10) != 0)
                throw new ArgumentException("The specified ISBN's check digit is invalid.", "Check");
        }

        /// <summary>
        /// Returns the symbols that will be recognized by the encoder.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>The encoder will ignore any symbol other than those listed above; however they will be passed
        /// on to the generator and appear in the ISBN13 number above the barcode.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        /// <summary>
        /// Gets or sets the ISBN13 to be encoded.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>This property takes an ISBN13 string.  The ISBN13 is extracted by dropping all 
        /// characters other than 0-9. The ISBN13 is then checked, and if it is valid, the underlying 
        /// encoder is set.  The upper text area of the barcode generator is set to include the ISBN13 
        /// number, if the barcode <see cref="Barcodes.IBarcodeSizer.Mode"/> property includes the 
        /// <see cref="Barcodes.BarcodeRenderMode.Numbered"/> flag. The property may also be set to null.</remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null and did not contain a 13 digit ISBN,
        /// did not start with 978 or 979, or the check digit was invalid.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will be "Length", "Start", or "Check", respectively, depending on the error.</exception>
        public override string Text {
            get {
                return isbn;
            }
            set {
                string encoded;
                CheckISBN13(value, out encoded);
                isbn = value;
                base.Text = encoded.Substring(0, 12);
                ((EAN13Generator)base.Generator).UpperText = "ISBN-13 " + value;
            }
        }
    }

    /// <summary>
    /// An encoder for ISSN barcodes.
    /// </summary>
    /// <remarks>This encoder </remarks>
    public class ISSNEncoder : EAN13Encoder {
        private string issn = null;

        private string CheckISSN(string value) {
            if (value == null)
                return null;
            int digitCount=0;
            int weight = 0,val;
            string encoded = "";
            foreach (char c in value.ToUpper()) {
                val = "0123456789X".IndexOf(c);
                if (val == -1)
                    continue;
                if (val == 10 && digitCount != 7)
                    throw new ArgumentException("There was an X in a position other than the check digit position.", "X");
                if (digitCount < 8) {
                    weight += val * (8 - digitCount);
                }
                digitCount++;
            }

            if (digitCount != 8 && digitCount != 10)
                throw new ArgumentException("The length of the ISSN number must be 8 or 10 digits.", "Length");

            if ((weight % 11) != 0)
                throw new ArgumentException("The check digit of the ISSN number was invalid.", "Check");

            if (digitCount == 8)
                return "977" + encoded.Substring(0, 7) + "00";
            else
                return "977" + encoded.Substring(0, 7) + encoded.Substring(8);
        }

        /// <summary>
        /// Gets or sets the ISSN to be encoded.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>This property takes an ISSN string, consisting of an 8 digit ISSN or 10 digit ISSN with 
        /// issue code. The ISSN is extracted by dropping all characters other than 0-9 and 'X' (case 
        /// insensitive). The ISSN is then checked, and if it is valid, the underlying encoder is set.  
        /// The upper text area of the barcode generator is set to show the ISSN number, if the barcode 
        /// <see cref="Barcodes.IBarcodeSizer.Mode"/> property includes the <see cref="Barcodes.BarcodeRenderMode.Numbered"/> flag. 
        /// The property may also be set to null.</remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null and did not contain an 8 digit ISSN or 10 digit ISSN with issue code,
        /// had an 'X' in any position other than the check digit position, or the check digit was invalid.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will be "Length", "X", or "Check", respectively, depending on the error.</exception>
        public override string Text {
            get {
                return issn;
            }
            set {
                string validated = CheckISSN(value);
                issn = value;
                base.Text = validated;
                if (validated == null)
                    return;
                ((EAN13Generator)Generator).UpperText = "ISSN " + value;
            }
        }
    }

    /// <summary>
    /// An encoder for EAN8 barcodes.
    /// </summary>
    public class EAN8Encoder : BarcodeEncoder {
        private void CheckAndEncodeText(string t, out byte[] data) {
            data = null;
            if (t == null)
                return;

            if (t.Length != 7 && t.Length != 8)
                throw new ArgumentException("The specified string is not 7 or 8 digits long.", "Length");

            data = new byte[8];
            int checkSum = 0;
            int weight = 3;
            for (int i = 0; i < 7; i++) {
                int num = (int)"0123456789".IndexOf(t[i]);
                if (num == -1)
                    throw new ArgumentException("The specified string contains non-digit characters.", "Digit");
                checkSum += weight*num;
                weight = 4 - weight;
                data[i] = (byte)num;
            }
            data[7] = (byte)((10 - checkSum % 10) % 10);

            if (t.Length == 8 && ("0123456789"[data[7]] != t[7]))
                throw new ArgumentException("The specified string contains a check digit which does not match the calculated check digit.", "Check");
        }

        private string text = null;
        /// <summary>
        /// Gets or sets the data to be encoded into EAN8.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>This property takes a string of exactly 7 or 8 digits, without spaces, dashes, or 
        /// other separators. If the string is 7 digits long, the check digit will be calculated and 
        /// appended automatically.  If the string is 8 digits long, the check digit will be calculated
        /// and compared with the 8th digit.  If they do not match, an <see cref="System.ArgumentException"/> will be 
        /// thrown.  The property may also be set to null.</remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null, did not have 7 or 8 digits,
        /// was not composed of digits 0-9, or included an invalid check digit.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will be "Length", "Digit", or "Check", respectively, depending on the error.</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                var dataBytes = default(byte[]);
                SetTextPropertyValue(
                    value,
                    out text,
                    ref dataBytes,
                    (string str, out byte[] data, ref byte[] data1) =>
                        CheckAndEncodeText(str, out data));
            }
        }

        /// <summary>
        /// Returns the symbols that can be encoded.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>The encoder can only encode digits.  If any non-digit symbol is passed to the encoder, it will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        /// <summary>
        /// Returns the encoder capability flags./>
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.Text"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.Text; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.EAN8Encoder"/> class.
        /// </summary>
        public EAN8Encoder() : base(new EAN8Generator()) { }
    }

    /// <summary>
    /// An encoder for UPCE barcodes.
    /// </summary>
    public class UPCEEncoder : BarcodeEncoder {
        //Acceptable inputs: 7, 8, 11, or 12 digits.
        private void CheckAndEncodeText(string value, out string data) {
            int len = value.Length;
            data = null;
            byte checkDigit;
            switch (len) {
                case 7:
                    //Calculate a check digit.
                    checkDigit = CalculateShortChecksum(value);
                    data = value + checkDigit.ToString();
                    return;
                case 8:
                    //Verify check digit.
                    checkDigit = CalculateShortChecksum(value);
                    if (checkDigit != "0123456789".IndexOf(value[7]))
                        throw new ArgumentException("The specified string includes a check digit, which did not agree with the calculated digit.", "Check");
                    data = value;
                    return;
                case 11:
                    //Full UPC without check digit. Compress.
                    if (value[0] != '0' && value[0] != '1')
                        throw new ArgumentException("The specified string cannot be converted into a UPCE barcode.", "Invalid");
                    data = FindEncoder(value.Substring(1));
                    data = value[0] + data;
                    checkDigit = CalculateShortChecksum(data);
                    data = data + checkDigit.ToString();
                    return;
                case 12:
                    if (value[0] != '0' && value[0] != '1')
                        throw new ArgumentException("The specified string cannot be converted into a UPCE barcode.", "Invalid");
                    data = FindEncoder(value.Substring(1, 10));
                    data = value[0] + data;
                    checkDigit = CalculateShortChecksum(data);
                    if ("0123456789"[checkDigit] != value[11])
                        throw new ArgumentException("The specified string includes a check digit, which did not agree with the calculated digit.", "Check");
                    data = data + checkDigit.ToString();
                    return;
                default:
                    throw new ArgumentException("The specified string is not a valid length (7, 8, 11, or 12 digits).", "Length");
            }
        }

        //Holdes a UPCA to UPCE converter.  If a UPCA code matches expression, 
        //Match() returns the UPCE code formatted by output.
        private struct Encoder {
            public Regex expression;
            public string output;

            public Encoder(string regex, string output) {
                expression = new Regex(regex);
                this.output = output;
            }

            public string Match(string code) {
                Match m = expression.Match(code);
                if (!m.Success)
                    return null;
                return string.Format(output, null, m.Groups[1], m.Groups[2]);
            }
        }

        private Encoder[] encoders = {
            new Encoder("(\\d\\d)00000(\\d\\d\\d)","{1}{2}0"),
            new Encoder("(\\d\\d)10000(\\d\\d\\d)","{1}{2}1"),
            new Encoder("(\\d\\d)20000(\\d\\d\\d)","{1}{2}2"),
            new Encoder("(\\d\\d)300000(\\d\\d)","{1}3{2}3"),
            new Encoder("(\\d\\d)400000(\\d\\d)","{1}4{2}3"),
            new Encoder("(\\d\\d)500000(\\d\\d)","{1}5{2}3"),
            new Encoder("(\\d\\d)600000(\\d\\d)","{1}6{2}3"),
            new Encoder("(\\d\\d)700000(\\d\\d)","{1}7{2}3"),
            new Encoder("(\\d\\d)800000(\\d\\d)","{1}8{2}3"),
            new Encoder("(\\d\\d)900000(\\d\\d)","{1}9{2}3"),
            new Encoder("(\\d\\d\\d\\d)00000(\\d)","{1}{2}4"),
            new Encoder("(\\d\\d\\d\\d\\d)0000([5-9])","{1}{2}")
        };

        private string FindEncoder(string code) {
            string result;
            foreach (Encoder e in encoders) {
                result = e.Match(code);
                if (result != null)
                    return result;
            }
            throw new ArgumentException("The specified string cannot be converted into a UPCE barcode.", "Invalid");
        }

        private byte CalculateShortChecksum(string upce) {
            byte[] n = new byte[upce.Length];
            for (int i = 0; i < upce.Length; i++) {
                int val = "0123456789".IndexOf(upce[i]);
                if (val == -1)
                    throw new ArgumentException("The specified string contains non-digit characters.", "Digit"); 
                n[i] = (byte)val;
            }
            byte checkSum=0;
            switch (n[6]) {
                case 0:
                case 1:
                case 2:
                    checkSum = (byte)(n[1] + n[4] + n[6] + 3 * (n[0] + n[2] + n[3] + n[5]));
                    break;
                case 3:
                    checkSum = (byte)(n[1] + n[3] + n[4] + 3 * (n[0] + n[2] + n[5]));
                    break;
                case 4:
                    checkSum = (byte)(n[1] + n[3] + 3 * (n[0] + n[2] + n[4] + n[5]));
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    checkSum = (byte)(n[1] + n[3] + n[5] + 3 * (n[2] + n[4] + n[6]));
                    break;
            }
            return (byte)((10 - checkSum % 10) % 10);
        }

        

        string input;
        /// <summary>
        /// Gets or sets the data to be encoded into UPCE.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks><para>This property takes a string of exactly 7, 8, 11 or 12 digits, without spaces, dashes, or 
        /// other separators. If the string is 7 digits long, the check digit will be calculated and 
        /// appended automatically.  If the string is 8 digits long, the check digit will be calculated
        /// and compared with the 8th digit.  If they do not match, an <see cref="System.ArgumentException"/> will be 
        /// thrown.</para><para>If the string is 11 or 12 digits long, it is treated as a UPCA barcode. The barcode is 
        /// compressed into a UPCE barcode, if possible.  The check digit is then appended or checked, if the input had
        /// 11 or 12 digits, respectively.</para>
        /// <para>The property may also be set to null.</para></remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null, did not have 7 or 8 digits,
        /// was not composed of digits 0-9, included an invalid check digit, or could not be converted into a UPCE
        /// barcode.  The <see cref="System.ArgumentException.ParamName"/> property will be "Length", "Digit", "Check", or "Invalid", 
        /// respectively, depending on the error.</exception>
        public override string Text {
            get {
                return input;
            }
            set {
                string data;
                CheckAndEncodeText(value, out data);
                input = value;

                byte[] encodedData = new byte[8];
                for (int i = 0; i < 8; i++)
                    encodedData[i] = (byte)"0123456789".IndexOf(data[i]);
                GeneratorInstance.Data = encodedData;
            }
        }

        /// <summary>
        /// Returns the encoder capability flags./>
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.Text"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.Text; }
        }

        /// <summary>
        /// Returns the symbols that can be encoded.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>The encoder can only encode digits.  If any non-digit symbol is passed to the encoder, it will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.UPCEEncoder"/> class.
        /// </summary>
        public UPCEEncoder() : base(new UPCEGenerator()) { }
    }

    /// <summary>
    /// An encoder for UPC+2 and UPC+5 addon barcodes.
    /// </summary>
    /// <remarks>This is a composite encoder; the generator will be null until the data to be encoded is specified.</remarks>
    public class UPC25Encoder : BarcodeEncoder {
        private void CheckAndEncodeText(string t, out byte[] data) {
            data = null;
            if (t == null) {
                GeneratorInstance = null;
                return;
            }

            data = new byte[t.Length];
            for (int i = 0; i < t.Length; i++) {
                int val="0123456789".IndexOf(t[i]);
                if (val == -1)
                    throw new ArgumentException("The specified string contains non-digit characters.", "Digit");
                data[i] = (byte)val;
            }

            switch (t.Length) {
                case 2:
                    if (Generator is UPC2Generator)
                        break;
                    GeneratorInstance = new UPC2Generator();
                    break;
                case 5:
                    if (Generator is UPC5Generator)
                        break;
                    GeneratorInstance = new UPC5Generator();
                    break;
                default:
                    throw new ArgumentException("The specified string must be exactly two or five digits long.", "Length");
            }

            //The checksumming is handled in the Generator for mini UPCs.
        }

        private string text = null;

        /// <summary>
        /// Gets or sets the data to be encoded into UPC+2 or UPC+5.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>This property takes a string of exactly 2 or 5 digits, without spaces, dashes, or 
        /// other separators. If the string is 2 digits long, the generator will be configured to generate a
        /// UPC+2 barcode.  If the string is 5 digits long, the generator will be configured to generate a 
        /// UPC+5 barcode.  The property may also be set to null.  As this is a composite encoder, and the
        /// generator depends on data being encoded, the <see cref="Barcodes.BarcodeEncoder.Generator"/> property will
        /// return null if the data to be encoded has not been set, and the generator reference might change
        /// if the data to be encoded is changed.</remarks>
        /// <exception cref="System.ArgumentException">The specified string was not null, did not have 2 or 5 digits,
        /// or was not composed of digits 0-9.  The <see cref="System.ArgumentException.ParamName"/> property will be 
        /// "Length" or "Digit", respectively, depending on the error.</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                byte[] data;
                CheckAndEncodeText(value, out data);
                text = value;
                if (GeneratorInstance!=null) //The generator is set in CheckAndEncodeText, since it varies with the length being encoded.
                    GeneratorInstance.Data = data;
            }
        }

        /// <summary>
        /// Returns the encoder capability flags./>
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.Text"/>|<see cref="Barcodes.BarcodeEncoderFlags.Composite"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.Text|BarcodeEncoderFlags.Composite; }
        }

        /// <summary>
        /// Returns the symbols that can be encoded.
        /// </summary>
        /// <value>Always returns <c>"0123456789"</c>.</value>
        /// <remarks>The encoder can only encode digits.  If any non-digit symbol is passed to the encoder, it will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.UPC25Encoder"/> class.
        /// </summary>
        public UPC25Encoder() : base(null) { }
    }

    /// <summary>
    /// The sizing control class for all EAN-based barcodes.
    /// </summary>
    /// <remarks>All of the EAN-based barcode symbologies (EAN13, EAN8, UPCE, UPC+2, and UPC+5) have the same basic 
    /// sizing requirements, with the only variation being in guard widths, notch sizes, etc.</remarks>
    public class EANSizer : BarcodeSizer, IBarcodeModularSizer {
        private int leftGuardSize;
        private int rightGuardSize;
        private int moduleLength;
        private int guardedModuleLength;
        private int notchLength;

        /// <summary>
        /// Constructs a new 
        /// </summary>
        /// <param name="left">The width of the left guard, measured in modules.</param>
        /// <param name="right">The width of the right guard, measured in modules.</param>
        /// <param name="module">The total width of the barcode, excluding guards, measured in modules.</param>
        /// <param name="notch">The total depth of the notch, measured in modules.</param>
        public EANSizer(int left, int right, int module, int notch) {
            leftGuardSize = left;
            rightGuardSize = right;
            moduleLength = module;
            guardedModuleLength = module+left+right;
            notchLength = notch;
        }

        private float currentX = 130f; //mils*10
        private readonly float nominalX = 130f;
        private int currentModule = 1;
        private int nominalModule = 1;

        private int nominalY = 10000;
        private int nominalHeight = 1;

        private int nominalFontTMils = 1250;
        private int nominalFontHeight = 9;


        /*
         * Explanation of the sizes returned in the next seven properties.
         * _______________________________
         * |              ^              |
         * |              5              |
         * |     _________V_________     |
         * |<-1->|# # # # # # # # #|<-2->|
         * |     |# # # # # # # # #|     |
         * |     |# 6 # # # # # # #|     |
         * |     |# # # # #_#_#_#_#|     |
         * |     |#_______#___4___#|     |
         * |          ^                  |
         * |          3                  |
         * |__________V__________________|
         * 
         * 1 - LeftGuard
         * 2 - RightGuard
         * 3 - BottomDepth
         * 4 - NotchDepth
         * 5 - TopDepth
         * 6 - ModuleWidth
         */              
    
        /// <summary>
        /// Returns the current module width, in pixels.
        /// </summary>
        public int ModuleWidth {
            get {
                return currentModule;
            }
        }

        /// <summary>
        /// Returns the left guard width, in pixels.
        /// </summary>
        public int LeftGuard {
            get {
                if ((Mode&BarcodeRenderMode.Guarded)!=0) {
                    return currentModule * leftGuardSize;
                } else {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Returns the right guard width, in pixels.
        /// </summary>
        public int RightGuard {
            get {
                if ((Mode&BarcodeRenderMode.Guarded)!=0) {
                    return currentModule * rightGuardSize;
                } else {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The distance from the top of the notch to the bottom of the barcode, in pixels.
        /// </summary> 
        public int NotchDepth {
            get {
                if ((Mode&BarcodeRenderMode.Numbered)!=0) {
                    return currentModule * notchLength;
                } else if ((Mode&BarcodeRenderMode.Notched)!=0) {
                    return currentModule * notchLength;
                } else {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The distance from the bottom of the barcode to the bottom of the bitmap, in pixels.
        /// </summary>
        public int BottomDepth {
            get {
                if ((Mode&BarcodeRenderMode.Numbered)!=0) {
                    return (int)FontHeight + currentModule - NotchDepth;
                } else
                    return 0;
            }
        }

        /// <summary>
        /// The height of the upper blank zone above the barcode, in pixels.
        /// </summary>
        public int TopDepth {
            get {
                if ((Mode&BarcodeRenderMode.Numbered)!=0) {
                    Font sf=FontHolder.GenerateFont(FontHeight * .8f);
                    return sf.Height*upperLines;
                } else
                    return 0;
            }
        }

        /// <summary>
        /// The font height to use for human readable decorations, in pixels.
        /// </summary>
        public float FontHeight {
            get {
                if ((Mode&BarcodeRenderMode.Numbered)!=0) {
                    if (currentModule < nominalModule)
                        return nominalFontHeight;
                    else
                        return nominalFontHeight * currentModule / nominalModule;
                } else
                    return 0f;
            }
        }

        /// <summary>
        /// The total width of the barcode, in pixels, given the current DPI and module width.
        /// </summary>
        public override int Width {
            get {
                if ((Mode&BarcodeRenderMode.Guarded)!=0)
                    return currentModule * guardedModuleLength;
                else
                    return currentModule * moduleLength;
            }
        }

        private int upperLines=0;
        /// <summary>
        /// The number of lines of text appearing above the barcode.
        /// </summary>
        /// <remarks>This value is set by <see cref="Barcodes.EAN13Generator.UpperText"/>.</remarks>
        public int UpperLines{
            get { return upperLines; }
            set { upperLines = value; }
        }

        /// <summary>
        /// The minimum height of the barcode, in pixels.
        /// </summary>
        public override int Height {
            get {
                if ((Mode&BarcodeRenderMode.Numbered)!=0) {
                    Font sf = FontHolder.GenerateFont(FontHeight * .8f);
                    return upperLines * sf.Height + Math.Max(nominalFontHeight + currentModule * 2, currentModule * (notchLength + 1));
                } else if ((Mode&BarcodeRenderMode.Notched)!=0) {
                    return currentModule * (notchLength + 1);
                } else {
                    return currentModule;
                }
            }
        }

        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>The height of all items added to the barcode, in pixels.</value>
        /// <remarks><para>For EAN-based barcodes, the extra height comes from the text at the top and bottom of the barcode.
        /// Note that the "extra height" is measured from the top of the notch (if present), not from the absolute bottom
        /// of the bars (the bars that hang down in the middle and on the edges).</para>
        /// <para>This property is meant to be used to generate a desired barcode height.  To calculate the height
        /// of the size to pass to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, multiply the desired height
        /// (in inches) by the DPI, and add this number to it.</para><para>This property's value is potentially impacted by the value of <see cref="Barcodes.IBarcodeSizer.Mode"/>.
        /// The barcode render mode flags should be set before using this value.</para></remarks>
        public override int ExtraHeight {
            get { return Height - currentModule; }
        }

        //Calculates the module pixel width, given the desired module width and DPI.
        //All of the calculations of specific dimensions are handled in their respective properties.
        private void CalculateSizes() {
            if (DPI != 0) {
                float tenthMilsPerDot = 10000 / DPI;
                nominalModule = (int)Math.Ceiling(nominalX / tenthMilsPerDot);
                currentModule = (int)Math.Ceiling(currentX / tenthMilsPerDot);
                nominalFontHeight = (int)Math.Ceiling(nominalFontTMils / tenthMilsPerDot);
                nominalHeight = (int)Math.Ceiling(nominalY / tenthMilsPerDot);
            } else {
                currentModule = 1;
                nominalFontHeight = 9;
            }
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
        /// <exception cref="System.ArgumentException">The value specified was less than zero</exception>
        public override float DPI {
            set {
                base.DPI = value;
                CalculateSizes();
            }
        }

        /// <summary>
        /// Checks to see if a specified size is valid.
        /// </summary>
        /// <param name="size">A size to test for validity.</param>
        /// <returns>True if this size is valid, false otherwise.</returns>
        public override bool IsValidSize(Size size) {
            return size.Width == Width && size.Height >= Height;
        }

        /// <summary>
        /// Given a size, returns the largest valid size contained by that size.
        /// </summary>
        /// <param name="size">A maximum size, from which to find a valid size.</param>
        /// <returns>A valid size.</returns>
        /// <exception cref="System.ArgumentException">The specified size is smaller than the minimum size in one or both dimensions.</exception>
        public override Size GetValidSize(Size size) {
            if (size.Width < Width || size.Height < Height)
                throw new ArgumentException("The specified size is smaller than the minimum size.");

            return new Size(Width, size.Height);
        }

        /// <summary>
        /// The rendering mode flags, which control the way the barcode is rendered.
        /// </summary>
        /// <value>The flags which control which aspects of the barcode are rendered.</value>
        /// <remarks>The flags which may be used are <see cref="BarcodeRenderMode.Guarded"/>, <see cref="BarcodeRenderMode.Notched"/>,
        /// and <see cref="BarcodeRenderMode.Numbered"/>.  All other flags have no effect.  If the
        /// <see cref="BarcodeRenderMode.Numbered"/> flag is specified, the <see cref="BarcodeRenderMode.Guarded"/> and <see cref="BarcodeRenderMode.Notched"/>
        /// are automatically set.</remarks>
        public override BarcodeRenderMode Mode {
            get {
                return base.Mode;
            }
            set {
                base.Mode = value;
                if ((value & BarcodeRenderMode.Numbered) != 0) // Numbered barcodes must be notched and guarded.
                    base.Mode = value | BarcodeRenderMode.Notched | BarcodeRenderMode.Guarded;
            }
        }

        #region IBarcodeModularSizer Members

        /// <summary>
        /// Gets and sets module width of the barcode (in mils).
        /// </summary>
        /// <value>The module width of the barcode, in mils (1 mil=1/1000 of an inch).</value>
        /// <remarks>The default value is 13 mils (.33 mm).</remarks>
        /// <exception cref="System.ArgumentException">The specified module width is less than <see cref="Barcodes.IBarcodeModularSizer.MinimumModule"/>.</exception>
        public float Module {
            get {
                return currentX / 10;
            }
            set {
                if (value < 10.4f)
                    throw new ArgumentException("The specified module size is below the minimum of 10.4 mils.");
                currentX = value * 10;
                CalculateSizes();
            }
        }

        /// <summary>
        /// The minimum module width of the barcode (in mils).
        /// </summary>
        /// <value>Always returns 10.4 mils (.264 mm).</value>
        /// <remarks>The value of <see cref="Barcodes.EANSizer.Module"/> cannot be set lower than the value
        /// returned by this property.</remarks>
        public float MinimumModule {
            get { return 10.4f; }
        }

        #endregion
    }

    /// <summary>
    /// The base class for the EAN-based barcode generators.
    /// </summary>
    /// <remarks>This class implements the actual barcode generation method, and call the 
    /// <see cref="Barcodes.EANGenerator.Decorator"/> method in derived classes to add the 
    /// notches and numbering to the barcode.  It does not implement the <see cref="Barcodes.BarcodeGenerator.Data"/>
    /// property, nor does it handle storage of encoded data.  That is the responsibility of 
    /// derived classes.  The only way this class accesses encoded data is via the 
    /// <see cref="Barcodes.EANGenerator.GenerateEncoding"/> method.</remarks>
    public abstract class EANGenerator : BarcodeGenerator {
        /// <summary>
        /// Called by <see cref="GenerateBarcode"/> to obtain the encoded module pattern.
        /// </summary>
        /// <returns>A <see cref="System.Collections.BitArray"/> containing the module pattern.</returns>
        /// <remarks>The returned bit array contains one bit for each module that makes up the width
        /// of the barcode itself (excluding guard zones).  A true value is rendered as a dark bar, and
        /// a false value is a blank space.</remarks>
        protected abstract BitArray GenerateEncoding();

        /// <summary>
        /// Called by <see cref="GenerateBarcode"/> to decorate the barcode with notches and numbering.
        /// </summary>
        /// <param name="g">A graphics contexts containing the barcode.</param>
        /// <param name="size">The size of the barcode bitmap.</param>
        /// <remarks>When this method is called, the barcode has been drawn into the graphics context,
        /// between two blank guard zones.  The barcode itself stretches from the top to the bottom of
        /// the bitmap.  It is the job of the decorator to trim the barcode the the dimensions specified
        /// by <see cref="Barcodes.EANSizer"/>.</remarks>
        protected abstract void Decorator(Graphics g, Size size);

        /// <summary>
        /// Gets the generator's capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/>.</value>
        public override BarcodeGeneratorFlags Flags {
            get { return BarcodeGeneratorFlags.Linear; }
        }

        /// <summary>
        /// Generates a bitmap of the barcode, at a specified size.
        /// </summary>
        /// <param name="size">The size of bitmap to generate.</param>
        /// <returns>A bitmap of the barcode.</returns>
        /// <remarks>The order of operations is as follows:
        /// <list type="number">
        /// <item><description>Check the <paramref name="size"/> and <see cref="Barcodes.BarcodeGenerator.Data"/>
        /// to ensure validity.</description></item>
        /// <item><description>Call the <see cref="Barcodes.EANGenerator.GenerateEncoding"/> method.</description></item>
        /// <item><description>Using the data returned from step 2, generate a raw barcode bitmap.</description></item>
        /// <item><description>Call the <see cref="Barcodes.EANGenerator.Decorator"/> method.</description></item>
        /// <item><description>Return the resulting bitmap.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="System.ArgumentException">The specified <paramref name="size"/> was not valid.</exception>
        /// <exception cref="System.InvalidOperationException">The generator does not have any data set.</exception>
        public override Bitmap GenerateBarcode(Size size) {
            if (!Sizer.IsValidSize(size))
                throw new ArgumentException("The specified size is not valid.");
            if (Data == null)
                throw new InvalidOperationException("The generator does not have any encoded data.");
            BitArray encodedBits = GenerateEncoding();

            int module;
            EANSizer eanSizer = ((EANSizer)this.Sizer);

            module = eanSizer.ModuleWidth;

            Bitmap bm = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppRgb);

            BitmapData bd = bm.LockBits(Rectangle.FromLTRB(0, 0, size.Width, size.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            int i;
            int[] block = new int[bd.Width * bd.Height];
            for (i = 0; i < block.Length; i++)
                block[i] = -1;

            //Marshal.Copy(bd.Scan0, block, 0, block.Length);
            //Generate the first row.
            int gap = size.Width - eanSizer.RightGuard - eanSizer.LeftGuard;
            int offset = eanSizer.LeftGuard;

            for (i = 0; i < gap; i++, offset++)
                if (encodedBits[i / module])
                    block[offset] = 0;

            //Fill down.
            for (i = bd.Width; i < block.Length; i++)
                block[i] = block[i - bd.Width];
            Marshal.Copy(block, 0, bd.Scan0, block.Length);
            bm.UnlockBits(bd);

            //Decorate with notches and numbers.
            Graphics g = Graphics.FromImage(bm);
            Decorator(g, size);
            g.Dispose();

            return bm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.EANGenerator"/> class, with the specified sizer.
        /// </summary>
        /// <param name="sizer">The sizer for the generator to use.</param>
        protected EANGenerator(IBarcodeSizer sizer) : base(sizer) { }
    }

    /// <summary>
    /// A generator for EAN13/UPCA barcodes.
    /// </summary>
    public class EAN13Generator : EANGenerator {
        private byte[] data = null;
        private BitArray encodedBits;

        /// <summary>
        /// Gets or sets the byte array of encoded data used by the generator to generate the barcode.
        /// </summary>
        /// <value>The byte array of encoded data.</value>
        /// <remarks>This data only has very basic validation to ensure that it does not cause exceptions
        /// when used (such as array overruns). It is not checked for validity.  The user should not set this
        /// directly, instead, it should be set by an encoder class.  This property does <b>not</b> make a
        /// copy of the byte array that is supplied to it, it only keeps a reference.  Its behavior if the underlying
        /// array is modified, is undefined.</remarks>
        /// <exception cref="System.ArgumentException">The specified byte array is invalid, in either length or content.
        /// The <see cref="System.ArgumentException.ParamName"/> property will be "Length" or "Data", respectively.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                if (value != null) {
                    if (value.Length != 13)
                        throw new ArgumentException("EAN13 data is of the incorrect length.", "Length");
                    foreach (byte b in value)
                        if (b >= 10)
                            throw new ArgumentException("EAN13 data contains invalid data.","Data");
                }
                data = value;
                encodedBits = null;
            }
        }

        private string upperText=null;

        /// <summary>
        /// Gets or sets the text to appear above the barcode.
        /// </summary>
        /// <value>The text to appear above the barcode.</value>
        /// <remarks>The minimum height of the barcode will be increased, depending on the number
        /// of lines of text that are displayed.  However, the width of the barcode will not be, so
        /// the length of the lines of text should be kept short.</remarks>
        public string UpperText {
            get { return upperText; }
            set { 
                upperText = value;
                if (upperText == null)
                    ((EANSizer)Sizer).UpperLines = 0;
                else {
                    int lines = 1;
                    foreach (char c in value)
                        if (c == '\n')
                            lines++;
                    ((EANSizer)Sizer).UpperLines = lines;
                }
            }
        }

        #region Encoding Table
        private static readonly bool[,] encoding =
             {
                {false,false,false,true,true,false,true},
                {false,false,true,true,false,false,true},
                {false,false,true,false,false,true,true},
                {false,true,true,true,true,false,true},
                {false,true,false,false,false,true,true},
                {false,true,true,false,false,false,true},
                {false,true,false,true,true,true,true},
                {false,true,true,true,false,true,true},
                {false,true,true,false,true,true,true},
                {false,false,false,true,false,true,true}
            };

        private static readonly bool[,] parity =
           {
                {false,false,false,false,false,false},
                {false,false,true,false,true,true},
                {false,false,true,true,false,true},
                {false,false,true,true,true,false},
                {false,true,false,false,true,true},
                {false,true,true,false,false,true},
                {false,true,true,true,false,false},
                {false,true,false,true,false,true},
                {false,true,false,true,true,false},
                {false,true,true,false,true,false}
            };
        #endregion

        /// <summary>
        /// Generates the pattern of bars and spaces that comprise the barcode.
        /// </summary>
        /// <returns>A <see cref="System.Collections.BitArray"/> representing the barcode pattern.</returns>
        protected override BitArray GenerateEncoding() {
            if (encodedBits != null)
                return encodedBits;
            encodedBits = new BitArray(95);

            int pos = 0;
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            int i, j;
            byte p = data[0];
            bool bit;
            for (i = 1; i < 7; i++) {
                byte x = data[i];
                for (j = 0; j < 7; j++) {
                    if (parity[p, i - 1])
                        bit = !encoding[x, 6 - j];
                    else
                        bit = encoding[x, j];
                    encodedBits[pos++] = bit;
                }
            }
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            for (i = 7; i < 13; i++) {
                byte x = data[i];
                for (j = 0; j < 7; j++) {
                    encodedBits[pos++] = !encoding[x, j];
                }
            }
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            return encodedBits;
        }

        /// <summary>
        /// Adds the notches and numbers to the completed barcode.
        /// </summary>
        /// <param name="g">A graphics context containing the barcode.</param>
        /// <param name="size">The size of the barcode bitmap.</param>
        protected override void Decorator(Graphics g, Size size) {
            EANSizer eanSizer = (EANSizer)Sizer;
            int module = eanSizer.ModuleWidth;
            int leftGuard = eanSizer.LeftGuard;
            int rightGuard = size.Width - eanSizer.RightGuard;
            int bottom = size.Height - eanSizer.BottomDepth;
            int notchGuard = bottom - eanSizer.NotchDepth;
            float fontSize=eanSizer.FontHeight;
            //((EANSizer)sizer).DecodeSize(size, out module, out leftGuard, out rightGuard, out notchGuard, out bottom, out fontSize);

            int leftNotchLeft = leftGuard + module * 3;
            if (isUPCA) //UPCA drops the first and last encoded symbols out of the notch.
                leftNotchLeft += module * 7;
            int leftNotchRight = leftGuard + module * (3 + 7 * 6);
            int rightNotchLeft = leftGuard + module * (8 + 7 * 6);
            int rightNotchRight = leftGuard + module * (8 + 7 * 12);
            if (isUPCA)
                rightNotchRight -= module * 7;

            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(leftNotchLeft, notchGuard, leftNotchRight, size.Height));
            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(rightNotchLeft, notchGuard, rightNotchRight, size.Height));
            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(0, bottom, size.Width, size.Height));

            if (fontSize != 0) {
                byte[] data = Data;
                string leftString = "", rightString = "";
                int i;
                if (isUPCA) {
                    for (i = 2; i < 7; i++) {
                        leftString += data[i].ToString();
                        rightString += data[i + 5].ToString();
                    }
                } else {
                    for (i = 1; i < 7; i++) {
                        leftString += data[i].ToString();
                        rightString += data[i + 6].ToString();
                    }
                }

                Font f = FontHolder.GenerateFont(fontSize);
                Font sf = FontHolder.GenerateFont(fontSize * .8f);

                if ((Sizer.Mode & BarcodeRenderMode.Guarded) != 0) {
                    if (isUPCA) {
                        g.DrawString(data[1].ToString(), sf, Brushes.Black, new PointF(leftGuard, notchGuard + module - f.Height + f.Size), FontHolder.RightJustifyRTL);
                        g.DrawString(data[12].ToString(), sf, Brushes.Black, new PointF(rightGuard, notchGuard + module - f.Height + f.Size), FontHolder.LeftJustify);
                    } else {
                        g.DrawString(data[0].ToString(), sf, Brushes.Black, new PointF(leftGuard, notchGuard + module - f.Height + f.Size), FontHolder.RightJustifyRTL);
                    }
                }
                g.DrawString(leftString, f, Brushes.Black, RectangleF.FromLTRB(leftNotchLeft + module, notchGuard + module - f.Height + f.Size, leftNotchRight - module, size.Height), FontHolder.CenterJustify);
                g.DrawString(rightString, f, Brushes.Black, RectangleF.FromLTRB(rightNotchLeft + module, notchGuard + module - f.Height + f.Size, rightNotchRight - module, size.Height), FontHolder.CenterJustify);
                if (upperText != null) {
                    g.FillRectangle(Brushes.White, 0, 0, size.Width, eanSizer.TopDepth);
                    g.DrawString(upperText, sf, Brushes.Black, RectangleF.FromLTRB(0, 0, size.Width, eanSizer.TopDepth), FontHolder.CenterJustify);
                }
            }
        }

        private bool isUPCA = false;

        /// <summary>
        /// Initializes a new instance of <see cref="Barcodes.EAN13Generator"/>, set to generate EAN13 barcodes.
        /// </summary>
        public EAN13Generator() : base(new EANSizer(11, 7, 95, 5)) { }

        /// <summary>
        /// Initializes a new instance of <see cref="Barcodes.EAN13Generator"/>, with an option to generate UPCA barcodes.
        /// </summary>
        /// <param name="isUPCA">True if the generator should generate UPCA barcodes.</param>
        public EAN13Generator(bool isUPCA)
            : base(new EANSizer(11, 7, 95, 5)) {
            this.isUPCA = isUPCA;
        }
    }

    /// <summary>
    /// A generator for EAN8 barcodes.
    /// </summary>
    public class EAN8Generator : EANGenerator {
        private byte[] data;
        private BitArray encodedBits;

        /// <summary>
        /// Gets or sets the byte array of encoded data used by the generator to generate the barcode.
        /// </summary>
        /// <value>The byte array of encoded data.</value>
        /// <remarks>This data only has very basic validation to ensure that it does not cause exceptions
        /// when used (such as array overruns). It is not checked for validity.  The user should not set this
        /// directly, instead, it should be set by an encoder class.  This property does <b>not</b> make a
        /// copy of the byte array that is supplied to it, it only keeps a reference.  Its behavior if the underlying
        /// array is modified, is undefined.</remarks>
        /// <exception cref="System.ArgumentException">The specified byte array is invalid, in either length or content.
        /// The <see cref="System.ArgumentException.ParamName"/> property will be "Length" or "Data", respectively.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                if (value != null) {
                    if (value.Length != 8)
                        throw new ArgumentException("EAN8 data is the incorrect length.", "Length");
                    foreach (byte b in value)
                        if (b >= 10)
                            throw new ArgumentException("EAN8 data contains invalid data.","Data");
                }
                data = value;
                encodedBits = null;
            }
        }

        #region Encoding Table
        private static readonly bool[,] encoding =
             {
                {false,false,false,true,true,false,true},
                {false,false,true,true,false,false,true},
                {false,false,true,false,false,true,true},
                {false,true,true,true,true,false,true},
                {false,true,false,false,false,true,true},
                {false,true,true,false,false,false,true},
                {false,true,false,true,true,true,true},
                {false,true,true,true,false,true,true},
                {false,true,true,false,true,true,true},
                {false,false,false,true,false,true,true}
            };
        #endregion

        /// <summary>
        /// Generates the pattern of bars and spaces that comprise the barcode.
        /// </summary>
        /// <returns>A <see cref="System.Collections.BitArray"/> representing the barcode pattern.</returns>
        protected override BitArray GenerateEncoding() {
            if (encodedBits != null)
                return encodedBits;
            encodedBits = new BitArray(67);

            int pos = 0;
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            int i, j;
            for (i = 0; i < 4; i++) {
                byte x = data[i];
                for (j = 0; j < 7; j++) {
                    encodedBits[pos++] = encoding[x, j];
                }
            }
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            for (i = 4; i < 8; i++) {
                byte x = data[i];
                for (j = 0; j < 7; j++) {
                    encodedBits[pos++] = !encoding[x, j];
                }
            }
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            Debug.Assert(pos == 67);
            return encodedBits;
        }

        /// <summary>
        /// Adds the notches and numbers to the completed barcode.
        /// </summary>
        /// <param name="g">A graphics context containing the barcode.</param>
        /// <param name="size">The size of the barcode bitmap.</param>
        protected override void Decorator(Graphics g, Size size) {
            EANSizer eanSizer = (EANSizer)Sizer;
            int module = eanSizer.ModuleWidth;
            int leftGuard=eanSizer.LeftGuard;
            int rightGuard=size.Width-eanSizer.RightGuard;            
            int bottom=size.Height-eanSizer.BottomDepth;
            int notchGuard=bottom-eanSizer.NotchDepth;
            float fontSize=eanSizer.FontHeight;

            int leftNotchLeft = leftGuard + module * 3;
            int leftNotchRight = leftGuard + module * (3 + 7 * 4);
            int rightNotchLeft = leftGuard + module * (8 + 7 * 4);
            int rightNotchRight = leftGuard + module * (8 + 7 * 8);

            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(leftNotchLeft, notchGuard, leftNotchRight, size.Height));
            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(rightNotchLeft, notchGuard, rightNotchRight, size.Height));
            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(0, bottom, size.Width, size.Height));

            if (fontSize != 0) {
                byte[] data = Data;
                string leftString = "", rightString = "";
                int i;
                for (i = 0; i < 4; i++) {
                    leftString += data[i].ToString();
                    rightString += data[i + 4].ToString();
                }


                Font f = FontHolder.GenerateFont(fontSize);

                g.DrawString(leftString, f, Brushes.Black, RectangleF.FromLTRB(leftNotchLeft + module, notchGuard + module - f.Height + f.Size, leftNotchRight - module, size.Height), FontHolder.CenterJustify);
                g.DrawString(rightString, f, Brushes.Black, RectangleF.FromLTRB(rightNotchLeft + module, notchGuard + module - f.Height + f.Size, rightNotchRight - module, size.Height), FontHolder.CenterJustify);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.EAN8Generator"/> class.
        /// </summary>
        public EAN8Generator() : base(new EANSizer(7, 7, 67, 5)) { }
    }

    /// <summary>
    /// A generator for UPCE barcodes.
    /// </summary>
    public class UPCEGenerator : EANGenerator {
        private byte[] data;
        private BitArray encodedBits;

        /// <summary>
        /// Gets or sets the byte array of encoded data used by the generator to generate the barcode.
        /// </summary>
        /// <value>The byte array of encoded data.</value>
        /// <remarks>This data only has very basic validation to ensure that it does not cause exceptions
        /// when used (such as array overruns). It is not checked for validity.  The user should not set this
        /// directly, instead, it should be set by an encoder class.  This property does <b>not</b> make a
        /// copy of the byte array that is supplied to it, it only keeps a reference.  Its behavior if the underlying
        /// array is modified, is undefined.</remarks>
        /// <exception cref="System.ArgumentException">The specified byte array is invalid, in either length or content.
        /// The <see cref="System.ArgumentException.ParamName"/> property will be "Length" or "Data", respectively.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                if (value != null) {
                    if (value.Length != 8)
                        throw new ArgumentException("UPCE data is the wrong length.","Length");
                    foreach (byte b in value)
                        if (b>=10)
                            throw new ArgumentException("UPCE data contains invalid data.","Data");
                    if ((value[0] & 0xfe) != 0)
                        throw new ArgumentException("UPCE data contains invalid data.", "Data");
                }
                data = value;
                encodedBits = null;
            }
        }

        #region Encoding Table
        private static readonly bool[,] encoding =
             {
                {false,false,false,true,true,false,true},
                {false,false,true,true,false,false,true},
                {false,false,true,false,false,true,true},
                {false,true,true,true,true,false,true},
                {false,true,false,false,false,true,true},
                {false,true,true,false,false,false,true},
                {false,true,false,true,true,true,true},
                {false,true,true,true,false,true,true},
                {false,true,true,false,true,true,true},
                {false,false,false,true,false,true,true}
            };

        private static readonly bool[,] parity =
           {
                {true,true,true,false,false,false},
                {true,true,false,true,false,false},
                {true,true,false,false,true,false},
                {true,true,false,false,false,true},
                {true,false,true,true,false,false},
                {true,false,false,true,true,false},
                {true,false,false,false,true,true},
                {true,false,true,false,true,false},
                {true,false,true,false,false,true},
                {true,false,false,true,false,true}
            };
        #endregion

        /// <summary>
        /// Generates the pattern of bars and spaces that comprise the barcode.
        /// </summary>
        /// <returns>A <see cref="System.Collections.BitArray"/> representing the barcode pattern.</returns>
        protected override BitArray GenerateEncoding() {
            if (encodedBits != null)
                return encodedBits;
            encodedBits = new BitArray(65);

            int pos = 0;
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            int i, j;
            bool invert = (data[0] == 1);
            byte p = data[7];
            bool bit;
            for (i = 1; i < 7; i++) {
                byte x = data[i];
                for (j = 0; j < 7; j++) {
                    if (parity[p, i - 1] ^ invert)
                        bit = !encoding[x, 6 - j];
                    else
                        bit = encoding[x, j];
                    encodedBits[pos++] = bit;
                }
            }
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            Debug.Assert(pos == 51);
            return encodedBits;
        }

        /// <summary>
        /// Adds the notches and numbers to the completed barcode.
        /// </summary>
        /// <param name="g">A graphics context containing the barcode.</param>
        /// <param name="size">The size of the barcode bitmap.</param>
        protected override void Decorator(Graphics g, Size size) {
            EANSizer eanSizer = (EANSizer)Sizer;
            int module = eanSizer.ModuleWidth;
            int leftGuard = eanSizer.LeftGuard;
            int rightGuard = size.Width - eanSizer.RightGuard;
            int bottom = size.Height - eanSizer.BottomDepth;
            int notchGuard = bottom - eanSizer.NotchDepth;
            float fontSize = eanSizer.FontHeight;

            int leftNotchLeft = leftGuard + module * 3;
            int leftNotchRight = leftGuard + module * (3 + 7 * 6);

            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(leftNotchLeft, notchGuard, leftNotchRight, size.Height));
            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(0, bottom, size.Width, size.Height));

            if (fontSize != 0) {
                byte[] data = Data;
                string leftString = "";
                int i;
                for (i = 1; i < 7; i++) {
                    leftString += data[i].ToString();
                }
                Font f = FontHolder.GenerateFont(fontSize);
                Font sf = FontHolder.GenerateFont(fontSize * .8f);
                if ((Sizer.Mode & BarcodeRenderMode.Guarded) != 0) {
                    g.DrawString(data[0].ToString(), sf, Brushes.Black, new PointF(leftGuard, notchGuard + module - f.Height + f.Size), FontHolder.RightJustifyRTL);
                    g.DrawString(data[7].ToString(), sf, Brushes.Black, new PointF(rightGuard, notchGuard + module - f.Height + f.Size), FontHolder.LeftJustify);
                }
                g.DrawString(leftString, f, Brushes.Black, RectangleF.FromLTRB(leftNotchLeft + module, notchGuard + module - f.Height + f.Size, leftNotchRight - module, size.Height), FontHolder.CenterJustify);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.UPCEGenerator"/> class.
        /// </summary>
        public UPCEGenerator() : base(new EANSizer(9, 7, 51, 5)) { }
    }

    /// <summary>
    /// A generator for UPC+2 barcodes.
    /// </summary>
    public class UPC2Generator : EANGenerator {
        private byte[] data;
        private BitArray encodedBits;

        #region Encoding Table
        private static readonly bool[,] encoding =
             {
                {false,false,false,true,true,false,true},
                {false,false,true,true,false,false,true},
                {false,false,true,false,false,true,true},
                {false,true,true,true,true,false,true},
                {false,true,false,false,false,true,true},
                {false,true,true,false,false,false,true},
                {false,true,false,true,true,true,true},
                {false,true,true,true,false,true,true},
                {false,true,true,false,true,true,true},
                {false,false,false,true,false,true,true}
            };
        
        private static readonly bool[,] parity ={ { false, false }, { false, true }, { true, false }, { true, true } };
        #endregion

        /// <summary>
        /// Gets or sets the byte array of encoded data used by the generator to generate the barcode.
        /// </summary>
        /// <value>The byte array of encoded data.</value>
        /// <remarks>This data only has very basic validation to ensure that it does not cause exceptions
        /// when used (such as array overruns). It is not checked for validity.  The user should not set this
        /// directly, instead, it should be set by an encoder class.  This property does <b>not</b> make a
        /// copy of the byte array that is supplied to it, it only keeps a reference.  Its behavior if the underlying
        /// array is modified, is undefined.</remarks>
        /// <exception cref="System.ArgumentException">The specified byte array is invalid, in either length or content.
        /// The <see cref="System.ArgumentException.ParamName"/> property will be "Length" or "Data", respectively.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                if (value != null) {
                    if (value.Length != 2)
                        throw new ArgumentException("UPC2 data is the wrong length.","Length");
                    foreach (byte b in value)
                        if (b >= 10)
                            throw new ArgumentException("UPC2 data contains invalid data.","Data");
                }
                data = value;
                encodedBits = null;
            }
        }

        private void EncodeByte(ref int pos, byte data, bool even) {
            for (int i = 0; i < 7; i++) {
                if (even)
                    encodedBits[pos++] = !encoding[data, 6 - i];
                else
                    encodedBits[pos++] = encoding[data, i];
            }
        }

        /// <summary>
        /// Generates the pattern of bars and spaces that comprise the barcode.
        /// </summary>
        /// <returns>A <see cref="System.Collections.BitArray"/> representing the barcode pattern.</returns>
        protected override BitArray GenerateEncoding() {
            if (encodedBits != null)
                return encodedBits;

            encodedBits = new BitArray(20);
            int pos = 0;

            int p = (data[0] * 10 + data[1]) % 4;

            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            encodedBits[pos++] = true;
            EncodeByte(ref pos, data[0], parity[p,0]);
            encodedBits[pos++]=false;
            encodedBits[pos++]=true;
            EncodeByte(ref pos, data[1],parity[p,1]);

            return encodedBits;
        }

        /// <summary>
        /// Adds the numbers to the completed barcode.
        /// </summary>
        /// <param name="g">A graphics context containing the barcode.</param>
        /// <param name="size">The size of the barcode bitmap.</param>
        protected override void Decorator(Graphics g, Size size) {
            EANSizer eanSizer = (EANSizer)Sizer;
            int module = eanSizer.ModuleWidth;
            int leftGuard = eanSizer.LeftGuard;
            int rightGuard = size.Width - eanSizer.RightGuard;
            int bottom = size.Height - eanSizer.BottomDepth;
            int notchGuard = bottom - eanSizer.NotchDepth;
            float fontSize = eanSizer.FontHeight;


            //Numbers are put on top, so use (bottom-size.Height) as the size to put at the top.
            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(0, 0, size.Width, size.Height-bottom));

            if (fontSize != 0) {
                byte[] data = Data;
                string leftString = data[0].ToString()+data[1].ToString();
                Font f = FontHolder.GenerateFont(fontSize);
                g.DrawString(leftString, f, Brushes.Black, new RectangleF(leftGuard, 0, rightGuard - leftGuard, size.Height - bottom), FontHolder.CenterJustify);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.UPC2Generator"/> class.
        /// </summary>
        public UPC2Generator() : base(new EANSizer(9,5,20,0)) {}
    }

    /// <summary>
    /// A generator for UPC+5 barcodes.
    /// </summary>
    public class UPC5Generator : EANGenerator {
        private byte[] data;
        private BitArray encodedBits;

        /// <summary>
        /// Gets or sets the byte array of encoded data used by the generator to generate the barcode.
        /// </summary>
        /// <value>The byte array of encoded data.</value>
        /// <remarks>This data only has very basic validation to ensure that it does not cause exceptions
        /// when used (such as array overruns). It is not checked for validity.  The user should not set this
        /// directly, instead, it should be set by an encoder class.  This property does <b>not</b> make a
        /// copy of the byte array that is supplied to it, it only keeps a reference.  Its behavior if the underlying
        /// array is modified, is undefined.</remarks>
        /// <exception cref="System.ArgumentException">The specified byte array is invalid, in either length or content.
        /// The <see cref="System.ArgumentException.ParamName"/> property will be "Length" or "Data", respectively.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                if (value != null) {
                    if (value.Length != 5)
                        throw new ArgumentException("UPC2 data is the wrong length.");
                    foreach (byte b in value)
                        if (b >= 10)
                            throw new ArgumentException("UPC2 data contains invalid data.");
                }
                data = value;
                encodedBits = null;
            }
        }

        #region Encoding Table
        private static readonly bool[,] encoding = {
            {false,false,false,true,true,false,true},
            {false,false,true,true,false,false,true},
            {false,false,true,false,false,true,true},
            {false,true,true,true,true,false,true},
            {false,true,false,false,false,true,true},
            {false,true,true,false,false,false,true},
            {false,true,false,true,true,true,true},
            {false,true,true,true,false,true,true},
            {false,true,true,false,true,true,true},
            {false,false,false,true,false,true,true}
        };

        private static readonly bool[,] parity ={
            {true, true, false, false, false},
            {true, false, true, false, false},
            {true, false, false, true, false},
            {true, false, false, false, true},
            {false, true, true, false, false},
            {false, false, true, true, false},
            {false, false, false, true, true},
            {false, true, false, true, false},
            {false, true, false, false, true},
            {false, false, true, false, true},
        };
        #endregion

        private void EncodeByte(ref int pos, byte data, bool even) {
            for (int i = 0; i < 7; i++) {
                if (even)
                    encodedBits[pos++] = !encoding[data, 6 - i];
                else
                    encodedBits[pos++] = encoding[data, i];
            }
        }

        /// <summary>
        /// Generates the pattern of bars and spaces that comprise the barcode.
        /// </summary>
        /// <returns>A <see cref="System.Collections.BitArray"/> representing the barcode pattern.</returns>
        protected override BitArray GenerateEncoding() {
            if (encodedBits != null)
                return encodedBits;

            encodedBits = new BitArray(47);
            int pos = 0;
            int p = (data[0] + data[2] + data[4]) * 3 + (data[1] + data[3]) * 9;
            p = p % 10;

            encodedBits[pos++] = true;
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            encodedBits[pos++] = true;
            EncodeByte(ref pos, data[0], parity[p, 0]);
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            EncodeByte(ref pos, data[1], parity[p, 1]);
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            EncodeByte(ref pos, data[2], parity[p, 2]);
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            EncodeByte(ref pos, data[3], parity[p, 3]);
            encodedBits[pos++] = false;
            encodedBits[pos++] = true;
            EncodeByte(ref pos, data[4], parity[p, 4]);

            return encodedBits;
        }

        /// <summary>
        /// Adds the numbers to the completed barcode.
        /// </summary>
        /// <param name="g">A graphics context containing the barcode.</param>
        /// <param name="size">The size of the barcode bitmap.</param>
        protected override void Decorator(Graphics g, Size size) {
            EANSizer eanSizer = (EANSizer)Sizer;
            int module = eanSizer.ModuleWidth;
            int leftGuard = eanSizer.LeftGuard;
            int rightGuard = size.Width - eanSizer.RightGuard;
            int bottom = size.Height - eanSizer.BottomDepth;
            int notchGuard = bottom - eanSizer.NotchDepth;
            float fontSize = eanSizer.FontHeight;

            //Numbers are put on top, so use (bottom-size.Height) as the size to put at the top.
            g.FillRectangle(Brushes.White, Rectangle.FromLTRB(0, 0, size.Width, size.Height-bottom));

            if (fontSize != 0) {
                byte[] data = Data;
                string leftString="";
                for (int i = 0; i < 5; i++)
                    leftString += data[i].ToString();
                Font f = FontHolder.GenerateFont(fontSize);
                g.DrawString(leftString, f, Brushes.Black, new RectangleF(leftGuard, 0,rightGuard-leftGuard,size.Height-bottom), FontHolder.CenterJustify);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.UPC5Generator"/> class.
        /// </summary>
        public UPC5Generator() : base(new EANSizer(9, 5, 47, 0)) { }
    }

    //U - UPCA
    //M - UPCE
    //E - EAN13/8
    //I - ISBN/ISBN13
    //S - ISSN
    /// <summary>
    /// A compositing encoder, for producing composite EAN barcodes.
    /// </summary>
    /// <remarks>A composite EAN barcode is an EAN barcode, followed by a UPC+2 or UPC+5 barcode.  The extra barcode is
    /// often used to provide supplemental information, such as price or issue number.  The two barcodes must meet certain
    /// sizing requirements, including maximum distance between the two barcodes, and relative heights.</remarks>
    public class EANCompositeEncoder : IBarcodeEncoder {
        private EANCompositeGenerator generator;

        private BarcodeEncoder leftEncoder=null, rightEncoder=null;

        #region IBarcodeEncoder Members

        /// <summary>
        /// Not supported.
        /// </summary>
        public byte[] Data {
            get {
                throw new NotSupportedException("This encoder cannot encode binary data.");
            }
            set {
                throw new NotSupportedException("This encoder cannot encode binary data.");
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public byte[] EncodedData {
            get { return null; }
        }

        //Parses the input and sets up generators.
        private void ProcessInput(string value) {
            if (value == null) {
                //Reset everything.
                generator.LeftSide = null;
                generator.RightSide = null;
                return;
            }

            string[] parts;
            string left, right;
            char type = value[0];

            parts = value.ToUpper().Substring(1).Split('/');
            if (parts.Length != 2)
                throw new ArgumentException("The data is invalid or not formatted properly.","Format");
            left = parts[0];
            right = parts[1];

            //Check the input.
            BarcodeEncoder newLeftEncoder=null;
            switch (type) {
                case 'E': //EAN
                    if (left.Length == 7 || left.Length == 8) {//EAN8
                        newLeftEncoder = new EAN8Encoder();
                    } else { //EAN13
                        newLeftEncoder = new EAN13Encoder();
                    } 
                    break;
                case 'I': //ISBN
                    int digitCount = 0;
                    foreach (char c in left.ToUpper())
                        if ("0123456789X".IndexOf(c) != -1)
                            digitCount++;
                    if (digitCount == 13) {
                        newLeftEncoder = new ISBN13Encoder();
                    } else {
                        newLeftEncoder = new ISBNEncoder();
                    }
                    break;
                case 'M': //UPCE
                    newLeftEncoder = new UPCEEncoder();
                    break;
                case 'S': //ISSN
                    newLeftEncoder = new ISSNEncoder();
                    break;
                case 'U': //UPCA
                    newLeftEncoder = new UPCAEncoder();
                    break;
                default:
                    throw new ArgumentException("The data type flag '" + type + "' is not recognized.","Flag");
            }
            try {
                newLeftEncoder.Text = left; //If this fails, it will throw an exception.
            } catch (ArgumentException ex) {
                throw new ArgumentException("The left barcode data could not be encoded.","Left", ex);
            }

            //Ok, the left encoder's data was valid.  Now try for the right.
            BarcodeEncoder newRightEncoder = new UPC25Encoder();
            try {
                newRightEncoder.Text = right;
            } catch (ArgumentException ex) {
                throw new ArgumentException("The right barcode data could not be encoded.","Right", ex);
            }

            //Ok, nothing bad happened. Rig up the generator.
            this.leftEncoder = newLeftEncoder;
            this.rightEncoder = newRightEncoder;

            generator.LeftSide = (EANGenerator)newLeftEncoder.Generator;
            generator.RightSide = (EANGenerator)newRightEncoder.Generator;
        }

        private string text;
        /// <summary>
        /// Gets or sets the string to be encoded.
        /// </summary>
        /// <value>The string to be encoded, or null.</value>
        /// <remarks>The data to be encoded must be formated as follows:
        /// <br/>&apos;<i>Mode</i>&apos; &lt;<i>Left Data</i>&gt;&apos;/&apos;&lt;<i>Right Data</i>&gt;
        /// <br/><br/>Mode is a single letter from the following list:
        /// <list type="table">
        /// <listheader><item>Letter</item><description>Left Barcode Type</description></listheader>
        /// <item><term>'E'</term><description>EAN13/EAN8</description></item>
        /// <item><term>'M'</term><description>UPCE</description></item>
        /// <item><term>'I'</term><description>ISBN/ISBN13</description></item>
        /// <item><term>'S'</term><description>ISSN</description></item>
        /// <item><term>'U'</term><description>UPCA</description></item>
        /// </list><br/>
        /// <i>Left Data</i> is the string that would be passed to the corresponding encoder's Text property.<br/>
        /// <i>Right Data</i> is the string that would be passed to the UPC+2 or UPC+5 encoder's Text property. The two data strings
        /// are separated by a slash '/', with no spaces.  The &lt;, &gt;, and &apos; are not used.
        /// <br/>The string is converted to uppercase before processing.
        /// </remarks>
        /// <example>
        /// I1-2345-6789-X/51999<br/>
        /// E9781234567897/52
        /// </example>
        /// <exception cref="System.ArgumentException">If there is an error in the formatting of the data string, this exception
        /// is thrown with its <see cref="System.ArgumentException.ParamName"/> property set to "Length" or "Flag", if the
        /// formatting is invalid or the mode flag is unrecognized, respectively.  If the left or right data is invalid,
        /// the exception's <see cref="System.ArgumentException.ParamName"/> property will be "Left" or "Right", and the
        /// inner exception will be the exception thrown by the underlying encoder.</exception>
        public string Text {
            get {
                return text;
            }
            set {
                ProcessInput(value);
                text = value; //ProcessInput will do all the work and throw exceptions on errors.
            }
        }

        /// <summary>
        /// Gets the encoder's capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.Text"/>|<see cref="Barcodes.BarcodeEncoderFlags.Composite"/>.</value>
        public BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.Text | BarcodeEncoderFlags.Composite; }
        }

        /// <summary>
        /// Gets the symbols that are recognized by the encoder, and its underlying encoders.
        /// </summary>
        /// <value>Always returns <c>"0123456789UuMmEeIiSsXx/"</c>. </value>
        /// <remarks>The actual symbols that are valid depends on the mode flag at the beginning of the data
        /// string. It is up to the underlying encoders to handle non-encodable symbols.</remarks>
        public string TextSymbols {
            get { return "0123456789UuMmEeIiSsXx/"; }
        }

        /// <summary>
        /// Gets the generator associated with this encoder.
        /// </summary>
        public IBarcodeGenerator Generator {
            get { return generator; }
        }

        /// <summary>
        /// Gets the sizer associated with the generator, that is associated with this encoder.
        /// </summary>
        public IBarcodeSizer Sizer {
            get { return generator.Sizer; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.EANCompositeEncoder"/> class.
        /// </summary>
        public EANCompositeEncoder() {
            generator = new EANCompositeGenerator();
        }
    }

    /// <summary>
    /// Sizing control for the two barcode generators that comprise the composite EAN barcode.
    /// </summary>
    public class EANCompositeSizer : IBarcodeSizer, IBarcodeModularSizer {
        private EANSizer leftSizer;
        private EANSizer rightSizer;

        /// <summary>
        /// Gets or sets the sizer for the left barcode.
        /// </summary>
        /// <value>The sizer for the left barcode.</value>
        /// <remarks>When a sizer is set, this property also sets the sizer's module width, render mode flags, and DPI.</remarks>
        public EANSizer LeftSizer {
            get { return leftSizer; }
            set { 
                leftSizer = value;
                //Make sure that the sizer has all its options set.
                if (leftSizer != null) {
                    Module = Math.Max(module, leftSizer.MinimumModule);
                    Mode = mode;
                    DPI = dpi;
                }
                CalculateSizes();
            }
        }

        /// <summary>
        /// Gets or sets the sizer for the right barcode.
        /// </summary>
        /// <value>The sizer for the right barcode.</value>
        /// <remarks>When a sizer is set, this property also sets the sizer's module width, render mode flags, and DPI.</remarks>
        public EANSizer RightSizer {
            get { return rightSizer; }
            set { 
                rightSizer = value;
                //Make sure that the sizer has all its options set.
                if (rightSizer != null) {
                    Module = Math.Max(module, rightSizer.MinimumModule);
                    Mode = mode;
                    DPI = dpi;
                }
                CalculateSizes();
            }
        }

        private void CalculateSizes() {
            if (leftSizer == null || rightSizer == null) {
                width = height = 0;
                return;
            }

            if ((Mode & BarcodeRenderMode.Guarded) != 0) {
                width = leftSizer.Width - leftSizer.RightGuard + 11 * leftSizer.ModuleWidth + rightSizer.Width - rightSizer.LeftGuard;
            } else {
                width = leftSizer.Width + rightSizer.Width + 11 * leftSizer.ModuleWidth;
            }

            height = Math.Max(rightSizer.Height + leftSizer.BottomDepth+leftSizer.TopDepth, leftSizer.Height);
        }

        #region IBarcodeSizer Members
        private int width=0;
        /// <summary>
        /// Returns the width of the composite barcode, in pixels.
        /// </summary>
        public int Width {
            get { return width; }
        }

        private int height=0;
        /// <summary>
        /// Returns the minimum height of the composite barcode, in pixels.
        /// </summary>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// Returns the height of extra items placed around the primary (left) barcode.
        /// </summary>
        /// <value>The height of extra items, in pixels.</value>
        /// <remarks>To set the height of the left barcode, multiply the desired height (in inches) by the
        /// DPI, and add this value.  The height of the secondary (right) barcode is dependant on the height of the
        /// primary barcode, and cannot be set separately.</remarks>
        public int ExtraHeight {
            get {
                if (leftSizer == null || rightSizer==null)
                    return 0;
                return Height - leftSizer.TopDepth - leftSizer.BottomDepth - leftSizer.NotchDepth; 
            }
        }

        /// <summary>
        /// Returns the minimum size of the composite barcode, in pixels.
        /// </summary>
        public Size Size {
            get { return new Size(width, height); }
        }

        private float dpi = 0f;
        /// <summary>
        /// Gets or sets the dots-per-inch (DPI) of the barcode.
        /// </summary>
        /// <value>The DPI of the barcode.</value>
        /// <remarks>If this is set to zero, the generators operate in "logical mode", which produces barcodes
        /// with the correct relative placement and sizing of bars and spaces, but not at the proper size for
        /// printing.  It might also not place the numbers correctly if it is operating in logical mode.</remarks>
        public float DPI {
            get {
                return dpi;
            }
            set {
                dpi = value;
                if (leftSizer != null)
                    leftSizer.DPI = dpi;
                if (rightSizer != null)
                    rightSizer.DPI = dpi;
                CalculateSizes();
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <value>Always returns 0.</value>
        public float AspectRatioMin {
            get { return 0; }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <value>Always returns 0.</value>
        public float AspectRatioMax {
            get { return 0; }
        }

        private BarcodeRenderMode mode;
        /// <summary>
        /// Gets or sets the rendering mode flags for the two underlying generators.
        /// </summary>
        /// <value>The current rendering mode flags.</value>
        /// <remarks>Valid rendering mode flags are <see cref="Barcodes.BarcodeRenderMode.Guarded"/>, 
        /// <see cref="Barcodes.BarcodeRenderMode.Notched"/>, and <see cref="Barcodes.BarcodeRenderMode.Numbered"/>.
        /// If <see cref="Barcodes.BarcodeRenderMode.Numbered"/> is set, it will automatically set
        /// <see cref="Barcodes.BarcodeRenderMode.Guarded"/> and <see cref="Barcodes.BarcodeRenderMode.Notched"/>.</remarks>
        public BarcodeRenderMode Mode {
            get {
                return mode;
            }
            set {
                mode = value;
                if ((mode & BarcodeRenderMode.Numbered) != 0)
                    mode |= BarcodeRenderMode.Guarded | BarcodeRenderMode.Notched;
                if (leftSizer!=null)
                leftSizer.Mode = mode;
                if (rightSizer!=null)
                rightSizer.Mode = mode;
            CalculateSizes();
            }
        }

        /// <summary>
        /// Checks to see if the specified size is valid.
        /// </summary>
        /// <param name="size">The size of the barcode, in pixels.</param>
        /// <returns>True if the barcode size is valid, false otherwise.</returns>
        public bool IsValidSize(Size size) {
            return (size.Width == width && size.Height >= height);
        }

        /// <summary>
        /// Calculate the largest barcode within a given size.
        /// </summary>
        /// <param name="size">The maximum size of the barcode, in pixels.</param>
        /// <returns>A valid barcode size.</returns>
        /// <exception cref="System.ArgumentException">The specified size was smaller than the minimum size in one or both dimensions.</exception>
        public Size GetValidSize(Size size) {
            if (size.Width < width || size.Height < height)
                throw new ArgumentException("The specified size is smaller than the minimum size.");
            return new Size(width, size.Height);
        }

        #endregion

        #region IBarcodeModularSizer Members
        private float module=13f;
        /// <summary>
        /// Gets or sets the desired module width, in mils.
        /// </summary>
        /// <value>The current desired module width, in mils (1 mil is 1/1000th of an inch, .0254 mm).  The default value is 13 mils (.33 mm).</value>
        /// <remarks>The barcode generator will generate a barcode with a module width that is as close as
        /// possible to, but not smaller than, the specified module width, within the limits of the 
        /// specified DPI.  It cannot be set lower than the value returned by <see cref="Barcodes.EANCompositeSizer.MinimumModule"/>.</remarks>
        /// <exception cref="System.ArgumentException">The specified module width is smaller than 
        /// <see cref="Barcodes.EANCompositeSizer.MinimumModule"/>.</exception>
        public float Module {
            get {
                return module;  
            }
            set {
                module = value;

                if (leftSizer!=null)
                leftSizer.Module = value;

                if (rightSizer!=null)
                rightSizer.Module = value;

            CalculateSizes();
            }
        }

        /// <summary>
        /// The minimum module width, in mils.
        /// </summary>
        /// <value>Always returns 10.4 mils.</value>
        /// <remarks>One mil is 1/1000th of an inch, or .0254 mm.</remarks>
        public float MinimumModule {
            get {
                //All of the EAN derived barcodes have a minimum module width of 10.4 mils.
                return 10.4f;
            }
        }

        #endregion

        //Given a size, calculate the size, placement, and cut of the left barcode.
        /// <summary>
        /// Calculates the size and placement of the left barcode, for use by the generator.
        /// </summary>
        /// <param name="size">The total size of the barcode.</param>
        /// <param name="leftPlacement">Returns the rectangle which specifies the placement in the finished barcode.</param>
        /// <param name="leftCut">Returns the rectangle which specified the portion of the generated barcode to place.</param>
        /// <returns>The size to pass to the left barcode generator.</returns>
        internal Size CalculateLeftSize(Size size, out Rectangle leftPlacement, out Rectangle leftCut) {
            leftPlacement = new Rectangle(0, 0, leftSizer.Width, size.Height);
            leftCut = leftPlacement;
            return new Size(leftSizer.Width, size.Height);
        }

        /// <summary>
        /// Calculates the size and placement of the right barcode, for use by the generator.
        /// </summary>
        /// <param name="size">The total size of the barcode.</param>
        /// <param name="rightPlacement">Returns the rectangle which specifies the placement in the finished barcode.</param>
        /// <param name="rightCut">Returns the rectangle which specified the portion of the generated barcode to place.</param>
        /// <returns>The size to pass to the right barcode generator.</returns>
        internal Size CalculateRightSize(Size size, out Rectangle rightPlacement, out Rectangle rightCut) {
            Size rightSize=new Size(rightSizer.Width, size.Height-leftSizer.BottomDepth-leftSizer.TopDepth);
            rightPlacement = Rectangle.FromLTRB(leftSizer.Width - leftSizer.RightGuard + 11 * leftSizer.ModuleWidth, leftSizer.TopDepth, size.Width, rightSize.Height+leftSizer.TopDepth);
            rightCut = Rectangle.FromLTRB(rightSizer.LeftGuard, 0, rightSize.Width, rightSize.Height);
            return rightSize;
        }
    }

    /// <summary>
    /// Generator which produces composite EAN barcodes.
    /// </summary>
    public class EANCompositeGenerator : IBarcodeGenerator {
        private EANGenerator leftSide;
        private EANGenerator rightSide;
        private EANCompositeSizer sizer;

        /// <summary>
        /// Gets or sets the left barcode generator.
        /// </summary>
        /// <value>The generator for the left barcode.</value>
        public EANGenerator LeftSide {
            get { return leftSide; }
            set {
                leftSide = value;
                if (value != null)
                    sizer.LeftSizer = (EANSizer)leftSide.Sizer;
                else
                    sizer.LeftSizer = null;
            }
        }


        /// <summary>
        /// Gets or sets the right barcode generator.
        /// </summary>
        /// <value>The generator for the right barcode.</value>
        public EANGenerator RightSide {
            get { return rightSide; }
            set {
                rightSide = value;
                if (value != null)
                    sizer.RightSizer = (EANSizer)rightSide.Sizer;
                else
                    sizer.RightSizer = null;
            }
        }

        #region IBarcodeGenerator Members

        /// <summary>
        /// Gets the composite barcode sizer.
        /// </summary>
        /// <value>The composite barcode sizer.</value>
        public IBarcodeSizer Sizer {
            get { return sizer; }
        }

        /// <summary>
        /// Gets the generator capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/> | <see cref="Barcodes.BarcodeGeneratorFlags.VariableLength"/>.</value>
        public BarcodeGeneratorFlags Flags {
            get { return BarcodeGeneratorFlags.Linear | BarcodeGeneratorFlags.VariableLength; }
        }

        /// <summary>
        /// Generates a composite barcode of the specified size.
        /// </summary>
        /// <param name="size">The size of the barcode to generate.</param>
        /// <returns>A bitmat containing the composite barcode.</returns>
        /// <exception cref="System.ArgumentException">The specified size is invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The left generator or the right generator, 
        /// or both, are not set up properly, or an error occured when the component barcodes were being 
        /// generated (in which case, the inner exception will be set.)</exception>
        public Bitmap GenerateBarcode(Size size) {
            if (!sizer.IsValidSize(size))
                throw new ArgumentException("The specified size is invalid.");
            if (leftSide == null)
                throw new InvalidOperationException("The left barcode generator is null.");
            if (rightSide == null)
                throw new InvalidOperationException("The right barcode generator is null.");

            Rectangle leftPlacement, leftCut;
            Size leftSize = sizer.CalculateLeftSize(size, out leftPlacement, out leftCut);
            Rectangle rightPlacement, rightCut;
            Size rightSize = sizer.CalculateRightSize(size, out rightPlacement, out rightCut);


            Bitmap leftBitmap,rightBitmap;

            try {
                leftBitmap = leftSide.GenerateBarcode(leftSize);
            } catch (Exception ex) {
                throw new InvalidOperationException("An error occured in the left generator.", ex);
            }

            try {
                rightBitmap = rightSide.GenerateBarcode(rightSize);
            } catch (Exception ex) {
                throw new InvalidOperationException("An error occured in the right generator.", ex);
            }

            Bitmap composite = new Bitmap(size.Width, size.Height,leftBitmap.PixelFormat);
            Graphics g = Graphics.FromImage(composite);

            g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);

            g.DrawImage(leftBitmap, leftPlacement, leftCut, GraphicsUnit.Pixel);
            g.DrawImage(rightBitmap, rightPlacement, rightCut, GraphicsUnit.Pixel);

            g.Dispose();

            return composite;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.EANCompositeGenerator"/> class.
        /// </summary>
        public EANCompositeGenerator() {
            sizer = new EANCompositeSizer();
        }
    }
}