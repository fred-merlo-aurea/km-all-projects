using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Collector;
using ECN_Framework_BusinessLayer.Communicator;

namespace ecn.collector.main.report
{
    public partial class download : System.Web.UI.Page
    {
        private const string CsvExtension = ".csv";
        private const string XmlExtension = ".xml";
        private const string XlsExtension = ".xls";
        private const string Quote = "\"";
        private const string TxtFileExtension = ".txt";
        private const string ImagesVirtualPath = "Images_VirtualPath";
        private const string CarriageReturn = "\r";
        private const string NewLine = "\n";
        private const string Tab = "\t";
        private const string Comma = ",";

        private int FilterID
        {
            get
            {
                int filterID=0;
                if (Request.QueryString["FilterID"] != null)
                {
                    filterID=Convert.ToInt32(Request.QueryString["FilterID"]);
                }
                return filterID;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write($@"<link href='/ecn.accounts/styles/progressbar.css' type='text/css' rel='stylesheet' />
              <body><script type='text/javascript' src='/ecn.accounts/scripts/ProgressBar.js'></script>
              <script language='javascript' type='text/javascript'>
              initDownLoader();
              </script></body>");
            Response.Flush();
            DownloadLink.Visible = false;
            Response.Flush();

            var customerId = Request.QueryString["custID"];
            var groupId = Request.QueryString["grpID"];
            var downloadType = Request.QueryString["fileType"];
            var httpFilePath =
                $"{ConfigurationManager.AppSettings[ImagesVirtualPath]}/customers/{customerId}/downloads/";
            var osFilePath = Server.MapPath(
                $"{ConfigurationManager.AppSettings[ImagesVirtualPath]}/customers/{customerId}/downloads/");
            var delimiter = downloadType.Equals(XlsExtension) || downloadType.Equals(TxtFileExtension) ? Tab : Comma;

            var emailProfilesDt = Participant.ExportParticipants(Convert.ToInt32(groupId), Convert.ToInt32(customerId));
            var fileName = $"{customerId}_{groupId}_emails{downloadType}";
            var fileWithPath = osFilePath + fileName;

            if(!Directory.Exists(osFilePath))
            {
                Directory.CreateDirectory(osFilePath);
            }

            if(File.Exists(fileWithPath))
            {
                File.Delete(fileWithPath);
            }

            WriteFile(fileWithPath, downloadType, emailProfilesDt, delimiter);

            DownloadLink.NavigateUrl = httpFilePath + fileName;
            DownloadLink.Visible = true;
            Response.Write($@"<link href='/ecn.accounts/styles/progressbar.css' type='text/css' rel='stylesheet' />
              body><script type='text/javascript' src='/ecn.accounts/scripts/ProgressBar.js'></script>
              <script language='javascript' type='text/javascript'>
              completed();
              </script></body>");
        }

        private void WriteFile(string fileWithPath, string downloadType, DataTable emailProfilesDt, string delimiter)
        {
            TextWriter txtfile = File.AppendText(fileWithPath);
            try
            {
                if (string.IsNullOrWhiteSpace(downloadType))
                {
                    throw new ArgumentNullException(nameof(downloadType));
                }

                if (downloadType.Equals(XlsExtension) ||
                   downloadType.Equals(CsvExtension) ||
                   downloadType.Equals(TxtFileExtension))
                {
                    var newline = new StringBuilder();
                    var columnHeadings = GetDataTableColumns(emailProfilesDt);
                    var aListEnum = columnHeadings.GetEnumerator();

                    while(aListEnum.MoveNext())
                    {
                        newline.AppendFormat("{0}{1}{0}{2}", downloadType.Equals(CsvExtension) 
                                ? Quote 
                                : string.Empty,
                            aListEnum.Current, 
                            delimiter);
                    }

                    txtfile.WriteLine(newline);
                    foreach(DataRow dr in emailProfilesDt.Rows)
                    {
                        newline = new StringBuilder();
                        aListEnum.Reset();
                        while(aListEnum.MoveNext())
                        {
                            if(aListEnum.Current != null)
                            {
                                var columnText = dr[aListEnum.Current.ToString()].ToString()
                                    .Replace(NewLine, string.Empty);
                                columnText = columnText.Replace(CarriageReturn, string.Empty);
                                columnText = columnText.Replace(Quote, string.Empty);
                                newline.Append(
                                    $"{(downloadType.Equals(CsvExtension) ? Quote : string.Empty)}{columnText}{(downloadType.Equals(CsvExtension) ? Quote : string.Empty)}{delimiter}");
                            }
                        }

                        txtfile.WriteLine(newline);
                    }
                }
                else
                {
                    if(downloadType.Equals(XmlExtension))
                    {
                        using(var emailProfilesDs = new DataSet())
                        {
                            emailProfilesDs.Tables.Add(emailProfilesDt);
                            emailProfilesDs.WriteXml(txtfile);
                        }
                    }
                }
            }
            finally
            {
                txtfile.Close();
            }
        }

        public ArrayList GetDataTableColumns(DataTable dataTable)
        {
            int nColumns = dataTable.Columns.Count;
            ArrayList columnHeadings = new ArrayList();
            DataColumn dataColumn = null;
            for (int i = 0; i < nColumns; i++)
            {
                dataColumn = dataTable.Columns[i];
                columnHeadings.Add(dataColumn.ColumnName.ToString());
            }
            return columnHeadings;
        }
        
    }
}
