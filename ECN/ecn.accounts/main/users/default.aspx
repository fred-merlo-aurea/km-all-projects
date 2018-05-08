<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits=" ecn.accounts.usersmanager.users_main" %>

<%@ Register Assembly="ecn.controls" Namespace="ecn.controls" TagPrefix="ecn" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel='stylesheet' href="/ecn.communicator/MasterPages/ECN_MainMenu.css" type="text/css" />
    <link rel='stylesheet' href="/ecn.communicator/MasterPages/ECN_Controls.css" type="text/css" />
    <style type="text/css">
        .aspBtn {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
            background: -moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
            background-color: #ededed;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #dcdcdc;
            display: inline-block;
            color: black;
            font-family: arial;
            font-size: 9px;
            font-weight: bold;
            padding: 2px 10px;
            text-decoration: none;
            text-shadow: 1px 1px 0px #ffffff;
        }

            .aspBtn:hover {
                background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
                background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
                background-color: #dfdfdf;
            }

            .aspBtn:active {
                position: relative;
            }
    </style>

    <script type="text/javascript">
        function SetScrollEvent()
        {
            window.scrollTo(0, 0);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upnl1" runat="server">
        <ContentTemplate>

            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table height="67" width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblErrorMessagePhError" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </asp:PlaceHolder>

            <table id="Table1" cellspacing="1" cellpadding="1" width="98%" border="0" style="padding-top: 30px; padding-bottom: 20px;">
                <tr>
                    <td class="tableHeader" align="left">
                        <asp:Panel ID="pnlBaseChannel" runat="Server" Visible="false">
                            <table width="100%" border='0'>
                                <tr>
                                    <td class="tableHeader">Channels
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList class="formfield" ID="ddlBaseChannels" Style="display: inline"
                                            runat="Server" Visible="true" OnSelectedIndexChanged="ddlBaseChannels_OnSelectedIndexChanged"
                                            AutoPostBack="true" DataTextField="BaseChannelName" DataValueField="PlatformClientGroupID"
                                            Width="249">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                            </table>
                        </asp:Panel>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:Panel ID="pnlCustomer" runat="Server" Visible="false">
                            <table width="100%" border='0'>
                                <tr>
                                    <td class="tableHeader" align="left">Customers
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:DropDownList class="formfield" ID="ddlCustomers" runat="Server" Visible="true"
                                            OnSelectedIndexChanged="ddlCustomers_OnSelectedIndexChanged" AutoPostBack="true"
                                            DataTextField="customerName" DataValueField="PlatformClientID" EnableViewState="true"
                                            Width="249">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td style="vertical-align: top;">
                        <table width="100%" border='0'>
                            <tr>
                                <td class="tableHeader" align="left">Search username/name/email
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="txtSearch" runat="server" value="" MaxLength="50"></asp:TextBox>&nbsp;
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: middle;">
                        <asp:Button ID="btnAddNewUser" runat="server" Text="Add New User" OnClick="btnAddNewUser_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkShowSysAdmin" Visible="false" runat="server" Text="Show System Admins" OnCheckedChanged="chkShowSysAdmin_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowDisabledUsers" runat="server" Text="Show Disabled/Locked Users" AutoPostBack="true" OnCheckedChanged="chkShowDisabledUsers_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowDisabledUserRoles" runat="server" Text="Show Disabled/Pending User Roles" AutoPostBack="true" OnCheckedChanged="chkShowDisabledUserRoles_CheckedChanged" />
                                </td>
                                <td>
                                    
                                </td>
                                <td>
                                    <asp:Button ID="btnDownloadUsers" runat="server" Text="Download" OnClick="btnDownloadUsers_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan='4' align="center">
                        <asp:Label ID="lblErrorMessage" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td colspan='4' align="left">
                        <ecn:ecnGridView ID="grdUsers" AllowPaging="false" RowStyle-Font-Size="Small" PageSize="20" RowStyle-Height="40px" HeaderStyle-Font-Size="Medium" Width="100%"
                            AutoGenerateColumns="false" runat="server" GridLines="Horizontal" OnRowDataBound="grdUsers_RowDataBound" OnRowCommand="grdUsers_RowCommand" OnRowDeleting="grdUsers_RowDeleting">
                            <AlternatingRowStyle CssClass="gridaltrow" />
                            <Columns>
                                <asp:BoundField DataField="UserName" ItemStyle-Wrap="true" HeaderText="User Name" ItemStyle-Width="25%" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                <asp:TemplateField HeaderText="Name" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# String.Format("{0} {1}", DataBinder.Eval(Container.DataItem,"FirstName").ToString(), DataBinder.Eval(Container.DataItem,"LastName").ToString()) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ClientName" ItemStyle-Wrap="true" HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="25%" />
                                <asp:TemplateField HeaderText="Role" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserRole" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Role Status" ItemStyle-Wrap="true" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <ul class="ECN-InfoLinks" style="padding-left: 0px; padding-top: 10px; padding-right: 0px; display: inline;">
                                            <li style="width: 100%; text-align: center; padding-top: 0px; padding-bottom: 0px;">
                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" />
                                                <ul style="left: 30px;">
                                                    <li style="text-align: left;">
                                                        <asp:LinkButton ID="btnEdit" runat="server" Text="Edit User" CssClass="aspBtn" OnClick="btnEdit_Click" /></li>
                                                    <li id="liDeleteRole" runat="server" style="text-align: left;">
                                                        <asp:LinkButton ID="btnDelete" runat="server" Text="Disable User's Role" CssClass="aspBtn" OnClick="btnDelete_Click" /></li>
                                                    <li id="liResend" runat="server" style="text-align: left;">
                                                        <asp:LinkButton ID="btnResendInvite" runat="server" Text="Resend Invite" CssClass="aspBtn" OnClick="btnResendInvite_Click" />
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </ecn:ecnGridView>

                        <br />

                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlPager" runat="server" Visible="false">
                            <table cellpadding="0" border="0" width="100%">
                                <tr>
                                    <td class="label" style="width: 33%; text-align: left;">Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                    </td>
                                    <td class="label" style="width: 33%; text-align: center;">Show Rows:
                                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UserGrid_SelectedIndexChanged" CssClass="formfield">
                                                    <asp:ListItem Value="5" />
                                                    <asp:ListItem Value="10" />
                                                    <asp:ListItem Value="15" />
                                                    <asp:ListItem Value="20" />
                                                </asp:DropDownList>
                                    </td>
                                    <td class="label" style="width: 34%; text-align: right;">Page
                                                <asp:TextBox ID="txtGoToPageContent" runat="server" AutoPostBack="true" OnTextChanged="GoToPageUser_TextChanged" class="formtextfield" Width="30px" />
                                        of
                                                <asp:Label ID="lblTotalNumberOfPagesGroup" runat="server" CssClass="label" />
                                        &nbsp;
                                                <asp:Button ID="btnPreviousGroup" runat="server" ToolTip="Previous Page" OnClick="btnPreviousGroup_Click" class="formbuttonsmall" Text="<< Previous" />
                                        <asp:Button ID="btnNextGroup" runat="server" ToolTip="Next Page" OnClick="btnNextGroup_Click" class="formbuttonsmall" Text="Next >>" />

                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
