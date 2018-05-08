using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace UsefulControlsTest
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class CarsUnTypedXml : System.Web.UI.Page
	{
		protected DataSet unTypedCars;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack || chkShowList.Checked == true)
			{
				// Get the data
				LoadUnTypedDataFromXml();

			}
			else
			{
				// Hide the CategorizedCheckBoxList so that we don't get an error
				CategorizedCheckBoxList1.Visible = false;

				// Hide the "ShowList" checkbox
				chkShowList.Visible = false;

				// Hide the button, too
				btnTestValues.Visible = false;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		

		/// <summary>
		/// Reads the selected values from the categorized checkbox list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnTestValues_Click(object sender, System.EventArgs e)
		{
			if(CategorizedCheckBoxList1.Selections.Count > 0)
			{
				StringBuilder Sb = new StringBuilder();
				Sb.Append("The following values were selected:<ul>");
			
				foreach(string check in CategorizedCheckBoxList1.Selections)
				{
					Sb.Append("<li>");
					Sb.Append(check);
					Sb.Append("</li>");
				}

				Sb.Append("</ul>");
				Label1.Text = Sb.ToString();
			}
			else
			{
				Label1.Text = "No checkboxes were selected.";
			}
		}

		

		/// <summary>
		/// Loads data from an XML file into our typed dataset.
		/// </summary>
		protected void LoadUnTypedDataFromXml()
		{
			unTypedCars = new DataSet();
			unTypedCars.ReadXml(Server.MapPath("UnTypedCars.xml"));
			CategorizedCheckBoxList1.DataTable = unTypedCars.Tables[0];
//			CategorizedCheckBoxList1.DataTextColumn = "Model";
//			CategorizedCheckBoxList1.DataValueColumn = "CarModelPK";
//			CategorizedCheckBoxList1.DataCategoryColumn = "Make";
		}

	}
}
