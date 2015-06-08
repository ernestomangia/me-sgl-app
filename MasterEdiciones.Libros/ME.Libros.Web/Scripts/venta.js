﻿function abrirModalVentaItem(url) {
    var ventaItemViewModels = new Array();
    $(".hiddenProductoId").each(function () {
        var item = {
            ProductoId: $(this).val(),
            Cantidad: $(this).next(".hiddenCantidad").val(),
            PrecioVentaVendido: $(this).next(".hiddenPrecioVentaVendido").val()
        };
        ventaItemViewModels.push(item);
    });

    $.ajax({
        method: "POST",
        url: url,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(ventaItemViewModels),
        dataType: "html",
        error: function (jqXhr, status, error) {
            mensajeError("Ha ocurrido un error");
        },
        success: function (data) {
            $("#contenedorVentaItem").html(data);
            $('#modalVentaItem').modal('toggle');
        },
        timeout: 10000,
        cache: false
    });
}

function crearVentaItem(form) {
    $(".modalVentaItem .validationSummary").addClass("hide");
    $(".modalVentaItem .validationSummary ul").remove();

    $.ajax({
        method: "POST",
        url: $(form).attr("action"),
        data: $(form).serialize(),
        dataType: "json",
        error: function (jqXhr, status, error) {
            mensajeError("Ha ocurrido un error");
        },
        success: function (data) {
            if (data.Success) {
                var ventaItem = data.VentaItem;
                agregarVentaItem(ventaItem);
                $('#modalVentaItem').modal('toggle');
                mensajeSuccess("Se agrego el " + ventaItem.Producto.Nombre + " al detalle de la venta");
            } else {
                var errores = "<ul>";
                $.each(data.Errors, function (key, value) {
                    $.each(value.Value, function (key2, value2) {
                        errores += "<li>" + value2 + "</li>";
                    });
                });
                errores += "</ul>";
                $(".modalVentaItem .validationSummary").append(errores);
                $(".modalVentaItem .validationSummary").removeClass("hide");
            }
        },
        timeout: 10000,
        cache: false
    });
}

function agregarVentaItem(ventaItem) {
    var indexItems = parseInt($("#cantidadItems").val());
    var modificar = getModificarButton(indexItems);
    var eliminar = getEliminarButton(indexItems);
    var table = $("#ventaDetalleTable").DataTable();
    var nroItem = indexItems + 1;
    table.row.add([
        nroItem,
        //0,
        ventaItem.Producto.Nombre,
        ventaItem.Cantidad,
        ventaItem.PrecioVentaVendido,
        ventaItem.MontoItemVendido,
        modificar + eliminar
    ]).draw();

    var hiddenProductoId = "<input type='hidden' id='Items[" + indexItems + "].ProductoId' class='hiddenProductoId' name='Items[" + indexItems + "].ProductoId' value='" + ventaItem.ProductoId + "' />";
    var hiddenCantidad = "<input type='hidden'  id='Items[" + indexItems + "].Cantidad' class='hiddenCantidad' name='Items[" + indexItems + "].Cantidad' value='" + ventaItem.Cantidad + "' />";
    var hiddenPrecioVentaVendido = "<input type='hidden' id='Items[" + indexItems + "].PrecioVentaVendido' class='hiddenPrecioVentaVendido' name='Items[" + indexItems + "].PrecioVentaVendido' value='" + ventaItem.PrecioVentaVendido + "' />";
    var hiddenMontoItemVendido = "<input type='hidden' id='Items[" + indexItems + "].MontoItemVendido' class='hiddenMontoItemVendido' name='Items[" + indexItems + "].MontoItemVendido' value='" + ventaItem.MontoItemVendido + "' />";
    $("#formVenta").append(hiddenProductoId + hiddenCantidad + hiddenPrecioVentaVendido + hiddenMontoItemVendido);
    $("#cantidadItems").val(indexItems + 1);
}

function getModificarButton(indexItem) {
    return ""; return "<button class='btn btn-warning btn-sm btnModificar' type='button' onclick='modificarVentaItem(" + indexItem + ");' title='Modificar'>" +
        "<span class='glyphicon glyphicon-pencil' aria-hidden='true'></span>" +
        "</button>";
}

function getEliminarButton(indexItem) {
    return "<button class='btn btn-danger btn-sm btnEliminar' type='button' data-toggle='modal' data-target='#modalEliminar' onclick='setearId(" + indexItem + ");'  title='Eliminar'>" +
        "<span class='glyphicon glyphicon-trash' aria-hidden='true'></span>" +
        "</button>";
}

