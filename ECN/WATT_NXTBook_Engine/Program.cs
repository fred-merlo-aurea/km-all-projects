using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Security;
using System.Data;
using WATT_NXTBook_Engine.NXTBookAPI.BusinessLogic;
using WATT_NXTBook_Engine.NXTBookAPI.Entity;
using ECN_Framework_BusinessLayer.Communicator;

namespace WATT_NXTBook_Engine
{
    class Program
    {
        private const string GDFShortName = "nxtbooktoken";
        private const string No = "N";
        private const string SettingApplication = "KMCommon_Application";
        private const string SettingAccessKey = "NXTBook_AccessKey";
        private const string SettingSubIdFormat = "{0}_SubID";
        private const string ColumnDataValue = "DataValue";
        private const string ColumnEmailAddress = "EmailAddress";
        private const string AccessTypeUnlimited = "unlimited";
        private const string ExceptionDataError = "Error";
        private const string ErrorTypeAccess = "access";
        public static StreamWriter logFile;
        public static KMPlatform.Entity.User user;
        static void Main(string[] args)
        {
            logFile = new StreamWriter(new FileStream(ConfigurationManager.AppSettings["LogPath"] + DateTime.Now.ToString("MM-dd-yyyy") + ".log", System.IO.FileMode.Append));
            writeToLog(""); writeToLog("");
            writeToLog("-Start-----------------");
            user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["AccessKey"].ToString(), false);
            bool DoFullSync = Convert.ToBoolean(ConfigurationManager.AppSettings["DoFullSync"].ToString());
            bool DoJob1 = Convert.ToBoolean(ConfigurationManager.AppSettings["DoJob1"].ToString());
            bool DoJob2 = Convert.ToBoolean(ConfigurationManager.AppSettings["DoJob2"].ToString());

            DateTime? dateFrom = DateTime.Now.AddDays(-1);
            if (DoFullSync)
                dateFrom = null;


            //Get subscription groups
            string[] subGroups = ConfigurationManager.AppSettings["SubGroups"].ToString().Split(',');
            if (DoJob1)
            {
                writeToLog("-----Starting Job 1");
                try
                {
                    string Field = ConfigurationManager.AppSettings["Field"].ToString();
                    string FieldValue = ConfigurationManager.AppSettings["FieldValue"].ToString();
                    //loop through sub groups
                    foreach (string s in subGroups)
                    {
                        writeToLog("-----Starting GroupID:" + s);
                        try
                        {
                            //get target groupid
                            int targetGroup = Convert.ToInt32(ConfigurationManager.AppSettings["Sub_" + s].ToString());

                            DataTable emailList = ECN_Framework_BusinessLayer.Communicator.Email.GetEmailsForWATT_NXTBookSync(Convert.ToInt32(s), true, dateFrom, Field, FieldValue);

                            DoImport(emailList, targetGroup);
                        }
                        catch (Exception ex1)
                        {
                            //catch individual group errors so the sync can run for other groups
                            KM.Common.Entity.ApplicationLog.LogCriticalError(ex1, "WATT_NXTBookEngine.Job1.LoopThroughGroups", Convert.ToInt32(ConfigurationManager.AppSettings[SettingApplication].ToString()));
                            writeToLog("-----ERROR");
                            writeToLog(ex1.Message);
                            writeToLog(ex1.StackTrace);
                        }

                        writeToLog("-----Done with GroupID:" + s);
                    }
                }
                catch (Exception ex)
                {
                    //catch 
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "WATT_NXTBookEngine.Job1.MainLoop", Convert.ToInt32(ConfigurationManager.AppSettings[SettingApplication].ToString()));
                    writeToLog("-----ERROR");
                    writeToLog(ex.Message);
                    writeToLog(ex.StackTrace);
                }
                writeToLog("-----Done with Job 1");
            }
            else
            {
                writeToLog("-----Skipping Job1");
            }
            //Done with Job#1 syncing field values
            //Start Job#2 - get subscribers that don't have NXTBook token, create one and then post to NXTBook

