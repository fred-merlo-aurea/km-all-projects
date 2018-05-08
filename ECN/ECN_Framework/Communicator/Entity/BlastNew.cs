//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Data.SqlClient;
//using System.Data;
//using ECN_Framework.Communicator.Abstract;
//using ECN_Framework.Communicator.Object;
//using System.Text;
//using System.Configuration;
//using ECN_Framework.Common;
//using ECN_Framework.Accounts.Entity;

//namespace ECN_Framework.Communicator.Entity
//{
//    [Serializable]
//    [DataContract]
//    public class BlastNew
//    {
//        #region Properties
//        [DataMember]
//        public int BlastID { get; set; }
//        [DataMember]
//        public int? CustomerID { get; set; }
//        [DataMember]
//        public string EmailSubject { get; set; }
//        [DataMember]
//        public string EmailFrom { get; set; }
//        [DataMember]
//        public string EmailFromName { get; set; }
//        [DataMember]
//        public DateTime? SendTime { get; set; }
//        [DataMember]
//        public int? AttemptTotal { get; set; }
//        [DataMember]
//        public int? SendTotal { get; set; }
//        [DataMember]
//        public int? SendBytes { get; set; }
//        [DataMember]
//        public string StatusCode { get; set; }
//        [DataMember]
//        public string BlastType { get; set; }
//        [DataMember]
//        public int? CodeID { get; set; }
//        [DataMember]
//        public int? LayoutID { get; set; }
//        [DataMember]
//        public int? GroupID { get; set; }
//        [DataMember]
//        public DateTime? FinishTime { get; set; }
//        [DataMember]
//        public int? SuccessTotal { get; set; }
//        [DataMember]
//        public string BlastLog { get; set; }
//        [DataMember]
//        public int? UserID { get; set; }
//        [DataMember]
//        public int? FilterID { get; set; }
//        [DataMember]
//        public string Spinlock { get; set; }
//        [DataMember]
//        public string ReplyTo { get; set; }
//        [DataMember]
//        public string TestBlast { get; set; }
//        [DataMember]
//        public string BlastFrequency { get; set; }
//        [DataMember]
//        public string RefBlastID { get; set; }
//        [DataMember]
//        public string BlastSuppression { get; set; }
//        [DataMember]
//        public bool? AddOptOuts_to_MS { get; set; }
//        [DataMember]
//        public string DynamicFromName { get; set; }
//        [DataMember]
//        public string DynamicFromEmail { get; set; }
//        [DataMember]
//        public string DynamicReplyToEmail { get; set; }
//        [DataMember]
//        public int? BlastEngineID { get; set; }
//        [DataMember]
//        public bool? HasEmailPreview { get; set; }
//        [DataMember]
//        public int? BlastScheduleID { get; set; }
//        [DataMember]
//        public int? OverrideAmount { get; set; }
//        [DataMember]
//        public bool? OverrideIsAmount { get; set; }
//        [DataMember]
//        public DateTime? StartTime { get; set; }
//        [DataMember]
//        public int? SMSOptInTotal { get; set; }
//        [DataMember]
//        public int? CampaignItemID { get; set; }
//        [DataMember]
//        public string NodeID { get; set; }
//        [DataMember]
//        public int? SmartSegmentID { get; set; }
//        [DataMember]
//        public int? SampleID { get; set; }
//        //Codes(ID)
//        public ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes? StatusCodeID { get; set; }
//        public ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes? BlastTypeID { get; set; }
//        //validation
//        public List<ECN_Framework_Common.Objects.ValidationError> ErrorList { get; set; }
//        //optional
//        public ECN_Framework_Entities.Communicator.Group Group { get; set; }
//        public ECN_Framework_Entities.Communicator.Layout Layout { get; set; }
//        public KMPlatform.Entity.User BlastUser { get; set; }
//        public ECN_Framework_Entities.Communicator.Filter Filter { get; set; }
//        public ECN_Framework_Entities.Communicator.SmartSegment Segment { get; set; }
//        public ECN_Framework_Entities.Communicator.BlastSchedule Schedule { get; set; }
//        public ECN_Framework_Entities.Communicator.BlastFields Fields { get; set; }
//        #endregion

//        #region Private Helpers
//        private static BlastAbstract GetBlast(DataRow row)
//        {
//            BlastAbstract b = null;

//            b = GetInstance(GetBlastType(row["BlastType"].ToString()));
//            b.BlastType = row["BlastType"].ToString();

