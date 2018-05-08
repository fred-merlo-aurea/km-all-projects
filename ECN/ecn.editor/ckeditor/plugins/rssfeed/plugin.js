(function () {
    //Section 1 : Code to execute when the toolbar button is pressed
    var a = {
        exec: function (editor) {
            var prevReturnValue = window.returnValue; // Save the current returnValue 
            window.returnValue = undefined;
            
            var media = window.open(editor.config.rssfeedUrl, "_blank", "width=400px,height=300px, resizable=yes, toolbar=no,status=no,location=no,menubar=no", false);


        }
    },
    //Section 2 : Create the button and add the functionality to it
b = 'rssfeed';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);
            editor.ui.addButton('rssfeed', {
                label: 'RSSFeed',
                icon: '/ecn.editor/ckeditor/plugins/rssfeed/rss_feed.png',
                command: b
            });
        }
    });
})();