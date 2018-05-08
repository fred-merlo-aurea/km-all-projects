<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="filters.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Group.filters" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<style type="text/css">
    .accordionHeader
    {
        border: 1px solid #2F4F4F;
        background-color: Gray;
        font-family: Arial, Sans-Serif;
        color: white;
        font-weight: bold;
        padding: 5px;
        margin-top: 5px;
        cursor: pointer;
    }
    
    .accordionHeaderSelected
    {
        border: 1px solid #2F4F4F;
        background-color: Gray;
        font-family: Arial, Sans-Serif;
        font-weight: bold;
        color: white;
        padding: 5px;
        margin-top: 5px;
        cursor: pointer;
    }
    .modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
    .modalPopupFull
    {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        width: 100%;
        height: 100%;
    }
    .modalPopup
    {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
    }
    .style1
    {
        width: 100%;
    }
    .buttonMedium
    {
        width: 135px;
        background: url(buttonMedium.gif) no-repeat left top;
        border: 0;
        font-weight: bold;
        color: #ffffff;
        height: 20px;
        cursor: pointer;
        padding-top: 2px;
    }
    .TransparentGrayBackground
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        height: 100%;
        width: 100%;
        min-height: 100%;
        min-width: 100%;
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
    .MyCalendar 
    {
        background-color:white;
    }
