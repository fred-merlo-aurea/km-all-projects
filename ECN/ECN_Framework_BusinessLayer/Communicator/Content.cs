using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml;
using HtmlAgilityPack;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;
using KM.Common.Functions;
using CommonStringFunctions = KM.Common.StringFunctions;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Content
    {
        private const string ContentTypeHtml = "html";
        private const string ContentTypeText = "text";
        private const string ContentTypeFeed = "feed";
        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Content;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Content;

        public static bool CreatedUserExists(int userID)
        {
            List<ECN_Framework_Entities.Communicator.Content> contentList = new List<ECN_Framework_Entities.Communicator.Content>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                bool result = ECN_Framework_DataLayer.Communicator.Content.CreatedUserExists(userID);
                scope.Complete();
                return result;
            }
        }

        public static bool HasPermission(KMPlatform.Enums.Access AccessCode, KMPlatform.Entity.User user)
        {
            if (AccessCode == KMPlatform.Enums.Access.View)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Content.ToString()) ||
                //    KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Content.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                    return true;
            }
            else if (AccessCode == KMPlatform.Enums.Access.Edit)
            {
                //if (KMPlatform.BusinessLogic.User.HasPermission(user.UserID, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Create_Content.ToString()))
                if (KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                    return true;
            }
            return false;
        }

        public static List<ECN_Framework_Entities.Communicator.Content> GetByFolderID(int folderID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.Content> contentList = new List<ECN_Framework_Entities.Communicator.Content>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentList = ECN_Framework_DataLayer.Communicator.Content.GetByFolderID(folderID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(contentList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (contentList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Content content in contentList)
                {
                    content.FilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID(content.ContentID, user, getChildren);
                    content.AliasList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID(content.ContentID, user, getChildren);
                    //find and attach any dynamic tags
                    System.Collections.Generic.List<string> toParse = new List<string>();
                    toParse.Add(content.ContentSource);
                    toParse.Add(content.ContentText);
                    toParse.Add(content.ContentMobile);
                    List<string> tagNameList = GetTags(toParse);
                    if (tagNameList.Count > 0)
                    {
                        List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                        ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                        foreach (string tagName in tagNameList)
                        {
                            tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag(tagName, user, getChildren);
                            tagList.Add(tag);
                        }
                        content.DynamicTagList = tagList;
                    }
                }
            }

            return contentList;
        }

        public static List<ECN_Framework_Entities.Communicator.Content> GetByFolderIDCustomerID(int folderID, KMPlatform.Entity.User user, bool getChildren, string archiveFilter = "all")
        {
            List<ECN_Framework_Entities.Communicator.Content> contentList = new List<ECN_Framework_Entities.Communicator.Content>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentList = ECN_Framework_DataLayer.Communicator.Content.GetByFolderIDCustomerID(folderID, user.CustomerID, archiveFilter);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(contentList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (contentList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Content content in contentList)
                {
                    content.FilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID(content.ContentID, user, getChildren);
                    content.AliasList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID(content.ContentID, user, getChildren);
                    //find and attach any dynamic tags
                    System.Collections.Generic.List<string> toParse = new List<string>();
                    toParse.Add(content.ContentSource);
                    toParse.Add(content.ContentText);
                    toParse.Add(content.ContentMobile);
                    List<string> tagNameList = GetTags(toParse);
                    if (tagNameList.Count > 0)
                    {
                        List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                        ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                        foreach (string tagName in tagNameList)
                        {
                            tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag(tagName, user, getChildren);
                            tagList.Add(tag);
                        }
                        content.DynamicTagList = tagList;
                    }
                }
            }

            return contentList;
        }

        public static List<ECN_Framework_Entities.Communicator.Content> GetByLayoutID(int layoutID, KMPlatform.Entity.User user, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.Content> contentList = new List<ECN_Framework_Entities.Communicator.Content>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentList = ECN_Framework_DataLayer.Communicator.Content.GetByLayoutID(layoutID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(contentList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (contentList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Content content in contentList)
                {
                    content.FilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID(content.ContentID, user, getChildren);
                    content.AliasList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID(content.ContentID, user, getChildren);
                    //find and attach any dynamic tags
                    System.Collections.Generic.List<string> toParse = new List<string>();
                    toParse.Add(content.ContentSource);
                    toParse.Add(content.ContentText);
                    toParse.Add(content.ContentMobile);
                    List<string> tagNameList = GetTags(toParse);
                    if (tagNameList.Count > 0)
                    {
                        List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                        ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                        foreach (string tagName in tagNameList)
                        {
                            tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag(tagName, user, getChildren);
                            tagList.Add(tag);
                        }
                        content.DynamicTagList = tagList;
                    }
                }
            }

            return contentList;
        }

        public static List<ECN_Framework_Entities.Communicator.Content> GetByLayoutID_NoAccessCheck(int layoutID, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.Content> contentList = new List<ECN_Framework_Entities.Communicator.Content>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentList = ECN_Framework_DataLayer.Communicator.Content.GetByLayoutID(layoutID);
                scope.Complete();
            }

            if (contentList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Content content in contentList)
                {
                    content.FilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID_NoAccessCheck(content.ContentID, getChildren);
                    content.AliasList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID_NoAccessCheck(content.ContentID, getChildren);
                    //find and attach any dynamic tags
                    System.Collections.Generic.List<string> toParse = new List<string>();
                    toParse.Add(content.ContentSource);
                    toParse.Add(content.ContentText);
                    toParse.Add(content.ContentMobile);
                    List<string> tagNameList = GetTags(toParse);
                    if (tagNameList.Count > 0)
                    {
                        List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                        ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                        foreach (string tagName in tagNameList)
                        {
                            tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag_NoAccessCheck(tagName, content.CustomerID.Value, getChildren);
                            tagList.Add(tag);
                        }
                        content.DynamicTagList = tagList;
                    }
                }
            }

            return contentList;
        }

        public static bool GetValidatedStatusByContentID_NoAccessCheck(int contentID)
        {
            bool ValidatedStatus = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ValidatedStatus = ECN_Framework_DataLayer.Communicator.Content.GetValidatedStatusByContentID(contentID);
                scope.Complete();
            }
            return ValidatedStatus;
        }
        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.Content GetByContentID_NoAccessCheck(int contentID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Content content = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                content = ECN_Framework_DataLayer.Communicator.Content.GetByContentID(contentID);
                scope.Complete();
            }


            if (content != null && getChildren)
            {
                content.FilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID_NoAccessCheck(content.ContentID, getChildren);
                content.AliasList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID_NoAccessCheck(content.ContentID, getChildren);
                //find and attach any dynamic tags
                System.Collections.Generic.List<string> toParse = new List<string>();
                toParse.Add(content.ContentSource);
                toParse.Add(content.ContentText);
                toParse.Add(content.ContentMobile);
                List<string> tagNameList = GetTags(toParse);
                if (tagNameList.Count > 0)
                {
                    List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                    ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                    foreach (string tagName in tagNameList)
                    {
                        tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag_NoAccessCheck(tagName, content.CustomerID.Value, getChildren);
                        tagList.Add(tag);
                    }
                    content.DynamicTagList = tagList;
                }
            }

            return content;
        }

        public static ECN_Framework_Entities.Communicator.Content GetByContentID_NoAccessCheck_UseAmbientTransaction(int contentID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Content content = null;
            using (TransactionScope scope = new TransactionScope())
            {
                content = ECN_Framework_DataLayer.Communicator.Content.GetByContentID(contentID);
                scope.Complete();
            }


            if (content != null && getChildren)
            {
                content.FilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID_NoAccessCheck_UseAmbientTransaction(content.ContentID, getChildren);
                content.AliasList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID_NoAccessCheck_UseAmbientTransaction(content.ContentID, getChildren);
                //find and attach any dynamic tags
                System.Collections.Generic.List<string> toParse = new List<string>();
                toParse.Add(content.ContentSource);
                toParse.Add(content.ContentText);
                toParse.Add(content.ContentMobile);
                List<string> tagNameList = GetTags(toParse);
                if (tagNameList.Count > 0)
                {
                    List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                    ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                    foreach (string tagName in tagNameList)
                    {
                        tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag_NoAccessCheck_UseAmbientTransaction(tagName, content.CustomerID.Value, getChildren);
                        tagList.Add(tag);
                    }
                    content.DynamicTagList = tagList;
                }
            }

            return content;
        }

        public static ECN_Framework_Entities.Communicator.Content GetByContentID(int contentID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Content content = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                content = ECN_Framework_DataLayer.Communicator.Content.GetByContentID(contentID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(content, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (content != null && getChildren)
            {
                content.FilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID(content.ContentID, user, getChildren);
                content.AliasList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID(content.ContentID, user, getChildren);
                //find and attach any dynamic tags
                System.Collections.Generic.List<string> toParse = new List<string>();
                toParse.Add(content.ContentSource);
                toParse.Add(content.ContentText);
                toParse.Add(content.ContentMobile);
                List<string> tagNameList = GetTags(toParse);
                if (tagNameList.Count > 0)
                {
                    List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                    ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                    foreach (string tagName in tagNameList)
                    {
                        tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag(tagName, user, getChildren);
                        tagList.Add(tag);
                    }
                    content.DynamicTagList = tagList;
                }
            }

            return content;
        }

        public static ECN_Framework_Entities.Communicator.Content GetByContentID_UseAmbientTransaction(int contentID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Content content = null;
            using (TransactionScope scope = new TransactionScope())
            {
                content = ECN_Framework_DataLayer.Communicator.Content.GetByContentID(contentID);
                scope.Complete();
            }

            if (!HasPermission(KMPlatform.Enums.Access.View, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(content, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (content != null && getChildren)
            {
                content.FilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID_UseAmbientTransaction(content.ContentID, user, getChildren);
                content.AliasList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID_UseAmbientTransaction(content.ContentID, user, getChildren);
                //find and attach any dynamic tags
                System.Collections.Generic.List<string> toParse = new List<string>();
                toParse.Add(content.ContentSource);
                toParse.Add(content.ContentText);
                toParse.Add(content.ContentMobile);
                List<string> tagNameList = GetTags(toParse);
                if (tagNameList.Count > 0)
                {
                    List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                    ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                    foreach (string tagName in tagNameList)
                    {
                        tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag_UseAmbientTransaction(tagName, user, getChildren);
                        tagList.Add(tag);
                    }
                    content.DynamicTagList = tagList;
                }
            }

            return content;
        }

        public static List<string> GetTags(System.Collections.Generic.List<string> toParse, bool fullTag = false)
        {
            List<string> tagNameList = new List<string>();
            foreach (string s in toParse)
            {
                System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("ECN.DynamicTag", System.Text.RegularExpressions.RegexOptions.IgnoreCase | RegexOptions.Compiled);
                System.Text.RegularExpressions.MatchCollection MatchList = regMatch.Matches(s);
                if (MatchList.Count > 0)
                {
                    if ((MatchList.Count % 2) != 0)
                    {
                        List<ECNError> errorList = new List<ECNError>();
                        errorList.Add(new ECNError(Enums.Entity.Content, Enums.Method.Validate, "Invalid Dynamic tags"));
                        throw new ECNException(errorList, Enums.ExceptionLayer.Business);
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regMatchGood = new System.Text.RegularExpressions.Regex("ECN.DynamicTag.[a-zA-Z0-9_]+?.ECN.DynamicTag", System.Text.RegularExpressions.RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
                        System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(s);
                        if ((MatchList.Count / 2) > MatchListGood.Count)
                        {

                            List<ECNError> errorList = new List<ECNError>();
                            errorList.Add(new ECNError(Enums.Entity.Content, Enums.Method.Validate, "Invalid Dynamic tags"));
                            throw new ECNException(errorList, Enums.ExceptionLayer.Business);
                        }
                        else
                        {
                            foreach (System.Text.RegularExpressions.Match m in MatchListGood)
                            {
                                if (!string.IsNullOrEmpty(m.Value.ToString()))
                                {
                                    string tempTagName = m.Value.ToString();
                                    if (!fullTag)
                                    {
                                        tempTagName = tempTagName.Replace("ECN.DynamicTag.", string.Empty);
                                        tempTagName = tempTagName.Replace(".ECN.DynamicTag", string.Empty);
                                    }
                                    if (!tagNameList.Contains(tempTagName))
                                        tagNameList.Add(tempTagName);
                                }
                            }
                        }
                    }
                }
            }

            return tagNameList;
        }

        public static bool Exists(int contentID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Content.Exists(contentID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int contentID, string contentTitle, int folderID, int customerID)//if contentid is not null we will exclude it
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Content.Exists(contentID, contentTitle, folderID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(string contentTitle, int folderID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Content.Exists(contentTitle, folderID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ValidateHTMLTags(string html)
        {
            Regex rgBodyEnd = new Regex("</body.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgBodyStart = new Regex("<body.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgHeadStart = new Regex("<head.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgHeadEnd = new Regex("</head.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgHTMLEnd = new Regex("</html.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgHTMLStart = new Regex("<html.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (rgHTMLStart.IsMatch(html) && !rgHTMLEnd.IsMatch(html))
            {
                return false;
            }

            if (rgHeadStart.IsMatch(html) && !rgHeadEnd.IsMatch(html))
            {
                return false;
            }

            if (rgBodyStart.IsMatch(html) && !rgBodyEnd.IsMatch(html))
            {
                return false;
            }

            return true;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.Content content, KMPlatform.Entity.User user)
        {
            var method = Enums.Method.Validate;
            var errorList = new List<ECNError>();

            if (string.IsNullOrWhiteSpace(content.ContentTitle))
            {
                errorList.Add(new ECNError(Entity, method, "ContentTitle is invalid"));
            }

            if (content.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, method, "CustomerID is invalid"));
            }
            else
            {
                CheckForCustomerInfo(content, user, method, errorList);
            }

            if (!string.Equals(content.LockedFlag.Trim(), "Y", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(content.LockedFlag.Trim(), "N", StringComparison.OrdinalIgnoreCase))
            {
                errorList.Add(new ECNError(Entity, method, "LockedFlag is invalid"));
            }

            CheckForContentStrings(content, method, errorList);

            // Validating RSS Feeds here as well
            ValidateRssFeed(content.ContentSource, content.CustomerID, errorList);
            ValidateRssFeed(content.ContentText, content.CustomerID, errorList);
            ValidateRssFeed(content.ContentMobile, content.CustomerID, errorList);

            CheckForInvalidTags(content, user, method, errorList);
            
            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }

            CheckForBadContent(content);
            CheckLinkLength(content);
        }

        private static void CheckForCustomerInfo(ECN_Framework_Entities.Communicator.Content content, KMPlatform.Entity.User user, Enums.Method method, List<ECNError> errorList)
        {
            if (content.FolderID == null || (content.FolderID > 0 && !Folder.Exists(content.FolderID.Value, content.CustomerID.Value, FolderTypes.CNT)))
            {
                errorList.Add(new ECNError(Entity, method, "FolderID is invalid"));
            }
            else if (string.IsNullOrWhiteSpace(content.ContentTitle))
            {
                errorList.Add(new ECNError(Entity, method, "ContentTitle cannot be empty"));
            }
            else if (Exists(content.ContentID, content.ContentTitle, content.FolderID.Value, content.CustomerID.Value))
            {
                errorList.Add(new ECNError(Entity, method, "ContentTitle already exists in this folder"));
            }

            if (!RegexUtilities.IsValidObjectName(content.ContentTitle))
            {
                errorList.Add(new ECNError(Entity, method, "ContentTitle has invalid characters"));
            }

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                if (!Accounts.Customer.Exists(content.CustomerID.Value))
                {
                    errorList.Add(new ECNError(Entity, method, "CustomerID is invalid"));
                }

                if (content.CreatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(content.CreatedUserID.Value, false))
                    && content.ContentID <= 0 && (content.CreatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(content.CreatedUserID.Value, content.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, method, "CreatedUserID is invalid"));
                }

                if (content.ContentID > 0 && (content.UpdatedUserID == null || (content.UpdatedUserID.HasValue && !KM.Platform.User.IsSystemAdministrator(KMPlatform.BusinessLogic.User.GetByUserID(content.UpdatedUserID.Value, false))))
                    && content.ContentID > 0 && (content.UpdatedUserID == null || !KMPlatform.BusinessLogic.User.Exists(content.UpdatedUserID.Value, content.CustomerID.Value)))
                {
                    errorList.Add(new ECNError(Entity, method, "UpdatedUserID is invalid"));
                }

                if (content.Sharing.Trim().Length == 0 || (!string.Equals(content.Sharing.Trim(), "N", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(content.Sharing.Trim(), "Y", StringComparison.OrdinalIgnoreCase)))
                {
                    errorList.Add(new ECNError(Entity, method, "Sharing is invalid"));
                }

                else if (string.Equals(content.Sharing.Trim(), "Y", StringComparison.OrdinalIgnoreCase)
                    && ((!KM.Platform.User.IsChannelAdministrator(user) && !KM.Platform.User.IsSystemAdministrator(user) && !KM.Platform.User.IsAdministrator(user)) || (!KMPlatform.BusinessLogic.Client.HasServiceFeature(user.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ContentSharing))))
                {
                    errorList.Add(new ECNError(Entity, method, "Sharing is invalid"));
                }

                scope.Complete();
            }
        }

        private static void CheckForContentStrings(ECN_Framework_Entities.Communicator.Content content, Enums.Method method, List<ECNError> errorList)
        {
            if (content.ContentTypeCode.Trim() == ContentTypeCode.TEXT.ToString() && string.IsNullOrWhiteSpace(content.ContentText))
            {
                errorList.Add(new ECNError(Entity, method, "ContentText is invalid"));
            }
            if (content.ContentTypeCode.Trim() == ContentTypeCode.HTML.ToString() && string.IsNullOrWhiteSpace(content.ContentSource))
            {
                errorList.Add(new ECNError(Entity, method, "ContentSource is invalid"));
            }
            if (content.ContentTypeCode.Trim() == ContentTypeCode.HTML.ToString() && string.IsNullOrWhiteSpace(content.ContentMobile))
            {
                errorList.Add(new ECNError(Entity, method, "ContentMobile is invalid"));
            }
            if (content.ContentTypeCode.Trim() == ContentTypeCode.SMS.ToString() && string.IsNullOrWhiteSpace(content.ContentSMS))
            {
                errorList.Add(new ECNError(Entity, method, "ContentSMS is invalid"));
            }
        }

        private static void ValidateRssFeed(string content, int? customerId, List<ECNError> errorList)
        {
            // TOG followed by a literal dot 
            // followed by as few as possible of any character (captured as match.Group[1])
            // followed by a literal dot and then TAG, again
            var pattern = new Regex(
                                @"ECN.RSSFEED\.(.*?)\.ECN.RSSFEED",
                                RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (Match match in pattern.Matches(content))
            {
                var feedName = match.Groups[1].Value;
                var rssFeed = RSSFeed.GetByFeedName(feedName, customerId.Value);
                try
                {
                    RSSFeed.Verify(rssFeed.URL);
                }
                catch (ECNException ecnE)
                {
                    foreach (ECNError error in ecnE.ErrorList)
                    {
                        if (!errorList.Exists(x => x.Entity == error.Entity && x.ErrorMessage.Contains(feedName)))
                        {
                            error.ErrorMessage += " \"" + feedName + "\"";
                            errorList.Add(error);
                        }
                    }
                }
            }
        }

        private static void CheckForInvalidTags(ECN_Framework_Entities.Communicator.Content content, KMPlatform.Entity.User user, Enums.Method method, List<ECNError> errorList)
        {
            var toParse = new List<string>
            {
                content.ContentSource,
                content.ContentText,
                content.ContentMobile
            };

            var tagNameList = GetTags(toParse);
            if (tagNameList.Count > 0)
            {
                foreach (var tagName in tagNameList)
                {
                    var tag = DynamicTag.GetByTag(tagName, user, false);
                    if (tag == null)
                    {
                        errorList.Add(new ECNError(Entity, method, "Invalid Dynamic Tag - " + tagName));
                        break;
                    }
                }
            }
        }

        public static void ReadyContent(ECN_Framework_Entities.Communicator.Content content, bool keepComments)
        {
            if (content.ContentSource.Trim().Length > 0)
            {
                content.ContentSource = CommonStringFunctions.CleanString(content.ContentSource.Trim());
            }
            else
            {
                content.ContentSource = CommonStringFunctions.CleanString(content.ContentText.Trim());
            }

            if (content.ContentMobile.Trim().Length > 0)
            {
                content.ContentMobile = CommonStringFunctions.CleanString(content.ContentMobile.Trim());

            }
            else
            {
                content.ContentMobile = content.ContentSource;
            }

            content.ContentSource = content.ContentSource.Replace("<tbody>", "");
            content.ContentSource = content.ContentSource.Replace("</tbody>", "");
            content.ContentSource = content.ContentSource.Replace("<TBODY>", "");
            content.ContentSource = content.ContentSource.Replace("</TBODY>", "");
            content.ContentSource = content.ContentSource.Replace("<tbody>", "");
            content.ContentSource = content.ContentSource.Replace("</tbody>", "");

            content.ContentMobile = content.ContentMobile.Replace("<tbody>", "");
            content.ContentMobile = content.ContentMobile.Replace("</tbody>", "");
            content.ContentMobile = content.ContentMobile.Replace("<TBODY>", "");
            content.ContentMobile = content.ContentMobile.Replace("</TBODY>", "");
            content.ContentMobile = content.ContentMobile.Replace("<tbody>", "");
            content.ContentMobile = content.ContentMobile.Replace("</tbody>", "");

            //Remove all the Comments in HTML <!-- -->
            if (!keepComments)
            {
                content.ContentSource = HtmlFunctions.StripHtmlComments(content.ContentSource);
                content.ContentMobile = HtmlFunctions.StripHtmlComments(content.ContentMobile);
            }

            content.ContentSource = ReplaceAnchor(content.ContentSource);
            content.ContentMobile = ReplaceAnchor(content.ContentMobile);

            if (content.ContentText.Trim().Length > 0)
            {
                content.ContentText = CommonStringFunctions.CleanString(content.ContentText.Trim());
            }
            else
            {
                content.ContentText = CommonStringFunctions.CleanString(HtmlFunctions.StripTextFromHtml(content.ContentSource));
            }

            content.ContentText = ReplaceAnchor(content.ContentText);

            content.ContentTitle = CommonStringFunctions.CleanString(content.ContentTitle.Trim());
        }

        private static string ReplaceAnchor(string str)
        {
            return CommonStringFunctions.ReplaceAnchor(str);
        }

        public static bool TopicParamExists(ECN_Framework_Entities.Communicator.Content content)
        {
            string strText = content.ContentSource;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(strText);
            HtmlAgilityPack.HtmlNodeCollection nodesToCheck = doc.DocumentNode.SelectNodes("//a[contains(@href,'topic=') ]");
            if (nodesToCheck != null)
            {
                return true;
            }
            strText = content.ContentMobile;
            doc.LoadHtml(strText);
            nodesToCheck = doc.DocumentNode.SelectNodes("//a[contains(@href,'topic=') ]");
            if (nodesToCheck != null)
            {
                return true;
            }
            return CorTopicParamTextOnly(content.ContentText);
        }

        private static readonly Regex EmbeddedUrlPattern = new Regex(@"\[url:(.*?topic=.*?)\]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static bool CorTopicParamTextOnly(string textToProcess)
        {
            var theUrl = "";
            var match = EmbeddedUrlPattern.Match(textToProcess);
            if (null != match)
            {
                theUrl = match.Groups[1].Value.Trim();
            }
            return theUrl.ToUpper().Contains("TOPIC=");
        }

        private static void CheckForBadContent(ECN_Framework_Entities.Communicator.Content content)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (content.ContentSource.ToLower().Contains("http://emailactivity.com/engines/linkfrom.aspx") ||
                content.ContentText.ToLower().Contains("http://emailactivity.com/engines/linkfrom.aspx") ||
                content.ContentMobile.ToLower().Contains("http://emailactivity.com/engines/linkfrom.aspx"))
            {
                errorList.Add(new ECNError(Entity, Method, "Invalid Email Activity URL in content"));
            }
            if (content.ContentSource.ToLower().Contains("http://emailactivity.ecn5.com/engines/open.aspx") ||
                content.ContentText.ToLower().Contains("http://emailactivity.ecn5.com/engines/open.aspx") ||
                content.ContentMobile.ToLower().Contains("http://emailactivity.ecn5.com/engines/open.aspx"))
            {
                errorList.Add(new ECNError(Entity, Method, "Invalid Email Activity URL in content"));
            }
            if (content.ContentSource.ToLower().Contains("http://tracking.ecn5.com/engines/linkfrom.aspx") ||
                content.ContentText.ToLower().Contains("http://tracking.ecn5.com/engines/linkfrom.aspx") ||
                content.ContentMobile.ToLower().Contains("http://tracking.ecn5.com/engines/linkfrom.aspx"))
            {
                errorList.Add(new ECNError(Entity, Method, "Invalid Email Activity URL in content"));
            }
            if (content.ContentSource.ToLower().Contains("http://tracking.ecn5.com/engines/open.aspx") ||
                content.ContentText.ToLower().Contains("http://tracking.ecn5.com/engines/open.aspx") ||
                content.ContentMobile.ToLower().Contains("http://tracking.ecn5.com/engines/open.aspx"))
            {
                errorList.Add(new ECNError(Entity, Method, "Invalid Email Activity URL in content"));
            }
            if (content.ContentSource.ToLower().Contains("http://email.ecn5.com/engines/linkfrom.aspx") ||
                content.ContentText.ToLower().Contains("http://email.ecn5.com/engines/linkfrom.aspx") ||
                content.ContentMobile.ToLower().Contains("http://email.ecn5.com/engines/linkfrom.aspx"))
            {
                errorList.Add(new ECNError(Entity, Method, "Invalid Email Activity URL in content"));
            }
            if (content.ContentSource.ToLower().Contains("http://email.ecn5.com/engines/open.aspx") ||
                content.ContentText.ToLower().Contains("http://email.ecn5.com/engines/open.aspx") ||
                content.ContentMobile.ToLower().Contains("http://email.ecn5.com/engines/open.aspx"))
            {
                errorList.Add(new ECNError(Entity, Method, "Invalid Email Activity URL in content"));
            }

            string[] toValidate = { content.ContentSource, content.ContentMobile, content.ContentText };
            if (!ValidateCodeSnippets(toValidate))
                errorList.Add(new ECNError(Entity, Method, "Badly formed codesnippet in content"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        private static void CheckLinkLength(ECN_Framework_Entities.Communicator.Content content)
        {
            //Method now checks for link query parameters that are reserved 11/19/2013 JWelter
            List<ECNError> errorList = new List<ECNError>();
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content.ContentSource);
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//a[@href]");
            System.Collections.Generic.List<string> linkList = new System.Collections.Generic.List<string>();
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    string href = "";
                    HtmlAgilityPack.HtmlAttribute attHREF = node.Attributes["href"];
                    if (attHREF != null)
                    {
                        href = attHREF.Value;
                    }
                    linkList.Add(href);
                }
            }

            nodes = doc.DocumentNode.SelectNodes("//area[@href]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    string href = "";
                    HtmlAgilityPack.HtmlAttribute attHREF = node.Attributes["href"];
                    if (attHREF != null)
                    {
                        href = attHREF.Value;
                    }
                    linkList.Add(href);
                }
            }

            int maxLength = 1792;
            for (int aLoop = 0; aLoop < linkList.Count; aLoop++)
            {
                if (linkList[aLoop].Trim().Length > maxLength)
                {
                    errorList.Add(new ECNError(Entity, Method, "URL max length exceeded, must be less than " + maxLength.ToString()));
                    throw new ECNException(errorList);
                }

                //string reservedCheck = string.Empty;
                //try
                //{
                //    reservedCheck = linkList[aLoop].Trim().Split('?')[1];
                //}
                //catch
                //{
                //}

                //if (reservedCheck.Contains("b=") || reservedCheck.Contains("lid=") || reservedCheck.Contains("e=") || reservedCheck.Contains("l="))
                //{
                //    errorList.Add(new ECNError(Entity, Method, "URL contains reserved query parameters"));
                //    throw new ECNException(errorList);
                //}

            }
        }

        private static bool ValidateCodeSnippets(string[] toValidate)
        {
            foreach (string s in toValidate)
            {
                //Bad snippets - catches odd number of double % and catches non-alpha, non-numeric between the sets of double %
                System.Text.RegularExpressions.Regex regMatch = new System.Text.RegularExpressions.Regex("%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase | RegexOptions.Compiled);
                System.Text.RegularExpressions.MatchCollection MatchList = regMatch.Matches(s);
                if (MatchList.Count > 0)
                {
                    if ((MatchList.Count % 2) != 0)
                    {
                        return false;
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex regMatchGood = new System.Text.RegularExpressions.Regex("%%[a-zA-Z0-9_]+?%%", System.Text.RegularExpressions.RegexOptions.IgnoreCase | RegexOptions.Compiled);
                        System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(s);
                        if ((MatchList.Count / 2) > MatchListGood.Count)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static int Save(ECN_Framework_Entities.Communicator.Content content, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new SecurityException();

            if (content.ContentID > 0)
            {
                if (!Exists(content.ContentID, content.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "ContentID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(content, user);


            //Commenting this out for now JWelter 8/18/2015

            //Removing comments and CDATA sections
            //Regex regComments = new Regex("<!--.*?-->", RegexOptions.Singleline);
            //content.ContentSource = regComments.Replace(content.ContentSource, "");
            //content.ContentMobile = regComments.Replace(content.ContentMobile, "");

            //Regex regCData = new Regex(@"\<\!\[CDATA\[.*?\]\]\>", RegexOptions.Singleline);
            //content.ContentSource = regCData.Replace(content.ContentSource, "");
            //content.ContentMobile = regCData.Replace(content.ContentMobile, "");


            content.ContentSource = CreateUniqueLinkIDs(content.ContentSource);
            content.ContentMobile = CreateUniqueLinkIDs(content.ContentMobile);


            using (TransactionScope scope = new TransactionScope())
            {
                content.ContentID = ECN_Framework_DataLayer.Communicator.Content.Save(content);
                if (content.FilterList != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.ContentFilter filter in content.FilterList)
                    {
                        filter.ContentID = content.ContentID;
                        ContentFilter.Save(filter, user);
                    }
                }
                if (content.AliasList != null)
                {
                    foreach (ECN_Framework_Entities.Communicator.LinkAlias linkAlias in content.AliasList)
                    {
                        linkAlias.ContentID = content.ContentID;
                        LinkAlias.Save(linkAlias, user);
                    }
                }
                scope.Complete();
            }
            return content.ContentID;
        }

        private static string CreateUniqueLinkIDs(string content)
        {
            StringBuilder retContent = new StringBuilder();
            RegexOptions ro = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
            List<string> ecnIDs = new List<string>();
            MatchCollection mc = Regex.Matches(content, "<((?!link)[^>]*)href=[\\s\\S]*?([\"'])(.*?)\\2([^>]*)>", ro);

            Regex ecnIdRe = new Regex(@"ecn_id=([""']?)(.*?)\1", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Func<Match, string> getEcnId = (linkMatch) =>
            {
                string toSearch = String.Join(" ", (linkMatch.Groups[1].Value ?? ""), (linkMatch.Groups[4].Value ?? ""));
                Match ecnIdMatch = ecnIdRe.Match(toSearch);
                return ecnIdMatch.Success ? ecnIdMatch.Groups[2].Value : null;
            };

            for (int i = mc.Count; i > 0; i--)
            {

                int currentIndex = i - 1;
                string originalA = "";
                string ecn_ID = getEcnId(mc[currentIndex]);
                originalA = mc[currentIndex].Value;
                try
                {
                    Regex hRefIndex = new Regex("href[\\s\\S]*?=[\\s\\S]*?([\"'])", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    int indexOfHREF = 0;
                    if (!originalA.Trim().StartsWith("ECN") && string.IsNullOrEmpty(ecn_ID))
                    {
                        Match hrefMatch = hRefIndex.Match(originalA);
                        indexOfHREF = hrefMatch.Index + hrefMatch.Length;
                        string url = originalA.Substring(indexOfHREF, originalA.IndexOf(hrefMatch.Groups[1].Value, indexOfHREF) - indexOfHREF);
                        if (!url.StartsWith("%%"))
                        {
                            if (!CheckURLForHardCoded(url))
                            {
                                List<ECNError> errorList = new List<ECNError>();
                                errorList.Add(new ECNError(Entity, Enums.Method.Validate, "Content contains hardcoded reserved links.  Please replace them with the correct codesnippets"));
                                throw new ECNException(errorList);
                            }
                            Uri RedirectUri = new Uri(url);

                            string newGuid = Guid.NewGuid().ToString();
                            ecnIDs.Add(newGuid);

                            content = content.Remove(mc[currentIndex].Index, originalA.Length);
                            content = content.Insert(mc[currentIndex].Index, originalA.Insert(indexOfHREF - (hrefMatch.Length + 1), " ecn_id=\"" + newGuid + "\""));
                        }
                    }
                    else if (!string.IsNullOrEmpty(ecn_ID))//adding this to handle duplicate ecn_ids
                    {
                        int indexOFID = originalA.IndexOf("ecn_id=\"") + 8;
                        string ID = originalA.Substring(indexOFID, originalA.IndexOf("\"", indexOFID) - indexOFID);

                        Match hrefMatch = hRefIndex.Match(originalA);
                        indexOfHREF = hrefMatch.Index + hrefMatch.Length;
                        string url = originalA.Substring(indexOfHREF, originalA.IndexOf(hrefMatch.Groups[1].Value, indexOfHREF) - indexOfHREF);
                        if (!CheckURLForHardCoded(url))
                        {
                            List<ECNError> errorList = new List<ECNError>();
                            errorList.Add(new ECNError(Entity, Enums.Method.Validate, "Content contains hardcoded reserved links.  Please replace them with the correct codesnippets"));
                            throw new ECNException(errorList);
                        }
                        else
                        {
                            if (!ecnIDs.Contains(ID))
                            {
                                ecnIDs.Add(ID);
                            }
                            else
                            {
                                string newGuid = Guid.NewGuid().ToString();
                                ecnIDs.Add(newGuid);
                                content = content.Remove(mc[currentIndex].Index, originalA.Length);
                                content = content.Insert(mc[currentIndex].Index, originalA.Replace(ID, newGuid));
                            }
                        }
                    }
                }
                catch (ECNException ecn)
                {
                    throw ecn;
                }
                catch (Exception ex)
                {

                }
            }



            //Commenting this out because area tags are now handled above 9/25/2015 JWelter
            //RegexOptions roArea = RegexOptions.Singleline;
            //MatchCollection mcArea = Regex.Matches(content, "<area.*?href=\".*?\".*?>", roArea);
            //for (int i = mcArea.Count; i > 0; i--)
            //{
            //    int currentindex = i - 1;
            //    string originalA = "";

            //    originalA = mcArea[currentindex].Value;
            //    try
            //    {
            //        int indexOfHREF = 0;
            //        if (!originalA.Trim().StartsWith("ECN") && !originalA.Trim().Contains("ecn_id="))
            //        {
            //            indexOfHREF = originalA.IndexOf("href=\"") + 6;
            //            string url = originalA.Substring(indexOfHREF, originalA.IndexOf("\"", indexOfHREF) - indexOfHREF);
            //            if (!url.StartsWith("%%"))
            //            {
            //                Uri RedirectUri = new Uri(url);

            //                string newGuid = Guid.NewGuid().ToString();
            //                ecnIDs.Add(newGuid);

            //                content = content.Remove(mcArea[currentindex].Index, originalA.Length);
            //                content = content.Insert(mcArea[currentindex].Index, originalA.Insert(indexOfHREF - 7, " ecn_id=\"" + newGuid + "\""));
            //            }
            //        }
            //        else if (originalA.Trim().Contains("ecn_id="))//adding this to handle duplicate ecn_ids
            //        {
            //            int indexOFID = originalA.IndexOf("ecn_id=\"") + 8;
            //            string ID = originalA.Substring(indexOFID, originalA.IndexOf("\"", indexOFID) - indexOFID);

            //            if (!ecnIDs.Contains(ID))
            //            {
            //                ecnIDs.Add(ID);
            //            }
            //            else
            //            {
            //                string newGuid = Guid.NewGuid().ToString();
            //                ecnIDs.Add(newGuid);

            //                content = content.Remove(mcArea[currentindex].Index, originalA.Length);

            //                content = content.Insert(mcArea[currentindex].Index, originalA.Replace(ID, newGuid));

            //            }

            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }

            //}
            return content;
        }


        private static bool CheckURLForHardCoded(string url)
        {

            if (url.ToLower().Contains("engines/unsubscribe.aspx"))
            {
                return false;
            }
            else if (url.ToLower().Contains("ea.ecn5.com/click"))
            {
                return false;
            }
            else if (url.ToLower().Contains("ecn5.com/engines/linkfrom"))
            {
                return false;
            }

            return true;
        }

        public static bool FolderUsed(int folderID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Content.FolderUsed(folderID);
                scope.Complete();
            }
            return exists;
        }

        public static void Delete(int contentID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                throw new SecurityException();

            if (Exists(contentID, user.CustomerID))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.Layout.ContentUsedInLayout(contentID))
                {
                    if (!ECN_Framework_BusinessLayer.Communicator.DynamicTag.ContentUsedInDynamicTag(contentID, user.CustomerID))
                    {
                        if (!ECN_Framework_BusinessLayer.Communicator.ContentFilter.Exists(contentID, user.CustomerID))
                        {
                            //we are getting the id and security check needs the object...the get will perform the security check as well so no need to do it twice
                            GetByContentID(contentID, user, false);


                            using (TransactionScope scope = new TransactionScope())
                            {
                                if (ECN_Framework_BusinessLayer.Communicator.ContentFilter.Exists(contentID, user.CustomerID))
                                    ECN_Framework_BusinessLayer.Communicator.ContentFilter.Delete(contentID, user);

                                if (ECN_Framework_BusinessLayer.Communicator.LinkAlias.Exists(contentID, user.CustomerID))
                                    ECN_Framework_BusinessLayer.Communicator.LinkAlias.Delete(contentID, user);

                                ECN_Framework_DataLayer.Communicator.Content.Delete(contentID, user.CustomerID, user.UserID);
                                scope.Complete();
                            }
                        }
                        else
                        {
                            errorList.Add(new ECNError(Entity, Method, "Content is used by Content Filter"));
                            throw new ECNException(errorList);
                        }
                    }
                    else
                    {
                        errorList.Add(new ECNError(Entity, Method, "Content is used by Dynamic Tag"));
                        throw new ECNException(errorList);
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Content is used in layout(s)"));
                    throw new ECNException(errorList);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Content does not exist"));
                throw new ECNException(errorList);
            }

        }

        public static string GetContent(int? contentID, ContentTypeCode type, bool isMobile, KMPlatform.Entity.User user, int? emailID = null, int? groupID = null, int? blastid = null)
        {
            var returnHTML = string.Empty;
            if (contentID != null && contentID > 0)
            {
                var content = GetByContentID(contentID.Value, user, false);
                if (content != null)
                {
                    returnHTML = GetContentByType(content, type, user.CustomerID, blastid, isMobile);
                }
            }

            if (emailID != null && groupID != null)
            {
                return DynamicTag.ReplaceDynamicTags(returnHTML, emailID.Value, groupID.Value, user);
            }
            else
            {
                return returnHTML;
            }
        }

        private static string GetContentByType(ECN_Framework_Entities.Communicator.Content content, ContentTypeCode type, int customerId, int? blastid, bool isMobile)
        {
            var returnHTML = string.Empty;
            var contentTypeCode = content.ContentTypeCode;

            switch (contentTypeCode.ToLower())
            {
                case ContentTypeHtml:
                    if (type == ContentTypeCode.HTML)
                    {
                        if (isMobile && !string.IsNullOrWhiteSpace(content.ContentMobile))
                        {
                            returnHTML = content.ContentMobile;
                        }
                        else
                        {
                            returnHTML = content.ContentSource;
                        }

                        if (blastid == null)
                        {
                            // content is HTML, do NOT search for cached RSS info by BlastID
                            ContentReplacement.RSSFeed.Replace(ref returnHTML, customerId, false, null);
                        }
                    }
                    else
                    {
                        returnHTML = content.ContentText;
                        if (blastid == null)
                        {
                            // content is HTML, do NOT search for cached RSS info by BlastID
                            ContentReplacement.RSSFeed.Replace(ref returnHTML, customerId, false, null);
                        }
                    }
                    break;
                case ContentTypeText:
                    {
                        returnHTML = content.ContentText;
                        if (blastid == null)
                        {
                            ContentReplacement.RSSFeed.Replace(ref returnHTML, customerId, true, null);
                        }
                        break;
                    }
                case ContentTypeFeed:
                    returnHTML = ECN_Framework_Common.Functions.HTTPFunctions.GetWebFeed(content.ContentURL);
                    if (type != ContentTypeCode.HTML)
                    {
                        returnHTML = CommonStringFunctions.CleanHtmlString(returnHTML);
                    }
                    break;
                default:
                    if (type == ContentTypeCode.HTML)
                    {
                        returnHTML = string.Empty;
                    }
                    else
                    {
                        returnHTML = content.ContentText;
                    }
                    break;
            }

            return returnHTML;
        }

        public static string GetContent_NoAccessCheck(int? contentId, ContentTypeCode type, bool isMobile, int? emailId = null, int? groupId = null, int? blastId = null)
        {
            var returnHTML = string.Empty;
            if (contentId != null && contentId > 0)
            {
                var content = GetByContentID_NoAccessCheck(contentId.Value, false);
                if (content != null)
                {
                    returnHTML = GetContentByType(content, type, content.CustomerID.Value, blastId, isMobile);

                    if (emailId != null && groupId != null && content != null)
                    {
                        return DynamicTag.ReplaceDynamicTags_NoAccessCheck(returnHTML, content.CustomerID.Value, emailId.Value, groupId.Value);
                    }
                }
            }

            return returnHTML;
        }

        public static DataSet GetByContentTitle(string title, int? folderID, int? ValidatedOnly, int? userID, DateTime? updatedDateFrom, DateTime? updatedDateTo, KMPlatform.Entity.User user, int currentPage, int pageSize, string sortDirection, string sortColumn, string archiveFilter = "all")
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new SecurityException();

            DataSet dsContent = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dsContent = ECN_Framework_DataLayer.Communicator.Content.GetByContentTitle(title, folderID, ValidatedOnly, user.CustomerID, userID, updatedDateFrom, updatedDateTo, customer.BaseChannelID.Value, currentPage, pageSize, sortDirection, sortColumn, archiveFilter);
                scope.Complete();
            }

            return dsContent;
        }


        public static List<ECN_Framework_Entities.Communicator.Content> GetListByContentTitle(string title, int? folderID, int? userID, DateTime? updatedDateFrom, DateTime? updatedDateTo, KMPlatform.Entity.User user, int currentPage, int pageSize, string sortDirection, string sortColumn, string archiveFilter = "all")
        {
            List<ECN_Framework_Entities.Communicator.Content> contentList = new List<ECN_Framework_Entities.Communicator.Content>();

            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new SecurityException();

            DataSet dsContent = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentList = ECN_Framework_DataLayer.Communicator.Content.GetListByContentTitle(title, folderID, user.CustomerID, userID, updatedDateFrom, updatedDateTo, customer.BaseChannelID.Value, currentPage, pageSize, sortDirection, sortColumn, archiveFilter);
                scope.Complete();
            }

            return contentList;
        }


        public static List<ECN_Framework_Entities.Communicator.Content> GetByContentSearch(string title, int? folderID, int? userID, DateTime? updatedDateFrom, DateTime? updatedDateTo,
            KMPlatform.Entity.User user, bool getChildren, bool? archived = null)
        {
            List<ECN_Framework_Entities.Communicator.Content> contentList = new List<ECN_Framework_Entities.Communicator.Content>();
            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                contentList = ECN_Framework_DataLayer.Communicator.Content.GetByContentSearch(title, folderID, user.CustomerID, userID, updatedDateFrom, updatedDateTo, archived);
                scope.Complete();
            }

            if (contentList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Content content in contentList)
                {
                    content.FilterList = ECN_Framework_BusinessLayer.Communicator.ContentFilter.GetByContentID(content.ContentID, user, getChildren);
                    content.AliasList = ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByContentID(content.ContentID, user, getChildren);
                    //find and attach any dynamic tags
                    System.Collections.Generic.List<string> toParse = new List<string>();
                    toParse.Add(content.ContentSource);
                    toParse.Add(content.ContentText);
                    toParse.Add(content.ContentMobile);
                    List<string> tagNameList = GetTags(toParse);
                    if (tagNameList.Count > 0)
                    {
                        List<ECN_Framework_Entities.Communicator.DynamicTag> tagList = new List<ECN_Framework_Entities.Communicator.DynamicTag>();
                        ECN_Framework_Entities.Communicator.DynamicTag tag = null;
                        foreach (string tagName in tagNameList)
                        {
                            tag = ECN_Framework_BusinessLayer.Communicator.DynamicTag.GetByTag(tagName, user, getChildren);
                            tagList.Add(tag);
                        }
                        content.DynamicTagList = tagList;
                    }
                }
            }

            return contentList;
        }

        public static int CheckForTransnippet(string html)
        {
            try
            {
                //make sure we have an equal number of opening and closing trans tags
                //open tags
                Regex transnippetRegEx = new Regex(@"<transnippet ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                MatchCollection transnippetMatchs = transnippetRegEx.Matches(html);
                int transOpenCount = transnippetMatchs.Count;
                //close tags
                transnippetRegEx = new Regex(@"</transnippet>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                transnippetMatchs = transnippetRegEx.Matches(html);
                int transCloseCount = transnippetMatchs.Count;

                if (transOpenCount > 0)
                {
                    if (transOpenCount != transCloseCount)
                    {
                        return -1;
                    }

                    transnippetRegEx = new Regex(@"<transnippet_detail>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    transnippetMatchs = transnippetRegEx.Matches(html);
                    transOpenCount = transnippetMatchs.Count;

                    //close tags
                    transnippetRegEx = new Regex(@"</transnippet_detail>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    transnippetMatchs = transnippetRegEx.Matches(html);
                    transCloseCount = transnippetMatchs.Count;

                    if (transOpenCount > 0)
                    {
                        if (transOpenCount != transCloseCount)
                        {
                            return -1;
                        }
                        else
                        {
                            return transOpenCount;
                        }
                    }
                }

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static string ModifyHTML(string htmlOriginal, DataTable emailProfileDataTable)
        {
            string htmlModified = string.Empty;
            System.Collections.Generic.List<string> htmlList = new System.Collections.Generic.List<string>();

            //new split the html
            while (htmlOriginal.Length > 0)
            {
                int posTransOpen = htmlOriginal.IndexOf("<transnippet ");
                int posTransClose = htmlOriginal.IndexOf("</transnippet>");
                if (posTransOpen > 0)
                {
                    htmlModified += htmlOriginal.Substring(0, posTransOpen);
                    htmlOriginal = htmlOriginal.Substring(posTransOpen, htmlOriginal.Length - posTransOpen);
                }
                else if (posTransOpen == 0)
                {
                    string sortField = "";
                    string filterField = "";
                    string filterValue = "";
                    //<transnippet filter_field="ItemStatus" filter_value="RENT" sort="Property1Name">
                    //<transnippet filter_field="ItemStatus" filter_value="PURCHASE" sort="Property3Name">
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(htmlOriginal.Substring(0, htmlOriginal.IndexOf(">") + 1) + "</transnippet>");
                    XmlElement root = doc.DocumentElement;
                    if (root.HasAttribute("sort"))
                    {
                        sortField = root.Attributes["sort"].Value;
                    }
                    if (root.HasAttribute("filter_field"))
                    {
                        filterField = root.Attributes["filter_field"].Value;
                    }
                    if (root.HasAttribute("filter_value"))
                    {
                        filterValue = root.Attributes["filter_value"].Value;
                    }

                    htmlModified += GetTransDetail(htmlOriginal.Substring(0, posTransClose + 14), emailProfileDataTable, filterField, filterValue, sortField);
                    htmlOriginal = htmlOriginal.Substring(posTransClose + 14, htmlOriginal.Length - posTransClose - 14);
                }
                else
                {
                    htmlModified += htmlOriginal;
                    htmlOriginal = string.Empty;
                }
            }
            return htmlModified;
        }

        private static string GetTransDetail(string line, DataTable emailProfileDataTable, string filterField, string filterValue, string sortField)
        {
            line = line.Replace(line.Substring(0, line.IndexOf(">") + 1), "");
            line = line.Replace("</transnippet>", "");

            string lineModified = string.Empty;
            System.Collections.Generic.List<string> htmlList = new System.Collections.Generic.List<string>();
            string filter = string.Empty;
            string sort = string.Empty;
            if (filterField.Length > 0 && filterValue.Length > 0)
            {
                filter = filterField + " = '" + filterValue.Trim().ToUpper() + "'";
            }
            if (sortField.Length > 0)
            {
                sort = sortField.Trim().ToUpper() + " ASC";
            }

            //split the html
            int posTransOpen = 0;
            int posTransClose = 0;
            while (line.Length > 0)
            {
                posTransOpen = line.IndexOf("<transnippet_detail>");
                posTransClose = line.IndexOf("</transnippet_detail>");
                if (posTransOpen > 0)
                {
                    htmlList.Add(line.Substring(0, posTransOpen));
                    line = line.Substring(posTransOpen, line.Length - posTransOpen);
                }
                else if (posTransOpen == 0)
                {
                    htmlList.Add(line.Substring(0, posTransClose + 21));
                    line = line.Substring(posTransClose + 21, line.Length - posTransClose - 21);
                }
                else
                {
                    htmlList.Add(line);
                    line = string.Empty;
                }
            }

            foreach (string innerLine in htmlList)
            {
                //find out how many transnippet sections we need and then add them
                if (innerLine.IndexOf("<transnippet_detail>") == 0)
                {
                    DataRow[] dr;
                    DataView dv = emailProfileDataTable.DefaultView;
                    if (sort.Length > 0)
                    {
                        dv.Sort = sort;
                    }
                    if (filter.Length > 0)
                    {
                        dv.RowFilter = filter;
                    }
                    dr = dv.ToTable().Select();

                    if (dr.Length <= 0)
                    {
                        return string.Empty;
                    }

                    for (int i = 0; i < dr.Length; i++)
                    {
                        //replace all code snippets with corresponding value from emailprofiledataset
                        string newLine = innerLine;
                        string[] matchList = GetSnippets(newLine);
                        for (int l = 0; l < matchList.Length; l++)
                        {
                            newLine = newLine.Replace("##" + matchList[l] + "##", dr[i][matchList[l]].ToString());
                        }

                        lineModified += newLine;
                    }

                    lineModified = lineModified.Replace("<transnippet_detail>", "");
                    lineModified = lineModified.Replace("</transnippet_detail>", "");
                }
                else
                {
                    lineModified += innerLine;
                }
            }

            return lineModified;
        }

        private static string[] GetSnippets(string line)
        {
            string[] matches = null;
            MatchCollection MatchList1 = null;
            Regex regMatch = new System.Text.RegularExpressions.Regex("##", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection MatchList = regMatch.Matches(line);
            if (MatchList.Count > 0)
            {
                if ((MatchList.Count % 2) != 0)
                {
                    return matches;
                }
                else
                {
                    Regex regMatchGood = new Regex("##[a-zA-Z0-9_]+?##", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    System.Text.RegularExpressions.MatchCollection MatchListGood = regMatchGood.Matches(line);
                    if ((MatchList.Count / 2) > MatchListGood.Count)
                    {
                        return matches;
                    }
                }
            }

            //%% and ##
            Regex reg1 = new System.Text.RegularExpressions.Regex("##.+?##", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchList1 = reg1.Matches(line);
            matches = new string[MatchList1.Count];
            int p = 0;
            foreach (Match m in MatchList1)
            {
                if (!string.IsNullOrEmpty(m.Value.ToString()))
                {
                    matches[p] = m.Value.ToString().Replace("##", string.Empty);
                }
                p++;
            }

            return matches;
        }

        public static bool CheckForDynamicTags(string html)
        {
            if (html.IndexOf("ECN.DynamicTag.") > -1)
            {
                return true;
            }
            else
                return false;

        }

        public static void ValidateContentStatus(int layoutID)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            List<ECN_Framework_Entities.Communicator.Content> contentList = ECN_Framework_BusinessLayer.Communicator.Content.GetByLayoutID_NoAccessCheck(layoutID, true);

            foreach (ECN_Framework_Entities.Communicator.Content content in contentList)
            {
                if (!String.IsNullOrEmpty(content.ContentSource))
                {
                    content.IsValidated = content.IsValidated.HasValue ? content.IsValidated.Value : false;
                    if (!(bool)content.IsValidated)
                    {
                        errorList.Add(new ECNError(Entity, Method, "Content Not Validated. Please validate the Content and re-submit the blast."));
                        break;
                    }
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
        public static void ValidateLinks(int layoutID)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            List<ECN_Framework_Entities.Communicator.Content> contentList = ECN_Framework_BusinessLayer.Communicator.Content.GetByLayoutID_NoAccessCheck(layoutID, true);

            foreach (ECN_Framework_Entities.Communicator.Content content in contentList)
            {
                if (!String.IsNullOrEmpty(content.ContentSource) && content.ContentSource.Contains("href"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(content.ContentSource);

                    var hrefList = doc.DocumentNode.SelectNodes("//a");
                    if (hrefList != null && hrefList.Count > 0)
                    {
                        foreach (HtmlAgilityPack.HtmlNode href in hrefList)
                        {
                            if (href.Attributes.Any(x => x.Name.ToLower().Equals("href")))
                            {
                                if (String.IsNullOrEmpty(href.Attributes.First(x => x.Name.ToLower().Equals("href")).Value))
                                {
                                    errorList.Add(new ECNError(Entity, Method, "Content contains a link with a blank URL. Please correct the Content and re-submit the blast. "));
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void ArchiveContent(ECN_Framework_Entities.Communicator.Content cont, bool archive, int UserID)
        {
            cont.Archived = archive;
            cont.UpdatedDate = DateTime.Now;
            cont.UpdatedUserID = UserID;
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Content.Save(cont);
                scope.Complete();
            }

        }
        public static bool CheckHTMLTag(string html, Regex regex)
        {
            var bReturn = false;
            if (regex.IsMatch(html))
            {
                bReturn = true;
            }
            return bReturn;
        }

        public static bool ValidateHTMLContent(string html)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            Regex rgBodyEnd = new Regex("</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgBodyStart = new Regex("<body[^<]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgHTMLEnd = new Regex("</html>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgHTMLStart = new Regex("<html>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var rgViewState = new Regex(Utils.ViewStateRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            bool htmlIsNullOrEmpty = false;
            bool bodyEndError = false;
            bool htmlEndError = false;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);
            List<HtmlParseError> parseErrors = htmlDoc.ParseErrors.ToList();

            if (parseErrors != null)
            {
                foreach (var htmlParseError in parseErrors)
                {
                    switch (htmlParseError.Code)
                    {
                        case HtmlParseErrorCode.TagNotOpened:
                            if ((htmlParseError.Reason == "Start tag <body> was not found") && CheckHTMLTag(html, rgBodyEnd))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains a closing Body tag without an opening tag"));
                            }
                            else if ((htmlParseError.Reason == "Start tag <html> was not found") && CheckHTMLTag(html, rgHTMLEnd))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains a closing HTML tag without an opening tag"));
                            }
                            break;
                        case HtmlParseErrorCode.TagNotClosed:
                            if (htmlParseError.Reason == "End tag </body> was not found")
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains an opening Body tag without a closing tag"));
                                bodyEndError = true;
                            }
                            else if (htmlParseError.Reason == "End tag </html> was not found")
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains an opening HTML tag without a closing tag"));
                                htmlEndError = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            if (CheckHTMLTag(html, rgViewState))
            {
                errorList.Add(new ECNError(Entity, Method, "Content contains a ViewState field, which is not supported"));
            }
            if (!CheckHTMLTag(html, rgBodyEnd) && CheckHTMLTag(html, rgBodyStart))
            {
                if (!bodyEndError)
                    errorList.Add(new ECNError(Entity, Method, "Content contains an opening Body tag without a closing tag"));
            }
            if (!CheckHTMLTag(html, rgHTMLEnd) && CheckHTMLTag(html, rgHTMLStart))
            {
                if (!htmlEndError)
                    errorList.Add(new ECNError(Entity, Method, "Content contains an opening HTML tag without a closing tag"));
            }
            Regex rgHeadStart = new Regex("<head[^<]*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgHeadEnd = new Regex("</head>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (!CheckHTMLTag(html, rgHeadEnd) && CheckHTMLTag(html, rgHeadStart))
            {
                errorList.Add(new ECNError(Entity, Method, "Content contains an opening Head tag without a closing tag"));
            }
            else if (!CheckHTMLTag(html, rgHeadStart) && CheckHTMLTag(html, rgHeadEnd))
            {
                errorList.Add(new ECNError(Entity, Method, "Content contains a closing Head tag without an opening tag"));
            }



            //Base Tags are not allowed in Content
            Regex rgBaseStart = new Regex("<base.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Regex rgBaseEnd = new Regex("</base.*?>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (CheckHTMLTag(html, rgBaseStart) || CheckHTMLTag(html, rgBaseEnd))
            {
                errorList.Add(new ECNError(Entity, Method, "Base Tags are not allowed in Content"));
            }
            htmlIsNullOrEmpty = String.IsNullOrEmpty(html);
            if (!htmlIsNullOrEmpty)
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                int headCount = doc.DocumentNode.Descendants("head").Count();
                if (headCount > 1)
                {
                    errorList.Add(new ECNError(Entity, Method, "Content cannot contain multiple Head tags"));
                }
                int bodyCount = doc.DocumentNode.Descendants("body").Count();
                if (bodyCount > 1)
                {
                    errorList.Add(new ECNError(Entity, Method, "Content cannot contain multiple Body tags"));
                }
                int htmlcount = doc.DocumentNode.Descendants("html").Count();
                if (htmlcount > 1)
                {
                    errorList.Add(new ECNError(Entity, Method, "Content cannot contain multiple HTML tags"));
                }
            }
            // Empty Links are not Allowed
            if (!htmlIsNullOrEmpty && html.Contains("href"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var hrefList = doc.DocumentNode.SelectNodes("//a");
                if (hrefList != null && hrefList.Count > 0)
                {
                    foreach (HtmlAgilityPack.HtmlNode href in hrefList)
                    {
                        if (href.Attributes.Any(x => x.Name.ToLower().Equals("href")))
                        {
                            string hrefLink = href.Attributes.First(x => x.Name.ToLower().Equals("href")).Value;
                            if (String.IsNullOrEmpty(hrefLink.Trim()))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains an &lt;a&gt; tag with a blank URL."));
                                break;
                            }
                            if (char.IsWhiteSpace(hrefLink, 0))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains an &lt;a&gt; tag with a leading space in the URL."));
                                break;
                            }
                            if (char.IsWhiteSpace(hrefLink, hrefLink.Length - 1))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains an &lt;a&gt; tag with a trailing space in the URL."));
                                break;
                            }
                        }
                    }
                }
                var ahrefList = doc.DocumentNode.SelectNodes("//area");
                if (ahrefList != null && ahrefList.Count > 0)
                {
                    foreach (HtmlAgilityPack.HtmlNode href in ahrefList)
                    {
                        if (href.Attributes.Any(x => x.Name.ToLower().Equals("href")))
                        {
                            string hrefLink = href.Attributes.First(x => x.Name.ToLower().Equals("href")).Value;
                            if (String.IsNullOrEmpty(hrefLink.Trim()))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains an &lt;area&gt; tag with a blank URL."));
                                break;
                            }
                            if (char.IsWhiteSpace(hrefLink, 0))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains an &lt;area&gt; tag with a leading space in the URL."));
                                break;
                            }
                            if (char.IsWhiteSpace(hrefLink, hrefLink.Length - 1))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains an &lt;area&gt; tag with a trailing space in the URL."));
                                break;
                            }
                        }
                    }
                }
                var lhrefList = doc.DocumentNode.SelectNodes("//link");
                if (lhrefList != null && lhrefList.Count > 0)
                {
                    foreach (HtmlAgilityPack.HtmlNode href in lhrefList)
                    {
                        if (href.Attributes.Any(x => x.Name.ToLower().Equals("href")))
                        {
                            string hrefLink = href.Attributes.First(x => x.Name.ToLower().Equals("href")).Value;
                            if (String.IsNullOrEmpty(hrefLink.Trim()))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains a &lt;link&gt; tag with a blank URL."));
                                break;
                            }
                            if (char.IsWhiteSpace(hrefLink, 0))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains a &lt;link&gt; tag with a leading space in the URL."));
                                break;
                            }
                            if (char.IsWhiteSpace(hrefLink, hrefLink.Length - 1))
                            {
                                errorList.Add(new ECNError(Entity, Method, "Content contains a &lt;link&gt; tag with a trailing space in the URL."));
                                break;
                            }
                        }
                    }
                }
                doc = null;
            } // End Empty Links are not Allowed

            rgBodyEnd = null;
            rgBaseEnd = null;
            rgBaseStart = null;
            rgBodyStart = null;
            rgHeadEnd = null;
            rgHeadStart = null;
            rgHTMLEnd = null;
            rgHTMLStart = null;
            htmlDoc = null;

            parseErrors = null;
            if (errorList.Count > 0)
            {

                throw new ECNException(errorList);
            }
            else
            {
                return true;
            }
        }
    }
}
