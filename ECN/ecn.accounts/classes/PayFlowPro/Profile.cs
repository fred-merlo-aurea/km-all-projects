using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ecn.common.classes.billing;

namespace ecn.accounts.classes.PayFlowPro
{
    public class Profile
    {
        public Profile(string profileName, string customerName, CreditCard creditCard, FrequencyEnum frequency, double amount)
        {
            _profileName = profileName;
            _customerName = customerName;
            _creditCard = creditCard;
            _frequency = frequency;
            _amount = amount;
        }

        #region Properties

        private string _id = null;
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

        public bool IsNew
        {
            get { return _id == null || _id.Trim() == string.Empty; }
        }

        private string _profileName;
        public string ProfileName
        {
            get
            {
                return (this._profileName);
            }
            set
            {
                this._profileName = value;
            }
        }

        private string _customerName;
        public string CustomerName
        {
            get
            {
                return (this._customerName);
            }
            set
            {
                this._customerName = value;
            }
        }


        private CreditCard _creditCard;
        public CreditCard CreditCard
        {
            get
            {
                return (this._creditCard);
            }
            set
            {
                this._creditCard = value;
            }
        }

        private FrequencyEnum _frequency;
        public FrequencyEnum Frequency
        {
            get
            {
                return (this._frequency);
            }
            set
            {
                this._frequency = value;
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

        private string _description;
        public string Description
        {
            get
            {
                return (this._description);
            }
            set
            {
                this._description = value;
            }
        }
        #endregion

        public string PayPeriod
        {
            get
            {
                switch (Frequency)
                {
                    case FrequencyEnum.Annual:
                        return "YEAR";
                    case FrequencyEnum.Quarterly:
                        return "QTER";
                    case FrequencyEnum.Monthly:
                        return "MONT";
                    case FrequencyEnum.BiWeekly:
                        return "BIWK";
                    case FrequencyEnum.Weekly:
                        return "WEEK";
                    default:
                        throw new ApplicationException(string.Format("Unkown recurring frequency '{0}'. ", Frequency));
                }
            }
        }
    }
}
