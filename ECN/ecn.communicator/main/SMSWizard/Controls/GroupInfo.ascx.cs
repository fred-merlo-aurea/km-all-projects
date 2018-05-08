using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ecn.common.classes;
using ecn.communicator.classes;
using ecn.communicator.classes.ImportData;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.SMSWizard.Controls
{
    public partial class GroupInfo : System.Web.UI.UserControl, IWizard
	{
		private const string ErrPhoneNumberRequired = "ERROR - Phone Number is required to import data.";
		private const string ErrSelectExistingGroup = "Select existing group or new group";
		private const string Action = "Action";
		private const string EmailAddress = "emailaddress";
		private const string Mobile = "mobile";
		private const string SortOrder = "sortOrder";
		private const string Totals = "Totals";
		private const string TotalRecordsInFile = "Total Records in the File";
		private const string New = "New";
		private const string Changed = "Changed";
		private const string Duplicates = "Duplicate(s)";
		private const string Skipped = "Skipped";
		private const string SkippedMasterSuppression = "Skipped (Emails in Master Suppression)";
		private const string T = "T";
		private const string I = "I";
		private const string U = "U";
		private const string D = "D";
		private const string S = "S";
		private const string M = "M";
		private const string NonBreakingSpace = "&nbsp;";
		private const string TimeToImportRecords = "Time to import records";
		private const string SortOrderAsc = "sortorder asc";
		private const string Ignore = "ignore";
        private const string ViewStateKeyGroupId = "GroupID";
	    protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator1;

		ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

		protected System.Web.UI.HtmlControls.HtmlSelect tableColumnHeadersSelectbox;
 //columnNames of Emails table	

	    Hashtable hUpdatedRecords = new Hashtable();

        public int WizardID { get; set; } = 0;

        public string ErrorMessage { get; set; } = string.Empty;

        private int GroupID
        {
            get
            {
                if (ViewState[ViewStateKeyGroupId] == null)
                {
                    return 0;
                }

                return (int)ViewState[ViewStateKeyGroupId];
            }

            set
            {
                ViewState[ViewStateKeyGroupId] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		public void Initialize() 
		{
			int SelectedFolderID = 0;
			plNewGroup.Visible = false;


			if (WizardID != 0)
			{
				try
				{
					ecn.communicator.classes.Wizard w = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);

					if (w.GroupID > 0)
						SelectedFolderID = Convert.ToInt32(DataFunctions.ExecuteScalar("Select isnull(FolderID ,0) as folderID from Groups where GroupID = " + w.GroupID));

					LoadFoldersDR(SelectedFolderID);
					LoadGroupsDR(SelectedFolderID, w.GroupID);
					plExistingGroup.Visible = true;
					
					//enable create option - disable if user dont have access
					plCreate.Visible = true;

					if(!(sc.CheckChannelAdmin() || sc.CheckSysAdmin() || sc.CheckAdmin()))
					{
                        //if (!(es.HasPermission("ecn.communicator", "grouppriv") && es.HasPermission("ecn.communicator", "addgroup")))
                        if (!KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.Edit))
							plCreate.Visible = false;
						else
						{
							//if (!es.HasPermission("ecn.communicator","addemails"))
                            if (!KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.AddEmails))
								rbNewGroup.Visible = false;

							//if (!es.HasPermission("ecn.communicator","importdata"))
                            if (!KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.ImportEmails))
								rbImportGroup.Visible = false;
						}
					}
				}
				catch (Exception ex)
				{
					ErrorMessage = "ERROR - " + ex.Message;
				}
			}
		}


		public bool Save() 
		{
			int FilterID = 0;

			Page.Validate();

			if (Page.IsValid)
			{
				try
				{
					if (rbExistingGroup.Checked)
					{
						GroupID = Convert.ToInt32(drpGroup.SelectedValue);
					}
					else if (rbNewGroup.Checked)
					{
						AddEmails();
					}
					else if (rbImportGroup.Checked)
					{
						if (!rbImporttoExisting.Checked && !rbImporttoNew.Checked)
						{
							ErrorMessage = "ERROR - Import Not completed!";	
							return false;
						}
					}

					if (GroupID > 0)
					{
						ecn.communicator.classes.Wizard w = new ecn.communicator.classes.Wizard();
				
						w.ID = WizardID;
						w.GroupID = GroupID;
						w.FilterID = FilterID;
						w.CompletedStep = 2;
						w.Save();
						return true;
					}
				}
				catch (Exception ex)
				{
					ErrorMessage = "ERROR - " + ex.Message;
				}
			}			
			return false;
		}

		#region load dropdowns

		private void LoadFoldersDR(int FolderID)
		{
			drpFolder.DataSource = DataLists.GetFoldersDR(sc.CustomerID().ToString(), "GRP");
			drpFolder.DataBind();
			drpFolder.Items.Insert(0, new ListItem("root", "0"));
			try
			{
				drpFolder.Items.FindByValue(FolderID.ToString()).Selected = true;
			}
			catch{}

			drpFolder1.DataSource = DataLists.GetFoldersDR(sc.CustomerID().ToString(), "GRP");
			drpFolder1.DataBind();
			drpFolder1.Items.Insert(0, new ListItem("root", "0"));
			drpFolder1.Items.FindByValue("0").Selected = true;

			drpFolder2.DataSource = DataLists.GetFoldersDR(sc.CustomerID().ToString(), "GRP");
			drpFolder2.DataBind();
			drpFolder2.Items.Insert(0, new ListItem("root", "0"));
			drpFolder2.Items.FindByValue("0").Selected = true;		
			LoadGroups2DR(0);

			drpFolder3.DataSource = DataLists.GetFoldersDR(sc.CustomerID().ToString(), "GRP");
			drpFolder3.DataBind();
			drpFolder3.Items.Insert(0, new ListItem("root", "0"));
			drpFolder3.Items.FindByValue("0").Selected = true;		
		}

		private void LoadGroupsDR(int FolderID, int GroupID)
		{
			
			string sqlQuery=
                " SELECT g.GroupID, g.GroupName + ' (' + convert(varchar, count(eg.emailgroupID)) + ')' as 'GroupName' " +
				" FROM Groups g join emailgroups eg on g.groupID = eg.groupID"+
				" WHERE CustomerID="+sc.CustomerID()+
				" and folderID = " + FolderID + " AND g.GroupID in (select GroupID from dbo.fn_getGroupsforUser("+sc.CustomerID()+","+ sc.UserID() +")) " + 
				" group by g.GroupName, g.GroupID"+
                " having count(eg.emailgroupID) > 0 " + 
				" ORDER BY GroupName ";

			DataTable dtGroups = DataFunctions.GetDataTable(sqlQuery);


			drpGroup.DataSource = dtGroups;
			drpGroup.DataTextField = "GroupName";
			drpGroup.DataValueField = "GroupID";
			drpGroup.DataBind();

			drpGroup.Items.Insert(0, new ListItem("----- Select List -----",""));

            if (GroupID > 0)
            {
                drpGroup.ClearSelection();
                rbExistingGroup.Checked = true;
                try
                {
                    drpGroup.Items.FindByValue(GroupID.ToString()).Selected = true;
                }
                catch { }
            }
          
		}

		private void LoadGroups2DR(int FolderID)
		{
			/*string sqlQuery=
				" SELECT g.GroupName, g.GroupID"+
				" FROM Groups g"+
				" WHERE CustomerID="+sc.CustomerID()+
				" and folderID = " + FolderID + " AND g.GroupID in (select GroupID from dbo.fn_getGroupsforUser("+sc.CustomerID()+","+ sc.UserID() +")) " + 
				" ORDER BY GroupName ";
            */
            string sqlQuery =
                " SELECT g.GroupID, g.GroupName + ' (' + convert(varchar, count(eg.emailgroupID)) + ')' as 'GroupName' " +
                " FROM Groups g left outer join emailgroups eg on g.groupID = eg.groupID " +
                " WHERE CustomerID=" + sc.CustomerID() +
                "  and folderID = " + FolderID + " AND g.GroupID in (select GroupID from dbo.fn_getGroupsforUser(" + sc.CustomerID() + "," + sc.UserID() + ")) " +
                " group by g.GroupName, g.GroupID" +
                " ORDER BY GroupName ";

			DataTable dtGroups = DataFunctions.GetDataTable(sqlQuery);

			drpGroup2.DataSource = dtGroups;
			drpGroup2.DataTextField = "GroupName";
			drpGroup2.DataValueField = "GroupID";
			drpGroup2.DataBind();

			drpGroup2.Items.Insert(0, new ListItem("----- Select List -----",""));
		}

		

		#endregion

		#region Validate / Add Phone Numbers (new list option)
		public void AddEmails() 
		{	
			if (ValidatePhoneNumber())
			{
				
				string gname	= DataFunctions.CleanString(txtGroupName.Text);

				string sqlcheck= " SELECT COUNT(*) FROM Groups WHERE GroupName='"+gname+ "' AND CustomerID="+sc.CustomerID();

				if (Convert.ToInt32(DataFunctions.ExecuteScalar(sqlcheck)) == 0) 
				{
					string sqlquery=
						" INSERT INTO Groups (GroupName, GroupDescription, CustomerID, FolderID, OwnerTypeCode, PublicFolder ) " +
						" VALUES " + 
						"('"+gname+"', '"+gname+"', "+sc.CustomerID()+", " + Convert.ToInt32(drpFolder1.SelectedValue) + ", 'customer' , 0 );select @@IDENTITY ";

					GroupID = Convert.ToInt32(DataFunctions.ExecuteScalar(sqlquery));

					sqlquery=
						" if (select count(groupID)  from usergroups where userID = " + sc.UserID() + " ) > 0 " +
						" INSERT INTO UserGroups ( UserID, GroupID ) VALUES (+ " +sc.UserID()+","+GroupID+")";

					DataFunctions.Execute(sqlquery);
				} 
				else 
				{
					throw new Exception("<font color='#000000'>\""+gname+"\"</font> already exists. Please enter a different name.");
				}

                string PhoneNumber = txtPhoneNumber.Text;
				try
				{
				    ImportFunctions importFunctions = new ImportFunctions();               

                    importFunctions.LoadPhoneNumberFromString(PhoneNumber, "html", "U", GroupID);
				}
				catch
				{
					throw;
				}
			}
		}

		private bool ValidatePhoneNumber()
		{
			bool isValidPH = true;
			string InvalidNumbers = string.Empty;
			string phonenumber = string.Empty;
			Regex cr = new Regex("\n");
			int numbercount = 0;
            foreach (string s in cr.Split(txtPhoneNumber.Text))
			{
                numbercount++;
                if (numbercount > 500)
				{
					throw new Exception("List should be less than 500 phone numbers.");
				}

                phonenumber = StringFunctions.Remove(s, StringFunctions.NonDomain());
                if (!isValidPhoneNumber(phonenumber))
				{
					isValidPH = false;
                    InvalidNumbers += phonenumber + (InvalidNumbers == string.Empty ? "" : ", ");
				}
			}

            if (!isValidPH)
			{
                throw new Exception("Invalid Phone Number :<br/><br/>" + InvalidNumbers);
			}

            return isValidPH;	
		}

		private bool isValidPhoneNumber(string phoneNumber) 
		{

            Regex regex = new Regex(@"\d{10}");
            return regex.IsMatch(phoneNumber);
		}

		#endregion

		#region radio button events

		protected void rbExistingGroup_CheckedChanged(object sender, System.EventArgs e)
		{
			plExistingGroup.Visible=true;
			plNewGroup.Visible = false;
			plImportGroup.Visible=false;
			plImportCompleted.Visible = false;
		}

		protected void rbNewGroup_CheckedChanged(object sender, System.EventArgs e)
		{
			plExistingGroup.Visible=false;
			plNewGroup.Visible = true;
			plImportGroup.Visible=false;
			plImportCompleted.Visible = false;
		}

		protected void rbImportGroup_CheckedChanged(object sender, System.EventArgs e)
		{
			plUpload.Visible = false;
			plImportGroup.Visible=true;
			plExistingGroup.Visible=false;
			plNewGroup.Visible = false;
			plImportCompleted.Visible = false;
		}

		protected void rbImporttoExisting_CheckedChanged(object sender, System.EventArgs e)
		{
			plImporttoExisting.Visible = true;
			plImporttoNew.Visible = false;
		}

		protected void rbImporttoNew_CheckedChanged(object sender, System.EventArgs e)
		{
			plImporttoExisting.Visible = false;
			plImporttoNew.Visible = true;		
		}

		#endregion


		#region dropdown events
		protected void drpFolder_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			LoadGroupsDR(Convert.ToInt32(drpFolder.SelectedValue), 0);
		}

		protected void drpFolder2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			LoadGroups2DR(Convert.ToInt32(drpFolder2.SelectedValue));
		}

		

		protected void drpGroup2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (lblfilename.Text != string.Empty)
				BuildMappingGrid(lblfilename.Text);
		}
		#endregion

		#region Upload & Import Button events

		protected void btnUpload_Click(object sender, System.EventArgs e)
		{
			plUpload.Visible = false;

			if (!rbImporttoExisting.Checked && !rbImporttoNew.Checked)
			{
				ErrorMessage = "ERROR - Select an Existing List or a New List to Import.";	
				return;
			}

			if (fBrowse.PostedFile.FileName.ToString().Trim() == string.Empty)
			{
				ErrorMessage = "ERROR - A file must be specified to upload.";
				return;
			}
			else if (fBrowse.PostedFile.FileName.ToLower().EndsWith(".xls") && txtSheetName.Text == string.Empty)
			{
				ErrorMessage="ERROR - Enter the Worksheet name for the Excel file.";
				return;
			}
			
			// check File Size - if > 50MB display message.
			if (fBrowse.PostedFile.ContentLength > 52428800)
			{
				ErrorMessage="ERROR - Your File exceeds 50 mb or 100,000 records.";
				return;			
			}

            if (fBrowse.PostedFile.FileName.ToLower().EndsWith(".xls") || fBrowse.PostedFile.FileName.ToLower().EndsWith(".xlsx") || fBrowse.PostedFile.FileName.ToLower().EndsWith(".xml") ||
				fBrowse.PostedFile.FileName.ToLower().EndsWith(".txt") || fBrowse.PostedFile.FileName.ToLower().EndsWith(".csv"))
			{
				string filename = StringFunctions.Replace(System.IO.Path.GetFileName(fBrowse.PostedFile.FileName)," ","_");
				filename = StringFunctions.Replace(filename,"\'","_");

				string FileUploadPath = getPhysicalPath();

				if (!Directory.Exists(FileUploadPath))
					Directory.CreateDirectory(FileUploadPath);

				fBrowse.PostedFile.SaveAs(FileUploadPath +"\\"+ filename);

                //if (filename.ToLower().EndsWith(".txt"))
                //{
                //    if (drpDelimiter.SelectedValue != "c")
                //        CreateSchemaFile(filename ,drpDelimiter.SelectedValue.ToString());
                //    else
                //        DeleteSchemaFile();
                //}


				// Check No of Records in File - If > 10000 - dislay message
				DataTable dtFile = GetDataTableByFileType(filename, txtSheetName.Text, 100001);
				if (dtFile == null)
				{
					ErrorMessage="ERROR - No Records Found in the uploaded file.";
					return;			
				}
				if (dtFile.Rows.Count > 100000)
				{
					ErrorMessage="ERROR - Your File exceeds 50 mb or 100,000 records.";
					return;			
				}
				else
				{
					lblfilename.Text = filename;
					drpGroup2.AutoPostBack = true;
					BuildMappingGrid(filename);
					plUpload.Visible = true;
					btnImport.Visible = true;
				}
				dtFile.Dispose();
			}
			else
			{
				ErrorMessage="ERROR - Cannot Upload File: <br><br>Only files with following extensions (Excel, XML, TXT & CSV) are supported.";
			}
		}

		protected void btnImport_Click(object sender, EventArgs e)
		{
			var mapper = new ImportMapper();
			var noEmailAddress = false;

			try
			{
				btnImport.Visible = false;

				if (ValidateMapping(mapper, ref noEmailAddress))
				{
					return;
				}

				if (CreateGroup(mapper))
				{
					return;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.TraceError(ex.Message);
				Reset(mapper);
				ErrorMessage = ErrPhoneNumberRequired;
				return;
			}

			try
			{
				BuildXmlUdf(noEmailAddress);

				plImportCompleted.Visible = true;
				plImportGroup.Visible = false;

				if(hUpdatedRecords.Count <= 0)
				{
					return;
				}

				UpdateRecords();
			}
			catch(Exception ex)
			{
				System.Diagnostics.Trace.TraceError(ex.Message);
				Reset(mapper);
				ErrorMessage = $"ERROR: {ex.Message}";
			}
		}

		private void UpdateRecords()
		{
			using(var dtRecords = new DataTable())
			{
				dtRecords.Columns.Add(Action);
				dtRecords.Columns.Add(Totals);
				dtRecords.Columns.Add(SortOrder);

				DataRow row;

				foreach(DictionaryEntry de in hUpdatedRecords)
				{
					row = dtRecords.NewRow();

					if(de.Key.ToString() == T)
					{
						row[Action] = TotalRecordsInFile;
						row[SortOrder] = 1;
					}
					else if(de.Key.ToString() == I)
					{
						row[Action] = New;
						row[SortOrder] = 2;
					}
					else if(de.Key.ToString() == U)
					{
						row[Action] = Changed;
						row[SortOrder] = 3;
					}
					else if(de.Key.ToString() == D)
					{
						row[Action] = Duplicates;
						row[SortOrder] = 4;
					}
					else if(de.Key.ToString() == S)
					{
						row[Action] = Skipped;
						row[SortOrder] = 5;
					}
					else if(de.Key.ToString() == M)
					{
						row[Action] = SkippedMasterSuppression;
						row[SortOrder] = 6;
					}

					row[Totals] = de.Value;
					dtRecords.Rows.Add(row);
				}

				row = dtRecords.NewRow();
				row[Action] = NonBreakingSpace;
				row[Totals] = string.Empty;
				row[SortOrder] = 8;
				dtRecords.Rows.Add(row);

				var startDateTime = DateTime.Now;
				var duration = DateTime.Now - startDateTime;

				row = dtRecords.NewRow();
				row[Action] = TimeToImportRecords;
				row[Totals] = $"{duration.Hours}:{duration.Minutes}:{duration.Seconds}";
				row[SortOrder] = 9;
				dtRecords.Rows.Add(row);

				var dv = dtRecords.DefaultView;
				dv.Sort = SortOrderAsc;

				dgImport.DataSource = dv;
				dgImport.DataBind();
			}
		}

		private void BuildXmlUdf(bool noEmailAddress)
		{
			const int maximumRowsLimit = 100;
			var udfExists = false;
			var colRemove = new ArrayList();
			var emailAddressOnly = true;
			var mobileNumbersOnly = false;
			var dtFile = PopulateDataTable(colRemove, noEmailAddress, ref udfExists, ref emailAddressOnly, ref mobileNumbersOnly);
			var hGdfFields = GetGroupDataFields(GroupID);
			var iTotalRecords = dtFile.Rows.Count;
			var iProgressInc = iTotalRecords / 10;
			var iProgressCount = iProgressInc;
			var iProgressPercent = 0;

			for (var cnt = 0; cnt < dtFile.Rows.Count; cnt++)
			{
				if (cnt == 0 && dtFile.Rows.Count >= maximumRowsLimit)
				{
					initNotify("Importing !");
				}

				var drFile = dtFile.Rows[cnt];
				var xmlProfile = new StringBuilder(string.Empty);
				xmlProfile.Append("<Emails>");
				var xmlUdf = new StringBuilder(string.Empty);

				AppendXmlUdf(dtFile, xmlProfile, drFile, udfExists, hGdfFields, xmlUdf);

				if(cnt != 0 && cnt % 10000 == 0 || cnt == dtFile.Rows.Count - 1)
				{
					UpdateToDB(Convert.ToInt32(sc.CustomerID()), GroupID,
						$"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlProfile}</XML>",
						$"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlUdf}</XML>", emailAddressOnly,
						mobileNumbersOnly);
				}

				if((cnt == iProgressCount || cnt == dtFile.Rows.Count - 1) && dtFile.Rows.Count >= maximumRowsLimit)
				{
					iProgressPercent = iProgressPercent + 10;

					if (iTotalRecords == 0)
					{
					    throw new DivideByZeroException(nameof(iTotalRecords));
					}

					Notify(iProgressPercent <= maximumRowsLimit
							? iProgressPercent.ToString()
							: "101",
						$"Importing ({iProgressCount} / {iTotalRecords})");

					iProgressCount = iProgressCount + iProgressInc;

					if(cnt == dtFile.Rows.Count - 1)
					{
						Thread.Sleep(1000);
					}
				}
			}

			hGdfFields.Clear();
		}

		private void AppendXmlUdf(
			DataTable dtFile,
			StringBuilder xmlProfile,
			DataRow drFile,
			bool udfExists,
			Hashtable hGdfFields,
			StringBuilder xmlUdf)
		{
			var bRowCreated = false;
			foreach(DataColumn dcFile in dtFile.Columns)
			{
				if(dcFile.ColumnName.IndexOf("user_", StringComparison.Ordinal) == -1 &&
				   dcFile.ColumnName.IndexOf("delete", StringComparison.Ordinal) == -1)
				{
					xmlProfile.Append(
						$"<{dcFile.ColumnName}>{CleanXMLString(drFile[dcFile.ColumnName].ToString())}</{dcFile.ColumnName}>");
				}

				if(udfExists && hGdfFields.Count > 0 && dcFile.ColumnName.IndexOf("user_", StringComparison.Ordinal) > -1)
				{
					if(!bRowCreated)
					{
						xmlUdf.Append("<row>");
						xmlUdf.Append($"<ea>{CleanXMLString(drFile[EmailAddress].ToString())}</ea>");
						bRowCreated = true;
					}

					xmlUdf.Append($"<udf id=\"{hGdfFields[dcFile.ColumnName]}\">");
					xmlUdf.Append($"<v>{CleanXMLString(drFile[dcFile.ColumnName].ToString())}</v>");
					xmlUdf.Append("</udf>");
				}
			}

			xmlProfile.Append("</Emails>");

			if(bRowCreated)
			{
				xmlUdf.Append("</row>");
			}
		}

		private DataTable PopulateDataTable(
			ArrayList colRemove,
			bool noEmailAddress,
			ref bool udfExists,
			ref bool emailAddressOnly,
			ref bool mobileNumbersOnly)
		{
			var dtFile = GetDataTableByFileType(lblfilename.Text, txtSheetName.Text, 0);

			for(var i = 0; i < dtFile.Columns.Count; i++)
			{
				var mstr = Parent.Page.Master;
				if(mstr != null)
				{
					var selectedColumnName =
						Request.Params.Get($"{mstr.ClientID}$ContentPlaceHolder1$ECNWizard$ColumnHeaderSelect{i}");
					if(selectedColumnName.ToLower() == Ignore)
					{
						colRemove.Add($"delete{i}");
						dtFile.Columns[i].ColumnName = $"delete{i}";
					}
					else
					{
						if(selectedColumnName.IndexOf("user_", StringComparison.OrdinalIgnoreCase) > -1)
						{
							udfExists = true;
						}
						else if(selectedColumnName.ToLower() != EmailAddress)
						{
							emailAddressOnly = false;
						}

						dtFile.Columns[i].ColumnName = selectedColumnName.ToLower();
					}
				}
			}

			for(var j = 0; j < colRemove.Count; j++)
			{
				dtFile.Columns.Remove(colRemove[j].ToString());
			}

			colRemove.Clear();

			if(noEmailAddress)
			{
				//logic to add email address
				dtFile.Columns.Add(EmailAddress, typeof(string));
				for(var i = 0; i < dtFile.Rows.Count; i++)
				{
					if(isValidPhoneNumber(dtFile.Rows[i][Mobile].ToString()))
					{
						dtFile.Rows[i][EmailAddress] = $"{dtFile.Rows[i][Mobile]}@KMautogenerated.com";
					}
				}

				mobileNumbersOnly = true;
			}

			return dtFile;
		}

		private bool CreateGroup(ImportMapper mapper)
		{
			if(rbImporttoExisting.Checked)
			{
				GroupID = Convert.ToInt32(drpGroup2.SelectedValue);
			}
			else if(rbImporttoNew.Checked)
			{
				if(GroupID == 0)
				{
					var gname = DataFunctions.CleanString(txtGroupName1.Text);

					var sqlcheck = $" SELECT COUNT(*) FROM Groups WHERE GroupName=\'{gname}\' AND CustomerID={sc.CustomerID()}";

					if(Convert.ToInt32(DataFunctions.ExecuteScalar(sqlcheck)) == 0)
					{
						var sqlquery =
							$" INSERT INTO Groups (GroupName, GroupDescription, CustomerID, FolderID, OwnerTypeCode, PublicFolder )  VALUES (\'{gname}\', \'{gname}\', {sc.CustomerID()}, {Convert.ToInt32(drpFolder3.SelectedValue)}, \'customer\' , 0 );select @@IDENTITY ";

						GroupID = Convert.ToInt32(DataFunctions.ExecuteScalar(sqlquery));

						sqlquery =
							$" if (select count(groupID)  from usergroups where userID = {sc.UserID()} ) > 0  INSERT INTO UserGroups ( UserID, GroupID ) VALUES (+ {sc.UserID()},{GroupID})";

						DataFunctions.Execute(sqlquery);
					}
					else
					{
						Reset(mapper);
						ErrorMessage =
							$"ERROR - <font color=\'#000000\'>\"{gname}\"</font> already exists. Please enter a different name.";
						return true;
					}
				}
			}
			else
			{
				Reset(mapper);
				ErrorMessage = ErrSelectExistingGroup;
				return true;
			}

			return false;
		}

		private bool ValidateMapping(ImportMapper mapper, ref bool noEmailAddress)
		{
			// Just load one line of data to get the column count.
			var dtFile = GetDataTableByFileType(lblfilename.Text, txtSheetName.Text, 1);

			var duplicatedColumns = new StringBuilder();
			var duplicationColumnCount = 0;

			for (var i = 0; i < dtFile.Columns.Count; i++)
			{
				var mstr = Parent.Page.Master;

				if(mstr != null)
				{
					var selectedColumnName =
						Request.Params.Get($"{mstr.ClientID}$ContentPlaceHolder1$ECNWizard$ColumnHeaderSelect{i}");
					if (!selectedColumnName.Equals(Ignore, StringComparison.OrdinalIgnoreCase) && !mapper.AddMapping(i, selectedColumnName))
					{
						duplicationColumnCount++;
						duplicatedColumns.Append($"{(duplicatedColumns.Length > 0 ? "/" : string.Empty)}{selectedColumnName}");
					}
				}
			}

			if(duplicationColumnCount > 0)
			{
				Reset(mapper);
				ErrorMessage = $"ERROR - You have selected duplicate field names.<BR><BR>{duplicatedColumns}.";
				return true;
			}

			if(mapper.MappingCount == 0)
			{
				Reset(mapper);
				ErrorMessage = ErrPhoneNumberRequired;
				return true;
			}

			if(!mapper.HasMobileNumber)
			{
				Reset(mapper);
				ErrorMessage = ErrPhoneNumberRequired;
				return true;
			}

			if(!mapper.HasEmailAddress)
			{
				noEmailAddress = true;
			}

			return false;
		}

		private string CleanXMLString(string text)
		{
			text = text.Replace("&", "&amp;") ;
			text = text.Replace("\"", "&quot;");
			text = text.Replace("<", "&lt;") ;
			text = text.Replace(">", "&gt;") ;
			return text ;
		}

		#endregion

		#region Build Mapping Table
		private void BuildMappingGrid(string file)
		{
			BuildEmailImportForm(GetDataTableByFileType(file, txtSheetName.Text, 6), new ImportMapper());
		}

		private void Reset(ImportMapper mapper) 
		{
			BuildEmailImportForm(GetDataTableByFileType(lblfilename.Text, txtSheetName.Text, 6), mapper);  
			btnImport.Visible = true;
		}

		private void BuildEmailImportForm(DataTable dtFile, ImportMapper mapper) 
		{
			if (dtFile == null) 
			{
				ErrorMessage = "ERROR - No Records Found in the uploaded file.";
				return;
			}			

			HtmlTableRow tableRows = null;
			HtmlTableCell headerColumn = null;	// <td> to hold the header which is the emails table dropdown columns
			HtmlTableCell dataColumn = null;		// <td> to hold the data from the file

			for(int i = 0; i<dtFile.Columns.Count; i++)
			{
				if (i==0)
				{
					tableRows = new HtmlTableRow();		//create a <TR> ie the ROW 

					headerColumn = new HtmlTableCell();	//create a <TD> ie headerColumn
					headerColumn.Style.Add("background-color","#cccccc");
					headerColumn.Width="25%";
					headerColumn.Style.Add("padding-left","5px");

					dataColumn	= new HtmlTableCell();
					dataColumn.Style.Add("background-color","#cccccc");
					dataColumn.Width="75%";
					dataColumn.Style.Add("padding-left","5px");

					Label lblCol1Header = new Label();
					Label lblCol2Header = new Label();

					lblCol1Header.CssClass="label10";
					lblCol1Header.Text = "<strong>Field Name</strong>";

					lblCol2Header.CssClass="label10";
					lblCol2Header.Text = "<strong>Data</strong>";

					headerColumn.Controls.Add(lblCol1Header);
					dataColumn.Controls.Add(lblCol2Header);

					tableRows.Cells.Add(headerColumn);
					tableRows.Cells.Add(dataColumn);
					dataCollectionTable.Rows.Add(tableRows);
				}

				//build the HTML select control which has the EmailTable's column headings
				tableColumnHeadersSelectbox = buildColumnHeaderDropdowns("ColumnHeaderSelect"+i);
				tableColumnHeadersSelectbox.SelectedIndex = tableColumnHeadersSelectbox.Items.IndexOf(tableColumnHeadersSelectbox.Items.FindByText(mapper.GetColumnName(i)));
				tableRows = new HtmlTableRow();		//create a <TR> ie the ROW 
				headerColumn = new HtmlTableCell();	//create a <TD> ie headerColumn
				headerColumn.Controls.Add(tableColumnHeadersSelectbox);	//add the <select/> to the TD
				headerColumn.VAlign = "middle";
				headerColumn.Align = "center";
				
				tableRows.Cells.Add(headerColumn);	//add the <td> to the <tr>

				//now add the data from the file to the next TD
				dataColumn	= new HtmlTableCell();
				dataColumn.Style.Add("font-family","Verdana, Arial, Helvetica, sans-serif");
				dataColumn.Style.Add("font-size","10px");
				dataColumn.Style.Add("padding-left","5px");

				Label tableDataColumnLabel = new Label();
				string textData = "";
				int lineStartCheck = 0;
				foreach(DataRow dr in dtFile.Rows)
				{
					lineStartCheck++;
					if (lineStartCheck > 5)
					{
						textData += "<font color=orange><b>&nbsp;&nbsp;&nbsp;more...</b></font>";
						break;
					}
					textData += dr[i].ToString()+", ";
				}

				tableDataColumnLabel.Text = textData; //string.Format("&nbsp;{0}", StringFunctions.Left(textData, textData.Length>100?100:textData.Length)+((textData.Length>100)?"<font color=orange><b>&nbsp;&nbsp;&nbsp;more...</b></font>":" "));
				dataColumn.Controls.Add(tableDataColumnLabel);
				tableRows.Cells.Add(	dataColumn);

				dataCollectionTable.Rows.Add(tableRows);	//add the <TR> to the table
			}
		}

		private HtmlSelect buildColumnHeaderDropdowns(string selectBoxName)
		{
			ArrayList columnHeaderSelect = new ArrayList();				
			for(int i=0; i< ColumnManager.ColumnCount; i++) 
				columnHeaderSelect.Insert(i, ColumnManager.GetColumnNameByIndex(i));
			
			HtmlSelect selectbox = new HtmlSelect();
			selectbox.ID = selectBoxName;
			selectbox.Attributes.Add("class","label10");
			selectbox.DataSource = columnHeaderSelect;	//build the <select/>
			selectbox.DataBind(); 
			return selectbox;
		}

		#endregion

		#region Import data helper methods/properties	
		private DataTable GetDataTableByFileType(string fileName, string excelSheetName, int maxRecordsToRetrieve) 
		{
			int startLine = 0;
			string physicalDataPath = getPhysicalPath();	
		
			string fileType = string.Empty;

            if (fileName.ToLower().EndsWith(".xls") || fileName.ToLower().EndsWith(".xlsx"))
				fileType = "X";
			else if (fileName.ToLower().EndsWith(".txt"))
				fileType = "O";			
			else if (fileName.ToLower().EndsWith(".csv"))
				fileType = "C";			
			else if (fileName.ToLower().EndsWith(".xml"))
				fileType = "XML";			

			try 
			{			
				return FileImporter.GetDataTableByFileType(physicalDataPath, fileType , fileName, excelSheetName, startLine, maxRecordsToRetrieve, "");
			}
			catch(ArgumentException) 
			{
				ErrorMessage = "ERROR - Unknow file type.<br> Please contact System Administrator by sending an <a href='mailto:support@teckman.com'>email</a> along with the following Stack Trace.<br><br>";
			}
			catch(Exception ex)
			{
				switch (fileType) 
				{
					case "X": 
						ErrorMessage = "ERROR - Error occured while fetching data from the Excel File.<br> Please contact System Administrator by sending an <a href='mailto:support@teckman.com'>email</a> along with the following Stack Trace.<br><br>"+ex.ToString();
						break;
					default:
						ErrorMessage = "ERROR - Error occured while fetching data from the CSV File.<br> Please contact System Administrator by sending an <a href='mailto:support@teckman.com'>email</a> along with the following Stack Trace.<br><br>"+ex.ToString();
						break;
				}
			}			
			return null;
		}

		private EmailTableColumnManagerCommunicator _columnManager;
		public EmailTableColumnManagerCommunicator ColumnManager 
		{
			get 
			{
				if (_columnManager == null) 
				{
					_columnManager = new EmailTableColumnManagerCommunicator();			
					_columnManager.AddGroupDataFields(GroupDataFields);
				}
				return (this._columnManager);
			}			
		}
        
		protected ArrayList GroupDataFields 
		{
			get 
			{
				int GroupID = drpGroup2.SelectedValue==string.Empty?0:Convert.ToInt32(drpGroup2.SelectedValue);
				return GroupDataField.GetGroupDataFieldsByGroupID(Convert.ToInt32(GroupID));
			}			
		}
        
        private string getPhysicalPath()
        {
            string DataPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + sc.CustomerID() + "/data");

            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }
            return DataPath;
        }
        
		private Hashtable GetGroupDataFields(int groupID) 
		{
			string sqlstmt= " SELECT * FROM GroupDatafields WHERE GroupID="+ groupID;
                        
			DataTable emailstable = DataFunctions.GetDataTable(sqlstmt);
            
			Hashtable fields = new Hashtable();
			foreach(DataRow dr in emailstable.Rows) 
				fields.Add("user_" + dr["ShortName"].ToString().ToLower(), Convert.ToInt32(dr["GroupDataFieldsID"]));

			return fields;
		}

		#endregion

        private void UpdateToDB(int CustomerID, int GroupID, string xmlProfile, string xmlUDF, bool EmailaddressOnly, bool MobileNumbersOnly)
		{
			SqlCommand cmd = new SqlCommand("sp_importEmails");
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@CustomerID", SqlDbType.VarChar);
			cmd.Parameters["@CustomerID"].Value = CustomerID;

			cmd.Parameters.Add("@GroupID", SqlDbType.VarChar);
			cmd.Parameters["@GroupID"].Value = GroupID;

			cmd.Parameters.Add("@xmlProfile", SqlDbType.Text);
			cmd.Parameters["@xmlProfile"].Value = xmlProfile;

			cmd.Parameters.Add("@xmlUDF", SqlDbType.Text);
			cmd.Parameters["@xmlUDF"].Value = xmlUDF;

			cmd.Parameters.Add("@formattypecode", SqlDbType.VarChar);
            cmd.Parameters["@formattypecode"].Value = "HTML";

            if (MobileNumbersOnly)
            {
                cmd.Parameters.Add("@subscribetypecode", SqlDbType.VarChar);
                cmd.Parameters["@subscribetypecode"].Value = "U";
            }
            else
            {
                cmd.Parameters.Add("@subscribetypecode", SqlDbType.VarChar);
                cmd.Parameters["@subscribetypecode"].Value = "S";
            }

			cmd.Parameters.Add("@EmailAddressOnly", SqlDbType.Bit);
			cmd.Parameters["@EmailAddressOnly"].Value = EmailaddressOnly?1:0;


			DataTable dtRecords = DataFunctions.GetDataTable(cmd);

			if (dtRecords.Rows.Count > 0)
			{
				foreach(DataRow dr in dtRecords.Rows)
				{
					if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
						hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
					else
					{
						int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
						hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
					}
				}
				
			}

			cmd.Dispose();
		}

		#region ProgressBar
		public  void initNotify( string StrSplash)
		{
			// Only do this on the first call to the page
			//Register loadingNotifier.js for showing the Progress Bar
			Response.Write(string.Format(@"<link href='/ecn.accounts/styles/progressbar.css' type='text/css' rel='stylesheet' />
				<body><script type='text/javascript' src='/ecn.accounts/scripts/ProgressBar.js'></script>
              <script language='javascript' type='text/javascript'>
              initLoader('{0}');
              </script></body>",StrSplash));
			// Send it to the client
			Response.Flush();


		}
		public  void Notify(string strPercent, string strMessage)
		{
			//Update the Progress bar
			Response.Write(string.Format("<script language='javascript' type='text/javascript'>setProgress({0},'{1}'); </script>", strPercent, strMessage));
			Response.Flush();
		}
		#endregion

      
	}
}
