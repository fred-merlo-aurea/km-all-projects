using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class Campaigns
    {
        public Campaigns() { }
        #region Properties
        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
        public int AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Count { get; set; }
        public int? BrandID { get; set; }
        #endregion

        #region Data
        public static List<Campaigns> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Campaigns> retList = new List<Campaigns>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select c.CampaignID, CampaignName, count(distinct subscriptionID) as count, BrandID from Campaigns c left join Campaignfilter cf on c.CampaignID = cf.CampaignID  left join Campaignfilterdetails cfd on cfd.CampaignfilterID = cf.CampaignfilterID group by c.CampaignID, CampaignName, BrandID order by c.CampaignName", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Campaigns> builder = DynamicBuilder<Campaigns>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Campaigns x = builder.Build(rdr);
                    retList.Add(x);
                }
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

        public static List<Campaigns> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<Campaigns> retList = Get(clientconnection).FindAll(x => x.BrandID == brandID);
            return retList;
        }

        public static List<Campaigns> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return Get(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static List<Campaigns> GetCampaignsByID(KMPlatform.Object.ClientConnections clientconnection, string strIDs)
        {
            List<Campaigns> retList = new List<Campaigns>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select c.CampaignID, CampaignName, count(distinct subscriptionID) as count from Campaigns c left join Campaignfilter cf on c.CampaignID = cf.CampaignID  left join Campaignfilterdetails cfd on cfd.CampaignfilterID = cf.CampaignfilterID where c.campaignID in (" + strIDs + ") group by c.CampaignID, CampaignName order by c.CampaignName", conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Campaigns> builder = DynamicBuilder<Campaigns>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    Campaigns x = builder.Build(rdr);
                    retList.Add(x);
                }
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

        public static int CampaignExists(KMPlatform.Object.ClientConnections clientconnection, string Campaignname)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Campaigns WHERE CampaignName=@CampaignName");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@CampaignName", Campaignname));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static List<int> GetDataByCampaignID(KMPlatform.Object.ClientConnections clientconnection, int CampaignID, int CampaignFilterID)
        {
            List<int> sub_id = new List<int>();

            string query = "select distinct s.SubscriptionID from subscriptions s left outer join SubscriberMasterValues smv on s.SubscriptionID = smv.SubscriptionID ";
            query += " join  CampaignFilterDetails pdf on s.SubscriptionID = pdf.SubscriptionID join Campaignfilter pf on pdf.CampaignFilterID = pf.CampaignFilterID ";

            if (CampaignID == 0)
            {
                query += " where pf.CampaignFilterID  = @CampaignFilterID";
            }
            else
            {
                query += " where pf.CampaignID  = @CampaignID";
            }
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@CampaignID", CampaignID));
            cmd.Parameters.Add(new SqlParameter("@CampaignFilterID", CampaignFilterID));
            cmd.CommandTimeout = 20000;
            try
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    sub_id.Add(Int32.Parse(dr["SubscriptionID"].ToString()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return sub_id;
        }

        public static int GetCountByCampaignID(KMPlatform.Object.ClientConnections clientconnection, int CampaignID)
        {
            SqlCommand cmd = new SqlCommand("select count(distinct subscriptionID) as count from Campaigns p left join Campaignfilter pf on p.CampaignID = pf.CampaignID  left join Campaignfilterdetails pfd on pfd.CampaignfilterID = pf.CampaignfilterID where p.campaignID = @CampaignID group by p.CampaignID, CampaignName");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@CampaignID", CampaignID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }


        public static List<int> getSubscriptionIDforCampaignOperation(KMPlatform.Object.ClientConnections clientconnection, string operation, List<string> Selected_CampaignID, List<string> Suppressed_CampaignID, int brandID)
        {
            List<int> CampaignList = null;

            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);

            if (Selected_CampaignID.Count > 0)
            {
                List<List<int>> allSelectedList = new List<List<int>>();
                List<int> allSuppressedList = new List<int>();

                foreach (string campaignID in Selected_CampaignID)
                {
                    List<int> sID = Campaigns.GetDataByCampaignID(clientconnection, Int32.Parse(campaignID), 0);

                    allSelectedList.Add(sID);
                }

                foreach (string campaignID in Suppressed_CampaignID)
                {
                     List<int> sID = Campaigns.GetDataByCampaignID(clientconnection, Int32.Parse(campaignID), 0);

                    allSuppressedList.AddRange(sID);
                }

                #region UNION, INTERSECT & EXCEPT

                if (string.IsNullOrEmpty(operation))
                {
                    CampaignList = allSelectedList.FirstOrDefault();
                }
                else
                {
                    if (operation.Equals("INTERSECT", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (List<int> l in allSelectedList)
                        {
                            if (CampaignList == null)
                                CampaignList = l;
                            else
                                CampaignList = CampaignList.Intersect(l).ToList();
                        }
                    }
                    else if (operation.Equals("UNION", StringComparison.OrdinalIgnoreCase))
                    {
                        CampaignList = (from e in allSelectedList
                                        from e2 in e
                                        select e2).Distinct().ToList();
                    }
                    else if (operation.Equals("NOTIN", StringComparison.OrdinalIgnoreCase))
                    {
                        CampaignList = (from e in allSelectedList
                                        from e2 in e
                                        select e2).Distinct().ToList();

                        CampaignList = CampaignList.Except(allSuppressedList).ToList();
                    }
                    else if (operation.Equals("SINGLE", StringComparison.OrdinalIgnoreCase))
                    {
                        CampaignList = allSelectedList.FirstOrDefault();
                    }
                }

                allSelectedList = null;
                allSuppressedList = null;

                #endregion
            }

            return CampaignList;
        }

        public static StringBuilder GetCampaignQuery(int CampaignID, int CampaignFilterID)
        {
            string query = "select distinct 1, s.SubscriptionID from subscriptions s left outer join SubscriberMasterValues smv on s.SubscriptionID = smv.SubscriptionID ";
            query += " join  CampaignFilterDetails pdf on s.SubscriptionID = pdf.SubscriptionID join Campaignfilter pf on pdf.CampaignFilterID = pf.CampaignFilterID ";

            if (CampaignID == 0)
            {
                query += " where pf.CampaignFilterID  = " + CampaignFilterID;
            }
            else
            {
                query += " where pf.CampaignID  = " + CampaignID;
            }

            StringBuilder Queries = new StringBuilder();
            Queries.Append("<xml><Queries>");
            Queries.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", 1, query));
            Queries.Append("</Queries><Results>");
            Queries.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", 1, "1", "", "", "", ""));
            Queries.Append("</Results></xml>");
            return Queries;
        }

        public static StringBuilder GetCampaignSegmentationQuery(string SelectedCampaignID, string SuppressedCampaignID, string SelectedOperation)
        {
            StringBuilder sbQuery = new StringBuilder();
            StringBuilder sbResult = new StringBuilder();
            int i = 0;

            sbQuery.Append("<xml><Queries>");

            string[] Selected_CampaignID = SelectedCampaignID.Split(',');
            string[] Suppressed_CampaignID = SuppressedCampaignID.Split(',');
            string SelectedFilterNo = string.Empty;
            string SuppressedFilterNo = string.Empty;

            foreach (string s in Selected_CampaignID)
            {
                if (s != string.Empty)
                {
                    i++;
                    SelectedFilterNo += SelectedFilterNo == "" ? i.ToString() : "," + i.ToString(); 
                    string query = "select distinct " + i +  " , s.SubscriptionID from subscriptions s left outer join SubscriberMasterValues smv on s.SubscriptionID = smv.SubscriptionID ";
                    query += " join  CampaignFilterDetails pdf on s.SubscriptionID = pdf.SubscriptionID join Campaignfilter pf on pdf.CampaignFilterID = pf.CampaignFilterID ";
                    query += " where pf.CampaignID  = " + s;

                    sbQuery.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", i, query));
                }
            }

            foreach (string s in Suppressed_CampaignID)
            {
                if (s != string.Empty)
                {
                    i++;
                    SuppressedFilterNo += SuppressedFilterNo == "" ? i.ToString() : "," + i.ToString();
                    string query = "select distinct " + i + " , s.SubscriptionID from subscriptions s left outer join SubscriberMasterValues smv on s.SubscriptionID = smv.SubscriptionID ";
                    query += " join  CampaignFilterDetails pdf on s.SubscriptionID = pdf.SubscriptionID join Campaignfilter pf on pdf.CampaignFilterID = pf.CampaignFilterID ";
                    query += " where pf.CampaignID  = " + s;

                    sbQuery.Append(string.Format("<Query filterno=\"{0}\" ><![CDATA[{1}]]></Query>", i, query));
                }
            }

            sbResult.Append(string.Format("<Result linenumber=\"{0}\"  selectedfilterno=\"{1}\" selectedfilteroperation=\"{2}\" suppressedfilterno=\"{3}\" suppressedfilteroperation=\"{4}\"  filterdescription=\"{5}\"></Result>", i, SelectedFilterNo, SelectedOperation, SuppressedFilterNo, "", ""));

            sbQuery.Append("</Queries>");
            sbQuery.Append("<Results>");
            sbQuery.Append(sbResult.ToString());
            sbQuery.Append("</Results>");
            sbQuery.Append("</xml>");
            return sbQuery;
        }

        #endregion

        #region CRUD
        public static int Insert(KMPlatform.Object.ClientConnections clientconnection, string CampaignName, int UserID, int BrandID)
        {
            SqlCommand cmd = new SqlCommand("e_Campaigns_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CampaignName", CampaignName);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@BrandID", BrandID);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int CampaignID)
        {
            SqlCommand cmd = new SqlCommand("delete from Campaignfilterdetails where CampaignfilterID in (select CampaignfilterID from CampaignFilter where CampaignID = @CampaignID); delete from Campaignfilter where CampaignID = @CampaignID; delete from Campaigns where CampaignID = @CampaignID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@CampaignID", CampaignID));
            cmd.CommandTimeout = 0;
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}