function eliminarVentaItem(indexItem) {
    $("#Items\\[" + indexItem + "\\]\\.ProductoId").remove();
    $("#Items\\[" + indexItem + "\\]\\.Cantidad").remove();
    $("#Items\\[" + indexItem + "\\]\\.PrecioVentaVendido").remove();
    $("#Items\\[" + indexItem + "\\]\\.MontoItemVendido").remove();
    var table = $("#ventaDetalleTable").DataTable();
    table.row(indexItem).remove().draw();
    actualizarHiddens();
    $('#modalEliminar').modal('toggle');
    mensajeSuccess("Se elimino el item Nº " + (parseInt(indexItem) + 1) + " exitosamente");
}

function modificarVentaItem(indexItem) {
}

function actualizarHiddens() {
    var indexItem = 0;
    var table = $("#ventaDetalleTable").DataTable();
    $(".hiddenProductoId").each(function () {
        table.cell(indexItem, 0).data(indexItem + 1).draw();
        $(this).attr("id", "Items[" + indexItem + "].ProductoId");
        $(this).attr("name", "Items[" + indexItem + "].ProductoId");
        $(this).next(".hiddenCantidad").attr("id", "Items[" + indexItem + "].Cantidad");
        $(this).next(".hiddenCantidad").attr("name", "Items[" + indexItem + "].Cantidad");
        $(this).next(".hiddenPrecioVentaVendido").attr("id", "Items[" + indexItem + "].PrecioVentaVendido");
        $(this).next(".hiddenPrecioVentaVendido").attr("name", "Items[" + indexItem + "].PrecioVentaVendido");
        $(this).next(".hiddenMontoItemVendido").attr("id", "Items[" + indexItem + "].MontoItemVendido");
        $(this).next(".hiddenMontoItemVendido").attr("name", "Items[" + indexItem + "].MontoItemVendido");
        indexItem++;
    });

    $("#cantidadItems").val(indexItem);
}

function calcularTotales(dt) {
    var api = dt.api();

    if (api.column(4).data().length == 0) {
        return;
    }

    // Remove the formatting to get integer data for summation
    var intVal = function (i) {
        return typeof i === 'string' ?
            i.replace(/[\$,]/g, '') * 1 :
            typeof i === 'number' ?
            i : 0;
    };

    // Total over all pages
    total = api
        .column(4)
        .data()
        .reduce(function (a, b) {
            return intVal(a) + intVal(b);
        });

    // Total over this page
    pageTotal = api
        .column(4, { page: 'current' })
        .data()
        .reduce(function (a, b) {
            return intVal(a) + intVal(b);
        }, 0);

    // Update footer
    $(api.column(4).footer()).html(
        '$' + pageTotal + ' ( $' + total + ' total)'
    );
    $("#MontoVendido, #MontoCalculado").val(total);
}

function getProducto() {
    var idProducto = $("#ProductoId :selected").attr("value");
    if (idProducto > 0) {
        $.ajax({
            method: "GET",
            url: '/Producto/Get' + "?id=" + idProducto,
            dataType: "json",
            error: function (jqXHR, status, error) {
                mensajeError("Error: " + error + " - Status: " + status);
            },
            success: function (data) {
                var precioVenta = parseFloat(data.PrecioVenta);
                calcularMontoItem(precioVenta);
                $("#precioSugerido").text(formatFloat(precioVenta));
                $("#PrecioVentaVendido").val(formatFloat(precioVenta));
            },
            timeout: 10000,
            cache: false
        });
    } else {
        $("#PrecioVentaVendido, #MontoItemVendido").val("");
        $("#precioSugerido").text("-");
    }
}

function formatFloat(value) {
    return Globalize.format(value, "n2");
}

function calcularMontoItem(precioVentaVendido) {
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && precioVentaVendido >= 0) {
        var monto = precioVentaVendido * cantidad;
        $("#MontoItemVendido").val(formatFloat(monto));
    } else {
        $("#MontoItemVendido").val("");
    }
}

function isValidKey(keyCode) {
    return keyCode == undefined || // caso change event
        (keyCode >= 48 && keyCode <= 57) || // 0-9
        (keyCode >= 96 && keyCode <= 105) || // 0-9 numpad
        keyCode == 188 || // Tab
        keyCode == 8; // Back space
}