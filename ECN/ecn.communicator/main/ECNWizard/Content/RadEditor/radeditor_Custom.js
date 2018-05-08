
if ('undefined' != typeof(Telerik.Web.UI.Editor)) {
    Telerik.Web.UI.Editor.CommandList["CodeSnippet"] = function (commandName, editor, args) {


        var cbCodesnippet = function (sender, args) {
            editor.pasteHtml(args);
        }
        editor.showExternalDialog('/ecn.editor/ckeditor/plugins/CodeSnippet/CodeSnippets.aspx', null, 750, 750, cbCodesnippet, null, 'Code Snippets', true, Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move, false, false);
    };

    Telerik.Web.UI.Editor.CommandList["InsertLinkManager"] = function (commandName, editor, args) {
        var elem = editor.getSelectedElement(); //returns the selected element.
        var paramsToReceive = new Array();//first element will be link to replace, if there is one
        paramsToReceive[1] = editor.get_html(false,null);
        if (elem) {
            if (elem.tagName == "A") {
                editor.selectElement(elem);
                paramsToReceive[0] = elem.outerHTML;
            }
            else {
                //remove links if present from the current selection - because of JS error thrown in IE
                editor.fire("Unlink");

                //remove Unlink command from the undo/redo list
                var commandsManager = editor.get_commandsManager();
                var commandIndex = commandsManager.getCommandsToUndo().length - 1;
                commandsManager.removeCommandAt(commandIndex);

                var content = editor.getSelectionHtml();

                var link = editor.get_document().createElement("A");

                link.innerHTML = content;
                paramsToReceive[0] = content;
            }
        }
        else
        {
            var link = editor.get_document().createElement("A");
            paramsToReceive[0] = link.outerHTML;
        }

        var myCallbackFunction = function (sender, args) {
            if (args[2] == false) {
                editor.pasteHtml(args[1])
            }
            else
            {
                var toReplace = args[0];
                var ecnIDregex =/ecn_ID="(.*?)"/i;
                var matches = ecnIDregex.exec(toReplace);
                var ecnID = "";
                if (matches != null && matches[1] != null)
                {
                    ecnID = matches[1];
                }
                var stringFormat = "<a ";
                var stringA = args[1];
                var aFromManager = $(stringA);
                stringFormat += "href=\"" + aFromManager.attr('href') + "\" ";
                if(ecnID.length > 0)
                    stringFormat += "ecn_ID=\"" + ecnID + "\" ";

                if (aFromManager.attr('target').length > 0)
                    stringFormat += "target='" + aFromManager.attr('target') + "'";
                
                stringFormat += ">";
                if (aFromManager[0].text.length > 0)
                    stringFormat += aFromManager[0].text;
                else if (aFromManager[0].innerHTML.length > 0)
                    stringFormat += aFromManager[0].innerHTML;
                stringFormat += "</a>";

                var html = editor.get_html(false, null);
                html = html.replace(toReplace, stringFormat);
                editor.set_html(html);
            }
        }

        editor.showExternalDialog(
             '/ecn.communicator/main/ECNWizard/Content/RadEditor/EditorDialogs/LinkManager.aspx',
             paramsToReceive,
             500,
             500,
             myCallbackFunction,
             null,
             'Insert Link',
             true,
             Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move,
             false,
             false);
    };

    Telerik.Web.UI.Editor.CommandList["Transnippet"] = function (commandName, editor, args) {
        var htmlToReplace;
        var paramsToPass;
        var paramsToReceive = new Array();
        var element = editor.getSelectedElement();
        if (element != null && element.tagName == "table") {

            var RegexTable = new RegExp("(<table[\\s\\S]*id=[\"']transnippet_.*?[\"'][\\s\\S]*</table>)");
            var matches = RegexTable.exec(element.outerHTML);
            if (matches) {
                paramsToPass = matches[1];
                htmlToReplace = matches[1];
            }
        }
        else if(element != null) {
            editor.selectElement(element, true);
            var trElement = editor.getSelectedElement();
            var RegexTR = new RegExp("(<table[\\s\\S]*id=[\"']transnippet_.*?[\"'][\\s\\S]*" + trElement.outerHTML + "[\\s\\S]*</table>)");
            var matches = RegexTR.exec(editor.get_html(false, null));
            if (matches) {
                paramsToPass = matches[1];
                htmlToReplace = matches[1];
            }
        }
            paramsToReceive[0] = paramsToPass;
            paramsToReceive[1] = htmlToReplace;
            var cbTransnippet = function (sender, args) {
                if (args[1] != null && args[1].length > 0) {
                    var html = editor.get_html(false, null);
                    html = html.replace(args[1], args[0]);
                    editor.set_html(html);
                }
                else
                {
                    editor.pasteHtml(args[0]);
                }
        }
        editor.showExternalDialog('/ecn.editor/ckeditor/plugins/transnippet/transnippets.aspx', paramsToReceive, 750, 500, cbTransnippet, null, 'Transnippet', true, Telerik.Web.UI.WindowBehaviors.Close +Telerik.Web.UI.WindowBehaviors.Move, false, false);
    };
    Telerik.Web.UI.Editor.CommandList["DynamicTag"] = function (commandName, editor, args) {
        var cbDynamicTag = function (sender, args) {
            editor.pasteHtml(args);
        }
        editor.showExternalDialog('/ecn.editor/ckeditor/plugins/DynamicTag/DynamicTag.aspx', null, 750, 500, cbDynamicTag, null, 'DynamicTag', true, Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move, false, false);
    };

    Telerik.Web.UI.Editor.CommandList["HtmlUpload"] = function (commandName, editor, args) {
        var cbHtmlUpload = function (sender, args) {
            editor.pasteHtml(args);
        }
        editor.showExternalDialog('/ecn.editor/ckeditor/plugins/HtmlUpload/HtmlUpload.aspx', null, 750, 500, cbHtmlUpload, null, 'HtmlUpload', true, Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move, false, false);
    };

    Telerik.Web.UI.Editor.CommandList["SocialShare"] = function (commandName, editor, args) {
        var cbSocialShare = function (sender, args) {
            editor.pasteHtml(args);
        }
        editor.showExternalDialog('/ecn.editor/ckeditor/plugins/SocialShare/SocialShare.aspx', null, 750, 500, cbSocialShare, null, 'SocialShare', true, Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move, false, false);
    };

    Telerik.Web.UI.Editor.CommandList["RSSFeed"] = function (commandName, editor, args) {
        var cbRSSFeed = function (sender, args) {
            editor.pasteHtml(args);
        }
        editor.showExternalDialog('/ecn.editor/ckeditor/plugins/rssfeed/rssfeed.aspx', null, 750, 500, cbRSSFeed, null, 'RSSFeed', true, Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move, false, false);
    };

    Telerik.Web.UI.Editor.CommandList["Templates"] = function (commandName, editor, args) {
        var cbTemplates = function (sender, args) {

            editor.set_html(args);
        }
        editor.showExternalDialog('/ecn.editor/ckeditor/plugins/xmltemplates/xmltemplates.aspx', null, 750, 500, cbTemplates, null, 'Templates', true, Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move, false, false);
    };

    Telerik.Web.UI.Editor.CommandList["Preview"] = function (commandName, editor, args) {
        var cbPreview = function (sender, args) {
        }
        var html = editor.get_html();
        window.open("", "", "width=750, height=500").document.write(html);
        

        //        window.open('data:text/html,<title>Hello Data URL</title><p>File-less HTML page!</p>', 'Hello World', 'width=400,height=200');
        // editor.showExternalDialog('/ecn.editor/ckeditor/plugins/xmltemplates/xmltemplates.aspx', null, 750, 500, cbPreview, null, 'Preview', true, Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move, false, false);
    };
}


