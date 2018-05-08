using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class Objects
    {
        public static List<Object.SubscriberProduct> GetProductDemographics(KMPlatform.Object.ClientConnections client, string emailAddress, string productCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberProduct_Select_Email_ProductCode";
            cmd.Parameters.AddWithValue("@Email", emailAddress);
            cmd.Parameters.AddWithValue("@ProductCode", productCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetSubscriberProductList(cmd);
        }
        public static List<Object.SubscriberConsensus> GetDemographics(KMPlatform.Object.ClientConnections client, string emailAddress)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberConsensus_Select_Email";
            cmd.Parameters.AddWithValue("@Email", emailAddress);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetSubscriberConsensusList(cmd);
        }
        public static List<Object.SubscriberConsensus> GetSubscriberConsensusList(SqlCommand cmd)
        {
            List<Object.SubscriberConsensus> retList = new List<Object.SubscriberConsensus>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.SubscriberConsensus retItem = new Object.SubscriberConsensus();
                        DynamicBuilder<Object.SubscriberConsensus> builder = DynamicBuilder<Object.SubscriberConsensus>.CreateBuilder(rdr);
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
        public static List<Object.SubscriberProduct> GetSubscriberProductList(SqlCommand cmd)
        {
            List<Object.SubscriberProduct> retList = new List<Object.SubscriberProduct>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.SubscriberProduct retItem = new Object.SubscriberProduct();
                        DynamicBuilder<Object.SubscriberProduct> builder = DynamicBuilder<Object.SubscriberProduct>.CreateBuilder(rdr);
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

        public static List<Object.Dimension> GetDimensions(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Dimension_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetDimensionList(cmd);
        }
        public static List<Object.Dimension> GetDimensionList(SqlCommand cmd)
        {
            List<Object.Dimension> retList = new List<Object.Dimension>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.Dimension retItem = new Object.Dimension();
                        DynamicBuilder<Object.Dimension> builder = DynamicBuilder<Object.Dimension>.CreateBuilder(rdr);
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
        public static List<FrameworkUAD.Object.CustomField> GetCustomFields_Product(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ProductCustomField_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return CustomFieldList(cmd);
        }
        public static List<FrameworkUAD.Object.CustomField> GetCustomFields_AdHoc(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ProductCustomFieldAdHoc_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return CustomFieldList(cmd);
        }
        public static List<FrameworkUAD.Object.CustomField> GetCustomFields_Consensus(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ConsensusCustomField_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return CustomFieldList(cmd);
        }
        public static List<FrameworkUAD.Object.CustomField> GetCustomFields_ConsensusAdHoc(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ConsensusCustomFieldAdHoc_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return CustomFieldList(cmd);
        }
        public static List<Object.CustomField> CustomFieldList(SqlCommand cmd)
        {
            List<Object.CustomField> retList = new List<Object.CustomField>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.CustomField retItem = new Object.CustomField();
                        DynamicBuilder<Object.CustomField> builder = DynamicBuilder<Object.CustomField>.CreateBuilder(rdr);
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

        public static List<FrameworkUAD.Object.CustomFieldBrand> GetCustomFieldsBrand(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_BrandCustomField_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return CustomFieldsBrandList(cmd);
        }
        public static List<Object.CustomFieldBrand> CustomFieldsBrandList(SqlCommand cmd)
        {
            List<Object.CustomFieldBrand> retList = new List<Object.CustomFieldBrand>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.CustomFieldBrand retItem = new Object.CustomFieldBrand();
                        DynamicBuilder<Object.CustomFieldBrand> builder = DynamicBuilder<Object.CustomFieldBrand>.CreateBuilder(rdr);
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


        public static List<FrameworkUAD.Object.CustomFieldValue> GetCustomFieldValues(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CustomFieldValue_Select_Product";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return CustomFieldValueList(cmd);
        }
        public static List<FrameworkUAD.Object.CustomFieldValue> GetConsensusCustomFieldValues(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_CustomFieldValue_Select_Consensus";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return CustomFieldValueList(cmd);
        }
        public static List<Object.CustomFieldValue> CustomFieldValueList(SqlCommand cmd)
        {
            List<Object.CustomFieldValue> retList = new List<Object.CustomFieldValue>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.CustomFieldValue retItem = new Object.CustomFieldValue();
                        DynamicBuilder<Object.CustomFieldValue> builder = DynamicBuilder<Object.CustomFieldValue>.CreateBuilder(rdr);
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


        
    }
}
