using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using Circulation.Helpers;
using Telerik.Windows.Controls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for Response.xaml
    /// </summary>
    public partial class Response : UserControl
    {
        #region Entity
        private KMPlatform.Entity.Client myClient { get; set; }
        private KMPlatform.Object.Product myProduct { get; set; }
        //private FrameworkUAD.Entity.Subscription mySubscription { get; set; }
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription { get; set; }
        private List<FrameworkUAD.Entity.ResponseGroup> Questions { get; set; }
        private List<FrameworkUAD.Entity.CodeSheet> Answers { get; set; }
        public List<FrameworkUAD.Entity.MarketingMap> MarketingMapList { get; set; }
        //public List<FrameworkUAD.Entity.SubscriptionResponseMap> ResponseMapList { get; set; }
        private List<FrameworkUAD_Lookup.Entity.Code> mlist;
        private List<FrameworkUAD_Lookup.Entity.Code> parList;
        private List<FrameworkUAD_Lookup.Entity.Code> qSourceList;
        private List<FrameworkUAD_Lookup.Entity.Action> actionList;
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypeList;
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList;
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodeList;
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypeList;
        private List<FrameworkUAD_Lookup.Entity.Code> codeList;
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList;
        public List<FrameworkUAD.Entity.ProductSubscriptionDetail> psdList;
        #endregion
        #region Worker
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> p3Worker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> qsWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> aWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> mWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IMarketingMap> mmWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> psdWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rtWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> rWorker;
        //private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriptionResponseMap> srmWorker;
        #endregion
        #region Service Response
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> qualSouResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> par3cResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> marketingResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MarketingMap>> mmResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> psdResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> rtResponse;
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> rResponse;
        //private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriptionResponseMap>> srmResponse;
        #endregion
        #region Var
        public List<string> QuestionNotAnswered;
        public int QSourceID { get; set; }
        public int Par3C { get; set; }
        public bool MadeResponseChange { get; set; }
        public bool RequalOnlyChange { get; set; }
        public int Copies { get; set; }
        //private bool isDisabledControls = false;
        private bool firstLoad = true;
        public bool QualDateChanged = false;
        public bool isTriggerQualDateChange = false;
        private int catCodeTypeId = 0;
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        #endregion
        public Response(KMPlatform.Entity.Client publisher, KMPlatform.Object.Product product,
            FrameworkUAD.Entity.ProductSubscription prdSubscription, List<FrameworkUAD.Entity.MarketingMap> mmList, List<FrameworkUAD.Entity.ResponseGroup> rtList, List<FrameworkUAD.Entity.CodeSheet> rList, 
            List<FrameworkUAD_Lookup.Entity.Code> par3, List<FrameworkUAD_Lookup.Entity.Code> qSource, List<FrameworkUAD_Lookup.Entity.Action> action,
            List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catType, List<FrameworkUAD_Lookup.Entity.CategoryCode> catCode, List<FrameworkUAD_Lookup.Entity.TransactionCode> transCode, List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transTypeCode,
            List<FrameworkUAD_Lookup.Entity.Code> code, List<FrameworkUAD_Lookup.Entity.CodeType> codeType, bool isLockedMode = false, int catTypeId = 0)
        {
            InitializeComponent();
            MadeResponseChange = false;
            myClient = publisher;
            myProduct = product;
            //mySubscription = subscription;
            myProductSubscription = prdSubscription;
            Questions = rtList;
            Answers = rList;
            parList = par3;
            qSourceList = qSource;
            actionList = action;
            catTypeList = catType;
            catCodeList = catCode;
            catCodeTypeId = catTypeId;
            transCodeList = transCode;
            codeList = code;
            codeTypeList = codeType;
            transCodeTypeList = transTypeCode;

            Copies = 1;

            p3Worker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            qsWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();

            rtWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
            rWorker = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
            //srmWorker = FrameworkServices.ServiceClient.UAD_SubscriptionResponseMapClient();

            aWorker = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
            mWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            mmWorker = FrameworkServices.ServiceClient.UAD_MarketingMapClient();
            psdList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
            //ResponseMapList = new List<FrameworkUAD.Entity.SubscriptionResponseMap>();

            //if (srmList != null)
            //    ResponseMapList = srmList;

            MarketingMapList = mmList;

            LoadData();
        }
        private void LoadData()
        {
            //Par3c
            if (parList == null)
            {
                par3cResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
                par3cResponse = p3Worker.Proxy.Select(accessKey);
                if (par3cResponse != null && par3cResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    parList = par3cResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            cbPar3C.ItemsSource = parList;
            cbPar3C.DisplayMemberPath = "DisplayName";
            cbPar3C.SelectedValuePath = "Par3CID";

            //QSource
            if (qSourceList == null)
            {
                qualSouResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
                qualSouResponse = qsWorker.Proxy.Select(accessKey);
                if (qualSouResponse != null && qualSouResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    qSourceList = qualSouResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            cbQSource.ItemsSource = qSourceList;
            cbQSource.DisplayMemberPath = "DisplayName";
            cbQSource.SelectedValuePath = "QSourceID";

            // ResponseType/Question
            if (Questions == null || Questions.Count == 0)
            {
                rtResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
                rtResponse = rtWorker.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, myProduct.ProductID);
                if (rtResponse != null && rtResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    Questions = rtResponse.Result.Where(x => x.IsActive == true).OrderBy(y => y.DisplayOrder).ThenBy(z => z.DisplayName).ToList();
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            // Response/Answer
            if (Answers == null || Answers.Count == 0)
            {
                rResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
                rResponse = rWorker.Proxy.Select(accessKey, myProduct.ProductID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (rResponse != null && rResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    Answers = rResponse.Result.Where(x => x.IsActive == true).ToList();
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            psdList = myProductSubscription.ProductMapList;

            if (psdList == null || psdList.Count == 0)
            {
                psdWorker = FrameworkServices.ServiceClient.UAD_PubSubscriptionDetailClient();
                psdResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>>();
                psdResponse = psdWorker.Proxy.Select(accessKey, myProductSubscription.PubSubscriptionID, myClient.ClientConnections);
                if (Common.CheckResponse(psdResponse.Result, psdResponse.Status) == true)
                {
                    psdList = psdResponse.Result;
                    #region Validate List to ensure there is only one instance of each ResponseGroupID
                    List<FrameworkUAD.Entity.ProductSubscriptionDetail> deletes = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                    foreach(FrameworkUAD.Entity.ProductSubscriptionDetail psd in psdList)
                    {
                        int rGroupID = Answers.Where(x => x.CodeSheetID == psd.CodeSheetID).Select(x=> x.ResponseGroupID).FirstOrDefault();
                        foreach(FrameworkUAD.Entity.ProductSubscriptionDetail psd2 in psdList)
                        {
                            int rGroupID2 = Answers.Where(x => x.CodeSheetID == psd2.CodeSheetID).Select(x => x.ResponseGroupID).FirstOrDefault();
                            if(rGroupID == rGroupID2 && psd.CodeSheetID != psd2.CodeSheetID)
                            {
                                if (psd.DateCreated > psd2.DateCreated)
                                    deletes.Add(psd2);
                                else
                                    deletes.Add(psd);
                            }
                        }
                    }
                    psdList = psdList.Except(deletes).ToList();
                    #endregion
                }
            }
            else
            {
                #region Validate List to ensure there is only one instance of each ResponseGroupID
                List<FrameworkUAD.Entity.ProductSubscriptionDetail> deletes = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
                foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd in psdList)
                {
                    int rGroupID = Answers.Where(x => x.CodeSheetID == psd.CodeSheetID).Select(x => x.ResponseGroupID).FirstOrDefault();
                    bool? isMulti = Questions.Where(x => x.ResponseGroupID == rGroupID).Select(x=> x.IsMultipleValue).FirstOrDefault();
                    if (isMulti == false)
                    {
                        foreach (FrameworkUAD.Entity.ProductSubscriptionDetail psd2 in psdList)
                        {
                            int rGroupID2 = Answers.Where(x => x.CodeSheetID == psd2.CodeSheetID).Select(x => x.ResponseGroupID).FirstOrDefault();
                            if (rGroupID == rGroupID2 && psd.CodeSheetID != psd2.CodeSheetID)
                            {
                                if (psd.DateCreated > psd2.DateCreated)
                                    deletes.Add(psd2);
                                else
                                    deletes.Add(psd);
                            }
                        }
                    }
                }
                psdList = psdList.Except(deletes).ToList();
                #endregion
            }

            if (myProductSubscription.QualificationDate != null)
                dpQualDate.SelectedDate = myProductSubscription.QualificationDate;
            else
                dpQualDate.SelectedDate = DateTime.Today;

            if (myProductSubscription.PubQSourceID != 0)
            {
                cbQSource.SelectedValue = myProductSubscription.PubQSourceID;
                QSourceID = myProductSubscription.PubQSourceID;
            }

            if (myProductSubscription.Par3CID > 0)
            {

                cbPar3C.SelectedValue = myProductSubscription.Par3CID;
                Par3C = myProductSubscription.Par3CID;
            }

            if (actionList == null)
            {
                FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>> actionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>>();
                actionResponse = aWorker.Proxy.Select(accessKey);
                if (actionResponse != null && actionResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    actionList = actionResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
            }

            bool addAsterisk = false;

            if (myProductSubscription.PubTransactionID > 0)
            {
                if (catCodeTypeId < 0 && myProductSubscription.IsNewSubscription == false)
                {
                    int CategoryID = myProductSubscription.PubCategoryID;
                    int tr = 0;

                    if (myProductSubscription.IsPaid == false)
                    {
                        if (myProductSubscription.IsSubscribed == true)
                            tr = transCodeList.SingleOrDefault(x => x.TransactionCodeID == myProductSubscription.PubTransactionID && x.TransactionCodeTypeID == transCodeTypeList.SingleOrDefault(r => r.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Active.ToString().Replace("_", " "))).TransactionCodeTypeID).TransactionCodeID;
                        else
                            tr = transCodeList.SingleOrDefault(x => x.TransactionCodeID == myProductSubscription.PubTransactionID && x.TransactionCodeTypeID == transCodeTypeList.SingleOrDefault(r => r.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Inactive.ToString().Replace("_", " "))).TransactionCodeTypeID).TransactionCodeID;
                    }
                    else
                    {
                        if (myProductSubscription.IsSubscribed == true)
                            tr = transCodeList.SingleOrDefault(x => x.TransactionCodeID == myProductSubscription.PubTransactionID && x.TransactionCodeTypeID == transCodeTypeList.SingleOrDefault(r => r.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Active.ToString().Replace("_", " "))).TransactionCodeTypeID).TransactionCodeID;
                        else
                            tr = transCodeList.SingleOrDefault(x => x.TransactionCodeID == myProductSubscription.PubTransactionID && x.TransactionCodeTypeID == transCodeTypeList.SingleOrDefault(r => r.TransactionCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Inactive.ToString().Replace("_", " "))).TransactionCodeTypeID).TransactionCodeID;
                    }
                    int codeTypeId = codeTypeList.SingleOrDefault(y => y.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Action.ToString())).CodeTypeId;

                    FrameworkUAD_Lookup.Entity.Action soloAction = actionList.SingleOrDefault(a => a.CategoryCodeID == CategoryID && a.TransactionCodeID == tr && a.ActionTypeID == codeList.SingleOrDefault(x => x.CodeTypeId == codeTypeId && x.CodeName.Equals(FrameworkUAD_Lookup.Enums.ActionTypes.Data_Entry.ToString().Replace("_", " "))).CodeId);

                    if (myProductSubscription != null && myProductSubscription.IsSubscribed == true && soloAction != null &&
                        (soloAction.CategoryCodeID == 2 || soloAction.CategoryCodeID == 6 || soloAction.CategoryCodeID == 12 || soloAction.CategoryCodeID == 16 || soloAction.CategoryCodeID == 24 ||
                        soloAction.CategoryCodeID == 8 || soloAction.CategoryCodeID == 20 || soloAction.CategoryCodeID == 22 || soloAction.CategoryCodeID == 28))
                    {
                        iudCopies.IsEnabled = true;
                    }
                    else
                    {
                        iudCopies.IsEnabled = false;
                    }

                    iudCopies.Value = myProductSubscription.Copies;
                    Copies = myProductSubscription.Copies;

                    // Get catType, will determine if asterisk should be added after the question

                    if (soloAction != null)
                    {
                        FrameworkUAD_Lookup.Entity.CategoryCodeType ccType = new FrameworkUAD_Lookup.Entity.CategoryCodeType();
                        int cctId = catCodeList.First(x => x.CategoryCodeID == soloAction.CategoryCodeID).CategoryCodeTypeID;

                        ccType = catTypeList.SingleOrDefault(z => z.CategoryCodeTypeID == cctId);
                        if (ccType.CategoryCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ")) ||
                           ccType.CategoryCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ")))
                        {
                            addAsterisk = true;
                        }
                    }
                }
            }

            if (catCodeTypeId > 0)
            {
                FrameworkUAD_Lookup.Entity.CategoryCodeType ccType = new FrameworkUAD_Lookup.Entity.CategoryCodeType();

                ccType = catTypeList.SingleOrDefault(z => z.CategoryCodeTypeID == catCodeTypeId);
                if (ccType.CategoryCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ")) ||
                   ccType.CategoryCodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ")))
                {
                    addAsterisk = true;
                }
            }

            // Format answers with numbers in front
            foreach (var a in Answers)
            {                
                if(!a.ResponseDesc.Contains(a.ResponseValue + "."))
                    a.ResponseDesc = a.ResponseValue + ". " + a.ResponseDesc;
            }

            #region Qual Responses
            //For each question create a label and checklist box of answers, if multiple is allowed configure to allow multiple checks
            foreach (FrameworkUAD.Entity.ResponseGroup q in Questions)
            {
                if (Answers.Count(x => x.ResponseGroupID == q.ResponseGroupID) > 0)
                {
                    StackPanel sp = new StackPanel();
                    sp.Name = "sp" + q.ResponseGroupID.ToString();
                    sp.Orientation = Orientation.Vertical;
                    sp.Margin = new Thickness(5, 0, 5, 10);

                    TextBlock tb = new TextBlock();

                    if (q.DisplayName.Length > 25)
                    {
                        tb.Inlines.Add(new Run(q.DisplayName.Substring(0, 25)) { Foreground = Brushes.Black });
                    }
                    else
                    {
                        tb.Inlines.Add(new Run(q.DisplayName) { Foreground = Brushes.Black });
                    }
                    tb.FontSize = 14;
                    tb.FontWeight = FontWeights.Bold;
                    tb.Margin = new Thickness(5, 5, 5, 10);
                    tb.Name = "tbL" + q.ResponseGroupID.ToString();

                    if (myProductSubscription != null && q.IsRequired == true && addAsterisk == true)
                    {
                        tb.Inlines.Add(new Run("*") { Foreground = Brushes.Red });
                    }
                    sp.Children.Add(tb);

                    Telerik.Windows.Controls.RadListBox clb = new Telerik.Windows.Controls.RadListBox();
                    clb.Width = 215;
                    clb.Height = 125;
                    clb.Name = "clb" + q.ResponseGroupID.ToString();
                    List<FrameworkUAD.Entity.CodeSheet> codes = Answers.Where(x => x.ResponseGroupID == q.ResponseGroupID).ToList();
                    //First orders list by DisplayOrder. Then orders by Number in ResponseValue and then Letter in ResponseValue.
                    List<FrameworkUAD.Entity.CodeSheet> displayValid = codes.Where(x => x.DisplayOrder > 0 && x.DisplayOrder != null).OrderBy(x => x.DisplayOrder).ToList();
                    List<FrameworkUAD.Entity.CodeSheet> displayInvalid = codes.Except(displayValid)
                    .OrderBy(s2 =>
                    {
                        int i = 0;
                        return int.TryParse(s2.ResponseValue, out i) ? i : int.MaxValue;
                    })
                    .ThenBy(s2 => s2.ResponseValue)
                    .ToList();
                    codes.Clear();
                    codes.AddRange(displayValid);
                    codes.AddRange(displayInvalid);
                    clb.ItemsSource = codes;
                    //clb.ItemsSource = Answers.Where(x => x.ResponseGroupID == q.ResponseGroupID).OrderBy(y => y.DisplayOrder).ToList();
                    //clb.DisplayMemberPath = "DisplayName";
                    clb.SelectedValuePath = "CodeSheetID";
                    clb.Tag = "AllowMultiples=" + q.IsMultipleValue.ToString() + ",IsRequired=" + q.IsRequired.ToString() + ",Question=" + q.DisplayName;
                    clb.SelectionChanged += clb_SelectionChanged;

                    clb.ItemContainerStyle = this.FindResource("CircQualListBoxItem") as Style;

                    if (q.IsMultipleValue == true)
                        clb.SelectionMode = SelectionMode.Multiple;
                    else
                        clb.SelectionMode = SelectionMode.Single;

                    //clb.Items.SortDescriptions.OrderBy();

                    DataTemplate dtp = new DataTemplate();
                    //set up the stack panel
                    FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(StackPanel));
                    spFactory.Name = "itemStack";
                    spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

                    //set checkbox
                    FrameworkElementFactory itemCheck = new FrameworkElementFactory(typeof(CheckBox));
                    itemCheck.SetValue(CheckBox.MarginProperty, new Thickness(2));
                    itemCheck.SetBinding(CheckBox.IsCheckedProperty, new Binding()
                    {
                        RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Telerik.Windows.Controls.RadListBoxItem), 1),
                        Path = new PropertyPath(Telerik.Windows.Controls.RadListBoxItem.IsSelectedProperty)
                    });

                    spFactory.AppendChild(itemCheck);

                    //set textblock
                    FrameworkElementFactory itemText = new FrameworkElementFactory(typeof(TextBlock));
                    itemText.SetBinding(TextBlock.TextProperty, new Binding("ResponseDesc"));
                    spFactory.AppendChild(itemText);

                    dtp.VisualTree = spFactory;
                    clb.ItemTemplate = dtp;

                    string otherReponse = "";
                    foreach (FrameworkUAD.Entity.CodeSheet r in clb.Items)
                    {
                        if (psdList != null && psdList.Exists(x => x.CodeSheetID == r.CodeSheetID))
                        {
                            if (r.IsOther == true)
                            {
                                FrameworkUAD.Entity.ProductSubscriptionDetail sss = psdList.Where(x => x.CodeSheetID == r.CodeSheetID).OrderByDescending(x => x.DateCreated).FirstOrDefault();
                                otherReponse = sss.ResponseOther;
                            }
                            // Adding to clb here because ResponseMapList where ResponseID == r.ResponseID Other answer was getting blanked out
                            clb.SelectedItems.Add(r);
                        }
                    }
                    sp.Children.Add(clb);

                    TextBlock tbOther = new TextBlock();
                    //Label lbO = new Label();
                    tbOther.Name = "tbOther" + q.ResponseGroupID.ToString();
                    tbOther.Visibility = Visibility.Visible;

                    tbOther.HorizontalAlignment = HorizontalAlignment.Left;
                    tbOther.Text = "Other Response";
                    tbOther.FontSize = 14;
                    tbOther.FontWeight = FontWeights.Bold;
                    tbOther.Margin = new Thickness(0, 2, 0, 2);
                    tbOther.Foreground = Brushes.Black;                    
                    sp.Children.Add(tbOther);

                    TextBox tbO = new TextBox();
                    tbO.Name = "tbO" + q.ResponseGroupID.ToString();
                    tbO.Visibility = Visibility.Visible;
                    tbO.MaxLength = 300;

                    if (string.IsNullOrEmpty(otherReponse))
                    {
                        tbOther.Visibility = Visibility.Hidden;
                        tbO.Visibility = Visibility.Hidden;
                    }

                    tbO.HorizontalAlignment = HorizontalAlignment.Left;
                    tbO.Width = 215;
                    tbO.Margin = new Thickness(0, 2, 0, 2);
                    tbO.Text = otherReponse;
                    tbO.TextChanged += tbO_TextChanged;
                    sp.Children.Add(tbO);

                    spHolder.Children.Add(sp);
                }
            }
            #endregion

            #region Contact Methods
            mlist = new List<FrameworkUAD_Lookup.Entity.Code>();
            marketingResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
            marketingResponse = mWorker.Proxy.Select(accessKey);
            if (marketingResponse != null && marketingResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                mlist = marketingResponse.Result;
            else
                Core_AMS.Utilities.WPF.MessageServiceError();

            mmResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.MarketingMap>>();
            if (myProductSubscription != null && myProductSubscription.IsNewSubscription == false && (MarketingMapList == null || MarketingMapList.Count() == 0))
            {
                mmResponse = mmWorker.Proxy.SelectSubscriber(accessKey, myProductSubscription.PubSubscriptionID, myClient.ClientConnections);
                if (mmResponse != null && mmResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    MarketingMapList = mmResponse.Result.Where(mm => mm.IsActive == true).ToList();
            }

            if (MarketingMapList == null)
                MarketingMapList = new List<FrameworkUAD.Entity.MarketingMap>();

            StackPanel cmSp = new StackPanel();
            cmSp.Name = "spContactMethod";
            cmSp.Orientation = Orientation.Vertical;
            cmSp.Margin = new Thickness(5, 0, 5, 10);

            TextBlock cmTb = new TextBlock();
            //Label cmLbQ = new Label();
            cmTb.Text = "Contact Method";
            cmTb.FontSize = 14;
            cmTb.FontWeight = FontWeights.Bold;
            cmTb.Margin = new Thickness(5, 5, 5, 10);
            cmTb.Foreground = Brushes.Black;

            cmSp.Children.Add(cmTb);

            Telerik.Windows.Controls.RadListBox cmClb = new Telerik.Windows.Controls.RadListBox();
            cmClb.Width = 215;
            cmClb.Height = 125;
            cmClb.Name = "clbContactMethod";
            cmClb.ItemsSource = mlist;
            //cmClb.DisplayMemberPath = "MarketingName";
            cmClb.SelectedValuePath = "MarketingID";
            cmClb.SelectionChanged += cmClb_SelectionChanged;
            cmClb.SelectionMode = SelectionMode.Multiple;

            cmClb.ItemContainerStyle = this.FindResource("CircQualListBoxItem") as Style;
            #region Old Set Style
            //if (isDisabledControls)
            //{
            //    var setter = new Setter()
            //    {
            //        Property = Control.IsEnabledProperty,
            //        Value = false,
            //    };

            //    var style = new Style()
            //    {
            //        Setters = { setter },
            //    };

            //    cmClb.ItemContainerStyle = style;
            //}
            #endregion
            DataTemplate cmDtp = new DataTemplate();
            //set up the stack panel
            FrameworkElementFactory cmSpFactory = new FrameworkElementFactory(typeof(StackPanel));
            cmSpFactory.Name = "cmItemStack";
            cmSpFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            //set checkbox
            FrameworkElementFactory cmItemCheck = new FrameworkElementFactory(typeof(CheckBox));
            cmItemCheck.SetValue(CheckBox.MarginProperty, new Thickness(2));
            cmItemCheck.SetBinding(CheckBox.IsCheckedProperty, new Binding()
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Telerik.Windows.Controls.RadListBoxItem), 1),
                Path = new PropertyPath(Telerik.Windows.Controls.RadListBoxItem.IsSelectedProperty)
            });
            cmSpFactory.AppendChild(cmItemCheck);

            //set textblock
            FrameworkElementFactory cmItemText = new FrameworkElementFactory(typeof(TextBlock));
            cmItemText.SetBinding(TextBlock.TextProperty, new Binding("MarketingName"));
            cmSpFactory.AppendChild(cmItemText);

            cmDtp.VisualTree = cmSpFactory;
            cmClb.ItemTemplate = cmDtp;

            foreach (FrameworkUAD_Lookup.Entity.Code m in cmClb.Items)
            {
                if (myProductSubscription != null)
                {
                    FrameworkUAD.Entity.MarketingMap mm = MarketingMapList.SingleOrDefault(x => x.MarketingID == m.CodeId && x.PublicationID == myProduct.ProductID);
                    if (mm != null)
                    {
                        if (mm.IsActive)
                            cmClb.SelectedItems.Add(m);
                    }
                }
            }

            cmSp.Children.Add(cmClb);
            spHolder.Children.Add(cmSp);
            #endregion

            firstLoad = false;
        }

        public void DisableAllControls()
        {
            spMain.IsEnabled = false;
        }
        public void EnableAllControls()
        {
            #region old disable
            //cbQSource.IsEnabled = true;
            //var setter = new Setter()
            //{
            //    Property = Control.IsEnabledProperty,
            //    Value = true,
            //};

            //var style = new Style()
            //{
            //    Setters = { setter },
            //};
            //List<Telerik.Windows.Controls.RadListBox> list = Core_AMS.Utilities.WPF.FindVisualChildren<Telerik.Windows.Controls.RadListBox>(spHolder).ToList();
            //foreach (Telerik.Windows.Controls.RadListBox c in list)
            //{
            //    c.IsEnabled = true;

            //}
            //cbPar3C.IsEnabled = true;
            //dpQualDate.IsEnabled = true;
            //spHolder.IsEnabled = false;
            #endregion

            spMain.IsEnabled = true;
        }
        private void clb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (firstLoad == false)
            {
                Telerik.Windows.Controls.RadListBox clb = (Telerik.Windows.Controls.RadListBox)sender;
                FrameworkUAD.Entity.CodeSheet current = new FrameworkUAD.Entity.CodeSheet();
                ArrayList aItem = new ArrayList(e.AddedItems);
                ArrayList bItem = new ArrayList(e.RemovedItems);

                string otherResponse = "";
                if (aItem.Count > 0)
                {
                    current = (FrameworkUAD.Entity.CodeSheet)aItem[0];

                    if (current.IsOther == true)
                    {
                        string id = clb.Name.Replace("clb", "");
                        UIElement element = Core_AMS.Utilities.WPF.FindChild<StackPanel>(spHolder, "sp" + id) as UIElement;
                        TextBox tbO = Core_AMS.Utilities.WPF.FindChild<TextBox>(element, "tbO" + id);
                        TextBlock lbO = Core_AMS.Utilities.WPF.FindChild<TextBlock>(element, "tbOther" + id);

                        lbO.Visibility = Visibility.Visible;
                        tbO.Visibility = Visibility.Visible;
                        otherResponse = Microsoft.VisualBasic.Interaction.InputBox("Please enter the other response.", "Other Response", "");
                        tbO.Text = otherResponse;
                    }

                    bool existsInList = false;
                    if(psdList != null)
                        existsInList = psdList.Exists(x => x.CodeSheetID == current.CodeSheetID);

                    if (existsInList == true)
                    {
                        foreach (var r in psdList)
                        {
                            if (r.CodeSheetID == current.CodeSheetID)
                            {
                                r.DateUpdated = DateTime.Now;
                                r.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            }
                        }
                    }
                    else
                    {
                        psdList.Add(new FrameworkUAD.Entity.ProductSubscriptionDetail
                        {
                            PubSubscriptionID = myProductSubscription.PubSubscriptionID,
                            SubscriptionID = myProductSubscription.SubscriptionID,
                            CodeSheetID = current.CodeSheetID,
                            DateCreated = DateTime.Now,
                            DateUpdated = null,
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            UpdatedByUserID = null,
                            ResponseOther = otherResponse,
                            
                        });
                    }

                    otherResponse = string.Empty;
                }

                if (bItem.Count > 0)
                {
                    current = (FrameworkUAD.Entity.CodeSheet)bItem[0];
                    //foreach (var y in psdList.Where(s => s.PubSubscriptionID == myProductSubscription.PubSubscriptionID && s.CodeSheetID == current.CodeSheetID))
                    //{
                    //    y.DateUpdated = DateTime.Now;
                    //    y.UpdatedByUserID = Home.myAppData.AuthorizedUser.User.UserID;
                    //}
                    psdList.RemoveAll(x => x.CodeSheetID == current.CodeSheetID);

                    if (current.IsOther == true)
                    {
                        string id = clb.Name.Replace("clb", "");
                        UIElement element = Core_AMS.Utilities.WPF.FindChild<StackPanel>(spHolder, "sp" + id) as UIElement;
                        TextBox tbO = Core_AMS.Utilities.WPF.FindChild<TextBox>(element, "tbO" + id);
                        TextBlock lbO = Core_AMS.Utilities.WPF.FindChild<TextBlock>(element, "tbOther" + id);

                        lbO.Visibility = Visibility.Hidden;
                        tbO.Visibility = Visibility.Hidden;
                    }
                }

                MadeResponseChange = true;
                RequalOnlyChange = true;
                isTriggerQualDateChange = true;
            }

        }
        public void AddRemoveAsterisk(bool addAsterisk)
        {
            foreach (FrameworkUAD.Entity.ResponseGroup q in Questions)
            {
                UIElement element = Core_AMS.Utilities.WPF.FindChild<StackPanel>(spHolder, "sp" + q.ResponseGroupID) as UIElement;
                TextBlock tbLo = Core_AMS.Utilities.WPF.FindChild<TextBlock>(element, "tbL" + q.ResponseGroupID.ToString());

                if (tbLo != null)
                {
                    if (myProductSubscription != null && addAsterisk == true && q.IsRequired == true)
                    {
                        tbLo.Inlines.Clear();
                        tbLo.Inlines.Add(new Run(q.DisplayName) { Foreground = Brushes.Black });
                        tbLo.Inlines.Add(new Run("*") { Foreground = Brushes.Red });
                    }
                    else if (myProductSubscription != null && addAsterisk == false && q.IsRequired == true)
                    {
                        tbLo.Inlines.Clear();
                        tbLo.Inlines.Add(new Run(q.DisplayName) { Foreground = Brushes.Black });
                    }
                }
            }
        }
        private void cmClb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (firstLoad == false)
            {
                //Telerik.Windows.Controls.RadListBox cmClb = (Telerik.Windows.Controls.RadListBox)sender;
                FrameworkUAD_Lookup.Entity.Code current = new FrameworkUAD_Lookup.Entity.Code();
                ArrayList aItem = new ArrayList(e.AddedItems);
                ArrayList bItem = new ArrayList(e.RemovedItems);

                if (aItem.Count > 0)
                {
                    current = (FrameworkUAD_Lookup.Entity.Code)aItem[0];

                    bool inList = false;
                    if(MarketingMapList != null)
                        inList = MarketingMapList.Exists(x => x.MarketingID == current.CodeId);
                    if (inList == false)
                    {
                        MarketingMapList.Add(new FrameworkUAD.Entity.MarketingMap
                        {
                            MarketingID = current.CodeId,
                            PubSubscriptionID = myProductSubscription.PubSubscriptionID,
                            PublicationID = myProduct.ProductID,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            DateUpdated = null,
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            UpdatedByUserID = null
                        });
                    }
                    else
                    {
                        foreach (var m in MarketingMapList)
                        {
                            if (m.MarketingID == current.CodeId)
                            {
                                m.IsActive = true;
                                m.DateUpdated = DateTime.Now;
                                m.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            }
                        }
                    }
                }
                else
                {
                    current = (FrameworkUAD_Lookup.Entity.Code)bItem[0];
                    //var itemToRemove = MarketingMapList.Single(f => f.MarketingID == current.MarketingID);
                    //MarketingMapList.Remove(itemToRemove);
                    foreach (var y in MarketingMapList.Where(m => m.MarketingID == current.CodeId))
                    {
                        y.IsActive = false;
                        y.DateUpdated = DateTime.Now;
                        y.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    }
                }
                //dpQualDate.SelectedDate = DateTime.Now;
                MadeResponseChange = true;
                RequalOnlyChange = true;
            }
        }
        private void tbO_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TextCompositionEventArgs
            TextBox tbO = (TextBox)sender;
            
            int id = Convert.ToInt32(tbO.Name.Replace("tbO",""));
            int respId = -1;
            string otherResponse = tbO.Text;

            foreach (var a in Answers)
            {
                if (a.ResponseGroupID == id)
                {
                    //int displayLength = a.ResponseValue.Length;
                    
                    //if (displayLength > 20)
                    //{
                        if (a.IsOther == true)
                        {
                            respId = a.CodeSheetID;    
                        }
                    //}
                    //else
                    //{
                    //    if (a.DisplayName.ToLower().Contains("other"))
                    //    {
                    //        respId = a.ResponseID;    
                    //    }
                    //}
                }
            }

            foreach (var r in psdList)
            {
                if (respId == r.CodeSheetID)
                {
                    r.ResponseOther = otherResponse;
                }
            }

            //dpQualDate.SelectedDate = DateTime.Now;
            MadeResponseChange = true;
            RequalOnlyChange = true;   

        }
        public string RequiredAnswered()
        {
            bool allAnswered = true;
            StringBuilder sb = new StringBuilder();
            QuestionNotAnswered = new List<string>();

            foreach (var q in Questions)
            {
                if (q.IsRequired == true)
                {
                    var x = (from rml in psdList
                             join r in Answers on rml.CodeSheetID equals r.CodeSheetID
                             join rt in Questions on r.ResponseGroupID equals rt.ResponseGroupID
                             where rt.IsRequired == true && rt.ResponseGroupID == q.ResponseGroupID
                             select rml.SubscriptionID).ToList();
                    if (x.Count == 0)
                    {
                        //allAnswered = false;
                        if (!QuestionNotAnswered.Contains(q.DisplayName))
                        {
                            QuestionNotAnswered.Add(q.DisplayName);
                        }
                    }
                }
            }

            if (QuestionNotAnswered.Count != 0)
            {
                allAnswered = false;
                foreach (string s in QuestionNotAnswered)
                {
                    if (sb.Length == 0)
                        sb.Append(s);
                    else
                        sb.Append("," + s);
                }
            }
            string ua = sb.ToString();
            return ua;
        }
        private void cbQSourceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (firstLoad == false)
            {
                int qsourceID = 0;
                if (cbQSource.SelectedValue != null)
                {
                    int.TryParse(cbQSource.SelectedValue.ToString(), out qsourceID);
                    QSourceID = qsourceID;
                    if (cbQSource.SelectedValue.ToString() != myProductSubscription.PubQSourceID.ToString())
                    {
                        MadeResponseChange = true;
                        RequalOnlyChange = true;
                        isTriggerQualDateChange = true;
                    }
                }
                else
                {
                    QSourceID = qsourceID;
                    MadeResponseChange = true;
                    RequalOnlyChange = true;
                    isTriggerQualDateChange = true;
                }
            }
        }
        private void cbPar3C_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (firstLoad == false)
            {
                int par3c = 0;
                if (cbPar3C.SelectedValue != null)
                {
                    int.TryParse(cbPar3C.SelectedValue.ToString(), out par3c);
                    Par3C = par3c;
                    if (!cbPar3C.SelectedValue.ToString().Equals(myProductSubscription.Par3CID.ToString()))
                    {
                        MadeResponseChange = true;
                        RequalOnlyChange = true;
                        isTriggerQualDateChange = true;
                    }
                }
                else
                {
                    Par3C = par3c;
                    MadeResponseChange = true;
                    RequalOnlyChange = true;
                    isTriggerQualDateChange = true;
                }
            }
        }
        public void spQualDate_LostFocus(object sender, RoutedEventArgs e)
        {
            QualDateChanged = true;
        }

        //public void EnableCheckBoxes(bool disableControls)
        //{
        //    bool enable = true;

        //    if (disableControls == true)
        //        enable = false;

        //    IEnumerable<StackPanel> collection = spHolder.Children.OfType<StackPanel>();

        //    foreach (var x in collection)
        //    {
        //        IEnumerable<RadListBox> radListBox = x.Children.OfType<RadListBox>();
        //        foreach (var y in radListBox)
        //        {
        //            //var setter = new Setter()
        //            //{
        //            //    Property = Control.IsEnabledProperty,
        //            //    Value = enable,
        //            //};

        //            //var style = new Style()
        //            //{
        //            //    Setters = { setter },
        //            //};

        //            y.IsEnabled = false;
        //        }
        //    }
        //}

        #region Find Module
        public void UpdateCopiesControl(object sender)
        {
            DependencyObject item = null;
            if (sender.GetType() == typeof(RadNumericUpDown))
                item = (RadNumericUpDown)sender;

            if (item != null)
            {
                Window w = Core_AMS.Utilities.WPF.GetParentWindow(item);
                foreach (StackPanel sp in Core_AMS.Utilities.WPF.FindVisualChildren<StackPanel>(w))
                {
                    bool found = false;
                    UIElement el = Core_AMS.Utilities.WPF.FindChild<SubscriptionContainer>(sp, "mainControl") as UIElement;
                    if (el != null)
                    {
                        found = true;
                        foreach (TabItem ti in Core_AMS.Utilities.WPF.FindVisualChildren<TabItem>(el))
                        {
                            #region Qualification Copies
                            TabItem ele = Core_AMS.Utilities.WPF.FindChild<TabItem>(ti, "tabPaid") as TabItem;
                            if (ele != null)
                            {
                                foreach (DependencyObject g in Common.GetChildObjects(ele))
                                {
                                    UIElement elem = Core_AMS.Utilities.WPF.FindChild<ScrollViewer>(g, "svPaid") as UIElement;
                                    if (elem != null)
                                    {
                                        foreach (DependencyObject r in Common.GetChildObjects(elem))
                                        {
                                            //Paid eleme = Core_AMS.Utilities.WPF.FindChild<Paid>(r, "modPaid");
                                            //if (eleme != null && eleme.iudCopies.Value != iudCopies.Value)
                                            //    eleme.iudCopies.Value = iudCopies.Value;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    if (found)
                        break;
                }
            }
        }
        #endregion

        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            RadButton me = sender as RadButton;
            RadDropDownButton dp = me.ParentOfType<RadDropDownButton>();
            dp.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void iudCopies_ValueChanged_1(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (firstLoad == false)
            {
                Copies = Convert.ToInt32(iudCopies.Value.Value);
                UpdateCopiesControl(sender);
                MadeResponseChange = true;
                RequalOnlyChange = true;
            }
        }
    }
}
