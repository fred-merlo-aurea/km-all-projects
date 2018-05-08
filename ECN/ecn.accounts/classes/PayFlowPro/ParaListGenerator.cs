using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

using ecn.common.classes.billing;

namespace ecn.accounts.classes.PayFlowPro
{
    public class ParaListGenerator
    {
        public string GetAuthString(PayFlowProCreditCardProcessor processor)
        {
            return string.Format("USER={0}&VENDOR={1}&PARTNER={2}&PWD={3}", processor.User, processor.Vendor, processor.Partner, processor.Password);
        }

        public string GetSalesTransactionParas(CreditCard creditCard, double amount, string description)
        {
            return string.Format("&TRXTYPE=S&TENDER=C&ACCT={0}&EXPDATE={1}&AMT={2}&STREET={3}&ZIP={4}&Name={5}&CVV2={6}&COMMENT1={7}",
                creditCard.CardNumber,
                creditCard.ExpirationDate.ToString("MMyy"),
                amount.ToString("######.00"),
                creditCard.BillingContact.StreetAddress,
                creditCard.BillingContact.Zip,
                creditCard.BillingContact.ContactName,
                creditCard.SecurityNumber,
                description);
        }

        public string GetVoidSalesTransactionParas(string transactionID)
        {
            StringBuilder paraList = new StringBuilder();
            ConstructParaList(paraList, "TRXTYPE", "V");
            ConstructParaList(paraList, "TENDER", "C");
            ConstructParaList(paraList, "ORIGID", transactionID);
            return paraList.ToString();
        }

        public string GetParasToAddProfile(Profile profile, DateTime start)
        {
            StringBuilder paraList = new StringBuilder();
            ConstructParaList(paraList, "ACTION", "A");
            ConstructRequiredParas(paraList, profile, start);
            return paraList.ToString();
        }

        public string GetParasToCancelProfile(string profileID)
        {
            StringBuilder paraList = new StringBuilder();
            ConstructParaList(paraList, "ACTION", "C");
            ConstructParaList(paraList, "TRXTYPE", "R");
            ConstructParaList(paraList, "ORIGPROFILEID", profileID);
            return paraList.ToString();
        }

        public string GetParasToModifyProfile(Profile profile, DateTime start)
        {
            StringBuilder paraList = new StringBuilder();
            ConstructParaList(paraList, "ACTION", "M");
            ConstructParaList(paraList, "ORIGPROFILEID", profile.ID);

            if (profile.CreditCard != null)
            {
                ConstructRequiredParas(paraList, profile, start);
                return paraList.ToString();
            }

            ConstructParaList(paraList, "TRXTYPE", "R");
            ConstructParaList(paraList, "TENDER", "C");
            ConstructParaList(paraList, "PROFILENAME", profile.ProfileName);
            ConstructParaList(paraList, "AMT", profile.Amount.ToString(".00"));
            ConstructParaList(paraList, "START", start.ToString("MMddyyyy"));
            ConstructParaList(paraList, "TERM", "0");
            ConstructParaList(paraList, "PAYPERIOD", profile.PayPeriod);
            ConstructParaList(paraList, "COMMENT1", profile.Description);
            ConstructParaList(paraList, "COMPANYNAME", profile.CustomerName);
            ConstructParaList(paraList, "OPTIONALTRX", "A");
            ConstructParaList(paraList, "OPTIONALTRXAMT", profile.Amount.ToString(".00"));
            return paraList.ToString();
        }

        public string GetParasToQueryProfileHistory(string profileID)
        {
            StringBuilder paraList = new StringBuilder();
            ConstructParaList(paraList, "ACTION", "I");
            ConstructParaList(paraList, "TRXTYPE", "R");
            ConstructParaList(paraList, "PAYMENTHISTORY", "Y");
            ConstructParaList(paraList, "ORIGPROFILEID", profileID);
            return paraList.ToString();
        }

        private void ConstructRequiredParas(StringBuilder paraList, Profile profile, DateTime start)
        {
            ConstructParaList(paraList, "TRXTYPE", "R");
            ConstructParaList(paraList, "TENDER", "C");
            ConstructParaList(paraList, "PROFILENAME", profile.ProfileName);
            ConstructParaList(paraList, "ACCT", profile.CreditCard.CardNumber);
            ConstructParaList(paraList, "EXPDATE", profile.CreditCard.ExpirationDate.ToString("MMyy"));
            ConstructParaList(paraList, "STREET", profile.CreditCard.BillingContact.StreetAddress);
            ConstructParaList(paraList, "ZIP", profile.CreditCard.BillingContact.Zip);
            ConstructParaList(paraList, "Name", profile.CreditCard.BillingContact.ContactName);
            ConstructParaList(paraList, "CVV2", profile.CreditCard.SecurityNumber);
            ConstructParaList(paraList, "AMT", profile.Amount.ToString(".00"));
            ConstructParaList(paraList, "START", start.ToString("MMddyyyy"));
            ConstructParaList(paraList, "TERM", "0");
            ConstructParaList(paraList, "PAYPERIOD", profile.PayPeriod);
            ConstructParaList(paraList, "EMAIL", profile.CreditCard.BillingContact.Email);
            ConstructParaList(paraList, "COMMENT1", profile.Description);
            ConstructParaList(paraList, "COMPANYNAME", profile.CustomerName);
            ConstructParaList(paraList, "OPTIONALTRX", "A");
            ConstructParaList(paraList, "OPTIONALTRXAMT", profile.Amount.ToString(".00"));
        }

        private static void ConstructParaList(StringBuilder paraList, string key, string val)
        {
            paraList.Append(string.Format("&{0}={1}", key, val));
        }

    }
}
