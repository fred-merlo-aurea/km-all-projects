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
    /// An encoder for ITF14 barcodes, used to encode GTIN numbers in a standardized manner.
    /// </summary>
    /// <remarks>The ITF14 encoder sets up the module width and rendering flags of the underlying
    /// Interleaved 2of5 generator.  These should not be changed.</remarks>
    public class ITF14Encoder : I2of5Encoder {
        private void CheckText(string value) {
            if (value == null) {
                base.Text = null;
                return;
            }

            foreach (char c in value) {
                if (!char.IsDigit(c))
                    throw new ArgumentException("The input to IFT14 must be a 7, 11, or 12 digit number without a checksum digit.", "Digit");
            }

            if (value.Length != 13)
                throw new ArgumentException("The input to IFT14 must be a 7, 11, or 12 digit number without a checksum digit.", "Length");
        }

        private string text = "";
        /// <summary>
        /// Gets or sets the data to be encoded.
        /// </summary>
        /// <value>The data to be encoded.</value>
        /// <remarks><para>ITF14 can only encode 13 digit strings, beginning with a mode indicator digit as defined in the EAN/UCC
        /// specification, and ending with a 7, 11, or 12 digit number, corresponding to the EAN/UCC-8, UCC-12, and EAN/UCC-13
        /// numbers, respectively, without their check digits.  The space between the mode indicator and the 7, 11, or 12 digit 
        /// number must be filled with zeros so that the total length is 13. (That is, five zeros for a EAN/UCC-8 number,
        /// one zero for a UCC-12 number, and no zeros for an EAN/UCC-13 number).  The check digit is calculated by the
        /// encoder, as it might be different from the original check digit in the EAN/UCC number.</para><para>
        /// The mode indicator is a single digit, 0-9.  To encode standard UCC-12 or EAN/UCC-13 numbers as ITF14,
        /// use mode 0.  To convert an EAN/UCC-8, UCC-12, or EAN/UCC-13 number to EAN/UCC-14, use 1-8 (the actual
        /// digit to use is assigned by the EAN/UCC).  Mode indicator 9 is used to encode variable measure trade
        /// items.  For more information, check the EAN/UCC specifications.</para></remarks>
        public override string Text {
            get {
                return text;
                ;
            }
            set {
                CheckText(value); //This will set base.Text.
                text = value;
            }
        }

        /// <summary>
        /// Gets the size that will produce a barcode of the standardized size, 1.25 inches tall.
        /// </summary>
        /// <value>The standard size of the barcode.</value>
        public Size StandardSize {
            get {
                if (Sizer.DPI == 0)
                    return Sizer.Size;
                else //Internal barcode height is 1.25 inches.
                    return new Size(Sizer.Width, Sizer.Height - 1 + (int)Math.Ceiling(5 * Sizer.DPI / 4));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Barcodes.ITF14Encoder"/> class.
        /// </summary>
        /// <remarks>This constructor sets the module width and rendering mode of the underlying <see cref="Barcodes.I2of5Encoder"/>.</remarks>
        public ITF14Encoder() {
            ((IBarcodeModularSizer)Sizer).Module = 40f;
            Sizer.Mode = BarcodeRenderMode.Guarded | BarcodeRenderMode.Numbered | BarcodeRenderMode.Braced;
        }
    }
}