using Circulation.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for PaidNew.xaml
    /// </summary>
    public partial class PaidNew : UserControl, INotifyPropertyChanged
    {
        //Paid exists within a tab on the SubscriptionContainer module. It allows editing of all SubscriptionPaid information.
        #region Properties

        #region Private
        private int _paymentTypeID;
        private int _creditCardTypeID;
        private int _deliverID;
        private decimal _amountPaid;
        private decimal _amount;
        private decimal _balanceDue;
        private int _priceCodeID;
        private decimal _copyRate;
        private string _creditCard;
        private string _ccExpirationMonth;
        private string _ccExpirationYear;
        private string _checkNumber;
        private string _ccHolderName;
        private int _copies;
        private int _term;
        private int _numberIssues;
        private int _issuesToGo;
        private int _totalIssues;
        private DateTime _payDate;
        private DateTime _startIssueDate;
        private DateTime _expireDate;
        private bool _madePaidChange;
        private int _graceIssues;
        private decimal _writeOff;
        private string _otherType;
        private int _categoryCodeID;
        private int _frequency;
        private bool _isCopiesEnabled;
        private bool _enabled;
        #endregion

        public int PaymentTypeID
        {
            get { return _paymentTypeID; }
            set
            {
                _paymentTypeID = value;
                mySubscriptionPaid.PaymentTypeID = _paymentTypeID;
                CheckPaymentType();
                if(_paymentTypeID != originalPaid.PaymentTypeID)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PaymentTypeID"));
                }
            }
        }
        public int CreditCardTypeID
        {
            get { return _creditCardTypeID; }
            set
            {
                _creditCardTypeID = value;
                mySubscriptionPaid.CreditCardTypeID = _creditCardTypeID;
                if (_creditCardTypeID != originalPaid.CreditCardTypeID)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CreditCardTypeID"));
                }
            }
        }
        public int DeliverID
        {
            get { return _deliverID; }
            set
            {
                _deliverID = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DeliverID"));
                }
            }
        }
        public decimal AmountPaid
        {
            get { return _amountPaid; }
            set
            {
                Decimal.TryParse(value.ToString(), out _amountPaid);
                mySubscriptionPaid.AmountPaid = _amountPaid;
                if (_amountPaid != originalPaid.AmountPaid)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AmountPaid"));
                }
            }
        }
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                Decimal.TryParse(value.ToString(), out _amount);
                mySubscriptionPaid.Amount = _amount;
                if (_amount != originalPaid.Amount)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Amount"));
                }
            }
        }
        public decimal BalanceDue
        {
            get { return _balanceDue; }
            set
            {
                _balanceDue = value;
                mySubscriptionPaid.BalanceDue = _balanceDue;
                if (_balanceDue != originalPaid.BalanceDue)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BalanceDue"));
                }
            }
        }
        public int PriceCodeID
        {
            get { return _priceCodeID; }
            set
            {
                _priceCodeID = value;
                mySubscriptionPaid.PriceCodeID = _priceCodeID;
                if (_priceCodeID != originalPaid.PriceCodeID)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PriceCodeID"));
                }
            }
        }
        public decimal CopyRate
        {
            get { return _copyRate; }
            set
            {
                _copyRate = value;
                this.MadePaidChange = true;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CopyRate"));
                }
            }
        }
        public string CreditCard
        {
            get { return _creditCard; }
            set
            {
                //int.TryParse((value).ToString(), out _creditCard);
                _creditCard = value;
                mySubscriptionPaid.CCNumber = _creditCard.ToString().Trim();
                if (_creditCard.ToString() != originalPaid.CCNumber)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CreditCard"));
                }
            }
        }
        public string CCExpirationMonth
        {
            get { return _ccExpirationMonth; }
            set
            {
                _ccExpirationMonth = value;
                mySubscriptionPaid.CCExpirationMonth = _ccExpirationMonth.Trim();
                if (_ccExpirationMonth != originalPaid.CCExpirationMonth.Trim())
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CCExpirationMonth"));
                }
            }
        }
        public string CCExpirationYear
        {
            get { return _ccExpirationYear; }
            set
            {
                _ccExpirationYear = value;
                mySubscriptionPaid.CCExpirationYear = _ccExpirationYear;
                if (_ccExpirationYear != originalPaid.CCExpirationYear)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CCExpirationYear"));
                }
            }
        }
        public string CheckNumber
        {
            get { return _checkNumber; }
            set
            {
                _checkNumber = (value ?? "");
                mySubscriptionPaid.CheckNumber = _checkNumber.ToString();
                if (_checkNumber.ToString() != originalPaid.CheckNumber)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CheckNumber"));
                }
            }
        }
        public string CCHolderName
        {
            get { return _ccHolderName; }
            set
            {
                _ccHolderName = (value ?? "");
                mySubscriptionPaid.CCHolderName = _ccHolderName;
                if (_ccHolderName != originalPaid.CCHolderName)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CCHolderName"));
                }
            }
        }
        public int Copies
        {           
            set 
            {
                if (value > 0 && _copies != value)
                {
                    _copies = value;
                    //this.MadePaidChange = true;
                    if (CopiesChanged != null)
                        CopiesChanged(Copies);

                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Copies"));
                    }
                }
            } 
            get { return _copies; }
        }
        public int Term
        {
            get { return _term; }
            set
            {
                _term = value;
                mySubscriptionPaid.Term = _term;
                if (_term != originalPaid.Term)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Term"));
                }
            }
        }
        public int TotalIssues
        {
            get { return _totalIssues; }
            set
            {
                _totalIssues = value;
                mySubscriptionPaid.TotalIssues = _totalIssues;
                if (_totalIssues != originalPaid.TotalIssues)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TotalIssues"));
                }
            }
        }
        public DateTime PayDate
        {
            get { return _payDate; }
            set
            {
                _payDate = value;
                mySubscriptionPaid.PaidDate = _payDate;
                if (_payDate != originalPaid.PaidDate)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PayDate"));
                }
            }
        }
        public DateTime StartIssueDate
        {
            get { return _startIssueDate; }
            set
            {
                _startIssueDate = value;
                mySubscriptionPaid.StartIssueDate = _startIssueDate;
                if (_startIssueDate != originalPaid.StartIssueDate)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StartIssueDate"));
                }
            }
        }
        public DateTime ExpireDate
        {
            get { return _expireDate; }
            set
            {
                _expireDate = value;
                mySubscriptionPaid.ExpireIssueDate = _expireDate;
                if (_expireDate != originalPaid.ExpireIssueDate)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ExpireDate"));
                }
            }
        }
        public bool MadePaidChange
        {
            get { return _madePaidChange; }
            set
            {
                if (_madePaidChange != value)
                {
                    _madePaidChange = value;
                    if (InfoChanged != null)
                        InfoChanged(_madePaidChange);
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MadePaidChange"));
                }
            }
        }
        public int GraceIssues
        {
            get { return _graceIssues; }
            set
            {
                _graceIssues = value;
                mySubscriptionPaid.GraceIssues = _graceIssues;
                if (_graceIssues != originalPaid.GraceIssues)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("GraceIssues"));
                }
            }
        }
        public decimal WriteOff
        {
            get { return _writeOff; }
            set
            {
                _writeOff = value;
                mySubscriptionPaid.WriteOffAmount = _writeOff;
                if (_writeOff != originalPaid.WriteOffAmount)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("WriteOff"));
                }
            }
        }
        public string OtherType
        {
            get { return _otherType; }
            set
            {
                _otherType = value;
                mySubscriptionPaid.OtherType = _otherType;
                if (_otherType != originalPaid.OtherType)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("OtherType"));
                }
            }
        }
        private int CategoryCodeID
        {
            get { return _categoryCodeID; }
            set
            {
                _categoryCodeID = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CategoryCodeID"));
                }
            }
        }
        public int Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;
                mySubscriptionPaid.Frequency = _frequency;
                if (_frequency != originalPaid.Frequency)
                    this.MadePaidChange = true;
                else if (CheckChanges() == false)
                    this.MadePaidChange = false;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Frequency"));
                }
            }
        }
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Enabled"));
                }
            }
        }
        public bool IsCopiesEnabled
        {
            get { return _isCopiesEnabled; }
            set
            {
                _isCopiesEnabled = value;
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsCopiesEnabled"));
                }
            }
        }
        public event Action<int> CopiesChanged;
        public event Action<bool> InfoChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Entity/Lists
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription = new FrameworkUAD.Entity.ProductSubscription();
        private FrameworkUAD.Entity.SubscriptionPaid mySubscriptionPaid = new FrameworkUAD.Entity.SubscriptionPaid();
        private List<FrameworkUAD_Lookup.Entity.Action> actionList = new List<FrameworkUAD_Lookup.Entity.Action>();
        private List<FrameworkUAD_Lookup.Entity.Country> countryList = new List<FrameworkUAD_Lookup.Entity.Country>();
        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> paymentTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> creditCardTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private FrameworkUAD.Entity.SubscriptionPaid originalPaid = new FrameworkUAD.Entity.SubscriptionPaid();
        #endregion
        #region Workers
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IPriceCode> pcWorker = FrameworkServices.ServiceClient.UAD_PriceCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeW = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        #endregion
        #region Response
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.PriceCode>> priceCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.PriceCode>>();
        #endregion

        public PaidNew(FrameworkUAD.Entity.ProductSubscription subscriber, FrameworkUAD.Entity.SubscriptionPaid subscriptionPaid)
        {
            InitializeComponent();
            actionList = Home.Actions;
            codeTypeList = Home.CodeTypes;
            codeList = Home.Codes;

            mySubscriptionPaid = subscriptionPaid;
            myProductSubscription = subscriber;
            countryList = Home.Countries;
            transCodeList = Home.TransactionCodes;
            catCodeList = Home.CategoryCodes;

            int paymentTypeId = codeTypeList.SingleOrDefault(s => s.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.Payment.ToString()).CodeTypeId;
            int ccTypeId = codeTypeList.SingleOrDefault(s => s.CodeTypeName == FrameworkUAD_Lookup.Enums.CodeType.Credit_Card.ToString().Replace("_", " ")).CodeTypeId;

            paymentTypes = codeList.Where(x => x.CodeTypeId == paymentTypeId).ToList();
            if (paymentTypes.Where(x => x.CodeId == 0).Count() == 0)
                paymentTypes.Add(new FrameworkUAD_Lookup.Entity.Code() { CodeId = 0, CodeTypeId = paymentTypeId, IsActive = true, CodeValue = "", CodeName = "", DisplayName = "" });

            cbPaymentType.ItemsSource = paymentTypes.OrderBy(x=> x.CodeId);
            cbPaymentType.SelectedValuePath = "CodeId";
            cbPaymentType.DisplayMemberPath = "CodeName";

            creditCardTypes = codeList.Where(x => x.CodeTypeId == ccTypeId).ToList();
            if (creditCardTypes.Where(x => x.CodeId == 0).Count() == 0)
                creditCardTypes.Add(new FrameworkUAD_Lookup.Entity.Code() { CodeId = 0, CodeTypeId = ccTypeId, IsActive = true, CodeValue = "", CodeName = "", DisplayName = "" });

            cbCreditCardType.ItemsSource = creditCardTypes.OrderBy(x=> x.CodeId);
            cbCreditCardType.SelectedValuePath = "CodeId";
            cbCreditCardType.DisplayMemberPath = "CodeName";

            mySubscriptionPaid.CCNumber = mySubscriptionPaid.CCNumber.Trim();
            originalPaid = new FrameworkUAD.Entity.SubscriptionPaid(mySubscriptionPaid);

            SetUpMonthYearcb();

            LoadData();

            this.DataContext = this;
        }

        private void LoadData()
        {
            codeResponse = codeW.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Payment);
            if (Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                paymentTypes = codeResponse.Result;

            PaymentTypeID = mySubscriptionPaid.PaymentTypeID;
            CreditCardTypeID = mySubscriptionPaid.CreditCardTypeID;
            Amount = mySubscriptionPaid.Amount;
            CCHolderName = mySubscriptionPaid.CCHolderName;
            AmountPaid = mySubscriptionPaid.AmountPaid;
            TotalIssues = mySubscriptionPaid.TotalIssues;
            Frequency = mySubscriptionPaid.Frequency;
            Term = mySubscriptionPaid.Term;
            CCExpirationMonth = mySubscriptionPaid.CCExpirationMonth.Trim();
            CCExpirationYear = mySubscriptionPaid.CCExpirationYear;
            CheckNumber = mySubscriptionPaid.CheckNumber;
            CCHolderName = mySubscriptionPaid.CCHolderName;
            PayDate = mySubscriptionPaid.PaidDate;
            StartIssueDate = mySubscriptionPaid.StartIssueDate;
            ExpireDate = mySubscriptionPaid.ExpireIssueDate;
            GraceIssues = mySubscriptionPaid.GraceIssues;
            OtherType = mySubscriptionPaid.OtherType;

            //int cc = 0;
            //int.TryParse(mySubscriptionPaid.CCNumber, out cc);
            //if(cc > 0)
            CreditCard = mySubscriptionPaid.CCNumber;

            if (myProductSubscription.IsNewSubscription == true)
                _copies = 1;
            else
                _copies = myProductSubscription.Copies;

            if (myProductSubscription.PubCategoryID > 0)
            {
                FrameworkUAD_Lookup.Entity.CategoryCode c = catCodeList.Where(x => x.CategoryCodeID == myProductSubscription.PubCategoryID).FirstOrDefault();
                CategoryCodeID = myProductSubscription.PubCategoryID;
                if (c != null)
                {
                    if ((CategoryCodeID != null || CategoryCodeID != 0) && (c.CategoryCodeValue == 11 || c.CategoryCodeValue == 21 || c.CategoryCodeValue == 25 || c.CategoryCodeValue == 28 || c.CategoryCodeValue == 31 ||
                            c.CategoryCodeValue == 35 || c.CategoryCodeValue == 51 || c.CategoryCodeValue == 56 || c.CategoryCodeValue == 62))
                        iudCopies.IsEnabled = true;
                    else
                        iudCopies.IsEnabled = false;
                }
            }
            this.MadePaidChange = false;
        }
        #region UI Events
        public void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Common.NumberValidation(sender, e);
        }
        #endregion
        #region Helpers
        public class ComboBoxPairsMonth
        {
            public string MonthNumber { get; set; }
            public string MonthName { get; set; }

            public ComboBoxPairsMonth(string monthNumber, string monthName)
            {
                MonthNumber = monthNumber;
                MonthName = monthName;
            }
        }
        private void SetUpMonthYearcb()
        {
            List<ComboBoxPairsMonth> cbm = new List<ComboBoxPairsMonth>();
            List<string> cby = new List<string>();

            cbm.Add(new ComboBoxPairsMonth("1", "1, Janurary"));
            cbm.Add(new ComboBoxPairsMonth("2", "2, February"));
            cbm.Add(new ComboBoxPairsMonth("3", "3, March"));
            cbm.Add(new ComboBoxPairsMonth("4", "4, April"));
            cbm.Add(new ComboBoxPairsMonth("5", "5, May"));
            cbm.Add(new ComboBoxPairsMonth("6", "6, June"));
            cbm.Add(new ComboBoxPairsMonth("7", "7, July"));
            cbm.Add(new ComboBoxPairsMonth("8", "8, August"));
            cbm.Add(new ComboBoxPairsMonth("9", "9, September"));
            cbm.Add(new ComboBoxPairsMonth("10", "10, October"));
            cbm.Add(new ComboBoxPairsMonth("11", "11, November"));
            cbm.Add(new ComboBoxPairsMonth("12", "12, December"));

            cbMonth.ItemsSource = cbm;
            cbMonth.SelectedValuePath = "MonthNumber";
            cbMonth.DisplayMemberPath = "MonthName";

            // Build years
            for (int i = 0; i < 15; i++)
            {
                int year = DateTime.Now.Year - i;
                cby.Add(year.ToString());
            }
            int yearCounter = 0;
            for (int i = 15; i <= 30; i++)
            {
                yearCounter += 1;
                int year = DateTime.Now.Year + yearCounter;
                cby.Add(year.ToString());
            }

            cbYear.ItemsSource = cby.OrderBy(x=> x).ToList();
        }
        private void CheckPaymentType()
        {
            if (_paymentTypeID == paymentTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.PaymentType.Check.ToString()).Select(x => x.CodeId).FirstOrDefault())
            {
                lbCreditCardType.Visibility = System.Windows.Visibility.Hidden;
                cbCreditCardType.Visibility = System.Windows.Visibility.Hidden;

                stkMonthYearExp.Visibility = System.Windows.Visibility.Hidden;

                lbName.Visibility = System.Windows.Visibility.Visible;
                tbNameOn.Visibility = System.Windows.Visibility.Visible;
                lbPaymentNumber.Visibility = System.Windows.Visibility.Visible;
                tbCreditCard.Visibility = System.Windows.Visibility.Visible;
                lbPaymentNumber.Text = "Check Number:";
                lbName.Text = "Name:";

                tbCreditCard.Visibility = System.Windows.Visibility.Hidden;
                tbCheckNumber.Visibility = System.Windows.Visibility.Visible;
                //tbOtherType.Visibility = System.Windows.Visibility.Hidden;

                lbCCMonthYear.Visibility = System.Windows.Visibility.Hidden;
                lbCCMonthYearBreak.Visibility = System.Windows.Visibility.Hidden;
                cbMonth.Visibility = System.Windows.Visibility.Hidden;
                cbYear.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (_paymentTypeID == paymentTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.PaymentType.Credit_Card.ToString().ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
            {
                lbCreditCardType.Visibility = System.Windows.Visibility.Visible;
                cbCreditCardType.Visibility = System.Windows.Visibility.Visible;

                stkMonthYearExp.Visibility = System.Windows.Visibility.Visible;

                lbName.Visibility = System.Windows.Visibility.Visible;
                tbNameOn.Visibility = System.Windows.Visibility.Visible;
                lbPaymentNumber.Visibility = System.Windows.Visibility.Visible;
                lbPaymentNumber.Text = "Credit Card Last 4:";
                lbName.Text = "Name:";

                tbCreditCard.Visibility = System.Windows.Visibility.Visible;
                tbCheckNumber.Visibility = System.Windows.Visibility.Hidden;
                //tbOtherType.Visibility = System.Windows.Visibility.Hidden;

                lbCCMonthYear.Visibility = System.Windows.Visibility.Visible;
                lbCCMonthYearBreak.Visibility = System.Windows.Visibility.Visible;
                cbMonth.Visibility = System.Windows.Visibility.Visible;
                cbYear.Visibility = System.Windows.Visibility.Visible;
            }
            //else if (cbPaymentType.Text.Equals("Other", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    lbCreditCardType.Visibility = System.Windows.Visibility.Hidden;
            //    cbCreditCardType.Visibility = System.Windows.Visibility.Hidden;
            //    stkMonthYearExp.Visibility = System.Windows.Visibility.Hidden;

            //    //lbNameOn.Content = "Name on Credit Card:";
            //    tbNameOn.Visibility = System.Windows.Visibility.Hidden;
            //    lbPaymentNumber.Visibility = System.Windows.Visibility.Hidden;

            //    tbCreditCard.Visibility = System.Windows.Visibility.Hidden;
            //    tbCheckNumber.Visibility = System.Windows.Visibility.Hidden;
            //    tbOtherType.Visibility = System.Windows.Visibility.Visible;

            //    lbCCMonthYear.Visibility = System.Windows.Visibility.Hidden;
            //    lbCCMonthYearBreak.Visibility = System.Windows.Visibility.Hidden;
            //    cbMonth.Visibility = System.Windows.Visibility.Hidden;
            //    cbYear.Visibility = System.Windows.Visibility.Hidden;

            //    lbName.Visibility = System.Windows.Visibility.Visible;
            //    lbName.Text = "Other Type:";
            //}
            else if (cbPaymentType.Text == null || _paymentTypeID == paymentTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.PaymentType.Bill_Me_Later.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault())
            {
                lbCreditCardType.Visibility = System.Windows.Visibility.Hidden;
                cbCreditCardType.Visibility = System.Windows.Visibility.Hidden;
                lbName.Visibility = System.Windows.Visibility.Hidden;
                tbNameOn.Visibility = System.Windows.Visibility.Hidden;
                //lbNameOn.Content = string.Empty;
                stkMonthYearExp.Visibility = System.Windows.Visibility.Hidden;
                lbPaymentNumber.Visibility = System.Windows.Visibility.Hidden;
                lbPaymentNumber.Text = string.Empty;

                tbCreditCard.Visibility = System.Windows.Visibility.Hidden;
                tbCheckNumber.Visibility = System.Windows.Visibility.Hidden;
                //tbOtherType.Visibility = System.Windows.Visibility.Hidden;

                lbCCMonthYear.Visibility = System.Windows.Visibility.Hidden;
                lbCCMonthYearBreak.Visibility = System.Windows.Visibility.Hidden;
                cbMonth.Visibility = System.Windows.Visibility.Hidden;
                cbYear.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private bool CheckChanges()
        {
            bool infoChanged = false;
            if (this.PaymentTypeID != originalPaid.PaymentTypeID)
                infoChanged = true;
            if (this.CreditCardTypeID != originalPaid.CreditCardTypeID)
                infoChanged = true;
            if (this.AmountPaid != originalPaid.AmountPaid)
                infoChanged = true;
            if (this.Amount != originalPaid.Amount)
                infoChanged = true;
            if (this.CreditCard != null && this.CreditCard.ToString().Trim() != originalPaid.CCNumber.Trim())
                infoChanged = true;
            if (this.BalanceDue != originalPaid.BalanceDue)
            {
                infoChanged = true;
            }
            if (this.PriceCodeID != originalPaid.PriceCodeID)
            {
                infoChanged = true;
            }
            if (this.CCExpirationMonth != originalPaid.CCExpirationMonth.Trim())
            {
                infoChanged = true;
            }
            if (this.CCExpirationYear != originalPaid.CCExpirationYear)
            {
                infoChanged = true;
            }
            if (this.CheckNumber != originalPaid.CheckNumber)
            {
                infoChanged = true;
            }
            if (this.CCHolderName != originalPaid.CCHolderName)
            {
                infoChanged = true;
            }
            if (this.Term != originalPaid.Term)
            {
                infoChanged = true;
            } 
            if (this.TotalIssues != originalPaid.TotalIssues)
            {
                infoChanged = true;
            } 
            if (this.PayDate != originalPaid.PaidDate)
            {
                infoChanged = true;
            }
            if (this.StartIssueDate != originalPaid.StartIssueDate)
                infoChanged = true;
            if (this.ExpireDate != originalPaid.ExpireIssueDate)
                infoChanged = true;
            if (this.GraceIssues != originalPaid.GraceIssues)
                infoChanged = true;
            if (this.WriteOff != originalPaid.WriteOffAmount)
                infoChanged = true;
            if (this.OtherType != originalPaid.OtherType)
                infoChanged = true;
            if (this.Frequency != originalPaid.Frequency)
                infoChanged = true;

            return infoChanged;
        }
        #endregion
    }
}
