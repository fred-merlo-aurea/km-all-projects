<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    CodeBehind="popuptest.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.popuptest" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
 <asp:UpdatePanel ID="UpPubList" runat="server">
        <ContentTemplate>
            <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Create new Forms">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">

                        </td>
                        <td style="width: 80%; vertical-align: top;">
                            <table style="width: 100%" cellpadding="10" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:HiddenField ID="hfldFormId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Menu ID="MnuForm" Orientation="Horizontal" runat="server" CssClass="menuTabs"
                                            StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                                            >
                                            <Items>
                                                <asp:MenuItem Text="Form Details" Value="0" Selected="true"></asp:MenuItem>
                                                <asp:MenuItem Text="Response Emails" Value="1"></asp:MenuItem>
                                            </Items>
                                        </asp:Menu>
                                        <div style="background-color: #eeeeee; border-color: Black; border-style: solid;
                                            border-width: 1px">
                                            <asp:MultiView ID="MultiViewForms" runat="server" ActiveViewIndex="0">
                                                <asp:View ID="View1" runat="server">
                                                    <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 30%; text-align: left">
                                                                Form Name
                                                            </td>
                                                            <td style="width: 70%; text-align: left">
                                                                <asp:TextBox ID="txtFormName" runat="server" MaxLength="50"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="ReqFldVal1" runat="server" ControlToValidate="txtFormName"
                                                                    ErrorMessage="*" ValidationGroup="ValGroup1"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Description
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            Countries
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                        <td>
                                                                            Selected Countries
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <span style="font-size: 14px; color: Red; font-weight: bold">Profile Field </span>
                                                                <br />
                                                                <br />
                                                                <div class="reorderListDemo" style="display: none">
                                                                    <ajaxToolkit:ReorderList ID="ReorderList1" runat="server" PostBackOnReorder="false"
                                                                        CallbackCssStyle="callbackStyle" DragHandleAlignment="left" DataKeyField="PSFieldID"
                                                                        SortOrderField="sortorder" Width="100%">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%" style="border: solid 1px #eeeeee">
                                                                                <tr>
                                                                                    <td width="15%" align="left">
                                                                                        <%# Eval("ECNFieldName") %>
                                                                                    </td>
                                                                                    <td width="45%" align="left">
                                                                                        <%# Eval("DisplayName")%>
                                                                                    </td>
                                                                                    <td width="10%" align="center">
                                                                                        <%# Eval("DataType")%>
                                                                                    </td>
                                                                                    <td width="10%" align="center">
                                                                                        <%# Eval("ControlType")%>
                                                                                    </td>
                                                                                    <td width="10%" align="center">
                                                                                        <%# Eval("Required")%>
                                                                                    </td>
                                                                                    <td width="10%" align="center">
                                                                                        <a href='NewPub_FieldForm.aspx?PubId=<%# DataBinder.Eval(Container,"DataItem.PubId") %>&PFID=0&PSFieldID=<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>'
                                                                                            onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:530, width:800, objectHeight:530, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                                                            <img src="../Images/icon-edit.gif" alt='Add / Edit' style="border: 0px" /></a>
                                                                                        &nbsp;&nbsp;&nbsp;
                                                                                        <asp:ImageButton ID="imgbtnFormDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                                                            CommandName="FormDelete" OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                                                            CausesValidation="false" Visible='<%# Convert.ToBoolean(Convert.ToInt32(DataBinder.Eval(Container,"DataItem.IsDelete")) == 1 ? "true" : "false")%>' />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                        <ReorderTemplate>
                                                                            <asp:Panel ID="Panel2" runat="server" CssClass="reorderCue" />
                                                                        </ReorderTemplate>
                                                                        <DragHandleTemplate>
                                                                            <div class="dragHandle">
                                                                            </div>
                                                                        </DragHandleTemplate>
                                                                    </ajaxToolkit:ReorderList>
                                                                </div>
                                                                <asp:GridView ID="grdProfileFields" runat="server" AllowSorting="false" AllowPaging="false"
                                                                    AutoGenerateColumns="false" Width="100%" DataKeyNames="PSFieldID" ShowFooter="false"
                                                                    >
                                                                    <Columns>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPSFieldID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ECNFieldName" ItemStyle-HorizontalAlign="Left" HeaderText="Field Name"
                                                                            ItemStyle-Width="15%" />
                                                                        <asp:BoundField DataField="DisplayName" ItemStyle-HorizontalAlign="Left" HeaderText="Display Name"
                                                                            ItemStyle-Width="40%" />
                                                                        <asp:BoundField DataField="DataType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                            HeaderText="Data Type" ItemStyle-Width="10%" />
                                                                        <asp:BoundField DataField="ControlType" HeaderText="Control" ReadOnly="true" ItemStyle-Width="10%"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="Required" HeaderText="Required" ReadOnly="true" ItemStyle-Width="10%"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <a href='NewPub_FieldForm.aspx?PubId=<%# DataBinder.Eval(Container,"DataItem.PubId") %>&PFID=0&PSFieldID=<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>'
                                                                                    onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:530, width:800, objectHeight:530, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                                                    <img src="../Images/icon-edit.gif" alt='Add / Edit' style="border: 0px" /></a>
                                                                                &nbsp;&nbsp;&nbsp;
                                                                                <asp:ImageButton ID="imgbtnFormDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                                                    CommandName="FormDelete" OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                                                    CausesValidation="false" Visible='<%# Convert.ToBoolean(Convert.ToInt32(DataBinder.Eval(Container,"DataItem.IsDelete")) == 1 ? "true" : "false")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <span style="font-size: 14px; color: Red; font-weight: bold">Demographic Field</span>
                                                                <br />
                                                                <asp:GridView ID="grdDemoGraphicFields" runat="server" AllowPaging="false" AllowSorting="false"
                                                                    AutoGenerateColumns="false" Width="100%" DataKeyNames="PSFieldID" ShowFooter="false"
                                                                   >
                                                                    <Columns>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPSFieldID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ECNFieldName" HeaderText="Field  Name" ItemStyle-Width="15%"
                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="DisplayName" HeaderText="Display Name" ItemStyle-Width="40%"
                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="DataType" HeaderText="Data Type" ItemStyle-Width="10%"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="ControlType" HeaderText="Control" ItemStyle-Width="10%"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="Required" HeaderText="Required" ItemStyle-Width="10%"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <a href='NewPub_FieldForm.aspx?PubId=<%# DataBinder.Eval(Container,"DataItem.PubId") %>&PFID=0&PSFieldID=<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>'
                                                                                    onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:530, width:800, objectHeight:530, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                                                    <img src="../Images/icon-edit.gif" alt='Add / Edit' style="border: 0px" /></a>
                                                                                &nbsp;&nbsp;&nbsp;
                                                                                <asp:ImageButton ID="imgbtnFormDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                                                    CommandName="FormDelete" OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                                                    CausesValidation="false" Visible='<%# Convert.ToBoolean(Convert.ToInt32(DataBinder.Eval(Container,"DataItem.IsDelete")) == 1 ? "true" : "false")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="gridfooter">
                                                                Select Fields&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:DropDownList ID="ddlPubNonSubscriptionField" runat="server" Width="150px">
                                                                </asp:DropDownList>
                                                                &nbsp;
                                                                <a id='details' runat="server" href="NewPub_FieldForm.aspx?PubId=9" onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:530, width:800, objectHeight:530, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )"><img src='../images/btnAdd.gif' alt='Add' border='0px;'></a>
                                                                 <a id="lnkaddproduct" href="NewPub_FieldForm.aspx?PubId=9" runat="server" onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:550, width:800, objectHeight:500, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )"><img src="../images/btnAdd.gif" border="0" alt="" /></a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="text-align: right">
                                                               
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="View2" runat="server">
                                                    
                                                </asp:View>
                                            </asp:MultiView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </JF:BoxPanel>
            <asp:Button ID="btnReload" runat="server" Text="reload" OnClick="btnReload_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
