using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace ecn.webservices.client.WATTAPI
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            lblResult2.Text = "";
            lblResult1.Text = "";
        }

        protected void btnGetUDF_Click(object sender, EventArgs e)
        {
            ecn.webservices.client.WATTAPI_LOCAL.WATTAPI s = new ecn.webservices.client.WATTAPI_LOCAL.WATTAPI();

            try
            {
                var listGDF = s.GetUDFList(tbAccessKey.Text, drpPubCodes.SelectedItem.Value);
                dgUDFs.DataSource = listGDF; //.Where(x => x.ShortName != "Industry" && x.ShortName != "NonUSProvince" && x.ShortName != "EktronUserID" && x.ShortName != "SubscriberEmail").ToList();
                dgUDFs.DataBind();
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
        }


        protected void btnGetSubscriber_Click(object sender, EventArgs e)
        {
            string AccessKey;
            AccessKey = tbAccessKey.Text;

            ecn.webservices.client.WATTAPI_LOCAL.WATTAPI s = new ecn.webservices.client.WATTAPI_LOCAL.WATTAPI();

            ecn.webservices.client.WATTAPI_LOCAL.CustomerData cd = s.GetProfile(AccessKey, txtGetEmailaddress.Text, drpPubCodes.SelectedItem.Value);

            if (cd != null)
            {
                tbUserName.Text = cd.EktronUserName;
                if (cd.BirthDay.ToString() != "")
                    tbBirthDay.Text = cd.BirthDay.ToString();

                tbFirstName.Text = cd.FirstName;
                tbLastName.Text = cd.LastName;
                tbAddress1.Text = cd.AddressLine1;
                tbAddress2.Text = cd.AddressLine2;
                tbCity.Text = cd.City;
                tbState.Text = cd.State;
                tbCountry.Text = cd.Country;
                tbPostalCode.Text = cd.PostalCode;
                tbCompanyName.Text = cd.CompanyName;
                tbAge.Text = cd.Age.ToString();
                tbIncome.Text = cd.Income.ToString();
                tbTitle.Text = cd.Title;
                tbFullname.Text = cd.FullName;
                tbOccupation.Text = cd.Occupation;
                tbPhone.Text = cd.Phone;
                tbMobile.Text = cd.Mobile;
                tbFax.Text = cd.Fax;
                tbWebsite.Text = cd.Website;

                Hashtable hUDF = ToHashtable(cd.hUDF);


                rbSponsoredLinks.ClearSelection();
                rbSideRoads.ClearSelection();
                rbVideoAlert.ClearSelection();
                rbNonUSProvince.ClearSelection();
                rbWPOU_ENews.ClearSelection();
                rbAgribusiness.ClearSelection();
                rbDesignTips.ClearSelection();
                rbEquipmentTips.ClearSelection();
                rbFeed_ENews.ClearSelection();
                rbIA_Ciberboletin.ClearSelection();
                rbManagement_Tips.ClearSelection();
                rbPet_ENews.ClearSelection();
                rbPetfood_Nutrition.ClearSelection();
                rbPig_ENews.ClearSelection();
                rbShoptips.ClearSelection();
                rbTipSheet.ClearSelection();
                rbPoultryUpdate.ClearSelection();

                if (hUDF.Contains("SPONSOREDLINKS"))
                {
                    if (hUDF["SPONSOREDLINKS"].ToString().ToUpper() == "Y")
                        rbSponsoredLinks.Items.FindByValue("Y").Selected = true;
                    else
                        rbSponsoredLinks.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("SIDEROADS"))
                {
                    if (hUDF["SIDEROADS"].ToString().ToUpper() == "Y")
                        rbSideRoads.Items.FindByValue("Y").Selected = true;
                    else
                        rbSideRoads.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("VIDEOALERT"))
                {
                    if (hUDF["VIDEOALERT"].ToString().ToUpper() == "Y")
                        rbVideoAlert.Items.FindByValue("Y").Selected = true;
                    else
                        rbVideoAlert.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("INDUSTRY"))
                    tbIndustry.Text = hUDF["INDUSTRY"].ToString();

                if (hUDF.Contains("EKTRONUSERID"))
                    tbUserName.Text = hUDF["EKTRONUSERID"].ToString();

                if (hUDF.Contains("SUBSCRIBEREMAIL"))
                    tbEmail.Text = hUDF["SUBSCRIBEREMAIL"].ToString();

                if (hUDF.Contains("NONUSPROVINCE"))
                {
                    if (hUDF["NONUSPROVINCE"].ToString().ToUpper() == "Y")
                        rbNonUSProvince.Items.FindByValue("Y").Selected = true;
                    else
                        rbNonUSProvince.Items.FindByValue("N").Selected = true;
                }

                //MasterGroup newsletters

                if (hUDF.Contains("AGRIBUSINESS"))
                {
                    if (hUDF["AGRIBUSINESS"].ToString().ToUpper() == "Y")
                        rbAgribusiness.Items.FindByValue("Y").Selected = true;
                    else
                        rbAgribusiness.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("DESIGNTIPS"))
                {
                    if (hUDF["DESIGNTIPS"].ToString().ToUpper() == "Y")
                        rbDesignTips.Items.FindByValue("Y").Selected = true;
                    else
                        rbDesignTips.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("EQUIPMENTTIPS"))
                {
                    if (hUDF["EQUIPMENTTIPS"].ToString().ToUpper() == "Y")
                        rbEquipmentTips.Items.FindByValue("Y").Selected = true;
                    else
                        rbEquipmentTips.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("FEED_ENEWS"))
                {
                    if (hUDF["FEED_ENEWS"].ToString().ToUpper() == "Y")
                        rbFeed_ENews.Items.FindByValue("Y").Selected = true;
                    else
                        rbFeed_ENews.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("IA_CIBERBOLETIN"))
                {
                    if (hUDF["IA_CIBERBOLETIN"].ToString().ToUpper() == "Y")
                        rbIA_Ciberboletin.Items.FindByValue("Y").Selected = true;
                    else
                        rbIA_Ciberboletin.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("MANAGEMENT_TIPS"))
                {
                    if (hUDF["MANAGEMENT_TIPS"].ToString().ToUpper() == "Y")
                        rbManagement_Tips.Items.FindByValue("Y").Selected = true;
                    else
                        rbManagement_Tips.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("PET_ENEWS"))
                {
                    if (hUDF["PET_ENEWS"].ToString().ToUpper() == "Y")
                        rbPet_ENews.Items.FindByValue("Y").Selected = true;
                    else
                        rbPet_ENews.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("PETFOOD_NUTRITION"))
                {
                    if (hUDF["PETFOOD_NUTRITION"].ToString().ToUpper() == "Y")
                        rbPetfood_Nutrition.Items.FindByValue("Y").Selected = true;
                    else
                        rbPetfood_Nutrition.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("PIG_ENEWS"))
                {
                    if (hUDF["PIG_ENEWS"].ToString().ToUpper() == "Y")
                        rbPig_ENews.Items.FindByValue("Y").Selected = true;
                    else
                        rbPig_ENews.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("SHOPTIPS"))
                {
                    if (hUDF["SHOPTIPS"].ToString().ToUpper() == "Y")
                        rbShoptips.Items.FindByValue("Y").Selected = true;
                    else
                        rbShoptips.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("TIPSHEET"))
                {
                    if (hUDF["TIPSHEET"].ToString().ToUpper() == "Y")
                        rbTipSheet.Items.FindByValue("Y").Selected = true;
                    else
                        rbTipSheet.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("WPOU_ENEWS"))
                {
                    if (hUDF["WPOU_ENEWS"].ToString().ToUpper() == "Y")
                        rbWPOU_ENews.Items.FindByValue("Y").Selected = true;
                    else
                        rbWPOU_ENews.Items.FindByValue("N").Selected = true;
                }

                if (hUDF.Contains("POULTRYUPDATE"))
                {
                    if (hUDF["POULTRYUPDATE"].ToString().ToUpper() == "Y")
                        rbPoultryUpdate.Items.FindByValue("Y").Selected = true;
                    else
                        rbPoultryUpdate.Items.FindByValue("N").Selected = true;
                }
            }
            else
            {
                lblResult2.Text = "Subscriber not in the system";
            }
        }


        protected void btnAddProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string AccessKey;
                ecn.webservices.client.WATTAPI_LOCAL.CustomerData cd = new ecn.webservices.client.WATTAPI_LOCAL.CustomerData();

                AccessKey = tbAccessKey.Text;
                cd.EktronUserName = tbUserName.Text;
                cd.PubCode = drpPubCodes.SelectedItem.Value;
                if (tbBirthDay.Text.ToString() != "")
                    cd.BirthDay = Convert.ToDateTime(tbBirthDay.Text.ToString());

                cd.FirstName = tbFirstName.Text;
                cd.LastName = tbLastName.Text;
                cd.AddressLine1 = tbAddress1.Text;
                cd.AddressLine2 = tbAddress2.Text;
                cd.City = tbCity.Text;
                cd.State = tbState.Text;
                cd.Country = tbCountry.Text;
                cd.PostalCode = tbPostalCode.Text;
                cd.CompanyName = tbCompanyName.Text;

                //New fields

                if (tbAge.Text != "")
                {
                    cd.Age = Convert.ToInt16(tbAge.Text);
                }

                if (tbIncome.Text != "")
                {
                    cd.Income = Convert.ToDecimal(tbIncome.Text);
                }

                //cd.Gender = tbGender.Text;
                //cd.Password = tbPassword.Text;

                Hashtable hUDF = new Hashtable();


                hUDF.Add("SponsoredLinks", rbSponsoredLinks.SelectedItem.Value);
                hUDF.Add("SideRoads", rbSideRoads.SelectedItem.Value);
                hUDF.Add("VideoAlert", rbVideoAlert.SelectedItem.Value);
                hUDF.Add("INDUSTRY", tbIndustry.Text);
                hUDF.Add("NONUSPROVINCE", rbNonUSProvince.SelectedItem.Value);
                hUDF.Add("EKTRONUSERID", tbUserName.Text);
                hUDF.Add("SUBSCRIBEREMAIL", tbEmail.Text);

                //MasterGroup newsletters
                hUDF.Add("Agribusiness", rbAgribusiness.SelectedItem.Value);
                hUDF.Add("DesignTips", rbDesignTips.SelectedItem.Value);
                hUDF.Add("EquipmentTips", rbEquipmentTips.SelectedItem.Value);
                hUDF.Add("Feed_ENews", rbFeed_ENews.SelectedItem.Value);
                hUDF.Add("IA_Ciberboletin", rbIA_Ciberboletin.SelectedItem.Value);
                hUDF.Add("Management_Tips", rbManagement_Tips.SelectedItem.Value);
                hUDF.Add("Pet_ENews", rbPet_ENews.SelectedItem.Value);
                hUDF.Add("Petfood_Nutrition", rbPetfood_Nutrition.SelectedItem.Value);
                hUDF.Add("Pig_ENews", rbPig_ENews.SelectedItem.Value);
                hUDF.Add("Shoptips", rbShoptips.SelectedItem.Value);
                hUDF.Add("TipSheet", rbTipSheet.SelectedItem.Value);
                hUDF.Add("WPOU_ENews", rbWPOU_ENews.SelectedItem.Value);
                hUDF.Add("PoultryUpdate", rbPoultryUpdate.SelectedItem.Value);

                ecn.webservices.client.WATTAPI_LOCAL.WATTAPI s = new ecn.webservices.client.WATTAPI_LOCAL.WATTAPI();

                string result = s.AddProfile(AccessKey, cd, ToArray(hUDF));
                lblResult2.Text = result;
            }
            catch (Exception ex)
            {
                lblResult2.Text = ex.Message;
            }

        }

        private object[][] ToArray(Hashtable ht)
        {
            object[][] oo = new object[ht.Count][];
            int i = 0;
            foreach (object key in ht.Keys)
            {
                oo[i] = new object[] { key, ht[key] };
                i++;
            }
            return oo;
        }

        public Hashtable ToHashtable(object[][] oo)
        {
            Hashtable ht = new Hashtable(oo.Length);
            foreach (object[] pair in oo)
            {
                object key = pair[0].ToString().ToUpper();
                object value = pair[1];
                ht[key] = value;
            }
            return ht;
        }

        protected void btnNewUDF_Click(object sender, EventArgs e)
        {
            try
            {
                string accessKey = tbAccessKey.Text;
                string pubCode = drpPubCodes.SelectedItem.Value;
                string udf = tbNewUdfName.Text;
                ecn.webservices.client.WATTAPI_LOCAL.WATTAPI s = new ecn.webservices.client.WATTAPI_LOCAL.WATTAPI();
                string result = s.CreateUDF(accessKey, pubCode, udf);
                lblResult1.Text = result;
            }
            catch (Exception ex)
            {
                lblResult1.Text = ex.Message;
            }
        }

    }
}