
function cargarPartial(url, div) {
    $.get(url, function (data) {
        $("#" + div).html(data);
    });
}