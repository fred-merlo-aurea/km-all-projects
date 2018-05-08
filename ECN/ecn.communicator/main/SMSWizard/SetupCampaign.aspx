<%@ Page Language="c#" EnableEventValidation="false" Inherits="ecn.communicator.main.SMSWizard.SetupCampaign" Codebehind="SetupCampaign.aspx.cs"  MasterPageFile="~/MasterPages/Communicator.Master "%>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin: 10px 0; padding: 0;" align="center">
        <table width="100%" cellspacing="0" cellpadding="0" border='0'>
            <tr>
                <td valign="bottom" align="left">
                    <table cellspacing="0" cellpadding="0" border='0'>
                        <tr>
                            <!--							<td valign="middle" align="center"><asp:image id="imgstep1" runat="Server"></asp:image></td>
							<td valign="middle" align="center"><asp:image id="imgstep2" runat="Server"></asp:image></td>
							<td valign="middle" align="center"><asp:image id="imgstep3" runat="Server"></asp:image></td>
							<td valign="middle" align="center"><asp:image id="imgstep4" runat="Server"></asp:image></td>
							<td valign="middle" align="center"><asp:image id="imgstep5" runat="Server"></asp:image></td>
							-->
                            <td colspan="5">
                                &nbsp;</td>
                            <td valign="top" align="left" rowspan="2">
                                <asp:imagebutton id="btnHome" runat="Server" alt="Message home" imageurl="/ecn.images/images/campaignHome.gif"></asp:imagebutton>
                            </td>
                        </tr>
                        <tr>
                            <td class="steps tabNav" valign="bottom">
                                <asp:imagebutton id="btnStep1" runat="Server" causesvalidation="True"></asp:imagebutton>
                            </td>
                            <td class="steps tabNav" valign="bottom">
                                <asp:imagebutton id="btnStep2" runat="Server" causesvalidation="True"></asp:imagebutton>
                            </td>
                            <td class="steps tabNav" valign="bottom">
                                <asp:imagebutton id="btnStep3" runat="Server" causesvalidation="True"></asp:imagebutton>
                            </td>
                            <td class="steps tabNav" valign="bottom">
                                <asp:imagebutton id="btnStep4" runat="Server" causesvalidation="True"></asp:imagebutton>
                            </td>
                            <td class="steps tabNav" valign="bottom">
                                <asp:imagebutton id="btnStep5" runat="Server" causesvalidation="True"></asp:imagebutton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gradient buttonPad" align='right' valign="middle">
                    <ul class="surveyNav">
                        <!-- items are in reverse order because they're floated right -->
                        <li>
                            <asp:linkbutton id="btnNext1" runat="Server" text="Next&nbsp;&raquo;" onclick="btnNext_Click"></asp:linkbutton>
                        </li>
                        <li>
                            <asp:linkbutton id="btnPrevious1" runat="Server" causesvalidation="False" text="&laquo;&nbsp;Previous"
                                onclick="btnPrevious_Click"></asp:linkbutton>
                        </li>
                        <li>
                            <asp:linkbutton id="btnSave1" runat="Server" text="Save" onclick="btnSave_Click"></asp:linkbutton>
                        </li>
                        <li>
                            <asp:linkbutton id="btnCancel1" runat="Server" causesvalidation="False" text="Cancel"
                                onclick="btnCancel_Click"></asp:linkbutton>
                        </li>
                    </ul>
            </tr>
            <tr>
                <td class="greyOutSide offWhite center label">
                    <br />
                    <asp:placeholder id="phError" runat="Server" visible="false">
						<TABLE cellspacing="0" cellpadding="0" width="674" align="center">
							<tr>
								<td id="errorTop"></td>
							</tr>
							<tr>
								<td id="errorMiddle">
									<TABLE height="67" width="80%">
										<tr>
											<td valign="top" align="center" width="20%"><IMG style="padding:0 0 0 15px;"
													src="/ecn.images/images/errorEx.jpg"></td>
											<td valign="middle" align="left" width="80%" height="100%">
												<asp:label id="lblErrorMessage" runat="Server"></asp:label></td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<tr>
								<td  id="errorBottom"></td>
							</tr>
						</TABLE>
					</asp:placeholder>
                </td>
            </tr>
            <tr>
                <td class=" greyOutSide offWhite center label">
                    <asp:placeholder id="phwizContent" runat="Server"></asp:placeholder>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="gradient buttonPad" valign="middle" align='right'>
                    <ul class="surveyNav">
                        <!-- items are in reverse order because they're floated right -->
                        <li>
                            <asp:linkbutton id="btnNext2" runat="Server" text="Next&nbsp;&raquo;" onclick="btnNext_Click"></asp:linkbutton>
                        </li>
                        <li>
                            <asp:linkbutton id="btnPrevious2" runat="Server" causesvalidation="False" text="&laquo;&nbsp;Previous"
                                onclick="btnPrevious_Click"></asp:linkbutton>
                        </li>
                        <li>
                            <asp:linkbutton id="btnSave2" runat="Server" text="Save" onclick="btnSave_Click"></asp:linkbutton>
                        </li>
                        <li>
                            <asp:linkbutton id="btnCancel2" runat="Server" causesvalidation="False" text="Cancel"
                                onclick="btnCancel_Click"></asp:linkbutton>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
