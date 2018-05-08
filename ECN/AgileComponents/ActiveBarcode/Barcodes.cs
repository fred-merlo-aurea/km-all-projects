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
    //Utility

    /// <summary>
    /// A utility class that holds font-related information.  It contains the FontName field, which is the font used
    /// to decorate the barcodes.
    /// </summary>
    internal class FontHolder {
        public static readonly string FontName = "Microsoft Sans Serif";

        public static readonly StringFormat LeftJustify;
        public static readonly StringFormat CenterJustify;
        public static readonly StringFormat RightJustifyRTL;

        static FontHolder() {
            LeftJustify = new StringFormat();
            LeftJustify.Alignment = StringAlignment.Near;

            CenterJustify = new StringFormat();
            CenterJustify.Alignment = StringAlignment.Center;

            RightJustifyRTL = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            RightJustifyRTL.Alignment = StringAlignment.Near;
        }

        public static Font GenerateFont(float fontSize) {
            return new Font(FontName, fontSize, GraphicsUnit.Pixel);
        }
    }

    /// <summary>
    /// A utility class that holds barcode related utilities.  It contains the CalculateModuleWidth method, which
    /// calculates the target module width required to obtain a desired pixel width.
    /// </summary>
    public class BarcodeUtilities {
        /// <summary>
        /// Calculates the module width for a given barcode, with a default tolerance and approach direction.
        /// </summary>
        /// <remarks>
        /// This routine is the equivilent to a call of
        /// <c><see cref="Barcodes.BarcodeUtilities.CalculateModuleWidth(Barcodes.IBarcodeModularSizer,int, bool, float)"/>(<paramref name="sizer"/>, <paramref name="width"/>, false, .1f)</c>.
        /// It returns a module width that produces a barcode that is as close to the target as possible, but no larger.</remarks> 
        /// <param name="sizer">The sizer instance for the barcode to be sized.</param>
        /// <param name="width">The desired pixel width.</param>
        /// <returns>A module width which produces a barcode as close to the target as possible, but no larger.</returns>
        /// <exception cref="System.ArgumentException">The specified pixel width <paramref name="width"/> is smaller than the smallest 
        /// possible width, as calculated with a module width of <see cref="Barcodes.IBarcodeModularSizer.MinimumModule"/>.</exception>
        /// <exception cref="System.NullReferenceException">The specified <paramref name="sizer"/> is null.</exception>
        /// <exception cref="System.InvalidOperationException">The <see cref="Barcodes.IBarcodeSizer.DPI"/> is set to zero.</exception>
        public static float CalculateModuleWidth(IBarcodeModularSizer sizer, int width) {
            return CalculateModuleWidth(sizer, width, false, .1f);
        }

        /// <summary>
        /// Calculates the module width for a given barcode, with a default tolerance.
        /// </summary>
        /// <remarks>
        /// This routine is the equivilent to a call of
        /// <c><see cref="Barcodes.BarcodeUtilities.CalculateModuleWidth(Barcodes.IBarcodeModularSizer,int, bool, float)"/>(<paramref name="sizer"/>, <paramref name="width"/>, <paramref name="aimHigh"/>, .1f)</c>.
        /// </remarks>
        /// <param name="sizer">The sizer instance for the barcode to be sized.</param>
        /// <param name="width">The desired pixel width.</param>
        /// <param name="aimHigh">True to return the module width that produces a width as close as possible 
        /// to but not less than the specified width, false to return a module width as close as possible but
        /// not greater than the specified width.</param>
        /// <returns>A module width, which produces a barcode as close to the target as possible, but no larger or smaller, depending on <paramref name="aimHigh"/>.</returns>
        /// <exception cref="System.ArgumentException">The specified pixel width <paramref name="width"/> is smaller than the smallest 
        /// possible width, as calculated with a module width of <see cref="Barcodes.IBarcodeModularSizer.MinimumModule"/>.</exception>
        /// <exception cref="System.NullReferenceException">The specified <paramref name="sizer"/> is null.</exception>
        /// <exception cref="System.InvalidOperationException">The <see cref="Barcodes.IBarcodeSizer.DPI"/> is set to zero.</exception>
        public static float CalculateModuleWidth(IBarcodeModularSizer sizer, int width, bool aimHigh) {
            return CalculateModuleWidth(sizer, width, aimHigh, .1f);
        }

        /// <summary>
        /// Calculates the module width for a given barcode.
        /// </summary>
        /// <param name="sizer">The sizer instance for the barcode to be sized.</param>
        /// <param name="width">The desired pixel width.</param>
        /// <param name="aimHigh">True to return the module width that produces a width as close as possible 
        /// to but not less than the specified width, false to return a module width as close as possible but
        /// not greater than the specified width.</param>
        /// <param name="tolerance">The desired module tolerance; once calculations narrow the module rande 
        /// to a distance lower than this, calculations stop.</param>
        /// <returns>A module width, which produces a barcode as close to the target as possible, but no larger 
        /// or smaller, depending on <paramref name="aimHigh"/>.</returns>
        /// <exception cref="System.ArgumentException">The specified pixel width <paramref name="width"/> is smaller than the smallest 
        /// possible width, as calculated with a module width of <see cref="Barcodes.IBarcodeModularSizer.MinimumModule"/>, or 
        /// <paramref name="tolerance"/> is less than <c>0.01</c>.</exception>
        /// <exception cref="System.NullReferenceException">The specified <paramref name="sizer"/> is null.</exception>
        /// <exception cref="System.InvalidOperationException">The <see cref="Barcodes.IBarcodeSizer.DPI"/> is set to zero.</exception>
        public static float CalculateModuleWidth(IBarcodeModularSizer sizer, int width, bool aimHigh, float tolerance) {
            if (sizer == null)
                throw new NullReferenceException("The specified sizer must not be null.");
            if (sizer.DPI == 0)
                throw new InvalidOperationException("The DPI must be set (non-zero) in order to calculate the module width.");
            if (tolerance < .01)
                throw new ArgumentException("The specified tolerance must be greater than or equal to 0.01.");
            
            float currentModule = sizer.Module;

            try {
                //We do a binary search, looking for a module width that gives the desired width.
                float currentLow = sizer.MinimumModule;
                float currentHigh = sizer.MinimumModule;

                sizer.Module = currentLow;
                if (width < sizer.Width)
                    throw new ArgumentException("The specified width is unattainable; it requires a module width below MinimumModule.");

                //Find two bracketing module widths, one lower and one higher than the target width.
                //To ensure that this doesn't take long, it starts ad MinimumModule, and doubles the
                //high estimate each time.
                do {
                    currentLow = currentHigh;
                    currentHigh *= 2;
                    sizer.Module = currentHigh;
                } while (width > sizer.Width);

                //Ok, the desired module width is between currentLow and currentHigh.
                //Do a binary search to narrow down the range.
                float currentMiddle;

                while ((currentHigh - currentLow) > tolerance) {
                    currentMiddle = (currentLow + currentHigh) / 2;
                    sizer.Module = currentMiddle;
                    if (width < sizer.Width)
                        currentHigh = currentMiddle;
                    else if (width > sizer.Width)
                        currentLow = currentMiddle;
                    else
                        return currentMiddle; //Width==sizer.Width. We found the perfect size.
                }
                //This will be close to the desired size, but will be slightly larger or smaller than the specified width.
                return aimHigh?currentHigh:currentLow; 
            } finally {
                //Make sure the module is reset to its previous value.
                sizer.Module = currentModule;
            }
        }
    }

    /// <summary>
    /// Flags that specify the capabilities and behaviors of a barcode generator implementing <see cref="Barcodes.IBarcodeGenerator"/>.
    /// </summary>
    [Flags]
    public enum BarcodeGeneratorFlags {
        /// <summary>
        /// No capabilities.
        /// </summary>
        None = 0,

        /// <summary>
        /// The barcode generated is a 1D linear barcode. This flag and <see cref="Barcodes.BarcodeGeneratorFlags.TwoDimensional"/> are mutually exclusive.
        /// </summary>
        Linear = 1,

        /// <summary>
        /// The barcode generated is a 2D barcode. This flag and <see cref="Barcodes.BarcodeGeneratorFlags.Linear"/> are mutually exclusive.
        /// </summary>
        TwoDimensional = 2,

        /// <summary>
        /// The barcode's height is strictly dependant on the barcode's width.  The 
        /// Height parameter of any size passed to <see cref="IBarcodeGenerator.GenerateBarcode"/>
        /// will be ignored.
        /// </summary>
        HeightDependant = 4,

        /// <summary>
        /// The barcode has a particular range of aspect ratios, Height/Width, that it must meet, as specified by
        /// <see cref="IBarcodeSizer.AspectRatioMin"/> and <see cref="IBarcodeSizer.AspectRatioMax"/>.
        /// </summary>
        AspectRatio = 8,

        /// <summary>
        /// The barcode's width is dependant upon the data to be encoded. <see cref="IBarcodeEncoder.Text"/> or 
        /// <see cref="IBarcodeEncoder.Data"/> must be set before obtaining a width from <see cref="IBarcodeSizer.Width"/>.
        /// </summary>
        VariableLength = 16,

        /// <summary>
        /// The barcode's dimensions are strictly dictated by the standard.  The size passed into 
        /// <see cref="IBarcodeGenerator.GenerateBarcode"/> must be the size returned by 
        /// <see cref="IBarcodeSizer.Size"/>.
        /// </summary>
        FixedDimensions = 32,
    }

    /// <summary>
    /// Flags that control rendering options for barcodes.
    /// </summary>
    /// <remarks>Certain flags are not used by certain generators.  The flags that are used by a given generator
    /// will be specified in the generator's documentation.  Furthermore, depending on the generator, setting some
    /// flags will set other flags.</remarks>
    [Flags]
    public enum BarcodeRenderMode {
        /// <summary>
        /// No options.
        /// </summary>
        None = 0,

        /// <summary>
        /// Include blank "guard" or "quiet" zones on each end of the barcode.
        /// </summary>
        Guarded = 1,

        /// <summary>
        /// Include notches in barcode, into which numbers can be drawn.
        /// </summary>
        Notched = 2,

        /// <summary>
        /// Include human-readable barcode information.
        /// </summary>
        Numbered = 4,

        /// <summary>
        /// Include "bearer bars", two horizontal bars at the top and bottom of the barcode, to prevent partial scans.
        /// </summary>
        Braced=8,

        /// <summary>
        /// Put the barcode in a box, including guard zones.
        /// </summary>
        Boxed=16
    }

    /// <summary>
    /// Flags that specifiy the capabilities of an <see cref="Barcodes.IBarcodeEncoder"/>.
    /// </summary>
    [Flags]
    public enum BarcodeEncoderFlags {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,

        /// <summary>
        /// The barcode can encode strings of particular characters.  The data to be encoded is set in 
        /// <see cref="IBarcodeEncoder.Text"/>. The characters that can be used are specified in 
        /// <see cref="IBarcodeEncoder.TextSymbols"/>.
        /// </summary>
        Text = 1,

        /// <summary>
        /// The barcode information to be encoded can be set with <see cref="IBarcodeEncoder.Data"/>.
        /// </summary>
        Data = 2,

        /// <summary>
        /// The barcode can encode arbitrary binary data, set with <see cref="IBarcodeEncoder.Data"/>.
        /// </summary>
        Binary = 4,

        /// <summary>
        /// The barcode encodes multi-byte data codes, set with <see cref="IBarcodeEncoder.Data"/>.
        /// </summary>
        ExtendedData = 8,

        /// <summary>
        /// The barcode encodes ASCII data (0-127), set with either the <see cref="IBarcodeEncoder.Text"/>
        /// or <see cref="IBarcodeEncoder.Data"/>.
        /// </summary>
        ASCII=16,

        /// <summary>
        /// The barcode encoder is a composite encoder.  The generator used (as returned by <see cref="IBarcodeEncoder.Generator"/>) 
        /// and corresponding sizer, as returned by <see cref="IBarcodeEncoder.Sizer"/>, might not set until the data to be encoded 
        /// has been set, and might change after the data to be encoded changes.
        /// </summary>
        Composite=32,
    }

    //Interfaces

    /// <summary>
    /// The main interface for a barcode encoder.  The encoder encodes plaintext data into codes suitable for
    /// barcode generation by an <see cref="Barcodes.IBarcodeGenerator"/>.
    /// </summary>
    /// <remarks>This is the main interface that the library user interacts with.  Under most circumstances,
    /// classes implementing <see cref="Barcodes.IBarcodeGenerator"/> should not be used directly.</remarks>
    public interface IBarcodeEncoder {
        /// <summary>
        /// Gets or sets the data to be encoded by a barcode, as a byte array.
        /// </summary>
        /// <value>The byte data to be encoded.</value>
        /// <exception cref="System.NotSupportedException">The encoder does not support directly setting the
        /// data to be encoded.</exception>
        /// <exception cref="System.ArgumentException">The data supplied to the encoder is invalid, either in length
        /// or in content.  The message of the exception gives more information.</exception>
        byte[] Data { get;set;}

        /// <summary>
        /// Gets or sets the data to be encoded by a barcode, as a text string.
        /// </summary>
        /// <value>The text to be encoded.</value>
        /// <remarks>The value returned by this property is equal to the value passed in; it might not exactly represent
        /// the value encoded by the barcode. (For instance, in ISBN barcodes, dashes used to separate the ISBN are not encoded
        /// but will be retained and returned by this property.)</remarks>
        /// <exception cref="System.NotSupportedException">The encoder does not support directly setting the data
        /// as a text string.</exception>
        /// <exception cref="System.ArgumentException">The data supplied to the encoder is invalid, either in length
        /// or in content.  The message of the exception gives more information.</exception>
        string Text { get;set;}

        /// <summary>
        /// Flags that represent the encoder's capabilities.
        /// </summary>
        /// <value>Flags that represent the encoder's capabilities.</value>
        BarcodeEncoderFlags Flags { get;}

        /// <summary>
        /// Symbols that are encodable by this encoder.
        /// </summary>
        /// <value>The valid characters that may be encoded.</value>
        /// <remarks>This returns a string containing all of the valid characters (or characters that might be encoded) appear in 
        /// a string passed to <see cref="Barcodes.IBarcodeEncoder.Text"/>.  This does not mean that any arrangement of such characters are
        /// necessarily valid, or that other characters will cause an encoding error. (For instance, in <see cref="Barcodes.ISBNEncoder"/>,
        /// the valid characters are <c>"0123456789xX"</c>.  However, passing in an ISBN with dashes in it will not cause
        /// an error; the dashes are simply ignored.)  The behavior of a given encoder will be described in the documentation of 
        /// that encoder.  This property is primarily for reference.
        /// This property only returns a value if the <see cref="Barcodes.BarcodeEncoderFlags.Text"/> flag is set in <see cref="Barcodes.IBarcodeEncoder.Flags"/>.
        /// </remarks>
        string TextSymbols { get;}

        /// <summary>
        /// Returns the generator being used by this encoder.
        /// </summary>
        /// <value>The generator being used by this encoder.</value>
        /// <remarks>This property might return <c>null</c> if the encoder is a composite encoder and the data has not been set.  The
        /// <see cref="Barcodes.IBarcodeGenerator"/> returned should not be saved, as it might change if the data to be encoded is changed.</remarks>
        IBarcodeGenerator Generator { get;}

        /// <summary>
        /// Returns the sizer being used by this encoder.
        /// </summary>
        /// <value>The sizer for the generator being used by this encoder.</value>
        /// <remarks>This property might return <c>null</c> if the encoder is a composite encoder and the data has not been set.  The
        /// <see cref="Barcodes.IBarcodeSizer"/> returned should not be saved, as it might change if the data to be encoded is changed.</remarks>
        IBarcodeSizer Sizer { get;}
    }

    /// <summary>
    /// Interface to the sizing controls of a barcode generator.
    /// </summary>
    /// <remarks>This interface contains two properties that directly affect the size of a generated barcode: they are
    /// <see cref="Barcodes.IBarcodeSizer.DPI"/> and <see cref="Barcodes.IBarcodeSizer.Mode"/>.</remarks>
    /// <seealso cref="Barcodes.IBarcodeModularSizer"/>
    public interface IBarcodeSizer {
        /// <summary>
        /// The width of the barcode.
        /// </summary>
        /// <value>The width of the barcode, in pixels.</value>
        /// <remarks>This is a fixed value, in the sense that any generated barcode must have a width equal to this.
        /// The width can be modified by changing the value of <see cref="Barcodes.IBarcodeSizer.DPI"/>, 
        /// <see cref="Barcodes.IBarcodeSizer.Mode"/>, and in the case of sizers implementing <see cref="Barcodes.IBarcodeModularSizer"/>,
        /// <see cref="Barcodes.IBarcodeModularSizer.Module"/>.</remarks>
        int Width { get;}

        /// <summary>
        /// The height of the barcode.
        /// </summary>
        /// <value>The height of the barcode, in pixels.</value>
        /// <remarks>This typically represents the minimum height; a size passed to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>
        /// may (in most cases) have any height greater than or equal to this value.  The height may be constrained by aspect ratio 
        /// requirements. If the sizer's generator's <see cref="Barcodes.IBarcodeGenerator.Flags"/>
        /// value includes the <see cref="Barcodes.BarcodeGeneratorFlags.HeightDependant"/> flag, this value is the one that will be used
        /// by <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, regardless of the value passed in.</remarks>
        int Height { get;}

        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>The height of all items added to the barcode, in pixels.</value>
        /// <remarks><para>This property is meant to be used to generate a desired barcode height.  To calculate the height
        /// of the size to pass to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, multiply the desired height
        /// (in inches) by the DPI, and add this number to it.  This is also an important number in aspect ratio 
        /// calculations; this number is subtracted from the barcode's specified height before the aspect ratio is
        /// checked, since the aspect ratio is meant for the barcode itself, not the extra text and guards that 
        /// surround it.</para><para>This property's value is potentially impacted by the value of <see cref="Barcodes.IBarcodeSizer.Mode"/>.
        /// The barcode render mode flags should be set before using this value.</para></remarks>
        int ExtraHeight { get;}

        /// <summary>
        /// The current size of the barcode.
        /// </summary>
        /// <value>The current size of the barcode.</value>
        /// <remarks>This varies given the current <see cref="Barcodes.IBarcodeSizer.DPI"/>, <see cref="Barcodes.IBarcodeSizer.Mode"/>,
        /// and (if implemented) <see cref="Barcodes.IBarcodeModularSizer.Module"/> values.  If the sizer's generator's <see cref="Barcodes.IBarcodeGenerator.Flags"/>
        /// value includes the <see cref="Barcodes.BarcodeGeneratorFlags.FixedDimensions"/> flag, this is the only valid size that may be
        /// passed to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>.
        /// </remarks>
        Size Size { get;}

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
        /// <exception cref="System.ArgumentException">When setting the DPI, the value specified was less than zero, or, in the case of
        /// barcodes with strict dimensional requirements (such as Postnet), the requirements cannot be met with the specified DPI.</exception>
        float DPI { get;set;}

        /// <summary>
        /// The minimum ratio of height over width.
        /// </summary>
        /// <value>This is the minimum aspect ratio of height over width, or zero if there is no minimum.</value>
        float AspectRatioMin { get;}

        /// <summary>
        /// The maximum ratio of height over width.
        /// </summary>
        /// <value>This is the maximum aspect ratio of height over width, or zero if there is no maximum.</value>
        float AspectRatioMax { get;}

        /// <summary>
        /// The rendering mode flags, which control the way the barcode is rendered.
        /// </summary>
        /// <value>The flags which control which aspects of the barcode are rendered.</value>
        /// <remarks>The particular flags available depends on the barcode being generated.  Any flags that are not available
        /// to a given barcode are ignored.  Some barcode encoders may set default flags, depending on the standard.  Some
        /// generators may set extra flags if certain flags are specified.</remarks>
        BarcodeRenderMode Mode { get; set;}

        /// <summary>
        /// Checks to see if a specified size is valid.
        /// </summary>
        /// <param name="size">A size to test for validity.</param>
        /// <returns>True if this size may be passed to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, false otherwise.</returns>
        bool IsValidSize(Size size);

        /// <summary>
        /// Given a size, returns the largest valid size contained by that size.
        /// </summary>
        /// <param name="size">A maximum size, from which to find a valid size.</param>
        /// <returns>A valid size which may be passed to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>.</returns>
        /// <exception cref="System.ArgumentException">The specified size is smaller than the minimum size in one or both dimensions.</exception>
        Size GetValidSize(Size size);
    }

    /// <summary>
    /// Interface to the sizing controls of a modular barcode generator.  This interface includes a property to set modular width.
    /// </summary>
    /// <remarks>This interface contains three properties that directly affect the size of a generated barcode: they are
    /// <see cref="Barcodes.IBarcodeSizer.DPI"/>, <see cref="Barcodes.IBarcodeModularSizer.Module"/> and 
    /// <see cref="Barcodes.IBarcodeSizer.Mode"/>.</remarks>
    /// <seealso cref="Barcodes.IBarcodeSizer"/>
    public interface IBarcodeModularSizer : IBarcodeSizer {
        /// <summary>
        /// Gets and sets module width of the barcode (in mils).
        /// </summary>
        /// <value>The module width of the barcode, in mils (1 mil is 1/1000 of an inch, or .0254 mm).</value>
        /// <remarks>In a modular barcode, the module width is the width of the narrowest bar.  The widths of spaces,
        /// wide bars, wide spaces, quiet zones, etc. are multiples of this module width.  Changing the module width of a barcode
        /// changes the total width of the barcode, as returned by <see cref="Barcodes.IBarcodeSizer.Width"/>.</remarks>
        /// <exception cref="System.ArgumentException">The specified module width is less than <see cref="Barcodes.IBarcodeModularSizer.MinimumModule"/>.</exception>
        float Module { get;set;}

        /// <summary>
        /// The minimum module width of the barcode.
        /// </summary>
        /// <value>The minimum module width of the barcode, in mils (1 mil is 1/1000 of an inch, or .0254 mm).</value>
        /// <remarks>The value of <see cref="Barcodes.IBarcodeModularSizer.Module"/> cannot be set lower than the value
        /// returned by this property.</remarks>
        float MinimumModule { get;}
    }

    /// <summary>
    /// Interface to the barcode generator.  This interface contains the <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/> method,
    /// which generates the actual barcode.
    /// </summary>
    public interface IBarcodeGenerator {
        /// <summary>
        /// Gets the sizing controls for this barcode generator.
        /// </summary>
        /// <value>An object which controls the barcode's sizing.</value>
        /// <remarks>The returned object might also implement the <see cref="Barcodes.IBarcodeModularSizer"/>, if 
        /// the barcode generator generates a modular barcode.</remarks>
        IBarcodeSizer Sizer { get;}

        /// <summary>
        /// Gets the flags that represent the capabilities of the generator and its corresponding sizer.
        /// </summary>
        /// <value>Flags representing the generator's (and sizer's) capabilities.</value>
        BarcodeGeneratorFlags Flags { get;}

        /// <summary>
        /// Generates a barcode of a specified size, using the data that has been set previously by its corresponding
        /// encoder.
        /// </summary>
        /// <param name="size">The size of the barcode to return.</param>
        /// <returns>A bitmap of the barcode, of the specified size.</returns>
        /// <exception cref="System.ArgumentException">The specified size is invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The data that is to be encoded has not been set yet.</exception>
        Bitmap GenerateBarcode(Size size);
    }

    //Abstract Implementations
    /// <summary>
    /// A base implementation of the <see cref="Barcodes.IBarcodeEncoder"/> interface. The base class for most
    /// of the encoders in this library.
    /// </summary>
    public abstract class BarcodeEncoder : IBarcodeEncoder {
        private BarcodeGenerator generator;

        public delegate void CheckAndEncodeText(string value, out byte[] encoded, ref byte[] data);

        /// <summary>
        /// Constructs a BarcodeEncoder.
        /// </summary>
        /// <param name="generator">The generator that this encoder uses.</param>
        protected BarcodeEncoder(BarcodeGenerator generator) {
            this.generator = generator;
        }

        /// <summary>
        /// Gets or sets the data to be encoded into a barcode.
        /// </summary>
        /// <value>A byte array of data to be encoded.</value>
        /// <remarks>Not all encoders are capable of accepting byte arrays to be encoded.  This property will
        /// store a copy of the byte array specified.  The actual data encoded into the barcode might vary from
        /// the data provided, depending on the interpretation of the data provided, but the value returned
        /// by this property is the original byte array that it was set to.<br/>The default implementation of this
        /// property is to throw a <see cref="System.NotSupportedException"/>.  Derived classes must override
        /// this property if they support setting data to be encoded via byte array.</remarks>
        /// <exception cref="System.NotSupportedException">The encoder does not support setting the data
        /// to be encoded by byte array.</exception>
        /// <exception cref="System.ArgumentException">The data to be encoded is invalid, either by length
        /// or content.</exception>
        public virtual byte[] Data {
            get {
                throw new NotSupportedException("This encoder does not support encoding by byte array.");
            }
            set {
                throw new NotSupportedException("This encoder does not support encoding by byte array.");
            }
        }

        /// <summary>
        /// Gets the data sent to the generator, to be converted into a barcode.
        /// </summary>
        /// <value>A byte array of the data used by the generator, or null.</value>
        /// <remarks>This is used primarily for debugging purposes.  The default behavior of this property
        /// is to return the value of the <see cref="Barcodes.BarcodeGenerator.Data"/> property of the
        /// generator specified in the constructor, or set by <see cref="Barcodes.BarcodeEncoder.GeneratorInstance"/>, 
        /// or null if the generator has not been set.
        /// </remarks>
        public virtual byte[] EncodedData { get { if (generator == null) return null; else return generator.Data; } }

        /// <summary>
        /// Gets or sets the text to be encoded into a barcode.
        /// </summary>
        /// <value>A string of the data to be encoded.</value>
        /// <remarks>Not all encoders are capable of accepting strings to be encoded.  This property will
        /// store a copy of the string specified.  The actual data encoded into the barcode might vary from
        /// the data provided, depending on the interpretation of the data provided, but the value returned
        /// by this property is the original string that it was set to.<br/>The default implementation of this
        /// property is to throw a <see cref="System.NotSupportedException"/>.  Derived classes must override
        /// this property if they support setting data to be encoded via string.</remarks>
        /// <exception cref="System.NotSupportedException">The encoder does not support setting the data
        /// to be encoded by string.</exception>
        /// <exception cref="System.ArgumentException">The data to be encoded is invalid, either by length
        /// or content.</exception>
        public virtual string Text {
            get {
                throw new NotSupportedException("This encoder does not support encoding by string.");
            }
            set {
                throw new NotSupportedException("This encoder does not support encoding by string.");
            }
        }

        /// <summary>
        /// Gets flags representing the capabilities of the encoder, as described in <see cref="Barcodes.BarcodeEncoderFlags"/>.
        /// </summary>
        /// <value>Flags representing the capabilities of the encoder.</value>
        public abstract BarcodeEncoderFlags Flags { get;}

        /// <summary>
        /// Gets a string containing the symbols that can be encoded.
        /// </summary>
        /// <value>A string containing the symbols that can be encoded, or null.</value>
        /// <remarks>This return a string of encodable symbols, if the <see cref="Barcodes.BarcodeEncoderFlags.Text"/> flag
        /// is set, otherwise it returns null.  This property only acts as a guide, because the presence of symbols
        /// that are not in this string, does not necessarily cause encoding to fail.<br/>The default behavior is to return
        /// null.</remarks>
        public virtual string TextSymbols { get { return null; } }

        /// <summary>
        /// Gets the generator associated with this encoder.
        /// </summary>
        /// <value>The generator associated with this encoder, or null if a generator has not been set.</value>
        public IBarcodeGenerator Generator { get { return generator; } }

        /// <summary>
        /// Gets or sets the generator associated with this encoder.
        /// </summary>
        /// <value>The generator associated with this encoder.</value>
        /// <remarks>This property differs from <see cref="Barcodes.BarcodeEncoder.Generator"/> because this both
        /// allows the generator to be set, and returns the generator as an instance of <see cref="Barcodes.BarcodeGenerator"/>
        /// instead of an instance of <see cref="Barcodes.IBarcodeGenerator"/>. It is for internal use by encoders that
        /// change their generator depending on their data (composite encoders).</remarks>
        protected BarcodeGenerator GeneratorInstance { get { return generator; } set { generator = value; } }

        /// <summary>
        /// Gets the sizer associated with the generator, associated with this encoder.
        /// </summary>
        /// <value>The sizer associated with the generator associated with this encoder, or null if a generator has not been set.</value>
        public IBarcodeSizer Sizer {
            get {
                if (generator == null)
                    return null;
                else
                    return generator.Sizer;
            }
        }
        
        public void SetTextPropertyValue(
            string value, 
            out string text, 
            ref byte[] data, 
            CheckAndEncodeText checkAndEncodeText)
        {
            var encoded = default(byte[]);
            checkAndEncodeText(value, out encoded, ref data);
            text = value;
            GeneratorInstance.Data = encoded;
        }
    }

    /// <summary>
    /// A base implementation of the <see cref="Barcodes.IBarcodeSizer"/> interface.  This class provides a base
    /// for most of the sizer classes used internally in this library.
    /// </summary>
    public abstract class BarcodeSizer : IBarcodeSizer {
        /// <summary>
        /// The width of the barcode.
        /// </summary>
        /// <value>The width of the barcode.</value>
        /// <remarks>This is a fixed value, in the sense that any generated barcode must have a width equal to this.
        /// The width can be modified by changing the value of <see cref="Barcodes.IBarcodeSizer.DPI"/>, 
        /// <see cref="Barcodes.IBarcodeSizer.Mode"/>, and in the case of sizers implementing <see cref="Barcodes.IBarcodeModularSizer"/>,
        /// <see cref="Barcodes.IBarcodeModularSizer.Module"/>.</remarks>
        public abstract int Width { get;}

        /// <summary>
        /// The height of the barcode.
        /// </summary>
        /// <value>The height of the barcode.</value>
        /// <remarks>This typically represents the minimum height; a size passed to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>
        /// may (in most cases) have any height greater than or equal to this value.  The height may be constrained by aspect ratio 
        /// requirements. If the sizer's generator's <see cref="Barcodes.IBarcodeGenerator.Flags"/>
        /// value includes the <see cref="Barcodes.BarcodeGeneratorFlags.HeightDependant"/> flag, this value is the one that will be used
        /// by <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, regardless of the value passed in.</remarks>
        public abstract int Height { get;}

        /// <summary>
        /// The height of all items added to the barcode.
        /// </summary>
        /// <value>The height of all items added to the barcode, in pixels.</value>
        /// <remarks><para>This property is meant to be used to generate a desired barcode height.  To calculate the height
        /// of the size to pass to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, multiply the desired height
        /// (in inches) by the DPI, and add this number to it.  This is also an important number in aspect ratio 
        /// calculations; this number is subtracted from the barcode's specified height before the aspect ratio is
        /// checked, since the aspect ratio is meant for the barcode itself, not the extra text and guards that 
        /// surround it.</para><para>This property's value is potentially impacted by the value of <see cref="Barcodes.IBarcodeSizer.Mode"/>.
        /// The barcode render mode flags should be set before using this value.</para></remarks>
        public abstract int ExtraHeight { get;}

        /// <summary>
        /// The current size of the barcode.
        /// </summary>
        /// <value>The current size of the barcode, from <see cref="Barcodes.BarcodeSizer.Width"/> and <see cref="Barcodes.BarcodeSizer.Height"/>.</value>
        /// <remarks>This varies given the current <see cref="Barcodes.IBarcodeSizer.DPI"/>, <see cref="Barcodes.IBarcodeSizer.Mode"/>,
        /// and (if implemented) <see cref="Barcodes.IBarcodeModularSizer.Module"/> values.  If the sizer's generator's <see cref="Barcodes.IBarcodeGenerator.Flags"/>
        /// value includes the <see cref="Barcodes.BarcodeGeneratorFlags.FixedDimensions"/> flag, this is the only valid size that may be
        /// passed to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>.  The return value of this property is derived
        /// from <see cref="Barcodes.BarcodeSizer.Width"/> and <see cref="Barcodes.BarcodeSizer.Height"/>.
        /// </remarks>
        public Size Size { get { return new Size(Width, Height); } }

        private float dpi = 0;
        /// <summary>
        /// The current DPI (dots-per-inch) of the barcode.
        /// </summary>
        /// <value>The current DPI (dots-per-inch) of the barcode.</value>
        /// <remarks>
        /// This value represents the DPI that the barcode will be printed at.  This value defaults to zero, which represents "logical" mode.
        /// In "logical" mode, the generated barcode represents the relative sizing and positioning of barcode elements, but they are not
        /// necessarily in the proper size for printing.
        /// <br/>
        /// If the value is greater than zero, the barcode generator will generate a barcode using the specified DPI.
        /// <br/>
        /// Derived classes can override the DPI property to capture changes to the DPI, in order to recalculate sizing information.
        /// </remarks>
        /// <exception cref="System.ArgumentException">When setting the DPI, the value specified was less than zero, or, in the case of
        /// barcodes with strict dimensional requirements (such as Postnet), the requirements cannot be met with the specified DPI.</exception>
        public virtual float DPI {
            get {
                return dpi;
            }
            set {
                if (value < 0)
                    throw new ArgumentException("The specified DPI is less than zero.");
                dpi = value;
            }
        }

        /// <summary>
        /// The minimum ratio of height over width.
        /// </summary>
        /// <value>This is the minimum aspect ratio of height over width, or zero if there is no minimum.</value>
        /// <remarks>The default return value is zero.</remarks>
        public virtual float AspectRatioMin { get { return 0; } }

        /// <summary>
        /// The maximum ratio of height over width.
        /// </summary>
        /// <value>This is the maximum aspect ratio of height over width, or zero if there is no maximum.</value>
        /// <remarks>The default return value is zero.</remarks>
        public virtual float AspectRatioMax { get { return 0; } }

        private BarcodeRenderMode mode=BarcodeRenderMode.None;
        /// <summary>
        /// The rendering mode flags, which control the way the barcode is rendered.
        /// </summary>
        /// <value>The flags which control which aspects of the barcode are rendered.</value>
        /// <remarks>The particular flags available depends on the barcode being generated.  Any flags that are not available
        /// to a given barcode are ignored.  Some barcode encoders may set default flags, depending on the standard.  Some
        /// generators may set extra flags if certain flags are specified.  The default value is <see cref="Barcodes.BarcodeRenderMode.None"/>.</remarks>
        public virtual BarcodeRenderMode Mode {
            get {
                return mode;
            }
            set {
                mode = value;
            }
        }

        /// <summary>
        /// Checks to see if a specified size is valid.
        /// </summary>
        /// <param name="size">A size to test for validity.</param>
        /// <returns>True if this size may be passed to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>, false otherwise.</returns>
        public abstract bool IsValidSize(Size size);

        /// <summary>
        /// Given a size, returns the largest valid size contained by that size.
        /// </summary>
        /// <param name="size">A maximum size, from which to find a valid size.</param>
        /// <returns>A valid size which may be passed to <see cref="Barcodes.IBarcodeGenerator.GenerateBarcode"/>.</returns>
        /// <exception cref="System.ArgumentException">The specified size is smaller than the minimum size in one or both dimensions.</exception>
        public abstract Size GetValidSize(Size size);
    }
    
    /// <summary>
    /// A base implementation of the <see cref="Barcodes.IBarcodeGenerator"/> interface.  This class provides the base
    /// for most of the generators in this library.
    /// </summary>
    public abstract class BarcodeGenerator : IBarcodeGenerator {
        private IBarcodeSizer sizer;
        /// <summary>
        /// Returns the sizer used by this generator.
        /// </summary>
        /// <value>The sizer used by this generator.</value>
        public IBarcodeSizer Sizer { get { return sizer; } }

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
        public abstract byte[] Data { get; set;}

        /// <summary>
        /// Gets the flags representing the capabilities of the generator and corresponding sizer.
        /// </summary>
        /// <value>The capability flags of the generator and sizer.</value>
        public abstract BarcodeGeneratorFlags Flags { get;}

        /// <summary>
        /// Generates a bitmap of the barcode, of the specified size and data.
        /// </summary>
        /// <param name="size">The size of barcode to generate.</param>
        /// <returns>A bitmap of the generated barcode.</returns>
        /// <exception cref="System.ArgumentException">The specified size is invalid.</exception>
        /// <exception cref="System.InvalidOperationException">The data that is to be encoded has not been set yet.</exception>
        public abstract Bitmap GenerateBarcode(Size size);

        /// <summary>
        /// Constructs a new instance of BarcodeGenerator.
        /// </summary>
        /// <param name="sizer">The sizer to use.</param>
        protected BarcodeGenerator(IBarcodeSizer sizer) {
            this.sizer = sizer;
        }
    }
}
