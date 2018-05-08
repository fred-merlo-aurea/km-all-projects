<%@ Register TagPrefix="uc1" TagName="QuoteViewer" Src="../includes/QuoteViewer.ascx" %>

<%@ Page Language="c#" Inherits="ecn.accounts.Engines.QuoteApproval" CodeBehind="QuoteApproval.aspx.cs"
    Title="QuoteApproval" %>

<%@ Register TagPrefix="uc1" TagName="QuoteOptionSelector" Src="../includes/QuoteOptionSelector.ascx" %>
<link rel="stylesheet" href="/ecn.images/images/stylesheet.css" type="text/css" />
<link rel="stylesheet" href="/ecn.images/images/stylesheet_default.css" type="text/css" />
<style type='text/css'>
    .errormsg
    {
        font-weight: bold;
        font-size: 13px;
        color: #ff0000;
        font-family: Verdana, Arial, Helvetica, sans-serif;
        background-color: #fcf8e9;
    }
</style>
<div id="container">
    <div id="outer">
        <form id="Form1" method="post" runat="Server">
        <table width="715" border='0'>
            <tr>
                <td width="100%">
                    <img src="/ecn.images/images/ECN-Header.jpg" />
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <asp:label id="lblErrorMessage" runat="Server" visible="False" width="100%" cssclass="errormsg"></asp:label>
                    <asp:label id="lblMessage" runat="Server" visible="true" width="100%" cssclass="tableheader"></asp:label>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <table width="100%" style="min-height: 800px">
                        <tr>
                            <td align="center" style="font-family: Arial" colspan="2">
                                <div style="font-size: 11px">
                                    <asp:datalist id="dltProgressIndicator" repeatdirection="Horizontal" gridlines="None"
                                        runat="Server" visible="False">
													<SelectedItemStyle ForeColor="Black" BackColor="#CCCCCC"></SelectedItemStyle>
													<ItemStyle ForeColor="Gray"></ItemStyle>
													<ItemTemplate>
														<asp:LinkButton ID="btnStep" Enabled="False" CausesValidation="False" OnClick="btnStep_OnClick"
															runat="Server">
															<%# DataBinder.Eval(Container, "DataItem") %>
														</asp:LinkButton>
													</ItemTemplate>
												</asp:datalist>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="height: 20px">
                                <asp:placeholder id="phdComponents" runat="Server"></asp:placeholder>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" height="20">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <table width="100%">
                        <tr>
                            <td align="center" width="50%">
                                <asp:button id="btnPrevious" onclick="btnPreviousAction_OnClick" visible="False"
                                    runat="Server" text="Previous" causesvalidation="False" cssclass="formbuttonsmall"></asp:button>
                                &nbsp;
                                <asp:button id="btnDecline" onclick="btnDecline_OnClick" visible="False" runat="Server"
                                    text="Decline" causesvalidation="False" cssclass="formbuttonsmall"></asp:button>
                                &nbsp;
                                <asp:button id="btnNextAction" onclick="btnNextAction_OnClick" visible="False" runat="Server"
                                    text="  Next  " cssclass="formbuttonsmall"></asp:button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </form>
    </div>
</div>
</body> </html> 