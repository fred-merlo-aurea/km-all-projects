<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.communicator.blastsmanager.SetMessageThreshold"
    CodeBehind="SetMessageThreshold.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript">
        window.setTimeout("window.open('timeout.htm','Timeout', 'left=100,top=100,height=250,width=300,resizable=no,scrollbar=no,status=no' )", 1500000);
    </script>
    <style>
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        fieldset
        {
            margin: 0.5em 0px;
            padding: 0.0px 0.5em 0px 0.5em;
            border: 1px solid #ccc;
        }
        fieldset p
        {
            margin: 2px 12px 10px 10px;
        }
        fieldset.login label, fieldset.register label, fieldset.changePassword label
        {
            display: block;
        }
        fieldset label.inline
        {
            display: inline;
        }
        legend
        {
            font-size: 1.5em;
            font-weight: 600;
            padding: 2px 4px 8px 4px;
            font-family: "Helvetica Neue" , "Lucida Grande" , "Segoe UI" , Arial, Helvetica, Verdana, sans-serif;
        }
        .TransparentGrayBackground
        {
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
        .overlay
        {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: gray;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
        }
        * html .overlay
        {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }
        .loader
        {
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
        * html .loader
        {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
        }
        .modalBackground
        {
            background-color: #000000;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .popupbody
        {
            /*background:#fffff url(images/blank.gif) repeat-x top;*/
            z-index: 101;
            background-color: #FFFFFF;
            font-family: calibri, trebuchet ms, myriad, tahoma, verdana;
            font-size: 12px;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="Panel3" CssClass="overlay" runat="server">
            <asp:Panel ID="Panel4" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
                <tbody>
                <tr>
                <td>
                  <asp:placeholder id="phError" runat="Server" visible="false">
						<table cellspacing="0" cellpadding="0" width="674" align="center">
							<tr>
								<td id="errorTop"></td>
							</tr>
							<tr>
								<td id="errorMiddle">
									<table height="67" width="80%">
										<tr>
											<td valign="top" align="center" width="20%"><img style="padding:0 0 0 15px;"
													src="/ecn.images/images/errorEx.jpg"></td>
											<td valign="middle" align="left" width="80%" height="100%">
												<asp:label id="lblErrorMessage" runat="Server"></asp:label></td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td  id="errorBottom"></td>
							</tr>
						</table>
					</asp:placeholder>
                </td>
                </tr>
                    <tr>
                        <td class="label" align="center">
                            <br />
                            <table>
                                <tr>
                                    <td width="33%">
                                        &nbsp;
                                    </td>
                                    <td align="center" class="label">
                                        <p style="font-size: 12px;">
                                            By setting this threshold you are setting a daily limit for the channel on the number
                                            of messages that can be sent to each individual. Setting the value to zero (0) will
                                            set an unlimited threshold.
                                        </p>
                                    </td>
                                    <td width="33%">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="center" class="label">
                            <br />
                            Channel Threshold :
                            <asp:DropDownList ID="ddlThreshold" runat="server" CssClass="formfield">
                                <asp:ListItem Selected="True">0</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="label" align="center">
                            <br />
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button"
                                Width="80px" />
                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                Width="80px" CssClass="button" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <br />
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
