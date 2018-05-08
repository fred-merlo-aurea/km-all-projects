using EntityEnums = ECN_Framework_Common.Objects.Enums.Entity;

namespace EmailMarketing.API.Search
{
    public static class SearchConfiguration
    {
        private const string LowerThanOrEqualComparator = "<=";
        private const string GreaterThanOrEqualsComparator = ">=";
        private const string ContainsComparator = "contains";
        private const string EqualsComparator = "=";
        private const string TitleItem = "Title";
        private const string FolderIdItem = "FolderID";
        private const string UpdatedDateFromItem = "UpdatedDateFrom";
        private const string UpdatedDateToItem = "UpdatedDateTo";
        private const string ArchivedItem = "Archived";
        private const string LastUpdatedByUserItem = "LastUpdatedByUser";
        private const string ImageNameItem = "ImageName";
        private const string FolderNameItem = "FolderName";
        private const string RecursiveItem = "Recursive";
        private const string NameItem = "Name";
        private const string GroupIdItem = "GroupID";
        private const string BaseChannelIdItem = "BaseChannelID";
        private const string ParentIdItem = "ParentID";
        private const string TypeItem = "Type";
        private const string EmailSubjectItem = "EmailSubject";
        private const string IsTestItem = "IsTest";
        private const string StatusCodeItem = "StatusCode";
        private const string ModifiedFromItem = "ModifiedFrom";
        private const string ModifiedToItem = "ModifiedTo";
        private const string CampaignIdItem = "CampaignID";
        private const string CampaignNameItem = "CampaignName";
        private const string CampaignItemName = "CampaignItemName";
        private const string ContentGroup = "content";
        private const string MessageGroup = "message";
        private const string ImageGroup = "image";
        private const string ImageFolderGroup = "imagefolder";
        private const string Group = "group";
        private const string FilterGroup = "filter";
        private const string CustomFieldGroup = "customfield";
        private const string CustomerGroup = "customer";
        private const string FolderGroup = "folder";
        private const string SimpleBlastGroup = "simpleblast";
        private const string SimpleBlastV2Group = "simpleblastv2";

        /// <summary>
        /// Collection of controller search registrations, singleton.
        /// </summary>
        public static readonly SearchConfigurationLibrary Library;

        private static SearchConfigurationItem _titleConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = TitleItem,
            PropertyType = typeof(string),
            Comparators = new[] { ContainsComparator }
        };

