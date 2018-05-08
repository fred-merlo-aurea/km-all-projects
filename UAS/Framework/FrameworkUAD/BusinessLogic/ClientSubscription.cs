using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class ClientSubscription
    {
        public List<Object.Subscription> Select(string email, KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties = false)
        {
            List<Object.Subscription> retList = null;
            retList = DataAccess.ClientSubscription.Select(email, client);
            if (includeCustomProperties == true)
            {
                foreach (var c in retList)
                {
                    GetCustomProperties(c, client, clientDisplayName, true);
                }
            }

            if (retList != null)
                return FormatZipCode(retList);
            else
                return retList;
        }
        public List<Object.Subscription> Select(KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties = false)
        {
            List<Object.Subscription> retList = null;
            retList = DataAccess.ClientSubscription.Select(client);
            if (includeCustomProperties == true)
            {
                foreach (var c in retList)
                {
                    GetCustomProperties(c, client, clientDisplayName, true);
                }
            }

            if (retList != null)
                return FormatZipCode(retList);
            else
                return retList;
        }
        private void GetCustomProperties(Object.Subscription sub, KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties)
        {
            SubscriberConsensusDemographic scdData = new SubscriberConsensusDemographic();
            sub.SubscriptionConsensusDemographics = scdData.Select(sub.SubscriptionID, client);
            ClientProductSubscription psData = new ClientProductSubscription();
            sub.ProductList = psData.Select(sub.SubscriptionID, client, clientDisplayName, includeCustomProperties).ToList();
        }
        public List<Object.Subscription> FormatZipCode(List<Object.Subscription> list)
        {
            if (list != null)
            {
                foreach (Object.Subscription s in list)
                {
                    if (s.Country != null)
                    {
                        try
                        {
                            #region Canada
                            if (s.Country.Equals("Canada", StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (s.Zip.Length == 6)
                                {
                                    s.Zip = s.Zip.Substring(0, 3) + " " + s.Zip.Substring(3, 3);
                                    s.Plus4 = string.Empty;
                                }
                                else if (s.Zip.Length == 7 && s.Zip.Contains(" "))
                                {
                                    s.Plus4 = string.Empty;
                                }
                                else if (s.Zip.Length == 3 && s.Plus4.Length == 3)
                                {
                                    s.Zip = s.Zip + " " + s.Plus4;
                                    s.Plus4 = string.Empty;
                                }
                                else if (s.Zip.Length > 7)
                                {
                                    if (s.Zip.Contains(" "))
                                        s.Zip = s.Zip.Substring(0, 7);
                                    else
                                        s.Zip = s.Zip.Substring(0, 3) + " " + s.Zip.Substring(3, 3);

                                    s.Plus4 = string.Empty;
                                }
                            }
                            #endregion
                            #region USA
                            else if (s.Country.Equals("UNITED STATES", StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (s.Zip.Length == 5 && s.Zip.StartsWith("000"))
                                    s.Zip = string.Empty;

                                if (s.Zip.Length <= 3)
                                    s.Zip = string.Empty;
                                else if (s.Zip.Length == 4)
                                    s.Zip = "0" + s.Zip;
                                else if (s.Zip.Length >= 5)//>7
                                {
                                    string zipOriginal = s.Zip.Replace("-", "");
                                    if (s.Zip.Length >= 5 && s.Zip.Length <= 9)
                                    {
                                        s.Zip = zipOriginal.Substring(0, 5);
                                        s.Plus4 = string.Empty;
                                    }
                                    else//>=10
                                        s.Zip = s.Zip;

                                    if (zipOriginal.Length == 9)
                                        s.Plus4 = zipOriginal.Substring(5, 4);
                                }

                                if (s.Plus4.Length > 4)
                                    s.Plus4 = string.Empty;

                                string zipCheck = Core_AMS.Utilities.StringFunctions.GetNumbersOnly(s.Zip);
                                if (zipCheck.Length < 5)
                                    s.Zip = string.Empty;
                            }
                            #endregion
                            #region Mexico or Foreign
                            else//Mexico or Foreign  (ps.Country.Equals("MEXICO", StringComparison.CurrentCultureIgnoreCase) || ps.CountryID == 429)
                            {
                                //do nothing with ZipCode - just keep whatever is there
                                s.Plus4 = string.Empty;
                            }
                            #endregion
                        }
                        catch { }//suppress any null errors
                    }
                }
            }
            return list;
        }

    }
}
