using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Xml;

namespace KMPS.MD.Objects
{

    public class ScheduledFields
    {
        public ScheduledFields()
        {

        }
        public string FieldName { get; set; }
        public string FieldAlias { get; set; }

        public static DataTable GetAvailableFieldsForList()
        {
            string sqlquery = " select sc.name as FieldName, 's.' + sc.name as FieldAlias from sysobjects so join syscolumns sc on so.id = sc.id where so.name = 'Subscriptions' and sc.name not in ('EMAIL','FNAME','LNAME','COMPANY','TITLE','ADDRESS','MAILSTOP','CITY','STATE','ZIP','PLUS4','COUNTRY','FORZIP','PHONE','MOBILE','FAX','CategoryID','TransactionID','pubids','Demo31','Demo32','Demo33','Demo34','Demo35','Demo36','SubscriptionID', 'SEQUENCE', 'CountryID','IsExcluded','PubID','STATLIST','IGRP_CNT','CGRP_NO','CGRP_CNT', 'EmailExists','FaxExists','PhoneExists','PRIORITY','Origssrc','SALES','Source','subsrc','Latitude','Longitude','IsLatLonValid','LatLonMsg','verified') " +
                                "UNION " +
                                "select DisplayName as FieldName, 'im.' + ColumnReference as FieldAlias from MasterGroups where IsActive = 1 " +
                                "order by FieldName asc";
            SqlCommand cmd = new SqlCommand(sqlquery);
            cmd.CommandType = CommandType.Text;
            return DataFunctions.getDataTable(cmd, new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString()));
        }
    }

    public class ScheduledExports
    {
        public ScheduledExports() 
        {
            SchedID = -1;
            SchedName = string.Empty;
            FTPHost = string.Empty;
            FTPPort = string.Empty;
            FTPUser = string.Empty;
            FTPPass = string.Empty;
            FieldsXML = string.Empty;
            SchedXML = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            IsActive = null;
            FormatType = string.Empty;
            IsDaily = null;
            MonthlyDate = string.Empty;
            Sunday = null;
            Monday = null;
            Tuesday = null;
            Wednesday = null;
            Thursday = null;
            Friday = null;
            Saturday = null;
            FieldsList = new List<ScheduledFields>();
            FiltersList = new List<ScheduledFilters>();
        }

        #region Properties
        public int SchedID { get; set; }
        public string SchedName { get; set; }
        public string FTPHost { get; set; }
        public string FTPPort { get; set; }
        public string FTPUser { get; set; }
        public string FTPPass { get; set; }
        public string FieldsXML { get; set; }
        public string SchedXML { get; set; }
        public int? CreatedUserID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedUserID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string FormatType { get; set; }
        //additional info
        public bool? IsDaily { get; set; }
        public string MonthlyDate { get; set; }
        public bool? Sunday { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }        //
        public List<ScheduledFields> FieldsList { get; set; }
        public List<ScheduledFilters> FiltersList { get; set; }
        #endregion


        public static DataTable GetAllForGrid()
        {
            string sqlquery = "SELECT s.SchedID, s.SchedName, s.FormatType, u.UserName as LastUser, IsNull(s.UpdatedDate, s.CreatedDate) as LastDate, s.IsActive " +
                                "FROM ScheduledExports s JOIN ecn5_accounts..Users u on IsNull(s.UpdatedUserID, s.CreatedUserID) = u.UserID " +
                                "ORDER BY s.SchedName ASC";
            SqlCommand cmd = new SqlCommand(sqlquery);
            cmd.CommandType = CommandType.Text;
            return DataFunctions.getDataTable(cmd, new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString()));



            //<asp:BoundField DataField="SchedName" HeaderText="Name" SortExpression="SchedName">
            //            <HeaderStyle HorizontalAlign="Left" Width="40%" />
            //        </asp:BoundField>
            //        <asp:BoundField DataField="FormatType" HeaderText="Format" SortExpression="FormatType">
            //            <HeaderStyle HorizontalAlign="Left" Width="10%" />
            //        </asp:BoundField>
            //        <asp:BoundField DataField="LastUser" HeaderText="EditedBy" SortExpression="LastUser"                        >
            //            <HeaderStyle HorizontalAlign="Left" Width="10%" />
            //        </asp:BoundField>
            //        <asp:BoundField DataField="LastDate" HeaderText="EditedDate" SortExpression="LastDate" ItemStyle-HorizontalAlign="Center">
            //            <HeaderStyle HorizontalAlign="Center" Width="10%" />
            //        </asp:BoundField>
            //        <asp:TemplateField HeaderText="Active" SortExpression="IsActive"
            //            ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
            //            <ItemTemplate>
            //                <%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%></ItemTemplate>
            //        </asp:TemplateField>
            //        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="10%" ItemStyle-Width="10%"
            //            ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
            //            FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
            //            <ItemTemplate>
            //                <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("SchedID")%>'
            //                    OnCommand="lnkEdit_Command"><img src="../Images/ic-edit.gif" alt=""/></asp:LinkButton>
            //            </ItemTemplate>
            //        </asp:TemplateField>
            //        <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="10%" ItemStyle-Width="10%"
            //            ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
            //            FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
            //            <ItemTemplate>
            //                <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("SchedID")%>'
            //                    OnCommand="lnkDelete_Command"><img src="../Images/icon-delete.gif" alt=""/></asp:LinkButton>
            //            </ItemTemplate>
            //        </asp:TemplateField>
        }
        

        public static List<ScheduledExports> GetAll()
        {
            List<ScheduledExports> retList = new List<ScheduledExports>();
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
            SqlCommand cmd = new SqlCommand("SELECT * FROM ScheduledExports ORDER BY ISNULL(UpdatedDate, CreatedDate) DESC", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                retList = ExcRdrList(rdr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList; 
        }

        private static List<ScheduledExports> ExcRdrList(SqlDataReader rdr)
        {
            List<ScheduledExports> retlist = new List<ScheduledExports>();

            #region Reader
            while (rdr.Read())
            {
                ScheduledExports retItem = new ScheduledExports();
                int index;
                string name;

                #region Reader

                name = "SchedID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.SchedID = Convert.ToInt32(rdr[index].ToString());

                name = "SchedName";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.SchedName = rdr[index].ToString();

                name = "FTPHost";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.FTPHost = rdr[index].ToString();

                name = "FTPPort";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.FTPPort = rdr[index].ToString();

                name = "FTPUser";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.FTPUser = rdr[index].ToString();

                name = "FTPPass";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.FTPPass = rdr[index].ToString();

                name = "FieldsXML";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.FieldsXML = rdr[index].ToString();

                name = "SchedXML";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.SchedXML = rdr[index].ToString();

                name = "FormatType";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.FormatType = rdr[index].ToString();

                name = "CreatedUserID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.CreatedUserID = Convert.ToInt32(rdr[index].ToString());

                name = "CreatedDate";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.CreatedDate = Convert.ToDateTime(rdr[index].ToString());

                name = "UpdatedUserID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.UpdatedUserID = Convert.ToInt32(rdr[index].ToString());

                name = "UpdatedDate";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.UpdatedDate = Convert.ToDateTime(rdr[index].ToString());

                name = "IsDeleted";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.IsDeleted = Convert.ToBoolean(rdr[index].ToString());

                name = "IsActive";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.IsActive = Convert.ToBoolean(rdr[index].ToString());

                retItem.FieldsList = GetScheduledFields(retItem.FieldsXML);

                retItem.FiltersList = ScheduledFilters.GetByScheduleID(retItem.SchedID);

                GetSchedule(retItem);

                retlist.Add(retItem);

                #endregion
            }

            #endregion

            return retlist;

        }

        private static List<ScheduledFields> GetScheduledFields(string fieldsXML)
        {
            //parse xml
            //FieldsXML
            //<?xml version="1.0" encoding="utf-8"?>
            //<Fields>
            //  <Field>
            //    <FieldName>emailaddress</FieldName>
            //    <FieldAlias>s.somevalue</FieldAlias>
            //  </Field>
            //</Fields>

            List<ScheduledFields> retlist = new List<ScheduledFields>();
            ScheduledFields retItem = null;            

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fieldsXML);
            XmlNode node = doc.SelectSingleNode("//Fields");
            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "Field")
                    {
                        retItem = new ScheduledFields();
                        foreach (XmlNode innerChild in child.ChildNodes)
                        {
                            if (child.Name == "FieldName")
                            {
                                retItem.FieldName = child.Value;
                            }
                            else if (child.Name == "FieldAlias")
                            {
                                retItem.FieldAlias = child.Value;
                            }
                        }
                        retlist.Add(retItem);
                    }
                }
            }
            return retlist;
        }

        private static void GetSchedule(ScheduledExports schedExp)
        {
            //parse xml
            //<?xml version="1.0" encoding="utf-8"?>
            //<Schedule>
            //  <Days>
            //    <Sunday>True</Sunday>
            //    <Monday>True</Monday>
            //    <Tuesday>True</Tuesday>
            //    <Wednesday>True</Wednesday>
            //    <Thursday>True</Thursday>
            //    <Friday>True</Friday>
            //    <Saturday>True</Saturday>
            //  </Days>
            //  <SchedDate>1/1/1900</SchedDate>
            //</Schedule>

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(schedExp.SchedXML);

            XmlNode node = doc.SelectSingleNode("//Schedule/SchedDate");
            if (node != null)
            {
                schedExp.IsDaily = false;
                schedExp.MonthlyDate = node.Value;
            }
            else
            {
                schedExp.IsDaily = true;
                schedExp.MonthlyDate = "";
                node = doc.SelectSingleNode("//Schedule/Days");
                if (node != null)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        switch (child.Name)
                        {
                            case "Sunday":
                                schedExp.Sunday = Convert.ToBoolean(child.Value);
                                break;
                            case "Monday":
                                schedExp.Monday = Convert.ToBoolean(child.Value);
                                break;
                            case "Tuesday":
                                schedExp.Tuesday = Convert.ToBoolean(child.Value);
                                break;
                            case "Wednesday":
                                schedExp.Wednesday = Convert.ToBoolean(child.Value);
                                break;
                            case "Thursday":
                                schedExp.Thursday = Convert.ToBoolean(child.Value);
                                break;
                            case "Friday":
                                schedExp.Friday = Convert.ToBoolean(child.Value);
                                break;
                            case "Saturday":
                                schedExp.Saturday = Convert.ToBoolean(child.Value);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}