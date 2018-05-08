function maximize(e) {
    var kendoEditor = $(this).data('kendoEditor');
    var container = $("#editorContainer");
    container.empty();
    container.html(
        "<div id='screenEditorWindow'>\
            <textarea id='screenEditor'></textarea>\
        </div>"
    );
    var editorElement = container.find("#screenEditor");
    var windowElement = container.find("#screenEditorWindow");
    editorElement.css('height', '100%');
    var tools = $.extend(true, [], kendoEditor.options.tools);
    $.each(tools, function (index, tool) {
        if (tool.name == 'maximize') {
            tools.remove(index);
        }
    });
    editorElement.kendoEditor({
        tools: tools
    });
    var screenEditor = editorElement.data('kendoEditor');
    screenEditor.value(kendoEditor.value());
    windowElement.kendoWindow({
        modal: true,
        draggable: false,
        animation: false,
        resizable: false,
        close: function () {
            kendoEditor.value(screenEditor.value());
            kendoEditor.trigger('change');
        }
    });
    var editorWindow = windowElement.data('kendoWindow');
    editorWindow.maximize();
}