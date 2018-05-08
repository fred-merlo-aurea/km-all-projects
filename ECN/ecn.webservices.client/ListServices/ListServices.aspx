<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="ListServices.aspx.cs" Inherits="ecn.webservices.client.ListServices.ListServices" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register Src="~/ListServices/LookupCirculation.ascx" TagName="LookupCirculation" TagPrefix="uc1" %>--%>
<%@ Register Src="~/ListServices/GetListEmailProfilesByEmailAddress.ascx" TagName="GetListEmailProfilesByEmailAddress"
    TagPrefix="uc2" %>
<%@ Register Src="~/ListServices/GetListFolders.ascx" TagName="GetListFolders" TagPrefix="uc3" %>
<%@ Register Src="~/ListServices/AddListToFolder.ascx" TagName="AddListToFolder"
    TagPrefix="uc4" %>
<%@ Register Src="~/ListServices/AddCustomField.ascx" TagName="AddCustomField" TagPrefix="uc5" %>
<%@ Register Src="~/ListServices/AddSubscribers.ascx" TagName="AddSubscribers" TagPrefix="uc6" %>
<%@ Register Src="~/ListServices/AddSubscriberUsingSmartForm.ascx" TagName="AddSubscribersSF"
    TagPrefix="uc7" %>
<%@ Register Src="~/ListServices/UpdateList.ascx" TagName="UpdateList" TagPrefix="uc8" %>
<%@ Register Src="~/ListServices/DeleteList.ascx" TagName="DeleteList" TagPrefix="uc9" %>
<%@ Register Src="~/ListServices/GetListByName.ascx" TagName="GetListByName" TagPrefix="uc10" %>
<%@ Register Src="~/ListServices/DeleteFolder.ascx" TagName="DeleteFolder" TagPrefix="uc11" %>
<%@ Register Src="~/ListServices/DeleteSubscriber.ascx" TagName="DeleteSubscriber"
    TagPrefix="uc12" %>
<%@ Register Src="~/ListServices/DeleteCustomField.ascx" TagName="DeleteCustomField"
    TagPrefix="uc13" %>
<%@ Register Src="~/ListServices/UpdateCustomField.ascx" TagName="UpdateCustomField"
    TagPrefix="uc14" %>
<%@ Register Src="~/ListServices/UnsubscribeSubscriber.ascx" TagName="UnsubscribeSubscriber"
    TagPrefix="uc15" %>
<%@ Register Src="~/ListServices/GetCustomFields.ascx" TagName="GetCustomFields"
    TagPrefix="uc16" %>
<%@ Register Src="~/ListServices/AddToMasterSuppressionList.ascx" TagName="AddToMasterSuppressionList"
    TagPrefix="uc17" %>
<%@ Register Src="~/ListServices/AddFolder.ascx" TagName="AddFolder" TagPrefix="uc18" %>
<%@ Register Src="~/ListServices/GetLists.ascx" TagName="GetLists" TagPrefix="uc19" %>
<%@ Register Src="~/ListServices/UpdateEmailAddress.ascx" TagName="UpdateEmailAddress"
    TagPrefix="uc20" %>
<%@ Register Src="~/ListServices/AddSubscribersWithDupes.ascx" TagName="AddSubscribersWithDupes"
    TagPrefix="uc21" %>
<%@ Register Src="~/ListServices/GetSubscriberStatus.ascx" TagName="GetSubscriberStatus"
    TagPrefix="uc22" %>
<%@ Register Src="~/ListServices/GetSubscriberCount.ascx" TagName="GetSubscriberCount"
    TagPrefix="uc23" %>
<%@ Register Src="~/ListServices/GetFilters.ascx" TagName="GetFilters"
    TagPrefix="uc24" %>
<%@ Register Src="~/ListServices/GetListByFolder.ascx" TagName="GetListByFolder"
    TagPrefix="uc25" %>
<%@ Register Src="~/ListServices/AddSubscribersGenerateUDF.ascx" TagName="AddSubscribersGenerateUDF"
    TagPrefix="uc26" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<style type="text/css">    
    .accordionLink {         
        margin: 0px;
        padding: 0px;
        color: blue;
    }  