//            b.BlastID = Convert.ToInt32(row["BlastID"].ToString());
//            if (!DBNull.Value.Equals(row["CustomerID"]))
//                b.CustomerID = Convert.ToInt32(row["CustomerID"].ToString());
//            b.EmailSubject = row["EmailSubject"].ToString();
//            b.EmailFrom = row["EmailFrom"].ToString();
//            b.EmailFromName = row["EmailFromName"].ToString();
//            if (!DBNull.Value.Equals(row["SendTime"]))
//                b.SendTime = Convert.ToDateTime(row["SendTime"].ToString());
//            if (!DBNull.Value.Equals(row["AttemptTotal"]))
//                b.AttemptTotal = Convert.ToInt32(row["AttemptTotal"].ToString());
//            if (!DBNull.Value.Equals(row["SendTotal"]))
//                b.SendTotal = Convert.ToInt32(row["SendTotal"].ToString());
//            if (!DBNull.Value.Equals(row["SendBytes"]))
//                b.SendBytes = Convert.ToInt32(row["SendBytes"].ToString());
//            b.StatusCode = row["StatusCode"].ToString();
//            b.StatusCodeID = GetBlastStatusCode(b.StatusCode);
//            if (!DBNull.Value.Equals(row["CodeID"]))
//                b.CodeID = Convert.ToInt32(row["CodeID"].ToString());
//            if (!DBNull.Value.Equals(row["LayoutID"]))
//                b.LayoutID = Convert.ToInt32(row["LayoutID"].ToString());
//            if (!DBNull.Value.Equals(row["GroupID"]))
//                b.GroupID = Convert.ToInt32(row["GroupID"].ToString());
//            if (!DBNull.Value.Equals(row["FinishTime"]))
//                b.FinishTime = Convert.ToDateTime(row["FinishTime"].ToString());
//            if (!DBNull.Value.Equals(row["SuccessTotal"]))
//                b.SuccessTotal = Convert.ToInt32(row["SuccessTotal"].ToString());
//            b.BlastLog = row["BlastLog"].ToString();
//            if (!DBNull.Value.Equals(row["UserID"]))
//                b.UserID = Convert.ToInt32(row["UserID"].ToString());
//            if (!DBNull.Value.Equals(row["FilterID"]))
//                b.FilterID = Convert.ToInt32(row["FilterID"].ToString());
//            b.Spinlock = row["Spinlock"].ToString();
//            b.ReplyTo = row["ReplyTo"].ToString();
//            b.TestBlast = row["TestBlast"].ToString();
//            b.BlastFrequency = row["BlastFrequency"].ToString();
//            b.RefBlastID = row["RefBlastID"].ToString();
//            b.BlastSuppression = row["BlastSuppression"].ToString();
//            if (!DBNull.Value.Equals(row["AddOptOuts_to_MS"]))
//                b.AddOptOuts_to_MS = Convert.ToBoolean(row["AddOptOuts_to_MS"].ToString());
//            b.DynamicFromName = row["DynamicFromName"].ToString();
//            b.DynamicFromEmail = row["DynamicFromEmail"].ToString();
//            b.DynamicReplyToEmail = row["DynamicReplyToEmail"].ToString();
//            if (!DBNull.Value.Equals(row["BlastEngineID"]))
//                b.BlastEngineID = Convert.ToInt32(row["BlastEngineID"].ToString());
//            if (!DBNull.Value.Equals(row["HasEmailPreview"]))
//                b.HasEmailPreview = Convert.ToBoolean(row["HasEmailPreview"].ToString());
//            if (!DBNull.Value.Equals(row["BlastScheduleID"]))
//                b.BlastScheduleID = Convert.ToInt32(row["BlastScheduleID"].ToString());
//            if (!DBNull.Value.Equals(row["OverrideAmount"]))
//                b.OverrideAmount = Convert.ToInt32(row["OverrideAmount"].ToString());
//            if (!DBNull.Value.Equals(row["OverrideIsAmount"]))
//                b.OverrideIsAmount = Convert.ToBoolean(row["OverrideIsAmount"].ToString());
//            if (!DBNull.Value.Equals(row["StartTime"]))
//                b.StartTime = Convert.ToDateTime(row["StartTime"].ToString());
//            if (!DBNull.Value.Equals(row["SMSOptInTotal"]))
//                b.SMSOptInTotal = Convert.ToInt32(row["SMSOptInTotal"].ToString());
//            if (!DBNull.Value.Equals(row["SmartSegmentID"]))
//                b.SmartSegmentID = Convert.ToInt32(row["SmartSegmentID"].ToString());
//            b.CampaignItemID = Convert.ToInt32(row["CampaignItemID"].ToString());
//            b.NodeID = row["NodeID"].ToString();
//            if (!DBNull.Value.Equals(row["SampleID"]))
//                b.SampleID = Convert.ToInt32(row["SampleID"].ToString());
//            return b;
//        }

