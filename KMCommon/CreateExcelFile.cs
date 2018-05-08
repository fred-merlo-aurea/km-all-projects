using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace KM.Common
{

    public class CreateExcelFile
    {
        public static bool CreateExcelDocument<T>(List<T> list, string xlsxFilePath)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ListToDataTable(list));

            return CreateExcelDocument(ds, xlsxFilePath);
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static bool CreateExcelDocument(DataTable dt, string xlsxFilePath)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            dt1 = dt.Copy();
            ds.Tables.Add(dt1);

            return CreateExcelDocument(ds, xlsxFilePath);
        }

        public static bool CreateExcelDocument(DataSet ds, string excelFilename)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
                {
                    CreateParts(ds, document);
                }
                Trace.WriteLine("Successfully created: " + excelFilename);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

        private static void CreateParts(DataSet ds, SpreadsheetDocument document)
        {
            WorkbookPart workbookPart = document.AddWorkbookPart();
            Workbook workbook = new Workbook();
            workbookPart.Workbook = workbook;

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            Stylesheet stylesheet = new Stylesheet();
            workbookStylesPart.Stylesheet = stylesheet;

            Sheets sheets = new Sheets();

            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                string workSheetID = "rId" + worksheetNumber.ToString();
                string worksheetName = dt.TableName;

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>(workSheetID);
                WriteDataTableToExcelWorksheet(dt, worksheetPart);

                Sheet sheet = new Sheet() { Name = worksheetName, SheetId = (UInt32Value)worksheetNumber, Id = workSheetID };
                sheets.Append(sheet);

                worksheetNumber++;
            }

            workbook.Append(sheets);
        }

        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart1)
        {
            Worksheet worksheet = new Worksheet();
            SheetViews sheetViews = new SheetViews();

            SheetView sheetView = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
            sheetViews.Append(sheetView);

            SheetData sheetData1 = new SheetData();
            string cellValue = "";

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            int rowIndex = 1;
            Row row1 = new Row() { RowIndex = (UInt32Value)1U };
            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];

                AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, row1);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal");     //  eg "System.String" or "System.Decimal"
            }
            sheetData1.Append(row1);


            //
            //  Now, step through each row of data in our DataTable...
            //
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                Row newExcelRow = new Row() { RowIndex = (UInt32Value)(uint)rowIndex };

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();

                    // Create cell with data
                    if (IsNumericColumn[colInx] && !string.IsNullOrEmpty(cellValue))
                        AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                    else
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                }
                sheetData1.Append(newExcelRow);
            }

            worksheet.Append(sheetViews);
            worksheet.Append(sheetData1);

            worksheetPart1.Worksheet = worksheet;
        }

        private static void AppendTextCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static void AppendNumericCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell() { CellReference = cellReference };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //  Each Excel cell we write must have the cell name stored with it.
            //
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }
    }
}
