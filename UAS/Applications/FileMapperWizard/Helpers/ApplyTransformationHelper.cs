using System;
using System.Linq;
using System.Windows;
using Core_AMS.Utilities;
using FileMapperWizard.Modules;
using FrameworkServices;
using FrameworkUAS.Entity;
using FrameworkUAS.Object;
using UAS_WS.Interface;

namespace FileMapperWizard.Helpers
{
    public class ApplyTransformationHelper
    {
        private const string UnexpectedError = "Unexpected Error";
        private const string AlreadyAddedMessageCaption = "Informational";

        public static readonly string AlreadyAddedErrorMessage = "Transformation already added.";
        public static readonly string SourceNotClearErrorMessage = "Failed to Add Transformation. Source was unclear.";
        public static readonly string ColumnNotClearErrorMessage = "Failed to Add Transformation. Column selected was unclear.";
        public static readonly string TransformationNotClearErrorMessage = "Failed to Add Transformation. Transformation to add unclear.";
        
        public static bool ApplyTransformation(
            ApplyTransformationParameters parameters,
            ServiceClient<ITransformationFieldMap> transformationFieldMapClient,
            FMUniversal thisContainer)
        {
            if (parameters.TransformationID <= 0)
            {
                WPF.Message(TransformationNotClearErrorMessage, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, UnexpectedError);
                return false;
            }
            if (parameters.FieldMappingID <= 0)
            {
                WPF.Message(ColumnNotClearErrorMessage, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, UnexpectedError);
                return false;
            }
            if (parameters.SourceFileID <= 0)
            {
                WPF.Message(SourceNotClearErrorMessage, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, UnexpectedError);
                return false;
            }

            var findCurrent = transformationFieldMapClient.Proxy
                .Select(AppData.myAppData.AuthorizedUser.AuthAccessKey)
                .Result
                .FirstOrDefault(x =>
                x.SourceFileID == parameters.SourceFileID
                && x.TransformationID == parameters.TransformationID
                && x.FieldMappingID == parameters.FieldMappingID);

            if (findCurrent == null)
            {
                var tfm = new TransformationFieldMap()
                {
                    TransformationFieldMapID = 0,
                    TransformationID = parameters.TransformationID,
                    SourceFileID = parameters.SourceFileID,
                    FieldMappingID = parameters.FieldMappingID,
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    CreatedByUserID = parameters.AuthorizedUserId,
                    UpdatedByUserID = parameters.AuthorizedUserId
                };

                var response = transformationFieldMapClient.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, tfm);
                var saveResult = response.Result;

                if (
                    thisContainer != null &&
                    saveResult > 0 &&
                    (!thisContainer.fieldMappingsWithTransformations.Contains(parameters.FieldMappingID)))
                {
                    thisContainer.fieldMappingsWithTransformations.Add(parameters.FieldMappingID);
                }

                return true;
            }
            else
            {
                WPF.Message(AlreadyAddedErrorMessage, MessageBoxButton.OK, MessageBoxImage.Information, AlreadyAddedMessageCaption);
                return true;
            }
        }
    }
}