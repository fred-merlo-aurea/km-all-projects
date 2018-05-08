<%@ Page MasterPageFile="~/MasterPages/Site.master" Language="C#" AutoEventWireup="true"
    CodeBehind="Pub_Events.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.Pub_Events" Title="KMPS Form Builder - Events" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Publisher/LeftMenu.ascx" TagName="LeftMenu" TagPrefix="lftMenu" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpPubList" runat="server">
        <ContentTemplate>
            <JF:BoxPanel ID="BoxPanel2" runat="server" Width="100%" Title="Manage Publication Events">
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <lftMenu:LeftMenu ID="LeftMenu" runat="server" CurrentMenu="EVENTS" />
                        </td>
                        <td style="width: 2%;">
                            &nbsp;
                        </td>
                        <td style="width: 78%; vertical-align: top;">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdPublisherEvents" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                            Width="100%" AllowPaging="true" DataKeyNames="EventID,EventDesc,EventURL" DataSourceID="SqlDataSourcePEventConnect"
                                            OnRowCommand="grdPublisherEvents_RowCommand">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEventID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.EventID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DisplayName" HeaderText="Event Name" ReadOnly="true" SortExpression="DisplayName"
                                                    ItemStyle-Width="20%"   headerstyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign=Top />
                                                <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="true" SortExpression="Location"
                                                    ItemStyle-Width="20%"   headerstyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign=Top/>                                                    
                                                <asp:BoundField DataField="EventType" HeaderText="Type" ReadOnly="true" SortExpression="EventType"
                                                    ItemStyle-Width="10%"   headerstyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap=false ItemStyle-VerticalAlign=Top/>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Starts" ItemStyle-Width="8%"
                                                    SortExpression="StartDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign=Top>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.StartDate","{0:d}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Finishes" ItemStyle-Width="8%"
                                                    SortExpression="EndDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign=Top>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.EndDate","{0:d}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="EventTime" HeaderText="Time" ReadOnly="true" SortExpression="EventTime"
                                                    ItemStyle-Width="12%" ItemStyle-VerticalAlign=Top/>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="5%" HeaderText="Active" SortExpression="IsActive" ItemStyle-VerticalAlign=Top>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIsActive" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IsActive").ToString().ToUpper()=="Y"? "YES" : "NO"%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%" ItemStyle-VerticalAlign=Top>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnEventsEdit" runat="server" ImageUrl="~/images/icon-edit.gif"
                                                            CommandName="EventsEdit" CausesValidation="false" />
                                                        &nbsp;
                                                        <asp:ImageButton ID="imgbtnEventsDelete" runat="server" ImageUrl="~/Images/icon-delete.gif"
                                                            CommandName="EventsDelete" OnClientClick="return confirm('Are you sure, you want to delete this record?')"
                                                            CausesValidation="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSourcePEventConnect" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                                            SelectCommand="sp_PublicationEvents" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:QueryStringParameter QueryStringField="PubID" Name="PubID" Type="Int32" DefaultValue="0" />
                                                <asp:Parameter Name="EventID" Type="Int32" DefaultValue="0" />
                                                <asp:ControlParameter ControlID="txtDisplayName" PropertyName="Text" Name="DisplayName"
                                                    DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:ControlParameter ControlID="txtDescription" PropertyName="Text" Name="EventDesc"
                                                    DefaultValue="0" Type="String" ConvertEmptyStringToNull="false" />
                                                <asp:ControlParameter ControlID="rbtlstIsActive" PropertyName="SelectedValue" Name="IsActive"
                                                    DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:ControlParameter ControlID="ddlType" PropertyName="SelectedValue" Name="EventType"
                                                    DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:ControlParameter ControlID="txtURL" PropertyName="Text" Name="EventURL" DefaultValue="0"
                                                    Type="String" ConvertEmptyStringToNull="false" />
                                                <asp:ControlParameter ControlID="txtStartDate" PropertyName="Text" Name="StartDate"
                                                    DefaultValue="01/01/1753" Type="DateTime" />
                                                <asp:ControlParameter ControlID="txtEndDate" PropertyName="Text" Name="EndDate" DefaultValue="01/01/1753"
                                                    Type="DateTime" />
                                                <asp:ControlParameter ControlID="txtEventTime" PropertyName="Text" Name="EventTime"
                                                    DefaultValue="0" Type="String" ConvertEmptyStringToNull="false" />
                                                <asp:ControlParameter ControlID="txtLocation" PropertyName="Text" Name="Location"
                                                    DefaultValue="0" Type="String" ConvertEmptyStringToNull="false" />
                                                <asp:Parameter Name="AddedBy" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:Parameter Name="ModifiedBy" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
                                                <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; text-align: left">
                                        <JF:BoxPanel ID="BoxPanel1" runat="server" Width="100%" Title="Add Event">
                                            <table style="width: 90%;" border="0" cellpadding="5" cellspacing="0">
                                                <tr>
                                                    <td style="text-align: left; width: 20%">
                                                        Event Name
                                                    </td>
                                                    <td style="text-align: left; width: 80%">
                                                        <asp:TextBox ID="txtDisplayName" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqNewsletterName" runat="server" ControlToValidate="txtDisplayName"
                                                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;" valign="top">
                                                        Description
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" TextMode="MultiLine"
                                                            Rows="5" Width="400"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        Is Active
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rbtlstIsActive" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Event Type
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlType" runat="server" Width="200px">
                                                            <asp:ListItem Text="Trade Show" Value="Trade Show" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Conference" Value="Conference"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        Location
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLocation" runat="server" MaxLength="50" Width="400px" Text=""></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        Link
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtURL" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        Start Date
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtStartDate" runat="server" Width="80px" ReadOnly="false" MaxLength="50"
                                                            AutoPostBack="false"></asp:TextBox>&nbsp;
                                                        <ajaxToolkit:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate"
                                                            Format="MM/dd/yyyy" CssClass="MyCalendar">
                                                        </ajaxToolkit:CalendarExtender>
                                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartDate"
                                                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;">
                                                        End Date
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEndDate" runat="server" Width="80px" ReadOnly="false" MaxLength="50"
                                                            AutoPostBack="false"></asp:TextBox>&nbsp;
                                                        <ajaxToolkit:CalendarExtender ID="calEndDate" runat="server" CssClass="MyCalendar"
                                                            TargetControlID="txtEndDate" Format="MM/dd/yyyy">
                                                        </ajaxToolkit:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndDate"
                                                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left;" valign="top">
                                                        Event Time
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEventTime" runat="server" MaxLength="50" Width="80px" Text=""></asp:TextBox><br />
                                                        <em><span style="font-size: 8pt">Tip: Type 'A' or 'P' to switch AM/PM</span></em>
                                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="txtEventTime"
                                                            Mask="99:99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                            OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="True" ErrorTooltipEnabled="True" />
                                                    </td>
                                                </tr>
                                                                                <tr>
                                    <td colspan="2" >
                                        <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:HiddenField ID="hfldEventId" runat="server" />
                                    </td>
                                </tr>

                                                <tr>
                                                    <td colspan="2" style="text-align: left">
                                                        <asp:Button ID="btnAddEvent" runat="server" Text="SAVE" OnClick="btnAddEvent_Click"
                                                            CssClass="button" />
                                                        <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                                            CausesValidation="false" CssClass="button" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </JF:BoxPanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </JF:BoxPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
