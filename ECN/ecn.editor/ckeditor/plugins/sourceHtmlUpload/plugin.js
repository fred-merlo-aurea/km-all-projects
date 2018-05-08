/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

(function () {
    var a = {
        exec: function (editor) {
            var prevReturnValue = window.returnValue; // Save the current returnValue 
            window.returnValue = undefined; 
           
            var media = window.open(editor.config.htmlUploadUrl, "_blank", "width=200px,height=500px, resizable=yes, toolbar=no,status=no,menubar=no,location=no", false);

        }
    },

  b = 'htmlUpload';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);
            editor.ui.addButton('htmlUpload', {
                label: 'HtmlUpload',
                icon: '/ecn.editor/ckeditor/plugins/htmlUpload/codesnippet.png',
                command: b
            });
        }
    });

})();

CKEDITOR.config.htmlUpload_base = "";