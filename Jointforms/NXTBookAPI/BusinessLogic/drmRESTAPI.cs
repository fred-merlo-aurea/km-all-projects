
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXTBookAPI.Entity;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Web;

namespace NXTBookAPI.BusinessLogic
{
    public class drmRESTAPI
    {
        /// <summary>
        /// Executes request to NxtBook Set Profile API method.  If it fails, will throw exception with "Request" and/or "Error" entries in the Exception.Data property to be handled in the calling application
        /// </summary>
        /// <param name="apikey"></param>
        /// <param name="drmprofile"></param>
        /// <returns></returns>
        public static bool SetProfile(string apikey, drmProfile drmprofile)
        {
            //https://drm.nxtbook.com/fx/drm/rest/SetProfile?apikey=9dce37bbb46b626570261613d69ae86e&profilestr=%7B%22subscriptionid%22%3A%22220f82d396ac109dcc6264a1d281eddb%22%2C%22update%22%3A%221%22%2C%22noupdate%22%3A%220%22%2C%22email%22%3A%22richardb%40icdnet.com%22%2C%22password%22%3A%22MyPassword%22%2C%22changepassword%22%3A%22%22%2C%22firstname%22%3A%22Richard%22%2C%22lastname%22%3A%22Burns%22%2C%22phone%22%3A%225555555555%22%2C%22address1%22%3A%221122+Main+St.%22%2C%22address2%22%3A%22Suite+190%22%2C%22city%22%3A%22Minneapolis%22%2C%22country%22%3A%22US%22%2C%22state%22%3A%22MN%22%2C%22zipcode%22%3A%2255555%22%2C%22organization%22%3A%22KM%22%2C%22extraparams%22%3A%22%22%2C%22access_type%22%3A%22timerestricted%22%2C%22access_nbissues%22%3A%22%22%2C%22access_firstissue%22%3A%22%22%2C%22access_limitdate%22%3A%22%22%2C%22access_startdate%22%3A%222015-01-01%22%2C%22access_enddate%22%3A%222015-12-31%22%7D
            HttpWebRequest request = null;
            drmError errorObject = null;
            try
            {

                if (drmprofile == null)
                {
                    throw new Exception("Profile is null/blank.");
                }
                else
                {

                    DateTime value;
                    if (!string.IsNullOrEmpty(drmprofile.access_startdate) && DateTime.TryParse(drmprofile.access_startdate, out value))
                    {
                        drmprofile.access_startdate = value.ToString("yyyy-MM-dd");
                    }

                    if (!string.IsNullOrEmpty(drmprofile.access_enddate) && DateTime.TryParse(drmprofile.access_enddate, out value))
                    {
                        drmprofile.access_enddate = value.ToString("yyyy-MM-dd");
                    }

                }


                string postParams = "https://drm.nxtbook.com/fx/drm/rest/SetProfile?apikey=" + apikey + "&profilestr=";
                string profileJSON = JsonConvert.SerializeObject(drmprofile);

                profileJSON = HttpUtility.UrlEncode(profileJSON);

                postParams += profileJSON;

                request = (HttpWebRequest)WebRequest.Create(postParams);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                string responseData = "";
                bool error = false;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    error = !response.StatusCode.Equals(HttpStatusCode.OK);
                    if (!error)
                    {
                        using (Stream s = response.GetResponseStream())
                        {
                            StreamReader sr = new StreamReader(s);
                            responseData = sr.ReadToEnd();
                            sr.Close();
                        }
                    }
                }

                bool bSuccess = true;
                if (error)
                {
                    bSuccess = false;
                    //Have an error, add request object to exception data
                    Exception reqError = new Exception();

                    if (request != null)
                    {
                        reqError.Data.Add("Request", request);
                    }

                    if(!string.IsNullOrWhiteSpace(responseData))
                    {
                        reqError.Data.Add("ResponseData", responseData);
                    }
                    throw reqError;
                }
                else if (responseData.Contains("faultString"))
                {
                    bSuccess = false;
                    errorObject = JsonConvert.DeserializeObject<drmError>(responseData);

                    //Have an error, add request and drmError object to exception data
                    Exception reqError = new Exception();
                    if (!responseData.ToLower().Contains("access already exists"))
                    {
                        if (request != null)
                            reqError.Data.Add("Request", request);

                        if (errorObject != null)
                            reqError.Data.Add("Error", errorObject);

                        if (!string.IsNullOrWhiteSpace(responseData))
                        {
                            reqError.Data.Add("ResponseData", responseData);
                        }


                        throw reqError;
                    }
                }

                return bSuccess;
            }
            catch (Exception ex)
            {
                if (request != null && !ex.Data.Contains("Request"))
                {
                    ex.Data.Add("Request", request);
                }

                if (errorObject != null && !ex.Data.Contains("Error"))
                    ex.Data.Add("Error", errorObject);

                throw ex;

            }
        }
    }
}
