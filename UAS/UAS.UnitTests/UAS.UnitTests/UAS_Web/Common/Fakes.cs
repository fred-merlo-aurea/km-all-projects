using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.QualityTools.Testing.Fakes;
using UAS.Web.Controllers.Common.Fakes;
using UAS.Web.Controllers.FileMapperWizard.Fakes;

namespace UAS.UnitTests.UAS_Web.Common
{
    [ExcludeFromCodeCoverage]
    public class Fakes
    {
        private IDisposable _context;

        protected static readonly string TransformationFieldMapExceptionMessage =
            "<li>Failed to Add Transformation. Transformation to add unclear.</li>";

        protected static readonly string FieldMappingIdExceptionMessage =
            "<li>Failed to Add Transformation. Column selected was unclear.</li>";

        protected static readonly string SourceFileIdExceptionMessage =
            "<li>Failed to Add Transformation. Source was unclear.</li>";

        protected static readonly string ErrorApplyingTransformationMessage =
            "<li>Error applying transformation.</li>";

        public void SetupContext()
        {
            _context = ShimsContext.Create();
        }

        public void DisposeContext()
        {
            _context.Dispose();
        }

        protected virtual void ShimForCurrentUser()
        {
            ShimBaseController.AllInstances.CurrentUserGet =
                user => new KMPlatform.Entity.User()
                {
                    UserID = 1
                };
        }

        protected virtual void ShimForApplyTransformationPubMap()
        {
            ShimFileMapperWizardController.AllInstances.ApplyTransformationPubMapInt32ListOfString =
                (_, transformationId, pubIDs) => string.Empty;
        }
    }
}
