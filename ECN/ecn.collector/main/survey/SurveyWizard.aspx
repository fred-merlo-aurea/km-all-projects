<%@ Page Language="c#" Inherits="ecn.collector.main.survey.SurveyWizard" CodeBehind="SurveyWizard.aspx.cs" MasterPageFile="~/MasterPages/Collector.Master"  %>

<%@ MasterType VirtualPath="~/MasterPages/Collector.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
<div style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 10px 0px;
    padding-top: 0px" align="center">
    <table width="100%" cellspacing="0" cellpadding="0" border='0'>
        <tr>
            <td valign="bottom" align="left">
                <table cellspacing="0" cellpadding="0" border='0' width="100%">
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
                        <td width="50%" align='right' class="homeButton">
                            <asp:linkbutton id="btnHome" runat="Server" causesvalidation="False" text="<span>Survey Home</span>"
                                onclick="btnHome_Click"></asp:linkbutton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="gradient buttonPad" valign="middle" align='right'>
                <ul class="surveyNav">
                    <!-- items are in reverse order because they're floated right -->
                    <li>
                        <asp:linkbutton id="btnNext1" runat="Server" CssClass="btnbgGreen" text="Next&nbsp;&raquo;" onclick="btnNext_Click"></asp:linkbutton>
                    </li>
                    <li>
                        <asp:linkbutton id="btnPrevious1" runat="Server"  CssClass="btnbgGray" causesvalidation="False" text="&laquo;&nbsp;Previous"
                            onclick="btnPrevious_Click"></asp:linkbutton>
                    </li>
                    <li>
                        <asp:linkbutton id="btnSave1" runat="Server" text="Save" onclick="btnSave_Click" CssClass="btnbgGray" ></asp:linkbutton>
                    </li>
                    <li>
                        <asp:linkbutton id="btnCancel1" runat="Server" causesvalidation="False" text="Cancel" CssClass="btnbgRed" 
                            onclick="btnCancel_Click"></asp:linkbutton>
                    </li>
                </ul>
            </td>
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
											<td valign="top" align="center" width="20%"><IMG style="PADDING-RIGHT: 0px; PADDING-LEFT: 15px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px"
													src="/ecn.images/images/errorEx.jpg"></td>
											<td valign="middle" align="left" width="80%" height="100%">
												<asp:label id="lblErrorMessage" runat="Server"></asp:label></td>
										</tr>
									</TABLE>
								</td>
							</tr>
							<tr>
								<td id="errorBottom"></td>
							</tr>
						</TABLE>
					</asp:placeholder>
            </td>
        </tr>
        <tr>
            <td class="greyOutSide offWhite label">
                <asp:placeholder id="phwizContent" runat="Server"></asp:placeholder>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="gradient buttonPad" valign="middle" align='right'>
                <ul class="surveyNav">
                    <!-- items are in reverse order because they're floated right -->
                    <li>
                        <asp:linkbutton id="btnNext2" runat="Server" text="Next&nbsp;&raquo;" onclick="btnNext_Click" CssClass="btnbgGreen" ></asp:linkbutton>
                    </li>
                    <li>
                        <asp:linkbutton id="btnPrevious2" runat="Server" causesvalidation="False" text="&laquo;&nbsp;Previous"
                            onclick="btnPrevious_Click" CssClass="btnbgGray" ></asp:linkbutton>
                    </li>
                    <li>
                        <asp:linkbutton id="btnSave2" runat="Server" text="Save" onclick="btnSave_Click" CssClass="btnbgGray" ></asp:linkbutton>
                    </li>
                    <li>
                        <asp:linkbutton id="btnCancel2" runat="Server" causesvalidation="False" text="Cancel" CssClass="btnbgRed" 
                            onclick="btnCancel_Click"></asp:linkbutton>
                    </li>
                </ul>
            </td>
        </tr>
    </table>
    <br />
</div>
</asp:content>




