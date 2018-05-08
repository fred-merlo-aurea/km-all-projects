using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using ecn.common.classes;
using System.Text;
using System.Net.Mail;
using System.Linq;
using System.Collections.Generic;
using ECN.TextPowerAdvanced;
using ECN.TextPowerWebManagement;
using ECN.Communicator;
using System.Threading;

namespace ecn.communicator.classes
{
    public class SmsFunctions
    {
        LicenseCheck lc;  
 
        public SmsFunctions()
        {
            lc = new LicenseCheck();
        }              

        public void SendBlast(int BlastID)
        {
            //Get Blast Details from DB
            string sqlBlastQuery = " SELECT * FROM Blasts WHERE BlastID=" + BlastID;
            DataTable dt = DataFunctions.GetDataTable(sqlBlastQuery,DataFunctions.GetConnectionString("communicator"));
            DataRow blast_info = dt.Rows[0];
            int CustomerID = (int)blast_info["CustomerID"];

            //Get EmailIDs & PhoneNumbers from DB using Blast details
            DataTable  profilesToSend=GetNumbers(blast_info);

            //Create Opt In
            List<int> OptInsmsActivity = CreateOptIn(profilesToSend, CustomerID, BlastID);

            ////Update WelcomeMsg Activity table in DB
            //InsertSMSActivity(OptInsmsActivity, BlastID, true);
            
            ////Create Send List with the same name as BlastID
            //List<int> smsActivity = CreateSendList(profilesToSend, BlastID, CustomerID);

            ////Send out a text message to the SendList 
            //SendToSendList(BlastID, GetContent(blast_info), CustomerID);

            ////Update Message Activity table in DB
            //InsertSMSActivity(smsActivity, BlastID, false);

            //Update Blast details in DB
            UpdateECN_BlastStatus(BlastID, "deployed");         
        }

        private void UpdateECN_BlastStatus(int BlastID, string status)
        {
            string sqlBlastQuery = " update Blasts set StatusCode='" + status + "' WHERE BlastID=" + BlastID;
            DataFunctions.Execute("communicator", sqlBlastQuery);
        }

        private List<int> CreateSendList(DataTable profilesToSend, int BlastID, int CustomerID)
        {
            List<string> sendList = (from src in profilesToSend.AsEnumerable()
                                     orderby src.Field<string>("CellNumber")
                                    select src.Field<string>("CellNumber")).Distinct().ToList();

            DataTable dt = new DataTable();
            DataColumn CellNumber = new DataColumn("CellNumber", typeof(string));
            dt.Columns.Add(CellNumber);
            DataColumn Carrier = new DataColumn("Carrier", typeof(string));
            dt.Columns.Add(Carrier);
            string keyword = GetCustomerKWD(CustomerID);
            int rowcount = 0;
            int counter = 0;
            int remainingCount = sendList.Count;
            for (int i = 0; i < sendList.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["CellNumber"] = sendList[i];
                dr["Carrier"] = "";
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                rowcount++;
                if (rowcount == 500)
                {
                    counter++;
                    remainingCount = sendList.Count - (500 * counter);
                    bool createList = counter > 1 ? false : true;

                    DataSet ds_numbers = new DataSet();
                    ds_numbers.Tables.Add(dt.Copy());
                    Console.WriteLine("Adding 500 numbers to SendList:  " + BlastID.ToString() + " (" + remainingCount.ToString() + " remaining)");
                    SMSBlast.Adv_MergeSendList(SMSBlast.Adv_AuthenticationInfo(keyword), BlastID.ToString(), ds_numbers, createList);                      
                    //Refresh 
                    dt = new DataTable();
                    CellNumber = new DataColumn("CellNumber", typeof(string));
                    dt.Columns.Add(CellNumber);
                    Carrier = new DataColumn("Carrier", typeof(string));
                    dt.Columns.Add(Carrier);
                    rowcount = 0;
                }
            }
            if (remainingCount != 0)
            {
                counter++;
                bool createList = counter > 1 ? false : true;
                DataSet ds_numbers = new DataSet();
                ds_numbers.Tables.Add(dt.Copy());
                Console.WriteLine("Adding " + remainingCount + " numbers to SendList:  " + BlastID.ToString() + " (0 remaining)");
                if (counter == 1)
                    SMSBlast.Adv_MergeSendList(SMSBlast.Adv_AuthenticationInfo(GetCustomerKWD(CustomerID)), BlastID.ToString(), ds_numbers, createList);              

            }
            Console.WriteLine("SendList created");

            //Update License
            lc.UpdateUsed(CustomerID, "smsblock10k", sendList.Count);

            List<int> smsActivity = (from src in profilesToSend.AsEnumerable()
                                        select src.Field<int>("EmailID")).Distinct().ToList();

            //Wait for TextPower to Lookup Carrier Codes
            int noCarrier = (from src in profilesToSend.AsEnumerable()
                             where src.Field<string>("Carrier")==null
                             select src.Field<string>("CellNumber")).Distinct().Count();
            int delayMinutes = Convert.ToInt32(Math.Ceiling((double) noCarrier / 10000));
            Thread.Sleep(delayMinutes * 60000 * 15);
             
            return smsActivity;          
        }

