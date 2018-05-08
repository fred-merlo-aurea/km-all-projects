<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Markets.aspx.cs" Inherits="KMPS.MD.Main.Markets"
    MasterPageFile="~/MasterPages/Site.Master" %>

<%@ Register Assembly="ecn.controls" Namespace="ecn.controls" TagPrefix="ecn" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="../images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="MarketErrorDiv" runat="Server" visible="false">
                    <table cellspacing="0" cellpadding="0" width="674" align="center">
                        <tr>
                            <td id="Td2">
                                <table width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img id="Img2" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
                                                alt="" />
                                        </td>
                                        <td valign="middle" align="left" width="80%" height="100%">
                                            <asp:Label ID="MarketErrorLabel" runat="Server" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 90%; text-align: left; padding-left: 10px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">
                        <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                            <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 40px;">
                                <td valign="middle">
                                    <b>Brand
                                    <asp:Label ID="lblColon" runat="server" Visible="false" Text=":"></asp:Label></b>&nbsp;&nbsp;
                                    <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label>
                                    <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                        AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                        DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <div style="border: solid 1px #5783BD">
                                    <asp:GridView ID="gvMarkets" runat="server" DataKeyNames="MarketID" Width="90%" OnRowCommand="gvMarkets_RowCommand" 
                                        AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="gvMarkets_PageIndexChanging"
                                        BorderWidth="0" CellPadding="5" CellSpacing="0" EditRowStyle-BorderWidth="0" EnableSortingAndPagingCallbacks="false">
                                        <Columns>
                                            <asp:BoundField DataField="MarketName" HeaderText="Market" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibEdit" runat="server" CausesValidation="False" CommandName="EditMarket"
                                                        CommandArgument='<%# DataBinder.Eval(Container,"DataItem.MarketID") %>' ImageUrl="~/images/ic-edit.gif" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibDelete" runat="server" CausesValidation="False" CommandName="DeleteMarket"
                                                        CommandArgument='<%# DataBinder.Eval(Container,"DataItem.MarketID") %>' ImageUrl="~/images/icon-delete.gif"
                                                        OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <ecn:BoxPanel ID="bpAddMarkets" runat="server" Title="Add Markets" Layout="table"
                        HorizontalAlign="Left" Width="100%">
                        <div id="divError" runat="Server" visible="false">
                            <table cellspacing="0" cellpadding="0" width="100%" align="center">
                                <tr>
                                    <td id="errorMiddle">
                                        <table width="100%">
                                            <tr>
                                                <td valign="top" align="center" width="5%">
                                                    <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                                        alt="" />
                                                </td>
                                                <td valign="middle" align="left" width="95%" height="100%">
                                                    <asp:Label ID="lblErrorMessage" runat="Server" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table width="100%" border="0" cellpadding="5" cellspacing="5">
                            <tr>
                                <td width="7%" align="right"><b>Market Name</b></td>
                                <td width="93%">
                                    <asp:TextBox ID="txtMarketName" runat="server" MaxLength="100" Width="250"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ReqFldTxtName" runat="server" ControlToValidate="txtMarketName"
                                        ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lblMarketID" Visible="false" runat="server" Text="0" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <b>Filters</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="right"><b>Score</b></td>
                                <td>
                                    <asp:DropDownList ID="drpAdhocInt" runat="server" Font-Names="Arial" Font-Size="x-small"
                                        Width="100" OnSelectedIndexChanged="drpAdhocInt_selectedindexchanged" AutoPostBack="true">
                                        <asp:ListItem Selected="true" Value="Range">RANGE</asp:ListItem>
                                        <asp:ListItem Value="Equal">EQUAL</asp:ListItem>
                                        <asp:ListItem Value="Greater">GREATER THAN</asp:ListItem>
                                        <asp:ListItem Value="Lesser">LESSER THAN</asp:ListItem>
                                    </asp:DropDownList>&nbsp;
                                            <asp:TextBox ID="txtAdhocIntFrom" Width="65" CssClass="formfield" MaxLength="500"
                                                runat="server"></asp:TextBox>&nbsp;To&nbsp;
                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftbe1" runat="server" TargetControlID="txtAdhocIntFrom"
                                                FilterType="Numbers"
                                                ValidChars="." />
                                    <asp:TextBox ID="txtAdhocIntTo" Width="65" CssClass="formfield" MaxLength="500" runat="server"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbe2" runat="server" TargetControlID="txtAdhocIntTo"
                                        FilterType="Numbers"
                                        ValidChars="." />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Types</b></td>
                                <td>
                                    <asp:DropDownList ID="ddlPubTypes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPubTypes_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <table cellspacing="0" cellpadding="0" border='0' align="left" width="50%">
                                        <tr>
                                            <td style="text-align: center">
                                                <b>Available</b><br />
                                                <br />
                                                <asp:ListBox ID="lbAvailablePubs" runat="server" AutoPostBack="False" SelectionMode="Multiple"
                                                    Width="350px" Height="300px"></asp:ListBox>
                                            </td>
                                            <td style="text-align: center">
                                                <asp:Button ID="btnAddPub" runat="server" Text=">>" Enabled="true" OnClick="btnAddPub_Click"
                                                    CssClass="button" CausesValidation="false" /><br />
                                                <asp:Button ID="btnRemovePub" runat="server" Text="<<" Enabled="true" OnClick="btnRemovePub_Click"
                                                    CssClass="button" CausesValidation="false" />
                                            </td>
                                            <td style="text-align: center">
                                                <b>Selected</b><br />
                                                <br />
                                                <asp:ListBox ID="lbSelectedPubs" runat="server" AutoPostBack="True" Width="350px"
                                                    Height="300px" SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center" colspan="2">
                                    <b>All Selected Items<br /></td>
                            </tr>
                            <tr>
                                <td style="text-align: center" align="center" colspan="2">
                                    <asp:GridView ID="grdItems" runat="server" AutoGenerateColumns="False" DataKeyNames="ItemID"
                                        ShowFooter="False" AllowPaging="false" AllowSorting="false" Width="100%" CellPadding="10"
                                        OnRowDeleting="grdItems_RowDeleting" EmptyDataText="None Selected">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Item Type" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                                ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemType" runat="server" Visible="false" Text='<%# Eval("ItemType")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MasterGroup/PubType" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                                ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroupID" runat="server" Visible="false" Text='<%# Eval("GroupID")%>'></asp:Label>
                                                    <asp:Label ID="lblGroupTitle" runat="server" Text='<%# Eval("GroupTitle")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entries" HeaderStyle-Width="63%" ItemStyle-Width="63%"
                                                ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEntryID" runat="server" Visible="false" Text='<%# Eval("EntryID")%>'></asp:Label>
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
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label><br />
                                    <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button"
                                        ValidationGroup="Main" />
                                    <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                        CssClass="button" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </ecn:BoxPanel>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
