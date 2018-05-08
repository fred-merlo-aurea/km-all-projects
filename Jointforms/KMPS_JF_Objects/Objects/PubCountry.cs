using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace KMPS_JF_Objects.Objects
{
    [Serializable]
    public class PubCountry
    {
        private int _countryID = 0;
        private int _PFID = 0;
        private string _countryName = string.Empty;
        private string _countryCode = string.Empty;
        private string _continent = string.Empty;
        private bool _isNonQualified = false;

        public int CountryID
        {
            get
            {
                return _countryID;
            }
            set
            {
                _countryID = value;
            }
        }

        public int PFID
        {
            get
            {
                return _PFID;
            }
            set
            {
                _PFID = value;
            }
        }

        public string CountryName
        {
            get
            {
                return _countryName;
            }
            set
            {
                _countryName = value;
            }
        }
        public string CountryCode
        {
            get
            {
                return _countryCode;
            }
            set
            {
                _countryCode = value;
            }
        }

        public string Continent
        {
            get
            {
                return _continent;
            }
            set
            {
                _continent = value;
            }
        }

        public bool IsNonQualified
        {
            get
            {
                return _isNonQualified;
            }
            set
            {
                _isNonQualified = value;
            }
        }

        public PubCountry(int PFID, int CountryID, string CountryName, string CountryCode, string Continent, bool IsNonQualified)
        {
            _PFID = PFID;
            _countryID = CountryID;
            _countryName = CountryName;
            _countryCode = CountryCode;
            _continent = Continent;
            _isNonQualified = IsNonQualified;
        }

        public static PubCountry GetCountryByCountryID(int countryID)
        {
            SqlCommand cmd = new SqlCommand(string.Format("select * from Country  with (NOLOCK) where CountryID = {0}", countryID.ToString())); 
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;
            SqlDataReader rdr = DataFunctions.ExecuteReader(cmd);
            PubCountry ctry = null; 
            
            while (rdr.Read())
            {
                ctry = new PubCountry(0, Convert.ToInt32(rdr["CountryID"]), rdr["CountryName"].ToString().ToUpper(), rdr["CountryCode"].ToString().ToUpper(), rdr["Continent"].ToString().ToUpper(), false); 
            }

            return ctry;
        }

        public static List<PubCountry> GetCountriesForPub(int PubID)
        {
            List<PubCountry> listCountry_US_Canada_Mexico = new List<PubCountry>();
            List<PubCountry> listPubCountry = new List<PubCountry>();

            SqlCommand cmd = new SqlCommand(string.Format("select pf.pubID, pfc.PfID, c.*, pfc.IsNonQual from pubforms pf  with (NOLOCK) join PubFormsForCountry pfc  with (NOLOCK) on pf.PFID = pfc.pfid join Country c on pfc.CountryID = c.CountryID where pf.PubID = {0} order by c.countryName", PubID));
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = 0;
            
            SqlDataReader rdr = DataFunctions.ExecuteReader(cmd);

            while (rdr.Read())
            {
                if (rdr["CountryName"].ToString().Equals("United States", StringComparison.OrdinalIgnoreCase) || rdr["CountryName"].ToString().Equals("canada", StringComparison.OrdinalIgnoreCase) || rdr["CountryName"].ToString().Equals("mexico", StringComparison.OrdinalIgnoreCase))
                {
                    listCountry_US_Canada_Mexico.Add(new PubCountry(Convert.ToInt32(rdr["PFID"]), Convert.ToInt32(rdr["CountryID"]), rdr["CountryName"].ToString().ToUpper(), rdr["CountryCode"].ToString().ToUpper(), rdr["Continent"].ToString().ToUpper(), Convert.ToBoolean(rdr["IsNonQual"])));
                }
                else
                {
                    listPubCountry.Add(new PubCountry(Convert.ToInt32(rdr["PFID"]), Convert.ToInt32(rdr["CountryID"]), rdr["CountryName"].ToString().ToUpper(), rdr["CountryCode"].ToString().ToUpper(), rdr["Continent"].ToString().ToUpper(), Convert.ToBoolean(rdr["IsNonQual"])));
                }
            }
            
            //reorder countries - US (first), Canada (second) & mexico (third).
            if (listCountry_US_Canada_Mexico.Count > 0)
            {
                    PubCountry pc = listCountry_US_Canada_Mexico.SingleOrDefault(x => x.CountryName.Equals("mexico", StringComparison.OrdinalIgnoreCase));

                    if (pc != null)
                        listPubCountry.Insert(0, pc);

                    pc = listCountry_US_Canada_Mexico.SingleOrDefault(x => x.CountryName.Equals("canada", StringComparison.OrdinalIgnoreCase));

                    if (pc != null)
                        listPubCountry.Insert(0, pc);

                    pc = listCountry_US_Canada_Mexico.SingleOrDefault(x => x.CountryName.Equals("United States", StringComparison.OrdinalIgnoreCase));

                    if (pc != null)
                        listPubCountry.Insert(0, pc);
            }

            return listPubCountry;
        }
    }
}
