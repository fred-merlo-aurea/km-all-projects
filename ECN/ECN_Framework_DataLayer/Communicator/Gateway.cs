using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class Gateway
    {
        public static List<ECN_Framework_Entities.Communicator.Gateway> GetByCustomerID(int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_Gateway_Select_CustomerID";

            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Gateway GetByGatewayID(int GatewayID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_Gateway_Select_GatewayID";
            cmd.Parameters.AddWithValue("@GatewayID", GatewayID);

            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.Gateway GetByGatewayPubCode(string pubCode, string typeCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_Gateway_Select_GatewayPubCode";
            cmd.Parameters.AddWithValue("@PubCode", pubCode);
            cmd.Parameters.AddWithValue("@TypeCode", typeCode);

            return Get(cmd);
        }

        public static int Save(ECN_Framework_Entities.Communicator.Gateway gateway)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_Gateway_Save";

            if (gateway.GatewayID > 0)
                cmd.Parameters.AddWithValue("@GatewayID", gateway.GatewayID);
            cmd.Parameters.AddWithValue("@Name", gateway.Name);
            cmd.Parameters.AddWithValue("@CustomerID", gateway.CustomerID);
            cmd.Parameters.AddWithValue("@PubCode", gateway.PubCode);
            cmd.Parameters.AddWithValue("@GroupID", gateway.GroupID);
            cmd.Parameters.AddWithValue("@TypeCode", gateway.TypeCode);
            cmd.Parameters.AddWithValue("@Header", gateway.Header);
            cmd.Parameters.AddWithValue("@Footer", gateway.Footer);
            cmd.Parameters.AddWithValue("@ShowForgotPassword", gateway.ShowForgotPassword);
            cmd.Parameters.AddWithValue("@ForgotPasswordText", gateway.ShowForgotPassword ? gateway.ForgotPasswordText : "");
            cmd.Parameters.AddWithValue("@ShowSignup", gateway.ShowSignup);
            cmd.Parameters.AddWithValue("@SignupText", gateway.ShowSignup ? gateway.SignupText : "");
            cmd.Parameters.AddWithValue("@SignupURL", gateway.ShowSignup ? gateway.SignupURL : "");
            cmd.Parameters.AddWithValue("@SubmitText", gateway.SubmitText);
            cmd.Parameters.AddWithValue("@UseStyleFrom", gateway.UseStyleFrom);
            cmd.Parameters.AddWithValue("@Style", gateway.Style);
            cmd.Parameters.AddWithValue("@UseConfirmation", gateway.UseConfirmation);
            cmd.Parameters.AddWithValue("@UseRedirect", gateway.UseRedirect);
            cmd.Parameters.AddWithValue("@RedirectURL", gateway.RedirectURL);
            cmd.Parameters.AddWithValue("@RedirectDelay", gateway.RedirectDelay);
            cmd.Parameters.AddWithValue("@ConfirmationMessage", gateway.ConfirmationMessage);
            cmd.Parameters.AddWithValue("@ConfirmationText", gateway.ConfirmationText);
            cmd.Parameters.AddWithValue("@LoginOrCapture", gateway.LoginOrCapture);
            cmd.Parameters.AddWithValue("@ValidateEmail", gateway.ValidateEmail);
            cmd.Parameters.AddWithValue("@ValidatePassword", gateway.ValidatePassword);
            cmd.Parameters.AddWithValue("@ValidateCustom", gateway.ValidateCustom);
            if (gateway.GatewayID > 0)
                cmd.Parameters.AddWithValue("@UpdatedUserID", gateway.UpdatedUserID);
            else
                cmd.Parameters.AddWithValue("@CreatedUserID", gateway.CreatedUserID);
            cmd.Parameters.AddWithValue("@IsDeleted", gateway.IsDeleted);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());

        }

        public static ECN_Framework_Entities.Communicator.Gateway GetByPubCode_TypeCode(string PubCode, string TypeCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_Gateway_Select_PubCode_TypeCode";
            cmd.Parameters.AddWithValue("@PubCode", PubCode);
            cmd.Parameters.AddWithValue("@TypeCode", TypeCode);

            return Get(cmd);
        }

        public static bool Exists(int GatewayID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_Gateway_Exists";
            cmd.Parameters.AddWithValue("@GatewayID", GatewayID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static bool Exists_PubCode_TypeCode(string PubCode, string TypeCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_Gateway_Exists_PubCode_TypeCode";
            cmd.Parameters.AddWithValue("@PubCode", PubCode);
            cmd.Parameters.AddWithValue("@TypeCode", TypeCode);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        private static List<ECN_Framework_Entities.Communicator.Gateway> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Gateway> retList = new List<ECN_Framework_Entities.Communicator.Gateway>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Gateway retItem = new ECN_Framework_Entities.Communicator.Gateway();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Gateway>.CreateBuilder(rdr);
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

            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        private static ECN_Framework_Entities.Communicator.Gateway Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.Gateway retItem = new ECN_Framework_Entities.Communicator.Gateway();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {

                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Gateway>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);

                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }

            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }
    }
}
