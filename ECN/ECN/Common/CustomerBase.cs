using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using ecn.common.classes.billing;


namespace ecn.common.classes
{
    public class CustomerBase
    {
        public static readonly string ParameCustomerId = "@customerID";
        public static readonly string ParamSalutation = "@salutation";
        public static readonly string ParamContactName = "@contactName";
        public static readonly string ParamFirstName = "@firstName";
        public static readonly string ParamLastName = "@lastName";
        public static readonly string ParamContactTitle = "@contactTitle";
        public static readonly string ParamPhone = "@phone";
        public static readonly string ParamFax = "@fax";
        public static readonly string ParamEmail = "@email";
        public static readonly string ParamStreetAddress = "@streetAddress";
        public static readonly string ParamCity = "@city";
        public static readonly string ParamState = "@state";
        public static readonly string ParamCountry = "@Country";
        public static readonly string ParamZip = "@zip";
        public static readonly string ParamBaseChannelId = "@baseChannelID";
        public static readonly string ParamCustomerName = "@customerName";
        public static readonly string ParamAddress = "@address";
        public static readonly string ParamWebAddress = "@webAddress";
        public static readonly string ParamTechContact = "@techContact";
        public static readonly string ParamTechEmail = "@techEmail";
        public static readonly string ParamTechPhone = "@techPhone";
        public static readonly string ParamSubscriptionsEmail = "@subscriptionsEmail";
        public static readonly string AppSettingcommunicatordb = "communicatordb";
        public static readonly int IdNone = -1;
        public static readonly string SubscriptionDefaultEmail = "subscriptions@bounce2.com";

        private int _baseChannelId;
        private string _techContact;
        private string _techEmail;
        private string _techPhone;
        private string _subscriptionsEmail = SubscriptionDefaultEmail;
        private string _name;
        private int _collectorLevel = IdNone;
        private int _creatorLevel = IdNone;
        private int _accountLevel = IdNone;
        private Contact _generalContact;
        private string _webAddress;

        public CustomerBase() {}

        public CustomerBase(int id, int baseChannelId)
        {
            ID = id;
            _baseChannelId = baseChannelId;
        }

        public int ID { get; set; } = IdNone;

        public bool IsNew => ID == IdNone;

        private Contact _billingContact;
        public Contact BillingContact
        {
            get
            {
                if (!IsNew && _billingContact == null)
                {
                    _billingContact = GetBillingContact(ID);
                }
                return _billingContact;
            }
            set
            {
                _billingContact = value;
            }
        }

        public string TechContact
        {
            get
            {
                if (!IsNew && _techContact == null)
                {
                    _techContact = LoadedCustomerBase.TechContact;
                }
                return _techContact;
            }
            set
            {
                _techContact = value;
            }
        }

        public string TechEmail
        {
            get
            {
                if (!IsNew && _techEmail == null)
                {
                    _techEmail = LoadedCustomerBase.TechEmail;
                }
                return _techEmail;
            }
            set
            {
                _techEmail = value;
            }
        }

        public string TechPhone
        {
            get
            {
                if (!IsNew && _techPhone == null)
                {
                    _techPhone = LoadedCustomerBase.TechPhone;
                }
                return _techPhone;
            }
            set
            {
                _techPhone = value;
            }
        }

        public string SubscriptionsEmail
        {
            get
            {
                if (!IsNew && _subscriptionsEmail == null)
                {
                    _subscriptionsEmail = LoadedCustomerBase.SubscriptionsEmail;
                }
                return _subscriptionsEmail;
            }
            set
            {
                _subscriptionsEmail = value;
            }
        }

        public string Name
        {
            get
            {
                if (!IsNew && _name == null)
                {
                    _name = LoadedCustomerBase.Name;
                }
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int CollectorLevel
        {
            get
            {
                if (!IsNew && _collectorLevel == IdNone)
                {
                    _collectorLevel = LoadedCustomerBase.CollectorLevel;
                }
                return _collectorLevel;
            }
            set
            {
                _collectorLevel = value;
            }
        }

        public int CreatorLevel
        {
            get
            {
                if (!IsNew && _creatorLevel == IdNone)
                {
                    _creatorLevel = LoadedCustomerBase.CreatorLevel;
                }
                return _creatorLevel;
            }
            set
            {
                _creatorLevel = value;
            }
        }

        public int AccountLevel
        {
            get
            {
                if (!IsNew && _accountLevel == IdNone)
                {
                    _accountLevel = LoadedCustomerBase.AccountLevel;
                }
                return _accountLevel;
            }
            set
            {
                _accountLevel = value;
            }
        }

        public int BaseChannelID
        {
            get
            {
                if (!IsNew && _baseChannelId == -1)
                {
                    _baseChannelId = LoadedCustomerBase.BaseChannelID;
                }
                return _baseChannelId;
            }
            set
            {
                _baseChannelId = value;
            }
        }

        public Contact GeneralContact
        {
            get
            {
                if (!IsNew && _generalContact == null)
                {
                    _generalContact = LoadedCustomerBase.GeneralContact;
                }
                return _generalContact;
            }
            set
            {
                _generalContact = value;
            }
        }

        public string WebAddress
        {
            get
            {
                if (!IsNew && _webAddress == null)
                {
                    _webAddress = LoadedCustomerBase.WebAddress;
                }
                return _webAddress;
            }
            set
            {
                _webAddress = value;
            }
        }

        protected void MapBillingContactParametersBase(SqlCommand command)
        {
            command.Parameters.Add(ParameCustomerId, SqlDbType.Int, 4).Value = ID;
            command.Parameters.Add(ParamSalutation, SqlDbType.VarChar, 10).Value = BillingContact.Salutation;
            command.Parameters.Add(ParamContactName, SqlDbType.VarChar, 50).Value = BillingContact.ContactName;
            command.Parameters.Add(ParamFirstName, SqlDbType.VarChar, 50).Value = BillingContact.FirstName;
            command.Parameters.Add(ParamLastName, SqlDbType.VarChar, 50).Value = BillingContact.LastName;
            command.Parameters.Add(ParamContactTitle, SqlDbType.VarChar, 50).Value = BillingContact.ContactTitle;
            command.Parameters.Add(ParamPhone, SqlDbType.VarChar, 50).Value = BillingContact.Phone;
            command.Parameters.Add(ParamFax, SqlDbType.VarChar, 50).Value = BillingContact.Fax;
            command.Parameters.Add(ParamEmail, SqlDbType.VarChar, 50).Value = BillingContact.Email;
            command.Parameters.Add(ParamStreetAddress, SqlDbType.VarChar, 200).Value = BillingContact.StreetAddress;
            command.Parameters.Add(ParamCity, SqlDbType.VarChar, 50).Value = BillingContact.City;
            command.Parameters.Add(ParamState, SqlDbType.VarChar, 50).Value = BillingContact.State;
            command.Parameters.Add(ParamCountry, SqlDbType.VarChar, 20).Value = BillingContact.Country;
            command.Parameters.Add(ParamZip, SqlDbType.VarChar, 20).Value = BillingContact.Zip;
        }

        public bool CustomerNameExists()
        {
            var countObj = DataFunctions.ExecuteScalar(
                    $"SELECT Count(*) from [Customer] where CustomerName = '{DataFunctions.CleanString(Name)}' " +
                    $"and IsDeleted = 0 and BaseChannelID = {BaseChannelID}");
            var count = 0;
            if (countObj != null)
            {
                int.TryParse(countObj.ToString(), out count);
            }
            return count > 0;
        }

        public string Validate()
        {
            var validateMessage = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Name))
            {
                validateMessage.Append("Customer Name is missing/");
            }
            if (GeneralContact == null)
            {
                validateMessage.Append("General Contact is missing/");
            }

            if (BillingContact == null)
            {
                validateMessage.Append("Billing Contact is missing/");
            }

            return validateMessage.ToString();
        }

