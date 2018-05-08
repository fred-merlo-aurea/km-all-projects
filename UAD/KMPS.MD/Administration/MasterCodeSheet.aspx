<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="MasterCodeSheet.aspx.cs" Inherits="KMPS.MDAdmin.MasterCodeSheet" %>

<%@ MasterType VirtualPath="Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">

        function ValidateDelete() {
            if (!confirm('Are you sure you want to delete?')) return false;

            if (!confirm('Are you sure you want to delete MasterCodesheet. It will delete all mapping for the MasterCodesheet?')) return false;

            return true;
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">

        function btnUploadClick()
        {
            document.getElementById('InprogressDiv').style.visibility = "visible";
        }

    </script>

    <div style="text-align: right">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
        <ContentTemplate>
            <table cellpadding="5" cellspacing="5" border="0">
                <tr>
                    <td align="right">
                        <asp:Label ID="Label11" runat="Server">Master Groups :</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpMasterGroups" runat="server" AutoPostBack="True" 
                            DataTextField="DisplayName" DataValueField="MasterGroupID"
                            Width="300px" OnSelectedIndexChanged="drpMasterGroups_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>

                        <asp:RoundedCornersExtender ID="RoundedCornersExtender4" runat="server" TargetControlID="pnlRoundUploadFile"
                            Radius="6" Corners="All" />
                        <asp:ModalPopupExtender ID="mdlPopupCsv" runat="server" TargetControlID="txtfield"
                            PopupControlID="pnlUploadFile" CancelControlID="btnClose" BackgroundCssClass="modalBackground" />
                        <asp:Panel ID="pnlUploadFile" runat="server" Style="display: none" 
                            CssClass="modalPopup">
                            <asp:Panel ID="pnlRoundUploadFile" runat="server" Width="600" CssClass="modalPopup2">
                                <div align="center" style="text-align: center; height: 175px; padding: 10px 10px 10px 10px;">

                                            <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                                                <tr style="background-color: #5783BD;">
                                                    <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold" colspan="3">
                                                        Upload CSV File
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding: 5px; font-size: 12px; font-weight: bold">
                                                        <asp:Label ID="UploadCsvFileLabel" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <input id="FileSelector" type="file" name="FileSelector" runat="server" style="width: 400px;" />                                  
                                                        <br />
                                                        <span class="highLightOne">(CSV Files ONLY)</span>
                                                        <br />
                                                        <div id="InprogressDiv" style="visibility: hidden;font-style:oblique">
                                                            Uploading...
                                                        </div>
                                                        <br />
                                                        <asp:Button ID="btnClose" Text="Cancel" CssClass="button" runat="server" CausesValidation="false" />
                                                        <asp:Button ID="btnUpload" runat="server" Text="Load File" CssClass="button" OnClientClick="btnUploadClick(); return true;" OnClick="btnUpload_Click" CausesValidation="false"></asp:Button>
                                                    </td>
                </tr>
            </table>
                                </div>
                            </asp:Panel>
                        </asp:Panel>

                        <asp:Button ID="btnUploadFile" runat="server" CssClass="button" Text="Upload CSV" OnClick="btnUploadFile_Click" CausesValidation="false" />
                        <asp:TextBox ID="txtfield" runat="server" style="display:none;"></asp:TextBox>
                        <%--<asp:HiddenField ID="hdnField" runat="server" />--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:TextBox ID="txtSearch" runat="server"  Width="250px"></asp:TextBox>&nbsp;&nbsp;
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" />&nbsp;
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="button" OnClick="btnReset_Click" />
                    </td>
                </tr>
            </table>

            <br />
            <asp:GridView ID="gvMasterCodeSheet" runat="server" AutoGenerateColumns="False" DataKeyNames="MasterID"
                EnableModelValidation="True" AllowSorting="True" OnRowDeleting="gvMasterCodeSheet_RowDeleting"
                AllowPaging="True" OnRowCommand="gvMasterCodeSheet_RowCommand" OnSorting="gvMasterCodeSheet_Sorting" OnPageIndexChanging="gvMasterCodeSheet_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="MasterValue" HeaderText="Value" SortExpression="MasterValue"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MasterDesc" HeaderText="Description" SortExpression="MasterDesc"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="40%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MasterDesc1" HeaderText="Description 1" SortExpression="MasterDesc1"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="35%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Enable Searching" SortExpression="EnableSearching"  ItemStyle-Width="10%"  ItemStyle-HorizontalAlign="Center"> 
                        <ItemTemplate><%# (Boolean.Parse(Eval("EnableSearching").ToString())) ? "Yes" : "No"%></ItemTemplate> 
                    </asp:TemplateField> 
                    <asp:ButtonField HeaderStyle-Width="10%" ItemStyle-Width="10%" ButtonType="Link"
                        Text="<img src='Images/ic-edit.gif' style='border:none;'>" CommandName="Select"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                    <%--WGH - need to allow delete--%>
                    <%--<asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:ImageButton ID="ibDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                ImageUrl="~/Images/icon-delete.gif" OnClientClick="return confirm('Are you sure you want to delete this Responses?');" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" 
                        ItemStyle-HorizontalAlign="center" 
                         HeaderStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("MasterID")%>' OnClientClick="return ValidateDelete();"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop">
                        </td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                            alt="" />
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
            </div>
            <div id="divMessage" runat="server" visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="Td1">
                        </td>
                    </tr>
                    <tr>
                        <td id="Td2">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img2" style="padding: 0 0 0 15px;" src="~/images/checkmark.gif" runat="server"
                                            alt="" />
                                    </td>
                                    <td valign="middle" align="left" width="80%" height="100%">
                                        <asp:Label ID="lblMessage" runat="Server" CssClass="Menu"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="Td3">
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">Add Responses</asp:Label></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel"
                Height="200px" BorderWidth="1">
                <table cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtMasterID" runat="server" Visible="false" Text="0"></asp:TextBox>
                            <asp:HiddenField ID="hfMasterID" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblResponseValue" runat="Server">Value :</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtResponseValue" runat="server" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqResponseValue" runat="server" ControlToValidate="txtResponseValue"
                                ErrorMessage="*" ValidationGroup="save" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblResponseDesc" runat="Server">Description :</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtResponseDesc" runat="server" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqResponseDesc" runat="server" ControlToValidate="txtResponseDesc"
                                ErrorMessage="*" ValidationGroup="save" ></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblResponseDesc1" runat="Server">Description 1 :</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtResponseDesc1" runat="server" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label1" runat="Server">Enable Searching :</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="drpEnableSearching" runat="server">
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" ValidationGroup="save"  />
                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                CssClass="button" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
