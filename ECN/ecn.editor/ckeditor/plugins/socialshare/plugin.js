/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

(function () {
    var a = {
        exec: function (editor) {
            var prevReturnValue = window.returnValue; // Save the current returnValue 
            window.returnValue = undefined;
            
            var media = window.open(editor.config.socialshareUrl, "_blank", "width=750px,height=500px, resizable=yes, toolbar=no,status=no,menubar=no,location=no", false);

        }
    },

  b = 'socialshare';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);
            editor.ui.addButton('socialshare', {
                label: 'SocialShare',
                icon: '/ecn.editor/ckeditor/plugins/socialshare/socialshare.png',
                command: b
            });
        }
    });

})();

CKEDITOR.config.socialshare_base = "";