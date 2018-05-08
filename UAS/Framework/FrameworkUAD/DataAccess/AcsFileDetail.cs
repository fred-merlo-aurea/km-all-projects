using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using FrameworkUAS.BusinessLogic;
using KM.Common;
using static Core_AMS.Utilities.BulkDataReader<FrameworkUAD.Entity.AcsFileDetail>;
using FileLogEntity = FrameworkUAS.Entity.FileLog;

namespace FrameworkUAD.DataAccess
{
    public class AcsFileDetail
    {
        private const string InsertProcessCode = "AcsFileDetail.Save";
        private const int InsertFileStatusTypeId = -99;
        private const int InsertSourceFileId = -99;
        private const string DatabaseNamePostFix = "[dbo].[AcsFileDetail]";

        private static readonly string[] InsertColumns =
        {
            "RecordType",
            "FileVersion",
            "SequenceNumber",
            "AcsMailerId",
            "KeylineSequenceSerialNumber",
            "MoveEffectiveDate",
            "MoveType",
            "DeliverabilityCode",
            "UspsSiteID",
            "LastName",
            "FirstName",
            "Prefix",
            "Suffix",
            "OldAddressType",
            "OldUrb",
            "OldPrimaryNumber",
            "OldPreDirectional",
            "OldStreetName",
            "OldSuffix",
            "OldPostDirectional",
            "OldUnitDesignator",
            "OldSecondaryNumber",
            "OldCity",
            "OldStateAbbreviation",
            "OldZipCode",
            "NewAddressType",
            "NewPmb",
            "NewUrb",
            "NewPrimaryNumber",
            "NewPreDirectional",
            "NewStreetName",
            "NewSuffix",
            "NewPostDirectional",
            "NewUnitDesignator",
            "NewSecondaryNumber",
            "NewCity",
            "NewStateAbbreviation",
            "NewZipCode",
            "Hyphen",
            "NewPlus4Code",
            "NewDeliveryPoint",
            "NewAbbreviatedCityName",
            "NewAddressLabel",
            "FeeNotification",
            "NotificationType",
            "IntelligentMailBarcode",
            "IntelligentMailPackageBarcode",
            "IdTag",
            "HardcopyToElectronicFlag",
            "TypeOfAcs",
            "FulfillmentDate",
            "ProcessingType",
            "CaptureType",
            "MadeAvailableDate",
            "ShapeOfMail",
            "MailActionCode",
            "NixieFlag",
            "ProductCode1",
            "ProductCodeFee1",
            "ProductCode2",
            "ProductCodeFee2",
            "ProductCode3",
            "ProductCodeFee3",
            "ProductCode4",
            "ProductCodeFee4",
            "ProductCode5",
            "ProductCodeFee5",
            "ProductCode6",
            "ProductCodeFee6",
            "Filler",
            "EndMarker",
            "ProductCode",
            "OldAddress1",
            "OldAddress2",
            "OldAddress3",
            "NewAddress1",
            "NewAddress2",
            "NewAddress3",
            "SequenceID",
            "TransactionCodeValue",
            "CategoryCodeValue",
            "IsIgnored",
            "AcsActionId",
            "CreatedDate",
            "CreatedTime",
            "ProcessCode"
        };