//        private static ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes GetBlastStatusCode(string code)
//        {
//            ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes returnType;
//            switch (code.Trim().ToLower())
//            {
//                case "active":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Active;
//                    break;
//                case "cancelled":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Cancelled;
//                    break;
//                case "deleted":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Deleted;
//                    break;
//                case "deployed":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Deployed;
//                    break;
//                case "pending":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Pending;
//                    break;
//                case "sent":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Sent;
//                    break;
//                case "system":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.System;
//                    break;
//                default:
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCodes.Unknown;
//                    break;
//            }
//            return returnType;
//        }

//        private static ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes GetBlastType(string type)
//        {
//            ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes returnType;
//            switch (type.Trim().ToLower())
//            {
//                case "html": //regular blast
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Regular;
//                    break;
//                case "text": //regular blast
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Regular;
//                    break;
//                case "sample":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.AB;
//                    break;
//                case "champion":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Champion;
//                    break;
//                case "sms":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.SMS;
//                    break;
//                case "social":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Social;
//                    break;
//                case "layout":
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Layout;
//                    break;
//                default:
//                    returnType = ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Unknown;
//                    break;
//            }
//            return returnType;
//        }

//        private static BlastAbstract GetInstance(ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes BlastType)
//        {
//            BlastAbstract b = null;

//            switch (BlastType)
//            {
//                case ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Regular: //regular blast
//                    b = new BlastRegular();
//                    break;
//                case ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.AB: //ab blast
//                    b = new BlastAB();
//                    break;
//                case ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Champion: //champion blast
//                    b = new BlastChampion();
//                    break;
//                case ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.SMS: //sms blast
//                    b = new BlastSMS();
//                    break;
//                case ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Social: //social blast
//                    b = new BlastSocial();
//                    break;
//                case ECN_Framework_Common.Objects.Communicator.Enums.BlastTypes.Layout: //layout blast
//                    b = new BlastLayout();
//                    break;
//            }
//            return b;
//        }

//        private static List<BlastAbstract> GetList(SqlCommand cmd, int customerID, bool includeAll)
//        {
//            List<BlastAbstract> retList = new List<BlastAbstract>();

//            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());

//            if (dt == null || dt.Rows.Count <= 0)
//            {
//                throw new ECN_Framework_Common.Objects.DataNotFoundException("DATA NOT FOUND!");
//            }
//            else
//            {
//                foreach (DataRow row in dt.Rows)
//                {
//                    BlastAbstract retItem = GetByBlastID(Convert.ToInt32(row["BlastID"]), customerID, includeAll);
//                    if (retItem != null && ECN_Framework_BusinessLayer.Communicator.BlastFields.Exists(retItem.BlastID, retItem.CustomerID.Value))
//                    {
//                        retItem.Fields = ECN_Framework_BusinessLayer.Communicator.BlastFields.GetByBlastID(retItem.BlastID, retItem.CustomerID.Value, false);
//                    }
//                    retList.Add(retItem);
//                }

//                if (retList.Count == 0)
//                {
//                    retList = null;
//                    throw new ECN_Framework_Common.Objects.DataNotFoundException("DATA NOT FOUND!");
//                }
//            }

//            return retList;
//        }
//        #endregion
//        #region Data
//        #region Select
//        public static BlastAbstract GetByBlastID(int blastID, int customerID, bool includeAll)
//        {
//            BlastAbstract retItem = null;

//            SqlCommand cmd = new SqlCommand("select * from blasts with (nolock) where blastID = @BlastID and CustomerID = @CustomerID");
//            cmd.CommandType = CommandType.Text;
//            cmd.Parameters.AddWithValue("@BlastID", blastID);
//            cmd.Parameters.AddWithValue("@CustomerID", customerID);

