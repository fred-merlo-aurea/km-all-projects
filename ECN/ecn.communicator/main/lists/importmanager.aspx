<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.importmanager" CodeBehind="importmanager.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="ecn" TagName="uploader" Src="../../includes/uploader.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                                <table cellspacing="0" cellpadding="0" width="674" align="center">
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
    <table id="layoutWrapper" cellspacing="0" cellpadding="1" width="100%" border='0'>
        <tr>
            <!-- made left columns 60% width -->
            <td class="tableHeader" width="50%">
            </td>
            <td class="tableHeader" width="50%">
            </td>
            <!-- added left margin -anthony -->
        </tr>
        <!-- 9-15-06 1330               -->
        <tr>
            <td valign="top" width="50%">
                <div align="center">
                    <table width="100%" cellpadding="1" cellspacing="0" style="border-right: #999999 1px solid;
                        border-top: #999999 1px solid; border-left: #999999 1px solid; border-bottom: #999999 1px solid"
                        align="center">
                        <tr>
                            <td class="tableHeader1" colspan="2" valign="middle" align="left" height="25">
                                &nbsp;&nbsp;Upload Files
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <ecn:uploader ID="uploadbox" runat="Server"></ecn:uploader>
                            </td>
                        </tr>
                    </table>
                </div>
                <table cellpadding="0" cellspacing="0" border='0'>
                    <tr>
                        <td>
                            <span class="label"></span>
                        </td>
                    </tr>
                </table>
                <table width="100%" cellpadding="1" cellspacing="0" style="border-right: #999999 1px solid;
                    border-top: #999999 1px solid; border-left: #999999 1px solid; border-bottom: #999999 1px solid">
                    <!--		<tr><td></td></tr>  -->
                    <tr>
                        <td class="tableHeader1" colspan="2" valign="middle" align="left" height="25">
                            &nbsp;&nbsp;Import Data
                        </td>
                    </tr>
                    <tr>
                        <td class="tableContent" align='left' valign="top" height="30" style="position: relative; left: 15%; bottom: -15px;">
                            &nbsp;Group&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom" align="left">
                            <div style="position: relative; left: 60%;">
                                <asp:RadioButton ID="rbGroupChoice1" runat="server" Text="" GroupName="GroupChoice" AutoPostBack="true" OnCheckedChanged="GroupChoice_CheckedChanged" Checked ="true"/>
                                <asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgSelectGroup_Click" Visible="true" />
                                <asp:Label ID="lblSelectGroupName" runat="server" Text="-No Group Selected-" Font-Size="Small"></asp:Label>
                                <asp:HiddenField ID="hfGroupSelectionMode" runat="server" Value="None" />
                                <asp:HiddenField ID="hfSelectGroupID" runat="server" Value="0" />
                               <%-- <asp:DropDownList CssClass="tableContent" ID="drpFolder" AutoPostBack="true" runat="Server"
                                    DataValueField="FolderID" DataTextField="FolderName" EnableViewState="true" Width="260px"
                                    OnSelectedIndexChanged="drpFolder_SelectedIndexChanged">
                                </asp:DropDownList>--%>
                            </div>
                            <div style="padding-bottom: 8px; position: relative; left: 60%;">
                                <asp:RadioButton ID="rbGroupChoice2" runat="server" Text="Master Suppression Group" GroupName="GroupChoice" AutoPostBack="true" OnCheckedChanged="GroupChoice_CheckedChanged"/>
                            </div>
                        </td>
                    </tr>
                    </tr>
                    <%--<tr>
                        <td class="tableContent" valign="bottom" align='right'>
                            &nbsp;Group&nbsp;
                        </td>
                        <td valign="bottom" align="left">
                            <!-- added width to dropdown boxes - anthony 9-15-06 1400 -->
                            <asp:DropDownList CssClass="tableContent" ID="GroupID" EnableViewState="true" runat="Server"
                                DataTextField="GroupName" DataValueField="GroupID" OnSelectedIndexChanged="GroupID_SelectedIndexChanged" AutoPostBack="true" Width="260">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="Server" ID="val_GroupID" ControlToValidate="GroupID"
                                ErrorMessage="Please select a Group" CssClass="errormsg" Display="Static">&laquo;&laquo;&nbsp;Reqiured</asp:RequiredFieldValidator>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="tableContent" valign="middle" align='left'>
                            &nbsp;SubscribeType&nbsp;
                        </td>
                        <td valign="top" align="left">
                            <asp:DropDownList CssClass="tableContent" ID="SubscribeTypeCode" EnableViewState="true"
                                runat="Server" DataTextField="CodeName" DataValueField="CodeValue" Width="260" Style="position: relative; right: 23%;">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableContent" valign="middle" align='left'>
                            &nbsp;FormatType&nbsp;
                        </td>
                        <td valign="top" align="left">
                            <asp:DropDownList CssClass="tableContent" ID="FormatTypeCode" EnableViewState="true"
                                runat="Server" DataTextField="CodeName" DataValueField="CodeValue" Width="260" Style="position: relative; right: 23%;">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableContent" valign="middle" align='left' valign="top">
                            &nbsp;File&nbsp;
                        </td>
                        <td valign="top" align="left">
                            <asp:DropDownList CssClass="tableContent" ID="ImportFile" EnableViewState="true"
                                AutoPostBack="true" runat="Server" DataTextField="FileName" DataValueField="FileName"
                                Width="260" OnSelectedIndexChanged="ImportFile_SelectedIndexChanged" Style="position: relative; right: 23%;">
                            </asp:DropDownList>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableContent" valign="middle" align='left'>
                        </td>
                        <td class="tableContent">
                            <span class="tableContent" style="position: relative; right: 23%;">(CSV, TXT, XLS or XLSX files)</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tableContent" valign="middle" align='left'>
                        </td>
                        <td valign="middle" align="left" class="tableContent" nowrap style="position: relative; right: 14%;">
                            <asp:Panel ID="ExcelPanel" runat="Server" Visible="false" CssClass="smallBold" >
                                <table cellpadding="1" border='0'>
                                    <tr>
                                        <td class="smallBold" nowrap align='right'>
                                            &nbsp;Line number to start&nbsp;:&nbsp;
                                        </td>
                                        <td align="left">
                                            &nbsp;
                                            <asp:TextBox ID="xlslinenumber" runat="Server" EnableViewState="true" CssClass="formtextfieldsmall"
                                                Visible="true" Columns="2" MaxLength="4" AutoSize Text="0"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="smallBold" nowrap align='right'>
                                            &nbsp;Sheet Name&nbsp;:&nbsp;
                                        </td>
                                        <td align="left">
                                            &nbsp;
                                            <asp:TextBox ID="sheetName" runat="Server" EnableViewState="true" CssClass="formtextfieldsmall"
                                                Visible="true" Columns="6" MaxLength="50" AutoSize Text="Sheet1"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="CSVTXTPanel" runat="Server" Visible="true" CssClass="smallBold">
                                <table cellpadding="1" border='0'>
                                    <tr>
                                        <td class="smallBold" nowrap align='right'>
                                            &nbsp;Line number to start&nbsp;:&nbsp;
                                        </td>
                                        <td align="left">
                                            &nbsp;
                                            <asp:TextBox ID="linenumber" runat="Server" EnableViewState="true" CssClass="formtextfieldsmall"
                                                Visible="true" Columns="2" MaxLength="4" AutoSize Text="0"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <asp:PlaceHolder ID="phTXTDelimiter" runat="Server" Visible="false">
                                        <tr>
                                            <td class="smallBold" nowrap align='right'>
                                                &nbsp;Delimiter&nbsp;:&nbsp;
                                            </td>
                                            <td align="left">
                                                &nbsp;
                                                <asp:DropDownList ID="drpDelimiter" runat="Server" Width="70" CssClass="tableContent">
                                                    <asp:ListItem runat="Server" Value="c" Text="Commas" Selected="true" />
                                                    <asp:ListItem runat="Server" Value="TabDelimited" Text="Tabs" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableContent" valign="middle" align='right'>
                            <!--&nbsp;Duplicates&nbsp;-->
                        </td>
                        <td valign="top">
                            <table border='0'>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList CssClass="tableContent" RepeatColumns="2" RepeatLayout="flow"
                                            EnableViewState="true" ID="HandleDuplicates" DataTextField="display" DataValueField="value"
                                            runat="Server" Visible="false">
                                            <asp:ListItem id="I" runat="Server" Value="I" Text="Insert" Selected="true" EnableViewState="true" />
                                            <asp:ListItem id="U" runat="Server" Value="U" Text="Update" EnableViewState="true" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" align="center" height="30" colspan="2">
                            <asp:Button runat="Server" ID="ImportButton" Text="Start Import process" CssClass="formbutton"
                                OnClick="ImportIt"></asp:Button>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="ShowResponse" runat="Server"></asp:Label>
                <asp:Label ID="TestLabel" runat="Server"></asp:Label>
            </td>
            <td valign="top">
                <!-- added left margin - anthony 9-15-06 1330-->
                <table width="100%" cellpadding="1" cellspacing="0" style="border-right: #999999 1px solid;
                    border-top: #999999 1px solid; border-left: #999999 1px solid; border-bottom: #999999 1px solid"
                    align="center">
                    <tr>
                        <td class="tableHeader1" colspan="2" valign="middle" align="left" height="25">
                            &nbsp;&nbsp;File Library
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DataGrid ID="FileGrid" runat="Server" Width="100%" CssClass="grid" Height="100%"
                                AutoGenerateColumns="False" AllowPaging="true" PageSize="15" pag OnPageIndexChanged="Grid_Change">
                                <ItemStyle></ItemStyle>
                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                <PagerStyle Mode="NumericPages" HorizontalAlign="Right"></PagerStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="FileName" HeaderText="File" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Size" HeaderText="Size" HeaderStyle-HorizontalAlign="center"
                                        ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Date" HeaderText="Date" HeaderStyle-HorizontalAlign="center"
                                        ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                    <asp:TemplateColumn>
                                    <ItemTemplate>
                                          <asp:HyperLink ID="hyperDetails" runat="server" NavigateUrl='<%# "importmanager.aspx?deletefile="+ HttpUtility.UrlEncode(Eval("FileName").ToString()) %>' Text="Delete" />      
                                    </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <br />
                            <asp:Label ID="errorlabel" runat="Server" CssClass="errormsg" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
</asp:Content>
