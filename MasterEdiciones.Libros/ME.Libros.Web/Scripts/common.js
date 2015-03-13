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


function guardarEntidad(msjSuccess) {
    var formUrl = $(".formEntidad").attr("action");
    $.ajax({
        method: "POST",
        url: formUrl,
        data: $(".formEntidad").serialize(),
        dataType: "json",
        error: function (text, error) {
            mensajeError("Ha ocurrido un error: " + text + error);
        },
        success: function (data) {
            if (data.success) {
                $("div .errorCustom").hide();
                mensajeSuccess(msjSuccess);
                var urlListado = formUrl.substring(0, formUrl.lastIndexOf("/") + 1);
                //cargarPestaña(urlListado);
                window.location.href = urlListado;
            } else {
                $("div .errorCustom > ul").empty();
                $.each(data.mensajes, function (key, value) {
                    $("div .errorCustom > ul").append("<li><b>" + key + "</b>: " + value + "</li>");
                });
                $("div .errorCustom").show();
            }
        },
        timeout: 10000,
        cache: false
    });
}