//            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
//            if (dt == null || dt.Rows.Count <= 0)
//            {
//                throw new ECN_Framework_Common.Objects.DataNotFoundException("DATA NOT FOUND!");
//            }
//            else
//            {
//                retItem = GetBlast(dt.Rows[0]);
//                if (retItem.CustomerID != customerID)
//                {
//                    retItem = null;
//                    throw new ECN_Framework_Common.Objects.SecurityException("SECURITY VIOLATION!");
//                }
//                else if (includeAll)
//                {
//                    if (retItem.GroupID != null)
//                        retItem.Group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(retItem.GroupID.Value, retItem.CustomerID.Value, includeAll);
//                    if (retItem.LayoutID != null)
//                        retItem.Layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(retItem.LayoutID.Value, retItem.CustomerID.Value, includeAll);
//                    if (retItem.UserID != null)
//                        retItem.BlastUser = KMPlatform.BusinessLogic.User.GetByUserID(retItem.UserID.Value, retItem.CustomerID.Value, includeAll);
//                    if (retItem.FilterID != null)
//                        retItem.Filter = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID(retItem.FilterID.Value, retItem.CustomerID.Value);
//                    if (retItem.SmartSegmentID != null)
//                        retItem.Segment = ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetSmartSegmentByID(retItem.SmartSegmentID.Value);
//                    if (retItem.BlastScheduleID != null)
//                        retItem.Schedule = ECN_Framework_BusinessLayer.Communicator.BlastSchedule.GetByBlastID(retItem.BlastID, true);
//                }
//                if (ECN_Framework_BusinessLayer.Communicator.BlastFields.Exists(blastID, customerID))
//                {
//                    retItem.Fields = ECN_Framework_BusinessLayer.Communicator.BlastFields.GetByBlastID(blastID, customerID, false);
//                }
//            }

//            return retItem;
//        }

//        public static List<BlastAbstract> GetByCampaignID(int campaignID, int customerID, bool includeAll)
//        {
//            SqlCommand cmd = new SqlCommand("select * from blasts with (nolock) where CampaignID = @CampaignID");
//            cmd.CommandType = CommandType.Text;
//            cmd.Parameters.AddWithValue("@CampaignID", campaignID);
//            cmd.Parameters.AddWithValue("@CustomerID", customerID);
//            return GetList(cmd, customerID, includeAll);
//        }

//        public static List<BlastAbstract> GetBySampleID(int sampleID, int customerID, bool includeAll)
//        {
//            SqlCommand cmd = new SqlCommand("select * from blasts with (nolock) where SampleID = @SampleID");
//            cmd.CommandType = CommandType.Text;
//            cmd.Parameters.AddWithValue("@SampleID", sampleID);
//            cmd.Parameters.AddWithValue("@CustomerID", customerID);
//            return GetList(cmd, customerID, includeAll);
//        }

//        public static List<BlastAbstract> GetByCustomerID(int customerID, bool includeAll)
//        {
//            SqlCommand cmd = new SqlCommand("select * from blasts with (nolock) where CustomerID = @CustomerID order by SendTime DESC");
//            cmd.CommandType = CommandType.Text;
//            cmd.Parameters.AddWithValue("@CustomerID", customerID);
//            return GetList(cmd, customerID, includeAll);
//        }

//        public static bool Exists(int blastID, int customerID)
//        {
//            SqlCommand cmd = new SqlCommand();
//            cmd.CommandType = CommandType.Text;
//            cmd.CommandText = "select count(BlastID) from Blasts with (nolock) where CustomerID = @CustomerID and BlastID = @BlastID";
//            cmd.Parameters.AddWithValue("@CustomerID", customerID);
//            cmd.Parameters.AddWithValue("@BlastID", blastID);
//            return Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
//        }

//        public static bool RefBlastsExists(string blastIDs, int customerID, DateTime sendTime)
//        {
//            char[] delimiter = { ',' };
//            string[] strBlasts = blastIDs.Split(delimiter);
//            int blastCount = 0;
//            for (int j = 0; j < strBlasts.Length; j++)
//            {
//                try
//                {
//                    blastCount++;
//                }
//                catch
//                {
//                }
//            }
//            if (blastCount > 0)
//            {
//                Int32 countActual = Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar("SELECT COUNT(BlastID) FROM Blasts with (nolock) WHERE BlastID in (" + blastIDs + ") AND CustomerID = " + customerID + " AND SendTime < '" + sendTime + "'", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()));
//                if (countActual == blastCount)
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public static int? RefBlastCampaign(string blastIDs, int customerID)
//        {
//            int? campaignID = null;

//            campaignID = Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar("SELECT TOP 1 CampaignID FROM Blasts with (nolock) WHERE BlastID in (" + blastIDs + ") AND CustomerID = " + customerID + " AND CampaignID IS NOT NULL", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()));

//            return campaignID;
//        }
//        #endregion
//        #region CRUD
//        public static bool SetHasEmailPreview(int blastID, bool hasEmailPreview)
//        {
//            SqlCommand cmd = new SqlCommand();
//            cmd.CommandType = CommandType.StoredProcedure;
//            cmd.CommandText = "e_Blast_Set_HasEmailPreview";
//            cmd.Parameters.AddWithValue("@BlastID", blastID);
//            cmd.Parameters.AddWithValue("@hasEmailPreview", hasEmailPreview);

//            return ECN_Framework_DataLayer.DataFunctions.ExecuteNonQuery(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
//        }

