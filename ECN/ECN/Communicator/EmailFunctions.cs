using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ecn.common.classes;
using ECN_Framework_Common.Functions;
using ECN_Framework_Entities.Communicator;
using KM.Common;
using Content = ECN_Framework_BusinessLayer.Communicator.Content;
using ContentFilter = ECN_Framework_BusinessLayer.Communicator.ContentFilter;
using DataFunctions = ecn.common.classes.DataFunctions;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;
using Layout = ECN_Framework_BusinessLayer.Communicator.Layout;
using EntitiesLayout = ECN_Framework_Entities.Communicator.Layout;
using StringFunctions = ecn.common.classes.StringFunctions;
using Template = ECN_Framework_BusinessLayer.Communicator.Template;
using EntitiesTemplate = ECN_Framework_Entities.Communicator.Template;
using User = KMPlatform.BusinessLogic.User;

namespace ecn.communicator.classes
{

    /// Basic functions for blasting a message. It also includes some static preview functions for the HTML email and the Text Email

    public class EmailFunctions
    {
        private const string BlastId = "BlastID";
        private const string ContentSource = "ContentSource";
        private const string ContentText = "ContentText";
        private const string EcnEngineAccessKey = "ECNEngineAccessKey";
        private const string Hyphen = "-";
        private const string OneEmail = "oneemail";
        private const string PersonalizedContentId = "PersonalizedContentID";
        private const string Slash = "/";
        private const string Slot1 = "slot1";
        private const string Slot2 = "slot2";
        private const string Slot3 = "slot3";
        private const string Slot4 = "slot4";
        private const string Slot5 = "slot5";
        private const string Slot6 = "slot6";
        private const string Slot7 = "slot7";
        private const string Slot8 = "slot8";
        private const string Slot9 = "slot9";

        LicenseCheck licenseCheck;

        public EmailFunctions()
        {
            // Initialize the license code
            licenseCheck = new LicenseCheck();
        }

        /// Sends a single email to the specified email address
        /// This methods should ONLY be invoked from the ECN side because it uses the local pickup directory in the ECN production server side.		

        /// <param name="BlastID"> Which blast to send</param>
        /// <param name="EmailID"> The Email ID we will be sending our info to</param>
        /// <param name="fromEmailAddress"> Which from address to use</param>
        /// <param name="VirtualPath"> The virtual path for images </param>
        /// <param name="HostName"> The hostname for link writing</param>
        /// <param name="BounceDomain"> The domain to bounce bad emails too</param>
        public void SendSingle(int BlastID, int EmailID, string fromEmailAddress,
            string VirtualPath, string HostName, string BounceDomain, KMPlatform.Entity.User user, int fromEmailID)
        {
            bool isResend = fromEmailAddress == "resendaddress";

            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(BlastID, false);

            if (fromEmailAddress.Length > 0 && !isResend)
            {
                blast.EmailFrom = fromEmailAddress;
            }

            //add CreateEmailList function to retreive UDF's
            int groupID = 0;
            try
            {
                groupID = blast.GroupID.Value;
            }
            catch (Exception) { }
            try
            {
                if (blast.BlastType.ToUpper() == "LAYOUT" || blast.BlastType.ToUpper() == "NOOPEN")
                {
                    int refBlastID = ECN_Framework_BusinessLayer.Communicator.BlastSingle.GetRefBlastID(BlastID, fromEmailID, blast.CustomerID.Value, blast.BlastType);
                    ECN_Framework_Entities.Communicator.BlastAbstract refblast = ECN_Framework_BusinessLayer.Communicator.BlastAbstract.GetByBlastID_NoAccessCheck(refBlastID, false);
                    groupID = refblast.GroupID.Value;
                }
            }
            catch (Exception) { }
            bool testBlast = false;
            if (blast.TestBlast.ToUpper().Equals("Y"))
                testBlast = true;

            SendBlastForEmails(blast, VirtualPath, HostName, BounceDomain, CreateEmailListAll(" and Emails.EmailID=" + EmailID, blast.CustomerID.Value, groupID, BounceDomain, BlastID.ToString()), true, testBlast, fromEmailAddress, false);
            if (!isResend)
            {
                Blasts my_blast = new Blasts(BlastID);
                my_blast.Inc();
            }
        }

