using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public class PaidForm
    {
        public int PaidFormID { get; set; }
        public string FormName { get; set; }
        public string HeaderHTML { get; set; }
        public string FooterHTML { get; set; }
        public string ThankYouHTML { get; set; }
        public bool ShowQuantity { get; set; }
        public string PaymentGateway { get; set; }
        public bool ShowCountry { get; set; }
        public bool AllowPromoCode { get; set; }
        public int CustomerID { get; set; }
        public int QuantityAllowed { get; set; }
        public bool UseTestMode { get; set; }
        public string AuthorizeDotNetKey { get; set; }
        public string AuthorizeDotNetLogin { get; set; }
        public string FormRedirect { get; set; }
        public string PayflowAccount { get; set; }
        public string PayflowPassword { get; set; }
        public string PayflowSignature { get; set; }
        public string PayflowPartner { get; set; }
        public string PayflowVendor { get; set; }


        public static PaidForm GetByPaidFormID(int PaidFormID)
        {
            //DataTable dt = DataFunctions.GetDataTable(cmd);

            PaidForm retItem = new PaidForm();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from PaidForm  with (NOLOCK) where PaidFormID=@PaidFormID";
            cmd.Parameters.Add(new SqlParameter("@PaidFormID", SqlDbType.Int)).Value = PaidFormID;

            SqlDataReader rdr = DataFunctions.ExecuteReader(cmd);
            DynamicBuilder<PaidForm> builder = DynamicBuilder<PaidForm>.CreateBuilder(rdr);

            while (rdr.Read())
            {
                retItem = builder.Build(rdr);
            }
            rdr.Close();
            rdr.Dispose();
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }
    }
}
