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

namespace ecn.accounts.classes.PayFlowPro
{
    public class Transaction
    {
        public Transaction(string transactionID, DateTime processedDateTime, int resultCode, int statusCode, double amount)
        {
            _id = transactionID;
            _dateTime = processedDateTime;
            _resultCode = resultCode;
            _statusCode = statusCode;
            _amount = amount;
        }

        private string _id;
        public string ID
        {
            get
            {
                return (this._id);
            }
            set
            {
                this._id = value;
            }
        }

        private DateTime _dateTime;
        public DateTime DateTime
        {
            get
            {
                return (this._dateTime);
            }
            set
            {
                this._dateTime = value;
            }
        }

        private int _resultCode;
        public int ResultCode
        {
            get
            {
                return (this._resultCode);
            }
            set
            {
                this._resultCode = value;
            }
        }

        private int _statusCode;
        public int StatusCode
        {
            get
            {
                return (this._statusCode);
            }
            set
            {
                this._statusCode = value;
            }
        }

        private double _amount;
        public double Amount
        {
            get
            {
                return (this._amount);
            }
            set
            {
                this._amount = value;
            }
        }

        public static Transaction FindTransactionByProcessedDate(ArrayList transactions, DateTime expectedProcessDate)
        {
            foreach (Transaction t in transactions)
            {
                TimeSpan diff = t.DateTime - expectedProcessDate;
                if (diff.Days <= 3 && diff.Days >= 0)
                {
                    return t;
                }
            }
            return null;
        }
    }
}
