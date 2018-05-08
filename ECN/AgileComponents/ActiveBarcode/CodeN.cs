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
    /// An encoder that generates Code 11 barcodes.
    /// </summary>
    /// <remarks>Code 11 is commonly used in telecommunications equipment; it is capable of
    /// encoding the digits 0-9 and the dash (-) symbol.  It is not a secure symbology, in the sense
    /// that it is possible for a printing error to convert one valid symbol into another,
    /// so the use of one or two check digits is recommended.  If the <see cref="Barcodes.Code11Encoder.AppendChecksum"/>
    /// property is set to true, the encoder will append one or two check digits, depending
    /// on the length of the data.</remarks>
    public class Code11Encoder : BarcodeEncoder {
        //I have read conflicting descriptions of the checksum calculation.
        //I have implemented the one that I found the most sources for.
        //Sum of all digits, weighted by position (1 is rightmost), mod 11 for C
        //and mod 9 for K.
        private void CheckAndEncodeText(string value, out byte[] data) {
            data = null;                

            if (useChecksum) {
                if (value.Length >= 10)
                    data = new byte[value.Length + 2];
                else
                    data = new byte[value.Length + 1];
            } else {
                data = new byte[value.Length];
            }

            int i = 0;
            for (; i < value.Length; i++) {
                int val="0123456789-".IndexOf(value[i]);
                if (val == -1)
                    throw new ArgumentException("The specified string contains symbols other than digits and the dash character.", "Digit");
                data[i] = (byte)val;
            }

            if (!useChecksum)
                return;

            //Calculate C.
            int weight = 1;
            int sum = 0;
            for (i = value.Length - 1; i >= 0; i--) {
                sum += data[i] * weight;
                weight++;
            }
            data[value.Length] = (byte)(sum % 11);

            if (value.Length < 10)
                return;

            //Calculate K
            weight = 1;
            sum = 0;
            for (i = value.Length; i >= 0; i--) {
                sum += data[i] * weight;
                weight++;
            }
            data[value.Length + 1] = (byte)(sum % 9);
        }

        string text;

        /// <summary>
        /// Gets or sets the data to be encoded by a barcode, as a text string.
        /// </summary>
        /// <value>The text to be encoded.</value>
        /// <remarks>Code 11 can only encode strings containing digits and dashes (-).</remarks>
        /// <exception cref="System.ArgumentException">The data supplied to the encoder contains
        /// characters other than 0-9 and dash (-).  The <see cref="System.ArgumentException.ParamName"/>
        /// property will be set to "Digit".</exception>
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
        /// Symbols that are encodable by this encoder.
        /// </summary>
        /// <value>Always returns <c>"0123456789-"</c>.</value>
        /// <remarks>This encoder can only encode digits and the dash (-) symbol.  If any other symbols are passed in, the encoder will fail.</remarks>
        public override string TextSymbols {
            get {
                return "0123456789-";
            }
        }

        bool useChecksum = true;
        /// <summary>
        /// Gets or sets whether checksum digits should be appended to the encoded data.
        /// </summary>
        /// <value>True if checksum digits should be appended, false otherwise.</value>
        /// <remarks><para>If this is set to true, the encoder will append one checksum
        /// digit if the data length is 9 digits or less, and two checksum characters
        /// if the length is ten or more.</para><para>The first digit appended is called
        /// the 'C' digit; it is calculated by weighting the data digits, from right to left,
        /// with a value of 1 for the rightmost digit, two for the next digit, etc. After the digits are weighted, they are summed, and the sum modulo 11
        /// is used as the first check digit.</para><para>The second check digit, if used,
        /// is called the 'K' digit.  Its calculation is similar to the 'C' digit, except the 'C'
        /// digit is the included as the first digit (weighted at 1), and the check digit is the remainder
        /// modulo 10 instead of modulo 9.</para><para>If the value of this property
        /// is changed after the data has been set, the data will be re-encoded to include or exclude
        /// the check digits.</para></remarks>
        public bool AppendChecksum {
            get {
                return useChecksum;
            }
            set {
                useChecksum = value;
                if (text != null) {
                    byte[] data;
                    CheckAndEncodeText(text, out data);
                    GeneratorInstance.Data = data;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code11Encoder"/> class.
        /// </summary>
        public Code11Encoder() : base(new Code11Generator()) { }
    }

    /// <summary>
    /// An encoder that generates Code 39 barcodes.
    /// </summary>
    /// <remarks><para>Code 39 is a very commonly used barcode, found in many fields.  It's standard form
    /// can encode the symbols 0-9, A-Z, as well as dash (-), dot (.), space ( ), dollar ($),
    /// slash (/), plus (+), and percent (%). It also has an extended form, which is capable of encoding
    /// all 128 of the lower ASCII symbols.  The encoder will not automatically switch to the
    /// extended encoding if extended characters are detected; this must be enabled by setting the
    /// <see cref="Barcodes.Code39Encoder.UseExtended"/> to true.</para><para>Code 39 barcodes don't normally require
    /// a checksum digit, but if the <see cref="Barcodes.Code39Encoder.AddChecksum"/> property is
    /// set to true, the encoder will automatically add a mod 43 checksum digit.</para></remarks>
    /// <seealso cref="Barcodes.Code39Encoder.UseExtended"/>
    /// <seealso cref="Barcodes.Code39Encoder.AddChecksum"/>
    /// <seealso cref="Barcodes.Code39Generator"/>
    /// <seealso cref="Barcodes.Code39Sizer"/>
    public class Code39Encoder : BarcodeEncoder {
        private byte[] data = null;
        private string text = null;

        private bool useExtended = false;
        /// <summary>
        /// Gets or sets whether the encoder should detect and encode characters of the full ASCII set
        /// </summary>
        /// <value>True if extended characters will be encoded, false otherwise. Defaults to false.</value>
        /// <remarks>Changing this property's value does not re-encode text or data that was set before
        /// the property's value was set.  You must set this property before setting the text or data.
        /// If this property is false, and extended characters are detected, the encoder will throw an
        /// exception.</remarks>
        public bool UseExtended {
            get { return useExtended; }
            set {
                useExtended = value;
            }
        }

        private bool addChecksum = false;

        /// <summary>
        /// Gets or sets whether the encoder should append a check digit.
        /// </summary>
        /// <value>True if a check digit should be appended, false otherwise.</value>
        public bool AddChecksum {
            get { return addChecksum; }
            set { addChecksum = value; }
        }

        #region Encoding Table
        // {Extended?, Basic, Ext1, Ext2}
        byte[,] encodingTable = 
            {
                {1, 0, 42, 30}, //0  - NUL
                {1, 0, 39, 10}, //1  - SOH
                {1, 0, 39, 11}, //2  - STX
                {1, 0, 39, 12}, //3  - ETX
                {1, 0, 39, 13}, //4  - EOT
                {1, 0, 39, 14}, //5  - ENQ
                {1, 0, 39, 15}, //6  - ACK
                {1, 0, 39, 16}, //7  - BEL
                {1, 0, 39, 17}, //8  - BS
                {1, 0, 39, 18}, //9  - HT
                {1, 0, 39, 19}, //10 - LF
                {1, 0, 39, 20}, //11 - VT
                {1, 0, 39, 21}, //12 - FF
                {1, 0, 39, 22}, //13 - CR
                {1, 0, 39, 23}, //14 - SO
                {1, 0, 39, 24}, //15 - SI
                {1, 0, 39, 25}, //16 - DLE
                {1, 0, 39, 26}, //17 - DC1
                {1, 0, 39, 27}, //18 - DC2
                {1, 0, 39, 28}, //19 - DC3
                {1, 0, 39, 29}, //20 - DC4
                {1, 0, 39, 30}, //21 - NAK
                {1, 0, 39, 31}, //22 - SYN
                {1, 0, 39, 32}, //23 - ETB
                {1, 0, 39, 33}, //24 - CAN
                {1, 0, 39, 34}, //25 - EM
                {1, 0, 39, 35}, //26 - SUB
                {1, 0, 42, 10}, //27 - ESC
                {1, 0, 42, 11}, //28 - FS
                {1, 0, 42, 12}, //29 - GS
                {1, 0, 42, 13}, //30 - RS
                {1, 0, 42, 14}, //31 - YS
                {0, 38, 38, 255}, //Space
                {1, 0, 40, 10},   //!
                {1, 0, 40, 11},   //"
                {1, 0, 40, 12},   //#
                {0, 39, 40, 13},  //$
                {0, 42, 40, 14},  //%
                {1, 0, 40, 15},   //&
                {1, 0, 40, 16},   //'
                {1, 0, 40, 17},   //( - 40
                {1, 0, 40, 18},   //)
                {1, 0, 40, 19},  //*
                {0, 41, 40, 20},  //+
                {1, 0, 40, 21},   //,
                {0, 36, 36, 255}, //-
                {0, 37, 37, 255}, //.
                {0, 40, 40, 24},  // /
                {0, 0, 0, 255},   //0
                {0, 1, 1, 255},
                {0, 2, 2, 255},   //2 - 50
                {0, 3, 3, 255},
                {0, 4, 4, 255},
                {0, 5, 5, 255},
                {0, 6, 6, 255},
                {0, 7, 7, 255},
                {0, 8, 8, 255},
                {0, 9, 9, 255},
                {1, 0, 40, 35},   //:
                {1, 0, 42, 15},   //;
                {1, 0, 42, 16},   //< - 60
                {1, 0, 42, 17},   //=
                {1, 0, 42, 18},   //>
                {1, 0, 42, 19},   //?
                {1, 0, 42, 31},   //@
                {0, 10, 10, 255}, //A
                {0, 11, 11, 255}, //B
                {0, 12, 12, 255}, //C
                {0, 13, 13, 255}, //D
                {0, 14, 14, 255}, //E
                {0, 15, 15, 255}, //F - 70
                {0, 16, 16, 255}, //G
                {0, 17, 17, 255}, //H
                {0, 18, 18, 255}, //I
                {0, 19, 19, 255}, //J
                {0, 20, 20, 255}, //K
                {0, 21, 21, 255}, //L
                {0, 22, 22, 255}, //M
                {0, 23, 23, 255}, //N
                {0, 24, 24, 255}, //O
                {0, 25, 25, 255}, //P - 80
                {0, 26, 26, 255}, //Q
                {0, 27, 27, 255}, //R
                {0, 28, 28, 255}, //S
                {0, 29, 29, 255}, //T
                {0, 30, 30, 255}, //U
                {0, 31, 31, 255}, //V
                {0, 32, 32, 255}, //W
                {0, 33, 33, 255}, //X
                {0, 34, 34, 255}, //Y
                {0, 35, 35, 255}, //Z - 90
                {1, 0, 42, 20},   //[
                {1, 0, 42, 21},   //\
                {1, 0, 42, 22},   //]
                {1, 0, 42, 23},   //^
                {1, 0, 42, 24},   //_
                {1, 0, 42, 32},   //`
                {1, 0, 41, 10},   //a
                {1, 0, 41, 11},   //b
                {1, 0, 41, 12},   //c
                {1, 0, 41, 13},   //d - 100
                {1, 0, 41, 14},   //e
                {1, 0, 41, 15},   //f
                {1, 0, 41, 16},   //g
                {1, 0, 41, 17},   //h
                {1, 0, 41, 18},   //i
                {1, 0, 41, 19},   //j
                {1, 0, 41, 20},   //k
                {1, 0, 41, 21},   //l
                {1, 0, 41, 22},   //m
                {1, 0, 41, 23},   //n - 110
                {1, 0, 41, 24},   //o
                {1, 0, 41, 25},   //p
                {1, 0, 41, 26},   //q
                {1, 0, 41, 27},   //r
                {1, 0, 41, 28},   //s
                {1, 0, 41, 29},   //t
                {1, 0, 41, 30},   //u
                {1, 0, 41, 31},   //v
                {1, 0, 41, 32},   //w
                {1, 0, 41, 33},   //x - 120
                {1, 0, 41, 34},   //y
                {1, 0, 41, 35},   //z
                {1, 0, 42, 25},   //{
                {1, 0, 42, 26},   //|
                {1, 0, 42, 27},   //}
                {1, 0, 42, 28},   //~
                {1, 0, 42, 29}    //DEL
            };
        #endregion

        private void CheckAndEncodeData(byte[] value, out byte[] encoded) {
            encoded = null;
            if (value == null)
                return;

            bool useExtended = false;
            //Check the data and determine if we need to use the extended encoding.
            foreach (byte b in value) {
                if (b > 127)
                    throw new ArgumentException("The data contains characters from outside the lower ASCII character set.", "Invalid");
                if (encodingTable[b, 0] == 1)
                    useExtended = true;
            }
            if (useExtended && !this.useExtended)
                throw new ArgumentException("The data contains characters from the extended encoding, and UseExtended is false.", "Extended");

            List<byte> bytes = new List<byte>();

            if (useExtended) {
                foreach (byte b in value) {
                    bytes.Add(encodingTable[b, 2]);
                    if (encodingTable[b, 3] != 255)
                        bytes.Add(encodingTable[b, 3]);
                }
            } else {
                foreach (byte b in value) {
                    bytes.Add(encodingTable[b, 1]);
                }
            }

            if (addChecksum) {
                int checkSum = 0;
                foreach (byte b in bytes)
                    checkSum += b;
                bytes.Add((byte)(checkSum % 43));
            }

            encoded = bytes.ToArray();
        }

        private void CheckAndEncodeText(string value, out byte[] encoded, ref byte[] data) {
            encoded = null;
            if (value == null)
                return;
            byte[] ascii = Encoding.UTF8.GetBytes(value);

            //CheckAndEncodeData will make sure that the ASCII is valid.
            CheckAndEncodeData(ascii, out encoded);
            data = ascii;
        }

        /// <summary>
        /// Gets or sets the data to be encoded into a barcode.
        /// </summary>
        /// <value>A byte array of data to be encoded.</value>
        /// <remarks>The byte values in the byte array are encoded as their corresponding ASCII characters.
        /// If any byte contains a value
        /// above 127, an <see cref="System.ArgumentException"/> will be thrown.  Likewise, if the byte array contains
        /// values outside of the standard encoding system (0-9, A-Z, -. $/+%), and <see cref="Barcodes.Code39Encoder.UseExtended"/>
        /// is false, an <see cref="System.ArgumentException"/> will be thrown.</remarks>
        /// <exception cref="System.ArgumentException">The data to be encoded contains values above 127,
        /// or contains extended characters and <see cref="Barcodes.Code39Encoder.UseExtended"/> is false.
        /// The <see cref="System.ArgumentException.ParamName"/>
        /// property will contain the values "Invalid" or "Extended", respectively.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                byte[] encoded;
                CheckAndEncodeData(value, out encoded);
                data = (byte[])value.Clone();
                GeneratorInstance.Data = encoded;
                text = null;
            }
        }

        /// <summary>
        /// Gets or sets the data to be encoded by a barcode, as a text string.
        /// </summary>
        /// <value>The text to be encoded.</value>
        /// <remarks>Code 39 can encode the entire lower ASCII set (0-127), but it will fail
        /// if <see cref="Barcodes.Code39Encoder.UseExtended"/> is false and a character other than
        /// (0-9, A-Z, and "-. $/+%"), is encountered.</remarks>
        /// <exception cref="System.ArgumentException">The string to be encoded contains values outside the lower ASCII set,
        /// or contains extended characters and <see cref="Barcodes.Code39Encoder.UseExtended"/> is false.
        /// The <see cref="System.ArgumentException.ParamName"/>
        /// property will contain the values "Invalid" or "Extended", respectively.</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                SetTextPropertyValue(
                    value, 
                    out text, 
                    ref data,
                    (string str, out byte[] encoded, ref byte[] data1) =>
                        CheckAndEncodeText(str, out encoded, ref data1));
                ((Code39Generator)Generator).Text = value;
            }
        }

        /// <summary>
        /// Returns the list of standard (non-extended) Code 39 characters.
        /// </summary>
        public override string TextSymbols {
            get {
                return "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
            }
        }

        /// <summary>
        /// Gets flags that represent the encoder's capabilities.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.ASCII"/>|<see cref="Barcodes.BarcodeEncoderFlags.Data"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.ASCII | BarcodeEncoderFlags.Data; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code39Encoder"/> class.
        /// </summary>
        public Code39Encoder() : base(new Code39Generator()) { }
    }

    /// <summary>
    /// An encoder that generates Code 93 barcodes.
    /// </summary>
    /// <remarks><para>Code 93 is a continuous variant of Code 39.  It's standard form
    /// can encode the symbols 0-9, A-Z, as well as dash (-), dot (.), space ( ), dollar ($),
    /// slash (/), plus (+), and percent (%), as with Code 39. It also has an extended form, which is capable of encoding
    /// all 128 of the lower ASCII symbols, but unlike Code 39, the extended form is easily integrated into
    /// barcodes and does not require a separate property to be set.</para><para>Code 93 barcodes 
    /// require two checksum digits; the encoder will automatically add two mod-47 based checksum digits.</para></remarks>
    /// <seealso cref="Barcodes.Code93Generator"/>
    /// <seealso cref="Barcodes.Code93Sizer"/>
    public class Code93Encoder : BarcodeEncoder {
        private byte[] data;
        private string text;

        #region Encoding Table
        // {Ext1, Ext2}
        byte[,] encodingTable = 
            {
                {46, 30}, //0  - NUL
                {43, 10}, //1  - SOH
                {43, 11}, //2  - STX
                {43, 12}, //3  - ETX
                {43, 13}, //4  - EOT
                {43, 14}, //5  - ENQ
                {43, 15}, //6  - ACK
                {43, 16}, //7  - BEL
                {43, 17}, //8  - BS
                {43, 18}, //9  - HT
                {43, 19}, //10 - LF
                {43, 20}, //11 - VT
                {43, 21}, //12 - FF
                {43, 22}, //13 - CR
                {43, 23}, //14 - SO
                {43, 24}, //15 - SI
                {43, 25}, //16 - DLE
                {43, 26}, //17 - DC1
                {43, 27}, //18 - DC2
                {43, 28}, //19 - DC3
                {43, 29}, //20 - DC4
                {43, 30}, //21 - NAK
                {43, 31}, //22 - SYN
                {43, 32}, //23 - ETB
                {43, 33}, //24 - CAN
                {43, 34}, //25 - EM
                {43, 35}, //26 - SUB
                {46, 10}, //27 - ESC
                {46, 11}, //28 - FS
                {46, 12}, //29 - GS
                {46, 13}, //30 - RS
                {46, 14}, //31 - YS
                {38, 255}, //Space
                {44, 10},   //!
                {44, 11},   //"
                {44, 12},   //#
                {39, 255},  //$
                {42, 255},  //%
                {44, 15},   //&
                {44, 16},   //'
                {44, 17},   //( - 40
                {44, 18},   //)
                {44, 19},   //*
                {41, 255},  //+
                {44, 21},   //,
                {36, 255}, //-
                {37, 255}, //.
                {40, 255},  // /
                {0, 255},   //0
                {1, 255},
                {2, 255},   //2 - 50
                {3, 255},
                {4, 255},
                {5, 255},
                {6, 255},
                {7, 255},
                {8, 255},
                {9, 255},
                {44, 35},   //:
                {46, 15},   //;
                {46, 16},   //< - 60
                {46, 17},   //=
                {46, 18},   //>
                {46, 19},   //?
                {46, 31},   //@
                {10, 255}, //A
                {11, 255}, //B
                {12, 255}, //C
                {13, 255}, //D
                {14, 255}, //E
                {15, 255}, //F - 70
                {16, 255}, //G
                {17, 255}, //H
                {18, 255}, //I
                {19, 255}, //J
                {20, 255}, //K
                {21, 255}, //L
                {22, 255}, //M
                {23, 255}, //N
                {24, 255}, //O
                {25, 255}, //P - 80
                {26, 255}, //Q
                {27, 255}, //R
                {28, 255}, //S
                {29, 255}, //T
                {30, 255}, //U
                {31, 255}, //V
                {32, 255}, //W
                {33, 255}, //X
                {34, 255}, //Y
                {35, 255}, //Z - 90
                {46, 20},   //[
                {46, 21},   //\
                {46, 22},   //]
                {46, 23},   //^
                {46, 24},   //_
                {46, 32},   //`
                {45, 10},   //a
                {45, 11},   //b
                {45, 12},   //c
                {45, 13},   //d - 100
                {45, 14},   //e
                {45, 15},   //f
                {45, 16},   //g
                {45, 17},   //h
                {45, 18},   //i
                {45, 19},   //j
                {45, 20},   //k
                {45, 21},   //l
                {45, 22},   //m
                {45, 23},   //n - 110
                {45, 24},   //o
                {45, 25},   //p
                {45, 26},   //q
                {45, 27},   //r
                {45, 28},   //s
                {45, 29},   //t
                {45, 30},   //u
                {45, 31},   //v
                {45, 32},   //w
                {45, 33},   //x - 120
                {45, 34},   //y
                {45, 35},   //z
                {46, 25},   //{
                {46, 26},   //|
                {46, 27},   //}
                {46, 28},   //~
                {46, 29}    //DEL
            };
        #endregion

        private void CheckAndEncodeData(byte[] value, out byte[] encoded) {
            encoded = null;
            if (value == null)
                return;

            //Check the data and determine if we need to use the extended encoding.
            foreach (byte b in value) {
                if (b > 127)
                    throw new ArgumentException("The specified data contains invalid symbols.", "Invalid");
            }

            List<byte> bytes = new List<byte>();


            foreach (byte b in value) {
                bytes.Add(encodingTable[b, 0]);
                if (encodingTable[b, 1] != 255)
                    bytes.Add(encodingTable[b, 1]);
            }

            bytes.Add(0);
            bytes.Add(0); //These are the placeholders for the two check digits.
            encoded = bytes.ToArray();

            encoded[encoded.Length - 2] = CalculateDigit(encoded, encoded.Length - 2, 20);
            encoded[encoded.Length - 1] = CalculateDigit(encoded, encoded.Length - 1, 15);
        }

        private byte CalculateDigit(byte[] encoded, int p, int wrap) {
            int checkSum = 0;
            int weight = 1;
            int i;
            for (i = p - 1; i >= 0; i--) {
                checkSum += weight * encoded[i];
                weight++;
                if (weight > wrap)
                    weight = 1;
            }
            return (byte)(checkSum % 47);
        }

        private void CheckAndEncodeText(string value, out byte[] encoded, ref byte[] data) {
            encoded = null;
            if (value == null)
                return;
            byte[] ascii = Encoding.UTF8.GetBytes(value);

            //CheckAndEncodeData will make sure that the ASCII is valid.
            CheckAndEncodeData(ascii, out encoded);
            data = ascii;
        }

        /// <summary>
        /// Gets or sets the data to be encoded into a barcode.
        /// </summary>
        /// <value>A byte array of data to be encoded.</value>
        /// <remarks>The byte values in the byte array are encoded as their corresponding ASCII 
        /// characters. If any byte contains a value above 127, an <see cref="System.ArgumentException"/> 
        /// will be thrown.</remarks>
        /// <exception cref="System.ArgumentException">The data to be encoded contains values above 127.
        /// The <see cref="System.ArgumentException.ParamName"/>
        /// property will contain the value "Invalid".</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                byte[] encoded;
                CheckAndEncodeData(value, out encoded);
                data = (byte[])value.Clone();
                GeneratorInstance.Data = encoded;
                text = null;
            }
        }

        /// <summary>
        /// Gets or sets the data to be encoded by a barcode, as a text string.
        /// </summary>
        /// <value>The text to be encoded.</value>
        /// <remarks>Code 39 can encode the entire lower ASCII set (0-127).</remarks>
        /// <exception cref="System.ArgumentException">The data to be encoded contains values outside the lower ASCII set.
        /// The <see cref="System.ArgumentException.ParamName"/>
        /// property will contain the value "Invalid".</exception>
        public override string Text {
            get {
                return text;
            }
            set
            {
                SetTextPropertyValue(
                    value, 
                    out text, 
                    ref data,
                    (string str, out byte[] encoded, ref byte[] data1) =>
                        CheckAndEncodeText(str, out encoded, ref data1));
                ((Code93Generator)Generator).Text = value;
            }
        }

        /// <summary>
        /// Gets flags that represent the encoder's capabilities.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.ASCII"/>|<see cref="Barcodes.BarcodeEncoderFlags.Data"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.ASCII | BarcodeEncoderFlags.Data; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code93Encoder"/> class.
        /// </summary>
        public Code93Encoder() : base(new Code93Generator()) { }
    }

    /// <summary>
    /// These Code128 encoding modes control the codeset used to encode data.
    /// </summary>
    /// <remarks>The encoding mode must be passed as a parameter to the <see cref="Barcodes.Code128Encoder(Code128EncodingMode)"/> constructor.
    /// It cannot be set after the class has been initialized.</remarks>
    public enum Code128EncodingMode {
        /// <summary>
        /// The encoder will use all three encoding sets, switching between the three to produce the shortest possible barcode.
        /// This is the default mode and should be used unless there is a reason to only encode in one encoding.
        /// </summary>
        Auto,
        /// <summary>
        /// The encoder will only use the A encoding set. This set can encode any valid ASCII data, but may result in a
        /// longer barcode than using Auto.
        /// </summary>
        A,
        /// <summary>
        /// The encoder will only use the B encoding set. This set can encode any valid ASCII data, but may result in a
        /// longer barcode than using Auto.
        /// </summary>
        B,
        /// <summary>
        /// The encoder will only use the C encoding set. This set can only encode strings composed of an even number of
        /// digits, as well as the FNC1 function code.  Passing any other data will result in an exception when encoding.
        /// </summary>
        C
    }

    /// <summary>
    /// An encoder to encode data into Code128.
    /// </summary>
    /// <remarks>Code128 is a widely used barcode symbology, capable of encoding the entire lower ASCII set (0-127),
    /// as well as four control characters (FNC1-FNC4).  Code128 has three separate encoding styles (A, B, and C),
    /// and can switch between them in the same barcode.  Set A encodes ASCII characters 0-95, SetB encodes ASCII
    /// characters 32-127, and set C encodes all pairs of digits from 00 to 99.  Code128 is the basis for 
    /// EAN-128, and is used by <see cref="Barcodes.EAN128Encoder"/>, as well as HIBC and other topologies.</remarks>
    public class Code128Encoder : BarcodeEncoder {
        private class EncodingNode {
            public byte State;
            public EncodingNode Previous;
            public byte Code;
            public int Cost;
            public bool Shift;

            public EncodingNode(byte state, byte code, EncodingNode previous, bool shift, int cost) {
                State = state;
                Previous = previous;
                Code = code;
                Cost = cost;
                Shift = shift;
            }
        }

        private Code128EncodingMode encodingMode;

        /// <summary>
        /// Gets the encoding mode that the encoder is using.
        /// </summary>
        /// <value>The encoding mode that the encoder is using.</value>
        /// <remarks>The encoding mode can only be set by passing it to the <see cref="Barcodes.Code128Encoder(Code128EncodingMode)"/> constructor.</remarks>
        public Code128EncodingMode EncodingMode {
            get { return encodingMode; }
        }

        private void Compare(ref EncodingNode node, byte state, int code, EncodingNode previous, bool shift, int cost) {
            if (node == null || node.Cost > cost)
                node = new EncodingNode(state, (byte)code, previous, shift, cost);
        }

        private int[,] costDelta ={ { 1, 3, 1, 2, 2, 2 }, { 2, 2, 2, 1, 3, 1 }, { 2, 3, 2, 2, 3, 2 } };

        private void ProcessNode(EncodingNode[,] nodes, int i, int j, byte currentByte, byte nextByte) {
            //i = column;
            //j = State (A->0, B->1, C->2);
            //currentByte - The next byte to be encoded.
            //nextByte - The next byte after that.

            EncodingNode node = nodes[i, j];
            if (node == null)
                return;
            int cost = nodes[i, j].Cost;

            bool useA, useB, useC;
            if (encodingMode != Code128EncodingMode.Auto) {
                useA = encodingMode == Code128EncodingMode.A;
                useB = encodingMode == Code128EncodingMode.B;
                useC = encodingMode == Code128EncodingMode.C;
            } else
                useA = useB = useC = true;

            if (currentByte <= 31) {
                if (useA)
                    Compare(ref nodes[i + 1, 0], 0, 64 + currentByte, node, false, cost + costDelta[j, 0]);
                if (useB)
                    Compare(ref nodes[i + 1, 1], 1, 64 + currentByte, node, true, cost + costDelta[j, 1]);
            } else if (currentByte <= 95) {
                if (useA)
                    Compare(ref nodes[i + 1, 0], 0, currentByte - 32, node, false, cost + costDelta[j, 2]);
                if (useB)
                    Compare(ref nodes[i + 1, 1], 1, currentByte - 32, node, false, cost + costDelta[j, 3]);
            } else if (currentByte <= 127) {
                if (useA)
                    Compare(ref nodes[i + 1, 0], 0, currentByte - 32, node, true, cost + costDelta[j, 4]);
                if (useB)
                    Compare(ref nodes[i + 1, 1], 1, currentByte - 32, node, false, cost + costDelta[j, 5]);
            } else if (currentByte <= 131) {
                switch (currentByte) {
                    case 128:
                        if (useA)
                            Compare(ref nodes[i + 1, 0], 0, 102, node, false, cost + ((j == 0) ? 1 : 2));
                        if (useB)
                            Compare(ref nodes[i + 1, 1], 1, 102, node, false, cost + ((j == 1) ? 1 : 2));
                        if (useC)
                            Compare(ref nodes[i + 1, 2], 2, 102, node, false, cost + ((j == 2) ? 1 : 2));
                        break;
                    case 129:
                        if (useA)
                            Compare(ref nodes[i + 1, 0], 0, 97, node, false, cost + ((j == 0) ? 1 : 2));
                        if (useB)
                            Compare(ref nodes[i + 1, 1], 1, 97, node, false, cost + ((j == 1) ? 1 : 2));
                        break;
                    case 130:
                        if (useA)
                            Compare(ref nodes[i + 1, 0], 0, 96, node, false, cost + ((j == 0) ? 1 : 2));
                        if (useB)
                            Compare(ref nodes[i + 1, 1], 1, 96, node, false, cost + ((j == 1) ? 1 : 2));
                        break;
                    case 131:
                        if (useA)
                            Compare(ref nodes[i + 1, 0], 0, 101, node, false, cost + ((j == 0) ? 1 : 2));
                        if (useB)
                            Compare(ref nodes[i + 1, 1], 1, 100, node, false, cost + ((j == 1) ? 1 : 2));
                        break;
                }
            }

            if (currentByte >= 48 && currentByte <= 57) {
                //It's a digit, we might have a C node.
                if (nextByte >= 48 && nextByte <= 57) {
                    //There are two digits, make a C node.
                    byte code = (byte)(10 * (currentByte - 48) + nextByte - 48);
                    if (useC)
                        Compare(ref nodes[i + 2, 2], 2, code, node, false, cost + ((j == 2) ? 1 : 2));
                }
            }
        }

        private void CheckAndEncodeData(byte[] value, out byte[] data) {
            data = null;

            foreach (byte b in value) {
                if (b > 131)
                    throw new ArgumentException("The byte array to be encoded contains invalid codes.", "Invalid");
            }

            EncodingNode[,] nodes = new EncodingNode[value.Length + 1, 3];
            //Initialize the first node(s).
            byte currentByte = value[0], nextByte;
            nodes[0, 0] = new EncodingNode(0, 103, null, false, 1);
            nodes[0, 1] = new EncodingNode(1, 104, null, false, 1);
            nodes[0, 2] = new EncodingNode(2, 105, null, false, 1);

            //Ok, we've initialize the table.  Now, run through the rest of the byte string
            //And find the shortest path.
            int i = 0, len = value.Length;
            for (i = 0; i < len; i++) {
                currentByte = value[i];
                if (i < (len - 1))
                    nextByte = value[i + 1];
                else
                    nextByte = 255;

                ProcessNode(nodes, i, 0, currentByte, nextByte);
                ProcessNode(nodes, i, 1, currentByte, nextByte);
                ProcessNode(nodes, i, 2, currentByte, nextByte);
            }

            //Ok, now the shortest path is in there somewhere.
            EncodingNode smallestNode = null;
            for (i = 0; i < 3; i++) {
                if (nodes[value.Length, i] == null)
                    continue;
                if (smallestNode == null || smallestNode.Cost > nodes[value.Length, i].Cost)
                    smallestNode = nodes[value.Length, i];
            }

            if (smallestNode == null) //It was unencodable.
                throw new ArgumentException("The specified byte array was unencodable given the current encoding method.", "Method");

            List<byte> byteCodes = new List<byte>();
            while (smallestNode != null) {
                byteCodes.Add(smallestNode.Code);
                if (smallestNode.Shift)
                    byteCodes.Add(98); //Shift code.
                if (smallestNode.Previous != null && smallestNode.State != smallestNode.Previous.State)
                    byteCodes.Add((byte)(101 - smallestNode.State)); //Transition
                smallestNode = smallestNode.Previous;
            }

            byteCodes.Reverse();
            byteCodes.Add(0);//This is a placeholder for the checksum.
            byteCodes.Add(106);
            data = byteCodes.ToArray();
            data[data.Length - 2] = CalculateChecksum(data, data.Length - 2);
        }

        private byte CalculateChecksum(byte[] data, int p) {
            int checkSum, weight = 1;
            checkSum = data[0];
            for (int i = 1; i < p; i++) {
                checkSum += data[i] * weight;
                weight++;
            }
            return (byte)(checkSum % 103);
        }

        private bool useFunctionEscapes=false;

        /// <summary>
        /// Gets or sets whether the text encoder recognizes function escape sequences.
        /// </summary>
        /// <value>True if the encoder will recognize function escapes, false otherwise.</value>
        /// <remarks>For information about function escape sequences, see <see cref="Barcodes.Code128Encoder.Text"/>.
        /// Note that this property must be set <i>before</i> setting <see cref="Barcodes.Code128Encoder.Text"/>.  If the value
        /// is changed after <see cref="Barcodes.Code128Encoder.Text"/> is set, the data will not be reencoded with
        /// the new value.</remarks>
        /// <seealso cref="Barcodes.Code128Encoder.Text"/>
        public bool UseFunctionEscapes {
            get { return useFunctionEscapes; }
            set { useFunctionEscapes = value; }
        }

        private void CheckAndEncodeText(string value, out byte[] encoded, ref byte[] data) {
            encoded = null;
            if (value == null)
                return;

            List<int> escapeData=null;
            //Get function encodings.
            if (useFunctionEscapes) {
                escapeData = new List<int>();
                string temp = "";
                int pos = 0,i;
                for (i = 0; i < value.Length; i++) {
                    if (value[i] != '%') {
                        temp += value[i];
                        pos++;
                        continue;
                    }

                    if (i == (value.Length - 1)) {
                        //It's the last character, so emit it.
                        temp += '%';
                        pos++;
                        continue;
                    }

                    switch (value[i + 1]) {
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                            temp += 'X'; //This is a placeholder.
                            escapeData.Add(pos);
                            escapeData.Add("1234".IndexOf(value[i + 1]) + 1);
                            pos++;
                            i++;
                            continue;
                        case '%':
                            temp += '%';
                            pos++;
                            i++;
                            continue;
                        default:
                            temp += '%';
                            pos++;
                            break;
                    }
                }

                value = temp;
            }

            //Convert to ASCII bytes.
            byte[] ascii = Encoding.UTF8.GetBytes(value);
            foreach (byte b in ascii)
                if (b > 127)
                    throw new ArgumentException("The text string specified contains non-ASCII or high-ASCII characters.", "Invalid");

            //Reintegrate function codes.
            if (useFunctionEscapes) {
                for (int i = 0; i < escapeData.Count; i += 2) {
                    ascii[escapeData[i]] = (byte)(127 + escapeData[i + 1]);
                }
            }

            //CheckAndEncodeData will make sure that the ASCII is valid.
            CheckAndEncodeData(ascii, out encoded);
        }

        private byte[] data;
        private string text;

        /// <summary>
        /// Gets or sets the data to be encoded into a barcode.
        /// </summary>
        /// <value>A byte array of data to be encoded.</value>
        /// <remarks>The byte values in the byte array are encoded as their corresponding ASCII characters.
        /// The function codes FNC1-FNC4 are mapped to 128-131 respectively. If any byte contains a value
        /// above 131, an <see cref="System.ArgumentException"/> is thrown.  Likewise, if the byte array is unencodable
        /// given the coding method (<see cref="Barcodes.Code128EncodingMode"/>) specified in the constructor.</remarks>
        /// <exception cref="System.ArgumentException">The data to be encoded contains values above 131,
        /// or the data cannot be encoded given the current encoding method.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will contain the values "Invalid" or "Method", respectively.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                byte[] encoded;
                CheckAndEncodeData(value, out encoded);
                data = (byte[])value.Clone();
                GeneratorInstance.Data = encoded;
                text = null;
            }
        }

        /// <summary>
        /// Gets or sets the text to be encoded into a barcode.
        /// </summary>
        /// <value>A string of the data to be encoded.</value>
        /// <remarks>While the text string encoding method does not natively support encoding function codes, however,
        /// if the <see cref="Barcodes.Code128Encoder.UseFunctionEscapes"/> property is set to true, the encoder
        /// will interpret the following character escape sequences, as listed:
        /// <para>
        /// <list type="table">
        /// <listheader><term>Sequence</term><description>Interpretation</description></listheader>
        /// <item><term>%1</term><description>FNC1</description></item>
        /// <item><term>%2</term><description>FNC1</description></item>
        /// <item><term>%3</term><description>FNC1</description></item>
        /// <item><term>%4</term><description>FNC1</description></item>
        /// <item><term>%%</term><description>%</description></item>
        /// </list>
        /// </para><para>Note that the encoder will only recognize these sequences. Any other sequence
        /// involving percent signs will be passed through as-is.</para></remarks>
        /// <exception cref="System.ArgumentException">The text contains non-ASCII or high-ASCII (above 127) characters,
        /// or the data cannot be encoded given the current encoding method.  The <see cref="System.ArgumentException.ParamName"/>
        /// property will contain the values "Invalid" or "Method", respectively.</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                byte[] encoded;
                CheckAndEncodeText(value, out encoded, ref this.data);
                text = value;
                GeneratorInstance.Data = encoded;
                ((Code128Generator)Generator).Text = value;
            }
        }

        /// <summary>
        /// Gets flags that represent the encoder's capabilities.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.ASCII"/>|<see cref="Barcodes.BarcodeEncoderFlags.Data"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get { return BarcodeEncoderFlags.ASCII | BarcodeEncoderFlags.Data; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code128Encoder"/> class, with the encoding mode set to <see cref="Barcodes.Code128EncodingMode.Auto"/>.
        /// </summary>
        public Code128Encoder()
            : base(new Code128Generator()) {
            encodingMode = Code128EncodingMode.Auto;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code128Encoder"/> class, with the specified encoding mode.
        /// </summary>
        /// <param name="mode">The encoding mode to use.</param>
        public Code128Encoder(Code128EncodingMode mode)
            : base(new Code128Generator()) {
            encodingMode = mode;
        }
    }

    /// <summary>
    /// Abstract class that implements the common functionality of the Code N sizers.
    /// </summary>
    /// <seealso cref="Barcodes.Code128Encoder"/>
    /// <seealso cref="Barcodes.Code128Generator"/>
    public abstract class CodeNSizer : BarcodeSizer, IBarcodeModularSizer {
        private int minimumHeight25 = 1;
        /// <summary>
        /// Returns the number of pixels in 1/4th of an inch (6.35 mm), given the current DPI.
        /// </summary>
        /// <remarks>This value is used as a minimum height by most of the Code N barcodes.</remarks>
        public int MinimumHeight25 { get { return minimumHeight25; } set { minimumHeight25 = value; } }

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
        
        /// <summary>
        /// Method called to calculate the various sizes when DPI or Module Width is changed..
        /// </summary>
        /// <remarks>This mathod is called when <see cref="Barcodes.CodeNSizer.DPI"/> or <see cref="Barcodes.CodeNSizer.Module"/> is changed,
        /// so that the derived class can set the <see cref="Barcodes.CodeNSizer.ModuleWidth"/>, <see cref="Barcodes.CodeNSizer.GuardWidth"/>,
        /// and <see cref="Barcodes.CodeNSizer.MinimumHeight25"/> properties, as well as any other properties they need
        /// to set on their own.</remarks>
        protected abstract void CalculateSizes();

        private int length=0;
        /// <summary>
        /// The length of the data being encoded.
        /// </summary>
        /// <remarks>This property is provided for derived classes to recieve information about the encoded
        /// data, from its corresponding generator.  The property is not otherwise used by <see cref="Barcodes.CodeNSizer"/>,
        /// and its usage is left to derived classes and their corresponding generators.</remarks>
        public int Length {
            get { return length; }
            set { length = value; }
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
        /// <remarks>This method is overridden in derived classes, and must return the total width of the
        /// barcode data, given the current value of <see cref="Barcodes.CodeNSizer.ModuleWidth"/>. The
        /// <see cref="Barcodes.CodeNSizer.Length"/> property is included so that the derived class can 
        /// recieve information from its corresponding generator.</remarks>
        public abstract int WidthWithoutGuards { get;}


        /// <summary>
        /// The total width of the barcode, in pixels, given the current DPI and module width.
        /// </summary>
        public override int Width {
            get { return WidthWithoutGuards + (((Mode & BarcodeRenderMode.Guarded) != 0) ? 2 * guardWidth : 0); }
        }

        /// <summary>
        /// The minimum height of the barcode, in pixels.
        /// </summary>
        public override int Height {
            get { return minimumHeight25+FontHeightAddon; }
        }

        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>The height of all items added to the barcode, in pixels.</value>
        /// <remarks><para>For Code N barcodes, the only extra height comes from the text at the bottom of the barcode.</para>
        /// <para>This property is meant to be used to generate a desired barcode height.  To calculate the height
        /// of the size to pass to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, multiply the desired height
        /// (in inches) by the DPI, and add this number to it.  It is also important in aspect ratio calculations, as this
        /// value will be subtracted from the size passed to the generator, before checking the aspect ratio.</para>
        /// <para>This property's value is potentially impacted by the value of <see cref="Barcodes.IBarcodeSizer.Mode"/>.
        /// The barcode render mode flags should be set before using this value.</para></remarks>
        public override int ExtraHeight {
            get { return FontHeightAddon; }
        }

        private float module;
        /// <summary>
        /// Gets or sets the desired module width, in mils.
        /// </summary>
        /// <value>The current desired module width, in mils (1 mil is 1/1000th of an inch, .0254 mm).  The default value is 10 mils (.33 mm).</value>
        /// <remarks>The barcode generator will generate a barcode with a module width that is as close as
        /// possible to, but not smaller than, the specified module width, within the limits of the 
        /// specified DPI.  It cannot be set lower than the value returned by the <see cref="Barcodes.CodeNSizer.MinimumModule"/>
        /// implementation of the corresponding derived class.</remarks>
        /// <exception cref="System.ArgumentException">The specified module width is smaller than 
        /// <see cref="Barcodes.CodeNSizer.MinimumModule"/>.</exception>
        public float Module {
            get {
                return module;
            }
            set {
                if (value < MinimumModule)
                    throw new ArgumentException("The specified module width is smaller than the minimum width.");
                module = value;
                CalculateSizes();
            }
        }

        /// <summary>
        /// Gets the minimum module width, in mils.
        /// </summary>
        /// <value>The minimum module width, in mils. (1 mil is 1/1000th of an inch, or .025mm)</value>
        public abstract float MinimumModule { get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.CodeNSizer"/> class.
        /// </summary>
        /// <param name="defaultModule">The default module width.  This should be greater than the
        /// value returned by the derived class's <see cref="Barcodes.CodeNSizer.MinimumModule"/>
        /// property, but this is not checked.</param>
        protected CodeNSizer(float defaultModule) {
            module = defaultModule;
        }

        /// <summary>
        /// Checks to see if a specified size is valid.
        /// </summary>
        /// <param name="size">A size to test for validity.</param>
        /// <returns>True if this size may be passed to <see cref="Barcodes.CodeNGenerator.GenerateBarcode"/>, false otherwise.</returns>
        public override bool IsValidSize(Size size) {
            return (size.Width == Width && size.Height >= size.Height);
        }

        /// <summary>
        /// Given a size, returns the largest valid size contained by that size.
        /// </summary>
        /// <param name="size">A maximum size, from which to find a valid size.</param>
        /// <returns>A valid size which may be passed to <see cref="Barcodes.CodeNGenerator.GenerateBarcode"/>.</returns>
        /// <exception cref="System.ArgumentException">The specified size is smaller than the minimum size in one or both dimensions.</exception>
        public override Size GetValidSize(Size size) {
            if (size.Width < Width || size.Height < Height)
                throw new ArgumentException("The specified size is smaller than the minumum size.");
            return new Size(Width, size.Height);
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
    }

    /// <summary>
    /// A class that controls the sizing of Code 11 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.Code11Encoder"/>
    /// <seealso cref="Barcodes.Code11Generator"/>
    public class Code11Sizer : CodeNSizer {
        /// <summary>
        /// Calculates the barcode sizes when the DPI or Module width is changed.
        /// </summary>
        protected override void CalculateSizes() {
            if (DPI == 0) {
                MinimumHeight25 = 1;
                ModuleWidth = 1;
                GuardWidth = 10;
            } else {
                float tenthMilsPerDot = 10000 / DPI;
                ModuleWidth = (int)Math.Ceiling(Module * 10 / tenthMilsPerDot);
                MinimumHeight25 = (int)Math.Ceiling(2500 / tenthMilsPerDot);
                GuardWidth = 10 * ModuleWidth;
            }
        }

        /// <summary>
        /// The total width of the barcode, excluding any guard (quiet) zones, in pixels.
        /// </summary>
        public override int WidthWithoutGuards {
            get { return (Length * 8 - 1) * ModuleWidth; }
        }

        /// <summary>
        /// The minimum height of the barcode, in pixels.
        /// </summary>
        public override int Height {
            get { return FontHeightAddon + Math.Max(base.Height, (int)Math.Ceiling(.15f * Width)); }
        }

        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>The height of all items added to the barcode, in pixels.</value>
        /// <remarks><para>For Code 11 barcodes, the only extra height comes from the text at the bottom of the barcode.</para>
        /// <para>This property is meant to be used to generate a desired barcode height.  To calculate the height
        /// of the size to pass to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, multiply the desired height
        /// (in inches) by the DPI, and add this number to it.  It is also important in aspect ratio calculations, as this
        /// value will be subtracted from the size passed to the generator, before checking the aspect ratio.</para>
        /// <para>This property's value is potentially impacted by the value of <see cref="Barcodes.IBarcodeSizer.Mode"/>.
        /// The barcode render mode flags should be set before using this value.</para></remarks>
        public override int ExtraHeight {
            get { return FontHeightAddon; }
        }

        /// <summary>
        /// Gets the minimum aspect ratio (height/width) of the barcode, not including the numbering.
        /// </summary>
        /// <value>This always returns a value of 0.15 (3/20).</value>
        public override float AspectRatioMin {
            get {
                return 0.15f;
            }
        }

        /// <summary>
        /// Gets the minimum module width, in mils.
        /// </summary>
        /// <value>The minimum module width, in mils. (1 mil is 1/1000th of an inch, or .025mm). Always returns 7.5 mils (.19 mm).</value>
        public override float MinimumModule {
            get { return 7.5f; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code11Sizer"/> class.
        /// </summary>
        public Code11Sizer() : base(10f) { }
    }

    /// <summary>
    /// A class that controls the sizing of Code 39 barcodes.
    /// </summary>
    /// <remarks>This sizer is slightly different from the other Code N sizers, since Code39 uses
    /// two different bar widths, instead of specific modular bar widths.</remarks>
    /// <seealso cref="Barcodes.Code39Encoder"/>
    /// <seealso cref="Barcodes.Code39Generator"/>
    public class Code39Sizer : CodeNSizer {
        private int narrowModuleWidth = 1;
        /// <summary>
        /// Gets the width of narrow bars, in pixels.
        /// </summary>
        public int NarrowModuleWidth { get { return narrowModuleWidth; } }

        private int wideModuleWidth = 3;
        /// <summary>
        /// Gets the width of wide bars, in pixels.
        /// </summary>
        public int WideModuleWidth { get { return wideModuleWidth; } }
        
        /// <summary>
        /// Gets the width of spaces, in pixels.
        /// </summary>
        public int GapWidth { get { return narrowModuleWidth; } }

        /// <summary>
        /// Calculates the sizes used by the Code 39 generator, when the DPI or Module Width is changed.
        /// </summary>
        protected override void CalculateSizes() {
            if (DPI != 0) {
                float tenthMilsPerDPI = 10000f / DPI;

                MinimumHeight25 = (int)Math.Ceiling(2500 / tenthMilsPerDPI);
                narrowModuleWidth = (int)Math.Ceiling(Module*10 / tenthMilsPerDPI);
                float actualModuleWidth = narrowModuleWidth * tenthMilsPerDPI;
                if (actualModuleWidth < 200f)
                    wideModuleWidth = Math.Min(narrowModuleWidth*2,(int)Math.Ceiling(21 * Module / tenthMilsPerDPI));
                else
                    wideModuleWidth = Math.Min(narrowModuleWidth*3,(int)Math.Ceiling(28 * Module / tenthMilsPerDPI));
                GuardWidth = Math.Max((int)Math.Ceiling(1000 / tenthMilsPerDPI), narrowModuleWidth * 10);
            } else {
                narrowModuleWidth = 1;
                wideModuleWidth = 3;
                MinimumHeight25 = 1;
                GuardWidth = 10;
            }
        }

        /// <summary>
        /// The total width of the barcode, excluding any guard (quiet) zones, in pixels.
        /// </summary>
        public override int WidthWithoutGuards {
            get { return (narrowModuleWidth * 6 + wideModuleWidth * 3 + GapWidth) * Length - GapWidth; }
        }

        /// <summary>
        /// The minimum height of the barcode, in pixels.
        /// </summary>
        public override int Height {
            get {
                if (DPI != 0)
                    return FontHeightAddon+Math.Max(base.MinimumHeight25, (int)Math.Ceiling(WidthWithoutGuards * .15f));
                else
                    return FontHeightAddon+MinimumHeight25;
            }
        }

        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>The height of all items added to the barcode, in pixels.</value>
        /// <remarks><para>For Code 39 barcodes, the only extra height comes from the text at the bottom of the barcode.</para>
        /// <para>This property is meant to be used to generate a desired barcode height.  To calculate the height
        /// of the size to pass to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, multiply the desired height
        /// (in inches) by the DPI, and add this number to it.  It is also important in aspect ratio calculations, as this
        /// value will be subtracted from the size passed to the generator, before checking the aspect ratio.</para>
        /// <para>This property's value is potentially impacted by the value of <see cref="Barcodes.IBarcodeSizer.Mode"/>.
        /// The barcode render mode flags should be set before using this value.</para></remarks>
        public override int ExtraHeight {
            get { return FontHeightAddon; }
        }

        /// <summary>
        /// Gets the minimum aspect ratio (height/width) of the barcode, not including the numbering.
        /// </summary>
        /// <value>This always returns a value of 0.15 (3/20).</value>
        public override float AspectRatioMin {
            get { return .15f; }
        }

        /// <summary>
        /// Gets the minimum module width, in mils.
        /// </summary>
        /// <value>The minimum module width, in mils. (1 mil is 1/1000th of an inch, or .025mm). Always returns 6.7 mils (.17 mm).</value>
        public override float MinimumModule {
            get { return 6.7f; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code39Sizer"/> class.
        /// </summary>
        public Code39Sizer() : base(10f) { }
    }

    /// <summary>
    /// A class that controls the sizing of Code 93 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.Code93Encoder"/>
    /// <seealso cref="Barcodes.Code93Generator"/>
    public class Code93Sizer : CodeNSizer {
        /// <summary>
        /// Calculates the barcode sizes when the DPI or Module width is changed.
        /// </summary>
        protected override void CalculateSizes() {
            if (DPI == 0) {
                ModuleWidth = 1;
                MinimumHeight25 = 1;
                GuardWidth = 10;
            } else {
                float tenthMilsPerDot = 10000 / DPI;
                ModuleWidth = (int)Math.Ceiling(Module * 10/ tenthMilsPerDot);
                MinimumHeight25 = (int)Math.Ceiling(2500 / tenthMilsPerDot);
                GuardWidth = 10 * ModuleWidth;
            }
        }

        /// <summary>
        /// The total width of the barcode itself, excluding any guard (quiet) zones, in pixels.
        /// </summary>
        public override int WidthWithoutGuards {
            get { return Length * ModuleWidth; }
        }

        /// <summary>
        /// Gets the minimum module width, in mils.
        /// </summary>
        /// <value>The minimum module width, in mils. (1 mil is 1/1000th of an inch, or .025mm). Always returns 6.7 mils (.17 mm).</value>
        public override float MinimumModule {
            get {
                return 6.7f;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code93Sizer"/> class.
        /// </summary>
        public Code93Sizer() : base(10f) { }
    }

    /// <summary>
    /// A class that controls the sizing of Code 128 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.Code128Encoder"/>
    /// <seealso cref="Barcodes.Code128Generator"/>
    public class Code128Sizer : CodeNSizer {
        /// <summary>
        /// Calculates the barcode sizes when the DPI or Module width is changed.
        /// </summary>
        protected override void CalculateSizes() {
            if (DPI == 0) {
                ModuleWidth = 1;
                GuardWidth = 10;
            } else {
                float tenthMilsPerDot = 10000 / DPI;
                ModuleWidth = (int)Math.Ceiling(Module * 10 / tenthMilsPerDot);
                MinimumHeight25 = (int)Math.Ceiling(2500 / tenthMilsPerDot);
                GuardWidth = 10 * ModuleWidth;
            }
        }

        /// <summary>
        /// The total width of the barcode itself, excluding any guard (quiet) zones, in pixels.
        /// </summary>
        public override int WidthWithoutGuards {
            get { return Length * ModuleWidth; }
        }

        /// <summary>
        /// Gets the minimum module width, in mils.
        /// </summary>
        /// <value>The minimum module width, in mils. (1 mil is 1/1000th of an inch, or .025mm). Always returns 6.7 mils (.17 mm).</value>
        public override float MinimumModule {
            get { return 6.7f; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code128Sizer"/> class.
        /// </summary>
        public Code128Sizer() : base(10f) { }
    }

    /// <summary>
    /// The base class for all Code N generators, implementing common functionality.
    /// </summary>
    public abstract class CodeNGenerator : BarcodeGenerator {
        private byte[] data;
        private BitArray encodedData;

        /// <summary>
        /// Gets or sets the bit array that encodes the barcode pattern.
        /// </summary>
        /// <remarks>The default interpretation of this data 
        /// (used in <see cref="Barcodes.CodeNGenerator.GenerateBarcode"/>) is a true bit corresponds with
        /// a dark bar one module wide, and a false is a space one module wide.</remarks>
        protected BitArray EncodedData { get { return encodedData; } set { encodedData = value; } }

        /// <summary>
        /// Called on derived class to check data sent to the <see cref="Barcodes.CodeNGenerator.Data"/> property.
        /// </summary>
        /// <param name="data">The data to check</param>
        /// <remarks>If the specified data is invalid, the derived class should throw
        /// an <see cref="System.ArgumentException"/> exception.</remarks>
        /// <exception cref="System.ArgumentException">Thrown if the data is invalid.</exception>
        protected abstract void CheckData(byte[] data);

        /// <summary>
        /// Called on a derived class when the derived class should encode its data into
        /// a barcode pattern.
        /// </summary>
        /// <remarks>This is called after the data is checked with <see cref="Barcodes.CodeNGenerator.CheckData"/>.</remarks>
        protected abstract void EncodeData();

        private string text;
        /// <summary>
        /// Gets or sets the text to be written below the barcode.
        /// </summary>
        /// <value>A string to display under the barcode.</value>
        /// <remarks>This is normally set by the barcodes' corresponding generator or encoder,
        /// and should usually not be set directly.  However, if the default human readable
        /// text is not sufficient, it can be changed.  Note that the barcode sizer does not take
        /// the length of this text into account, nor does it support more than one line
        /// of text.  While there is enough room to display the encoded data, the total amount
        /// of text displayable depends on many factors.  The string is only displayed if
        /// the <see cref="Barcodes.BarcodeRenderMode.Numbered"/> flag is set.</remarks>
        public string Text {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Gets or sets the data to be encoded into a barcode pattern.
        /// </summary>
        /// <value>A byte array of the data to </value>
        /// <remarks>This data is first checked by calling the derived class's implementation
        /// of <see cref="Barcodes.CodeNGenerator.CheckData"/>, before it is saved.  If the
        /// data is invalid, an <see cref="System.ArgumentException"/> will be thrown.
        /// This property saves a reference to the underlying array; it does not make a copy.
        /// The behavior if the underlying byte array is changed is undefined.</remarks>
        /// <exception cref="System.ArgumentException">The data specified was invalid.</exception>
        public override byte[] Data {
            get {
                return data;
            }
            set {
                CheckData(value);
                data = value;
                encodedData = null;
                if (data == null)
                    return;
                EncodeData(); //This sets the sizing information needed by Code11Sizer.
            }
        }

        /// <summary>
        /// Generates a barcode of a specified size, using the data that has been set previously by its corresponding
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

            CodeNSizer codeNSizer = (CodeNSizer)Sizer;
            Bitmap bm = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppRgb);
            Graphics g = Graphics.FromImage(bm);
            g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);

            int module, guard;
            module = ((CodeNSizer)Sizer).ModuleWidth;
            guard = ((CodeNSizer)Sizer).GuardWidth;

            int pos = guard;
            for (int i = 0; i < encodedData.Length; i++) {
                if (encodedData[i])
                    g.FillRectangle(Brushes.Black, pos, 0, module, size.Height);
                pos += module;
            }

            if (codeNSizer.FontHeight != 0) {
                g.FillRectangle(Brushes.White, 0, size.Height - codeNSizer.FontHeightAddon, size.Width, codeNSizer.FontHeightAddon);
                Font f = FontHolder.GenerateFont(codeNSizer.FontHeight);
                g.DrawString(text, f, Brushes.Black, Rectangle.FromLTRB(guard, size.Height - codeNSizer.FontHeightAddon, size.Width - guard, size.Height),FontHolder.CenterJustify);
            }

            g.Dispose();
            return bm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.CodeNGenerator"/> class.
        /// </summary>
        /// <param name="sizer">The sizer object used by this generator.</param>
        public CodeNGenerator(IBarcodeSizer sizer) : base(sizer) { }
    }

    /// <summary>
    /// A class that generates Code 11 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.Code11Encoder"/>
    /// <seealso cref="Barcodes.Code11Sizer"/>
    public class Code11Generator : CodeNGenerator {
        /// <summary>
        /// Checks the data sent to the generator, to ensure that it is valid.
        /// </summary>
        /// <param name="data">A byte array of data to encode.</param>
        protected override void CheckData(byte[] data) {
            if (data == null)
                return;
            foreach (byte b in data)
                if (b > 10)
                    throw new ArgumentException("The specified data contains invalid codes.");
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
                return base.Data;
            }
            set {
                base.Data = value;
                if (Data != null) { //Set the text that appears below the barcode, from the data that was supplied.
                    string text = "";
                    foreach (byte b in Data)
                        if (b == 10)
                            text += '-';
                        else
                            text += b.ToString();
                    Text = text;
                }
            }
        }

        #region Encoding Table
        private bool[,] encodings ={
            {true,false,true,false,true,true,true},
            {true,true,false,true,false,true,true},
            {true,false,false,true,false,true,true},
            {true,true,false,false,true,false,true},
            {true,false,true,true,false,true,true},
            {true,true,false,true,true,false,true},
            {true,false,false,true,true,false,true},
            {true,false,true,false,false,true,true},
            {true,true,false,true,false,false,true},
            {true,true,true,false,true,false,true},
            {true,false,true,true,true,false,true},
            {true,false,true,true,false,false,true}
        };
        #endregion

        private void EncodeByte(ref int p, byte b) {
            for (int i = 0; i < 7; i++)
                EncodedData[p++] = encodings[b, i];
            EncodedData[p++] = false;
        }

        /// <summary>
        /// Encodes the barcode data in <see cref="Barcodes.CodeNGenerator.Data"/> into a barcode pattern.
        /// </summary>
        protected override void EncodeData() {
            if (EncodedData != null)
                return;

            EncodedData = new BitArray(8 * (2 + Data.Length));
            int pos = 0;

            EncodeByte(ref pos, 11);
            foreach (byte b in Data)
                EncodeByte(ref pos, b);
            EncodeByte(ref pos, 11);

            ((Code11Sizer)Sizer).Length = Data.Length + 2;
        }

        /// <summary>
        /// Gets the barcode generator capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/>|<see cref="Barcodes.BarcodeGeneratorFlags.VariableLength"/>|<see cref="Barcodes.BarcodeGeneratorFlags.AspectRatio"/>.</value>
        public override BarcodeGeneratorFlags Flags {
            get { return BarcodeGeneratorFlags.Linear | BarcodeGeneratorFlags.VariableLength | BarcodeGeneratorFlags.AspectRatio; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code11Generator"/> class.
        /// </summary>
        public Code11Generator() : base(new Code11Sizer()) { }
    }

    /// <summary>
    /// A class that generates Code 39 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.Code39Encoder"/>
    /// <seealso cref="Barcodes.Code39Sizer"/>
    public class Code39Generator : CodeNGenerator {
        #region Encoding Table
        private bool[,] encoding = {
            {false,false,false,true,true,false,true,false,false},    //0
            {true,false,false,true,false,false,false,false,true},
            {false,false,true,true,false,false,false,false,true},
            {true,false,true,true,false,false,false,false,false},
            {false,false,false,true,true,false,false,false,true},
            {true,false,false,true,true,false,false,false,false},
            {false,false,true,true,true,false,false,false,false},
            {false,false,false,true,false,false,true,false,true},
            {true,false,false,true,false,false,true,false,false},
            {false,false,true,true,false,false,true,false,false},
            {true,false,false,false,false,true,false,false,true},    //10
            {false,false,true,false,false,true,false,false,true},
            {true,false,true,false,false,true,false,false,false},
            {false,false,false,false,true,true,false,false,true},
            {true,false,false,false,true,true,false,false,false},
            {false,false,true,false,true,true,false,false,false},
            {false,false,false,false,false,true,true,false,true},
            {true,false,false,false,false,true,true,false,false},
            {false,false,true,false,false,true,true,false,false},
            {false,false,false,false,true,true,true,false,false},
            {true,false,false,false,false,false,false,true,true},    //20
            {false,false,true,false,false,false,false,true,true},
            {true,false,true,false,false,false,false,true,false},
            {false,false,false,false,true,false,false,true,true},
            {true,false,false,false,true,false,false,true,false},
            {false,false,true,false,true,false,false,true,false},
            {false,false,false,false,false,false,true,true,true},
            {true,false,false,false,false,false,true,true,false},
            {false,false,true,false,false,false,true,true,false},
            {false,false,false,false,true,false,true,true,false},
            {true,true,false,false,false,false,false,false,true},    //30
            {false,true,true,false,false,false,false,false,true},
            {true,true,true,false,false,false,false,false,false},
            {false,true,false,false,true,false,false,false,true},
            {true,true,false,false,true,false,false,false,false},
            {false,true,true,false,true,false,false,false,false},
            {false,true,false,false,false,false,true,false,true},
            {true,true,false,false,false,false,true,false,false},
            {false,true,true,false,false,false,true,false,false},
            {false,true,false,true,false,true,false,false,false},
            {false,true,false,true,false,false,false,true,false},    //40
            {false,true,false,false,false,true,false,true,false},
            {false,false,false,true,false,true,false,true,false},
            {false,true,false,false,true,false,true,false,false}
        };
        #endregion

        private void EncodeByte(ref int pos, byte b) {
            for (int i = 0; i < 9; i++) {
                EncodedData[pos++] = encoding[b, i];
            }
        }

        /// <summary>
        /// Checks the data sent to the generator, to ensure that it is valid.
        /// </summary>
        /// <param name="data">A byte array of data to encode.</param>
        protected override void CheckData(byte[] data) {
            if (data == null)
                return;
            foreach (byte b in data)
                if (b > 42)
                    throw new ArgumentException("The specified data contains invalid codes.");
        }

        /// <summary>
        /// Encodes the barcode data in <see cref="Barcodes.CodeNGenerator.Data"/> into a barcode pattern.
        /// </summary>
        protected override void EncodeData() {
            if (EncodedData != null)
                return;

            EncodedData = new BitArray(9 * (2 + Data.Length));

            int pos=0;
            EncodeByte(ref pos, 43);
            foreach (byte b in Data)
                EncodeByte(ref pos, b);
            EncodeByte(ref pos, 43);

            ((Code39Sizer)Sizer).Length = Data.Length + 2;
        }

        /// <summary>
        /// Gets the barcode generator capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/>|<see cref="Barcodes.BarcodeGeneratorFlags.VariableLength"/>|<see cref="Barcodes.BarcodeGeneratorFlags.AspectRatio"/>.</value>
        public override BarcodeGeneratorFlags Flags {
            get { return BarcodeGeneratorFlags.AspectRatio | BarcodeGeneratorFlags.VariableLength | BarcodeGeneratorFlags.Linear; }
        }

        /// <summary>
        /// Generates a Code 39 barcode of a specified size, using the data that has been set previously by its corresponding
        /// encoder.
        /// </summary>
        /// <param name="size">The size of the barcode to return.</param>
        /// <returns>A bitmap of the barcode, of the specified size.</returns>
        /// <exception cref="System.ArgumentException">The specified size is invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The data that is to be encoded has not been set yet.</exception>
        public override Bitmap GenerateBarcode(Size size) {
            if (Data == null)
                throw new InvalidOperationException("The data for the generator has not been set.");
            if (!Sizer.IsValidSize(size))
                throw new ArgumentException("The specified size is not valid.");
            EncodeData();

            Bitmap bm = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppRgb);
            Graphics g = Graphics.FromImage(bm);
            g.FillRectangle(Brushes.White, 0, 0, size.Width, size.Height);

            int narrow, wide, gap, guard;
            Code39Sizer code39Sizer = ((Code39Sizer)Sizer);
            narrow = code39Sizer.NarrowModuleWidth;
            wide = code39Sizer.WideModuleWidth;
            gap = code39Sizer.GapWidth;
            guard = code39Sizer.GuardWidth;

            int pos = guard;
            int count=0;
            bool color=true;
            for (int i = 0; i < EncodedData.Length; i++) {
                int len=EncodedData[i] ? wide : narrow;
                if (color)
                    g.FillRectangle(Brushes.Black, pos, 0, len , size.Height);
                pos += len;
                count++;
                if (i == EncodedData.Length - 1)
                    break; //This skips the last gap.
                if (count == 9) {
                    pos += gap;
                    count = 0;
                } else
                    color = !color;
            }

            
            if (code39Sizer.FontHeight != 0) {
                g.FillRectangle(Brushes.White, 0, size.Height - code39Sizer.FontHeightAddon, size.Width, code39Sizer.FontHeightAddon);
                Font f = FontHolder.GenerateFont(code39Sizer.FontHeight);
                g.DrawString(Text, f, Brushes.Black, Rectangle.FromLTRB(guard, size.Height - code39Sizer.FontHeightAddon, size.Width - guard, size.Height), FontHolder.CenterJustify);
            }


            g.Dispose();
            return bm;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code39Generator"/> class.
        /// </summary>
        public Code39Generator() : base(new Code39Sizer()) { }
    }

    /// <summary>
    /// A class that generates Code 93 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.Code93Encoder"/>
    /// <seealso cref="Barcodes.Code93Sizer"/>
    public class Code93Generator : CodeNGenerator {
        #region Encoding Table
        private bool[,] encodings = {
            {true,false,false,false,true,false,true,false,false},
            {true,false,true,false,false,true,false,false,false},
            {true,false,true,false,false,false,true,false,false},
            {true,false,true,false,false,false,false,true,false},
            {true,false,false,true,false,true,false,false,false},
            {true,false,false,true,false,false,true,false,false},
            {true,false,false,true,false,false,false,true,false},
            {true,false,true,false,true,false,false,false,false},
            {true,false,false,false,true,false,false,true,false},
            {true,false,false,false,false,true,false,true,false},
            {true,true,false,true,false,true,false,false,false},
            {true,true,false,true,false,false,true,false,false},
            {true,true,false,true,false,false,false,true,false},
            {true,true,false,false,true,false,true,false,false},
            {true,true,false,false,true,false,false,true,false},
            {true,true,false,false,false,true,false,true,false},
            {true,false,true,true,false,true,false,false,false},
            {true,false,true,true,false,false,true,false,false},
            {true,false,true,true,false,false,false,true,false},
            {true,false,false,true,true,false,true,false,false},
            {true,false,false,false,true,true,false,true,false},
            {true,false,true,false,true,true,false,false,false},
            {true,false,true,false,false,true,true,false,false},
            {true,false,true,false,false,false,true,true,false},
            {true,false,false,true,false,true,true,false,false},
            {true,false,false,false,true,false,true,true,false},
            {true,true,false,true,true,false,true,false,false},
            {true,true,false,true,true,false,false,true,false},
            {true,true,false,true,false,true,true,false,false},
            {true,true,false,true,false,false,true,true,false},
            {true,true,false,false,true,false,true,true,false},
            {true,true,false,false,true,true,false,true,false},
            {true,false,true,true,false,true,true,false,false},
            {true,false,true,true,false,false,true,true,false},
            {true,false,false,true,true,false,true,true,false},
            {true,false,false,true,true,true,false,true,false},
            {true,false,false,true,false,true,true,true,false},
            {true,true,true,false,true,false,true,false,false},
            {true,true,true,false,true,false,false,true,false},
            {true,true,true,false,false,true,false,true,false},
            {true,false,true,true,false,true,true,true,false},
            {true,false,true,true,true,false,true,true,false},
            {true,true,false,true,false,true,true,true,false},
            {true,false,false,true,false,false,true,true,false},
            {true,true,true,false,true,true,false,true,false},
            {true,true,true,false,true,false,true,true,false},
            {true,false,false,true,true,false,false,true,false},
            {true,false,true,false,true,true,true,true,false}
        };
        #endregion


        /// <summary>
        /// Checks the data sent to the generator, to ensure that it is valid.
        /// </summary>
        /// <param name="data">A byte array of data to encode.</param>
        protected override void CheckData(byte[] data) {
            if (data == null)
                return;
            foreach (byte b in data)
                if (b > 46)
                    throw new ArgumentException("The specified data contains invalid codes.");
        }

        private void EncodeByte(ref int p, byte b) {
            for (int i = 0; i < 9; i++)
                EncodedData[p++] = encodings[b, i];
        }

        /// <summary>
        /// Encodes the barcode data in <see cref="Barcodes.CodeNGenerator.Data"/> into a barcode pattern.
        /// </summary>
        protected override void EncodeData() {
            if (EncodedData != null)
                return;

            EncodedData = new BitArray((Data.Length+2)*9+1);
            int pos = 0;
            EncodeByte(ref pos,47);
            foreach (byte b in Data)
                EncodeByte(ref pos, b);
            EncodeByte(ref pos,47);
            EncodedData[pos++] = true;

            ((Code93Sizer)Sizer).Length = EncodedData.Length;
        }

        /// <summary>
        /// Gets the barcode generator capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/>|<see cref="Barcodes.BarcodeGeneratorFlags.VariableLength"/>.</value>
        public override BarcodeGeneratorFlags Flags {
            get { return BarcodeGeneratorFlags.Linear | BarcodeGeneratorFlags.VariableLength; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code93Generator"/> class.
        /// </summary>
        public Code93Generator() : base(new Code93Sizer()) { }
    }

    /// <summary>
    /// A class that generates Code 128 barcodes.
    /// </summary>
    /// <seealso cref="Barcodes.Code128Encoder"/>
    /// <seealso cref="Barcodes.Code128Sizer"/>
    public class Code128Generator : CodeNGenerator {
        
        
        private void EncodeByte(ref int pos, byte b) {
            for (int i = 0; i < 11; i++)
                EncodedData[pos++] = encodings[b, i];
        }

        /// <summary>
        /// Checks the data sent to the generator, to ensure that it is valid.
        /// </summary>
        /// <param name="data">A byte array of data to encode.</param>
        protected override void CheckData(byte[] data) {
            if (data == null)
                return;
            if (data.Length < 2)
                throw new ArgumentException("The specified data is too short.");
            foreach (byte b in data)
                if (b > 106)
                    throw new ArgumentException("The specified data contains invalid codes.");
            if (data[0] < 103 || data[0] > 105)
                throw new ArgumentException("The specified data does not start with a valid start code.");
            if (data[data.Length - 1] != 106)
                throw new ArgumentException("The specified data does not end with the stop code.");
        }

        /// <summary>
        /// Encodes the barcode data in <see cref="Barcodes.CodeNGenerator.Data"/> into a barcode pattern.
        /// </summary>
        protected override void EncodeData() {
            if (EncodedData != null)
                return;

            EncodedData = new BitArray((Data.Length) * 11+2);
            int pos = 0;
            foreach (byte b in Data)
                EncodeByte(ref pos, b);
            EncodedData[pos++] = true;
            EncodedData[pos++] = true;

            ((Code128Sizer)Sizer).Length = EncodedData.Length;
        }

        #region Encoding Table
        private bool[,] encodings = {
            {true,true,false,true,true,false,false,true,true,false,false},  //0
            {true,true,false,false,true,true,false,true,true,false,false}, 
            {true,true,false,false,true,true,false,false,true,true,false}, 
            {true,false,false,true,false,false,true,true,false,false,false}, 
            {true,false,false,true,false,false,false,true,true,false,false}, 
            {true,false,false,false,true,false,false,true,true,false,false}, 
            {true,false,false,true,true,false,false,true,false,false,false}, 
            {true,false,false,true,true,false,false,false,true,false,false}, 
            {true,false,false,false,true,true,false,false,true,false,false}, 
            {true,true,false,false,true,false,false,true,false,false,false}, 
            {true,true,false,false,true,false,false,false,true,false,false},//10 
            {true,true,false,false,false,true,false,false,true,false,false}, 
            {true,false,true,true,false,false,true,true,true,false,false}, 
            {true,false,false,true,true,false,true,true,true,false,false}, 
            {true,false,false,true,true,false,false,true,true,true,false}, 
            {true,false,true,true,true,false,false,true,true,false,false}, 
            {true,false,false,true,true,true,false,true,true,false,false}, 
            {true,false,false,true,true,true,false,false,true,true,false}, 
            {true,true,false,false,true,true,true,false,false,true,false}, 
            {true,true,false,false,true,false,true,true,true,false,false}, 
            {true,true,false,false,true,false,false,true,true,true,false},  //20
            {true,true,false,true,true,true,false,false,true,false,false}, 
            {true,true,false,false,true,true,true,false,true,false,false}, 
            {true,true,true,false,true,true,false,true,true,true,false}, 
            {true,true,true,false,true,false,false,true,true,false,false}, 
            {true,true,true,false,false,true,false,true,true,false,false}, 
            {true,true,true,false,false,true,false,false,true,true,false}, 
            {true,true,true,false,true,true,false,false,true,false,false}, 
            {true,true,true,false,false,true,true,false,true,false,false}, 
            {true,true,true,false,false,true,true,false,false,true,false}, 
            {true,true,false,true,true,false,true,true,false,false,false},  //30
            {true,true,false,true,true,false,false,false,true,true,false}, 
            {true,true,false,false,false,true,true,false,true,true,false}, 
            {true,false,true,false,false,false,true,true,false,false,false}, 
            {true,false,false,false,true,false,true,true,false,false,false}, 
            {true,false,false,false,true,false,false,false,true,true,false}, 
            {true,false,true,true,false,false,false,true,false,false,false}, 
            {true,false,false,false,true,true,false,true,false,false,false}, 
            {true,false,false,false,true,true,false,false,false,true,false}, 
            {true,true,false,true,false,false,false,true,false,false,false}, 
            {true,true,false,false,false,true,false,true,false,false,false},//40 
            {true,true,false,false,false,true,false,false,false,true,false}, 
            {true,false,true,true,false,true,true,true,false,false,false}, 
            {true,false,true,true,false,false,false,true,true,true,false}, 
            {true,false,false,false,true,true,false,true,true,true,false}, 
            {true,false,true,true,true,false,true,true,false,false,false}, 
            {true,false,true,true,true,false,false,false,true,true,false}, 
            {true,false,false,false,true,true,true,false,true,true,false}, 
            {true,true,true,false,true,true,true,false,true,true,false}, 
            {true,true,false,true,false,false,false,true,true,true,false}, 
            {true,true,false,false,false,true,false,true,true,true,false},  //50
            {true,true,false,true,true,true,false,true,false,false,false}, 
            {true,true,false,true,true,true,false,false,false,true,false}, 
            {true,true,false,true,true,true,false,true,true,true,false},
            {true,true,true,false,true,false,true,true,false,false,false},
            {true,true,true,false,true,false,false,false,true,true,false},
            {true,true,true,false,false,false,true,false,true,true,false},
            {true,true,true,false,true,true,false,true,false,false,false},
            {true,true,true,false,true,true,false,false,false,true,false},
            {true,true,true,false,false,false,true,true,false,true,false},
            {true,true,true,false,true,true,true,true,false,true,false},    //60
            {true,true,false,false,true,false,false,false,false,true,false},
            {true,true,true,true,false,false,false,true,false,true,false},
            {true,false,true,false,false,true,true,false,false,false,false},
            {true,false,true,false,false,false,false,true,true,false,false},
            {true,false,false,true,false,true,true,false,false,false,false},
            {true,false,false,true,false,false,false,false,true,true,false},
            {true,false,false,false,false,true,false,true,true,false,false},
            {true,false,false,false,false,true,false,false,true,true,false},
            {true,false,true,true,false,false,true,false,false,false,false},
            {true,false,true,true,false,false,false,false,true,false,false},//70
            {true,false,false,true,true,false,true,false,false,false,false},
            {true,false,false,true,true,false,false,false,false,true,false},
            {true,false,false,false,false,true,true,false,true,false,false},
            {true,false,false,false,false,true,true,false,false,true,false},
            {true,true,false,false,false,false,true,false,false,true,false},
            {true,true,false,false,true,false,true,false,false,false,false},
            {true,true,true,true,false,true,true,true,false,true,false},
            {true,true,false,false,false,false,true,false,true,false,false},
            {true,false,false,false,true,true,true,true,false,true,false},
            {true,false,true,false,false,true,true,true,true,false,false},  //80
            {true,false,false,true,false,true,true,true,true,false,false},
            {true,false,false,true,false,false,true,true,true,true,false},
            {true,false,true,true,true,true,false,false,true,false,false},
            {true,false,false,true,true,true,true,false,true,false,false},
            {true,false,false,true,true,true,true,false,false,true,false},
            {true,true,true,true,false,true,false,false,true,false,false},
            {true,true,true,true,false,false,true,false,true,false,false},
            {true,true,true,true,false,false,true,false,false,true,false},
            {true,true,false,true,true,false,true,true,true,true,false},
            {true,true,false,true,true,true,true,false,true,true,false},    //90
            {true,true,true,true,false,true,true,false,true,true,false},
            {true,false,true,false,true,true,true,true,false,false,false},
            {true,false,true,false,false,false,true,true,true,true,false},
            {true,false,false,false,true,false,true,true,true,true,false},
            {true,false,true,true,true,true,false,true,false,false,false},
            {true,false,true,true,true,true,false,false,false,true,false},
            {true,true,true,true,false,true,false,true,false,false,false},
            {true,true,true,true,false,true,false,false,false,true,false},
            {true,false,true,true,true,false,true,true,true,true,false},
            {true,false,true,true,true,true,false,true,true,true,false},    //100
            {true,true,true,false,true,false,true,true,true,true,false},
            {true,true,true,true,false,true,false,true,true,true,false},
            {true,true,false,true,false,false,false,false,true,false,false},
            {true,true,false,true,false,false,true,false,false,false,false},
            {true,true,false,true,false,false,true,true,true,false,false},
            {true,true,false,false,false,true,true,true,false,true,false}   //106
        };
        #endregion

        /// <summary>
        /// Gets the barcode generator capability flags.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/>|<see cref="Barcodes.BarcodeGeneratorFlags.VariableLength"/>.</value>
        public override BarcodeGeneratorFlags Flags {
            get { return BarcodeGeneratorFlags.VariableLength | BarcodeGeneratorFlags.Linear; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.Code128Generator"/> class.
        /// </summary>
        public Code128Generator() : base(new Code128Sizer()) { }
    }                                                                                     
}