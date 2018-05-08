using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ActiveUp.WebControls;

namespace ActiveUp.WebControls {
    /// <summary>
    /// Date formats available in HIBC secondary data encodings.
    /// </summary>
    public enum HibcDateFormat {
        /// <summary>
        /// The date is encoded as YYMMDDHH (Year, Month, Day, Hour).
        /// </summary>
        YearMonthDayHour,
        /// <summary>
        /// The date is encoded as MMYY (Month, Year).
        /// </summary>
        MonthYear,
        /// <summary>
        /// The date is encoded as MMDDYY (Month, Day, Year).
        /// </summary>
        MonthDayYear,
        /// <summary>
        /// The date is encoded as YYMMDD (Year, Month, Day).
        /// </summary>
        YearMonthDay,
        /// <summary>
        /// The date is encoded as YYJJJ (Year, Julian Day).
        /// </summary>
        YearJulian,
        /// <summary>
        /// The date is encoded as YYJJJHH (Year, Julian Day, Hour).
        /// </summary>
        YearJulianHour
    }

    /// <summary>
    /// Holds data for the HIBC secondary barcode, encoding date, quantity, and serial/lot/batch number.
    /// </summary>
    public class SecondaryDataHibc {
        private string lot;
        private int quantity;
        private DateTime date;
        private HibcDateFormat dateFormat = HibcDateFormat.MonthYear;

        private static readonly Regex lotCheck = new Regex("[0-9A-Za-z]{1,13}");

        /// <summary>
        /// Gets or sets the serial, lot, or batch number to be encoded.
        /// </summary>
        /// <value>The serial, lot, or batch number, or null.</value>
        /// <remarks>The serial, lot, or batch number must be between 1 and 13 (inclusive) alphanumeric characters long.
        /// If there is no serial, lot, or batch number, this property should be set to null, not "".  The default value is null.</remarks>
        /// <exception cref="System.ArgumentException">The specified serial, lot, or batch number is not between 1 and 13 (inclusive)
        /// characters long, or contains non-alphanumeric characters.</exception>
        public string Lot {
            get {return lot;}
            set {
                if (value!=null && !lotCheck.IsMatch(value))
                    throw new ArgumentException("The specified lot/batch/serial number is invalid (must be 1-13 alphanumeric characters long).");
                lot=value.ToUpper();
            }
        }

        /// <summary>
        /// Gets or sets the date to be encoded.
        /// </summary>
        /// <value>The date to be encoded, or null.</value>
        /// <remarks>The date should be set to null if there is no date to encode. The default value is null.</remarks>
        public DateTime Date {
            get { return date; }
            set { date = value; }
        }
 
        /// <summary>
        /// Gets and sets the format used to encode the date and time supplied.
        /// </summary>
        /// <value>The <see cref="Barcodes.HIBC.DateFormat"/> format value.</value>
        public HibcDateFormat DateFormat {
            get { return dateFormat; }
            set { dateFormat = value; }
        }

        /// <summary>
        /// Gets or sets the quantity to encode.
        /// </summary>
        /// <value>The quantity to encode, or null for no quantity.</value>
        /// <remarks>A value of zero will be encoded as a quantity of zero; it will not result in no quantity encoding.
        /// If you do not want a quantity encoding to appear at all, set this value to null. The default value is 
        /// null.  The quantity, if set, must be between 0 and 99,999 inclusive.</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">The specified quantity was not between 0 and 
        /// 99,999 inclusive.</exception>
        public int Quantity {
            get {return quantity;}
            set {
                if (value!=null && (value<0 || value>99999))
                    throw new ArgumentOutOfRangeException("The specified quantity must be between 0 and 99,999 inclusive, or null.");
                quantity=value;
            }
        }

