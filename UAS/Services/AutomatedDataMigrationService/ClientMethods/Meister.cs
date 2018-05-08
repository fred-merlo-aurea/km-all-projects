using System.Text;
using System.Data;
using System.Data.SqlClient;
using Core.ADMS.Events;
using Core_AMS.Utilities;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using System.Linq;
using FrameworkUAS.DataAccess;
using KM.Common.Data;
using KM.Common.Import;
using KMPlatform.Entity;
using AdHocDimension = FrameworkUAS.BusinessLogic.AdHocDimension;
using AdHocDimensionGroupPubcodeMap = FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap;
using ClientCustomProcedure = FrameworkUAS.Entity.ClientCustomProcedure;
using DataFunctions = KM.Common.DataFunctions;
using SourceFile = FrameworkUAS.Entity.SourceFile;

namespace ADMS.ClientMethods
{
    class Meister : ClientSpecialCommon
    {
        private const string CventOptOutsDimensionGroup = "Meister_CventOptOuts";
        private const string EmailFieldName = "Email Address";
        private const string FipsCounyDimensionGroup = "Meister_FipsCounty";
        private const string Demo15StandardField = "DEMO15";
        private const string FipsCountyDim = "FIPS_COUNTY";
        private const string CountyValue = "County";
        private const string FipsMatchValue = "FIPS";
        private const string RemoveBadPhoneNumberDimensionGroup = "Meister_RemoveBadPhoneNumber";
        private const string HomePhoneBadFieldName = "HomePhone_Bad";

        public void ECN_DemoOptOuts(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.ClientCustomProcedure p, string processCode)
        {
            //ECN Demo Opt-Outs
            //Set demo35 = N if email matches email from ECN groupID 181967 (all group members from 181967)
            //181967 is a master suppression group so SubscriberTypeCode will be:  ? (unknown), M (master suppressed), B (bounced too many times).....

            //Set demo34 = N if email matches email from ECN groupIDs 181965, 183933, 181966 (only consider group members where subscribetypecode = ‘U’)
            //SubscriberTypeCode possible values:  S (subscribed),U (unsubscribed),M (master suppressed)

            //sp_getEmailsListFromGroup

            #region demo35
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getEmailsListFromGroup";
            cmd.Parameters.AddWithValue("@groupID", 181967);

            dt = FrameworkUAS.DataAccess.DataFunctions.GetDataTableViaAdapter(cmd, ConnectionString.ECN_Communicator.ToString());
            //EmailID (int), EmailAddress (string), Firstname (string), LastName (string), Voice (string), GroupID (int), FormatTypeCode (string), SubscribeTypeCode (string), CreatedOn (datetime), LastChaged (datetime), Dates(string -this is html)

            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder xml = new StringBuilder();
                xml.AppendLine("<XML>");
                foreach (DataRow dr in dt.Rows)
                {
                    xml.AppendLine("<Emails>");
                    try
                    {
                        string email = dr["EmailAddress"].ToString().Trim();
                        if (email.Length > 100)
                            email = email.Substring(0, 100);
                        xml.AppendLine("<Email>");
                        xml.AppendLine(Core_AMS.Utilities.XmlFunctions.CleanAllXml(email));
                        xml.AppendLine("</Email>");
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, this.GetType().Name.ToString() + ".ECN_DemoOptOuts");
                    }
                    xml.AppendLine("</Emails>");
                }
                xml.AppendLine("</XML>");

                //now call a custom sproc ccp_Meister_Demo35OptOuts
                try
                {
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ccp_Meister_Demo35OptOuts";
                    cmd.Parameters.AddWithValue("@xml", xml.ToString());
                    cmd.Parameters.AddWithValue("@ProcessCode", processCode);
                    cmd.Connection = FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
                    DataFunctions.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ECN_DemoOptOuts - ccp_Meister_Demo35OptOuts");
                }
            }

            #region 193397 
            dt = null;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getEmailsListFromGroup";
            cmd.Parameters.AddWithValue("@groupID", 193397);

