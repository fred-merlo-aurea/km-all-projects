using System;
using System.Web.UI;
using System.IO;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Interfaces;
using ECN_Framework_BusinessLayer.Communicator;
using System.Data;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace ECN_Framework
{
    public class WebPageHelper : Page
    {
        private const string DefaultTheme = "1";
        private const string UDFName = "UDFName";
        private const string UDFData = "UDFData";
        protected const string BlastManager = "Blast Manager";
        private const string LikeSearchSyntax = " AND (EmailAddress like '%{0}%' ) ";
        private const string EqualsSearchSyntax = " AND (EmailAddress like '{0}' ) ";
        private const string EndsWithSearchSyntax = " AND (EmailAddress like '%{0}' ) ";
        private const string StartsWithSearchSyntax = " AND (EmailAddress like '{0}%' ) ";
        private const string FilterStrNotEmty = " AND ( {0})";
        private const string EqualsSearchCriteria = "equals";
        private const string EndsWithSearchCriteria = "ends";
        private const string StartsWithSearchCriteria = "starts";
        private const string LikeSearchCriteria = "like";
        private const string ExcelFileExtension = ".xls";
        private const string TextFileExtension = ".txt";
        private const string CsvFileExtension = ".csv";
        private const string XmlFileExtension = ".xml";
        private const string CsvFileContentType = "text/csv";
        private const string XmlFileContentType = "text/xml";
        private const string ExcelFileContentType = "application/vnd.ms-excel";
        private const string TabChar = "\t";
        private const string NewLineChar = "\n";
        private const string CarriageReturnChar = "\r";
        private const string CommaChar = ",";
        private const string WildcardChar = "*";
        private const string DoubleQuoteChar = @"""";
        private const string ContentDispositionHeaderName = "content-disposition";
        private const string AttachmentHeaderName = "attachment; filename=";
        private const string SingleQuoteOpenChar = @" '";
        private const string SingleQuoteCloseChar = @"' ";
        private const string CampaignItemIdName = "CampaignItemID";
        private const string Ascending = "ASC";
        private const string Descending = "DESC";
        private ECNSession _ecnSession;
        private ISessionDataProvider _sessionDataProvider;

        protected ISessionDataProvider SessionDataProvider
        {
            get
            {
                return SafeGetSessionDataProvider(_ecnSession);
            }
            set
            {
                _sessionDataProvider = value; 
            }
        }

        protected virtual int getCampaignItemID()
        {
            return RequestQueryString(CampaignItemIdName, 0);
        }

        protected T RequestQueryString<T>(string key, T defaultValue)
        {
            try
            {
                return (T)Convert.ChangeType(Request.QueryString[key], defaultValue.GetType());
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            InitializeTheme();

            base.OnPreInit(e);
        }

        protected virtual void InitializeTheme()
        {
            _ecnSession = ECNSession.CurrentSession();

            if (_ecnSession != null)
            {
                try
                {
                    var baseChannelServerPath = "~/App_Themes/" + _ecnSession.CurrentBaseChannel.BaseChannelID;
                    if (Directory.Exists(Server.MapPath(baseChannelServerPath)))
                    {
                        Theme = _ecnSession.CurrentBaseChannel.BaseChannelID.ToString();
                    }
                    else
                    {
                        Theme = DefaultTheme;
                    }
                }
                catch
                {
                    Theme = DefaultTheme;
                }
            }
        }

        protected ISessionDataProvider SafeGetSessionDataProvider(ECNSession ecnSession)
        {
            return _sessionDataProvider ?? (_sessionDataProvider = new ECNSessionDataProvider(ecnSession));
        }

        public string getUDFName()
        {
            try
            {
                return Request.QueryString[UDFName];
            }
            catch
            {
                return string.Empty;
            }
        }

        public string getUDFData()
        {
            try
            {
                return Request.QueryString[UDFData];
            }
            catch
            {
                return string.Empty;
            }
        }

        public static ArrayList GetDataTableColumns(DataTable dataTable)
        {
            var columnHeadings = new ArrayList();
            for(var i = 0; i < dataTable.Columns.Count; i++)
            {
                var dataColumn = dataTable.Columns[i];
                columnHeadings.Add(dataColumn.ColumnName);
            }

            return columnHeadings;
        }

        public static void PopulateResponse(string osFilePath, string outFileName, string downloadType,
            DataTable emailProfilesDt, string delimiter, string tfile, HttpResponseBase response)
        {
            if(!Directory.Exists(osFilePath))
            {
                Directory.CreateDirectory(osFilePath);
            }

            if(File.Exists(outFileName))
            {
                File.Delete(outFileName);
            }

            TextWriter txtfile = File.AppendText(outFileName);
            try
            {

                if(downloadType.Equals(ExcelFileExtension) || downloadType.Equals(CsvFileExtension) ||
                   downloadType.Equals(TextFileExtension))
                {
                    var newline = new StringBuilder();

                    var columnHeadings = GetDataTableColumns(emailProfilesDt);
                    var aListEnum = columnHeadings.GetEnumerator();

                    while(aListEnum.MoveNext())
                    {
                        newline.Append(downloadType.Equals(CsvFileExtension) ? DoubleQuoteChar : string.Empty);
                        newline.Append(aListEnum.Current);
                        newline.Append(downloadType.Equals(CsvFileExtension) ? DoubleQuoteChar : string.Empty);
                        newline.Append(delimiter);
                    }

                    txtfile.WriteLine(newline.ToString());

                    foreach(DataRow dataRow in emailProfilesDt.Rows)
                    {
                        newline = new StringBuilder();
                        aListEnum.Reset();
                        while(aListEnum.MoveNext())
                        {
                            var columnText = string.Empty;
                            if(aListEnum.Current != null)
                            {
                                columnText = dataRow[aListEnum.Current.ToString()].ToString().Replace(NewLineChar, string.Empty);
                            }

                            columnText = columnText.Replace(CarriageReturnChar, string.Empty);
                            columnText = columnText.Replace(DoubleQuoteChar, string.Empty);
                            newline.Append((downloadType.Equals(CsvFileExtension) ? DoubleQuoteChar : string.Empty) + columnText +
                                           (downloadType.Equals(CsvFileExtension) ? DoubleQuoteChar : string.Empty) + delimiter);
                        }

                        txtfile.WriteLine(newline.ToString());
                    }
                }
                else if(downloadType.Equals(XmlFileExtension))
                {
                    using(var emailProfilesDs = new DataSet())
                    {
                        emailProfilesDs.Tables.Add(emailProfilesDt);
                        emailProfilesDs.WriteXml(txtfile);
                    }
                }
            }
            finally
            {
                txtfile.Close();
            }

            switch(downloadType)
            {
                case ExcelFileExtension:
                    response.ContentType = ExcelFileContentType;
                    break;
                case CsvFileExtension:
                case TextFileExtension:
                    response.ContentType = CsvFileContentType;
                    break;
                case XmlFileExtension:
                    response.ContentType = XmlFileContentType;
                    break;
            }

            response.AddHeader(ContentDispositionHeaderName, AttachmentHeaderName + tfile);
            response.WriteFile(outFileName);
            response.Flush();
            response.End();
        }

        public static string PopulateFilter(string downloadType, string emailAddr, string searchType,
            int filterId, string subscribeTypeAll, ref string subscribeType, out string delimiter)
        {
            delimiter = string.Empty;
            string filter;

            delimiter = downloadType.Equals(ExcelFileExtension) || downloadType.Equals(TextFileExtension) ? TabChar : CommaChar;

            subscribeType = subscribeType.Equals(WildcardChar) ? subscribeTypeAll : SingleQuoteOpenChar + subscribeType + SingleQuoteCloseChar;

            if(emailAddr.Length > 0)
            {
                switch(searchType)
                {
                    case EqualsSearchCriteria:
                        filter = string.Format(EqualsSearchSyntax, emailAddr);
                        break;
                    case EndsWithSearchCriteria:
                        filter = string.Format(EndsWithSearchSyntax, emailAddr);
                        break;
                    case StartsWithSearchCriteria:
                        filter = string.Format(StartsWithSearchSyntax, emailAddr);
                        break;
                    case LikeSearchCriteria:
                    default:
                        filter = string.Format(LikeSearchSyntax, emailAddr);
                        break;
                }
            }
            else
            {
                filter = string.Empty;
            }

            var filterStr = string.Empty;
            if(filterId > 0)
            {
                var filterObject =
                    Filter.GetByFilterID(filterId,
                        ECNSession.CurrentSession().CurrentUser);
                filterStr = filterObject.WhereClause;
            }

            if(!filterStr.Equals(string.Empty))
            {
                filter += string.Format(FilterStrNotEmty, filterStr);
            }

            return filter;
        }

        protected void SetSortCommand(string sortField, string sortDirection, string sortExpression)
        {
            if (sortExpression == ViewState[sortField].ToString())
            {
                switch (ViewState[sortDirection].ToString())
                {
                    case Ascending:
                        ViewState[sortDirection] = Descending;
                        break;
                    case Descending:
                        ViewState[sortDirection] = Ascending;
                        break;
                }
            }
            else
            {
                ViewState[sortField] = sortExpression;
                ViewState[sortDirection] = Ascending;
            }
        }
        
        public static int IntTryParse(string input)
        {
            int returnValue;

            if (!int.TryParse(input, out returnValue))
            {
                throw new InvalidCastException($"{input} cannot be parsed to integer");
            }

            return returnValue;
        }
    }
}