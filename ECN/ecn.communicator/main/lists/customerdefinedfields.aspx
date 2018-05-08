<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.customerdefinedfields"
    CodeBehind="customerdefinedfields.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup2 {
            background-color: #ffffff;
            width: 270px;
            vertical-align: top;
            z-index: 1000000;
            color: #000000;
        }

        .modalPopup {
            background-color: transparent;
            padding: 1em 6px;
        }

        .TransparentGrayBackground {
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

        .overlay {
            position: fixed;
            z-index: 50;
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
    <script type="text/javascript">
        function deleteGDF(theID) {
            if (confirm('Are you Sure?\n Selected UDF and its associated Data for all Emails will be permanently deleted.')) {
                window.location = "customdefinedfieldeditor.aspx?CDLID=" + theID + "&delete=1";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
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
            </asp:PlaceHolder>
            <asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="mpeAddTransaction" runat="server" TargetControlID="btnShowPopup1"
                PopupControlID="pnlPopupDimensions" CancelControlID="btnCopyUDFClose" BackgroundCssClass="modalBackground" />
            <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender" runat="server" BehaviorID="RoundedCornersBehavior"
                TargetControlID="pnlPopupDimensionsRound" Radius="6" Corners="All" />
            <asp:Panel ID="pnlPopupDimensions" runat="server" Width="450px" Style="display: none"
                CssClass="modalPopup">
                <asp:Panel ID="pnlPopupDimensionsRound" runat="server" Width="450px" CssClass="modalPopup2">
                    <asp:Label ID="lblCopyFromGroupID" runat="server" Text="" Visible="false"></asp:Label>
                    <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="greySidesLtB">
                            <tr>
                                <td class="gradientTwo addPage" style="border-right: none;">&nbsp;<span style="font-size: 12px; font-weight: bold"> Add Transaction
                                </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="5" cellspacing="5" border="0">
                                        <tr>
                                            <td class="formLabel" valign="middle" align="right">Transaction Name&nbsp;
                                            </td>
                                            <td class="formLabel" valign="middle" align="left">
                                                <asp:TextBox ID="txtTransactionName" runat="Server" Width="136px" class="formfield"
                                                    ValidationGroup="Group"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtransactionname" runat="Server" ErrorMessage="<< required"
                                                    Font-Names="arial" Font-Size="xx-small" ControlToValidate="txtTransactionName"
                                                    ValidationGroup="Group"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlAddTransNameMsg" runat="server" Visible="false">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblAddTransNameMsg" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td class="label" align="center" valign="middle" colspan="2">
                                                <asp:Button ID="btnAdd" runat="server" class="formbuttonsmall" OnClick="btnAdd_Click"
                                                    Text="Add" ValidationGroup="Group" Width="90px" />
                                                &nbsp; &nbsp;
                                                <asp:Button runat="server" Text="Close" ID="btnCopyUDFClose" class="formbuttonsmall"
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
            <asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="mpeCopyUDF" runat="server" TargetControlID="btnShowPopup2"
                PopupControlID="pnlPopupDimensions2" CancelControlID="btnClose" BackgroundCssClass="modalBackground" />
            <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" BehaviorID="RoundedCornersBehavior2"
                TargetControlID="pnlPopupDimensionsRound2" Radius="6" Corners="All" />
            <asp:Panel ID="pnlPopupDimensions2" runat="server" Width="490px" Style="display: none"
                CssClass="modalPopup">
                <asp:Panel ID="pnlPopupDimensionsRound2" runat="server" Width="490px" CssClass="modalPopup2">
                    <asp:Label ID="lblFilterGroupID" runat="server" Text="" Visible="false"></asp:Label>
                    <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="greySidesLtB">
                            <tr>
                                <td class="gradientTwo addPage" style="border-right: none;">&nbsp;<span style="font-size: 12px; font-weight: bold"> Copy UDF </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="5" cellspacing="5" border="0" width="100%">
                            </tr>
                            <asp:Panel ID="pnlCopyUDFMsg" runat="server" Visible="false">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblCopyUDFMsg" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td class="formLabel" valign="middle" align="right">Source Group &nbsp;
                                </td>
                                <td class="formLabel" valign="middle" align="left">
                                    <asp:DropDownList ID="drpSourceGroup" runat="server" DataValueField="GroupID" DataTextField="GroupName"
                                        CssClass="formfield" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="drpSourceGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvTransGroupName" runat="Server" ErrorMessage="<< required"
                                        Font-Names="arial" Font-Size="xx-small" ControlToValidate="drpSourceGroup" ValidationGroup="valdrp"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <asp:Panel ID="pnlCopyUDFMessage" runat="server" Visible="false">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblCopyUDFMessage" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td colspan="2">
                                    <div id="divGrid" runat="Server" align="center" style="text-align: center; height: 250px; padding: 0px 10px 0px 10px; overflow: auto;">
                                        <ecnCustom:ecnGridView ID="gvUDF" runat="Server" CssClass="grid" AutoGenerateColumns="False"
                                            Width="100%" DataKeyNames="GroupDatafieldsID">
                                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundField DataField="GroupDatafieldsID" HeaderText="" ItemStyle-HorizontalAlign="left"
                                                    HeaderStyle-HorizontalAlign="left" Visible="false"></asp:BoundField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkCopyUDF" runat="server" Checked="true"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ShortName" HeaderText="Short Name" ItemStyle-HorizontalAlign="left"
                                                    HeaderStyle-HorizontalAlign="left"></asp:BoundField>
                                                <asp:BoundField DataField="LongName" HeaderText="Long Description" ItemStyle-HorizontalAlign="left"
                                                    HeaderStyle-HorizontalAlign="left"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Transaction Name" ItemStyle-HorizontalAlign="left"
                                                    HeaderStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </ecnCustom:ecnGridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="label" align="middle" valign="middle" colspan="2">
                                    <asp:Button ID="btnCopy" runat="server" class="formbuttonsmall" OnClick="btnCopy_Click"
                                        Text="Copy" ValidationGroup="valdrp" Width="90px" />
                                    &nbsp; &nbsp;
                                    <asp:Button runat="server" Text="Close" ID="btnClose" class="formbuttonsmall" Width="90px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                        </td> </tr> </table>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <div style="text-align: right; padding-bottom: 10px; width: 100%;">
                <div class="tablecontent" style="padding-bottom: 10px">
                    <asp:Button ID="btnAddTransaction" runat="server" Text="Add Transaction" class="formbuttonsmall"
                        OnClick="btnAddTransaction_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCopyUDF" runat="server" Text="Copy UDF" class="formbuttonsmall"
                        OnClick="btnCopyUDF_Click" />
                </div>
            </div>
            <asp:DataGrid ID="CustomDataGrid" runat="Server" CssClass="grid" AutoGenerateColumns="False"
                Width="100%" OnItemDataBound="ConfirmUDFDelete_ItemDataBound" DataKeyField="GroupDatafieldsID"
                OnDeleteCommand="CustomDataGrid_Command">
                <ItemStyle></ItemStyle>
                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                <EditItemStyle HorizontalAlign="Right" />
                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                <Columns>
                    <asp:BoundColumn DataField="ShortName" HeaderText="Short Name" Visible="false">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="LongName" HeaderText="Long Description" ItemStyle-HorizontalAlign="left"
                        HeaderStyle-HorizontalAlign="left"></asp:BoundColumn>
                    <asp:BoundColumn DataField="CodeSnippet" HeaderText="Code Snippet" ItemStyle-HorizontalAlign="left"
                        HeaderStyle-HorizontalAlign="left"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Transactional" HeaderText="Transactional" ItemStyle-HorizontalAlign="center"
                        ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="GroupingName" HeaderText="Transaction" ItemStyle-HorizontalAlign="center"
                        ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="IsPublic" HeaderText="IsPublic" ItemStyle-HorizontalAlign="center"
                        ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="center">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                    </asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <a href='customdefinedfieldeditor.aspx?GroupDatafieldsID=<%# DataBinder.Eval(Container.DataItem, "GroupDatafieldsID") %>&GroupID=<%# DataBinder.Eval(Container.DataItem, "GroupID") %>'>
                                <center>
                                    <img src="/ecn.images/images/icon-edits1.gif" alt='Edit Content' border='0'></center>
                            </a>
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                    </asp:TemplateColumn>
                    <asp:ButtonColumn ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" Text="<img src=/ecn.images/images/icon-delete1.gif alt='Delete UDF' border='0'>"
                        CommandName="Delete">
                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                    </asp:ButtonColumn>
                </Columns>
            </asp:DataGrid>
            <br />
            <asp:Panel ID="pnlAddUDF" runat="server">
                <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
                    <tbody>
                        <tr>
                            <td class="tableHeader" valign="middle" align='right' width="10%">Short Name
                            </td>
                            <td align="left">
                                <asp:TextBox ID="short_name" runat="Server" Width="136px" class="formfield" ValidationGroup="newudfgroup"></asp:TextBox>
                                &nbsp;<font size="-2" face='verdana' color="#000000">(unique name)
                            <asp:RequiredFieldValidator ID="rfvshortname" runat="Server" ErrorMessage="<< required" Font-Names="arial"
                                Font-Size="xx-small" ControlToValidate="short_name" ValidationGroup="newudfgroup"></asp:RequiredFieldValidator>
                                </font>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableHeader" valign="middle" align='right' height="41">Long Description
                            </td>
                            <td height="41" valign="middle" align="left">
                                <asp:TextBox ID="long_name" runat="Server" Width="400px" Height="30px" class="formfield"
                                    TextMode="multiline"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableHeader" valign="middle" align='right' width="10%">Is Public?
                            </td>
                            <td align="left">
                                <asp:CheckBox ID="isPublicChkbox" runat="Server" Enabled="false"></asp:CheckBox>
                                &nbsp;<font size="-2" face='verdana' color="#000000">(Check this box to allow users
                                to manage their email preferences for their subscriptions.)</font>
                            </td>
                        </tr>
                        <asp:Panel ID="transGroup" runat="server" Visible="false">
                            <tr>
                                <td class="tableHeader" valign="middle" align='right' width="10%">Transaction
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="drpTransactionName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpTransactionName_SelectedIndexChanged" DataValueField="datafieldsetID"
                                        DataTextField="name" CssClass="formfield">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="pnlWholeDefaultValue" Visible="false" runat="server">
                            <tr>
                                <td class="tableHeader" valign="middle" align="right" width="10%">Use Default Value?
                                </td>
                                <td align="left">
                                    <asp:CheckBox ID="chkUseDefaultValue" runat="server" AutoPostBack="true" OnCheckedChanged="chkUseDefaultValue_CheckedChanged" />
                                </td>
                            </tr>
                            <asp:Panel ID="pnlDefaultValue" runat="server" Visible="false">
                                <tr>
                                    <td width="10%"></td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlDefaultType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDefaultType_SelectedIndexChanged">
                                                        <asp:ListItem Text="Default Value" Value="default" />
                                                        <asp:ListItem Text="System Value" Value="system" />
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDefaultValue" runat="server" Visible="false" />
                                                    <asp:DropDownList ID="ddlSystemValues" runat="server" Visible="false">
                                                        <asp:ListItem Text="Current Date" Value="CurrentDate" Selected="true" />
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>

                            </asp:Panel>
                        </asp:Panel>
                        <tr>
                            <td class="tableHeader" valign="top" align="center" colspan="2">
                                <asp:Button class="formbutton" ID="add_button" runat="Server" Text="Add Field" OnClick="add_button_Click"
                                    ValidationGroup="newudfgroup"></asp:Button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
