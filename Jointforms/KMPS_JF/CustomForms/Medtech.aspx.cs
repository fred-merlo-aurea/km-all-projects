using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.accounts.classes;
using System.Data;
using System.Data.SqlClient; 
using KMPS_JF_Objects.Objects;

namespace KMPS_JF.CustomForms
{
    public partial class Medtech : System.Web.UI.Page
    {
        private Groups _pubGroup = null;
        private Groups _otherGroup = null;
        private Groups _multinewslettergroup = null;
        private Groups _newsweekgroup = null;
        private Emails _EmailProfile = null;
        private Publication _pub = null;

        /// <summary>
        /// Get the pub code from query string
        /// </summary>
        public string PubCode
        {
            get
            {
                try
                {
                    return Request.QueryString["pubcode"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Get the email from query string
        /// </summary>
        public string LoginID
        {
            get
            {
                try
                {
                    return Request.QueryString["e"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }


        public string PWD
        {
            get
            {
                try
                {
                    return Request.QueryString["pwd"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public Groups PubGroup
        {
            get
            {
                return this._pubGroup;
            }
            set
            {
                this._pubGroup = value;
            }
        }

        /// <summary>
        /// The other group in the customer account where the profile is found...
        /// </summary>
        
        public Groups OtherGroup
        {
            get
            {
                return this._otherGroup;
            }
            set
            {
                this._otherGroup = value;
            }
        }

        
        /// <summary>
        /// Group for Multinewsletter
        /// </summary>
        public Groups PubMultiNewsLetterGroup
        {
            get
            {
                try
                {
                    return new Groups(int.Parse(ConfigurationManager.AppSettings[PubCode.ToUpper() + "_MULTINEWSLETTERGROUP"]));
                }
                catch
                {
                    return null;  
                }
            }
            set
            {
                this._multinewslettergroup = value;
            }
        }

        /// <summary>
        /// Group for newsweek
        /// </summary>
        public Groups PubNewsWeekGroup
        {
            get
            {
                try
                {
                    return new Groups(int.Parse(ConfigurationManager.AppSettings[PubCode.ToUpper() + "_NEWSWEEKGROUP"]));
                }
                catch
                {
                    return null;  
                }
            }
            set
            {
                this._newsweekgroup = value;
            }
        }

        public string MultiNewsletterPubCode
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings[PubCode.ToUpper() + "_MNPUBCODE"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public Publication pub
        {
            get
            {
                return this._pub; 
            }
            set
            {
                this._pub = value;  
            }
        }

        public Emails EmailProfile
        {
            get
            {
                return this._EmailProfile;
            }
            set
            {
                this._EmailProfile = value;
            }
        }

        /// <summary>
        /// Find Email Profile in a group
        /// </summary>
        private Emails GetEmailFromGroup(Groups group)       
        {
            return group.WhatEmail(LoginID);    
        }

        /// <summary>
        /// This is the group where the email profile is to be pulled from the other groups found
        /// Iterate through all the groups for that Medtech customer account
        /// </summary>        
        private void GetOtherGroupEmailForHITN()  
        {
            int custID = PubGroup.CustomerID();

            string sql = "select top 1 e.EmailAddress, eg.GroupID from emails e join emailgroups eg on e.emailID = eg.emailID where CustomerID = " + PubGroup.CustomerID().ToString() + " and EmailAddress = @EmailAddress and GroupID not in (" + pub.ECNDefaultGroupID.ToString() + ")";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@EmailAddress", LoginID.ToString());

            DataTable dt = DataFunctions.GetDataTable("communicator", cmd);
            List<Groups> lstGroup = new List<Groups>();

            foreach (DataRow dr in dt.Rows)
            {
                lstGroup.Add(new Groups(dr["GroupID"].ToString()));
            }

            foreach (Groups g in lstGroup)
            {
                OtherGroup = g;
                EmailProfile = g.WhatEmail(LoginID);
                if (EmailProfile != null)
                    break;
            }
        }

        private void CopyProfile(Emails newEmailProfile, Emails oldEmailProfile)
        {
            Emails e = this.GetEmailByEmailAddressAndCustomerID(oldEmailProfile.EmailAddress(), oldEmailProfile.CustomerID());
            string sql = "update Emails set FirstName=@FirstName, LastName=@LastName, FullName=@FullName, Title=@Title, Company=@Company, Occupation=@Occupation, Address=@Address, City=@City, State=@State, Zip=@Zip, Country=@Country, Mobile=@Mobile, Fax=@Fax, Website=@WebSite, Age=@Age, Income=@Income, Gender=@Gender, Voice=@Voice, Notes=@Notes, User1=@User1, User6=@User6 where EmailID=@EmailID";
            SqlCommand cmdUpdateProfile = new SqlCommand();
            cmdUpdateProfile.CommandText = sql;
            cmdUpdateProfile.Parameters.AddWithValue("@FirstName", e.FirstName);
            cmdUpdateProfile.Parameters.AddWithValue("@LastName", e.LastName);
            cmdUpdateProfile.Parameters.AddWithValue("@FullName", e.FullName);
            cmdUpdateProfile.Parameters.AddWithValue("@Title", e.Title);
            cmdUpdateProfile.Parameters.AddWithValue("@Company", e.Company);
            cmdUpdateProfile.Parameters.AddWithValue("@Occupation", e.Occupation);
            cmdUpdateProfile.Parameters.AddWithValue("@Address", e.Address);
            cmdUpdateProfile.Parameters.AddWithValue("@City", e.City);
            cmdUpdateProfile.Parameters.AddWithValue("@State", e.State);
            cmdUpdateProfile.Parameters.AddWithValue("@Zip", e.Zip);
            cmdUpdateProfile.Parameters.AddWithValue("@Country", e.Country);
            cmdUpdateProfile.Parameters.AddWithValue("@Voice", e.Voice);
            cmdUpdateProfile.Parameters.AddWithValue("@Mobile", e.Mobile);
            cmdUpdateProfile.Parameters.AddWithValue("@Fax", e.Fax);
            cmdUpdateProfile.Parameters.AddWithValue("@Website", e.Website);
            cmdUpdateProfile.Parameters.AddWithValue("@Age", e.Age);
            cmdUpdateProfile.Parameters.AddWithValue("@Income", e.Income);
            cmdUpdateProfile.Parameters.AddWithValue("@Gender", e.Gender);
            cmdUpdateProfile.Parameters.AddWithValue("@Notes", e.Notes);
            cmdUpdateProfile.Parameters.AddWithValue("@User1", e.User1);
            cmdUpdateProfile.Parameters.AddWithValue("@User6", e.User6);
            cmdUpdateProfile.Parameters.AddWithValue("@EmailID", newEmailProfile.ID());
            DataFunctions.Execute("communicator", cmdUpdateProfile);
        }

        public Emails GetEmailByEmailAddressAndCustomerID(string EmailAddress, int CustomerID)
        {
            Emails e = new Emails();
            DataTable dt = DataFunctions.GetDataTable("Select top 1 * from Emails where EmailAddress = '" + EmailAddress + "' and CustomerID = " + CustomerID, ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString);

            if (dt.Rows.Count <= 0)
            {
                return null;
            }

            foreach (DataRow dr in dt.Rows)
            {
                e.FirstName = !dr.IsNull("FirstName") ? dr["FirstName"].ToString() : "";
                e.LastName = !dr.IsNull("LastName") ? dr["LastName"].ToString() : "";
                e.CustID = CustomerID;
                e.Email = EmailAddress;
                e.ID(!dr.IsNull("EmailID") ? Convert.ToInt32(dr["EmailID"].ToString()) : 0);
                e.FullName = !dr.IsNull("FullName") ? dr["FullName"].ToString() : string.Empty;
                e.Title = !dr.IsNull("Title") ? dr["Title"].ToString() : string.Empty;
                e.Occupation = !dr.IsNull("Occupation") ? dr["Occupation"].ToString() : string.Empty;
                e.Company = !dr.IsNull("Company") ? dr["Company"].ToString() : string.Empty;
                e.Address = !dr.IsNull("Address") ? dr["Address"].ToString() : string.Empty;
                e.City = !dr.IsNull("City") ? dr["City"].ToString() : string.Empty;
                e.State = !dr.IsNull("State") ? dr["State"].ToString() : string.Empty;
                e.Zip = !dr.IsNull("Zip") ? dr["Zip"].ToString() : string.Empty;
                e.Country = !dr.IsNull("Country") ? dr["Country"].ToString() : string.Empty;
                e.Voice = !dr.IsNull("Voice") ? dr["Voice"].ToString() : string.Empty;
                e.Mobile = !dr.IsNull("Mobile") ? dr["Mobile"].ToString() : string.Empty;
                e.Fax = !dr.IsNull("Fax") ? dr["Fax"].ToString() : string.Empty;
                e.Website = !dr.IsNull("Website") ? dr["Website"].ToString() : string.Empty;
                e.Age = !dr.IsNull("Age") ? dr["Age"].ToString() : string.Empty;
                e.Income = !dr.IsNull("Income") ? dr["Income"].ToString() : string.Empty;
                e.Gender = !dr.IsNull("Gender") ? dr["Gender"].ToString() : string.Empty;
                e.User1 = !dr.IsNull("User1") ? dr["User1"].ToString() : string.Empty;
                e.User2 = !dr.IsNull("User2") ? dr["User2"].ToString() : string.Empty;
                e.User3 = !dr.IsNull("User3") ? dr["User3"].ToString() : string.Empty;
                e.User4 = !dr.IsNull("User4") ? dr["User4"].ToString() : string.Empty;
                e.User5 = !dr.IsNull("User5") ? dr["User5"].ToString() : string.Empty;
                e.User6 = !dr.IsNull("User6") ? dr["User6"].ToString() : string.Empty;
                e.UserEvent1 = !dr.IsNull("UserEvent1") ? dr["UserEvent1"].ToString() : string.Empty;
                e.UserEvent2 = !dr.IsNull("UserEvent2") ? dr["UserEvent2"].ToString() : string.Empty;
                e.UserEvent1Date = !dr.IsNull("UserEvent1Date") ? Convert.ToDateTime(dr["UserEvent1Date"].ToString()) : Convert.ToDateTime("01/01/1900");
                e.UserEvent2Date = !dr.IsNull("UserEvent2Date") ? Convert.ToDateTime(dr["UserEvent2Date"].ToString()) : Convert.ToDateTime("01/01/1900");
                e.Notes = !dr.IsNull("Notes") ? dr["Notes"].ToString() : string.Empty;
            }

            return e;
        }  


        /// <summary>
        /// Copy profile and udf fields to HITN Group from other group where the matching profile is found. 
        /// </summary>       
        private void CopyProfileAndUDFToGroup(Groups NewPubGroup)
        {
            if (NewPubGroup != null)
            {
                if (NewPubGroup.CustomerID() == OtherGroup.CustomerID())     
                {
                    NewPubGroup.AttachEmail(EmailProfile, "html", "S");
                    SqlCommand cmd = new SqlCommand("SELECT gdf.GroupDatafieldsID, gdf.ShortName, edv.DataValue from EmailDataValues edv join GroupDatafields gdf on gdf.GroupDatafieldsID = edv.GroupDatafieldsID join Emails e on e.EmailID = edv.EmailID where e.EmailAddress = @EmailAddress and gdf.GroupID = @groupID and gdf.ShortName in ('BUSINESS','FUNCTION')");
                    cmd.Parameters.AddWithValue("@EmailAddress", LoginID.ToString());
                    cmd.Parameters.AddWithValue("@groupID", OtherGroup.ID());
                    DataTable dt = DataFunctions.GetDataTable("communicator", cmd);

                    foreach (DataRow dr in dt.Rows)
                    {
                        string sql = "SELECT GroupDatafieldsID from GroupDataFields where GroupID = " + NewPubGroup.ID().ToString() + "  and IsDeleted=0 and ShortName = '" + dr["ShortName"].ToString() + "'";
                        string gdf = DataFunctions.ExecuteScalar("communicator", new SqlCommand(sql)).ToString();
                        NewPubGroup.AttachUDFToEmail(EmailProfile, gdf, dr["DataValue"].ToString());
                    }
                }
                else
                {
                    //copy profile to new customer
                    Emails e = this.GetEmailByEmailAddressAndCustomerID(LoginID, NewPubGroup.CustomerID());

                    if (e == null)
                    {
                        this.CreateNewProfileFromEmailAddress(LoginID, NewPubGroup.CustomerID(), NewPubGroup.ID());     
                        this.EmailProfile = this.OtherGroup.WhatEmail(LoginID);
                        this.CopyProfile(NewPubGroup.WhatEmail(LoginID), EmailProfile);
                    }
                    else
                    {
                        NewPubGroup.AttachEmail(e, "html", "S");
                        this.CopyProfile(NewPubGroup.WhatEmail(LoginID), e);
                    }
                }
            }
        }

        /// <summary>
        /// Get the password from the 
        /// </summary>
        /// <returns></returns>
        private string GetPasswordByEmailAndGroupID(int GroupID)
        {
            try
            {
                if (PWD.Length > 0)
                {
                    return PWD;
                }
                else
                {                    
                    SqlCommand cmd = new SqlCommand("select e.Password as 'Password' from Emails e join EmailGroups eg on e.EmailID = eg.EmailID where e.EmailAddress = @EmailAddress and eg.GroupID = @groupID");
                    cmd.Parameters.AddWithValue("@EmailAddress", LoginID.ToString());
                    cmd.Parameters.AddWithValue("@groupID", GroupID);
                    string Password = Convert.ToString(DataFunctions.ExecuteScalar("communicator", cmd));   
                    return Password;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private void CreateNewProfileFromEmailAddress(string EmailAddress, int CustomerID, int GroupID)       
        {
            SqlCommand cmdCreateProfile = new SqlCommand("CreateNewProfileWithEmailAddress");
            cmdCreateProfile.CommandType = CommandType.StoredProcedure;
            cmdCreateProfile.Parameters.AddWithValue("@EmailAddress", EmailAddress);
            cmdCreateProfile.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdCreateProfile.Parameters.AddWithValue("@GroupID", GroupID);
            DataFunctions.Execute("communicator", cmdCreateProfile);
        }

        protected void Page_Load(object sender, EventArgs e)   
        {
            if (!IsPostBack)
            {
                if (PubCode.Length > 0 && LoginID.Length > 0)  
                {
                    try
                    {
                        pub = Publication.GetPublicationbyID(0, PubCode);
                    }
                    catch
                    {
                        pub = null;
                    }

                    this.PubGroup = new Groups(pub.ECNDefaultGroupID);
                    string LoginParam = string.Empty;                     

                    Emails epMultiNL = null;
                    Emails epNewsWeek = null; 

                    //Check in the Multinewsletter group first (eg:- HITNMN or HFNSMN)         
                    if (PubMultiNewsLetterGroup != null)
                    {
                        epMultiNL = this.GetEmailFromGroup(PubMultiNewsLetterGroup);     
                    }

                    //case when the profile is found in the multinewsletter group
                    if (epMultiNL != null)      
                    {
                        //check in HITN newsweek group. If does not exists copy from Multi Newsletter group
                        epNewsWeek = this.GetEmailFromGroup(PubNewsWeekGroup);

                        if (epNewsWeek == null)      
                        {
                            this.EmailProfile = epMultiNL; 
                            this.OtherGroup = PubMultiNewsLetterGroup;  
                            this.CopyProfileAndUDFToGroup(PubNewsWeekGroup);
                            this.CopyProfileAndUDFToGroup(PubGroup);  
                        }
                    }
                    else  // check in newsweek group
                    {
                        epNewsWeek = this.GetEmailFromGroup(PubNewsWeekGroup);

                        if (epNewsWeek != null)
                        {
                            this.EmailProfile = epNewsWeek;  
                            this.OtherGroup = PubNewsWeekGroup;  
                            this.CopyProfileAndUDFToGroup(PubMultiNewsLetterGroup);
                            this.CopyProfileAndUDFToGroup(PubGroup);  //copy profile to original group    
                        }
                        else
                        {
                            // Get the profile from original group
                            Emails epMain = this.GetEmailFromGroup(PubGroup);

                            if (epMain != null)
                            {
                                this.EmailProfile = epMain;
                                this.OtherGroup = PubGroup;                                   
                                this.CopyProfileAndUDFToGroup(PubMultiNewsLetterGroup);      
                                this.CopyProfileAndUDFToGroup(PubGroup); //copy profile to original group
                            }
                            else
                            {
                                // Check in other groups for the customer
                                this.GetOtherGroupEmailForHITN();

                                if (this.EmailProfile != null)
                                {
                                    //copy to MN group and newsweek group
                                    this.CopyProfileAndUDFToGroup(PubNewsWeekGroup);
                                    this.CopyProfileAndUDFToGroup(PubMultiNewsLetterGroup);
                                    this.CopyProfileAndUDFToGroup(PubGroup);   
                                }
                                else
                                {
                                    if (PubMultiNewsLetterGroup != null)
                                    {
                                        this.CreateNewProfileFromEmailAddress(LoginID, PubMultiNewsLetterGroup.CustomerID(), PubMultiNewsLetterGroup.ID());
                                        this.OtherGroup = PubMultiNewsLetterGroup;
                                        this.EmailProfile = PubMultiNewsLetterGroup.WhatEmail(LoginID); 
                                    }

                                    if (PubNewsWeekGroup != null)
                                    {
                                        this.CreateNewProfileFromEmailAddress(LoginID, PubNewsWeekGroup.CustomerID(), PubNewsWeekGroup.ID());
                                        this.OtherGroup = PubNewsWeekGroup;
                                        this.EmailProfile = PubNewsWeekGroup.WhatEmail(LoginID);
                                    }
                                    
                                    this.CopyProfileAndUDFToGroup(PubGroup);      
                                }
                            }
                        }
                    }

                    LoginParam = "e=" + LoginID + "&pwd=" + GetPasswordByEmailAndGroupID(PubNewsWeekGroup.ID());
                    Response.Redirect("../Forms/Subscription.aspx?PubCode=" + MultiNewsletterPubCode + "&cp=true" + "&step=form&loginparam=" + KMPS_JF_Objects.Objects.Utilities.Encrypt(LoginParam.Replace(" ", "+"), "Pas5pr@se", "s@1tValue", "SHA1", 2, "@1B2c3D4e5F6g7H8", 64));
                }
                else
                {
                    Response.Write("Invalid Parameters!!!");
                }
            }
        }
    }
}


