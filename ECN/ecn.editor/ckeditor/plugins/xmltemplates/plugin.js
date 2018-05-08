(function () {
    //Section 1 : Code to execute when the toolbar button is pressed
    var a = {
        exec: function (editor) {
            var prevReturnValue = window.returnValue; // Save the current returnValue 
            window.returnValue = undefined;
            
            var media = window.open(editor.config.xmltemplateUrl, "_blank", "scrollbars=yes,width=500px,height=500px, resizable=yes, toolbar=no,status=no,location=no,menubar=no", false);

        }
    },
    //Section 2 : Create the button and add the functionality to it
b = 'xmltemplates';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);
            editor.ui.addButton('xmltemplates', {
                label: 'Templates',
                icon: '/ecn.editor/ckeditor/plugins/xmltemplates/xmltemplates.png',
                command: b
            });
        }
    });
})();