using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAD_Lookup.DataAccess
{
    public class Code
    {
        public static List<Entity.Code> Select()
        {

            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = "UAD_LOOKUP";

                List<Entity.Code> retItem = (List<Entity.Code>) CacheUtil.GetFromCache("Code", DatabaseName);

                if (retItem == null)
                {
                    retItem = GetData();

                    CacheUtil.AddToCache("Code", retItem, DatabaseName);
                }

                return retItem;
            }
            else
            {
                return GetData();
            }
         
           
        }
        public static List<Model.Operator> GetOperators()
        {
            List<Model.Operator> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Operators_Select";

            retItem = DataFunctions.GetList<Model.Operator>(cmd);
            return retItem;
        }
        public static List<Entity.Code> Select(int codeTypeId)
        {
            List<Entity.Code> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select_CodeTypeId";
            cmd.Parameters.AddWithValue("@CodeTypeId", codeTypeId);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Code> Select(Enums.CodeType codeType)
        {
            List<Entity.Code> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select_CodeTypeName";
            cmd.Parameters.AddWithValue("@CodeTypeName", codeType.ToString().Replace("_", " "));

            retItem = GetList(cmd);
            return retItem;
        }
        public static DataTable dtGetCode(string codeType)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dt_CodeType";
            cmd.Parameters.AddWithValue("@CodeTypeName", codeType);
            dt = KM.Common.DataFunctions.GetDataTable(cmd, ConnectionString.UAD_Lookup.ToString());
            return dt;
        }
        public static List<Entity.Code> SelectForDemographicAttribute(Enums.CodeType codeType, int dataCompareResultQueId, string ftpFolder)
        {
            List<Entity.Code> retList = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_SelectForDemographicAttribute";
            cmd.Parameters.AddWithValue("@CodeTypeName", codeType.ToString().Replace("_", " "));
            cmd.Parameters.AddWithValue("@DataCompareResultQueId", dataCompareResultQueId);
            cmd.Parameters.AddWithValue("@FtpFolder", ftpFolder);
            retList = GetList(cmd);
            //try
            //{
            //    using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAS.ToString()))
            //    {
            //        if (rdr != null)
            //        {
            //            Entity.Code retItem = new Entity.Code();
            //            DynamicBuilder<Entity.Code> builder = DynamicBuilder<Entity.Code>.CreateBuilder(rdr);
            //            while (rdr.Read())
            //            {
            //                retItem = builder.Build(rdr);
            //                if (retItem != null)
            //                {
            //                    retList.Add(retItem);
            //                }
            //            }
            //            rdr.Close();
            //            rdr.Dispose();
            //        }
            //    }
            //}
            //catch { }
            //finally
            //{
            //    cmd.Connection.Close();
            //    cmd.Dispose();
            //}
            return retList;
        }
        public static List<Entity.Code> SelectChildren(int parentCodeID)
        {
            List<Entity.Code> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select_ParentCodeId";
            cmd.Parameters.AddWithValue("@ParentCodeId", parentCodeID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Code> SelectChildren(Enums.CodeType parentCodeType, string parentCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select_ParentCodeType_ParentCodeName";
            cmd.Parameters.AddWithValue("@parentCodeType", parentCodeType.ToString().Replace("_"," "));
            cmd.Parameters.AddWithValue("@parentCode", parentCode);
            return GetList(cmd);
        }
        public static bool CodeExist(string codeName, int codeTypeId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CodeExist_CodeName_CodeValue";
            cmd.Parameters.Add(new SqlParameter("@CodeTypeId", codeTypeId));
            cmd.Parameters.Add(new SqlParameter("@CodeName", codeName.Replace("_", " ")));

            return Convert.ToBoolean(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
        public static bool CodeExist(string codeName, Enums.CodeType codeType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CodeExist_CodeTypeName_CodeName";
            cmd.Parameters.Add(new SqlParameter("@CodeTypeName", codeType.ToString().Replace("_", " ")));
            cmd.Parameters.Add(new SqlParameter("@CodeName", codeName.Replace("_", " ")));

            return Convert.ToBoolean(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
        public static bool CodeValueExist(string codeValue, int codeTypeId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CodeValueExist_CodeTypeId_CodeValue";
            cmd.Parameters.Add(new SqlParameter("@CodeTypeId", codeTypeId));
            cmd.Parameters.Add(new SqlParameter("@CodeValue", codeValue.Replace("_", " ")));

            return Convert.ToBoolean(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
        public static bool CodeValueExist(string codeValue, Enums.CodeType codeType)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CodeValueExist_CodeTypeName_CodeValue";
            cmd.Parameters.Add(new SqlParameter("@CodeTypeName", codeType.ToString().Replace("_", " ")));
            cmd.Parameters.Add(new SqlParameter("@CodeValue", codeValue.Replace("_", " ")));

            return Convert.ToBoolean(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
        public static Entity.Code SelectCodeId(int codeId)
        {
            Entity.Code retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select_CodeId";
            cmd.Parameters.AddWithValue("@CodeId", codeId);

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.Code SelectCodeName(Enums.CodeType codeType, string codeName)
        {
            Entity.Code retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select_CodeTypeName_CodeName";
            cmd.Parameters.AddWithValue("@CodeTypeName", codeType.ToString().Replace("_", " "));
            cmd.Parameters.AddWithValue("@CodeName", codeName.Replace("_", " "));

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.Code SelectCodeValue(Enums.CodeType codeType, string codeValue)
        {
            Entity.Code retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select_CodeTypeName_CodeValue";
            cmd.Parameters.AddWithValue("@CodeTypeName", codeType.ToString().Replace("_", " "));
            cmd.Parameters.AddWithValue("@CodeValue", codeValue.Replace("_", " "));

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.Code Get(SqlCommand cmd)
        {
            Entity.Code retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Code();
                        DynamicBuilder<Entity.Code> builder = DynamicBuilder<Entity.Code>.CreateBuilder(rdr);
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

        public static List<Entity.Code> GetData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Select";
            List<Entity.Code> retList = new List<Entity.Code>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Code retItem = new Entity.Code();
                        DynamicBuilder<Entity.Code> builder = DynamicBuilder<Entity.Code>.CreateBuilder(rdr);
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

        public static List<Entity.Code> GetList(SqlCommand cmd)
        {
            List<Entity.Code> retList = new List<Entity.Code>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAD_Lookup.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Code retItem = new Entity.Code();
                        DynamicBuilder<Entity.Code> builder = DynamicBuilder<Entity.Code>.CreateBuilder(rdr);
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
        public static int Save(Entity.Code x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Code_Save";
            cmd.Parameters.Add(new SqlParameter("@CodeId", x.CodeId));
            cmd.Parameters.Add(new SqlParameter("@CodeTypeId", x.CodeTypeId));
            cmd.Parameters.Add(new SqlParameter("@CodeName", x.CodeName));
            cmd.Parameters.Add(new SqlParameter("@CodeValue", x.CodeValue));
            cmd.Parameters.Add(new SqlParameter("@DisplayName", x.DisplayName));
            cmd.Parameters.Add(new SqlParameter("@CodeDescription", x.CodeDescription));
            cmd.Parameters.Add(new SqlParameter("@DisplayOrder", x.DisplayOrder));
            cmd.Parameters.Add(new SqlParameter("@CodeValue", x.CodeValue));
            cmd.Parameters.Add(new SqlParameter("@CodeValue", x.CodeValue));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAD_Lookup.ToString()));
        }
        #region Circ
        #region Action Type
        //public static List<Entity.ActionType> Select()
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_ActionType_Select";
        //    return GetList(cmd);
        //}
        //public static int Save(Entity.ActionType at)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_ActionType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@ActionTypeID", at.ActionTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@ActionTypeName", at.ActionTypeName));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Circulation.ToString()));
        //}
        #endregion
        #region AddressType
        //public static List<Entity.AddressType> Select()
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_AddressType_Select";
        //    return GetList(cmd);
        //}
        //public static int Save(Entity.AddressType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_AddressType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@AddressTypeID", x.AddressTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@AddressTypeName", x.AddressTypeName));
        //    cmd.Parameters.Add(new SqlParameter("@AddressTypeCode", x.AddressTypeCode));
        //    cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
        //    cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
        //    cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
        //    cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
        //    cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Circulation.ToString()));
        //}
        #endregion
        #region CreditCardType
        //public static List<Entity.CreditCardType> Select()
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_CreditCardType_Select";
        //    return GetList(cmd);
        //}
        //public static int Save(Entity.CreditCardType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_CreditCardType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@CreditCardTypeID", x.CreditCardTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@CreditCardName", x.CreditCardName));
        //    cmd.Parameters.Add(new SqlParameter("@CreditCardCode", x.CreditCardCode));
        //    cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
        //    cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
        //    cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
        //    cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
        //    cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Circulation.ToString()));
        //}
        #endregion
        #region PaymentType
        //public static List<Entity.PaymentType> Select()
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_PaymentType_Select";
        //    return GetList(cmd);
        //}
        //public static int Save(Entity.PaymentType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_PaymentType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@PaymentTypeID", x.PaymentTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@PaymentTypeName", x.PaymentTypeName));
        //    cmd.Parameters.Add(new SqlParameter("@PaymentTypeCode", x.PaymentTypeCode));
        //    cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
        //    cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
        //    cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
        //    cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
        //    cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Circulation.ToString()));
        //}
        #endregion
        #region QualificationSourceType
        //public static List<Entity.QualificationSourceType> Select()
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_QualificationSourceType_Select";
        //    return GetList(cmd);
        //}
        //public static int Save(Entity.QualificationSourceType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_QualificationSourceType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@QSourceTypeID", x.QSourceTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@QSourceTypeName", x.QSourceTypeName));
        //    cmd.Parameters.Add(new SqlParameter("@QSourceTypeCode", x.QSourceTypeCode));
        //    cmd.Parameters.Add(new SqlParameter("@DisplayName", x.DisplayName));
        //    cmd.Parameters.Add(new SqlParameter("@DisplayOrder", x.DisplayOrder));
        //    cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
        //    cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
        //    cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
        //    cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
        //    cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Circulation.ToString()));
        //}
        #endregion
        #region SubscriberSourceType
        //public static List<Entity.SubscriberSourceType> Select()
        //{
        //    List<Entity.SubscriberSourceType> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_SubscriberSourceType_Select";

        //    retItem = GetList(cmd);
        //    return retItem;
        //}
        //public static int Save(Entity.SubscriberSourceType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_SubscriberSourceType_Save";
        //    //cmd.Parameters.Add(new SqlParameter("@TransactionCodeID", tct.TransactionCodeID));
        //    //cmd.Parameters.Add(new SqlParameter("@TransactionCodeName", tct.TransactionCodeName));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Circulation.ToString()));
        //}
        #endregion
        #region UserLogType
        //public static List<Entity.UserLogType> Select()
        //{
        //    List<Entity.UserLogType> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_UserLogType_Select";

        //    retItem = GetList(cmd);
        //    return retItem;
        //}
        //public static int Save(Entity.UserLogType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_UserLogType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@UserLogTypeID", x.UserLogTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@UserLogTypeName", x.UserLogTypeName));
        //    cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
        //    cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
        //    cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
        //    cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
        //    cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Circulation.ToString()));
        //}
        #endregion
        #endregion
        #region UAD
        #region ExportType

        #endregion
        #region RecurrenceType

        #endregion
        #endregion
        #region UAS
        #region ConfigurationType

        #endregion
        #region FieldMapppingType
        //public static List<Entity.FieldMappingType> Select()
        //{
        //    List<Entity.FieldMappingType> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_FieldMappingType_Select";

        //    retItem = GetList(cmd);
        //    return retItem;
        //}
        //public static Entity.FieldMappingType Select(Enums.FieldMappingTypeName fmtName)
        //{
        //    Entity.FieldMappingType retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_FieldMappingType_Select_Name";
        //    cmd.Parameters.AddWithValue("@Name", fmtName.ToString());
        //    retItem = Get(cmd);
        //    return retItem;
        //}
        //public static int Save(Entity.FieldMappingType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_FieldMappingType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@FieldMappingTypeID", x.FieldMappingTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@Name", x.Name));
        //    cmd.Parameters.Add(new SqlParameter("@Code", x.Code));
        //    cmd.Parameters.Add(new SqlParameter("@DisplayOrder", x.DisplayOrder));
        //    cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
        //    cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
        //    cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
        //    cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
        //    cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.UAD_Lookup.ToString()));
        //}
        #endregion
        #region FileSnippetType
        //public static List<Entity.FileSnippetType> Select()
        //{
        //    List<Entity.FileSnippetType> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_FileSnippetType_Select";

        //    retItem = GetList(cmd);
        //    return retItem;
        //}
        //public static int Save(Entity.FileSnippetType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_FileSnippetType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@FileSnippetID", x.FileSnippetTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@FileSnippetName", x.FileSnippetTypeName));
        //    cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
        //    cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
        //    cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
        //    cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.UAD_Lookup.ToString()));
        //}
        #endregion
        #region FileStatusType
        //public static List<Entity.FileStatusType> Select()
        //{
        //    List<Entity.FileStatusType> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_FileStatusType_Select";

        //    retItem = GetList(cmd);
        //    return retItem;
        //}
        //public static Entity.FileStatusType Select(int fileStatusTypeID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_FileStatusType_Select_FileStatusTypeID";
        //    cmd.Parameters.AddWithValue("@FileStatusTypeID", fileStatusTypeID);
        //    return Get(cmd);
        //}
        //public static Entity.FileStatusType Select(BusinessLogic.Enums.FileStatusTypeName fileStatusName)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_FileStatusType_Select_FileStatusName";
        //    cmd.Parameters.AddWithValue("@FileStatusName", fileStatusName.ToString());
        //    return Get(cmd);
        //}
        #endregion
        #region FileRecurranceType
        //public static List<Entity.SourceFileType> Select()
        //{
        //    List<Entity.SourceFileType> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_SourceFileType_Select";

        //    retItem = GetList(cmd);
        //    return retItem;
        //}
        //public static Entity.SourceFileType Select(int SourceFileTypeID)
        //{
        //    Entity.SourceFileType retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_SourceFileType_Select_SourceFileTypeID";
        //    cmd.Parameters.AddWithValue("@SourceFileTypeID", SourceFileTypeID);

        //    retItem = Get(cmd);
        //    return retItem;
        //}
        #endregion
        #region TransformationType
        //public static List<Entity.TransformationType> Select()
        //{
        //    List<Entity.TransformationType> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_TransformationType_Select";

        //    retItem = GetList(cmd);
        //    return retItem;
        //}
        //public static Entity.TransformationType Select(int transformationTypeID)
        //{
        //    Entity.TransformationType retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_TransformationType_Select_TransformationTypeID";
        //    cmd.Parameters.Add(new SqlParameter("@transformationTypeID", transformationTypeID));

        //    retItem = Get(cmd);
        //    return retItem;
        //}
        //public static int Save(Entity.TransformationType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_TransformationType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@TransformationTypeID", x.TransformationTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@TransformationTypeName", x.TransformationTypeName));
        //    cmd.Parameters.Add(new SqlParameter("@DisplayOrder", x.DisplayOrder));
        //    cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
        //    cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
        //    cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
        //    cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
        //    cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.UAD_Lookup.ToString()));
        //}
        #endregion
        #region UserLogType
        //public static List<Entity.UserLogType> Select()
        //{
        //    List<Entity.UserLogType> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_UserLogType_Select";

        //    retItem = GetList(cmd);
        //    return retItem;
        //}

        //public static int Save(Entity.UserLogType x)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_UserLogType_Save";
        //    cmd.Parameters.Add(new SqlParameter("@UserLogTypeID", x.UserLogTypeID));
        //    cmd.Parameters.Add(new SqlParameter("@UserLogTypeName", x.UserLogTypeName));
        //    cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
        //    cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
        //    cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
        //    cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
        //    cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.UAD_Lookup.ToString()));
        //}
        #endregion
        #region DatabaseDestinationType

        #endregion
        #region DatabaseFileType

        #endregion
        #region PostalServiceType

        #endregion
        #endregion
    }
}
