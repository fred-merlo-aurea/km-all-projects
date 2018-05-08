using FileMapperWizard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.DragDrop;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for JoinTransformation.xaml
    /// </summary>
    public partial class JoinTransformation : UserControl
    {
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationPubMap> bltpm = FrameworkServices.ServiceClient.UAS_TransformationPubMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformation> blt = FrameworkServices.ServiceClient.UAS_TransformationClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IDBWorker> dbWorker = FrameworkServices.ServiceClient.UAS_DBWorkerClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformJoin> tjData = FrameworkServices.ServiceClient.UAS_TransformJoinClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationFieldMap> tfmData = FrameworkServices.ServiceClient.UAS_TransformationFieldMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMapping> fmData = FrameworkServices.ServiceClient.UAS_FieldMappingClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> blfmt = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();

        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        public Dictionary<int, string> pubCode = new Dictionary<int, string>();
        public Dictionary<string, string> delimiters = new Dictionary<string, string>();
        public KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        public int myJoinID = 0;
        public int myTransformationID = 0;
        public List<int> oldSavedPubCodes = new List<int>();
        public int sourceFileID;
        public string fieldMappingID = "";
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> svCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        public FrameworkUAD_Lookup.Entity.Code transformationType = new FrameworkUAD_Lookup.Entity.Code();
        StackPanel myParent;
        FileMapperWizard.Modules.FMUniversal thisContainer;

        public JoinTransformation(FrameworkUAS.Object.AppData appData, FileMapperWizard.Modules.FMUniversal container, int sfID, KMPlatform.Entity.Client client, StackPanel parent, string transID, string mapID, bool existing)
        {
            InitializeComponent();
            thisContainer = container;
            myAppData = appData;
            myParent = parent;
            myClient = client;
            sourceFileID = sfID;
            fieldMappingID = mapID;
            svCodes = codeWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            List<FrameworkUAD_Lookup.Entity.Code> codes = svCodes.Result;
            transformationType = codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Join_Columns.ToString().Replace('_', ' '));

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
            rcbColumnDelimiters.ItemsSource = delimiters;
            rcbColumnDelimiters.SelectedValuePath = "Key";
            rcbColumnDelimiters.DisplayMemberPath = "Value";
            if (existing == false)
            {
                List<FrameworkUAS.Entity.FieldMapping> thisFM = fmData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.Where(x => x.SourceFileID == sourceFileID).ToList();
                foreach (FrameworkUAS.Entity.FieldMapping fm in thisFM)
                {
                    rlbAvailableCols.Items.Add(fm.IncomingField);
                }
            }
            else
            {
                int tID = 0;
                int.TryParse(transID, out tID);
                List<FrameworkUAS.Entity.Transformation> allTrans = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                FrameworkUAS.Entity.Transformation thisTrans = allTrans.SingleOrDefault(x => x.TransformationID == tID);
                List<FrameworkUAS.Entity.TransformJoin> tj = tjData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisTrans.TransformationID).Result;
                FrameworkUAS.Entity.TransformJoin tjSolo = tj.SingleOrDefault(x => x.TransformationID == thisTrans.TransformationID);
                myTransformationID = tID;
                myJoinID = tjSolo.TransformJoinID;
                tbxJName.Text = thisTrans.TransformationName;
                tbxJDesc.Text = thisTrans.TransformationDescription;
                rcbColumnDelimiters.SelectedValue = tjSolo.Delimiter;

                List<FrameworkUAS.Entity.FieldMapping> thisFM = fmData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, sourceFileID).Result;
                rlbAvailableCols.Items.Clear();
                rlbJoinedCols.Items.Clear();
                List<string> joinCols = new List<string>();
                char del = ',';
                if (tjSolo != null && tjSolo.Delimiter != null)
                    char.TryParse(tjSolo.Delimiter, out del);

                joinCols = tjSolo.ColumnsToJoin.Split(del).ToList();
                foreach (string jc in joinCols)
                {
                    rlbJoinedCols.Items.Add(jc);
                }
                foreach (FrameworkUAS.Entity.FieldMapping fm in thisFM)
                {
                    if (!rlbJoinedCols.Items.Contains(fm.IncomingField) && sourceFileID > 0)
                    {
                        rlbAvailableCols.Items.Add(fm.IncomingField);
                    }
                }
                rlbJoinPubcodes.Items.Clear();
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
                            rlbJoinPubcodes.Items.Add(i.Value.ToString());

                    }
                }
            }
            DragDropManager.AddPreviewDropHandler(rlbJoinedCols, OnDroppedJoinedCols);
            DragDropManager.AddPreviewDropHandler(rlbAvailableCols, OnDroppedAvailCols);
        }

        private void btnViewFormat_Click(object sender, RoutedEventArgs e)
        {
            string errMsg = "";
            if (rlbJoinedCols.Items.Count == 0)
                errMsg = "Joined Columns \n";
            if (rcbColumnDelimiters.SelectedItem == null)
                errMsg = errMsg + "Column Delimiter";

            if (errMsg.Length > 0)
            {
                errMsg = "Please select or resolve the missing data and try again: \n \n" + errMsg;
                Core_AMS.Utilities.WPF.Message(errMsg, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Missing Data");
            }
            else
            {
                string format = "";
                foreach (string col in rlbJoinedCols.Items)
                {
                    format = format + rcbColumnDelimiters.SelectedValue.ToString() + col;
                }
                Core_AMS.Utilities.WPF.Message("Data will display as follows\n\n" + format.TrimStart(rcbColumnDelimiters.SelectedValue.ToString().ToCharArray()), MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Data Preview");
            }
        }

        private bool SaveJoinTransformation()
        {
            string saveAs = tbxJName.Text;
            string desc = tbxJDesc.Text;
            if (desc == "" || saveAs == "")
            {
                Core_AMS.Utilities.WPF.Message("Description and/or Transformation Name were blank. Please fill these out and save again.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Incomplete Submission");
                return false;
            }


            List<FrameworkUAS.Entity.Transformation> allTrans = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID).Result;
            FrameworkUAS.Entity.Transformation found = allTrans.FirstOrDefault(x => x.TransformationName.Equals(saveAs, StringComparison.CurrentCultureIgnoreCase));
            if (myTransformationID > 0 && found != null)
            {
                MessageBoxResult res = MessageBox.Show("Do you wish to overwrite this transformation?", "Confirm Overwrite", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.No) return false;
            }
            else if (myTransformationID == 0 && found != null)
            {
                Core_AMS.Utilities.WPF.Message("Transformation name exists. Rename and save again.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Incomplete Submission");
                return false;
            }

            //Save to the Database the MappingName and Description
            int result = dbWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myTransformationID, transformationType.CodeId, saveAs, desc, myClient.ClientID, myAppData.AuthorizedUser.User.UserID).Result;
            if (!(result > 0))
            {
                Core_AMS.Utilities.WPF.Message("An error occurred and we could not save.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Save Failure");
                return false;
            }

            myTransformationID = result;
            return true;
        }
        private bool SaveJoinTransform()
        {
            List<int> newSavedPubCodes = new List<int>();
            string columns = "";
            if (rcbColumnDelimiters.SelectedValue != null)
            {
                if (rlbJoinedCols.Items.Count > 0)
                {
                    //string format = "";
                    foreach (string col in rlbJoinedCols.Items)
                    {
                        columns = columns + rcbColumnDelimiters.SelectedValue.ToString() + col;
                    }
                }
            }

            string delimiter = "";
            if (rcbColumnDelimiters.SelectedValue != null)
            {
                columns = columns.TrimStart(rcbColumnDelimiters.SelectedValue.ToString().ToCharArray());
                delimiter = rcbColumnDelimiters.SelectedValue.ToString();//cbDelimiter.Text;
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("Delimiter must be selected. Please select and save again.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Incomplete Submission");
                return false;
            }

            if (rcbAssignPubcodes.SelectedItem == null)
            {
                Core_AMS.Utilities.WPF.Message("Please select a Pub Code before saving.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Incomplete Submission");
                return false;
            }

            if (columns == "")
            {
                Core_AMS.Utilities.WPF.Message("Columns were not set. Please fill these out and save again.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Incomplete Submission");
                return false;
            }

            List<FrameworkUAS.Entity.TransformationPubMap> tpmList = bltpm.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;

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
                        FrameworkUAS.Entity.TransformationPubMap tpm = new FrameworkUAS.Entity.TransformationPubMap()
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

            FrameworkUAS.Entity.TransformJoin x = new FrameworkUAS.Entity.TransformJoin()
            {
                TransformJoinID = myJoinID,
                TransformationID = myTransformationID,
                ColumnsToJoin = columns,
                Delimiter = rcbColumnDelimiters.SelectedValue.ToString(),
                IsActive = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUserID = myAppData.AuthorizedUser.User.UserID,
                UpdatedByUserID = myAppData.AuthorizedUser.User.UserID
            };

            int TransJoinID = tjData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, x).Result;

            myJoinID = TransJoinID;

            return true;
        }

        private void btnJCApply_Click(object sender, RoutedEventArgs e)
        {
            #region SAVE
            bool saveOne = SaveJoinTransformation();
            if (saveOne)
            {
                bool saveTwo = SaveJoinTransform();
                if (saveOne && saveTwo)
                {
            
                    #region APPLY
                    if (ApplyTransformation())
                    {
                        myParent.Children.Clear();
                        Core_AMS.Utilities.WPF.MessageSaveComplete();
                    }
                    #endregion
                }
            }
            #endregion
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

        private void OnDroppedJoinedCols(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            var data = DragDropPayloadManager.GetDataFromObject(e.Data, typeof(string));
            var item = (string)((List<object>)data).First();
            if (item != null)
            {
                rlbJoinedCols.Items.Add(item);
            }
            e.Handled = true;
        }
        private void OnDroppedAvailCols(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
        {
            var data = DragDropPayloadManager.GetDataFromObject(e.Data, typeof(string));
            var item = (string)((List<object>)data).First();
            if (item != null)
            {
                rlbAvailableCols.Items.Add(item);
            }
            e.Handled = true;
        }

        private void rcbAssignPubcodes_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            if (rcbAssignPubcodes.SelectedValue != null)
            {
                rlbJoinPubcodes.Items.Clear();
                if (rcbAssignPubcodes.SelectedItems.Count > 0)
                {
                    foreach (var i in rcbAssignPubcodes.SelectedItems)
                    {
                        rlbJoinPubcodes.Items.Add(i.ToString());
                    }
                }
            }
        }

        //private void lstJoinPubcodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (rcbAssignPubcodes.SelectedItem != null)
        //    {
        //        rlbJoinPubcodes.Items.Clear();
        //        if (rcbAssignPubcodes.SelectedItems.Count > 0)
        //        {
        //            foreach (var i in rcbAssignPubcodes.SelectedItems)
        //            {
        //                rlbJoinPubcodes.Items.Add(i.ToString());
        //            }
        //        }
        //    }
        //}
    }
}
