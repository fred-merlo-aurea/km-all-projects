<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MTAEditor.aspx.cs" Inherits="ecn.communicator.Customers.MTAEditor"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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
        .style1
        {
            height: 25px;
        }
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalPopup2
        {
            background-color: #ffffff;
            width: 270px;
            vertical-align: top;
            z-index: 1000000;
            color: #000000;
        }
        .modalPopup3
        {
            background-color: #ffffff;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
        }
        .modalPopup
        {
            background-color: transparent;
            padding: 1em 6px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function expandcollapse(obj, row) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == "none") {
                div.style.display = "block";
                if (row == 'alt') {
                    img.src = "http://images.ecn5.com/images/collapse_blue.jpg";
                }
                else {
                    img.src = "http://images.ecn5.com/images/collapse_blue.jpg";
                }
                img.alt = "Close to view other MTAs";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "http://images.ecn5.com/images/expand_blue.jpg";
                }
                else {
                    img.src = "http://images.ecn5.com/images/expand_blue.jpg";
                }
                img.alt = "Expand to show IPs and Customers";
            }
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="btnShowPopupMTA" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="mpeMTA" runat="server" TargetControlID="btnShowPopupMTA"
                PopupControlID="pnlPopupMTA" CancelControlID="btnCloseMTA" BackgroundCssClass="modalBackground" />
            <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender" runat="server" BehaviorID="RoundedCornersBehavior"
                TargetControlID="pnlPopupMTARound" Radius="6" Corners="All" />
            <asp:Panel ID="pnlPopupMTA" runat="server" Width="625px" Style="display: none" CssClass="modalPopup3">
                <asp:Panel ID="pnlPopupMTARound" runat="server" Width="625px" CssClass="modalPopup2">
                    <asp:Label ID="lblMTAID" runat="server" Text="" Visible="false"></asp:Label>
                    <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="greySidesLtB">
                            <tr height="20">
                                <td class="gradientTwo addPage" style="border-right: none;">
                                    &nbsp;<span style="font-size: 12px; font-weight: bold"> MTA Add/Edit </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td class="formLabel" valign="middle" align="right" width="30%">
                                                MTA Name&nbsp;
                                            </td>
                                            <td class="formLabel" valign="middle" align="left" width="70%">
                                                <asp:TextBox ID="txtMTAName" runat="Server" class="formtextfield" Columns="100" EnableViewState="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="val_MTAName" runat="Server" CssClass="errormsg" ControlToValidate="txtMTAName"
                                                    ErrorMessage="MTA Name is a required field." Display="static" ValidationGroup="MTA">^^ Required ^^</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel" valign="middle" align="right" width="30%">
                                                Domain Name&nbsp;
                                            </td>
                                            <td class="formLabel" valign="middle" align="left" width="70%">
                                                <asp:TextBox ID="txtDomainName" runat="Server" class="formtextfield" Columns="100"
                                                    EnableViewState="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="val_DomainName" runat="Server" CssClass="errormsg"
                                                    ControlToValidate="txtDomainName" ErrorMessage="Domain Name is a required field."
                                                    Display="static" ValidationGroup="MTA">^^ Required ^^</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel" valign="middle" align="right">
                                                Server Name&nbsp;
                                            </td>
                                            <td class="formLabel" valign="middle" align="left">
                                                <asp:DropDownList ID="ddlBlastConfig" runat="server" CssClass="formfield">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlEMessage" runat="server" Visible="false">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblEMessage" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td align="right" class="label" valign="middle" align="right">
                                                &nbsp;
                                            </td>
                                            <td class="label" align="left" valign="middle">
                                                <asp:Button ID="btnSaveMTA" runat="server" class="formbuttonsmall" OnClick="btnSaveMTA_Click"
                                                    Text="Add" ValidationGroup="MTA" Width="90px" />
                                                &nbsp; &nbsp;
                                                <asp:Button runat="server" Text="Close" ID="btnCloseMTA" class="formbuttonsmall"
                                                    Width="90px"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Button ID="btnShowPopupMTAIP" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="mpeMTAIP" runat="server" TargetControlID="btnShowPopupMTAIP"
                PopupControlID="pnlPopupMTAIP" CancelControlID="btnCloseMTAIP" BackgroundCssClass="modalBackground" />
            <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtenderIP" runat="server"
                BehaviorID="RoundedCornersBehaviorIP" TargetControlID="pnlPopupMTAIPRound" Radius="6"
                Corners="All" />
            <asp:Panel ID="pnlPopupMTAIP" runat="server" Width="625px" Style="display: none"
                CssClass="modalPopup3">
                <asp:Panel ID="pnlPopupMTAIPRound" runat="server" Width="625px" CssClass="modalPopup2">
                    <asp:Label ID="lblIPID" runat="server" Text="" Visible="false"></asp:Label>
                    <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="greySidesLtB">
                            <tr height="20">
                                <td class="gradientTwo addPage" style="border-right: none;">
                                    &nbsp;<span style="font-size: 12px; font-weight: bold"> MTA IP Add/Edit </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td class="formLabel" valign="middle" align="right">
                                                MTA Name&nbsp;
                                            </td>
                                            <td class="formLabel" valign="middle" align="left">
                                                <asp:DropDownList ID="ddlMTA" runat="server" CssClass="formfield">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel" valign="middle" align="right" width="30%">
                                                IP Address&nbsp;
                                            </td>
                                            <td class="formLabel" valign="middle" align="left" width="70%">
                                                <asp:TextBox ID="txtIPAddress" runat="Server" class="formtextfield" Columns="100"
                                                    EnableViewState="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="val_IPAddress" runat="Server" CssClass="errormsg"
                                                    ControlToValidate="txtIPAddress" ErrorMessage="IPAddress is a required field."
                                                    Display="static" ValidationGroup="IP">^^ Required ^^</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel" valign="middle" align="right" width="30%">
                                                Host Name&nbsp;
                                            </td>
                                            <td class="formLabel" valign="middle" align="left" width="70%">
                                                <asp:TextBox ID="txtHostName" runat="Server" class="formtextfield" Columns="100"
                                                    EnableViewState="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="val_HostName" runat="Server" CssClass="errormsg"
                                                    ControlToValidate="txtHostName" ErrorMessage="Host Name is a required field."
                                                    Display="static" ValidationGroup="IP">^^ Required ^^</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlEMessage2" runat="server" Visible="false">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblEMessage2" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td align="right" class="label" valign="middle" align="right">
                                                &nbsp;
                                            </td>
                                            <td class="label" align="left" valign="middle">
                                                <asp:Button ID="btnSaveMTAIP" runat="server" class="formbuttonsmall" OnClick="btnSaveMTAIP_Click"
                                                    Text="Add" ValidationGroup="IP" Width="90px" />
                                                &nbsp; &nbsp;
                                                <asp:Button runat="server" Text="Close" ID="btnCloseMTAIP" class="formbuttonsmall"
                                                    Width="90px"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Button ID="btnShowPopupMTACustomer" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="mpeMTACustomer" runat="server" TargetControlID="btnShowPopupMTACustomer"
                PopupControlID="pnlPopupMTACustomer" CancelControlID="btnCloseMTACustomer" BackgroundCssClass="modalBackground" />
            <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtenderCustomer" runat="server"
                BehaviorID="RoundedCornersBehaviorCustomer" TargetControlID="pnlPopupMTACustomerRound"
                Radius="6" Corners="All" />
            <asp:Panel ID="pnlPopupMTACustomer" runat="server" Width="625px" Style="display: none"
                CssClass="modalPopup3">
                <asp:Panel ID="pnlPopupMTACustomerRound" runat="server" Width="625px" CssClass="modalPopup2">
                    <asp:Label ID="lblCustomerMTAID" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblCustomerID" runat="server" Text="" Visible="false"></asp:Label>
                    <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="greySidesLtB">
                            <tr height="20">
                                <td class="gradientTwo addPage" style="border-right: none;">
                                    &nbsp;<span style="font-size: 12px; font-weight: bold"> MTA Customer Add/Edit </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td class="formLabel" valign="middle" align="right">
                                                MTA Name&nbsp;
                                            </td>
                                            <td class="formLabel" valign="middle" align="left">
                                                <asp:DropDownList ID="ddlMTACustomer" runat="server" CssClass="formfield">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel" valign="middle" align="right">
                                                Customer Name&nbsp;
                                            </td>
                                            <td class="formLabel" valign="middle" align="left">
                                                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="formfield">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label" valign="middle" align='right' width="120">
                                                Default&nbsp;
                                            </td>
                                            <td class="label" align="left">
                                                <asp:DropDownList ID="ddlIsDefault" runat="server" CssClass="formfield" Width="70px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlEMessage3" runat="server" Visible="false">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblEMessage3" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td align="right" class="label" valign="middle" align="right">
                                                &nbsp;
                                            </td>
                                            <td class="label" align="left" valign="middle">
                                                <asp:Button ID="btnSaveMTACustomer" runat="server" class="formbuttonsmall" OnClick="btnSaveMTACustomer_Click"
                                                    Text="Add" ValidationGroup="Customer" Width="90px" />
                                                &nbsp; &nbsp;
                                                <asp:Button runat="server" Text="Close" ID="btnCloseMTACustomer" class="formbuttonsmall"
                                                    Width="90px"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <table id="wrapper1" cellspacing="1" cellpadding="1" width="100%" border='0'>
                <tbody>
                    <tr>
                        <td class="label" align="right" width="10%" valign="middle">
                            <asp:Button ID="btnAddMTA" runat="server" Text="Add MTA" class="formbuttonsmall"
                                CausesValidation="False" OnClick="btnAddMTA_Click" />
                            <asp:Button ID="btnAddMTAIP" runat="server" Text="Add MTA IP" class="formbuttonsmall"
                                CausesValidation="False" OnClick="btnAddMTAIP_Click" />
                            <asp:Button ID="btnAddMTACustomer" runat="server" Text="Add MTA Customer" class="formbuttonsmall"
                                CausesValidation="False" OnClick="btnAddMTACustomer_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <fieldset>
                <table id="wrapper2" cellspacing="1" cellpadding="1" width="100%" border='0'>
                    <tbody>
                        <tr>
                            <td class="tableHeader" align="left" valign="middle">
                                Customer Name&nbsp;
                            </td>
                            <td class="tableHeader" align="left" valign="middle">
                                MTA Name&nbsp;
                            </td>
                            <td class="tableHeader" align="left" valign="middle">
                                Domain Name&nbsp;
                            </td>
                            <td class="tableHeader" align="left" valign="middle">
                                IP&nbsp;
                            </td>
                            <td class="tableHeader" align="left" valign="middle">
                                Host Name&nbsp;
                            </td>
                            <td class="tableHeader" align="left" valign="middle" width="12%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="label" align="left" valign="middle">
                                <asp:TextBox ID="txtSearchCustomer" runat="server" Text="" class="formtextfield"></asp:TextBox>
                            </td>
                            <td class="label" align="left" valign="middle">
                                <asp:TextBox ID="txtSearchMTA" runat="server" Text="" class="formtextfield"></asp:TextBox>
                            </td>
                            <td class="label" align="left" valign="middle">
                                <asp:TextBox ID="txtSearchDomain" runat="server" Text="" class="formtextfield"></asp:TextBox>
                            </td>
                            <td class="label" align="left" valign="middle">
                                <asp:TextBox ID="txtSearchIP" runat="server" Text="" class="formtextfield"></asp:TextBox>
                            </td>
                            <td class="label" align="left" valign="middle">
                                <asp:TextBox ID="txtSearchHost" runat="server" Text="" class="formtextfield"></asp:TextBox>
                            </td>
                            <td class="label" align="left" width="12%" valign="middle">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" class="formbuttonsmall" CausesValidation="False"
                                    OnClick="btnSearch_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Reset" class="formbuttonsmall" CausesValidation="False"
                                    OnClick="btnClear_Click" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </fieldset>
            <br />
            <div>
                <asp:GridView ID="GridView1" AllowPaging="True" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                    DataKeyNames="MTAID" runat="server" GridLines="None" OnRowDataBound="GridView1_RowDataBound"
                    OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand"
                    OnRowEditing="GridView1_RowEditing" OnRowDeleting="GridView1_RowDeleting" AllowSorting="true"
                    PageSize="20">
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <RowStyle ForeColor="#ffffff" BackColor="#4b83ac" Font-Bold="True"></RowStyle>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <a href="javascript:expandcollapse('div<%# Eval("MTAID") %>', 'one');">
                                    <img id="imgdiv<%# Eval("MTAID") %>" alt="Click to show/hide IPs and Customers for MTA  <%# Eval("MTAID") %>"
                                        border="0" src="http://images.ecn5.com/images/expand_blue.jpg" />
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MTAID" Visible="false" HeaderText="" />
                        <asp:BoundField DataField="MTAName" Visible="true" HeaderText="MTA Name" />
                        <asp:BoundField DataField="DomainName" Visible="true" HeaderText="Domain Name" />
                        <asp:BoundField DataField="ConfigName" Visible="true" HeaderText="Config Name" />
                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit MTA' border='0'&gt;"
                                    CausesValidation="false" ID="lbEditMTA" CommandName="Edit" CommandArgument='<%#Eval("MTAID")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="center"
                            ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete MTA' border='0'&gt;"
                                    CausesValidation="false" ID="DeleteMTABtn" CommandName="Delete" CommandArgument='<%#Eval("MTAID")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr>
                                    <td colspan="100%" align="left">
                                        <div id="div<%# Eval("MTAID") %>" style="display: block; position: relative; left: 29px;
                                            overflow: auto; width: 97%">
                                            <table cellpadding="3" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td width="50%" valign="top">
                                                        <asp:GridView ID="gvIP" Width="100%" CssClass="grid" OnRowEditing="gvIP_RowEditing"
                                                            OnRowDeleting="gvIP_RowDeleting" AutoGenerateColumns="false" runat="server" DataKeyNames="MTAID"
                                                            OnRowCommand="gvIP_RowCommand" GridLines="None">
                                                            <Columns>
                                                                <asp:BoundField DataField="MTAID" Visible="false" HeaderText="" />
                                                                <asp:BoundField DataField="IPID" Visible="false" HeaderText="" />
                                                                <asp:BoundField DataField="IPAddress" Visible="true" HeaderText="IP Address" HeaderStyle-HorizontalAlign="left"
                                                                    ItemStyle-HorizontalAlign="left" />
                                                                <asp:BoundField DataField="HostName" Visible="true" HeaderText="Host Name" HeaderStyle-HorizontalAlign="left"
                                                                    ItemStyle-HorizontalAlign="left" />
                                                                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit MTA IP' border='0'&gt;"
                                                                            CausesValidation="false" ID="lbEditMTAIP" CommandName="Edit" CommandArgument='<%#Eval("IPID")%>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete MTA IP' border='0'&gt;"
                                                                            CausesValidation="false" ID="DeleteMTAIPBtn" CommandName="Delete" CommandArgument='<%#Eval("IPID")%>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#c3c3c3" Font-Bold="True" Height="30px" HorizontalAlign="Left" />
                                                        </asp:GridView>
                                                    </td>
                                                    <td width="50%" valign="top">
                                                        <asp:GridView ID="gvCustomer" Width="100%" CssClass="grid" AutoGenerateColumns="false"
                                                            runat="server" DataKeyNames="MTAID" OnRowEditing="gvCustomer_RowEditing" OnRowDeleting="gvCustomer_RowDeleting"
                                                            OnRowCommand="gvCustomer_RowCommand" GridLines="None">
                                                            <HeaderStyle BackColor="#c3c3c3" Font-Bold="True" Height="30px" HorizontalAlign="Left" />
                                                            <Columns>
                                                                <asp:BoundField DataField="MTAID" Visible="false" HeaderText="" />
                                                                <asp:BoundField DataField="CustomerID" Visible="false" HeaderText="" />
                                                                <asp:BoundField DataField="CustomerName" Visible="true" HeaderText="Customer Name"
                                                                    HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                                                                <asp:TemplateField HeaderText="IsDefault" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <%# (Boolean.Parse(Eval("IsDefault").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit MTA Customer' border='0'&gt;"
                                                                            CausesValidation="false" ID="lbEditMTACustomer" CommandName="Edit" CommandArgument='<%#Eval("MTAID") + ","+Eval("CustomerID") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="4%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete MTA Customer' border='0'&gt;"
                                                                            CausesValidation="false" ID="DeleteMTACustomerBtn" CommandName="Delete" CommandArgument='<%#Eval("MTAID") + ","+Eval("CustomerID") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <div id="divError" runat="Server" visible="false">
                    <table cellspacing="0" cellpadding="0" width="674" align="center">
                        <tr>
                            <td id="errorTop">
                            </td>
                        </tr>
                        <tr>
                            <td id="errorMiddle">
                                <table width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img id="Img1" style="padding: 0 0 0 15px;" src="images/errorEx.jpg" runat="server"
                                                alt="" />
                                        </td>
                                        <td valign="middle" align="left" width="80%" height="100%">
                                            <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="errorBottom">
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
