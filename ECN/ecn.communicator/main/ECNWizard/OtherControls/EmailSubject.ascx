<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailSubject.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.OtherControls.EmailSubject" %>

<script type="text/javascript">
    <%-- function pageLoad(sender, args) {
        var emojiButton = $(".selectEmoji");
        emojiButton.click(function (event) { openPopup(event) });

        $("body").click(function () {
            hidePopup();
        });

        // Prevent events from getting pass .popup
        $("#popup").click(function (e) {
            e.stopPropagation();
        });

        var divSubject = document.getElementById('<%= divSubject.ClientID %>');
        divSubject.addEventListener('keypress', function (e) {
            if(e.keyCode == 13)
            {
                e.preventDefault();
            }
            else
            {
                var emailSubject = this.innerHTML;
                var hiddenSubject = $('<%= txtEmailSubject.ClientID %>');
                hiddenSubject.value = ConvertEmailSubject(hiddenSubject);
            }
        });

    }



    function openPopup(event) {
        //get caret position
        $('#popup').fadeIn("slow").css("display", "inline-block");
        var imgButtonPos = $(".selectEmoji").position();

        $('#popup').css("top", imgButtonPos.top + 30).css("left",imgButtonPos.left + 30);

        var position = getCaretCharacterOffsetWithin(document.getElementById('<%= divSubject.ClientID %>'));
        document.getElementById('<%= hfCaretPosition.ClientID %>').value = position;
        event.stopPropagation();
    }

    function hidePopup() {
        $("#popup").fadeOut();
    }

    function imageSelected(image) {

        var imgHTML = "";
        var divSub = document.getElementById('<%= divSubject.ClientID %>');
        var txtEmailSubject = document.getElementById('<%= txtEmailSubject.ClientID %>');
        var convertedSubject = "";
        var text = divSub.innerHTML;
        if (text == null)
            text = "";

        var caretPosition = document.getElementById('<%= hfCaretPosition.ClientID %>').value;

        if (caretPosition == null)
            caretPosition = 0;

        var newImgTag = document.createElement('img');
        newImgTag.src = image.getAttribute("src");
        newImgTag.id = image.id;
        newImgTag.setAttribute("style","width:23px;height:23px;");
        var regSplit = new RegExp("(<img.*?\/?>)");

        var imgSplit = new Array();

        imgSplit = text.split(regSplit);
        var textFullSplit = new Array();

        for(var i = 0;i < imgSplit.length;i++)
        {
            var current = imgSplit[i];
            if(current.includes('<img'))
            {
                //currentindex is image add to finalSplit
                textFullSplit.push(current);
            }
            else
            {
                //currentindex is plain text, loop through each char and add to final split
                for(var j = 0; j < current.length;j++)
                {
                    textFullSplit.push(current.charAt(j));
                }
            }
        }

        var finalText = "";

        if (text.length > 0) {
            if (textFullSplit.length == caretPosition)
            {
                textFullSplit.push(newImgTag.outerHTML);
            }
            else
            {
                textFullSplit.splice(caretPosition,0, newImgTag.outerHTML);
            }
            
            for(var i = 0;i < textFullSplit.length;i++)
            {
                finalText = finalText.concat(textFullSplit[i]);
            }
        }
        else
        {
            finalText = newImgTag.outerHTML;
        }
        divSub.innerHTML = "";

        divSub.innerHTML = finalText;
        txtEmailSubject.value = ConvertEmailSubject(finalText);
        hidePopup();
    }

    function ConvertEmailSubject(subject)
    {
        var regImg = new RegExp("(<img(.*?)\/?>)");
        var regId = new RegExp("id=\"(.*?)\"");
        
        
        var ImgMatche;
        while(ImgMatche = regImg.exec(subject))
        {
            
            var IdMatch = regId.exec(ImgMatche[0]);
            if (IdMatch) {
                toReplace = "ECN.UNI." + IdMatch[1] + ".ECN.UNI";

                subject = subject.replace(ImgMatche[0], toReplace);
            }
        }

        return subject;
    }

    function getCaretCharacterOffsetWithin(element) {
        var caretOffset = 0;
        try {
            var doc = element.ownerDocument || element.document;
            var win = doc.defaultView || doc.parentWindow;
            var sel;
            if (typeof win.getSelection != "undefined") {
                sel = win.getSelection();
                if (sel.rangeCount > 0) {
                    var range = win.getSelection().getRangeAt(0);
                    caretOffset = getCharacterOffsetWithin(range, element);
                }
            } else if ((sel = doc.selection) && sel.type != "Control") {
                var textRange = sel.createRange();
                var preCaretTextRange = doc.body.createTextRange();
                preCaretTextRange.moveToElementText(element);
                preCaretTextRange.setEndPoint("EndToEnd", textRange);
                caretOffset = preCaretTextRange.text.length;
            }
        }
        catch (err) { }
        return caretOffset;
    }

    function getCharacterOffsetWithin(range, node) {
        var treeWalker = document.createTreeWalker(
            node,
            NodeFilter.SHOW_TEXT,
            function (node) {
                var nodeRange = document.createRange();
                nodeRange.selectNode(node);
                return nodeRange.compareBoundaryPoints(Range.END_TO_END, range) < 1 ?
                    NodeFilter.FILTER_ACCEPT : NodeFilter.FILTER_REJECT;
            },
            false
        );

        var charCount = 0;
        while (treeWalker.nextNode()) {
            charCount += treeWalker.currentNode.length;
        }
        if (range.startContainer.nodeType == 3) {
            charCount += range.startOffset;
        }
        return charCount;
    }--%>
</script>

<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />



<%--<asp:TextBox ID="divSubject" runat="server"  />--%>

<asp:HiddenField ID="txtEmailSubject" runat="server" />
<asp:HiddenField ID="hfCaretPosition" runat="server" Value="0" />

<script type="text/javascript">
    if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
    else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
    else { document.addEventListener('load', pageloaded, false); }
    function pageloaded() {

        var initialString = $('#<%= txtEmailSubject.ClientID %>').val();
        initialString = initialString.replace(/\r?\n|\r/g, ' ');
        initialString = twemoji.parse(eval("'" + initialString + "'"));
        $('#<%= txtEmailSubject.ClientID %>').twemojiPicker({ init: initialString, height: '30px' });

        }

        function getImage(arraytosearch, key, valuetosearch) {

            for (var i = 0; i < arraytosearch.length; i++) {

                if (arraytosearch[i][key] == valuetosearch) {
                    return arraytosearch[i];
                }
            }
            return null;
        }

</script>


<%--<div id="popup" class="popup_box">
            <table style="width:100%;height:100%; border:1px solid gray;">
                <tr>
                    <td>
                        <asp:DataList ID="dlEmoji" Width="380" Height="380" runat="server" RepeatColumns="4" CellSpacing="0" CellPadding="0" RepeatDirection="Horizontal">
                            <ItemTemplate>
                                <img onclick="imageSelected(this)" id="<%# DataBinder.Eval(Container.DataItem,"Id") %>" src="<%# DataBinder.Eval(Container.DataItem, "dataURL") %>" style="height:30px;width:30px;" />
                                
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>

</div>--%>