        //Returns a partially encoded data string, without preamble, link, or check digits.
        private string Encoded {
            get {
                string encoded = "";
                bool hasLot = (lot != null);
                bool hasDate = (date != null);
                bool hasQty = (quantity != null);

                string dateEncoded = "";
                if (hasDate) {
                    switch (dateFormat) {
                        case HibcDateFormat.MonthYear:                            
                            dateEncoded = date.ToString("MMyy");
                            break;
                        case HibcDateFormat.MonthDayYear:
                            dateEncoded = date.ToString("2MMddyy");
                            break;
                        case HibcDateFormat.YearJulian:
                            dateEncoded = date.ToString("5yy") + date.DayOfYear.ToString("000");
                            break;
                        case HibcDateFormat.YearJulianHour:
                            dateEncoded = date.ToString("6yy") + date.DayOfYear.ToString("000") + date.ToString("HH");
                            break;
                        case HibcDateFormat.YearMonthDay:
                            dateEncoded = date.ToString("3yyMMdd");
                            break;
                        case HibcDateFormat.YearMonthDayHour:
                            dateEncoded = date.ToString("4yyMMddHH");
                            break;
                    }
                }

                string qtyEncoded="";
                if (hasQty) {
                    if (quantity < 100) {
                        qtyEncoded = string.Format("8{0:00}", quantity);
                    } else {
                        qtyEncoded = string.Format("9{0:00000}", quantity);
                    }
                }

                encoded = qtyEncoded + dateEncoded;
                if (lot != null) {
                    if (dateEncoded == "")
                        encoded += "7";
                    encoded += lot;
                }

                return encoded;
            }
        }

        /// <summary>
        /// Gets the shortened HIBC secondary encoding, if one exists.
        /// </summary>
        /// <value>The shortened HIBC secondary encoding, or null.</value>
        /// <remarks>There are two cases in which the HIBC secondary data can be encoded in a shortened form:
        /// if there is only a serial/lot/batch number present, or there is only a date present and the 
        /// <see cref="SecondaryData.DateFormat"/> is <see cref="DateFormat.YearJulian"/>.
        /// This property will return complete secondary encoding strings in these two cases; otherwise it returns null.</remarks>
        public string HIBCShortEncoded {
            get {
                if (quantity == null && lot == null && date != null && dateFormat == HibcDateFormat.YearJulian)
                    return "+" + date.ToString("yy") + date.DayOfYear.ToString("000");
                if (quantity == null && date == null && lot != null)
                    return "+$" + lot;
                return null;
            }
        }

        /// <summary>
        /// Gets the complete HIBC secondary encoding, for use with HIBC primary encodings.
        /// </summary>
        /// <value>A complete HIBC secondary encoding.</value>
        /// <remarks>This encoding is for use in Code39 and Code128 secondary barcodes, and combined barcodes,
        /// with HIBC LIC data.  It is not for use in EAN128 barcodes; that encoding is returned by 
        /// <see cref="Barcodes.HIBC.SecondaryData.GS1Encoded"/>.</remarks>
        public string HIBCEncoded {
            get {
                return "+$$" + Encoded;
            }
        }

        /// <summary>
        /// Gets the complete GS1 secondary encoding, for use in EAN128 barcodes.
        /// </summary>
        /// <value>A complete secondary encoding, formatted for inclusion in an EAN128 barcode.</value>
        /// <remarks>This encoding is for use in EAN128 barcodes, used with GS1 primary data.  It is not suitable
        /// for use in Code39 or Code128 barcodes used with HIBC LIC primary data; that encoding is returned
        /// by <see cref="Barcodes.HIBC.SecondaryData.HIBCEncoded"/>.</remarks>
        public string GS1Encoded {
            get { return "(22)" + Encoded; }
        }

        

    }
    
    /// <summary>
    /// Barcode formats available to use for the HIBC primary data.
    /// </summary>
    /// <remarks>The actual formats available depends on whether the primary data is in HIBC LIC form or GS1 form.</remarks>
    public enum PrimaryEncodingMode {
        /// <summary>
        /// Encode HIBC LIC data as a Code39 barcode.
        /// </summary>
        Code39,
        /// <summary>
        /// Encode HIBC LIC data as a Code128 barcode.
        /// </summary>
        Code128,
        /// <summary>
        /// Encode GS1 data as an EAN128 barcode.
        /// </summary>
        EAN128,
        /// <summary>
        /// Encode GS1 data as an Interleaved 2of5 barcode.
        /// </summary>
        I2of5
    }

