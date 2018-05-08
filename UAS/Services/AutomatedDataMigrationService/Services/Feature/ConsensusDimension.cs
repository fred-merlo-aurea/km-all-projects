using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ADMS.Services.Feature
{
    public class ConsensusDimension : ServiceBase
    {
        public void ProcessFile(KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, FileInfo fileInfo)
        {
            //on UAD call SP_IMPORT_SUBSCRIBER_MASTERCODESHEET passing @MasterGroupID int and @importXML
            //<mastergrouplist>
            //<mastergroup>
            //<igroupno>
            //<mastervalue>
            //<masterdesc>

            if (client.FtpFolder.Equals("Canon"))
            {
                #region Canon
                //IGRPNO	MASTERVALUE	MASTERDESC = generated file

                //CanonConsensusDimension.zip

                if (sourceFile.Extension.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) && fileInfo.Extension.Equals(".zip", StringComparison.CurrentCultureIgnoreCase))
                {
                    Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
                    string destinationPath = Core.ADMS.BaseDirs.getClientArchiveDir() + "\\" + client.FtpFolder + "\\ConsensusDimension\\" + DateTime.Now.ToString("MMddyyyy");
                    ff.ExtractZipFile(fileInfo, destinationPath);

                    List<FileInfo> fileList = new List<FileInfo>();
                    DirectoryInfo di = new DirectoryInfo(destinationPath);
                    fileList = di.GetFiles().ToList();
                    Core_AMS.Utilities.FileWorker fw = new Core_AMS.Utilities.FileWorker();
                    if (fileList.Exists(x => x.Name.ToLower().Contains("lead")) && fileList.Exists(x => x.Name.ToLower().Contains("exh")))
                    {
                        try
                        {
                            //Event Uid,Contract Number,Reg Uid,Exhibitor ID,Registrants Add Date,Show Code,City,Scan Date
                            //1.Read BOSTON_lead_data_file.xlsx into table > [Event Uid], [Reg Uid], [Exhibitor ID], and [Show Code]
                            DataTable dtLead = fw.GetData(fileList.First(x => x.Name.ToLower().Contains("lead")));
                            foreach (DataColumn dc in dtLead.Columns)
                                dc.ColumnName = dc.ColumnName.ToString().Trim();

                            //2.For BOSTON_lead_data_file.xlsx, assign [PUBCODE] based on show code and information from Jesse Metcalfe or Pat Connell.
                            //********going to assume PubCode will now be in the file we get
                            dtLead.Columns.Add("PubCode");
                            dtLead.AcceptChanges();
                            foreach (DataRow dr in dtLead.Rows)
                            {
                                dr["PubCode"] = dr["Show Code"].ToString() + "TESTUAD";///TO DO????

                            }
                            dtLead.AcceptChanges();


                            //Evt Uid,Exhibitor ID,Contract Number,Booth Number,Company,Address 1,Address 2,City,State,	Zipcode,Company,Phone Area Code,Phone,Fax Area Code,Fax,Internet Address
                            //3.Read BOSTON_exh_data_file.xlsx into table > [Evt Uid], [Exhibitor ID], and [Booth Number]
                            DataTable dtExh = fw.GetData(fileList.First(x => x.Name.ToLower().Contains("exh")));
                            foreach (DataColumn dc in dtExh.Columns)
                                dc.ColumnName = dc.ColumnName.ToString().Trim();
                            dtExh.AcceptChanges();
                            //4.       Join BOSTON_lead_data_file.xlsx table with BOSTON_exh_data_file.xlsx into table on [Event Uid] = [Evt Uid] and [Exhibitor ID] = [Exhibitor ID]

                            DataTable dtResult = new DataTable();
                            dtResult.TableName = "RegData";
                            dtResult.Columns.Add("PubCode", typeof(string));
                            dtResult.Columns.Add("RegID", typeof(int));
                            dtResult.Columns.Add("BoothNumber", typeof(int));
                            dtResult.AcceptChanges();

                            foreach (DataRow dr in dtExh.Rows)
                            {
                                List<DataRow> match = dtLead.Select("[Exhibitor ID] = " + dr["Exhibitor ID"].ToString() + " and [Event Uid] = " + dr["Evt Uid"].ToString()).ToList();
                                foreach (DataRow m in match)
                                {
                                    try
                                    {
                                        DataRow newDR = dtResult.NewRow();
                                        newDR["PubCode"] = m["PubCode"].ToString();
                                        newDR["RegID"] = int.Parse(m["Reg Uid"].ToString());
                                        newDR["BoothNumber"] = int.Parse(dr["Booth Number"].ToString());

                                        dtResult.Rows.Add(newDR);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogError(ex, client, this.GetType().Name.ToString() + ".ProcessFile");
                                    }
                                }
                            }
                            dtResult.AcceptChanges();

                            //want lead.PubCode, lead.Reg Uid, exhib.Booth Number
                            //send dtResult to a sproc
                            StringWriter sw = new StringWriter();
                            dtResult.WriteXml(sw);
                            string xml = Core_AMS.Utilities.XmlFunctions.CleanSerializedXML(sw.ToString());

                            //and
                            //Join with data on [PUBCODE] = [PUBCODE] and [Reg Uid] = [REGID] to create relational file for account services to upload selecting [IGRP_NO] as IGROUPNO, 
                            //concatenate([PUBCODE]+” “+[Booth Number]) as MASTERVALUE, 
                            //and concatenate([PUBCODE]+” “+[Booth Number]) as MASTERDESC.   

                            FrameworkUAS.BusinessLogic.ClientMethods cmData = new FrameworkUAS.BusinessLogic.ClientMethods();
                            cmData.Canon_ConsensusDimension_EventSwipe(xml, client);

                        }
                        catch (Exception ex)
                        {
                            LogError(ex, client, this.GetType().Name.ToString() + ".ProcessFile");
                        }
                    }
                }
                #endregion
            }
            else if (client.FtpFolder.Equals("GLM"))
            {
                #region GLM
                try
                {
                    //Scan Time,Location,Badge,RegClass,FirstName,LastName,Company,Street,Street2,City,State,Zipcode,Country,Phone,Email,SumField
                    Core_AMS.Utilities.FileWorker fw = new Core_AMS.Utilities.FileWorker();
                    DataTable dt = fw.GetData(fileInfo);
                    //1.       Read into table > [LOCATION], [BADGE], AND [EMAIL]
                    //2.       Sum occurrences of [EMAIL] and populate corresponding “new” field.  
                    //New field should be named similar to this: [SUMBADGENYNOW201308] with the “201308” in the field name changing to match what Account Services assigns based on pubcode.
                    //3.       We output a relational data file for account services to upload as a supplemental file for each [LOCATION] and [BADGE].  
                    //I’m not sure if this process will change with the new system.  Sunil may know.
                    //4.       The .csv file we output contains [IGRP_NO], [LOCATION], and [LOCATION] for the location file 
                    //and [IGRP_NO], [BADGE], and [BADGE] for the badge file.  
                    //The header row for both of these files needs to be: IGRPNO, MASTERVALUE, MASTERDESC.
                    //5.       In order to get the igrp_no data for badge & location files, we match on email within the corresponding pubcode.

                    //** We are really creating 3 ConsensusDimensions - Badge, Location, SumBadgeNYNOW201408
                    // need ActivityDescription, ActivityResponse for Badge/BadgeValue, Location/LocationValue, SUMBADGENYNOW201408/Count of email

                    List<FrameworkUAD.Object.ConsensusDimension> badgeList = new List<FrameworkUAD.Object.ConsensusDimension>();
                    List<FrameworkUAD.Object.ConsensusDimension> locationList = new List<FrameworkUAD.Object.ConsensusDimension>();
                    List<FrameworkUAD.Object.ConsensusDimension> emailList = new List<FrameworkUAD.Object.ConsensusDimension>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        FrameworkUAD.Object.ConsensusDimension cdBadge = new FrameworkUAD.Object.ConsensusDimension();
                        FrameworkUAD.Object.ConsensusDimension cdLocation = new FrameworkUAD.Object.ConsensusDimension();
                        FrameworkUAD.Object.ConsensusDimension cdEmail = new FrameworkUAD.Object.ConsensusDimension();

                        cdBadge.ActivityDescription = "Badge";
                        cdBadge.ActivityResponse = dr["Badge"].ToString();
                        cdBadge.Address1 = dr["Street"].ToString();
                        cdBadge.Address2 = dr["Street2"].ToString();
                        cdBadge.City = dr["City"].ToString();
                        cdBadge.Company = dr["Company"].ToString();
                        cdBadge.Country = dr["Country"].ToString();
                        cdBadge.Email = dr["Email"].ToString();
                        cdBadge.FirstName = dr["FirstName"].ToString();
                        cdBadge.LastName = dr["LastName"].ToString();
                        cdBadge.Phone = dr["Phone"].ToString();
                        cdBadge.State = dr["State"].ToString();
                        cdBadge.Zipcode = dr["Zipcode"].ToString();

                        badgeList.Add(cdBadge);

                        cdLocation.ActivityDescription = "Location";
                        cdLocation.ActivityResponse = dr["Location"].ToString();
                        cdLocation.Address1 = dr["Street"].ToString();
                        cdLocation.Address2 = dr["Street2"].ToString();
                        cdLocation.City = dr["City"].ToString();
                        cdLocation.Company = dr["Company"].ToString();
                        cdLocation.Country = dr["Country"].ToString();
                        cdLocation.Email = dr["Email"].ToString();
                        cdLocation.FirstName = dr["FirstName"].ToString();
                        cdLocation.LastName = dr["LastName"].ToString();
                        cdLocation.Phone = dr["Phone"].ToString();
                        cdLocation.State = dr["State"].ToString();
                        cdLocation.Zipcode = dr["Zipcode"].ToString();

                        locationList.Add(cdLocation);

                        cdEmail.ActivityDescription = dr["SumField"].ToString();
                        cdEmail.ActivityResponse = "0";
                        cdEmail.Address1 = dr["Street"].ToString();
                        cdEmail.Address2 = dr["Street2"].ToString();
                        cdEmail.City = dr["City"].ToString();
                        cdEmail.Company = dr["Company"].ToString();
                        cdEmail.Country = dr["Country"].ToString();
                        cdEmail.Email = dr["Email"].ToString();
                        cdEmail.FirstName = dr["FirstName"].ToString();
                        cdEmail.LastName = dr["LastName"].ToString();
                        cdEmail.Phone = dr["Phone"].ToString();
                        cdEmail.State = dr["State"].ToString();
                        cdEmail.Zipcode = dr["Zipcode"].ToString();

                        emailList.Add(cdEmail);
                    }

                    var emailGroup = (from b in emailList
                                      group b by b.Email into e
                                      select e);

                    int groupCount = emailGroup.Count();

                    List<FrameworkUAD.Object.ConsensusDimension> groupedEmailList = new List<FrameworkUAD.Object.ConsensusDimension>();
                    foreach (var email in emailGroup)
                    {
                        foreach (var x in email)
                        {
                            FrameworkUAD.Object.ConsensusDimension cdEmail = new FrameworkUAD.Object.ConsensusDimension();

                            cdEmail.ActivityDescription = x.ActivityDescription;
                            cdEmail.ActivityResponse = email.Count().ToString();
                            cdEmail.Address1 = x.Address1;
                            cdEmail.Address2 = x.Address2;
                            cdEmail.City = x.City;
                            cdEmail.Company = x.Company;
                            cdEmail.Country = x.Country;
                            cdEmail.Email = x.Email;
                            cdEmail.FirstName = x.FirstName;
                            cdEmail.LastName = x.LastName;
                            cdEmail.Phone = x.Phone;
                            cdEmail.State = x.State;
                            cdEmail.Zipcode = x.Zipcode;

                            groupedEmailList.Add(cdEmail);
                            break;
                        }
                    }

                    //Master_BrandAssociation
                    //Master_Badge_Swipe
                    //Master_Location_Swipe
                    FrameworkUAD.BusinessLogic.MasterGroup mgData = new FrameworkUAD.BusinessLogic.MasterGroup();
                    List<FrameworkUAD.Entity.MasterGroup> mgList = mgData.Select(client.ClientConnections);

                    int badgeMG = 0;
                    int locMG = 0;
                    int groupMG = 0;
                    int.TryParse(mgList.SingleOrDefault(x => x.ColumnReference.Equals("Master_Badge_Swipe", StringComparison.CurrentCultureIgnoreCase)).MasterGroupID.ToString(), out badgeMG);
                    int.TryParse(mgList.SingleOrDefault(x => x.ColumnReference.Equals("Master_Location_Swipe", StringComparison.CurrentCultureIgnoreCase)).MasterGroupID.ToString(), out locMG);
                    int.TryParse(mgList.SingleOrDefault(x => x.ColumnReference.Equals("Master_BrandAssociation", StringComparison.CurrentCultureIgnoreCase)).MasterGroupID.ToString(), out groupMG);

                    FrameworkUAD.BusinessLogic.ConsensusDimension cdData = new FrameworkUAD.BusinessLogic.ConsensusDimension();
                    cdData.SaveXML(badgeList, badgeMG, client.ClientConnections);
                    cdData.SaveXML(locationList, locMG, client.ClientConnections);
                    cdData.SaveXML(groupedEmailList, groupMG, client.ClientConnections);
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ProcessFile");
                }
                #endregion
            }
            else if (client.FtpFolder.Equals("SpecialityFoods"))//NASFT
            {
                #region SpecialityFoods
                try
                {
                    //Relational Data Special File for specific tradeshow pubcode.  Relational Data Special File for specific tradeshow pubcode.  There are two tradeshows each year and two files that will be relational to the new tradeshow’s pubcode only.
                    //1.       Read Show_Winter_2013_Dir.xlsx  into table > [BoothLabel], [SubProdCatID]
                    //2.       Read wff13_lead_export.xlsx into table > [Exh#], [Booth#], [RegNumb]
                    //3.       Join Read wff13_lead_export.xlsx with data on [RegNumb] for corresponding [PUBCODE] to create relational file for account services to upload selecting distinct values: [igrp_no] as IGROUPNO, [BOOTH#] as MASTERVALUE, [BOOTH#] as MASTERDESC.  Output as .csv for booth.
                    //4.       Join Read wff13_lead_export.xlsx  with data on [RegNumb] for corresponding [PUBCODE] to create relational file for account services to upload selecting distinct values: [igrp_no] as IGROPNO, [Exh#] AS MASTERVALUE, [Exh#] as MASTERDESC.  Output as .csv for Exhibitor.
                    //5.       Join Read wff13_lead_export.xlsx  with data on [RegNumb] for corresponding [PUBCODE] 
                    //And
                    //Join with Read Show_Winter_2013_Dir.xlsx  on [BoothLabel] = [Booth#]  to create relational file for account services to upload 
                    //selecting distinct values: [igrp_no] as IGROUPNO, [SubProdCatID] as MASTERVALUE, [SubProdCatID] as MASTERDESC.  Output as .csv for DIRCATID.
                    if (sourceFile.Extension.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) && fileInfo.Extension.Equals(".zip", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Core_AMS.Utilities.FileFunctions ff = new Core_AMS.Utilities.FileFunctions();
                        string destinationPath = Core.ADMS.BaseDirs.getClientArchiveDir() + "\\" + client.FtpFolder + "\\ConsensusDimension\\" + DateTime.Now.ToString("MMddyyyy");
                        ff.ExtractZipFile(fileInfo, destinationPath);

                        List<FileInfo> fileList = new List<FileInfo>();
                        DirectoryInfo di = new DirectoryInfo(destinationPath);
                        fileList = di.GetFiles().ToList();
                        if (fileList.Exists(x => x.Name.ToLower().Contains("show")) && fileList.Exists(x => x.Name.ToLower().Contains("lead")))
                        {
                            //BoothLabel,CompanyName,SubProdCatID,SubProdCatText1,KM MAF Code,  ----Show file
                            Core_AMS.Utilities.FileWorker fw = new Core_AMS.Utilities.FileWorker();
                            DataTable dtBooth = fw.GetData(fileList.First(x => x.Name.ToLower().Contains("show")));//small ref file 5783 rows
                            foreach (DataRow dr in dtBooth.Rows)
                            {
                                foreach (DataColumn dc in dtBooth.Columns)
                                {
                                    dr[dc.ColumnName] = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(dr[dc.ColumnName].ToString());
                                }
                            }
                            dtBooth.AcceptChanges();

                            //Event,Exh#,Comp 1,Comp 2,Booth#,RegNumb,OrgSeq,Asset,Date
                            //RegNumb is Subscriptions.Sequence
                            DataTable dtEvent = fw.GetData(fileList.First(x => x.Name.ToLower().Contains("lead")));//big file 113,275 rows
                            foreach (DataRow dr in dtEvent.Rows)
                            {
                                foreach (DataColumn dc in dtEvent.Columns)
                                {
                                    dr[dc.ColumnName] = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(dr[dc.ColumnName].ToString());
                                }
                            }
                            dtEvent.AcceptChanges();

                            //create 3 files Booth, Exh, SubProdCat

                            DataTable dtResult = new DataTable();
                            dtResult.TableName = "RegData";
                            dtResult.Columns.Add("Booth", typeof(string));//event
                            dtResult.Columns.Add("Exh", typeof(int));//event
                            dtResult.Columns.Add("SubProdCatID", typeof(int));//ref file - show
                            dtResult.Columns.Add("RegNumb", typeof(int));//event
                            dtResult.AcceptChanges();

                            //get the PubCode
                            int rowindex = 0;
                            foreach (DataRow dr in dtEvent.Rows)
                            {
                                try
                                {
                                    List<DataRow> match = dtBooth.Select("[BoothLabel] = '" + dr["Booth#"].ToString() + "'").ToList();
                                    foreach (DataRow m in match)
                                    {
                                        try
                                        {
                                            DataRow newDR = dtResult.NewRow();
                                            newDR["Booth"] = dr["Booth#"].ToString();
                                            newDR["Exh"] = int.Parse(dr["Exh#"].ToString());
                                            newDR["SubProdCatID"] = int.Parse(m["SubProdCatID#"].ToString());
                                            newDR["RegNumb"] = int.Parse(dr["RegNumb"].ToString());

                                            dtResult.Rows.Add(newDR);
                                        }
                                        catch (Exception ex)
                                        {
                                            LogError(ex, client, this.GetType().Name.ToString() + ".ProcessFile");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogError(ex, client, this.GetType().Name.ToString() + ".ProcessFile");
                                }
                                rowindex++;

                            }
                            dtResult.AcceptChanges();

                            StringWriter sw = new StringWriter();
                            dtResult.WriteXml(sw);
                            string xml = Core_AMS.Utilities.XmlFunctions.CleanSerializedXML(sw.ToString());

                            FrameworkUAS.BusinessLogic.ClientMethods cmData = new FrameworkUAS.BusinessLogic.ClientMethods();
                            cmData.SpecialityFoods_ConsensusDimension_EventSwipe(xml, client);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, client, this.GetType().Name.ToString() + ".ProcessFile");
                }
                #endregion SpecialityFoods
            }
            else
            {
                #region Standard
                //default IGRPNO,MASTERVALUE,MASTERDESC
                //CSV file, Quote Encapsulated
                //FirstName,LastName,Company,Address1,Address2,City,State,Zipcode,Country,Phone,Email,ActivityDescription,ActivityResponse
                Core_AMS.Utilities.FileWorker fw = new Core_AMS.Utilities.FileWorker();
                DataTable dt = fw.GetData(fileInfo);
                List<FrameworkUAD.Object.ConsensusDimension> retList = Core_AMS.Utilities.DataTableFunctions.ConvertToList<FrameworkUAD.Object.ConsensusDimension>(dt);
                //XmlDocument xDoc = Core_AMS.Utilities.XmlFunctions.GetXML<List<FrameworkUAS.Object.ConsensusDimension>>(retList, "ConDimensions");
                //string xml = Core_AMS.Utilities.XmlFunctions.CleanSerializedXML(xDoc.InnerXml.ToString());
                FrameworkUAD.BusinessLogic.ConsensusDimension cdData = new FrameworkUAD.BusinessLogic.ConsensusDimension();
                cdData.SaveXML(retList, sourceFile.MasterGroupID, client.ClientConnections);
                #endregion
            }
        }
    }
}
