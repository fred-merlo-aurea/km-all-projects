<%@ Control Language="c#" Inherits="ecn.communicator.main.SMSWizard.Controls.GroupInfo" Codebehind="GroupInfo.ascx.cs" %>
<asp:PlaceHolder ID="plCreate" Visible="false" runat="server">
    <div class="section">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td class="formLabel">
                    Select one of the Phone Number list options below.</td>
            </tr>
            <tr>
                <td class="formLabel" width="100%">
                    <asp:RadioButton ID="rbNewGroup" runat="Server" Text="Create New Phone Number List &nbsp;<span class='highLightOne'>(500 Recipients or Less. One Phone Number per line)</span>"
                        GroupName="grpSelect" CssClass="expandAccent" AutoPostBack="true" OnCheckedChanged="rbNewGroup_CheckedChanged">
                    </asp:RadioButton></td>
            </tr>
            <asp:PlaceHolder ID="plNewGroup" runat="server" Visible="false">
                <tr>
                    <td style="padding-left: 30px">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td class="formLabel" align="left" width="15%">
                                    Save in &nbsp;Folder:&nbsp;</td>
                                <td class="dataOne" align="left" width="85%">
                                    <asp:DropDownList ID="drpFolder1" CssClass="dataOne" AutoPostBack="true" runat="server"
                                        DataValueField="FolderID" DataTextField="FolderName" EnableViewState="true" Width="250px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="formLabel" align="left">
                                    List&nbsp;Name:&nbsp;</td>
                                <td class="dataOne" align="left">
                                    <asp:TextBox ID="txtGroupName" CssClass="dataOne" runat="server" Width="250" MaxLength="100"></asp:TextBox>&nbsp;
                                    <asp:RequiredFieldValidator ID="rfvtxtGroupName" runat="server" Font-Size="xx-small"
                                        ControlToValidate="txtGroupName" ErrorMessage="« required" Font-Italic="True"
                                        Font-Bold="True"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td class="formLabel" valign="top" align="left" width="15%">
                                    Phone&nbsp;Number(s):&nbsp;</td>
                                <td class="dataOne" align="left" width="85%">
                                    <asp:TextBox ID="txtPhoneNumber" CssClass="dataOne" runat="server" EnableViewState="true"
                                        Rows="10" Columns="75" TextMode="multiline"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </asp:PlaceHolder>
        </table>
    </div>
    <div class="section">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td class="formLabel" width="100%">
                    <asp:RadioButton ID="rbImportGroup" Text="Import New Phone Number List" GroupName="grpSelect"
                        CssClass="expandAccent" runat="Server" AutoPostBack="true" OnCheckedChanged="rbImportGroup_CheckedChanged">
                    </asp:RadioButton></td>
            </tr>
            <asp:PlaceHolder ID="plImportGroup" runat="server" Visible="false">
                <tr>
                    <td style="padding-left: 30px">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td class="section">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td class="label" align="center" colspan="2">
                                                <font color="#a40000"><strong>(Do not click the "NEXT" button until Import is Complete).</strong></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="label10" align="left" colspan="2">
                                                <asp:RadioButton ID="rbImporttoExisting" runat="Server" Text="Import to Existing List:"
                                                    GroupName="grpImport" CssClass="formLabel" AutoPostBack="true" OnCheckedChanged="rbImporttoExisting_CheckedChanged">
                                                </asp:RadioButton></td>
                                        </tr>
                                        <asp:PlaceHolder ID="plImporttoExisting" runat="server" Visible="false">
                                            <tr>
                                                <td class="formLabel" style="padding-left: 20px" align="left" width="25%">
                                                    Select&nbsp;Folder:&nbsp;</td>
                                                <td class="formLabel" align="left" width="75%">
                                                    <asp:DropDownList ID="drpFolder2" CssClass="dataOne" AutoPostBack="true" runat="server"
                                                        DataValueField="FolderID" DataTextField="FolderName" EnableViewState="true" Width="200px"
                                                        OnSelectedIndexChanged="drpFolder2_SelectedIndexChanged">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td class="formLabel" style="padding-left: 20px" align="left">
                                                    Existing&nbsp;List:&nbsp;</td>
                                                <td class="label10" align="left">
                                                    <asp:DropDownList ID="drpGroup2" CssClass="dataOne" runat="server" Width="200px"
                                                        OnSelectedIndexChanged="drpGroup2_SelectedIndexChanged">
                                                    </asp:DropDownList>&nbsp;
                                                    <asp:RequiredFieldValidator ID="rfvdrpGroup2" runat="server" Font-Size="xx-small"
                                                        ControlToValidate="drpGroup2" ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                            </tr>
                                        </asp:PlaceHolder>
                                        <tr>
                                            <td class="formLabel" align="left" colspan="2">
                                                <asp:RadioButton ID="rbImporttoNew" runat="Server" Text="Import a New List:" GroupName="grpImport"
                                                    CssClass="formLabel" AutoPostBack="true" OnCheckedChanged="rbImporttoNew_CheckedChanged">
                                                </asp:RadioButton></td>
                                        </tr>
                                        <asp:PlaceHolder ID="plImporttoNew" runat="server" Visible="false">
                                            <tr>
                                                <td class="formLabel" style="padding-left: 20px" align="left" width="25%">
                                                    Save in&nbsp;Folder:&nbsp;</td>
                                                <td class="dataOne" align="left" width="75%">
                                                    <asp:DropDownList ID="drpFolder3" CssClass="dataOne" runat="server" DataValueField="FolderID"
                                                        DataTextField="FolderName" EnableViewState="true" Width="200px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td class="formLabel" style="padding-left: 20px" align="left">
                                                    New List&nbsp;Name:&nbsp;</td>
                                                <td class="dataOne" align="left">
                                                    <asp:TextBox ID="txtGroupName1" CssClass="dataOne" runat="server" Width="200" MaxLength="100"></asp:TextBox>&nbsp;
                                                    <asp:RequiredFieldValidator ID="rfvtxtGroupName1" runat="server" Font-Size="xx-small"
                                                        ControlToValidate="txtGroupName1" ErrorMessage="« required" Font-Italic="True"
                                                        Font-Bold="True"></asp:RequiredFieldValidator></td>
                                            </tr>
                                        </asp:PlaceHolder>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="section" valign="top">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td class="headingOne" align="left" colspan="3">
                                                Select File to Import:&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel" align="left" width="15%">
                                                Import From:&nbsp;</td>
                                            <td class="dataOne" align="left" width="50%">
                                                <input class="dataOne" id="fBrowse" type="file" name="fBrowse" runat="server">&nbsp;<span
                                                    class="highLightOne">(CSV, TXT, XLS, XLSX or XML File)</span>
                                                <asp:Label ID="lblfilename" runat="server" Visible="false"></asp:Label></td>
                                            <td class="formLabel" align="center" rowspan="2">
                                                <asp:Button ID="btnUpload" runat="server" Text="Upload File" CssClass="label10" OnClick="btnUpload_Click">
                                                </asp:Button></td>
                                        </tr>
                                        <!--
									<tr>
										<TD class="label10" align="left"><strong>File Type:&nbsp;</strong></TD>
										<TD class="label10" align="left">
											<asp:radiobuttonlist id="rblFileType" CssClass="label10" runat="server" RepeatDirection="Horizontal">
												<asp:ListItem Value="C">CSV</asp:ListItem>
												<asp:ListItem Value="X">Excel</asp:ListItem>
												<asp:ListItem Value="O">Text</asp:ListItem>
												<asp:ListItem Value="XML">XML</asp:ListItem>
											</asp:radiobuttonlist></TD>
									</tr>
									-->
                                        <tr>
                                            <td class="formLabel" align="left">
                                                Worksheet Name :&nbsp;</td>
                                            <td class="dataOne" align="left">
                                                <asp:TextBox ID="txtSheetName" CssClass="dataOne" runat="server" Width="200px" value="Sheet1"></asp:TextBox>&nbsp;<span
                                                    class="highLightOne">(Excel Only)</span></td>
                                        </tr>
                                        <tr>
                                            <td class="formLabel" align="left">
                                                Delimiter:&nbsp;</td>
                                            <td class="dataOne" align="left">
                                                <asp:DropDownList ID="drpDelimiter" CssClass="dataOne" runat="server" Width="70">
                                                    <asp:ListItem runat="server" Value="c" Text="Commas" Selected="true" ID="Listitem1"
                                                        NAME="Listitem1" />
                                                    <asp:ListItem runat="server" Value="TabDelimited" Text="Tabs" ID="Listitem2" NAME="Listitem2" />
                                                </asp:DropDownList>&nbsp;<span class="highLightOne">(for TEXT File)</span></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="headingOne" align="left">
                                    Map Data with Profile fields:&nbsp;</td>
                            </tr>
                            <asp:PlaceHolder ID="plUpload" runat="server" Visible="false">
                                <tr>
                                    <td align="left" width="100%">
                                        <table id="dataCollectionTable" bordercolor="#cccccc" cellspacing="0" cellpadding="3"
                                            width="100%" align="left" border="1" runat="server">
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label" align="center">
                                        <font color="#a40000"><strong>(Do not click the "NEXT" button until Import is Complete.)</strong></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnImport" runat="server" Text="Import Data" CssClass="formLabel"
                                            OnClick="btnImport_Click"></asp:Button></td>
                                </tr>
                            </asp:PlaceHolder>
                        </table>
                    </td>
                </tr>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plImportCompleted" runat="server" Visible="false">
                <tr>
                    <td style="padding-left: 30px">
                        <table cellspacing="0" cellpadding="0" width="80%" border="0">
                            <tr>
                                <td class="label" align="left" colspan="3">
                                    <font color="#05b835"><strong>Import Successful.&nbsp;&nbsp;Click the "NEXT" button
                                        to Continue.</strong></font></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DataGrid ID="dgImport" CssClass="gridWizard" runat="server" Width="100%" AutoGenerateColumns="False"
                                        AllowSorting="True" CellPadding="2">
                                        <AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
                                        <HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                                                HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Totals" HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%">
                                            </asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </asp:PlaceHolder>
        </table>
    </div>