        private void SendToSendList(int BlastID, string message, int CustomerID)
        {
            SMSBlast.Adv_SendToSendList(SMSBlast.Adv_AuthenticationInfo(GetCustomerKWD(CustomerID)), BlastID.ToString(), message);
        }

        private void InsertSMSActivity(List<int> smsActivity, int BlastID, bool IsWelcomeMsg)
        {
            StringBuilder xml_data = new StringBuilder();

            int rowcount = 0;
            for (int i = 0; i < smsActivity.Count; i++)
            {
                xml_data.Append(string.Format("<Blast EmailID=\"{0}\"/>", smsActivity[i]));
                rowcount++;
                if (rowcount == 5000)
                {
                    SqlCommand SMSActivity = new SqlCommand("sp_InsertSMSActivity");
                    SMSActivity.CommandTimeout = 0;
                    SMSActivity.CommandType = CommandType.StoredProcedure;
                    SMSActivity.Parameters.AddWithValue("@xmldata", xml_data.ToString());
                    SMSActivity.Parameters.AddWithValue("@blastID", BlastID);
                    SMSActivity.Parameters.AddWithValue("@IsWelcomeMsg", IsWelcomeMsg);
                    DataFunctions.Execute(DataFunctions.GetConnectionString("activity"),SMSActivity);
                    xml_data.Clear();
                    rowcount = 0;
                }
            }
            if (xml_data.ToString() != String.Empty)
            {
                SqlCommand SMSActivity = new SqlCommand("sp_InsertSMSActivity");
                SMSActivity.CommandTimeout = 0;
                SMSActivity.CommandType = CommandType.StoredProcedure;
                SMSActivity.Parameters.AddWithValue("@xmldata", xml_data.ToString());
                SMSActivity.Parameters.AddWithValue("@blastID", BlastID);
                SMSActivity.Parameters.AddWithValue("@IsWelcomeMsg", IsWelcomeMsg);
                DataFunctions.Execute("activity", SMSActivity);
                xml_data.Clear();
            }
        }

        private DataTable GetNumbers(DataRow blast_info)
        {
            int GroupID = (int)blast_info["GroupID"];
            int BlastID = (int)blast_info["BlastID"];
            int CustomerID = (int)blast_info["CustomerID"];
            string sqlEmailGroupsQuery =
               " SELECT e.EmailID, e.Mobile as 'CellNumber', e.CarrierCode as 'Carrier', e.SMSOptIn " +
               " FROM Emails e " +
               " inner join EmailGroups eg " +
               " on e.EmailID=eg.EmailID " +
               " where eg.GroupID="+GroupID+" and (e.SMSOptIn is null OR e.SMSOptIn='optin') and eg.SMSEnabled='True' and e.CustomerID="+ CustomerID;
            DataTable dt = DataFunctions.GetDataTable(sqlEmailGroupsQuery, DataFunctions.GetConnectionString("communicator"));           
            return dt;
        }

        private string GetAutoWelcome(int CustomerID)
        {
            string sqlWelcomeMsgQuery =
             " SELECT TextPowerWelcomeMsg from Customers where CustomerID=" + CustomerID;
            DataTable dt = DataFunctions.GetDataTable(sqlWelcomeMsgQuery, DataFunctions.GetConnectionString("accounts"));
            return dt.Rows[0]["TextPowerWelcomeMsg"].ToString();
        }

