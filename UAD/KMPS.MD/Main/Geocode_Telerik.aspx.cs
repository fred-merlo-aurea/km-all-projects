using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Security;
using System.Collections.Generic;
using KMPS.MD.Objects;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using Telerik.Web.UI;

namespace KMPS.MD.Main
{
    public partial class Geocode_Telerik : System.Web.UI.Page
    {

        private static string TOOLTIP_TEMPLATE = @"

               <div class=""leftCol"">

                    <div class=""flag flag-{0}""></div>

               </div>

               <div class=""rightCol"">

                    <div class=""country"">{1}</div>

                    <div class=""city"">{2}</div>

                    <div class=""address"">{3}</div>

               </div>";

 

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            try
            {
                BindMap_Telerik("5446 vinewood lane n", "Plymouth", "MN", "55446", "10", 4);
            }
            catch (Exception ex)
            {
                //lbMsg.Text = ex.Message;

                
            }
        }

        private DataTable GetSubscriberByGL(double minLat, double maxLat, double minLon, double maxLon, int brandID)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DataFunctions.GetSqlConnection();
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberByGL", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MinLat", minLat);
            cmd.Parameters.AddWithValue("@MaxLat", maxLat);
            cmd.Parameters.AddWithValue("@MinLon", minLon);
            cmd.Parameters.AddWithValue("@MaxLon", maxLon);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            dt = DataFunctions.getDataTable(cmd, DataFunctions.GetSqlConnection());
            return dt;

        }

        private void BindMap_Telerik(string streetaddress, string city, string state, string zip, string selradius, int brandID)
        {
            try
            {
                //Validate Address
                Location mylocation = new Location();
                if ((streetaddress != string.Empty) || (city != string.Empty) || (!state.Equals("*")))
                {
                    mylocation.Street = streetaddress;
                    mylocation.City = city;
                    mylocation.Region = state;
                }
                mylocation.PostalCode = zip;
                mylocation = Location.ValidateBingAddress(mylocation);

                if (mylocation.IsValid)
                {
                    string address = mylocation.Street + " " + mylocation.City + " " + mylocation.Region;
                    Double radiusLatTotal = Convert.ToDouble(10) / 69D;
                    Double PI_180 = Math.PI / 180D;

                    Double salonLat = Convert.ToDouble(mylocation.Latitude);
                    Double salonLon = Convert.ToDouble(mylocation.Longitude);

                    Double minLat = salonLat - radiusLatTotal;
                    Double maxLat = salonLat + radiusLatTotal;

                    Double minLon = salonLon + (radiusLatTotal / Math.Cos(minLat * PI_180));
                    Double maxLon = salonLon - (radiusLatTotal / Math.Cos(maxLat * PI_180));

                    //Get Subscriber Data
                    DataTable subscriberData = new DataTable();
                    subscriberData = GetSubscriberByGL(minLat, maxLat, minLon, maxLon, brandID);
                    //DataTable myStops = GetMap(minLat, maxLat, minLon, maxLon, address, mylocation.Latitude, mylocation.Longitude, mylocation.PostalCode, subscriberData);

                    DataSet ds = new DataSet("TelerikOffices");
                    DataTable dt = new DataTable("TelerikOfficesTable");
                    dt.Columns.Add("Shape", Type.GetType("System.String"));
                    dt.Columns.Add("Country", Type.GetType("System.String"));
                    dt.Columns.Add("City", Type.GetType("System.String"));
                    dt.Columns.Add("Address", Type.GetType("System.String"));
                    dt.Columns.Add("Latitude", Type.GetType("System.Decimal"));
                    dt.Columns.Add("Longitude", Type.GetType("System.Decimal"));

                    foreach (DataRow dr in subscriberData.Rows)
                    {
                        dt.Rows.Add("PinTarget", dr["Country"].ToString(), dr["City"].ToString(), dr["Address"].ToString(), dr["Latitude"].ToString(), dr["Longitude"].ToString());
                    }

                    dt.Rows.Add("PinTarget", "UNITED STATES OF AMERICA", "Plymouth", "test1", 45.079419203102600, -93.507973998785000);
                    dt.Rows.Add("PinTarget", "UNITED STATES OF AMERICA", "MAPLE GROVE", "test2", 45.074399029836100, -93.441796274855700);
                    dt.Rows.Add("PinTarget", "UNITED STATES OF AMERICA", "GOLDEN VALLEY", "test3", 44.991818675771400, -93.390419567003800);
                    //dt.Rows.Add("PinTarget", "Germany", "INGELHEIM", "test4", 45.028090355917800, -93.378979023546000);
                    //dt.Rows.Add("PinTarget", "United States", "Palo Alto", "170 University Ave.<br />Palo Alto 94301", 37.444610, -122.163283);
                    //dt.Rows.Add("PinTarget", "United States", "Boston, MA", "201 Jones Rd Waltham<br />Boston MA 02451", 42.375067, -71.272233);
                    //dt.Rows.Add("PinTarget", "Denmark", "Copenhagen", "Vesterbrogade 149<br />Copenhagen DK-1620 Copenhagen V", 55.670312, 12.538266);
                    //dt.Rows.Add("PinTarget", "Australia", "Sydney", "Suite 705, 80 Mount St<br>Sydney North Sydney, NSW 2060", -33.838707, 151.207959);
                    //dt.Rows.Add("PinTarget", "Bulgaria", "Sofia", "33 Alexander Malinov Blvd.<br />Sofia 1729", 42.650613, 23.379025);
                    //dt.Rows.Add("PinTarget", "India", "Gurgaon", "Unit No 505, Tower A Spaze iTech<br />Park Gurgaon Sohna Road Sector 49<br />Gurgaon Haryana. 122002", 28.410139, 77.042439);
                    //dt.Rows.Add("PinTarget", "Germany", "Munich", "Balanstrasse 73<br />Munich 81541 Munich", 48.117227, 11.601990);
                    ds.Tables.Add(dt);

                    RadGeoCode.DataSource = dt;
                    RadGeoCode.DataBind();


                    //DataTable subscriberCount = SetSubcribersByRadiusCount(mylocation.Latitude, mylocation.Longitude, subscriberData, Int32.Parse(selradius));
                    //gvSubscibersByRadius.DataSource = subscriberCount;
                    //gvSubscibersByRadius.DataBind();

                    //double radius = 2.1 * Convert.ToDouble(selradius);
                    //if (myStops.Rows.Count > 0)
                    //{

                    //    string dt = GetJSONString(myStops);
                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadMap", "loadMap(" + dt + "," + mylocation.Latitude + "," + mylocation.Longitude + "," + radius + ");", true);
                    //    lbMsg.Text = "";
                    //    btnDownload.Visible = true;
                    //    gvSubscibersByRadius.Visible = true;

                    //}
                    //else
                    //{
                    //    gvSubscibersByRadius.Visible = false;
                    //    btnDownload.Visible = false;
                    //}
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        protected void RadMap1_ItemDataBound(object sender, Telerik.Web.UI.Map.MapItemDataBoundEventArgs e)
        {

            MapMarker marker = e.Item as MapMarker;

            if (marker != null)
            {

                DataRowView item = e.DataItem as DataRowView;

                string country = item.Row["Country"] as string;

                string city = item.Row["City"] as string;

                string address = item.Row["Address"] as string;

                marker.TooltipSettings.Content = String.Format(TOOLTIP_TEMPLATE, country.ToLower().Replace(" ", string.Empty), country, city, address);

            }

        }
    }
}