    /// <summary>
    /// Barcode formats available to use for the HIBC secondary data.
    /// </summary>
    /// <remarks>The actual formats available depend on the primary encoding method used.</remarks>
    public enum SecondaryEncodingMode {
        /// <summary>
        /// Encode the secondary data as a Code39 barcode.  This is only available if the primary barcode is
        /// Code39 or Code128.
        /// </summary>
        Code39,
        /// <summary>
        /// Encode the secondary data as a Code128 barcode.  This is only available if the primary barcode is
        /// Code39 or Code128.
        /// </summary>
        Code128,
        /// <summary>
        /// Encode the secondary data as an EAN128 barcode.  This is the only option for a separate secondary
        /// barcode if the primary data is in GS1 format (EAN128 or I2of5).
        /// </summary>
        EAN128,
        /// <summary>
        /// Attempt to combine the secondary data with the primary barcode.  If the primary is encoded as EAN128, 
        /// this might result in an <see cref="System.ArgumentException"/> being thrown due to the total combined
        /// length being greater than the maximum encodable size.
        /// </summary>
        Combined,
        /// <summary>
        /// Ignore the secondary data; do not emit a secondary barcode.
        /// </summary>
        None
    }

    /// <summary>
    /// A holding class, containing static methods that prepare barcode encoder instances.
    /// </summary>
    /// <remarks>The four static methods each return an array of one or two <see cref="Barcodes.IBarcodeEncoder"/> objects,
    /// the first representing the primary barcode, and the second, the secondary barcode, if one is requested.  There are
    /// two standards used to encode HIBC data: the HIBC LIC (Labeler Identification Code), which consists of a LIC, a PCN
    /// (Product or Catalog Number) and a Unit of Measure digit; and the GS1, which uses an EAN/UCC assigned GS1 number, along
    /// with a Unit of Measure digit.  These two primary data standards each carry their own encoding requirements and
    /// available symbologies, as described in their corresponding methods, 
    /// <see cref="Barcodes.HIBC.HIBCEncoder.EncodeHIBC"/> and
    /// <see cref="Barcodes.HIBC.HIBCEncoder.EncodeGS1"/>.</remarks>
    public class HIBCEncoder {
        private static Regex licCheck=new Regex("[A-Z][A-Z0-9]{3}",RegexOptions.IgnoreCase);
        private static Regex pcnCheck=new Regex("[A-Z0-9]{1,13}",RegexOptions.IgnoreCase);

        private static byte CalculateHIBCCheck(string value) {
            int checkSum = 0;
            foreach (char c in value) {
                int val = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%".IndexOf(c);
                if (val == -1)
                    return 255;
                checkSum += val;
            }
            return (byte)(checkSum % 43);
        }

