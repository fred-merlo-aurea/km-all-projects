<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.communicator.main.ECNWizard.wizardSetup_Numbers" CodeBehind="wizardSetup_Numbers.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
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
        /* scrollable root element */
        #wizard 
        {
            background:#EDEDED;
            border:3px solid #789;
            font-size:12px;
            margin:20px auto;
            width:900px;
            overflow:hidden;
            position:relative;
            /* rounded corners for modern browsers */
            -moz-border-radius:5px;
            -webkit-border-radius:5px;
        }

        /* scrollable items */
        #wizard .items {
            clear:both;
            position:relative;
        }
              

        /* title */
        #wizard h2 {
            border-bottom:1px dotted #ccc;
            font-size:22px;
            font-weight:normal;
            margin:10px 0 0 0;
            padding-bottom:15px;
        }

        #wizard h2 em {
            display:block;
            font-size:14px;
            color:#666;
            font-style:normal;
            margin-top:5px;
        }
        
        #wizard legend {
            color:#4E78A0;
        }
        /* input fields 
        #wizard ul {
            padding:0px !important;
            margin:0px !important;
        }

        #wizard li {
            list-style-type:none;
            list-style-image:none;
            margin-bottom:25px;
        }

        #wizard label {
            font-size:16px;
            display:block;
        }

        #wizard label strong {
            color:#789;
            position:relative;
            top:-1px;
        }

        #wizard label em {
            font-size:11px;
            color:#666;
            font-style:normal;
        }

        #wizard .text {
            width:100%;
            padding:5px;
            border:1px solid #ccc;
            color:#456;
            letter-spacing:1px;
        }

        #wizard select {
            border:1px solid #ccc;
            width:94%;
            padding:4px;
        }

        #wizard label span {
            color:#b8128f;
            font-weight:bold;
            position:relative;
            top:4px;
            font-size:20px;
        }

        #wizard .double label {
            width:50%;
            float:left;
        }

        #wizard .double .text {
            width:93%;
        }

        #wizard .clearfix {
            clear:left;
            padding-top:10px;
        }

        #wizard .right {
            float:right;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <div>
                        <br />
                        <br />
                        <b>Processing...</b><br />
                        <br />
                        <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                        <br />
                        <br />
                        <br />
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
    <div style="margin: 5px 0; padding: 0;"  >
        <table width="100%" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td valign="bottom" align="right">
                    <table cellspacing="0" cellpadding="0" border="0">                                     
                        <tr>
                        <td>
                            
                        </td>
                        <td colspan="5" align="right">
                            <table id="tabsCollectionTable" bordercolor="#cccccc" cellspacing="0" cellpadding="2" align="right" border="0" runat="server">                               
                            </table>                               
                        </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center">
                <td>
                    <br />
                    <asp:placeholder id="phError" runat="Server" visible="false">
						<table cellspacing="0" cellpadding="0" width="674">
							<tr>
								<td id="errorTop"></td>
							</tr>
							<tr>
								<td id="errorMiddle">
									<table height="67" width="80%">
										<tr>
											<td valign="top" align="center" width="20%">
                                                <IMG style="padding:0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                            </td>
											<td valign="middle" align="left" width="80%" height="100%">
												<asp:label id="lblErrorMessage" runat="Server"></asp:label>
                                            </td>
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
            <tr align="center">
                <td>
                    <div id="wizard">
                            <div class="items">
                            <asp:placeholder id="phwizContent" runat="Server"></asp:placeholder>
                            </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td  valign="middle" align="right">
                <br />
                    <ul class="surveyNav">                       
                        <li>
                            <asp:imagebutton id="btnPrevious2" runat="Server" causesvalidation="False" text="&laquo;&nbsp;Previous"
                                onclick="btnPrevious_Click" Width="140" Height="32"></asp:imagebutton>
                        </li>
                        <li>
                            <asp:imagebutton id="btnSave2" runat="Server" text="Save" onclick="btnSave_Click" Width="140" Height="32"></asp:imagebutton>
                        </li>
                         <li>
                            <asp:imagebutton id="btnNext2" runat="Server" text="Next&nbsp;&raquo;" onclick="btnNext_Click" Width="140" Height="32"></asp:imagebutton>
                        </li>
                        <li>
                            <asp:imagebutton id="btnCancel2" runat="Server" causesvalidation="False" text="Cancel"
                                onclick="btnCancel_Click" Visible="false"></asp:imagebutton>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>