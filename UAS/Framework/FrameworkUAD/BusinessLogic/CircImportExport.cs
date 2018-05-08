using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using FrameworkUAD.BusinessLogic.Helpers;

namespace FrameworkUAD.BusinessLogic
{
    public class CircImportExport
    {
        public List<Object.CircImportExport> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Object.CircImportExport> retList = null;
            retList = DataAccess.CircImportExport.Select(client);
            return FormatZipCode(retList);
        }
        public List<Object.CircImportExport> Select(int PublisherID, int PublicationID, KMPlatform.Object.ClientConnections client)
        {
            List<Object.CircImportExport> retList = null;
            retList = DataAccess.CircImportExport.Select(PublisherID, PublicationID, client);
            return FormatZipCode(retList);
        }
        public Object.CircImportExport FormatZipCode(Object.CircImportExport ps)
        {
            #region Canada
            if (ps.Country.Equals("Canada", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 2)
            {
                if (ps.ZipCode.Length == 6)
                {
                    ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);
                    ps.Plus4 = string.Empty;
                }
                else if (ps.ZipCode.Length == 7 && ps.ZipCode.Contains(" "))
                {
                    ps.Plus4 = string.Empty;
                }
                else if (ps.ZipCode.Length == 3 && ps.Plus4.Length == 3)
                {
                    ps.ZipCode = ps.ZipCode + " " + ps.Plus4;
                    ps.Plus4 = string.Empty;
                }
                else if (ps.ZipCode.Length > 7)
                {
                    if (ps.ZipCode.Contains(" "))
                        ps.ZipCode = ps.ZipCode.Substring(0, 7);
                    else
                        ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);

                    ps.Plus4 = string.Empty;
                }
            }
            #endregion
            #region USA
            else if (ps.Country.Equals("UNITED STATES", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 1)
            {
                var zipCodeArgs = ZipCodeMethodsHelper.ExecuteUsaFormatting(ps.ZipCode, ps.Plus4);
                ps.ZipCode = zipCodeArgs.ZipCode;
                ps.Plus4 = zipCodeArgs.Plus4Code;
            }
            #endregion
            #region Mexico or Foreign
            else//Mexico or Foreign  (ps.Country.Equals("MEXICO", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 429)
            {
                //do nothing with ZipCode - just keep whatever is there
                ps.Plus4 = string.Empty;
            }
            #endregion
            return ps;
        }
        public List<Object.CircImportExport> FormatZipCode(List<Object.CircImportExport> list)
        {
            foreach (Object.CircImportExport ps in list)
            {
                #region Canada
                if (ps.Country.Equals("Canada", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 2)
                {
                    if (ps.ZipCode.Length == 6)
                    {
                        ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);
                        ps.Plus4 = string.Empty;
                    }
                    else if (ps.ZipCode.Length == 7 && ps.ZipCode.Contains(" "))
                    {
                        ps.Plus4 = string.Empty;
                    }
                    else if (ps.ZipCode.Length == 3 && ps.Plus4.Length == 3)
                    {
                        ps.ZipCode = ps.ZipCode + " " + ps.Plus4;
                        ps.Plus4 = string.Empty;
                    }
                    else if (ps.ZipCode.Length > 7)
                    {
                        if (ps.ZipCode.Contains(" "))
                            ps.ZipCode = ps.ZipCode.Substring(0, 7);
                        else
                            ps.ZipCode = ps.ZipCode.Substring(0, 3) + " " + ps.ZipCode.Substring(3, 3);

                        ps.Plus4 = string.Empty;
                    }
                }
                #endregion
                #region USA
                else if (ps.Country.Equals("UNITED STATES", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 1)
                {
                    var zipCodeArgs = ZipCodeMethodsHelper.ExecuteUsaFormatting(ps.ZipCode, ps.Plus4);
                    ps.ZipCode = zipCodeArgs.ZipCode;
                    ps.Plus4 = zipCodeArgs.Plus4Code;
                }
                #endregion
                #region Mexico or Foreign
                else//Mexico or Foreign  (ps.Country.Equals("MEXICO", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 429)
                {
                    //do nothing with ZipCode - just keep whatever is there
                    ps.Plus4 = string.Empty;
                }
                #endregion
            }
            return list;
        }
        public DataTable FormatZipCode(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dt.Columns.Contains("Country") && dt.Columns.Contains("ZipCode"))
                {
                    #region Canada
                    if (dr["Country"].ToString().Equals("Canada", StringComparison.CurrentCultureIgnoreCase) || dr["CountryID"].ToString() == "2")
                    {
                        if (dr["ZipCode"].ToString().Length == 6)
                        {
                            dr["ZipCode"] = dr["ZipCode"].ToString().Substring(0, 3) + " " + dr["ZipCode"].ToString().Substring(3, 3);
                            dr["Plus4"] = string.Empty;
                        }
                        else if (dr["ZipCode"].ToString().Length == 7 && dr["ZipCode"].ToString().Contains(" "))
                        {
                            dr["Plus4"] = string.Empty;
                        }
                        else if (dr["ZipCode"].ToString().Length == 3 && dr["Plus4"].ToString().Length == 3)
                        {
                            dr["ZipCode"] = dr["ZipCode"].ToString() + " " + dr["Plus4"].ToString();
                            dr["Plus4"] = string.Empty;
                        }
                        else if (dr["ZipCode"].ToString().Length > 7)
                        {
                            if (dr["ZipCode"].ToString().Contains(" "))
                                dr["ZipCode"] = dr["ZipCode"].ToString().Substring(0, 7);
                            else
                                dr["ZipCode"] = dr["ZipCode"].ToString().Substring(0, 3) + " " + dr["ZipCode"].ToString().Substring(3, 3);

                            dr["Plus4"] = string.Empty;
                        }
                    }
                    #endregion
                    #region USA
                    else if (dr["Country"].ToString().Equals("UNITED STATES", StringComparison.CurrentCultureIgnoreCase) || dr["CountryID"].ToString() == "1")
                    {
                        var zipCodeArgs = ZipCodeMethodsHelper.ExecuteUsaFormatting(dr["ZipCode"].ToString(), dr["Plus4"].ToString());
                        dr["ZipCode"] = zipCodeArgs.ZipCode;
                        dr["Plus4"] = zipCodeArgs.Plus4Code;
                    }
                    #endregion
                    #region Mexico or Foreign
                    else//Mexico or Foreign  (ps.Country.Equals("MEXICO", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 429)
                    {
                        //do nothing with ZipCode - just keep whatever is there
                        dr["Plus4"] = string.Empty;
                    }
                    #endregion
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        public DataTable SelectDataTable(int publisherID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = DataAccess.CircImportExport.SelectDataTable(publisherID, publicationID, client);
            return FormatZipCode(dt);
        }
        public bool SaveBulkSqlUpdate(int UserID, List<Object.CircImportExport> list, KMPlatform.Object.ClientConnections client)
        {
            bool done = true;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            int userID = UserID;

            List<Object.CircImportExport> bulkUpdateList = new List<Object.CircImportExport>();
            foreach (Object.CircImportExport x in list)
            {
                counter++;
                processedCount++;
                done = false;
                bulkUpdateList.Add(x);
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            done = DataAccess.CircImportExport.CircUpdateBulkSql(userID, bulkUpdateList, client);

                            scope.Complete();
                        }

                        catch (Exception)

                        {
                            scope.Dispose();
                            done = false;
                            //string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            //FrameworkUAS.BusinessLogic.FileLog fl = new FileLog();
                            //fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message));
                        }
                    }
                    counter = 0;
                    bulkUpdateList = new List<Object.CircImportExport>();
                }
            }

            return done;
        }
    }
}
