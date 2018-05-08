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

namespace KMPS.MD.Tools
{
    public partial class geocode : BrandsPageBase
    {
        protected override Panel BrandPanel => pnlBrand;
        protected override DropDownList BrandDropDown => drpBrand;
        protected override HiddenField BrandIdHiddenField => hfBrandID;
        protected override Label BrandNameLabel => lblBrandName;

        delegate void RebuildSubscriberList();

        delegate void HidePanel();

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.Menu = "Tools";
            Master.SubMenu = "GeoCoding";

            RebuildSubscriberList delNoParam = new RebuildSubscriberList(Reload);
            this.DownloadPanel1.DelMethod = delNoParam;

            HidePanel delNoParam1 = new HidePanel(Hide);
            this.DownloadPanel1.hideDownloadPopup = delNoParam1;

            if (!IsPostBack)
            {
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.GeoCode, KMPlatform.Enums.Access.View))
                {
                    Response.Redirect("../SecurityAccessError.aspx");
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.GeoCode, KMPlatform.Enums.Access.Edit))
                {
                    btnSave.Visible = false;
                }

                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.GeoCode, KMPlatform.Enums.Access.Delete))
                {
                    btnDelReport.Visible = false;
                }

                //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(),
                //      "var interval = setInterval(function(){if ((eval(\"typeof VEMap\") != \"undefined\") && (document.getElementById(\"myMap\").attachEvent != undefined))  " +
                //      "{clearInterval(interval); GetMap();}}, 10);", true);

                GetStates();

                LoadBrands();
                
                DownloadPanel1.Showexporttoemailmarketing = true;
                DownloadPanel1.Showsavetocampaign = true;
                DownloadPanel1.error = false;
                DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
            }
        }

        protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControls();
            drpdownLocations.Items.Clear();

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "removepolygon", "removepolygon();", true);                     

            imglogo.ImageUrl = "";
            imglogo.Visible = false;
            hfBrandID.Value = drpBrand.SelectedValue;

            if (Convert.ToInt32(drpBrand.SelectedValue) >= 0)
            {
                if (Convert.ToInt32(drpBrand.SelectedValue) > 0)
                {
                    Brand b = Brand.GetByID(Master.clientconnections, Convert.ToInt32(drpBrand.SelectedValue));
                    if (b != null)
                    {
                        if (b.Logo != string.Empty)
                        {
                            int customerID = Master.UserSession.CustomerID;
                            imglogo.ImageUrl = "../Images/logo/" + customerID + "/" + b.Logo;
                            imglogo.Visible = true;
                        }
                    }
                }

                loadsavedlocations();
            }

            DownloadPanel1.BrandID = Convert.ToInt32(hfBrandID.Value);
        }

        protected void DropDownListCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetStates();
            RadMtxtboxZipCode.Text = "";
            if(DropDownListCountry.SelectedValue.Equals("UNITED STATES", StringComparison.OrdinalIgnoreCase))
                RadMtxtboxZipCode.Mask = "#####";
            else
                RadMtxtboxZipCode.Mask = "L#L #L#";
        }

        private void GetStates()
        {
            DropDownListState.Items.Clear();
            DropDownListState.DataSource = KMPS.MD.Objects.Region.GetByCountry(DropDownListCountry.SelectedValue);
            DropDownListState.DataBind();
            DropDownListState.Items.Insert(0, new ListItem("-Select-", "*"));
        }

        private void Reload()
        {
            if (hfselected.Value.Equals(""))
            {
                DataTable dt = new DataTable();
                DownloadPanel1.SubscriptionID = null;
                DownloadPanel1.SubscribersQueries = HeatMapLocations.GetSelectedSubscribersQueriesByGL(Master.clientconnections, Double.Parse(minLatHiddenField.Value), Double.Parse(maxLatHiddenField.Value), Double.Parse(minLonHiddenField.Value), Double.Parse(maxLonHiddenField.Value), Double.Parse(MyLocationLatitude.Value), Double.Parse(MyLocationLongitude.Value), Convert.ToInt32(textboxRadius.Text), Convert.ToInt32(hfBrandID.Value));
            }
            else
            {
                StringBuilder latitude = new StringBuilder();
                StringBuilder longitude = new StringBuilder();
                string[] locations = hfselected.Value.Split(':');
                for (int i = 0; i < locations.Count() - 1; i++)
                {
                    string[] latlongs = locations[i].Split('|');

                    if (latitude.Length > 0)
                        latitude.Append("," + latlongs[0]);
                    else
                        latitude.Append(latlongs[0]);

                    if (longitude.Length > 0)
                        longitude.Append("," + latlongs[1]);
                    else
                        longitude.Append(latlongs[1]);
                }

                DownloadPanel1.SubscriptionID = null;
                DownloadPanel1.SubscribersQueries = HeatMapLocations.GetSelectedSubscribersQueries(latitude, longitude);
            }        
        }

        private void Hide()
        {
            DownloadPanel1.Visible = false;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            try
            {
                BindMap(textboxAddress.Text, textboxCity.Text, DropDownListState.SelectedValue, RadMtxtboxZipCode.TextWithLiterals, textboxRadius.Text, Convert.ToInt32(hfBrandID.Value));
            }
            catch (Exception ex)
            {
                lbMsg.Text = ex.Message;

                btnDownload.Visible = false;
            }
        }

        protected void SaveLocation_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.GeoCode, KMPlatform.Enums.Access.Edit))
            {
                Location mylocation = new Location();
                mylocation.Street = textboxAddress.Text;
                mylocation.City = textboxCity.Text;
                mylocation.Region = DropDownListState.SelectedValue;
                mylocation.PostalCode = RadMtxtboxZipCode.TextWithLiterals;
                mylocation = Location.ValidateBingAddress(mylocation, DropDownListCountry.SelectedValue);

                if (mylocation.IsValid)
                {
                    txtLocationName.Text = "";
                    lbMsg.Text = "";
                    this.mdlPopSave.Show();
                }
                else
                {
                    lbMsg.Text = "Please verify the address entered";
                }
            }
        }

        protected void btnPopupSaveLocation_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.GeoCode, KMPlatform.Enums.Access.Edit))
            {
                HeatMapLocations h = new HeatMapLocations();
                h.Address = textboxAddress.Text;
                h.State = DropDownListState.SelectedValue;
                h.CreatedBy = Master.LoggedInUser;
                h.City = textboxCity.Text;
                h.Zip = RadMtxtboxZipCode.TextWithLiterals;
                h.Radius = textboxRadius.Text;
                h.LocationName = txtLocationName.Text;
                h.BrandID = Convert.ToInt32(hfBrandID.Value);

                HeatMapLocations.SaveLocation(Master.clientconnections, h);

                this.mdlPopSave.Hide();
                loadsavedlocations();
            }
        }

        private void DetailsDownloadByGL(int DownloadCount)
        {
            if (DownloadCount > 0)
            {
                DataTable dt = new DataTable();
                dt = GetSubscriberByGL(Double.Parse(minLatHiddenField.Value), Double.Parse(maxLatHiddenField.Value), Double.Parse(minLonHiddenField.Value), Double.Parse(maxLonHiddenField.Value), Double.Parse(MyLocationLatitude.Value), Double.Parse(MyLocationLongitude.Value), Convert.ToInt32(textboxRadius.Text), Convert.ToInt32(hfBrandID.Value));
                DownloadPanel1.SubscriptionID = null;
                DownloadPanel1.SubscribersQueries = HeatMapLocations.GetSelectedSubscribersQueriesByGL(Master.clientconnections, Double.Parse(minLatHiddenField.Value), Double.Parse(maxLatHiddenField.Value), Double.Parse(minLonHiddenField.Value), Double.Parse(maxLonHiddenField.Value), Double.Parse(MyLocationLatitude.Value), Double.Parse(MyLocationLongitude.Value), Convert.ToInt32(textboxRadius.Text), Convert.ToInt32(hfBrandID.Value));
                DownloadPanel1.Visible = true;
                DownloadPanel1.downloadCount = DownloadCount;
                DownloadPanel1.ValidateExportPermission();
                DownloadPanel1.LoadControls();
                DownloadPanel1.LoadDownloadTemplate();
                DownloadPanel1.loadExportFields();
            }
            else
            {
                DownloadPanel1.error = true;
                DownloadPanel1.errormsg = "Download Count must be greater than 0";
                DownloadPanel1.Visible = true;
                DownloadPanel1.isError();
            }
        }

        private void DetailsDownload(int DownloadCount)
        {
            if (DownloadCount > 0)
            {
                StringBuilder latitude = new StringBuilder();
                StringBuilder longitude = new StringBuilder();
                string[] locations = hfselected.Value.Split(':');
                for (int i = 0; i <= locations.Count() - 1; i++)
                {
                    string[] latlongs = locations[i].Split('|');

                    if (latitude.Length > 0)
                        latitude.Append("," + latlongs[0]);
                    else
                        latitude.Append(latlongs[0]);

                    if (longitude.Length > 0)
                        longitude.Append("," + latlongs[1]);
                    else
                        longitude.Append(latlongs[1]);
                }

                DownloadPanel1.SubscriptionID = null;
                DownloadPanel1.SubscribersQueries = HeatMapLocations.GetSelectedSubscribersQueries(latitude, longitude);
                DownloadPanel1.Visible = true;
                DownloadPanel1.downloadCount = DownloadCount;
                DownloadPanel1.ValidateExportPermission();
                DownloadPanel1.LoadControls();
                DownloadPanel1.LoadDownloadTemplate();
                DownloadPanel1.loadExportFields();
            }
            else
            {
                DownloadPanel1.error = true;
                DownloadPanel1.errormsg = "Download Count must be greater than 0";
                DownloadPanel1.Visible = true;
                DownloadPanel1.isError();
            }
        }

        private void BindMap(string streetaddress, string city, string state, string zip, string selradius, int brandID)
        {
            try
            {
                drpdownLocations.SelectedValue = "*";
                //Validate Address
                Location mylocation = new Location();
                if ((streetaddress != string.Empty) || (city != string.Empty) || (!state.Equals("*")))
                {
                    mylocation.Street = streetaddress;
                    mylocation.City = city;
                    mylocation.Region = state;
                }
                mylocation.PostalCode = zip;
                mylocation = Location.ValidateBingAddress(mylocation, DropDownListCountry.SelectedValue);

                if (mylocation.IsValid)
                {
                    string address = mylocation.Street + " " + mylocation.City + " " + mylocation.Region;
                    Double radiusLatTotal = Convert.ToDouble(textboxRadius.Text) / 69D;
                    

                    Double PI_180 = Math.PI / 180D;

                    Double latitude = Convert.ToDouble(mylocation.Latitude);
                    Double longitude = Convert.ToDouble(mylocation.Longitude);

                    Double minLat = latitude - radiusLatTotal;
                    Double maxLat = latitude + radiusLatTotal;

                    Double minLon = longitude - (radiusLatTotal / Math.Cos(maxLat * PI_180));
                    Double maxLon = longitude + (radiusLatTotal / Math.Cos(minLat * PI_180));

                    //Double minLon = longitude - (Convert.ToDouble(textboxRadius.Text) / Math.Abs(Math.Cos(latitude * PI_180) * 69D));
                    //Double maxLon = longitude + (Convert.ToDouble(textboxRadius.Text) / Math.Abs(Math.Cos(latitude * PI_180) * 69D));

                    MyLocationLatitude.Value = latitude.ToString();
                    MyLocationLongitude.Value = longitude.ToString();
                    //Get Subscriber Data
                    DataTable subscriberData = new DataTable();
                    subscriberData = GetSubscriberByGL(minLat, maxLat, minLon, maxLon, latitude, longitude, Convert.ToInt32(textboxRadius.Text), brandID);
                    DataTable myStops = GetMap(minLat, maxLat, minLon, maxLon, address, mylocation.Latitude, mylocation.Longitude, mylocation.PostalCode, subscriberData);
                    DataTable subscriberCount=  SetSubcribersByRadiusCount(mylocation.Latitude, mylocation.Longitude, subscriberData, Int32.Parse(selradius));
                    gvSubscibersByRadius.DataSource =subscriberCount;
                    gvSubscibersByRadius.DataBind();

                    double radius = 1.6 * Convert.ToDouble(selradius);
                    if (myStops.Rows.Count > 0)
                    {
                      
                        string dt = GetJSONString(myStops);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "loadMap", "loadMap(" + dt + "," + mylocation.Latitude + "," + mylocation.Longitude + "," + radius + ");", true);                     
                        lbMsg.Text = "";
                        btnDownload.Visible = true;
                        gvSubscibersByRadius.Visible = true;

                    }
                    else
                    {
                        gvSubscibersByRadius.Visible = false;
                        btnDownload.Visible = false;
                    }
                }
                else
                {
                    lbMsg.Text = "Please verify the address entered";
                    btnDownload.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        private DataTable SetSubcribersByRadiusCount(double actLat, double actLong, DataTable dt, int radius)
        {
            DataTable result = new DataTable();
           
                int loops = 0;
                int interval = 0;
                if (radius<11)
                {
                    interval = 5;
                }
                if ((radius > 10) && (radius < 51))
                {
                    interval = 10;
                }
                else if ((radius > 50) && (radius < 101))
                {
                    interval = 20;
                }
                else if ((radius > 100) && (radius < 201))
                {
                    interval = 50;
                }
                else if (radius > 200)
                {
                    interval = 100;
                }

                loops = DivideRoundingUp(radius, interval);

                result.Columns.Add("Range", typeof(string));
                result.Columns.Add("Subscribers", typeof(string));
                int upperLimit = 0;
                int lowerLimit = 0;
                int prev = 0;
                int counts = 0;
                for (int i = 0; i <= loops; i++)
                {
                    DataRow dr = result.NewRow();
                    if (i < loops)
                    {
                        lowerLimit = upperLimit;
                        if (i == loops - 1)
                        {
                            upperLimit = radius;
                        }
                        else
                        {
                            upperLimit = interval * (i + 1);
                        }
                        counts = GetSubcribersByRadiusCount(dt, actLat, actLong, upperLimit); 
                        dr[0] = lowerLimit.ToString() + "-" + upperLimit.ToString();
                        dr[1] = counts - prev;
                        prev = counts;
                    }
                    else
                    {
                        dr[0] = "<b>Total Subscribers</b>";
                        dr[1] = "<b>" + counts.ToString() + "</b>";
                    }
                    result.Rows.Add(dr);
                    result.AcceptChanges();
                }              
          
            return result;
        }

        private int GetSubcribersByRadiusCount(DataTable dt, double actLat, double actLong, int radius)
        {
            Double radiusLatTotal = Convert.ToDouble(radius) / 69D;
            Double PI_180 = Math.PI / 180D;

            Double salonLat = Convert.ToDouble(actLat);
            Double salonLon = Convert.ToDouble(actLong);

            Double minLat = salonLat - radiusLatTotal;
            Double maxLat = salonLat + radiusLatTotal;
            Double minLon = salonLon + (radiusLatTotal / Math.Cos(minLat * PI_180));
            Double maxLon = salonLon - (radiusLatTotal / Math.Cos(maxLat * PI_180));

            int count = 0;
            if ((minLat < maxLat) && (minLon < maxLon))
            {
                count = dt.AsEnumerable()
                        .Count(row => row.Field<decimal>("Latitude") > Convert.ToDecimal(minLat)
                                   && row.Field<decimal>("Latitude") < Convert.ToDecimal(maxLat)
                                   && row.Field<decimal>("Longitude") > Convert.ToDecimal(minLon)
                                   && row.Field<decimal>("Longitude") < Convert.ToDecimal(maxLon));

            }
            else if ((minLat > maxLat) && (minLon < maxLon))
            {
                count = dt.AsEnumerable()
                      .Count(row => row.Field<decimal>("Latitude") < Convert.ToDecimal(minLat)
                                 && row.Field<decimal>("Latitude") > Convert.ToDecimal(maxLat)
                                 && row.Field<decimal>("Longitude") > Convert.ToDecimal(minLon)
                                 && row.Field<decimal>("Longitude") < Convert.ToDecimal(maxLon));

            }
            else if ((minLat < maxLat) && (minLon > maxLon))
            {
                count = dt.AsEnumerable()
                      .Count(row => row.Field<decimal>("Latitude") > Convert.ToDecimal(minLat)
                                 && row.Field<decimal>("Latitude") < Convert.ToDecimal(maxLat)
                                 && row.Field<decimal>("Longitude") < Convert.ToDecimal(minLon)
                                 && row.Field<decimal>("Longitude") > Convert.ToDecimal(maxLon));

            }
            else if ((minLat > maxLat) && (minLon > maxLon))
            {
                count = dt.AsEnumerable()
                      .Count(row => row.Field<decimal>("Latitude") < Convert.ToDecimal(minLat)
                                 && row.Field<decimal>("Latitude") > Convert.ToDecimal(maxLat)
                                 && row.Field<decimal>("Longitude") < Convert.ToDecimal(minLon)
                                 && row.Field<decimal>("Longitude") > Convert.ToDecimal(maxLon));
            }
            return count;
        }

        public int DivideRoundingUp(int x, int y)
        {
            int remainder;
            int quotient = Math.DivRem(x, y, out remainder);
            return remainder == 0 ? quotient : quotient + 1;
        }
         

        public string GetJSONString(DataTable Dt)
        {
            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;

            for (int i = 0; i < Dt.Columns.Count; i++)
            {

                StrDc[i] = Dt.Columns[i].Caption;

                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();
            Sb.Append("{\"" + Dt.TableName + "\" : [");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                string TempStr = HeadStr;
                Sb.Append("{");

                for (int j = 0; j < Dt.Columns.Count; j++)
                {

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
                }

                Sb.Append(TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]}");

            return Sb.ToString();
        }

        private DataTable GetSubscriberByGL(double minLat, double maxLat, double minLon, double maxLon, double Latitude, double Longitude, int RadiusMax, int brandID)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(Master.clientconnections);
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberByGL", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MinLat", minLat);
            cmd.Parameters.AddWithValue("@MaxLat", maxLat);
            cmd.Parameters.AddWithValue("@MinLon", minLon);
            cmd.Parameters.AddWithValue("@MaxLon", maxLon);
            cmd.Parameters.AddWithValue("@Latitude", Latitude);
            cmd.Parameters.AddWithValue("@Longitude", Longitude);
            cmd.Parameters.AddWithValue("@RadiusMax", RadiusMax);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            dt = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(Master.clientconnections));
            return dt;
        }

        private DataTable GetSubscriberCountByRadius(double minLat100, double maxLat100, double minLon100, double maxLon100,
            double minLat200, double maxLat200, double minLon200, double maxLon200,
            double minLat300, double maxLat300, double minLon300, double maxLon300)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(Master.clientconnections);
            SqlCommand cmd = new SqlCommand("sp_GetSubscriberCountByRadius", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MinLat100", minLat100);
            cmd.Parameters.AddWithValue("@MaxLat100", maxLat100);
            cmd.Parameters.AddWithValue("@MinLon100", minLon100);
            cmd.Parameters.AddWithValue("@MaxLon100", maxLon100);

            cmd.Parameters.AddWithValue("@MinLat200", minLat200);
            cmd.Parameters.AddWithValue("@MaxLat200", maxLat200);
            cmd.Parameters.AddWithValue("@MinLon200", minLon200);
            cmd.Parameters.AddWithValue("@MaxLon200", maxLon200);
            
            cmd.Parameters.AddWithValue("@MinLat300", minLat300);
            cmd.Parameters.AddWithValue("@MaxLat300", maxLat300);
            cmd.Parameters.AddWithValue("@MinLon300", minLon300);
            cmd.Parameters.AddWithValue("@MaxLon300", maxLon300);

            dt = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(Master.clientconnections));
            return dt;

        }

        public DataTable GetMap(double minLat, double maxLat, double minLon, double maxLon, string address, double actLat, double actLong, string actZip, DataTable mapsdata)
        {
            minLatHiddenField.Value = minLat.ToString();
            maxLatHiddenField.Value = maxLat.ToString();
            minLonHiddenField.Value = minLon.ToString();
            maxLonHiddenField.Value = maxLon.ToString();

            DataTable dt = new DataTable("MapPoints");
            dt = mapsdata.Copy();
            countsHiddenField.Value = dt.Rows.Count.ToString();

         
            var  addresspoints = (from src in dt.AsEnumerable()
                                            group src by new 
                                            {                                            
                                                Latitude=src.Field<decimal>("Latitude"),
                                                Longitude = src.Field<decimal>("Longitude"),
                                            }
                                            into grp1              
                                            select new
                                                 {
                                                    SubscriberID = grp1.Count(),
                                                    Latitude = grp1.Key.Latitude,
                                                    Longitude = grp1.Key.Longitude
                                                 });

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("SubscriberID");
            dt1.Columns.Add("Latitude");
            dt1.Columns.Add("Longitude");
            foreach (var item in addresspoints)
            {
                dt1.Rows.Add(item.SubscriberID, item.Latitude, item.Longitude);
            }   

            string blue = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/KMPS.MD/Images/blue-pin.png";
            string red = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/KMPS.MD/Images/red-pin.png";
            List<MapItem> listMap = new List<MapItem>();
            foreach (DataRow dr in dt1.Rows)
            {
                MapItem mi = new MapItem();
                //mi.SubscriberID = Convert.ToInt32(dr["SubscriberID"].ToString());
                mi.SubscriberID = dr["SubscriberID"].ToString();
                // mi.MapAddress = dr["Address"].ToString();
                //mi.Latitude = TruncateDecimal(Convert.ToDecimal(dr["Latitude"].ToString()), 6);
                //mi.Longitude = TruncateDecimal(Convert.ToDecimal(dr["Longitude"].ToString()), 6);
                mi.Latitude = Convert.ToDecimal(dr["Latitude"].ToString());
                mi.Longitude = Convert.ToDecimal(dr["Longitude"].ToString());
                mi.MarkerImage = blue;
               // mi.ZipCode = dr["ZIP"].ToString();
                listMap.Add(mi);
            }

            MapItem centerpoint = new MapItem();
            //centerpoint.SubscriberID = Convert.ToInt32(actZip);
            centerpoint.SubscriberID = actZip;
            centerpoint.MapAddress = address;
            centerpoint.Latitude = Convert.ToDecimal(actLat);
            centerpoint.Longitude = Convert.ToDecimal(actLong);
            centerpoint.MarkerImage = red;
            centerpoint.ZipCode = actZip;
            listMap.Add(centerpoint);

            listMap = listMap.OrderBy(x => x.ZipCode).ToList();          
            DataTable dtNew = MapStops(listMap);
            return dtNew;
        }

        public decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }

        private DataTable MapStops(List<MapItem> items)
        {
            //SubscriberID, MapAddress, Latitude, Longitude, '/Images/blue-pin.png' AS 'markerImage', ZipCode 
            DataTable dtNew = new DataTable();
            DataColumn dcSubscriberID = new DataColumn("Sc");
            dtNew.Columns.Add(dcSubscriberID);
            //DataColumn dcMapAddress = new DataColumn("MapAddress");
            //dtNew.Columns.Add(dcMapAddress);
            DataColumn dcLatitude = new DataColumn("Lt");
            dtNew.Columns.Add(dcLatitude);
            DataColumn dcLongitude = new DataColumn("Lg");
            dtNew.Columns.Add(dcLongitude);
            DataColumn dcmarkerImage = new DataColumn("MI");
            dtNew.Columns.Add(dcmarkerImage);
            //DataColumn dcZipCode = new DataColumn("ZipCode");
            //dtNew.Columns.Add(dcZipCode);
            dtNew.AcceptChanges();
            foreach (MapItem mi in items)
            {
                DataRow dr = dtNew.NewRow();
                dr["Sc"] = mi.SubscriberID;
                //dr["MapAddress"] = mi.MapAddress;
                dr["Lt"] = mi.Latitude;
                dr["Lg"] = mi.Longitude;
                dr["MI"] = mi.MarkerImage;
                //dr["ZipCode"] = mi.ZipCode;
                dtNew.Rows.Add(dr);
            }
            dtNew.AcceptChanges();

            StringBuilder sb = new StringBuilder();
            Dictionary<string, object> resultMain = new Dictionary<string, object>();
            int index = 0;
            foreach (DataRow dr in dtNew.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dtNew.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                resultMain.Add(index.ToString(), result);
                index++;
            }
            dtNew.TableName = "MapPoints";

            return dtNew;
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (hfselected.Value.Equals(""))
                DetailsDownloadByGL(Int32.Parse(countsHiddenField.Value));
            else
            {
                StringBuilder sb = new StringBuilder();
                string[] locations = hfselected.Value.Split(':');
                for (int i = 0; i < locations.Count(); i++)
                {
                    string[] latlongs = locations[i].Split('|');
                    sb.AppendLine("<Locations Latitude=\"" + latlongs[0] + "\" Longitude=\"" + latlongs[1] + "\"/>");
                }
                DataTable dt = new DataTable();
                dt = KMPS.MD.Objects.HeatMapLocations.GetSelectedSubscribers(Master.clientconnections, sb.ToString());
                DetailsDownload(dt.Rows.Count);
            }
        }

        protected void btnUndo_Click(object sender, EventArgs e)
        {
            Response.Redirect("GeoCode.aspx?brandID=" + Convert.ToInt32(hfBrandID.Value));
        }

        protected void ResetControls()
        {
            btnDownload.Visible = false;
            textboxAddress.Text = "";
            textboxCity.Text = "";
            DropDownListCountry.ClearSelection();
            RadMtxtboxZipCode.Mask = "#####";
            GetStates();
            RadMtxtboxZipCode.Text = "";
            textboxRadius.Text = "";
            gvSubscibersByRadius.DataSource = null;
            gvSubscibersByRadius.DataBind();
            gvSubscibersByRadius.Visible = false;
            hfselected.Value = "";
            minLatHiddenField.Value = "";
            maxLatHiddenField.Value = "";
            minLonHiddenField.Value = "";
            MyLocationLatitude.Value = string.Empty;
            maxLonHiddenField.Value = "";
            MyLocationLatitude.Value = string.Empty;
            MyLocationLongitude.Value = string.Empty;
        }

        protected void btnLoadLocation_Click(object sender, EventArgs e)
        {
            DataTable dt = DataFunctions.getDataTable("select Address, City, State, Zip, Radius from HeatMapLocations where LocationID='" + drpdownLocations.SelectedValue + "'", DataFunctions.GetClientSqlConnection(Master.clientconnections));
            textboxAddress.Text = dt.Rows[0]["Address"].ToString();
            textboxCity.Text = dt.Rows[0]["City"].ToString();

            string country = KMPS.MD.Objects.Region.GetByRegionCode(dt.Rows[0]["State"].ToString()).Country;

            DropDownListCountry.SelectedValue = country;

            if (DropDownListCountry.SelectedValue.Equals("UNITED STATES", StringComparison.OrdinalIgnoreCase))
                RadMtxtboxZipCode.Mask = "#####";
            else
                RadMtxtboxZipCode.Mask = "L#L #L#";

            GetStates();

            DropDownListState.SelectedValue = dt.Rows[0]["State"].ToString();
            RadMtxtboxZipCode.Text = dt.Rows[0]["Zip"].ToString();
            textboxRadius.Text = dt.Rows[0]["Radius"].ToString();
            BindMap(dt.Rows[0]["Address"].ToString(), dt.Rows[0]["City"].ToString(), dt.Rows[0]["State"].ToString(), dt.Rows[0]["Zip"].ToString(), dt.Rows[0]["Radius"].ToString(), Convert.ToInt32(hfBrandID.Value));

        }

        protected void btnDelLocation_Click(object sender, EventArgs e)
        {
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.GeoCode, KMPlatform.Enums.Access.Delete))
            {
                KMPS.MD.Objects.HeatMapLocations.DeleteLocation(Master.clientconnections, Int32.Parse(drpdownLocations.SelectedValue));
                loadsavedlocations();
            }
        }

        private void loadsavedlocations()
        {
            drpdownLocations.Items.Clear();

            List<HeatMapLocations> hmp = new List<HeatMapLocations>();

            if (Convert.ToInt32(hfBrandID.Value) > 0)
                hmp = HeatMapLocations.GetByBrandID(Master.clientconnections, Convert.ToInt32(hfBrandID.Value));
            else
                hmp = HeatMapLocations.GetNotInBrand(Master.clientconnections);

            if (hmp.Count > 0)
            {
                drpdownLocations.DataSource = hmp;
                drpdownLocations.DataValueField = "LocationID";
                drpdownLocations.DataTextField = "LocationName";
                drpdownLocations.DataBind();
                ListItem item = new ListItem("-Select-", "*");
                drpdownLocations.Items.Insert(0, item);
            }
        }

        protected override void LoadPageFilters()
        {
            loadsavedlocations();
        }

        protected override void ShowBrandUI(Brand brand)
        {
            lblColon.Visible = true;
            
            if (brand.Logo != string.Empty)
            {
                var customerID = Master.UserSession.CustomerID;
                imglogo.ImageUrl = $"../Images/logo/{customerID}/{brand.Logo}";
                imglogo.Visible = true;
            }
        }
    }
}