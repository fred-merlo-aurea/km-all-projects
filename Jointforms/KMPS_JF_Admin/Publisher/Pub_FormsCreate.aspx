<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    CodeBehind="Pub_FormsCreate.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.Pub_FormsCreate"
    EnableEventValidation="false" Title="KMPS Form Builder - Publication Forms" ValidateRequest="false" %>

<%@ Register Src="~/Publisher/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="lftMenu" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript" src="../scripts/telerik_replaceChars.js"></script>
    <script type="text/javascript">
        function OnClientLoad(editor, args)    
    {    
       editor.get_filtersManager().add(new RadEditorCustomFilter());    
    }    
    RadEditorCustomFilter = function()    
    {    
       RadEditorCustomFilter.initializeBase(this);    
       this.set_isDom(false);    
       this.set_enabled(true);    
       this.set_name("RadEditor HTMLCharacter");    
       this.set_description("RadEditor Convert characters to html entities");    
    }
    
    RadEditorCustomFilter.prototype =    
    {    
       getHtmlContent : function(content)    
       {
           var regUni = new RegExp('[\u2600-\u26FF]');
           var newContent = content;
           if (regUni.test(newContent)) {
               for (var i = 0; i < tel_RepCharacters.length; i++) {
                   var regex = new RegExp(tel_RepCharacters[i].character, "gi");
                   newContent = newContent.replace(regex, tel_RepCharacters[i].html);
               }
           }
  
         return newContent;    
       }  
    }    
    RadEditorCustomFilter.registerClass('RadEditorCustomFilter', Telerik.Web.UI.Editor.Filter);    
