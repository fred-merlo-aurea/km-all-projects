using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using KMPS_JF_Objects.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SourceMediaPaidPub.Objects
{

	public class Magazine
	{
		[JsonProperty("SourceMediaPubCodes")]
		public List<SourcemediaPubcode> SourcemediaPubcodes { get; set; }
	}

	public class SourcemediaPubcode
	{
		
		public string PubCode { get; set; }
		public string GroupID { get; set; }
		public string CustomerID { get; set; }
		public string Title { get; set; }
		public string Term { get; set; }
		
		[JsonProperty("HasPremium")]
		public bool HasPremium { get; set; }

        [JsonProperty("ThankYouHTML")]
        public string ThankYouHTML { get; set; }

		[JsonProperty("States")]
		public string States { get; set; }
		[JsonProperty("Pricerange")]
		public IList<PriceRange> PriceRanges { get; set; }

		[JsonProperty("StandardCoverImage")]
		public string StandardCoverImage { get; set; }
		[JsonProperty("PremiumCoverImage")]
		public string PremiumCoverImage { get; set; }

		[JsonProperty("PaymentGateway")]
		public string PaymentGateway { get; set; }

		[JsonProperty("PremiumPubCodes")]
		public List<PremiumPubCode> PremiumPubCodes { get; set; }

		public string Name { get; private set; }

		public bool IsSubscriptionRenewal(int CustomerID, string EmailAddress, int GroupID)
		{
			SqlCommand cmd = new SqlCommand("sp_IsSubscriptionRenewal");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int)).Value = CustomerID;
			cmd.Parameters.Add(new SqlParameter("@EmailAddress", SqlDbType.VarChar)).Value = EmailAddress;
			cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = GroupID;
			DataTable dt = DataFunctions.GetDataTable(cmd);
			return Convert.ToInt32(dt.Rows[0][0].ToString()) == 1 ? true : false;
		}
	}

	public class PremiumPubCode
	{
		public string PubCode { get; set; }
		public string GroupID { get; set; }
		public string CustomerID { get; set; }
		public double Price { get; set; }

		[JsonProperty("Term")]
		public string Term { get; set; }
	}
}