        private static SearchConfigurationItem _folderIdConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = FolderIdItem,
            PropertyType = typeof(long),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _updatedDateFromConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = UpdatedDateFromItem,
            PropertyType = typeof(string),
            Comparators = new[] { GreaterThanOrEqualsComparator }
        };

        private static SearchConfigurationItem _updatedDateToConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = UpdatedDateToItem,
            PropertyType = typeof(string),
            Comparators = new[] { LowerThanOrEqualComparator }
        };

        private static SearchConfigurationItem _archivedConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = ArchivedItem,
            PropertyType = typeof(bool),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _lastUpdatedByUserConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = LastUpdatedByUserItem,
            PropertyType = typeof(long),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _imageNameConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = ImageNameItem,
            PropertyType = typeof(string),
            Comparators = new[] { EqualsComparator, ContainsComparator }
        };

        private static SearchConfigurationItem _folderNameConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = FolderNameItem,
            PropertyType = typeof(string),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _recursiveConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = RecursiveItem,
            PropertyType = typeof(bool),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _groupIdConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = GroupIdItem,
            PropertyType = typeof(long),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _nameConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = NameItem,
            PropertyType = typeof(string),
            Comparators = new[] { ContainsComparator }
        };

        private static SearchConfigurationItem _baseChannelIdConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = BaseChannelIdItem,
            PropertyType = typeof(long),
            Comparators = new[] { EqualsComparator },
            ValidationMethod = SearchConfigurationItem.BasicValidationMethods.DiscreteCriteria
        };

        private static SearchConfigurationItem _parentIdConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = ParentIdItem,
            PropertyType = typeof(long),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _typeConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = TypeItem,
            PropertyType = typeof(string),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _emailSubjectConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = EmailSubjectItem,
            PropertyType = typeof(string),
            Comparators = new[] { ContainsComparator }
        };

        private static SearchConfigurationItem _isTestConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = IsTestItem,
            PropertyType = typeof(bool),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _statusCodeConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = StatusCodeItem,
            PropertyType = typeof(string),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _modifiedFromConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = ModifiedFromItem,
            PropertyType = typeof(string),
            Comparators = new[] { GreaterThanOrEqualsComparator }
        };

        private static SearchConfigurationItem _modifiedToConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = ModifiedToItem,
            PropertyType = typeof(string),
            Comparators = new[] { LowerThanOrEqualComparator }
        };

        private static SearchConfigurationItem _campaignIdConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = CampaignIdItem,
            PropertyType = typeof(int),
            Comparators = new[] { EqualsComparator }
        };

        private static SearchConfigurationItem _campaignNameConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = CampaignNameItem,
            PropertyType = typeof(string),
            Comparators = new[] { ContainsComparator, EqualsComparator }
        };

        private static SearchConfigurationItem _campaignItemNameConfigurationItem = new SearchConfigurationItem
        {
            PropertyName = CampaignItemName,
            PropertyType = typeof(string),
            Comparators = new[] { ContainsComparator, EqualsComparator }
        };

        private static SearchConfigurationGroup _contentConfigurationGroup = new SearchConfigurationGroup(false, EntityEnums.Content)
        {
            [_titleConfigurationItem.PropertyName] = _titleConfigurationItem,
            [_folderIdConfigurationItem.PropertyName] = _folderIdConfigurationItem,
            [_updatedDateFromConfigurationItem.PropertyName] = _updatedDateFromConfigurationItem,
            [_updatedDateToConfigurationItem.PropertyName] = _updatedDateToConfigurationItem,
            [_archivedConfigurationItem.PropertyName] = _archivedConfigurationItem
        };

        private static SearchConfigurationGroup _messageConfigurationGroup = new SearchConfigurationGroup(false, EntityEnums.Layout)
        {
            [_titleConfigurationItem.PropertyName] = _titleConfigurationItem,
            [_folderIdConfigurationItem.PropertyName] = _folderIdConfigurationItem,
            [_updatedDateFromConfigurationItem.PropertyName] = _updatedDateFromConfigurationItem,
            [_updatedDateToConfigurationItem.PropertyName] = _updatedDateToConfigurationItem,
            [_lastUpdatedByUserConfigurationItem.PropertyName] = _lastUpdatedByUserConfigurationItem,
            [_archivedConfigurationItem.PropertyName] = _archivedConfigurationItem
        };

        private static SearchConfigurationGroup _imageConfigurationGroup = new SearchConfigurationGroup(true)
        {
            [_imageNameConfigurationItem.PropertyName] = _imageNameConfigurationItem,
            [_folderNameConfigurationItem.PropertyName] = _folderNameConfigurationItem,
            [_recursiveConfigurationItem.PropertyName] = _recursiveConfigurationItem
        };

        private static SearchConfigurationGroup _imageFolderConfigurationGroup = new SearchConfigurationGroup(true)
        {
            [_folderNameConfigurationItem.PropertyName] = _folderNameConfigurationItem,
            [_recursiveConfigurationItem.PropertyName] = _recursiveConfigurationItem
        };

        private static SearchConfigurationGroup _groupConfigurationGroup = new SearchConfigurationGroup(true, EntityEnums.Group)
        {
            [_nameConfigurationItem.PropertyName] = _nameConfigurationItem,
            [_folderIdConfigurationItem.PropertyName] = _folderIdConfigurationItem,
            [_archivedConfigurationItem.PropertyName] = _archivedConfigurationItem
        };

        private static SearchConfigurationGroup _filterConfigurationGroup = new SearchConfigurationGroup(false, EntityEnums.Filter)
        {
            [_groupIdConfigurationItem.PropertyName] = _groupIdConfigurationItem,
            [_archivedConfigurationItem.PropertyName] = _archivedConfigurationItem
        };

        private static SearchConfigurationGroup _customFieldConfigurationGroup = new SearchConfigurationGroup(false, EntityEnums.GroupDataFields)
        {
            [_groupIdConfigurationItem.PropertyName] = _groupIdConfigurationItem
        };

        private static SearchConfigurationGroup _customerConfigurationGroup = new SearchConfigurationGroup(false, EntityEnums.Customer)
        {
            [_baseChannelIdConfigurationItem.PropertyName] = _baseChannelIdConfigurationItem
        };

        private static SearchConfigurationGroup _folderConfigurationGroup = new SearchConfigurationGroup(false, EntityEnums.Folder)
        {
            [_parentIdConfigurationItem.PropertyName] = _parentIdConfigurationItem,
            [_typeConfigurationItem.PropertyName] = _typeConfigurationItem
        };

        private static SearchConfigurationGroup _simpleBlastConfigurationGroup = new SearchConfigurationGroup(true, EntityEnums.Folder)
        {
            [_emailSubjectConfigurationItem.PropertyName] = _emailSubjectConfigurationItem,
            [_isTestConfigurationItem.PropertyName] = _isTestConfigurationItem,
            [_statusCodeConfigurationItem.PropertyName] = _statusCodeConfigurationItem,
            [_modifiedFromConfigurationItem.PropertyName] = _modifiedFromConfigurationItem,
            [_modifiedToConfigurationItem.PropertyName] = _modifiedToConfigurationItem,
            [_groupIdConfigurationItem.PropertyName] = _groupIdConfigurationItem,
            [_campaignIdConfigurationItem.PropertyName] = _campaignIdConfigurationItem,
            [_campaignNameConfigurationItem.PropertyName] = _campaignNameConfigurationItem,
            [_campaignItemNameConfigurationItem.PropertyName] = _campaignItemNameConfigurationItem
        };

        private static SearchConfigurationGroup _simpleBlastV2ConfigurationGroup = new SearchConfigurationGroup(true, EntityEnums.Folder)
        {
            [_emailSubjectConfigurationItem.PropertyName] = _emailSubjectConfigurationItem,
            [_isTestConfigurationItem.PropertyName] = _isTestConfigurationItem,
            [_statusCodeConfigurationItem.PropertyName] = _statusCodeConfigurationItem,
            [_modifiedFromConfigurationItem.PropertyName] = _modifiedFromConfigurationItem,
            [_modifiedToConfigurationItem.PropertyName] = _modifiedToConfigurationItem,
            [_groupIdConfigurationItem.PropertyName] = _groupIdConfigurationItem,
            [_campaignIdConfigurationItem.PropertyName] = _campaignIdConfigurationItem,
            [_campaignNameConfigurationItem.PropertyName] = _campaignNameConfigurationItem,
            [_campaignItemNameConfigurationItem.PropertyName] = _campaignItemNameConfigurationItem
        };

        static SearchConfiguration()
        {
            Library = new SearchConfigurationLibrary
            {
                [ContentGroup] = _contentConfigurationGroup,
                [MessageGroup] = _messageConfigurationGroup,
                [ImageGroup] = _imageConfigurationGroup,
                [ImageFolderGroup] = _imageFolderConfigurationGroup,
                [Group] = _groupConfigurationGroup,
                [FilterGroup] = _filterConfigurationGroup,
                [CustomFieldGroup] = _customFieldConfigurationGroup,
                [CustomerGroup] = _customerConfigurationGroup,
                [FolderGroup] = _folderConfigurationGroup,
                [SimpleBlastGroup] = _simpleBlastConfigurationGroup,
                [SimpleBlastV2Group] = _simpleBlastV2ConfigurationGroup,
            };
        }
    }
}