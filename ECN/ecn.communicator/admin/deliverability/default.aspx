<%@ Page Language="c#" Inherits="ecn.communicator.admin.deliverability._default"
    CodeBehind="default.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 250px;
        }
        .style1
        {
            width: 100%;
        }
        .overlay
        {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: gray;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
        }
        * html .overlay
        {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }
        .loader
        {
            z-index: 100;
            position: fixed;
            width: 120px;
            margin-left: -60px;
            background-color: #F4F3E1;
            font-size: x-small;
            color: black;
            border: solid 2px Black;
            top: 40%;
            left: 50%;
        }
        * html .loader
        {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
        }
    </style>
     <%--   <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/themes/base/jquery-ui.css"
        type="text/css" media="all" />
    <link rel="stylesheet" href="http://static.jquery.com/ui/css/demo-docs-theme/ui.theme.css"
        type="text/css" media="all" />--%>
  <%--  <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/jquery-ui.min.js"
        type="text/javascript"></script>--%>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script type="text/javascript">
     Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

         $("#<%=startdate.ClientID%>").datepicker({
             defaultDate: "-1d",
             changeMonth: true,
             numberOfMonths: 1,
             onSelect: function (selectedDate) {
                 $("#<%=enddate.ClientID%>").datepicker("option", "minDate", selectedDate);
             }
         });
         $("#<%=enddate.ClientID%>").datepicker({
             changeMonth: true,
             numberOfMonths: 1,
             onSelect: function (selectedDate) {
                 $("#<%=startdate.ClientID%>").datepicker("option", "maxDate", selectedDate);
             }
         });
         $("#<%=startdateIP.ClientID%>").datepicker({
             defaultDate: "-1d",
             changeMonth: true,
             numberOfMonths: 1,
             onSelect: function (selectedDate) {
                 $("#<%=enddateIP.ClientID%>").datepicker("option", "minDate", selectedDate);
             }
         });
         $("#<%=enddateIP.ClientID%>").datepicker({
             changeMonth: true,
             numberOfMonths: 1,
             onSelect: function (selectedDate) {
                 $("#<%=startdateIP.ClientID%>").datepicker("option", "maxDate", selectedDate);
             }
         });
     });    
    </script>
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="update1" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                         <div>
                    <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <div style="float: left; width: 100%;">
                <asp:Label runat="server" ID="Label1" ForeColor="Red" Font-Bold="True"></asp:Label>
                <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%">
                    <cc1:TabPanel runat="server" HeaderText="By Customer" ID="TabPanel1">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblRptHeader" Font-Bold="True" Text="Email Deliverability by Customer"
                                Font-Size="Medium"></asp:Label>
                            <br />
                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <div id="content">
                                            From
                                            <asp:TextBox ID="startdate" class="field" runat="server"></asp:TextBox>
                                            To
                                            <asp:TextBox ID="enddate" class="field" runat="server"></asp:TextBox>
                                            &nbsp
                                            <asp:Button runat="server" Text="Submit" ID="btnSubmitCust" OnClick="btnSubmitCust_Click"
                                                class="formbuttonsmall" />
                                        </div>
                                    </td>
                                    <td align="right">
                                        Bounce Threshold
                                    </td>
                                    <td align="right">
                                        <asp:DropDownList ID="dropdownView" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropdownView_SelectedIndexChanged"
                                            CssClass="formfield">
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="gvBlasts" runat="server" ForeColor="Black" CssClass="gridWizard"
                                Width="100%" AutoGenerateColumns="False" PageSize="30" OnSorting="gvBlasts_Sorting"
                                AllowSorting="true" AllowPaging="True" OnPageIndexChanging="gvBlasts_PageIndexChanging">
                                <AlternatingRowStyle CssClass="gridaltrowWizard" />
                                <Columns>
                                    <asp:TemplateField HeaderText="CustID" SortExpression="CustomerID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbCustomerID" runat="server" Text='<%# Eval("CustomerID") %>'
                                                CommandArgument='<%# Eval("CustomerID") %>' OnCommand="lnkReport_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CustomerName" SortExpression="CustomerName">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("CustomerName") %>' runat="server" ID="gvlblCustomerName"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TotalSent" SortExpression="TotalSent">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("TotalSent") %>' runat="server" ID="gvlblTotalSent"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hard" SortExpression="HardBounces">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("HardBounces") %>' runat="server" ID="gvlblHardBounces"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="HBPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("HBPerc") %>' runat="server" ID="gvlblHBPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Soft" SortExpression="SoftBounces">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("SoftBounces") %>' runat="server" ID="gvlblSoftBounces"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="SBPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("SBPerc") %>' runat="server" ID="gvlblSBPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Block" SortExpression="MailBlock">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("MailBlock") %>' runat="server" ID="gvlblMailBlock"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="MBPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("MBPerc") %>' runat="server" ID="gvlblMBPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Complaint" SortExpression="Complaint">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("Complaint") %>' runat="server" ID="gvlblComplaint"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="ComPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("ComPerc") %>' runat="server" ID="gvlblComPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OptOut" SortExpression="OptOut">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("OptOut") %>' runat="server" ID="gvlblOptOut"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="OptOPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("OptOPerc") %>' runat="server" ID="gvlblOptOPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MasterSupp" SortExpression="MasterSupp">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("MasterSupp") %>' runat="server" ID="gvlblMasterSupp"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="MSPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("MSPerc") %>' runat="server" ID="gvlblMSPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" />
                                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="Gray" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#383838" />
                            </asp:GridView>
                            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                            <cc1:ModalPopupExtender ID="mdlPopup" runat="server" TargetControlID="btnShowPopup"
                                PopupControlID="pnlPopup" BackgroundCssClass="modalBackground" DynamicServicePath=""
                                Enabled="True" />
                            <asp:Panel ID="pnlError" runat="server" Visible="False">
                                <asp:Label ID="lblError" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                            </asp:Panel>
                            <asp:Panel ID="pnlPopup" runat="server" Width="950px" Style="display: none" CssClass="modalPopup">
                                <asp:Label runat="server" ID="lblSubRptHeader" Font-Bold="True" Text="Email Deliverability by Blasts"
                                    Font-Size="Small"></asp:Label>
                                <br />
                                <br />
                                <asp:GridView ID="gvSubBlasts" runat="server" ForeColor="Black" CssClass="gridWizard"
                                    OnRowDataBound="gvSubBlasts_RowDataBound" AutoGenerateColumns="False" Width="100%">
                                    <AlternatingRowStyle CssClass="gridaltrowWizard" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="CustID">
                                            <ItemTemplate>
                                                <asp:Label ID="gvlblCustomerID" runat="server" Text='<%# Eval("CustomerID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CustomerName">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("CustomerName") %>' runat="server" ID="gvlblCustomerNameSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BlastID">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="gvlblBlastID" runat="server" Text='<%# Eval("BlastID") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TotalSent">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("TotalSent") %>' runat="server" ID="gvlblTotalSentSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hard">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("HardBounces") %>' runat="server" ID="gvlblHardBouncesSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("HBPerc") %>' runat="server" ID="gvlblHBPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Soft">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("SoftBounces") %>' runat="server" ID="gvlblSoftBouncesSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("SBPerc") %>' runat="server" ID="gvlblSBPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Block">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("MailBlock") %>' runat="server" ID="gvlblMailBlockSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("MBPerc") %>' runat="server" ID="gvlblMBPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Complaint">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("Complaint") %>' runat="server" ID="gvlblComplaintSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("ComPerc") %>' runat="server" ID="gvlblComPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OptOut">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("OptOut") %>' runat="server" ID="gvlblOptOutSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("OptOPerc") %>' runat="server" ID="gvlblOptOPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MasterSupp">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("MasterSupp") %>' runat="server" ID="gvlblMasterSuppSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("MSPerc") %>' runat="server" ID="gvlblMSPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="Gray" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#383838" />
                                </asp:GridView>
                                <div align="center" style="width: 100%">
                                    <br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnClose" runat="server" CssClass="button" OnClick="btnClose_Click"
                                        Text="Close" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                                <br />
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="By IP" ID="TabPanel2">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblIPRptHeader" Font-Bold="True" Text="Email Deliverability by IP"
                                Font-Size="Medium"></asp:Label>
                            <br />
                            <br />
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <div id="Div1">
                                            From
                                            <asp:TextBox ID="startdateIP" class="field" runat="server"></asp:TextBox>
                                            To
                                            <asp:TextBox ID="enddateIP" class="field" runat="server"></asp:TextBox>
                                            IPFilter
                                            <asp:TextBox ID="IPFilter" class="field" runat="server"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="startdate_TextBoxWatermarkExtender" runat="server"
                                                Enabled="True" TargetControlID="IPFilter" WatermarkText="Optional">
                                            </cc1:TextBoxWatermarkExtender>
                                            &nbsp
                                            <asp:Button runat="server" Text="Submit" ID="btnSubmitIP" OnClick="btnSubmitIP_Click"
                                                class="formbuttonsmall" />
                                        </div>
                                    </td>
                                    <td align="right">
                                        Bounce Threshold
                                    </td>
                                    <td align="right">
                                        <asp:DropDownList ID="dropdownViewIP" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropdownViewIP_SelectedIndexChanged"
                                            CssClass="formfield">
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="gvBlastsIP" runat="server" ForeColor="Black" CssClass="gridWizard"
                                PageSize="30" Width="100%" AutoGenerateColumns="False" OnSorting="gvBlastsIP_Sorting"
                                AllowSorting="true" AllowPaging="True" OnPageIndexChanging="gvBlastsIP_PageIndexChanging">
                                <AlternatingRowStyle CssClass="gridaltrowWizard" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SourceIP" SortExpression="SourceIP">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSourceIP" runat="server" Text='<%# Eval("SourceIP") %>' CommandArgument='<%# Eval("SourceIP") %>'
                                                OnCommand="lnkIPReport_Command" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HostName" SortExpression="HostName">
                                        <ItemTemplate>
                                            <asp:Label ID="lbHostName" runat="server" Text='<%# Eval("HostName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TotalSent" SortExpression="TotalSent">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("TotalSent") %>' runat="server" ID="gvlblIPTotalSent"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hard" SortExpression="HardBounces">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("HardBounces") %>' runat="server" ID="gvlblIPHardBounces"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="HBPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("HBPerc") %>' runat="server" ID="gvlblIPHBPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Soft" SortExpression="SoftBounces">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("SoftBounces") %>' runat="server" ID="gvlblIPSoftBounces"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="SBPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("SBPerc") %>' runat="server" ID="gvlblIPSBPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Block" SortExpression="MailBlock">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("MailBlock") %>' runat="server" ID="gvlblIPMailBlock"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="MBPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("MBPerc") %>' runat="server" ID="gvlblIPMBPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Complaint" SortExpression="Complaint">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("Complaint") %>' runat="server" ID="gvlblIPComplaint"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="ComPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("ComPerc") %>' runat="server" ID="gvlblIPComPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OptOut" SortExpression="OptOut">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("OptOut") %>' runat="server" ID="gvlblIPOptOut"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="OptOPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("OptOPerc") %>' runat="server" ID="gvlblIPOptOPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MasterSupp" SortExpression="MasterSupp">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("MasterSupp") %>' runat="server" ID="gvlblIPMasterSupp"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perc (%)" SortExpression="MSPerc">
                                        <ItemTemplate>
                                            <asp:Label Text='<%#Eval("MSPerc") %>' runat="server" ID="gvlblIPMSPerc"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" />
                                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="Gray" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#383838" />
                            </asp:GridView>
                            <asp:Button ID="btnIPShowPopup" runat="server" Style="display: none" />
                            <cc1:ModalPopupExtender ID="mdlPopupIP" runat="server" TargetControlID="btnIPShowPopup"
                                PopupControlID="pnlIPPopup" BackgroundCssClass="modalBackground" DynamicServicePath=""
                                Enabled="True" />
                            <asp:Panel ID="pnlIPError" runat="server" Visible="False">
                                <asp:Label ID="IPlblError" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                            </asp:Panel>
                            <asp:Panel ID="pnlIPPopup" runat="server" Width="950px" Style="display: none" CssClass="modalPopup">
                                <asp:Label runat="server" ID="lblIPSubRptHeader" Font-Bold="True" Text="Email Deliverability by Blasts"
                                    Font-Size="Small"></asp:Label>
                                <br />
                                <br />
                                <asp:GridView ID="gvSubBlastsIP" runat="server" ForeColor="Black" CssClass="gridWizard"
                                    OnRowDataBound="gvSubBlastsIP_RowDataBound" AutoGenerateColumns="False" Width="100%">
                                    <AlternatingRowStyle CssClass="gridaltrowWizard" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SourceIP">
                                            <ItemTemplate>
                                                <asp:Label ID="gvSubIPSourceIP" runat="server" Text='<%# Eval("SourceIP") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CustomerName">
                                            <ItemTemplate>
                                                <asp:Label ID="gvSubIPCustomer" runat="server" Text='<%# Eval("CustomerName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BlastID">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="gvSubIPBlastID" runat="server" Text='<%# Eval("BlastID") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TotalSent">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("TotalSent") %>' runat="server" ID="gvSubIPTotalSentSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hard">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("HardBounces") %>' runat="server" ID="gvSubIPHardBouncesSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("HBPerc") %>' runat="server" ID="gvSubIPHBPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Soft">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("SoftBounces") %>' runat="server" ID="gvSubIPSoftBouncesSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("SBPerc") %>' runat="server" ID="gvSubIPSBPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Block">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("MailBlock") %>' runat="server" ID="gvSubIPMailBlockSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("MBPerc") %>' runat="server" ID="gvSubIPMBPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Complaint">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("Complaint") %>' runat="server" ID="gvSubIPComplaintSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("ComPerc") %>' runat="server" ID="gvSubIPComPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OptOut">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("OptOut") %>' runat="server" ID="gvSubIPOptOutSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("OptOPerc") %>' runat="server" ID="gvSubIPOptOPercSub"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MasterSupp">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("MasterSupp") %>' runat="server" ID="gvSubIPMasterSupp"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Perc (%)">
                                            <ItemTemplate>
                                                <asp:Label Text='<%#Eval("MSPerc") %>' runat="server" ID="gvSubIPMSPerc"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="Gray" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#383838" />
                                </asp:GridView>
                                <div align="center" style="width: 100%">
                                    <br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnIPClose" runat="server" CssClass="button" OnClick="btnClose_Click"
                                        Text="Close" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </div>
                                <br />
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