        public void CreateMasterSupressionGroup()
        {
            AssertCustomerIsSaved("Create master supression group");
            var communicatorDb = ConfigurationManager.AppSettings[AppSettingcommunicatordb];
            DataFunctions.Execute(
                $"insert into {communicatorDb}.dbo.Groups " +
                "(CustomerID , GroupName, OwnerTypeCode,MasterSupression) " +
                $"values ({ID},\'Master Suppression\',\'customer\',1)");
        }

        protected void AssertCustomerIsSaved(string message)
        {
            if (ID < 0)
            {
                throw new InvalidOperationException($"{message} is not allowed before a customer is saved.");
            }
        }

        public void CreatePickupConfig(string pickupPath, string mailingIP)
        {
            AssertCustomerIsSaved("Create pickup directory");
            CustomerConfig.CreatePickupConfig(ID, pickupPath);
            CustomerConfig.CreateComConfig(ID, "MailingIP", mailingIP);
        }

        public virtual void MapCustomerGeneralContact(SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Parameters.Add(ParamBaseChannelId, SqlDbType.Int, 4).Value = BaseChannelID;
            command.Parameters.Add(ParamCustomerName, SqlDbType.VarChar, 150).Value = Name;
            command.Parameters.Add(ParamAddress, SqlDbType.VarChar, 255).Value = GeneralContact.StreetAddress;
            command.Parameters.Add(ParamCity, SqlDbType.VarChar, 255).Value = GeneralContact.City;
            command.Parameters.Add(ParamState, SqlDbType.VarChar, 255).Value = GeneralContact.State;
            command.Parameters.Add(ParamCountry, SqlDbType.VarChar, 50).Value = GeneralContact.Country;
            command.Parameters.Add(ParamZip, SqlDbType.VarChar, 50).Value = GeneralContact.Zip;
            command.Parameters.Add(ParamSalutation, SqlDbType.VarChar, 10).Value = GeneralContact.Salutation;
            command.Parameters.Add(ParamContactName, SqlDbType.VarChar, 255).Value = GeneralContact.ContactName;
            command.Parameters.Add(ParamFirstName, SqlDbType.VarChar, 50).Value = GeneralContact.FirstName;
            command.Parameters.Add(ParamLastName, SqlDbType.VarChar, 50).Value = GeneralContact.LastName;
            command.Parameters.Add(ParamContactTitle, SqlDbType.VarChar, 255).Value = GeneralContact.ContactTitle;
            command.Parameters.Add(ParamEmail, SqlDbType.VarChar, 255).Value = GeneralContact.Email;
            command.Parameters.Add(ParamPhone, SqlDbType.VarChar, 255).Value = GeneralContact.Phone;
            command.Parameters.Add(ParamFax, SqlDbType.VarChar, 50).Value = GeneralContact.Fax;
        }

        public virtual void MapCustomerLevelInformation(SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Parameters.Add(ParamWebAddress, SqlDbType.VarChar, 255).Value = WebAddress;
            command.Parameters.Add(ParamTechContact, SqlDbType.VarChar, 50).Value = TechContact;
            command.Parameters.Add(ParamTechEmail, SqlDbType.VarChar, 255).Value = TechEmail;
            command.Parameters.Add(ParamTechPhone, SqlDbType.VarChar, 255).Value = TechPhone;
            command.Parameters.Add(ParamSubscriptionsEmail, SqlDbType.VarChar, 255).Value = SubscriptionsEmail;
        }

        protected virtual CustomerBase LoadedCustomerBase
        {
            get { throw new NotImplementedException(); }
        }

        protected virtual Contact GetBillingContact(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
