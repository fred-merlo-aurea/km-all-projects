<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="CodeSheet.aspx.cs" Inherits="KMPS.MDAdmin.CodeSheet" %>

<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

        function ValidateDelete() {
            if (!confirm('Are you sure you want to delete?')) return false;

            if (!confirm('Are you sure you want to delete Codesheet. It will delete all mapping for the Codesheet?')) return false;

            return true;
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="text-align: right">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td align="right">Product :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpPubs" runat="server" AutoPostBack="True"
                            DataTextField="PubName" DataValueField="PubID"
                            OnSelectedIndexChanged="drpPubs_SelectedIndexChanged" Width="300px">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right">Response Group :
                    </td>
                    <td>
                        <asp:DropDownList ID="drpGroup" runat="server" AutoPostBack="True"
                            DataTextField="ResponseGroupName" DataValueField="ResponseGroupID" OnSelectedIndexChanged="drpGroup_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rqdrpGroup" ControlToValidate="drpGroup" runat="server" ErrorMessage="*" ValidationGroup="save" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="gvCodeSheet" runat="server" AutoGenerateColumns="False" DataKeyNames="CodeSheetID"
                AllowSorting="True" AllowPaging="True" OnRowCommand="gvCodeSheet_RowCommand" OnSorting="gvCodeSheet_Sorting"
                OnRowDataBound="gvCodeSheet_RowDataBound" OnRowDeleting="gvCodeSheet_RowDeleting" OnPageIndexChanging="gvCodeSheet_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="ResponseValue" HeaderText="Value" SortExpression="ResponseValue"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ResponseDesc" HeaderText="Description" SortExpression="ResponseDesc">
                        <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DisplayName" HeaderText="Report Group" SortExpression="DisplayName">
                        <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Active" SortExpression="IsActive" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate><%# (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Other" SortExpression="IsOther" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate><%# (Boolean.Parse(Eval("IsOther").ToString())) ? "Yes" : "No"%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:ButtonField HeaderStyle-Width="5%" ItemStyle-Width="5%" ButtonType="Link"
                        Text="<img src='Images/ic-edit.gif' style='border:none;'>" CommandName="Select"
                        ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:ButtonField>
                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                        ItemStyle-HorizontalAlign="center"
                        HeaderStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("CodeSheetID")%>' OnClientClick="return ValidateDelete();"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="60%" ItemStyle-Width="60%">
                        <ItemTemplate>
                            <%--                            <tr>
                                <td colspan="100%">--%>
                            <asp:GridView ID="grdMaster" runat="server" AutoGenerateColumns="False" ShowFooter="false"
                                AllowPaging="false" AllowSorting="false" Width="95%" CellPadding="10">
                                <Columns>
                                    <asp:TemplateField HeaderText="Master Value" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                        ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            <%# Eval("MasterValue") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Master Description" HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            <%# Eval("MasterDesc") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Master Group" HeaderStyle-Width="70%" ItemStyle-Width="70%" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                        <ItemTemplate>
                                            <%# Eval("DisplayName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--                                </td>
                            </tr>  --%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </div>
            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">Add Responses</asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel" BorderWidth="1">
                <table cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td align="right">
                            <asp:HiddenField ID="hfCodeSheetID" runat="server" Value="0" />
                            <asp:TextBox ID="txtPubID" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Value :
                        </td>
                        <td>
                            <asp:TextBox ID="txtResponseValue" runat="server" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqResponseValue" runat="server" ControlToValidate="txtResponseValue"
                                ErrorMessage="*" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Description :
                        </td>
                        <td>
                            <asp:TextBox ID="txtResponseDesc" runat="server" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqResponseDesc" runat="server" ControlToValidate="txtResponseDesc"
                                ErrorMessage="*" ValidationGroup="save"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Report Groups :
                        </td>
                        <td>
                        <asp:DropDownList ID="drpReportGroups" runat="server" 
                            DataTextField="DisplayName" DataValueField="ReportGroupID">
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Active :</td>
                        <td>
                            <asp:CheckBox ID="cbActive" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Other :</td>
                        <td>
                            <asp:CheckBox ID="cbIsOther" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Master Code Sheet Ref :
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td colspan="3">
                                        <b>Types :</b>
                                        <asp:DropDownList ID="ddlPubTypes" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlPubTypes_SelectedIndexChanged">
                                        </asp:DropDownList><br />
                                        <br />
                                    </td>
                                </tr>

                                <tr>
                                    <td style="text-align: center" width="40%">
                                        <b>Available
                                                <asp:Label ID="lblAvailablePubs" runat="server" Text=""></asp:Label></b><br />
                                        <br />
                                        <asp:ListBox ID="lbAvailablePubs" runat="server" AutoPostBack="False"
                                            SelectionMode="Multiple" Width="350px" Height="300px"></asp:ListBox>
                                    </td>
                                    <td style="text-align: center" width="20%">
                                        <asp:Button ID="btnAddPub" runat="server" Text=">>" Enabled="true"
                                            OnClick="btnAddPub_Click" CssClass="button" CausesValidation="false" /><br />
                                        <asp:Button ID="btnRemovePub" runat="server" Text="<<" Enabled="true"
                                            OnClick="btnRemovePub_Click" CssClass="button" CausesValidation="false" />
                                    </td>
                                    <td style="text-align: center" width="40%">
                                        <b>Selected
                                                <asp:Label ID="lblSelectedPubs" runat="server" Text=""></asp:Label></b><br />
                                        <br />
                                        <asp:ListBox ID="lbSelectedPubs" runat="server" AutoPostBack="True"
                                            Width="350px" Height="300px" SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="3" style="text-align: center">
                                        <b>All Selected Items<br /></td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: center" align="center">
                                        <asp:GridView ID="grdItems" runat="server" AutoGenerateColumns="False" DataKeyNames="ItemID"
                                            ShowFooter="False" AllowPaging="false" AllowSorting="false" Width="100%" CellPadding="10"
                                            OnRowDeleting="grdItems_RowDeleting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Item Type" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemType" runat="server" Visible="False" Text='<%# Eval("ItemType")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MasterGroup/PubType" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                                    ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGroupID" runat="server" Visible="False" Text='<%# Eval("GroupID")%>'></asp:Label>
                                                        <asp:Label ID="lblGroupTitle" runat="server" Text='<%# Eval("GroupTitle")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Entries" HeaderStyle-Width="63%" ItemStyle-Width="63%"
                                                    ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEntryID" runat="server" Visible="False" Text='<%# Eval("EntryID")%>'></asp:Label>
                                                        <asp:Label ID="lblEntryTitle" runat="server" Text='<%# Eval("EntryTitle")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:ButtonField ButtonType="Link" Text="<img src='../images/icon-delete.gif' style='border:none;'>"
                                                    CommandName="Delete" HeaderText="Remove" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="7%" ItemStyle-Width="7%" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" ValidationGroup="save" />
                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                CssClass="button" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