//        public static int Insert(BlastAbstract blast)
//        {
//            SqlCommand cmd = new SqlCommand("e_Blast_Insert");
//            cmd.CommandType = CommandType.StoredProcedure;
//            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)blast.CustomerID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@EmailSubject", blast.EmailSubject));
//            cmd.Parameters.Add(new SqlParameter("@EmailFrom", blast.EmailFrom));
//            cmd.Parameters.Add(new SqlParameter("@EmailFromName", blast.EmailFromName));
//            cmd.Parameters.Add(new SqlParameter("@SendTime", (object)blast.SendTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@AttemptTotal", (object)blast.AttemptTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SendTotal", (object)blast.SendTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SendBytes", (object)blast.SendBytes ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@StatusCode", blast.StatusCode));
//            cmd.Parameters.Add(new SqlParameter("@BlastType", blast.BlastType));
//            cmd.Parameters.Add(new SqlParameter("@CodeID", (object)blast.CodeID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)blast.LayoutID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)blast.GroupID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@FinishTime", (object)blast.FinishTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SuccessTotal", (object)blast.SuccessTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@BlastLog", blast.BlastLog));
//            cmd.Parameters.Add(new SqlParameter("@UserID", (object)blast.UserID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)blast.FilterID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@Spinlock", blast.Spinlock));
//            cmd.Parameters.Add(new SqlParameter("@ReplyTo", blast.ReplyTo));
//            cmd.Parameters.Add(new SqlParameter("@TestBlast", blast.TestBlast));
//            cmd.Parameters.Add(new SqlParameter("@BlastFrequency", blast.BlastFrequency));
//            cmd.Parameters.Add(new SqlParameter("@RefBlastID", blast.RefBlastID));
//            cmd.Parameters.Add(new SqlParameter("@BlastSuppression", blast.BlastSuppression));
//            cmd.Parameters.Add(new SqlParameter("@AddOptOuts_to_MS", (object)blast.AddOptOuts_to_MS ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@DynamicFromName", blast.DynamicFromName));
//            cmd.Parameters.Add(new SqlParameter("@DynamicFromEmail", blast.DynamicFromEmail));
//            cmd.Parameters.Add(new SqlParameter("@DynamicReplyToEmail", blast.DynamicReplyToEmail));
//            cmd.Parameters.Add(new SqlParameter("@BlastEngineID", (object)blast.BlastEngineID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@HasEmailPreview", (object)blast.HasEmailPreview ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@BlastScheduleID", (object)blast.BlastScheduleID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@OverrideAmount", (object)blast.OverrideAmount ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@OverrideIsAmount", (object)blast.OverrideIsAmount ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@StartTime", (object)blast.StartTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SMSOptInTotal", (object)blast.SMSOptInTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)blast.CampaignItemID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SmartSegmentID", (object)blast.SmartSegmentID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@NodeID", blast.NodeID));
//            cmd.Parameters.Add(new SqlParameter("@SampleID", (object)blast.SampleID ?? DBNull.Value));
//            return Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()));
//        }

//        public static int Insert(BlastAbstract blast, SqlCommand cmd)
//        {
//            cmd.CommandType = CommandType.StoredProcedure;
//            cmd.CommandText = "e_Blast_Insert";
//            cmd.Parameters.Clear();
//            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)blast.CustomerID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@EmailSubject", blast.EmailSubject));
//            cmd.Parameters.Add(new SqlParameter("@EmailFrom", blast.EmailFrom));
//            cmd.Parameters.Add(new SqlParameter("@EmailFromName", blast.EmailFromName));
//            cmd.Parameters.Add(new SqlParameter("@SendTime", (object)blast.SendTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@AttemptTotal", (object)blast.AttemptTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SendTotal", (object)blast.SendTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SendBytes", (object)blast.SendBytes ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@StatusCode", blast.StatusCode));
//            cmd.Parameters.Add(new SqlParameter("@BlastType", blast.BlastType));
//            cmd.Parameters.Add(new SqlParameter("@CodeID", (object)blast.CodeID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)blast.LayoutID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)blast.GroupID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@FinishTime", (object)blast.FinishTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SuccessTotal", (object)blast.SuccessTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@BlastLog", blast.BlastLog));
//            cmd.Parameters.Add(new SqlParameter("@UserID", (object)blast.UserID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)blast.FilterID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@Spinlock", blast.Spinlock));
//            cmd.Parameters.Add(new SqlParameter("@ReplyTo", blast.ReplyTo));
//            cmd.Parameters.Add(new SqlParameter("@TestBlast", blast.TestBlast));
//            cmd.Parameters.Add(new SqlParameter("@BlastFrequency", blast.BlastFrequency));
//            cmd.Parameters.Add(new SqlParameter("@RefBlastID", blast.RefBlastID));
//            cmd.Parameters.Add(new SqlParameter("@BlastSuppression", blast.BlastSuppression));
//            cmd.Parameters.Add(new SqlParameter("@AddOptOuts_to_MS", (object)blast.AddOptOuts_to_MS ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@DynamicFromName", blast.DynamicFromName));
//            cmd.Parameters.Add(new SqlParameter("@DynamicFromEmail", blast.DynamicFromEmail));
//            cmd.Parameters.Add(new SqlParameter("@DynamicReplyToEmail", blast.DynamicReplyToEmail));
//            cmd.Parameters.Add(new SqlParameter("@BlastEngineID", (object)blast.BlastEngineID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@HasEmailPreview", (object)blast.HasEmailPreview ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@BlastScheduleID", (object)blast.BlastScheduleID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@OverrideAmount", (object)blast.OverrideAmount ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@OverrideIsAmount", (object)blast.OverrideIsAmount ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@StartTime", (object)blast.StartTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SMSOptInTotal", (object)blast.SMSOptInTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)blast.CampaignItemID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SmartSegmentID", (object)blast.SmartSegmentID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@NodeID", blast.NodeID));
//            cmd.Parameters.Add(new SqlParameter("@SampleID", (object)blast.SampleID ?? DBNull.Value));
//            return Convert.ToInt32(cmd.ExecuteScalar().ToString());
//        }

