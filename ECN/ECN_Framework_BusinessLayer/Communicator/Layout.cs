using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_BusinessLayer.Accounts.Interfaces;
using ECN_Framework_BusinessLayer.Communicator.Interfaces;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using EntitiesCommunicator = ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class Layout
    {
        private const string TemplateSourceColumnName = "TemplateSource";
        private const string TemplateTextColumnName = "TemplateText";
        private const string TableOptionsColumnName = "TableOptions";
        private const string TableCellAttributes = " cellpadding=0 cellspacing=0 ";
        private static ILayoutManager _layoutManager;
        private static ICustomerManager _customerManager;
        private static IUserManager _userManager;
        private static IAccessCheckManager _accessCheckManager;
        private static IContentManager _contentManager;
        private static ITemplateManager _templateManager;
        private static IFolderManager _folderManager;
        private static IMessageTypeManager _messageTypeManager;
        private static IConversionLinksManager _conversionLinksManager;
        delegate bool UpdateFolderConditionDelegate(EntitiesCommunicator.Layout layout);

        static readonly KMPlatform.Enums.Services ServiceCode = KMPlatform.Enums.Services.EMAILMARKETING;
        static readonly KMPlatform.Enums.ServiceFeatures ServiceFeatureCode = KMPlatform.Enums.ServiceFeatures.Content;

        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Layout;

        static Layout()
        {
            _layoutManager = new LayoutManager();
            _customerManager = new CustomerManager();
            _userManager = new UserManager();
            _accessCheckManager = new AccessCheckManager();
            _contentManager = new ContentManager();
            _templateManager = new TemplateManager();
            _folderManager = new FolderManager();
            _messageTypeManager = new MessageTypeManager();
            _conversionLinksManager = new ConversionLinksManager();
        }

        public static void Initialize(ILayoutManager layoutManager, ICustomerManager customerManager)
        {
            _layoutManager = layoutManager;
            _customerManager = customerManager;
        }

        public static void Initialize(
            ILayoutManager layoutManager, 
            IUserManager userManager, 
            IAccessCheckManager accessCheckManager,
            IContentManager contentManager,
            ITemplateManager templateManager,
            IFolderManager folderManager,
            IMessageTypeManager messageTypeManager,
            IConversionLinksManager conversionLinksManager)
        {
            _layoutManager = layoutManager;
            _userManager = userManager;
            _accessCheckManager = accessCheckManager;
            _contentManager = contentManager;
            _folderManager = folderManager;
            _templateManager = templateManager;
            _messageTypeManager = messageTypeManager;
            _conversionLinksManager = conversionLinksManager;
        }

        public static bool CreatedUserExists(int userID)
        {
            List<ECN_Framework_Entities.Communicator.Layout> contentList = new List<ECN_Framework_Entities.Communicator.Layout>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                bool result = ECN_Framework_DataLayer.Communicator.Layout.CreatedUserExists(userID);
                scope.Complete();
                return result;
            }
        }
        public static bool Exists(int layoutID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Layout.Exists(layoutID, customerID);
                scope.Complete();
            }
            return exists;
        }
        public static bool IsArchived(int layoutID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Layout.IsArchived(layoutID, customerID);
                scope.Complete();
            }
            return exists;
        }
        public static bool IsValidated(int layoutID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Layout.IsValidated(layoutID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists_UseAmbientTransaction(int layoutID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope())
            {
                exists = ECN_Framework_DataLayer.Communicator.Layout.Exists(layoutID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool Exists(int layoutID, string layoutName, int folderID, int customerID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Layout.Exists(layoutID, layoutName, folderID, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static bool ContentUsedInLayout(int contentID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Layout.ContentUsedInLayout(contentID);
                scope.Complete();
            }
            return exists;
        }

        public static bool TemplateUsedInLayout(int templateID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Layout.TemplateUsedInLayout(templateID);
                scope.Complete();
            }
            return exists;
        }

        public static bool MessageTypeUsedInLayout(int messageTypeID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Layout.MessageTypeUsedInLayout(messageTypeID);
                scope.Complete();
            }
            return exists;
        }

        public static List<EntitiesCommunicator.Layout> GetByCustomerID(int customerID, User user, bool getChildren)
        {
            var layoutList = new List<EntitiesCommunicator.Layout>();
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                layoutList = _layoutManager.GetByCustomerID(customerID).ToList();
                scope.Complete();
            }

            ConfirmAccess(user, layoutList);
            return FillLayoutList(user, getChildren, layoutList, UpdateFolderIfNotNullAndNotZero).ToList();
        }

        public static List<EntitiesCommunicator.Layout> GetByFolderIDCustomerID(int folderID, User user, bool getChildren)
        {
            var layoutList = new List<EntitiesCommunicator.Layout>();
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                layoutList = _layoutManager.GetByFolderIDCustomerID(folderID, user.CustomerID).ToList();
                scope.Complete();
            }

            ConfirmAccess(user, layoutList);
            return FillLayoutList(user, getChildren, layoutList, UpdateFolderIfNotNullAndNotZero).ToList();
        }

        public static List<ECN_Framework_Entities.Communicator.Layout> GetByFolderIDCustomerID_NoAccessCheck(int folderID, int customerId, bool getChildren)
        {
            List<ECN_Framework_Entities.Communicator.Layout> layoutList = new List<ECN_Framework_Entities.Communicator.Layout>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                layoutList = ECN_Framework_DataLayer.Communicator.Layout.GetByFolderIDCustomerID(folderID, customerId);
                scope.Complete();
            }

           
            if (layoutList != null && layoutList.Count > 0 && getChildren)
            {
                foreach (ECN_Framework_Entities.Communicator.Layout layout in layoutList)
                {
                    if (layout.ContentSlot1 != null)
                    {
                        layout.Slot1 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot1.Value, getChildren);
                    }
                    if (layout.ContentSlot2 != null)
                    {
                        layout.Slot2 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot2.Value, getChildren);
                    }
                    if (layout.ContentSlot3 != null)
                    {
                        layout.Slot3 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot3.Value, getChildren);
                    }
                    if (layout.ContentSlot4 != null)
                    {
                        layout.Slot4 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot4.Value, getChildren);
                    }
                    if (layout.ContentSlot5 != null)
                    {
                        layout.Slot5 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot5.Value,getChildren);
                    }
                    if (layout.ContentSlot6 != null)
                    {
                        layout.Slot6 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot6.Value, getChildren);
                    }
                    if (layout.ContentSlot7 != null)
                    {
                        layout.Slot7 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot7.Value, getChildren);
                    }
                    if (layout.ContentSlot8 != null)
                    {
                        layout.Slot8 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot8.Value, getChildren);
                    }
                    if (layout.ContentSlot9 != null)
                    {
                        layout.Slot9 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot9.Value,  getChildren);
                    }
                    if (layout.TemplateID != null)
                    {
                        layout.Template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(layout.TemplateID.Value);
                    }
                    if (layout.FolderID != null && layout.FolderID != 0)
                    {
                        layout.Folder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByFolderID_NoAccessCheck(layout.FolderID.Value);
                    }
                    if (layout.MessageTypeID != null)
                    {
                        layout.MessageType = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByMessageTypeID_NoAccessCheck(layout.MessageTypeID.Value);
                    }
                    layout.ConvLinks = ConversionLinks.GetByLayoutID_NoAccessCheck(layout.LayoutID);
                }
            }
            return layoutList;
        }

        /// <summary>
        /// Only Call when you don't have the currently logged in user(like from ActivityEngines)
        /// </summary>
        /// <param name="blastID"></param>
        /// <returns></returns>
        public static ECN_Framework_Entities.Communicator.Layout GetByLayoutID_NoAccessCheck(int layoutID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Layout layout = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                layout = ECN_Framework_DataLayer.Communicator.Layout.GetByLayoutID(layoutID);
                scope.Complete();
            }


            if (layout != null && getChildren)
            {
                if (layout.ContentSlot1 != null)
                {
                    layout.Slot1 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot1.Value, getChildren);
                }
                if (layout.ContentSlot2 != null)
                {
                    layout.Slot2 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot2.Value, getChildren);
                }
                if (layout.ContentSlot3 != null)
                {
                    layout.Slot3 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot3.Value, getChildren);
                }
                if (layout.ContentSlot4 != null)
                {
                    layout.Slot4 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot4.Value, getChildren);
                }
                if (layout.ContentSlot5 != null)
                {
                    layout.Slot5 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot5.Value, getChildren);
                }
                if (layout.ContentSlot6 != null)
                {
                    layout.Slot6 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot6.Value, getChildren);
                }
                if (layout.ContentSlot7 != null)
                {
                    layout.Slot7 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot7.Value, getChildren);
                }
                if (layout.ContentSlot8 != null)
                {
                    layout.Slot8 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot8.Value, getChildren);
                }
                if (layout.ContentSlot9 != null)
                {
                    layout.Slot9 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(layout.ContentSlot9.Value, getChildren);
                }
                if (layout.TemplateID != null)
                {
                    layout.Template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(layout.TemplateID.Value);
                }
                if (layout.FolderID != null && layout.FolderID != 0)
                {
                    layout.Folder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByFolderID_NoAccessCheck(layout.FolderID.Value);
                }
                if (layout.MessageTypeID != null)
                {
                    layout.MessageType = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByMessageTypeID_NoAccessCheck(layout.MessageTypeID.Value);
                }
                layout.ConvLinks = ConversionLinks.GetByLayoutID_NoAccessCheck(layoutID);
            }
            return layout;
        }

        public static ECN_Framework_Entities.Communicator.Layout GetByLayoutID_NoAccessCheck_UseAmbientTransaction(int layoutID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Layout layout = null;
            using (TransactionScope scope = new TransactionScope())
            {
                layout = ECN_Framework_DataLayer.Communicator.Layout.GetByLayoutID(layoutID);
                scope.Complete();
            }


            if (layout != null && getChildren)
            {
                if (layout.ContentSlot1 != null)
                {
                    layout.Slot1 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck_UseAmbientTransaction(layout.ContentSlot1.Value, getChildren);
                }
                if (layout.ContentSlot2 != null)
                {
                    layout.Slot2 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck_UseAmbientTransaction(layout.ContentSlot2.Value, getChildren);
                }
                if (layout.ContentSlot3 != null)
                {
                    layout.Slot3 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck_UseAmbientTransaction(layout.ContentSlot3.Value, getChildren);
                }
                if (layout.ContentSlot4 != null)
                {
                    layout.Slot4 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck_UseAmbientTransaction(layout.ContentSlot4.Value, getChildren);
                }
                if (layout.ContentSlot5 != null)
                {
                    layout.Slot5 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck_UseAmbientTransaction(layout.ContentSlot5.Value, getChildren);
                }
                if (layout.ContentSlot6 != null)
                {
                    layout.Slot6 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck_UseAmbientTransaction(layout.ContentSlot6.Value, getChildren);
                }
                if (layout.ContentSlot7 != null)
                {
                    layout.Slot7 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck_UseAmbientTransaction(layout.ContentSlot7.Value, getChildren);
                }
                if (layout.ContentSlot8 != null)
                {
                    layout.Slot8 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck_UseAmbientTransaction(layout.ContentSlot8.Value, getChildren);
                }
                if (layout.ContentSlot9 != null)
                {
                    layout.Slot9 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck_UseAmbientTransaction(layout.ContentSlot9.Value, getChildren);
                }
                if (layout.TemplateID != null)
                {
                    layout.Template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck_UseAmbientTransaction(layout.TemplateID.Value);
                }
                if (layout.FolderID != null && layout.FolderID != 0)
                {
                    layout.Folder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByFolderID_NoAccessCheck_UseAmbientTransaction(layout.FolderID.Value);
                }
                if (layout.MessageTypeID != null)
                {
                    layout.MessageType = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByMessageTypeID_NoAccessCheck_UseAmbientTransaction(layout.MessageTypeID.Value);
                }
                layout.ConvLinks = ConversionLinks.GetByLayoutID_NoAccessCheck_UseAmbientTransaction(layoutID);
            }
            return layout;
        }

        public static EntitiesCommunicator.Layout GetByLayoutID(int layoutID, User user, bool getChildren)
        {
            EntitiesCommunicator.Layout layout = null;
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                layout = _layoutManager.GetByLayoutID(layoutID);
                scope.Complete();
            }

            if (!_userManager.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
            {
                throw new SecurityException();
            }

            if (!_accessCheckManager.CanAccessByCustomer(layout, user))
            {
                throw new SecurityException();
            }

            if (layout != null && getChildren)
            {
                FillLayout(user, getChildren, layout, UpdateFolderIfNotNullAndNotZero);
                layout.ConvLinks = _conversionLinksManager.GetByLayoutID(layoutID, user).ToList();
            }
            return layout;
        }

        public static ECN_Framework_Entities.Communicator.Layout GetByLayoutID_UseAmbientTransaction(int layoutID, KMPlatform.Entity.User user, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.Layout layout = null;
            using (TransactionScope scope = new TransactionScope())
            {
                layout = ECN_Framework_DataLayer.Communicator.Layout.GetByLayoutID(layoutID);
                scope.Complete();
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(layout, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            if (layout != null && getChildren)
            {
                if (layout.ContentSlot1 != null)
                {
                    layout.Slot1 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_UseAmbientTransaction(layout.ContentSlot1.Value, user, getChildren);
                }
                if (layout.ContentSlot2 != null)
                {
                    layout.Slot2 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_UseAmbientTransaction(layout.ContentSlot2.Value, user, getChildren);
                }
                if (layout.ContentSlot3 != null)
                {
                    layout.Slot3 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_UseAmbientTransaction(layout.ContentSlot3.Value, user, getChildren);
                }
                if (layout.ContentSlot4 != null)
                {
                    layout.Slot4 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_UseAmbientTransaction(layout.ContentSlot4.Value, user, getChildren);
                }
                if (layout.ContentSlot5 != null)
                {
                    layout.Slot5 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_UseAmbientTransaction(layout.ContentSlot5.Value, user, getChildren);
                }
                if (layout.ContentSlot6 != null)
                {
                    layout.Slot6 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_UseAmbientTransaction(layout.ContentSlot6.Value, user, getChildren);
                }
                if (layout.ContentSlot7 != null)
                {
                    layout.Slot7 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_UseAmbientTransaction(layout.ContentSlot7.Value, user, getChildren);
                }
                if (layout.ContentSlot8 != null)
                {
                    layout.Slot8 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_UseAmbientTransaction(layout.ContentSlot8.Value, user, getChildren);
                }
                if (layout.ContentSlot9 != null)
                {
                    layout.Slot9 = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_UseAmbientTransaction(layout.ContentSlot9.Value, user, getChildren);
                }
                if (layout.TemplateID != null)
                {
                    layout.Template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_UseAmbientTransaction(layout.TemplateID.Value, user);
                }
                if (layout.FolderID != null && layout.FolderID != 0)
                {
                    layout.Folder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByFolderID_UseAmbientTransaction(layout.FolderID.Value, user);
                }
                if (layout.MessageTypeID != null)
                {
                    layout.MessageType = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByMessageTypeID_UseAmbientTransaction(layout.MessageTypeID.Value, user);
                }
                layout.ConvLinks = ConversionLinks.GetByLayoutID_UseAmbientTransaction(layoutID, user);
            }
            return layout;
        }

        public static bool FolderUsed(int folderID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.Layout.FolderUsed(folderID);
                scope.Complete();
            }
            return exists;
        }

        

        public static void Validate(ECN_Framework_Entities.Communicator.Layout layout, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (layout.CustomerID == null)
            {
                errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    if (!ECN_Framework_BusinessLayer.Accounts.Customer.Exists(layout.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "CustomerID is invalid"));
                    else
                    {
                        ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(layout.CustomerID.Value, false);
                        if (layout.TemplateID == null || (!ECN_Framework_BusinessLayer.Communicator.Template.Exists(layout.TemplateID.Value, customer.BaseChannelID.Value)))
                            errorList.Add(new ECNError(Entity, Method, "TemplateID is invalid"));
                    }

                    var invalidation = Base.UserValidation.Invalidate(layout);
                    if (!string.IsNullOrWhiteSpace(invalidation))
                    {
                        errorList.Add(new ECNError(Entity, Method, invalidation));
                    }

                    scope.Complete();
                }


                if (layout.FolderID == null || (layout.FolderID > 0 && !Folder.Exists(layout.FolderID.Value, layout.CustomerID.Value, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT)))
                {
                    errorList.Add(new ECNError(Entity, Method, "FolderID is invalid"));
                }
                else
                {
                    if (layout.LayoutName.Trim() == string.Empty)
                        errorList.Add(new ECNError(Entity, Method, "LayoutName cannot be empty"));
                    else if (Exists(layout.LayoutID, layout.LayoutName, layout.FolderID.Value, layout.CustomerID.Value))
                        errorList.Add(new ECNError(Entity, Method, "LayoutName already exists in this folder"));
                    else if (layout.LayoutName.Contains("&"))
                        errorList.Add(new ECNError(Entity, Method, "LayoutName cannot contain '&'"));
                    else if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(layout.LayoutName))
                        errorList.Add(new ECNError(Entity, Method, "LayoutName contains invalid characters"));
                }

                if (layout.ContentSlot1 == null)
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot1 cannot be empty"));
                else if (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(layout.ContentSlot1.Value, layout.CustomerID.Value))
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot1 is invalid"));

                if (layout.ContentSlot2 != null && layout.ContentSlot2 != 0 && (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(layout.ContentSlot2.Value, layout.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot2 is invalid"));

                if (layout.ContentSlot3 != null && layout.ContentSlot3 != 0 && (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(layout.ContentSlot3.Value, layout.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot3 is invalid"));

                if (layout.ContentSlot4 != null && layout.ContentSlot4 != 0 && (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(layout.ContentSlot4.Value, layout.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot4 is invalid"));

                if (layout.ContentSlot5 != null && layout.ContentSlot5 != 0 && (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(layout.ContentSlot5.Value, layout.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot5 is invalid"));

                if (layout.ContentSlot6 != null && layout.ContentSlot6 != 0 && (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(layout.ContentSlot6.Value, layout.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot6 is invalid"));

                if (layout.ContentSlot7 != null && layout.ContentSlot7 != 0 && (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(layout.ContentSlot7.Value, layout.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot7 is invalid"));

                if (layout.ContentSlot8 != null && layout.ContentSlot8 != 0 && (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(layout.ContentSlot8.Value, layout.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot8 is invalid"));

                if (layout.ContentSlot9 != null && layout.ContentSlot9 != 0 && (!ECN_Framework_BusinessLayer.Communicator.Content.Exists(layout.ContentSlot9.Value, layout.CustomerID.Value)))
                    errorList.Add(new ECNError(Entity, Method, "ContentSlot9 is invalid"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static void Save(ECN_Framework_Entities.Communicator.Layout layout, KMPlatform.Entity.User user)
        {
            if (layout.CreatedUserID == null && layout.UpdatedUserID != null)
                layout.CreatedUserID = layout.UpdatedUserID;

            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (layout.LayoutID > 0)
            {
                if (!Exists(layout.LayoutID, layout.CustomerID.Value))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "LayoutID is invalid"));
                    throw new ECNException(errorList);
                }
            }

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Edit))
                throw new ECN_Framework_Common.Objects.SecurityException();

            Validate(layout, user);
            using (TransactionScope scope = new TransactionScope())
            {
                layout.LayoutID = ECN_Framework_DataLayer.Communicator.Layout.Save(layout);
                if (layout.ConvLinks != null)
                {
                    List<ECN_Framework_Entities.Communicator.ConversionLinks> convLinksListCopy = new List<ECN_Framework_Entities.Communicator.ConversionLinks>();
                    foreach (ECN_Framework_Entities.Communicator.ConversionLinks convLink in layout.ConvLinks)
                    {
                        convLink.LayoutID = layout.LayoutID;
                        ECN_Framework_Entities.Communicator.ConversionLinks convLinkCopy = convLink;
                        ConversionLinks.Save(convLinkCopy, user);
                        convLinksListCopy.Add(convLinkCopy);
                    }
                    layout.ConvLinks = convLinksListCopy;
                }
                scope.Complete();
            }
        }

        public static void Delete(int layoutID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Delete;
            List<ECNError> errorList = new List<ECNError>();

            if (Exists(layoutID, user.CustomerID))
            {
                if (!ECN_Framework_BusinessLayer.Communicator.Blast.ActivePendingOrSentByLayout(layoutID, user.CustomerID))
                {
                    GetByLayoutID(layoutID, user, false);

                    if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.Delete))
                        throw new ECN_Framework_Common.Objects.SecurityException();

                    using (TransactionScope scope = new TransactionScope())
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.ConversionLinks.Exists(layoutID, user.CustomerID))
                            ECN_Framework_BusinessLayer.Communicator.ConversionLinks.Delete(layoutID, user);
                        ECN_Framework_DataLayer.Communicator.Layout.Delete(layoutID, user.CustomerID, user.UserID);
                        scope.Complete();
                    }
                }
                else
                {
                    errorList.Add(new ECNError(Entity, Method, "Message is used in blast(s). Deleting is not allowed."));
                    throw new ECNException(errorList);
                }
            }
            else
            {
                errorList.Add(new ECNError(Entity, Method, "Layout does not exist"));
                throw new ECNException(errorList);
            }

        }

        public static void Archive(ECN_Framework_Entities.Communicator.Layout l, bool archive , int userID)
        {
            l.Archived = archive;
            l.UpdatedDate = DateTime.Now;
            l.UpdatedUserID = userID;
            using(TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.Layout.Save(l);
                scope.Complete();
            }

        }

        public static List<EntitiesCommunicator.Layout> GetByLayoutSearch(
            string name, 
            int? folderID, 
            int? userID, 
            DateTime? updatedDateFrom, 
            DateTime? updatedDateTo, 
            User user, 
            bool getChildren, 
            bool? archived = null)
        {
            return GetByLayoutSearch(name, folderID, userID, user.CustomerID, updatedDateFrom, updatedDateTo, user, getChildren, archived);
        }

        public static List<EntitiesCommunicator.Layout> GetByLayoutSearch(
            string name, 
            int? folderID, 
            int? userID, 
            int customerId, 
            DateTime? updatedDateFrom, 
            DateTime? updatedDateTo, 
            User user, 
            bool getChildren,
            bool? archived = false)
        {
            var layoutList = new List<EntitiesCommunicator.Layout>();
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                layoutList = _layoutManager.GetByLayoutSearch(name, folderID, customerId, userID, updatedDateFrom, updatedDateTo, archived).ToList();
                scope.Complete();
            }

            ConfirmAccess(user, layoutList);
            return FillLayoutList(user, getChildren, layoutList, UpdateFolderIfNotNull).ToList();
        }

        public static int GetLayoutUserID(int layoutID)
        {
            return ECN_Framework_DataLayer.Communicator.Layout.GetByLayoutID(layoutID).CreatedUserID.Value;
        }

        public static DataSet GetByLayoutName(string name, int? folderID, int? userID,int ValidatedOnly, DateTime? updatedDateFrom, DateTime? updatedDateTo, KMPlatform.Entity.User user, int CurrentPage, int PageSize, string SortDirection, string SortColumn, string ArchiveFilter)
        {
            ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(user.CustomerID, false);
            DataSet dsLayout = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dsLayout = ECN_Framework_DataLayer.Communicator.Layout.GetByLayoutName(name, folderID, user.CustomerID, userID, ValidatedOnly, updatedDateFrom, updatedDateTo, customer.BaseChannelID.Value, CurrentPage, PageSize, SortDirection, SortColumn, ArchiveFilter);
                scope.Complete();
            }
            return dsLayout;
        }

        public static DataTable GetByLayoutID(int layoutID, int customerID, int baseChannelID)
        {
            DataTable dtLayout = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtLayout = _layoutManager.GetByLayoutID(layoutID, customerID, baseChannelID);
                scope.Complete();
            }
            return dtLayout;
        }

        public static string GetPreview(int layoutID, ContentTypeCode type, bool isMobile, User user, int? emailID = null, int? groupID = null, int? blastid = null)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var dtLayout = GetLayoutsDataTable(layoutID, user.CustomerID);
            if (dtLayout == null || dtLayout.Rows.Count <= 0)
            {
                return string.Empty;
            }

            var slots = new int?[9];
            var tableOptions = GetSlotsValuesAndTableOptions(type, dtLayout, slots);

            return EmailBody(
                    dtLayout.Rows[0][TemplateSourceColumnName]?.ToString(),
                    dtLayout.Rows[0][TemplateTextColumnName]?.ToString(),
                    tableOptions, 
                    slots[0],
                    slots[1], 
                    slots[2],
                    slots[3],
                    slots[4], 
                    slots[5], 
                    slots[6], 
                    slots[7], 
                    slots[8], 
                    type, 
                    isMobile, 
                    user, 
                    emailID,
                    groupID, 
                    blastid);
        }

        public static string GetPreviewNoAccessCheck(int layoutID, ContentTypeCode type, bool isMobile, int customerID, int? emailID = null, int? groupID = null, int? blastid = null)
        {
            var dtLayout = GetLayoutsDataTable(layoutID, customerID);
            if (dtLayout == null || dtLayout.Rows.Count <= 0)
            {
                return string.Empty;
            }

            var slots = new int?[9];
            var tableOptions = GetSlotsValuesAndTableOptions(type, dtLayout, slots);

            return EmailBody_NoAccessCheck(
                    dtLayout.Rows[0][TemplateSourceColumnName]?.ToString(),
                    dtLayout.Rows[0][TemplateTextColumnName]?.ToString(),
                    tableOptions,
                    slots[0],
                    slots[1],
                    slots[2],
                    slots[3],
                    slots[4],
                    slots[5],
                    slots[6],
                    slots[7],
                    slots[8],
                    type,
                    isMobile,
                    emailID,
                    groupID,
                    blastid);
        }

        private static DataTable GetLayoutsDataTable(int layoutID, int customerID)
        {
            var customer = _customerManager.GetByCustomerId(customerID, false);
            return GetByLayoutID(layoutID, customerID, customer.BaseChannelID.Value);
        }

        private static string GetSlotsValuesAndTableOptions(ContentTypeCode type, DataTable dtLayout, int?[] slots)
        {
            var tableOptions = dtLayout.Rows[0][TableOptionsColumnName]?.ToString();
            if (type == ContentTypeCode.HTML)
            {
                if (string.IsNullOrWhiteSpace(tableOptions))
                {
                    tableOptions = TableCellAttributes;
                }
            }

            var ContentSoltsNames = new string[]
            {
                "ContentSlot1",
                "ContentSlot2",
                "ContentSlot3",
                "ContentSlot4",
                "ContentSlot5",
                "ContentSlot6",
                "ContentSlot7",
                "ContentSlot8",
                "ContentSlot9"
            };

            var counter = 0;
            foreach (var slotName in ContentSoltsNames)
            {
                if (slots.Length >= counter && slots.Length < counter)
                {
                    var slotValue = dtLayout.Rows[0][slotName]?.ToString();
                    var intValue = 0;

                    if (int.TryParse(slotValue, out intValue))
                    {
                        slots[counter] = intValue;
                    }
                    else
                    {
                        Trace.WriteLine($"Unable to convert {slotValue} to int.");
                    }

                    counter++;
                }
            }

            return tableOptions;
        }

        public static string EmailBody(
            string TemplateSource, string TemplateText, string TableOptions,
            int? Slot1, int? Slot2, int? Slot3, int? Slot4, int? Slot5, int? Slot6, int? Slot7, int? Slot8, int? Slot9, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode type, bool isMobile, KMPlatform.Entity.User user, int? emailID = null, int? groupID = null, int? blastid = null)
        {
            string body = string.Empty;
            if (type == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML)
            {
                body = TemplateSource;
            }
            else if (type == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.TEXT)
            {
                body = TemplateText;
            }
            body = StringFunctions.Replace(body, "%%slot1%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot1, type, isMobile, user, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot2%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot2, type, isMobile, user, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot3%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot3, type, isMobile, user, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot4%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot4, type, isMobile, user, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot5%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot5, type, isMobile, user, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot6%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot6, type, isMobile, user, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot7%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot7, type, isMobile, user, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot8%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot8, type, isMobile, user, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot9%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot9, type, isMobile, user, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "®", "&reg;");
            body = StringFunctions.Replace(body, "©", "&copy;");
            body = StringFunctions.Replace(body, "™", "&trade;");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");
            if (type == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML)
            {
                if (TableOptions.Length < 1)
                {
                    TableOptions = " cellpadding=0 cellspacing=0 width='100%'";
                }
                else if (!TableOptions.ToLower().Contains("width"))
                {
                    TableOptions += " width='100%'";
                }
                body = "<table " + TableOptions + "><tr><td>" + body + "</td></tr></table>";
            }
            return body;
        }

        public static string EmailBody_NoAccessCheck(
            string TemplateSource, string TemplateText, string TableOptions,
            int? Slot1, int? Slot2, int? Slot3, int? Slot4, int? Slot5, int? Slot6, int? Slot7, int? Slot8, int? Slot9, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode type, bool isMobile, int? emailID = null, int? groupID = null, int? blastid = null)
        {
            string body = string.Empty;
            if (type == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML)
            {
                body = TemplateSource;
            }
            else if (type == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.TEXT)
            {
                body = TemplateText;
            }
            body = StringFunctions.Replace(body, "%%slot1%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent_NoAccessCheck(Slot1, type, isMobile, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot2%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent_NoAccessCheck(Slot2, type, isMobile, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot3%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent_NoAccessCheck(Slot3, type, isMobile, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot4%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent_NoAccessCheck(Slot4, type, isMobile, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot5%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent_NoAccessCheck(Slot5, type, isMobile, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot6%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent_NoAccessCheck(Slot6, type, isMobile, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot7%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent_NoAccessCheck(Slot7, type, isMobile, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot8%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent_NoAccessCheck(Slot8, type, isMobile, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "%%slot9%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent_NoAccessCheck(Slot9, type, isMobile, emailID, groupID, blastid));
            body = StringFunctions.Replace(body, "®", "&reg;");
            body = StringFunctions.Replace(body, "©", "&copy;");
            body = StringFunctions.Replace(body, "™", "&trade;");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");
            if (type == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML)
            {
                if (TableOptions.Length < 1)
                {
                    TableOptions = " cellpadding=0 cellspacing=0 width='100%'";
                }
                else if (!TableOptions.ToLower().Contains("width"))
                {
                    TableOptions += " width='100%'";
                }
                body = "<table " + TableOptions + "><tr><td>" + body + "</td></tr></table>";
            }
            return body;
        }


        public static string HTMLPagePreview(string QueryValue, string HeaderCode, string FooterCode, string TemplateSource,
            int? Slot1, int? Slot2, int? Slot3, int? Slot4, int? Slot5, int? Slot6, int? Slot7, int? Slot8, int? Slot9, KMPlatform.Entity.User user)
        {
            string page = "";
            page += "<table border='0' width='100%' cellpadding='0' cellspacing='0'><tr><td>" + HeaderCode + "</td></tr>";
            page += "<tr><td>";

            string body = TemplateSource;
            body = StringFunctions.Replace(body, "%%slot1%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot1, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user));
            body = StringFunctions.Replace(body, "%%slot2%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot2, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user));
            body = StringFunctions.Replace(body, "%%slot3%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot3, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user));
            body = StringFunctions.Replace(body, "%%slot4%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot4, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user));
            body = StringFunctions.Replace(body, "%%slot5%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot5, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user));
            body = StringFunctions.Replace(body, "%%slot6%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot6, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user));
            body = StringFunctions.Replace(body, "%%slot7%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot7, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user));
            body = StringFunctions.Replace(body, "%%slot8%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot8, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user));
            body = StringFunctions.Replace(body, "%%slot9%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot9, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, user));
            body = StringFunctions.Replace(body, "®", "&reg;");
            body = StringFunctions.Replace(body, "©", "&copy;");
            body = StringFunctions.Replace(body, "™", "&trade;");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");
            page += body + "</td></tr>";
            page += "<tr><td>";
            page += FooterCode;
            page += "</td></tr></table>";
            return page;
        }


        public static string HTMLEdit(
           string TemplateSource, string TemplateText, string TableOptions,
           int? Slot1, int? Slot2, int? Slot3, int? Slot4, int? Slot5, int? Slot6, int? Slot7, int? Slot8, int? Slot9, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode type, bool isMobile, KMPlatform.Entity.User user)
        {
            string body = string.Empty;
            if (type == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML)
            {
                body = TemplateSource;
            }
            else if (type == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.TEXT)
            {
                body = TemplateText;
            }
            body = StringFunctions.Replace(body, "%%slot1%%", @"<div id=div_slot1 style=""border:1px;border-style:dashed;"">%%slot1%%</div>");
            body = StringFunctions.Replace(body, "%%slot2%%", @"<div id=div_slot2 style=""border:1px;border-style:dashed;"">%%slot2%%</div>");
            body = StringFunctions.Replace(body, "%%slot3%%", @"<div id=div_slot3 style=""border:1px;border-style:dashed;"">%%slot3%%</div>");
            body = StringFunctions.Replace(body, "%%slot4%%", @"<div id=div_slot4 style=""border:1px;border-style:dashed;"">%%slot4%%</div>");
            body = StringFunctions.Replace(body, "%%slot5%%", @"<div id=div_slot5 style=""border:1px;border-style:dashed;"">%%slot5%%</div>");
            body = StringFunctions.Replace(body, "%%slot6%%", @"<div id=div_slot6 style=""border:1px;border-style:dashed;"">%%slot6%%</div>");
            body = StringFunctions.Replace(body, "%%slot7%%", @"<div id=div_slot7 style=""border:1px;border-style:dashed;"">%%slot7%%</div>");
            body = StringFunctions.Replace(body, "%%slot8%%", @"<div id=div_slot8 style=""border:1px;border-style:dashed;"">%%slot8%%</div>");
            body = StringFunctions.Replace(body, "%%slot9%%", @"<div id=div_slot9 style=""border:1px;border-style:dashed;"">%%slot9%%</div>");

            body = StringFunctions.Replace(body, "%%slot1%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot1, type, isMobile, user));
            body = StringFunctions.Replace(body, "%%slot2%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot2, type, isMobile, user));
            body = StringFunctions.Replace(body, "%%slot3%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot3, type, isMobile, user));
            body = StringFunctions.Replace(body, "%%slot4%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot4, type, isMobile, user));
            body = StringFunctions.Replace(body, "%%slot5%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot5, type, isMobile, user));
            body = StringFunctions.Replace(body, "%%slot6%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot6, type, isMobile, user));
            body = StringFunctions.Replace(body, "%%slot7%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot7, type, isMobile, user));
            body = StringFunctions.Replace(body, "%%slot8%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot8, type, isMobile, user));
            body = StringFunctions.Replace(body, "%%slot9%%", ECN_Framework_BusinessLayer.Communicator.Content.GetContent(Slot9, type, isMobile, user));
            body = StringFunctions.Replace(body, "®", "&reg;");
            body = StringFunctions.Replace(body, "©", "&copy;");
            body = StringFunctions.Replace(body, "™", "&trade;");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");
            if (type == ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML)
            {
                if (TableOptions.Length < 1)
                {
                    TableOptions = " cellpadding=0 cellspacing=0 width='100%'";
                }
                else if (!TableOptions.ToLower().Contains("width"))
                {
                    TableOptions += " width='100%'";
                }
                body = "<table " + TableOptions + "><tr><td>" + body + "</td></tr></table>";
            }
            return body;
        }

        public static System.Collections.Generic.List<string> ValidateLayoutContent(int layoutID)
        {
            System.Collections.Generic.List<string> retList = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Communicator.Layout.ValidateLayoutContent(layoutID);
                scope.Complete();
            }
            return retList;
        }

        public static System.Collections.Generic.List<string> ValidateLayoutContent_UseAmbientTransaction(int layoutID)
        {
            System.Collections.Generic.List<string> retList = null;
            using (TransactionScope scope = new TransactionScope())
            {
                retList = ECN_Framework_DataLayer.Communicator.Layout.ValidateLayoutContent(layoutID);
                scope.Complete();
            }
            return retList;
        }

        public static DataTable GetLayoutDR(int customerID, int userID, KMPlatform.Entity.User user)
        {
            DataTable dtLayout = null;

            if (!KM.Platform.User.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
                throw new ECN_Framework_Common.Objects.SecurityException();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtLayout = ECN_Framework_DataLayer.Communicator.Layout.GetLayoutDR(customerID, userID);
                scope.Complete();
            }

            return dtLayout;
        }

        private static void ConfirmAccess(User user, IList<EntitiesCommunicator.Layout> layoutList)
        {
            if (!_userManager.HasAccess(user, ServiceCode, ServiceFeatureCode, KMPlatform.Enums.Access.View))
            {
                throw new SecurityException();
            }

            if (!_accessCheckManager.CanAccessByCustomer(layoutList, user))
            {
                throw new SecurityException();
            }
        }

        private static IList<EntitiesCommunicator.Layout> FillLayoutList(
            User user, 
            bool getChildren, 
            IList<EntitiesCommunicator.Layout> layoutList, 
            UpdateFolderConditionDelegate updateFolder)
        {
            if (layoutList.Any() && getChildren)
            {
                foreach (var layout in layoutList)
                {
                    FillLayout(user, getChildren, layout, updateFolder);
                    layout.ConvLinks = _conversionLinksManager.GetByLayoutID(layout.LayoutID, user).ToList();
                }
            }

            return layoutList;
        }

        private static void FillLayout(User user, bool getChildren, EntitiesCommunicator.Layout layout, UpdateFolderConditionDelegate updateFolder)
        {
            if (layout == null)
            {
                throw new ArgumentNullException("layout");
            }

            if (layout.ContentSlot1 != null)
            {
                layout.Slot1 = _contentManager.GetByContentID(layout.ContentSlot1.Value, user, getChildren);
            }
            if (layout.ContentSlot2 != null)
            {
                layout.Slot2 = _contentManager.GetByContentID(layout.ContentSlot2.Value, user, getChildren);
            }
            if (layout.ContentSlot3 != null)
            {
                layout.Slot3 = _contentManager.GetByContentID(layout.ContentSlot3.Value, user, getChildren);
            }
            if (layout.ContentSlot4 != null)
            {
                layout.Slot4 = _contentManager.GetByContentID(layout.ContentSlot4.Value, user, getChildren);
            }
            if (layout.ContentSlot5 != null)
            {
                layout.Slot5 = _contentManager.GetByContentID(layout.ContentSlot5.Value, user, getChildren);
            }
            if (layout.ContentSlot6 != null)
            {
                layout.Slot6 = _contentManager.GetByContentID(layout.ContentSlot6.Value, user, getChildren);
            }
            if (layout.ContentSlot7 != null)
            {
                layout.Slot7 = _contentManager.GetByContentID(layout.ContentSlot7.Value, user, getChildren);
            }
            if (layout.ContentSlot8 != null)
            {
                layout.Slot8 = _contentManager.GetByContentID(layout.ContentSlot8.Value, user, getChildren);
            }
            if (layout.ContentSlot9 != null)
            {
                layout.Slot9 = _contentManager.GetByContentID(layout.ContentSlot9.Value, user, getChildren);
            }
            if (layout.TemplateID != null)
            {
                layout.Template = _templateManager.GetByTemplateID(layout.TemplateID.Value, user);
            }
            if (updateFolder(layout))
            {
                layout.Folder = _folderManager.GetByFolderID(layout.FolderID.Value, user);
            }
            if (layout.MessageTypeID != null)
            {
                layout.MessageType = _messageTypeManager.GetByMessageTypeID(layout.MessageTypeID.Value, user);
            }
        }

        private static bool UpdateFolderIfNotNull(EntitiesCommunicator.Layout layout)
        {
            return layout?.FolderID != null;
        }

        private static bool UpdateFolderIfNotNullAndNotZero(EntitiesCommunicator.Layout layout)
        {
            return layout?.FolderID != null && layout.FolderID != 0;
        }
    }
}