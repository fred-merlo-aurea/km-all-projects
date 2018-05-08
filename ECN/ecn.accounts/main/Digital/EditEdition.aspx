<%@ Page Language="c#" Inherits="ecn.accounts.main.Digital.EditEdition" CodeBehind="EditEdition.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="0" cellpadding="5" width="800" border='0'>
        <tr>
            <td class="tableHeader" valign="top" width="20%">
                Edition&nbsp;Name :&nbsp;</td>
            <td>
                <asp:textbox id="txtEditionName" runat="Server" columns="40" cssclass="formfield"></asp:textbox>
                <asp:requiredfieldvalidator id="rfv1" runat="Server" font-bold="True" font-italic="True"
                    errormessage=">> required" controltovalidate="txtEditionName" font-size="xx-small"></asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top">
                Publication :&nbsp;</td>
            <td>
                <asp:dropdownlist id="drpPublicationList" runat="Server" cssclass="formfield" datavaluefield="PublicationID"
                    datatextfield="PublicationName"></asp:dropdownlist>
                <asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="Server" font-bold="True"
                    font-italic="True" errormessage=">> required" controltovalidate="drpPublicationList"
                    font-size="xx-small"></asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align="left">
                Edition Type :</td>
            <td>
                <asp:dropdownlist id="drpEditionType" runat="Server" cssclass="formfield" width="125px">
					<asp:ListItem value="" Selected="True">----- Select Type -----</asp:ListItem>
					<asp:ListItem value="1Flyer">1 Page Flyer</asp:ListItem>
					<asp:ListItem value="2Flyer">2 Page Flyer</asp:ListItem>
					<asp:ListItem value="Catalog">Catalog</asp:ListItem>
					<asp:ListItem value="Magazine">Magazine</asp:ListItem>
				</asp:dropdownlist>
                <asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="Server" font-bold="True"
                    font-italic="True" errormessage=">> required" controltovalidate="drpEditionType"
                    font-size="xx-small"></asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align="left">
                Status:</td>
            <td>
                <asp:dropdownlist id="drpStatus" runat="Server" cssclass="formfield" width="100px">
					<asp:ListItem value="Active" Selected="True">Active</asp:ListItem>
					<asp:ListItem value="InActive">InActive</asp:ListItem>
					<asp:ListItem value="Archieve">Archieve</asp:ListItem>
				</asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top">
                Activation Date :</td>
            <td>
                <asp:textbox id="txtActivationDate" runat="Server" columns="20" width="70" cssclass="formfield"></asp:textbox>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top">
                De-Activation Date :</td>
            <td>
                <asp:textbox id="txtDeActivationDate" runat="Server" columns="20" width="70" cssclass="formfield"></asp:textbox>
            </td>
        </tr>
        <!--
		<tr>
			<td class="tableHeader" valign="top">Login Required:</td>
			<td class="tableContent"><asp:checkbox id="chklogin" runat="Server"></asp:checkbox>&nbsp;
			</td>
		</tr>
		<tr>
			<td class="tableHeader" valign="top">Enable Search :</td>
			<td class="tableContent"><asp:checkbox id="chkSearch" runat="Server"></asp:checkbox>&nbsp;</td>
			</td></tr>
		<tr>
			<td class="tableHeader" valign="top">Turn off link underline :</td>
			<td class="tableContent"><asp:checkbox id="chkunderline" runat="Server"></asp:checkbox>&nbsp;</td>
			</td>
		</tr>-->
        <tr>
            <td class="tableHeader" valign="top" align="left">
                File Name :</td>
            <td>
                <asp:label id="lblFileName" runat="Server" cssclass="tableContent"></asp:label>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align="left">
                Total Pages :</td>
            <td>
                <asp:label id="lblTotalPages" runat="Server" cssclass="tableContent"></asp:label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:label id="lblErrorMessage" runat="Server" visible="False" font-size="x-small"
                    forecolor="Red"></asp:label>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align="center" colspan="2">
                <br />
                <asp:button class="formbutton" id="UpdateButton" runat="Server" text="Update Edition"
                    onclick="Save"></asp:button>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td bgcolor='#eeeeee' colspan="2">
                <cpanel:DataPanel class="noUnderline" ID="dpnlUpload" runat="Server" AllowTitleExpandCollapse="True"
                    TitleText="Replace Existing Digital Edition File" Collapsed="true" ExpandText="Click to display"
                    CollapseText="Click to hide" CollapseImageUrl="/ecn.images/images/collapse2.gif"
                    ExpandImageUrl="/ecn.images/images/collapse2.gif">
                    <table cellspacing="0" cellpadding="5" width="800" border='0' bgcolor="#ffffff">
                        <tr>
                            <td class="tableHeader" valign="top">
                                PDF File :</td>
                            <td>
                                <input class="blue_border_box" id="FileUpload1" type="file" size="40" name="FileUpload1"
                                    runat="Server">
                            </td>
                        </tr>
                        <tr>
                            <td class="tableHeader" align="center" colspan="2">
                                <br />
                                <asp:button class="formbutton" id="btnReUpload" runat="Server" visible="true" text="Upload"
                                    causesvalidation="false" onclick="btnReUpload_Click"></asp:button>
                            </td>
                        </tr>
                    </table>
                </cpanel:DataPanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td bgcolor='#eeeeee' colspan="2">
                <cpanel:DataPanel class="noUnderline" ID="dpnlHistory" runat="Server" AllowTitleExpandCollapse="True"
                    TitleText="History" Collapsed="false" ExpandText="Click to display" CollapseText="Click to hide"
                    CollapseImageUrl="/ecn.images/images/collapse2.gif" ExpandImageUrl="/ecn.images/images/collapse2.gif">
                    <asp:datagrid id="dgVersions" runat="Server" cssclass="grid" width="100%" datakeyfield="EditionID"
                        allowsorting="True" autogeneratecolumns="False" onitemdatabound="dgVersions_ItemDataBound"
                        ondeletecommand="dgVersions_DeleteCommand" backcolor="#ffffff">
						<AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
						<HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
						<Itemstyle HorizontalAlign="Center"></Itemstyle>
						<Columns>
							<asp:BoundColumn DataField="FileName" HeaderText="FileName" headerstyle-HorizontalAlign="left">
								<ItemStyle Width="40%" HorizontalAlign="left"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="Version" HeaderText="Version" headerstyle-HorizontalAlign="Center">
								<ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="Pages" HeaderText="Pages" headerstyle-HorizontalAlign="Center">
								<ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
							</asp:BoundColumn>
							<asp:BoundColumn DataField="CreateDate" HeaderText="Date Created" headerstyle-HorizontalAlign="Center">
								<ItemStyle Width="20%" HorizontalAlign="Center"></ItemStyle>
							</asp:BoundColumn>
							<asp:HyperLinkColumn Text="&lt;img src=/ecn.images/images/icon-preview.gif alt='View Digital Edtion' border='0'&gt;"
								DataNavigateUrlField="EditionID" DataNavigateUrlFormatString="/digitaledition/magazine.aspx?eID={0}"
								HeaderText="Preview" headerstyle-HorizontalAlign="Center" target="_new">
								<ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
							</asp:HyperLinkColumn>
							<asp:TemplateColumn HeaderText="Delete" headerstyle-HorizontalAlign="Center">
								<ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
								<ItemTemplate>
									<asp:ImageButton id="btnDelete" runat="Server" AlternateText="Delete Edition" ImageUrl="/ecn.images/images/icon-delete1.gif"
										CommandName="Delete" />
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid>
                </cpanel:DataPanel>
            </td>
        </tr>
    </table>
</asp:content>
