using System;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Configuration;

namespace ecn.communicator.classes
{
    public class PageWatch
    {
        #region Private Properties
        private int _PageWatchID;
        private int _AdminUserID;
        private string _Name;
        private string _URL;
        private DateTime _NextScheduleTime;
        private List<PageWatchTag> _PWT;
        private string _UserName;
        private bool _IsUpdated;
        #endregion

        #region Public Properties
        public int PageWatchID// { get; set; }
        {
            get
            {
                return _PageWatchID;
            }
            set
            {
                _PageWatchID = value;
            }
        }
        public int AdminUserID
        {
            get
            {
                return _AdminUserID;
            }
            set
            {
                _AdminUserID = value;
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        public string URL
        {
            get
            {
                return _URL;
            }
            set
            {
                _URL = value;
            }
        }
        public DateTime NextScheduleTime
        {
            get
            {
                return _NextScheduleTime;
            }
            set
            {
                _NextScheduleTime = value;
            }
        }
        public List<PageWatchTag> PWT
        {
            get
            {
                return _PWT;
            }
            set
            {
                _PWT = value;
            }
        }
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }
        public bool IsUpdated
        {
            get
            {
                return _IsUpdated;
            }
            set
            {
                _IsUpdated = value;
            }
        }
        public enum ScheduleType
        {
            Scheduled,
            Unscheduled,
            Both
        };
        #endregion

        #region Public Methods
        public static void UpdatePageRecord(int pageWatchID, DateTime nextScheduleTime)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update pagewatch set scheduletime = @ScheduleTime where pagewatchid = @PageWatchID";
            cmd.Parameters.Add(new SqlParameter("@PageWatchID", pageWatchID));
            cmd.Parameters.Add(new SqlParameter("@ScheduleTime", nextScheduleTime));
            DataFunctions.Execute(ConfigurationManager.AppSettings["connString"], cmd);
        }

        public static void SetInactive(int pageWatchID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update pagewatch set IsActive = 0 where pagewatchid = @PageWatchID";
            cmd.Parameters.Add(new SqlParameter("@PageWatchID", pageWatchID));
            DataFunctions.Execute(ConfigurationManager.AppSettings["connString"], cmd);
        }

        public static string ValidateURL(string url)
        {
            string errorMessage = string.Empty;

            try
            {
                // Open the requested URL
                WebRequest req = WebRequest.Create(url);
                // Get the stream from the returned web response
                StreamReader stream = new StreamReader(req.GetResponse().GetResponseStream());
            }
            catch (System.UriFormatException)
            {
                errorMessage = "Invalid URL: " + url;
            }
            catch (System.Net.WebException)
            {
                errorMessage = "Invalid URL: " + url;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }

            return errorMessage;
        }

        public static string GetPageWatchURL(int pageWatchID)
        {
            string url = string.Empty;
            string sqlSelect = "SELECT URL From PageWatch WHERE PageWatchID = @PageWatchID";
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PageWatchID", pageWatchID));
            try
            {
                url = DataFunctions.ExecuteScalar(cmd).ToString();
            }
            catch (Exception)
            {
            }
            return url;
        }

        public static List<PageWatch> GetPageRecordsForCustomer(int customerID, ScheduleType sType, PageWatchTag.ChangeType cType)
        {
            PageWatch pw = null;
            List<PageWatch> pwList = new List<PageWatch>();
            string sqlSelect = "Select pw.PageWatchID, pw.Name, pw.URL, pw.AdminUserID, u.UserName, " +
                                    "NextScheduleTime = CASE FrequencyType " +
                                        "WHEN 'HR' THEN DATEADD(HOUR, pw.FrequencyNo ,ISNULL(pw.ScheduleTime, GETDATE())) " +
                                        "WHEN 'DAY' THEN DATEADD(DAY, pw.FrequencyNo ,ISNULL(pw.ScheduleTime, GETDATE())) " +
                                        "WHEN 'WEEK' THEN DATEADD(WEEK, pw.FrequencyNo ,ISNULL(pw.ScheduleTime, GETDATE())) " +
                                        "WHEN 'MONTH' THEN DATEADD(MONTH, pw.FrequencyNo ,ISNULL(pw.ScheduleTime, GETDATE()))  END " +
                                "From PageWatch pw " +
                                    "JOIN ecn5_accounts..Users u on pw.AdminUserID = u.UserID " +
                                "Where pw.IsActive = 'true' And pw.CustomerID = @CustomerID ";
                                if (sType == ScheduleType.Scheduled)
                                {
                                    sqlSelect += "And (pw.ScheduleTime IS NULL Or pw.ScheduleTime <= GETDATE()) ";
                                }
                                else if (sType == ScheduleType.Unscheduled)
                                {
                                    sqlSelect += "And (pw.ScheduleTime IS NOT NULL AND pw.ScheduleTime > GETDATE()) ";
                                }
                                sqlSelect += "Order By pw.AdminUserID ASC, pw.PageWatchID ASC";

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pw = new PageWatch();
                    if (reader["PageWatchID"] != DBNull.Value)
                    {
                        pw._PageWatchID = (int)reader["PageWatchID"];
                    }
                    if (reader["AdminUserID"] != DBNull.Value)
                    {
                        pw._AdminUserID = (int)reader["AdminUserID"];
                    }
                    if (reader["Name"] != DBNull.Value)
                    {
                        pw._Name = (string)reader["Name"];
                    }
                    if (reader["URL"] != DBNull.Value)
                    {
                        pw._URL = (string)reader["URL"];
                    }
                    if (reader["NextScheduleTime"] != DBNull.Value)
                    {
                        pw._NextScheduleTime = Convert.ToDateTime(reader["NextScheduleTime"].ToString());
                    }
                    if (reader["UserName"] != DBNull.Value)
                    {
                        pw._UserName = (string)reader["UserName"];
                    }
                    pw._PWT = PageWatchTag.GetTagRecords(pw._PageWatchID, cType);
                    pwList.Add(pw);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return pwList;
        }
        #endregion
    }