//        public static void Update(BlastAbstract blast)
//        {
//            StringBuilder sqlBuilder = new StringBuilder();
//            sqlBuilder.Append("UPDATE Blasts SET ");
//            sqlBuilder.Append("CustomerID=@CustomerID,EmailSubject=@EmailSubject,EmailFrom=@EmailFrom,EmailFromName=@EmailFromName,SendTime=@SendTime,AttemptTotal=@AttemptTotal,SendTotal=@SendTotal,SendBytes=@SendBytes,");
//            sqlBuilder.Append("StatusCode=@StatusCode,BlastType=@BlastType,CodeID=@CodeID,LayoutID=@LayoutID,GroupID=@GroupID,FinishTime=@FinishTime,SuccessTotal=@SuccessTotal,BlastLog=@BlastLog,UserID=@UserID,");
//            sqlBuilder.Append("FilterID=@FilterID,Spinlock=@Spinlock,ReplyTo=@ReplyTo,TestBlast=@TestBlast,BlastFrequency=@BlastFrequency,RefBlastID=@RefBlastID,BlastSuppression=@BlastSuppression,AddOptOuts_to_MS=@AddOptOuts_to_MS,");
//            sqlBuilder.Append("DynamicFromName=@DynamicFromName,DynamicFromEmail=@DynamicFromEmail,DynamicReplyToEmail=@DynamicReplyToEmail,BlastEngineID=@BlastEngineID,HasEmailPreview=@HasEmailPreview,BlastScheduleID=@BlastScheduleID,");
//            sqlBuilder.Append("OverrideAmount=@OverrideAmount,OverrideIsAmount=@OverrideIsAmount,StartTime=@StartTime,SMSOptInTotal=@SMSOptInTotal,CampaignItemID=@CampaignItemID,SmartSegmentID=@SmartSegmentID,SampleID=@SampleID,NodeID=@NodeID ");
//            sqlBuilder.Append("WHERE BlastID = @BlastID");
//            SqlCommand cmd = new SqlCommand(sqlBuilder.ToString());
//            cmd.CommandType = CommandType.Text;
//            cmd.Parameters.Add(new SqlParameter("@BlastID", blast.BlastID));
//            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)blast.CustomerID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@EmailSubject", blast.EmailSubject));
//            cmd.Parameters.Add(new SqlParameter("@EmailFrom", blast.EmailFrom));
//            cmd.Parameters.Add(new SqlParameter("@EmailFromName", blast.EmailFromName));
//            cmd.Parameters.Add(new SqlParameter("@SendTime", (object)blast.SendTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@AttemptTotal", (object)blast.AttemptTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SendTotal", (object)blast.SendTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SendBytes", (object)blast.SendBytes ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@StatusCode", blast.StatusCode));
//            cmd.Parameters.Add(new SqlParameter("@BlastType", blast.BlastType));
//            cmd.Parameters.Add(new SqlParameter("@CodeID", (object)blast.CodeID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)blast.LayoutID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)blast.GroupID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@FinishTime", (object)blast.FinishTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SuccessTotal", (object)blast.SuccessTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@BlastLog", blast.BlastLog));
//            cmd.Parameters.Add(new SqlParameter("@UserID", (object)blast.UserID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)blast.FilterID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@Spinlock", blast.Spinlock));
//            cmd.Parameters.Add(new SqlParameter("@ReplyTo", blast.ReplyTo));
//            cmd.Parameters.Add(new SqlParameter("@TestBlast", blast.TestBlast));
//            cmd.Parameters.Add(new SqlParameter("@BlastFrequency", blast.BlastFrequency));
//            cmd.Parameters.Add(new SqlParameter("@RefBlastID", blast.RefBlastID));
//            cmd.Parameters.Add(new SqlParameter("@BlastSuppression", blast.BlastSuppression));
//            cmd.Parameters.Add(new SqlParameter("@AddOptOuts_to_MS", (object)blast.AddOptOuts_to_MS ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@DynamicFromName", blast.DynamicFromName));
//            cmd.Parameters.Add(new SqlParameter("@DynamicFromEmail", blast.DynamicFromEmail));
//            cmd.Parameters.Add(new SqlParameter("@DynamicReplyToEmail", blast.DynamicReplyToEmail));
//            cmd.Parameters.Add(new SqlParameter("@BlastEngineID", (object)blast.BlastEngineID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@HasEmailPreview", (object)blast.HasEmailPreview ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@BlastScheduleID", (object)blast.BlastScheduleID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@OverrideAmount", (object)blast.OverrideAmount ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@OverrideIsAmount", (object)blast.OverrideIsAmount ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@StartTime", (object)blast.StartTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SMSOptInTotal", (object)blast.SMSOptInTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)blast.CampaignItemID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SmartSegmentID", (object)blast.SmartSegmentID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@NodeID", blast.NodeID));
//            cmd.Parameters.Add(new SqlParameter("@SampleID", (object)blast.SampleID ?? DBNull.Value));
//            ECN_Framework_DataLayer.DataFunctions.ExecuteNonQuery(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
//        }

