function showDilogue() {
    $('#myModal').modal('show');
}

$(document).ready(function () {
    
    //$("#myForm").validate();
   
    $("#myForm").validator({
        feedback: {
            success: 'glyphicon-ok',
            error: 'glyphicon-remove'
        }
    })
    var contentlable = $("#ContentID option:selected").text();
    $("#cntlbl").text(contentlable);
    $("#ContentID").on("change", function (e) {
        var contentlable = $("#ContentID option:selected").text();
        $("#cntlbl").text(contentlable);
        $('#myModal').modal('hide');
    });

});