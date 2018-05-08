using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using ecn.webservice.Facades.Params;
using ECN_Framework_BusinessLayer.Accounts;
using ECN_Framework_BusinessLayer.Communicator;
using KM.Common.Extensions;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using CommunicatorObjects = ECN_Framework_Common.Objects.Communicator;
using AccountEntities = ECN_Framework_Entities.Accounts;

namespace ecn.webservice.Facades
{
    public class ContentFacade : FacadeBase, IContentFacade
    {
        private const string CDataSectionReplacement = " ";
        private const string CDataSectionRegex = @"<!\[CDATA\[(.*?)\]\]>";
        private const string DefaultTitleDateTimeFormat = "yyyyMMdd-HH:mm:ss";
        private const string LockedFlagIsLocked = "Y";
        private const string SharingFlagDisabled = "N";

        public string SearchForContent(WebMethodExecutionContext context, string XMLSearch)
        {
            var title = string.Empty;
            int? folderID = null;
            int? userID = null;
            DateTime? updatedDateFrom = null;
            DateTime? updatedDateTo = null;

            GetContentSearchValues(
                XMLSearch,
                context.User.CustomerID,
                ref title,
                ref folderID,
                ref userID,
                ref updatedDateFrom,
                ref updatedDateTo);

            var contentList = Content.GetByContentSearch(
                title,
                folderID,
                userID,
                updatedDateFrom,
                updatedDateTo,
                context.User,
                false);

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, BuildContentReturnXML(contentList));
        }