</style>
<asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="upMain" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
            <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
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
<asp:UpdatePanel ID="upMain" runat="server">
    <ContentTemplate>
        <table width="100%">
            <tr>
                <td>
                    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                        <table cellspacing="0" cellpadding="0" width="100%" align="center">
                            <tr>
                                <td id="errorTop">
                                </td>
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
                                <td id="errorBottom">
                                </td>
                            </tr>
                        </table>
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tbody>
                            <tr>
                                <td>
                                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="tableHeader" align="right" valign="middle">
                                                Filter Name&nbsp;&nbsp;
                                            </td>
                                            <td class="label" align="left" valign="middle">
                                                <asp:TextBox class="formtextfield" ID="txtFilterName" runat="Server" EnableViewState="true"
                                                    Columns="25"></asp:TextBox>&nbsp;&nbsp;
                                            </td>
                                            <td class="tableHeader" align="right" valign="middle">
                                                Filter Group Connector&nbsp;&nbsp;
                                            </td>
                                            <td class="label" align="left" valign="middle">
                                                <asp:DropDownList ID="ddlGroupCompareType" runat="server" CssClass="formfield">
                                                    <asp:ListItem>OR</asp:ListItem>
                                                    <asp:ListItem>AND</asp:ListItem>
                                                </asp:DropDownList>&nbsp;&nbsp;
                                            </td>
                                            <td align="left" valign="middle">
                                                <asp:Button ID="btnAddFilter" runat="server" Text="Add" class="formbuttonsmall" OnClick="btnAddFilter_Click"
                                                    ValidationGroup="Filter" />&nbsp;&nbsp;
                                            </td>
                                             <td align="left" valign="middle">
                                                <asp:Button ID="btnPreview" runat="server" Text="Preview" class="formbuttonsmall"
                                                OnClick="btnPreview_Click" CausesValidation="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <hr />
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td width="187" valign="top">
                                <table cellpadding="0" cellspacing="0" width="100%" style="margin-left: 3px;">
                                    <tr height="20" width="60%">
                                        <td class="gradientTwo formLabel" style="border-right: none;">
                                            &nbsp;<span style="align: left;"> Filter Details </span>
                                        </td>
                                        <td class="gradientTwo addPage" style="border-left: none;">
                                            <div style="height: 20px; min-width: 1px;">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="greySidesLtB" style="background: #fff;" colspan="2">
                                            <div style="width: 200px; height: 450px; overflow: auto">
                                                <asp:TreeView ID="tvFilter" runat="server" OnSelectedNodeChanged="tvFilter_SelectedNodeChanged"
                                                    ShowLines="True" OnTreeNodeCollapsed="tvFilter_TreeNodeCollapsed" OnTreeNodeExpanded="tvFilter_TreeNodeExpanded"
                                                    Height="400px" ShowExpandCollapse="False">
                                                    <SelectedNodeStyle BackColor="#CCCCCC" Font-Bold="True" BorderColor="#546E96" />
                                                </asp:TreeView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table cellpadding="0" cellspacing="0" width="98%" align="center" style="margin-left: 5px;">
                                    <tr height="20">
                                        <td align="left" width="70%" class='gradientTwo formLabel' style="border-right: none;">
                                            <div>
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lblCurrentName" runat="server" Text=""></asp:Label>
                                            </div>
                                        </td>
                                        <td class="gradientTwo formLabel" width="30%" align="center">
                                            <div class="right" id="dvAddGroup" runat="server">
                                                <asp:LinkButton ID="lbAdd" runat="server" Text='<span>Add Filter Group</span>' OnClick="lbAdd_Click"></asp:LinkButton>
                                            </div>
                                            <div class="right" id="dvAddCondition" runat="server">
                                                <asp:LinkButton ID="lbAddCondition" runat="server" Text='<span>Add Condition</span>'
                                                    OnClick="lbAddCondition_Click"></asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr bgcolor="#ffffff">
                                        <td align="center" colspan="2" class='surveyPageBottomDef'>
                                            <br />
                                            <asp:Panel ID="pnlGroup" runat="server">
                                                <table cellpadding="0" cellspacing="0" width="98%" style="max-width:98%;" align="center">
                                                    <tr>
                                                        <td valign="top">
                                                            <ecnCustom:ecnGridView ID="gvFilterGroup" runat="Server" AutoGenerateColumns="False"
                                                                CssClass="grid" DataKeyNames="FilterGroupID" OnRowCommand="gvFilterGroup_RowCommand"
                                                                OnRowDeleting="gvFilterGroup_RowDeleting" OnRowEditing="gvFilterGroup_RowEditing"
                                                                PageSize="15" Width="100%" ShowEmptyTable="True" EmptyTableRowText="No Filter Groups to display">
                                                                <HeaderStyle CssClass="gridheader" />
                                                                <FooterStyle CssClass="tableHeader1" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Name" HeaderStyle-HorizontalAlign="left" HeaderText="Name"
                                                                        ItemStyle-HorizontalAlign="left">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ConditionCompareType" HeaderStyle-HorizontalAlign="center"
                                                                        HeaderText="Connector" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="5%"
                                                                        ItemStyle-Width="5%">
                                                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="5%" HeaderText="Edit"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%" InsertVisible="False">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbEditFilterGroup" runat="Server" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FilterGroupID") %>'
                                                                                CommandName="Edit" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Filter Group' border='0'&gt;"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="5%" HeaderText="Delete"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbDeleteFilterGroup" runat="Server" CausesValidation="false"
                                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FilterGroupID") %>'
                                                                                CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this filter group?')"
                                                                                Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Filter Group' border='0'&gt;"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="5%" HeaderText="Add Condition"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbAddFilterCondition" runat="Server" CausesValidation="false"
                                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FilterGroupID") %>'
                                                                                CommandName="AddFilterCondition" Text="&lt;img src=/ecn.images/images/ic-new.gif alt='Add Filter Condition' border='0'&gt;"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="gridaltrow" />
                                                            </ecnCustom:ecnGridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlCondition" runat="server">
                                                <table cellpadding="2" cellspacing="0" width="98%" style="max-width:98%;" align="center">
                                                    <tr>
                                                        <td valign="top">
                                                            <ecnCustom:ecnGridView ID="gvFilterCondition" runat="Server" AllowPaging="False"
                                                                AllowSorting="False" AutoGenerateColumns="False" CssClass="grid" DataKeyNames="FilterConditionID"
                                                                OnRowCommand="gvFilterCondition_RowCommand" OnRowDeleting="gvFilterCondition_RowDeleting"
                                                                OnRowEditing="gvFilterCondition_RowEditing" PageSize="15" Width="100%" ShowEmptyTable="True"
                                                                EmptyTableRowText="No filter conditions to display">
                                                                <HeaderStyle CssClass="gridheader" />
                                                                <FooterStyle CssClass="tableHeader1" />
                                                                <Columns>
                                                                    <%--<asp:BoundField datafield="Field" headerstyle-horizontalalign="center" 
                                                            HeaderText="Field" itemstyle-horizontalalign="center" />--%>
                                                                    <asp:TemplateField HeaderText="Field" SortExpression="" ItemStyle-HorizontalAlign="center">
                                                                        <ItemTemplate>
                                                                            <%#Eval("Field")%><%# (Eval("FieldType").ToString() == "Date") ? " (" + Eval("DatePart") + ")" : ""%></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comparator" SortExpression="" ItemStyle-HorizontalAlign="center">
                                                                        <ItemTemplate>
                                                                            <%# (int.Parse(Eval("NotComparator").ToString()) == 1) ? "Not " : "" %><%#Eval("Comparator")%></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--<asp:BoundField datafield="CompareValue" headerstyle-horizontalalign="center" 
                                                            HeaderText="CompareValue" itemstyle-horizontalalign="center" />--%>
                                                                    <asp:TemplateField HeaderText="Compare Value" SortExpression="" ItemStyle-HorizontalAlign="center"
                                                                        HeaderStyle-HorizontalAlign="center">
                                                                        <ItemTemplate>
                                                                            <%# EvalWithMaxLength("CompareValue",20)%></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="5%" HeaderText="Edit"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbEditFilterCondition" runat="Server" CausesValidation="false"
                                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FilterConditionID") %>'
                                                                                CommandName="Edit" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Filter Condition' border='0'&gt;"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="5%" HeaderText="Delete"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbDeleteFilterCondition" runat="Server" CausesValidation="false"
                                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.FilterConditionID") %>'
                                                                                CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this filter condition?')"
                                                                                Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Filter Condition' border='0'&gt;"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <AlternatingRowStyle CssClass="gridaltrow" />
                                                            </ecnCustom:ecnGridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlError" runat="server" Visible="false">
                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="mpeSubFilter" runat="server" TargetControlID="btnShowPopup1"
    PopupControlID="pnlPopupDimensions" CancelControlID="btnCloseSubFilter" BackgroundCssClass="modalBackground" />
