using System;
using System.Linq;
using System.Windows;
using Core_AMS.Utilities;
using FrameworkServices;
using FrameworkUAS.Object;
using UAS_WS.Interface;

namespace FileMapperWizard.Helpers
{
    public class SaveTransformationHelper
    {
        private const string IncompleteSubmissionCaption = "Incomplete Submission";
        private const string SaveErrorMessageCaption = "Save Failure";
        private const string ConfirmOverriteMessageCaption = "Confirm Overwrite";

        public static readonly string DescriptionOrNameIsEmptyErrorMessage = "Description and/or Transformation Name were blank. Please fill these out and save again.";
        public static readonly string ConfirmOverriteMessage = "Do you wish to overwrite this transformation?";
        public static readonly string NameExistErrorMessage = "Transformation name exists. Rename and save again.";
        public static readonly string SaveErrorMessage = "An error occurred an we could not save.";
        
        public static SaveTransformationResult SaveTransformSplit(
            SaveTransformationParameters saveParams,
            ServiceClient<ITransformation> transformationClient,
            ServiceClient<IDBWorker> dbWorker)
        {
            var result = new SaveTransformationResult();
            result.Success = false;

            if (string.IsNullOrWhiteSpace(saveParams.Description) || string.IsNullOrWhiteSpace(saveParams.Name))
            {
                WPF.Message(
                    DescriptionOrNameIsEmptyErrorMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning,
                    MessageBoxResult.OK,
                    IncompleteSubmissionCaption);

                return result;
            }

            var allTrans = transformationClient.Proxy.Select(AppData.myAppData.AuthorizedUser.AuthAccessKey, saveParams.ClientId).Result;

            var found = allTrans.FirstOrDefault(x => x.TransformationName.Equals(
                saveParams.Name,
                StringComparison.CurrentCultureIgnoreCase));

            if (saveParams.TransformationID > 0 && found != null)
            {
                MessageBoxResult res = MessageBox.Show(
                    ConfirmOverriteMessage,
                    ConfirmOverriteMessageCaption,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (res == MessageBoxResult.No)
                {
                    return result;
                }
            }
            else if (saveParams.TransformationID == 0 && found != null)
            {
                WPF.Message(
                    NameExistErrorMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning,
                    MessageBoxResult.OK,
                    IncompleteSubmissionCaption);
                return result;
            }

            var transformId = dbWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey,
                saveParams.TransformationID,
                saveParams.TransformationCodeId,
                saveParams.Name,
                saveParams.Description,
                saveParams.ClientId,
                saveParams.AuthorizedUserId).Result;

            if (transformId <= 0)
            {
                WPF.Message(SaveErrorMessage,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning,
                    MessageBoxResult.OK,
                    SaveErrorMessageCaption);
                return result;
            }

            result.TransformID = transformId;
            result.Success = true;
            return result;
        }
    }
}
