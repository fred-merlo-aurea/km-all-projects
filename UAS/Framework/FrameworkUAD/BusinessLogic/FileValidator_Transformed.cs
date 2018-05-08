using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class FileValidator_Transformed
    {
        public bool SaveBulkSqlInsert(List<Entity.FileValidator_Transformed> list, KMPlatform.Object.ClientConnections client)
        {
            bool done = true;
            int BatchSize = 1000;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            int checkCount = 1;
            string processCode = list.FirstOrDefault().ProcessCode;
            foreach (Entity.FileValidator_Transformed x in list)
            {
                try
                {
                    //string msg = "Checking Transformed Subscriber: " + checkCount.ToString() + " of " + total.ToString();
                    //Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);
                    FormatData(x);
                }
                catch (Exception ex)
                {
                    string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                    fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, processCode));
                }
                checkCount++;
            }

            List<Entity.FileValidator_Transformed> bulkProcessList = new List<Entity.FileValidator_Transformed>();
            foreach (Entity.FileValidator_Transformed x in list)
            {
                counter++;
                processedCount++;
                done = false;
                bulkProcessList.Add(x);
                if (processedCount == total || counter == BatchSize)
                {
                    try
                    {
                        done = DataAccess.FileValidator_Transformed.SaveBulkSqlInsert(bulkProcessList, client);
                        if (done == true)
                        {
                            FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                            fl.Save(new FrameworkUAS.Entity.FileLog(x.SourceFileID, -99, "Start Bulk Insert  SubscriberDemographicTransformed : processed count = " + processedCount.ToString(), x.ProcessCode));
                            done = DataAccess.FileValidator_DemographicTransformed.SaveBulkSqlInsert(bulkProcessList, client);// CHANGE: will not work until we replace SubscriberOriginalID with RecordIdentifier column from SubscriberOriginal
                            fl.Save(new FrameworkUAS.Entity.FileLog(x.SourceFileID, -99, "End Bulk Insert SubscriberDemographicTransformed : processed count = " + processedCount.ToString(), x.ProcessCode));
                        }
                    }
                    catch (Exception ex)
                    {
                        done = false;
                        string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                        FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                        fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, processCode));
                    }
                    counter = 0;
                    bulkProcessList = new List<Entity.FileValidator_Transformed>();
                }
            }

            return done;
        }
        public void FormatData(Entity.FileValidator_Transformed x)
        {
            try
            {
                //x = PopulateNull(x);

                //if (x.STRecordIdentifier == Guid.Empty)
                //    x.STRecordIdentifier = Guid.NewGuid();
                //if (x.Phone != null)
                //    x.Phone = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Phone);
                //if (x.Mobile != null)
                //    x.Mobile = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Mobile);
                //if (x.Fax != null)
                //    x.Fax = Core_AMS.Utilities.StringFunctions.FormatPhoneNumbersOnly(x.Fax);
                //if (x.Email != null && x.Email.Length <= 4)
                //    x.Email = string.Empty;

                #region truncate strings
                if (x.PubCode != null && x.PubCode.Length > 100)
                    x.PubCode = x.PubCode.Substring(0, 100);
                if (x.FName != null && x.FName.Length > 100)
                    x.FName = x.FName.Substring(0, 100);
                if (x.LName != null && x.LName.Length > 100)
                    x.LName = x.LName.Substring(0, 100);
                if (x.Title != null && x.Title.Length > 100)
                    x.Title = x.Title.Substring(0, 100);
                if (x.Company != null && x.Company.Length > 100)
                    x.Company = x.Company.Substring(0, 100);
                if (x.Address != null && x.Address.Length > 255)
                    x.Address = x.Address.Substring(0, 255);
                if (x.MailStop != null && x.MailStop.Length > 255)
                    x.MailStop = x.MailStop.Substring(0, 255);
                if (x.City != null && x.City.Length > 50)
                    x.City = x.City.Substring(0, 50);
                if (x.State != null && x.State.Length > 50)
                    x.State = x.State.Substring(0, 50);
                if (x.Zip != null && x.Zip.Length > 50)
                    x.Zip = x.Zip.Substring(0, 50);
                if (x.Plus4 != null && x.Plus4.Length > 50)
                    x.Plus4 = x.Plus4.Substring(0, 50);
                if (x.ForZip != null && x.ForZip.Length > 50)
                    x.ForZip = x.ForZip.Substring(0, 50);
                if (x.County != null && x.County.Length > 100)
                    x.County = x.County.Substring(0, 100);
                if (x.Country != null && x.Country.Length > 100)
                    x.Country = x.Country.Substring(0, 100);
                if (x.Email != null && x.Email.Length > 100)
                    x.Email = x.Email.Substring(0, 100);
                if (x.RegCode != null && x.RegCode.Length > 5)
                    x.RegCode = x.RegCode.Substring(0, 5);
                if (x.Verified != null && x.Verified.Length > 100)
                    x.Verified = x.Verified.Substring(0, 100);
                if (x.SubSrc != null && x.SubSrc.Length > 50)
                    x.SubSrc = x.SubSrc.Substring(0, 50);
                if (x.OrigsSrc != null && x.OrigsSrc.Length > 50)
                    x.OrigsSrc = x.OrigsSrc.Substring(0, 50);
                if (x.Par3C != null && x.Par3C.Length > 10)
                    x.Par3C = x.Par3C.Substring(0, 10);
                if (x.Source != null && x.Source.Length > 50)
                    x.Source = x.Source.Substring(0, 50);
                if (x.Priority != null && x.Priority.Length > 4)
                    x.Priority = x.Priority.Substring(0, 4);
                if (x.Sic != null && x.Sic.Length > 8)
                    x.Sic = x.Sic.Substring(0, 8);
                if (x.SicCode != null && x.SicCode.Length > 20)
                    x.SicCode = x.SicCode.Substring(0, 20);
                if (x.Gender != null && x.Gender.Length > 1024)
                    x.Gender = x.Gender.Substring(0, 1024);
                if (x.IGrp_Rank != null && x.IGrp_Rank.Length > 2)
                    x.IGrp_Rank = x.IGrp_Rank.Substring(0, 2);
                if (x.CGrp_Rank != null && x.CGrp_Rank.Length > 2)
                    x.CGrp_Rank = x.CGrp_Rank.Substring(0, 2);
                if (x.Address3 != null && x.Address3.Length > 255)
                    x.Address3 = x.Address3.Substring(0, 255);
                if (x.Home_Work_Address != null && x.Home_Work_Address.Length > 10)
                    x.Home_Work_Address = x.Home_Work_Address.Substring(0, 10);
                if (x.PubIDs != null && x.PubIDs.Length > 2000)
                    x.PubIDs = x.PubIDs.Substring(0, 2000);
                if (x.Demo7 != null && x.Demo7.Length > 1)
                    x.Demo7 = x.Demo7.Substring(0, 1);
                if (x.LatLonMsg != null && x.LatLonMsg.Length > 500)
                    x.LatLonMsg = x.LatLonMsg.Substring(0, 500);
                #endregion

                if (x.QDate == DateTime.Parse("0001-01-01T00:00:00") || x.QDate == DateTime.MinValue || x.QDate <= DateTime.Parse("1/1/1900"))
                    x.QDate = DateTime.Now;
                if (x.StatusUpdatedDate == DateTime.Parse("0001-01-01T00:00:00") || x.StatusUpdatedDate == DateTime.MinValue || x.StatusUpdatedDate <= DateTime.Parse("1/1/1900"))
                    x.StatusUpdatedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message, "FormatData"));
            }
        }
        private Entity.FileValidator_Transformed PopulateNull(Entity.FileValidator_Transformed so)
        {
            #region FileValidator_Transformed
            //Type type = so.GetType();
            //PropertyInfo[] properties = type.GetProperties();
            //foreach (PropertyInfo property in properties)
            //{
            //    string propType = property.PropertyType.ToString();

            //    if (property.GetValue(so, null) == null)
            //    {
            //        if (propType.Equals("System.Int32", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            property.SetValue(so, -1);
            //        }
            //        else if (propType.Equals("System.Guid", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            property.SetValue(so, Guid.NewGuid());
            //        }
            //        else if (propType.Equals("System.DateTime", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            DateTime dtColValue = DateTime.Parse("1/1/1900");
            //            property.SetValue(so, dtColValue);
            //        }
            //        else if (propType.Equals("System.Boolean", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            property.SetValue(so, false);
            //        }
            //        else if (propType.Equals("System.Double", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            property.SetValue(so, -1);
            //        }
            //        else if (propType.Equals("System.Decimal", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            property.SetValue(so, -1);
            //        }
            //        else if (propType.Equals("System.Nullable`1[System.DateTime]"))
            //        {
            //            DateTime dtColValue = DateTime.Parse("1/1/1900");
            //            property.SetValue(so, dtColValue);
            //        }
            //        else if (propType.Equals("System.Nullable`1[System.Int32]"))
            //        {
            //            property.SetValue(so, -1);
            //        }
            //        else
            //        {
            //            property.SetValue(so, string.Empty);
            //        }
            //    }

            //    if (propType.Equals("System.String", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        try
            //        {
            //            string value = Core_AMS.Utilities.XmlFunctions.FormatXMLSpecialCharacters(property.GetValue(so).ToString().Trim());
            //            property.SetValue(so, value);
            //        }
            //        catch { }
            //    }

            //    if (propType.Equals("System.DateTime", StringComparison.CurrentCultureIgnoreCase) || propType.Equals("System.Nullable`1[System.DateTime]", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        try
            //        {
            //            string value = property.GetValue(so).ToString();
            //            DateTime shortDate = DateTime.Parse(value);
            //            if (shortDate == DateTime.Parse("0001-01-01T00:00:00") || shortDate == DateTime.MinValue || shortDate < DateTime.Parse("1/1/1900"))
            //            {
            //                shortDate = DateTime.Parse("1/1/1900");
            //                property.SetValue(so, shortDate);
            //            }
            //        }
            //        catch { }
            //    }
            //}
            #endregion
            #region SubscriberDemographicOriginal
            //foreach (Entity.FileValidator_DemographicTransformed sdt in so.FV_DemographicTransformedList)
            //{
            //    Type sdtType = sdt.GetType();
            //    PropertyInfo[] sdtProperties = sdtType.GetProperties();
            //    foreach (PropertyInfo sdtProperty in sdtProperties)
            //    {
            //        string propType = sdtProperty.PropertyType.ToString();
            //        if (sdtProperty.GetValue(sdt, null) == null)
            //        {
            //            if (propType.Equals("System.Int32", StringComparison.CurrentCultureIgnoreCase))
            //            {
            //                sdtProperty.SetValue(sdt, -1);
            //            }
            //            else if (propType.Equals("System.DateTime", StringComparison.CurrentCultureIgnoreCase))
            //            {
            //                DateTime dtColValue = DateTime.Parse("1/1/1900");
            //                sdtProperty.SetValue(sdt, dtColValue);
            //            }
            //            else if (propType.Equals("System.Boolean", StringComparison.CurrentCultureIgnoreCase))
            //            {
            //                sdtProperty.SetValue(sdt, false);
            //            }
            //            else if (propType.Equals("System.Double", StringComparison.CurrentCultureIgnoreCase))
            //            {
            //                sdtProperty.SetValue(sdt, -1);
            //            }
            //            else if (propType.Equals("System.Decimal", StringComparison.CurrentCultureIgnoreCase))
            //            {
            //                sdtProperty.SetValue(sdt, -1);
            //            }
            //            else if (propType.Equals("System.Nullable`1[System.DateTime]"))
            //            {
            //                DateTime dtColValue = DateTime.Parse("1/1/1900");
            //                sdtProperty.SetValue(sdt, dtColValue);
            //            }
            //            else if (propType.Equals("System.Nullable`1[System.Int32]"))
            //            {
            //                sdtProperty.SetValue(sdt, -1);
            //            }
            //            else
            //            {
            //                sdtProperty.SetValue(sdt, string.Empty);
            //            }
            //        }

            //        if (propType.Equals("System.String", StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            try
            //            {
            //                string value = Core_AMS.Utilities.XmlFunctions.FormatXMLSpecialCharacters(sdtProperty.GetValue(sdt).ToString().Trim());
            //                sdtProperty.SetValue(sdt, value);
            //            }
            //            catch { }
            //        }
            //    }
            //}
            #endregion

            return so;
        }
    }
}
