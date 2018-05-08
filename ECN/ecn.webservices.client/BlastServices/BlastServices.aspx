<%@ Page Title="Home Page" Language="C#" validateRequest="false" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="BlastServices.aspx.cs" Inherits="ecn.webservices.client.BlastServices.BlastServices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Src="~/BlastServices/AddRegularBlast.ascx" TagName="AddRegularBlast" TagPrefix="uc1" %>
<%@ Register Src="~/BlastServices/AddRegularScheduledBlast.ascx" TagName="AddRegularScheduledBlast" TagPrefix="uc2" %>
<%@ Register Src="~/BlastServices/AddRegularScheduledAdvancedBlast.ascx" TagName="AddRegularScheduledAdvancedBlast" TagPrefix="uc14" %>
<%@ Register Src="~/BlastServices/GetBlastReport.ascx" TagName="GetBlastReport" TagPrefix="uc3" %>
<%@ Register Src="~/BlastServices/UpdateBlast.ascx" TagName="UpdateBlast" TagPrefix="uc4" %>
<%@ Register Src="~/BlastServices/DeleteBlast.ascx" TagName="DeleteBlast" TagPrefix="uc5" %>
<%@ Register Src="~/BlastServices/GetBlast.ascx" TagName="GetBlast" TagPrefix="uc6" %>
<%@ Register Src="~/BlastServices/SearchForBlast.ascx" TagName="SearchForBlast" TagPrefix="uc7" %>
<%@ Register Src="~/BlastServices/GetBlastReportByISP.ascx" TagName="GetBlastReportByISP" TagPrefix="uc8" %>
<%@ Register Src="~/BlastServices/GetBlastOpensReport.ascx" TagName="GetBlastOpensReport" TagPrefix="uc9" %>
<%@ Register Src="~/BlastServices/GetBlastClicksReport.ascx" TagName="GetBlastClicksReport" TagPrefix="uc10" %>
<%@ Register Src="~/BlastServices/GetBlastBounceReport.ascx" TagName="GetBlastBounceReport" TagPrefix="uc11" %>
<%@ Register Src="~/BlastServices/GetBlastUnsubscribeReport.ascx" TagName="GetBlastUnsubscribeReport" TagPrefix="uc12" %>
<%@ Register Src="~/BlastServices/GetBlastDeliveryReport.ascx" TagName="GetBlastDeliveryReport" TagPrefix="uc13" %>
<%@ Register Src="~/BlastServices/GetSubscriberCount.ascx" TagName="GetSubscriberCount" TagPrefix="uc14" %>
    
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0"
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
        ContentCssClass="accordionContent" FadeTransitions="false" FramesPerSecond="40" 
        TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
        <Panes>
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header><a href="" class="accordionLink">1. Add Blast</a></Header>
                <Content>
                     <p>
                        <uc1:AddRegularBlast ID="AddRegularBlast" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>  
            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header><a href="" class="accordionLink">2. Add Scheduled Blast</a></Header>
                <Content>
                     <p>
                        <uc2:AddRegularScheduledBlast ID="AddRegularScheduledBlast" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane> 
            <ajaxToolkit:AccordionPane ID="AccordionPane14" runat="server">
                <Header><a href="" class="accordionLink">3. Add Scheduled Advanced Blast</a></Header>
                <Content>
                     <p>
                        <uc14:AddRegularScheduledAdvancedBlast ID="AddRegularScheduledAdvancedBlast" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                <Header><a href="" class="accordionLink">4. Get Blast Report</a></Header>
                <Content>
                     <p>
                        <uc3:GetBlastReport ID="GetBlastReport" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane> 
            <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                <Header><a href="" class="accordionLink">5. Update Blast</a></Header>
                <Content>
                     <p>
                        <uc4:UpdateBlast ID="UpdateBlast" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane5" runat="server">
                <Header><a href="" class="accordionLink">6. Delete Blast</a></Header>
                <Content>
                     <p>
                        <uc5:DeleteBlast ID="DeleteBlast" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane6" runat="server">
                <Header><a href="" class="accordionLink">7. Get Blast</a></Header>
                <Content>
                     <p>
                        <uc6:GetBlast ID="GetBlast" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane7" runat="server">
                <Header><a href="" class="accordionLink">8. Search For Blast</a></Header>
                <Content>
                     <p>
                        <uc7:SearchForBlast ID="SearchForBlast" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane8" runat="server">
                <Header><a href="" class="accordionLink">9. Get Blast Report By ISP</a></Header>
                <Content>
                     <p>
                        <uc8:GetBlastReportByISP ID="GetBlastReportByISP" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane9" runat="server">
                <Header><a href="" class="accordionLink">10. Get Blast Opens Report</a></Header>
                <Content>
                     <p>
                        <uc9:GetBlastOpensReport ID="GetBlastOpensReport" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane10" runat="server">
                <Header><a href="" class="accordionLink">11. Get Blast Clicks Report</a></Header>
                <Content>
                     <p>
                        <uc10:GetBlastClicksReport ID="GetBlastClicksReport" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane11" runat="server">
                <Header><a href="" class="accordionLink">12. Get Blast Bounce Report</a></Header>
                <Content>
                     <p>
                        <uc11:GetBlastBounceReport ID="GetBlastBounceReport" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane12" runat="server">
                <Header><a href="" class="accordionLink">13. Get Blast Unsubscribe Report</a></Header>
                <Content>
                     <p>
                        <uc12:GetBlastUnsubscribeReport ID="GetBlastUnsubscribeReport" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane13" runat="server">
                <Header><a href="" class="accordionLink">14. Get Blast Delivery Report</a></Header>
                <Content>
                     <p>
                        <uc13:GetBlastDeliveryReport ID="GetBlastDeliveryReport" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>            
            <ajaxToolkit:AccordionPane ID="AccordionPane15" runat="server">
                <Header><a href="" class="accordionLink">16. Get Subscriber Count</a></Header>
                <Content>
                     <p>
                        <uc14:GetSubscriberCount ID="GetSubscriberCount" runat="server" />
                   </p>
                </Content>
            </ajaxToolkit:AccordionPane>
        </Panes>
    </ajaxToolkit:Accordion>
</asp:Content>

