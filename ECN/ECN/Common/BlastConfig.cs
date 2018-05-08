using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ecn.common.classes
{

    public class BlastConfig
    {
        private string _MTAName = string.Empty;
        private string _BounceName = string.Empty;
        private string _MTAServer = string.Empty;
        private string _SMTPServer = string.Empty;
        private int _MTAReset = 0;

        public string MTAName
        {
            get
            {
                return _MTAName;
            }
        }
        public string BounceName
        {
            get
            {
                return _BounceName;
            }
        }
        public string MTAServer
        {
            get
            {
                return _MTAServer;
            }
        }
        public string SMTPServer
        {
            get
            {
                return _SMTPServer;
            }
        }
        public int MTAReset
        {
            get
            {
                return _MTAReset;
            }
        }

        public BlastConfig(int CustomerID)
        {
            try
            {
                //Proc will get the blastconfig record by first looking at the mta tables and if not found, looking at the customer table
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetBlastConfig";
                cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
                DataTable dt = DataFunctions.GetDataTable(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    _MTAName = dt.Rows[0]["MTAName"].ToString();
                    _BounceName = dt.Rows[0]["BounceName"].ToString();
                    _MTAServer = dt.Rows[0]["MTAServer"].ToString();
                    _SMTPServer = dt.Rows[0]["SMTPServer"].ToString();
                    _MTAReset = Convert.ToInt32(dt.Rows[0]["MTAReset"].ToString());
                    try
                    {
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["OverrideMTAForTesting"].ToString()))
                        {
                            _MTAServer = ConfigurationManager.AppSettings["MTAForTesting"].ToString();
                            _SMTPServer = ConfigurationManager.AppSettings["MTAForTesting"].ToString();
                            _MTAReset = Convert.ToInt32(ConfigurationManager.AppSettings["MTAResetForTesting"].ToString());
                        }
                        //else if (Convert.ToBoolean(ConfigurationManager.AppSettings["OverrideMTAByApplication"].ToString()))
                        //{
                        //    if (_MTAServer == ConfigurationManager.AppSettings["MTAToReplace"].ToString())
                        //    {
                        //        _MTAServer = ConfigurationManager.AppSettings["MTAReplaceValue"].ToString();
                        //        _SMTPServer = ConfigurationManager.AppSettings["MTAReplaceValue"].ToString();
                        //    }
                        //}
                    }
                    catch (Exception)
                    {
                    }
                }

            }
            catch (Exception)
            {
            }

            //old code - worked but was not optimal
            //try
            //{
            //    //GreenArrow and Port25
            //    String select = string.Empty;
            //    select =    "select bc.* " +
            //                "from MTACustomer mtac " +
            //                    "join MTA m on mtac.MTAID = m.MTAID " +
            //                    "join BlastConfig bc on m.BlastConfigID = bc.BlastConfigID " +
            //                "where mtac.CustomerID = @CustomerID";
            //    SqlCommand cmd = new SqlCommand(select);
            //    cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
            //    DataTable dt = DataFunctions.GetDataTable(cmd);
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        _MTAName = dt.Rows[0]["MTAName"].ToString();
            //        _BounceName = dt.Rows[0]["BounceName"].ToString();
            //        _MTAServer = dt.Rows[0]["MTAServer"].ToString();
            //        _SMTPServer = dt.Rows[0]["SMTPServer"].ToString();
            //        _MTAReset = Convert.ToInt32(dt.Rows[0]["MTAReset"].ToString());
            //        try
            //        {
            //            if (Convert.ToBoolean(ConfigurationManager.AppSettings["OverrideMTAForTesting"].ToString()))
            //            {
            //                _MTAServer = ConfigurationManager.AppSettings["MTAForTesting"].ToString();
            //                _SMTPServer = ConfigurationManager.AppSettings["MTAForTesting"].ToString();
            //                _MTAReset = Convert.ToInt32(ConfigurationManager.AppSettings["MTAResetForTesting"].ToString());
            //            }
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    }
            //    //see if it's IronPort
            //    else
            //    {
            //        select = string.Empty;
            //        select = "select bc.* " +
            //                    "from ecn5_communicator..BlastConfig bc " +
            //                        "join ecn5_accounts..Customer c on bc.BlastConfigID = c.BlastConfigID " +
            //                    "where c.CustomerID = @CustomerID";
            //        cmd = new SqlCommand(select);
            //        cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
            //        dt = DataFunctions.GetDataTable(cmd);
            //        if (dt != null && dt.Rows.Count > 0)
            //        {
            //            _MTAName = dt.Rows[0]["MTAName"].ToString();
            //            _BounceName = dt.Rows[0]["BounceName"].ToString();
            //            _MTAServer = dt.Rows[0]["MTAServer"].ToString();
            //            _SMTPServer = dt.Rows[0]["SMTPServer"].ToString();
            //            _MTAReset = Convert.ToInt32(dt.Rows[0]["MTAReset"].ToString());
            //            try
            //            {
            //                if (Convert.ToBoolean(ConfigurationManager.AppSettings["OverrideMTAForTesting"].ToString()))
            //                {
            //                    _MTAServer = ConfigurationManager.AppSettings["MTAForTesting"].ToString();
            //                    _SMTPServer = ConfigurationManager.AppSettings["MTAForTesting"].ToString();
            //                    _MTAReset = Convert.ToInt32(ConfigurationManager.AppSettings["MTAResetForTesting"].ToString());
            //                }
            //            }
            //            catch (Exception)
            //            {
            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //}
        }

        //public static string GetMTAName(int CustomerID)
        //{
        //    string mtaName = string.Empty;
        //    try
        //    {
        //        DataRow dr = GetRecord(CustomerID);
        //        if (dr != null)
        //        {
        //            mtaName = dr["MTAName"].ToString().Trim();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return mtaName;
        //}

        //public static string GetBounceName(int CustomerID)
        //{
        //    string bounceName = string.Empty;
        //    try
        //    {
        //        DataRow dr = GetRecord(CustomerID);
        //        if (dr != null)
        //        {
        //            bounceName = dr["BounceName"].ToString().Trim();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return bounceName;
        //}

        //public static string GetMTAServer(int CustomerID)
        //{
        //    string mtaServer = string.Empty;
        //    try
        //    {
        //        DataRow dr = GetRecord(CustomerID);
        //        if (dr != null)
        //        {
        //            mtaServer = dr["MTAServer"].ToString().Trim();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return mtaServer;
        //}

        //public static string GetSMTPServer(int CustomerID)
        //{
        //    string smtpServer = string.Empty;
        //    try
        //    {
        //        DataRow dr = GetRecord(CustomerID);
        //        if (dr != null)
        //        {
        //            smtpServer = dr["SMTPServer"].ToString().Trim();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return smtpServer;
        //}

        //public static int GetMTAReset(int CustomerID)
        //{
        //    int mtaReset = 0;
        //    try
        //    {
        //        DataRow dr = GetRecord(CustomerID);
        //        if (dr != null)
        //        {
        //            mtaReset = Convert.ToInt32(dr["MTAReset"].ToString().Trim());
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return mtaReset;
        //}

        //private static DataRow GetRecord(int CustomerID)
        //{
        //    DataRow dr = null;
        //    try
        //    {
        //        String select = string.Empty;
        //        select = "select bc.* " +
        //                        "from ecn5_communicator..BlastConfig bc " +
        //                            "join ecn5_accounts..Customer c on bc.BlastConfigID = c.BlastConfigID " +
        //                        "where c.CustomerID = @CustomerID";
        //        SqlCommand cmd = new SqlCommand(select);
        //        cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
        //        DataTable dt = DataFunctions.GetDataTable(cmd);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            dr = dt.Rows[0];
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return dr;
        //}        
    }
}