</asp:PlaceHolder>
<div class="section">
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td width="100%" class="formLabel">
                <asp:RadioButton ID="rbExistingGroup" AutoPostBack="true" CssClass="expandAccent"
                    GroupName="grpSelect" Text="Use Existing Phone Number List" runat="Server" Checked="true"
                    OnCheckedChanged="rbExistingGroup_CheckedChanged"></asp:RadioButton></td>
        </tr>
        <asp:PlaceHolder ID="plExistingGroup" Visible="false" runat="server">
            <tr>
                <td style="padding-left: 30px">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="formLabel" align="left" width="15%">
                                Select&nbsp;Folder:&nbsp;</td>
                            <td class="dataOne" align="left" width="85%">
                                <asp:DropDownList ID="drpFolder" CssClass="dataOne" AutoPostBack="true" runat="server"
                                    DataValueField="FolderID" DataTextField="FolderName" EnableViewState="true" Width="250px"
                                    OnSelectedIndexChanged="drpFolder_SelectedIndexChanged">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="formLabel" align="left">
                                Select&nbsp;List:&nbsp;</td>
                            <td class="dataOne" align="left">
                                <asp:DropDownList ID="drpGroup" CssClass="dataOne" AutoPostBack="true" runat="server"
                                    Width="250px">
                                </asp:DropDownList>&nbsp;
                                <asp:RequiredFieldValidator ID="rfvdrpGroup" runat="server" Font-Size="xx-small"
                                    ControlToValidate="drpGroup" ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator></td>
                        </tr>
                      
                    </table>
                </td>
            </tr>
        </asp:PlaceHolder>
    </table>
</div>