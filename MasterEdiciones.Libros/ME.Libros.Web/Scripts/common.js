$(document).ajaxSend(function () {
    $('#loading-indicator').show();
});

$(document).ajaxComplete(function () {
    $('#loading-indicator').hide();
});

function mensajeSuccess(text) {
    toastr.success(text);
}

function mensajeError(text) {
    toastr.error(text);
}

function cargarPestaña(url) {
    cargarVistaParcial(url, "tabContenedor", false);
}

function cargarVistaParcial(url, divId, async) {
    if (url == "#") return; // Sacar esta linea luego de tener todos los link completos
    if (async == undefined) {
        async = true;
    }
    $.ajax({
        method: "GET",
        async: async,
        url: url,
        dataType: "html",
        error: function (text, error) {
            mensajeError("Ha ocurrido un error: " + text + error);
        },
        success: function (data) {
            $("#" + divId).html(data);
        },
        timeout: 10000,
        cache: false
    });
}

function setearId(id) {
    $("#idEntidad").val(id);
}

function eliminarEntidad(url, msjSuccess, msjError) {
    var id = $("#idEntidad").val();
    $.ajax({
        method: "GET",
        url: url + "?id=" + id,
        dataType: "json",
        error: function (text, error) {
            mensajeError("Ha ocurrido un error: " + text + error);
        },
        success: function (data) {
            if (data.success) {
                mensajeSuccess(msjSuccess);
                $("#tr_" + id).remove();
            } else {
                mensajeError(msjError + " Mensaje: " + data.mensaje);
            }
        },
        timeout: 10000,
        cache: false
    });
}