//        public static void Update(BlastAbstract blast, SqlCommand cmd)
//        {
//            StringBuilder sqlBuilder = new StringBuilder();
//            sqlBuilder.Append("UPDATE Blasts SET ");
//            sqlBuilder.Append("CustomerID=@CustomerID,EmailSubject=@EmailSubject,EmailFrom=@EmailFrom,EmailFromName=@EmailFromName,SendTime=@SendTime,AttemptTotal=@AttemptTotal,SendTotal=@SendTotal,SendBytes=@SendBytes,");
//            sqlBuilder.Append("StatusCode=@StatusCode,BlastType=@BlastType,CodeID=@CodeID,LayoutID=@LayoutID,GroupID=@GroupID,FinishTime=@FinishTime,SuccessTotal=@SuccessTotal,BlastLog=@BlastLog,UserID=@UserID,");
//            sqlBuilder.Append("FilterID=@FilterID,Spinlock=@Spinlock,ReplyTo=@ReplyTo,TestBlast=@TestBlast,BlastFrequency=@BlastFrequency,RefBlastID=@RefBlastID,BlastSuppression=@BlastSuppression,AddOptOuts_to_MS=@AddOptOuts_to_MS,");
//            sqlBuilder.Append("DynamicFromName=@DynamicFromName,DynamicFromEmail=@DynamicFromEmail,DynamicReplyToEmail=@DynamicReplyToEmail,BlastEngineID=@BlastEngineID,HasEmailPreview=@HasEmailPreview,BlastScheduleID=@BlastScheduleID,");
//            sqlBuilder.Append("OverrideAmount=@OverrideAmount,OverrideIsAmount=@OverrideIsAmount,StartTime=@StartTime,SMSOptInTotal=@SMSOptInTotal,CampaignItemID=@CampaignItemID,SmartSegmentID=@SmartSegmentID,SampleID=@SampleID,NodeID=@NodeID ");
//            sqlBuilder.Append("WHERE BlastID = @BlastID");
//            cmd.CommandType = CommandType.Text;
//            cmd.CommandText = sqlBuilder.ToString();
//            cmd.Parameters.Clear();
//            cmd.Parameters.Add(new SqlParameter("@BlastID", blast.BlastID));
//            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)blast.CustomerID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@EmailSubject", blast.EmailSubject));
//            cmd.Parameters.Add(new SqlParameter("@EmailFrom", blast.EmailFrom));
//            cmd.Parameters.Add(new SqlParameter("@EmailFromName", blast.EmailFromName));
//            cmd.Parameters.Add(new SqlParameter("@SendTime", (object)blast.SendTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@AttemptTotal", (object)blast.AttemptTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SendTotal", (object)blast.SendTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SendBytes", (object)blast.SendBytes ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@StatusCode", blast.StatusCode));
//            cmd.Parameters.Add(new SqlParameter("@BlastType", blast.BlastType));
//            cmd.Parameters.Add(new SqlParameter("@CodeID", (object)blast.CodeID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@LayoutID", (object)blast.LayoutID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@GroupID", (object)blast.GroupID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@FinishTime", (object)blast.FinishTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SuccessTotal", (object)blast.SuccessTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@BlastLog", blast.BlastLog));
//            cmd.Parameters.Add(new SqlParameter("@UserID", (object)blast.UserID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)blast.FilterID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@Spinlock", blast.Spinlock));
//            cmd.Parameters.Add(new SqlParameter("@ReplyTo", blast.ReplyTo));
//            cmd.Parameters.Add(new SqlParameter("@TestBlast", blast.TestBlast));
//            cmd.Parameters.Add(new SqlParameter("@BlastFrequency", blast.BlastFrequency));
//            cmd.Parameters.Add(new SqlParameter("@RefBlastID", blast.RefBlastID));
//            cmd.Parameters.Add(new SqlParameter("@BlastSuppression", blast.BlastSuppression));
//            cmd.Parameters.Add(new SqlParameter("@AddOptOuts_to_MS", (object)blast.AddOptOuts_to_MS ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@DynamicFromName", blast.DynamicFromName));
//            cmd.Parameters.Add(new SqlParameter("@DynamicFromEmail", blast.DynamicFromEmail));
//            cmd.Parameters.Add(new SqlParameter("@DynamicReplyToEmail", blast.DynamicReplyToEmail));
//            cmd.Parameters.Add(new SqlParameter("@BlastEngineID", (object)blast.BlastEngineID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@HasEmailPreview", (object)blast.HasEmailPreview ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@BlastScheduleID", (object)blast.BlastScheduleID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@OverrideAmount", (object)blast.OverrideAmount ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@OverrideIsAmount", (object)blast.OverrideIsAmount ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@StartTime", (object)blast.StartTime ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SMSOptInTotal", (object)blast.SMSOptInTotal ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@CampaignItemID", (object)blast.CampaignItemID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@SmartSegmentID", (object)blast.SmartSegmentID ?? DBNull.Value));
//            cmd.Parameters.Add(new SqlParameter("@NodeID", blast.NodeID));
//            cmd.Parameters.Add(new SqlParameter("@SampleID", (object)blast.SampleID ?? DBNull.Value));
//            cmd.ExecuteNonQuery();
//        }

