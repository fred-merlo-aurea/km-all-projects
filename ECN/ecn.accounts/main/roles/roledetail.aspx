<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="roledetail.aspx.cs" Inherits="ecn.accounts.main.roles.roledetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">        
    <link rel='stylesheet' href="/ecn.communicator/MasterPages/ECN_MainMenu.css" type="text/css" />
    <link rel='stylesheet' href="/ecn.communicator/MasterPages/ECN_Controls.css" type="text/css" />
    <script type="text/javascript" language="javascript">


        var parenItemSelected = false;
        var originalNode = null;
        function OnClientNodeClicked(sender, args) {
            
            var currNode = args.get_item();
            if (!originalNode)
            {
                originalNode = currNode;
            }

            var childNodes = currNode.get_childItems();
            if (childNodes.length > 0 && parenItemSelected == false)
            {
                if (currNode.get_selected()) {
                    CheckAllChildren(childNodes, childNodes.length);
                }
                else
                {
                    UnCheckAllChildren(currNode, childNodes, childNodes.length);
                }
            }

            if (originalNode == currNode)
            {
                DoParents(originalNode);

                var perm = sender.getCellByColumnUniqueName(originalNode, "Permission").innerText;
                if (originalNode.get_selected()) {
                    if (perm != "View") {
                        CheckForView(originalNode, sender);
                    }
                }
                else
                {
                    if (perm == "View") {
                        CheckForViewDeselect(currNode, sender);
                    }
                }

                parenItemSelected = false;
                originalNode = null;
            }


        }

        function DoParents(currNode)
        {
            var parent = currNode.get_parentItem();
            if(parent)
            {
                parenItemSelected = true;
                var selected = currNode.get_selected();
                if (selected == true)
                {
                    parent.set_selected(currNode.get_selected());
                    DoParents(parent);
                }
                else
                {
                    var children = parent.get_childItems();
                    var canUnselect = true;
                    for(var i = 0;i < children.length ;i++)
                    {
                        var currentChild = children[i];
                        if(currentChild.get_selected())
                        {
                            canUnselect = false;
                            break;
                        }
                    }
                    if(canUnselect == true)
                    {
                        parent.set_selected(false);
                        DoParents(parent);
                    }
                }
                
            }
        }

        function CheckForView(currNode, sender) {
            var parentItem = currNode.get_parentItem();
            var perm = sender.getCellByColumnUniqueName(currNode, "Permission").innerText;
            if (perm != "View") {
                if (parentItem) {
                    var siblings = parentItem.get_childItems();
                    if (siblings) {
                        for (var i = 0; i < siblings.length; i++) {
                            if (sender.getCellByColumnUniqueName(siblings[i], "Permission").innerText == "View" && !siblings[i].get_selected()) {
                                siblings[i].set_selected(true);
                            }
                        }
                    }
                }
            }

        }

        function CheckForViewDeselect(currNode, sender) {
            var parentItem = currNode.get_parentItem();
            if (parentItem) {
                var perm = sender.getCellByColumnUniqueName(currNode, "Permission").innerText;
                if (perm == "View") {
                    var siblings = parentItem.get_childItems();
                    if (siblings) {
                        for (var i = 0; i < siblings.length; i++) {
                            if (siblings[i].get_selected())
                                siblings[i].set_selected(false);
                        }
                        parentItem.set_selected(false);
                    }
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
        <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="5" Visible="true"
        AssociatedUpdatePanelID="ProductFeatureUpdatePannel" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="http://images.ecn5.com/images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="ProductFeatureUpdatePannel" UpdateMode="Conditional" runat="server">
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
            <asp:HiddenField ID="hfSecurityGroupID" Value="" runat="server" />
            <table style="width: 100%;">
                <asp:Panel ID="ClientGroupDropDownPanel" runat="server">
                    <tr>
                        <td width="120px">Channel:</td>
                        <td>
                            <asp:DropDownList ID="drpclientgroup" OnSelectedIndexChanged="drpclientgroup_SelectedIndexChanged"
                                AutoPostBack="true"
                                OnDataBound="drpclientgroup_DataBound" DataTextField="clientgroupname" DataValueField="clientgroupID" runat="server"
                                CssClass="ECNLabel10">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td>Role Name:</td>
                    <td>
                        <asp:TextBox ID="tbSecurityGroupName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td>
                        <asp:TextBox ID="tbSecurityGroupDescription" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <asp:Panel ID="IsActiveRadioButtonPanel" runat="server">
                    <tr>
                        <td>Active?</td>
                        <td>
                            <asp:RadioButtonList ID="rbIsActive" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="IsChannelWideRadioButtonPanel" runat="server">
                    <tr>
                        <td>Will this be used across all customers?</td>
                        <td>
                            <asp:RadioButtonList ID="rbIsChannelWide" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="rbIsChannelWide_SelectedIndexChanged">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value=""></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="ClientDropDownPanel" runat="server">
                    <tr>
                        <td>Customer:</td>
                        <td>
                            <asp:DropDownList ID="drpClient" DataTextField="clientName" AutoPostBack="true"
                                OnSelectedIndexChanged="drpClient_SelectedIndexChanged" DataValueField="clientID" runat="server"
                                CssClass="ECNLabel10">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </asp:Panel>

                <tr>
                    <th colspan="2">&nbsp;</th>
                </tr>
                <tr>
                    <th colspan="2">Product/Feature Access</th>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="upRoleAccess" runat="server">
                            <ContentTemplate>


                                <telerik:RadTreeList runat="server"
                                    ID="tlSecurityGroupAccess"
                                    ClientSettings-AllowPostBackOnItemClick="false"
                                    OnItemCreated="tlSecurityGroupAccess_ItemCreated"
                                    OnNeedDataSource="tlSecurityGroupAccess_NeedDataSource"
                                    DataKeyNames="ID"
                                    ParentDataKeyNames="PID"
                                    AutoGenerateColumns="False"
                                    Style="width: 100%;"
                                    AllowPaging="false"
                                    AllowMultiItemSelection="True"
                                    AllowMultiItemEdit="true">
                                    <Columns>
                                        <telerik:TreeListSelectColumn UniqueName="SelectColumn" HeaderStyle-Width="38px" HeaderText="Select" />
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Product" DataField="ServiceName" UniqueName="SName" DataType="System.String" HeaderStyle-Width="200px"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Feature" DataField="ServiceFeatureName" UniqueName="SFName" DataType="System.String" HeaderStyle-Width="200px"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Permission" DataField="AccessLevelName" UniqueName="Permission" DataType="System.String" HeaderStyle-Width="150px"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Description" DataField="Description" UniqueName="Description" DataType="System.String" HeaderStyle-Width="100%"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceOrServiceFeatureClientMapID" DataField="MAPID" UniqueName="MAPID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceID" DataField="ServiceID" UniqueName="ServiceID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceFeatureID" DataField="ServiceFeatureID" UniqueName="ServiceFeatureID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceFeatureAccessMapID" DataField="ServiceFeatureAccessMapID" UniqueName="ServiceFeatureAccessMapID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="IsAdditionalCost" DataField="IsAdditionalCost" UniqueName="IsAdditionalCost" DataType="System.Boolean" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ParentID" DataField="PID" UniqueName="PID" DataType="System.String" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                    </Columns>
                                    <ClientSettings AllowPostBackOnItemClick="false" ClientEvents-OnItemClick="" ClientEvents-OnItemSelected="OnClientNodeClicked" Selecting-UseSelectColumnOnly="true" Selecting-AllowItemSelection="true" ClientEvents-OnItemDeselected="OnClientNodeClicked" />
                                </telerik:RadTreeList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;">
                        <asp:Button runat="server" ID="SubmitButton" Text="Create" AccessKey="S" OnClick="SubmitButton_Click" />
                    </td>
                    
                        
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
