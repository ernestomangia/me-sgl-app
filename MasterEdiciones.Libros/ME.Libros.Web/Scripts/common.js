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
    $(".modalEliminar .validationSummary").addClass("hide");
    $(".modalEliminar .validationSummary ul").remove();

    $.ajax({
        method: "GET",
        url: url + "?id=" + id,
        dataType: "json",
        error: function (jqXhr, status, error) {
            mensajeError("Ha ocurrido un error");
        },
        success: function (data) {
            if (data.Success) {
                var dataTable = $(".dataTableCustom").DataTable();
                $('.modalEliminar').modal('toggle');
                mensajeSuccess(msjSuccess);
                dataTable.row($("#tr_" + id)).remove().draw();

                $(".btnCancelarEliminar").click();
            } else {
                var errores = "<ul>";
                $.each(data.Errors, function (key, value) {
                    $.each(value.Value, function (key2, value2) {
                        errores += "<li>" + value2 + "</li>";
                    });
                });
                errores += "</ul>";
                $(".modalEliminar .validationSummary").append(errores);
                $(".modalEliminar .validationSummary").removeClass("hide");
            }
        },
        timeout: 10000,
        cache: false
    });
}

function AnularEntidad(url, msjSuccess, msjError) {
     var id = $("#idEntidad").val();
    $(".modalAnular .validationSummary").addClass("hide");
    $(".modalAnular .validationSummary ul").remove();

    $.ajax({
        method: "GET",
        url: url + "?id=" + id,
        dataType: "json",
        error: function (jqXhr, status, error) {
            mensajeError("Ha ocurrido un error");
        },
        success: function (data) {
            if (data.Success) {
                var dataTable = $(".dataTableCustom").DataTable();
                $('.modalEliminar').modal('toggle');
                mensajeSuccess(msjSuccess);
                console.log($("#tr_" + id + " .CambioEstado"));
                dataTable.cell($("#tr_" + id),(6)).data('Anulado').draw();
               // dataTable.cell(10,5).data('Anulado').draw();
                $(".btnCancelarEliminar").click();
            } else {
                var errores = "<ul>";
                $.each(data.Errors, function (key, value) {
                    $.each(value.Value, function (key2, value2) {
                        errores += "<li>" + value2 + "</li>";
                    });
                });
                errores += "</ul>";
                $(".modalEliminar .validationSummary").append(errores);
                $(".modalEliminar .validationSummary").removeClass("hide");
            }
        },
        timeout: 10000,
        cache: false
    });

}

function FormatJsonDate(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    return dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
}

function validationSummaryVisibility(form) {
    if ($(form).valid()) {
        $(form).find(".validationSummary").addClass("hide");
    } else {
        $(form).find(".validationSummary").removeClass("hide");
    }
}
