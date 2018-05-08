<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="NewPub_FieldForm.aspx.cs"
    Inherits="KMPS_JF_Setup.Publisher.NewPub_FieldForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
        .style2
        {
            height: 30px;
        }
    </style>
</head>
<body style="background-color: White;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="script1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td style="text-align: center">
                        <table width="100%" border="0" cellpadding="2" cellspacing="0">
                            <tr>
                                <td colspan="2" align="right" style="padding-right: 10px;" class="style2">
                                    <asp:Button CssClass="button" ID="btnAddTop" runat="server" Text=" Save " OnClick="btnAdd_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="style1">
                                    <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; text-align: left">
                                    ECN Field Name
                                </td>
                                <td style="width: 85px; text-align: left">
                                    <asp:DropDownList ID="ddlECNField" runat="server" Width="200px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%; text-align: left">
                                    ECN Combined Field Name
                                </td>
                                <td style="width: 85px; text-align: left">
                                    <asp:DropDownList ID="ddlECNCombinedField" runat="server" Width="200px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    Display Name
                                </td>
                                <td style="text-align: left">
                                     <telerik:RadEditor runat="server" ID="RadEditorDisplayName" Height="300px" Width="100%"></telerik:RadEditor>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    Is Active
                                </td>
                                <td style="text-align: left">
                                    <asp:RadioButtonList ID="rbtlstIsActive" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    Required
                                </td>
                                <td style="text-align: left">
                                    <asp:RadioButtonList ID="rbtlstRequired" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    Field Group
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlGrouping" runat="server" Width="200px">
                                        <asp:ListItem Text="Profile Field" Value="P"></asp:ListItem>
                                        <asp:ListItem Text="Demographic Field" Value="D"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="Req1" runat="server" ErrorMessage="*" Font-Bold="false"
                                        ControlToValidate="ddlGrouping" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    Control Type
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlControlType" runat="server" Width="200px" OnSelectedIndexChanged="ddlControlType_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Text="TextBox" Value="TextBox"></asp:ListItem>
                                        <asp:ListItem Text="Categorized Checkbox" Value="CatCheckbox"></asp:ListItem>
                                        <asp:ListItem Text="Checkbox" Value="Checkbox"></asp:ListItem>
                                        <asp:ListItem Text="Dropdown" Value="Dropdown"></asp:ListItem>
                                        <asp:ListItem Text="Radio" Value="Radio"></asp:ListItem>
                                        <asp:ListItem Text="Hidden" Value="Hidden"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <Panel ID="pnlMaxSelections" runat="server" Visible="false">
                             <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="lblMaxSelections" runat="server" Text="MaxSelections"></asp:Label>
                                </td>
                                <td style="text-align: left">
                                     <asp:TextBox ID="txtMaxSelections" runat="server" Width="200" Text="1"/>
                                </td>
                            </tr>
                            </Panel>
                            <asp:Panel ID="pnlDefaultValue" runat="server" Visible="false">
                                <tr>
                                    <td style="text-align: left">
                                        Default Value
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtDefaultValue" runat="server" Width="200" />
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlText" runat="server" Visible="false">
                                <tr>
                                    <td align="left">
                                        Data Type
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlDataType" runat="server" Width="200px">
                                            <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                            <asp:ListItem Text="Number" Value="Number"></asp:ListItem>
                                            <asp:ListItem Text="Currency" Value="Currency"></asp:ListItem>
                                            <asp:ListItem Text="Date" Value="Date"></asp:ListItem>
                                            <asp:ListItem Text="Zip" Value="Zip"></asp:ListItem>
                                            <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        Max Length
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtMaxLength" runat="server" MaxLength="10"></asp:TextBox>
                                        <asp:RangeValidator ID="RangeValidator1" ControlToValidate="txtMaxLength" runat="server" Type="Integer"
                                            MinimumValue="0" MaximumValue="250" ErrorMessage="Max length should not be greater than 250"></asp:RangeValidator>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlPrePopulate" runat="server">
                                <tr>
                                    <td align="left">
                                        Pre populate
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rblstprepopulate" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Yes" Value="1" Selected="True" />
                                            <asp:ListItem Text="No" Value="0" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlOther" runat="server" Visible="false">
                                <tr>
                                    <td align="left" valign="top">
                                        Control Value
                                    </td>
                                    <td align="left">
                                        <div id="Grid" runat="server" style="width: 100%;" class="grid">
                                            <table border="0" cellpadding="2" width="100%" cellspacing="2" style="border: 0px solid;">
                                                <tr class="gridheader">
                                                    <th style="padding-left: 5px;">
                                                        DataValue
                                                    </th>
                                                    <th style="padding-left: 5px;">
                                                        DataText
                                                    </th>
                                                    <th style="padding-left: 5px;">
                                                        Category
                                                    </th>
                                                    <th style="padding-left: 5px;">
                                                        Default
                                                    </th>
                                                    <th>
                                                        &nbsp;
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtControlFieldValue" runat="server" Width="180px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtControlFieldName" runat="server" Width="180px" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCategoryName" runat="server" Width="180px" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbDefaultNew" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="btnNew" ImageUrl="~/Images/icon-add.gif" runat="server" OnClick="btnNew_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblDataValueErr" runat="server" Visible="false" ForeColor="Red" Font-Size="10pt"
                                            Font-Bold="true" />
                                        <br />
                                        <asp:GridView ID="grdControlValue" SkinID="skicontrolitems" runat="server" ShowFooter="true"
                                            AllowSorting="false" AutoGenerateColumns="false" OnRowCommand="grdControlValue_RowCommand"
                                            OnRowEditing="grdControlValue_RowEditing" OnRowCancelingEdit="grdControlValue_RowCancelingEdit"
                                            OnRowUpdating="grdControlValue_RowUpdating" OnRowDeleting="grdControlValue_RowDeleting"
                                            OnRowDataBound="grdControlValue_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sort">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSortOrder" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SortOrder") %>' />
                                                        <asp:DropDownList ID="drpItemOrder" OnSelectedIndexChanged="drpItemOrder_selectedindexchanged"
                                                            AutoPostBack="true" runat="server" Visible="true" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblSortOrder" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SortOrder") %>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DataValue" SortExpression="DataValue">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblControlFieldValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DataValue") %>'
                                                            Visible="true" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtControlFieldValueEdit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DataValue") %>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="DataText" HeaderText="DataText">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblControlFieldName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DataText") %>'
                                                            Visible="true" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtControlFieldNameEdit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DataText") %>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Category" HeaderText="Category">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategoryName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Category") %>'
                                                            Visible="true" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtCategoryName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Category") %>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Default">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDefault" runat="server" Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsDefault")) ? "Y":"N"  %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="rbDefaultEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"IsDefault") %>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Branch">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsBranching")) ? "Y":"N"  %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="NonQual">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNonQual" runat="server" Text='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsNonQual")) ? "Y":"N"  %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="~/Images/icon-edit.gif" ID="btnEdit" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                                            CommandName="Edit" />&nbsp;
                                                        <asp:ImageButton ImageUrl="~/Images/icon-delete.gif" ID="btnDelete" runat="server"
                                                            CommandArgument='<%# Container.DataItemIndex %>' Visible='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsBranching")) && !Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsNonQual"))  %>'
                                                            CommandName="Delete" />&nbsp;
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:ImageButton ID="btnUpdate" ImageUrl="~/Images/icon-save.gif" runat="server"
                                                            CommandArgument='<%# Container.DataItemIndex %>' CommandName="Update" />&nbsp;
                                                        <asp:ImageButton ID="btnCancel" ImageUrl="~/Images/icon-cancel.gif" CommandName="Cancel"
                                                            runat="server" CommandArgument='<%# Container.DataItemIndex %>' />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlQS" runat="server" Visible="false">
                                <tr>
                                    <td style="text-align: left">
                                        Query String Name
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txtQueryString" runat="server" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlOT" runat="server" Visible="true">
                                <tr>
                                    <td style="text-align: left">
                                        Show Optional TextBox
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButtonList ID="rbtShowTextBox" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rbtShowTextBox_SelectedIndexChanged">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlIsShowtextBox" runat="server" Visible="false">
                                <tr>
                                    <td style="text-align: left">
                                        ECN Field Name (Textbox)
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlECNTextFieldName" runat="server" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlNon" runat="server" Visible="false">
                                <tr>
                                    <td style="text-align: left">
                                        None Of The Above
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RadioButtonList ID="rbtNoneOfTheAbove" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="false" Selected="true"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlMultiColumnFormat" runat="server" Visible="false">
                                <tr>
                                    <td align="left">
                                        Number of Columns
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rblstColumnFormat" EnableViewState="false" runat="server"
                                            RepeatDirection="Horizontal" CellPadding="1" CellSpacing="1">
                                            <asp:ListItem Text="1" Value="1" Selected="True" />
                                            <asp:ListItem Text="2" Value="2" />
                                            <asp:ListItem Text="3" Value="3" />
                                            <asp:ListItem Text="4" Value="4" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        Repeat Direction
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rblstRepeatDirection" EnableViewState="false" runat="server"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Horizontal" Value="HOR" />
                                            <asp:ListItem Text="Vertical" Value="VER" Selected="True" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center">
                                    <asp:Button CssClass="button" ID="btnAdd" runat="server" Text=" Save " OnClick="btnAdd_Click" />
                                    <asp:SqlDataSource ID="sqlPubFormFields" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                        InsertCommand="sp_SavePubSubscriptionFormsField" InsertCommandType="StoredProcedure"
                                        OnInserted="sqlPubFormFields_Inserted">
                                        <InsertParameters>
                                            <asp:QueryStringParameter DefaultValue="0" Name="PSFieldID" QueryStringField="PSFieldID"
                                                Type="Int32" />
                                            <asp:QueryStringParameter DefaultValue="0" Name="PubID" QueryStringField="PubId"
                                                Type="Int32" />
                                            <asp:ControlParameter Name="ECNFieldName" ControlID="ddlECNField" Type="String" PropertyName="SelectedValue"
                                                ConvertEmptyStringToNull="false" />
                                            <asp:ControlParameter Name="ECNCombinedFieldName" ControlID="ddlECNCombinedField"
                                                Type="String" PropertyName="SelectedValue" ConvertEmptyStringToNull="false" />
                                            <asp:ControlParameter ControlID="RadEditorDisplayName" DefaultValue="" Name="DisplayName"
                                                PropertyName="Content" Type="String" ConvertEmptyStringToNull="false" />
                                            <asp:ControlParameter Name="Grouping" ControlID="ddlGrouping" Type="String" PropertyName="SelectedValue"
                                                DefaultValue="P" />
                                            <asp:ControlParameter Name="DataType" ControlID="ddlDataType" Type="String" PropertyName="SelectedValue"
                                                ConvertEmptyStringToNull="false" />
                                            <asp:ControlParameter Name="ControlType" ControlID="ddlControlType" Type="String"
                                                PropertyName="SelectedValue" ConvertEmptyStringToNull="false" />
                                            <asp:ControlParameter Name="QueryStringName" ControlID="txtQueryString" Type="String"
                                                ConvertEmptyStringToNull="false" />
                                            <asp:ControlParameter Name="MaxLength" ControlID="txtMaxLength" Type="String" ConvertEmptyStringToNull="false" />
                                            <asp:ControlParameter Name="Required" ControlID="rbtlstRequired" Type="String" ConvertEmptyStringToNull="false"
                                                PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="DefaultValue" ControlID="txtDefaultValue" Type="String"
                                                ConvertEmptyStringToNull="false" PropertyName="Text" DefaultValue="" />
                                            <asp:Parameter Name="ValidationType" Type="String" DefaultValue="0" />
                                            <asp:ControlParameter Name="IsActive" ControlID="rbtlstIsActive" Type="String" ConvertEmptyStringToNull="false"
                                                PropertyName="SelectedValue" />
                                            <asp:Parameter Name="ControlValue" Type="String" DefaultValue="0" />
                                            <asp:Parameter Name="AddedBy" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
                                            <asp:Parameter Name="ModifiedBy" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
                                            <asp:ControlParameter Name="ShowTextField" ControlID="rbtShowTextBox" Type="String"
                                                ConvertEmptyStringToNull="false" PropertyName="SelectedValue" />
                                            <asp:Parameter Name="ECNTextFieldName" Type="String" DefaultValue="0" />
                                            <asp:Parameter Name="ID" Type="Int32" DefaultValue="0" Direction="Output" />
                                            <asp:ControlParameter Name="ShowNonOfTheAbove" ControlID="rbtNoneOfTheAbove" Type="Boolean"
                                                ConvertEmptyStringToNull="false" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="ColumnFormat" ControlID="rblstColumnFormat" Type="String"
                                                ConvertEmptyStringToNull="false" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="RepeatDirection" ControlID="rblstRepeatDirection" Type="String"
                                                ConvertEmptyStringToNull="false" PropertyName="SelectedValue" />
                                            <asp:ControlParameter Name="Prepopulate" ControlID="rblstprepopulate" Type="String"
                                                ConvertEmptyStringToNull="false" PropertyName="SelectedValue" />
                                            <asp:Parameter Name="MaxSelections" Type="Int32" DefaultValue="-1" />
                                        </InsertParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>