        /// <summary>
        /// Encodes an HIBC primary data string.
        /// </summary>
        /// <param name="LIC">The Labeler Identification Code, a four character code, consisting of a letter and three alphanumeric characters.</param>
        /// <param name="PCN">A Product or Catalog Number, consisting of 1-13 alphanumeric characters.</param>
        /// <param name="unitOfMeasure">A number from 0 to 9, inclusive, representing the unit of measure.</param>
        /// <param name="secondary">An optional <see cref="Barcodes.HIBC.SecondaryData"/> object containing secondary data.</param>
        /// <param name="primaryMode">The symbology to use for the primary barcode.  Must be <see cref="Barcodes.HIBC.PrimaryEncodingMode.Code39"/> or <see cref="Barcodes.HIBC.PrimaryEncodingMode.Code128"/>.</param>
        /// <param name="secondaryMode">The symbology to use for the secondary barcode.  May be any <see cref="Barcodes.HIBC.SecondaryEncodingMode"/> value other than <see cref="Barcodes.HIBC.SecondaryEncodingMode.EAN128"/>, however,
        /// if <paramref name="secondary"/> is null, this value must be <see cref="Barcodes.HIBC.SecondaryEncodingMode.None"/>.</param>
        /// <returns>An array of <see cref="Barcodes.IBarcodeEncoder"/> objects, containing one or two pre-set encoders.</returns>
        /// <remarks>This is the same function as <see cref="Barcodes.HIBC.HIBCEncoder.EncodeHIBC(string, SecondaryData, PrimaryEncodingMode, SecondaryEncodingMode)"/>, except 
        /// that the primary data (LIC, PCN, and Unit of Measure) is combined into one parameter in the alternate method.</remarks>
        /// <exception cref="System.ArgumentException">There was an error in one or more of the arguments
        /// (<paramref name="LIC"/>, <paramref name="PCN"/>, or <paramref name="unitOfMeasure"/>).  The <see cref="System.ArgumentException.ParamName"/>
        /// will be the name of the invalid parameter.</exception>
        /// <exception cref="System.InvalidOperationException">The <paramref name="primaryMode"/> or <paramref name="secondaryMode"/>
        /// values are invalid for the type of data provided (HIBC vs. GS1).</exception>
        /// <exception cref="System.NullReferenceException">Argument <paramref name="secondary"/> is null and <paramref name="secondaryMode"/> is not <see cref="Barcodes.HIBC.SecondaryEncodingMode.None"/>.</exception>
        public static IBarcodeEncoder[] EncodeHIBC(string LIC, string PCN, byte unitOfMeasure, SecondaryDataHibc secondary, PrimaryEncodingMode primaryMode, SecondaryEncodingMode secondaryMode) {
            if (!licCheck.IsMatch(LIC))
                throw new ArgumentException("LIC is invalid; it must be four alphanumeric characters, starting with a letter.","LIC");
            if (!pcnCheck.IsMatch(PCN))
                throw new ArgumentException("PCN is invalid; it must be 1-13 alphanumeric characters.","PCN");
            if (unitOfMeasure > 9)
                throw new ArgumentException("unitOfMeasure must be between 0 and 9 inclusive.","unitOfMeasure");
            return EncodeHIBC(LIC + PCN + unitOfMeasure.ToString(), secondary, primaryMode, secondaryMode);
        }