        public static List<Entity.AcsFileDetail> Select(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.AcsFileDetail> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AcsFileDetail_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.AcsFileDetail Get(SqlCommand cmd)
        {
            Entity.AcsFileDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.AcsFileDetail();
                        DynamicBuilder<Entity.AcsFileDetail> builder = DynamicBuilder<Entity.AcsFileDetail>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        public static List<Entity.AcsFileDetail> GetList(SqlCommand cmd)
        {
            List<Entity.AcsFileDetail> retList = new List<Entity.AcsFileDetail>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.AcsFileDetail retItem = new Entity.AcsFileDetail();
                        DynamicBuilder<Entity.AcsFileDetail> builder = DynamicBuilder<Entity.AcsFileDetail>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        public static int Save(Entity.AcsFileDetail x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AcsFileDetail_Save";
            cmd.Parameters.Add(new SqlParameter("@AcsFileDetailId", x.AcsFileDetailId));
            cmd.Parameters.Add(new SqlParameter("@AcsFileDetailId", x.AcsFileDetailId));
            cmd.Parameters.Add(new SqlParameter("@RecordType", x.RecordType));
            cmd.Parameters.Add(new SqlParameter("@FileVersion", x.FileVersion));
            cmd.Parameters.Add(new SqlParameter("@SequenceNumber", x.SequenceNumber));
            cmd.Parameters.Add(new SqlParameter("@AcsMailerId", x.AcsMailerId));
            cmd.Parameters.Add(new SqlParameter("@KeylineSequenceSerialNumber", x.KeylineSequenceSerialNumber));
            cmd.Parameters.Add(new SqlParameter("@MoveEffectiveDate", x.MoveEffectiveDate));
            cmd.Parameters.Add(new SqlParameter("@MoveType", x.MoveType));
            cmd.Parameters.Add(new SqlParameter("@DeliverabilityCode", x.DeliverabilityCode));
            cmd.Parameters.Add(new SqlParameter("@UspsSiteID", x.UspsSiteID));
            cmd.Parameters.Add(new SqlParameter("@LastName", x.LastName));
            cmd.Parameters.Add(new SqlParameter("@FirstName", x.FirstName));
            cmd.Parameters.Add(new SqlParameter("@Prefix", x.Prefix));
            cmd.Parameters.Add(new SqlParameter("@Suffix", x.Suffix));
            cmd.Parameters.Add(new SqlParameter("@OldAddressType", x.OldAddressType));
            cmd.Parameters.Add(new SqlParameter("@OldUrb", x.OldUrb));
            cmd.Parameters.Add(new SqlParameter("@OldPrimaryNumber", x.OldPrimaryNumber));
            cmd.Parameters.Add(new SqlParameter("@OldPreDirectional", x.OldPreDirectional));
            cmd.Parameters.Add(new SqlParameter("@OldStreetName", x.OldStreetName));
            cmd.Parameters.Add(new SqlParameter("@OldSuffix", x.OldSuffix));
            cmd.Parameters.Add(new SqlParameter("@OldPostDirectional", x.OldPostDirectional));
            cmd.Parameters.Add(new SqlParameter("@OldUnitDesignator", x.OldUnitDesignator));
            cmd.Parameters.Add(new SqlParameter("@OldSecondaryNumber", x.OldSecondaryNumber));
            cmd.Parameters.Add(new SqlParameter("@OldCity", x.OldCity));
            cmd.Parameters.Add(new SqlParameter("@OldStateAbbreviation", x.OldStateAbbreviation));
            cmd.Parameters.Add(new SqlParameter("@OldZipCode", x.OldZipCode));
            cmd.Parameters.Add(new SqlParameter("@NewAddressType", x.NewAddressType));
            cmd.Parameters.Add(new SqlParameter("@NewPmb", x.NewPmb));
            cmd.Parameters.Add(new SqlParameter("@NewUrb", x.NewUrb));
            cmd.Parameters.Add(new SqlParameter("@NewPrimaryNumber", x.NewPrimaryNumber));
            cmd.Parameters.Add(new SqlParameter("@NewPreDirectional", x.NewPreDirectional));
            cmd.Parameters.Add(new SqlParameter("@NewStreetName", x.NewStreetName));
            cmd.Parameters.Add(new SqlParameter("@NewSuffix", x.NewSuffix));
            cmd.Parameters.Add(new SqlParameter("@NewPostDirectional", x.NewPostDirectional));
            cmd.Parameters.Add(new SqlParameter("@NewUnitDesignator", x.NewUnitDesignator));
            cmd.Parameters.Add(new SqlParameter("@NewSecondaryNumber", x.NewSecondaryNumber));
            cmd.Parameters.Add(new SqlParameter("@NewCity", x.NewCity));
            cmd.Parameters.Add(new SqlParameter("@NewStateAbbreviation", x.NewStateAbbreviation));
            cmd.Parameters.Add(new SqlParameter("@NewZipCode", x.NewZipCode));
            cmd.Parameters.Add(new SqlParameter("@Hyphen", x.Hyphen));
            cmd.Parameters.Add(new SqlParameter("@NewPlus4Code", x.NewPlus4Code));
            cmd.Parameters.Add(new SqlParameter("@NewDeliveryPoint", x.NewDeliveryPoint));
            cmd.Parameters.Add(new SqlParameter("@NewAbbreviatedCityName", x.NewAbbreviatedCityName));
            cmd.Parameters.Add(new SqlParameter("@NewAddressLabel", x.NewAddressLabel));
            cmd.Parameters.Add(new SqlParameter("@FeeNotification", x.FeeNotification));
            cmd.Parameters.Add(new SqlParameter("@NotificationType", x.NotificationType));
            cmd.Parameters.Add(new SqlParameter("@IntelligentMailBarcode", x.IntelligentMailBarcode));
            cmd.Parameters.Add(new SqlParameter("@IntelligentMailPackageBarcode", x.IntelligentMailPackageBarcode));
            cmd.Parameters.Add(new SqlParameter("@IdTag", x.IdTag));
            cmd.Parameters.Add(new SqlParameter("@HardcopyToElectronicFlag", x.HardcopyToElectronicFlag));
            cmd.Parameters.Add(new SqlParameter("@TypeOfAcs", x.TypeOfAcs));
            cmd.Parameters.Add(new SqlParameter("@FulfillmentDate", x.FulfillmentDate));
            cmd.Parameters.Add(new SqlParameter("@ProcessingType", x.ProcessingType));
            cmd.Parameters.Add(new SqlParameter("@CaptureType", x.CaptureType));
            cmd.Parameters.Add(new SqlParameter("@MadeAvailableDate", x.MadeAvailableDate));
            cmd.Parameters.Add(new SqlParameter("@ShapeOfMail", x.ShapeOfMail));
            cmd.Parameters.Add(new SqlParameter("@MailActionCode", x.MailActionCode));
            cmd.Parameters.Add(new SqlParameter("@NixieFlag", x.NixieFlag));
            cmd.Parameters.Add(new SqlParameter("@ProductCode1", x.ProductCode1));
            cmd.Parameters.Add(new SqlParameter("@ProductCodeFee1", x.ProductCodeFee1));
            cmd.Parameters.Add(new SqlParameter("@ProductCode2", x.ProductCode2));
            cmd.Parameters.Add(new SqlParameter("@ProductCodeFee2", x.ProductCodeFee2));
            cmd.Parameters.Add(new SqlParameter("@ProductCode3", x.ProductCode3));
            cmd.Parameters.Add(new SqlParameter("@ProductCodeFee3", x.ProductCodeFee3));
            cmd.Parameters.Add(new SqlParameter("@ProductCode4", x.ProductCode4));
            cmd.Parameters.Add(new SqlParameter("@ProductCodeFee4", x.ProductCodeFee4));
            cmd.Parameters.Add(new SqlParameter("@ProductCode5", x.ProductCode5));
            cmd.Parameters.Add(new SqlParameter("@ProductCodeFee5", x.ProductCodeFee5));
            cmd.Parameters.Add(new SqlParameter("@ProductCode6", x.ProductCode6));
            cmd.Parameters.Add(new SqlParameter("@ProductCodeFee6", x.ProductCodeFee6));
            cmd.Parameters.Add(new SqlParameter("@Filler", x.Filler));
            cmd.Parameters.Add(new SqlParameter("@EndMarker", x.EndMarker));
            cmd.Parameters.Add(new SqlParameter("@ProductCode", x.ProductCode));
            cmd.Parameters.Add(new SqlParameter("@OldAddress1", x.OldAddress1));
            cmd.Parameters.Add(new SqlParameter("@OldAddress2", x.OldAddress2));
            cmd.Parameters.Add(new SqlParameter("@OldAddress3", x.OldAddress3));
            cmd.Parameters.Add(new SqlParameter("@NewAddress1", x.NewAddress1));
            cmd.Parameters.Add(new SqlParameter("@NewAddress2", x.NewAddress2));
            cmd.Parameters.Add(new SqlParameter("@NewAddress3", x.NewAddress3));
            cmd.Parameters.Add(new SqlParameter("@SequenceID", x.SequenceID));
            cmd.Parameters.Add(new SqlParameter("@TransactionCodeValue", x.TransactionCodeValue));
            cmd.Parameters.Add(new SqlParameter("@CategoryCodeValue", x.CategoryCodeValue));
            cmd.Parameters.Add(new SqlParameter("@IsIgnored", x.IsIgnored));
            cmd.Parameters.Add(new SqlParameter("@AcsActionId", x.AcsActionId));
            cmd.Parameters.Add(new SqlParameter("@ProcessCode", x.ProcessCode));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool Insert(List<Entity.AcsFileDetail> fileDetails, KMPlatform.Object.ClientConnections client)
        {
            if (fileDetails == null)
            {
                throw new ArgumentNullException(nameof(fileDetails));
            }

            var dataTable = ToDataTable(fileDetails);
            using (var sqlConnection = DataFunctions.GetClientSqlConnection(client))
            {
                try
                {
                    sqlConnection.Open();
                    using (var sqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.TableLock, null))
                    {
                        sqlBulkCopy.DestinationTableName = GetDestinationTableName(sqlConnection);
                        sqlBulkCopy.BatchSize = 0;
                        sqlBulkCopy.BulkCopyTimeout = 0;

                        foreach (var columnName in InsertColumns)
                        {
                            sqlBulkCopy.ColumnMappings.Add(columnName, columnName);
                        }

                        sqlBulkCopy.WriteToServer(dataTable);
                    }
                }
                catch (Exception exception) 
                    when(exception is SqlException
                      || exception is IOException
                      || exception is InvalidOperationException)
                {
                    var message = StringFunctions.FormatException(exception);
                    var fileLog = new FileLog();
                    var fileLogEntity = new FileLogEntity(
                                                InsertSourceFileId,
                                                InsertFileStatusTypeId,
                                                message,
                                                InsertProcessCode);

                    fileLog.Save(fileLogEntity);
                    
                    // POSSIBLE BUG: exception is rethrown...
                    throw;
                }
            }

            return true;
        }

        public static bool Update(string xml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AcsFileDetail_Update_Xml";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool UpdateSubscriberAddress(string xml, int publicationID, KMPlatform.Object.ClientConnections client, int userLogId, int userId, int SourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ACS_UpdateSubscriberAddress";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Parameters.AddWithValue("@publicationID", publicationID);
            cmd.Parameters.AddWithValue("@userLogId", userLogId);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@SourceFileID", SourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool KillSubscriber(string xml, int publicationID, KMPlatform.Object.ClientConnections client, int appId, int userId, int userLogId, int SourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ACS_KillSubscriber";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Parameters.AddWithValue("@publicationID", publicationID);
            cmd.Parameters.AddWithValue("@appId", appId);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@userLogId", userLogId);
            cmd.Parameters.AddWithValue("@SourceFileID", SourceFileID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool UpdateUADSubscriberAddress(string xml, int userId, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ACS_Update_UAS_SubscriberAddress";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Parameters.AddWithValue("@userID", userId);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool KillUADSubscriber(string xml, int clientID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_ACS_Kill_UAS_Subscriber";
            cmd.Parameters.AddWithValue("@xml", xml);
            cmd.Parameters.AddWithValue("@clientID", clientID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        private static string GetDestinationTableName(SqlConnection sqlConnection)
        {
            return $"[{sqlConnection.Database}].{DatabaseNamePostFix}";
        }
    }
}
