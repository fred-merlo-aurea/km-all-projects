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
using KM.Common;

namespace ecn.webservice.CustomAPI
{
    public class WattLogic
    {
        private const string EncodingIso88591 = "iso-8859-1";
        private const string XmlVersionOnq = "1.0";
        private const string XmlElementXml = "XML";
        private const string XmlElementEmails = "Emails";
        private const string XmlElementEmailAddress = "emailaddress";
        private const string XmlElementFirstName = "firstname";
        private const string XmlElementLastName = "lastname";
        private const string XmlElementFullName = "fullname";
        private const string XmlElementCompany = "company";
        private const string XmlElementAddress = "address";
        private const string XmlElementAddress2 = "address2";
        private const string XmlElementCity = "city";
        private const string XmlElementState = "state";
        private const string XmlElementZip = "zip";
        private const string XmlElementContry = "country";
        private const string XmlElementBirthDate = "birthdate";
        private const string XmlElementTitle = "title";
        private const string XmlElementOccupation = "occupation";
        private const string XmlElementVoice = "voice";
        private const string XmlElementMobile = "mobile";
        private const string XmlElementFax = "fax";
        private const string XmlElementWebsite = "website";
        private const string XmlElementAge = "age";
        private const string XmlElementIncome = "income";
        private const string XmlElementGender = "gender";
        private const string XmlElementPassword = "password";

        public string AddProfile(CustomerData cd, Hashtable hUDF, string accessKey)
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

            DataTable retDT = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(KMPlatform.BusinessLogic.User.GetByCustomerID(customerID)[0], customerID, groupID, EmailAddressToXML(cd), DataToXML(cd, groupID, hUDF, accessKey), "html", "S", false);

            UpdateProfileToMasterGroup(cd, hUDF, accessKey);

            if (retDT.Rows[5].ItemArray[1] != "0")
            {
                return "Profile added or updated";
            }
            else
            {
                return "No Profile added or updated";
            }
            //@result values ('T',@@ROWCOUNT) 
            //'S', @@ROWCOUNT
            //('D',@@ROWCOUNT)
            //('U',@@ROWCOUNT)
            //('I',@@ROWCOUNT)
        }

