using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using System.Data;

namespace Core_AMS.Utilities
{
    public class ExcelFunctions
    {
        private ThemableColor borderColor;
        public ThemableColor BorderColor
        {
            get
            {
                if (borderColor == null)
                    return new ThemableColor(Colors.Black);
                else
                    return borderColor;
            }
            set
            {
                borderColor = value;
            }
        }
        #region borders
        #region medium
        public CellBorders LeftTopRightBottomBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.Medium, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        public CellBorders LeftTopRightBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.None, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        public CellBorders LeftTopBottomBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.Medium, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        public CellBorders LeftRightBottomBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.Medium, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        public CellBorders LeftRightBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.None, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        public CellBorders BottomBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.Medium, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }

        public CellBorders LeftBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.None, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        public CellBorders RightBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.None, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }

        public CellBorders LeftBottomBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.Medium, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }

        public CellBorders RightBottomBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.Medium, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        #endregion
        #region Thin
        public CellBorders LeftTopBottom_ThinRight()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.Thin, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.Medium, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        public CellBorders TopBottom_ThinLeftThinRight()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Thin, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.Thin, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.Medium, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        public CellBorders TopRightBottom_ThinLeft()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.Thin, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.Medium, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.Medium, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        #endregion
        #region Thick
        public CellBorders ThickBottom()
        {
            CellBorders cb = new CellBorders(
                   new CellBorder(CellBorderStyle.None, BorderColor),   // Left border
                   new CellBorder(CellBorderStyle.None, BorderColor),   // Top border
                   new CellBorder(CellBorderStyle.None, BorderColor),   // Right border
                   new CellBorder(CellBorderStyle.Thick, BorderColor), // Bottom border
                   new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                   new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                   new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                   new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        #endregion
        #region Dash
        public CellBorders DashedLeftTopRightBottomBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.MediumDashed, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.MediumDashed, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.MediumDashed, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.MediumDashed, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        public CellBorders DashedTopBottomBorder()
        {
            CellBorders cb = new CellBorders(
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Left border
                    new CellBorder(CellBorderStyle.MediumDashed, BorderColor),   // Top border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Right border
                    new CellBorder(CellBorderStyle.MediumDashed, BorderColor), // Bottom border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside horizontal border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Inside vertical border
                    new CellBorder(CellBorderStyle.None, BorderColor),   // Diagonal up border
                    new CellBorder(CellBorderStyle.None, BorderColor));  // Diagonal down border
            return cb;
        }
        #endregion
        #endregion
        #region set cell values
        public void SetCellText(CellSelection cs, string text, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            cs.SetValueAsText(text);

        }
        public void SetCellDouble(CellSelection cs, double value, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            if ((value % 1) != 0)
                cs.SetFormat(new CellValueFormat("###,###.##"));
            else
                cs.SetFormat(new CellValueFormat("###,###.00"));
            cs.SetValue(value);
        }
        public void SetCellDoubleDollars(CellSelection cs, double value, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            if ((value % 1) != 0)
                cs.SetFormat(new CellValueFormat("$ ###,###.##"));
            else
                cs.SetFormat(new CellValueFormat("$ ###,###.00"));
            cs.SetValue(value);

        }
        public void SetCellDecimal(CellSelection cs, decimal value, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            if ((value % 1) != 0)
                cs.SetFormat(new CellValueFormat("###,###.##"));
            else
                cs.SetFormat(new CellValueFormat("###,###.00"));
            cs.SetValue((double) value);
        }
        public void SetCellDecimalFour(CellSelection cs, decimal value, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            if ((value % 1) != 0)
                cs.SetFormat(new CellValueFormat("###,###.####"));
            else
                cs.SetFormat(new CellValueFormat("###,###.0000"));
            cs.SetValue((double) value);
        }

        public void SetCellDecimalDollars(CellSelection cs, decimal value, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            try
            {
                if ((value % 1) != 0)
                    cs.SetFormat(new CellValueFormat("$ ###,###.##"));
                else
                    cs.SetFormat(new CellValueFormat("$ ###,###.00"));
            }
            catch (Exception ex)
            {
                string msg = StringFunctions.FormatException(ex);
                cs.SetFormat(new CellValueFormat("$ ###,###.00"));
            }
            cs.SetValue((double) value);

        }
        public void SetCellInt(CellSelection cs, int value, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            cs.SetFormat(new CellValueFormat("###,###"));
            cs.SetValue(value);

        }
        public void SetCellIntWithDecimals(CellSelection cs, int value, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            cs.SetFormat(new CellValueFormat("###,###.00"));
            cs.SetValue(value);

        }
        public void SetCellIntDollars(CellSelection cs, int value, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            cs.SetFormat(new CellValueFormat("$ ###,###"));
            cs.SetValue(value);

        }
        public void SetCellIntDollarsWithDecimals(CellSelection cs, int value, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            cs.SetFormat(new CellValueFormat("$ ###,###.00"));
            cs.SetValue(value);

        }

        public void SetCellNumberWithDecimalsFormula(CellSelection cs, string formula, bool isBold, GradientFill gradientFill = null, RadHorizontalAlignment horAlign = RadHorizontalAlignment.Center, RadVerticalAlignment vertAlign = RadVerticalAlignment.Center, int fontSize = 12)
        {
            cs.Merge();
            cs.SetFontFamily(new ThemableFontFamily("Helvetica"));
            cs.SetFontSize(fontSize);
            cs.SetHorizontalAlignment(horAlign);
            cs.SetVerticalAlignment(vertAlign);
            cs.SetIsBold(isBold);
            if (gradientFill != null)
                cs.SetFill(gradientFill);
            cs.SetFormat(new CellValueFormat("###,###.##"));
            cs.SetValueAsFormula(formula);

        }
        #endregion
        #region Saving
        public void SaveWorkbook(Workbook wb, string fullName)
        {
            Telerik.Windows.Documents.Spreadsheet.FormatProviders.IWorkbookFormatProvider formatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.XlsxFormatProvider();
            using (System.IO.FileStream output = new System.IO.FileStream(fullName, System.IO.FileMode.Create))
            {
                formatProvider.Export(wb, output);
            }
        }
        #endregion
        #region Getting Row/Column by index
        public string GetColumn(int index)
        {
            return ColumnIndex[index];
        }
        public int GetRow(int index)
        {
            return RowIndex[index];
        }
        private Dictionary<int, string> _columnIndex;
        /// <summary>
        /// column index is the Key, column letter is the value
        /// columns A-BZ = 78 columns, Indexes(Key) 0-77
        /// </summary>
        public Dictionary<int, string> ColumnIndex
        {
            get
            {
                if (_columnIndex == null || _columnIndex.Count == 0)
                {
                    _columnIndex = new Dictionary<int, string>();
                    _columnIndex.Add(0, "A");
                    _columnIndex.Add(1, "B");
                    _columnIndex.Add(2, "C");
                    _columnIndex.Add(3, "D");
                    _columnIndex.Add(4, "E");
                    _columnIndex.Add(5, "F");
                    _columnIndex.Add(6, "G");
                    _columnIndex.Add(7, "H");
                    _columnIndex.Add(8, "I");
                    _columnIndex.Add(9, "J");
                    _columnIndex.Add(10, "K");
                    _columnIndex.Add(11, "L");
                    _columnIndex.Add(12, "M");
                    _columnIndex.Add(13, "N");
                    _columnIndex.Add(14, "O");
                    _columnIndex.Add(15, "P");
                    _columnIndex.Add(16, "Q");
                    _columnIndex.Add(17, "R");
                    _columnIndex.Add(18, "S");
                    _columnIndex.Add(19, "T");
                    _columnIndex.Add(20, "U");
                    _columnIndex.Add(21, "V");
                    _columnIndex.Add(22, "W");
                    _columnIndex.Add(23, "X");
                    _columnIndex.Add(24, "Y");
                    _columnIndex.Add(25, "Z");
                    _columnIndex.Add(26, "AA");
                    _columnIndex.Add(27, "AB");
                    _columnIndex.Add(28, "AC");
                    _columnIndex.Add(29, "AD");
                    _columnIndex.Add(30, "AE");
                    _columnIndex.Add(31, "AF");
                    _columnIndex.Add(32, "AG");
                    _columnIndex.Add(33, "AH");
                    _columnIndex.Add(34, "AI");
                    _columnIndex.Add(35, "AJ");
                    _columnIndex.Add(36, "AK");
                    _columnIndex.Add(37, "AL");
                    _columnIndex.Add(38, "AM");
                    _columnIndex.Add(39, "AN");
                    _columnIndex.Add(40, "AO");
                    _columnIndex.Add(41, "AP");
                    _columnIndex.Add(42, "AQ");
                    _columnIndex.Add(43, "AR");
                    _columnIndex.Add(44, "AS");
                    _columnIndex.Add(45, "AT");
                    _columnIndex.Add(46, "AU");
                    _columnIndex.Add(47, "AV");
                    _columnIndex.Add(48, "AW");
                    _columnIndex.Add(49, "AX");
                    _columnIndex.Add(50, "AY");
                    _columnIndex.Add(51, "AZ");
                    _columnIndex.Add(52, "BA");
                    _columnIndex.Add(53, "BB");
                    _columnIndex.Add(54, "BC");
                    _columnIndex.Add(55, "BD");
                    _columnIndex.Add(56, "BE");
                    _columnIndex.Add(57, "BF");
                    _columnIndex.Add(58, "BG");
                    _columnIndex.Add(59, "BH");
                    _columnIndex.Add(60, "BI");
                    _columnIndex.Add(61, "BJ");
                    _columnIndex.Add(62, "BK");
                    _columnIndex.Add(63, "BL");
                    _columnIndex.Add(64, "BM");
                    _columnIndex.Add(65, "BN");
                    _columnIndex.Add(66, "BO");
                    _columnIndex.Add(67, "BP");
                    _columnIndex.Add(68, "BQ");
                    _columnIndex.Add(69, "BR");
                    _columnIndex.Add(70, "BS");
                    _columnIndex.Add(71, "BT");
                    _columnIndex.Add(72, "BU");
                    _columnIndex.Add(73, "BV");
                    _columnIndex.Add(74, "BW");
                    _columnIndex.Add(75, "BX");
                    _columnIndex.Add(76, "BY");
                    _columnIndex.Add(77, "BZ");

                    return _columnIndex;
                }
                else
                    return _columnIndex;
            }
        }
        private Dictionary<int, int> _rowIndex;
        /// <summary>
        /// row index is the Key, row number is the value
        /// rows 1-100, Indexes(Key) 0-99
        /// </summary>
        public Dictionary<int, int> RowIndex
        {
            get
            {
                if (_rowIndex == null || _rowIndex.Count == 0)
                {
                    _rowIndex = new Dictionary<int, int>();
                    int row = 1;
                    int index = 0;
                    int counter = 0;
                    while (counter <= 99)
                    {
                        _rowIndex.Add(index + counter, row + counter);
                        counter++;
                    }
                    return _rowIndex;
                }
                else
                    return _rowIndex;
            }
        }
        #endregion
        #region Get Worksheet
        public Worksheet GetWorksheet(DataTable data, string sheetName = "")
        {
            borderColor = new ThemableColor(Colors.Black);

            var wb = new Workbook();
            if (!string.IsNullOrEmpty(sheetName))
                wb.Name = sheetName;
            wb.SuspendLayoutUpdate();
            wb.History.IsEnabled = false;
            Worksheet ws = wb.Worksheets.Add();
            if (!string.IsNullOrEmpty(sheetName))
                ws.Name = sheetName;
            ws.Columns.Insert(0, data.Columns.Count);
            ws.DefaultRowHeight = new RowHeight(23, true);
            RowSelection selection = ws.Rows[0, data.Columns.Count - 1];
            selection.Insert();

            int rowIndex = 0;
            foreach (DataRow dr in data.Rows)
            {
                int colIndex = 0;
                foreach (DataColumn dc in data.Columns)
                {
                    CellSelection cs = ws.Cells[rowIndex, colIndex];
                    cs.SetValueAsText(dr[dc.ColumnName].ToString());
                    colIndex++;
                }
                rowIndex++;
            }
            wb.History.IsEnabled = true;
            wb.ResumeLayoutUpdate();
            return ws;
        }
        public Workbook GetWorkbook(DataTable data, string sheetName = "")
        {
            borderColor = new ThemableColor(Colors.Black);

            var wb = new Workbook();
            if (!string.IsNullOrEmpty(sheetName))
                wb.Name = sheetName;
            wb.SuspendLayoutUpdate();
            wb.History.IsEnabled = false;
            Worksheet ws = wb.Worksheets.Add();
            if (!string.IsNullOrEmpty(sheetName))
                ws.Name = sheetName;
            ws.Columns.Insert(0, data.Columns.Count);
            ws.DefaultRowHeight = new RowHeight(23, true);
            RowSelection selection = ws.Rows[0, data.Columns.Count - 1];
            selection.Insert();

            int headerIndex = 0;
            foreach (DataColumn dc in data.Columns)
            {
                try
                {
                    CellSelection cs = ws.Cells[0, headerIndex];
                    cs.SetValueAsText(dc.ColumnName.ToString());
                }
                catch (Exception ex)
                {
                    string msg = StringFunctions.FormatException(ex);
                }
                headerIndex++;
            }

            int rowIndex = 1;
            foreach (DataRow dr in data.Rows)
            {
                try
                {
                    int colIndex = 0;
                    foreach (DataColumn dc in data.Columns)
                    {
                        try
                        {
                            CellSelection cs = ws.Cells[rowIndex, colIndex];
                            cs.SetValueAsText(dr[dc.ColumnName].ToString());
                        }
                        catch (Exception ex)
                        {
                            string msg = StringFunctions.FormatException(ex);
                        }
                        colIndex++;
                    }
                }
                catch (Exception ex)
                {
                    string msg = StringFunctions.FormatException(ex);
                }
                rowIndex++;
            }
            wb.History.IsEnabled = true;
            wb.ResumeLayoutUpdate();
            return wb;
        }
        #endregion
    }
}
