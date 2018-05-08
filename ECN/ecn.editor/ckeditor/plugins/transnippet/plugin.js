
(function () {
    //Section 1 : Code to execute when the toolbar button is pressed
    var a = {
        exec: function (editor) {
            var prevReturnValue = window.returnValue; // Save the current returnValue 
            window.returnValue = undefined;
            var selected;
            var htmlToReplace;
            var paramsToPass;
            var paramsToReceive = new Array();
            try
            {
                selected = editor.getSelection().getRanges([0]);


                if (selected != null) {

                    var RegexTable = new RegExp("(<table[\\s\\S]*id=[\"']transnippet_.*?[\"'][\\s\\S]*</table>)");
                    var matches = RegexTable.exec(selected[0].startContainer.getText());
                    if (matches) {
                        paramsToPass = matches[1];
                        htmlToReplace = matches[1];
                    }
                    else
                    {
                        var trElement = selected[0].startContainer;
                        var RegexTR = new RegExp("(<table[\\s\\S]*id=[\"']transnippet_.*?[\"'][\\s\\S]*" + trElement.getText()+ "[\\s\\S]*</table>)");
                        var matches = RegexTR.exec(editor.getData(false, null));
                        if (matches) {
                            paramsToPass = matches[1];
                            htmlToReplace = matches[1];
                        }
                    }

                }
            
                editor.config.transnippet.htmlFromEditor = htmlToReplace;
                editor.config.transnippet.htmlTransHTML = paramsToPass;
                
            }
            catch(err)
            {
                
                editor.config.transnippet.htmlFromEditor = "";
                editor.config.transnippet.htmlTransHTML = "";
            }
            var media = window.open(editor.config.transnippetUrl + "?editorName=" + editor.name, "_blank", "width=850px,height=900px, resizable=yes, toolbar=no,status=no,menubar=no,location=no,scrollbars=yes", false);

        }
    },
    //Section 2 : Create the button and add the functionality to it
b = 'transnippet';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);          
            editor.ui.addButton('transnippet', {
                label: 'Transnippet',
                icon: '/ecn.editor/ckeditor/plugins/transnippet/transnippet.png',
                command: b
            });
        }
    });
})();