        public CustomerData GetProfile(string pubCode, string Emailaddress, string accessKey)
        {
            int customerID;   //System.Configuration.ConfigurationManager.AppSettings["CustomerID"].ToString() 
            int groupID;      //System.Configuration.ConfigurationManager.AppSettings["GroupID"].ToString() 
            int EmailID = 0;
            Hashtable hUDF = new Hashtable();

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

            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(accessKey, false);
            ECN_Framework_Entities.Communicator.EmailGroup emailProfiles = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailAddressGroupID(Emailaddress,groupID, user);
            
            CustomerData cd = new CustomerData();

            if (emailProfiles != null)
            {
                ECN_Framework_Entities.Communicator.Email emailProfile = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailIDGroupID(emailProfiles.EmailID, emailProfiles.GroupID, user);
                cd.AddressLine1 = emailProfile.Address;
                cd.AddressLine2 = emailProfile.Address2;
                Int16 age;
                Int16.TryParse(emailProfile.Age, out age);
                cd.Age = age;
                cd.BirthDay = emailProfile.Birthdate;
                cd.City = emailProfile.City;
                cd.State = emailProfile.State;
                cd.CompanyName = emailProfile.Company;
                cd.Country = emailProfile.Country;
                cd.Title = emailProfile.Title;
                cd.EktronUserName = Emailaddress;
                cd.Fax = emailProfile.Fax;
                cd.FirstName = emailProfile.FirstName;
                cd.LastName = emailProfile.LastName;
                cd.FullName = emailProfile.FullName;
                cd.Mobile = emailProfile.Mobile;
                cd.Occupation = emailProfile.Occupation;
                cd.Password = emailProfile.Password;
                cd.Phone = emailProfile.Voice;
                cd.Mobile = emailProfile.Mobile;
                cd.Website = emailProfile.Website;
                cd.PostalCode = emailProfile.Zip;
                Decimal income;
                Decimal.TryParse(emailProfile.Income, out income);
                cd.Income = income;
                cd.Gender = emailProfile.Gender;
                cd.PubCode = pubCode;

                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Communicator"].ToString());
                SqlCommand cmdUDF = new SqlCommand("select ShortName, datavalue from GroupDatafields gdf left outer join EmailDataValues edv on edv.GroupDatafieldsID = gdf.GroupDatafieldsID left outer join emails e on e.emailID = edv.emailID where GDF.GroupID in (" + ConfigurationManager.AppSettings["MasterGroup_GroupID"].ToString() + "," + groupID + ") and  e.emailaddress = '" + Emailaddress.Replace("'", "''") + "'", conn);
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

        public int CreateUDF(string pubcode, string newUDF, string accessKey)
        {
            int groupID;//System.Configuration.ConfigurationManager.AppSettings["GroupID"].ToString()
            int customerID;
            System.Xml.Linq.XDocument xd = System.Xml.Linq.XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/PubCode.xml"));

            var gid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == pubcode.ToUpper()
                      select (int)x.Element("GroupID");
            var cid = from x in xd.Descendants("Customer")
                      where (string)x.Attribute("pubCode") == pubcode.ToUpper()
                      select (int)x.Element("CustomerID");



            try
            {
                groupID = gid.First();
                customerID = cid.First();
            }
            catch
            {
                throw new Exception("Invalid Pubcode");
            }

            //check for duplicate name
            KMPlatform.Entity.User user = KMPlatform.BusinessLogic.User.GetByAccessKey(accessKey, false);
            List<ECN_Framework_Entities.Communicator.GroupDataFields> listGDF = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, user);
            int check = listGDF.Where(x => x.ShortName.ToLower().Equals(newUDF.ToLower())).Count();

            ECN_Framework_Entities.Communicator.GroupDataFields gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();

            if (check == 0)
            {
                gdf.GroupID = groupID;
                gdf.ShortName = newUDF;
                gdf.LongName = newUDF;
                gdf.SurveyID = null;
                gdf.DatafieldSetID = null;
                gdf.IsPublic = "N";
                gdf.IsPrimaryKey = false;
                gdf.CustomerID = customerID;
                gdf.CreatedUserID = user.UserID;

                return ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf, user);
            }
            else
                return -2;
        }

        private string EmailAddressToXML(CustomerData customer)
        {
            Guard.NotNull(customer, nameof(customer));

            var xmlDoc = new XmlDocument();
            var xmlDeclaration = xmlDoc.CreateXmlDeclaration(XmlVersionOnq, EncodingIso88591, null);
            var rootNode = xmlDoc.CreateElement(XmlElementXml);
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);

            var parentNode = xmlDoc.CreateElement(XmlElementEmails);
            xmlDoc.DocumentElement.PrependChild(parentNode);

            var mainNode = xmlDoc.CreateElement(XmlElementEmailAddress);
            var categoryText = xmlDoc.CreateTextNode(customer.EktronUserName);
            parentNode.AppendChild(mainNode);
            mainNode.AppendChild(categoryText);

            var firstNameNode = xmlDoc.CreateElement(XmlElementFirstName);
            var firstNameText = xmlDoc.CreateTextNode(customer.FirstName);
            parentNode.AppendChild(firstNameNode);
            firstNameNode.AppendChild(firstNameText);

            var lastNameNode = xmlDoc.CreateElement(XmlElementLastName);
            var lastNameText = xmlDoc.CreateTextNode(customer.LastName);
            parentNode.AppendChild(lastNameNode);
            lastNameNode.AppendChild(lastNameText);

            var fullNameNode = xmlDoc.CreateElement(XmlElementFullName);
            var fullNameText = xmlDoc.CreateTextNode($"{customer.FirstName} {customer.LastName}");
            parentNode.AppendChild(fullNameNode);
            fullNameNode.AppendChild(fullNameText);

            var companyNode = xmlDoc.CreateElement(XmlElementCompany);
            var companyText = xmlDoc.CreateTextNode(customer.CompanyName);
            parentNode.AppendChild(companyNode);
            companyNode.AppendChild(companyText);

            EmailFillAddressFields(customer, xmlDoc, parentNode);