<asp:Panel ID="pnlPopupDimensions" runat="server" Width="425px" Style="display: none"
    CssClass="modalPopup">
    <asp:Panel ID="pnlPopupDimensionsRound" runat="server" Width="425px" CssClass="modalPopup2">
        <asp:UpdateProgress ID="upProgressFilterEdit" runat="server" DisplayAfter="10" Visible="true"
            AssociatedUpdatePanelID="UpdatePanel6" DynamicLayout="true">
            <ProgressTemplate>
                <asp:Panel ID="upProgressFilterGroupP1" CssClass="overlay" runat="server">
                    <asp:Panel ID="upProgressFilterGroupP2" CssClass="loader" runat="server">
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
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">

            <ContentTemplate>
             <asp:PlaceHolder ID="phError_FilterGroup" runat="Server" Visible="false">
                        <table cellspacing="0" cellpadding="0" width="100%" align="center">
                            <tr>
                                <td>
                                    <table height="67" width="80%">
                                        <tr>
                                            <td valign="top" align="center" width="20%">
                                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                            </td>
                                            <td valign="middle" align="left" width="80%" height="100%">
                                                <asp:Label ID="lblErrorMessage_FilterGroup" runat="Server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:PlaceHolder>
                <asp:Label ID="lblFilterGroupID" runat="server" Text="" Visible="false"></asp:Label>
                <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="greySidesLtB">
                        <tr height="20">
                            <td class="gradientTwo addPage" style="border-right: none;">
                                &nbsp;<span style="font-size: 12px; font-weight: bold"> Filter Groups Add/Edit </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="5" cellspacing="5" border="0">
                                    <tr>
                                        <td class="formLabel" valign="middle" align="right" width="30%">
                                            Filter Group Name&nbsp;
                                        </td>
                                        <td class="formLabel" valign="middle" align="left" width="70%">
                                            <asp:TextBox ID="txtFilterGroupName" runat="Server" class="formtextfield" Columns="25"
                                                EnableViewState="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="val_FilterGroupName" runat="Server" CssClass="errormsg"
                                                ControlToValidate="txtFilterGroupName" ErrorMessage="Filter Group name is a required field."
                                                Display="static" ValidationGroup="Group">^^ Required ^^</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel" valign="middle" align="right">
                                            Filter Condition Connector&nbsp;
                                        </td>
                                        <td class="formLabel" valign="middle" align="left">
                                            <asp:DropDownList ID="ddlConditionCompareType" runat="server" CssClass="formfield">
                                                <asp:ListItem>OR</asp:ListItem>
                                                <asp:ListItem>AND</asp:ListItem>
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
                                            <asp:Button ID="btnAddFilterGroup" runat="server" class="formbuttonsmall" OnClick="btnAddFilterGroup_Click"
                                                Text="Add" ValidationGroup="Group" Width="90px" />
                                            &nbsp; &nbsp;
                                            <asp:Button runat="server" Text="Close" ID="btnCloseSubFilter"  OnClick="btnCloseFilterGroup_Click" class="formbuttonsmall"
                                                Width="90px"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>

<asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="mpeFilterCondition" runat="server" TargetControlID="btnShowPopup2"
    PopupControlID="pnlPopupDimensions1" CancelControlID="btnCloseFilterCondition"
    BackgroundCssClass="modalBackground" />
<asp:Panel ID="pnlPopupDimensions1" runat="server" Width="600px" Style="display: none;"
    CssClass="modalPopup">
    <asp:Panel ID="pnlPopupDimensionsRound1" runat="server" Width="600px" CssClass="modalPopup2">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10" Visible="true"
            AssociatedUpdatePanelID="UpdatePanel6" DynamicLayout="true">
            <ProgressTemplate>
                <asp:Panel ID="upProgressFilterConditionP1" CssClass="overlay" runat="server">
                    <asp:Panel ID="upProgressFilterConditionP2" CssClass="loader" runat="server">
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
             <asp:PlaceHolder ID="phError_FilterCondition" runat="Server" Visible="false">
                        <table cellspacing="0" cellpadding="0" width="100%" align="center">
                            <tr>
                                <td>
                                    <table height="67" width="80%">
                                        <tr>
                                            <td valign="top" align="center" width="20%">
                                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                            </td>
                                            <td valign="middle" align="left" width="80%" height="100%">
                                                <asp:Label ID="lblErrorMessage_FilterCondition" runat="Server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:PlaceHolder>
                <asp:Label ID="lblFilterConditionID" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label ID="lblRefGroupID" runat="server" Text="" Visible="false"></asp:Label>
                <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                    <table cellpadding="0" cellspacing="0" class="greySidesLtB">
                        <tr height="20">
                            <td align="center" class="gradientTwo formLabel" style="border-right: none;">
                                &nbsp; <span style="font-size: 12px; font-weight: bold">Filter Condition Add/Edit</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="5" cellspacing="5" border="0">
                                    <tr>
                                        <td class="formLabel" align="right" valign="middle">
                                            Field&nbsp;
                                        </td>
                                        <td class="formLabel" valign="middle" align="left">
                                            <asp:DropDownList ID="ddlField" CssClass="formfield" runat="server" OnSelectedIndexChanged="ddlField_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel" valign="middle" align="right">
                                            Field Type&nbsp;
                                        </td>
                                        <td class="formLabel" valign="middle" align="left">
                                            <asp:DropDownList ID="ddlFieldType" CssClass="formfield" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlFieldType_SelectedIndexChanged">
                                                <asp:ListItem>String</asp:ListItem>
                                                <asp:ListItem>Number</asp:ListItem>
                                                <asp:ListItem Value="Date">Date [MM/DD/YYYY]</asp:ListItem>
                                                <asp:ListItem Value="Money">Money $$</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel" valign="middle" align="right">
                                            Date Part&nbsp;
                                        </td>
                                        <td class="formLabel" valign="middle" align="left">
                                            <asp:DropDownList ID="ddlDatePart" CssClass="formfield" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="ddlDatePart_SelectedIndexChanged">
                                                <asp:ListItem>full</asp:ListItem>
                                                <asp:ListItem>month</asp:ListItem>
                                                <asp:ListItem>day</asp:ListItem>
                                                <asp:ListItem>year</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel" align="right" valign="middle">
                                            Comparator&nbsp;
                                        </td>
                                        <td class="formLabel" valign="middle" align="left">
                                            <asp:CheckBox ID="cbxNot" runat="server" Text="NOT "></asp:CheckBox>
                                            <asp:DropDownList ID="ddlComparator" CssClass="formfield" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="ddlComparator_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="formLabel" align="right" valign="middle">
                                            Value&nbsp;
                                        </td>
                                        <td class="formLabel" valign="middle" align="left">
                                            <asp:TextBox class="formtextfield" ID="txtCompareValue" runat="Server" EnableViewState="true"
                                                width="450px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="val_CompareValue" runat="Server" CssClass="errormsg"
                                                ControlToValidate="txtCompareValue" ErrorMessage="Compare Value is a required field."
                                                Display="dynamic" ValidationGroup="Condition">^^ Required ^^</asp:RequiredFieldValidator>&nbsp;
                                            <asp:ImageButton ID="ibChooseDate" runat="server" ImageUrl="/ecn.images/images/icon-calendar.gif"
                                                Visible="false" />&nbsp;
                                            <ajaxToolkit:ModalPopupExtender ID="mpeCalendar" runat="server" TargetControlID="ibChooseDate"
                                                PopupControlID="pnlPopupDimensions2" CancelControlID="btnCancelDate" BackgroundCssClass="modalBackground" />
                                            <asp:Panel ID="pnlPopupDimensions2" runat="server" Width="325px" Style="display: none;"
                                                CssClass="modalPopup">
                                                <asp:Panel ID="pnlCalendar" runat="server" Width="325px" CssClass="modalPopup2">
                                                    <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                                                        <table cellpadding="0" cellspacing="0" width="100%" class="greySidesLtB">
                                                            <tr height="20">
                                                                <td align="center" class="gradientTwo formLabel" style="border-right: none;">
                                                                    &nbsp; <span style="font-size: 12px; font-weight: bold">Choose Date</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table border="0" width="100%" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td class="label" valign="middle" align="left" width="100%">
                                                                                <asp:RadioButton ID="rbToday" runat="server" GroupName="DatePicker" Text="Today"
                                                                                    AutoPostBack="True" OnCheckedChanged="rbToday_CheckedChanged" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="label" valign="middle" align="left">
                                                                                <asp:RadioButton ID="rbTodayPlus" runat="server" GroupName="DatePicker" Text="Today"
                                                                                    AutoPostBack="True" OnCheckedChanged="rbTodayPlus_CheckedChanged" />&nbsp;&nbsp;
                                                                                <asp:DropDownList ID="ddlPlusMinus" runat="server" CssClass="formfield">
                                                                                    <asp:ListItem Value="Plus" />
                                                                                    <asp:ListItem Value="Minus" />
                                                                                </asp:DropDownList>
                                                                                &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtDays" runat="server" class="formtextfield"></asp:TextBox>&nbsp;
                                                                                <asp:RangeValidator ID="rvDays" runat="Server" CssClass="errormsg" ControlToValidate="txtDays" 
                                                                                    ErrorMessage="Enter a number between 1 and 18250." Display="Dynamic" ValidationGroup="Calendar" 
                                                                                    MaximumValue="18250" MinimumValue="1" Type="Integer" Enabled="False">
                                                                                </asp:RangeValidator>
                                                                                <asp:RequiredFieldValidator ID="rfvDays" ValidationGroup="Calendar" Display="Dynamic" CssClass="errormsg" ControlToValidate="txtDays" 
                                                                                    runat="server" ErrorMessage="Enter a number between 1 and 18250." Enabled="False">
                                                                                </asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="label" valign="middle" align="left">
                                                                                <asp:RadioButton ID="rbSelect" runat="server" GroupName="DatePicker" Text="Other"
                                                                                    AutoPostBack="True" OnCheckedChanged="rbSelect_CheckedChanged" />
                                                                                &nbsp;&nbsp;
                                                                                <asp:TextBox ID="txtDatePicker" Width="70" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;<asp:ImageButton
                                                                                    ID="btnDatePicker" runat="server" ImageUrl="/ecn.images/images/icon-calendar.gif" /><ajaxToolkit:CalendarExtender
                                                                                        ID="CalendarExtender1" runat="server" CssClass="MyCalendar" TargetControlID="txtDatePicker"
                                                                                        Format="MM/dd/yyyy" PopupButtonID="btnDatePicker" >
                                                                                    </ajaxToolkit:CalendarExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvDatePicker" ValidationGroup="Calendar" Display="Dynamic"
                                                                                    CssClass="errormsg" ControlToValidate="txtDatePicker" runat="server" ErrorMessage="Enter a valid date."
                                                                                    Enabled="False"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="100%" height="40" align="center">
                                                                                <asp:Button runat="server" Text="Choose Date" ID="btnSaveDate" class="formbuttonsmall"
                                                                                    OnClick="btnSaveDate_Click" ValidationGroup="Calendar" Width="90px"></asp:Button>
                                                                                <asp:Button runat="server" Text="Cancel" ID="btnCancelDate" class="formbuttonsmall"
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="label" align="center" valign="middle" colspan="2">
                                            <asp:Button ID="btnAddFilterCondition" runat="server" Text="Add" class="formbuttonsmall"
                                                OnClick="btnAddFilterCondition_Click" ValidationGroup="Condition" Width="90px" />&nbsp;
                                            <asp:Button runat="server" Text="Close" ID="btnCloseFilterCondition"  OnClick="btnCloseFilterCondition_Click" class="formbuttonsmall"
                                                Width="90px"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
