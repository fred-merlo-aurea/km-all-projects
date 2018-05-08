<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketsNew.aspx.cs" Inherits="KMPS.MD.Main.MarketsNew"
    MasterPageFile="~/MasterPages/Site.Master" %>

<%@ Register Assembly="ecn.controls" Namespace="ecn.controls" TagPrefix="ecn" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div class="UpdateProgress" style="position: absolute; z-index: 10; color: black;
                font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center;
                width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft);
                top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="../images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            <br />
            <div style="width: 80%; text-align: left">
                <div style="border: solid 1px #5783BD">
                    <asp:DataList ID="dlMarkets" runat="server" DataKeyField="MarketID" Width="100%" OnItemCommand="dlMarkets_ItemCommand" >
                        <HeaderTemplate>
                            <table class="grid" cellspacing="0" cellpadding="5" width="100%" align="center" border='0'>
                                <tr>
                                    <th align="left" width="90%">
                                        &nbsp;&nbsp;&nbsp; Market
                                    </th>
                                    <th align='right' width="10%">
                                    </th>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table cellspacing="0" cellpadding="5" width="100%" align="center" border='0'>
                                <tr>
                                    <td width="90%">
                                        &nbsp;&nbsp;&nbsp;
                                        <%# DataBinder.Eval(Container.DataItem, "MarketName")%>&nbsp;
                                    </td>
                                    <td width="10%" align='right'>
                                        <asp:ImageButton ID="ibEdit" runat="server" CausesValidation="False" CommandName="EditMarket"
                                            CommandArgument='<%# DataBinder.Eval(Container,"DataItem.MarketID") %>' ImageUrl="~/images/ic-edit.gif" />&nbsp;
                                        <asp:ImageButton ID="ibDelete" runat="server" CausesValidation="False" CommandName="DeleteMarket"
                                            CommandArgument='<%# DataBinder.Eval(Container,"DataItem.MarketID") %>' ImageUrl="~/images/icon-delete.gif"
                                            OnClientClick="return confirm('Are you sure you want to delete?');" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <table cellspacing="0" cellpadding="5" width="100%" align="center" border='0'>
                                <tr style="background-color: #CFCFCF">
                                    <td width="90%">
                                        &nbsp;&nbsp;&nbsp;
                                        <%# DataBinder.Eval(Container.DataItem, "MarketName")%>&nbsp;
                                    </td>
                                    <td width="10%" align='right'>
                                        <asp:ImageButton ID="ibEdit" runat="server" CausesValidation="False" CommandName="EditMarket"
                                            CommandArgument='<%# DataBinder.Eval(Container,"DataItem.MarketID") %>' ImageUrl="~/images/ic-edit.gif" />&nbsp;
                                        <asp:ImageButton ID="ibDelete" runat="server" CausesValidation="False" CommandName="DeleteMarket"
                                            CommandArgument='<%# DataBinder.Eval(Container,"DataItem.MarketID") %>' ImageUrl="~/images/icon-delete.gif"
                                            OnClientClick="return confirm('Are you sure you want to delete?');" />
                                    </td>
                                </tr>
                            </table>
                        </AlternatingItemTemplate>
                    </asp:DataList>
                </div>
                <br />
                <ecn:BoxPanel ID="bpAddMarkets" runat="server" Title="Add Markets" Layout="table"
                    HorizontalAlign="Left" Width="50%">
                    <div id="divError" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" width="100%" align="center">
                            <tr>
                                <td id="errorTop">
                                </td>
                            </tr>
                            <tr>
                                <td id="errorMiddle" width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top" align="center" width="20%">
                                                <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                                    alt="" />
                                            </td>
                                            <td valign="middle" align="left" width="80%" height="100%">
                                                <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
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
                    </div>
                    <table width="100%" border="0" cellpadding="5" cellspacing="5">
                        <tr>
                            <td colspan="3">
                                &nbsp;<asp:Label ID="lblMarketID" Visible="false" runat="server" Text="0" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <b>Market Name :</b>
                                <asp:TextBox ID="txtMarketName" runat="server" MaxLength="100" Width="250"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ReqFldTxtName" runat="server" ControlToValidate="txtMarketName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <b>Publication Types :</b>
                                <asp:DropDownList ID="ddlPubTypes" runat="server" AutoPostBack="true" 
                                        onselectedindexchanged="ddlPubTypes_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td style="text-align: center" width="40%">
                                <b>Available
                                    <asp:Label ID="lblAvailablePubs" runat="server" Text="PUBS / SHOWS / EPRODUCTS"></asp:Label></b><br />
                                <br />
                                <asp:ListBox ID="lbAvailablePubs" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="lbAvailablePubs_SelectedIndexChanged" 
                                    SelectionMode="Multiple" Width="350px" Height="300px"></asp:ListBox>
                            </td>
                            <td style="text-align: center" width="20%">
                                <asp:Button ID="btnAddPub" runat="server" Text=">>" Enabled="False" 
                                    onclick="btnAddPub_Click" CssClass="button" CausesValidation="false"/><br />
                                <asp:Button ID="btnRemovePub" runat="server" Text="<<" Enabled="False" 
                                    onclick="btnRemovePub_Click" CssClass="button" CausesValidation="false"/>
                            </td>
                            <td style="text-align: center" width="40%">
                                <b>Selected
                                    <asp:Label ID="lblSelectedPubs" runat="server" Text="PUBS / SHOWS / EPRODUCTS"></asp:Label></b><br />
                                <br />
                                <asp:ListBox ID="lbSelectedPubs" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="lbSelectedPubs_SelectedIndexChanged" 
                                    Width="350px" Height="300px" SelectionMode="Multiple"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <b>Master Group :</b>
                                <asp:DropDownList ID="ddlMasterGroups" runat="server" AutoPostBack="true" 
                                        onselectedindexchanged="ddlMasterGroups_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center" width="40%">
                                <b>Available
                                    <asp:Label ID="lblAvailableDimensions" runat="server" Text="Dimensions"></asp:Label></b><br />
                                <br />
                                <asp:ListBox ID="lbAvailableDimensions" runat="server" AutoPostBack="True" 
                                    Width="350px" Height="300px" onselectedindexchanged="lbAvailableDimensions_SelectedIndexChanged" 
                                    SelectionMode="Multiple"></asp:ListBox>
                            </td>
                            <td style="text-align: center" width="20%">
                                <asp:Button ID="btnAddDimension" runat="server" Text=">>" Enabled="False" CssClass="button"  CausesValidation="false"
                                    onclick="btnAddDimension_Click" /><br />
                                <asp:Button ID="btnRemoveDimension" runat="server" Text="<<" Enabled="False" CssClass="button" CausesValidation="false"
                                    onclick="btnRemoveDimension_Click" />
                            </td>
                            <td style="text-align: center" width="40%">
                                <b>Selected
                                    <asp:Label ID="lblSelectedDimensions" runat="server" Text="Dimensions"></asp:Label></b><br />
                                <br />
                                <asp:ListBox ID="lbSelectedDimensions" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="lbSelectedDimensions_SelectedIndexChanged" 
                                    SelectionMode="Multiple" Width="350px" Height="300px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <b>All Selected Items<br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center" align="center">
                                <asp:Xml ID="xmlCurrent" runat="server" ></asp:Xml>
                            </td>
                        </tr>                        
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label><br />
                                <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </ecn:BoxPanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