            var birthDateNode = xmlDoc.CreateElement(XmlElementBirthDate);
            var birthDateText = xmlDoc.CreateTextNode(customer.BirthDay.ToString());
            parentNode.AppendChild(birthDateNode);
            birthDateNode.AppendChild(birthDateText);

            EmailFillOptinalFields(customer, xmlDoc, parentNode);

            return xmlDoc.InnerXml;
        }

        private static void EmailFillAddressFields(CustomerData customer, XmlDocument xmlDoc, XmlNode parentNode)
        {
            Guard.NotNull(customer, nameof(customer));
            Guard.NotNull(xmlDoc, nameof(xmlDoc));
            Guard.NotNull(parentNode, nameof(parentNode));

            var add1Node = xmlDoc.CreateElement(XmlElementAddress);
            var add1Text = xmlDoc.CreateTextNode(customer.AddressLine1);
            parentNode.AppendChild(add1Node);
            add1Node.AppendChild(add1Text);

            var add2Node = xmlDoc.CreateElement(XmlElementAddress2);
            var add2Text = xmlDoc.CreateTextNode(customer.AddressLine2);
            parentNode.AppendChild(add2Node);
            add2Node.AppendChild(add2Text);

            var cityNode = xmlDoc.CreateElement(XmlElementCity);
            var cityText = xmlDoc.CreateTextNode(customer.City);
            parentNode.AppendChild(cityNode);
            cityNode.AppendChild(cityText);

            var stateNode = xmlDoc.CreateElement(XmlElementState);
            var stateText = xmlDoc.CreateTextNode(customer.State);
            parentNode.AppendChild(stateNode);
            stateNode.AppendChild(stateText);

            var zipNode = xmlDoc.CreateElement(XmlElementZip);
            var zipText = xmlDoc.CreateTextNode(customer.PostalCode);
            parentNode.AppendChild(zipNode);
            zipNode.AppendChild(zipText);

            var countryNode = xmlDoc.CreateElement(XmlElementContry);
            var countryText = xmlDoc.CreateTextNode(customer.Country);
            parentNode.AppendChild(countryNode);
            countryNode.AppendChild(countryText);
        }

        private static void EmailFillOptinalFields(CustomerData customer, XmlDocument xmlDoc, XmlNode parentNode)
        {
            Guard.NotNull(customer, nameof(customer));

            if (!string.IsNullOrWhiteSpace(customer.Title))
            {
                var titleNode = xmlDoc.CreateElement(XmlElementTitle);
                var titleText = xmlDoc.CreateTextNode(customer.Title);
                parentNode.AppendChild(titleNode);
                titleNode.AppendChild(titleText);
            }

            if (!string.IsNullOrWhiteSpace(customer.FullName))
            {
                var fullnameNode = xmlDoc.CreateElement(XmlElementFullName);
                var fullnameText = xmlDoc.CreateTextNode(customer.FullName);
                parentNode.AppendChild(fullnameNode);
                fullnameNode.AppendChild(fullnameText);
            }

            if (!string.IsNullOrWhiteSpace(customer.Occupation))
            {
                var occupationNode = xmlDoc.CreateElement(XmlElementOccupation);
                var occupationText = xmlDoc.CreateTextNode(customer.Occupation);
                parentNode.AppendChild(occupationNode);
                occupationNode.AppendChild(occupationText);
            }

            EmailFillPhone(customer, xmlDoc, parentNode);

            FillIncomeAge(customer, xmlDoc, parentNode);

            if (!string.IsNullOrWhiteSpace(customer.Gender))
            {
                var genderNode = xmlDoc.CreateElement(XmlElementGender);
                var genderText = xmlDoc.CreateTextNode(customer.Gender);
                parentNode.AppendChild(genderNode);
                genderNode.AppendChild(genderText);
            }

            if (!string.IsNullOrWhiteSpace(customer.Password))
            {
                var passwordNode = xmlDoc.CreateElement(XmlElementPassword);
                var passwordText = xmlDoc.CreateTextNode(customer.Password);
                parentNode.AppendChild(passwordNode);
                passwordNode.AppendChild(passwordText);
            }
        }

        private static void FillIncomeAge(CustomerData customer, XmlDocument xmlDoc, XmlNode parentNode)
        {
            Guard.NotNull(customer, nameof(customer));

            if (customer.Age > 0)
            {
                var ageNode = xmlDoc.CreateElement(XmlElementAge);
                var ageText = xmlDoc.CreateTextNode(customer.Age.ToString());
                parentNode.AppendChild(ageNode);
                ageNode.AppendChild(ageText);
            }

            if (customer.Income > 0)
            {
                var incomeNode = xmlDoc.CreateElement(XmlElementIncome);
                var incomeText = xmlDoc.CreateTextNode(customer.Income.ToString());
                parentNode.AppendChild(incomeNode);
                incomeNode.AppendChild(incomeText);
            }
        }

        private static void EmailFillPhone(CustomerData customer, XmlDocument xmlDoc, XmlNode parentNode)
        {
            Guard.NotNull(customer, nameof(customer));

            if (!string.IsNullOrWhiteSpace(customer.Phone))
            {
                var voiceNode = xmlDoc.CreateElement(XmlElementVoice);
                var voiceText = xmlDoc.CreateTextNode(customer.Phone);
                parentNode.AppendChild(voiceNode);
                voiceNode.AppendChild(voiceText);
            }

            if (!string.IsNullOrWhiteSpace(customer.Mobile))
            {
                var mobileNode = xmlDoc.CreateElement(XmlElementMobile);
                var mobileText = xmlDoc.CreateTextNode(customer.Mobile);
                parentNode.AppendChild(mobileNode);
                mobileNode.AppendChild(mobileText);
            }

            if (!string.IsNullOrWhiteSpace(customer.Fax))
            {
                var faxNode = xmlDoc.CreateElement(XmlElementFax);
                var faxText = xmlDoc.CreateTextNode(customer.Fax);
                parentNode.AppendChild(faxNode);
                faxNode.AppendChild(faxText);
            }

            if (!string.IsNullOrWhiteSpace(customer.Website))
            {
                var websiteNode = xmlDoc.CreateElement(XmlElementWebsite);
                var websiteText = xmlDoc.CreateTextNode(customer.Website);
                parentNode.AppendChild(websiteNode);
                websiteNode.AppendChild(websiteText);
            }
        }

        /// <summary>
        /// Copy the profile to the Master Group
        /// </summary>
        private void UpdateProfileToMasterGroup(CustomerData cd, Hashtable newsletterUDF, string accessKey)
        {
            int MasterGroupCustomerID = Convert.ToInt32(ConfigurationManager.AppSettings["MasterGroup_CustomerID"].ToString());
            int MasterGroupID = Convert.ToInt32(ConfigurationManager.AppSettings["MasterGroup_GroupID"].ToString());
            List<string> newsLetterUDF = ConfigurationManager.AppSettings["MasterGroup_Newsletter_UDFs"].ToString().ToUpper().Split(',').ToList<string>();
            int EmailID = 0;
            string SubscribeTypeCode = string.Empty;

            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Communicator"].ToString());
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
            }

            if (masterUDF.Count > 0)
            {
                ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["MasterGroup_AccessKey"].ToString(), false), MasterGroupCustomerID, MasterGroupID, EmailAddressToXML(cd), DataToXML(cd, MasterGroupID, masterUDF, accessKey), "html", "S", false);
            }
        }


        public static Hashtable GetUDFValues(int groupID, int EmailID)
        {
            Hashtable htUDFValues = new Hashtable();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Communicator"].ToString());
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
            DataTable dt = ds.Tables[1];

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

        private string DataToXML(CustomerData cd, int groupID, Hashtable hUDF, string accessKey)
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
                List<ECN_Framework_Entities.Communicator.GroupDataFields> listGDF = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
                if (groupID == Convert.ToInt32(ConfigurationManager.AppSettings["MasterGroup_GroupID"].ToString()))
                {
                    listGDF = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["MasterGroup_AccessKey"].ToString(), false));
                }
                else
                    listGDF = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, KMPlatform.BusinessLogic.User.GetByAccessKey(accessKey, false));

                IDictionaryEnumerator en = hUDF.GetEnumerator();

                while (en.MoveNext())
                {
                    ECN_Framework_Entities.Communicator.GroupDataFields gdf = listGDF.SingleOrDefault(x => x.ShortName == en.Key.ToString().ToUpper());
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