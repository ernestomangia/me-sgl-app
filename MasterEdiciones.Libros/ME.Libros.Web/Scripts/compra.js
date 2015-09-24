function abrirModalCompraItem(url, productoId) {
    var compraItemViewModels = new Array();
    var i = 0;
    $(".hiddenProductoId").each(function () {
        var preFix = "#Items\\[" + i + "\\]\\.";
        var item = {
            ProductoId: $(preFix + "ProductoId").val(),
            Cantidad: $(preFix + "Cantidad").val(),
            PrecioCompraComprado: $(preFix + "PrecioCompraComprado").val(),
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
                mensajeSuccess("Se agrego el producto " + compraItem.Producto.Nombre + " al detalle de la compra");
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
            formatCurrency(compraItem.PrecioCompraComprado),
            formatCurrency(compraItem.MontoItemComprado),
            modificar + " " + eliminar
    ]).draw();

    var hiddenProductoId = "<input type='hidden' id='Items[" + indexItems + "].ProductoId' class='hiddenProductoId' name='Items[" + indexItems + "].ProductoId' value='" + compraItem.ProductoId + "' />";
    var hiddenCantidad = "<input type='hidden'  id='Items[" + indexItems + "].Cantidad' class='hiddenCantidad' name='Items[" + indexItems + "].Cantidad' value='" + compraItem.Cantidad + "' />";
    var hiddenPrecioCompraComprado = "<input type='hidden' id='Items[" + indexItems + "].PrecioCompraComprado' class='hiddenPrecioCompraComprado' name='Items[" + indexItems + "].PrecioCompraComprado' value='" + formatFloat(compraItem.PrecioCompraComprado) + "' />";
    var hiddenMontoItemComprado = "<input type='hidden' id='Items[" + indexItems + "].MontoItemComprado' class='hiddenMontoItemComprado' name='Items[" + indexItems + "].MontoItemComprado' value='" + formatFloat(compraItem.MontoItemComprado) + "' />";
    $("#formCompra").append(hiddenProductoId + hiddenCantidad + hiddenPrecioCompraComprado + hiddenMontoItemComprado);
    $("#cantidadItems").val(indexItems + 1);
}

function getHtmlBotonModificar(productoId) {
    return "<button class='btn btn-warning btn-sm btnModificar' type='button' onclick='javascript:abrirModalCompraItem(\"/CompraItem/ModificarItem\", " + productoId + ");' title='Modificar item'>" +
        "<span class='glyphicon glyphicon-pencil' aria-hidden='true'></span>" +
        "</button>";
}

function getHtmlBotonEliminar(indexItem) {
    return "<button class='btn btn-danger btn-sm btnEliminar' type='button' data-toggle='modal' data-target='#modalEliminarCompraItem' onclick='javascript:setearIdCompraItem(" + indexItem + ");' title='Eliminar item'>" +
        "<span class='glyphicon glyphicon-trash' aria-hidden='true'></span>" +
        "</button>";
}

function eliminarCompraItem(indexItem) {
    $("#Items\\[" + indexItem + "\\]\\.ProductoId").remove();
    $("#Items\\[" + indexItem + "\\]\\.Cantidad").remove();
    $("#Items\\[" + indexItem + "\\]\\.PrecioCompraComprado").remove();
    $("#Items\\[" + indexItem + "\\]\\.MontoItemComprado").remove();
    var table = $("#compraDetalleTable").DataTable();
    table.row(indexItem).remove().draw();
    actualizarHiddens();
    $('#modalEliminarCompraItem').modal('toggle');
    mensajeSuccess("Se elimino el item Nº " + (parseInt(indexItem) + 1) + " exitosamente");
}

function modificarCompraItem(form) {
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
                actualizarCompraItemRow(compraItem);
                $('#modalCompraItem').modal('toggle');
                mensajeSuccess("Se modifico el producto " + compraItem.Producto.Nombre + " exitosamente");
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
        // PrecioCompraComprado
        $(oldPreFix + "PrecioCompraComprado").attr("id", preFix + "PrecioCompraComprado").attr("name", preFix + "PrecioCompraComprado");
        // MontoItemComprado 
        $(oldPreFix + "MontoItemComprado").attr("id", preFix + "MontoItemComprado").attr("name", preFix + "MontoItemComprado");
        indexItem++;
    });

    $("#cantidadItems").val(indexItem);

    indexItem = 0;
    $(".btnEliminar").each(function () {
        $(this).attr("onclick", "setearIdCompraItem(" +indexItem + ")");
    });
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
                var precioCompra = parseFloat(data.PrecioCosto);
                calcularMontosItem(precioCompra);
                $("#precioSugerido").text(formatFloat(precioCompra));
                $("#PrecioCompraComprado").val(formatFloat(precioCompra));
            },
            timeout: 10000,
            cache: false
        });
    } else {
        $("#PrecioCompraComprado, #MontoItemComprado").val("");
        $("#precioSugerido").text("-");
    }
}

function calcularMontosItem(precioCompraCalculado, precioCompraComprado) {
    if (precioCompraComprado === undefined) {
        precioCompraComprado = precioCompraCalculado;
    }
    calcularMontoItemComprado(precioCompraComprado);
    calcularMontoItemCalculado(precioCompraCalculado);
    calcularDiferencia();
}

function calcularMontoItemComprado(precioCompraComprado) {
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && precioCompraComprado >= 0) {
        var monto = precioCompraComprado * cantidad;
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
    var montComprado = Globalize.parseFloat($("#MontoItemComprado").val());
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && montoCalculado > 0 && montComprado >= 0) {
        var difAbsoluta = montComprado - montoCalculado;
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

function actualizarCompraItemRow(compraItem) {
    var hiddenProductoId = $(".hiddenProductoId[value=" + compraItem.ProductoId + "]");
    var subtringStart = hiddenProductoId.attr("id").indexOf("[") + 1;
    var subtringEnd = hiddenProductoId.attr("id").indexOf("]");
    var indexItem = parseInt(hiddenProductoId.attr("id").substring(subtringStart, subtringEnd));

    // Actualizar hiddens del  item modificado
    $("#Items\\[" + indexItem + "\\]\\.Cantidad").val(compraItem.Cantidad);
    $("#Items\\[" + indexItem + "\\]\\.PrecioCompraComprado").val(formatFloat(compraItem.PrecioCompraComprado));
    $("#Items\\[" + indexItem + "\\]\\.MontoItemComprado").val(formatFloat(compraItem.MontoItemComprado));

    // Actualizar la fila del DataTable correspondiente al item
    var table = $("#compraDetalleTable").DataTable();
    table.cell(indexItem, 2)
        .data(compraItem.Cantidad);
    table.cell(indexItem, 3)
        .data(formatCurrency(compraItem.PrecioCompraComprado));
    table.cell(indexItem, 4)
        .data(formatCurrency(compraItem.MontoItemComprado));

    // Redibujar para recalcular footer
    table.draw();
}

function loadCompraDetalleDataTable() {
    $("#compraDetalleTable").addClass("table table-striped table-hover hover table-bordered order-column KeyTable");
    $('#compraDetalleTable').dataTable({
        "order": [],
        "footerCallback": function (row, data, start, end, display) {
            var totales = calcularTotalColumna(this, [4]);
            if ($("#Id").length == 0) {
                $("#MontoComprado, #MontoCalculado").val(formatFloat(totales[0].total));
            }
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(2),td:eq(3),td:eq(4)', nRow).addClass("text-right");
        },
        lengthMenu: [5, 10]
    });
}