            dt = FrameworkUAS.DataAccess.DataFunctions.GetDataTableViaAdapter(cmd, ConnectionString.ECN_Communicator.ToString());
            //EmailID (int), EmailAddress (string), Firstname (string), LastName (string), Voice (string), GroupID (int), FormatTypeCode (string), SubscribeTypeCode (string), CreatedOn (datetime), LastChaged (datetime), Dates(string -this is html)

            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder xml = new StringBuilder();
                xml.AppendLine("<XML>");
                foreach (DataRow dr in dt.Rows)
                {
                    xml.AppendLine("<Emails>");
                    try
                    {
                        string email = dr["EmailAddress"].ToString().Trim();
                        if (email.Length > 100)
                            email = email.Substring(0, 100);
                        xml.AppendLine("<Email>");
                        xml.AppendLine(Core_AMS.Utilities.XmlFunctions.CleanAllXml(email));
                        xml.AppendLine("</Email>");
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, this.GetType().Name.ToString() + ".ECN_DemoOptOuts");
                    }
                    xml.AppendLine("</Emails>");
                }
                xml.AppendLine("</XML>");

                //now call a custom sproc ccp_Meister_Demo35OptOuts
                try
                {
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ccp_Meister_Demo35OptOuts";
                    cmd.Parameters.AddWithValue("@xml", xml.ToString());
                    cmd.Parameters.AddWithValue("@ProcessCode", processCode);
                    cmd.Connection = FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
                    DataFunctions.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ECN_DemoOptOuts - ccp_Meister_Demo35OptOuts");
                }
            }
            #endregion

            #endregion
            #region demo34
            #region 181965
            dt = null;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getEmailsListFromGroup";
            cmd.Parameters.AddWithValue("@groupID", 181965);

            dt = FrameworkUAS.DataAccess.DataFunctions.GetDataTableViaAdapter(cmd, ConnectionString.ECN_Communicator.ToString());
            //EmailID (int), EmailAddress (string), Firstname (string), LastName (string), Voice (string), GroupID (int), FormatTypeCode (string), SubscribeTypeCode (string), CreatedOn (datetime), LastChaged (datetime), Dates(string -this is html)

            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder xml = new StringBuilder();
                xml.AppendLine("<XML>");
                foreach (DataRow dr in dt.Rows)
                {
                    xml.AppendLine("<Emails>");
                    try
                    {
                        if (dr["SubscribeTypeCode"].ToString().Equals("U", System.StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            string email = dr["EmailAddress"].ToString().Trim();
                            if (email.Length > 100)
                                email = email.Substring(0, 100);
                            xml.AppendLine("<Email>");
                            xml.AppendLine(Core_AMS.Utilities.XmlFunctions.CleanAllXml(email));
                            xml.AppendLine("</Email>");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, this.GetType().Name.ToString() + ".ECN_DemoOptOuts");
                    }
                    xml.AppendLine("</Emails>");
                }
                xml.AppendLine("</XML>");

                //now call a custom sproc ccp_Meister_Demo35OptOuts
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ccp_Meister_Demo34OptOuts";
                cmd.Parameters.AddWithValue("@xml", xml.ToString());
                cmd.Parameters.AddWithValue("@ProcessCode", processCode);
                cmd.Connection = FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
                DataFunctions.ExecuteNonQuery(cmd);
            }
            #endregion
            #region 183933
            dt = null;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getEmailsListFromGroup";
            cmd.Parameters.AddWithValue("@groupID", 183933);

            dt = FrameworkUAS.DataAccess.DataFunctions.GetDataTableViaAdapter(cmd, ConnectionString.ECN_Communicator.ToString());
            //EmailID (int), EmailAddress (string), Firstname (string), LastName (string), Voice (string), GroupID (int), FormatTypeCode (string), SubscribeTypeCode (string), CreatedOn (datetime), LastChaged (datetime), Dates(string -this is html)

            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder xml = new StringBuilder();
                xml.AppendLine("<XML>");
                foreach (DataRow dr in dt.Rows)
                {
                    xml.AppendLine("<Emails>");
                    try
                    {
                        if (dr["SubscribeTypeCode"].ToString().Equals("U", System.StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            string email = dr["EmailAddress"].ToString().Trim();
                            if (email.Length > 100)
                                email = email.Substring(0, 100);
                            xml.AppendLine("<Email>");
                            xml.AppendLine(Core_AMS.Utilities.XmlFunctions.CleanAllXml(email));
                            xml.AppendLine("</Email>");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, this.GetType().Name.ToString() + ".ECN_DemoOptOuts");
                    }
                    xml.AppendLine("</Emails>");
                }
                xml.AppendLine("</XML>");

                //now call a custom sproc ccp_Meister_Demo35OptOuts
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ccp_Meister_Demo34OptOuts";
                cmd.Parameters.AddWithValue("@xml", xml.ToString());
                cmd.Parameters.AddWithValue("@ProcessCode", processCode);
                cmd.Connection = FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
                DataFunctions.ExecuteNonQuery(cmd);
            }
            #endregion
            #region 181966
            dt = null;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getEmailsListFromGroup";
            cmd.Parameters.AddWithValue("@groupID", 181966);

            dt = FrameworkUAS.DataAccess.DataFunctions.GetDataTableViaAdapter(cmd, ConnectionString.ECN_Communicator.ToString());
            //EmailID (int), EmailAddress (string), Firstname (string), LastName (string), Voice (string), GroupID (int), FormatTypeCode (string), SubscribeTypeCode (string), CreatedOn (datetime), LastChaged (datetime), Dates(string -this is html)

            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder xml = new StringBuilder();
                xml.AppendLine("<XML>");
                foreach (DataRow dr in dt.Rows)
                {
                    xml.AppendLine("<Emails>");
                    try
                    {
                        if (dr["SubscribeTypeCode"].ToString().Equals("U", System.StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            string email = dr["EmailAddress"].ToString().Trim();
                            if (email.Length > 100)
                                email = email.Substring(0, 100);
                            xml.AppendLine("<Email>");
                            xml.AppendLine(Core_AMS.Utilities.XmlFunctions.CleanAllXml(email));
                            xml.AppendLine("</Email>");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, this.GetType().Name.ToString() + ".ECN_DemoOptOuts - ccp_Meister_Demo34OptOuts");
                    }
                    xml.AppendLine("</Emails>");
                }
                xml.AppendLine("</XML>");

                //now call a custom sproc ccp_Meister_Demo35OptOuts
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ccp_Meister_Demo34OptOuts";
                cmd.Parameters.AddWithValue("@xml", xml.ToString());
                cmd.Parameters.AddWithValue("@ProcessCode", processCode);
                cmd.Connection = FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
                DataFunctions.ExecuteNonQuery(cmd);
            }
            #endregion
            #endregion
        }
        public void Excel_DemoOptOuts(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.ClientCustomProcedure p, string processCode)
        {
            //Excel Demo Opt-Outs
            //Set demo34 = N if email matches Email Address from 5-30-14 Opt outs from Cvent to Audience Development.xlsx (attached)
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ccp_Meister_ExcelDemoOptOuts";
                cmd.Parameters.AddWithValue("@ProcessCode", processCode);
                cmd.Connection = FrameworkUAD.DataAccess.DataFunctions.GetClientSqlConnection(client);
                DataFunctions.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".Excel_DemoOptOuts - ccp_Meister_ExcelDemoOptOuts");
            }
        }
        public void CventOptOuts(
            Client client,
            SourceFile cSpecialFile,
            ClientCustomProcedure ccp,
            FileMoved eventMessage)
        {
            var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
            {
                AhdData = new AdHocDimension(),
                EventMessage = eventMessage,
                Client = client,
                AdHocDimensionGroupName = CventOptOutsDimensionGroup,
                StandardField = EmailStandardField,
                CreatedDimension = string.Empty,
                DimensionValue = string.Empty,
                MatchValueField = EmailFieldName,
                DimensionOperator = EqualOperation,
                UpdateUAD = false
            };
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(fillAgGroupAndTableArgs);
        }
        public void FipsCounty(
            Client client,
            SourceFile cSpecialFile,
            ClientCustomProcedure ccp,
            FileMoved eventMessage)
        {
            var ahdData = new AdHocDimension();
            ahdData.Delete(eventMessage.SourceFile.SourceFileID);

            var fileWorker = new FileWorker();
            var fileConfiguration = new FileConfiguration {FileExtension = ".xlsx"};
            var dataTable = fileWorker.GetData(eventMessage.ImportFile, fileConfiguration);
            var list = new List<FrameworkUAS.Entity.AdHocDimension>();

            var agWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();

            var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
            {
                AdHocDimensionGroupName = FipsCounyDimensionGroup,
                Client = client,
                StandardField = Demo15StandardField,
                CreatedDimension = FipsCountyDim,
                IsPubcodeSpecific = true,
                EventMessage = eventMessage,
                DimensionValueField = CountyValue,
                MatchValueField = FipsMatchValue, 
                DimensionOperator = EqualOperation,
                UpdateUAD = false,
                AhdData = ahdData,
                AdditionalInitFunction = adg =>
                    {
                        var pubs = new List<string>
                            { "AWFG","AVG","FLG","PDH","TGC","GHG","FCI","CRL","CGR","PAG"};

                        foreach (var pubCode in pubs)
                        {
                            var mapGroup = new AdHocDimensionGroupPubcodeMap
                            {
                                AdHocDimensionGroupId = adg.AdHocDimensionGroupId,
                                CreatedByUserID = 1,
                                DateCreated = DateTime.Now,
                                IsActive = true,
                                Pubcode = pubCode,
                                UpdatedByUserID = 1
                            };

                            var mWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroupPubcodeMap();
                            mWorker.Save(mapGroup);
                        }
                    }
            };
            var adHocDimensionGroup = ClientMethodHelpers.CreateDimensionGroup(fillAgGroupAndTableArgs, agWorker);

            foreach (DataRow dr in dataTable.Rows)
            {
                ClientMethodHelpers.CreateDimensionValue(fillAgGroupAndTableArgs, adHocDimensionGroup, list, dr);
            }

            ahdData.SaveBulkSqlInsert(list);
        }
        public void RemoveBadPhoneNumber(Client client, SourceFile cSpecialFile, ClientCustomProcedure ccp, FileMoved eventMessage)
        {
            var fillAgGroupAndTableArgs = new FillAgGroupAndTableArgs()
            {
                AhdData = new AdHocDimension(),
                EventMessage = eventMessage,
                Client = client,
                AdHocDimensionGroupName = RemoveBadPhoneNumberDimensionGroup,
                StandardField = PhoneStandardField,
                CreatedDimension = string.Empty,
                DimensionValue = string.Empty,
                MatchValueField = HomePhoneBadFieldName,
                DimensionOperator = EqualOperation,
                UpdateUAD = false
            };
            ClientMethodHelpers.ReadAndFillAgGroupAndTable(fillAgGroupAndTableArgs);
        }
        public FrameworkUAD.Object.ImportFile CommaSeperateValues(FileMoved eventMessage, FrameworkUAD.Object.ImportFile data)
        {
            //first step is to get a distinct list of PubCodes
            List<string> pubCodes = new List<string>();
            pubCodes.Add("CGR");
            pubCodes.Add("TGCNL");
            FrameworkUAS.Entity.FieldMapping fmPubCode = eventMessage.SourceFile.FieldMappings.SingleOrDefault(x => x.MAFField.Equals("PubCode", StringComparison.CurrentCultureIgnoreCase));

            if (fmPubCode != null)
            {
                foreach (var key in data.DataTransformed.Keys)
                {
                    try
                    {
                        StringDictionary myRow = data.DataTransformed[key];
                        if (myRow[fmPubCode.IncomingField] != null)
                        {
                            var pubCode = myRow[fmPubCode.IncomingField];
                            if (pubCodes.Contains(pubCode))
                            {
                                if (myRow["DEMO9"] != null)
                                {
                                    string value = myRow["DEMO9"].ToString().Replace(",", "").Replace("\"", "");
                                    char[] allValues = value.ToCharArray();
                                    string newValue = string.Join(",", allValues);
                                    myRow["DEMO9"] = newValue;
                                }
                                if (myRow["DEMO10"] != null)
                                {
                                    string value = myRow["DEMO10"].ToString().Replace(",", "").Replace("\"","");
                                    char[] allValues = value.ToCharArray();
                                    string newValue = string.Join(",", allValues);
                                    myRow["DEMO10"] = newValue;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, client, this.GetType().Name.ToString() + ".CommaSeperateValues");
                    }
                }
            }

            return data;
        }
    }
}