    public class PageWatchTag
    {
        #region Private Properties
        private bool _IsChanged;
        private DateTime _DateChanged;
        private int _PageWatchTagID;
        private int _PageWatchID;
        private string _WatchTag;
        private string _Name;
        private string _PreviousHTML;
        #endregion

        #region Public Properties
        public bool IsChanged
        {
            get
            {
                return _IsChanged;
            }
            set
            {
                _IsChanged = value;
            }
        }
        public DateTime DateChanged
        {
            get
            {
                return _DateChanged;
            }
            set
            {
                _DateChanged = value;
            }
        }
        public int PageWatchTagID
        {
            get
            {
                return _PageWatchTagID;
            }
            set
            {
                _PageWatchTagID = value;
            }
        }
        public int PageWatchID
        {
            get
            {
                return _PageWatchID;
            }
            set
            {
                _PageWatchID = value;
            }
        }
        public string WatchTag
        {
            get
            {
                return _WatchTag;
            }
            set
            {
                _WatchTag = value;
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        public string PreviousHTML
        {
            get
            {
                return _PreviousHTML;
            }
            set
            {
                _PreviousHTML = value;
            }
        }
        public enum ChangeType
        {
            Changed,
            Unchanged,
            Both
        };
        #endregion

        #region Public Methods

        public static void SetInactive(int pageWatchTagID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update pagewatchtag set IsActive = 0 where pagewatchtagid = @PageWatchTagID";
            cmd.Parameters.Add(new SqlParameter("@PageWatchTagID", pageWatchTagID));
            DataFunctions.Execute(ConfigurationManager.AppSettings["connString"], cmd);
        }

        public static void UpdateTagRecord(int pageWatchTagID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update pagewatchtag set ischanged = 1, datechanged = GETDATE() where pagewatchtagid = @PageWatchTagID";
            cmd.Parameters.Add(new SqlParameter("@PageWatchTagID", pageWatchTagID));
            DataFunctions.Execute(ConfigurationManager.AppSettings["connString"], cmd);
        }

        public static List<PageWatchTag> GetTagRecords(int pageWatchID, ChangeType cType)
        {
            PageWatchTag pwt = null;
            List<PageWatchTag> pwtList = new List<PageWatchTag>();
            string sqlSelect = "Select PageWatchID, WatchTag, PageWatchTagID, Name, PreviousHTML, IsChanged, DateChanged " +
                                "From PageWatchTag " +
                                "Where PageWatchID = @PageWatchID And IsActive = 'true' ";
                                if (cType == ChangeType.Unchanged)
                                {
                                    sqlSelect += "And IsChanged = 'false' ";
                                }
                                else if (cType == ChangeType.Changed)
                                {
                                    sqlSelect += "And IsChanged = 'true' ";
                                }
                                sqlSelect += "Order By PageWatchID ASC, PageWatchTagID ASC";

            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.Add(new SqlParameter("@PageWatchID", pageWatchID));
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pwt = new PageWatchTag();
                    if (reader["PageWatchTagID"] != DBNull.Value)
                    {
                        pwt._PageWatchTagID = (int)reader["PageWatchTagID"];
                    }
                    if (reader["PageWatchID"] != DBNull.Value)
                    {
                        pwt._PageWatchID = (int)reader["PageWatchID"];
                    }
                    if (reader["WatchTag"] != DBNull.Value)
                    {
                        pwt._WatchTag = (string)reader["WatchTag"];
                    }
                    if (reader["Name"] != DBNull.Value)
                    {
                        pwt._Name = (string)reader["Name"];
                    }
                    if (reader["PreviousHTML"] != DBNull.Value)
                    {
                        pwt._PreviousHTML = (string)reader["PreviousHTML"];
                    }
                    if (reader["DateChanged"] != DBNull.Value)
                    {
                        pwt._DateChanged = Convert.ToDateTime(reader["DateChanged"].ToString());
                    }
                    if (reader["IsChanged"] != DBNull.Value)
                    {
                        pwt._IsChanged = Convert.ToBoolean(reader["IsChanged"].ToString());
                    }
                    pwtList.Add(pwt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return pwtList;
        }

        public static string ValidateAllTags(List<PageWatchTag> tagList, string url)
        {
            string errorMessage = string.Empty;
            string currentTag = string.Empty;
            try
            {
                foreach (PageWatchTag tag in tagList)
                {
                    currentTag = tag._WatchTag;
                    GetCurrentHTML(tag._WatchTag, url);
                }
            }
            catch (System.UriFormatException)
            {
                errorMessage = "Invalid URL: " + url;
            }
            catch (System.Net.WebException)
            {
                errorMessage = "Invalid URL: " + url;
            }
            catch (ArgumentOutOfRangeException)
            {
                errorMessage = "Invalid Tag: " + currentTag + " in URL: " + url;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }

            return errorMessage;
        }

        public static string ValidateTag(string tag, string url)
        {
            string errorMessage = string.Empty;
            try
            {
                GetCurrentHTML(tag, url);                
            }
            catch (System.UriFormatException)
            {
                errorMessage = "Invalid URL: " + url;
            }
            catch (System.Net.WebException)
            {
                errorMessage = "Invalid URL: " + url;
            }
            catch (ArgumentOutOfRangeException)
            {
                errorMessage = "Invalid Tag: " + tag + " in URL: " + url;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }

            return errorMessage;
        }

        public static bool CompareHTML(string previous, string current)
        {
            bool htmlMatch = false;
            //WGH - added due to encoding issues
            //insert temp record with html
            string insertSQL = "insert into ecn_temp..tempPageWatch (tempHTML) values (@currentHTML); select @@identity;";
            SqlCommand tempCMD = new SqlCommand();
            tempCMD.CommandType = CommandType.Text;
            tempCMD.CommandText = insertSQL;
            tempCMD.Parameters.AddWithValue("@currentHTML", current);
            int currHTMLID = Convert.ToInt32(DataFunctions.ExecuteScalar(tempCMD));

            //get temp record from db
            string sqlSelect = "Select tempHTML From ecn_temp..tempPageWatch Where htmlID = @htmlID ";
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.Add(new SqlParameter("@htmlID", currHTMLID));
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmd.Connection = conn;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["tempHTML"] != DBNull.Value)
                    {
                        current = (string)reader["tempHTML"];
                        break;
                    }
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
            }

            //delete temp record
            string sqlDelete = "DELETE ecn_temp..tempPageWatch WHERE htmlID = @htmlID";
            tempCMD = new SqlCommand();
            tempCMD.CommandType = CommandType.Text;
            tempCMD.CommandText = sqlDelete;
            tempCMD.Parameters.AddWithValue("@htmlID", currHTMLID);
            DataFunctions.Execute(tempCMD);

            if (previous == current)
            {
                htmlMatch = true;
            }

            return htmlMatch;
        }

        public static string GetCurrentHTML(string tag, string url)
        {
            string currentHTML = "";

            // Open the requested URL
            WebRequest req = WebRequest.Create(url);
            // Get the stream from the returned web response
            StreamReader stream = new StreamReader(req.GetResponse().GetResponseStream());
            // Get the stream from the returned web response
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string strLine;
            // Read the stream a line at a time and place each one
            // into the stringbuilder
            while ((strLine = stream.ReadLine()) != null)
            {
                // Ignore blank lines
                if (strLine.Length > 0)
                    sb.Append(strLine);
            }
            // Finished with the stream so close it now
            stream.Close();
            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(sb.ToString());
            string tempTag = tag.Replace("<", "");
            tempTag = tempTag.Replace(">", "");
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//" + tempTag.ToLower());
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    currentHTML = node.InnerHtml;
                    break;
                }
            }

            

