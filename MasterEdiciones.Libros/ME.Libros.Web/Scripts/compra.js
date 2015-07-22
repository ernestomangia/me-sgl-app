function abrirModalCompraItem(url, productoId) {
    var compraItemViewModels = new Array();
    var i = 0;
    $(".hiddenProductoId").each(function () {
        var preFix = "#Items\\[" + i + "\\]\\.";
        var item = {
            ProductoId: $(preFix + "ProductoId").val(),
            Cantidad: $(preFix + "Cantidad").val(),
            PrecioCosto: $(preFix + "PrecioCosto").val(),
            MontoItemComprado: $(preFix + "MontoItemComprado").val()
        };
        compraItemViewModels.push(item);
        i++;
    });

    $.ajax({
        method: "POST",
        url: url,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({ compraItemViewModels: compraItemViewModels, productoId: productoId }),
        dataType: "html",
        error: function (jqXhr, status, error) {
            mensajeError("Ha ocurrido un error");
        },
        success: function (data) {
            $("#contenedorCompraItem").html(data);
            $('#modalCompraItem').modal('toggle');
        },
        timeout: 10000,
        cache: false
    });
}

function crearCompraItem(form) {
    $(".modalCompraItem .validationSummary").addClass("hide");
    $(".modalCompraItem .validationSummary ul").remove();

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
                var compraItem = data.CompraItem;
                agregarCompraItem(compraItem);
                $('#modalCompraItem').modal('toggle');
                mensajeSuccess("Se agrego el " + compraItem.Producto.Nombre + " al detalle de la compra");
            } else {
                var errores = "<ul>";
                $.each(data.Errors, function (key, value) {
                    $.each(value.Value, function (key2, value2) {
                        errores += "<li>" + value2 + "</li>";
                    });
                });
                errores += "</ul>";
                $(".modalCompraItem .validationSummary").append(errores);
                $(".modalCompraItem .validationSummary").removeClass("hide");
            }
        },
        timeout: 10000,
        cache: false
    });
}

function agregarCompraItem(compraItem) {
    var indexItems = parseInt($("#cantidadItems").val());
    var modificar = getHtmlBotonModificar(compraItem.ProductoId);
    var eliminar = getHtmlBotonEliminar(indexItems);
    var table = $("#compraDetalleTable").DataTable();
    var nroItem = indexItems + 1;
    table.row.add([
            nroItem,
            compraItem.Producto.Nombre,
            compraItem.Cantidad,
            formatCurrency(compraItem.PrecioCosto),
            formatCurrency(compraItem.MontoItemComprado),
            modificar + " " + eliminar
    ]).draw();

    var hiddenProductoId = "<input type='hidden' id='Items[" + indexItems + "].ProductoId' class='hiddenProductoId' name='Items[" + indexItems + "].ProductoId' value='" + compraItem.ProductoId + "' />";
    var hiddenCantidad = "<input type='hidden'  id='Items[" + indexItems + "].Cantidad' class='hiddenCantidad' name='Items[" + indexItems + "].Cantidad' value='" + compraItem.Cantidad + "' />";
    var hiddenPrecioCosto = "<input type='hidden' id='Items[" + indexItems + "].PrecioCosto' class='hiddenPrecioCosto' name='Items[" + indexItems + "].PrecioCosto' value='" + formatFloat(compraItem.PrecioCosto) + "' />";
    var hiddenMontoItemComprado = "<input type='hidden' id='Items[" + indexItems + "].MontoItemComprado' class='hiddenMontoItemComprado' name='Items[" + indexItems + "].MontoItemComprado' value='" + formatFloat(compraItem.MontoItemComprado) + "' />";
    $("#formCompra").append(hiddenProductoId + hiddenCantidad + hiddenPrecioCosto + hiddenMontoItemComprado);
    $("#cantidadItems").val(indexItems + 1);
}

function getHtmlBotonModificar(productoId) {
    return "<button class='btn btn-warning btn-sm btnModificar' type='button' onclick='javascript:abrirModalCompraItem(\"/CompraItem/ModificarItem\", " + productoId + ");' title='Modificar item'>" +
        "<span class='glyphicon glyphicon-pencil' aria-hidden='true'></span>" +
        "</button>";
}