        private List<int> CreateOptIn(DataTable profilesToSend, int CustomerID, int BlastID)
        {
            List<string> pendingOptIn = (from src in profilesToSend.AsEnumerable()
                                    where src.Field<string>("SMSOptIn")== null
                                    orderby src.Field<string>("CellNumber")
                                    select src.Field<string>("CellNumber")).Distinct().ToList();

            List<int> smsActivity = new List<int>();
            if (pendingOptIn.Count > 0)
            {
                DataTable result = new DataTable();
                result.Columns.Add("Mobile", typeof(string));
                string keyword=GetCustomerKWD(CustomerID);
                int errorCounter = 0;
                //Call TP ManageOptIn and add each number to OptIn List from pending
                for (int i = 0; i < pendingOptIn.Count; i++)
                {
                   
                    try
                    {                       
                        SMSBlast.WebMgmt_ManageOptIn(SMSBlast.WebMgmt_AuthenticationInfo(keyword), "set", pendingOptIn[i], "");
                        result.Rows.Add(pendingOptIn[i].ToString());
                        Console.WriteLine("Number " + pendingOptIn[i] + " (" + (i + 1).ToString() + " out of " + pendingOptIn.Count + ") opted in for keyword :" + keyword);
                        errorCounter = 0;
                    }
                    catch
                    {
                        errorCounter++;
                        Console.WriteLine("Error:" + errorCounter.ToString() + " for number " + pendingOptIn[i]);
                        Thread.Sleep(10000 * 12);
                        if (errorCounter == 5)
                        {
                            Console.WriteLine("Number Skipped: " + pendingOptIn[i]);
                            errorCounter = 0;
                        }
                        else
                        {
                            i--;
                        }
                        
                    }
                    
                }
                DataSet ds_pending = new DataSet();
                ds_pending.Tables.Add(result);

                ////Call TP SendAutoWelcome for all the newly added OptIn Numbers & Update License for auto-welcome msgs
                //string WelcomeMsg = GetAutoWelcome(CustomerID);
                //SMSBlast.WebMgmt_SetWelcomeMsg(SMSBlast.WebMgmt_AuthenticationInfo(GetCustomerKWD(CustomerID)), GetAutoWelcome(CustomerID));
                //SMSBlast.WebMgmt_SendAutoWelcome(SMSBlast.WebMgmt_AuthenticationInfo(GetCustomerKWD(CustomerID)), ds_pending);

                ////Update License
                //lc.UpdateUsed(CustomerID, "smsblock10k", pendingOptIn.Count);

                //string sqlBlastQuery = " update Blasts set SMSOptInTotal=" + pendingOptIn.Count + " WHERE BlastID=" + BlastID;
                //DataFunctions.ExecuteScalar("communicator", sqlBlastQuery);
                
                ////Update SMSOptIn status for each pending number to optin in the Emails table
                //smsActivity = (from src in profilesToSend.AsEnumerable()
                //                            where src.Field<string>("SMSOptIn") == null
                //                            select src.Field<int>("EmailID")).Distinct().ToList();

                //UpdateECN_OptInOptOut(pendingOptIn, "optin", GetCustomerKWD(CustomerID));   
               
            }
            return smsActivity;
        }

        private void UpdateECN_OptInOptOut(List<string> numberList, string status,string keyword)
        {
            StringBuilder xml_data = new StringBuilder();

            int rowcount = 0;
            for (int i = 0; i < numberList.Count; i++)
            {
                xml_data.Append(string.Format("<Blast Mobile=\"{0}\"/>",numberList[i]));
                rowcount++;
                if (rowcount == 5000)
                {
                    SqlCommand SMSActivity = new SqlCommand("sp_UpdateOptInOptOut");
                    SMSActivity.CommandTimeout = 0;
                    SMSActivity.CommandType = CommandType.StoredProcedure;
                    SMSActivity.Parameters.AddWithValue("@xmldata", xml_data.ToString());
                    SMSActivity.Parameters.AddWithValue("@status", status);
                    SMSActivity.Parameters.AddWithValue("@keyword", keyword);
                    DataFunctions.Execute(DataFunctions.GetConnectionString("communicator"), SMSActivity);
                    xml_data.Clear();
                    rowcount = 0;
                }
            }
            if (xml_data.ToString() != String.Empty)
            {
                SqlCommand SMSActivity = new SqlCommand("sp_UpdateOptInOptOut");
                SMSActivity.CommandTimeout = 0;
                SMSActivity.CommandType = CommandType.StoredProcedure;
                SMSActivity.Parameters.AddWithValue("@xmldata", xml_data.ToString());
                SMSActivity.Parameters.AddWithValue("@status", status);
                SMSActivity.Parameters.AddWithValue("@keyword", keyword);
                DataFunctions.Execute(DataFunctions.GetConnectionString("communicator"), SMSActivity);
                xml_data.Clear();
            }           
        }
        
