using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace KMPS_JF_Objects.Controls
{
    //    public enum ControlType
    //    {
    //        Verticle,
    //        Horizontal
    //    }

    /// <summary>
    /// The CategorizedCheckBoxList is like a CheckBoxList, but with the ability to categorize the display of items.
    /// </summary>
    [Designer("KMPS_JF_Objects.Controls.CategorizedCheckBoxListControlDesigner"),
    DefaultProperty("DataTable"),
        ToolboxData("<{0}:ccbl runat=server></{0}:ccbl>")]
    public class CategorizedCheckBoxList : System.Web.UI.WebControls.WebControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        /// <summary>
        /// Initializes a new instance of the CategorizedCheckBoxList class.
        /// </summary>
        public CategorizedCheckBoxList()
        {
            // Init the class
        }


        /// <summary>
        /// The DataTable used to build the checkbox list.
        /// </summary>
        protected DataTable dataTable;

        /// <summary>
        /// The DataTable used to build the checkbox list.
        /// </summary>
        [Category("Data"),
        DefaultValue(""),
        Description("The DataTable that will be used to build the checkbox list.")]
        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }


        /// <summary>
        /// An ArrayList of the checkbox values that should be marked as selected. 
        /// The items in the ArrayList should be strings.
        /// </summary>
        protected ArrayList selections = new ArrayList();

        /// <summary>
        /// An ArrayList of the checkbox values that should be marked as selected. 
        /// The items in the ArrayList should be strings.
        /// </summary>
        [Browsable(false)]
        public ArrayList Selections
        {
            get
            {
                // Make sure that we read the post-back values.
                if (HttpContext.Current.Request.Form.Count > 0 && (selections == null || selections.Count == 0))
                {
                    // Get the values that the user submitted
                    ReadPostBack();
                }

                // Return the selections ArrayList
                return selections;
            }
            set
            {
                // Let the consuming page specify the selected values.
                selections = value;
            }
        }


        /// <summary>
        /// The name of the column that will be used for the text that is displayed next to the checkboxes.
        /// </summary>
        protected Boolean autopostback = false;

        /// <summary>
        /// The name of the column that will be used for the text that is displayed next to the checkboxes.
        /// </summary>
        [Category("Data"),
        DefaultValue(""),
        Description("Enabled AutoPostback for checkboxes.")]
        public Boolean AutoPostBack
        {
            get { return autopostback; }
            set { autopostback = value; }
        }

        /// <summary>
        /// The name of the column that will be used for the text that is displayed next to the checkboxes.
        /// </summary>
        protected string dataTextColumn;

        /// <summary>
        /// The name of the column that will be used for the text that is displayed next to the checkboxes.
        /// </summary>
        [Category("Data"),
        DefaultValue(""),
        Description("The name of the column that will be used for the text that is displayed next to the checkboxes.")]
        public string DataTextColumn
        {
            get { return dataTextColumn; }
            set { dataTextColumn = value; }
        }


        /// <summary>
        /// The name of the column that will be used for the value of the checkboxes.
        /// </summary>
        protected string dataValueColumn;

        /// <summary>
        /// The name of the column that will be used for the value of the checkboxes.
        /// </summary>
        [Category("Data"),
        DefaultValue(""),
        Description("The name of the column that will be used for the value of the checkboxes.")]
        public string DataValueColumn
        {
            get { return dataValueColumn; }
            set { dataValueColumn = value; }
        }


        /// <summary>
        /// The name of the column that will be used to group the checkboxes by category.
        /// </summary>
        protected string dataCategoryColumn;

        /// <summary>
        /// The name of the column that will be used to group the checkboxes by category.
        /// </summary>
        [Category("Data"),
        DefaultValue(""),
        Description("The name of the column that will be used to group the checkboxes by category.")]
        public string DataCategoryColumn
        {
            get { return dataCategoryColumn; }
            set { dataCategoryColumn = value; }
        }


        /// <summary>
        /// Optional from consumer. The CSS Class name for the table row containing each item.
        /// </summary>
        protected string rowCssClass;

        /// <summary>
        ///  The CSS Class name for the table row containing each item. Optional.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("The CSS Class name for the table row containing each item.")]
        public string RowCssClass
        {
            get { return rowCssClass; }
            set { rowCssClass = value; }
        }


        /// <summary>
        /// Optional from consumer. Controls the css class for the categories.
        /// </summary>
        protected string categoryCssClass;

        /// <summary>
        ///  Controls the css class for the categories. Optional.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("The CSS Class name for the categories.")]
        public string CategoryCssClass
        {
            get { return categoryCssClass; }
            set { categoryCssClass = value; }
        }


        /// <summary>
        /// Optional from consumer. Controls the css class for each checkbox.
        /// </summary>
        protected string checkBoxCssClass;

        /// <summary>
        /// Controls the css class for each checkbox. Optional.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("The CSS Class name for the checkbox fields.")]
        public string CheckBoxCssClass
        {
            get { return checkBoxCssClass; }
            set { checkBoxCssClass = value; }
        }


        /// <summary>
        /// Optional from consumer. Controls the css class for the text next to each checkbox.
        /// </summary>
        protected string textCssClass;

        /// <summary>
        /// Optional from consumer. The CSS Class name for the text next to each checkbox.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("The CSS Class name for the text next to each checkbox.")]
        public string TextCssClass
        {
            get { return textCssClass; }
            set { textCssClass = value; }
        }


        /// <summary>
        /// Optional from consumer. Determines whether all of the categories and checkboxes will share the same table.
        /// </summary>
        protected bool sharedTable = false;

        /// <summary>
        /// Determines whether all of the categories and checkboxes will share the same table.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("Determines whether all of the categories and checkboxes will share the same table, or if each category will have its own table.")]
        public bool SharedTable
        {
            get { return sharedTable; }
            set { sharedTable = value; }
        }


        /// <summary>
        /// Optional from consumer. Controls the cellpadding for the table.
        /// </summary>
        protected int cellPadding = 0;

        /// <summary>
        /// Optional from consumer. Controls the cellpadding for the table.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("Controls the cellpadding for the table.")]
        public int CellPadding
        {
            get { return cellPadding; }
            set { cellPadding = value; }
        }


        /// <summary>
        /// Optional from consumer. Controls the cellspacing for the table.
        /// </summary>
        protected int cellSpacing = 0;

        /// <summary>
        /// Optional from consumer. Controls the cellspacing for the table.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("Controls the cellspacing for the table.")]
        public int CellSpacing
        {
            get { return cellSpacing; }
            set { cellSpacing = value; }
        }


        /// <summary>
        /// Optional from consumer. The overall width of the table used to layout the control, in pixels, or percent (with % symbol).
        /// </summary>
        protected string tableWidth = string.Empty;

        /// <summary>
        ///  The overall width of the table used to layout the control, in pixels, or percent (with % symbol). Optional.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("The overall width of the table used to layout the control, in pixels, or percent (with % symbol).")]
        public string TableWidth
        {
            get { return tableWidth; }
            set { tableWidth = value; }
        }


        /// <summary>
        /// Optional from consumer. Specifies the css class assigned to the table.
        /// </summary>
        protected string tableCssClass;

        /// <summary>
        /// Optional from consumer. Specifies the css class assigned to the table.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("Specifies the css class to assign to the table.")]
        public string TableCssClass
        {
            get { return tableCssClass; }
            set { tableCssClass = value; }
        }


        /// <summary>
        /// The name (id) assigned to all of the checkboxes in the list.
        /// </summary>
        protected string htmlFieldName;

        /// <summary>
        /// The total number of checkbox items.
        /// </summary>
        protected int totalItems;


        /// <summary>
        /// The total number of rows used to display the items. 
        /// If there were 7 totalItems and 2 columns, the totalRows would be 2 (2 * 7 = 14).
        /// </summary>
        protected int totalRows; // TODO: Come back and check this. The above formula is WRONG!
        //		public int TotalRows
        //		{
        //			get{return totalRows;}
        //		}


        /// <summary>
        /// The number of columns that we are to use to layout the fields.
        /// </summary>
        protected int columns = 2;

        /// <summary>
        /// The number of columns that we are to use to layout the fields.
        /// </summary>
        [Category("Appearance"),
        DefaultValue(""),
        Description("The number of columns that will be used to layout the checkbox items.")]
        public int Columns
        {
            get { return columns; }
            set { columns = value; }
        }


        /// <summary>
        /// Used to store the HTML for the table tag for the layout.
        /// </summary>
        protected string tableTag;


        /// <summary> 
        /// <c>Render</c> writes out the HTML needed to render this control.
        /// </summary>
        /// <param name="output">The HTML text writer the we will utilize. 
        /// This is passed to the control automatically, by the host ASPX page.</param>
        protected override void Render(HtmlTextWriter output)
        {
            try
            {
                // No need for ViewState
                this.EnableViewState = false;

                // Retrieve a list of the values that were selected, if the form has already been submitted.
                ReadPostBack();

                // Should the control be visible?
                if (this.Visible == true)
                {
                    // Yes. Render the html.
                    //BuildCategorizedCheckBoxList(output);
                    BuildMultiColumnsCategorizedCheckBoxList(output);
                }
            }
            catch (Exception ex)
            {
                // Something bad happened. Let's tell the user what that was.
                output.Write("Error building CategorizedCheckBoxList:<br>");
                output.Write(ex.Message);
            }
        }

        protected void BuildMultiColumnsCategorizedCheckBoxList(HtmlTextWriter output)
        {
            // Do we have any data?
            if (dataTable == null || dataTable.Rows.Count < 1)
            {
                // There is no data, so there's nothing to render.
                return;
            }

            // First retrieve the column indexes of the specified columns.
            // Later, we'll get the values that we need using these indexes. 
            // This is faster than referencing a column by name.
            int CategoryColumnIndex = -1;
            int TextColumnIndex = -1;
            int ValueColumnIndex = -1;

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataTextColumn.ToLower() == dataTable.Columns[i].ColumnName.ToLower())
                {
                    TextColumnIndex = i;
                }

                if (dataValueColumn.ToLower() == dataTable.Columns[i].ColumnName.ToLower())
                {
                    ValueColumnIndex = i;
                }

                if (dataCategoryColumn.ToLower() == dataTable.Columns[i].ColumnName.ToLower())
                {
                    CategoryColumnIndex = i;
                }
            }

            /**********************************/
            /* Build the html to display of the items */
            /**********************************/
            // If the consuming page wants one single, shared table, write the opening tag, now.             
            if (this.sharedTable == true)
            {
                output.Write(GetTableTag());
            }

            output.Write("<tr>");

            // Create a string for the "previous" category
            string LastCategory = string.Empty;

            // Sort the data by category
            DataView Category = dataTable.DefaultView;
            //Category.Sort = this.dataCategoryColumn;

            // Assemble a distinct list of the categories found in the data
            ArrayList Categories = new ArrayList();
            for (int i = 0; i < Category.Count; i++)
            {
                if (!Categories.Contains(Category[i][CategoryColumnIndex].ToString()))
                    Categories.Add(Category[i][CategoryColumnIndex].ToString());
            }

            totalRows = Categories.Count / Columns;
            if (Categories.Count % Columns > 0)
            {
                totalRows++;
            }

            bool isLastColumn = false;
            bool isLastRow = false;
            int itemsPerRow = (Categories.Count / Columns) + (Categories.Count % Columns);

            // Loop through the categories
            for (int i = 0; i < Categories.Count; i++)
            {
                try
                {
                    /******************************************/
                    // NOTE: Originally, I was generating the dataview in a single step,  
                    // but I found that this caused problems when I used a dataset 
                    // generated from a static xml file read into a dataset.
                    // 
                    //CategoryItems = new DataView(dataTable, String.Format("{0}=\'{1}\'", this.dataCategoryColumn, Categories[i]), this.dataTextColumn, DataViewRowState.OriginalRows);
                    /******************************************/
                    // Get the rows for this category only
                    DataView CategoryItems = new DataView(dataTable);
                    CategoryItems.RowFilter = String.Format("{0}=\'{1}\'", this.dataCategoryColumn, Categories[i].ToString().Replace("\'", "\'\'"));
                    //CategoryItems.Sort = this.dataTextColumn;

                    // If the consuming page wants a separate table for each category, 
                    // write the opening tabel tag for the current category, now.


                    isLastColumn = (i == Columns - 1) || Columns == 1;
                    isLastRow = (i == Categories.Count - 1);

                    output.Write("<td valign=\"top\" style=\"padding-right:5px;\">");

                    if (this.sharedTable == true)
                        output.Write(GetTableTag());

                    // Add the category heading to the html
                    OutputCategoryRow(output, (string)Categories[i]);

                    // Calculate the total number of rows based on the item count and the number of columns
                    totalItems = CategoryItems.Count;
                    // If there was anything left-over as a result of the division, we need to add another row               
                    // Create an integer to hold the index number for the current item

                    int itemcount = 0;

                    for (int j = 0; j < totalItems; j++)
                    {
                        output.Write("<tr>");
                        output.Write("<td>");

                        OutputCheckBox(output,
                             htmlFieldName,
                             CategoryItems[j][TextColumnIndex].ToString().Trim(),
                             CategoryItems[j][ValueColumnIndex].ToString(),
                             IsChecked(CategoryItems[j][ValueColumnIndex].ToString()), itemcount
                             );

                        itemcount++;

                        output.Write("</td>");
                        output.Write("</tr>");
                    }

                    // Table tag
                    if (this.sharedTable == true)
                    {
                        output.Write("</table>");
                    }

                    output.Write("</td>");
                    // Finish the table

                    if (isLastColumn && !isLastRow)
                        output.Write("</tr><tr>");
                    else if (isLastRow)
                        output.Write("</tr>");

                }
                catch { }
            } //end for loop


            if (this.sharedTable == true)
            {
                output.Write("</table>");
            }
            /**********************************/
        }

        /// <summary>
        /// <c>BuildCategorizedCheckBoxList</c> outputs the HTML for the control.
        /// </summary>
        /// <param name="output"></param>
        protected void BuildCategorizedCheckBoxList(HtmlTextWriter output)
        {
            int itemcount = 0;
            // Do we have any data?
            if (dataTable == null || dataTable.Rows.Count < 1)
            {
                // There is no data, so there's nothing to render.
                return;
            }

            // First retrieve the column indexes of the specified columns.
            // Later, we'll get the values that we need using these indexes.
            // This is faster than referencing a column by name.
            int CategoryColumnIndex = -1;
            int TextColumnIndex = -1;
            int ValueColumnIndex = -1;

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataTextColumn.ToLower() == dataTable.Columns[i].ColumnName.ToLower())
                {
                    TextColumnIndex = i;
                }

                if (dataValueColumn.ToLower() == dataTable.Columns[i].ColumnName.ToLower())
                {
                    ValueColumnIndex = i;
                }

                if (dataCategoryColumn.ToLower() == dataTable.Columns[i].ColumnName.ToLower())
                {
                    CategoryColumnIndex = i;
                }
            }

            /**********************************/
            /* Build the html to display of the items */
            /**********************************/
            // If the consuming page wants one single, shared table, write the opening tag, now. 

            this.TableWidth = "100%";
            if (this.sharedTable == true)
            {
                output.Write(GetTableTag());
            }

            // Create a string for the "previous" category
            string LastCategory = string.Empty;


            // Sort the data by category
            DataView Category = dataTable.DefaultView;
            //Category.Sort = this.dataCategoryColumn;

            // Assemble a distinct list of the categories found in the data
            ArrayList Categories = new ArrayList();
            for (int i = 0; i < Category.Count; i++)
            {
                if (!Categories.Contains(Category[i][CategoryColumnIndex].ToString()))
                    Categories.Add(Category[i][CategoryColumnIndex].ToString());
            }

            // Loop through the categories
            for (int i = 0; i < Categories.Count; i++)
            {
                /******************************************/
                // NOTE: Originally, I was generating the dataview in a single step,  
                // but I found that this caused problems when I used a dataset 
                // generated from a static xml file read into a dataset.
                // 
                //CategoryItems = new DataView(dataTable, String.Format("{0}=\'{1}\'", this.dataCategoryColumn, Categories[i]), this.dataTextColumn, DataViewRowState.OriginalRows);
                /******************************************/
                // Get the rows for this category only
                DataView CategoryItems = new DataView(dataTable);
                CategoryItems.RowFilter = String.Format("{0}=\'{1}\'", this.dataCategoryColumn, Categories[i].ToString().Replace("\'", "\'\'"));
                //CategoryItems.Sort = this.dataTextColumn;

                // If the consuming page wants a separate table for each category, 
                // write the opening tabel tag for the current category, now.
                if (this.sharedTable == false)
                {
                    output.Write(GetTableTag());
                }

                // Add the category heading to the html
                OutputCategoryRow(output, (string)Categories[i]);

                // Calculate the total number of rows based on the item count and the number of columns
                totalItems = CategoryItems.Count;
                totalRows = totalItems / columns;

                // If there was anything left-over as a result of the division, we need to add another row
                if (totalItems % columns > 0)
                {
                    totalRows++;
                }

                // Create an integer to hold the index number for the current item
                int CurrentItemIndex = 0;

                // Now loop through the rows
                for (int Row = 0; Row < totalRows; Row++)
                {
                    // Start the row
                    output.Write("\t");
                    output.Write("<tr class=\"");
                    output.Write(this.rowCssClass);
                    output.Write("\">");
                    output.Write("\n");

                    // Column loop
                    for (int Col = 0; Col < columns; Col++)
                    {
                        // Make sure that we haven't hit a blank entry.
                        if (CurrentItemIndex == -1)
                        {
                            // Add an empty cell (two, actually)
                            AddEmptyCells(output);
                        }
                        else
                        {
                            // Now add the checkbox and text
                            OutputCheckBox(output,
                                htmlFieldName,
                                CategoryItems[CurrentItemIndex][TextColumnIndex].ToString(),
                                CategoryItems[CurrentItemIndex][ValueColumnIndex].ToString(),
                                IsChecked(CategoryItems[CurrentItemIndex][ValueColumnIndex].ToString()), itemcount
                                );

                            itemcount++;

                            // If we have more data left, increment the current index counter.
                            if (CurrentItemIndex < (totalItems - 1) && CurrentItemIndex != -1)
                            {
                                // increment the current index
                                CurrentItemIndex++;
                            }
                            else
                            {
                                // We're at the end of the items in the data table.
                                // Set the value of the current index to -1, which our rendering code 
                                // ignores (creates empty table cells)
                                CurrentItemIndex = -1;
                            }
                        }

                        // Add a line break
                        output.Write("\n");
                    }

                    // End the row
                    output.Write("\t");
                    output.Write("</tr>\n");
                }

                // Table tag
                if (this.sharedTable == false)
                {
                    output.Write("</table>\n");
                }
            }

            // Finish the table
            if (this.sharedTable == true)
            {
                output.Write("</table>\n");
            }

            /**********************************/
        }

        /// <summary>
        /// Returns the unique field name that we will assogn to the checkboxes, later. 
        /// </summary>
        protected void GetHtmlFieldName()
        {
            // Pickup the ID assigned to the control in the consuming ASPX page
            htmlFieldName = this.ID;
        }


        /// <summary>
        /// Looks for a match between the current value and the list of selected values.
        /// </summary>
        /// <param name="currentValue">The value that we want to look for.</param>
        /// <returns>True if the current value is contained in the selected list. Otherwise, false.</returns>
        protected bool IsChecked(string currentValue)
        {
            // If we have selections, continue
            if (selections != null && selections.Count > 0)
            {
                // Can we find the current value?
                if (selections.IndexOf(currentValue) > -1)
                {
                    // Yes, so this item should be marked as selected (checked)
                    return true;
                }
                else
                {
                    // No, so this item should not be selected
                    return false;
                }
            }
            else
            {
                // Nothing at all was selected, so return false
                return false;
            }
        }


        /// <summary>
        /// Outputs the html to display a category heading table row.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="category"></param>
        protected void OutputCategoryRow(HtmlTextWriter output, string category)
        {
            // Get rid of any white space on either side of the category name.
            string TrimmedCat = category.Trim();

            if (TrimmedCat != "")
            {
                output.Write("\t");
                output.Write("<tr>");
                output.Write("<td colspan=\"");

                // Remember, there are 2 columns for each item;
                // one for the checkbox, and another for the text.
                output.Write(Columns * 3);

                output.Write("\" class=\"");
                output.Write(this.categoryCssClass);
                output.Write("\">");
                output.Write(TrimmedCat);
                output.Write("</td></tr>");
                output.Write("\n");
            }
        }


        /// <summary>
        /// Outputs the html for an individual checkbox field and label.
        /// This consists of an html table cell &gt;td&lt; for the checkbox, and another for the label.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="val"></param>
        /// <param name="selected"></param>
        protected void OutputCheckBox(HtmlTextWriter output, string id, string text, string val, bool selected, int index)
        {
            output.Write("\t\t");
            output.Write("<td style=\"width:15px;\">");
            output.Write("<input type=\"checkbox\" name=\"");
            output.Write(id);
            output.Write("\" id=\"");
            output.Write(id + "_" + index);
            output.Write("\" value=\"");
            output.Write(val);
            output.Write("\"");

            if (AutoPostBack)
                output.Write(" onclick=\"javascript:setTimeout('__doPostBack(\\'" + id + "_" + index + "\\',\\'\\')', 0)\"");

            if (selected)
            {
                output.Write(" checked");
            }

            output.Write(" class=\"");
            output.Write(this.checkBoxCssClass);
            output.Write("\"");
            output.Write(">");
            output.Write("</td>");

            output.Write("<td align=\"left\" class=\"");
            output.Write(this.textCssClass);
            output.Write("\">");
            output.Write(text);
            output.Write("</td>");
        }


        /// <summary>
        /// Adds html for an empty entry. 
        /// </summary>
        /// <param name="output"></param>
        protected void AddEmptyCells(HtmlTextWriter output)
        {
            output.Write("<td>");
            output.Write("</td>");

            output.Write("<td class=\"");
            output.Write(this.textCssClass);
            output.Write("\">");
            output.Write("</td>");
        }


        /// <summary>
        /// Returns the html needed for the opening table tag for the list of checkboxes.
        /// </summary>
        /// <returns></returns>
        protected string GetTableTag()
        {
            // Do we need to build the table tag?
            if (tableTag == null || tableTag == "")
            {
                // Create a string builder and build the table tag
                StringBuilder Sb = new StringBuilder();
                Sb.Append("\n<table");
                Sb.Append(" cellpadding=\"");
                Sb.Append(this.cellPadding.ToString());
                Sb.Append("\"");
                Sb.Append(" cellspacing=\"");
                Sb.Append(this.cellSpacing.ToString());
                Sb.Append("\"");
                if (tableWidth != string.Empty)
                {
                    Sb.Append(" width=\"");
                    Sb.Append(this.tableWidth);
                    Sb.Append("\"");
                }
                if (this.tableCssClass != null && this.tableCssClass != "")
                {
                    Sb.Append(" class=\"");
                    Sb.Append(this.tableCssClass);
                    Sb.Append("\"");
                }
                Sb.Append(">");
                Sb.Append("\n");

                // Set the tableTag field
                tableTag = Sb.ToString();

                // Clean-up
                Sb = null;
            }

            // Return the tableTag
            return tableTag;
        }


        /// <summary>
        /// Retrieves a list of the checkbox values that were selected (checked), if the form was submitted. 
        /// This is kind of a poor-man's view state implementation. 
        /// But unlike view state, it doesn't add anything to the page weight. 
        /// </summary>
        protected void ReadPostBack()
        {
            // See what field name we are assigning to the checkboxes
            GetHtmlFieldName();

            // Were any checkboxes checked?
            if (HttpContext.Current.Request.Form[htmlFieldName] != null)
            {
                // Since we assigned the same field name to all of the checkboxes,
                // ASP.NET will give us a comma-delimited list of the selections.
                // First, conver the list to a string array.
                string[] Input = HttpContext.Current.Request.Form[htmlFieldName].Split(',');

                // Then, iterate through the array and add each value to our ArrayList.
                for (int i = 0; i < Input.Length; i++)
                {
                    selections.Add(Input[i]);
                }
            }
        }

        /// <summary>
        /// Helper method to arrange the checkbox items in a table with a variable number of columns.
        /// </summary>
        /// <param name="count">The total number of items to display</param>
        /// <returns>Two-dimensional integer array with the item indexes needed for each column in each row. 
        /// The first index (0) is the row, the second (1) is the column. 
        /// This method assignes a value of -1 to any position that should be left empty in the HTML table.</returns>
        public int[,] GetColumnArray(int count)
        {
            // Calculate the total number of rows based on the item count and the number of columns
            totalItems = count;
            totalRows = totalItems / columns;

            // If there was anything left-over as a result of the division, we need to add another row
            if (totalItems % columns > 0)
            {
                totalRows++;
            }

            // Create a two-dimensional array to hold everything
            // The length of the first dimension will be the number rows.
            // The length of the second dimension will be the number columns.
            int[,] Values = new int[totalRows, columns];

            // Loop through each row
            for (int Row = 0; Row < totalRows; Row++)
            {
                // Determine the starting index for this row.
                // This is the same calculation that we would perform to handle paging in a grid.
                int Start = (Row * columns);

                // Create an integer to hold the current index number of the item that belongs in this column and row
                int CurrentIndex = Start;

                // Loop through each column
                for (int Col = 0; Col < Columns; Col++)
                {
                    // Set the value for this row and column
                    Values[Row, Col] = CurrentIndex;

                    // If we have more data left, increment the current index counter.
                    if (CurrentIndex < (totalItems - 1) && CurrentIndex != -1)
                    {
                        // increment the current index
                        CurrentIndex++;
                    }
                    else if (CurrentIndex != -1)
                    {
                        // We're at the end of the items in the data table.
                        // Set the value of the current index to -1, which our rendering code knows it should ignore (create an empty table cell)
                        CurrentIndex = -1;
                    }

                }
            }

            // Return the two-dimensional array
            return Values;
        }
    }
}

