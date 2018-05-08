<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="layoutEditorNew.ascx.cs"
    Inherits="ecn.communicator.main.ECNWizard.Content.layoutEditorNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/main/ECNWizard/Content/contentEditor.ascx" TagName="contentEditor" TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Content/contentExplorer.ascx" TagName="contentExplorer" TagPrefix="uc1" %>

<style type="text/css">
    fieldset
    {
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
    }
    .styled-select
    {
        width: 240px;
        background: transparent;
        height: 28px;
        overflow: hidden;
           border: 1px solid #ccc;
    }
    .styled-text
    {
        width: 240px;
        height: 28px;
        overflow: hidden;
        border: 1px solid #ccc;
    }
    .aspBtn
    {
        -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
        -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
        box-shadow: inset 0px 1px 0px 0px #ffffff;
        background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
        background: -moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
        background-color: #ededed;
        -moz-border-radius: 6px;
        -webkit-border-radius: 6px;
        border-radius: 6px;
        border: 1px solid #dcdcdc;
        display: inline-block;
        color: #777777;
        font-family: arial;
        font-size: 7px;
        font-weight: bold;
        padding: 2px 10px;
        text-decoration: none;
        text-shadow: 1px 1px 0px #ffffff;
    }
    .aspBtn:hover
    {
        background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
        background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
        background-color: #dfdfdf;
    }
    .aspBtn:active
    {
        position: relative;
    }
