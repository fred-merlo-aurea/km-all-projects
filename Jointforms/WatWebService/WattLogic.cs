using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.Serialization; 
using System.Configuration; 

namespace WattWebService
{
    public class WattLogic
    {
        public string AddProfile(CustomerData cd, Hashtable hUDF)
        { 
            int customerID;   //System.Configuration.ConfigurationManager.AppSettings["CustomerID"].ToString() 
            int groupID;      //System.Configuration.ConfigurationManager.AppSettings["GroupID"].ToString() 

            System.Xml.Linq.XDocument xd = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/PubCode.xml"));
            var cid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == cd.PubCode.ToUpper() 
                      select (int)x.Element("CustomerID"); 

            try
            {
                customerID = cid.First();
            }
            catch
            {
                throw new Exception("Invalid Pubcode");
            }

            var gid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == cd.PubCode.ToUpper() 
                      select (int)x.Element("GroupID");

            groupID = gid.First(); 

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString()); 
            SqlCommand cmd = new SqlCommand("sp_importEmails", conn); 
            cmd.CommandType = CommandType.StoredProcedure; 
            cmd.Parameters.AddWithValue("@CustomerID", customerID);  
            cmd.Parameters.AddWithValue("@GroupID", groupID);  
            cmd.Parameters.AddWithValue("@xmlProfile", EmailAddressToXML(cd)); 
            cmd.Parameters.AddWithValue("@xmlUDF", DataToXML(cd, groupID, hUDF));  
            cmd.Parameters.AddWithValue("@formattypecode", "html"); 
            cmd.Parameters.AddWithValue("@subscribetypecode", "S"); 
            cmd.Parameters.AddWithValue("@EmailAddressOnly", 0); 


            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable retDT = new DataTable();
            DataColumn dc1 = new DataColumn("Code");
            retDT.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("RowCount");
            retDT.Columns.Add(dc2);
            retDT.AcceptChanges();

            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    DataRow dr = retDT.NewRow();
                    dr["Code"] = rdr.GetValue(0).ToString();
                    dr["RowCount"] = rdr.GetValue(1).ToString();
                    retDT.Rows.Add(dr);
                }
            }

            bool success = false;
            foreach (DataRow dr in retDT.Rows) 
            {
                if (Convert.ToInt32(dr["RowCount"].ToString()) > 0) 
                    success = true;
            }

            UpdateProfileToMasterGroup(cd, hUDF);  

            if (success)
                return "Profile added or updated";
            else
                return "No Profile added or updated";

            //@result values ('T',@@ROWCOUNT) 
            //'S', @@ROWCOUNT
            //('D',@@ROWCOUNT)
            //('U',@@ROWCOUNT)
            //('I',@@ROWCOUNT)

        }

        public CustomerData GetProfile(string pubCode, string Emailaddress)
        {
            int customerID;   //System.Configuration.ConfigurationManager.AppSettings["CustomerID"].ToString() 
            int groupID;      //System.Configuration.ConfigurationManager.AppSettings["GroupID"].ToString() 
            int EmailID = 0;
            Hashtable hUDF = new Hashtable();

            CustomerData cd = null;

            System.Xml.Linq.XDocument xd = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/PubCode.xml"));
            var cid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == pubCode.ToUpper()
                      select (int)x.Element("CustomerID");

            try
            {
                customerID = cid.First();
            }
            catch
            {
                throw new Exception("Invalid Pubcode");
            }

            var gid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == pubCode.ToUpper()
                      select (int)x.Element("GroupID");

            groupID = gid.First();

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString());
            SqlCommand cmd = new SqlCommand("select Emails.* from Emails join EmailGroups on Emails.EmailID = EmailGroups.EmailID where EmailGroups.GroupID = @groupID and emailaddress = @emailaddress", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@groupID", groupID);
            cmd.Parameters.AddWithValue("@emailaddress", Emailaddress);

            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (rdr.Read())
            {
                    cd = new CustomerData();
                    EmailID = Convert.ToInt32(rdr["EmailID"].ToString());
                    cd.EktronUserName = Emailaddress;
                    cd.PubCode = pubCode;
                    
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Birthdate")))
                        cd.BirthDay = Convert.ToDateTime(rdr["Birthdate"].ToString());

                    cd.FirstName = rdr["FirstName"].ToString();
                    cd.LastName = rdr["LastName"].ToString();
                    cd.AddressLine1 = rdr["Address"].ToString();
                    cd.AddressLine2 = rdr["Address2"].ToString();
                    cd.City = rdr["City"].ToString();
                    cd.State = rdr["State"].ToString();
                    cd.Country = rdr["Country"].ToString();
                    cd.PostalCode = rdr["Zip"].ToString();
                    cd.CompanyName = rdr["Company"].ToString();
                    cd.Title = rdr["Title"].ToString();
                    cd.FullName = rdr["FullName"].ToString();
                    cd.Occupation = rdr["Occupation"].ToString();
                    cd.Phone = rdr["Voice"].ToString();
                    cd.Mobile = rdr["Mobile"].ToString();
                    cd.Fax = rdr["Fax"].ToString();
                    cd.Website = rdr["Website"].ToString();

                    if (!rdr.IsDBNull(rdr.GetOrdinal("Age")))
                    {
                        try { cd.Age = Convert.ToInt16(rdr["Age"].ToString()); }
                        catch { };
                    }
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Income")))
                    {
                        try { cd.Income = Convert.ToDecimal(rdr["Income"].ToString()); }
                        catch { };
                    }
                                        

                    cd.Gender = rdr["Gender"].ToString();
                    cd.Password = rdr["Password"].ToString();
            }
            conn.Close();

            if (cd != null)
            {
                SqlCommand cmdUDF = new SqlCommand("select ShortName, datavalue from GroupDatafields gdf left outer join EmailDataValues edv on edv.GroupDatafieldsID = gdf.GroupDatafieldsID left outer join emails e on e.emailID = edv.emailID where GDF.GroupID in (" + ConfigurationManager.AppSettings["MasterGroup_GroupID"].ToString() + "," + groupID + ") and  e.emailaddress = '" + Emailaddress.Replace("'","''") + "'", conn);
                cmdUDF.CommandType = CommandType.Text;
                conn.Open();

                SqlDataReader rdrUDF = cmdUDF.ExecuteReader(CommandBehavior.CloseConnection);
                while (rdrUDF.Read())
                {
                    if (!hUDF.ContainsKey(rdrUDF["ShortName"].ToString().ToUpper()))
                        hUDF.Add(rdrUDF["ShortName"].ToString().ToUpper(), rdrUDF["Datavalue"].ToString());
                }

                cd.hUDF = HelperFunctions.ToArray(hUDF);

                conn.Close();

            }
            

            return cd;
        }

        public int CreateUDF(string pubcode, string newUDF)
        {
            int groupID;//System.Configuration.ConfigurationManager.AppSettings["GroupID"].ToString()
            System.Xml.Linq.XDocument xd = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/PubCode.xml"));

            var gid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == pubcode.ToUpper()  
                      select (int)x.Element("GroupID");



            try
            {
                groupID = gid.First();
            }
            catch
            {
                throw new Exception("Invalid Pubcode");
            }

            //check for duplicate name
            List<GroupDataField> listGDF = GroupDataFieldData.GetData(groupID);
            int check = listGDF.Where(x => x.ShortName.ToLower().Equals(newUDF.ToLower())).Count();

            if (check == 0)
            {
                GroupDataField gdf = new GroupDataField();
                gdf.GroupID = groupID;
                gdf.ShortName = newUDF;
                gdf.LongName = newUDF;
                gdf.SurveyID = -1;
                gdf.DataFieldSetID = null;
                gdf.IsPublic = "N";
                gdf.IsPrimaryKey = false;

                return GroupDataFieldData.Insert(gdf);
            }
            else
                return -2;
        }

        private string EmailAddressToXML(CustomerData cd)
        {
            XmlDocument xmlDoc = new XmlDocument();
            // Write down the XML declaration
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "iso-8859-1", null);
            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("XML");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);

            // Create a new <Category> element and add it to the root node
            XmlElement parentNode = xmlDoc.CreateElement("Emails");
            xmlDoc.DocumentElement.PrependChild(parentNode);

            // Create the required nodes
            XmlElement mainNode = xmlDoc.CreateElement("emailaddress");
            // retrieve the text 
            XmlText categoryText = xmlDoc.CreateTextNode(cd.EktronUserName);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(mainNode);
            // save the value of the fields into the nodes
            mainNode.AppendChild(categoryText);

            XmlElement firstNameNode = xmlDoc.CreateElement("firstname");
            // retrieve the text 
            XmlText firstNameText = xmlDoc.CreateTextNode(cd.FirstName);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(firstNameNode);
            // save the value of the fields into the nodes
            firstNameNode.AppendChild(firstNameText);

            XmlElement lastNameNode = xmlDoc.CreateElement("lastname");
            // retrieve the text
            XmlText lastNameText = xmlDoc.CreateTextNode(cd.LastName);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(lastNameNode);
            // save the value of the fields into the nodes
            lastNameNode.AppendChild(lastNameText);

            XmlElement fullNameNode = xmlDoc.CreateElement("fullname");
            // retrieve the text 
            XmlText fullNameText = xmlDoc.CreateTextNode(cd.FirstName + " " + cd.LastName);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(fullNameNode);
            // save the value of the fields into the nodes
            fullNameNode.AppendChild(fullNameText);

            XmlElement companyNode = xmlDoc.CreateElement("company");
            // retrieve the text 
            XmlText companyText = xmlDoc.CreateTextNode(cd.CompanyName);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(companyNode);
            // save the value of the fields into the nodes
            companyNode.AppendChild(companyText);

            XmlElement add1Node = xmlDoc.CreateElement("address");
            // retrieve the text 
            XmlText add1Text = xmlDoc.CreateTextNode(cd.AddressLine1);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(add1Node);
            // save the value of the fields into the nodes
            add1Node.AppendChild(add1Text);

            XmlElement add2Node = xmlDoc.CreateElement("address2");
            // retrieve the text 
            XmlText add2Text = xmlDoc.CreateTextNode(cd.AddressLine2);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(add2Node);
            // save the value of the fields into the nodes
            add2Node.AppendChild(add2Text);

            XmlElement cityNode = xmlDoc.CreateElement("city");
            // retrieve the text 
            XmlText cityText = xmlDoc.CreateTextNode(cd.City);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(cityNode);
            // save the value of the fields into the nodes
            cityNode.AppendChild(cityText);

            XmlElement stateNode = xmlDoc.CreateElement("state");
            // retrieve the text 
            XmlText stateText = xmlDoc.CreateTextNode(cd.State);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(stateNode);
            // save the value of the fields into the nodes
            stateNode.AppendChild(stateText);

            XmlElement zipNode = xmlDoc.CreateElement("zip");
            // retrieve the text 
            XmlText zipText = xmlDoc.CreateTextNode(cd.PostalCode);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(zipNode);
            // save the value of the fields into the nodes
            zipNode.AppendChild(zipText);

            XmlElement countryNode = xmlDoc.CreateElement("country");
            // retrieve the text 
            XmlText countryText = xmlDoc.CreateTextNode(cd.Country);
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(countryNode);
            // save the value of the fields into the nodes
            countryNode.AppendChild(countryText);

            XmlElement birthDateNode = xmlDoc.CreateElement("birthdate");
            // retrieve the text 
            XmlText birthDateText = xmlDoc.CreateTextNode(cd.BirthDay.ToString());
            // append the nodes to the parentNode without the value
            parentNode.AppendChild(birthDateNode);
            // save the value of the fields into the nodes
            birthDateNode.AppendChild(birthDateText);

            if (cd.Title != string.Empty)
            {
                XmlElement titleNode = xmlDoc.CreateElement("title");
                // retrieve the text 
                XmlText titleText = xmlDoc.CreateTextNode(cd.Title);
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(titleNode);
                // save the value of the fields into the nodes
                titleNode.AppendChild(titleText);
            }

            if (cd.FullName != string.Empty)
            {

                XmlElement fullnameNode = xmlDoc.CreateElement("fullname");
                // retrieve the text 
                XmlText fullnameText = xmlDoc.CreateTextNode(cd.FullName);
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(fullnameNode);
                // save the value of the fields into the nodes
                fullnameNode.AppendChild(fullnameText);
            }

            if (cd.Occupation != string.Empty)
            {

                XmlElement occupationNode = xmlDoc.CreateElement("occupation");
                // retrieve the text 
                XmlText occupationText = xmlDoc.CreateTextNode(cd.Occupation);
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(occupationNode);
                // save the value of the fields into the nodes
                occupationNode.AppendChild(occupationText);
            }

            if (cd.Phone != string.Empty)
            {

                XmlElement voiceNode = xmlDoc.CreateElement("voice");
                // retrieve the text 
                XmlText voiceText = xmlDoc.CreateTextNode(cd.Phone);
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(voiceNode);
                // save the value of the fields into the nodes
                voiceNode.AppendChild(voiceText);
            }

            if (cd.Mobile != string.Empty)
            {

                XmlElement mobileNode = xmlDoc.CreateElement("mobile");
                // retrieve the text 
                XmlText mobileText = xmlDoc.CreateTextNode(cd.Mobile);
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(mobileNode);
                // save the value of the fields into the nodes
                mobileNode.AppendChild(mobileText);

            }

            if (cd.Fax != string.Empty)
            {

                XmlElement faxNode = xmlDoc.CreateElement("fax");
                // retrieve the text 
                XmlText faxText = xmlDoc.CreateTextNode(cd.Fax);
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(faxNode);
                // save the value of the fields into the nodes
                faxNode.AppendChild(faxText);
            }

            if (cd.Website != string.Empty)
            {

                XmlElement websiteNode = xmlDoc.CreateElement("website"); 
                // retrieve the text 
                XmlText websiteText = xmlDoc.CreateTextNode(cd.Website);
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(websiteNode);
                // save the value of the fields into the nodes
                websiteNode.AppendChild(websiteText);
            }

            if (cd.Age > 0)
            {

                XmlElement ageNode = xmlDoc.CreateElement("age");
                // retrieve the text 
                XmlText ageText = xmlDoc.CreateTextNode(cd.Age.ToString());
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(ageNode);
                // save the value of the fields into the nodes
                ageNode.AppendChild(ageText);
            }

            if (cd.Income > 0)
            {

                XmlElement incomeNode = xmlDoc.CreateElement("income");
                // retrieve the text 
                XmlText incomeText = xmlDoc.CreateTextNode(cd.Income.ToString());
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(incomeNode);
                // save the value of the fields into the nodes
                incomeNode.AppendChild(incomeText);
            }
            if (cd.Gender != string.Empty)
            {

                XmlElement genderNode = xmlDoc.CreateElement("gender");
                // retrieve the text 
                XmlText genderText = xmlDoc.CreateTextNode(cd.Gender);
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(genderNode);
                // save the value of the fields into the nodes
                genderNode.AppendChild(genderText);
            }
            if (cd.Password != string.Empty)
            {

                XmlElement passwordNode = xmlDoc.CreateElement("password");
                // retrieve the text 
                XmlText passwordText = xmlDoc.CreateTextNode(cd.Password);
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(passwordNode);
                // save the value of the fields into the nodes
                passwordNode.AppendChild(passwordText);
            }

            return xmlDoc.InnerXml.ToString();
        }


        /// <summary>
        /// Copy the profile to the Master Group
        /// </summary>
        private void UpdateProfileToMasterGroup(CustomerData cd, Hashtable newsletterUDF)     
        {
            int MasterGroupCustomerID = Convert.ToInt32(ConfigurationManager.AppSettings["MasterGroup_CustomerID"].ToString()); 
            int MasterGroupID = Convert.ToInt32(ConfigurationManager.AppSettings["MasterGroup_GroupID"].ToString()); 
            List<string> newsLetterUDF = ConfigurationManager.AppSettings["MasterGroup_Newsletter_UDFs"].ToString().ToUpper().Split(',').ToList<string>();
            int EmailID = 0;
            string SubscribeTypeCode = string.Empty; 

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString());
            conn.Open(); 
            SqlCommand cmd = new SqlCommand("sp_WATTCheckSubscriberProfile", conn);   
            cmd.CommandType = CommandType.StoredProcedure;  
            cmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int)).Value = MasterGroupID;
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", SqlDbType.VarChar)).Value = cd.EktronUserName;     
            SqlDataReader dr = cmd.ExecuteReader(); 
            
            while (dr.Read())
            {
                if (dr.HasRows) 
                {
                    EmailID = Convert.ToInt32(dr["EmailID"].ToString());
                    SubscribeTypeCode = dr["SubscribeTypeCode"].ToString(); 
                }
            }
            
            conn.Close();   

            Hashtable hProfileUDF = new Hashtable();
            hProfileUDF = GetUDFValues(MasterGroupID, EmailID); //Master Group UDF and values  

            Hashtable masterUDF = new Hashtable(); 
           
            foreach (String s in newsLetterUDF)
            {
                string hUDFVal = "";
                string mUDF = "";

                if (newsletterUDF.ContainsKey(s))
                {
                    hUDFVal = newsletterUDF[s].ToString();

                    if (hProfileUDF.ContainsKey(s))
                        mUDF = hProfileUDF[s].ToString();
                }

                if ((hUDFVal.Equals("Y", StringComparison.OrdinalIgnoreCase) || hUDFVal.Equals("1", StringComparison.OrdinalIgnoreCase)) && mUDF.ToUpper() != "Y") //Exists in WATT but not in ECN
                {
                    masterUDF.Add(s, "Y");
                    masterUDF.Add(s + "_Source", "Watt webservice - " + cd.PubCode.ToString());
                }
                else if ((hUDFVal.Equals("N", StringComparison.OrdinalIgnoreCase) || hUDFVal.Equals("0", StringComparison.OrdinalIgnoreCase)) && (mUDF.ToUpper() != "N" && mUDF != ""))   //Not exists in WATT but exists in Master Group
                {
                    masterUDF.Add(s, "N");
                    masterUDF.Add(s + "_Source", "Watt webservice - " + cd.PubCode.ToString());
                }
                                        
                //if ((hUDFVal.Equals("Y",  StringComparison.OrdinalIgnoreCase) || hUDFVal.Equals("1",  StringComparison.OrdinalIgnoreCase)) && mUDF == "") //Exists in WATT but not in ECN
                //{
                //    masterUDF.Add(s, "Y");
                //    masterUDF.Add(s + "_Source", "Watt webservice - " +  cd.PubCode.ToString());  
                //}
                //else if ((hUDFVal.Equals("N", StringComparison.OrdinalIgnoreCase) || hUDFVal.Equals("0", StringComparison.OrdinalIgnoreCase)) && (mUDF == "Y" || mUDF == "1"))   //Not exists in WATT but exists in Master Group
                //{
                //    masterUDF.Add(s, "N");
                //    masterUDF.Add(s + "_Source", "Watt webservice - " + cd.PubCode.ToString());  
                //}
                //else if (hUDFVal == "Y" && mUDF == "N")  //Exists in both
                //{
                //    //No Update
                //}
            }

            if (masterUDF.Count > 0)
            {
                conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString());
                conn.Open();
                cmd = new SqlCommand("sp_importEmails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", MasterGroupCustomerID);
                cmd.Parameters.AddWithValue("@GroupID", MasterGroupID);
                cmd.Parameters.AddWithValue("@xmlProfile", EmailAddressToXML(cd));
                cmd.Parameters.AddWithValue("@xmlUDF", DataToXML(cd, MasterGroupID, masterUDF));
                cmd.Parameters.AddWithValue("@formattypecode", "html");
                cmd.Parameters.AddWithValue("@subscribetypecode", "S");
                cmd.Parameters.AddWithValue("@EmailAddressOnly", 0);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }


        public static Hashtable GetUDFValues(int groupID, int EmailID)
        {
            Hashtable htUDFValues = new Hashtable();  
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DbConn"].ToString()); 
            SqlCommand cmd = new SqlCommand(); 
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetEmailActivitySimpleUDFDataValues";
            cmd.Parameters.Add(new SqlParameter("@groupID", SqlDbType.Int)).Value = groupID;    
            cmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int)).Value = EmailID;   
            //SqlDataReader dr = cmd.ExecuteReader();

            SqlDataAdapter da = new SqlDataAdapter(cmd);  
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)   
            {
                if (!htUDFValues.ContainsKey(dr["ShortName"].ToString())) 
                {
                    htUDFValues.Add(dr["ShortName"].ToString().ToUpper(), dr["DataValue"].ToString());    
                }
            }

            //while (dr.Read())
            //{
            //    if (dr.HasRows && !htUDFValues.ContainsKey(dr["ShortName"].ToString()))
            //    {
            //        htUDFValues.Add(dr["ShortName"].ToString(), dr["DataValue"].ToString());
            //    }
            //}

            conn.Close();
            return htUDFValues; 
        } 
        
        private string DataToXML(CustomerData cd, int groupID, Hashtable hUDF)  
        {

            XmlDocument xmlDoc = new XmlDocument();
            // Write down the XML declaration
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "iso-8859-1", null);
            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("XML");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);

            if (hUDF.Count > 0)
            {
                // Create a new <Category> element and add it to the root node
                XmlElement parentNode = xmlDoc.CreateElement("row");
                xmlDoc.DocumentElement.PrependChild(parentNode);
                // Create the required nodes
                XmlElement eaNode = xmlDoc.CreateElement("ea");

                // retrieve the text 
                XmlText categoryText = xmlDoc.CreateTextNode(cd.EktronUserName); 
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(eaNode);

                // save the value of the fields into the nodes
                eaNode.AppendChild(categoryText);

                ////parse out ArrayList to get name/value pairs for UDFs - each string parse on ":" to split key/value
                //Dictionary<string, string> dictUDF = new Dictionary<string, string>();
                //foreach (string s in listUDF)
                //{
                //    Array a = s.Split(':');
                //    dictUDF.Add(a.GetValue(0).ToString(), a.GetValue(1).ToString());
                //}

                //dynamically get the GroupDataFields bases on the GroupID
                List<GroupDataField> listGDF = GroupDataFieldData.GetData(groupID); 

                IDictionaryEnumerator en = hUDF.GetEnumerator();

                while (en.MoveNext())
                {
                    GroupDataField gdf = listGDF.SingleOrDefault(x => x.ShortName == en.Key.ToString().ToUpper());
                    if (gdf != null)
                    {
                        XmlElement udfNode = xmlDoc.CreateElement("udf");
                        udfNode.SetAttribute("id", gdf.GroupDataFieldsID.ToString());
                        XmlElement valueNode = xmlDoc.CreateElement("v");
                        XmlText valueText = xmlDoc.CreateTextNode(en.Value.ToString());
                        udfNode.AppendChild(valueNode);   
                        valueNode.AppendChild(valueText);  
                        parentNode.AppendChild(udfNode);  
                    }
                }
            }
            return xmlDoc.InnerXml.ToString();
        } 
    }
}