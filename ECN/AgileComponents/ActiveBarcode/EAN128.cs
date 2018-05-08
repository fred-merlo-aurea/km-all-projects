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
    /// An encoder to validate and generate EAN-128 barcodes.
    /// </summary>
    /// <remarks>EAN-128 is used in many trade-related situations, and can encode information such as
    /// manufacturer, serial numbers, dimensions, weights, etc.  The complete specifications are
    /// available from the EAN/UCC.  This particular class is based on the specifications presented in
    /// the EAN/UCC specification version 6.0.  The encoder will check for properly formed data sequences,
    /// as defined by their Application Indicator (AI) codes, and then pass them on to the underlying
    /// Code128 encoder, after adding the proper FNC1 control characters.</remarks>
    public class EAN128Encoder : Code128Encoder {
        //This is used to check the individual pieces of the EAN128 barcode, consisting of blocks of (##)xxxxxxx, called
        //Element Strings.
        //The ## is the Application Identifier, or AI.
        //The xxxxxxx is the data that goes with the corresponding AI.
        private class AIChecker {
            //The AI must match this regex.
            public readonly Regex AIRegex;
            //The data must match this regex.
            public readonly Regex DataRegex;
            //If true, this is a fixed length data block, and does not have to be separated
            //from the next one by a FNC1 code.
            public readonly bool IsFixedLength;

            //If this is not null, the above regexes are ignored (but not the fixed length flag!).
            //Instead, each subchecker is checked, looking for a case where both of its regexes match the
            //AI and data.  If one matches, the code is valid.  If none match, the code is invalid.
            public readonly AIChecker[] subCheckers;

            //Builds a standard check; there is only one valid format for this code.
            public AIChecker(string aiRegex, string dataRegex, bool isFixed) {
                this.AIRegex = new Regex(aiRegex);
                this.DataRegex = new Regex(dataRegex);
                this.IsFixedLength = isFixed;
                this.subCheckers = null;
            }

            //Builds a multi-check; there are several valid formats for this code, based on AI value.
            public AIChecker(bool isFixed, params AIChecker[] checkers) {
                this.subCheckers = checkers;
                this.IsFixedLength = isFixed;
                this.AIRegex = null;
                this.DataRegex = null;
            }

            //Returns true if the AI and data are valid.
            public bool CheckData(string AI, string data) {
                if (subCheckers != null) {
                    foreach (AIChecker c in subCheckers)
                        if (c.CheckData(AI, data))
                            return true;
                    return false;
                } else {
                    if (!AIRegex.IsMatch(AI))
                        return false;
                    if (!DataRegex.IsMatch(data))
                        return false;
                    return true;
                }
            }
        }

        //A list of one hundred AI checkers.  Each AI can be identified by its first two digits, which
        //are used as the index into this array of checkers. Most of the checkers are null, because 
        //there is no valid AI that starts with those two digits.
        #region Checker Definitions
        private AIChecker[] checkers ={
            new AIChecker("00","[0-9]{18}", true), // 00
            new AIChecker("01","[0-9]{14}", true), // 01
            new AIChecker("02","[0-9]{14}", true), // 02
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            new AIChecker("10","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,20}",false),// 10
            new AIChecker("11","[0-9][0-9](0[1-9]|1[0-2])([0-2][0-9]|3[01])",true),// 11
            new AIChecker("12","[0-9][0-9](0[1-9]|1[0-2])([0-2][0-9]|3[01])",true),// 12
            new AIChecker("13","[0-9][0-9](0[1-9]|1[0-2])([0-2][0-9]|3[01])",true),// 13
            null,
            new AIChecker("15","[0-9][0-9](0[1-9]|1[0-2])([0-2][0-9]|3[01])",true),// 15
            null,
            new AIChecker("17","[0-9][0-9](0[1-9]|1[0-2])([0-2][0-9]|3[01])",true),// 17
            null,
            null,
            new AIChecker("20","[0-9]{2}",true),// 20
            new AIChecker("21","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,20}",false),// 21
            new AIChecker("22","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,29}",false),// 22
            null,
            new AIChecker("24[01]","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 24
            new AIChecker(// 25
                false,
                    new AIChecker("25[01]","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),
                    new AIChecker("253","[0-9]{13,30}",false)
                ),
            null,
            null,
            null,
            null,
            new AIChecker("30","[0-9]{1,8}",false),// 30
            new AIChecker("31[0-6][0-9]","[0-9]{6}",true),// 31
            new AIChecker("32[0-9][0-9]","[0-9]{6}",true),// 32
            new AIChecker("33[0-7][0-9]","[0-9]{6}",true),// 33
            new AIChecker("34[0-9][0-9]","[0-9]{6}",true),// 34
            new AIChecker("35[0-7][0-9]","[0-9]{6}",true),// 35
            new AIChecker("36[0-9][0-9]","[0-9]{6}",true),// 36
            new AIChecker("37","[0-9]{1,8}",false),// 37
            null,
            new AIChecker(false,// 39
                new AIChecker("39[02][0-9]","[0-9]{1,15}",false),
                new AIChecker("39[13][0-9]","[0-9]{4,18}",false)
            ),
            new AIChecker(false,// 40
                new AIChecker("40[013]","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),
                new AIChecker("402","[0-9]{17}",false)
            ),
            new AIChecker("41[0-5]","[0-9]{13}",true),// 41
            new AIChecker(false,
                new AIChecker("420","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,20}",false),// 42
                new AIChecker("421","[0-9]{3}[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,9}",false),
                new AIChecker("42[2456]","[0-9]{3}",false),
                new AIChecker("423","([0-9]{3}){1,5}",false)
            ),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            new AIChecker(false,// 70
                new AIChecker("7001","[0-9]{13}",false),
                new AIChecker("7002","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),
                new AIChecker("703[0-9]","[0-9]{3}[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,27}",false)
            ),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            new AIChecker(false,// 80
                new AIChecker("8001","[0-9]{14}",false),
                new AIChecker("8002","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,20}",false),
                new AIChecker("8003","[0-9]{14}[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{0,16}",false),
                new AIChecker("8004","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),
                new AIChecker("8005","[0-9]{6}",false),
                new AIChecker("8006","[0-9]{18}",false),
                new AIChecker("8007","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),
                new AIChecker("8008","[0-9][0-9](0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])([0-5][0-9]){0,2}",false),
                new AIChecker("8018","[0-9]{18}",false),
                new AIChecker("8020","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,25}",false)
            ),
            new AIChecker(false,// 81
                new AIChecker("8100","[0-9]{6}",false),
                new AIChecker("8101","[0-9]{10}",false),
                new AIChecker("8102","0[0-9]",false)
            ),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            new AIChecker("90","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 90
            new AIChecker("91","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 91
            new AIChecker("92","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 92
            new AIChecker("93","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 93
            new AIChecker("94","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 94
            new AIChecker("95","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 95
            new AIChecker("96","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 96
            new AIChecker("97","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 97
            new AIChecker("98","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 98
            new AIChecker("99","[-!\"%&'()*+,/.0-9:;<=>?A-Z_a-z]{1,30}",false),// 99
             
        };
        #endregion

        //Checks to make sure each Element String is valid, then concatenate them, inserting FNC1 codes
        //as necessary.
        private void CheckAndEncodeText(string value, out string encodedText) {
            encodedText=null;
            if (value == null)
                return;

            //Check that value only contains valid characters.
            Regex entry = new Regex("\\(([0-9]{2,4})\\)([-\\[\\]!\"%&'*+,/.0-9:;<=>?A-Z_a-z]{1,30})");

            //Split into components;
            MatchCollection matches = entry.Matches(value);

            int length = 0;
            foreach (Match m in matches) {
                length += m.Length;
            }

            if (length != value.Length)
                throw new ArgumentException("The specified string could not be separated into Element Strings.", "Split");

            //For each match, analyze the contents.
            List<string> appCodes = new List<string>();
            List<string> dataCodes = new List<string>();
            List<bool> fixedLength = new List<bool>();

            int dataChars = 0;

            foreach (Match m in matches) {
                string appCode = m.Groups[1].Value;
                string dataCode = m.Groups[2].Value;
                //Change all []'s int ()'s in dataCode.
                dataCode = dataCode.Replace('[', '(').Replace(']', ')');

                //appPreCode is between 0 and 99 inclusive, since the regex ensures that Substring(0,2) will be two digits.
                int appPreCode = int.Parse(appCode.Substring(0, 2));
                if (checkers[appPreCode] == null)
                    throw new ArgumentException("An element string has an AI that is not defined.", m.Value);
                if (!checkers[appPreCode].CheckData(appCode, dataCode))
                    throw new ArgumentException("An element string is invalid, based on the AI and data.", m.Value);

                appCodes.Add(appCode);
                dataCodes.Add(dataCode);
                fixedLength.Add(checkers[appPreCode].IsFixedLength);
            }

            //Ok, we've checked the codes and they check out. Assemble the data to be sent to the Code 128 encoder.
            encodedText = "%1";
            int i = 0, len = appCodes.Count-1;
            for (i = 0; i <= len; i++) {
                encodedText += appCodes[i];
                //Since we're using function escapes in the Code128 encoder,
                //all %'s that appear in the data must be converted to %%.
                encodedText += dataCodes[i].Replace("%", "%%"); 
                dataChars += appCodes[i].Length + dataCodes[i].Length;
                if (!fixedLength[i] && i != len) {
                    //We must append a new FNC1 code, because the previous was not fixed length, nor was it the last.
                    encodedText += "%1";
                    dataChars++;
                }
            }

            if (dataChars > 48)
                throw new ArgumentException("The resulting EAN-128 barcode contains more than 48 symbols.", "Length");
        }

        private string text;
        /// <summary>
        /// Gets or sets the data to be encoded into an EAN-128 barcode.
        /// </summary>
        /// <value>A string of the data to be encoded, or null.</value>
        /// <remarks><para>The string should be formatted in the standard EAN-128 format: "(##)xxxxxx", where 
        /// ## is the Application Identifier (AI), and xxxxxx is the data corresponding to that AI.
        /// This string forms what is called an "Element String".  You can pass in any number of element strings,
        /// concatenated together, without spaces or other marks between each element string.</para><para>
        /// The encoder first separates the supplied string into its component Element Strings.  If the
        /// encoder fails to separate the supplied string into valid Element Strings, it will throw an
        /// <see cref="System.ArgumentException"/> with a <see cref="System.ArgumentException.ParamName"/> of
        /// "Split".  This phase also checks for well formed Element Strings, ensuring that the AI contains
        /// at least 2 and no more than 4 digits, and is contained in parenthesis, before the data to be
        /// encoded, and that the data does not contain invalid characters (as defined in the EAN/UCC specification,
        /// version 6, table 3.A.3 - 1.<br/><b>Important Note:</b> The data portion of the element string can,
        /// by EAN/UCC specification, contain parenthesis.  However, this will confuse the encoder, since
        /// the AI must be in parenthesis.  As such, when
        /// passing a <b>data</b> string that contains parenthesis to the encoder, the parenthesis must be
        /// converted to square brackets ('(' to '[' and ')' to ']').  The AI remains in parenthesis. When the
        /// barcode is generated, the square brackets will be converted back to parenthesis when they appear in the
        /// human-readable numbering below the barcode.</para>
        /// <para>After it is separated, each individual element string is checked for validity,
        /// depending on the AI that is supplied.  If the Element String is not valid, it will throw an
        /// <see cref="System.ArgumentException"/> with the <see cref="System.ArgumentException.ParamName"/>
        /// property set to the element string that was invalid.</para><para>Once the element strings are checked,
        /// one final check is made to ensure that the total length of the barcode does not exceed 48 data symbols.
        /// This length is calculated by taking the total lengths of the element strings, (excluding the parenthesis
        /// that separate the AI from the data), plus any extra FNC1 codes that must be added to mark the end of a
        /// variable-length element string. (Variable length element strings are any strings with an AI that does 
        /// not begin with 00-04, 11-20, 31-36, or 41.  Note that not all of these AI's are defined as of EAN/UCC
        /// specification version 6.0, but their lengths have been defined.)  If the length is
        /// over 48, the encoder will throw an
        /// <see cref="System.ArgumentException"/> with the <see cref="System.ArgumentException.ParamName"/> set to
        /// "Length".</para></remarks>
        /// <exception cref="System.ArgumentException">There was an error encoding the specified string.</exception>
        public override string Text {
            get {
                return text;
            }
            set {
                string outText;
                CheckAndEncodeText(value, out outText);
                text = value;
                base.UseFunctionEscapes = true;
                base.Text = outText;
                ((Code128Generator)base.Generator).Text = value.Replace('[', '(').Replace(']', ')');
            }
        }

        /// <summary>
        /// Gets flags that represent the encoder's capabilities.
        /// </summary>
        /// <value>Always returns <see cref="Barcodes.BarcodeEncoderFlags.Composite"/>.</value>
        public override BarcodeEncoderFlags Flags {
            get {
                return BarcodeEncoderFlags.Composite;
            }
        }

        /// <summary>
        /// Gets the size that will produce a barcode of the standardized size.
        /// </summary>
        /// <value>The standard size of the barcode.</value>
        /// <remarks>This also performs numerous checks to ensure that the generated barcode will be
        /// within the specification.  This includes checking that the DPI is not zero, that the module
        /// width is between 9.8 and 40 mils, and that the total length of the barcode does not exceed
        /// 6.5 inches (16.5 cm).  If this property throws an <see cref="System.InvalidOperationException"/>,
        /// and the DPI is set and the module width <see cref="Barcodes.IBarcodeModularSizer.Module"/> is between
        /// 9.8 and 40 mils, then the barcode is too long to be encoded at the current module width, and the
        /// module width should be decreased.</remarks>
        /// <seealso cref="Barcodes.EAN128Encoder.CalculateModule"/>
        public Size StandardSize {
            get {
                if (Sizer.DPI == 0)
                    throw new InvalidOperationException("The DPI is not set.");
                if (((IBarcodeModularSizer)Generator.Sizer).Module <9.8f)
                    throw new InvalidOperationException("The current specified module width is less than the minimum allowable width (9.8 mils/0.25 mm).");
                if (((IBarcodeModularSizer)Generator.Sizer).Module > 40f)
                    throw new InvalidOperationException("The current specified module width is greater than the maximum allowable width (40 mils/1.016 mm).");
                Size stdSize=new Size(Sizer.Width,Sizer.ExtraHeight+(int)Math.Ceiling(1.25f*Sizer.DPI));
                if ((stdSize.Width/Sizer.DPI)>6.5f)
                    throw new InvalidOperationException("At the specified DPI and module width, the symbol would exceed the maximum allowable width, 6.5 inches (165 mm)");
                return stdSize;
            }
        }

        /// <summary>
        /// This is a convenience function, to calculate and set a valid module width.
        /// </summary>
        /// <remarks>This should be called if <see cref="Barcodes.EAN128Encoder.StandardSize"/> throws an
        /// <see cref="System.InvalidOperationException"/>.  It will set the module width to a value that results in
        /// a barcode with a total width less than 6.5 inches.</remarks>
        /// <exception cref="System.ArgumentException">The function cannot calculate a valid module width; this shouldn't happen.</exception>
        /// <exception cref="System.InvalidOperationException">The DPI is set to zero.</exception>
        public void CalculateModule() {
            float module = ActiveUp.WebControls.BarcodeUtilities.CalculateModuleWidth((IBarcodeModularSizer)Sizer, (int)(6.5f * Sizer.DPI));
            ((IBarcodeModularSizer)Sizer).Module = module;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.EAN128Encoder"/> class.
        /// </summary>
        public EAN128Encoder() {
            ((IBarcodeModularSizer)Generator.Sizer).Module = 10f;
            Sizer.Mode=BarcodeRenderMode.Guarded|BarcodeRenderMode.Numbered;
        }
    }
}


    

    