//        public static void UpdateRefBlastsCampaigns(string blastIDs, int campaignID)
//        {
//            ECN_Framework_DataLayer.DataFunctions.ExecuteNonQuery("UPDATE Blasts SET CampaignID = " + campaignID + " WHERE BlastID in (" + blastIDs + ") AND CampaignID IS NULL", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
//        }

//        public void Inc()
//        {
//            //int success = SuccessTotal() + 1;
//            //int send = SendTotal() + 1;
//            //int attempt = AttemptTotal() + 1;

//            //ECN_Framework_DataLayer.DataFunctions.Execute("Update Blasts set SuccessTotal = " + success + " , SendTotal = " + send + " , AttemptTotal = " + attempt + " WHERE BlastID = " + BlastID, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString());
//        }

//        //public int SuccessTotal()
//        //{
//        //    int SuccessTotal = 0;
//        //    if (BlastID > 0)
//        //    {
//        //        try
//        //        {
//        //            SuccessTotal = Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar("Select SuccessTotal from Blasts where BlastID=" + BlastID, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()));
//        //        }
//        //        catch
//        //        {

//        //        }
//        //    }
//        //    return SuccessTotal;
//        //}
//        //public int AttemptTotal()
//        //{
//        //    int AttemptTotal = 0;
//        //    if (BlastID > 0)
//        //    {
//        //        try
//        //        {
//        //            AttemptTotal = Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar("Select AttemptTotal from Blasts where BlastID=" + BlastID, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()));
//        //        }
//        //        catch
//        //        {

//        //        }
//        //    }
//        //    return AttemptTotal;
//        //}
//        //public int SendTotal()
//        //{
//        //    int SendTotal = 0;
//        //    if (BlastID > 0)
//        //    {
//        //        try
//        //        {
//        //            SendTotal = Convert.ToInt32(ECN_Framework_DataLayer.DataFunctions.ExecuteScalar("Select SendTotal from Blasts where BlastID=" + BlastID, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()));
//        //        }
//        //        catch
//        //        {

//        //        }
//        //    }
//        //    return SendTotal;
//        //}
//        #endregion
//        #endregion
//    }
//}
