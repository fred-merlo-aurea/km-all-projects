using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for DynamicDemographics.xaml
    /// </summary>
    public partial class DynamicDemographics : UserControl, INotifyPropertyChanged
    {
        //DEPRECATED -- FilterControls now controls this.
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> responseGroupData = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        //private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponse> responseData = FrameworkServices.ServiceClient.UAD_ResponseClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> codeSheetData = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientData = FrameworkServices.ServiceClient.UAS_ClientClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productData = FrameworkServices.ServiceClient.UAD_ProductClient();
        #endregion
        #region Variables/Lists
        private List<FrameworkUAD.Entity.ResponseGroup> responseGroupList = new List<FrameworkUAD.Entity.ResponseGroup>();
        private List<FrameworkUAD.Entity.CodeSheet> codeSheetList = new List<FrameworkUAD.Entity.CodeSheet>();
        private List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        public int openItems = 0; //This dynamically sets the Expand/Collapse text block if you manually expand all items.
        private int myProductID = 0;
        private Guid accessKey;
        KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        #endregion
        #region Properties
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                foreach (RadPanelBarItem p in rpbReportFilters.Items)
                {
                    if(p.IsExpanded != _isExpanded)
                        p.IsExpanded = _isExpanded;
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsExpanded"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region ServiceResponses
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> clientResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> csResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> rGroupResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        #endregion

        public DynamicDemographics(int pubID)
        {
            InitializeComponent();

            int clientID = -1;
            myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            clientID = myClient.ClientID;
            myProductID = pubID;

            if (clientID != 0)
                accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            else
                MessageBox.Show("Please select a Client.");
        }

        #region UI Events
        private void SelectAnyResponse(object sender, RoutedEventArgs e)
        {
            RadPanelBarItem item = sender as RadPanelBarItem;
            RadPanelBarItem parent = item.ParentItem;
            
            foreach(RadPanelBarItem i in parent.Items)
            {
                if(!i.Header.ToString().Contains("ZZ") && !i.Header.ToString().Contains("YY"))
                {
                    i.IsEnabled = false;
                    i.IsSelected = false;
                }
            }
        }

        private void UnSelectAnyResponse(object sender, RoutedEventArgs e)
        {
            RadPanelBarItem item = sender as RadPanelBarItem;
            RadPanelBarItem parent = item.ParentItem;

            int checkItem = parent.Items.Cast<RadPanelBarItem>().Where(x => x.Header.ToString().Contains("ZZ") || x.Header.ToString().Contains("YY") && x.IsSelected == true).Count();

            if (checkItem <= 1)
            {
                foreach (RadPanelBarItem i in parent.Items)
                {
                    i.IsEnabled = true;
                }
            }
        }
        #endregion

        public Helpers.FilterOperations.FilterDetailContainer GetSelection()
        {
            Helpers.FilterOperations.FilterDetailContainer myDetailContainer = new Helpers.FilterOperations.FilterDetailContainer();
            myDetailContainer.FilterDetail.FilterObjectType = "Responses";
            myDetailContainer.FilterDetail.FilterField = rpbReportFilters.Name.ToString();
            myDetailContainer.FilterDetail.FilterTypeId = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Dynamic.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();
            myDetailContainer.FilterDetail.SearchCondition = "EQUAL";
            foreach (RadPanelBarItem parent in rpbReportFilters.Items)
            {
                foreach (RadPanelBarItem child in parent.Items)
                {
                    if (child.IsSelected) //We add the tag onto the value which contains the parent Question. We need that to correctly filter Any/No Responses.
                    {
                        string value = "";
                        value = child.Tag.ToString();

                        myDetailContainer.Values.Add(new FrameworkUAS.Entity.FilterDetailSelectedValue() { SelectedValue = value });
                    }
                }
            }
            return myDetailContainer;
        }

        public void LoadData(List<FrameworkUAD_Lookup.Entity.CodeType> codeTypes, List<FrameworkUAD_Lookup.Entity.Code> codes)
        {
            if (myClient != null)
            {
                int id = 0;

                if(codeTypes.Count > 0 && codes.Count > 0)
                {
                    int groupTypeID = codeTypes.Where(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Response_Group.ToString().Replace("_", " "))).FirstOrDefault().CodeTypeId;
                    id = codes.Where(x => x.CodeTypeId == groupTypeID && x.CodeName == FrameworkUAD_Lookup.Enums.ResponseGroupTypes.Circ_and_UAD.ToString().Replace("_", " ")).Select(x=> x.CodeId).FirstOrDefault();

                    int filterTypeID = codeTypes.Where(x => x.CodeTypeName.Equals(FrameworkUAD_Lookup.Enums.CodeType.Filter_Type.ToString().Replace("_", " "))).FirstOrDefault().CodeTypeId;
                    filterTypes = codes.Where(x => x.CodeTypeId == filterTypeID).ToList();
                }

                rGroupResponse = responseGroupData.Proxy.Select(accessKey, myClient.ClientConnections, myProductID);
                if (Helpers.Common.CheckResponse(rGroupResponse.Result, rGroupResponse.Status))
                    responseGroupList = rGroupResponse.Result.Where(x => x.ResponseGroupTypeId == id).OrderBy(x => x.DisplayOrder).ToList();

                csResponse = codeSheetData.Proxy.Select(accessKey, myClient.ClientConnections);
                if (Helpers.Common.CheckResponse(csResponse.Result, csResponse.Status))
                    codeSheetList = csResponse.Result.Where(x=> x.IsActive == true).ToList();
            }
        }

        public void SetControls()
        {
            foreach (FrameworkUAD.Entity.ResponseGroup rt in responseGroupList)
            {
                RadPanelBarItem p = new RadPanelBarItem();
                p.Header = rt.DisplayName;
                p.Tag = rt.ResponseGroupID;
                p.Expanded += (sender, e) =>
                {
                    openItems++;
                    if (openItems == rpbReportFilters.Items.Count)
                        this.IsExpanded = true;
                    else if(openItems == 0)
                        this.IsExpanded = false;
                };
                p.Collapsed += (sender, e) =>
                {
                    openItems--;
                    if (openItems == rpbReportFilters.Items.Count)
                        this.IsExpanded = true;
                    else if(openItems == 0)
                        this.IsExpanded = false;
                };
                p.Background = new SolidColorBrush(Colors.White);
                p.MaxHeight = 100;
                rpbReportFilters.Items.Add(p);
            }
            foreach (RadPanelBarItem parent in rpbReportFilters.Items)
            {
                
                List<FrameworkUAD.Entity.CodeSheet> codes = codeSheetList.Where(x => x.ResponseGroupID == (int)parent.Tag).OrderBy(x => x.ResponseDesc).ToList();
                List<FrameworkUAD.Entity.CodeSheet> displayValid = codes.Where(x => x.DisplayOrder > 0 && x.DisplayOrder != null).OrderBy(x=> x.DisplayOrder).ToList();
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
                foreach (FrameworkUAD.Entity.CodeSheet r in codes)
                {
                    RadPanelBarItem child = new RadPanelBarItem();
                    child.Header = r.ResponseValue + ". " + r.ResponseDesc;
                    child.Name = "_" + r.CodeSheetID.ToString();
                    child.Tag = parent.Tag + "_" + r.CodeSheetID.ToString();
                    parent.Items.Add(child);
                }
                RadPanelBarItem temp = new RadPanelBarItem();
                temp.Selected += SelectAnyResponse;
                temp.Unselected += UnSelectAnyResponse;
                temp.Header = "YY. Any Response";
                temp.Name = "YY_" + parent.Tag;
                temp.Tag = parent.Tag + "_YY";
                parent.Items.Add(temp);

                RadPanelBarItem temp2 = new RadPanelBarItem();
                temp2.Selected += SelectAnyResponse;
                temp2.Unselected += UnSelectAnyResponse;
                temp2.Header = "ZZ. No Response";
                temp2.Name = "ZZ_" + parent.Tag;
                temp2.Tag = parent.Tag + "_ZZ";
                parent.Items.Add(temp2);
            }
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            if (_isExpanded)
                this.IsExpanded = false;
            else
                this.IsExpanded = true;
        }
    }
}