            if (DoJob2)
            {
                writeToLog("-----Starting Job 2");
                try
                {
                    string Field2 = ConfigurationManager.AppSettings["Field2"].ToString();
                    string FieldValue2 = ConfigurationManager.AppSettings["FieldValue2"].ToString();

                    foreach (string s in subGroups)
                    {
                        try
                        {
                            //get target groupid
                            int targetGroup = Convert.ToInt32(ConfigurationManager.AppSettings["Sub_" + s].ToString());
                            bool doFullNXTBookSync = Convert.ToBoolean(ConfigurationManager.AppSettings["DoFullNXTBookSync"].ToString());
                            DataTable emailListForNXTBook = ECN_Framework_BusinessLayer.Communicator.Email.GetEmailsForWATT_NXTBookSync(targetGroup, false, dateFrom, Field2, FieldValue2, doFullNXTBookSync);

                            DoNXTBookPost(emailListForNXTBook, targetGroup);


                        }
                        catch (Exception ex1)
                        {
                            KM.Common.Entity.ApplicationLog.LogCriticalError(ex1, "WATT_NXTBookEngine.Job2.LoopThroughGroups", Convert.ToInt32(ConfigurationManager.AppSettings[SettingApplication].ToString()));
                            writeToLog("-----ERROR");
                            writeToLog(ex1.Message);
                            writeToLog(ex1.StackTrace);
                        }

                    }
                    writeToLog("-----Done with Job 2");
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "WATT_NXTBookEngine.Job2.MainLoop", Convert.ToInt32(ConfigurationManager.AppSettings[SettingApplication].ToString()));
                    writeToLog("-----ERROR");
                    writeToLog(ex.Message);
                    writeToLog(ex.StackTrace);
                }
            }
            else
            {
                writeToLog("-----Skipping Job2");
            }

        }

        private static void DoImport(DataTable emailList, int targetGroupID)
        {
            int currentGDFID = -1;
            //string gdfShortName = "Mozaic_Issue_" + i.ID;
            string gdfShortName = "nxtbooktoken";
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(targetGroupID);
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID_NoAccessCheck(targetGroupID);
            if (gdfList.Count == 0 || !gdfList.Exists(x => x.ShortName == gdfShortName))
            {
                ECN_Framework_Entities.Communicator.GroupDataFields newGDF = new ECN_Framework_Entities.Communicator.GroupDataFields();
                newGDF.CustomerID = group.CustomerID;
                newGDF.GroupID = group.GroupID;
                newGDF.IsDeleted = false;
                newGDF.IsPrimaryKey = false;
                newGDF.IsPublic = No;
                newGDF.LongName = gdfShortName;
                newGDF.ShortName = gdfShortName;
                newGDF.CreatedUserID = user.UserID;
                try
                {
                    currentGDFID = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save_NoAccessCheck(newGDF, user);
                    writeToLog("------Creating new UDF for Group: " + group.GroupID.ToString());
                }
                catch (Exception ex)
                {
                    writeToLog("******Error: Couldn't create new UDF for Group: " + group.GroupID.ToString());
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "WATT_NXTBookEngine.Job1.CreateNxtbookUDF", Convert.ToInt32(ConfigurationManager.AppSettings[SettingApplication].ToString()));
                }
            }
            else
            {

                currentGDFID = gdfList.First(x => x.ShortName == gdfShortName).GroupDataFieldsID;
                writeToLog("------UDF already exists: " + currentGDFID.ToString());
            }

            int batchCount = 0;
            StringBuilder sbEmail = new StringBuilder();
            StringBuilder sbUDFValues = new StringBuilder();
            writeToLog("-----Total Records being updated: " + emailList.Rows.Count.ToString());
            writeToLog("------Start batching of emails for update");
            foreach (DataRow e in emailList.Rows)
            {

                sbEmail.Append("<Emails>");
                sbEmail.Append("<emailaddress>" + SecurityElement.Escape(e[ColumnEmailAddress].ToString()) + "</emailaddress>");
                sbEmail.Append("</Emails>");

                sbUDFValues.Append("<row>");
                sbUDFValues.Append("<ea>" + SecurityElement.Escape(e[ColumnEmailAddress].ToString()) + "</ea>");
                sbUDFValues.Append("<udf id=\"" + currentGDFID.ToString() + "\">");
                sbUDFValues.Append("<v><![CDATA[" + e["User1"].ToString() + "]]></v>");
                sbUDFValues.Append("</udf>");
                sbUDFValues.Append("</row>");


                if ((batchCount != 0) && (batchCount % 1000 == 0) || (batchCount == emailList.Rows.Count - 1))
                {
                    writeToLog("------Updating batch of:" + batchCount.ToString() + " " + DateTime.Now.ToString());
                    UpdateToDB(group.GroupID, group.CustomerID, user, sbEmail.ToString(), sbUDFValues.ToString());
                    sbEmail = new StringBuilder();
                    sbUDFValues = new StringBuilder();
                }

                batchCount++;

            }
        }

        private static void DoNXTBookPost(DataTable emailList, int targetGroupID)
        {
            // Get nxtbooktoken gdfid, if it hasn't been created then create it
            var currentGDFID = -1;
            var group = Group.GetByGroupID_NoAccessCheck(targetGroupID);
            var gdfList = GroupDataFields.GetByGroupID_NoAccessCheck(targetGroupID);

            currentGDFID = CreateGroupDataFields(currentGDFID, group, gdfList);

            // loop through and create password and post to nxtbook
            var nxtBookAccessKey = ConfigurationManager.AppSettings[SettingAccessKey];
            var emailBuilder = new StringBuilder();
            var udfValuesBuilder = new StringBuilder();
            var batchCount = 0;
            var subscriptionID = ConfigurationManager.AppSettings[string.Format(SettingSubIdFormat, targetGroupID.ToString())];

            foreach (DataRow e in emailList.Rows)
            {
                try
                {
                    var nxtBookPassword = string.Empty;
                    if (e[ColumnDataValue] != null && string.IsNullOrWhiteSpace(e[ColumnDataValue].ToString()))
                    {
                        nxtBookPassword = CreatePassword(16);
                    }
                    else
                    {
                        nxtBookPassword = e[ColumnDataValue].ToString();
                    }

                    var profile = new drmProfile
                    {
                        email = e[ColumnEmailAddress].ToString().Trim(),
                        password = nxtBookPassword,
                        subscriptionid = subscriptionID,
                        access_type = AccessTypeUnlimited
                    };

                    // do nxtbook post
                    try
                    {
                        if (drmRestAPI.SetProfile(nxtBookAccessKey, profile))
                        {
                            // successful - do email marketing update
                            UpdateEmailMarketing(emailList, currentGDFID, group, emailBuilder, udfValuesBuilder, batchCount, e, profile);
                        }
                    }
                    catch (Exception nxtBookEXC)
                    {
                        if (nxtBookEXC.Data.Count > 0)
                        {
                            var error = (drmError)nxtBookEXC.Data[ExceptionDataError];
                            if (error.faultString.IndexOf(ErrorTypeAccess, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                // subscriber has access, get password
                                profile = drmRestAPI.GetProfile(nxtBookAccessKey, subscriptionID, e[ColumnEmailAddress].ToString().Trim());
                                if (profile != null && !string.IsNullOrWhiteSpace(profile.password))
                                {
                                    // successful - do email marketing update
                                    UpdateEmailMarketing(emailList, currentGDFID, group, emailBuilder, udfValuesBuilder, batchCount, e, profile);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "WATT_NXTBookEngine.Job2.DoNXTBookPost", Convert.ToInt32(ConfigurationManager.AppSettings[SettingApplication].ToString()));
                }

                batchCount++;
            }
        }

        private static int CreateGroupDataFields(int currentGDFID, ECN_Framework_Entities.Communicator.Group group, List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList)
        {
            if (gdfList.Count == 0 || !gdfList.Exists(x => x.ShortName == GDFShortName))
            {
                var newGDF = new ECN_Framework_Entities.Communicator.GroupDataFields
                {
                    CustomerID = group.CustomerID,
                    GroupID = group.GroupID,
                    IsDeleted = false,
                    IsPrimaryKey = false,
                    IsPublic = No,
                    LongName = GDFShortName,
                    ShortName = GDFShortName,
                    CreatedUserID = user.UserID
                };

                try
                {
                    currentGDFID = GroupDataFields.Save(newGDF, user);
                    writeToLog("------Creating new UDF for Group: " + group.GroupID.ToString());
                }
                catch (Exception ex)
                {
                    writeToLog("******Error: Couldn't create new UDF for Group: " + group.GroupID.ToString());
                    KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "WATT_NXTBookEngine.Job2.CreateNxtbookUDF", Convert.ToInt32(ConfigurationManager.AppSettings[SettingApplication]));
                }
            }
            else
            {
                currentGDFID = gdfList.First(x => x.ShortName == GDFShortName).GroupDataFieldsID;
                writeToLog("------UDF already exists: " + currentGDFID.ToString());
            }

            return currentGDFID;
        }

        private static void UpdateEmailMarketing(DataTable emailList, int currentGDFID, ECN_Framework_Entities.Communicator.Group group, StringBuilder emailBuilder, StringBuilder udfValuesBuilder, int batchCount, DataRow e, drmProfile profile)
        {
            emailBuilder.Append("<Emails>");
            emailBuilder.Append("<emailaddress>" + SecurityElement.Escape(e[ColumnEmailAddress].ToString().Trim()) + "</emailaddress>");
            emailBuilder.Append("</Emails>");

            udfValuesBuilder.Append("<row>");
            udfValuesBuilder.Append("<ea>" + SecurityElement.Escape(e[ColumnEmailAddress].ToString().Trim()) + "</ea>");
            udfValuesBuilder.Append("<udf id=\"" + currentGDFID.ToString() + "\">");
            udfValuesBuilder.Append("<v><![CDATA[" + profile.password + "]]></v>");
            udfValuesBuilder.Append("</udf>");
            udfValuesBuilder.Append("</row>");

            if ((batchCount != 0) && (batchCount % 1000 == 0) || (batchCount == emailList.Rows.Count - 1))
            {
                writeToLog("------Updating batch of:" + batchCount.ToString() + " " + DateTime.Now.ToString());
                UpdateToDB(group.GroupID, group.CustomerID, user, emailBuilder.ToString(), udfValuesBuilder.ToString());
                emailBuilder.Clear();
                udfValuesBuilder.Clear();
            }
        }

        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private static void UpdateToDB(int groupID, int customerID, KMPlatform.Entity.User user, string xmlEmail, string xmlUDF)
        {
            DataTable dtResults = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(user, customerID, groupID,
                "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlEmail.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>",
                "HTML", "S", false, "", "WATT_NXTBook_Engine.Program.UpdateToDB");

        }


        #region Write to log file.
        //Write to log file
        public static void writeToLog(string text)
        {

            Console.WriteLine(text);
            logFile.AutoFlush = true;
            logFile.WriteLine(DateTime.Now.ToString() + " " + text);
            logFile.Flush();
        }
        #endregion
    }
}
