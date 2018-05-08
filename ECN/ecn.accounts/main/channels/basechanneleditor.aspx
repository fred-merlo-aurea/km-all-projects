<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.accounts.channelsmanager.basechanneleditor"
    CodeBehind="basechanneleditor.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="uc1" TagName="ChannelPartnerRatesEditor" Src="../../includes/ChannelPartnerRatesEditor.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ContactEditor" Src="../../includes/ContactEditor.ascx" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls.HtmlTextBox" Assembly="ActiveUp.WebControls.HtmlTextBox" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            var parenItemSelected = false;
            var originalNode = null;
            function OnClientNodeClicked(sender, args) {

                var currNode = args.get_item();
                if (!originalNode) {
                    originalNode = currNode;
                }

                var childNodes = currNode.get_childItems();
                if (childNodes.length > 0 && parenItemSelected == false) {
                    if (currNode.get_selected()) {
                        //CheckAllChildren(childNodes, childNodes.length);
                    }
                    else {
                        UnCheckAllChildren(currNode, childNodes, childNodes.length);
                    }
                }

                if (originalNode == currNode) {
                    DoParents(originalNode);
                    parenItemSelected = false;
                    originalNode = null;
                }


            }

            function DoParents(currNode) {
                var parent = currNode.get_parentItem();
                if (parent) {
                    parenItemSelected = true;
                    var selected = currNode.get_selected();
                    if (selected == true) {
                        parent.set_selected(currNode.get_selected());
                        DoParents(parent);
                    }
                    else {
                        var children = parent.get_childItems();
                        var canUnselect = true;
                        for (var i = 0; i < children.length ; i++) {
                            var currentChild = children[i];
                            if (currentChild.get_selected()) {
                                canUnselect = false;
                                break;
                            }
                        }
                        //if (canUnselect == true) {
                        //    parent.set_selected(false);
                        //    DoParents(parent);
                        //}
                    }

                }
            }
            

            function CheckSiblings(currNode) {
                var parent = currNode.get_parentItem();
                if (parent) {
                    var sibling = parent.get_childItems();
                    if (sibling.length == 1) {
                        if (parent.get_selected())
                            parent.set_selected(false);
                    }
                    else {
                        var deselectParent = false;
                        for (var i = 0; i < sibling.length; i++) {
                            if (sibling[i].get_selected()) {
                                deselectParent = false;
                                break;
                            }
                            else {
                                deselectParent = true;
                            }
                        }
                        if (deselectParent) {
                            if (parent.get_selected())
                                parent.set_selected(false);
                        }
                    }
                }
            }

            function UnCheckAllChildren(currNode, nodes, nodecount) {
                var i;
                for (i = 0; i < nodecount; i++) {
                    if (nodes[i].get_selected())
                        nodes[i].set_selected(false);
                }
                if (currNode.get_selected())
                    currNode.set_selected(false);
            }

            function CheckAllChildren(nodes, nodecount) {
                var i;

                for (i = 0; i < nodecount; i++) {
                    if (!nodes[i].get_selected()) {
                        nodes[i].set_selected(true);
                    }
                    CheckAllChildren(nodes[i].get_childItems(), nodes[i].get_childItems().length);
                }

            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border="0"
        align="center">
        <tr>
            <td colspan="3">
                <br />
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
                    <br />
                    <br />
                </asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <td class="sectionHeader" colspan='3' align="left">&nbsp;General Info
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align='right' width="125">&nbsp;Name&nbsp;
            </td>
            <td width="400" align="left">
                <asp:HiddenField ID="hfBaseChannelPlatformClientGroupID" runat="server" Value="" />
                <asp:HiddenField ID="hfBaseChannelGuid" runat="server" Value="" />
                <asp:TextBox ID="tbChannelName" CssClass="formfield" runat="Server" Size="40" EnableViewState="true" Width="280px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbChannelName" ErrorMessage="*" SetFocusOnError="False" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
            <td rowspan="5"></td>
        </tr>
        <tr>
            <td class="tableHeader" align='right' width="119">ChannelURL
            </td>
            <td width="292" align="left">
                <asp:TextBox ID="tbChannelURL" CssClass="formfield" runat="Server" Size="50" Width="280px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align='right' width="119">BounceDomain
            </td>
            <td width="292" align="left">
                <asp:TextBox ID="tbBounceDomain" CssClass="formfield" runat="Server" Size="30" Width="280px" ReadOnly
                    value="bounce2.com"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align='right' width="119">Web Address
            </td>
            <td width="292" align="left">
                <asp:TextBox ID="tbWebAddress" CssClass="formfield" runat="Server" Size="50" Width="280px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align='right' width="119">Channel Type
            </td>
            <td width="292" align="left">
                <asp:DropDownList ID="ddlChannelType" runat="Server" Width="136px" CssClass="formfield">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align='right' width="119">Partner Type
            </td>
            <td width="292" align="left">
                <asp:DropDownList ID="ddlChannelPartnerType" runat="Server" Width="136px" CssClass="formfield">
                    <asp:ListItem Value="4">Not applicable</asp:ListItem>
                    <asp:ListItem Value="1">Silver Program</asp:ListItem>
                    <asp:ListItem Value="2">Gold Program</asp:ListItem>
                    <asp:ListItem Value="3">Platinum Program</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align='right' width="119">MS Customer
            </td>
            <td width="292" align="left">
                <asp:DropDownList ID="ddlMSCustomer" runat="Server" CssClass="formfield" DataValueField="CustomerID"
                    DataTextField="CustomerName">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" style="text-align: right; width: 119px;">Active
            </td>
            <td style="width: 292px; text-align: left;">
                <asp:RadioButtonList ID="rblActive" RepeatDirection="Horizontal" runat="server">
                    <asp:ListItem Text="Yes" Value="yes" Selected="True" />
                    <asp:ListItem Text="No" Value="no" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td colspan='3'>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="sectionHeader" colspan='3' align="left">Contact
            </td>
        </tr>
        <tr>
            <td class="tableContent" colspan='3' align="left">
                <uc1:ContactEditor ID="ContactEditor" runat="Server"></uc1:ContactEditor>
            </td>
        </tr>
        <tr>
            <td width="119"></td>
        </tr>
        <tr>
            <td colspan='3'>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="sectionHeader" colspan='3' align="left">Products and Features
            </td>
        </tr>
        <tr>
            <td align="center" colspan='3'>
                <asp:UpdatePanel ID="upServiceFeatures" runat="server">
                    <ContentTemplate>
                        <telerik:RadTreeList ID="tlClientGroupServiceFeatures"
                            runat="server"
                            DataKeyNames="ID" 
                            ParentDataKeyNames="PID"
                            ClientSettings-AllowPostBackOnItemClick="false" 
                            AllowMultiItemEdit="true"
                            OnItemCreated="tlClientGroupServiceFeatures_ItemCreated"
                            OnNeedDataSource="tlClientGroupServiceFeatures_NeedDataSource"
                            AllowPaging="false"
                            AllowMultiItemSelection="True"
                            AutoGenerateColumns="False" 
                            Style="width: 100%;">
                            <Columns>
                                <telerik:TreeListSelectColumn UniqueName="SelectColumn" HeaderStyle-Width="38px" />
                                <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Product" DataField="ServiceName" UniqueName="Product" DataType="System.String" HeaderStyle-Width="200px"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Feature" DataField="ServiceFeatureName" UniqueName="Feature" DataType="System.String" HeaderStyle-Width="200px"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Description" DataField="Description" UniqueName="Description" DataType="System.String" HeaderStyle-Width="80%"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceOrServiceFeatureClientGroupMapID" DataField="MAPID" UniqueName="MAPID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceID" DataField="ServiceID" UniqueName="ServiceID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceFeatureID" DataField="ServiceFeatureID" UniqueName="ServiceFeatureID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="IsAdditionalCost" DataField="IsAdditionalCost" UniqueName="IsAdditionalCost" DataType="System.Boolean" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ID" DataField="ID" UniqueName="ID" DataType="System.String" Display="false" HeaderStyle-Width="10%" ></telerik:TreeListBoundColumn>
                                <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ParentID" DataField="PID" UniqueName="PID" DataType="System.String" Display="false" HeaderStyle-Width="10%" ></telerik:TreeListBoundColumn>
                            </Columns>
                            <ClientSettings AllowPostBackOnItemClick="false" ClientEvents-OnItemSelected="OnClientNodeClicked" Selecting-UseSelectColumnOnly="true" Selecting-AllowItemSelection="true" ClientEvents-OnItemDeselected="OnClientNodeClicked" />
                        </telerik:RadTreeList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align="center" colspan='3'>
                <br />
                <asp:TextBox ID="ChannelID" runat="Server" EnableViewState="true" Visible="false"></asp:TextBox><asp:Button
                    class="formbutton" ID="btnSave" OnClick="btnSave_Click" runat="Server" Visible="true"
                    Text="Create"></asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan='3'>
                <hr color="#000000" size="1">
            </td>
            <td></td>
        </tr>
    </table>
</asp:Content>
