using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ControlCenter.Controls
{
    /// <summary>
    /// Interaction logic for ResponseGroupCopy.xaml
    /// </summary>
    public partial class ResponseGroupCopy : UserControl
    {
        #region SERVICE CALLS
        FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> rgWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        #endregion
        #region VARIABLES
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> svProducts = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> svResponseGroups = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();

        FrameworkUAS.Service.Response<bool> svBoolRG = new FrameworkUAS.Service.Response<bool>();

        List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
        List<FrameworkUAD.Entity.ResponseGroup> responseGroups = new List<FrameworkUAD.Entity.ResponseGroup>();
        #endregion

        public ResponseGroupCopy()
        {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            InitializeComponent();            
            LoadData();  
        }

        #region Load Methods
        public void LoadData()
        {
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                svProducts = pWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                svResponseGroups = rgWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (svProducts.Result != null && svProducts.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)                
                    products = svProducts.Result;                
                else                
                    Core_AMS.Utilities.WPF.MessageServiceError();

                if (svResponseGroups.Result != null && svResponseGroups.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    responseGroups = svResponseGroups.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();

                LoadProducts(); 
                LoadResponseGroupCombo();

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        public void LoadProducts()
        {
            products = products.OrderBy(x => x.PubName).ToList();
            cbProduct.ItemsSource = null;
            cbProduct.ItemsSource = products;
            cbProduct.SelectedValuePath = "PubID";
            cbProduct.DisplayMemberPath = "PubName";

            lbxAvailable.Items.Clear();
            foreach (FrameworkUAD.Entity.Product p in products)
            {
                lbxAvailable.Items.Add(p.PubName.ToString());
            }            
        }

        public void LoadResponseGroupCombo()
        {
            if (cbProduct.SelectedValue != null)
            {
                int pubID = 0;
                int.TryParse(cbProduct.SelectedValue.ToString(), out pubID);

                List<FrameworkUAD.Entity.ResponseGroup> distinctResponseGroups = responseGroups.Where(x => x.PubID == pubID).ToList();
                cbResponseGroup.ItemsSource = distinctResponseGroups.OrderBy(x => x.ResponseGroupName);
                cbResponseGroup.SelectedValuePath = "ResponseGroupID";
                cbResponseGroup.DisplayMemberPath = "ResponseGroupName";

                lbxAvailable.Items.Clear();
                foreach (FrameworkUAD.Entity.Product p in products)
                {
                    if (p.PubID != pubID)
                        lbxAvailable.Items.Add(p.PubName.ToString());
                }   
            }
        }
        #endregion

        private void cbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadResponseGroupCombo();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            int pubID = 0;
            int rgID = 0;

            //Check From Prod|RG
            if (cbProduct.SelectedValue != null && cbResponseGroup != null)
            {
                int.TryParse(cbProduct.SelectedValue.ToString(), out pubID);
                int.TryParse(cbResponseGroup.SelectedValue.ToString(), out rgID);
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select a from product and response group before continuing.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }

            //Check selected to
            if (lbxAvailable.SelectedItems.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to copy this Response Group?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    //Copy
                    string selectedvalues = string.Empty;
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlElement xmlNode = xmlDoc.CreateElement("XML");
                    xmlDoc.AppendChild(xmlNode);

                    foreach (string p in lbxAvailable.SelectedItems)
                    {
                        FrameworkUAD.Entity.Product product = products.FirstOrDefault(x => x.PubName.Equals(p, StringComparison.CurrentCultureIgnoreCase));
                        if (product != null)
                        {
                            if (pubID == product.PubID)
                            {
                                Core_AMS.Utilities.WPF.MessageError("Cannot select the same Product in From and To. Please fix and continue.");
                                return;
                            }
                            XmlElement xmlPub;
                            xmlPub = xmlDoc.CreateElement("Pub");

                            XmlAttribute xmlID;
                            xmlID = xmlDoc.CreateAttribute("ID");
                            xmlID.InnerText = product.PubID.ToString();
                            xmlPub.Attributes.Append(xmlID);

                            xmlNode.AppendChild(xmlPub);

                            selectedvalues += selectedvalues == string.Empty ? product.PubName : "," + product.PubName;
                        }
                    }
                    svBoolRG = rgWorker.Proxy.Copy(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, rgID, xmlDoc.OuterXml, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if ((svBoolRG.Result == true || svBoolRG.Result == false) && svBoolRG.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        LoadData();
                        Core_AMS.Utilities.WPF.MessageSaveComplete();
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.MessageError("An unexpected error occured during a service request, please reload the page. If the problem persists please contact Customer Support.");
                        return;
                    }
                }
            }         
            else
            {
                Core_AMS.Utilities.WPF.Message("Please select a to product before continuing.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
        }
    }
}
