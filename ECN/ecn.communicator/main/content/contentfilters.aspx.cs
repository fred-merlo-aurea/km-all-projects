using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Objects;
using CommunicatorBusinessLayer = ECN_Framework_BusinessLayer.Communicator;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;

namespace ecn.communicator.contentmanager 
{
	public partial class contentfilters : ECN_Framework.WebPageHelper  
    {
		int selectedFolderID	= 0;
		int CustomerID		= 0;


        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

		protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CONTENT; ;
            Master.SubMenu = "";
            Master.Heading = "Create smartContent Rules";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icogroups.gif><B>Create smartContent Rule</B><br />To Create a new smartContent Rule for this Email, just type in the smartContent Rule name in the <i>'Text Field'</i> and hit <i>'Create new smartContent Rule'</i> button.<br />This will add a new smartContent Rule to your list.<br />After you create the smartContent Rule the new smartContent Rule is listed in the 'List of smartContent Rules section'. <br /><br /><B>Adding smartContent Rule Attributes and smartContent Rule Rows<br /></B>Click on <i>'Edit / Add smartContent Rule attributes'</i> to add smartContent Rule Rows<br />Creating a smartContent Rule row is done using the following attributes:<br />&nbsp;&nbsp;*&nbsp;<i><B>Compare field</B></i> - is the field that will be looked up for smartContent Ruleing email addresses in this group<br />&nbsp;&nbsp;*&nbsp;<i><B>Comparator</B></i> - is the Comparator field where you specify how the values match.<br />&nbsp;&nbsp;*&nbsp;<i><B>Compare Value</B></i> - is the the value to be compared.<br />&nbsp;&nbsp;*&nbsp;<i><B>Combine smartContent Rule rows</B></i> - if you have more smartContent Rule rows to be added use this field to combine.<br />Click on the <i>Add smartContent Rule</i> button to add the smartContent Rule row.<br /><br /><B>Deleting smartContent Rule</B><br />To delete unused smartContent Rules click on the 'Delete' hyperlink corresponding to the smartContent Rule to be deleted. <br /><u><b>Important NOTE: </b></u>Deleting a smartContent Rule will delete all the smartContent Rule attributes associated with this smartContent Rule";
            Master.HelpTitle = "smartContent Manager";	
            if(ECN_Framework_BusinessLayer.Communicator.ContentFilter.HasPermission(KMPlatform.Enums.Access.Edit, Master.UserSession.CurrentUser))
            {               
                int requestLayoutID = getLayoutID();
                int requestSlotNumber = getSlotNumber();
                CustomerID = Master.UserSession.CurrentUser.CustomerID;

				if(Page.IsPostBack == false)
                { 
					string action = getAction();
					int contentfilterID	 = getFilterID();
					int contentfilterDetailID	 = getFilterDetailID();
					String contentfilterDetaildisplay	= getFilterDetailDisplay();

					if(contentfilterDetaildisplay.Equals("true"))
                    {
						FilterAttribsPanel.Visible = true;
						AddFilterButtonPanel.Visible = false;
						RuleNameDisplay.Visible = true;
						FiltersPanel.Visible = false;

						loadAddFiltersGrid(contentfilterID);
					}

					LoadFolderControl("CNT");		
					selectedFolderID = Convert.ToInt32(FolderControl.SelectedFolderID);
					Session.Add("selectedFldr",selectedFolderID);
			
					if(action == "deleteFilter")
                    {
						DeleteFilter(contentfilterID);
					}
                    else if(action == "editFilter")
                    {
						EditFilter(contentfilterID);
					}
                    else if(action == "deleteFilterDetail")
                    {
						DeleteFilterDetail(contentfilterDetailID);
					}
                    else
                    {
						loadContentsDR(selectedFolderID, CustomerID);
                        loadGroupsGrid();
						loadFiltersGrid(requestLayoutID,requestSlotNumber);	
					}
				}
                else
                {
					selectedFolderID = Convert.ToInt32(FolderControl.SelectedFolderID);
					if(!(Session["selectedFldr"].ToString().Equals(selectedFolderID.ToString())))
                    {
						loadContentsDR(selectedFolderID, CustomerID);
					}
				}
				Session["selectedFldr"] = selectedFolderID;
			}
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();		
			}
		}
		
		#region getRequest Parameters
        private int getLayoutID()
        {
            int theLayoutID = 0;
			if(Request.QueryString["LayoutID"]!=null)
            {
                theLayoutID = Convert.ToInt32(Request.QueryString["LayoutID"].ToString());
			}
			return theLayoutID;
		}

        private int getSlotNumber()
        {
            int theSlotNumber = 0;
            if (Request.QueryString["SlotNumber"] != null)
            {
                theSlotNumber = Convert.ToInt32(Request.QueryString["SlotNumber"].ToString());
            }
            return theSlotNumber;
        }

		private String getAction() {
			String theAction = "";
			try {
				theAction = Request.QueryString["action"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theAction;
		}

		private int getFilterID()
        {
            int FilterID = 0;
            if (Request.QueryString["FilterID"] != null)
            {
                FilterID = Convert.ToInt32(Request.QueryString["FilterID"].ToString());
            }
            return FilterID;
		}

        private int getFilterDetailID()
        {
            int FilterDetailID = 0;
            if (Request.QueryString["FilterDetailID"] != null)
            {
                FilterDetailID = Convert.ToInt32(Request.QueryString["FilterDetailID"].ToString());
            }
            return FilterDetailID;
		}
		
		private string getFilterDetailDisplay() {
			string display = "";
			try {
				display = Request.QueryString["fd"].ToString();
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return display;
		}
		#endregion

		#region Data Load
		private void loadFiltersGrid(int LayoutID,int SlotNumber) 
        {
            List<ECN_Framework_Entities.Communicator.ContentFilter> contentFilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByLayoutIDSlotNumber(LayoutID, SlotNumber, Master.UserSession.CurrentUser ,false);
            FiltersGrid.DataSource = contentFilterList;
            FiltersGrid.DataBind();
            FiltersPager.RecordCount = contentFilterList.Count;
		}

        private void loadGroupsGrid()
        {
            List<ECN_Framework_Entities.Communicator.Group> grpList = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID_NoAccessCheck(Master.UserSession.CurrentUser.CustomerID, "active");
            var result = (from src in grpList
                          orderby src.GroupName
                          select src).ToList();
            GroupList.DataSource = result;
            GroupList.DataTextField = "GroupName";
            GroupList.DataValueField = "GroupID";
            GroupList.DataBind();
        }

        private void loadAddFiltersGrid(int FilterID)
        {
            DataTable contentFilterDetailViewList = ECN_Framework_BusinessLayer.Communicator.ContentFilterDetail.GetByContentIDFilterID(FilterID, Master.UserSession.CurrentUser);
            foreach (DataRow contentFilterDetail in contentFilterDetailViewList.AsEnumerable())
            {
                FilterNameTxtLabel.Text =contentFilterDetail["FilterName"].ToString();
                FilterIDValue.Text = FilterID.ToString();
                break;
            }

            AddFiltersGridArray.DataSource = contentFilterDetailViewList;
            AddFiltersGridArray.DataBind();
            BuildComparatorDR(0);
			
		}

		private void loadContentsDR(int selectedFolder, int CustomerID)
        {
            List<ECN_Framework_Entities.Communicator.Content> contentList = ECN_Framework_BusinessLayer.Communicator.Content.GetByFolderIDCustomerID(selectedFolderID, Master.UserSession.CurrentUser, false,"active");
            var result = (from src in contentList
                         orderby src.ContentTitle
                         select src).ToList();
            ContentList.DataSource = result;
            ContentList.DataTextField = "ContentTitle";
            ContentList.DataValueField = "ContentID";
			ContentList.DataBind();
			ContentList.Items.Insert(0,new ListItem("--select Content--",""));
		}

		private void BuildComparatorDR(int index)
        {
			switch(index.ToString()){
				case "0":
					Comparator.Items.Clear();
					Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
					Comparator.Items.Add(new ListItem("contains", "contains"));
					Comparator.Items.Add(new ListItem("ends with", "ending with"));
					Comparator.Items.Add(new ListItem("starts with", "starting with"));
					Default_CompareValuePanel.Visible	= true;
					DtTime_CompareValuePanel.Visible	= false;
					break;

				case "1":
					Comparator.Items.Clear();
					Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
					Comparator.Items.Add(new ListItem("contains", "contains"));
					Comparator.Items.Add(new ListItem("ends with", "ending with"));
					Comparator.Items.Add(new ListItem("starts with", "starting with"));
					Default_CompareValuePanel.Visible	= true;
					DtTime_CompareValuePanel.Visible	= false;
					break;

				case "2":
					Comparator.Items.Clear();
					Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
					Comparator.Items.Add(new ListItem("greater than [ > ]", "greater than"));
					Comparator.Items.Add(new ListItem("less than [ < ]", "less than"));
					Default_CompareValuePanel.Visible	= true;
					DtTime_CompareValuePanel.Visible	= false;
					break;

				case "3":
					Comparator.Items.Clear();
					Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
					Comparator.Items.Add(new ListItem("greater than [ > ]", "greater than"));
					Comparator.Items.Add(new ListItem("less than [ < ]", "less than"));
					Comparator.Items.Add(new ListItem("between dates", "between"));
					Default_CompareValuePanel.Visible	= false;
					DtTime_CompareValuePanel.Visible	= true;
					break;

				default:
					Comparator.Items.Clear();
					Comparator.Items.Add(new ListItem("equals [ = ]", "equals"));
					Comparator.Items.Add(new ListItem("contains", "contains"));
					Comparator.Items.Add(new ListItem("ends with", "ending with"));
					Comparator.Items.Add(new ListItem("starts with", "starting with"));
					break;
			}
		}
		#endregion

		#region FolderControl Load
		private void LoadFolderControl(string selectedFolderType){
			FolderControl.ID = "FolderControl";
			FolderControl.CustomerID			= Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID);
			FolderControl.FolderType			= selectedFolderType;
			FolderControl.NodesExpanded		= true;
			FolderControl.ChildrenExpanded	= false;
			FolderControl.LoadFolderTree();
		}
		#endregion

		#region Data Handlers
		public void FiltersList(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Communicator.ContentFilter  contentFilter= ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByFilterID(Convert.ToInt32(getFilterID()), Master.UserSession.CurrentUser,false);
            string theLayoutID = contentFilter.LayoutID.ToString();
            string SlotNumber = contentFilter.SlotNumber.ToString();
			Response.Redirect("contentfilters.aspx?LayoutID="+ theLayoutID + "&SlotNumber=" + SlotNumber);
		}

		public void DisplayAddFilterForm(object sender, EventArgs e)
        {
            try
            {
			    string lastInsertedFilterID = "";

			    string now				= System.DateTime.Now.ToString();
			    string FilterName		= ECN_Framework_Common.Functions.StringFunctions.CleanString(FilterNameTxt.Text);
                string cmp_content_id	= ContentList.SelectedValue.ToString();
			    string cmp_group_id		= GroupList.SelectedValue.ToString();

                ECN_Framework_Entities.Communicator.ContentFilter contentFilter = new ECN_Framework_Entities.Communicator.ContentFilter();
                contentFilter.LayoutID = Convert.ToInt32(getLayoutID());
                contentFilter.SlotNumber = Convert.ToInt32(getSlotNumber());
                contentFilter.GroupID = Convert.ToInt32(cmp_group_id);
                contentFilter.ContentID = Convert.ToInt32(cmp_content_id);
                contentFilter.FilterName = FilterName;
                contentFilter.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                contentFilter.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                ECN_Framework_BusinessLayer.Communicator.ContentFilter.Save(contentFilter, Master.UserSession.CurrentUser);
                lastInsertedFilterID = contentFilter.FilterID.ToString();

			    FilterAttribsPanel.Visible			= false;
			    AddFilterButtonPanel.Visible	= true;
                RuleNameDisplay.Visible = false;
			    FiltersPanel.Visible				= true;
			    FilterIDValue.Text					= lastInsertedFilterID;
			    FilterNameTxtValue.Text		= "";
			    CompValue.Text					= "";

			    loadFiltersGrid(getLayoutID(), getSlotNumber());	

			    FilterNameTxt.Text = "";
			    loadContentsDR(0, CustomerID);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }

		}

		public void AddFilter(object sender, EventArgs e)
        {
            try
            {
                var lastInsertedFilterId = Convert.ToInt32(FilterIDValue.Text);

                AddFiltersGridArray.DataSource = GetContentFilterDetailList(lastInsertedFilterId);
			    AddFiltersGridArray.DataBind();

                UpdateFilterWhereclause(lastInsertedFilterId);

			    RuleNameDisplay.Visible = true;
			    FilterIDValue.Text = lastInsertedFilterId.ToString();
			    FilterNameTxt.ReadOnly = true;
			    FilterNameTxt.Style.Add("background-color","#FCF8E9");

                var contentFilter = CommunicatorBusinessLayer.ContentFilter.GetByFilterID(
                    lastInsertedFilterId,
                    Master.UserSession.CurrentUser,
                    false);

                loadFiltersGrid(contentFilter.LayoutID.Value, contentFilter.SlotNumber.Value);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
		}

        public void UpdateFilterWhereclause(int FilterID)
        {
            try
            {
                ECN_Framework_Entities.Communicator.ContentFilter contentFilter = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByFilterID(FilterID, Master.UserSession.CurrentUser, false);
                List<ECN_Framework_Entities.Communicator.ContentFilterDetail> contentFilterDetailList = ECN_Framework_BusinessLayer.Communicator.ContentFilterDetail.GetByFilterID(FilterID, Master.UserSession.CurrentUser);
			    string whereClause	= "";
			    string fieldName		= "";
			    string comparator		= "";
			    string compareValue	= "";
			    string compareType	= "";
			    string fieldType		= "";
			    bool made_where = false;

			    foreach (ECN_Framework_Entities.Communicator.ContentFilterDetail contentFilterDetail in contentFilterDetailList ) {
                    compareType = contentFilterDetail.CompareType;
                    fieldName = contentFilterDetail.FieldName;
                    comparator = contentFilterDetail.Comparator;
				    compareValue	= contentFilterDetail.CompareValue;
                    fieldType = contentFilterDetail.FieldType;

				    if(fieldType.Equals("DATETIME") && comparator.Equals("equals"))
                    {
					    fieldName		= " CONVERT(varchar(10), "+fieldName+", 101) ";
					    compareValue	= " CONVERT(varchar(10), "+compareValue+", 101)";
				    }
				    string sub_where = "";

				    if(made_where){
					    sub_where += " "+compareType+" ";
				    }
				    made_where = true;
				    sub_where	+= fieldName;

				    compareValue = compareValue.Replace("Today", "getDate()");
				    compareValue = compareValue.Replace("[", "+(");
				    compareValue = compareValue.Replace("]", ")");
                    //removed '%' from 'ending','not ending', 'starting', 'not starting', 'contains', 'not contains' because it was getting the placement of '%' wrong. jWelter 05/05/2014
				    if(comparator.StartsWith("ending")){								//ending with
					    sub_where	+= " LIKE "+compareValue;
				    }else if(comparator.StartsWith("starting")){						//starting with
					    sub_where	+= " LIKE "+compareValue;	
				    }else if(comparator.Equals("contains")){							//contatins
					    sub_where	+= " LIKE "+compareValue;
				    }else if(comparator.Equals("equals")){								//equals
					    if(fieldType.Length > 0){
						    sub_where	+= " = "+compareValue+"";
					    }else{
						    sub_where	+= " = '"+compareValue+"' ";
					    }
				    }else if(comparator.Equals("greater than")){						//greater than
					    sub_where	+= " > "+compareValue+"";
				    }else if(comparator.Equals("less than")){							//less than
					    sub_where	+= " < "+compareValue+"";
				    }else if(comparator.Equals("between")){							//between dates
					    sub_where	+= " BETWEEN "+compareValue+"";
				    }else if(comparator.StartsWith("NOT ending")){					//NOT ending with
					    sub_where	+= " NOT LIKE "+compareValue;
				    }else if(comparator.StartsWith("NOT starting")){				//NOT starting with
					    sub_where	+= " NOT LIKE "+compareValue;
				    }else if(comparator.Equals("NOT contains")){					//NOT Contains with
					    sub_where	+= " NOT LIKE "+compareValue;
				    }else if(comparator.Equals("NOT equals")){						//NOT equals
					    if(fieldType.Length > 0){
						    sub_where	+= " <> "+compareValue+"";
					    }else{
						    sub_where	+= " <> '"+compareValue+"' ";
					    }
				    }else if(comparator.Equals("NOT greater than")){				//NOT greater than which means 'less than'
					    sub_where	+= " < "+compareValue+"";
				    }else if(comparator.Equals("NOT less than")){					//NOT greater than which means 'greater than'
					    sub_where	+= " > "+compareValue+"";
				    }else if(comparator.Equals("NOT between")){					//NOT between dates
					    sub_where	+= "NOT BETWEEN "+compareValue+"";
				    }
                    whereClause += sub_where;
			    }
                contentFilter.WhereClause= ECN_Framework_Common.Functions.StringFunctions.CleanString(whereClause);
                contentFilter.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                ECN_Framework_BusinessLayer.Communicator.ContentFilter.Save(contentFilter, Master.UserSession.CurrentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
		}

		public void EditFilter(int theFilterID)
        {
            ECN_Framework_Entities.Communicator.ContentFilter contentFilter = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByFilterID(Convert.ToInt32(theFilterID), Master.UserSession.CurrentUser,false);

            string theFilterName = contentFilter.FilterName;
			ContentPageToDisplay.Text = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(contentFilter.ContentID.Value ,Master.UserSession.CurrentUser, false).ContentTitle;
            string contentID = contentFilter.ContentID.ToString();
			ContentPagePreviewLabel.Text = "<a href='#' onClick=\"window.open('../content/contentPreview.aspx?ContentID="+contentID+"&type=html', 'Content_Preview', 'left=10,top=10,resizable=yes,scrollbar=yes,status=yes')\"><img src='/ecn.images/images/icon-preview-HTML.gif' alt='Preview Content as HTML'></a>&nbsp;<a href='#' onClick=\"window.open('../content/contentPreview.aspx?ContentID="+contentID+"&type=text', 'Content_Preview', 'left=10,top=10,resizable=yes,scrollbar=yes,status=yes')\"><img src='/ecn.images/images/icon-preview-TEXT.gif' alt='Preview Content as TEXT'></a>";
			FilterNameTxtLabel.Text = theFilterName;

            if (contentFilter.GroupID!=0) 
            {
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(contentFilter.GroupID.Value);
                UserDefinedFolder.Text = group.GroupName;

                List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(contentFilter.GroupID.Value);
			
				int i = CompFieldName.Items.Count;

                foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList) 
                {
                    CompFieldName.Items.Insert(i++, groupDataFields.ShortName);
				}
			} 
            else
            {
				UserDefinedFolder.Text = "No User Defined Fields";
			}

			FilterIDValue.Text = theFilterID.ToString();
			FilterAttribsPanel.Visible = true;
			AddFilterButtonPanel.Visible = false;
			RuleNameDisplay.Visible = true;
			FiltersPanel.Visible = false;

			loadAddFiltersGrid(theFilterID);
		}

		public void DeleteFilter(int theFilterID) 
        {
            try
            {
                ECN_Framework_Entities.Communicator.ContentFilter contentFilter = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByFilterID(Convert.ToInt32(theFilterID), Master.UserSession.CurrentUser, false);
                int LayoutID = contentFilter.LayoutID.Value;
                string SlotNumber = contentFilter.SlotNumber.ToString();
                ECN_Framework_BusinessLayer.Communicator.ContentFilter.Delete(contentFilter.ContentID.Value, contentFilter.FilterID, Master.UserSession.CurrentUser);
			    Response.Redirect("contentfilters.aspx?LayoutID="+LayoutID+ "&SlotNumber=" + SlotNumber);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
		}

        public void DeleteFilterDetail(int theFilterDetailID)
        {
            try
            {
                ECN_Framework_Entities.Communicator.ContentFilterDetail contentFilterDetail = ECN_Framework_BusinessLayer.Communicator.ContentFilterDetail.GetByFDID(Convert.ToInt32(theFilterDetailID), Master.UserSession.CurrentUser);
                ECN_Framework_BusinessLayer.Communicator.ContentFilterDetail.Delete(contentFilterDetail.FilterID.Value, theFilterDetailID, Master.UserSession.CurrentUser);
			    UpdateFilterWhereclause(contentFilterDetail.FilterID.Value);
                Response.Redirect("contentfilters.aspx?FilterID=" + contentFilterDetail.FilterID.Value + "&action=editFilter");
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
		}
		#endregion

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e){
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent(){    
			this.FirstAddFilterButton.Click += new EventHandler(DisplayAddFilterForm);
		}
		#endregion

		protected void ConvertToDataType_SelectedIndexChanged(object sender, System.EventArgs e) 
        {
			int CompFieldName_selIndex		= Convert.ToInt32(CompFieldName.SelectedIndex.ToString());
			int ConvertToDataType_selIndex	= Convert.ToInt32(ConvertToDataType.SelectedIndex.ToString());
			ErrorLabel.Text = "";
			ErrorLabel.Visible = false;

			if(ConvertToDataType_selIndex == 3){
				if((CompFieldName_selIndex < 21) || (CompFieldName_selIndex == 33)){
					ErrorLabel.Visible=true;
					ConvertToDataType.SelectedIndex = 0;
					BuildComparatorDR(0);
					ErrorLabel.Text = "ERROR: Compare Field that you have selected cannot be converted to DateTime. Please Correct";
				}else{
					BuildComparatorDR(3);
				}
			}else if(ConvertToDataType_selIndex == 2){
				BuildComparatorDR(2);	
				CompValueNumberValidator.Enabled = true;
			}else{
				CompValueNumberValidator.Enabled = false;
				BuildComparatorDR(ConvertToDataType_selIndex);	
				ErrorLabel.Visible=false;
			}
		}

        private List<CommunicatorEntities.ContentFilterDetail> GetContentFilterDetailList(int lastInsertedFilterId)
        {
            var cmpFieldName = $"[{CompFieldName.SelectedValue}]";
            var cmpComparator = Comparator.SelectedValue;
            if (ComparatorChkBox.Checked)
            {
                cmpComparator = $"NOT {cmpComparator}";
            }

            var cmpValue = CompValue.Text.Replace("'", string.Empty);
            var cmpFieldType = ConvertToDataType.SelectedValue;
            var cmpFieldTypeIndx = ConvertToDataType.SelectedIndex;

            var cmpValuesFixed = GetFixedCompareValues(cmpFieldTypeIndx, cmpFieldType, cmpComparator, cmpValue);
            var cmpFieldNameFixed = GetFixedCompareFieldName(cmpFieldTypeIndx, cmpFieldType, cmpFieldName);

            var contentFilterDetail = new CommunicatorEntities.ContentFilterDetail
            {
                FilterID = lastInsertedFilterId,
                FieldType = cmpFieldType,
                CompareType = CompType.SelectedValue,
                FieldName = cmpFieldNameFixed,
                Comparator = cmpComparator,
                CompareValue = cmpValuesFixed,
                CustomerID = Master.UserSession.CurrentUser.CustomerID,
                CreatedUserID = Master.UserSession.CurrentUser.UserID
            };

            CommunicatorBusinessLayer.ContentFilterDetail.Save(contentFilterDetail, Master.UserSession.CurrentUser);

            var contentFilterDetailList = CommunicatorBusinessLayer.ContentFilterDetail.GetByFilterID(
                lastInsertedFilterId,
                Master.UserSession.CurrentUser);
            return contentFilterDetailList;
        }

        private string GetFixedCompareValues(int fieldTypeIndex, string fieldType, string comparator, string valueToCompare)
        {
            if (fieldTypeIndex == 0)
            {
                return valueToCompare;
            }

            if (fieldTypeIndex > 0 && fieldTypeIndex < 3)
            {
                //Adding where clause parsing for 'ending','not ending','starting','not starting', 'contains', 'not contains' here because of misplaced '%' jWelter  05/05/2014
                if (comparator.StartsWith("ending"))
                {
                    return $"CONVERT ({fieldType}, \'%{valueToCompare}\')";
                }

                if (comparator.StartsWith("starting"))
                {
                    return $"CONVERT ({fieldType}, \'{valueToCompare}%\')";
                }

                if (comparator.StartsWith("contains"))
                {
                    return $"CONVERT ({fieldType}, \'%{valueToCompare}%\')";
                }

                if (comparator.ToLower().StartsWith("not ending"))
                {
                    return $"CONVERT ({fieldType}, \'%{valueToCompare}\')";
                }

                if (comparator.ToLower().StartsWith("not starting"))
                {
                    return $"CONVERT ({fieldType}, \'{valueToCompare}%\')";
                }

                if (comparator.ToLower().StartsWith("not contains"))
                {
                    return $"CONVERT ({fieldType}, \'%{valueToCompare}%\')";
                }
                
                return $"CONVERT ({fieldType}, \'{valueToCompare}\')";
            }

            if (fieldTypeIndex == 3)
            {
                var dateFrom = DtTime_Value1.Text;
                var dateTo = DtTime_Value2.Text;

                if (Comparator.SelectedValue.Equals("between"))
                {
                    return $"CONVERT ({fieldType}, \'{dateFrom}\') AND CONVERT ({fieldType}, \'{dateTo}\')";
                }

                return $"CONVERT (DATETIME, \'{dateFrom}\')";
            }

            if (fieldTypeIndex == 4)
            {
                return $"CONVERT ({fieldType}, \'{valueToCompare}\')";
            }

            return string.Empty;
        }

        private string GetFixedCompareFieldName(int fieldTypeIndex, string fieldType, string fieldName)
        {
            if (fieldTypeIndex == 0)
            {
                return fieldName;
            }

            if (fieldTypeIndex > 0 && fieldTypeIndex <= 4)
            {
                return $"CONVERT ({fieldType}, {fieldName})";
            }
            
            return string.Empty;
        }
    }
}