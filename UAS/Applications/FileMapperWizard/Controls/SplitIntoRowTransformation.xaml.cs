using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FileMapperWizard.Helpers;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for SplitIntoRowTransformation.xaml
    /// </summary>
    public partial class SplitIntoRowTransformation : UserControl
    {

        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformation> blt = FrameworkServices.ServiceClient.UAS_TransformationClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IDBWorker> dbWorker = FrameworkServices.ServiceClient.UAS_DBWorkerClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationPubMap> bltpm = FrameworkServices.ServiceClient.UAS_TransformationPubMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMapping> fmData = FrameworkServices.ServiceClient.UAS_FieldMappingClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationFieldMap> tfmData = FrameworkServices.ServiceClient.UAS_TransformationFieldMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformSplit> tsData = FrameworkServices.ServiceClient.UAS_TransformSplitClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();

        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        public List<int> oldSavedPubCodes = new List<int>();
        public Dictionary<int, string> pubCode = new Dictionary<int, string>();
        public Dictionary<string, string> delimiters = new Dictionary<string, string>();
        public KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        public int myTransformationID = 0;
        public int mySplitTransformationID = 0;
        public int sourceFileID = 0;
        public string fieldMappingID = "";
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> svCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        public FrameworkUAD_Lookup.Entity.Code transformationType = new FrameworkUAD_Lookup.Entity.Code();
        StackPanel myParent;
        FileMapperWizard.Modules.FMUniversal thisContainer;

        public SplitIntoRowTransformation(FrameworkUAS.Object.AppData appData, FileMapperWizard.Modules.FMUniversal container, int sfID, KMPlatform.Entity.Client client, StackPanel parent, string transID, string mapID, bool existing)
        {
            InitializeComponent();
            thisContainer = container;
            myParent = parent;
            myClient = client;
            sourceFileID = sfID;
            myAppData = appData;
            fieldMappingID = mapID;
            svCodes = codeWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            List<FrameworkUAD_Lookup.Entity.Code> codes = svCodes.Result;
            transformationType = codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace('_', ' '));

            pubCode = dbWorker.Proxy.GetPubIDAndCodesByClient(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient).Result;
            rcbAssignPubcodes.ItemsSource = pubCode.OrderBy(x => x.Value);
            rcbAssignPubcodes.SelectedMemberPath = "Key";
            rcbAssignPubcodes.DisplayMemberPath = "Value";
            delimiters.Add(" ", "SPACE");
            delimiters.Add(",", "COMMA");
            delimiters.Add("|", "PIPE");
            delimiters.Add(".", "PERIOD");
            delimiters.Add(":", "COLON");
            delimiters.Add(";", "SEMICOLON");
            delimiters.Add("~", "TILD");
            delimiters.Add("-", "DASH");
            delimiters.Add("_", "UNDERSCORE");
            rcbSplitDelimiter.ItemsSource = delimiters;
            rcbSplitDelimiter.SelectedValuePath = "Key";
            rcbSplitDelimiter.DisplayMemberPath = "Value";

            if(existing == true)
            {
                int tID = 0;
                int.TryParse(transID, out tID);
                myTransformationID = tID;
                List<FrameworkUAS.Entity.Transformation> allTrans = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                FrameworkUAS.Entity.Transformation thisTrans = allTrans.SingleOrDefault(x => x.TransformationID == tID);
                List<FrameworkUAS.Entity.TransformSplit> ts = tsData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisTrans.TransformationID).Result;
                FrameworkUAS.Entity.TransformSplit tsSolo = ts.SingleOrDefault(x => x.TransformationID == thisTrans.TransformationID);
                mySplitTransformationID = tsSolo.TransformSplitID;
                tbxSName.Text = thisTrans.TransformationName;
                tbxSDesc.Text = thisTrans.TransformationDescription;

                rlbSplitPubcodes.Items.Clear();
                List<FrameworkUAS.Entity.TransformationPubMap> tpm = bltpm.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisTrans.TransformationID).Result;
                if (tpm.Count > 0)
                {
                    foreach (FrameworkUAS.Entity.TransformationPubMap t in tpm)
                    {
                        if (t.PubID > 0)
                        {
                            oldSavedPubCodes.Add(t.PubID);
                            Dictionary<int, string> pub = new Dictionary<int, string>();
                            if (pubCode.SingleOrDefault(x => x.Key == t.PubID).Key != null)
                            {
                                pub.Add(pubCode.SingleOrDefault(x => x.Key == t.PubID).Key, pubCode.SingleOrDefault(x => x.Key == t.PubID).Value);
                                foreach (KeyValuePair<int, string> c in pub)
                                {
                                    rcbAssignPubcodes.SelectedItems.Add(c);
                                }
                            }
                        }
                    }
                    foreach (KeyValuePair<int, string> i in rcbAssignPubcodes.SelectedItems)
                    {
                        if (i.Value != null)
                            rlbSplitPubcodes.Items.Add(i.Value.ToString());

                    }
                }
                if (delimiters.SingleOrDefault(x => x.Value.Equals(tsSolo.Delimiter, StringComparison.CurrentCultureIgnoreCase)).Key != null)
                    rcbSplitDelimiter.SelectedValue = delimiters.SingleOrDefault(x => x.Value.Equals(tsSolo.Delimiter, StringComparison.CurrentCultureIgnoreCase)).Key;
            }
        }

        private void btnSRApply_Click(object sender, RoutedEventArgs e)
        {
            bool saveOne = SaveSplitRowTransformation();
            if (saveOne)
            {
                bool saveTwo = SaveSplitTransformation();
                if (saveOne && saveTwo)
                {
                    if (ApplyTransformation())
                    {
                        myParent.Children.Clear();
                        Core_AMS.Utilities.WPF.MessageSaveComplete();
                    }
                }
            }
        }

        #region SAVE
        private bool SaveSplitRowTransformation()
        {
            var parameters = new SaveTransformationParameters();
            parameters.TransformationID = myTransformationID;
            parameters.Name = tbxSName.Text;
            parameters.Description = tbxSDesc.Text;
            parameters.TransformationCodeId = transformationType.CodeId;
            parameters.ClientId = myClient.ClientID;
            parameters.AuthorizedUserId = myAppData.AuthorizedUser.User.UserID;

            var result = SaveTransformationHelper.SaveTransformSplit(parameters, blt, dbWorker);
            if (result.Success)
            {
                myTransformationID = result.TransformID;
            }
            return result.Success;
        }
        private bool SaveSplitTransformation()
        {
            var newSavedPubCodes = new List<int>();
            if (rcbAssignPubcodes.SelectedItem == null)
            {
                //Core_AMS.Utilities.WPF.Message("Pub Code was not selected for current transformation. Save Cancelled.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Invalid Transformation");
                MessageBoxResult overwrite = MessageBox.Show("Pub Code not selected. Is this intentional?", "Pub Code Selection", MessageBoxButton.YesNo);
                if (overwrite == MessageBoxResult.No)
                    return false;
            }

            if (rcbSplitDelimiter.SelectedItem == null)
            {
                Core_AMS.Utilities.WPF.Message("Delimiter was not selected for current transformation. Save Cancelled.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Invalid Transformation");
                return false;
            }

            string delimiter = rcbSplitDelimiter.Text;

            List<FrameworkUAS.Entity.TransformationPubMap> tpmList = bltpm.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;

            if (rcbAssignPubcodes.SelectedItems.Count == 0)
            {
                var tpm = new FrameworkUAS.Entity.TransformationPubMap()
                {
                    TransformationPubMapID = 0,
                    TransformationID = myTransformationID,
                    PubID = -1,
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    CreatedByUserID = myAppData.AuthorizedUser.User.UserID,
                    UpdatedByUserID = myAppData.AuthorizedUser.User.UserID
                };
                bltpm.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tpm);
            }
            else
            {
                foreach (KeyValuePair<int, string> sel in rcbAssignPubcodes.SelectedItems)
                {
                    int transformationPubMapID = 0;
                    int selectedPubCode = 0;
                    int.TryParse(sel.Key.ToString(), out selectedPubCode);
                    if (selectedPubCode != 0)
                    {
                        newSavedPubCodes.Add(selectedPubCode);
                        FrameworkUAS.Entity.TransformationPubMap t = tpmList.FirstOrDefault(a => a.TransformationID == myTransformationID && a.PubID == selectedPubCode);
                        if (t != null)
                            transformationPubMapID = t.TransformationPubMapID;

                        if (transformationPubMapID == 0)
                        {
                            var tpm = new FrameworkUAS.Entity.TransformationPubMap()
                            {
                                TransformationPubMapID = 0,
                                TransformationID = myTransformationID,
                                PubID = selectedPubCode,
                                IsActive = true,
                                DateCreated = DateTime.Now,
                                DateUpdated = DateTime.Now,
                                CreatedByUserID = myAppData.AuthorizedUser.User.UserID,
                                UpdatedByUserID = myAppData.AuthorizedUser.User.UserID
                            };
                            bltpm.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tpm);
                        }
                    }
                }
            }

            //Delete the old
            var deleteThese = new List<int>();
            deleteThese = oldSavedPubCodes.Except(newSavedPubCodes).ToList();
            if (deleteThese.Count > 0)
            {
                foreach (int pub in deleteThese)
                {
                    bltpm.Proxy.Delete(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myTransformationID, pub);
                }
            }
            oldSavedPubCodes.Clear();
            oldSavedPubCodes.AddRange(newSavedPubCodes);
            newSavedPubCodes.Clear();

            var x = new FrameworkUAS.Entity.TransformSplit()
            {
                TransformSplitID = mySplitTransformationID,
                TransformationID = myTransformationID,
                Delimiter = delimiter,
                IsActive = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUserID = myAppData.AuthorizedUser.User.UserID,
                UpdatedByUserID = myAppData.AuthorizedUser.User.UserID
            };

            int TransSplitID = tsData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, x).Result;

            mySplitTransformationID = TransSplitID;

            return true;
        }
        public bool ApplyTransformation()
        {
            var fieldMappingIDInt = 0;
            int.TryParse(fieldMappingID, out fieldMappingIDInt);

            var parameters = new ApplyTransformationParameters();
            parameters.TransformationID = myTransformationID;
            parameters.FieldMappingID = fieldMappingIDInt;
            parameters.SourceFileID = sourceFileID;
            parameters.AuthorizedUserId = myAppData.AuthorizedUser.User.UserID;

            var succeeded = ApplyTransformationHelper.ApplyTransformation(parameters, tfmData, thisContainer);
            return succeeded;
        }
        #endregion
        private void rcbSplitPubcodes_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            if (rcbAssignPubcodes.SelectedValue != null)
            {
                rlbSplitPubcodes.Items.Clear();
                if (rcbAssignPubcodes.SelectedItems.Count > 0)
                {
                    foreach (var i in rcbAssignPubcodes.SelectedItems)
                    {
                        rlbSplitPubcodes.Items.Add(i.ToString());
                    }
                }
            }
        }

        private void delimiterComboBoxes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //private void lstSplitPubcodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (lstSplitPubcodes.SelectedValue != null)
        //    {
        //        rlbSplitPubcodes.Items.Clear();
        //        if (lstSplitPubcodes.SelectedItems.Count > 0)
        //        {
        //            foreach (var i in lstSplitPubcodes.SelectedItems)
        //            {
        //                rlbSplitPubcodes.Items.Add(i.ToString());
        //            }
        //        }
        //    }
        //}
    }
}