        /// <summary>
        /// Encodes an HIBC primary data string.
        /// </summary>
        /// <param name="primary">The primary data string, consisting of the LIC, the PCN, and the unit of measure digit.</param>
        /// <param name="secondary">An optional <see cref="Barcodes.HIBC.SecondaryData"/> object containing secondary data.</param>
        /// <param name="primaryMode">The symbology to use for the primary barcode.  Must be <see cref="Barcodes.HIBC.PrimaryEncodingMode.Code39"/> or <see cref="Barcodes.HIBC.PrimaryEncodingMode.Code128"/>.</param>
        /// <param name="secondaryMode">The symbology to use for the secondary barcode.  May be any <see cref="Barcodes.HIBC.SecondaryEncodingMode"/> value other than <see cref="Barcodes.HIBC.SecondaryEncodingMode.EAN128"/>, however,
        /// if <paramref name="secondary"/> is null, this value must be <see cref="Barcodes.HIBC.SecondaryEncodingMode.None"/>.</param>
        /// <returns>An array of <see cref="Barcodes.IBarcodeEncoder"/> objects, containing one or two pre-set encoders.</returns>
        /// <remarks>This is the same function as <see cref="Barcodes.HIBC.HIBCEncoder.EncodeHIBC(string, SecondaryData, PrimaryEncodingMode, SecondaryEncodingMode)"/>, except 
        /// that the primary data (LIC, PCN, and Unit of Measure) is combined into one parameter in the alternate method.</remarks>
        /// <exception cref="System.ArgumentException">There was an error in the primary data.</exception>
        /// <exception cref="System.InvalidOperationException">The <paramref name="primaryMode"/> or <paramref name="secondaryMode"/>
        /// values are invalid for the type of data provided (HIBC vs. GS1).</exception>
        /// <exception cref="System.NullReferenceException">Argument <paramref name="secondary"/> is null and <paramref name="secondaryMode"/> is not <see cref="Barcodes.HIBC.SecondaryEncodingMode.None"/>.</exception>
        public static IBarcodeEncoder[] EncodeHIBC(string primary, SecondaryDataHibc secondary, PrimaryEncodingMode primaryMode, SecondaryEncodingMode secondaryMode) {
            if (primaryMode == PrimaryEncodingMode.EAN128 || primaryMode==PrimaryEncodingMode.I2of5)
                throw new InvalidOperationException("The primary mode can only be Code39 or Code128.");
            if (secondaryMode == SecondaryEncodingMode.EAN128)
                throw new InvalidOperationException("The secondary mode can only be Code39, Code128, Combined, or None.");
            if (secondaryMode != SecondaryEncodingMode.None && secondary == null)
                throw new NullReferenceException("The secondary encoding mode is not None and secondary is null.");

            Regex primaryCheck = new Regex("[A-Z][A-Z0-9]{3}[A-Z0-9]{1,13}[0-9]");
            primary=primary.ToUpper();
            if (!primaryCheck.IsMatch(primary))
                throw new ArgumentException("The specified primary data string (LIC+PCN+Unit) is invalid. The proper format is [A-Z][A-Z0-9]{3}[A-Z0-9]{1,13}[0-9].","primary");

            primary = "+" + primary;
            byte checkSum = CalculateHIBCCheck(primary);

            string secondaryEncoding = "";
            if (secondary != null) {
                if (secondary.HIBCShortEncoded != null)
                    secondaryEncoding = secondary.HIBCShortEncoded;
                else
                    secondaryEncoding = secondary.HIBCEncoded;
            }

            if (secondaryMode == SecondaryEncodingMode.Combined) {
                primary = primary + "/" + secondaryEncoding.Substring(1);
                checkSum = CalculateHIBCCheck(primary);
                primary += "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%"[checkSum];
            } else {
                char checkChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%"[checkSum];
                primary += checkChar;
                secondaryEncoding += checkChar;
                checkSum = CalculateHIBCCheck(secondaryEncoding);
                secondaryEncoding += "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%"[checkSum];
            }

            IBarcodeEncoder primaryEncoder, secondaryEncoder;
            if (primaryMode == PrimaryEncodingMode.Code39) {
                primaryEncoder = new Code39Encoder();
                primaryEncoder.Text = primary;
                ((Code39Generator)primaryEncoder.Generator).Text = "*" + primary.Replace(' ', '_') + "*";
            } else {//Code 128
                primaryEncoder = new Code128Encoder();
                primaryEncoder.Text = primary;
                ((Code128Generator)primaryEncoder.Generator).Text = "*" + primary.Replace(' ', '_') + "*";
            }
            
            if (secondaryMode == SecondaryEncodingMode.Combined || secondaryMode == SecondaryEncodingMode.None)
                return new IBarcodeEncoder[] { primaryEncoder };

            if (secondaryMode == SecondaryEncodingMode.Code39) {
                secondaryEncoder = new Code39Encoder();
                secondaryEncoder.Text = secondaryEncoding;
                ((Code39Generator)secondaryEncoder.Generator).Text = "*" + secondaryEncoding.Replace(' ', '_') + "*";
            } else {
                secondaryEncoder = new Code128Encoder();
                secondaryEncoder.Text = secondaryEncoding;
                ((Code128Generator)secondaryEncoder.Generator).Text = "*" + secondaryEncoding.Replace(' ', '_') + "*";
            }

            return new IBarcodeEncoder[] {primaryEncoder,secondaryEncoder};
        }

        private static byte CalculateGS1Check(string GS1) {
            int weight = 3;
            int checkSum = 0;
            for (int i = 0; i < 13; i++) {
                int val = "0123456789".IndexOf(GS1[i]);
                if (val == -1)
                    return 255;
                checkSum += val * weight;
                weight = 4 - weight;
            }

            return (byte)((10 - checkSum % 10) % 10);
        }

