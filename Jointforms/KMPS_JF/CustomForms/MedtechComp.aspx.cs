using System;
using System.Configuration; 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls; 
using System.Web.UI.WebControls;
using ecn.communicator.classes;
using ecn.accounts.classes;
using System.Data;
using System.Data.SqlClient;
using KMPS_JF.Class;
using KMPS_JF_Objects.Objects;

namespace KMPS_JF.CustomForms
{
    public partial class MedtechComp : System.Web.UI.Page
    {
        public string PubCode
        {
            get
            {
                try
                {
                    return Request.QueryString["pubcode"]; 
                }
                catch 
                { 
                    return string.Empty; 
                }     
            }
        }

        private Publication _pub;  
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

        private Groups _pubGroup; 
        public Groups PubGroup
        {
            get 
            { 
                return this._pubGroup;  
            }
            set { this._pubGroup = value; }
        }

        private Emails _EmailProfile; 
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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {                                                                   
                this.pub = Publication.GetPublicationbyID(0, PubCode);

                if (this.pub == null)
                {
                    Response.Write("Invalid Publication!!!");   
                    container.Visible = false; 
                    return;                                          
                }
            }
            catch {
                
                pnlErrorMessage.Visible = true;
                lblErrorMessage.Text = "Invalid Publication!!!";    
                pnlForm.Visible = false;       
                return; 
            }

            try
            {
                this.PubGroup = new Groups(pub.ECNDefaultGroupID);
            }
            catch { }                                                                                                                                                                                                    
        }

        private void CopyProfile(Emails emailProfile, Emails oldEmailProfile)     
        {                      
            Emails e  = this.GetEmailByEmailAddressAndCustomerID(oldEmailProfile.EmailAddress(), oldEmailProfile.CustomerID());   
            string sql = "update Emails set FirstName=@FirstName, LastName=@LastName, FullName=@FullName, Title=@Title, Company=@Company, Occupation=@Occupation, Address=@Address, City=@City, State=@State, Zip=@Zip, Country=@Country, Mobile=@Mobile, Fax=@Fax, Website=@WebSite, Age=@Age, Income=@Income, Gender=@Gender, Voice=@Voice, Notes=@Notes, User1=@User1, User6=@User6 where EmailID=@EmailID";
            SqlCommand cmdUpdateProfile = new SqlCommand();
            cmdUpdateProfile.CommandText = sql;
            cmdUpdateProfile.Parameters.AddWithValue("@FirstName",e.FirstName);
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
            cmdUpdateProfile.Parameters.AddWithValue("@EmailID", e.ID());               
            DataFunctions.Execute("communicator", cmdUpdateProfile);     
        }

