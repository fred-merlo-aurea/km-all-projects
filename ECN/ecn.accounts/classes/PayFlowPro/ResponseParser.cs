using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Specialized;

namespace ecn.accounts.classes.PayFlowPro
{
    public enum TransactionStatusEnum { Error, Pending, InProgress, Completed, Failed, Incomplete }

    public class ResponseParser
    {
        public static bool IsTransactionSuccessful(string response)
        {
            NameValueCollection tokens = GetTokens(response);
            int val = Convert.ToInt32(tokens["RESULT"]);
            return val == 0;
        }

        public static string GetReturnMessage(string response)
        {
            NameValueCollection tokens = GetTokens(response);
            string ret = tokens["RESPMSG"];
            if (ret == null)
            {
                return string.Empty;
            }
            return ret;
        }

        public static string GetTransactionID(string response)
        {
            NameValueCollection tokens = GetTokens(response);
            string ret = tokens["PNREF"];
            if (ret == null)
            {
                return "";
            }
            return ret;
        }

        public static string GetProfileID(string response)
        {
            NameValueCollection tokens = GetTokens(response);
            return tokens["PROFILEID"] == null ? "" : tokens["PROFILEID"];
        }

        public static ArrayList GetTransactionHistory(string response)
        {
            ArrayList history = new ArrayList();
            NameValueCollection tokens = GetTokens(response);

            for (int i = 1; i < 1000; i++)
            {
                string transactionID = GetTransactionIDByIndex(tokens, i);
                if (transactionID == null)
                {
                    break;
                }

                history.Add(new Transaction(transactionID,
                    GetProcessedDateByIndex(tokens, i),
                    GetResultCodeByIndex(tokens, i),
                    GetStatusCodeByIndex(tokens, i),
                    GetAmountByIndex(tokens, i)));
            }
            return history;
        }

        public static TransactionStatusEnum GetTransactionStatusByCode(int statusCode)
        {
            switch (statusCode)
            {
                case 1:
                    return TransactionStatusEnum.Error;
                case 6:
                    return TransactionStatusEnum.Pending;
                case 7:
                    return TransactionStatusEnum.InProgress;
                case 8:
                    return TransactionStatusEnum.Completed;
                case 11:
                    return TransactionStatusEnum.Failed;
                case 14:
                    return TransactionStatusEnum.Incomplete;
                default:
                    throw new ApplicationException(string.Format("Unknow transaction status code '{0}'.", statusCode));
            }
        }

        private static string GetTransactionIDByIndex(NameValueCollection tokens, int index)
        {
            string tokenName = string.Format("P_PNREF{0}", index);
            return tokens[tokenName];
        }

        private static DateTime GetProcessedDateByIndex(NameValueCollection tokens, int index)
        {
            string tokenName = string.Format("P_TRANSTIME{0}", index);
            string val = tokens[tokenName];

            try
            {
                return Convert.ToDateTime(val);
            }
            catch (Exception)
            {
                return DateTime.MaxValue;
            }
        }

        private static int GetResultCodeByIndex(NameValueCollection tokens, int index)
        {
            string tokenName = string.Format("P_RESULT{0}", index);
            string val = tokens[tokenName];
            if (val == null || val.Trim() == null)
            {
                return -1;
            }

            try
            {
                return Convert.ToInt32(val);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static int GetStatusCodeByIndex(NameValueCollection tokens, int index)
        {
            string tokenName = string.Format("P_TRANSTATE{0}", index);
            string val = tokens[tokenName];
            if (val == null || val.Trim() == null)
            {
                return -1;
            }

            try
            {
                return Convert.ToInt32(val);
            }
            catch (Exception)
            {
                return -1;
            }
        }


        private static double GetAmountByIndex(NameValueCollection tokens, int index)
        {
            string tokenName = string.Format("P_AMT{0}", index);
            string val = tokens[tokenName];
            try
            {
                return Convert.ToDouble(val) / 100;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private static NameValueCollection GetTokens(string response)
        {
            NameValueCollection ret = new NameValueCollection();
            if (response == null)
            {
                return ret;
            }

            string[] tokens = response.Split('&');
            if (tokens.Length < 1)
            {
                return ret;
            }

            foreach (string token in tokens)
            {
                string[] code = token.Split('=');
                if (code.Length != 2)
                {
                    break;
                }
                ret.Add(code[0], code[1]);
            }
            return ret;
        }
    }
}
