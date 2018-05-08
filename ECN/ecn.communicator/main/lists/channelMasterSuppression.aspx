<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="channelMasterSuppression.aspx.cs"
    Inherits="ecn.communicator.main.lists.channelMasterSuppression" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecn" TagName="uploader" Src="../../includes/uploader.ascx" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
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
<table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
    <tr>
        <td style="font-family: Arial; font-size: 12px;"  align="left"
            width="50%" >
            Add Emails to be Master suppressed for the Channel.<br />
            Use comma "," (or) semi-colon ";" (or) new line for separating email addresses.
        </td>
        <td style="font-family: Arial; font-size: 12px; font-weight: bold" align="right"
            valign="bottom" width="50%">
        </td>
    </tr>
    <tr>
        <td style="padding-left: 11px;"  align="left">
            <asp:textbox id="emailAddresses" runat="Server" enableviewstate="true" textmode="multiline"
                columns="55" rows="12" class="formfield" width="400px" />
        </td>
        <td valign="bottom" style="font-family: Arial; font-size: 12px; float: left; font-weight: bold">
            <asp:panel id="importResultsPNL" runat="server" visible="false">
                        RESULTS:
                        <br />
                        <asp:label id="MessageLabel" runat="Server" visible="false"></asp:label>
                        <asp:datagrid id="ResultsGrid" runat="Server" cssclass="grid" cellpadding="2" allowsorting="True" autogeneratecolumns="False" width="400px" visible="false">
			                <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
			                <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
			                <Itemstyle HorizontalAlign="Center"></Itemstyle>
			                <Columns>
				                <asp:BoundColumn DataField="Action" HeaderText="Description" Itemstyle-width="75%" headerstyle-width="15%"
					                headerstyle-HorizontalAlign="left" itemstyle-HorizontalAlign="left"></asp:BoundColumn>
				                <asp:BoundColumn DataField="Totals" HeaderText="Totals" Itemstyle-width="25%" headerstyle-width="25%"></asp:BoundColumn>
			                </Columns>
		                </asp:datagrid>
		            </asp:panel>
        </td>
    </tr>
    <tr>
        <td style="float: left; padding-left: 11px">
            <asp:button id="addEmailBTN" runat="Server" text="Add Emails to Master Suppression"
                class="formbuttonsmall" visible="true" onclick="addEmailBTN_Click" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<hr size="1" color="#999999">
<table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
    <tr>
        <td>
            <table border="0" width="40%" style="float: right;" cellpadding="3px" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <asp:textbox id="EmailsTXT" runat="Server" cssclass="formfield" columns="15"></asp:textbox>
                    </td>
                    <td>
                        <asp:button id="searchEmailsBTN" runat="Server" text="Search Emails" class="formbuttonsmall"
                            visible="true" onclick="searchEmailsBTN_Click" />
                    </td>
                    <td>
                        <asp:button id="exportEmailsBTN" runat="Server" text="Export Emails" class="formbuttonsmall"
                            visible="true" onclick="exportEmailsBTN_Click" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            
            <asp:datagrid id="emailsGrid" runat="Server" cssclass="grid" cellpadding="2" allowsorting="True"
                autogeneratecolumns="False" width="100%" visible="true" onitemdatabound="emailsGrid_ItemDataBound"
                onitemcommand="emailsGrid_ItemCommand">
		                <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
		                <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
		                <Itemstyle HorizontalAlign="Center"></Itemstyle>
		                <Columns>
			                <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address" Headerstyle-width="75%" headerstyle-HorizontalAlign="left" itemstyle-HorizontalAlign="left" />
			                <asp:BoundColumn DataField="DateAdded" HeaderText="Date Addred" Headerstyle-width="20%" headerstyle-HorizontalAlign="left" itemstyle-HorizontalAlign="left" />
			                <asp:TemplateColumn HeaderText="Delete" headerstyle-width="5%" itemstyle-width="5%" headerstyle-horizontalalign="center" itemstyle-horizontalalign="center" >							
								<ItemTemplate>
									<asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Content' border='0'&gt;" CausesValidation="false" ID="deleteEmailBTN" CommandName="deleteEmail" CommandArgument=<%# DataBinder.Eval(Container, "DataItem.EmailAddress") %>></asp:LinkButton>      
								</ItemTemplate>                
							</asp:TemplateColumn>			                
		                </Columns>
	                </asp:datagrid>
            <AU:PagerBuilder ID="emailsPager" runat="Server" Width="100%" ControlToPage="emailsGrid"
                PageSize="100">
                <PagerStyle CssClass="gridpager"></PagerStyle>
            </AU:PagerBuilder>
        </td>
    </tr>
</table>
<br />
</asp:content>
