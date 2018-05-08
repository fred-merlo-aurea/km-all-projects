<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.emaildataeditor" CodeBehind="emaildataeditor.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="mbsp" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.ScrollingPanel" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
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
<asp:textbox id="group_id" runat="Server" visible="False"></asp:textbox>
<asp:textbox id="email_id" runat="Server" visible="False"></asp:textbox>
<table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
    <tr>
        <td class="tableHeader" valign="top" align="left">
            <asp:datagrid id="EmailsGrid" runat="Server" width="800" autogeneratecolumns="False"
                visible="true" datakeyfield="GroupDatafieldsID" 
                onupdatecommand="EmailsGrid_Update" oncancelcommand="EmailsGrid_Cancel" oneditcommand="EmailsGrid_Edit"
                cssclass="grid">
						<ItemStyle></ItemStyle>
						<HeaderStyle CssClass="gridheader" ></HeaderStyle>
						<FooterStyle CssClass="tableHeader1"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="ShortName" HeaderText="UDF Name" ReadOnly="true" itemstyle-width="45%" headerstyle-width="45%"></asp:BoundColumn>
							<asp:BoundColumn DataField="DataValue" HeaderText="Data" itemstyle-width="45%" headerstyle-width="45%"></asp:BoundColumn>
						
							<asp:EditCommandColumn ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" headerstyle-width="10%" itemstyle-width="10%" EditText="<img src=/ecn.images/images/icon-edits1.gif alt='Edit UDF Information' border='0'>"
								CancelText="<img src=/ecn.images/images/icon-cancel.gif alt='Cancel Editing'  border='0'>" UpdateText="<img src=/ecn.images/images/icon-save.gif alt='Save UDF Information'  border='0'>"
								HeaderText="Edit">
                                </asp:EditCommandColumn>
						</Columns>
					</asp:datagrid>
            <br />
        </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        Add/Edit UDF Data
                    </td>
                </tr>
                <tr>
                    <td>
                        UDF Name
                    </td>
                    <td>
                        <asp:DropDownList ID="drpUDF" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        Data
                    </td>
                    <td>
                        <asp:TextBox ID="txtUDFValue" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnAddUDFValue" runat="server" Text="Add" OnClick="btnAddUDFValue_Click"/>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" valign="top" align="left">
            <asp:datagrid id="UDFGroupsGrid" runat="Server" width="800" autogeneratecolumns="False"
                visible="false" cssclass="grid">
						<ItemStyle></ItemStyle>
						<HeaderStyle CssClass="gridheader"></HeaderStyle>
						<FooterStyle CssClass="tableHeader1"></FooterStyle>
						<Columns>
							<asp:BoundColumn DataField="Name" HeaderText="Transaction Name" ReadOnly="true"></asp:BoundColumn>
							<asp:HyperLinkColumn ItemStyle-Width="15%"  ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"
								HeaderText="View" Text="<img  border='0' src=/ecn.images/images/icon-edits1.gif alt='Edit / View History for UDF'>"
								DataNavigateUrlField="URLLink" DataNavigateUrlFormatString="emaildataeditor.aspx?{0}"></asp:HyperLinkColumn>						
						</Columns>
					</asp:datagrid>
            
        </td>
    </tr>
    <tr>
        <td class="tableHeader" valign="top" align="left">
            <asp:Label ID="lblNoTransData" Text="No Transactional Data" runat="server" Visible="false" />
            <mbsp:ScrollingPanel ID="TransactionHistoryPanel" runat="Server" BorderWidth="1px"
                ScrollbarVisibility="Auto" Height="400px" Width="800px">
                <asp:datagrid id="UDFHistoryGrid" runat="Server" width="800" autogeneratecolumns="true"
                    cssclass="grid">
							<ItemStyle HorizontalAlign="center"></ItemStyle>
							<HeaderStyle CssClass="gridheader" HorizontalAlign="center"></HeaderStyle>
							<AlternatingItemStyle CssClass="gridaltrow"/>
						</asp:datagrid>
            </mbsp:ScrollingPanel>
        </td>
    </tr>
</table>
</asp:content>
