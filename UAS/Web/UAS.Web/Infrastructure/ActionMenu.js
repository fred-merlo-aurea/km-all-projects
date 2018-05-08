// For use inside a larger DataBound handler. Often called with 'this' as the argument.
// 9-8-2016
function ActionMenu_Register() {

     
    $(".ActionMenu").each(function () {
        eval($(this).parent().children("script").html());
    });
}
// Alias for ActionMenu_Register. Makes for a clearer interface.
// 9-8-2016
function ActionMenu_OnGridDataBoundEvent() {
    ActionMenu_Register();
}