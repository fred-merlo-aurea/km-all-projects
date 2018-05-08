<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AccountsHomePage.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ecn.accounts.main._default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ MasterType VirtualPath="~/MasterPages/AccountsHomePage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $(".subMenu").hide();
            $(".divBTN").click( function () {
                var btnURL = $(this).find(".btnProduct")[0];
                if(btnURL)
                {
                    window.location.href = btnURL.href;
                }
            });
        });
    </script>
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
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

        fieldset {
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
        }

        .overlay {
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
        


        .btnProduct{
            text-decoration:none !important;
            color:#F57C20 !important;
            font-size:10px !important;
        }

        .btnProduct:hover{
            text-decoration:none !important;
            color:white !important;
            font-size:10px !important;
            font-weight: normal !important;
        }

        .divBTN{
             background-color:white;
            border:1px solid #F57C20 !important;
            color:#F57C20 !important;
            border-radius:20px;
            padding-top:8px;
            padding-bottom:8px;
            padding-left:5px;
            padding-right:5px;
            margin-bottom:10px;
            text-decoration:none !important;
            width:110px;
            font-size:10px !important;
            text-align:center;
            
        }
        .divBTN:hover{
            background-color:#F57C20;
            border:1px solid #F57C20;
            border-radius:20px;
            color:white !important;
            font-weight: normal !important;
            cursor: pointer;
            /*padding-top:8px;
            padding-bottom:8px;
            padding-left:5px;
            padding-right:5px;
            text-decoration:none !important;
            margin:0px;
            
            font-size:10px !important;
            text-align:center;*/
        }

        .divBTN:hover > a{
            background-color:#F57C20;
            border:1px solid #F57C20;
            border-radius:20px;
            color:white !important;
            font-weight: normal !important;
            /*text-decoration:none !important;
            margin:0px;
            
            font-size:10px !important;
            text-align:center;*/
        }

        .divTool{
            display:none;
             background-color:white;
            border:1px solid #B6BCC6;
            position:absolute;
            z-index:15;
            padding:10px;
            width:300px;
            text-align:left;
            border-radius:10px;
            color:black;
            margin-top:15px;
        }

        .divBTN:hover > .divTool{
            display:block;
            background-color:white;
            border:1px solid #B6BCC6;
            position:absolute;
            
            padding:10px;
            width:300px;
            text-align:left;
            
            border-radius:10px;
        }

       
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="upMain" DynamicLayout="false">
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
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div id="platformapps" data-intro="Home - One place to access all the Applications in the Platform" data-position="bottom" style="padding-top:50px;padding-bottom:50px;">
                <table style="width:100%;">
                    <tr>
                        <td >                            
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:50%;text-align:right;"><img src="/EmailMarketing.Site/Content/images/Data_Icon.png" style="height:90px;" alt="Data" /></td>
                                    <td style="text-align:left;font-weight:bold;color:#1571B4;font-size:18px;padding-left:12px;">
                                        <!-- 1st section header-->
                                        Data
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width:33%;">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:50%;text-align:right;"><img src="/EmailMarketing.Site/Content/images/Digital_Icon.png" style="height:90px;" alt="Digital" /></td>
                                    <td  style="text-align:left;font-weight:bold;color:#1571B4;font-size:18px;padding-left:12px;">
                                        <!-- 2nd section header-->
                                        Digital
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width:33%;">
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:50%;text-align:right;"> <img src="/EmailMarketing.Site/Content/images/Audience_Icon.png" style="height:90px;" alt="Audience" /></td>
                                    <td style="text-align:left;font-weight:bold;color:#1571B4;font-size:18px;">
                                        <!-- 3rd section header-->
                                        Audience
                                        
                                    </td>
                                </tr>
                            </table>                                   
                        </td>
                    </tr>
                    <tr>
                        <td style="width:33%;color:#5799C5;font-size:12px;vertical-align:top;padding-top:20px;padding-left:10px;padding-right:30px;">                        
                            <!-- product description-->
                            The future of business is data driven and data is your most powerful weapon. km's data tools know how to harness that power and put it to work for your business. Our dynamic platform fueled by your vision gives you the edge you need to succeed. Let’s get to work.
                        </td>
                        <td style="width:33%;color:#5799C5;font-size:12px;vertical-align:top;padding-top:20px;padding-left:10px;padding-right:30px;">                           
                            In order to have innovation, you must first have imagination. Digital marketing requires a delicate balance between art and science, and is a perfect marriage of design and analytics. Our digital solutions are customizable, making it a breeze for you to achieve the results you want. Let’s create something new.                                                                
                        </td>
                        <td style="width:33%;color:#5799C5;font-size:12px; vertical-align:top;padding-top:20px;padding-left:10px;padding-right:30px;">                            
                            There’s nothing more important than building and engaging your audience. km's audience and customer management solutions provide you with the tools you need to do just that. Our tools are designed to effectively manage and maximize your evolving customers. Let’s grow your community.                                    
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <table style="width:100%;padding:5px;padding-right:30px;">
                                <tr>
                                    <td style="text-align:right;">
                                        <div class="divBTN" style="float:left;">
                                        <asp:HyperLink ID="hlMAF2" runat="server" Text="UAD" CssClass="btnProduct" />
                                            <div class="divTool">Unified Audience Database (UAD) allows you to combine demographic, behavioral, and contextual data into a single, unified profile.</div>
                                            </div>
                                        
                                    </td>
                                    <td style="text-align:left;">
                                        <div class="divBTN" style="float:right;">
                                        <asp:HyperLink ID="hlDataCompare" runat="server" NavigateUrl="/uad.datacompare/" Text="Data Compare" CssClass="btnProduct" />
                                            <div class="divTool">Data Compare is the ability to compare an uploaded list to existing profile fields and demographic fields within UAD.</div>
                                            </div>
                                        
                                    </td>
                                </tr>
                               
                            </table>
                        </td>
                        <td style="vertical-align:top;">
                           
                            <table style="width:100%;padding:5px;padding-right:30px;">
                                <tr>
                                    <td style="text-align:right;">
                                        <div class="divBTN" style="float:left;">
                                        <asp:HyperLink ID="hlCommunicator" runat="server" NavigateUrl="/ecn.communicator/main/default.aspx" Text="Email Marketing" CssClass="btnProduct" />
                                            <div class="divTool">Email Marketing allows you to send email campaigns and newsletters.</div>
                                        </div>
                                        
                                    </td>
                                    <td style="text-align:left;">
                                        <div class="divBTN" style="float:right;">
                                        <asp:HyperLink ID="hlMA" runat="server" NavigateUrl="/ecn.MarketingAutomation/" CssClass="btnProduct" Text="Marketing Automation"  />
                                            <div class="divTool">Marketing Automation allows you to establish triggered responses, streamline and schedule emails based on audience member real-time behaviors (action vs. no action).</div>
                                            </div>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;">
                                        <div class="divBTN" style="float:left;">
                                        <asp:HyperLink ID="hlFormsDesigner" runat="server"  NavigateUrl="/KMWeb/Forms" CssClass="btnProduct" Text="Form Designer" />
                                            <div class="divTool">Create online forms with this easy to use, drop-and-drag form designer. This feature-rich application gives you the tools to collect and manage your data from one location.</div>
                                            </div>
                                        
                                        
                                    </td>
                                    <td style="text-align:left;">
                                        <div class="divBTN" style="float:right;">
                                        <asp:HyperLink ID="hlDomainTracking" runat="server" NavigateUrl="/ecn.domaintracking/Main/Index/" Text="Domain Tracking" CssClass="btnProduct" />
                                            <div class="divTool">Domain Tracking is a feature that allows you to track your customer's behavior or activities throughout your website. Data can originate from an email delivered to a customer or by manually creating a KM cookie when your site is visited.</div>
                                            </div>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;">
                                        <div class="divBTN" style="float:left;">
                                        <asp:HyperLink ID="hlSurvey" runat="server"  NavigateUrl="/ecn.collector/main/survey/" CssClass="btnProduct" Text="Surveys" />
                                            <div class="divTool">Survey tool enables you to create web-based surveys.</div>
                                            </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="vertical-align:top;">
                            <table style="width:100%;padding:5px;padding-right:30px;">
                                <tr>
                                    <td style="text-align:right;width:50%;">
                                        <div class="divBTN" style="float:left;">
                                        <asp:HyperLink ID="hlUASWeb" runat="server" NavigateUrl="/UAS.Web"   Text="AMS" CssClass="btnProduct" />
                                            <div class="divTool">KM’s Audience Management System supports all circulation-based controlled and paid customer transactions. AMS also provides Audience Data Migration System (ADMS) file management.</div>
                                        </div>
                                        
                                    </td>
                                    <td style="text-align:left;">
                                        
                                    </td>
                                </tr>
                               
                            </table>
                        </td>
                    </tr>
                    
                </table>
               <%-- <table cellspacing="0" cellpadding="0" border="0" align="center" width="960px">
                    <tr>
                        <td>
                            <table cellspacing="4" cellpadding="4" width="100%" border="0">
                                <tr style="height: 120px;">
                                    <td width="50%">
                                        <asp:Panel ID="pnlCommunicator" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 20%">
                                                        <asp:HyperLink ID="hlCommunicatorImage" runat="server" NavigateUrl="/ecn.communicator/main/default.aspx" ImageUrl="/ecn.communicator/images/ecn-icon-home-email.png" ImageHeight="40" ImageWidth="60" Width="60" Height="40" />
                                                        
                                                    </td>
                                                    <td width="80%">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlCommunicator" runat="server" NavigateUrl="/ecn.communicator/main/default.aspx" CssClass="EcnSectionHeader" Text="Email Marketing" />
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <asp:Label ID="Label2" runat="server" Text="Email Marketing allows you to send email campaigns and newsletters." CssClass="EcnSectionDetails" Visible="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                    </td>
                                    <td width="50%">
                                        <asp:Panel ID="pnlUAD" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 20%;">
                                                        <asp:HyperLink ID="hlMAF2_Icon" runat="server" ImageUrl="/ecn.communicator/images/ecn-icon-home-audience.png" Width="62" Height="62" />
                                                        
                                                    </td>
                                                    <td width="80%">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlMAF2" runat="server" Text="Unified Audience Database" CssClass="EcnSectionHeader" />
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <asp:Label ID="Label4" runat="server" Text="Unified Audience Database (UAD) allows you to combine demographic, behavioral and contextual data into a single, unified profile." CssClass="EcnSectionDetails" Visible="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                    </td>
                                </tr>

                                <tr style="height: 120px;">
                                    <td>
                                         <asp:Panel ID="pnlMA" runat="server">
                                    
                                    
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width:20%;">
                                                        <asp:HyperLink ID="hlMAImage" runat="server" ImageUrl="/ecn.communicator/images/ecn-icon-home-automation.png" NavigateUrl="/ecn.MarketingAutomation/" />
                                                        
                                                    </td>
                                                    <td style="width:80%;">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlMA" runat="server" NavigateUrl="/ecn.MarketingAutomation/" Text="Marketing Automation" CssClass="EcnSectionHeader" />
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <span class="EcnSectionDetails">Marketing Automation allows you to establish triggered responses, streamline and schedule emails based on audience member real-time behaviors (action vs. no action).</span></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                    
                            </asp:Panel>
                                        </td>
                                        

                                    <td>
                                        <asp:Panel ID="pnlUASWeb" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 20%;">
                                                        <asp:HyperLink ID="hlUASWebImage" runat="server" NavigateUrl="/UAS.Web" ImageUrl="/ecn.communicator/images/ecn-icon-home-uas.png" />
                                                       
                                                    </td>
                                                    <td style="width: 80%;">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlUASWeb" runat="server" NavigateUrl="/UAS.Web" Text="Audience Management System(AMS)" CssClass="EcnSectionHeader" />
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <asp:Label ID="Label1" runat="server" Text="KM’s circulation fulfillment and management system for all circulation based controlled and paid customer transactions. AMS also provides Audience Data Migration System (ADMS) file management. " CssClass="EcnSectionDetails" Visible="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    
                                </tr>
                                <tr style="height: 120px;">
                                    <td>
                                        <asp:Panel ID="pnlFormsDesigner" runat="server">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td style="width:20%;">
                                                        <asp:HyperLink ID="hlFormsDesignerImage" runat="server" ImageUrl="/ecn.communicator/images/ecn-icon-home-formsdesigner.png" NavigateUrl="/KMWeb/Forms" />

                                                    </td>
                                                    <td style="width:80%;">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlFormsDesigner" runat="server" NavigateUrl="/KMWeb/Forms" Text="Form Designer" CssClass="EcnSectionHeader" />
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <span class="EcnSectionDetails">Create online forms with this easy to use, drop-and-drag form designer. This feature-rich application gives you the tools to collect and manage your data from one location.</span></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        

                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlDataCompare" runat="server">
                                            <table style="width:100%;">
                                                <tr>
                                                    <td style="width:20%;">
                                                        <asp:HyperLink ID="hlDataCompareImage" runat="server" ImageUrl="/ecn.communicator/images/ecn-icon-home-datacompare.png" NavigateUrl="/uad.datacompare/" />
                                                    </td>
                                                    <td style="width:80%;">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlDataCompare" runat="server" NavigateUrl="/uad.datacompare/" Text="Data Compare" CssClass="EcnSectionHeader" />
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <span class="EcnSectionDetails">Data compare is the ability to compare an uploaded list to existing profile fields and demographic fields within UAD.</span></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    
                                </tr>
                                <tr style="height: 120px;">
                                    <td>
                                        <asp:Panel ID="pnlCollector" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 20%;">
                                                        <asp:HyperLink ID="hlSurveyImage" runat="server" NavigateUrl="/ecn.collector/main/survey/" ImageUrl="/ecn.communicator/images/ecn-icon-home-surveys.png" ImageHeight="60" ImageWidth="60" Width="60" Height="60" />
                                                        
                                                    </td>
                                                    <td style="width: 80%;">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlSurvey" runat="server" NavigateUrl="/ecn.collector/main/survey/" Text="Surveys" CssClass="EcnSectionHeader" />
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <asp:Label ID="Label8" runat="server" Text="Survey tool enables you to create web-based surveys." CssClass="EcnSectionDetails" Visible="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        
                                    </td>
                                   <td>
                                        <asp:Panel ID="pnlDomainTracking" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width:20%;">
                                                        <asp:HyperLink ID="hlDomainTrackingImage" runat="server" ImageUrl="/ecn.communicator/images/ecn-icon-home-domaintracking.jpg" NavigateUrl="/ecn.domaintracking/Main/Index/" />
                                                        
                                                    </td>
                                                    <td style="width:80%;">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlDomainTracking" runat="server" NavigateUrl="/ecn.domaintracking/Main/Index/" Text="Domain Tracking" CssClass="EcnSectionHeader" />
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <span class="EcnSectionDetails">Domain Tracking is a feature that allows you to track your customer's behavior or activities throughout your website. Data can originate from an email delivered to a customer or by manually creating a KM cookie when your site is visited.</span></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                    </td>
                                    
                                    
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlDigitalEditions" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 20%;">
                                                        <asp:HyperLink ID="hlDigitalEditionsImage" runat="server" NavigateUrl="/ecn.publisher/main/edition/default.aspx" ImageUrl="/ecn.communicator/images/ecn-icon-home-digitaleditions.png" />
                                                       
                                                    </td>
                                                    <td style="width: 80%;">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlDigitalEditions" runat="server" NavigateUrl="/ecn.publisher/main/edition/default.aspx" Text="Digital Editions" CssClass="EcnSectionHeader" />
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <asp:Label ID="Label12" runat="server" Text="Converts static print-ready files into digital publications and magazines." CssClass="EcnSectionDetails" Visible="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    
                                    <td>
                                        <asp:Panel ID="pnlPRT" runat="server">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 20%;">
                                                        <asp:HyperLink ID="hlWQT2_Icon" runat="server" ImageUrl="/ecn.communicator/images/ecn-icon-home-wqt.png" />
                                                        
                                                    </td>
                                                    <td style="width: 80%;">
                                                        <table align="left">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HyperLink ID="hlWQT2" runat="server" Text="Product Reporting" CssClass="EcnSectionHeader" />
                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td align="left">
                                                                    <asp:Label ID="Label10" runat="server" Text="Product Reporting allows you to view your audience member reports via a dynamic web interface." CssClass="EcnSectionDetails" Visible="true"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                    </td>
                                </tr>
                               
                            </table>
                        </td>
                    </tr>
                </table>--%>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
    <asp:ModalPopupExtender ID="modalPopupPwdReset" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlPwdReset" TargetControlID="btnShowPopup2">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlPwdReset" CssClass="modalPopup">
        <asp:UpdateProgress ID="upPwdResetProgress" runat="server" DisplayAfter="10"
            Visible="true" AssociatedUpdatePanelID="upPwdReset" DynamicLayout="false">
            <ProgressTemplate>
                <asp:Panel ID="upPwdResetProgressP1" CssClass="overlay" runat="server">
                    <asp:Panel ID="upPwdResetProgressP2" CssClass="loader" runat="server">
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
        <asp:UpdatePanel ID="upPwdReset" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <br />
                <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                    <table cellspacing="0" cellpadding="0" width="400px" align="center">
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
                    <br />
                    <br />
                </asp:PlaceHolder>

                <table width="100%" style="padding-left: 10px; padding-right: 10px;">
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Password Reset" CssClass="ECN-Label-Heading" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <br />
                            <asp:Label ID="Label14" runat="server" Text="We have noticed that your password does not meet the password requirements. <br/>Please reset your password." CssClass="ECN-Label"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />

                <table width="100%" style="padding-left: 10px; padding-right: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="Label15" runat="server" Text="Password Requirements" CssClass="ECN-Label" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>Must be at least six characters long
                                </li>
                                <li>Must contain at least one lower case letter
                                </li>
                                <li>Must contain at least one upper case letter
                                </li>
                                <li>Must contain at least one number
                                </li>
                                <li>May contain only alpha-numeric characters
                                </li>

                            </ul>
                        </td>
                    </tr>
                </table>

                <center>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Old Password" CssClass="ECN-Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOldPasswrd" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="New Password" CssClass="ECN-Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewPasswrd" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Re-enter New Password" CssClass="ECN-Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReNewPasswrd" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ECN-Button-Medium" OnClick="btnSave_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
