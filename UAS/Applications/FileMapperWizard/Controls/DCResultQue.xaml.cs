using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for DCResultQue.xaml
    /// </summary>
    public partial class DCResultQue : UserControl
    {
        FileMapperWizard.Modules.DataCompareSteps thisDCSteps { get; set; }
        public DCResultQue(FileMapperWizard.Modules.DataCompareSteps dcSteps)
        {
            thisDCSteps = dcSteps;
            InitializeComponent();
            rbFileUpdate.Visibility = System.Windows.Visibility.Collapsed;
            LoadPreviousData();
        }
        private void LoadPreviousData()
        {
            //if (thisDCSteps.dataCompareResultQue != null)
            //{
            //    tbFileName.Text = thisDCSteps.fileName;
            //    string[] emails = thisDCSteps.emailAddresses.Split(',');
            //    foreach (string s in emails)
            //        rlbEmails.Items.Add(s);
            //}
        }
        private bool SetValues()
        {
            string filename = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbFileName.Text.Trim().Replace(" ", "_").Replace("-", "_"));
            if (string.IsNullOrEmpty(filename))
            {
                Core_AMS.Utilities.WPF.Message("Please enter a File Name.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Name required");
                return false;
            }
            else if (rlbEmails.Items.Count == 0)
            {
                Core_AMS.Utilities.WPF.Message("Please enter at least one Email Address for delivery.", MessageBoxButton.OK, MessageBoxImage.Warning, "Email Address required");
                return false;
            }
            //else if (thisDCSteps.dataCompareResultQue == null || rbFileUpdate.IsChecked == false)
            //{
            //    FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareResultQue> rqWorker = FrameworkServices.ServiceClient.UAS_DataCompareResultQueClient();
            //    FrameworkUAS.Service.Response<bool> resp = rqWorker.Proxy.FileNameExist(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisDCSteps.fmContainer.sourceFileID, filename);
            //    if (resp.Result != null && resp.Result == false)
            //    {
            //        thisDCSteps.fileName = filename;
            //        StringBuilder sb = new StringBuilder();
            //        foreach (var e in rlbEmails.Items)
            //            sb.Append(e.ToString() + ",");
            //        string emails = sb.ToString().TrimEnd(',');
            //        thisDCSteps.emailAddresses = emails;

            //        return true;
            //    }
            //    else
            //    {
            //        Core_AMS.Utilities.WPF.Message("File Name already exists.  Please enter a unique File Name.", MessageBoxButton.OK, MessageBoxImage.Warning, "File Name exists");
            //        if (thisDCSteps.dataCompareResultQue != null)
            //            rbFileUpdate.Visibility = System.Windows.Visibility.Visible;
            //        return false;
            //    }
            //}
            else
            {
                thisDCSteps.fileName = filename;
                StringBuilder sb = new StringBuilder();
                foreach (var e in rlbEmails.Items)
                    sb.Append(e.ToString() + ",");
                string emails = sb.ToString().TrimEnd(',');
                thisDCSteps.emailAddresses = emails;

                return true;
            }
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            //set values
            if (SetValues())
            {
                thisDCSteps.Step2ToStep1();

                var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisDCSteps);
                if (borderList.FirstOrDefault(x => x.Name.Equals("StepOneContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepOneContainer", StringComparison.CurrentCultureIgnoreCase));
                    thisBorder.Child = new FileMapperWizard.Controls.DCTarget(thisDCSteps);
                }
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            //set values
            if (SetValues())
            {
                //if (thisDCSteps.dataCompareResultQue == null)
                //{
                //    //need to do insert and get DataCompareResultQueId
                //    FrameworkUAS.Entity.DataCompareQue dcrq = new FrameworkUAS.Entity.DataCompareQue();
                //    dcrq.ClientId = thisDCSteps.fmContainer.myClient.ClientID;
                //    dcrq.UserId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                //    dcrq.SourceFileId = thisDCSteps.fmContainer.sourceFileID;
                //    dcrq.DataCompareTargetCodeId = thisDCSteps.targetCodeId;
                //    dcrq.ProductId = thisDCSteps.productId;
                //    dcrq.ProductName = thisDCSteps.productName;
                //    dcrq.BrandId = thisDCSteps.brandId;
                //    dcrq.BrandName = thisDCSteps.brandName;
                //    dcrq.MarketId = thisDCSteps.marketId;
                //    dcrq.MarketName = thisDCSteps.marketName;
                //    dcrq.IsConsensus = thisDCSteps.isConsensus;
                //    dcrq.FileName = thisDCSteps.fileName;
                //    dcrq.EmailAddress = thisDCSteps.emailAddresses;
                //    dcrq.DateQued = null;
                //    dcrq.IsResultComplete = false;
                //    dcrq.ResultCompleteDate = null;
                //    dcrq.DateCreated = DateTime.Now;

                //    FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareResultQue> rqWorker = FrameworkServices.ServiceClient.UAS_DataCompareResultQueClient();
                //    FrameworkUAS.Service.Response<int> resp = rqWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, dcrq);

                //    if (resp.Result != null && resp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                //        dcrq.DataCompareQueId = resp.Result;

                //    thisDCSteps.dataCompareResultQue = dcrq;
                //}
                //else
                //{
                //    FrameworkUAS.Entity.DataCompareQue dcrq = thisDCSteps.dataCompareResultQue;
                //    dcrq.ClientId = thisDCSteps.fmContainer.myClient.ClientID;
                //    dcrq.UserId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                //    dcrq.SourceFileId = thisDCSteps.fmContainer.sourceFileID;
                //    dcrq.DataCompareTargetCodeId = thisDCSteps.targetCodeId;
                //    dcrq.ProductId = thisDCSteps.productId;
                //    dcrq.ProductName = thisDCSteps.productName;
                //    dcrq.BrandId = thisDCSteps.brandId;
                //    dcrq.BrandName = thisDCSteps.brandName;
                //    dcrq.MarketId = thisDCSteps.marketId;
                //    dcrq.MarketName = thisDCSteps.marketName;
                //    dcrq.IsConsensus = thisDCSteps.isConsensus;
                //    dcrq.FileName = thisDCSteps.fileName;
                //    dcrq.EmailAddress = thisDCSteps.emailAddresses;
                //    dcrq.DateQued = null;
                //    dcrq.IsResultComplete = false;
                //    dcrq.ResultCompleteDate = null;
                //    dcrq.DateCreated = DateTime.Now;

                //    thisDCSteps.dataCompareResultQue = dcrq;

                //    FrameworkServices.ServiceClient<UAS_WS.Interface.IDataCompareResultQue> rqWorker = FrameworkServices.ServiceClient.UAS_DataCompareResultQueClient();
                //    rqWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, dcrq);
                //}

                thisDCSteps.Step2ToStep3();

                var borderList = Core_AMS.Utilities.WPF.FindVisualChildren<System.Windows.Controls.Border>(thisDCSteps);
                if (borderList.FirstOrDefault(x => x.Name.Equals("StepThreeContainer", StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Border thisBorder = borderList.FirstOrDefault(x => x.Name.Equals("StepThreeContainer", StringComparison.CurrentCultureIgnoreCase));
                    thisBorder.Child = new FileMapperWizard.Controls.DCProfileAttributes(thisDCSteps);
                }
            }
        }

        private void btnAddEmail_Click(object sender, RoutedEventArgs e)
        {
            string email = tbEmail.Text.Trim();
            if (Core_AMS.Utilities.StringFunctions.isEmail(email))
            {
                if (rlbEmails.Items.Count < 5)
                    rlbEmails.Items.Add(email);
                else
                    Core_AMS.Utilities.WPF.Message("You are only allowed 5 email addresses.", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
                Core_AMS.Utilities.WPF.Message("Invalid email address.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }

        private void btnDeleteEmail_Click(object sender, RoutedEventArgs e)
        {
            List<string> deletes = new List<string>();
            foreach (string email in rlbEmails.SelectedItems)
                deletes.Add(email);

            foreach (string email in deletes)
                rlbEmails.Items.Remove(email); 
        }
    }
}
