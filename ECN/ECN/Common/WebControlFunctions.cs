using System;
using System.Data;
using System.Web.UI.WebControls;

/*
 * class WebControlFunctions
 * a class containing a number of static methods
 * used to manipulate WebControl objects
 * 
 * Author: Dan Schwie <mailto:dschwie@teckman.com>
 * Copyright Tecknowledge Management 2003
 */
namespace ecn.common.classes {
	public class WebControlFunctions {
		public WebControlFunctions() {	
		}
		/*
		 * void SetSelectedIndex(DropDownList ddl, string selectedStr)
		 * set the selected index of a DropDownList object
		 * args: DropDownList ddl   - a DropDownList object
		 *       string selectedStr - string representation of the item that should be selected
		 */
		public static void SetSelectedIndex(DropDownList ddl, string selectedStr) {
			for(int i = 0; i <  ddl.Items.Count; i++) {
				if(ddl.Items[i].Value == selectedStr)		
					ddl.SelectedIndex = i;			
			}
		}
		/*
		 * void PopulateDropDownList(DropDownList ddl, DataTable dataSource, string displayStr, string valueStr)
		 * bind data to a DropDownList object
		 * args: DropDownList ddl     - a DropDownList object
		 *       DataTable dataSource - a DataTable containing the values to be used in populating the DropDownList object
		 *       string displayStr    - field in the database containing the text which the DropDownList displays
		 *       string valueStr      - field in the database containing the values which the DropDownList passes in the form
		 */
		public static void PopulateDropDownList(DropDownList ddl, DataTable dataSource, string displayStr, string valueStr) {
			ddl.DataSource     = dataSource;
			ddl.DataTextField  = displayStr;
			ddl.DataValueField = valueStr;
			ddl.DataBind();
		}
		/*
		 * void SetText(TextBox[] tbArr, string[] strArr)
		 * set the text in a number of TextBox objects
		 * args: TextBox[] tbArr - an array of TextBox objects
		 *       string[] strArr - an array of strings
		 */
		public static void SetText(TextBox[] tbArr, string[] strArr) {
			for(int i = 0; i < tbArr.Length; i++) {
				tbArr[i].Text = strArr[i];
			}
		}
	}
}
