using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml.Linq;
using FrameworkUAS.DataAccess.Helpers;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class ClientMethods
    {
        private const string ProcedureBriefMediaUpdateTaxBehavior = "ccp_BriefMedia_BMWU_Update_TaxBehavior";
        private const string ProcedureBriefMediaUpdatePageBehavior = "ccp_BriefMedia_BMWU_Update_PageBehavior";
        private const string ProcedureBriefMediaUpdateTopicCode = "ccp_BriefMedia_BMWU_Update_TopicCode";
        private const string ProcedureBriefMediaUpdateUsers = "ccp_BriefMedia_BMWU_Update_Users";
        private const string ProcedureBriefMediaInsertAccess = "ccp_BriefMedia_BMWU_Insert_Access";
        private const string ProcedureWattProcessMacMic = "ccp_WATT_Process_MacMic";
        private const string ProcedureWattProcessEcnGroupIdPubCode = "ccp_WATT_Process_ECN_GROUPID_PUBCODE";
        private const string ProcedureVcastProcessFileMxBooks = "ccp_Vcast_Process_File_MX_Books";
        private const string ProcedureVcastProcessFileMxElan = "ccp_Vcast_Process_File_MX_Elan";
        private const string ProcedureNorthstarRelationalAddGroup = "ccp_Northstar_Relational_AddGroup";
        private const string ProcedureGlmRelationalInsertData = "ccp_GLM_Relational_InsertData";
        private const string TagXml = "XML";
        private const string TagTax = "Tax";
        private const string TagAccessId = "AccessID";
        private const string TagTopicCode = "TopicCode";
        private const string TagTaxonomyId = "TaxonomyID";
        private const string TagPage = "Page";
        private const string TagPageId = "PageID";
        private const string TagEmailAddress = "EmailAddress";
        private const string TagTopic = "Topic";
        private const string TagTopicId = "TopicID";
        private const string TagSearchTerms = "SearchTerms";
        private const string TagPageUnderscoreId = "Page_ID";
        private const string TagWatt = "WATT";
        private const string TagPubCode = "Pubcode";
        private const string TagFoxColumnName = "FoxColumnName";
        private const string TagCodeSheetValue = "CodeSheetValue";
        private const string TagSubscribeNumber = "SUBSCRBNUM";
        private const string TagSub02Ans = "SUB02ANS";
        private const string TagSub03Ans = "SUB03ANS";
        private const string TagNorthStar = "Northstar";
        private const string TagPublishCode = "PubCode";
        private const string TagGlobalUserKey = "GlobalUserKey";
        private const string TagGroupId = "GroupId";
        private const string TagIsRecent = "IsRecent";
        private const string TagAddDate = "AddDate";
        private const string TagDropDate = "DropDate";
        private const string TagUser = "User";
        private const string TagDrupalId = "DrupalID";
        private const string TagFirstName = "FirstName";
        private const string TagLastName = "LastName";
        private const string TagEmail = "Email";
        private const string TagGlm = "GLM";
        private const string TagGlmEmail = "EMAIL";
        private const string TagLeadsSent = "LEADSSENT";
        private const string TagLikes = "LIKES";
        private const string TagBoardFollows = "BOARDFOLLOWS";
        private const string TagExhibitorFollows = "EXHIBITORFOLLOWS";
        private const string TagWattGroupId = "GROUPID";
        private const string TagWattPubCode = "PUBCODE";
        private const string TextInvalidEmail = "INVALID EMAIL";
        private const string ColumnDrupalUserId = "Drupal_User_id";
        private const string ColumnUserId = "UserID";
        private const string ColumnGlmEmail = "E-mail";
        private const string ColumnLeadsSent = "Leads Sent";
        private const string ColumnLikes = "Likes";
        private const string ColumnBoardFollows = "Board Follows";
        private const string ColumnExhibitorFollows = "Exhibitor Follows";
        private const string ColumnFirstName = "First_Name";
        private const string ColumnLastName = "Last_Name";
        private const string ColumnProductCode = "ProductCode";
        private const string ColumnFoxColumnName = "FOXColumnName";
        private const string ColumnTopicCode = "TopicCode";
        private const string ColumnTopicCodeText = "TopicCodeText";
        private const string ColumnTicketType = "TICKETTYPE";
        private const string ColumnTicketTypeDestination = "Ticket_Type";
        private const string ColumnTicketSubt = "TICKETSUBT";
        private const string ColumnTicketSubtDestination = "Ticket_Stub";
        private const string ColumnRegCode = "REGCODE";
        private const string ColumnPersonIdSource = "PERSON_ID";
        private const string ColumnPersonIdDestination = "Person_ID";
        private const string ColumnSourceCodeSource = "SOURCECODE";
        private const string ColumnSourceCodeDestination = "SourceCode";
        private const string ColumnPriCodeSource = "PRICODE";
        private const string ColumnPriCodeDestination = "PriCode";
        private const string ColumnSequenceSource = "SEQUENCE";
        private const string ColumnSequenceDestination = "Sequence";
        private const string ColumnIGroupNoSource = "IGRP_NO";
        private const string ColumnIGroupNoDestination = "IGroupNo";
        private const string ColumnSubscriberId = "SubscriberId";
        private const string ColumnOrderId = "OrderId";
        private const string ColumnECommProductId = "ECommProductId";
        private const string ColumnECommPubCode = "ECommPubCode";
        private const string ColumnDescription = "Description";
        private const string ColumnListPrice = "ListPrice";
        private const string ColumnSubscriptionId = "SubscriptionId";
        private const string ColumnPublicationId = "publicationId";
        private const string ColumnPublicationPubCode = "PublicationPubCode";
        private const string TableTempAdvanstarRegCodeCompare = "tempAdvanstarRegCodeCompare";
        private const string TableTempAdvanstarRegCode = "tempAdvanstarRegCode";
        private const string TableTempAdvanstarSourceCode = "tempAdvanstarSourceCode";
        private const string TableTempAdvanstarPriCode = "tempAdvanstarPriCode";
        private const string TableTempAdvanstarRefreshDupes = "tempAdvanstarRefreshDupes";
        private const string TableTempHaymarketCmsEcomm = "tempHaymarketCMS_Ecomm";
        private const string TableTempHaymarketCmsPublication = "tempHaymarketCMS_Publication";
        private const int BatchSizeHaymarket = 10000;

        #region Haymarket
        public static DataTable Haymarket_CMS_Select_Publication()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CMS_Select_Publication";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }
        public static DataTable Haymarket_CMS_Select_Publication_Paging(int page, int records)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CMS_Select_Publication_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", records);

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        public static DataTable Haymarket_CMS_Select_Activity()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CMS_Select_Activity";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }
        public static DataTable Haymarket_CMS_Select_Activity_Paging(int page, int records)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CMS_Select_Activity_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", records);

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }
        public static DataTable Haymarket_CMS_Select_Ecomm()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CMS_Select_Ecomm";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }
        public static DataTable Haymarket_CMS_Select_Ecomm_Paging(int page, int records)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CMS_Select_Ecomm_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", records);

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        public static int Haymarket_CMS_Get_Publication_Count()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Haymarket_CMS_Get_Publication_Count";

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static int Haymarket_CMS_Get_Activity_Count()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Haymarket_CMS_Get_Activity_Count";

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static int Haymarket_CMS_Get_Ecomm_Count()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Haymarket_CMS_Get_Ecomm_Count";

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static bool Haymarket_CreateTempCMSTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CreateCMSTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool Haymarket_DropTempCMSTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_DropCMSTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool Haymarket_CMS_SetPubCodes()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CMS_SetPubCodes";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool Haymarket_CMS_Insert_Subscriber(DataTable dtBulk)
        {
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.UAS.ToString());
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "tempHaymarketCMS_Subscriber");
                bc.BatchSize = 10000;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("SubscriberId", "SubscriberId");
                bc.ColumnMappings.Add("FirstName", "FirstName");
                bc.ColumnMappings.Add("LastName", "LastName");
                bc.ColumnMappings.Add("PrimaryEmailAddress", "PrimaryEmailAddress");
                bc.ColumnMappings.Add("Company", "Company");
                bc.ColumnMappings.Add("JobTitle", "JobTitle");
                bc.ColumnMappings.Add("IndustrySector", "IndustrySector");
                bc.ColumnMappings.Add("OriginalPublicationId", "OriginalPublicationId");
                bc.ColumnMappings.Add("CreateDate", "CreateDate");
                bc.ColumnMappings.Add("YearOfBirth", "YearOfBirth");
                bc.ColumnMappings.Add("DayOfBirth", "DayOfBirth");
                bc.ColumnMappings.Add("MonthOfBirth", "MonthOfBirth");

                bc.WriteToServer(dtBulk);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
        public static bool Haymarket_CMS_Update_Address(DataTable dtUpdate)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<XML>");
            foreach (DataRow dr in dtUpdate.Rows)
            {
                if (!dr["SubscriberId"].ToString().Equals("NULL") && dr["SubscriberId"].ToString().Length > 0)
                {
                    xml.AppendLine("<Address>");

                    xml.AppendLine("<SubscriberId>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["SubscriberId"].ToString())) + "</SubscriberId>");
                    xml.AppendLine("<Address1>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Address1"].ToString())) + "</Address1>");
                    xml.AppendLine("<Address2>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Address2"].ToString())) + "</Address2>");
                    xml.AppendLine("<CityCode>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["CityCode"].ToString())) + "</CityCode>");
                    xml.AppendLine("<StateCode>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["StateCode"].ToString())) + "</StateCode>");
                    xml.AppendLine("<CountryCode>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["CountryCode"].ToString())) + "</CountryCode>");
                    xml.AppendLine("<PostalCode>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["PostalCode"].ToString())) + "</PostalCode>");
                    xml.AppendLine("<TelephoneNumber>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["TelephoneNumber"].ToString())) + "</TelephoneNumber>");
                    xml.AppendLine("<MobileNumber>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["MobileNumber"].ToString())) + "</MobileNumber>");
                    xml.AppendLine("<FaxNumber>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["FaxNumber"].ToString())) + "</FaxNumber>");

                    xml.AppendLine("</Address>");
                }
            }
            xml.AppendLine("</XML>");
            StringBuilder cleanXML = new StringBuilder();
            string clean = KM.Common.DataFunctions.CleanSerializedXML(xml.ToString());
            cleanXML.Append(clean);

            bool done = false;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CMSTable_UpdateAddress";
            cmd.Parameters.AddWithValue("@Xml", cleanXML.ToString());
            done = KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());


            return done;
        }

        public static bool Haymarket_CMS_Insert_PublicationPubCode(DataTable dtBulk)
        {
            if (dtBulk == null)
            {
                throw new ArgumentNullException(nameof(dtBulk));
            }

            var mappings = new Dictionary<string, string>
            {
                [ColumnSubscriberId] = ColumnSubscriberId,
                [ColumnSubscriptionId] = ColumnSubscriptionId,
                [ColumnPublicationId] = ColumnPublicationId,
                [ColumnPublicationPubCode] = ColumnPublicationPubCode
            };

            ClientMethodsHelper.BulkCopy(dtBulk, TableTempHaymarketCmsPublication, mappings, BatchSizeHaymarket);
            return true;
        }

        public static bool Haymarket_CMS_Insert_Activity(DataTable dtBulk)
        {
            #region xml
            //StringBuilder xml = new StringBuilder();
            //xml.AppendLine("<XML>");
            //foreach (DataRow dr in dtUpdate.Rows)
            //{
            //    if (!dr["SubscriberId"].ToString().Equals("NULL") && dr["SubscriberId"].ToString().Length > 0)
            //    {
            //        xml.AppendLine("<Activity>");

            //        xml.AppendLine("<SubscriberId>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["SubscriberId"].ToString()) + "</SubscriberId>");
            //        xml.AppendLine("<ActivityId>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["ActivityId"].ToString()) + "</ActivityId>");
            //        xml.AppendLine("<ShortTitle>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["ShortTitle"].ToString()) + "</ShortTitle>");
            //        xml.AppendLine("<PublishDate>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["PublishDate"].ToString()) + "</PublishDate>");
            //        xml.AppendLine("<ExpirationDate>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["ExpirationDate"].ToString()) + "</ExpirationDate>");
            //        xml.AppendLine("<PrimaryCountries>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["PrimaryCountries"].ToString()) + "</PrimaryCountries>");
            //        xml.AppendLine("<SecondaryCountries>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["SecondaryCountries"].ToString()) + "</SecondaryCountries>");
            //        xml.AppendLine("<AccreditationInfo>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["AccreditationInfo"].ToString()) + "</AccreditationInfo>");
            //        xml.AppendLine("<ActivitFormat>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["ActivitFormat"].ToString()) + "</ActivitFormat>");
            //        xml.AppendLine("<CEType>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["CEType"].ToString()) + "</CEType>");
            //        xml.AppendLine("<ContentType>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["ContentType"].ToString()) + "</ContentType>");
            //        xml.AppendLine("<EcommStatus>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["EcommStatus"].ToString()) + "</EcommStatus>");
            //        xml.AppendLine("<Partner>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Partner"].ToString()) + "</Partner>");
            //        xml.AppendLine("<ProfessionIds>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["ProfessionIds"].ToString()) + "</ProfessionIds>");
            //        xml.AppendLine("<SpecialtyIds>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["SpecialtyIds"].ToString()) + "</SpecialtyIds>");
            //        xml.AppendLine("<TopicIds>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["TopicIds"].ToString()) + "</TopicIds>");
            //        xml.AppendLine("<Professions>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Professions"].ToString()) + "</Professions>");
            //        xml.AppendLine("<percentage>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["percentage"].ToString()) + "</percentage>");
            //        xml.AppendLine("<AccreditationName>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["AccreditationName"].ToString()) + "</AccreditationName>");
            //        xml.AppendLine("<Credits>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Credits"].ToString()) + "</Credits>");
            //        xml.AppendLine("<Product>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Product"].ToString()) + "</Product>");
            //        xml.AppendLine("<Platform>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Platform"].ToString()) + "</Platform>");
            //        xml.AppendLine("<Model>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Model"].ToString()) + "</Model>");
            //        xml.AppendLine("<IsTablet>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["IsTablet"].ToString()) + "</IsTablet>");
            //        xml.AppendLine("<OsVersion>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["OsVersion"].ToString()) + "</OsVersion>");
            //        xml.AppendLine("<ActivityPubCode>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["ActivityPubCode"].ToString()) + "</ActivityPubCode>");

            //        xml.AppendLine("</Activity>");
            //    }
            //}
            //xml.AppendLine("</XML>");

            //StringBuilder cleanXML = new StringBuilder();
            //string clean = DataAccess.DataFunctions.CleanSerializedXML(xml.ToString());
            //cleanXML.Append(clean);

            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "ccp_Haymarket_CMSTable_Insert_Activity";
            //cmd.Parameters.AddWithValue("@Xml", cleanXML.ToString());

            //return DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.UAS.ToString());
            #endregion
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.UAS.ToString());
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "tempHaymarketCMS_Activity");
                bc.BatchSize = 10000;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("SubscriberId", "SubscriberId");
                bc.ColumnMappings.Add("ActivityId", "ActivityId");
                bc.ColumnMappings.Add("ShortTitle", "ShortTitle");
                bc.ColumnMappings.Add("PublishDate", "PublishDate");
                bc.ColumnMappings.Add("ExpirationDate", "ExpirationDate");
                bc.ColumnMappings.Add("PrimaryCountries", "PrimaryCountries");
                bc.ColumnMappings.Add("SecondaryCountries", "SecondaryCountries");
                bc.ColumnMappings.Add("Accreditation Info", "AccreditationInfo");
                bc.ColumnMappings.Add("ActivitFormat", "ActivitFormat");
                bc.ColumnMappings.Add("CE Type", "CEType");
                bc.ColumnMappings.Add("Content Type", "ContentType");
                bc.ColumnMappings.Add("Ecomm Status", "EcommStatus");
                bc.ColumnMappings.Add("Partner", "Partner");
                bc.ColumnMappings.Add("ProfessionIds", "ProfessionIds");
                bc.ColumnMappings.Add("SpecialtyIds", "SpecialtyIds");
                bc.ColumnMappings.Add("TopicIds", "ActivityTopicIds");
                bc.ColumnMappings.Add("Professions", "Professions");
                bc.ColumnMappings.Add("percentage", "percentage");
                bc.ColumnMappings.Add("AccreditationName", "AccreditationName");
                bc.ColumnMappings.Add("Credits", "Credits");
                bc.ColumnMappings.Add("Product", "Product");
                bc.ColumnMappings.Add("Platform", "Platform");
                bc.ColumnMappings.Add("Model", "Model");
                bc.ColumnMappings.Add("IsTablet", "IsTablet");
                bc.ColumnMappings.Add("OsVersion", "OsVersion");
                bc.ColumnMappings.Add("ActivityPubCode", "ActivityPubCode");

                bc.WriteToServer(dtBulk);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }

        public static bool Haymarket_CMS_Insert_Ecomm(DataTable dtBulk)
        {
            if (dtBulk == null)
            {
                throw new ArgumentNullException(nameof(dtBulk));
            }

            var mappings = new Dictionary<string, string>
            {
                [ColumnSubscriberId] = ColumnSubscriberId,
                [ColumnOrderId] = ColumnOrderId,
                [ColumnECommProductId] = ColumnECommProductId,
                [ColumnECommPubCode] = ColumnECommPubCode,
                [ColumnDescription] = ColumnDescription,
                [ColumnListPrice] = ColumnListPrice
            };

            ClientMethodsHelper.BulkCopy(dtBulk, TableTempHaymarketCmsEcomm, mappings, BatchSizeHaymarket);
            return true;
        }

        public static bool Haymarket_CMS_Update_Medical(DataTable dtUpdate)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<XML>");
            foreach (DataRow dr in dtUpdate.Rows)
            {
                if (!dr["SubscriberId"].ToString().Equals("NULL") && dr["SubscriberId"].ToString().Length > 0)
                {
                    xml.AppendLine("<Medical>");
                    //YearsInPracticeId PatientPopulationId Profession LicenseNumber DOB YearOfGraduation PracticeType PrimarySpecialty SeconddarySpecialty TopicIds
                    xml.AppendLine("<SubscriberId>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["SubscriberId"].ToString())) + "</SubscriberId>");
                    xml.AppendLine("<YearsInPracticeId>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["YearsInPracticeId"].ToString())) + "</YearsInPracticeId>");
                    xml.AppendLine("<PatientPopulationId>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["PatientPopulationId"].ToString())) + "</PatientPopulationId>");
                    xml.AppendLine("<Profession>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Profession"].ToString())) + "</Profession>");
                    xml.AppendLine("<LicenseNumber>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["LicenseNumber"].ToString())) + "</LicenseNumber>");
                    xml.AppendLine("<DOB>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["DOB"].ToString())) + "</DOB>");
                    xml.AppendLine("<YearOfGraduation>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["YearOfGraduation"].ToString())) + "</YearOfGraduation>");
                    xml.AppendLine("<PracticeType>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["PracticeType"].ToString())) + "</PracticeType>");
                    xml.AppendLine("<PrimarySpecialty>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["PrimarySpecialty"].ToString())) + "</PrimarySpecialty>");
                    xml.AppendLine("<SeconddarySpecialty>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["SeconddarySpecialty"].ToString())) + "</SeconddarySpecialty>");
                    xml.AppendLine("<MedicalTopicIds>" + Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["TopicIds"].ToString())) + "</MedicalTopicIds>");

                    xml.AppendLine("</Medical>");
                }
            }
            xml.AppendLine("</XML>");

            StringBuilder cleanXML = new StringBuilder();
            string clean = KM.Common.DataFunctions.CleanSerializedXML(xml.ToString());
            cleanXML.Append(clean);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Haymarket_CMSTable_Update_Medical";
            cmd.Parameters.AddWithValue("@Xml", cleanXML.ToString());

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        #endregion

        #region BriefMedia
        public static DataTable BriefMedia_Select_Data(KMPlatform.Entity.Client client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_BriefMedia_Select_Data";
            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.GetDataTable(cmd);
        }
        public static DataTable BriefMedia_Select_Data_Paging(KMPlatform.Entity.Client client, int page, int records)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_BriefMedia_Select_Data_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", records);
            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.GetDataTable(cmd);
        }

        public static int BriefMedia_Relational_Get_Count(KMPlatform.Entity.Client client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_BriefMedia_Relational_Get_Count";
            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd));
        }

        public static bool BriefMedia_CreateTempCMSTables(KMPlatform.Entity.Client client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_BriefMedia_CreateCMSTables";
            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool BriefMedia_DropTempCMSTables(KMPlatform.Entity.Client client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_BriefMedia_DropCMSTables";
            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool BriefMedia_Relational_Insert_Access(KMPlatform.Entity.Client client, DataTable dtBulk)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(dtBulk));
            }

            var root = new XElement(TagXml);

            foreach (DataRow row in dtBulk.Rows)
            {
                var accessId = row.GetString(TagAccessId);
                var userId = row.GetString(ColumnUserId);
                if (!string.IsNullOrWhiteSpace(accessId) && !string.IsNullOrWhiteSpace(userId))
                {
                    var node = new XElement(
                        TagUser,
                        new XElement(TagAccessId, Core_AMS.Utilities.StringFunctions.CleanXMLString(accessId)),
                        new XElement(TagDrupalId, Core_AMS.Utilities.StringFunctions.CleanXMLString(userId))
                    );

                    root.Add(node);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, ProcedureBriefMediaInsertAccess, client);
        }

        public static bool BriefMedia_Relational_Update_Users(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            var root = new XElement(TagXml);

            foreach (DataRow row in dtUpdate.Rows)
            {
                var userId = row.GetString(ColumnDrupalUserId);
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var email = row[TagEmail].ToString();
                    var emailText = Core_AMS.Utilities.StringFunctions.isEmail(email)
                        ? Core_AMS.Utilities.StringFunctions.CleanXMLString(email)
                        : Core_AMS.Utilities.StringFunctions.CleanXMLString(TextInvalidEmail);
                    var taxNode = new XElement(
                        TagUser,
                        new XElement(TagDrupalId, Core_AMS.Utilities.StringFunctions.CleanXMLString(userId)),
                        new XElement(TagFirstName, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[ColumnFirstName].ToString())),
                        new XElement(TagLastName, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[ColumnLastName].ToString())),
                        new XElement(TagEmail, emailText)
                    );

                    root.Add(taxNode);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, ProcedureBriefMediaUpdateUsers, client);
        }

        public static bool BriefMedia_Relational_Update_TaxBehavior(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            var root = new XElement(TagXml);

            foreach (DataRow row in dtUpdate.Rows)
            {
                var accessId = row.GetString(TagAccessId);
                if (!string.IsNullOrWhiteSpace(accessId))
                {
                    var taxNode = new XElement(
                        TagTax,
                        new XElement(TagAccessId, Core_AMS.Utilities.StringFunctions.CleanXMLString(accessId)),
                        new XElement(TagTopicCode, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagTaxonomyId].ToString()))
                    );

                    root.Add(taxNode);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, ProcedureBriefMediaUpdateTaxBehavior, client);
        }

        public static bool BriefMedia_Relational_Update_PageBehavior(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            var root = new XElement(TagXml);

            foreach (DataRow row in dtUpdate.Rows)
            {
                var accessId = row.GetString(TagAccessId);
                if (!string.IsNullOrWhiteSpace(accessId))
                {
                    var node = new XElement(
                        TagPage,
                        new XElement(TagAccessId, Core_AMS.Utilities.StringFunctions.CleanXMLString(accessId)),
                        new XElement(TagPageId, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagPageUnderscoreId].ToString()))
                    );

                    root.Add(node);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, ProcedureBriefMediaUpdatePageBehavior, client);
        }

        public static bool BriefMedia_Relational_Update_SearchBehavior(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<XML>");
            foreach (DataRow dr in dtUpdate.Rows)
            {
                if (!dr["Access_ID"].ToString().Equals("NULL") && dr["Access_ID"].ToString().Length > 0)
                {
                    xml.AppendLine("<Search>");

                    xml.AppendLine("<AccessID>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Access_ID"].ToString()) + "</AccessID>");
                    xml.AppendLine("<SearchTerm>" + Core_AMS.Utilities.StringFunctions.CleanXMLString(dr["Search_Term"].ToString()) + "</SearchTerm>");

                    xml.AppendLine("</Search>");
                }
            }
            xml.AppendLine("</XML>");
            StringBuilder cleanXML = new StringBuilder();
            string clean = KM.Common.DataFunctions.CleanSerializedXML(xml.ToString());
            cleanXML.Append(clean);

            bool done = false;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_BriefMedia_BMWU_Update_SearchBehavior";
            cmd.Parameters.AddWithValue("@Xml", cleanXML.ToString());
            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
            done = KM.Common.DataFunctions.ExecuteNonQuery(cmd);
            return done;
        }

        public static bool BriefMedia_Relational_Update_TopicCode(KMPlatform.Entity.Client client, DataTable dtUpdate)
        {
            var root = new XElement(TagXml);

            foreach (DataRow row in dtUpdate.Rows)
            {
                var emailAddress = row.GetString(TagEmailAddress);
                if (!string.IsNullOrWhiteSpace(emailAddress) && Core_AMS.Utilities.StringFunctions.isEmail(emailAddress))
                {
                    var node = new XElement(
                        TagTopic,
                        new XElement(TagEmailAddress, emailAddress),
                        new XElement(TagTopicId, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[ColumnTopicCode].ToString())),
                        new XElement(TagSearchTerms, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[ColumnTopicCodeText].ToString()))
                    );

                    root.Add(node);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, ProcedureBriefMediaUpdateTopicCode, client);
        }

        public static bool BriefMedia_Relational_CleanUpData(KMPlatform.Entity.Client client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_BriefMedia_BMWU_CleanUpData";
            cmd.Connection = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        #endregion

        #region GLM
        public static bool GLM_Relational_InsertData(DataTable dtBulk)
        {
            if (dtBulk == null)
            {
                throw new ArgumentNullException(nameof(dtBulk));
            }

            var root = new XElement(TagXml);

            foreach (DataRow row in dtBulk.Rows)
            {
                var email = row.GetString(ColumnGlmEmail);
                if (!string.IsNullOrWhiteSpace(email))
                {
                    var node = new XElement(
                        TagGlm,
                        new XElement(TagGlmEmail, Core_AMS.Utilities.StringFunctions.CleanXMLString(email)),
                        new XElement(TagLeadsSent, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[ColumnLeadsSent].ToString())),
                        new XElement(TagLikes, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[ColumnLikes].ToString())),
                        new XElement(TagBoardFollows, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[ColumnBoardFollows].ToString())),
                        new XElement(TagExhibitorFollows, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[ColumnExhibitorFollows].ToString()))
                    );

                    root.Add(node);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, ProcedureGlmRelationalInsertData, ConnectionString.UAS.ToString());
        }

        public static bool GLM_CreateTempCMSTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_GLM_CreateCMSTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool GLM_DropTempCMSTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_GLM_DropCMSTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }

        public static bool GLM_Relational_Update()
        {
            bool done = false;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_GLM_Relational_Update";
            done = KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());

            return done;
        }
        #endregion

        #region WATT
        public static bool WATT_CreateTempCMSTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_WATT_CreateCMSTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool WATT_DropTempCMSTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_WATT_DropCMSTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }

        public static bool WATT_Relational_Process_ECN_GROUPID_PUBCODE(DataTable dtBulk)
        {
            if (dtBulk == null)
            {
                throw new ArgumentNullException(nameof(dtBulk));
            }

            var root = new XElement(TagXml);

            foreach (DataRow row in dtBulk.Rows)
            {
                var groupId = row.GetString(TagWattGroupId);
                if (!string.IsNullOrWhiteSpace(groupId))
                {
                    var node = new XElement(
                        TagWatt,
                        new XElement(TagWattGroupId, Core_AMS.Utilities.StringFunctions.CleanXMLString(groupId)),
                        new XElement(TagWattPubCode, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagWattPubCode].ToString()))
                    );

                    root.Add(node);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, ProcedureWattProcessEcnGroupIdPubCode, ConnectionString.UAS.ToString());
        }

        public static bool WATT_Relational_Process_MacMic(DataTable dtBulk)
        {
            var root = new XElement(TagXml);

            foreach (DataRow row in dtBulk.Rows)
            {
                var productCode = row.GetString(ColumnProductCode);
                if (!string.IsNullOrWhiteSpace(productCode))
                {
                    var node = new XElement(
                        TagWatt,
                        new XElement(TagPubCode, Core_AMS.Utilities.StringFunctions.CleanXMLString(productCode)),
                        new XElement(TagFoxColumnName, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[ColumnFoxColumnName].ToString())),
                        new XElement(TagCodeSheetValue, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagCodeSheetValue].ToString()))
                    );

                    root.Add(node);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, ProcedureWattProcessMacMic, ConnectionString.UAS.ToString());
        }

        public static DataTable WATT_Get_Mic_Data()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_WATT_Get_Mic_Data";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }
        public static DataTable WATT_Get_Mac_Data()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_WATT_Get_Mac_Data";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        public static DataTable WATT_Get_ECN_Data(string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "getEnewsDataToDQM";
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Parameters.AddWithValue("@Deltastartdate", DBNull.Value);

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.ECN_Temp.ToString());
        }
        #endregion

        #region Vcast
        public static bool Vcast_CreateTempMXBooksTable()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Vcast_CreateTempMXBooksTable";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool Vcast_DropTempMXBooksTable()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Vcast_DropTempMXBooksTable";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }

        public static bool Vcast_Process_File_MX_Books(DataTable dtBulk)
        {
            return ProcessVcastFile(dtBulk, ProcedureVcastProcessFileMxBooks);
        }

        public static DataTable Vcase_Get_Distinct_MX_Books()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Vcast_Get_Distinct_MX_Books";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        public static bool Vcast_CreateTempMXElanTable()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Vcast_CreateTempMXElanTable";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool Vcast_DropTempMXElanTable()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Vcast_DropTempMXElanTable";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }

        public static bool Vcast_Process_File_MX_Elan(DataTable dtBulk)
        {
            return ProcessVcastFile(dtBulk, ProcedureVcastProcessFileMxElan);
        }

        public static DataTable Vcase_Get_Distinct_MX_Elan()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Vcast_Get_Distinct_MX_Elan";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        private static bool ProcessVcastFile(DataTable table, string procedureName)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var root = new XElement(TagXml);

            foreach (DataRow row in table.Rows)
            {
                var subscribeNumber = row.GetString(TagSubscribeNumber);
                if (!string.IsNullOrWhiteSpace(subscribeNumber))
                {
                    var node = new XElement(
                        TagWatt,
                        new XElement(TagSubscribeNumber, Core_AMS.Utilities.StringFunctions.CleanXMLString(subscribeNumber)),
                        new XElement(TagSub02Ans, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagSub02Ans].ToString())),
                        new XElement(TagSub03Ans, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagSub03Ans].ToString()))
                    );

                    root.Add(node);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, procedureName, ConnectionString.UAS.ToString());
        }

        #endregion

        #region Northstar
        public static bool Northstar_CreateTempCMSTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Northstar_CreateTempCMSTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool Northstar_DropTempCMSTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Northstar_DropTempCMSTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }

        public static bool Northstar_Relational_InsertPerson(DataTable dtTable)
        {
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.UAS.ToString());
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "tempNorthstarWebPersonGroup");
                bc.BatchSize = 10000;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("PubCode", "PubCode");
                bc.ColumnMappings.Add("GlobalUserKey", "GlobalUserKey");
                bc.ColumnMappings.Add("Email", "Email");
                bc.ColumnMappings.Add("LoginDate", "LoginDate");
                bc.ColumnMappings.Add("UpdateDate", "UpdateDate");
                bc.ColumnMappings.Add("IsLegacy", "IsLegacy");
                bc.ColumnMappings.Add("FirstName", "FirstName");
                bc.ColumnMappings.Add("LastName", "LastName");
                bc.ColumnMappings.Add("AccountNbr", "AccountNbr");
                bc.ColumnMappings.Add("Status", "Status");
                bc.ColumnMappings.Add("Title", "Title");
                bc.ColumnMappings.Add("Industry", "Industry");
                bc.ColumnMappings.Add("CompanyName", "CompanyName");
                bc.ColumnMappings.Add("Address", "Address");
                bc.ColumnMappings.Add("City", "City");
                bc.ColumnMappings.Add("State", "State");
                bc.ColumnMappings.Add("Country", "Country");
                bc.ColumnMappings.Add("Zip", "Zip");
                bc.ColumnMappings.Add("SalesVolume", "SalesVolume");
                bc.ColumnMappings.Add("Purchased", "Purchased");
                bc.ColumnMappings.Add("DestBooked", "DestBooked");
                bc.ColumnMappings.Add("Affiliations", "Affiliations");
                bc.ColumnMappings.Add("Services", "Services");
                bc.ColumnMappings.Add("JobResp", "JobResp");
                bc.ColumnMappings.Add("PlanResp", "PlanResp");
                bc.ColumnMappings.Add("FacType", "FacType");
                bc.ColumnMappings.Add("HoldOffSite", "HoldOffSite");
                bc.ColumnMappings.Add("HoldOffSiteAreas", "HoldOffSiteAreas");
                bc.ColumnMappings.Add("EstAttendance", "EstAttendance");
                bc.ColumnMappings.Add("PrimBusiness", "PrimBusiness");

                bc.WriteToServer(dtTable);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
        public static bool Northstar_Relational_AddGroup(DataTable dtTable)
        {
            var root = new XElement(TagXml);

            foreach (DataRow row in dtTable.Rows)
            {
                var userKey = row.GetString(TagGlobalUserKey);
                if (!string.IsNullOrWhiteSpace(userKey))
                {
                    var node = new XElement(
                        TagNorthStar,
                        new XElement(TagPublishCode, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagPublishCode].ToString())),
                        new XElement(TagGlobalUserKey, Core_AMS.Utilities.StringFunctions.CleanXMLString(userKey)),
                        new XElement(TagGroupId, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagGroupId].ToString())),
                        new XElement(TagIsRecent, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagIsRecent].ToString())),
                        new XElement(TagAddDate, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagAddDate].ToString())),
                        new XElement(TagDropDate, Core_AMS.Utilities.StringFunctions.CleanXMLString(row[TagDropDate].ToString()))
                    );

                    root.Add(node);
                }
            }

            var xml = KM.Common.DataFunctions.CleanSerializedXML(root.ToString());

            return ClientMethodsHelper.ExecuteNonQueryWithXml(xml, ProcedureNorthstarRelationalAddGroup, ConnectionString.UAS.ToString());
        }

        public static int Northstar_Get_WEB_Person_Group_Get_Count()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Northstar_Get_WEB_Person_Group_Get_Count";

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static DataTable Northstar_Get_WEB_Person_Group_Data_Paging(int page, int records)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Northstar_Get_WEB_Person_Group_Data_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", records);

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }
        public static DataTable Northstar_Get_WEB_Person_Group_Data()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Northstar_Get_WEB_Person_Group_Data";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }
        #endregion

        #region Advanstar
        public static DataTable Advanstar_Select_Data_Paging(int page, int records)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Advanstar_Select_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", records);

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        public static void Advanstar_Insert_PersonID(DataTable dtUpdate)
        {
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.UAS.ToString());
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "tempAdvanstarPersonID");
                bc.BatchSize = 2500;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("person_id", "Person_ID");
                bc.ColumnMappings.Add("sourcecode", "SourceCode");

                bc.WriteToServer(dtUpdate);
                bc.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static DataTable Advanstar_Insert_PersonID_Final()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Advanstar_Insert_PersonID_Final";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        public static bool Advanstar_CreateTempTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Advanstar_CreateTempTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }

        public static void Advanstar_Insert_RegCodeCompare(DataTable dtUpdate)
        {
            var mappings = new Dictionary<string, string>
            {
                [ColumnTicketType] = ColumnTicketType,
                [ColumnTicketSubt] = ColumnTicketSubt,
                [ColumnRegCode] = ColumnRegCode
            };

            ClientMethodsHelper.BulkCopy(dtUpdate, TableTempAdvanstarRegCodeCompare, mappings);
        }

        public static void Advanstar_Insert_RegCode(DataTable dtUpdate)
        {
            var mappings = new Dictionary<string, string>
            {
                [ColumnPersonIdSource] = ColumnPersonIdDestination,
                [ColumnTicketType] = ColumnTicketTypeDestination,
                [ColumnTicketSubt] = ColumnTicketSubtDestination
            };

            ClientMethodsHelper.BulkCopy(dtUpdate, TableTempAdvanstarRegCode, mappings);
        }

        public static DataTable Advanstar_Insert_RegCode_Final()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Advanstar_Insert_RegCode_Final";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        public static int Advanstar_GetCount(string name)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Advanstar_GetCount";
            cmd.Parameters.AddWithValue("@tableName", name);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static DataTable Advanstar_Select_Data_PagingRegCode(int page, int records)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Advanstar_Select_PagingRegCode";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", records);

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        public static bool Advanstar_DropTempTables()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Advanstar_DropTables";

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }

        public static void Advanstar_Insert_SourceCode(DataTable dtUpdate)
        {
            var mappings = new Dictionary<string, string>
            {
                [ColumnPersonIdSource] = ColumnPersonIdDestination,
                [ColumnSourceCodeSource] = ColumnSourceCodeDestination
            };

            ClientMethodsHelper.BulkCopy(dtUpdate, TableTempAdvanstarSourceCode, mappings);
        }

        public static void Advanstar_Insert_PriCode(DataTable dtUpdate)
        {
            var mappings = new Dictionary<string, string>
            {
                [ColumnSourceCodeSource] = ColumnSourceCodeDestination,
                [ColumnPriCodeSource] = ColumnPriCodeDestination
            };

            ClientMethodsHelper.BulkCopy(dtUpdate, TableTempAdvanstarPriCode, mappings);
        }

        public static void Advanstar_Insert_RefreshDupes(DataTable dtUpdate)
        {
            var mappings = new Dictionary<string, string>
            {
                [ColumnSequenceSource] = ColumnSequenceDestination,
                [ColumnIGroupNoSource] = ColumnIGroupNoDestination,
            };

            ClientMethodsHelper.BulkCopy(dtUpdate, TableTempAdvanstarRefreshDupes, mappings);
        }

        public static DataTable Advanstar_Select_Data()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Advanstar_Select_Data";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAS.ToString());
        }

        public static DataTable Advanstar_Select_InDupes()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Advanstar_Select_InDupes";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.AdvanstarMasterInDupes.ToString());
        }

        public static DataTable Advanstar_Select_Final()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Advanstar_Select_CBIATTEN_Final";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.AdvanstarMaster.ToString());
        }

        public static DataTable Advanstar_Select_PRDCDES()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Advanstar_Select_PRDCDE";

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.AdvanstarMaster.ToString());
        }

        public static int Advanstar_Get_ECN_Count(string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "getEnewsDataToDQM_Count";
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Parameters.AddWithValue("@Deltastartdate", DBNull.Value);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.ECN_Temp.ToString()));
        }

        public static DataTable Advanstar_Select_ECN_Paging(int page, int records, string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "getEnewsDataToDQM_Paging";
            cmd.Parameters.AddWithValue("@CurrentPage", page);
            cmd.Parameters.AddWithValue("@PageSize", records);
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Parameters.AddWithValue("@Deltastartdate", DBNull.Value);

            return KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.ECN_Temp.ToString());
        }
        #endregion

        #region Canon
        public static bool Canon_ConsensusDimension_EventSwipe(string xml, KMPlatform.Entity.Client client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_Canon_ConsensusDim_EventSwipe";
            cmd.Parameters.AddWithValue("@xml", xml);
            SqlConnection conn = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
            cmd.Connection = conn;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        #endregion

        #region Specialty Foods
        public static bool SpecialityFoods_ConsensusDimension_EventSwipe(string xml, KMPlatform.Entity.Client client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ccp_SpecialityFoods_ConsensusDim_EventSwipe";
            cmd.Parameters.AddWithValue("@xml", xml);
            SqlConnection conn = KMPlatform.DataAccess.DataFunctions.GetClientSqlConnection(client);
            cmd.Connection = conn;
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        #endregion
    }
}