</style>  
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        FadeTransitions="false" FramesPerSecond="40" TransitionDuration="250" AutoSize="None"
        RequireOpenedPane="false" SuppressHeaderPostbacks="true">
        <Panes>
            <%--<ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header><a href="" class="accordionLink">1. Lookup Circulation</a></Header>
                <Content>
                     <p>
                        <uc1:LookupCirculation ID="LookupCirculation" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>--%>
            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                    <a href="" class="accordionLink">2. Get List Email Profiles By EmailAddress</a></Header>
                <Content>
                    <p>
                        <uc2:GetListEmailProfilesByEmailAddress ID="GetListEmailProfilesByEmailAddress" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                <Header>
                    <a href="" class="accordionLink">3. Get List Folders</a></Header>
                <Content>
                    <p>
                        <uc3:GetListFolders ID="GetListFolders" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                <Header>
                    <a href="" class="accordionLink">4. Add List To Folder</a></Header>
                <Content>
                    <p>
                        <uc4:AddListToFolder ID="AddListToFolder" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane5" runat="server">
                <Header>
                    <a href="" class="accordionLink">5. Add Custom Field</a></Header>
                <Content>
                    <p>
                        <uc5:AddCustomField ID="AddCustomField" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane6" runat="server">
                <Header>
                    <a href="" class="accordionLink">6. Add Subscribers</a></Header>
                <Content>
                    <p>
                        <uc6:AddSubscribers ID="AddSubscribers" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane7" runat="server">
                <Header>
                    <a href="" class="accordionLink">7. Add Subscribers using SmartForm</a></Header>
                <Content>
                    <p>
                        <uc7:AddSubscribersSF ID="AddSubscribersSF" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane8" runat="server">
                <Header>
                    <a href="" class="accordionLink">8. Update List</a></Header>
                <Content>
                    <p>
                        <uc8:UpdateList ID="UpdateList" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane9" runat="server">
                <Header>
                    <a href="" class="accordionLink">9. Delete List</a></Header>
                <Content>
                    <p>
                        <uc9:DeleteList ID="DeleteList" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane10" runat="server">
                <Header>
                    <a href="" class="accordionLink">10. Get List By Name</a></Header>
                <Content>
                    <p>
                        <uc10:GetListByName ID="GetListByName" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane11" runat="server">
                <Header>
                    <a href="" class="accordionLink">11. Delete Folder</a></Header>
                <Content>
                    <p>
                        <uc11:DeleteFolder ID="DeleteFolder" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane12" runat="server">
                <Header>
                    <a href="" class="accordionLink">12. Delete Subscriber</a></Header>
                <Content>
                    <p>
                        <uc12:DeleteSubscriber ID="DeleteSubscriber" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane13" runat="server">
                <Header>
                    <a href="" class="accordionLink">13. Delete Custom Field</a></Header>
                <Content>
                    <p>
                        <uc13:DeleteCustomField ID="DeleteCustomField" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane14" runat="server">
                <Header>
                    <a href="" class="accordionLink">14. Update Custom Field</a></Header>
                <Content>
                    <p>
                        <uc14:UpdateCustomField ID="UpdateCustomField" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane15" runat="server">
                <Header>
                    <a href="" class="accordionLink">15. Unsubscribe Subscriber</a></Header>
                <Content>
                    <p>
                        <uc15:UnsubscribeSubscriber ID="UnsubscribeSubscriber" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane16" runat="server">
                <Header>
                    <a href="" class="accordionLink">16. Get Custom Fields</a></Header>
                <Content>
                    <p>
                        <uc16:GetCustomFields ID="GetCustomFields" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane17" runat="server">
                <Header>
                    <a href="" class="accordionLink">17. Add Emails To Master Suppression List</a></Header>
                <Content>
                    <p>
                        <uc17:AddToMasterSuppressionList ID="AddToMasterSuppressionList" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane18" runat="server">
                <Header>
                    <a href="" class="accordionLink">18. Add Folder</a></Header>
                <Content>
                    <p>
                        <uc18:AddFolder ID="AddFolder" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    <a href="" class="accordionLink">19. Get Lists</a></Header>
                <Content>
                    <p>
                        <uc19:GetLists ID="GetLists" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane20" runat="server">
                <Header>
                    <a href="" class="accordionLink">20. Update Email Address</a></Header>
                <Content>
                    <p>
                        <uc20:UpdateEmailAddress ID="UpdateEmailAddress1" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane21" runat="server">
                <Header>
                    <a href="" class="accordionLink">21. Add Subscribers With Dupes</a></Header>
                <Content>
                    <p>
                        <uc21:AddSubscribersWithDupes ID="AddSubscribersWithDupes" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane22" runat="server">
                <Header>
                    <a href="" class="accordionLink">22. Get Subscriber Status</a></Header>
                <Content>
                    <p>
                        <uc22:GetSubscriberStatus ID="GetSubscriberStatus" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane23" runat="server">
                <Header>
                    <a href="" class="accordionLink">23. Get Subscriber Count</a></Header>
                <Content>
                    <p>
                        <uc23:GetSubscriberCount ID="GetSubscriberCount" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane24" runat="server">
                <Header>
                    <a href="" class="accordionLink">24. Get Filters</a></Header>
                <Content>
                    <p>
                        <uc24:GetFilters ID="GetFilters" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane25" runat="server">
                <Header>
                    <a href="" class="accordionLink">25. Get List By Folder</a></Header>
                <Content>
                    <p>
                        <uc25:GetListByFolder ID="GetListByFolder" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            
            <ajaxToolkit:AccordionPane ID="AccordionPane26" runat="server">
                <Header>
                    <a href="" class="accordionLink">26. Add Subscriber Auto GenerateUDFs</a></Header>
                <Content>
                    <p>
                        <uc26:AddSubscribersGenerateUDF ID="AddSubscribersGenerateUDF" runat="server" />
                    </p>
                </Content>
            </ajaxToolkit:AccordionPane>

        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>