</style>
<style type="text/css">
    .choose1
    {
        padding: 20px; position:relative;
    }
    .popup1
    {
        display: none; z-index: 99999; position: absolute; right: 0; 
    }
    .choose2
    {
        padding: 20px; position:relative;
        position:relative;
    }
    .popup2
    {
         display: none; z-index: 99999; position: absolute; right: 0;
    }
    .choose3
    {
        padding: 20px; position:relative;
    }
    .popup3
    {
         display: none; z-index: 99999; position: absolute; right: 0;
    }
    .choose4
    {
        padding: 20px; position:relative;
    }
    .popup4
    {
        display: none; z-index: 99999; position: absolute; right: 0;
    }
    .choose5
    {
        padding: 20px; position:relative;
    }
    .popup5
    {
     display: none; z-index: 99999; position: absolute; right: 0;
    }
    .choose6
    {
        padding: 20px; position:relative;
    }
    .popup6
    {
       display: none; z-index: 99999; position: absolute; right: 0;
    }
    
    .choose7
    {
        padding: 20px; position:relative;
    }
    .popup7
    {
        display: none; z-index: 99999; position: absolute; right: 0;
    }
    
    .choose8
    {
        padding: 20px; position:relative;
    }
    .popup8
    {
        display: none; z-index: 99999; position: absolute; right: 0;
    }
    
    .choose9
    {
        padding: 20px; position:relative;
    }
    .popup9
    {
       display: none; z-index: 99999; position: absolute; right: 0;
    }
    
    .modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
    .modalPopup
    {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
    }
    .modalPopupCreateContent
    {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        height: 90%;
        overflow: auto;
    }
    .modalPopupPreview
    {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        height: 80%;
        overflow: auto;
    }
    
    .buttonMedium
    {
        width: 135px;
        background: url(buttonMedium.gif) no-repeat left top;
        border: 0;
        font-weight: bold;
        color: #ffffff;
        height: 20px;
        cursor: pointer;
        padding-top: 2px;
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
        .RadWindow.RadWindow_Default.rwNormalWindow.rwTransparentWindow
    {
         z-index:100020 !important;
    }
        .reContentArea{
            height:100% !important;
        }

</style>

<script type="text/javascript">
    $(function () {
        $(".makeItalic").css("font-style", "italic");
    });
</script>

<script type="text/javascript">
    function pageLoaded()
    {
        var popup = $find('<%= modalPopupCreateContent.ClientID %>');
        popup.add_shown(repaintEditor);
    }
    function hideModal()
    {
        $('.modalBackgroundTemp').css("display", "none");
    }
</script>

<script type="text/javascript">
    var hidepopupTimeoutId1;
    var hidepopupTimeoutId2;
    var hidepopupTimeoutId3;
    var hidepopupTimeoutId4;
    var hidepopupTimeoutId5;
    var hidepopupTimeoutId6;
    var hidepopupTimeoutId7;
    var hidepopupTimeoutId8;
    var hidepopupTimeoutId9;
    //Slot 1
    $(".choose1").live("mouseover", function () {
        clearTimeout(hidepopupTimeoutId1);
        $(".popup1").show();
    });

    $(".choose1").live("mouseout", function () {
        hidepopupTimeoutId1 = setTimeout(function () {
            $(".popup1").hide();
        }, 10);
    });

    $(".popup1").live("mouseenter", function () {
        clearTimeout(hidepopupTimeoutId1);
    });


    $(".popup1").live("mouseleave", function () {
        $(".popup1").hide();
    });

    //Slot 2
    $(".choose2").live("mouseover", function () {
        clearTimeout(hidepopupTimeoutId2);
        $(".popup2").show();
    });

    $(".choose2").live("mouseout", function () {
        hidepopupTimeoutId2 = setTimeout(function () {
            $(".popup2").hide();
        }, 10);
    });

    $(".popup2").live("mouseenter", function () {
        clearTimeout(hidepopupTimeoutId2);
    });


    $(".popup2").live("mouseleave", function () {
        $(".popup2").hide();
    });

    //Slot 3
    $(".choose3").live("mouseover", function () {
        clearTimeout(hidepopupTimeoutId3);
        $(".popup3").show();
    });

    $(".choose3").live("mouseout", function () {
        hidepopupTimeoutId3 = setTimeout(function () {
            $(".popup3").hide();
        }, 10);
    });

    $(".popup3").live("mouseenter", function () {
        clearTimeout(hidepopupTimeoutId3);
    });

    $(".popup3").live("mouseleave", function () {
        $(".popup3").hide();
    });

    //Slot 4
    $(".choose4").live("mouseover", function () {
        clearTimeout(hidepopupTimeoutId4);
        $(".popup4").show();
    });

    $(".choose4").live("mouseout", function () {
        hidepopupTimeoutId4 = setTimeout(function () {
            $(".popup4").hide();
        }, 10);
    });

    $(".popup4").live("mouseenter", function () {
        clearTimeout(hidepopupTimeoutId4);
    });


    $(".popup4").live("mouseleave", function () {
        $(".popup4").hide();
    });

    //Slot 5
    $(".choose5").live("mouseover", function () {
        clearTimeout(hidepopupTimeoutId5);
        $(".popup5").show();
    });

    $(".choose5").live("mouseout", function () {
        hidepopupTimeoutId5 = setTimeout(function () {
            $(".popup5").hide();
        }, 10);
    });

    $(".popup5").live("mouseenter", function () {
        clearTimeout(hidepopupTimeoutId5);
    });

    $(".popup5").live("mouseleave", function () {
        $(".popup5").hide();
    });

    //Slot 6
    $(".choose6").live("mouseover", function () {
        clearTimeout(hidepopupTimeoutId6);
        $(".popup6").show();
    });


    $(".choose6").live("mouseout", function () {
        hidepopupTimeoutId6 = setTimeout(function () {
            $(".popup6").hide();
        }, 10);
    });

    $(".popup6").live("mouseenter", function () {
        clearTimeout(hidepopupTimeoutId6);
    });


    $(".popup6").live("mouseleave", function () {
        $(".popup6").hide();
    });

    //Slot 7
    $(".choose7").live("mouseover", function () {
        clearTimeout(hidepopupTimeoutId6);
        $(".popup7").show();
    });


    $(".choose7").live("mouseout", function () {
        hidepopupTimeoutId6 = setTimeout(function () {
            $(".popup7").hide();
        }, 10);
    });

    $(".popup7").live("mouseenter", function () {
        clearTimeout(hidepopupTimeoutId6);
    });


    $(".popup7").live("mouseleave", function () {
        $(".popup7").hide();
    });

    //Slot 8
    $(".choose8").live("mouseover", function () {
        clearTimeout(hidepopupTimeoutId6);
        $(".popup8").show();
    });


    $(".choose8").live("mouseout", function () {
        hidepopupTimeoutId6 = setTimeout(function () {
            $(".popup8").hide();
        }, 10);
    });

    $(".popup8").live("mouseenter", function () {
        clearTimeout(hidepopupTimeoutId6);
    });


    $(".popup8").live("mouseleave", function () {
        $(".popup8").hide();
    });

    //Slot 9
    $(".choose9").live("mouseover", function () {
        clearTimeout(hidepopupTimeoutId6);
        $(".popup9").show();
    });


    $(".choose9").live("mouseout", function () {
        hidepopupTimeoutId6 = setTimeout(function () {
            $(".popup9").hide();
        }, 10);
    });

    $(".popup9").live("mouseenter", function () {
        clearTimeout(hidepopupTimeoutId6);
    });


    $(".popup9").live("mouseleave", function () {
        $(".popup9").hide();
    });

    function clickCreate(i) {
        var val = "<%=HiddenField_SelectedSlot.ClientID%>";
        val = val.replace("HiddenField_SelectedSlot", "btnCreateContentPostback" + i.toString())
        var Create = document.getElementById(val);
        if (Create) {
            Create.click();
        }
        repaint();

    };

    function clickExisting(i) {
        var val = "<%=HiddenField_SelectedSlot.ClientID%>";
        val = val.replace("HiddenField_SelectedSlot", "btnExistingContentPostback" + i.toString())
        var Edit = document.getElementById(val);
        if (Edit) {
            Edit.click();
        }
        repaint();
    };

    function clickDynamic(i) {
        var val = document.getElementById("<%=dynamicContentAccess.ClientID%>");
        if (val.value == "true") {
            var layoutID = getParameterByName("layoutid");
            if (layoutID == "") {
                confirm('You must click "Create" before adding Dynamic Content.');
            }
            else {
                location.href = "../../main/content/contentfilters.aspx?SlotNumber=" + i.toString() + "&LayoutID=" + layoutID;
            }
        }
        else {
            alert('You must upgrade your version of .communicator to access Dynamic Content');
        }
    };

    function getParameterByName(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href.toLowerCase());
        if (results == null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    };

</script>
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
<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>

        <table width="100%">
            <tr>
                <td>
                    <div style="text-align: right; padding-right: 8px">
                    <label class="tableHeader">Category:   </label><asp:dropdownlist ID="ddlCategoryFilter" runat="server" DataValueField="TemplateID" DataTextField="Category" AutoPostBack="True" OnSelectedIndexChanged="ddlCategoryFilter_IndexChanged"></asp:dropdownlist>    
                </div>
                </td>
            </tr>
            <tr>
                <td>
                    <fieldset>
                        <legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Size="Large" Text="Template"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </legend>
                        <table width="100%">
                            <tr align="right">
                                <td>
                                    <asp:Label runat="Server" Text="Selected Template:" Font-Bold="true" Font-Size="Medium"/>
                                    <asp:Label ID="lblTemplateName" runat="Server" CssClass="makeItalic" Text="No Selected Template" font-style="italic" Font-Size="Medium"/>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:DataList ID="templaterepeater" runat="server" CellSpacing="2" CellPadding="2"
                                        GridLines="None" RepeatDirection="Horizontal" RepeatColumns="6" RepeatLayout="Table"
                                        BorderWidth="0" OnItemCommand="DoItemSelect" SelectedItemStyle-BackColor="Transparent"
                                        SelectedItemStyle-VerticalAlign="Top" SelectedItemStyle-HorizontalAlign="Center"
                                        SelectedItemStyle-BorderWidth="2" SelectedItemStyle-BorderColor="orange" SelectedItemStyle-BorderStyle="Dashed"
                                        SelectedItemStyle-Font-Size="xxsmall" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-BorderWidth="0" DataKeyField="TemplateID">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl='<%#DataBinder.Eval(Container.DataItem,"TemplateImage")%>'
                                                CommandName="Select" /><br>
                                            <br>
                                            <asp:TextBox runat="server" ID="TemplateID" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "TemplateID") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <SelectedItemTemplate>
                                            <img src="<%#DataBinder.Eval(Container.DataItem,"TemplateImage")%>"><br>
                                            <br>
                                            <%#DataBinder.Eval(Container.DataItem,"TemplateName")%>
                                            <br>
                                            <asp:TextBox runat="server" ID="TemplateID" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "TemplateID") %>'></asp:TextBox>
                                            <asp:TextBox runat="server" ID="SlotsTotal" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "SlotsTotal") %>'></asp:TextBox>
                                        </SelectedItemTemplate>
                                    </asp:DataList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td>
                    <fieldset>
                        <legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Font-Size="Large" Text="Message"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </legend>
                        <table width="100%">
                            <tr align="right">
                                <td>
                                    <img src="/ecn.images/images/icon-preview-HTML.gif" alt="Preview" border="0">
                                    <asp:LinkButton ID="btnPreview" runat="Server" Text="Preview" Font-Bold="true" Font-Size="Medium"
                                        OnClick="btnPreview_Click">
                                    </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlHidden" runat="server">
                                        <asp:HiddenField ID="HiddenField_SelectedSlot" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField_Content1" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField_Content2" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField_Content3" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField_Content4" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField_Content5" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField_Content6" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField_Content7" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField_Content8" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField_Content9" runat="server" Value="0" />
                                        <asp:Button ID="btnExistingContentPostback1" Text="btn" runat="server" Style="display: none" OnClick="btnExistingContent1_Click" />
                                        <asp:Button ID="btnExistingContentPostback2" Text="btn" runat="server" Style="display: none" OnClick="btnExistingContent2_Click" />
                                        <asp:Button ID="btnExistingContentPostback3" Text="btn" runat="server" Style="display: none" OnClick="btnExistingContent3_Click" />
                                        <asp:Button ID="btnExistingContentPostback4" Text="btn" runat="server" Style="display: none" OnClick="btnExistingContent4_Click" />
                                        <asp:Button ID="btnExistingContentPostback5" Text="btn" runat="server" Style="display: none" OnClick="btnExistingContent5_Click" />
                                        <asp:Button ID="btnExistingContentPostback6" Text="btn" runat="server" Style="display: none" OnClick="btnExistingContent6_Click" />
                                        <asp:Button ID="btnExistingContentPostback7" Text="btn" runat="server" Style="display: none" OnClick="btnExistingContent7_Click" />
                                        <asp:Button ID="btnExistingContentPostback8" Text="btn" runat="server" Style="display: none" OnClick="btnExistingContent8_Click" />
                                        <asp:Button ID="btnExistingContentPostback9" Text="btn" runat="server" Style="display: none" OnClick="btnExistingContent9_Click" />
                                        <asp:Button ID="btnCreateContentPostback1" Text="btn" runat="server" Style="display: none" OnClick="btnCreateContent1_Click" />
                                        <asp:Button ID="btnCreateContentPostback2" Text="btn" runat="server" Style="display: none" OnClick="btnCreateContent2_Click" />
                                        <asp:Button ID="btnCreateContentPostback3" Text="btn" runat="server" Style="display: none" OnClick="btnCreateContent3_Click" />
                                        <asp:Button ID="btnCreateContentPostback4" Text="btn" runat="server" Style="display: none" OnClick="btnCreateContent4_Click" />
                                        <asp:Button ID="btnCreateContentPostback5" Text="btn" runat="server" Style="display: none" OnClick="btnCreateContent5_Click" />
                                        <asp:Button ID="btnCreateContentPostback6" Text="btn" runat="server" Style="display: none" OnClick="btnCreateContent6_Click" />
                                        <asp:Button ID="btnCreateContentPostback7" Text="btn" runat="server" Style="display: none" OnClick="btnCreateContent7_Click" />
                                        <asp:Button ID="btnCreateContentPostback8" Text="btn" runat="server" Style="display: none" OnClick="btnCreateContent8_Click" />
                                        <asp:Button ID="btnCreateContentPostback9" Text="btn" runat="server" Style="display: none" OnClick="btnCreateContent9_Click" />
                                        <asp:HiddenField ID="dynamicContentAccess" runat="server" Value="false" />
                                    </asp:Panel>
                                    <asp:Label ID="LabelPreview" runat="Server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr valign="top">
                <td width="100%">
                    <fieldset>
                        <legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="Large" Text="Details"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </legend>
                        <table cellspacing="0" cellpadding="4" border="0" style="width: 100%; border-collapse: collapse;">
                            <tr>
                                <td class="tableHeader" align='right'>
                                    &nbsp;<span class="label">Name</span>&nbsp;
                                </td>
                                <td align='left'>
                                    <asp:TextBox ID="LayoutName" runat="Server" CssClass="styled-text" Width="225" MaxLength="50"
                                        ValidationGroup="Main"></asp:TextBox>&nbsp;<asp:Button class="formbuttonsmall" ID="CreateAsNewTopButton"
                                            OnClick="CreateAsNewInitialize" runat="Server" Visible="false" Text="Create as new Message"
                                            CssClass="formfield"></asp:Button>
                                </td>
                                <td class="tableHeader" align='right'>
                                    &nbsp;<span class="label">Folder</span>&nbsp;
                                </td>
                                <td colspan="2" align='left'>
                                    <asp:DropDownList ID="folderID" runat="Server" DataTextField="FolderName" DataValueField="FolderID"
                                        EnableViewState="true" CssClass="styled-select" Width="225">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" align='right'>
                                    <span class="label">Border</span>&nbsp;
                                </td>
                                <td align='left'>
                                    <asp:RadioButtonList ID="TemplateBorder" runat="Server" EnableViewState="true" OnSelectedIndexChanged="TemplateBorder_Change"
                                        AutoPostBack="true" RepeatLayout="flow" RepeatColumns="3" CssClass="tableContent">
                                        <asp:ListItem id="WantBorder" runat="Server" Value="Y" Text="Yes" EnableViewState="true" />
                                        <asp:ListItem id="NoBorder" runat="Server" Value="N" Text="No" EnableViewState="true" />
                                        <asp:ListItem id="CustomBorder" runat="Server" Value="C" Text="Custom" EnableViewState="true" />
                                    </asp:RadioButtonList>
                                </td>
                                <td class="tableHeader" align='right'>
                                    <span class="label">Address&nbsp;</span>
                                </td>
                                <td align='left'>
                                    <asp:TextBox ID="DisplayAddress" runat="Server" Columns="70" Width="225" CssClass="styled-text"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" align='right'>
                                </td>
                                <td align='left'>
                                    <asp:TextBox ID="TableOptions" runat="Server" Visible="False" Width="225" Text=""
                                        CssClass="styled-text"></asp:TextBox>
                                </td>
                                <td class="tableHeader" align='right'>
                                    <asp:Label ID="lblMessageType" runat="server" Text="Message Type"></asp:Label>
                                </td>
                                <td align='left'>
                                    <asp:DropDownList ID="ddlMessageType" runat="Server" DataTextField="Name" DataValueField="MessageTypeID"
                                        EnableViewState="true" CssClass="styled-select" Width="225">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="tableHeader" valign="top" align='right'>
                                    <span class="label">Size&nbsp;</span>
                                </td>
                                <td colspan="3" align='left'>
                                    <asp:Label ID="SizeLabel" runat="Server" CssClass="tableContent"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button ID="btnShowPopup3" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="modalPopupPreview" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlPreview" TargetControlID="btnShowPopup3">
