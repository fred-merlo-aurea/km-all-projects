<%@ Register TagPrefix="uc1" TagName="ContactEditor" Src="../../includes/ContactEditor.ascx" %>

<%@ Page Language="c#" Inherits="ecn.accounts.customersmanager.customerdetail" CodeBehind="customerdetail.aspx.cs"
    MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 250px;
        }

        .style1 {
            width: 100%;
        }

        .overlay {
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

        * html .overlay {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }

        .loader {
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

        * html .loader {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
        }
    </style>
    <script language="javascript" type="text/javascript">

        function divexpandcollapse(divname) {

            var div = document.getElementById(divname);

            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {

                div.style.display = "inline";

                img.src = "/ecn.images/images/minus.gif";

            } else {

                div.style.display = "none";

                img.src = "/ecn.images/images/plus.gif";

            }

        }


        function deleteLicense(theID) {
            if (confirm('Are you Sure?\n Selected License will be permanently deleted.')) {
                window.location = "customerdetail.aspx?CLID=" + theID + "&CustomerID=<%=getCustomerID()%>";
            }
        }
        function deleteTemplate(theID) {
            if (confirm('Are you Sure?\n Selected Template will be permanently deleted.')) {
                window.location = "customerdetail.aspx?CTID=" + theID + "&CustomerID=<%=getCustomerID()%>";
            }
        }
        function toggleProduct(theID) {
            window.location = "customerdetail.aspx?CPID=" + theID + "&CustomerID=<%=getCustomerID()%>";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="5" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="800" border='0'
                style="padding-top: 5px; padding-bottom: 5px;">
                <tr>
                    <td>
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
                    <td bgcolor='#eeeeee'>
                        <cpanel:DataPanel ID="DataPanel1" runat="Server" ExpandImageUrl="/ecn.images/images/collapse2.gif"
                            CollapseImageUrl="/ecn.images/images/collapse2.gif" CollapseText="Click to hide Contact Details"
                            ExpandText="Click to display Customer Details" Collapsed="False" TitleText="Customer Contact Details"
                            AllowTitleExpandCollapse="True" Font-Bold="true">
                            <table cellspacing="0" cellpadding="0" width="800" bgcolor="#ffffff" border='0'>
                                <tr>
                                    <td>
                                        <table cellspacing="0" cellpadding="4" border='0'>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right'>
                                                    <asp:HiddenField ID="hfCustomerPlatformClientID" runat="server" Value="" />
                                                    Customer&nbsp;Name&nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustomerName" runat="Server" Columns="40" CssClass="formfield"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right'>Channel&nbsp;
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBaseChannels" runat="Server" CssClass="formfield" DataValueField="BaseChannelID"
                                                        DataTextField="BasechannelName" OnSelectedIndexChanged="ddlBaseChannels_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right'>Customer Type
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCustomerType" runat="Server" CssClass="formfield" DataValueField="CodeValue"
                                                        DataTextField="CodeName" Width="130px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right'></td>
                                                <td>
                                                    <asp:CheckBox ID="cbActiveStatus" runat="Server" CssClass="Label" Text="Active"></asp:CheckBox>&nbsp;
                                                    <asp:CheckBox ID="cbDemoCustomer" runat="Server" CssClass="Label" Text="Demo"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right'>MS Customer
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMSCustomer" runat="Server" CssClass="formfield" DataValueField="CustomerID"
                                                        DataTextField="CustomerName">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right'>Champion Determined By:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlAbWinnerType" runat="Server" CssClass="formfield">
                                                        <asp:ListItem Text="Opens" Value="opens" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Clicks" Value="clicks"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" style="text-align: right; vertical-align: top;">Default Blast as Test
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkDefaultBlastAsTest" CssClass="label" Text="" runat="Server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <asp:PlaceHolder ID="phAssignment" runat="Server">
                                            <table cellspacing="0" cellpadding="4" border='0'>
                                                <tr>
                                                    <td class="tableHeader" valign="middle" align='right'>Account Executive
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAccountExecutive" runat="Server" CssClass="formfield" Width="130px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tableHeader" valign="middle" align='right'>Account Manager
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAccountManager" runat="Server" CssClass="formfield" Width="130px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tableHeader" valign="middle" align='right'>Strategic
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblStrategic" runat="Server" CssClass="Label" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                                                            <asp:ListItem Value="N">No</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>

                                            </table>
                                        </asp:PlaceHolder>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="sectionHeader">General Address
                                    </td>
                                    <td class="sectionHeader">Billing Address&nbsp;&nbsp;
                                        <asp:LinkButton ID="lbtnCopy" runat="Server" CssClass="tableContent" CausesValidation="False"
                                            BorderStyle="None" Font-Underline="True" OnClick="lbtnCopy_Click">Copy General Address</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="469" height="46">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <uc1:ContactEditor ID="GeneralContact" runat="Server"></uc1:ContactEditor>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td height="46">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <uc1:ContactEditor ID="BillingContact" runat="Server"></uc1:ContactEditor>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="sectionHeader" colspan="2">Other information
                                    </td>
                                </tr>
                                <tr>
                                    <td width="468">
                                        <table>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right' width="119" height="26">Web Address
                                                </td>
                                                <td height="26">
                                                    <asp:TextBox ID="txtWebAddress" runat="Server" CssClass="formfield" Width="280px"
                                                        Size="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right' width="119">Pickup Path
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPickupPath" runat="Server" CssClass="formfield" Width="280px"
                                                        Size="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right' width="119">Mailing IP
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMailingIP" runat="Server" CssClass="formfield" Width="280px"
                                                        Size="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" valign="top" align='right' width="119">Subscription Email
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSubscriptionEmail" runat="Server" CssClass="formfield" Width="280px"
                                                        Size="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table>
                                            <tr>
                                                <td class="tableHeader" align='right' width="312">Technical Contact
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txttechContact" runat="Server" CssClass="formfield" Width="272px"
                                                        Size="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" align='right' width="312">Email
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txttechEmail" runat="Server" CssClass="formfield" Width="273px"
                                                        Size="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" align='right' width="312">Phone
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txttechPhone" runat="Server" CssClass="formfield" Width="273px"
                                                        Size="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" colspan="2">&nbsp;
                                    </td>
                                </tr>
                                
                            </table>
                        </cpanel:DataPanel>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
					<td class="sectionHeader" colspan="2">Products and Features</td>
				</tr>
                <tr>
                    <td align="center" colspan="2" bgcolor='#eeeeee'>
                        <asp:UpdatePanel ID="upServiceFeatures" runat="server">
                            <ContentTemplate>


                                <telerik:RadTreeList runat="server"
                                    ID="tlClientServiceFeatures"
                                    ClientSettings-AllowPostBackOnItemClick="false"
                                    OnItemCreated="tlClientServiceFeatures_ItemCreated" 
                                    OnNeedDataSource="tlClientServiceFeatures_NeedDataSource"
                                    DataKeyNames="ID" 
                                    ParentDataKeyNames="PID"
                                    AutoGenerateColumns="False" 
                                    Style="width: 826px" 
                                    AllowPaging="false" 
                                    AllowMultiItemSelection="True" 
                                    AllowMultiItemEdit="true">
                                    <Columns>
                                        <telerik:TreeListSelectColumn UniqueName="SelectColumn" HeaderStyle-Width="38px" />
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Product" DataField="ServiceName" UniqueName="Description" DataType="System.String" HeaderStyle-Width="150px"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Feature" DataField="ServiceFeatureName" UniqueName="Description" DataType="System.String" HeaderStyle-Width="200px"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="Description" DataField="Description" UniqueName="Description" DataType="System.String" HeaderStyle-Width="100%"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceOrServiceFeatureClientMapID" DataField="MAPID" UniqueName="MAPID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceID" DataField="ServiceID" UniqueName="ServiceID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ServiceFeatureID" DataField="ServiceFeatureID" UniqueName="ServiceFeatureID" DataType="System.Int32" HeaderStyle-Width="0px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="IsAdditionalCost" DataField="IsAdditionalCost" UniqueName="IsAdditionalCost" DataType="System.Boolean" HeaderStyle-Width="50px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                        <telerik:TreeListBoundColumn ReadOnly="true" HeaderText="ParentID" DataField="PID" UniqueName="PID" DataType="System.String" HeaderStyle-Width="50px" Visible="false" Display="false"></telerik:TreeListBoundColumn>
                                    </Columns>
                                    <ClientSettings AllowPostBackOnItemClick="false" ClientEvents-OnItemClick="" ClientEvents-OnItemSelected="OnClientNodeClicked" Selecting-UseSelectColumnOnly="true" Selecting-AllowItemSelection="true" ClientEvents-OnItemDeselected="OnClientNodeClicked" />
                                </telerik:RadTreeList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" align="center" colspan='3'>
                        <br />
                        <asp:Button class="formbutton" ID="btnSave" OnClick="btnSave_Click" runat="Server"
                            Text="Create New Customer" Visible="true"></asp:Button>
                        <%--                       <asp:Button class="formbutton" ID="btnUpdate" OnClick="btnUpdate_Click" runat="Server" Text="Update Customer record"
                        Visible="false"></asp:Button>--%>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" bgcolor='#eeeeee'>
                        <cpanel:DataPanel ID="Datapanel3" runat="Server" ExpandImageUrl="/ecn.images/images/collapse2.gif"
                            CollapseImageUrl="/ecn.images/images/collapse2.gif" CollapseText="Click to Templates, Quotes &amp; Licenses Authorized"
                            ExpandText="Click to Templates, Quotes &amp; Licenses" Collapsed="False" TitleText="Templates, Quotes &amp; Licenses"
                            AllowTitleExpandCollapse="True">
                            <table width="100%" bgcolor="#ffffff">
                                <tr>
                                    <td class="sectionHeader" colspan="2">Contacts
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <u>
                                            <asp:HyperLink ID="hlAddContacts" Text="Add Contacts" runat="server"></asp:HyperLink>
                                        </u>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdContacts" AllowPaging="true" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                                            runat="server" GridLines="None" DataKeyNames="ContactID, CustomerID" AllowSorting="false"
                                            OnPageIndexChanging="grdContacts_PageIndexChanging" OnRowDataBound="grdContacts_RowDataBound">
                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                                            <Columns>
                                                <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="20%" ItemStyle-Width="20%"></asp:BoundField>
                                                <asp:BoundField DataField="LastName" HeaderText="Last Name" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="20%" ItemStyle-Width="20%"></asp:BoundField>
                                                <asp:BoundField DataField="Email" DataFormatString="<a href=mailto:{0}>{0}</a>" HtmlEncodeFormatString="false"
                                                    HeaderText="Email" HeaderStyle-Width="20%" ItemStyle-Width="20%" />
                                                <asp:BoundField DataField="Phone" HeaderText="Phone" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="15%" ItemStyle-Width="15%"></asp:BoundField>
                                                <asp:BoundField DataField="Mobile" HeaderText="Mobile" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="15%" ItemStyle-Width="15%"></asp:BoundField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlEdit" runat="server" NavigateUrl="" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Contact' border='0'&gt;" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="sectionHeader" colspan="2">Notes
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <u>
                                            <asp:HyperLink ID="hlAddNotes" Text="Add Notes" runat="server"></asp:HyperLink>
                                        </u>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdNotes" AllowPaging="true" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                                            runat="server" GridLines="None" DataKeyNames="NoteID, CustomerID" AllowSorting="false" OnRowDataBound="grdNotes_RowDataBound"
                                            OnPageIndexChanging="grdNotes_PageIndexChanging">
                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                                            <Columns>
                                                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="50%" ItemStyle-Width="50%"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Billing" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbnotes" runat="server"><%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsBillingNotes"))?"Yes":"" %></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User Name" HeaderStyle-Width="20%"
                                                    ItemStyle-Width="20%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNoteUserName" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Updated Date" HeaderStyle-Width="20%"
                                                    ItemStyle-Width="20%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNoteUpdatedDate" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="sectionHeader" colspan="2">Templates
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdTemplates" AllowPaging="true" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                                            runat="server" GridLines="None" DataKeyNames="CTID" AllowSorting="false" OnPageIndexChanging="grdTemplates_PageIndexChanging"
                                            OnRowDeleting="grdTemplates_RowDeleting" OnRowCommand="grdTemplates_RowCommand">
                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                                            <Columns>
                                                <asp:BoundField DataField="CodeName" HeaderText="Type"></asp:BoundField>
                                                <asp:BoundField DataField="UpdatedDate" HeaderText="Modified" HeaderStyle-Width="20%"
                                                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundField>
                                                <asp:BoundField DataField="Active" HeaderText="Active" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="center"
                                                    HeaderStyle-HorizontalAlign="center"></asp:BoundField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="4%" ItemStyle-VerticalAlign="middle">
                                                    <ItemTemplate>
                                                        <a href='templatedetail.aspx?CTID=<%# DataBinder.Eval(Container.DataItem, "CTID") %>'>
                                                            <asp:Label Text="<center><img border='0' src=/ecn.images/images/icon-edits1.gif alt='Edit Customer Template'></center>"
                                                                runat="Server" ID="Label1" NAME="Label1" /></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CTID") %>' CommandName="Delete"
                                                            OnClientClick="return confirm('Are you Sure?\n Selected Template will be permanently deleted.');"
                                                            CausesValidation="false" runat="server"><img src="/ecn.images/images/icon-delete1.gif" alt='Delete Customer Template' border='0'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="sectionHeader" colspan="2">Quotes
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdQuote" AllowPaging="true" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                                            runat="server" GridLines="None" DataKeyNames="QuoteID" AllowSorting="false" OnPageIndexChanging="grdQuote_PageIndexChanging"
                                            OnRowDataBound="grdQuote_RowDataBound" OnRowDeleting="grdQuote_RowDeleting" OnRowCommand="grdQuote_RowCommand">
                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <a href="JavaScript:divexpandcollapse('div<%# Eval("QuoteID") %>');">
                                                            <img id="imgdiv<%# Eval("QuoteID") %>" width="15px" border="0" src="/ecn.images/images/plus.gif" />
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="QuoteID">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# string.Format("Q{0:0000}_{1:000000}_{2:00}{3:00}{4}_{5}", Eval("ChannelID"), Eval("CustomerID"), Eval("CreatedDate.Month"), Eval("CreatedDate.Day"), Eval("CreatedDate.Year"),  Convert.ToInt32(Eval("QuoteID")) == -1 ?"New": Eval("QuoteID").ToString() )%>'
                                                            runat="Server" ID="Label1" NAME="Label1" /></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CreatedDate" HeaderText="Create Date" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:BoundField DataField="StartDate" HeaderText="Start Date" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:BoundField DataField="UpdatedDate" HeaderText="Change Date" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnEdit" CommandArgument='<%# Eval("QuoteID") %>' CommandName="Edit"
                                                            runat="server"><img src="/ecn.images/images/icon-edits1.gif" border='0' alt='Edit Quote'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# Eval("QuoteID") %>' CommandName="Delete"
                                                            OnClientClick="return confirm('Are you Sure?\n Selected License will be permanently deleted.');"
                                                            runat="server"><img src="/ecn.images/images/icon-delete1.gif" border='0' alt='Delete Quote'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td colspan="100%">
                                                                <div id="div<%# Eval("QuoteID") %>" style="display: none; position: relative; left: 15px; overflow: auto">
                                                                    <asp:GridView ID="grdQuoteItem" AllowPaging="true" CssClass="grid" Width="95%" AutoGenerateColumns="false"
                                                                        runat="server" GridLines="None" DataKeyNames="QuoteID" AllowSorting="false" OnRowDataBound="grdQuoteItem_RowDataBound">
                                                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                                        <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="LicenseType" HeaderText="License Type" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="Rate" HeaderText="Rate" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="Quantity" HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="FrequencyType" HeaderText="Frequency" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="Rate" HeaderText="Total" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left" DataFormatString="${0:###,###.00}" />
                                                                            <asp:BoundField DataField="PriceType" HeaderText="Price Type" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:BoundField DataField="IsActive" HeaderText="Active" HeaderStyle-HorizontalAlign="Left"
                                                                                ItemStyle-HorizontalAlign="Left" />
                                                                            <asp:TemplateField HeaderText="IsCustomerCredit" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label Text='<%# Eval("IsCustomerCredit")%>' runat="Server" ID="lblIsCustomerCredit" /></a>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableHeader" colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="sectionHeader" colspan="2">Licenses
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdLicense" AllowPaging="false" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                                            runat="server" GridLines="None" DataKeyNames="CLID" AllowSorting="false" OnRowDeleting="grdLicense_RowDeleting"
                                            OnRowCommand="grdLicense_RowCommand">
                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                                            <Columns>
                                                <asp:BoundField DataField="LicenseTypeCode" HeaderText="Type" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="20%" ItemStyle-Width="20%"></asp:BoundField>
                                                <asp:BoundField DataField="AddDate" HeaderText="Start" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="20%" ItemStyle-Width="20%"></asp:BoundField>
                                                <asp:BoundField DataField="ExpirationDate" HeaderText="Expiration" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="20%" ItemStyle-Width="20%"></asp:BoundField>
                                                <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="15%" ItemStyle-Width="15%"></asp:BoundField>
                                                <asp:BoundField DataField="Used" HeaderText="Used" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="15%" ItemStyle-Width="15%"></asp:BoundField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" ItemStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlView" runat="server" NavigateUrl='<%# Eval("CLID", "LicenseEdit.aspx?CLID={0}") %>'
                                                            Text="View" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# Eval("CLID") %>' CommandName="Delete"
                                                            OnClientClick="return confirm('Are you Sure?\n Selected License will be permanently deleted.');"
                                                            runat="server"><img src="/ecn.images/images/icon-delete1.gif" border='0' alt='Delete License'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </cpanel:DataPanel>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td bgcolor='#eeeeee'>
                        <asp:GridView ID="grdFeatures" AllowPaging="false" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                            runat="server" GridLines="None" DataKeyNames="CustomerProductID, ProductID, HasWebsiteTarget"
                            AllowSorting="false" OnRowCommand="grdFeatures_RowCommand" OnRowEditing="grdFeatures_RowEditing"
                            OnRowDataBound="grdFeatures_RowDataBound">
                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                            <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ProductName" HeaderText="Product" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                <asp:BoundField DataField="ProductDetailName" HeaderText="Feature Name" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductDetailDesc" runat="server" Text='<%# Eval("ProductDetailDesc") %>'></asp:Label>
                                        <asp:Panel ID="WebsiteTargetPanel" runat="server" Visible='<%# Eval("HasWebsiteTarget") %>'>
                                            <p />
                                            <asp:Label ID="WebsiteTargetLabel" runat="server"></asp:Label>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="UpdatedDate" HeaderText="Modified" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Website Target"
                                    ItemStyle-Width="5%" Visible="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="EditWebsiteTargetButton" runat="server" Visible='<%# Eval("HasWebsiteTarget") %>'
                                            CommandName="showPopup" CommandArgument='<%# Eval("ProductID") %>'>
                                        <img src='<%#"/ecn.images/images/button_link.gif" %>' border='0' alt="Edit Website Target">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" ItemStyle-Width="5%"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnToggle" CommandArgument='<%# Eval("CustomerProductID") %>'
                                            CommandName="Toggle" runat="server"><img src='<%# Eval("Active").ToString() == "n" ? "/ecn.images/images/icon-delete1.gif" : "/ecn.images/images/tick.gif" %>' border='0'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />

    <%--    <asp:HiddenField ID="hiddenField_ProductDescription" runat="server" />
    <asp:HiddenField ID="hiddenField_SelectedProductID" runat="server" />--%>
    <asp:Button ID="btnShowPopup5" runat="server" Style="display: none" />

    <ajax:ModalPopupExtender ID="mdlWebsiteTarget" runat="server" TargetControlID="btnShowPopup5"
        PopupControlID="pnlWebsiteTarget" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="pnlWebsiteTarget" runat="server" Width="360px" Height="160px" Style="display: none"
        CssClass="modalPopup">
        <asp:UpdatePanel ID="ModalPopupUpdatePanel" runat="server">
            <ContentTemplate>
                <div id="Div1" align="center" style="text-align: center; height: 160px; padding: 10px 10px 10px 10px;"
                    runat="server">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">
                                <asp:Label ID="lblWebsiteTarget" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 10px" align="left">Website Address
                                <asp:TextBox ID="txtWebsiteAddress" Width="250px" runat="server" MaxLength="50" value=""></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqDownloadCount" runat="server" ControlToValidate="txtWebsiteAddress"
                                    ErrorMessage="* required" Display="Dynamic" ValidationGroup="WebsiteAddress"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSaveWebsiteTarget" runat="server" Text="Save" CssClass="button"
                                    CommandArgument='<%# Eval("ProductID") %>' OnClick="btnSaveWebsiteTarget_Click"
                                    ValidationGroup="WebsiteAddress" />
                                <asp:Button ID="btnCloseWebsiteTarget" Text="Cancel" CssClass="button" runat="server"
                                    OnClick="btnCloseWebsiteTarget_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Button ID="Button1" runat="server" Style="display: none" />

    <ajax:ModalPopupExtender ID="WebsiteAddressErrorExtender" runat="server" TargetControlID="Button1"
        PopupControlID="WebsiteTargetErrorPanel" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="WebsiteTargetErrorPanel" runat="server" Width="360px" Height="163px" Style="display: none"
        CssClass="modalPopup">
        <asp:UpdatePanel ID="WebsiteTargetErrorUpdatePanel" runat="server">
            <ContentTemplate>
                <div id="Div2" align="center" style="text-align: center; height: 140px; padding: 10px 10px 10px 10px;"
                    runat="server">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">
                                <asp:Label ID="WebsiteTargetErrorHeaderLabel" runat="server" Text="Error"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 10px" align="center">
                                <asp:Panel ID="WebsiteTargetErrorMessagePanel" Width="100%" runat="server" ScrollBars="Auto">
                                    <asp:TextBox ID="WebsiteTargetErrorText" runat="server" Enabled="false" TextMode="MultiLine"></asp:TextBox>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="CloseButton" runat="server" Text="Close" CssClass="button"
                                    OnClick="CloseButton_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