            return currentHTML;
        }
        #endregion
    }

    public class AdminEmail
    {
        #region Private Properties
        private int _AdminUserID;
        private string _UserName;
        private List<String> _PageChanges;
        #endregion

        #region Public Properties
        public int AdminUserID
        {
            get
            {
                return _AdminUserID;
            }
            set
            {
                _AdminUserID = value;
            }
        }
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }
        public List<string> PageChanges
        {
            get
            {
                return _PageChanges;
            }
            set
            {
                _PageChanges = value;
            }
        }
        #endregion

        //#region Public Methods
        //public static AdminUser GetAdminUser(int adminUserID)
        //{
        //    AdminUser au = null;
        //    string sqlSelect = "Select UserID, UserName " +
        //                        "From ecn5_accounts..Users " + 
        //                        "Where UserID = @UserID";

        //    SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);
        //    SqlCommand cmd = new SqlCommand(sqlSelect);
        //    cmd.Parameters.Add(new SqlParameter("@UserID", adminUserID));
        //    cmd.CommandType = CommandType.Text;

        //    SqlDataReader reader = null;
        //    try
        //    {
        //        conn.Open();
        //        cmd.Connection = conn;
        //        reader = cmd.ExecuteReader();
        //        if (reader.Read()) 
        //        {
        //            au._UserID = adminUserID;
        //            if (reader["UserName"] != DBNull.Value)
        //            {
        //                au._UserName = (string)reader["UserName"];
        //            } 
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        reader.Close();
        //        conn.Close();
        //    }
        //    return au;
        //}
        //#endregion
    }
}