</script>   
    
    <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%">
        <table style="width: 100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 20%; vertical-align: top;">
                    <lftMenu:LeftMenu ID="LeftMenu" runat="server" CurrentMenu="FORMS" />
                </td>
                <td valign="top" style="width: 80%">
                    <table style="width: 100%" cellpadding="10" cellspacing="0">
                        <tr>
                            <td>
                                <table style="width: 100%" cellpadding="2" cellspacing="2" border="0">
                                    <tr>
                                        <td width="25%"><font style="font-style: normal; font-size: small; font-weight: bold">Form Name :</font></td>
                                        <td width="75%">
                                            <asp:Label ID="lblFormName" runat="server" Font-Bold="true" Font-Size="small"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td width="25%"><font style="font-style: normal; font-size: small; font-weight: bold">Form Description :</font></td>
                                        <td width="75%">
                                            <asp:Label ID="lblFormDescription" Font-Bold="true" Font-Size="small" runat="server"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">

                                <asp:Menu ID="MnuForm" Orientation="Horizontal" runat="server" CssClass="menuTabs"
                                    StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                                    OnMenuItemClick="MnuForm_MenuItemClick">
                                    <Items>
                                        <asp:MenuItem Text="Form Details" Value="0" Selected="true"></asp:MenuItem>
                                        <asp:MenuItem Text="Form Settings" Value="1"></asp:MenuItem>
                                        <asp:MenuItem Text="Response Emails" Value="2"></asp:MenuItem>
                                        <asp:MenuItem Text="Non Qualified" Value="3"></asp:MenuItem>
                                        <asp:MenuItem Text="ThankYou Page" Value="4"></asp:MenuItem>
                                    </Items>
                                </asp:Menu>
                                <div style="background-color: #eeeeee; border-color: Black; border-style: solid; border-width: 1px">
                                    <asp:MultiView ID="MultiViewForms" runat="server" ActiveViewIndex="0">
                                        <asp:View ID="View1" runat="server">
                                            <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                                <tr>
                                                    <td style="width: 30%; text-align: left">Form Name
                                                    </td>
                                                    <td style="width: 70%; text-align: left">
                                                        <asp:TextBox ID="txtFormName" runat="server" MaxLength="50"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="ReqFldVal1" runat="server" ControlToValidate="txtFormName"
                                                            ErrorMessage="*" Text="<img src='../images/required_field.jpg'>" Display="Static"
                                                            ValidationGroup="ValGroup1"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Description
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Show Print option
                                                    </td>
                                                    <td nowrap="nowrap">
                                                        <asp:RadioButtonList ID="rbshowprint" runat="server" RepeatDirection="Horizontal"
                                                            RepeatLayout="Flow">
                                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        &nbsp;&nbsp;&nbsp;&nbsp; <font size="2">[ Override Print as Digital :
                                                            <asp:RadioButtonList ID="rbsShowPrintasDigital" runat="server" RepeatDirection="Horizontal"
                                                                RepeatLayout="Flow">
                                                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            ]</font>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Show Digital option
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rbshowdigital" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Enable Print and Digital Option
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblstPrintDigital" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr id="paidSubscription" runat="server" visible="true">
                                                    <td>Paid Subscription?
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rbpaidsub" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="evtPaidChanged"
                                                            AutoPostBack="true">
                                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr id="paidCost" runat="server" visible="false">
                                                    <td>Purchase Cost in USD
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PaypalPaidCost" runat="server" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" colspan="2">
                                                        <b>SUBSCRIPTION Question :</b><br />
                                                        <telerik:RadEditor runat="server" ID="RadEditorSUBSCRIPTIONQuestion" Height="150px" OnClientLoad="OnClientLoad" Width="100%"></telerik:RadEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" colspan="2">
                                                        <b>PRINT/DIGITAL Question :</b><br />
                                                        <telerik:RadEditor runat="server" ID="RadEditorPRINTDIGITALQuestion" Height="150px" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td style="text-align: center" width="45%">
                                                                    <b>Countries</b>
                                                                </td>
                                                                <td width="5%"></td>
                                                                <td style="text-align: center" width="45%">
                                                                    <b>Selected Countries</b>
                                                                </td>
                                                                <td width="5%"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: center">
                                                                    <asp:ListBox ID="lstSource" runat="server" Width="100%" Height="150px" SelectionMode="Multiple"
                                                                        DataSourceID="SqlDataSourcePNonCountry" DataTextField="CountryName" DataValueField="CountryID"></asp:ListBox>
                                                                    <asp:SqlDataSource ID="SqlDataSourcePNonCountry" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                                        SelectCommand="sp_PublishersNonCountry" SelectCommandType="StoredProcedure">
                                                                        <SelectParameters>
                                                                            <asp:QueryStringParameter DefaultValue="0" Name="PubId" QueryStringField="PubId"
                                                                                Type="Int32" />
                                                                            <asp:QueryStringParameter DefaultValue="0" Name="FormId" QueryStringField="PFId"
                                                                                Type="Int32" />
                                                                            <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:Button ID="btnAdd" runat="server" Text=">>" Width="50px" OnClick="btnAdd_Click"
                                                                        CssClass="buttonSmall" /><br />
                                                                    <asp:Button ID="btnRemove" runat="server" Text="<<" Width="50px" OnClick="btnRemove_Click"
                                                                        CssClass="buttonSmall" />
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ListBox ID="lstDestination" runat="server" Width="100%" Height="150px" SelectionMode="Multiple"
                                                                        DataSourceID="SqlDataSourcePCountry" DataTextField="CountryName" DataValueField="CountryID"></asp:ListBox>
                                                                    <asp:SqlDataSource ID="SqlDataSourcePCountry" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                                        SelectCommand="sp_PublishersCountry" SelectCommandType="StoredProcedure">
                                                                        <SelectParameters>
                                                                            <asp:QueryStringParameter DefaultValue="0" Name="PubId" QueryStringField="PubId"
                                                                                Type="Int32" />
                                                                            <asp:QueryStringParameter DefaultValue="0" Name="FormId" QueryStringField="PFId"
                                                                                Type="Int32" />
                                                                            <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>
                                                                </td>
                                                                <td width="5%">&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td style="text-align: center" width="45%">
                                                                    <b>NewsLetters</b>
                                                                </td>
                                                                <td width="5%"></td>
                                                                <td style="text-align: center" width="45%">
                                                                    <b>Selected NewsLetters</b>
                                                                </td>
                                                                <td width="5%">&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: center">
                                                                    <asp:ListBox ID="lstSourceNewsLetter" runat="server" Width="100%" Height="150px"
                                                                        SelectionMode="Multiple" DataSourceID="SqlDataSourcePNonNewsLetter" DataTextField="Displayname"
                                                                        DataValueField="NewsletterID"></asp:ListBox>
                                                                    <asp:SqlDataSource ID="SqlDataSourcePNonNewsLetter" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                                        SelectCommand="sp_PublishersNonNewsLetter" SelectCommandType="StoredProcedure">
                                                                        <SelectParameters>
                                                                            <asp:QueryStringParameter DefaultValue="0" Name="PubId" QueryStringField="PubId"
                                                                                Type="Int32" />
                                                                            <asp:QueryStringParameter DefaultValue="0" Name="FormId" QueryStringField="PFId"
                                                                                Type="Int32" />
                                                                            <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:Button ID="btnNewsletterAdd" runat="server" Text=">>" Width="50px" OnClick="btnNewsletterAdd_Click"
                                                                        CssClass="buttonSmall" /><br />
                                                                    <asp:Button ID="btnNewsletterRemove" runat="server" Text="<<" Width="50px" OnClick="btnNewsletterRemove_Click"
                                                                        CssClass="buttonSmall" />
                                                                </td>
                                                                <td style="text-align: center">
                                                                    <asp:ListBox ID="lstDestinationNewsLetter" runat="server" Width="100%" Height="150px"
                                                                        SelectionMode="Multiple" DataSourceID="SqlDataSourcePNewsLetter" DataTextField="Displayname"
                                                                        DataValueField="NewsletterID"></asp:ListBox>
                                                                    <asp:SqlDataSource ID="SqlDataSourcePNewsLetter" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                                        SelectCommand="sp_PublishersFormNewsLetter" SelectCommandType="StoredProcedure">
                                                                        <SelectParameters>
                                                                            <asp:QueryStringParameter DefaultValue="0" Name="PubId" QueryStringField="PubId"
                                                                                Type="Int32" />
                                                                            <asp:QueryStringParameter DefaultValue="0" Name="FormId" QueryStringField="PFId"
                                                                                Type="Int32" />
                                                                            <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>
                                                                </td>
                                                                <td valign="middle" width="5%" align="middle">
                                                                    <asp:ImageButton ID="btnmoveup" ImageUrl="~/images/up-arrow.gif" runat="server" OnClick="btnmoveup_Click"
                                                                        BorderWidth="0px" />
                                                                    <br />
                                                                    <br />
                                                                    <asp:ImageButton ID="btnmovedown" ImageUrl="~/images/down-arrow.gif" runat="server"
                                                                        OnClick="btnmovedown_Click" BorderWidth="0px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left" colspan="4">Do you want to Pre-Select NewsLetters?
                                                                    <asp:RadioButtonList RepeatLayout="flow" ID="rbPreSelectNewsletters" runat="server"
                                                                        RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                                        <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left" colspan="4">Do you want to show Newsletters as Collapsible?
                                                                    <asp:RadioButtonList RepeatLayout="flow" ID="rbNewsletterCollapsible" runat="server"
                                                                        RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                                        <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left" colspan="4">Do you want to show Search bar for newsletters?
                                                                    <asp:RadioButtonList RepeatLayout="flow" ID="rbNewsletterSearch" runat="server" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                                        <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: left" colspan="4">Where do you want to show the Newsletters in the form?
                                                                    <asp:DropDownList ID="drpNewsletterPosition" runat="server" Width="250">
                                                                        <asp:ListItem Value="A" Text="Above Profile fields" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Value="B" Text="Below Profile/demographic fields"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span style="font-size: 14px; color: Red; font-weight: bold">Login Fields</span>
                                                        <br />
                                                        <div>
                                                            <table class="grid" cellspacing="0" cellpadding="5" border="0" style="width: 100%; border-collapse: collapse;">
                                                                <tr align="left">
                                                                    <th align="left" scope="col">Field Name
                                                                    </th>
                                                                    <th align="left" scope="col">Display Name
                                                                    </th>
                                                                    <th align="center" scope="col">Data Type
                                                                    </th>
                                                                    <th align="center" scope="col">Control
                                                                    </th>
                                                                    <th align="center" scope="col">Required
                                                                    </th>
                                                                    <th scope="col">&nbsp;
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 15%;">EmailAddress
                                                                    </td>
                                                                    <td align="left" style="width: 40%;">Email Address
                                                                    </td>
                                                                    <td align="center" style="width: 10%;">TEXT
                                                                    </td>
                                                                    <td align="center" style="width: 10%;">TextBox
                                                                    </td>
                                                                    <td align="center" style="width: 10%;">Y
                                                                    </td>
                                                                    <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <asp:Panel ID="pnlPassword" runat="server" Visible="true">
                                                                    <tr>
                                                                        <td align="left" style="width: 15%;">Password
                                                                        </td>
                                                                        <td align="left" style="width: 40%;">Password
                                                                        </td>
                                                                        <td align="center" style="width: 10%;">TEXT
                                                                        </td>
                                                                        <td align="center" style="width: 10%;">TextBox
                                                                        </td>
                                                                        <td align="center" style="width: 10%;">Y
                                                                        </td>
                                                                        <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 15%;">Confirm Password
                                                                        </td>
                                                                        <td align="left" style="width: 40%;">Confirm Password
                                                                        </td>
                                                                        <td align="center" style="width: 10%;">TEXT
                                                                        </td>
                                                                        <td align="center" style="width: 10%;">TextBox
                                                                        </td>
                                                                        <td align="center" style="width: 10%;">Y
                                                                        </td>
                                                                        <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </asp:Panel>
                                                            </table>
                                                            <br />
                                                            <span style="font-size: 14px; color: Red; font-weight: bold">Profile Field </span>
                                                            <br />
                                                            <asp:GridView ID="grdProfileFields" runat="server" AllowSorting="false" AllowPaging="false"
                                                                AutoGenerateColumns="false" Width="100%" DataKeyNames="PSFieldID" ShowFooter="false"
                                                                OnRowCommand="grdProfileFields_RowCommand" OnRowDataBound="grdProfileFields_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Sort" ItemStyle-Width="10%" ItemStyle-Wrap="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSortorder" Visible="false " runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.SortOrder") %>'></asp:Label>
                                                                            <asp:DropDownList ID="drpProfileFieldsSort" runat="server" Width="40" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="drpProfileFieldsSort_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:ImageButton ID="btnup" CommandName="up" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>'
                                                                                ImageAlign="AbsMiddle" runat="server" ImageUrl="~/images/up-arrow.gif" />
                                                                            <asp:ImageButton ID="btndown" CommandName="down" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>'
                                                                                ImageAlign="AbsMiddle" runat="server" ImageUrl="~/images/down-arrow.gif" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Field Name" ItemStyle-Width="15%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblECNFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ECNFieldName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="DisplayName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderText="Display Name" ItemStyle-Width="45%" HtmlEncode="false" />
                                                                    <asp:BoundField DataField="ControlType" HeaderText="Control" ReadOnly="true" ItemStyle-Width="10%"
                                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-Width="10%" HeaderText="Required" SortExpression="Required">
                                                                        <ItemTemplate>
                                                                            <%# DataBinder.Eval(Container, "DataItem.Required").ToString().ToUpper()=="Y"? "YES" : "NO"%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-Width="5%" HeaderText="Separator">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="drpSeparator" runat="server">
                                                                                <asp:ListItem Text="None" Value="N" />
                                                                                <asp:ListItem Text="Line" Value="L" />
                                                                                <asp:ListItem Text="Blank" Value="B" />
                                                                            </asp:DropDownList>
                                                                            <asp:HiddenField ID="hiddenSeparatorType" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.SeparatorType") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-Wrap="false">
                                                                        <ItemTemplate>
                                                                            <a href='NewPub_FieldForm.aspx?PubId=<%# DataBinder.Eval(Container,"DataItem.PubId") %>&PFID=<%#DataBinder.Eval(Container,"DataItem.PFID")%>&PSFieldID=<%#DataBinder.Eval(Container,"DataItem.PSFieldID")%>'
                                                                                <%# DataBinder.Eval(Container,"DataItem.ECNFieldName").ToString().ToUpper() != "EMAILADDRESS" ? "" : "style=display:none" %>
                                                                                onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:700, width:800, objectHeight:700, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                                                <img src="../Images/icon-edit.gif" alt='Add / Edit' style="border: 0px" /></a>&nbsp;
                                                                            <asp:ImageButton ID="imgbtnFormDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                                                CommandName="FormDelete" OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                                                CommandArgument='<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>' CausesValidation="false"
                                                                                Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container,"DataItem.ECNFieldName").ToString().ToUpper() != "EMAILADDRESS" ? "true" : "false")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <br />
                                                            <asp:Panel ID="pnlpaid" runat="server" Visible="false">
                                                                <span style="font-size: 14px; color: Red; font-weight: bold">Paid Publication Fields</span>
                                                                <br />
                                                                <div>
                                                                    <table class="grid" cellspacing="0" cellpadding="5" border="0" style="width: 100%; border-collapse: collapse;">
                                                                        <tr align="left">
                                                                            <th align="left" scope="col">Field Name
                                                                            </th>
                                                                            <th align="left" scope="col">Display Name
                                                                            </th>
                                                                            <th align="center" scope="col">Data Type
                                                                            </th>
                                                                            <th align="center" scope="col">Control
                                                                            </th>
                                                                            <th align="center" scope="col">Required
                                                                            </th>
                                                                            <th scope="col">&nbsp;
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PaypalFirstName
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">First Name
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TextBox
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PayPalLastName
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">Last Name
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TextBox
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PaypalStreet
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">Street
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TextBox
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PaypalStreet2
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">Street 2
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TextBox
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">N
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PayPalCity
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">City
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TextBox
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PayPalState
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">State/Province
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TextBox
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PayPalCountry
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">Country
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TextBox
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PayPalZip
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">Zipcode/Postal Code
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TextBox
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PaypalCardType
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">Card Type
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">DropDown
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PaypalAcct
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">Card Number
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TextBox
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PaypalExpMonth
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">Expiration Date
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">DropDown
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 15%;">PaypalExpYear
                                                                            </td>
                                                                            <td align="left" style="width: 40%;">Expiration Date
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">TEXT
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">DropDown
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">Y
                                                                            </td>
                                                                            <td align="center" style="width: 10%;">&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span style="font-size: 14px; color: Red; font-weight: bold">Demographic Field</span>
                                                        <br />
                                                        <asp:GridView ID="grdDemoGraphicFields" runat="server" AllowPaging="false" AllowSorting="false"
                                                            AutoGenerateColumns="false" Width="100%" DataKeyNames="PSFieldID" ShowFooter="false"
                                                            OnRowCommand="grdDemoGraphicFields_RowCommand" OnRowDataBound="grdDemoGraphicFields_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Sort" ItemStyle-Width="10%" ItemStyle-Wrap="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSortorder" Visible="false " runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.SortOrder") %>'></asp:Label>
                                                                        <asp:DropDownList ID="drpDemoGraphicFieldsSort" runat="server" Width="40" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="drpDemoGraphicFieldsSort_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:ImageButton ID="btnup" CommandName="up" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>'
                                                                            ImageAlign="AbsMiddle" runat="server" ImageUrl="~/images/up-arrow.gif" />
                                                                        <asp:ImageButton ID="btndown" CommandName="down" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>'
                                                                            ImageAlign="AbsMiddle" runat="server" ImageUrl="~/images/down-arrow.gif" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Field Name" ItemStyle-Width="15%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblECNFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ECNFieldName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DisplayName" HeaderText="Display Name" ItemStyle-Width="35%"
                                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HtmlEncode="false" />
                                                                <asp:BoundField DataField="ControlType" HeaderText="Control" ItemStyle-Width="10%"
                                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="10%" HeaderText="Required" SortExpression="Required">
                                                                    <ItemTemplate>
                                                                        <%# DataBinder.Eval(Container, "DataItem.Required").ToString().ToUpper()=="Y"? "YES" : "NO"%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="5%" HeaderText="Separator">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="drpSeparator" runat="server">
                                                                            <asp:ListItem Text="None" Value="N" />
                                                                            <asp:ListItem Text="Line" Value="L" />
                                                                            <asp:ListItem Text="Blank" Value="B" />
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hiddenSeparatorType" runat="server" Value='<%# DataBinder.Eval(Container, "DataItem.SeparatorType") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" ItemStyle-VerticalAlign="Middle"
                                                                    ItemStyle-Wrap="false">
                                                                    <ItemTemplate>
                                                                        <a href='NewPub_FieldForm.aspx?PubId=<%# DataBinder.Eval(Container,"DataItem.PubId") %>&PFID=<%#DataBinder.Eval(Container,"DataItem.PFID")%>&PSFieldID=<%#DataBinder.Eval(Container,"DataItem.PSFieldID")%>'
                                                                            onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:750, width:800, objectHeight:700, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                                            <img src="../Images/icon-edit.gif" alt='Add / Edit' style="border: 0px" /></a>&nbsp;
                                                                        <asp:ImageButton ID="imgbtnFormDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                                            CommandName="FormDelete" OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                                            CommandArgument='<%# DataBinder.Eval(Container,"DataItem.PSFieldID") %>' CausesValidation="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">Select Fields&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:DropDownList ID="drpFields" runat="server" Width="150px" AutoPostBack="false"
                                                            OnSelectedIndexChanged="drpFields_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="../images/icon-add.gif"
                                                            OnClick="imgbtnAdd_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <a id="lnkpopup" runat="server" onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:700, width:800, objectHeight:700, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">ADD NEW FIELDS</a>
                                                    </td>
                                                </tr>
                                                <asp:Panel ID="pnlEditInactiveFields" runat="server" Visible="false">
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblInActiveMsg" runat="server" ForeColor="Red" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Edit Inactive Field:&nbsp; <a id="lnkEditInactiveFields" runat="server" onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:700, width:800, objectHeight:700, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                            <img src="../Images/icon-edit.gif" alt='Add / Edit' style="border: 0px" /></a>
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="View2" runat="server">
                                            <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="grdRules" runat="server" AllowSorting="false" AllowPaging="false"
                                                            AutoGenerateColumns="false" Width="100%" DataKeyNames="PSFieldID" ShowFooter="false">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Field Name" ItemStyle-Width="15%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblECNFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ECNFieldName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="DisplayName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                    HeaderText="Display Name" ItemStyle-Width="45%" HtmlEncode="false" />
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-Wrap="false">
                                                                    <ItemTemplate>
                                                                        <a href='FieldSettings.aspx?PubId=<%# DataBinder.Eval(Container,"DataItem.PubId") %>&PFFieldID=<%# DataBinder.Eval(Container,"DataItem.PFFieldID") %>&PFID=<%# DataBinder.Eval(Container,"DataItem.PFID") %>'
                                                                            onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:450, width:600, objectHeight:400, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                                            <img src='<%# Eval("IsSelected").ToString().ToUpper()=="Y"?"../Images/icon-settings-edit.png":"../Images/icon-settings.png" %>'
                                                                                alt='Add / Edit' style="border: 0px" /></a>&nbsp;
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="View3" runat="server">
                                            <table cellpadding="5" cellspacing="5">
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <br />
                                                        <asp:Menu ID="MnuResponseType" Orientation="Horizontal" runat="server" CssClass="menuTabs"
                                                            StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                                                            OnMenuItemClick="MnuResponseType_MenuItemClick">
                                                            <Items>
                                                                <asp:MenuItem Text="Print" Value="0" Selected="true"></asp:MenuItem>
                                                                <asp:MenuItem Text="Digital" Value="1"></asp:MenuItem>
                                                                <asp:MenuItem Text="Both" Value="2"></asp:MenuItem>
                                                                <asp:MenuItem Text="Non Qualified" Value="3"></asp:MenuItem>
                                                                <asp:MenuItem Text="Cancel" Value="4"></asp:MenuItem>
                                                                <asp:MenuItem Text="Other" Value="5"></asp:MenuItem>
                                                                <asp:MenuItem Text="Newsletter" Value="6"></asp:MenuItem>
                                                            </Items>
                                                        </asp:Menu>
                                                        <div runat="server" style="background-color: #eeeeee; border-color: Black; border-style: solid; border-width: 1px">
                                                            <asp:MultiView ID="multiViewResponseType" runat="server" ActiveViewIndex="0">
                                                                <asp:View ID="viewpnlPrintResponseEmail" runat="server">
                                                                    <%-- <asp:Panel ID="pnlPrintResponseEmail" runat="server" Visible="false"> --%>
                                                                    <asp:Panel ID="pnlFromPrint" runat="server">
                                                                        <table width="90%" cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td colspan="2" nowrap="nowrap">
                                                                                    <table width="100%" cellpadding="5">
                                                                                        <tr>
                                                                                            <td style="width: 15%; text-align: left" valign="top">From Name:
                                                                                            </td>
                                                                                            <td style="width: 85%; text-align: left" valign="top">
                                                                                                <asp:TextBox ID="FromNamePrint" runat="server" MaxLength="50" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FromNamePrint"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top">From Email:
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                                <asp:TextBox ID="txtFormEmailPrint" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFormEmailPrint"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFormEmailPrint"
                                                                                                    SetFocusOnError="true" ErrorMessage="Invalid Format" Font-Bold="false" ValidationGroup="ValGroup1"
                                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <table width="90%" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do You want to send Notification Email to the user for subscribing to the magazine?</b>
                                                                                <asp:RadioButtonList ID="rbUserNotificationPrint" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="rbUserNotificationPrint_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlUserNotificationPrint" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">User Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td valign="top" width="20%">User Email Subject:
                                                                                                    </td>
                                                                                                    <td valign="top" width="80%">
                                                                                                        <asp:TextBox ID="txtUserEmailSubPrint" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left">User Email Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorUserEmailBodyPrint"  OnClientLoad="OnClientLoad" Width="100%"></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do You want to send Notification Email to the user for Non Qualified Response?</b>
                                                                                <asp:RadioButtonList ID="rbUserNotificationNQResponsePrint" runat="server" Width="200px"
                                                                                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUserNotificationNQResponsePrint_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlNonQualResponse" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">User Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">User Email Subject:
                                                                                                    </td>
                                                                                                    <td width="80%">
                                                                                                        <asp:TextBox ID="txtUserEmailSubNQResponsePrint" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left" valign="top">User Email Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorEmailBodyNQResponsePrint" OnClientLoad="OnClientLoad"  Width="100%"></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do you want to send Notification Email to the Administrator?</b>
                                                                                <asp:RadioButtonList ID="rbAdminNotificationPrint" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="rbAdminNotificationPrint_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlAdminNotificationPrint" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">Administrator Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td width="20%">Administrator Email:
                                                                                                    </td>
                                                                                                    <td width="80%">
                                                                                                        <asp:TextBox ID="txtAdminEmailPrint" runat="server" Width="400" MaxLength="100"></asp:TextBox>
                                                                                                        <asp:CustomValidator ID="val_MultipleEmails" runat="server" ErrorMessage="Multiple Email Address should be sepatated by comma(,)!"
                                                                                                            ControlToValidate="txtAdminEmailPrint"></asp:CustomValidator>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td width="20%">Subject:
                                                                                                    </td>
                                                                                                    <td width="20%">
                                                                                                        <asp:TextBox ID="txtAdminEmailSubPrint" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left">Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorAdminEmailBodyPrint" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <%--   </asp:Panel>--%>
                                                                </asp:View>
                                                                <asp:View ID="viewpnlDigitalResponseEmail" runat="server">
                                                                    <%--    <asp:Panel ID="pnlDigitalResponseEmail" runat="server" Visible="false"> --%>
                                                                    <asp:Panel ID="pnlFromDigital" runat="server" Visible="false">
                                                                        <table width="90%" cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td colspan="2" nowrap="nowrap">
                                                                                    <table width="100%" cellpadding="2">
                                                                                        <tr>
                                                                                            <td style="width: 20%; text-align: left" valign="top">From Name:
                                                                                            </td>
                                                                                            <td style="width: 80%; text-align: left" valign="top">
                                                                                                <asp:TextBox ID="FromNameDigital" runat="server" MaxLength="50" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="FromNameDigital"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top">From Email:
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                                <asp:TextBox ID="txtFormEmailDigital" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFormEmailDigital"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtFormEmailDigital"
                                                                                                    SetFocusOnError="true" ErrorMessage="Invalid Format" Font-Bold="false" ValidationGroup="ValGroup1"
                                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <table width="90%" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do You want to send Notification Email to the user for subscribing to the magazine?</b>
                                                                                <asp:RadioButtonList ID="rbUserNotificationDigital" runat="server" Width="200px"
                                                                                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUserNotificationDigital_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlUserNotificationDigital" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">User Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td width="15%">User Email Subject:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtUserEmailSubDigital" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left" width="85%">User Email Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorUserEmailBodyDigital" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do you want to send Notification Email to the Administrator?</b>
                                                                                <asp:RadioButtonList ID="rbAdminNotificationDigital" runat="server" Width="200px"
                                                                                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbAdminNotificationDigital_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlAdminNotificationDigital" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">Administrator Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td width="20%">Administrator Email:
                                                                                                    </td>
                                                                                                    <td width="80%">
                                                                                                        <asp:TextBox ID="txtAdminEmailDigital" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td width="15%">Subject:
                                                                                                    </td>
                                                                                                    <td width="85%">
                                                                                                        <asp:TextBox ID="txtAdminEmailSubDigital" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left">Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorAdminEmailBodyDigital" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:View>
                                                                <asp:View ID="viewpnlBothResponseEmail" runat="server">
                                                                    <asp:Panel ID="pnlFromBoth" runat="server" Visible="false">
                                                                        <table width="90%" cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td colspan="2" nowrap="nowrap">
                                                                                    <table width="100%" cellpadding="5">
                                                                                        <tr>
                                                                                            <td style="width: 20%; text-align: left" valign="top">From Name:
                                                                                            </td>
                                                                                            <td style="width: 80%; text-align: left">
                                                                                                <asp:TextBox ID="FromNameBoth" runat="server" MaxLength="50" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="FromNameBoth"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" align="left">From Email:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFormEmailBoth" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtFormEmailBoth"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtFormEmailBoth"
                                                                                                    SetFocusOnError="true" ErrorMessage="Invalid Format" Font-Bold="false" ValidationGroup="ValGroup1"
                                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <table width="90%" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do You want to send Notification Email to the user for subscribing to the magazine?</b>
                                                                                <asp:RadioButtonList ID="rbUserNotificationBoth" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="rbUserNotificationBoth_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlUserNotificationBoth" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">User Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">User Email Subject:
                                                                                                    </td>
                                                                                                    <td width="80%">
                                                                                                        <asp:TextBox ID="txtUserEmailSubBoth" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left" valign="top">User Email Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorUserEmailBodyBoth" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do you want to send Notification Email to the Administrator?</b>
                                                                                <asp:RadioButtonList ID="rbAdminNotificationBoth" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="rbAdminNotificationBoth_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlAdminNotificationBoth" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">Administrator Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">Administrator Email:
                                                                                                    </td>
                                                                                                    <td valign="top" align="left" width="80%">
                                                                                                        <asp:TextBox ID="txtAdminEmailBoth" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">Subject:
                                                                                                    </td>
                                                                                                    <td valign="top" align="left" width="80%">
                                                                                                        <asp:TextBox ID="txtAdminEmailSubBoth" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left">Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorAdminEmailBodyBoth" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:View>
                                                                <asp:View ID="viewpnlNQResponseEmail" runat="server">
                                                                    <asp:Panel ID="pnlFromEmailNQ" runat="server">
                                                                        <table width="90%" cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td colspan="2" nowrap="nowrap">
                                                                                    <table width="100%" cellpadding="5">
                                                                                        <tr>
                                                                                            <td style="width: 20%; text-align: left" valign="top">From Name:
                                                                                            </td>
                                                                                            <td style="width: 80%; text-align: left">
                                                                                                <asp:TextBox ID="FromNameNQ" runat="server" MaxLength="50" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FromNameNQ"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" align="left">From Email:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFormEmailNQ" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtFormEmailNQ"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtFormEmailNQ"
                                                                                                    SetFocusOnError="true" ErrorMessage="Invalid Format" Font-Bold="false" ValidationGroup="ValGroup1"
                                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <table width="90%" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do You want to send Notification Email to the user for Non Qualified Country Selection?</b>
                                                                                <asp:RadioButtonList ID="rbUserNotificationNQCountry" runat="server" Width="200px"
                                                                                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbUserNotificationNQCountry_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow" Style="height: 21px">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlNQResponseCountry" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">User Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">User Email Subject:
                                                                                                    </td>
                                                                                                    <td width="80%">
                                                                                                        <asp:TextBox ID="txtUserNotificationSubNQCountry" runat="server" MaxLength="500"
                                                                                                            Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left" valign="top">User Email Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorNotificationSubNQCountry"  OnClientLoad="OnClientLoad" Width="100%" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <%--</asp:Panel>--%>
                                                                </asp:View>
                                                                <asp:View ID="viewpnlCancelResponseEmail" runat="server">
                                                                    <%--<asp:Panel ID="pnlCancelResponseEmail" runat="server" Visible="false"> --%>
                                                                    <asp:Panel ID="pnlFromCancel" runat="server" Visible="false">
                                                                        <table width="90%" cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td colspan="2" nowrap="nowrap">
                                                                                    <table width="100%" cellpadding="5">
                                                                                        <tr>
                                                                                            <td style="width: 20%; text-align: left" valign="top">From Name:
                                                                                            </td>
                                                                                            <td style="width: 80%; text-align: left">
                                                                                                <asp:TextBox ID="FromNameCancel" runat="server" MaxLength="50" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="FromNameCancel"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" align="left">From Email:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFormEmailCancel" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtFormEmailCancel"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtFormEmailCancel"
                                                                                                    SetFocusOnError="true" ErrorMessage="Invalid Format" Font-Bold="false" ValidationGroup="ValGroup1"
                                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <table width="90%" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do You want to send Notification Email to the user for cancelling the magazine?</b>
                                                                                <asp:RadioButtonList ID="rbUserNotificationCancel" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="rbUserNotificationCancel_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlUserNotificationCancel" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">User Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">User Email Subject:
                                                                                                    </td>
                                                                                                    <td width="80%">
                                                                                                        <asp:TextBox ID="txtUserEmailSubCancel" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left" valign="top">User Email Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorUserEmailBodyCancel" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do you want to send Notification Email to the Administrator?</b>
                                                                                <asp:RadioButtonList ID="rbAdminNotificationCancel" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="rbAdminNotificationCancel_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlAdminNotificationCancel" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">Administrator Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">Administrator Email:
                                                                                                    </td>
                                                                                                    <td valign="top" align="left" width="80%">
                                                                                                        <asp:TextBox ID="txtAdminEmailCancel" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">Subject:
                                                                                                    </td>
                                                                                                    <td valign="top" align="left" width="80%">
                                                                                                        <asp:TextBox ID="txtAdminEmailSubCancel" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left">Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorAdminEmailBodyCancel" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <%--   </asp:Panel>--%>
                                                                </asp:View>
                                                                <asp:View ID="viewpnlOtherResponseEmail" runat="server">
                                                                    <%--<asp:Panel ID="pnlOtherResponseEmail" runat="server" Visible="false"> --%>
                                                                    <asp:Panel ID="pnlFromOther" runat="server" Visible="false">
                                                                        <table width="90%" cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td colspan="2" nowrap="nowrap">
                                                                                    <table width="100%" cellpadding="5">
                                                                                        <tr>
                                                                                            <td style="width: 20%; text-align: left" valign="top">From Name:
                                                                                            </td>
                                                                                            <td style="width: 80%; text-align: left">
                                                                                                <asp:TextBox ID="FromNameOther" runat="server" MaxLength="50" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="FromNameOther"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" align="left">From Email:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFormEmailOther" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtFormEmailOther"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtFormEmailOther"
                                                                                                    SetFocusOnError="true" ErrorMessage="Invalid Format" Font-Bold="false" ValidationGroup="ValGroup1"
                                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <table width="90%" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do You want to send Notification Email to the user for subscribing to the magazine?</b>
                                                                                <asp:RadioButtonList ID="rbUserNotificationOther" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="rbUserNotificationOther_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlUserNotificationOther" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">User Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">User Email Subject:
                                                                                                    </td>
                                                                                                    <td width="80%">
                                                                                                        <asp:TextBox ID="txtUserEmailSubOther" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left" valign="top">User Email Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorUserEmailBodyOther" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do you want to send Notification Email to the Administrator?</b>
                                                                                <asp:RadioButtonList ID="rbAdminNotificationOther" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="rbAdminNotificationOther_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlAdminNotificationOther" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">Administrator Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">Administrator Email:
                                                                                                    </td>
                                                                                                    <td valign="top" align="left" width="80%">
                                                                                                        <asp:TextBox ID="txtAdminEmailOther" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td valign="top" align="left" width="20%">Subject:
                                                                                                    </td>
                                                                                                    <td valign="top" align="left" width="80%">
                                                                                                        <asp:TextBox ID="txtAdminEmailSubOther" runat="server" MaxLength="500" Width="540"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left">Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorAdminEmailBodyOther" Width="100%"  OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:View>
                                                                <asp:View ID="viewpnlNewsletterResponseEmail" runat="server">
                                                                    <asp:Panel ID="pnlFromNewsletter" runat="server" Visible="false">
                                                                        <table width="90%" cellpadding="5" cellspacing="5">
                                                                            <tr>
                                                                                <td colspan="2" nowrap="nowrap">
                                                                                    <table width="100%" cellpadding="5">
                                                                                        <tr>
                                                                                            <td style="width: 20%; text-align: left" valign="top">From Name:
                                                                                            </td>
                                                                                            <td style="width: 80%; text-align: left">
                                                                                                <asp:TextBox ID="FromNameNewsletter" runat="server" MaxLength="50" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="FromNameNewsletter"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td valign="top" align="left">From Email:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFormEmailNewsletter" runat="server" MaxLength="100" Width="400"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtFormEmailNewsletter"
                                                                                                    ErrorMessage="*" Display="Static" ValidationGroup="valRspEmails" Text="<img src='../images/required_field.jpg'>"></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtFormEmailNewsletter"
                                                                                                    SetFocusOnError="true" ErrorMessage="Invalid Format" Font-Bold="false" ValidationGroup="ValGroup1"
                                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <table width="90%" cellpadding="5" cellspacing="5">
                                                                        <tr>
                                                                            <td colspan="2" nowrap="nowrap">
                                                                                <b>Do you want to send Notification Email to the user for subscribing to the Newsletter?</b>
                                                                                <asp:RadioButtonList ID="rbNewletterNotification" runat="server" Width="200px"
                                                                                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbNewletterNotification_SelectedIndexChanged"
                                                                                    RepeatLayout="Flow">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False"></asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="false" Selected="True"></asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                                <br />
                                                                                <asp:Panel ID="pnlNewsLetterNotification" runat="server" Visible="false">
                                                                                    <fieldset>
                                                                                        <legend style="font-size: small; color: #32537E">Newsletter Notification</legend>
                                                                                        <div>
                                                                                            <table width="100%" cellpadding="5">
                                                                                                <tr>
                                                                                                    <td width="10%">Subject:
                                                                                                    </td>
                                                                                                    <td width="90%">
                                                                                                        <asp:TextBox ID="txtNewsLetterSub" runat="server" MaxLength="500" Width="570"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2" style="text-align: left">Body:
                                                                                                        <br />
                                                                                                        <telerik:RadEditor runat="server" ID="RadEditorNewsLetterBody" Width="100%" OnClientLoad="OnClientLoad" ></telerik:RadEditor>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:View>
                                                            </asp:MultiView>
                                                        </div>
                                                        <%--</asp:Panel>       --%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="View4" runat="server">
                                            <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <b>Do you want to setup for non qualification page?</b>
                                                        <asp:RadioButtonList ID="rbNonQualSetup" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                            AutoPostBack="true" OnSelectedIndexChanged="rbNonQualSetup_SelectedIndexChanged"
                                                            RepeatLayout="Flow" CellSpacing="2" CellPadding="2">
                                                            <asp:ListItem Text="Yes" Value="true" Selected="False" />
                                                            <asp:ListItem Text="No" Value="false" Selected="False" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel Visible="false" ID="pnlNonQual" runat="server">
                                                            <fieldset>
                                                                <div>
                                                                    <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                                                        <tr>
                                                                            <td colspan="2">Override Print as Digital:  
                                                                                <asp:RadioButtonList ID="rbNQOverridePrintAsDigital" runat="server" Width="200px"
                                                                                    RepeatDirection="Horizontal" RepeatLayout="Flow" CellSpacing="2"
                                                                                    CellPadding="2">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False" />
                                                                                    <asp:ListItem Text="No" Value="false" Selected="True" />
                                                                                </asp:RadioButtonList><br />
                                                                                <br />

                                                                                <font style="font-size: xx-small; color: red">if YES, Change PRINT to Digital and redirect to Paid Page or NQ Landing Page<br />
                                                                                    if NO, Redirect to Paid Page or NQ Landing Page<br />
                                                                                    <br />
                                                                                </font>

                                                                                URL of Non Qualified Page: (Media = Print)&nbsp;<br />
                                                                                <asp:TextBox ID="txtNQPrintRedirectUrl" Width="500px" runat="server" Rows="5" TextMode="MultiLine"
                                                                                    MaxLength="1000" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">Disable for Digital Option: 
                                                                                <asp:RadioButtonList ID="rblDisableNonQualSetup" runat="server" Width="200px" RepeatDirection="Horizontal"
                                                                                    RepeatLayout="Flow" CellSpacing="2" CellPadding="2" AutoPostBack="true" OnSelectedIndexChanged="rblDisableNonQualSetup_SelectedIndexChanged">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False" />
                                                                                    <asp:ListItem Text="No" Value="false" Selected="True" />
                                                                                </asp:RadioButtonList><br />
                                                                                <br />

                                                                                <font style="font-size: xx-small; color: red">
                                                                                    if YES, NQ rules are ignored<br />
                                                                                    if NO, Redirect to Paid Page or NQ Landing Page<br />
                                                                                    <br />
                                                                                </font>

                                                                                <asp:Panel ID="pnlNQDigitalRedirectUrl" runat="server">
                                                                                    URL of Non Qualified Page: (Media = Digital)&nbsp;<br />
                                                                                    <asp:TextBox ID="txtNQDigitalRedirectUrl" Width="500px" runat="server" Rows="5" TextMode="MultiLine"
                                                                                        MaxLength="1000" />
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2">Suspend ECN Post for Both:
                                                                                <asp:RadioButtonList ID="rbSuspendECNPostforBoth" runat="server" Width="200px"
                                                                                    RepeatDirection="Horizontal" RepeatLayout="Flow" CellSpacing="2"
                                                                                    CellPadding="2">
                                                                                    <asp:ListItem Text="Yes" Value="true" Selected="False" />
                                                                                    <asp:ListItem Text="No" Value="false" Selected="True" />
                                                                                </asp:RadioButtonList><br />
                                                                                <br />

                                                                                <font style="font-size: xx-small; color: red">if Suspend = NO, Change BOTH to Digital and redirect to Paid Page or NQ Landing Page)<br />
                                                                                    if Suspend = YES, Redirect to paid page without posting data to ECN<br />
                                                                                    <br />
                                                                                </font>

                                                                                URL of Non Qualified Page: (Media = Both)&nbsp;<br />
                                                                                <asp:TextBox ID="txtNQBothRedirectUrl" Width="500px" runat="server" Rows="5" TextMode="MultiLine"
                                                                                    MaxLength="1000" />
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="3" align="left">URL of Paid Page: (Link displayed in NQ Landing page) &nbsp;<br />
                                                                                <font style="font-size: xx-small; color: red">
                                                                                    Use %%paidlink%% codesnippet in Non Qualified Response or Non Qualified Country HTML to display link to below paid page URL.  
                                                                                    <br />
                                                                                </font>
                                                                                <asp:TextBox ID="txtPaidPageLink" Width="500px" runat="server" Rows="5" TextMode="MultiLine"
                                                                                    MaxLength="1000" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="45%" align="center">
                                                                                <b>Countries</b>
                                                                            </td>
                                                                            <td width="10%">&nbsp;
                                                                            </td>
                                                                            <td width="45%" align="center">
                                                                                <b>Selected Countries</b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: center" width="45%">
                                                                                <asp:ListBox ID="lstSourceNonQual" runat="server" Width="100%" Height="150px" SelectionMode="Multiple"
                                                                                    DataSourceID="SqlDataSourcePNonCountry" DataTextField="CountryName" DataValueField="CountryID"></asp:ListBox>
                                                                            </td>
                                                                            <td style="text-align: center" width="10%">
                                                                                <asp:Button ID="btnAddNonQual" runat="server" Text=">>" Width="50px" OnClick="btnAddNonQual_Click"
                                                                                    CssClass="buttonSmall" /><br />
                                                                                <asp:Button ID="btnRemoveNonQual" runat="server" Text="<<" Width="50px" OnClick="btnRemoveNonQual_Click"
                                                                                    CssClass="buttonSmall" />
                                                                            </td>
                                                                            <td style="text-align: center" width="45%">
                                                                                <asp:ListBox ID="lstDestinationNonQual" runat="server" Width="100%" Height="150px"
                                                                                    SelectionMode="Multiple" DataSourceID="SqlDataSourcePCountryNonQual" DataTextField="CountryName"
                                                                                    DataValueField="CountryID"></asp:ListBox>
                                                                                <asp:SqlDataSource ID="SqlDataSourcePCountryNonQual" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                                                                    SelectCommand="sp_PublishersCountryNonQual" SelectCommandType="StoredProcedure">
                                                                                    <SelectParameters>
                                                                                        <asp:QueryStringParameter DefaultValue="0" Name="PubId" QueryStringField="PubId"
                                                                                            Type="Int32" />
                                                                                        <asp:QueryStringParameter DefaultValue="0" Name="FormId" QueryStringField="PFId"
                                                                                            Type="Int32" />
                                                                                        <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                                                                    </SelectParameters>
                                                                                </asp:SqlDataSource>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3">
                                                                                <b>Other Form Fields</b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" colspan="3">
                                                                                <asp:GridView ID="grdNonQual" runat="server" AllowSorting="false" AllowPaging="false"
                                                                                    AutoGenerateColumns="false" Width="100%" DataKeyNames="PSFieldID" ShowFooter="false">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                                            HeaderText="Field Name" ItemStyle-Width="15%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblECNFieldNameNonQual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.ECNFieldName") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField DataField="DisplayName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                                            HeaderText="Display Name" ItemStyle-Width="45%" HtmlEncode="false" />
                                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" ItemStyle-Wrap="false">
                                                                                            <ItemTemplate>
                                                                                                <a href='NonQualFieldSettings.aspx?PubId=<%# DataBinder.Eval(Container,"DataItem.PubId") %>&PFFieldID=<%# DataBinder.Eval(Container,"DataItem.PFFieldID") %>&PFID=<%# DataBinder.Eval(Container,"DataItem.PFID") %>'
                                                                                                    onclick="return hs.htmlExpand(this, {objectType: 'iframe', height:450, width:600, objectHeight:400, outlineType: 'rounded-white', dimmingOpacity: 0.5, preserveContent : false } )">
                                                                                                    <img src='<%# Eval("IsSelected").ToString().ToUpper()=="Y"?"../Images/icon-settings-edit.png":"../Images/icon-settings.png" %>'
                                                                                                        alt='Add / Edit' style="border: 0px" /></a>&nbsp;
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </fieldset>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="View5" runat="server">
                                            <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <b>PRINT:</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadEditor runat="server" ID="RadEditorPrintThankYou" AllowScripts="true" OnClientLoad="OnClientLoad" Width="100%" ></telerik:RadEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>DIGITAL:</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadEditor runat="server" ID="RadEditorDigitalThankYou" AllowScripts="true" OnClientLoad="OnClientLoad" Width="100%" ></telerik:RadEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>No PRINT/DIGITAL:</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadEditor runat="server" ID="RadEditorDefaultThankYou" AllowScripts="true" OnClientLoad="OnClientLoad" Width="100%" ></telerik:RadEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Non Qualified Response:</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadEditor runat="server" ID="RadEditorNQResponseThankYou" AllowScripts="true" OnClientLoad="OnClientLoad" Width="100%" ></telerik:RadEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Non Qualified Country:</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadEditor runat="server" ID="RadEditorNQCountryThankyou" AllowScripts="true" OnClientLoad="OnClientLoad" Width="100%" ></telerik:RadEditor>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                    </asp:MultiView>
                                    <div style="text-align: right; padding: 10px 10px 10px 10px">
                                        <asp:Button ID="btnPrevious" Text="Previous" runat="server" OnClick="btnPrevious_Click"
                                            CssClass="button" />
                                        <asp:Button ID="btnNext" Text="Next" runat="server" OnClick="btnNext_Click" CssClass="button" />
                                        <asp:Button ID="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click"
                                            runat="server" CssClass="button" OnClientClick="return confirm('Are you sure you want to cancel?');" />
                                        <asp:Button ID="btnFinish" Text="Save" runat="server" OnClick="btnFinish_Click" CssClass="button" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </JF:BoxPanel>
    <asp:Button ID="btnReload" runat="server" Text="reload" OnClick="btnReload_Click" />
</asp:Content>