        public string SearchForMessages(WebMethodExecutionContext context, string XMLSearch)
        {
            var title = string.Empty;
            int? folderID = null;
            int? userID = null;
            DateTime? updatedDateFrom = null;
            DateTime? updatedDateTo = null;

            GetLayoutSearchValues(
                XMLSearch,
                context.User.CustomerID,
                ref title,
                ref folderID,
                ref userID,
                ref updatedDateFrom,
                ref updatedDateTo);

            var layoutList = Layout.GetByLayoutSearch(
                title,
                folderID,
                userID,
                updatedDateFrom,
                updatedDateTo,
                context.User,
                false);

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, BuildLayoutReturnXML(layoutList));
        }

        public string GetContentListByFolderId(WebMethodExecutionContext context, int folderId)
        {
            var contentList = Content.GetByFolderIDCustomerID(folderId, context.User, false);
            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);

            return GetSuccessResponse(context, BuildContentReturnXML(contentList));
        }

        public string GetMessageListByFolderId(WebMethodExecutionContext context, int folderId)
        {
            var layoutList = Layout.GetByFolderIDCustomerID(folderId, context.User, false);
            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);

            return GetSuccessResponse(context, BuildLayoutReturnXML(layoutList));
        }

        public string GetContent(WebMethodExecutionContext context, int contentId)
        {
            var contentList = new List<CommunicatorEntities.Content>
            {
                Content.GetByContentID(contentId, context.User, false)
            };

            if (contentList.Any() && contentList[0] != null)
            {
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, BuildContentReturnXML(contentList));
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "CONTENT DOESN'T EXIST");
        }

        public string GetMessage(WebMethodExecutionContext context, int layoutId)
        {
            var layoutList = new List<CommunicatorEntities.Layout>
            {
                Layout.GetByLayoutID(layoutId, context.User, false)
            };

            if (layoutList.Count > 0 && layoutList[0] != null)
            {
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetSuccessResponse(context, BuildLayoutReturnXML(layoutList));
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "LAYOUT DOESN'T EXIST");
        }

        public string PreviewMessage(WebMethodExecutionContext context, int messageId)
        {
            if (Layout.Exists(messageId, context.User.CustomerID))
            {
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                var messagePreview = Layout.GetPreview(
                    messageId,
                    CommunicatorObjects.Enums.ContentTypeCode.HTML,
                    false,
                    context.User);

                var responseText = $"<![CDATA[{messagePreview}]]>";

                return GetSuccessResponse(context, responseText);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "MESSAGE DOESN'T EXIST");
        }

        public string PreviewContent(WebMethodExecutionContext context, int contentId)
        {
            var content = Content.GetByContentID(contentId, context.User, false);
            if (content != null)
            {
                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                var responseText = $"<![CDATA[{content.ContentSource}]]>";

                return GetSuccessResponse(context, responseText);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "CONTENT DOESN'T EXIST");
        }

        public string GetFolders(WebMethodExecutionContext context)
        {
            var folderList = Folder.GetByType(
                context.User.CustomerID,
                CommunicatorObjects.Enums.FolderTypes.CNT.ToString(),
                context.User);

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, BuildFolderReturnXML(folderList));
        }

        public string GetTemplates(WebMethodExecutionContext context)
        {
            var customer = Customer.GetByCustomerID(
                context.User.CustomerID,
                false);
            var templateList = Template.GetByBaseChannelID(
                customer.BaseChannelID.Value,
                context.User);

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);

            return GetSuccessResponse(context, BuildTemplateReturnXML(templateList));
        }

        public string GetMessageTypes(WebMethodExecutionContext context)
        {
            var customer = Customer.GetByCustomerID(
                context.User.CustomerID,
                false);
            var messageTypeList = MessageType.GetByBaseChannelID(
                customer.BaseChannelID.Value,
                context.User);

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, BuildMessageTypeReturnXML(messageTypeList));
        }

        public string GetCustomerDepts(WebMethodExecutionContext context)
        {
            var departmentList = CustomerDepartment.GetByCustomerID(
                context.User.CustomerID,
                false);

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, BuildCustomerDepartmentReturnXML(departmentList));
        }

        public string DeleteFolder(WebMethodExecutionContext context, int folderId)
        {
            Folder.Delete(folderId, context.User);
            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, "FOLDER DELETED", folderId);
        }

        public string DeleteContent(WebMethodExecutionContext context, int contentId)
        {
            Content.Delete(contentId, context.User);
            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, "CONTENT DELETED", contentId);
        }

        public string DeleteMessage(WebMethodExecutionContext context, int messageId)
        {
            Layout.Delete(messageId, context.User);
            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetSuccessResponse(context, "MESSAGE DELETED", messageId);
        }

        public string UpdateContent(WebMethodExecutionContext context, ContentParams parameters)
        {
            var content = Content.GetByContentID(parameters.ContentId, context.User, false);
            if (content != null)
            {
                var folderId = content.FolderID == null ? 0 : content.FolderID.Value;
                if (!Content.Exists(parameters.ContentId, parameters.Title, folderId, context.User.CustomerID))
                {
                    FixTitleIfNeeded(parameters);

                    content.UpdatedUserID = context.User.UserID;
                    content.ContentSource = parameters.ContentHtml;
                    content.ContentMobile = parameters.ContentHtml;
                    content.ContentText = parameters.ContentText;
                    content.ContentTitle = parameters.Title;
                    Content.ReadyContent(content, false);
                    Content.Save(content, context.User);

                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    return GetSuccessResponse(context, "CONTENT UPDATED", content.ContentID);
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                var alreadyExistsMessage = $"{parameters.Title} ALREADY EXISTS FOR CUSTOMER";
                return GetFailResponse(context, alreadyExistsMessage);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "CONTENT DOESN'T EXIST FOR CUSTOMER");
        }

        public string AddContent(WebMethodExecutionContext context, ContentParams parameters)
        {
            if (parameters.FolderId == 0 || Folder.Exists(parameters.FolderId, context.User.CustomerID))
            {
                if (!Content.Exists(parameters.Title, parameters.FolderId, context.User.CustomerID))
                {
                    FixTitleIfNeeded(parameters);

                    var content = new CommunicatorEntities.Content
                    {
                        CreatedUserID = context.User.UserID,
                        LockedFlag = LockedFlagIsLocked,
                        ContentSource = parameters.ContentHtml,
                        ContentMobile = parameters.ContentHtml,
                        ContentText = parameters.ContentText,
                        FolderID = parameters.FolderId,
                        ContentTypeCode = CommunicatorObjects.Enums.ContentTypeCode.HTML.ToString(),
                        ContentTitle = parameters.Title,
                        CustomerID = context.User.CustomerID,
                        UseWYSIWYGeditor = parameters.UseWysiwigEditor,
                        Sharing = SharingFlagDisabled
                    };

                    Content.ReadyContent(content, false);
                    Content.Save(content, context.User);

                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    return GetSuccessResponse(context, "CONTENT CREATED", content.ContentID);
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetFailResponse(context, $"{parameters.Title} ALREADY EXISTS FOR CUSTOMER");
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "FOLDER DOES NOT EXIST FOR CUSTOMER");
        }

        public string AddMessage(WebMethodExecutionContext context, MessageParams parameters)
        {
            if (parameters.FolderId == null ||
                parameters.FolderId.Value == 0 ||
                Folder.Exists(parameters.FolderId.Value, context.User.CustomerID))
            {
                var folderId = parameters.FolderId == null ? 0 : parameters.FolderId.Value;

                if (!Layout.Exists(-1, parameters.LayoutName, folderId, context.User.CustomerID))
                {
                    var customer = Customer.GetByCustomerID(context.User.CustomerID, false);
                    if (parameters.MessageTypeId == null ||
                        parameters.MessageTypeId.Value == 0 ||
                        MessageType.Exists(parameters.MessageTypeId.Value, customer.BaseChannelID.Value))
                    {
                        if (Template.Exists(parameters.TemplateId, customer.BaseChannelID.Value))
                        {
                            FixTableBorderIfNeeded(parameters);

                            var layout = new CommunicatorEntities.Layout
                            {
                                LayoutName = parameters.LayoutName,
                                CustomerID = context.User.CustomerID,
                                CreatedUserID = context.User.UserID,
                                FolderID = parameters.FolderId,
                                TemplateID = parameters.TemplateId,
                                TableOptions = parameters.TableBorder,
                                DisplayAddress = parameters.Address
                            };

                            if (parameters.MessageTypeId != null && parameters.MessageTypeId.Value > 0)
                            {
                                layout.MessageTypeID = parameters.MessageTypeId.Value;
                            }

                            PopulateLayoutSlots(parameters, layout, false);
                            Layout.Save(layout, context.User);

                            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                            return GetSuccessResponse(context, "Message CREATED", Convert.ToInt32(layout.LayoutID));
                        }

                        context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                        return GetFailResponse(context, "TEMPLATE DOES NOT EXIST FOR CUSTOMER");
                    }

                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    return GetFailResponse(context, "MESSAGE TYPE DOES NOT EXIST FOR CUSTOMER");
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetFailResponse(context, string.Format("{0} ALREADY EXISTS FOR CUSTOMER", parameters.LayoutName));
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "FOLDER DOES NOT EXIST FOR CUSTOMER");
        }

        public string UpdateMessage(WebMethodExecutionContext context, MessageParams parameters)
        {
            var layout = Layout.GetByLayoutID(parameters.MessageId, context.User, false);
            if (layout != null)
            {
                var folderId = layout.FolderID == null ? 0 : layout.FolderID.Value;
                if (!Layout.Exists(parameters.MessageId, parameters.LayoutName, folderId, context.User.CustomerID))
                {
                    var customer = Customer.GetByCustomerID(context.User.CustomerID, false);
                    if (Template.Exists(parameters.TemplateId, customer.BaseChannelID.Value))
                    {
                        FixTableBorderIfNeeded(parameters);

                        layout.LayoutName = parameters.LayoutName;
                        layout.UpdatedUserID = context.User.UserID;
                        layout.TemplateID = parameters.TemplateId;
                        layout.TableOptions = parameters.TableBorder;
                        layout.DisplayAddress = parameters.Address;
                        layout.ContentSlot1 = parameters.Content0;
                        PopulateLayoutSlots(parameters, layout);
                        Layout.Save(layout, context.User);

                        context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                        return GetSuccessResponse(context, "Message UPDATED", Convert.ToInt32(layout.LayoutID));
                    }

                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    return GetFailResponse(context, "TEMPLATE DOES NOT EXIST FOR CUSTOMER");
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                var alreadyExistsMessage = $"{parameters.LayoutName} ALREADY EXISTS FOR CUSTOMER";
                return GetFailResponse(context, alreadyExistsMessage);
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "MESSAGE DOESN'T EXIST FOR CUSTOMER");
        }

        public string AddFolder(WebMethodExecutionContext context, FolderParams parameters)
        {
            if (parameters.ParentFolderId == null || parameters.ParentFolderId.Value == 0 ||
                Folder.Exists(parameters.ParentFolderId.Value, context.User.CustomerID))
            {
                var parentFolderId = parameters.ParentFolderId == null ? 0 : parameters.ParentFolderId.Value;

                if (!Folder.Exists(-1, parameters.FolderName, parentFolderId, context.User.CustomerID, "CNT"))
                {
                    var folder = new CommunicatorEntities.Folder
                    {
                        FolderName = parameters.FolderName,
                        FolderDescription = parameters.FolderDescription,
                        CustomerID = context.User.CustomerID,
                        CreatedUserID = context.User.UserID,
                        FolderType = CommunicatorObjects.Enums.FolderTypes.CNT.ToString(),
                        IsSystem = false,
                        ParentID = parentFolderId
                    };

                    Folder.Save(folder, context.User);
                    context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                    return GetSuccessResponse(context, "Folder CREATED", Convert.ToInt32(folder.FolderID));
                }

                context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
                return GetFailResponse(context, $"{parameters.FolderName} ALREADY EXISTS IN PARENT FOR CUSTOMER");
            }

            context.ApiLoggingManager.UpdateLog(context.ApiLogId, null);
            return GetFailResponse(context, "PARENT FOLDER DOES NOT EXIST FOR CUSTOMER");
        }

        private void FixTableBorderIfNeeded(MessageParams parameters)
        {
            if (parameters.TableBorder == "N")
            {
                parameters.TableBorder = string.Empty;
            }
            else
            {
                parameters.TableBorder = "border=1 bordercolor=black width=600 cellpadding=0 cellspacing=0";
            }
        }

        private void FixTitleIfNeeded(ContentParams parameters)
        {
            if (parameters.Title.IsNullOrWhiteSpace())
            {
                parameters.Title = $"CONTENT_{DateTime.Now.ToString(DefaultTitleDateTimeFormat)}";
            }
        }

        private void PopulateLayoutSlots(MessageParams parameters, CommunicatorEntities.Layout layout, bool skipFirstSlot = true)
        {
            if (!skipFirstSlot)
            {
                layout.ContentSlot1 = parameters.Content0;
            }
            if (parameters.Content1 != 0)
            {
                layout.ContentSlot2 = parameters.Content1;
            }
            if (parameters.Content2 != 0)
            {
                layout.ContentSlot3 = parameters.Content2;
            }
            if (parameters.Content3 != 0)
            {
                layout.ContentSlot4 = parameters.Content3;
            }
            if (parameters.Content4 != 0)
            {
                layout.ContentSlot5 = parameters.Content4;
            }
            if (parameters.Content5 != 0)
            {
                layout.ContentSlot6 = parameters.Content5;
            }
            if (parameters.Content6 != 0)
            {
                layout.ContentSlot7 = parameters.Content6;
            }
            if (parameters.Content7 != 0)
            {
                layout.ContentSlot8 = parameters.Content7;
            }
            if (parameters.Content8 != 0)
            {
                layout.ContentSlot9 = parameters.Content8;
            }
        }

        private void GetContentSearchValues(
            string XMLSearch,
            int customerID,
            ref string title,
            ref int? folderID,
            ref int? userID,
            ref DateTime? updatedDateFrom,
            ref DateTime? updatedDateTo)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XMLSearch);

            var node = xmlDoc.SelectSingleNode("//SearchFields");
            if (node != null)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Title":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                title = childNode.InnerText.Trim();
                            }
                            break;
                        case "User":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                var user = KMPlatform.BusinessLogic.User.GetByUserName(childNode.InnerText.Trim(), customerID, false);
                                if (user != null)
                                {
                                    userID = user.UserID;
                                }
                            }
                            break;
                        case "Folder":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                if (Folder.Exists(Convert.ToInt32(childNode.InnerText.Trim()), customerID))
                                {
                                    folderID = Convert.ToInt32(childNode.InnerText.Trim());
                                }
                            }
                            break;
                        case "ModifiedFrom":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                updatedDateFrom = Convert.ToDateTime(childNode.InnerText.Trim());
                            }
                            break;
                        case "ModifiedTo":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                updatedDateTo = Convert.ToDateTime(childNode.InnerText.Trim());
                            }
                            break;
                    }
                }
            }
        }

        private void GetLayoutSearchValues(
            string XMLSearch,
            int customerID,
            ref string title,
            ref int? folderID,
            ref int? userID,
            ref DateTime? updatedDateFrom,
            ref DateTime? updatedDateTo)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XMLSearch);

            var node = xmlDoc.SelectSingleNode("//SearchFields");
            if (node != null)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Title":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                title = childNode.InnerText.Trim();
                            }
                            break;
                        case "User":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                var user = KMPlatform.BusinessLogic.User.GetByUserName(childNode.InnerText.Trim(), customerID, false);
                                if (user != null)
                                {
                                    userID = user.UserID;
                                }
                            }
                            break;
                        case "Folder":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                if (Folder.Exists(Convert.ToInt32(childNode.InnerText.Trim()), customerID))
                                {
                                    folderID = Convert.ToInt32(childNode.InnerText.Trim());
                                }
                            }
                            break;
                        case "ModifiedFrom":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                updatedDateFrom = Convert.ToDateTime(childNode.InnerText.Trim());
                            }
                            break;
                        case "ModifiedTo":
                            if (childNode.InnerText.Trim().Length > 0)
                            {
                                updatedDateTo = Convert.ToDateTime(childNode.InnerText.Trim());
                            }
                            break;
                    }
                }
            }
        }

        private string BuildContentReturnXML(List<CommunicatorEntities.Content> contentList)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<DocumentElement xmlns=\"\">");
            foreach (var content in contentList)
            {
                stringBuilder.Append("<Content><ContentID>");
                stringBuilder.Append(content.ContentID);
                stringBuilder.Append("</ContentID><FolderID>");
                if (content.FolderID != null)
                {
                    stringBuilder.Append(content.FolderID.Value);
                }
                stringBuilder.Append("</FolderID><ContentTitle>");
                stringBuilder.Append("<![CDATA[");
                stringBuilder.Append(content.ContentTitle);
                stringBuilder.Append("]]>");
                stringBuilder.Append("</ContentTitle><ModifyDate>");
                if (content.UpdatedDate != null)
                {
                    stringBuilder.Append(content.UpdatedDate.Value);
                }
                else if (content.CreatedDate != null)
                {
                    stringBuilder.Append(content.CreatedDate.Value);
                }
                stringBuilder.Append("</ModifyDate><ContentSource><![CDATA[");
                stringBuilder.Append(CleanContentSource(content.ContentSource));
                stringBuilder.Append("]]></ContentSource><ContentText><![CDATA[");
                stringBuilder.Append(content.ContentText);
                stringBuilder.Append("]]></ContentText></Content>");
            }
            stringBuilder.Append("</DocumentElement>");
            return stringBuilder.ToString();
        }

        private string BuildLayoutReturnXML(List<CommunicatorEntities.Layout> layoutList)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<DocumentElement xmlns=\"\">");
            foreach (var layout in layoutList)
            {
                stringBuilder.Append("<Layout><LayoutID>");
                stringBuilder.Append(layout.LayoutID);
                stringBuilder.Append("</LayoutID><FolderID>");
                if (layout.FolderID != null)
                    stringBuilder.Append(layout.FolderID.Value);
                stringBuilder.Append("</FolderID><TemplateID>");
                if (layout.TemplateID != null)
                    stringBuilder.Append(layout.TemplateID.Value);
                stringBuilder.Append("</TemplateID><LayoutName>");
                stringBuilder.Append("<![CDATA[");
                stringBuilder.Append(layout.LayoutName);
                stringBuilder.Append("]]>");
                stringBuilder.Append("</LayoutName><ModifyDate>");
                if (layout.UpdatedDate != null)
                    stringBuilder.Append(layout.UpdatedDate.Value);
                else if (layout.CreatedDate != null)
                    stringBuilder.Append(layout.CreatedDate.Value);
                stringBuilder.Append("</ModifyDate><ContentSlot1>");
                if (layout.ContentSlot1 != null)
                    stringBuilder.Append(layout.ContentSlot1.Value);
                stringBuilder.Append("</ContentSlot1><ContentSlot2>");
                if (layout.ContentSlot2 != null)
                    stringBuilder.Append(layout.ContentSlot2.Value);
                stringBuilder.Append("</ContentSlot2><ContentSlot3>");
                if (layout.ContentSlot3 != null)
                    stringBuilder.Append(layout.ContentSlot3.Value);
                stringBuilder.Append("</ContentSlot3><ContentSlot4>");
                if (layout.ContentSlot4 != null)
                    stringBuilder.Append(layout.ContentSlot4.Value);
                stringBuilder.Append("</ContentSlot4><ContentSlot5>");
                if (layout.ContentSlot5 != null)
                    stringBuilder.Append(layout.ContentSlot5.Value);
                stringBuilder.Append("</ContentSlot5><ContentSlot6>");
                if (layout.ContentSlot6 != null)
                    stringBuilder.Append(layout.ContentSlot6.Value);
                stringBuilder.Append("</ContentSlot6><ContentSlot7>");
                if (layout.ContentSlot7 != null)
                    stringBuilder.Append(layout.ContentSlot7.Value);
                stringBuilder.Append("</ContentSlot7><ContentSlot8>");
                if (layout.ContentSlot8 != null)
                    stringBuilder.Append(layout.ContentSlot8.Value);
                stringBuilder.Append("</ContentSlot8><ContentSlot9>");
                if (layout.ContentSlot9 != null)
                    stringBuilder.Append(layout.ContentSlot9.Value);
                stringBuilder.Append("</ContentSlot9></Layout>");
            }
            stringBuilder.Append("</DocumentElement>");
            return stringBuilder.ToString();
        }

        private string BuildFolderReturnXML(List<CommunicatorEntities.Folder> folderList)
        {
            var folderXmlStringBuilder = new StringBuilder();
            folderXmlStringBuilder.Append("<DocumentElement xmlns=\"\">");
            foreach (CommunicatorEntities.Folder folder in folderList)
            {
                folderXmlStringBuilder
                    .Append("<Folder>")
                        .Append("<FolderID>")
                            .Append(folder.FolderID)
                        .Append("</FolderID>")
                        .Append("<FolderName>")
                            .Append("<![CDATA[")
                                .Append(folder.FolderName)
                            .Append("]]>")
                        .Append("</FolderName>")
                    .Append("</Folder>");
            }
            folderXmlStringBuilder.Append("</DocumentElement>");
            return folderXmlStringBuilder.ToString();
        }

        private string BuildTemplateReturnXML(List<CommunicatorEntities.Template> templateList)
        {
            var templateXmlStringBuilder = new StringBuilder();
            templateXmlStringBuilder.Append("<DocumentElement xmlns=\"\">");
            foreach (CommunicatorEntities.Template template in templateList)
            {
                templateXmlStringBuilder
                    .Append("<Template>")
                        .Append("<TemplateID>")
                            .Append(template.TemplateID)
                        .Append("</TemplateID>")
                        .Append("<TemplateName>")
                            .Append("<![CDATA[")
                                .Append(template.TemplateName)
                            .Append("]]>")
                        .Append("</TemplateName>")
                    .Append("</Template>");
            }

            templateXmlStringBuilder.Append("</DocumentElement>");
            return templateXmlStringBuilder.ToString();
        }

        private string BuildMessageTypeReturnXML(List<CommunicatorEntities.MessageType> messageTypeList)
        {
            var messageTypeXmlStringBuilder = new StringBuilder();
            messageTypeXmlStringBuilder.Append("<DocumentElement xmlns=\"\">");
            foreach (CommunicatorEntities.MessageType messageType in messageTypeList)
            {
                messageTypeXmlStringBuilder
                    .Append("<MessageType>")
                        .Append("<MessageTypeID>")
                            .Append(messageType.MessageTypeID)
                        .Append("</MessageTypeID>")
                        .Append("<MessageTypeName>")
                            .Append("<![CDATA[")
                                .Append(messageType.Name)
                            .Append("]]>")
                        .Append("</MessageTypeName>")
                    .Append("</MessageType>");
            }

            messageTypeXmlStringBuilder.Append("</DocumentElement>");
            return messageTypeXmlStringBuilder.ToString();
        }

        private string BuildCustomerDepartmentReturnXML(List<AccountEntities.CustomerDepartment> departmentList)
        {
            var departmentXmlStringBuilder = new StringBuilder();
            departmentXmlStringBuilder.Append("<DocumentElement xmlns=\"\">");
            foreach (AccountEntities.CustomerDepartment department in departmentList)
            {
                departmentXmlStringBuilder
                    .Append("<Department>")
                        .Append("<DepartmentID>")
                            .Append(department.DepartmentID)
                        .Append("</DepartmentID>")
                        .Append("<DepartmentName>")
                            .Append(department.DepartmentName)
                        .Append("</DepartmentName>")
                    .Append("</Department>");
            }

            departmentXmlStringBuilder.Append("</DocumentElement>");
            return departmentXmlStringBuilder.ToString();
        }

        private string CleanContentSource(string input)
        {
            input = RemoveCdataSections(input);
            input = RemoveTroublesomeCharacters(input);
            return input;
        }

        private string RemoveCdataSections(string input)
        {
            return Regex.Replace(input, CDataSectionRegex, CDataSectionReplacement);
        }

        private string RemoveTroublesomeCharacters(string inputString)
        {
            if (inputString == null)
            {
                return null;
            }

            var resultStringBuilder = new StringBuilder();

            foreach (var characterToExamine in inputString)
            {
                if (XmlConvert.IsXmlChar(characterToExamine))
                {
                    resultStringBuilder.Append(characterToExamine);
                }
            }
            return resultStringBuilder.ToString();
        }
    }
}