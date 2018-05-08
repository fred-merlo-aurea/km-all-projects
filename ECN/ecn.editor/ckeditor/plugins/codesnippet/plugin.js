/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

(function () {
    var a = {
        exec: function (editor) {
            var prevReturnValue = window.returnValue; // Save the current returnValue 
            window.returnValue = undefined;
            var media = window.open(editor.config.codesnippetUrl + "/?editor_id=" + editor.name, "_blank", "width=750px,height=600px, resizable=yes, toolbar=no,status=no", false);
            // Set an attribute on the popup window: the id of the editor that opened it.
            media.editor_id = editor.name;
        }
    },

  b = 'codesnippet';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);
            editor.ui.addButton('codesnippet', {
                label: 'Codesnippet',
                icon: '/ecn.editor/ckeditor/plugins/codesnippet/codesnippet.png',
                command: b
            });
        }
    });

})();

CKEDITOR.config.codesnippet_base = "";
