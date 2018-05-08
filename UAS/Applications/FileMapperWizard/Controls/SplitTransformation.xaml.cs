using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FileMapperWizard.Helpers;
using FrameworkServices;
using FrameworkUAD_Lookup.Entity;
using Telerik.Windows.Controls;
using UAS_WS.Interface;


namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for SplitTransformation.xaml
    /// </summary>
    public partial class SplitTransformation : UserControl
    {
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformation> blt = FrameworkServices.ServiceClient.UAS_TransformationClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IDBWorker> dbWorker = FrameworkServices.ServiceClient.UAS_DBWorkerClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationPubMap> bltpm = FrameworkServices.ServiceClient.UAS_TransformationPubMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IFieldMapping> fmData = FrameworkServices.ServiceClient.UAS_FieldMappingClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformationFieldMap> tfmData = FrameworkServices.ServiceClient.UAS_TransformationFieldMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ITransformSplitTrans> stData = FrameworkServices.ServiceClient.UAS_TransformSplitTransClient();  
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();

        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        public List<int> oldSavedPubCodes = new List<int>();
        public KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        public Dictionary<int, string> pubCode = new Dictionary<int, string>();
        public int myTransformationID = 0;
        public int myTransSplitID = 0;
        public int sourceFileID = 0;
        public string fieldMappingID = "";
        public string colName = "";
        FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> svCodes = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        public FrameworkUAD_Lookup.Entity.Code transformationType = new FrameworkUAD_Lookup.Entity.Code();
        StackPanel myParent;
        FileMapperWizard.Modules.FMUniversal thisContainer;

        public SplitTransformation(FrameworkUAS.Object.AppData appData, FileMapperWizard.Modules.FMUniversal container, int sfID, KMPlatform.Entity.Client client, StackPanel parent, string transID, RadComboBox mapID, bool existing)
        {
            InitializeComponent();
            thisContainer = container;
            myParent = parent;
            myClient = client;
            sourceFileID = sfID;
            myAppData = appData;
            svCodes = codeWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Transformation);
            List<FrameworkUAD_Lookup.Entity.Code> codes = svCodes.Result;
            transformationType = codes.FirstOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Transform.ToString().Replace('_', ' '));
            if (mapID != null)
            {
                fieldMappingID = mapID.SelectedValue.ToString();
                colName = mapID.Text;
            }

            rcbBeforeSplit.ItemsSource = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID, false).Result.Where(x => x.TransformationTypeID == codes.FirstOrDefault(y => y.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace('_', ' ')).CodeId).ToList();
            rcbBeforeSplit.DisplayMemberPath = "TransformationName";
            rcbBeforeSplit.SelectedValuePath = "TransformationID";
            rcbAfterSplit.ItemsSource = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID, false).Result.Where(x => x.TransformationTypeID == codes.FirstOrDefault(y => y.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace('_', ' ')).CodeId).ToList();
            rcbAfterSplit.DisplayMemberPath = "TransformationName";
            rcbAfterSplit.SelectedValuePath = "TransformationID";
            rcbApplyTrans.ItemsSource = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID, false).Result.Where(x => x.TransformationTypeID == codes.FirstOrDefault(y => y.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Data_Mapping.ToString().Replace('_', ' ')).CodeId).ToList();
            rcbApplyTrans.DisplayMemberPath = "TransformationName";
            rcbApplyTrans.SelectedValuePath = "TransformationID";

            if(existing == true)
            {
                int tID = 0;
                int.TryParse(transID, out tID);
                myTransformationID = tID;
                List<FrameworkUAS.Entity.Transformation> allTrans = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                FrameworkUAS.Entity.Transformation thisTrans = allTrans.SingleOrDefault(x => x.TransformationID == tID);
                rcbBeforeSplit.ItemsSource = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID, false).Result.Where(x => x.TransformationTypeID == codes.FirstOrDefault(y => y.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace('_', ' ')).CodeId).ToList();
                rcbBeforeSplit.DisplayMemberPath = "TransformationName";
                rcbBeforeSplit.SelectedValuePath = "TransformationID";
                rcbAfterSplit.ItemsSource = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID, false).Result.Where(x => x.TransformationTypeID == codes.FirstOrDefault(y => y.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Split_Into_Rows.ToString().Replace('_', ' ')).CodeId).ToList();
                rcbAfterSplit.DisplayMemberPath = "TransformationName";
                rcbAfterSplit.SelectedValuePath = "TransformationID";
                rcbApplyTrans.ItemsSource = blt.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientID, false).Result.Where(x => x.TransformationTypeID == codes.FirstOrDefault(y => y.CodeName == FrameworkUAD_Lookup.Enums.TransformationTypes.Data_Mapping.ToString().Replace('_', ' ')).CodeId).ToList();
                rcbApplyTrans.DisplayMemberPath = "TransformationName";
                rcbApplyTrans.SelectedValuePath = "TransformationID";

                List<FrameworkUAS.Entity.TransformSplitTrans> st = stData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, thisTrans.TransformationID).Result;
                FrameworkUAS.Entity.TransformSplitTrans stSolo = st.SingleOrDefault(x => x.TransformationID == thisTrans.TransformationID);
                if (stSolo != null)
                {
                    myTransSplitID = stSolo.SplitTransformID;
                    tbxTName.Text = thisTrans.TransformationName;
                    tbxTDesc.Text = thisTrans.TransformationDescription;
                    rcbBeforeSplit.SelectedValue = stSolo.SplitBeforeID;
                    rcbApplyTrans.SelectedValue = stSolo.DataMapID;
                    rcbAfterSplit.SelectedValue = stSolo.SplitAfterID;
                }
            }
        }

        private void btnSTApply_Click(object sender, RoutedEventArgs e)
        {
            bool saveOne = SaveTransformSplit();
            if (saveOne)
            {
                bool saveTwo = SaveTransSplit();
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
        private bool SaveTransformSplit()
        {
            var saveParams = new SaveTransformationParameters();
            saveParams.TransformationID = myTransformationID;
            saveParams.Name = tbxTName.Text;
            saveParams.Description = tbxTDesc.Text;
            saveParams.TransformationCodeId = transformationType.CodeId;
            saveParams.ClientId = myClient.ClientID;
            saveParams.AuthorizedUserId = myAppData.AuthorizedUser.User.UserID;
            var result = SaveTransformationHelper.SaveTransformSplit(saveParams, blt, dbWorker);
            if (result.Success)
            {
                myTransformationID = result.TransformID;
            }
            return result.Success;
        }

        private bool SaveTransSplit()
        {
            if (colName == null || colName == String.Empty)
            {
                Core_AMS.Utilities.WPF.Message("Column must be specified. Save Cancelled.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Invalid Transformation");
                return false;
            }

            int sbID = 0;
            if (rcbBeforeSplit.SelectedValue != null)
                int.TryParse(rcbBeforeSplit.SelectedValue.ToString(), out sbID);

            int dmID = 0;
            if (rcbApplyTrans.SelectedValue != null)
                int.TryParse(rcbApplyTrans.SelectedValue.ToString(), out dmID);

            int saID = 0;
            if (rcbAfterSplit.SelectedValue != null)
                int.TryParse(rcbAfterSplit.SelectedValue.ToString(), out saID);

            FrameworkUAS.Entity.TransformSplitTrans x = new FrameworkUAS.Entity.TransformSplitTrans()
            {
                SplitTransformID = myTransSplitID,
                TransformationID = myTransformationID,
                SplitBeforeID = sbID,
                DataMapID = dmID,
                SplitAfterID = saID,
                Column = colName,
                IsActive = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedByUserID = myAppData.AuthorizedUser.User.UserID,
                UpdatedByUserID = myAppData.AuthorizedUser.User.UserID
            };

            int result = stData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, x).Result;
            if (!(result > 0))
            {
                return false;
            }

            myTransSplitID = result;

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

    }
}
