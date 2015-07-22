function abrirModalVentaItem(url, productoId) {
    var ventaItemViewModels = new Array();
    var i = 0;
    $(".hiddenProductoId").each(function () {
        var preFix = "#Items\\[" + i + "\\]\\.";
        var item = {
            ProductoId: $(preFix + "ProductoId").val(),
            Cantidad: $(preFix + "Cantidad").val(),
            PrecioVentaVendido: $(preFix + "PrecioVentaVendido").val(),
            MontoItemVendido: $(preFix + "MontoItemVendido").val()
        };
        ventaItemViewModels.push(item);
        i++;
    });

    $.ajax({
        method: "POST",
        url: url,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({ ventaItemViewModels: ventaItemViewModels, productoId: productoId }),
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
    var modificar = getHtmlBotonModificar(ventaItem.ProductoId);
    var eliminar = getHtmlBotonEliminar(indexItems);
    var table = $("#ventaDetalleTable").DataTable();
    var nroItem = indexItems + 1;
    table.row.add([
            nroItem,
            ventaItem.Producto.Nombre,
            ventaItem.Cantidad,
            formatCurrency(ventaItem.PrecioVentaVendido),
            formatCurrency(ventaItem.MontoItemVendido),
            modificar + " " + eliminar
    ]).draw();

    var hiddenProductoId = "<input type='hidden' id='Items[" + indexItems + "].ProductoId' class='hiddenProductoId' name='Items[" + indexItems + "].ProductoId' value='" + ventaItem.ProductoId + "' />";
    var hiddenCantidad = "<input type='hidden'  id='Items[" + indexItems + "].Cantidad' class='hiddenCantidad' name='Items[" + indexItems + "].Cantidad' value='" + ventaItem.Cantidad + "' />";
    var hiddenPrecioVentaVendido = "<input type='hidden' id='Items[" + indexItems + "].PrecioVentaVendido' class='hiddenPrecioVentaVendido' name='Items[" + indexItems + "].PrecioVentaVendido' value='" + formatFloat(ventaItem.PrecioVentaVendido) + "' />";
    var hiddenMontoItemVendido = "<input type='hidden' id='Items[" + indexItems + "].MontoItemVendido' class='hiddenMontoItemVendido' name='Items[" + indexItems + "].MontoItemVendido' value='" + formatFloat(ventaItem.MontoItemVendido) + "' />";
    $("#formVenta").append(hiddenProductoId + hiddenCantidad + hiddenPrecioVentaVendido + hiddenMontoItemVendido);
    $("#cantidadItems").val(indexItems + 1);
}

function getHtmlBotonModificar(productoId) {
    return "<button class='btn btn-warning btn-sm btnModificar' type='button' onclick='javascript:abrirModalVentaItem(\"/VentaItem/ModificarItem\", " + productoId + ");' title='Modificar item'>" +
        "<span class='glyphicon glyphicon-pencil' aria-hidden='true'></span>" +
        "</button>";
}

function getHtmlBotonEliminar(indexItem) {
    return "<button class='btn btn-danger btn-sm btnEliminar' type='button' data-toggle='modal' data-target='#modalEliminar' onclick='javascript:setearId(" + indexItem + ");' title='Eliminar item'>" +
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

function actualizarHiddens() {
    var indexItem = 0;
    var table = $("#ventaDetalleTable").DataTable();
    $(".hiddenProductoId").each(function () {
        table.cell(indexItem, 0).data(indexItem + 1).draw();
        var oldPreFix = "#Items\\[" + $(this).attr("id").substring($(this).attr("id").indexOf("[") + 1, $(this).attr("id").indexOf("]")) + "\\]\\.";

        var preFix = "Items[" + indexItem + "].";
        // ProductoId
        $(this).attr("id", preFix + "ProductoId");
        $(this).attr("name", preFix + "ProductoId");
        // Cantidad
        $(oldPreFix + "Cantidad").attr("id", preFix + "Cantidad").attr("name", preFix + "Cantidad");
        // PrecioVentaVendido
        $(oldPreFix + "PrecioVentaVendido").attr("id", preFix + "PrecioVentaVendido").attr("name", preFix + "PrecioVentaVendido");
        // MontoItemVendido 
        $(oldPreFix + "MontoItemVendido").attr("id", preFix + "MontoItemVendido").attr("name", preFix + "MontoItemVendido");
        indexItem++;
    });

    $("#cantidadItems").val(indexItem);

    indexItem = 0;
    $(".btnEliminar").each(function () {
        $(this).attr("onclick", "setearId(" + indexItem + ")");
    });
}

function calcularTotales(dt, row, data, start, end, display) {
    var api = dt.api();

    if (api.column(4).data().length == 0) {
        return;
    }
    // Remove the formatting to get float data for summation
    var floatVal = function (i) {
        return typeof i === 'string'
            ? Globalize.parseFloat(i)
            : typeof i === 'number'
                ? i
                : 0;
    };

    // Total over all pages
    var total = api
        .column(4)
        .data()
        .reduce(function (a, b) {
            return floatVal(a) + floatVal(b);
        });

    // Total over this page
    var pageTotal = api
        .column(4, { page: 'current' })
        .data()
        .reduce(function (a, b) {
            return floatVal(a) + floatVal(b);
        });

    if (api.column(4).data().length == 1) {
        total = floatVal(total);
        pageTotal = floatVal(pageTotal);
    }

    // Update footer
    $(api.column(4).footer()).html(formatCurrency(pageTotal) + ' (' + formatCurrency(total) + ' total)');
    if ($("#Id").length == 0) {
        $("#MontoVendido, #MontoCalculado").val(formatFloat(total));
    }
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
                calcularMontosItem(precioVenta);
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

function formatCurrency(value) {
    return "$ " + Globalize.format(value, "n2");
}

function calcularMontosItem(precioVentaCalculado, precioVentaVendido) {
    if (precioVentaVendido === undefined) {
        precioVentaVendido = precioVentaCalculado;
    }
    calcularMontoItemVendido(precioVentaVendido);
    calcularMontoItemCalculado(precioVentaCalculado);
    calcularDiferencia();
}

function calcularMontoItemVendido(precioVentaVendido) {
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && precioVentaVendido >= 0) {
        var monto = precioVentaVendido * cantidad;
        $("#MontoItemVendido").val(formatFloat(monto));
    } else {
        $("#MontoItemVendido").val("");
    }
}

function calcularMontoItemCalculado(precioVentaCalculado) {
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && precioVentaCalculado >= 0) {
        var monto = precioVentaCalculado * cantidad;
        $("#subtotalSugerido").text(formatFloat(monto));
    } else {
        $("#subtotalSugerido").text("-");
    }
}

function calcularDiferencia() {
    var montoCalculado = Globalize.parseFloat($("#subtotalSugerido").text());
    var montoVendido = Globalize.parseFloat($("#MontoItemVendido").val());
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && montoCalculado > 0 && montoVendido >= 0) {
        var difAbsoluta = montoVendido - montoCalculado;
        var difRelativaPorcentual = (difAbsoluta / montoCalculado) * 100;
        if (difAbsoluta < 0) {
            $("#diferencia").parent().switchClass("label-success label-default", "label-danger");
        } else if (difAbsoluta > 0) {
            $("#diferencia").parent().switchClass("label-danger label-default", "label-success");
        } else {
            $("#diferencia").parent().switchClass("label-danger label-success", "label-default");
        }
        $("#diferencia").text(formatFloat(difAbsoluta) + " | " + formatFloat(difRelativaPorcentual) + "%");
    } else {
        // Si el precio de venta del producto es 0 no calcular diferencia de montos
        $("#diferencia").text("-");
        $("#diferencia").parent().switchClass("label-success label-danger", "label-default");
    }
}

function isValidKey(keyCode) {
    return keyCode == undefined || // caso change event
        (keyCode >= 48 && keyCode <= 57) || // 0-9
        (keyCode >= 96 && keyCode <= 105) || // 0-9 numpad
        keyCode == 188 || // Tab
        keyCode == 8; // Back space
}