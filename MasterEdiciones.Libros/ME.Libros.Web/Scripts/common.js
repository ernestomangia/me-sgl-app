/* Ajax Loader */
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

/* Toastr messages */
function mensajeSuccess(text) {
    toastr.success(text);
}

function mensajeError(text) {
    toastr.error(text);
}

function mensajeWarn(text) {
    toastr.warning(text);
}

/* Format functions */
function formatFloat(value) {
    return Globalize.format(value, "N2");
}

function formatCurrency(value) {
    return Globalize.format(value, "C");
}

function formatToShortDate(value) {
    return Globalize.format(value, "d");
}

function ConvertJsonDateToDate(value) {
    return new Date(parseInt(value.substr(6)));
}

function validationSummaryVisibility(form) {
    if ($(form).valid()) {
        $(form).find(".validationSummary").addClass("hide");
    } else {
        $(form).find(".validationSummary").removeClass("hide");
    }
}

function setMaxlength() {
    $("input[data-val-length-max]").each(function () {
        $(this).attr("maxlength", $(this).data().valLengthMax);
    });
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

/* AJAX Requests */
function getProductoByCodigoBarraRequest(url, codigoBarra) {
    return $.ajax({
        method: "GET",
        url: url,
        data: { codigoBarra: codigoBarra },
        dataType: "json",
        error: function (jqXhr, status, error) {
            mensajeError("Error: " + error + " - Status: " + status);
        },
        timeout: 10000,
        cache: false
    });
};

function getProductoRequest(url, id) {
    return $.ajax({
        method: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        error: function (jqXHR, status, error) {
            mensajeError("Error: " + error + " - Status: " + status);
        },
        timeout: 10000,
        cache: false
    });
};

/* */
function eliminarEntidad(url, msjSuccess, msjError) {
    var id = $("#idEntidad").val();
    var redirectUrl = $(".btnCancelar").attr("href");

    $(".modalEliminar .validationSummary").addClass("hide");
    $(".modalEliminar .validationSummary ul").remove();
    $.ajax({
        method: "GET",
        url: url,
        data: { "id": id, "redirectUrl": redirectUrl },
        dataType: "json",
        error: function (jqXhr, status, error) {
            mensajeError(msjError);
        },
        success: function (data) {
            if (data.Success) {
                if (data.isRedirect) {
                    window.location.href = data.redirectUrl;
                } else {
                    var dataTable = $(".dataTableCustom").DataTable();
                    $('.modalEliminar').modal('toggle');
                    mensajeSuccess(msjSuccess);
                    dataTable.row($("#tr_" + id)).remove().draw();
                }
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
                $('.modalAnular').modal('toggle');
                mensajeSuccess(msjSuccess);
                console.log($("#tr_" + id + " .CambioEstado"));
                dataTable.cell($("#tr_" + id), (6)).data('Anulado').draw();
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
                $(".modalAnular .validationSummary").append(errores);
                $(".modalAnular .validationSummary").removeClass("hide");
            }
        },
        timeout: 10000,
        cache: false
    });
}

function GetLocalidades() {
    var idProvincia = $(".provincia :selected").attr("value");
    if (idProvincia > 0) {
        $.ajax({
            method: "GET",
            url: "/Localidad/ListarLocalidades",
            data: "id=" + idProvincia,
            dataType: "json",
            error: function (jqXHR, status, error) {
                mensajeError("Error: " + error + " - Status: " + status);
            },
            success: function (data) {
                $(".localidad").empty();
                if (data.length > 0) {
                    $.each(data, function (key, value) {
                        $(".localidad").append(new Option(value.Nombre, value.Id));
                    });
                } else {
                    $(".localidad").append(new Option("Seleccione", ""));
                }
            },
            timeout: 10000,
            cache: false
        });
    } else {
        $(".localidad").empty().append(new Option("Seleccione", ""));
    }
}

// Handle tabs links
$(function () {
    var hash = window.location.hash;
    hash && $('ul.nav a[href="' + hash + '"]').tab('show');

    $('.nav-tabs a').click(function (e) {
        $(this).tab('show');
        var scrollmem = $('body').scrollTop();
        window.location.hash = this.hash;
        $('html,body').scrollTop(scrollmem);
    });
});

/* Validation functions */
// Handle key codes
function isValidKeyForCalc(keyCode) {
    return isChangeEvent(keyCode) ||
        isNumberKey(keyCode) ||
        isTabKey(keyCode) ||
        isBackSpaceKey(keyCode) || 
        isDeleteKey(keyCode);
}

function isChangeEvent(keyCode) {
    return keyCode == undefined;
}

function isTabKey(keyCode) {
    return keyCode == 188;
}

function isBackSpaceKey(keyCode) {
    return keyCode == 8;
}

function isNumberKey(keyCode) {
    return (keyCode >= 48 && keyCode <= 57) ||
        (keyCode >= 96 && keyCode <= 105); // Numpad
}

function isDeleteKey(keyCode) {
    return keyCode == 46;
}

// Codigo Barra
function isCodigoBarraValid(value) {
    return value != undefined && value.length == 13;
}

function calcularMontoComision(montoFacturado, comision) {
    return montoFacturado * comision / 100;
}

function calcularComision(montoFacturado, montoComision) {
    return montoComision / montoFacturado * 100;
}