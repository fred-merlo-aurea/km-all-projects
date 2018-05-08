using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class ClientProductSubscription
    {
        public List<Object.ProductSubscription> Select(int subscriptionID, KMPlatform.Object.ClientConnections client, string clientDisplayName, bool includeCustomProperties = false)
        {
            List<Object.ProductSubscription> retList = null;
            retList = DataAccess.ClientProductSubscription.Select(subscriptionID, client, clientDisplayName);
            if (includeCustomProperties == true)
            {
                foreach (var x in retList)
                {
                    x.SubscriberProductDemographics = GetCustomProperties(subscriptionID, x.PubCode, client).ToList();
                }
            }
            if (retList != null)
                return FormatZipCode(retList);
            else
                return retList;
        }
        private List<Object.SubscriberProductDemographic> GetCustomProperties(int subscriptionID, string pubCode, KMPlatform.Object.ClientConnections client)
        {
            List<Object.SubscriberProductDemographic> pubDetails = new List<Object.SubscriberProductDemographic>();
            SubscriberProductDemographic spdData = new SubscriberProductDemographic();
            pubDetails = spdData.Select(subscriptionID, pubCode, client).ToList();
            return pubDetails;
        }
        public List<Object.ProductSubscription> FormatZipCode(List<Object.ProductSubscription> list)
        {
            if (list != null)
            {
                foreach (Object.ProductSubscription ps in list)
                {
                    try
                    {
                        #region Canada
                        if (ps.Country.Equals("Canada", StringComparison.CurrentCultureIgnoreCase))
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
                        else if (ps.Country.Equals("UNITED STATES", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (ps.ZipCode.Length == 5 && ps.ZipCode.StartsWith("000"))
                                ps.ZipCode = string.Empty;

                            if (ps.ZipCode.Length <= 3)
                                ps.ZipCode = string.Empty;
                            else if (ps.ZipCode.Length == 4)
                                ps.ZipCode = "0" + ps.ZipCode;
                            else if (ps.ZipCode.Length >= 5)//>7
                            {
                                string zipOriginal = ps.ZipCode.Replace("-", "");
                                if (ps.ZipCode.Length > 5 && ps.ZipCode.Length <= 9)//Changed from >= to >... Should only need to check 6-9 for plus 4
                                {
                                    ps.ZipCode = zipOriginal.Substring(0, 5);
                                    ps.Plus4 = string.Empty;
                                }
                                else//>=10
                                    ps.ZipCode = ps.ZipCode;

                                if (zipOriginal.Length == 9)
                                    ps.Plus4 = zipOriginal.Substring(5, 4);
                            }

                            if (ps.Plus4.Length > 4)
                                ps.Plus4 = string.Empty;

                            string zipCheck = Core_AMS.Utilities.StringFunctions.GetNumbersOnly(ps.ZipCode);
                            if (zipCheck.Length < 5)
                                ps.ZipCode = string.Empty;
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
                    catch { }//suppress any null errors
                }
            }
            return list;
        }
    }
}