        /// <summary>
        /// Encodes the specified GS1 primary data and optional secondary data into one or more barcodes.
        /// </summary>
        /// <param name="GS1">The GS1 primary data, consisting of an indicator digit, a twelve digit GS1 identifier, and an optional check digit.</param>
        /// <param name="secondary">An optional instance of the <see cref="Barcodes.HIBC.SecondaryData"/> class, containing secondary data.</param>
        /// <param name="primaryMode">The symbology to use for the primary barcode.  Must be <see cref="Barcodes.HIBC.PrimaryEncodingMode.I2of5"/> or <see cref="Barcodes.HIBC.PrimaryEncodingMode.EAN128"/>.</param>
        /// <param name="secondaryMode">The symbology to use for the secondary barcode.  May be any <see cref="Barcodes.HIBC.SecondaryEncodingMode"/> value other than <see cref="Barcodes.HIBC.SecondaryEncodingMode.Code39"/>
        /// or <see cref="Barcodes.HIBC.SecondaryEncodingMode.Code39"/>, however,
        /// if <paramref name="secondary"/> is null, this value must be <see cref="Barcodes.HIBC.SecondaryEncodingMode.None"/>.</param>
        /// <returns>An array of <see cref="Barcodes.IBarcodeEncoder"/> objects, containing one or two pre-set encoders.</returns>
        /// <remarks></remarks>
        /// <exception cref="System.ArgumentException">There was an error in the primary data.</exception>
        /// <exception cref="System.InvalidOperationException">The <paramref name="primaryMode"/> or <paramref name="secondaryMode"/>
        /// values are invalid for the type of data provided (HIBC vs. GS1).</exception>
        /// <exception cref="System.NullReferenceException">Argument <paramref name="secondary"/> is null and <paramref name="secondaryMode"/> is not <see cref="Barcodes.HIBC.SecondaryEncodingMode.None"/>.</exception>
        public static IBarcodeEncoder[] EncodeGS1(string GS1, SecondaryDataHibc secondary, PrimaryEncodingMode primaryMode, SecondaryEncodingMode secondaryMode) {
            return EncodeGS1(GS1, null, secondary, primaryMode, secondaryMode);
        }

