using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KMPS.MD.Objects;

namespace KMPS.MD.Controls
{
    public abstract class DownloadPanelBase : BaseControl
    {
        private const string ViewStateShowHeaderCheckBox = "ShowHeaderCheckBox";
        private const string ViewStateDcRunID = "dcRunID";
        private const string ViewStateFilterCombination = "filterCombination";
        private const string ViewStateDownloadCount = "downloadCount";
        private const string ViewStateDcTypeCodeID = "dcTypeCodeID";
        private const string ViewStateDcTargetCodeID = "dcTargetCodeID";
        private const string ViewStateMatchedRecordsCount = "matchedRecordsCount";
        private const string ViewStateNonMatchedRecordsCount = "nonMatchedRecordsCount";
        private const string ViewStateTotalFileRecords = "TotalFileRecords";
        private const string ViewStateTotalUADRecordCount = "TotalUADRecordCount";
        private const string ViewStatePubIDs = "PubIDs";
        private const string ViewStateSubscribersQueries = "SubscribersQueries";
        private const string ViewStateHeaderText = "HeaderText";
        private const string ViewStateVisibleCbIsRecentData = "VisibleCbIsRecentData";

        protected abstract PlaceHolder PhProfileFields { get; }
        protected abstract ListBox LstAvailableProfileFields { get; }
        protected abstract ListBox LstSelectedFields { get; }
        protected abstract PlaceHolder PhDemoFields { get; }
        protected abstract ListBox LstAvailableDemoFields { get; }
        protected abstract ListBox LstAvailableAdhocFields { get; }
        protected abstract DropDownList DrpIsBillable { get; }
        protected abstract Button BtnExport { get; }
        protected abstract PlaceHolder PlNotes { get; }
        protected abstract PlaceHolder PhShowHeader { get; }
        protected abstract CheckBox CbShowHeader { get; }
        protected abstract ModalPopupExtender MdlDownloads { get; }
        protected abstract TextBox TxtDownloadCount { get; }
        protected abstract TextBox TxtPromocode { get; }
        protected abstract RadioButton RbDownload { get; }
        protected abstract HtmlGenericControl DvDownloads { get; }
        protected abstract TextBox TxtNotes { get; }
        protected abstract HiddenField HfDownloadTemplateID { get; }
        protected abstract DataGrid DgDataCompareResult { get; }
        protected abstract PlaceHolder PlDataCompareResult { get; }
        protected abstract Label LblDataCompareMessage { get; }
        protected abstract HtmlGenericControl DivError { get; }
        protected abstract Label LblErrorMessage { get; }

        public bool ShowHeaderCheckBox
        {
            get
            {
                try
                {
                    return (bool)ViewState[ViewStateShowHeaderCheckBox];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState[ViewStateShowHeaderCheckBox] = value;
            }
        }
        
        public int dcRunID
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateDcRunID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateDcRunID] = value;
            }
        }

        public string filterCombination
        {
            get
            {
                try
                {
                    return (string)ViewState[ViewStateFilterCombination];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState[ViewStateFilterCombination] = value;
            }
        }

        public int downloadCount
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateDownloadCount];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateDownloadCount] = value;
            }
        }

        public int dcTypeCodeID
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateDcTypeCodeID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateDcTypeCodeID] = value;
            }
        }

        public int dcTargetCodeID
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateDcTargetCodeID];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateDcTargetCodeID] = value;
            }
        }

        public int matchedRecordsCount
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateMatchedRecordsCount];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateMatchedRecordsCount] = value;
            }
        }

        public int nonMatchedRecordsCount
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateNonMatchedRecordsCount];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateNonMatchedRecordsCount] = value;
            }
        }

        public int TotalFileRecords
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateTotalFileRecords];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateTotalFileRecords] = value;
            }
        }

        public int TotalUADRecordCount
        {
            get
            {
                try
                {
                    return (int)ViewState[ViewStateTotalUADRecordCount];
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState[ViewStateTotalUADRecordCount] = value;
            }
        }

        public List<int> PubIDs
        {
            get
            {
                try
                {
                    return (List<int>)ViewState[ViewStatePubIDs];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState[ViewStatePubIDs] = value;
            }
        }

        public StringBuilder SubscribersQueries
        {
            get
            {
                try
                {
                    return (StringBuilder)Session[ViewStateSubscribersQueries];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                Session[ViewStateSubscribersQueries] = value;
            }
        }
        
        public string HeaderText
        {
            get
            {
                try
                {
                    return (string)ViewState[ViewStateHeaderText];
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState[ViewStateHeaderText] = value;
            }
        }
        
        public bool VisibleCbIsRecentData
        {
            get
            {
                try
                {
                    return (bool)ViewState[ViewStateVisibleCbIsRecentData];
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                ViewState[ViewStateVisibleCbIsRecentData] = value;
            }
        }
        
    }
}