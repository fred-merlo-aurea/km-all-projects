using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FileMapperWizard.Helpers;
using FrameworkUAS.Entity;
using FrameworkUAS.Object;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for DataMapTransformation.xaml
    /// </summary>
    public partial class DataMapTransformation : UserControl
    {
        private const string DuplicateSubmissionMessage = "Duplicate Transformation Source Mapped.";
        private const string DuplicateSubmissionCaption = "Duplicate Submission";
        private const string MatchNotSelectedMessage = "Please select a match type before saving.";
        private const string IncompleteSubmissionCaption = "Incomplete Submission";
        private const string PubCodeWasNotSelectedMessage = "Pub Code was not selected. If this is intentional click yes. If not click no and review before clicking apply.";
        private const string PubCodeWasNotSelectedCaption = "Pub Code Selection";
        private const string MappingsMissingMessage = "Nothing to save. Please add data mapping.";
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformation> blt = FrameworkServices.ServiceClient.UAS_TransformationClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IDBWorker> dbWorker = FrameworkServices.ServiceClient.UAS_DBWorkerClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationPubMap> bltpm = FrameworkServices.ServiceClient.UAS_TransformationPubMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMapping> fmData = FrameworkServices.ServiceClient.UAS_FieldMappingClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationFieldMap> tfmData = FrameworkServices.ServiceClient.UAS_TransformationFieldMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformDataMap> tdmData = FrameworkServices.ServiceClient.UAS_TransformDataMapClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();

        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        public List<int> oldSavedPubCodes = new List<int>();
        public KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        public Dictionary<int, string> pubCode = new Dictionary<int, string>();
        public int myTransformationID = 0;
        public int myDataID = 0;
        public int sourceFileID = 0;
        public string fieldMappingID = "";
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> svCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        public FrameworkUAD_Lookup.Entity.Code transformationType = new FrameworkUAD_Lookup.Entity.Code();
        StackPanel myParent;
        FileMapperWizard.Modules.FMUniversal thisContainer;

        public class CheckDataMapDups
        {
            public int Pub;
            public string Value;
        }

        public DataMapTransformation(FrameworkUAS.Object.AppData appData, FileMapperWizard.Modules.FMUniversal container, int sfID, KMPlatform.Entity.Client client, StackPanel parent, string transID, string mapID, bool existing)
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
            transformationType = codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Data_Mapping.ToString().Replace('_', ' '));

            pubCode = dbWorker.Proxy.GetPubIDAndCodesByClient(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient).Result;
            rcbAssignPubcodes.ItemsSource = pubCode.OrderBy(x => x.Value);
            rcbAssignPubcodes.SelectedMemberPath = "Key";
            rcbAssignPubcodes.DisplayMemberPath = "Value";

            if(existing == true)
            {
                int tID = 0;
                int.TryParse(transID, out tID);
                myTransformationID = tID;
                List<FrameworkUAS.Entity.Transformation> allTrans = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                FrameworkUAS.Entity.Transformation thisTrans = allTrans.SingleOrDefault(x => x.TransformationID == tID);
                List<FrameworkUAS.Entity.TransformDataMap> td = tdmData.Proxy.SelectForTransformation(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisTrans.TransformationID).Result;
                tbxDName.Text = thisTrans.TransformationName;
                tbxDDesc.Text = thisTrans.TransformationDescription;
                //rcbDataPubcodes            
                cbxMapsPub.IsChecked = thisTrans.MapsPubCode;
                cbxLastStep.IsChecked = thisTrans.LastStepDataMap;
                spSingleDataMap.Children.Clear();
                foreach (FrameworkUAS.Entity.TransformDataMap tdm in td)
                {
                    Dictionary<int, string> thisPub = new Dictionary<int, string>();
                    if (pubCode.SingleOrDefault(x => x.Key == tdm.PubID).Key != null)
                        thisPub.Add(pubCode.SingleOrDefault(x => x.Key == tdm.PubID).Key, pubCode.SingleOrDefault(x => x.Key == tdm.PubID).Value);

                    DataMap dm = new DataMap(myAppData, myClient, thisPub, pubCode, tdm);
                    spSingleDataMap.Children.Add(dm);
                }
            }
        }

        #region SAVE
        private bool SaveDataMapTransformation()
        {
            string saveAs = tbxDName.Text;
            string desc = tbxDDesc.Text;
            bool mapsPub = false;
            if (cbxMapsPub.IsChecked == true)
                mapsPub = true;
            else
                mapsPub = false;

            bool lastStep = false;
            if (cbxLastStep.IsChecked == true)
                lastStep = true;
            else
                lastStep = false;

            if (desc == "" || saveAs == "")
            {
                Core_AMS.Utilities.WPF.Message("Description and/or Transformation Name were blank. Please fill these out and save again.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, IncompleteSubmissionCaption);
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
                Core_AMS.Utilities.WPF.Message("Transformation name exists. Rename and save again.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, IncompleteSubmissionCaption);
                return false;
            }

            //Save to the Database the MappingName and Description
            int result = dbWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myTransformationID, transformationType.CodeId, saveAs, desc, myClient.ClientID, myAppData.AuthorizedUser.User.UserID, mapsPub, lastStep).Result;
            if (!(result > 0))
            {
                Core_AMS.Utilities.WPF.Message("An error occurred an we could not save.", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, "Save Failure");
                return false;
            }

            myTransformationID = result;
            return true;
        }
        private bool SaveDataMap()
        {
            if (!EnsureDataMapHasItems())
            {
                return false;
            }

            var sourcesOne = new List<string>();
            var sourcesTwo = new List<CheckDataMapDups>();
            var overridePubCodeCheck = false;
            foreach (DataMap dataMap in spSingleDataMap.Children)
            {
                if (!EnsurePubCodeExist(dataMap, ref overridePubCodeCheck) ||
                    !EnsureMatchTypeDefined(dataMap))
                {
                    return false;
                }

                var dataMapID = 0;
                int.TryParse(dataMap.ButtonTag, out dataMapID);
                var type = dataMap.MatchType;
                var source = dataMap.SourceData;
                var desired = dataMap.DesireData;
                var tpmList = bltpm.Proxy
                    .Select(AppData.myAppData.AuthorizedUser.AuthAccessKey)
                    .Result;

                if (dataMap.lstPubCode.SelectedItems.Count == 0)
                {
                    if (!EnsureNoDuplicateExist(sourcesOne, source))
                    {
                        return false;
                    }

                    CreateTransformDataMap(
                        sourcesOne,
                        source,
                        dataMap,
                        dataMapID,
                        type,
                        desired);
                }
                else
                {
                    foreach (KeyValuePair<int, string> sel in dataMap.lstPubCode.SelectedItems)
                    {
                        int selectedPubCode = 0;
                        int.TryParse(sel.Key.ToString(), out selectedPubCode);
                        if (selectedPubCode != 0)
                        {
                            if (!SaveTransformationPubMapForPubCodeSelected(
                                    sourcesTwo,
                                    selectedPubCode,
                                    source,
                                    tpmList))
                            {
                                return false;
                            }
                        }

                        CreateDataMapForPubMapsCreated(
                            dataMap, 
                            dataMapID,
                            selectedPubCode,
                            type, 
                            source,
                            desired);
                    }
                }
            }

            return true;
        }

        private void CreateDataMapForPubMapsCreated(DataMap sdmt, int dataMapID, int selectedPubCode, string type,
            string source, string desired)
        {
            var transformDataMapId = 0;
            int.TryParse(sdmt.ButtonTag, out transformDataMapId);
            transformDataMapId = transformDataMapId == 0 ? dataMapID : transformDataMapId;

            var x = new TransformDataMap()
            {
                TransformDataMapID = transformDataMapId,
                TransformationID = myTransformationID,
                PubID = selectedPubCode,
                MatchType = type,
                SourceData = source,
                DesiredData = desired,
                IsActive = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUserID = myAppData.AuthorizedUser.User.UserID,
                UpdatedByUserID = myAppData.AuthorizedUser.User.UserID
            };

            var tDataMapID = tdmData.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, x).Result;
        }

        private bool SaveTransformationPubMapForPubCodeSelected(List<CheckDataMapDups> sourcesTwo, int selectedPubCode, string source,
            List<TransformationPubMap> tpmList)
        {
            if (!EnsureDuplicatesMissingForPubCode(sourcesTwo, selectedPubCode, source))
            {
                return false;
            }

            sourcesTwo.Add(new CheckDataMapDups()
            {
                Pub = selectedPubCode,
                Value = source
            });
            var transformationPubMap =
                tpmList.FirstOrDefault(a => a.TransformationID == myTransformationID && a.PubID == selectedPubCode);
            if (transformationPubMap == null ||
                transformationPubMap.TransformationPubMapID == 0)
            {
                var tpm = new TransformationPubMap()
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
                bltpm.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, tpm);
            }

            return true;
        }

        private static bool EnsureDuplicatesMissingForPubCode(IEnumerable<CheckDataMapDups> sourcesTwo, int selectedPubCode, string source)
        {
            if (sourcesTwo.Any(c => c.Pub == selectedPubCode && c.Value == source))
            {
                Core_AMS.Utilities.WPF.Message(
                    DuplicateSubmissionMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning, 
                    MessageBoxResult.OK,
                    DuplicateSubmissionCaption);
                return false;
            }

            return true;
        }

        private void CreateTransformDataMap(
            ICollection<string> sourcesOne,
            string source,
            DataMap sdmt,
            int dataMapId,
            string type,
            string desired)
        {
            if (sdmt == null)
            {
                throw new ArgumentNullException(nameof(sdmt));
            }

            sourcesOne.Add(source);
            var tpm = new TransformationPubMap()
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
            bltpm.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, tpm);

            var transformDataMapId = 0;
            int.TryParse(sdmt.ButtonTag, out transformDataMapId);
            transformDataMapId = transformDataMapId == 0 ? dataMapId : transformDataMapId;

            var x = new TransformDataMap()
            {
                TransformDataMapID = transformDataMapId,
                TransformationID = myTransformationID,
                PubID = -1,
                MatchType = type,
                SourceData = source,
                DesiredData = desired,
                IsActive = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUserID = myAppData.AuthorizedUser.User.UserID,
                UpdatedByUserID = myAppData.AuthorizedUser.User.UserID
            };

            var tDataMapId = tdmData.Proxy
                .Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, x).Result;
            sdmt.ButtonTag = tDataMapId.ToString();
        }

        private static bool EnsureNoDuplicateExist(List<string> sourcesOne, string source)
        {
            if (sourcesOne.Contains(source))
            {
                Core_AMS.Utilities.WPF.Message(
                    DuplicateSubmissionMessage,
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning, 
                    MessageBoxResult.OK,
                    DuplicateSubmissionCaption);
                return false;
            }

            return true;
        }

        private static bool EnsureMatchTypeDefined(DataMap sdmt)
        {
            if (string.IsNullOrWhiteSpace(sdmt.MatchType))
            {
                Core_AMS.Utilities.WPF.Message(
                    MatchNotSelectedMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning, 
                    MessageBoxResult.OK,
                    IncompleteSubmissionCaption);
                return false;
            }

            return true;
        }

        private static bool EnsurePubCodeExist(DataMap dataMap, ref bool overridePubCodeCheck)
        {
            if (dataMap.myList.Count == 0 && !overridePubCodeCheck)
            {
                var overwrite = MessageBox.Show(
                    PubCodeWasNotSelectedMessage,
                    PubCodeWasNotSelectedCaption,
                    MessageBoxButton.YesNo);
                if (overwrite == MessageBoxResult.No)
                {
                    return false;
                }

                overridePubCodeCheck = true;
            }

            return true;
        }

        private bool EnsureDataMapHasItems()
        {
            if (spSingleDataMap.Children.Count == 0)
            {
                Core_AMS.Utilities.WPF.Message(
                    MappingsMissingMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning, 
                    MessageBoxResult.OK, 
                    IncompleteSubmissionCaption);
                return false;
            }

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
        private void btnDMApply_Click(object sender, RoutedEventArgs e)
        {
            bool saveOne = SaveDataMapTransformation();
            if (saveOne)
            {
                bool saveTwo = SaveDataMap();
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
        private void btnAddDataMap_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<int, string> thisPub = new Dictionary<int, string>();
            foreach (KeyValuePair<int, string> kvp in rcbAssignPubcodes.SelectedItems)
            {
                if (pubCode.SingleOrDefault(x => x.Key == kvp.Key).Key != null)
                    thisPub.Add(pubCode.SingleOrDefault(x => x.Key == kvp.Key).Key, pubCode.SingleOrDefault(x => x.Key == kvp.Key).Value);
            }

            DataMap dm = new DataMap(myAppData, myClient, thisPub, pubCode);
            spSingleDataMap.Children.Add(dm);
        }
    }
}