        /// <summary>
        /// Encodes the specified GS1 primary data and optional secondary data into one or more barcodes.
        /// </summary>
        /// <param name="GS1">The GS1 primary data, consisting of an indicator digit, a twelve digit GS1 identifier, and an optional check digit.</param>
        /// <param name="PCN">An optional Product or Catalog Number, to be included in an EAN128 barcode under Application Identifier (240). Ignored if <paramref name="primaryMode"/> is <see cref="Barcodes.HIBC.PrimaryEncodingMode.I2of5"/>.</param>
        /// <param name="secondary">An optional instance of the <see cref="Barcodes.HIBC.SecondaryData"/> class, containing secondary data.</param>
        /// <param name="primaryMode">The symbology to use for the primary barcode.  Must be <see cref="Barcodes.HIBC.PrimaryEncodingMode.I2of5"/> or <see cref="Barcodes.HIBC.PrimaryEncodingMode.EAN128"/>.</param>
        /// <param name="secondaryMode">The symbology to use for the secondary barcode.  May be any <see cref="Barcodes.HIBC.SecondaryEncodingMode"/> value other than <see cref="Barcodes.HIBC.SecondaryEncodingMode.Code39"/>
        /// or <see cref="Barcodes.HIBC.SecondaryEncodingMode.Code39"/>, however,
        /// if <paramref name="secondary"/> is null, this value must be <see cref="Barcodes.HIBC.SecondaryEncodingMode.None"/>.</param>
        /// <returns>An array of <see cref="Barcodes.IBarcodeEncoder"/> objects, containing one or two pre-set encoders.</returns>
        /// <remarks></remarks>
        /// <exception cref="System.ArgumentException">There was an error in the GS1 or PCN strings.</exception>
        /// <exception cref="System.InvalidOperationException">The <paramref name="primaryMode"/> or <paramref name="secondaryMode"/>
        /// values are invalid for the type of data provided (HIBC vs. GS1).</exception>
        /// <exception cref="System.NullReferenceException">Argument <paramref name="secondary"/> is null and <paramref name="secondaryMode"/> is not <see cref="Barcodes.HIBC.SecondaryEncodingMode.None"/>.</exception>
        /// <exception cref="System.OverflowException">If <paramref name="primaryMode"/> is <see cref="Barcodes.HIBC.PrimaryEncodingMode.EAN128"/>, the resulting barcode
        /// exceeds the maximum amount of data encodable in an EAN128 barcode.</exception>
        public static IBarcodeEncoder[] EncodeGS1(string GS1, string PCN, SecondaryDataHibc secondary, PrimaryEncodingMode primaryMode, SecondaryEncodingMode secondaryMode) {
            if (primaryMode == PrimaryEncodingMode.Code128 || primaryMode == PrimaryEncodingMode.Code39)
                throw new InvalidOperationException("The primary mode can only be EAN128 or I2of5.");
            if (secondaryMode == SecondaryEncodingMode.Code128 || secondaryMode == SecondaryEncodingMode.Code39)
                throw new InvalidOperationException("The secondary mode can only be EAN128, Combined, or None.");
            if (secondaryMode != SecondaryEncodingMode.None && secondary == null)
                throw new NullReferenceException("The secondary encoding mode is not None and secondary is null.");
            if (primaryMode != PrimaryEncodingMode.EAN128 && secondaryMode == SecondaryEncodingMode.Combined)
                throw new InvalidOperationException("The secondary data can not be combined when the primary encoding is not EAN128.");

            if (GS1.Length != 13 && GS1.Length != 14)
                throw new ArgumentException("The GS1 must be 13 or 14 digits long.","GS1");
            byte checkSum = CalculateGS1Check(GS1);
            if (checkSum == 255)
                throw new ArgumentException("The GS1 number contains non-digit characters.","GS1");
            if (GS1.Length == 14 && "0123456789"[checkSum] != GS1[13])
                throw new ArgumentException("The check digit specified in the GS1 number is invalid.","GS1");
            else if (GS1.Length == 13)
                GS1 += checkSum.ToString();

            if (PCN != null) {
                PCN = PCN.ToUpper();
                if (!pcnCheck.IsMatch(PCN))
                    throw new ArgumentException("The PCN number specified is invalid.","PCN");
            }

            string secondaryEncoding = "";
            if (secondary != null) {
                secondaryEncoding = secondary.GS1Encoded;
                secondaryEncoding += checkSum.ToString(); //Link-char.
            }

            IBarcodeEncoder ean128, itf, secondEAN;

            string primaryEncoding;
            if (primaryMode == PrimaryEncodingMode.EAN128) {
                primaryEncoding = "(01)" + GS1;
                if (PCN!=null)
                    primaryEncoding+="(240)"+PCN;
                if (secondaryMode == SecondaryEncodingMode.Combined && secondary!=null)
                    primaryEncoding += secondaryEncoding;
                ean128 = new EAN128Encoder();
                try {
                    ean128.Text = primaryEncoding;
                } catch (ArgumentException ex) {
                    throw new OverflowException("The combined primary EAN128 barcode was larger than the maximum allowable EAN128 size.", ex);
                }
                if (secondaryMode != SecondaryEncodingMode.EAN128 || secondary==null)
                    return new IBarcodeEncoder[] { ean128 };

                secondEAN = new EAN128Encoder();
                secondEAN.Text = secondaryEncoding;
                return new IBarcodeEncoder[] { ean128, secondEAN };
            } else {
                itf = new I2of5Encoder();

                itf.Text = GS1;

                if (secondaryMode == SecondaryEncodingMode.None || secondary==null)
                    return new IBarcodeEncoder[] { itf };

                //We have a secondary.
                secondEAN = new EAN128Encoder();
                secondEAN.Text = secondaryEncoding;
                return new IBarcodeEncoder[] { itf, secondEAN };
            }
        }
    }
}
