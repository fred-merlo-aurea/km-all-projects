
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ecn.activityengines.Tests.Setup.Mocks;
using Microsoft.QualityTools.Testing.Fakes;

namespace ecn.activityengines.Tests.Setup
{
    [ExcludeFromCodeCoverage]
    public class MocksContext : IDisposable
    {
        private readonly IDisposable _shimsContext;

        public MocksContext()
        {
            _shimsContext = ShimsContext.Create();
            PublicPreview = new PublicPreviewMock();
            Page = new PageMock();
            Request = new RequestMock();
            LayoutBusiness = new LayoutBusinessMock();
            ApplicationLog = new ApplicationLogMock();
            Encryption = new EncryptionMock();
            BlastBusiness = new BlastBusinessMock();
            EmailHistory = new EmailHistoryMock();
            Email = new EmailMock();
            EmailGroup = new EmailGroupMock();
            CustomerBusiness = new CustomerBusinessMock();
            BaseChannelBusiness = new BaseChannelBusinessMock();
            BlastSingleBusiness = new BlastSingleBusinessMock();
            ContentFilter = new ContentFilterMock();
            RSSFeed = new RSSFeedMock();
            Content = new ContentMock();
            TemplateFunctions = new TemplateFunctionsMock();
            CampaignItemTestBlast = new CampaignItemTestBlastMock();
            CampaignItem = new CampaignItemMock();
            BlastLink = new BlastLinkMock();
            DataFunctions = new DataFunctionsMock();
            CampaignItemSocialMedia = new CampaignItemSocialMediaMock();
            SocialMedia = new SocialMediaMock();
            GroupBusiness = new GroupBusinessMock();
            SmartFormsHistory = new SmartFormsHistoryMock();
            SmartFormTracking = new SmartFormTrackingMock();
            UserBusiness = new UserBusinessMock();
            GroupDataFields = new GroupDataFieldsMock();
            EmailDirect = new EmailDirectMock();
            Response = new ResponseMock();
            ShimAppSettings();
        }

        public NameValueCollection AppSettings { get; private set; }

        public PublicPreviewMock PublicPreview { get; private set; }

        public PageMock Page { get; private set; }

        public RequestMock Request { get; private set; }

        public LayoutBusinessMock LayoutBusiness { get; private set; }

        public ApplicationLogMock ApplicationLog { get; private set; }

        public EncryptionMock Encryption { get; private set; }

        public BlastBusinessMock BlastBusiness { get; private set; }

        public EmailHistoryMock EmailHistory { get; private set; }

        public EmailMock Email { get; private set; }

        public EmailGroupMock EmailGroup { get; private set; }

        public CustomerBusinessMock CustomerBusiness { get; private set; }

        public BaseChannelBusinessMock BaseChannelBusiness { get; private set; }

        public BlastSingleBusinessMock BlastSingleBusiness { get; private set; }

        public ContentFilterMock ContentFilter { get; private set; }

        public RSSFeedMock RSSFeed { get; private set; }

        public ContentMock Content { get; private set; }

        public TemplateFunctionsMock TemplateFunctions { get; private set; }

        public CampaignItemTestBlastMock CampaignItemTestBlast { get; private set; }

        public CampaignItemMock CampaignItem { get; private set; }

        public BlastLinkMock BlastLink { get; private set; }

        public DataFunctionsMock DataFunctions { get; private set; }

        public CampaignItemSocialMediaMock CampaignItemSocialMedia { get; private set; }

        public SocialMediaMock SocialMedia { get; }

        public GroupBusinessMock GroupBusiness { get; }

        public SmartFormsHistoryMock SmartFormsHistory { get; }

        public SmartFormTrackingMock SmartFormTracking { get; }

        public UserBusinessMock UserBusiness { get; }

        public GroupDataFieldsMock GroupDataFields { get; }

        public EmailDirectMock EmailDirect { get; }

        public ResponseMock Response { get; }

        public void Dispose()
        {
            _shimsContext.Dispose();
            DisposeAllDisposableProperties();
        }

        private void DisposeAllDisposableProperties()
        {
            var disposableProeprties = GetType().GetProperties()
                            .Where(property => IsDisposable(property.PropertyType))
                            .ToList();
            foreach (var property in disposableProeprties)
            {
                var disposableObject = property.GetValue(this) as IDisposable;
                disposableObject?.Dispose();
            }
        }

        private bool IsDisposable(Type type)
        {
            return typeof(IDisposable).IsAssignableFrom(type);
        }

        private void ShimAppSettings()
        {
            AppSettings = new NameValueCollection();
            ShimConfigurationManager.AppSettingsGet = () =>
            {
                var originalAppSettings = ShimsContext.ExecuteWithoutShims(() => ConfigurationManager.AppSettings);
                var result = new NameValueCollection(originalAppSettings);
                result.Add(AppSettings);
                return result;
            };
        }
    }
}