        private string GetContent(DataRow blast_info)
        {
            int LayoutID = (int)blast_info["LayoutID"];           
            string sqlLayoutsQuery =
               " SELECT ContentSMS " +
               " FROM Content c " +
               " inner join Layouts l " +
               " on c.ContentID= l.ContentSlot1 " +
               " where l.LayoutID=" + LayoutID;
            DataTable dt = DataFunctions.GetDataTable( sqlLayoutsQuery, DataFunctions.GetConnectionString("communicator"));
            string ContentText = dt.Rows[0][0].ToString();

            ContentText = ContentText + "-STOP2Quit.";
            return ContentText;
        }

        private string GetCustomerKWD(int CustomerID)
        {
            string sqlCustomerKWDQuery =
            " SELECT TextPowerKWD " +
            " FROM Customers  " +
            " where CustomerID=" + CustomerID;
            DataTable dt = DataFunctions.GetDataTable(sqlCustomerKWDQuery, DataFunctions.GetConnectionString("accounts"));
            return dt.Rows[0][0].ToString();
        }

        public void UpdateECN_DeliveryStatus(int BlastID, int CustomerID)
        { 
            //Call TP GetSendListMembers with BlastID as ListName and get the status
            string keyword = GetCustomerKWD(CustomerID);
            DataSet sendStatus=SMSBlast.Adv_GetSendListMembers(SMSBlast.Adv_AuthenticationInfo(keyword), BlastID.ToString());
            DataTable dt = sendStatus.Tables[0];
            StringBuilder xml_data = new StringBuilder();
           
            int rowcount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string status = "";
                string notes = "";
                if (dt.Rows[i][3].ToString().Equals("OK"))
                {
                    status = "sent";
                }
                else if (dt.Rows[i][3].ToString().Equals("No OptIn"))
                {
                    status = "failed";
                    notes = "optout";
                }
                else
                {
                    status = "";
                }
                xml_data.Append(string.Format("<Blast BlastID=\"{0}\" Mobile=\"{1}\" Carrier=\"{2}\" SendStatus=\"{3}\"  SendTime=\"{4}\" SendID=\"{5}\" FinalStatus=\"{6}\"  Notes=\"{7}\"/>",
                                                BlastID, dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), status, dt.Rows[i][4].ToString(), dt.Rows[i][6].ToString(), dt.Rows[i][7].ToString(), notes));
                 rowcount++;
                if (rowcount == 5000)
                {
                    SqlCommand SMSActivity = new SqlCommand("sp_UpdateSMSActivity");
                    SMSActivity.CommandTimeout = 0;
                    SMSActivity.CommandType = CommandType.StoredProcedure;
                    SMSActivity.Parameters.AddWithValue("@xmldata", xml_data.ToString());
                    DataFunctions.Execute("activity", SMSActivity);
                    xml_data.Clear();
                    rowcount = 0;
                }
            }
            if (xml_data.ToString() != String.Empty)
            {
                SqlCommand SMSActivity = new SqlCommand("sp_UpdateSMSActivity");
                SMSActivity.CommandTimeout = 0;
                SMSActivity.CommandType = CommandType.StoredProcedure;
                SMSActivity.Parameters.AddWithValue("@xmldata", xml_data.ToString());
                DataFunctions.Execute("activity", SMSActivity);
                xml_data.Clear();
            }
            //Change Blast status from deployed to sent
            UpdateECN_BlastStatus(BlastID, "sent");

            List<string> optout = (from src in dt.AsEnumerable()
                                   where src.Field<string>("LastSendStatus") == "No OptIn"
                                    select src.Field<string>("CellNumber")).Distinct().ToList();
            UpdateECN_OptInOptOut(optout, "optout", keyword);

        }

        public int GetBlastRemainingCount(int CustomerID, int GroupID)
        {
            string sqlEmailGroupsQuery =
                 " SELECT distinct e.Mobile as 'CellNumber', '' as 'Carrier', e.SMSOptIn " +
                 " FROM Emails e " +
                 " inner join EmailGroups eg " +
                 " on e.EmailID=eg.EmailID " +
                 " where eg.GroupID=" + GroupID + " and (e.SMSOptIn is null OR e.SMSOptIn='optin') and e.CustomerID=" + CustomerID;
            DataTable dt = DataFunctions.GetDataTable(sqlEmailGroupsQuery, DataFunctions.GetConnectionString("communicator"));
            return Convert.ToInt32(dt.Rows.Count);           
        }

    }
}
