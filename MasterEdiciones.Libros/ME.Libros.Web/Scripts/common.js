$(document).ajaxSend(function () {
    showAjaxLoader();
});

$(document).ajaxComplete(function () {
    hideAjaxLoader();
});

$(window).bind('beforeunload', function () {
    showAjaxLoader();
});

function showAjaxLoader() {
    $('#loading-indicator').show();
}

function hideAjaxLoader() {
    $('#loading-indicator').hide();
}

function mensajeSuccess(text) {
    toastr.success(text);
}

function mensajeError(text) {
    toastr.error(text);
}

function cargarPestaña(url) {
    cargarVistaParcial(url, "tabContenedor", false);
    //window.location.href = "http://localhost:24086/" + url;
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
        error: function (jqXhr, status, error) {
            mensajeError("Ha ocurrido un error");
        },
        success: function (data) {
            if (data.Success) {
                mensajeSuccess(msjSuccess);
                var dataTable = $(".dataTableCustom").DataTable();
                dataTable.row($("#tr_" + id)).remove().draw();
            } else {
                var errores = "";
                $.each(data.Errors, function (key, value) {
                    errores += key + ": " + value;
                });
                mensajeError(msjError + " Mensaje: " + errores);
            }
        },
        timeout: 10000,
        cache: false
    });
}