function getHtmlBotonEliminar(indexItem) {
    return "<button class='btn btn-danger btn-sm btnEliminar' type='button' data-toggle='modal' data-target='#modalEliminar' onclick='javascript:setearId(" + indexItem + ");' title='Eliminar item'>" +
        "<span class='glyphicon glyphicon-trash' aria-hidden='true'></span>" +
        "</button>";
}

function eliminarCompraItem(indexItem) {
    $("#Items\\[" + indexItem + "\\]\\.ProductoId").remove();
    $("#Items\\[" + indexItem + "\\]\\.Cantidad").remove();
    $("#Items\\[" + indexItem + "\\]\\.PrecioCosto").remove();
    $("#Items\\[" + indexItem + "\\]\\.MontoItemComprado").remove();
    var table = $("#compraDetalleTable").DataTable();
    table.row(indexItem).remove().draw();
    actualizarHiddens();
    $('#modalEliminar').modal('toggle');
    mensajeSuccess("Se elimino el item Nº " + (parseInt(indexItem) + 1) + " exitosamente");
}

function modificarCompraItem(indexItem) {
}

function actualizarHiddens() {
    var indexItem = 0;
    var table = $("#compraDetalleTable").DataTable();
    $(".hiddenProductoId").each(function () {
        table.cell(indexItem, 0).data(indexItem + 1).draw();
        var oldPreFix = "#Items\\[" + $(this).attr("id").substring($(this).attr("id").indexOf("[") + 1, $(this).attr("id").indexOf("]")) + "\\]\\.";

        var preFix = "Items[" + indexItem + "].";
        // ProductoId
        $(this).attr("id", preFix + "ProductoId");
        $(this).attr("name", preFix + "ProductoId");
        // Cantidad
        $(oldPreFix + "Cantidad").attr("id", preFix + "Cantidad").attr("name", preFix + "Cantidad");
        // PrecioCosto
        $(oldPreFix + "PrecioCosto").attr("id", preFix + "PrecioCosto").attr("name", preFix + "PrecioCosto");
        // MontoItemComprado 
        $(oldPreFix + "MontoItemComprado").attr("id", preFix + "MontoItemComprado").attr("name", preFix + "MontoItemComprado");
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
        $("#MontoComprado, #MontoCalculado").val(formatFloat(total));
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
                var precioCompra = parseFloat(data.PrecioCompra);
                calcularMontosItem(precioCompra);
                $("#precioSugerido").text(formatFloat(precioCompra));
                $("#PrecioCosto").val(formatFloat(precioCompra));
            },
            timeout: 10000,
            cache: false
        });
    } else {
        $("#PrecioCosto, #MontoItemComprado").val("");
        $("#precioSugerido").text("-");
    }
}

function formatFloat(value) {
    return Globalize.format(value, "n2");
}

function formatCurrency(value) {
    return "$ " + Globalize.format(value, "n2");
}

function calcularMontosItem(precioCompraCalculado, precioCosto) {
    if (precioCosto === undefined) {
        precioCosto = precioCompraCalculado;
    }
    calcularMontoItemComprado(precioCosto);
    calcularMontoItemCalculado(precioCompraCalculado);
    calcularDiferencia();
}

function calcularMontoItemComprado(precioCosto) {
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && precioCosto >= 0) {
        var monto = precioCosto * cantidad;
        $("#MontoItemComprado").val(formatFloat(monto));
    } else {
        $("#MontoItemComprado").val("");
    }
}

function calcularMontoItemCalculado(precioCompraCalculado) {
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && precioCompraCalculado >= 0) {
        var monto = precioCompraCalculado * cantidad;
        $("#subtotalSugerido").text(formatFloat(monto));
    } else {
        $("#subtotalSugerido").text("-");
    }
}

function calcularDiferencia() {
    var montoCalculado = Globalize.parseFloat($("#subtotalSugerido").text());
    var montoVendido = Globalize.parseFloat($("#MontoItemComprado").val());
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
        // Si el precio de compra del producto es 0 no calcular diferencia de montos
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