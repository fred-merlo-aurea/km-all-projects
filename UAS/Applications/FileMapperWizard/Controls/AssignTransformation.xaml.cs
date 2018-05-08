using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FileMapperWizard.Helpers;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for AssignTransformation.xaml
    /// </summary>
    public partial class AssignTransformation : UserControl
    {

        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformation> blt = FrameworkServices.ServiceClient.UAS_TransformationClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IDBWorker> dbWorker = FrameworkServices.ServiceClient.UAS_DBWorkerClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationPubMap> bltpm = FrameworkServices.ServiceClient.UAS_TransformationPubMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformAssign> taData = FrameworkServices.ServiceClient.UAS_TransformAssignClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMapping> fmData = FrameworkServices.ServiceClient.UAS_FieldMappingClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationFieldMap> tfmData = FrameworkServices.ServiceClient.UAS_TransformationFieldMapClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();

        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        public List<int> oldSavedPubCodes = new List<int>();
        public KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        public Dictionary<int, string> pubCode = new Dictionary<int, string>();
        public int myTransformationID = 0;
        public int myAssignID = 0;
        public int sourceFileID = 0;
        public int fieldMappingID = 0;
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> svCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        public FrameworkUAD_Lookup.Entity.Code transformationType = new FrameworkUAD_Lookup.Entity.Code();
        StackPanel myParent;
        FileMapperWizard.Modules.FMUniversal thisContainer;

        public AssignTransformation(FrameworkUAS.Object.AppData appData, FileMapperWizard.Modules.FMUniversal container, int sfID, KMPlatform.Entity.Client client, StackPanel parent, int transID, FrameworkUAS.Entity.FieldMapping fmObject, bool existing)
        {
            InitializeComponent();
            thisContainer = container;
            myParent = parent;
            myClient = client;
            sourceFileID = sfID;
            myAppData = appData;
            fieldMappingID = fmObject == null ? 0 : fmObject.FieldMappingID;     
            svCodes = codeWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            List<FrameworkUAD_Lookup.Entity.Code> codes = svCodes.Result;
            transformationType = codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Assign_Value.ToString().Replace('_', ' '));

            pubCode = dbWorker.Proxy.GetPubIDAndCodesByClient(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient).Result;
            rcbAssignPubcodes.ItemsSource = pubCode.OrderBy(x => x.Value);
            rcbAssignPubcodes.SelectedMemberPath = "Key";
            rcbAssignPubcodes.DisplayMemberPath = "Value";

            //hide if IsNonFileColumn = true and MAFField = PUBCODE
            if(fmObject != null && fmObject.IsNonFileColumn == true && fmObject.MAFField.Equals("PUBCODE", StringComparison.CurrentCultureIgnoreCase))
            {
                txtAssignPubcodes.Visibility = System.Windows.Visibility.Collapsed;
                rcbAssignPubcodes.Visibility = System.Windows.Visibility.Collapsed;
                txtSelectedPubCodes.Visibility = System.Windows.Visibility.Collapsed;
                rlbAssignPubcodes.Visibility = System.Windows.Visibility.Collapsed;
            }
            if(existing == true)
            {
                myTransformationID = transID;
                List<FrameworkUAS.Entity.Transformation> allTrans = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                FrameworkUAS.Entity.Transformation thisTrans = allTrans.SingleOrDefault(x => x.TransformationID == transID);
                List<FrameworkUAS.Entity.TransformAssign> ta = taData.Proxy.SelectForTransformation(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisTrans.TransformationID).Result;
                FrameworkUAS.Entity.TransformAssign taSolo = ta.SingleOrDefault(x => x.TransformationID == thisTrans.TransformationID);
                myAssignID = taSolo.TransformAssignID;
                tbxAName.Text = thisTrans.TransformationName;
                tbxADesc.Text = thisTrans.TransformationDescription;
                rlbAssignPubcodes.Items.Clear();
                oldSavedPubCodes.Clear();
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
                            rlbAssignPubcodes.Items.Add(i.Value.ToString());

                    }
                }
                txtBoxValue.Text = taSolo.Value;
            }
        }

        private void rcbAssignPubcodes_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            if (rcbAssignPubcodes.SelectedValue != null)
            {
                rlbAssignPubcodes.Items.Clear();
                if (rcbAssignPubcodes.SelectedItems.Count > 0)
                {
                    foreach (var i in rcbAssignPubcodes.SelectedItems)
                    {
                        rlbAssignPubcodes.Items.Add(i.ToString());
                    }
                }
            }
        }

        #region SAVE
        private bool SaveAssignTransformation()
        {
            string saveAs = tbxAName.Text;
            string desc = tbxADesc.Text;
            if (desc == "" || saveAs == "")
            {
                Core_AMS.Utilities.WPF.Message("Description and/or Transformation Name were blank. Please fill these out and save again.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Incomplete Submission");
                return false;
            }


            List<FrameworkUAS.Entity.Transformation> allTrans = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID).Result;
            FrameworkUAS.Entity.Transformation found = allTrans.FirstOrDefault(x => x.TransformationName.Equals(saveAs, StringComparison.CurrentCultureIgnoreCase));
            if (myTransformationID > 0 && found != null)
            {
                MessageBoxResult res = MessageBox.Show("Do you wish to overwrite this transformation?","Confirm Overwrite", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.No) return false;
            }
            else if (myTransformationID == 0 && found != null)
            {
                Core_AMS.Utilities.WPF.Message(msg: "Transformation name exists. Rename and save again.",btn: MessageBoxButton.OK,icon: MessageBoxImage.Warning,result: MessageBoxResult.OK,caption: "Incomplete Submission");
                return false;
            }

            //Save to the Database the MappingName and Description
            int result = dbWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myTransformationID, transformationType.CodeId, saveAs, desc, myClient.ClientID, myAppData.AuthorizedUser.User.UserID).Result;
            if (!(result > 0))
            {
                Core_AMS.Utilities.WPF.Message(msg: "An error occurred an we could not save.",btn: MessageBoxButton.OK,icon: MessageBoxImage.Warning,result: MessageBoxResult.OK,caption: "Save Failure");
                return false;
            }

            myTransformationID = result;
            return true;
        }
        private bool SaveAssign()
        {
            bool status = false;
            bool hasPubID = true;
            var PubCodes = new Dictionary<int, string>();
            var newSavedPubCodes = new List<int>();
            if (rcbAssignPubcodes.SelectedItems.Count == 0)
            {
                PubCodes.Add(-1, string.Empty);
                hasPubID = false;
            }
            else
            {
                hasPubID = true;
                foreach (KeyValuePair<int, string> sel in rcbAssignPubcodes.SelectedItems)
                    PubCodes.Add(sel.Key, sel.Value);
            }


            if (txtBoxValue.Text == "")
            {
                Core_AMS.Utilities.WPF.Message("Missing Value to Assign", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Required Value");
                return status;
            }

            string value = txtBoxValue.Text;

            foreach (KeyValuePair<int, string> sel in PubCodes)
            {
                int selectedPubCode = 0;
                int.TryParse(sel.Key.ToString(), out selectedPubCode);
                if (selectedPubCode > 0)
                {
                    newSavedPubCodes.Add(selectedPubCode);
                    FrameworkUAS.Entity.TransformationPubMap tpm = new FrameworkUAS.Entity.TransformationPubMap();
                    tpm.TransformationPubMapID = 0;
                    tpm.TransformationID = myTransformationID;
                    tpm.PubID = selectedPubCode;
                    tpm.IsActive = true;
                    tpm.DateCreated = DateTime.Now;
                    tpm.DateUpdated = DateTime.Now;
                    tpm.CreatedByUserID = myAppData.AuthorizedUser.User.UserID;
                    tpm.UpdatedByUserID = myAppData.AuthorizedUser.User.UserID;

                    bltpm.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, tpm);
                }
            }

            //Delete the old
            List<int> deleteThese = new List<int>();
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

            FrameworkUAS.Entity.TransformAssign x = new FrameworkUAS.Entity.TransformAssign();
            x.TransformAssignID = myAssignID;
            x.TransformationID = myTransformationID;
            x.Value = value;
            x.IsActive = true;
            x.HasPubID = hasPubID;
            x.DateCreated = DateTime.Now;
            x.DateUpdated = DateTime.Now;
            x.CreatedByUserID = myAppData.AuthorizedUser.User.UserID;
            x.UpdatedByUserID = myAppData.AuthorizedUser.User.UserID;


            int transAssignID = taData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, x).Result;
            myAssignID = transAssignID;

            string whatsReplace = "";
            if (hasPubID)
            {
                List<string> outputPubCodes = new List<string>();
                foreach (int i in oldSavedPubCodes)
                {
                    if (pubCode[i] != null)
                        outputPubCodes.Add(pubCode[i]);

                }
                whatsReplace = "  whose pubcode(s) is: " + string.Join(",", outputPubCodes).TrimEnd(',');
            }
            else
                whatsReplace = " for all records";
            
            Core_AMS.Utilities.WPF.Message("Column with this transformation applied to it shall replace row values with '" + value + "'" + whatsReplace + ".", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Text Representation");
            status = true;
            return status;
        }
        public bool ApplyTransformation()
        {
            var parameters = new ApplyTransformationParameters();
            parameters.TransformationID = myTransformationID;
            parameters.FieldMappingID = fieldMappingID;
            parameters.SourceFileID = sourceFileID;
            parameters.AuthorizedUserId = myAppData.AuthorizedUser.User.UserID;

            var succeeded = ApplyTransformationHelper.ApplyTransformation(parameters, tfmData, thisContainer);
            return succeeded;
        }
        #endregion

        private void btnAApply_Click(object sender, RoutedEventArgs e)
        {
            bool saveOne = SaveAssignTransformation();
            if (saveOne)
            {
                bool saveTwo = SaveAssign();
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

        private void lstAssignPubcodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rcbAssignPubcodes.SelectedValue != null)
            {
                rlbAssignPubcodes.Items.Clear();
                if (rcbAssignPubcodes.SelectedItems.Count > 0)
                {
                    foreach (var i in rcbAssignPubcodes.SelectedItems)
                    {
                        rlbAssignPubcodes.Items.Add(i.ToString());
                    }
                }
            }
        }
    }
}