//Custom Filters
JSFilter = function () {
    JSFilter.initializeBase(this);
    this.set_isDom(false);
    this.set_enabled(true);
    this.set_name("RadEditor jsfilter");
    this.set_description("RadEditor filter for removing inline js");
}
JSFilter.prototype = {
    getHtmlContent: function (content) {
        var newContent = content;
        var stripJS = new RegExp(/\son\S*=['\"]\S*['\"]/i);

        newContent = newContent.replace(/\son\S*=['\"]\S*['\"]/gi, "");
        return newContent;
    },
    getDesignContent: function (content) {
        var newContent = content;
        var stripJS = new RegExp(/\son\S*=['\"]\S*['\"]/i);

        newContent = newContent.replace(/\son\S*=['\"]\S*['\"]/gi, "");
        return newContent;

    }
}

socialFilter = function () {
    socialFilter.initializeBase(this);
    //set_isDom(false);// in order to create the filter as string filter
    this.set_isDom(false);
    this.set_enabled(true);
    this.set_name("RadEditor socialFilter");
    this.set_description("Relative social links");
}

socialFilter.prototype =
{
    //function executed when going into HTML mode and upon submit
    getHtmlContent: function (content) {
        var originalContent = content;
        //regual expression that will match opening A tag
        var regExp = new RegExp(/<a[^>]*href=((['\"]).*?(ECN_Social_[^>\"']*)\2)[^>]*>/gi);
        //current page domain name - to be excluded from the URL
        var domain = "http://" + window.location.host;
        //sub-folders path
        var path = window.location.pathname;
        path = path.substr(0, path.lastIndexOf("/") + 1);

        var newContent = originalContent.replace(regExp, function (match, offset, fullText) {
            return match.replace(domain + path, "");
        });
        return newContent;
    },

    getDesignContent: function (content) {
        var originalContent = content;
        //regual expression that will match opening A tag
        var regExp = new RegExp(/<a[^>]*href=((['\"]).*?(ECN_Social_[^>\"']*)\2)[^>]*>/gi);
        //current page domain name - to be excluded from the URL
        var domain = "http://" + window.location.host;
        //sub-folders path
        var path = window.location.pathname;
        path = path.substr(0, path.lastIndexOf("/") + 1);
        var newContent = originalContent.replace(regExp, function (match, offset, fullText) {
            return match.replace(domain + path, "");
        });
        return newContent;

    }
}

var telEditor;

//End Custom Filters

function OnClientLoad(editor, args)
{
  //  assignStyles(); 
    if (editor.get_filtersManager().getFilterByName('RadEditor jsfilter') == null) {
        try
        {
            JSFilter.registerClass('JSFilter', Telerik.Web.UI.Editor.Filter);
            editor.get_filtersManager().add(new JSFilter());
        }
        catch(err){}
    }

    if(editor.get_filtersManager().getFilterByName('RadEditor socialFilter') == null)
    {
        try
        {
            socialFilter.registerClass('socialFilter', Telerik.Web.UI.Editor.Filter);
            editor.get_filtersManager().add(new socialFilter());
        }
        catch(err){}
    }

    if (telEditor == null) {
        telEditor = editor;
    }


    editor.set_mode(2);
    editor.set_mode(1);
    editor.repaint();
    //AfterClientLoad(editor, args);
    //Editor.OnClientLoad(editor,args);
}

function repaint()
{
    if (telEditor == null) {
       
    }
    else {
        telEditor.set_mode(2);
        telEditor.set_mode(1);
        telEditor.repaint();
    }
}

function OnClientModeChanged(editor, args)
{

}


function assignStyles() {
    $("span.Templates").parents("a").parents("li").next().attr("style", "margin-right:300px !important");
    $("span.Unlink").parents("a").parents("li").next().attr("style", "margin-right:250px !important");
}