        public Emails GetEmailByEmailAddressAndCustomerID(string EmailAddress, int CustomerID)
        {
            Emails e = new Emails();            

            DataTable dt = DataFunctions.GetDataTable("Select top 1 * from Emails where EmailAddress = '" + EmailAddress + "' and CustomerID = " + CustomerID, ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString);

            if(dt.Rows.Count <= 0)
            {
                return null; 
            }
            
            foreach (DataRow dr in dt.Rows)      
            {
                e.ID(!dr.IsNull("EmailID") ? Convert.ToInt32(dr["EmailID"].ToString()) : 0);   
                e.FirstName = !dr.IsNull("FirstName") ? dr["FirstName"].ToString() : string.Empty;    
                e.LastName = !dr.IsNull("LastName") ? dr["LastName"].ToString() : string.Empty;
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

        private void CopyProfileAndUDFToGroup(Groups NewPubGroup, Groups OldGroup, string EmailAddress, string compUDF)        
        {
            Emails emailProfile = null;
            Emails oldEmailProfile = null;

            oldEmailProfile = OldGroup.WhatEmail(EmailAddress);
            oldEmailProfile.CustID = NewPubGroup.CustomerID(); 
            
            if (NewPubGroup.CustomerID() == OldGroup.CustomerID())   
            {
                emailProfile = oldEmailProfile;                
                NewPubGroup.AttachEmail(emailProfile, "html", "S");                    
            }
            else //copy to different customer account
            {
                emailProfile = NewPubGroup.WhatEmail(EmailAddress);  

                if (emailProfile == null)
                {
                    this.CreateNewProfileFromEmailAddress(EmailAddress, NewPubGroup.CustomerID(), NewPubGroup.ID());          
                    emailProfile = NewPubGroup.WhatEmail(EmailAddress);   
                }

                emailProfile.Email = EmailAddress;
                emailProfile.CustID = NewPubGroup.CustomerID(); 
                this.CopyProfile(emailProfile, oldEmailProfile);                       
            }

            //Copy All the UDFs

            SqlCommand cmd = new SqlCommand("SELECT gdf.GroupDatafieldsID, gdf.ShortName, edv.DataValue from EmailDataValues edv join GroupDatafields gdf on gdf.GroupDatafieldsID = edv.GroupDatafieldsID join Emails e on e.EmailID = edv.EmailID where e.EmailAddress = @EmailAddress  and gdf.IsDeleted=0 and gdf.GroupID = @groupID and gdf.ShortName in ('BUSINESS','FUNCTION')");
            cmd.Parameters.AddWithValue("@EmailAddress", EmailAddress);
            cmd.Parameters.AddWithValue("@groupID", OldGroup.ID());
            DataTable dt = DataFunctions.GetDataTable("communicator", cmd);          

            foreach (DataRow dr in dt.Rows)
            {
                if (!dr.IsNull("DataValue"))
                {
                    string sql = "SELECT GroupDatafieldsID from GroupDataFields where GroupID = " + NewPubGroup.ID().ToString() + " and ShortName = '" + dr["ShortName"].ToString() + "' and IsDeleted=0 ";
                    string gdf = DataFunctions.ExecuteScalar("communicator", new SqlCommand(sql)).ToString(); 
                    NewPubGroup.AttachUDFToEmail(emailProfile, gdf, dr["DataValue"].ToString());
                }
            }

            // Set Comp udf
            this.SetCompUDF(EmailAddress,NewPubGroup,emailProfile,compUDF);
            //Set status to subscribed
            this.UpdateSubscriberToSubscribed(emailProfile,NewPubGroup);
        }

        private void SetCompUDF(string EmailAddress, Groups NewPubGroup, Emails emailProfile, String compUDF)
        {
            string sql = "SELECT GroupDatafieldsID from GroupDataFields where GroupID = " + NewPubGroup.ID().ToString() + " and ShortName = 'Comp' and IsDeleted=0 ";
            string gdf = DataFunctions.ExecuteScalar("communicator", new SqlCommand(sql)).ToString();
            NewPubGroup.AttachUDFToEmail(emailProfile, gdf, compUDF);
        } 

        private Groups GetOtherGroupEmailForEmailProfile(string EmailAddress)          
        {                                                                    
            int custID = PubGroup.CustomerID();

            string sql = "select top 1 e.EmailAddress, eg.GroupID from emails e join emailgroups eg on e.emailID = eg.emailID join Groups g on eg.GroupID = g.GroupID and g.CustomerID = e.CustomerID where e.CustomerID = " + pub.ECNCustomerID.ToString() + " and e.EmailAddress = @EmailAddress and g.GroupID not in (" + pub.ECNDefaultGroupID.ToString() + ")";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Parameters.AddWithValue("@EmailAddress", EmailAddress);  

            DataTable dt = DataFunctions.GetDataTable("communicator", cmd);
            List<Groups> lstGroup = new List<Groups>();                       

            foreach (DataRow dr in dt.Rows)
            {
                lstGroup.Add(new Groups(dr["GroupID"].ToString()));
            }

            foreach (Groups g in lstGroup)
            {
                EmailProfile = g.WhatEmail(EmailAddress);
                if (EmailProfile != null)
                    return g; 
            }

            return null;
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

        private void UpdateSubscriberToSubscribed(Emails e, Groups g)
        {
            g.ModifiyEmailGroup(e.ID(), g.ID(), "html", "S");             
        }
      
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Groups OldGroup = null;
            Groups NewGroup = null;

            try
            {
                int selCount = 0;

                for (int i = 0; i < 15; i++)
                {
                    HtmlInputCheckBox chkNewsLetter = (HtmlInputCheckBox)this.Page.FindControl("ChkNewsLetter_" + i);

                    if (chkNewsLetter != null && chkNewsLetter.Checked) 
                        selCount++;                      
                }


                if (selCount == 0)
                {
                    pnlErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Please Select the NewsLetter!!";
                    return;
                }
            }
            catch { }  
          
            try
            {
                string compUDF = question_COMP_0.Checked ? "Y" : "N";
                
                //Check if the email exists in the main pub group
                this.EmailProfile = PubGroup.WhatEmail(txtEmailAddress.Value);

                if (this.EmailProfile != null) 
                    OldGroup = PubGroup;
                else
                    OldGroup = this.GetOtherGroupEmailForEmailProfile(txtEmailAddress.Value);     
              
                //Create a new profile in the publication group if the email address is not found in any group for the customer account
                if (OldGroup == null)
                {
                    this.CreateNewProfileFromEmailAddress(txtEmailAddress.Value, PubGroup.CustomerID(), PubGroup.ID());          
                    OldGroup = PubGroup;
                }

                string[] GroupsList = ConfigurationManager.AppSettings["NewsLetterGroups"].ToString().Split(',');

                for (int i = 0; i < 15; i++)
                {
                    HtmlInputCheckBox chkNewsLetter = (HtmlInputCheckBox)this.Page.FindControl("ChkNewsLetter_" + i);  

                    if (chkNewsLetter != null && chkNewsLetter.Checked)
                    {
                        NewGroup = new Groups(GroupsList[i]);
                        Emails ep = NewGroup.WhatEmail(txtEmailAddress.Value);       

                        //Check if the subscriber is already in the newsletter group    
                        if (ep == null)
                        {
                            this.CopyProfileAndUDFToGroup(NewGroup, OldGroup, txtEmailAddress.Value, compUDF);
                            ep = NewGroup.WhatEmail(txtEmailAddress.Value);  
                        }
                        else
                        {
                            // Set Comp udf                                                                               
                            this.SetCompUDF(txtEmailAddress.Value, NewGroup, ep, compUDF);          
                        }

                        this.UpdateSubscriberToSubscribed(ep, NewGroup);
                    }
                }

                Response.Redirect("http://eforms.kmpsgroup.com/jointforms/Forms/ThankYou.aspx?pubcode=" + PubCode);      
            }
            catch(Exception ex) 
            {
                pnlErrorMessage.Visible = true;                                         
                lblErrorMessage.Text = "Error:" + ex.ToString();         
            }
        }
    }
}