        public void SendBlastSingle(ECN_Framework_Entities.Communicator.Blast blast, int EmailID, int GroupID, string VirtualPath, string HostName, string BounceDomain)
        {
            DataTable dt = new DataTable();

            if (GroupID > 0)
            {
                if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                    KM.Common.FileFunctions.LogActivity(false, "Getting Email List ", string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

                dt = CreateEmailListAll(" and Emails.EmailID=" + EmailID, blast.CustomerID.Value, GroupID, BounceDomain, blast.BlastID.ToString());

                if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                    KM.Common.FileFunctions.LogActivity(false, "Done Getting EMail List ", string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

            }
            else
            {
                Console.WriteLine("Get MTA for customer---" + DateTime.Now);
                CustomerMTA cMTA = CustomerMTA.GetMTAOrDefault(blast.CustomerID.Value, blast.EmailFrom.Split('@')[1].ToString());
                int groupID = blast.GroupID.HasValue ? blast.GroupID.Value : 0;

                string sqlEmailQuery =
                    " SELECT *, " + blast.BlastID + " AS BlastID, 'bounce_'+LTRIM(STR(EmailID))+'-" + blast.BlastID + "@" + BounceDomain + "' AS BounceAddress, " +
                    groupID.ToString() + " AS GroupID, 'html' AS FormatTypeCode, 'S' AS SubscribeTypeCode, '" + cMTA.MTAName + "'  as mailRoute " +
                    " , 1 as IsMTAPriority " +
                    " FROM Emails e " +
                    " WHERE e.EmailID=" + EmailID;
                if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                    KM.Common.FileFunctions.LogActivity(false, "Getting Email List ", string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

                dt = DataFunctions.GetDataTable(sqlEmailQuery);
                if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                    KM.Common.FileFunctions.LogActivity(false, "Done Getting EMail List ", string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

                Console.WriteLine("Got MTA for customer ---" + DateTime.Now);
            }

            SendBlastForEmails(blast, VirtualPath, HostName, BounceDomain, dt, false, false, false);
            //blast.Inc(); --TODOSUNIL
            {
                Blasts my_blast = new Blasts(blast.BlastID);
                my_blast.Inc();
            }
        }

        public void SendSystem(ECN_Framework_Entities.Communicator.Blast event_blast, ECN_Framework_Entities.Communicator.Blast master_blast, int EmailID, string VirtualPath, string HostName, string BounceDomain)
        {
            master_blast.SendTime  = event_blast.SendTime;
            master_blast.FinishTime = event_blast.FinishTime;
            string sqlEmailQuery =
                " SELECT *, 'bounce_'+LTRIM(STR(EmailID))+'-" + master_blast.BlastID + "@" + BounceDomain + "' AS BounceAddress, " +
                master_blast.GroupID.Value + " AS GroupID, 'html' AS FormatTypeCode, 'S' AS SubscribeTypeCode " +
                " FROM Emails e " +
                " WHERE e.EmailID=" + EmailID;
            SendBlastForEmails(master_blast, VirtualPath, HostName, BounceDomain, DataFunctions.GetDataTable(sqlEmailQuery), true, true);
            //master_blast.Inc(); --TODOSUNIL
            {
                Blasts my_blast = new Blasts(master_blast.BlastID);
                my_blast.Inc();
            }
        }

        /// Creates a general pickup for a blast. Horrible name will stay as I cannot really refactor right now.

        /// <param name="BlastID"> The BlastID we want to send to</param>
        /// <param name="filterID"> The filter we want to use </param>
        /// <param name="VirtualPath"> The Virtual path for the links</param>
        /// <param name="HostName"> The hostname for the links</param>
        /// <param name="BounceDomain"> Which domain to bounce to</param>
        /// <param name="TestBlast"> Test of a Blast</param>
        public void SendBlast(int BlastID, string VirtualPath, string HostName, string BounceDomain)
        {
            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Starting EmailFunctions.SendBlast: {0}", BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(BlastID, false);

            bool TestBlast = false;
            if (blast.TestBlast.ToUpper() == "Y")
            {
                TestBlast = true;
            }
            // Make the list to send it too

            if (blast.BlastType.Equals("Champion"))
            {
                SendBlastForEmails(blast, VirtualPath, HostName, BounceDomain, null, false, TestBlast);
            }
            else
            {
                //int filterID = 0;
                //int.TryParse(blast_info["FilterID"].ToString(), out filterID);
                //int smartSegmentID = 0;
                //int.TryParse(blast_info["SmartSegmentID"].ToString(), out smartSegmentID);
                SendBlastForEmails(blast, VirtualPath, HostName, BounceDomain, CreateEmailList(blast, BounceDomain, TestBlast), false, TestBlast);
            }
            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Ending EmailFunctions.SendBlast: {0}", BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

        }

        //GetBlastRemainingCount - Method to get the remaining count of emails for this blast (pass the same params like the master Sproc)
        static public int GetBlastRemainingCount(int FilterID, int SmartSegmentID, int CustomerID, int GroupID, string BounceDomain, string BlastID, bool TestBlast)
        {
            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
            ECN_Framework_Entities.Communicator.CampaignItemBlast cib = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> filters = cib.Filters;
            ECN_Framework_Entities.Communicator.CampaignItem ci = new ECN_Framework_Entities.Communicator.CampaignItem();
            if (!TestBlast)
            {
                cib = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID_NoAccessCheck(Convert.ToInt32(BlastID), true);

                if (cib.Filters != null)
                    filters = cib.Filters;
                ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(Convert.ToInt32(BlastID), false);
                List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> suppList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(ci.CampaignItemID, true);

            }
            else
            {
                ECN_Framework_Entities.Communicator.CampaignItemTestBlast citb = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByBlastID_NoAccessCheck(Convert.ToInt32(BlastID), true);
                if (citb.Filters != null)
                {
                    filters = citb.Filters;
                }
            }

            #region building xml for proc
            StringBuilder actiontypes = new StringBuilder();
            actiontypes.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            actiontypes.Append("<SmartSegments>");
            StringBuilder SuppFilters = new StringBuilder();
            SuppFilters.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            SuppFilters.Append("<SuppFilters>");
            string filterIDs = string.Empty;
            if (filters != null && filters.Count > 0)
            {
                if (!TestBlast)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in filters.Where(x => x.CampaignItemBlastID != null))
                    {
                        if (cibf.FilterID != null)
                        {
                            filterIDs += cibf.FilterID.ToString() + ",";
                        }
                        else if (cibf.SmartSegmentID != null)
                        {
                            actiontypes.Append("<ssID id=\"" + cibf.SmartSegmentID.ToString() + "\">");
                            actiontypes.Append("<refBlastIDs>" + cibf.RefBlastIDs.ToString() + "</refBlastIDs></ssID>");
                        }
                    }
                }
                else if (TestBlast)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in filters.Where(x => x.CampaignItemTestBlastID != null))
                    {
                        if (cibf.FilterID != null)
                        {
                            filterIDs += cibf.FilterID.ToString() + ",";
                        }
                        else if (cibf.SmartSegmentID != null)
                        {
                            actiontypes.Append("<ssID id=\"" + cibf.SmartSegmentID.ToString() + "\">");
                            actiontypes.Append("<refBlastIDs>" + cibf.RefBlastIDs.ToString() + "</refBlastIDs></ssID>");
                        }
                    }
                }
            }
            actiontypes.Append("</SmartSegments>");
            filterIDs = filterIDs.TrimEnd(',');

            if (ci != null && ci.CampaignItemID > 0)
            {
                List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> suppList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(ci.CampaignItemID, true);
                foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in suppList)
                {
                    SuppFilters.Append("<SuppressionGroup id=\"" + cis.GroupID.Value.ToString() + "\">");

                    if (cis.Filters != null && cis.Filters.Count > 0)
                    {


                        if (cis.Filters.Count(x => x.FilterID != null) > 0)
                        {

                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cis.Filters.Where(x => x.FilterID != null).ToList())
                            {
                                SuppFilters.Append("<FilterID id=\"" + cibf.FilterID + "\">");
                                SuppFilters.Append("</FilterID>");
                            }


                        }
                        if (cis.Filters.Count(x => x.SmartSegmentID != null) > 0)
                        {
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cis.Filters.Where(x => x.SmartSegmentID != null).ToList())
                            {
                                if (cibf.SmartSegmentID != null)
                                {
                                    SuppFilters.Append("<ssID id=\"" + cibf.SmartSegmentID.ToString() + "\">");
                                    SuppFilters.Append("<refBlastIDs>" + cibf.RefBlastIDs.ToString() + "</refBlastIDs></ssID>");
                                }
                            }
                        }

                    }
                    SuppFilters.Append("</SuppressionGroup>");
                }
            }
            SuppFilters.Append("</SuppFilters>");
            SuppFilters = SuppFilters.Replace(",</", "</");
            #endregion

            #region Stored Procedure to get the remaining count of emails for this blast (pass the same params like the master Sproc)
            SqlConnection dbConn = new SqlConnection(DataFunctions.connStr.ToString());

            //-- Modified to use new proc -- Sunil 04/26/07
            SqlCommand emailsListCmd;
            try
            {
                emailsListCmd = new SqlCommand(ConfigurationManager.AppSettings["DynamicContentProc"], dbConn);
            }
            catch (Exception)
            {
                emailsListCmd = new SqlCommand("v_Blast_GetBlastEmailsListForDynamicContent", dbConn);
            }
            emailsListCmd.CommandTimeout = 0;
            emailsListCmd.CommandType = CommandType.StoredProcedure;

            //--CustomerID
            emailsListCmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            emailsListCmd.Parameters["@CustomerID"].Value = CustomerID;
            //--BlastID
            emailsListCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            emailsListCmd.Parameters["@BlastID"].Value = Convert.ToInt32(BlastID);
            //--GroupID
            emailsListCmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
            emailsListCmd.Parameters["@GroupID"].Value = GroupID;
            //--FilterID
            emailsListCmd.Parameters.Add(new SqlParameter("@FilterID", SqlDbType.VarChar));
            emailsListCmd.Parameters["@FilterID"].Value = filterIDs.ToString();
            //--Blast plus Bounce Domain - no need to send for getting counts.
            emailsListCmd.Parameters.Add(new SqlParameter("@BlastID_and_BounceDomain", SqlDbType.VarChar, 250));
            emailsListCmd.Parameters["@BlastID_and_BounceDomain"].Value = "";
            //--ActionType for smartSegment which is "unOpened / unClicked / unOpened&Unclicked"
            emailsListCmd.Parameters.Add(new SqlParameter("@ActionType", SqlDbType.VarChar));
            emailsListCmd.Parameters["@ActionType"].Value = actiontypes.ToString();
            //-- Reference BlastID for smartSegment
            //emailsListCmd.Parameters.Add(new SqlParameter("@refBlastID", SqlDbType.VarChar));
            //emailsListCmd.Parameters["@refBlastID"].Value = refBlastID;
            //blastSuppression list
            emailsListCmd.Parameters.Add(new SqlParameter("@SupressionList", SqlDbType.VarChar));
            emailsListCmd.Parameters["@SupressionList"].Value = SuppFilters.ToString();
            // for getting only counts
            emailsListCmd.Parameters.Add(new SqlParameter("@OnlyCounts", SqlDbType.Bit));
            emailsListCmd.Parameters["@OnlyCounts"].Value = 1;

            SqlDataAdapter da = new SqlDataAdapter(emailsListCmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "EmailListCount");
            dbConn.Close();

            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            #endregion
        }

        /// Refactoring: This method should be moved to Group. And parameters like CustomerID, GroupID should be removed.

        /// Creates an email list either with or without filter. Send in "" to filter to ensure that you get a full customerID / GroupID list.

        /// <param name="FilterID"> A filterid or "" for no filter</param>
        /// <param name="CustomerID"> The customer we are sending this for</param>
        /// <param name="GroupID"> The group we are sending this too</param>
        /// <param name="BounceDomain"> Where to bounce the addresses to.</param>
        /// <param name="BlastID"> Which blast are we working on?</param>
        /// <returns>A DataTable of Emails filtered by the specfied filter</returns>
        static public DataTable CreateEmailList(ECN_Framework_Entities.Communicator.Blast blast , string BounceDomain, bool TestBlast)
        {
            int CustomerID = blast.CustomerID.Value;
            int GroupID = blast.GroupID.Value;
            int BlastID = blast.BlastID;

            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Starting EmailFunctions.CreateEmailList: {0}", BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> filters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
            ECN_Framework_Entities.Communicator.CampaignItem ci = new ECN_Framework_Entities.Communicator.CampaignItem();
            if (!TestBlast)
            {
                ECN_Framework_Entities.Communicator.CampaignItemBlast cib = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID_NoAccessCheck(Convert.ToInt32(BlastID), true);

                if (cib.Filters != null)
                    filters = cib.Filters;
                ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(Convert.ToInt32(BlastID), false);

            }
            else
            {
                ECN_Framework_Entities.Communicator.CampaignItemTestBlast citb = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByBlastID_NoAccessCheck(Convert.ToInt32(BlastID), true);
                if (citb.Filters != null)
                {
                    filters = citb.Filters;
                }
            }

            #region building xml for proc
            StringBuilder actiontypes = new StringBuilder();
            actiontypes.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            actiontypes.Append("<SmartSegments>");
            StringBuilder SuppFilters = new StringBuilder();
            SuppFilters.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            SuppFilters.Append("<SuppFilters>");
            string filterIDs = string.Empty;
            if (filters != null && filters.Count > 0)
            {
                if (!TestBlast)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in filters.Where(x => x.CampaignItemBlastID != null))
                    {
                        if (cibf.FilterID != null)
                        {
                            filterIDs += cibf.FilterID.ToString() + ",";
                        }
                        else if (cibf.SmartSegmentID != null)
                        {
                            actiontypes.Append("<ssID id=\"" + cibf.SmartSegmentID.ToString() + "\">");
                            actiontypes.Append("<refBlastIDs>" + cibf.RefBlastIDs.ToString() + "</refBlastIDs></ssID>");
                        }
                    }
                }
                else if (TestBlast)
                {
                    foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in filters.Where(x => x.CampaignItemTestBlastID != null))
                    {
                        if (cibf.FilterID != null)
                        {
                            filterIDs += cibf.FilterID.ToString() + ",";
                        }
                        else if (cibf.SmartSegmentID != null)
                        {
                            actiontypes.Append("<ssID id=\"" + cibf.SmartSegmentID.ToString() + "\">");
                            actiontypes.Append("<refBlastIDs>" + cibf.RefBlastIDs.ToString() + "</refBlastIDs></ssID>");
                        }
                    }
                }
            }
            actiontypes.Append("</SmartSegments>");
            filterIDs = filterIDs.TrimEnd(',');

            if (ci != null && ci.CampaignItemID > 0 && !TestBlast)
            {
                List<ECN_Framework_Entities.Communicator.CampaignItemSuppression> suppList = ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.GetByCampaignItemID_NoAccessCheck(ci.CampaignItemID, true);
                foreach (ECN_Framework_Entities.Communicator.CampaignItemSuppression cis in suppList)
                {
                    SuppFilters.Append("<SuppressionGroup id=\"" + cis.GroupID.Value.ToString() + "\">");

                    if (cis.Filters != null && cis.Filters.Count > 0)
                    {


                        if (cis.Filters.Count(x => x.FilterID != null) > 0)
                        {

                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cis.Filters.Where(x => x.FilterID != null).ToList())
                            {
                                SuppFilters.Append("<FilterID id=\"" + cibf.FilterID + "\">");
                                SuppFilters.Append("</FilterID>");
                            }


                        }
                        if (cis.Filters.Count(x => x.SmartSegmentID != null) > 0)
                        {
                            foreach (ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf in cis.Filters.Where(x => x.SmartSegmentID != null).ToList())
                            {
                                if (cibf.SmartSegmentID != null)
                                {
                                    SuppFilters.Append("<ssID id=\"" + cibf.SmartSegmentID.ToString() + "\">");
                                    SuppFilters.Append("<refBlastIDs>" + cibf.RefBlastIDs.ToString() + "</refBlastIDs></ssID>");
                                }
                            }
                        }

                    }
                    SuppFilters.Append("</SuppressionGroup>");
                }
            }
            //looping through suppression groups
            if (filters != null && filters.Count > 0)
            {


            }
            SuppFilters.Append("</SuppFilters>");
            SuppFilters = SuppFilters.Replace(",</", "</");
            #endregion

            #region commented out by ashok  on 04/06/2005 cos we are implementing a Stored Proc for getting the emails.. !!

            /*			
			EmailListQueryGenerator generator = new EmailListQueryGenerator(GroupID, Convert.ToInt32(BlastID), BounceDomain);	
			string emails_sqlQuery = generator.GetSelectQuery(whereClause);			
			DataTable emails = DataFunctions.GetDataTable(emails_sqlQuery);
            
			string datafields_sqlQuery = "SELECT * from GroupDatafields gd WHERE GroupID = " + GroupID;
			DataTable datafields = DataFunctions.GetDataTable(datafields_sqlQuery);

			// No extra processing for people who don't use dynamic content.
			if(0 >= datafields.Rows.Count) {
				return emails;
			}
			// Add the "user_" columns to the emails fields
			foreach(DataRow dr in datafields.Rows){
				emails.Columns.Add("user_" + dr["ShortName"].ToString(),typeof(String));
			}
			// Add the data for each user column.. blah.. I wish we could do this in a select.
			foreach(DataRow dr in emails.Rows) {
				foreach(DataRow data_field in datafields.Rows){
					object user_value = DataFunctions.ExecuteScalar("SELECT DataValue FROM EmailDataValues WHERE EmailID=" + dr["EmailID"].ToString() + " AND GroupDatafieldsID=" + data_field["GroupDatafieldsID"].ToString());
					if(null != user_value) {
						dr["user_" + data_field["ShortName"].ToString()] = user_value.ToString();
					} else {
						dr["user_" + data_field["ShortName"].ToString()] = "";
					}
				}
			}*/
            #endregion

            #region Stored Procedure to create the email list pass groupID, CustomerID, FilterID (null), FilterWhereClause, BlastID + @ +bounceDomain
            SqlConnection dbConn = new SqlConnection(DataFunctions.connStr.ToString());

            //-- Modified to use new Dynamic content proc -- Sunil 02/14/08 for testing purpose.
            // Merge both procs..

            //SqlCommand emailsListCmd = new SqlCommand("sp_GetBlastEmailsList", dbConn);
            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Starting Dynamic Content Proc: {0}", BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

            SqlCommand emailsListCmd;
            try
            {
                emailsListCmd = new SqlCommand(ConfigurationManager.AppSettings["DynamicContentProc"], dbConn);
            }
            catch (Exception)
            {
                emailsListCmd = new SqlCommand("v_Blast_GetBlastEmailsListForDynamicContent", dbConn);
            }

            emailsListCmd.CommandTimeout = 0;
            emailsListCmd.CommandType = CommandType.StoredProcedure;

            //--CustomerID
            emailsListCmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            emailsListCmd.Parameters["@CustomerID"].Value = CustomerID;

            //BlastID
            emailsListCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            emailsListCmd.Parameters["@BlastID"].Value = Convert.ToInt32(BlastID);

            //--GroupID
            emailsListCmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
            emailsListCmd.Parameters["@GroupID"].Value = GroupID;

            //--FilterID
            emailsListCmd.Parameters.Add(new SqlParameter("@FilterID", SqlDbType.VarChar));
            emailsListCmd.Parameters["@FilterID"].Value = filterIDs;

            //--Blast plus Bounce Domain
            emailsListCmd.Parameters.Add(new SqlParameter("@BlastID_and_BounceDomain", SqlDbType.VarChar, 250));
            emailsListCmd.Parameters["@BlastID_and_BounceDomain"].Value = BlastID + "@" + BounceDomain;


            //--ActionType for smartSegment which is "unOpened / unClicked / unOpened&Unclicked"
            emailsListCmd.Parameters.Add(new SqlParameter("@ActionType", SqlDbType.VarChar));
            emailsListCmd.Parameters["@ActionType"].Value = actiontypes.ToString();

            ////-- Reference BlastID for smartSegment
            //emailsListCmd.Parameters.Add(new SqlParameter("@refBlastID", SqlDbType.VarChar));
            //emailsListCmd.Parameters["@refBlastID"].Value = refBlastID;

            //Suppression List
            emailsListCmd.Parameters.Add(new SqlParameter("@SupressionList", SqlDbType.VarChar));
            emailsListCmd.Parameters["@SupressionList"].Value = SuppFilters.ToString();

            //Set countonly to 0 to get the list
            emailsListCmd.Parameters.Add(new SqlParameter("@OnlyCounts", SqlDbType.Bit));
            emailsListCmd.Parameters["@OnlyCounts"].Value = 0;

            SqlDataAdapter da = new SqlDataAdapter(emailsListCmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "EmailListCount");
            dbConn.Close();

            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Ending Dynamic Content Proc: {0}", BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));


            if (ds.Tables[0].Rows.Count > 0 && blast.BlastType.ToLower() != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Personalization.ToString().ToLower())
            {
                SqlCommand CmdSeedList = new SqlCommand("sp_getSeedList");

                CmdSeedList.CommandType = CommandType.StoredProcedure;
                CmdSeedList.CommandTimeout = 0;

                CmdSeedList.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
                CmdSeedList.Parameters["@CustomerID"].Value = CustomerID;

                CmdSeedList.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
                CmdSeedList.Parameters["@BlastID"].Value = BlastID;

                DataTable dtSeedList = DataFunctions.GetDataTable("communicator", CmdSeedList);

                DataTable dtSeedListDefaultValues = ECN_Framework_BusinessLayer.Communicator.Blast.GetDefaultContentForSlotandDynamicTags(Convert.ToInt32(BlastID));

                foreach (DataRow drS in dtSeedList.Rows)
                {
                    DataRow drNewRow = UpdateSeedListContent(dtSeedListDefaultValues, ds.Tables[0].NewRow());
                    //drNewRow.ItemArray = dtSeedListDefaultValues.Rows[0].ItemArray;// ds.Tables[0].Rows[0].ItemArray;

                    drNewRow["EmailID"] = drS["EmailID"];
                    drNewRow["BlastID"] = drS["BlastID"];
                    drNewRow["EmailAddress"] = drS["EmailAddress"];
                    drNewRow["CustomerID"] = drS["CustomerID"];
                    drNewRow["FormatTypeCode"] = drS["FormatTypeCode"];
                    drNewRow["subscribetypecode"] = drS["subscribetypecode"];
                    drNewRow["FormatTypeCode"] = drS["FormatTypeCode"];
                    drNewRow["ConversionTrkCDE"] = drS["ConversionTrkCDE"];
                    drNewRow["BounceAddress"] = "bounce_" + drS["EmailID"] + "-" + BlastID + "@" + BounceDomain;//bounce_108102227-841651@bounce2.com
                    drNewRow["mailRoute"] = drS["mailRoute"];
                    drNewRow["groupID"] = drS["groupID"];

                    ds.Tables[0].Rows.Add(drNewRow);
                }

                int i = -1;
                //KM Seed List Emails
                string xmlFilePath = "KMSeedList.xml";
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(xmlFilePath);
                XmlNode nodeEmails = doc.SelectSingleNode("//KMSeedList/Emails");
                if (nodeEmails.HasChildNodes)
                {
                    foreach (XmlNode nodeChild in nodeEmails.ChildNodes)
                    {
                        if (nodeChild.Name == "EmailAddress")
                        {
                            DataRow drNewRowTemp = UpdateSeedListContent(dtSeedListDefaultValues, ds.Tables[0].NewRow());
                            //drNewRowTemp.ItemArray = dtSeedListDefaultValues.Rows[0].ItemArray;// ds.Tables[0].Rows[0].ItemArray;

                            drNewRowTemp["EmailID"] = i;
                            drNewRowTemp["BlastID"] = -1;
                            drNewRowTemp["EmailAddress"] = nodeChild.InnerText;
                            drNewRowTemp["CustomerID"] = ds.Tables[0].Rows[0]["CustomerID"];
                            drNewRowTemp["FormatTypeCode"] = "html";
                            drNewRowTemp["subscribetypecode"] = "S";
                            drNewRowTemp["ConversionTrkCDE"] = ds.Tables[0].Rows[0]["ConversionTrkCDE"];
                            drNewRowTemp["BounceAddress"] = "bounce_" + i + "-" + BlastID + "@" + BounceDomain;//bounce_108102227-841651@bounce2.com
                            drNewRowTemp["mailRoute"] = ds.Tables[0].Rows[0]["mailRoute"];
                            drNewRowTemp["groupID"] = ds.Tables[0].Rows[0]["groupID"];
                            ds.Tables[0].Rows.Add(drNewRowTemp);
                            i--;
                        }
                    }
                }

                //wgh - commented out for now - this is for adding our spam checking addresses to the email list to send
                //if has email preview then add this seed list for spam checking
                int hasEmailPreview = Convert.ToInt32(DataFunctions.ExecuteScalar("SELECT ISNULL(HasEmailPreview, 0) FROM Blast WHERE BlastID = " + BlastID));
                string hasEmailPreview_String = DataFunctions.ExecuteScalar("SELECT ISNULL(HasEmailPreview, 0) FROM Blast WHERE BlastID = " + BlastID).ToString();
                if (hasEmailPreview.ToString() == "1")
                {
                    if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                        KM.Common.FileFunctions.LogActivity(false, string.Format("Starting Email Preview: {0}", BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

                    EmailPreview.Preview p = new EmailPreview.Preview();
                    //List<EmailPreview.EmailResultEnum.EmailSpam> listSpam = EmailPreview.EmailResultEnum.GetAllEmailSpams();

                    ////do All Email Clients
                    //List<EmailPreview.EmailResultEnum.EmailClient> listClients = new List<EmailPreview.EmailResultEnum.EmailClient>();
                    //listClients = EmailPreview.EmailResultEnum.GetAllEmailClients();

                    string initialTestEmail = p.CreateCustomerEmailTestFromEngine(Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerID"].ToString()), Convert.ToInt32(BlastID));
                    if (initialTestEmail.Trim().Length > 0)
                    {

                        DataRow drNewRowTemp = UpdateSeedListContent(dtSeedListDefaultValues, ds.Tables[0].NewRow());
                        //drNewRowTemp.ItemArray = dtSeedListDefaultValues.Rows[0].ItemArray;//ds.Tables[0].Rows[0].ItemArray;

                        drNewRowTemp["EmailID"] = i;
                        drNewRowTemp["BlastID"] = -1;
                        drNewRowTemp["EmailAddress"] = initialTestEmail;
                        drNewRowTemp["CustomerID"] = ds.Tables[0].Rows[0]["CustomerID"];
                        drNewRowTemp["FormatTypeCode"] = "html";
                        drNewRowTemp["subscribetypecode"] = "S";
                        drNewRowTemp["ConversionTrkCDE"] = ds.Tables[0].Rows[0]["ConversionTrkCDE"];
                        drNewRowTemp["BounceAddress"] = "bounce_" + i + "-" + BlastID + "@" + BounceDomain;//bounce_108102227-841651@bounce2.com
                        drNewRowTemp["mailRoute"] = ds.Tables[0].Rows[0]["mailRoute"];
                        drNewRowTemp["groupID"] = ds.Tables[0].Rows[0]["groupID"];
                        ds.Tables[0].Rows.Add(drNewRowTemp);
                        string[] lPreviewTestEmails = EmailPreview.Preview.GetSpamSeedAddresses();
                        foreach (string email in lPreviewTestEmails)
                        {
                            i--;

                            drNewRowTemp = UpdateSeedListContent(dtSeedListDefaultValues, ds.Tables[0].NewRow());
                            //drNewRowTemp.ItemArray = dtSeedListDefaultValues.Rows[0].ItemArray;//ds.Tables[0].Rows[0].ItemArray;

                            drNewRowTemp["EmailID"] = i;
                            drNewRowTemp["BlastID"] = -1;
                            drNewRowTemp["EmailAddress"] = email;
                            drNewRowTemp["CustomerID"] = ds.Tables[0].Rows[0]["CustomerID"];
                            drNewRowTemp["FormatTypeCode"] = "html";
                            drNewRowTemp["subscribetypecode"] = "S";
                            drNewRowTemp["ConversionTrkCDE"] = ds.Tables[0].Rows[0]["ConversionTrkCDE"];
                            drNewRowTemp["BounceAddress"] = "bounce_" + i + "-" + BlastID + "@" + BounceDomain;//bounce_108102227-841651@bounce2.com
                            drNewRowTemp["mailRoute"] = ds.Tables[0].Rows[0]["mailRoute"];
                            drNewRowTemp["groupID"] = ds.Tables[0].Rows[0]["groupID"];
                            ds.Tables[0].Rows.Add(drNewRowTemp);
                        }
                    }
                    if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                        KM.Common.FileFunctions.LogActivity(false, string.Format("Ending Email Preview: {0}", BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

                }
            }


            DataTable emails = ds.Tables[0];
            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, string.Format("Ending EmailFunctions.CreateEmailList: {0}, Email Count: {0}", BlastID, emails.Rows.Count), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));


            #endregion
            Console.WriteLine(string.Format("The proc gave {0} emails {1}", emails.Rows.Count, DateTime.Now));
            //Latha: update all status fields
            return emails;
        }



        static private DataRow UpdateSeedListContent(DataTable dtSeedListDefaultValues, DataRow drRowToModify)
        {
            //if we have dynamic content or tags
            if (dtSeedListDefaultValues != null && dtSeedListDefaultValues.Rows.Count > 0)
            {
                foreach (DataColumn col in dtSeedListDefaultValues.Columns)
                {
                    drRowToModify[col.ColumnName] = dtSeedListDefaultValues.Rows[0][col.ColumnName];
                }
            }

            return drRowToModify;
        }

        /// Refactoring: This method should be moved to Group. And parameters like CustomerID, GroupID should be removed.

        /// Creates an email list either with or without filter. Send in "" to filter to ensure that you get a full customerID / GroupID list.

        /// <param name="FilterID"> A filterid or "" for no filter</param>
        /// <param name="CustomerID"> The customer we are sending this for</param>
        /// <param name="GroupID"> The group we are sending this too</param>
        /// <param name="BounceDomain"> Where to bounce the addresses to.</param>
        /// <param name="BlastID"> Which blast are we working on?</param>
        /// <returns>A DataTable of Emails filtered by the specfied filter</returns>
        static public DataTable CreateEmailListAll(string Filter, int CustomerID, int GroupID, string BounceDomain, string BlastID)
        {
            string actionType = "";
            int refBlastID = 0;
            Console.WriteLine("Get Email and UDFs---" + DateTime.Now);
            #region Stored Procedure to create the email list pass groupID, CustomerID, FilterID (null), FilterWhereClause, BlastID + @ +bounceDomain
            SqlConnection dbConn = new SqlConnection(DataFunctions.con_activity.ToString());

            //-- new Sproc which uses the smartSegmenting functionality.
            //-- ashok 9/7/05
            SqlCommand emailsListCmd = new SqlCommand("spFilteremails_ALL_with_smartsegment", dbConn);
            emailsListCmd.CommandTimeout = 0;
            emailsListCmd.CommandType = CommandType.StoredProcedure;

            //--GroupID
            emailsListCmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
            emailsListCmd.Parameters["@GroupID"].Value = GroupID;
            //--CustomerID
            emailsListCmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            emailsListCmd.Parameters["@CustomerID"].Value = CustomerID;
            //--FilterID
            emailsListCmd.Parameters.Add(new SqlParameter("@FilterID", SqlDbType.Int));
            emailsListCmd.Parameters["@FilterID"].Value = DBNull.Value;
            //--Filter
            emailsListCmd.Parameters.Add(new SqlParameter("@Filter", SqlDbType.VarChar, 8000));
            emailsListCmd.Parameters["@Filter"].Value = Filter.ToString();
            //--BlastID
            emailsListCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            emailsListCmd.Parameters["@BlastID"].Value = Convert.ToInt32(BlastID);
            //--Blast plus Bounce Domain
            emailsListCmd.Parameters.Add(new SqlParameter("@BlastID_and_BounceDomain", SqlDbType.VarChar, 250));
            emailsListCmd.Parameters["@BlastID_and_BounceDomain"].Value = BlastID + "@" + BounceDomain;
            //--ActionType for smartSegment which is "unOpened / unClicked / unOpened&Unclicked"
            emailsListCmd.Parameters.Add(new SqlParameter("@ActionType", SqlDbType.VarChar, 10));
            emailsListCmd.Parameters["@ActionType"].Value = actionType.ToString();
            //-- Reference BlastID for smartSegment
            emailsListCmd.Parameters.Add(new SqlParameter("@refBlastID", SqlDbType.Int));
            emailsListCmd.Parameters["@refBlastID"].Value = refBlastID.ToString();

            SqlDataAdapter da = new SqlDataAdapter(emailsListCmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "CreateEmailListAll");
            dbConn.Close();

            DataTable emails = ds.Tables[0];
            #endregion
            Console.WriteLine(string.Format("The proc gave {0} emails--{1}", emails.Rows.Count, DateTime.Now));
            return emails;
        }

        /// Emails a single user.

        /// <param name="ToEmail"> Who we wish to email</param>
        /// <param name="FromEmail"> The from email address</param>
        /// <param name="Subject"> The subject field of the email</param>
        /// <param name="Body"> The email text </param>
        public void SimpleSend(string ToEmail, string FromEmail, string Subject, string Body)
        {

            #region view Commented Code
            /*StringWriter sw = new StringWriter();
			string boundry_tag = "_=COMMUNICATOR=_" + QuotedPrintable.RandomString(32,true);
			boundry_tag = boundry_tag.ToLower();

			// make the RFC 822 Style email
			sw.WriteLine("X-Receiver: " + ToEmail);
			sw.WriteLine("From:\"" + FromEmail + "\" <"+ FromEmail + ">"); 
			sw.WriteLine("To: \"" + ToEmail + "\" <"+ ToEmail + ">"); 
			sw.WriteLine("Reply-To: " + FromEmail );
			sw.WriteLine("Date: " + DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'CST'"));
			//sw.WriteLine("Organization: Microsoft");
			sw.WriteLine("Subject: " + Subject );
			sw.WriteLine("X-Mailer: ECN Communicator 5.1");
			sw.WriteLine("X-RCPT-TO: <" + FromEmail + ">");
			sw.WriteLine("MIME-Version: 1.0");
                
			sw.WriteLine("Content-Type: multipart/alternative; boundary=\""+ boundry_tag + "\"");

			sw.Write("\r\n");

			sw.Write(QuotedPrintable.Encode(Body) + "\r\n");
			sw.Write("\r\n");
			sw.WriteLine("--" + boundry_tag );
			sw.WriteLine("Content-Type: text/html; charset=\"ISO-8859-1\"");
			sw.WriteLine("Content-Transfer-Encoding: quoted-printable");
			sw.Write("\r\n");

			sw.Write(QuotedPrintable.Encode(Body) + "\r\n");

			sw.WriteLine("--" + boundry_tag  + "--");
			sw.Write("\r\n");

			string filename = QuotedPrintable.RandomString(15,true) + ".eml";
			using (StreamWriter fw = new StreamWriter(@ConfigurationSettings.AppSettings["mailPickupDirectory"] + filename)) {
				fw.Write(sw.ToString());
			}*/
            #endregion

            if (ToEmail.ToString().Trim() != string.Empty)
            {
                #region view how the code should be - COMMENTED FOR NOW!!
                /*StringBuilder sw = new StringBuilder(100000);
                string eml_messageID = "x" + QuotedPrintable.RandomString(10, true) + "@enterprisecommunicationnetwork.com";
                string boundry_tag = "_=COMMUNICATOR=_" + QuotedPrintable.RandomString(32, true);
                boundry_tag = boundry_tag.ToLower();

                string textBody = HTMLFunctions.StripTextFromHtml(Body);

                // make the RFC 822 Style email
                sw.Append("X-Sender: " + FromEmail.Trim().ToString() + "\r\n");
                sw.Append("X-Receiver: " + ToEmail.Trim().ToString() + "\r\n");
                sw.Append("From: \"" + FromEmail.Trim().ToString() + "\" <" + FromEmail.Trim().ToString() + ">" + "\r\n");
                sw.Append("To: \"" + ToEmail.Trim().ToString() + "\" <" + ToEmail.Trim().ToString() + ">" + "\r\n");
                sw.Append("Reply-To: " + FromEmail.Trim().ToString() + "\r\n");
                //sw.WriteLine("Date: " + DateTime.Now.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'CST'"));
                int hours_to_add = 0 - Convert.ToInt32(DateTime.Now.ToString(" z") + "\r\n");
                sw.Append("Date: " + DateTime.Now.AddHours(hours_to_add).ToString("r") + "\r\n");

                //We don't need Organization Header Tag any more - ashok - 09/04/07
                //sw.Append("Organization: Microsoft" + "\r\n");
                // RFC 1522 Email Subject
                sw.Append("Subject:" + Subject.Trim().ToString() + "\r\n");
                sw.Append("X-Mailer: ECN Communicator 5.1" + "\r\n");
                sw.Append("X-RCPT-TO: <" + ToEmail.Trim().ToString() + ">" + "\r\n");
                sw.Append("Message-ID: <" + eml_messageID + ">" + "\r\n");
                sw.Append("MIME-Version: 1.0" + "\r\n");

                sw.Append("Content-Type: multipart/alternative; boundary=\"" + boundry_tag + "\"");
                sw.Append("\r\n");
                sw.Append("\r\n");
                sw.Append(QuotedPrintable.Encode(textBody) + "\r\n");

                sw.Append("\r\n");
                sw.Append("--" + boundry_tag);
                sw.Append("\r\n");

                sw.Append("Content-Type: text/plain; charset=\"ISO-8859-1\"");
                sw.Append("\r\n");
                sw.Append("Content-Transfer-Encoding: quoted-printable");
                sw.Append("\r\n");
                sw.Append("\r\n");
                sw.Append(QuotedPrintable.Encode(textBody) + "\r\n");
                sw.Append("\r\n");

                sw.Append("\r\n");
                sw.Append("--" + boundry_tag);
                sw.Append("\r\n");
                sw.Append("Content-Type: text/html; charset=\"ISO-8859-1\"");
                sw.Append("\r\n");
                sw.Append("Content-Transfer-Encoding: quoted-printable");
                sw.Append("\r\n");
                sw.Append("\r\n");
                sw.Append(QuotedPrintable.Encode(Body.ToString()) + "\r\n");

                sw.Append("--" + boundry_tag + "--");
                sw.Append("\r\n");
                sw.Append("\r\n");
                //return sw;
                 */
                #endregion

                System.Net.Mail.MailMessage simpleMail = new System.Net.Mail.MailMessage();
                simpleMail.From = new System.Net.Mail.MailAddress(FromEmail.ToString());
                simpleMail.To.Add(ToEmail.ToString());
                simpleMail.Subject = Subject.ToString();
                simpleMail.Body = Body.ToString();
                simpleMail.IsBodyHtml = true;
                simpleMail.Priority = System.Net.Mail.MailPriority.Normal;

                System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString());
                //trying twice to send the email as sometimes the connection is forcibly closed.
                try
                {
                    smtpclient.Send(simpleMail);
                }
                catch (Exception)
                {
                    smtpclient.Send(simpleMail);
                }
            }
        }

        /// Creates a general pickup for a blast. Horrible name will stay as I cannot really refactor right now.
        /// <param name="BlastID"> The BlastID we want to send to</param>
        /// <param name="filterID"> The filter we want to use </param>
        /// <param name="VirtualPath"> The Virtual path for the links</param>
        /// <param name="HostName"> The hostname for the links</param>
        /// <param name="BounceDomain"> Which domain to bounce to</param>
        //public void MSPickupSendThread(int BlastID, int filterID, string VirtualPath, string HostName, string BounceDomain)
        //{

        //    string sqlBlastQuery =
        //        " SELECT * " +
        //        " FROM Blast " +
        //        " WHERE BlastID=" + BlastID + " ";
        //    DataTable dt = DataFunctions.GetDataTable(sqlBlastQuery);
        //    DataRow blast_info = dt.Rows[0];

        //    // Make the list to send it too
        //    int smartSegmentID = 0;
        //    int.TryParse(blast_info["SmartSegmentID"].ToString(), out smartSegmentID);
        //    SendBlastForEmails(blast_info, VirtualPath, HostName, BounceDomain, CreateEmailList(filterID, smartSegmentID, blast.CustomerID"], blast.GroupID"], BounceDomain, BlastID.ToString()), false, false);

        //}

        /// Designed to get called by the bounce engine, it gathers all of the soft bounces and resends them. It then
        /// updates the log tables to indicate this resend.

        /// <param name="BlastID">The blast we want to resend for</param>
        /// <param name="VirtualPath"> The image path</param>
        /// <param name="HostName">The host for link creation</param>
        /// <param name="BounceDomain"> The domain that we want to bounces to</param>

        public void MSPickupReSendBounceThread(int BlastID, string VirtualPath, string HostName, string BounceDomain)
        {
            ECN_Framework_Entities.Communicator.Blast blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(BlastID, false);

            //-- Modified to use Sproc "FilterEmails_with_smartSegment" -- Sunil 4/16/2007 
            //-- Modified to use new proc -- Sunil 04/26/07
            SqlCommand emailsListCmd;
            try
            {
                emailsListCmd = new SqlCommand(ConfigurationManager.AppSettings["DynamicContentProc"]);
            }
            catch (Exception)
            {
                emailsListCmd = new SqlCommand("v_Blast_GetBlastEmailsListForDynamicContent");
            }
            emailsListCmd.CommandTimeout = 0;
            emailsListCmd.CommandType = CommandType.StoredProcedure;

            //--CustomerID
            emailsListCmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            emailsListCmd.Parameters["@CustomerID"].Value = blast.CustomerID.Value;
            //--BlastID
            emailsListCmd.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            emailsListCmd.Parameters["@BlastID"].Value = Convert.ToInt32(BlastID);
            //--GroupID
            emailsListCmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
            emailsListCmd.Parameters["@GroupID"].Value = blast.GroupID.Value;
            //--FilterID
            emailsListCmd.Parameters.Add(new SqlParameter("@FilterID", SqlDbType.Int));
            emailsListCmd.Parameters["@FilterID"].Value = 0;
            //--Blast plus Bounce Domain
            emailsListCmd.Parameters.Add(new SqlParameter("@BlastID_and_BounceDomain", SqlDbType.VarChar, 250));
            emailsListCmd.Parameters["@BlastID_and_BounceDomain"].Value = BlastID + "@" + BounceDomain;
            //--ActionType for smartSegment which is "softbounce"
            emailsListCmd.Parameters.Add(new SqlParameter("@ActionType", SqlDbType.VarChar, 10));
            emailsListCmd.Parameters["@ActionType"].Value = "softbounce";
            //-- Reference BlastID for smartSegment
            emailsListCmd.Parameters.Add(new SqlParameter("@refBlastID", SqlDbType.VarChar));
            emailsListCmd.Parameters["@refBlastID"].Value = "0";
            //blastSuppression list
            emailsListCmd.Parameters.Add(new SqlParameter("@SupressionList", SqlDbType.VarChar, 2000));
            emailsListCmd.Parameters["@SupressionList"].Value = "";
            // for getting only counts
            emailsListCmd.Parameters.Add(new SqlParameter("@OnlyCounts", SqlDbType.Bit));
            emailsListCmd.Parameters["@OnlyCounts"].Value = 0;

            DataTable EmailTable = DataFunctions.GetDataTable(emailsListCmd);

            SendBlastForEmails(blast, VirtualPath, HostName, BounceDomain, EmailTable, true, false);

            //update resend bounce - added by Sunil - 12/03/2008

            SqlCommand cmdResendUpdate = new SqlCommand("spReSend_Softbounce_Update");
            cmdResendUpdate.CommandTimeout = 0;
            cmdResendUpdate.CommandType = CommandType.StoredProcedure;

            //--BlastID
            cmdResendUpdate.Parameters.Add(new SqlParameter("@BlastID", SqlDbType.Int));
            cmdResendUpdate.Parameters["@BlastID"].Value = Convert.ToInt32(BlastID);

            DataFunctions.Execute("activity", cmdResendUpdate);

            /*
            String sqlQuery = "";

            sqlQuery = "SELECT distinct e.EmailID as EmailID" +
                " FROM Emails e, EmailActivityLog ea " +
                " WHERE ea.blastID = " + BlastID +
                " AND e.EmailID = ea.EmailID " +
                " AND ea.ActionValue IN ('soft', 'softbounce') ";
            dt = DataFunctions.GetDataTable(sqlQuery);

            foreach(DataRow dr in dt.Rows) {

                string emailID = dr["EmailID"].ToString();

                // Update the EmailActivityLog table with 
                // ActionTypeCode to 'bounce' and ActionValue to a 'resend' 
                // so that undelivered count(bounce) is maintained
                // in the "Successful:"  on reports
                sqlQuery = "UPDATE EmailActivityLog " +
                    " SET ActionTypeCode = 'bounce', ActionValue = 'resend' " +
                    " WHERE EmailID = " + emailID +
                    " AND BlastID = " + BlastID +
                    " AND ActionValue = 'soft'";
                DataFunctions.Execute(sqlQuery);


                // insert a different row in the EmailActivityLog table with 
                // ActionTypeCode to 'resend' and ActionValue to a 'resend'
                // so that there is a record of the bounce -> Unsubscribed 
                string now = System.DateTime.Now.ToString();
                sqlQuery = "INSERT INTO  EmailActivityLog ( " +
                    " EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue )" +
                    " VALUES ( " + emailID + ", " + BlastID + ", " +
                    " 'resend', '" + now + "', 'resend' )";
                DataFunctions.Execute(sqlQuery);
            }
             * */
        }

        #region Private Methods
        // Older form of sendblast before "forwards message on top of blast" came into vogue.
        private void SendBlastForEmails(ECN_Framework_Entities.Communicator.Blast blast, string VirtualPath, string HostName, string BounceDomain, DataTable EmailTable, bool Resend, bool TestBlast, bool doSocialMedia = true)
        {
            SendBlastForEmails(blast, VirtualPath, HostName, BounceDomain, EmailTable, Resend, TestBlast, "", doSocialMedia);
        }


        /// Sends an email blast, either static or dynamic, to the table indicated in the arg list

        /// <param name="blast_info">A row from the Blasts table that indicates which blast to use</param>
        /// <param name="VirtualPath">A virtual path to the images directory</param>
        /// <param name="HostName"> The host domain for link rewrites</param>
        /// <param name="BounceDomain"> The bounce domain for generating the soft bounces</param>
        /// <param name="EmailTable"> A selection from the Emails table that we want to blast to</param>

        private void SendBlastForEmails(ECN_Framework_Entities.Communicator.Blast blast, string VirtualPath, string HostName, string BounceDomain, DataTable EmailTable, bool Resend, bool TestBlast, string ForwardEmail, bool doSocialMedia = true)
        {
            // Reset the blast info if we have a champion blast
            //Console.WriteLine("Sending Blast for email");
            int sampleID = blast.SampleID.HasValue ? blast.SampleID.Value : 0;

            if (sampleID > 0 && blast.BlastType == ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Champion.ToString())
            {
                if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                    KM.Common.FileFunctions.LogActivity(false, string.Format("Starting SenBlastForEmails.Champion: {0}", blast.BlastID), string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

                int userID = ECN_Framework_BusinessLayer.Communicator.Blast.GetSampleBlastUserBySampleID(sampleID);
                //build user object 
                KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByUserID(userID, false);
                ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(blast.CustomerID.Value, false);
                ECN_Framework_Entities.Accounts.BaseChannel bc = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(c.BaseChannelID.Value);
                user.CurrentClient = new KMPlatform.BusinessLogic.Client().Select(c.PlatformClientID, true);
                user.CurrentClientGroup = new KMPlatform.BusinessLogic.ClientGroup().Select(bc.PlatformClientGroupID, true);
                user.CurrentSecurityGroup = new KMPlatform.BusinessLogic.SecurityGroup().Select(user.UserID, user.CurrentClient.ClientID, false, true);

                //KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"], true);
                ECN_Framework_Entities.Communicator.Sample sample = ECN_Framework_BusinessLayer.Communicator.Sample.GetBySampleID_NoAccessCheck(sampleID, user);
                int winningBlastID = Convert.ToInt32(ECN_Framework_BusinessLayer.Activity.View.BlastActivity.ChampionByProc_NoAccessCheck(sampleID, sample.CustomerID.Value, true, sample.ABWinnerType).Rows[0][0].ToString());
                ECN_Framework_Entities.Communicator.BlastAbstract winningBlast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(winningBlastID, false);
                if (winningBlast != null)
                {
                    ECN_Framework_Entities.Communicator.ChampionAudit championAudit = new ECN_Framework_Entities.Communicator.ChampionAudit();
                    List<ECN_Framework_Entities.Communicator.BlastAbstract> championList = ECN_Framework_BusinessLayer.Communicator.BlastChampion.GetBySampleID_NoAccessCheck(sampleID, false);
                    if (championList.Count == 3)
                    {
                        List<ECN_Framework_Entities.Communicator.BlastAbstract> abList = championList.FindAll(sampleElement => sampleElement.BlastType.ToLower() == "sample");
                        championAudit.SampleID = sampleID;
                        championAudit.BlastIDA = abList[0].BlastID;
                        championAudit.BlastIDB = abList[1].BlastID;
                        championAudit.BlastIDChampion = championList.Find(sampleElement => sampleElement.BlastType.ToLower() == "champion").BlastID;
                        championAudit.BlastIDWinning = winningBlastID;
                        DataTable abSampleCountTable = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetABSampleCount(abList[0].BlastID, abList[1].BlastID);
                        championAudit.BouncesA = Convert.ToInt32(abSampleCountTable.Rows[0].ItemArray[1]);
                        championAudit.BouncesB = Convert.ToInt32(abSampleCountTable.Rows[1].ItemArray[1]);
                        championAudit.OpensA = Convert.ToInt32(abSampleCountTable.Rows[0].ItemArray[2]);
                        championAudit.OpensB = Convert.ToInt32(abSampleCountTable.Rows[1].ItemArray[2]);
                        championAudit.ClicksA = Convert.ToInt32(abSampleCountTable.Rows[0].ItemArray[3]);
                        championAudit.ClicksB = Convert.ToInt32(abSampleCountTable.Rows[1].ItemArray[3]);
                        championAudit.Reason = sample.ABWinnerType;
                        ECN_Framework_BusinessLayer.Communicator.ChampionAudit.Insert(championAudit, user);
                    }
                }

                blast.EmailSubject = winningBlast.EmailSubject;
                blast.EmailFrom = winningBlast.EmailFrom;
                blast.ReplyTo = winningBlast.ReplyTo;
                blast.EmailFromName = winningBlast.EmailFromName;
                blast.GroupID = winningBlast.GroupID.Value;
                blast.LayoutID = winningBlast.LayoutID.Value;
                
                DataFunctions.Execute("Update Blast set EmailSubject = '" + blast.EmailSubject.ToString().Replace("'", "''") + "', GroupID = " + blast.GroupID + " , LayoutID = " + blast.LayoutID +
                    ", EmailFrom = '" + winningBlast.EmailFrom + "',ReplyTo = '" + winningBlast.ReplyTo + "',EmailFromName = '" + winningBlast.EmailFromName.Replace("'", "''") + "'" +
                    " WHERE BlastID = " + blast.BlastID);

                DataFunctions.Execute("Update CampaignItemBlast set EmailSubject = '" + blast.EmailSubject.ToString().Replace("'", "''") + "', GroupID = " + blast.GroupID + " , LayoutID = " + blast.LayoutID +
                    ", EmailFrom = '" + winningBlast.EmailFrom + "',ReplyTo = '" + winningBlast.ReplyTo + "',FromName = '" + winningBlast.EmailFromName.Replace("'", "''") + "'" +
                    " WHERE BlastID = " + blast.BlastID);

                sample.WinningBlastID = winningBlastID;
                sample.UpdatedUserID = user.UserID;
                ECN_Framework_BusinessLayer.Communicator.Sample.Save_NoAccessCheck(sample, user);

                //int filterID = 0;
                //int.TryParse(blast_info["FilterID"].ToString(), out filterID);
                //int smartSegmentID = 0;
                //int.TryParse(blast_info["SmartSegmentID"].ToString(), out smartSegmentID);
                EmailTable = CreateEmailList(blast, BounceDomain, TestBlast);
            }
            int blastID = blast.BlastID;
            if (doSocialMedia == true && blast.TestBlast.Equals("N") && (blast.BlastType.Equals("HTML") || blast.BlastType.Equals(ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Champion.ToString()) || blast.BlastType.ToLower().Equals("salesforce")))
                DoSocialMedia(blast.BlastID,  blast.LayoutID.Value, blast.GroupID.Value);
            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, "Start Create Layout and Emails Mapping ", string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

            Hashtable content_lookup = CreateLayoutAndEmailsMapping(blast, VirtualPath, HostName, BounceDomain, EmailTable, Resend, TestBlast, ForwardEmail);
            if (ECN_Framework_Common.Functions.LoggingFunctions.LogStatistics())
                KM.Common.FileFunctions.LogActivity(false, "Done Creating Layout and Emails Mapping ", string.Format("statistics_{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), DateTime.Now.ToShortDateString().ToString().Replace("/", "-")));

            int tempcount = 0;
            // record the number we intend on sending out.
            if (!Resend)
            {
                try
                {
                    tempcount = EmailTable.AsEnumerable().Select(r => r.Field<int>("emailid")).Distinct().Count();

                    //DataView view = EmailTable.DefaultView;
                    //DataTable distinctTable = view.ToTable("EmailTable", true, "emailid");
                    //if (distinctTable != null && distinctTable.Rows.Count > 0)
                    //    tempcount = distinctTable.Rows.Count;
                }
                catch (Exception)
                {
                }
                UpdateSendTotal(blast.BlastID, tempcount);
            }
            Blast(content_lookup);

            Console.WriteLine("Total Emails sent: " + tempcount + "--- " + DateTime.Now);
            DataFunctions.Execute("Update Blast set StatusCode = 'Sent' WHERE BlastID = " + blast.BlastID);
        }

        private void DoSocialMedia(int blastID, int layoutID, int groupID)
        {
            //KMPlatform.Entity.User user = new KMPlatform.BusinessLogic.User().SelectUser(userID, true);
            ECN_Framework_Entities.Communicator.CampaignItem CampaignItem = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID_NoAccessCheck(blastID, false);


            List<ECN_Framework_Entities.Communicator.CampaignItemSocialMedia> listCISM = ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.GetByCampaignItemID(CampaignItem.CampaignItemID);
            if (listCISM.Count > 0)
            {
                foreach (ECN_Framework_Entities.Communicator.CampaignItemSocialMedia cism in listCISM)
                {
                    if (cism.Status == ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType.Created.ToString() && cism.SimpleShareDetailID != null)
                    {
                        cism.Status = ECN_Framework_Common.Objects.Communicator.Enums.SocialMediaStatusType.Pending.ToString();
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.Save(cism);
                    }
                    //if (!cism.HasBeenSent.HasValue || !cism.HasBeenSent.Value)
                    //{
                    //    KM.Common.Entity.Encryption ec = KM.Common.Entity.Encryption.GetCurrentByApplicationID(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KMCommon_Application"]));
                    //    ECN_Framework_Entities.Communicator.SimpleShareDetail ssd = null;
                    //    ECN_Framework_Entities.Communicator.SocialMediaAuth sma = null;
                    //    //FaceBook
                    //    if (cism.SocialMediaID == 1 && cism.SimpleShareDetailID != null)
                    //    {
                    //        string vals = string.Empty;
                    //        string FBPagePOSTURL = string.Empty;
                    //        try
                    //        {
                    //            ssd = ECN_Framework_BusinessLayer.Communicator.SimpleShareDetail.GetBySimpleShareDetailID(cism.SimpleShareDetailID.Value);
                    //            sma = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.GetBySocialMediaAuthID(cism.SocialMediaAuthID.Value);

                    //            string previewLink = ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + ConfigurationManager.AppSettings["SocialPreview"].ToString();
                    //            string queryString = "blastID=" + blastID + "&layoutID=" + layoutID + "&m=1&g=" + groupID;
                    //            string encryptedString = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
                    //            previewLink += encryptedString;

                    //            FBPagePOSTURL = string.Format("https://graph.facebook.com/v2.2/{0}/feed?", ssd.PageID);
                    //            FBPagePOSTURL += "&access_token=" + ssd.PageAccessToken;
                    //            FBPagePOSTURL += "&link=" + System.Web.HttpUtility.UrlEncode(previewLink.Trim());
                    //            if (ssd.UseThumbnail.HasValue && ssd.UseThumbnail.Value)
                    //                FBPagePOSTURL += "&picture=" + System.Web.HttpUtility.UrlEncode(ssd.ImagePath.Trim());
                    //            FBPagePOSTURL += "&name=" + ssd.Title.Trim();
                    //            FBPagePOSTURL += "&caption=" + ssd.Title.Trim();
                    //            FBPagePOSTURL += "&description=" + ssd.SubTitle.Trim();
                    //            FBPagePOSTURL += "&message=" + ssd.Content.Trim().Replace(" ", "+");
                    //            FBPagePOSTURL += "&access_token=" + cism.PageAccessToken.Trim();

                    //            HttpWebRequest request = WebRequest.Create(FBPagePOSTURL) as HttpWebRequest;
                    //            request.Method = "POST";

                    //            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    //            {
                    //                StreamReader stream = new StreamReader(response.GetResponseStream());
                    //                vals = stream.ReadToEnd();
                    //                vals = ECN_Framework_Common.Objects.SocialMediaHelper.CleanJSONString(vals);
                    //                if (vals.Contains("id:"))
                    //                {
                    //                    //successful post
                    //                    cism.HasBeenSent = true;
                    //                    ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.Save(cism);
                    //                }
                    //                else
                    //                {
                    //                    //unsuccessful post
                    //                    Exception ex = new Exception(vals);
                    //                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "BlastEngine.DoSocialMedia", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), "Unsuccessful post to FaceBook: " + FBPagePOSTURL + ", BlastID: " + blastID.ToString() + ", Vals: " + vals);

                    //                }
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "BlastEngine.DoSocialMedia", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), "Unsuccessful post to FaceBook: " + FBPagePOSTURL + ", BlastID: " + blastID.ToString() + ", Vals: " + vals);
                    //        }
                    //    }
                    //    //Twitter
                    //    else if (cism.SocialMediaID == 2 && cism.SimpleShareDetailID != null)
                    //    {
                    //        ECN_Framework_Common.Objects.OAuthHelper oauth = new ECN_Framework_Common.Objects.OAuthHelper();
                    //        try
                    //        {
                    //            ssd = ECN_Framework_BusinessLayer.Communicator.SimpleShareDetail.GetBySimpleShareDetailID(cism.SimpleShareDetailID.Value);
                    //            sma = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.GetBySocialMediaAuthID(cism.SocialMediaAuthID.Value);

                    //            string postData = ssd.Content + " " + ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + ConfigurationManager.AppSettings["SocialPreview"].ToString();
                    //            string queryString = "blastID=" + blastID + "&layoutID=" + layoutID +"&m=2&g=" + groupID ;
                    //            string encryptedString = System.Web.HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, ec));
                    //            postData += encryptedString;

                    //            oauth.TweetOnBehalfOf(sma.Access_Token, sma.Access_Secret, postData, "");

                    //            if (string.IsNullOrEmpty(oauth.oauth_error))
                    //            {
                    //                cism.HasBeenSent = true;
                    //                ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.Save(cism);
                    //            }
                    //            else
                    //            {
                    //                //unsuccessful post
                    //                Exception ex = new Exception(oauth.oauth_error);
                    //                if(sma == null || ssd == null || oauth == null)
                    //                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "BlastEngine.DoSocialMedia", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), "Unsuccessful post to Twitter, BlastID: " + blastID.ToString());
                    //                else
                    //                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "BlastEngine.DoSocialMedia", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), "Unsuccessful post to Twitter, Token: " + sma.Access_Token + ", Secret: " + sma.Access_Secret + ", Content: " + ssd.Content + ", BlastID: " + blastID.ToString() + ", OauthError: " + oauth.oauth_error);
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            string oauthError = string.Empty;
                    //            if (oauth != null)
                    //            {
                    //                oauthError = oauth.oauth_error;
                    //            }
                    //            if (sma == null || ssd == null)
                    //                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "BlastEngine.DoSocialMedia", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), "Unsuccessful post to Twitter, BlastID: " + blastID.ToString() + ", OauthError: " + oauthError);
                    //            else
                    //                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "BlastEngine.DoSocialMedia", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), "Unsuccessful post to Twitter, Token: " + sma.Access_Token + ", Secret: " + sma.Access_Secret + ", Content: " + ssd.Content + ", BlastID: " + blastID.ToString() + ", OauthError: " + oauthError);
                    //        }
                    //    }
                    //    //LinkedIn
                    //    else if (cism.SocialMediaID == 3 && cism.SimpleShareDetailID != null)
                    //    {
                    //        try
                    //        {
                    //            ssd = ECN_Framework_BusinessLayer.Communicator.SimpleShareDetail.GetBySimpleShareDetailID(cism.SimpleShareDetailID.Value);
                    //            sma = ECN_Framework_BusinessLayer.Communicator.SocialMediaAuth.GetBySocialMediaAuthID(cism.SocialMediaAuthID.Value);

                    //            bool success = ECN_Framework_Common.Objects.SocialMediaHelper.PostToLI(sma.Access_Token, ssd.Title.Trim(), ssd.SubTitle.Trim(),ssd.Content, ssd.UseThumbnail.HasValue ? ssd.UseThumbnail.Value : false, ssd.ImagePath.Trim(),ssd.PageID, blastID, layoutID, groupID);

                    //            if (success)
                    //            {
                    //                cism.HasBeenSent = true;
                    //                ECN_Framework_BusinessLayer.Communicator.CampaignItemSocialMedia.Save(cism);

                    //            }

                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            if (sma == null || ssd == null)
                    //                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "BlastEngine.DoSocialMedia", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), "Unsuccessful post to LinkedIn, BlastID: " + blastID.ToString());
                    //            else
                    //                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "BlastEngine.DoSocialMedia", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), "Unsuccessful post to LinkedIn, Token: " + sma.Access_Token + ", Title: " + ssd.Title.Trim() + ", SubTitle: " + ssd.SubTitle.Trim() + ", BlastID: " + blastID.ToString());
                    //        }
                    //    }
                    //}
                }
            }
        }

        private Hashtable CreateLayoutAndEmailsMapping(Blast blast, string virtualPath, string hostName, string bounceDomain, DataTable emailTable, bool resend, bool testBlast, string forwardEmail)
        {
            Console.WriteLine($"EmailFunctions: {nameof(CreateLayoutAndEmailsMapping)}() - started at: {DateTime.Now}");
            var contentLookup = new Hashtable();
            if (blast.CustomerID != null)
            {
                var customerId = blast.CustomerID.Value;
                if (blast.LayoutID != null)
                {
                    var layoutId = blast.LayoutID.Value;
                    var layout = Layout.GetByLayoutID_NoAccessCheck(layoutId, false);
                    if (layout.TemplateID != null)
                    {
                        CreateLayoutTemplateIdNotNull(
                            blast,
                            virtualPath,
                            hostName,
                            emailTable,
                            resend,
                            testBlast,
                            forwardEmail,
                            layout,
                            layoutId,
                            customerId,
                            contentLookup);
                    }
                }
            }

            if (LoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    $"Ending {nameof(EmailFunctions)}.{nameof(this.CreateLayoutAndEmailsMapping)}: {blast.BlastID}",
                    $"statistics_{Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)}_{DateTime.Now.ToShortDateString().Replace(Slash, Hyphen)}");
            }

            return contentLookup;
        }

        private void CreateLayoutTemplateIdNotNull(
            Blast blast,
            string virtualPath,
            string hostName,
            DataTable emailTable,
            bool resend,
            bool testBlast,
            string forwardEmail,
            EntitiesLayout layout1,
            int layoutId,
            int customerId,
            Hashtable contentLookup)
        {
            var blastId = blast.BlastID;
            var layout = new Layouts(layoutId);
            var hashKey = OneEmail;
            var user = User.GetByAccessKey(ConfigurationManager.AppSettings[EcnEngineAccessKey], true);
            var dynamicFromName = blast.DynamicFromName;
            var dynamicFromEmail = blast.DynamicFromEmail;
            var dynamicReplyToEmail = blast.DynamicReplyToEmail;
            var hasDynamicContent = ContentFilter.HasDynamicContent(layoutId);
            var hasDynamicTags = false;
            if (layout1.TemplateID != null)
            {
                var template = Template.GetByTemplateID_NoAccessCheck(layout1.TemplateID.Value);
                var myhtmlbody = Layout.GetPreviewNoAccessCheck(layoutId, Enums.ContentTypeCode.HTML, false, customerId, null, null, blastId);
                var mytextbody = Layout.GetPreviewNoAccessCheck(layoutId, Enums.ContentTypeCode.TEXT, false, customerId, null, null, blastId);
                var myPersonalizedhtmlbody = string.Empty;
                var myPersonalizedtextbody = string.Empty;
                if (blast.BlastType.Equals(nameof(Enums.BlastType.Personalization), StringComparison.OrdinalIgnoreCase))
                {
                    myPersonalizedhtmlbody = GetHTMLBodyforPersonalizedEmail(template.TemplateSource, layout1.TableOptions);
                    myPersonalizedtextbody = GetTEXTBodyforPersonalizedEmail(template.TemplateText);
                }

                if (!hasDynamicContent)
                {
                    var toParse = new List<string> { myhtmlbody, mytextbody };
                    if (Content.GetTags(toParse, true).Count > 0)
                    {
                        hasDynamicTags = true;
                    }
                }

                Console.WriteLine($"    {nameof(CreateLayoutAndEmailsMapping)}() - Has Dynamic Tag: {hasDynamicTags}");
                Console.WriteLine($"    {nameof(CreateLayoutAndEmailsMapping)}() - Has Dynamic Slot: {hasDynamicContent}");

                // If we have non-dynamic content, we can use the older preview code. Otherwise we need to build the content ourselves.
                if (!hasDynamicContent && !hasDynamicTags)
                {
                    CreateLayoutNoDynamicContentAndNoDynamicTags(
                        blast,
                        virtualPath,
                        hostName,
                        emailTable,
                        resend,
                        testBlast,
                        forwardEmail,
                        template,
                        layout1,
                        myhtmlbody,
                        customerId,
                        blastId,
                        mytextbody,
                        myPersonalizedhtmlbody,
                        myPersonalizedtextbody,
                        contentLookup,
                        hashKey,
                        dynamicFromName,
                        dynamicFromEmail,
                        dynamicReplyToEmail);
                }
                else
                {
                    CreateLayoutHasDynamicContentOrDynamicTags(
                        blast,
                        virtualPath,
                        hostName,
                        emailTable,
                        resend,
                        testBlast,
                        forwardEmail,
                        template,
                        layout1,
                        contentLookup,
                        customerId,
                        blastId,
                        dynamicFromName,
                        dynamicFromEmail,
                        dynamicReplyToEmail);
                }
            }
        }

        private void CreateLayoutHasDynamicContentOrDynamicTags(
            Blast blast,
            string virtualPath,
            string hostName,
            DataTable emailTable,
            bool resend,
            bool testBlast,
            string forwardEmail,
            EntitiesTemplate template,
            EntitiesLayout layout,
            Hashtable contentLookup,
            int customerId,
            int blastId,
            string dynamicFromName,
            string dynamicFromEmail,
            string dynamicReplyToEmail)
        {
            if (LoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    $"Starting {nameof(EmailFunctions)}.{nameof(CreateLayoutAndEmailsMapping)} With Dynamic: {blast.BlastID}",
                    $"statistics_{Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)}_{DateTime.Now.ToShortDateString().Replace(Slash, Hyphen)}");
            }

            var dHasDynamicTag = new Dictionary<string, Tuple<List<string>, string, string>>();

            // Get the header information and the default content slots
            var templateSource = template.TemplateSource;
            var templateText = template.TemplateText;
            var tableOptions = layout.TableOptions;
            var defaultSlot1 = layout.ContentSlot1 ?? 0;
            var defaultSlot2 = layout.ContentSlot2 ?? 0;
            var defaultSlot3 = layout.ContentSlot3 ?? 0;
            var defaultSlot4 = layout.ContentSlot4 ?? 0;
            var defaultSlot5 = layout.ContentSlot5 ?? 0;
            var defaultSlot6 = layout.ContentSlot6 ?? 0;
            var defaultSlot7 = layout.ContentSlot7 ?? 0;
            var defaultSlot8 = layout.ContentSlot8 ?? 0;
            var defaultSlot9 = layout.ContentSlot9 ?? 0;

            // Go into every row and get the dynamic slot content based on their user data
            // Generate the page and cache the result
            foreach (DataRow email in emailTable.Rows)
            {
                Func<string, int, int> getSlotValue = (columnName, defaultValue) =>
                    {
                        var value = defaultValue;
                        if(email.Table.Columns.Contains(columnName) && !email.IsNull(columnName) && !int.TryParse(email[columnName].ToString(), out value)) { Trace.TraceError("Invalid cast"); }

                        return value;
                    };

                var slot1 = getSlotValue(Slot1, defaultSlot1);
                var slot2 = getSlotValue(Slot2, defaultSlot2);
                var slot3 = getSlotValue(Slot3, defaultSlot3);
                var slot4 = getSlotValue(Slot4, defaultSlot4);
                var slot5 = getSlotValue(Slot5, defaultSlot5);
                var slot6 = getSlotValue(Slot6, defaultSlot6);
                var slot7 = getSlotValue(Slot7, defaultSlot7);
                var slot8 = getSlotValue(Slot8, defaultSlot8);
                var slot9 = getSlotValue(Slot9, defaultSlot9);

                var hashKey = $"{slot1}{Hyphen}{slot2}{Hyphen}{slot3}{Hyphen}{slot4}{Hyphen}{slot5}{Hyphen}{slot6}{Hyphen}{slot7}{Hyphen}{slot8}{Hyphen}{slot9}";
                var textbody = string.Empty;
                var htmlbody = string.Empty;

                if (!dHasDynamicTag.ContainsKey(hashKey))
                {
                    textbody = TemplateFunctions.EmailTextBody(templateText, slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9);
                    htmlbody = TemplateFunctions.EmailHTMLBody(templateSource, tableOptions, slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8, slot9);
                    var toParse = new List<string> { htmlbody, textbody };
                    dHasDynamicTag.Add(hashKey, Tuple.Create(Content.GetTags(toParse, true), textbody, htmlbody));
                }

                var dynamictagParams = dHasDynamicTag[hashKey];
                var dynamicTags = dynamictagParams.Item1;
                if (dynamictagParams.Item1.Count > 0 && email[BlastId].ToString() != "-1")
                {
                    textbody = dynamictagParams.Item2;
                    htmlbody = dynamictagParams.Item3;
                    foreach (var s in dynamicTags)
                    {
                        hashKey = hashKey + Hyphen + email[s];
                        if (!contentLookup.ContainsKey(hashKey))
                        {
                            var dt2 = DataFunctions.GetDataTable($"SELECT {ContentText}, {ContentSource} FROM Content WHERE ContentID={email[s]} and IsDeleted = 0");
                            textbody = textbody.Replace(s, dt2.Rows[0][ContentText].ToString());
                            htmlbody = htmlbody.Replace(s, dt2.Rows[0][ContentSource].ToString());
                        }
                    }
                }

                // Check to see if we have created this content before. If not, create a new hash key and store the re-written template
                if (!contentLookup.ContainsKey(hashKey))
                {
                    Console.WriteLine($"   {nameof(CreateLayoutAndEmailsMapping)}() - new layout - hash_key - {hashKey} / {DateTime.Now}");
                    textbody = TemplateFunctions.LinkReWriterText(textbody, blast, customerId.ToString(), virtualPath, hostName, forwardEmail);
                    htmlbody = TemplateFunctions.LinkReWriter(htmlbody, blast, customerId.ToString(), virtualPath, hostName, forwardEmail);
                    if (blast.EnableCacheBuster == true)
                    {
                        htmlbody = TemplateFunctions.imgRewriter(htmlbody, blastId);
                        textbody = TemplateFunctions.imgRewriter(textbody, blastId);
                    }

                    htmlbody = TemplateFunctions.addOpensImage(htmlbody, blastId, virtualPath, hostName);
                    var omitDocType = false;
                    if (template.SlotsTotal.HasValue && template.SlotsTotal == 1)
                    {
                        var c = Content.GetByContentID_NoAccessCheck(slot1, false);
                        if (c.UseWYSIWYGeditor.HasValue && c.UseWYSIWYGeditor == false)
                        {
                            omitDocType = true;
                        }
                    }

                    contentLookup.Add(
                        hashKey,
                        new EmailBlast(blastId, emailTable.Clone(), htmlbody, textbody, resend, testBlast, dynamicFromName, dynamicFromEmail, dynamicReplyToEmail, omitDocType, blast));
                    ((EmailBlast)contentLookup[hashKey]).AddEmail(email);
                }
                else
                {
                    ((EmailBlast)contentLookup[hashKey]).AddEmail(email);
                }
            }
        }

        private void CreateLayoutNoDynamicContentAndNoDynamicTags(
            Blast blast,
            string virtualPath,
            string hostName,
            DataTable emailTable,
            bool resend,
            bool testBlast,
            string forwardEmail,
            EntitiesTemplate template,
            EntitiesLayout layout1,
            string myhtmlbody,
            int customerId,
            int blastId,
            string mytextbody,
            string myPersonalizedhtmlbody,
            string myPersonalizedtextbody,
            Hashtable contentLookup,
            string hashKey,
            string dynamicFromName,
            string dynamicFromEmail,
            string dynamicReplyToEmail)
        {
            if (LoggingFunctions.LogStatistics())
            {
                FileFunctions.LogActivity(
                    false,
                    $"Starting {nameof(EmailFunctions)}.{nameof(CreateLayoutAndEmailsMapping)} No Dynamic: {blast.BlastID}",
                    $"statistics_{Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)}_{DateTime.Now.ToShortDateString().Replace(Slash, Hyphen)}");
            }

            var omitDocType = false;
            if (template.SlotsTotal == 1 && layout1.ContentSlot1 != null)
            {
                if (Content.GetByContentID_NoAccessCheck(layout1.ContentSlot1.Value, false).UseWYSIWYGeditor == false)
                {
                    omitDocType = true;
                }
            }

            myhtmlbody = TemplateFunctions.LinkReWriter(myhtmlbody, blast, customerId.ToString(), virtualPath, hostName, forwardEmail);
            myhtmlbody = TemplateFunctions.addOpensImage(myhtmlbody, blastId, virtualPath, hostName);
            mytextbody = TemplateFunctions.LinkReWriterText(mytextbody, blast, customerId.ToString(), virtualPath, hostName, forwardEmail);
            if (blast.EnableCacheBuster == true)
            {
                myhtmlbody = TemplateFunctions.imgRewriter(myhtmlbody, blastId);
                mytextbody = TemplateFunctions.imgRewriter(mytextbody, blastId);
            }

            if (blast.BlastType.Equals(nameof(Enums.BlastType.Personalization), StringComparison.OrdinalIgnoreCase))
            {
                myPersonalizedhtmlbody = TemplateFunctions.LinkReWriter(myPersonalizedhtmlbody, blast, customerId.ToString(), virtualPath, hostName, forwardEmail);
                myPersonalizedhtmlbody = TemplateFunctions.addOpensImage(myPersonalizedhtmlbody, blastId, virtualPath, hostName);
                myPersonalizedtextbody = TemplateFunctions.LinkReWriterText(myPersonalizedtextbody, blast, customerId.ToString(), virtualPath, hostName, forwardEmail);
                if (blast.EnableCacheBuster == true)
                {
                    myPersonalizedhtmlbody = TemplateFunctions.imgRewriter(myPersonalizedhtmlbody, blastId);
                    myPersonalizedtextbody = TemplateFunctions.imgRewriter(myPersonalizedtextbody, blastId);
                }
            }

            if (!blast.BlastType.Equals(Enums.BlastType.Personalization.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                contentLookup.Add(
                    hashKey,
                    new EmailBlast(blastId, emailTable, myhtmlbody, mytextbody, resend, testBlast, dynamicFromName, dynamicFromEmail, dynamicReplyToEmail, omitDocType, blast));
            }
            else
            {
                var personalizedHashKey = $"{hashKey}{Hyphen}PERSONALIZEDBLAST";
                foreach (DataRow email in emailTable.Rows)
                {
                    if (string.IsNullOrWhiteSpace(email[PersonalizedContentId].ToString()) || int.Parse(email[PersonalizedContentId].ToString()) == 0)
                    {
                        if (!contentLookup.ContainsKey(hashKey))
                        {
                            contentLookup.Add(
                                hashKey,
                                new EmailBlast(
                                    blastId,
                                    emailTable.Clone(),
                                    myhtmlbody,
                                    mytextbody,
                                    resend,
                                    testBlast,
                                    dynamicFromName,
                                    dynamicFromEmail,
                                    dynamicReplyToEmail,
                                    omitDocType,
                                    blast));
                        }

                        ((EmailBlast)contentLookup[hashKey]).AddEmail(email);
                    }
                    else
                    {
                        if (!contentLookup.ContainsKey(personalizedHashKey))
                        {
                            contentLookup.Add(
                                personalizedHashKey,
                                new EmailBlast(
                                    blastId,
                                    emailTable.Clone(),
                                    myPersonalizedhtmlbody,
                                    myPersonalizedtextbody,
                                    resend,
                                    testBlast,
                                    dynamicFromName,
                                    dynamicFromEmail,
                                    dynamicReplyToEmail,
                                    true,
                                    blast));
                        }

                        // Just add the email to this, they want the same email as before
                        ((EmailBlast)contentLookup[personalizedHashKey]).AddEmail(email);
                    }
                }
            }
        }

        private void UpdateSendTotal(int blastID, int SendTotal)
        {
            DataFunctions.Execute(" UPDATE Blast SET " + " SendTotal= ISNULL(SendTotal,0) + " + SendTotal + " WHERE BlastID=" + blastID);
        }


        private void Blast(Hashtable content_lookup)
        {

            IDictionaryEnumerator content_slot = content_lookup.GetEnumerator();
            while (content_slot.MoveNext())
            {
                EmailBlast email_to_blast = (EmailBlast)content_slot.Value;
                email_to_blast.Blast();
            }

        }


        /// Returns the content ID that this particular email_id should see for a particular slot.

        /// <param name="email_row"> A row from the Emails Table</param>
        /// <param name="layout_id"> Which Layout we are blasting</param>
        /// <param name="slot_number"> What slot we are want dynamic</param>
        /// <param name="default_slot"> What we should default to if we can find nothing </param>
        /// <returns>ContentID of dynamic content</returns>
        static int getDynamicContentID(DataRow email_row, int layout_id, int slot_number, int default_slot)
        {
            string sqlstmt = "select WhereClause, ContentID from ContentFilter where IsDeleted = 0 and LayoutID=" + layout_id.ToString() + " AND SlotNumber = " + slot_number.ToString();


            //	    throw new Exception("lay=" + layout_id + " slot = " + slot_number + " default-slot = " + default_slot);
            DataTable dt = DataFunctions.GetDataTable(sqlstmt);
            string sqlquery = "";
            foreach (DataRow possible_slot in dt.Rows)
            {
                /*sqlquery = " SELECT distinct e.EmailID FROM Emails e outer join EmailDataValues v outer join GroupDatafields g WHERE " +
                                    "e.EmailID="+ email_row["EmailID"] +
                          " and v.EmailID = e.EmailID and v.GroupDatafieldsID = g.GroupDatafieldsID and (" +
                          possible_slot["WhereClause"] + ")";
                */
                sqlquery = "SELECT distinct e.EmailID FROM Emails e left outer join EmailDataValues v on (v.EmailID = e.EmailID) left outer join GroupDatafields g on ( v.GroupDatafieldsID = g.GroupDatafieldsID ) WHERE " +
                    "e.EmailID=" + email_row["EmailID"] + " and (" + possible_slot["WhereClause"] + " and g.IsDeleted = 0)";

                //		  throw new Exception("codereturn= " +  sqlquery );
                try
                {
                    object return_value = DataFunctions.ExecuteScalar(sqlquery);
                    if (null != return_value)
                    {
                        //throw new Exception("codereturn= " +  sqlquery );
                        return (int)possible_slot["ContentID"];
                    }
                }
                catch (Exception)
                {
                    //throw new Exception("exception= " +  sqlquery );
                    return default_slot;
                }
            }
            //	    throw new Exception("default= " +  sqlquery );
            return default_slot;
        }


        private static string GetHTMLBodyforPersonalizedEmail(string HTML, string tableoptions)
        {
            string body = string.Empty;
            string tableOptions = tableoptions;

            body = HTML;

            body = StringFunctions.Replace(body, "®", "&reg;");
            body = StringFunctions.Replace(body, "©", "&copy;");
            body = StringFunctions.Replace(body, "™", "&trade;");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");

            if (tableOptions.Length < 1)
            {
                tableOptions = " cellpadding=0 cellspacing=0 width='100%'";
            }
            else if (!tableOptions.ToLower().Contains("width"))
            {
                tableOptions += " width='100%'";
            }
            body = "<table " + tableOptions + "><tr><td>" + body + "</td></tr></table>";
            return body;
        }

        private static string GetTEXTBodyforPersonalizedEmail(string Text)
        {
            string body = Text;

            body = StringFunctions.Replace(body, "®", "");
            body = StringFunctions.Replace(body, "©", "");
            body = StringFunctions.Replace(body, "™", "");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");

            return body;
        }


        #endregion
    }
}