</cc1:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlPreview" CssClass="modalPopupPreview">
    <asp:UpdateProgress ID="upPreviewProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="upPreview" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upPreviewProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upPreviewProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upPreview" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:Label ID="lblPreview" runat="Server" Text=""></asp:Label>
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: center">
                        <asp:Button runat="server" Text="Close" ID="btnClosePreview" OnClientClick="hideModal" CssClass="formfield"
                            OnClick="btnClosePreview_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="modalPopupExistingContent" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlExistingContent" TargetControlID="btnShowPopup2">
</cc1:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlExistingContent" CssClass="modalPopup">
    <asp:UpdateProgress ID="upContentExplorerProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upContentExplorer" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upContentExplorerProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upContentExplorerProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upContentExplorer" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <uc1:contentExplorer ID="contentExplorer1" runat="server" />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: center">
                        <asp:Button runat="server" Text="Close" ID="btnExistingContent" CssClass="formfield"
                            OnClick="ExistingContent_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="modalPopupCreateContent" runat="server" BackgroundCssClass="modalBackground modalBackgroundTemp"
    PopupControlID="pnlCreateContent" TargetControlID="btnShowPopup1">
</cc1:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlCreateContent" CssClass="modalPopupCreateContent">
    <asp:UpdateProgress ID="upContentEditorProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upContentEditor" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upContentEditorProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upContentEditorProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upContentEditor" runat="server">
        <ContentTemplate>
            <uc1:contentEditor ID="contentEditor1" runat="server" />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: right">
                        <asp:Button runat="server" Text="Save" ID="btnCreateContent" OnClientClick="hideModal" CssClass="formfield"
                            OnClick="CreateContent_Save"></asp:Button>
                    </td>
                    <td style="text-align: left">
                        <asp:Button runat="server" Text="Close" ID="btnClose" OnClientClick="hideModal" CssClass="formfield" OnClick="CreateContent_Close">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
