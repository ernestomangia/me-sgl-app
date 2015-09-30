function abrirModalCompraItem(url, itemIndex) {
    var compraItemViewModels = new Array();
    var i = 0;
    $(".hiddenProductoId").each(function () {
        var preFix = "#Items\\[" + i + "\\]\\.";
        var item = {
            Orden: i + 1,
            ProductoId: $(preFix + "ProductoId").val(),
            Cantidad: $(preFix + "Cantidad").val(),
            PrecioCostoComprado: $(preFix + "PrecioCostoComprado").val(),
            MontoItemComprado: $(preFix + "MontoItemComprado").val()
        };
        compraItemViewModels.push(item);
        i++;
    });

    $.ajax({
        method: "POST",
        url: url,
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({ compraItemViewModels: compraItemViewModels, itemIndex: itemIndex }),
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
                mensajeSuccess("Se agregó el producto \"" + compraItem.Producto.Nombre + "\" al detalle de la compra");
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
    var itemIndex = parseInt($("#cantidadItems").val());
    var modificar = getHtmlBotonModificar(itemIndex);
    var eliminar = getHtmlBotonEliminar(itemIndex);
    var table = $("#compraDetalleTable").DataTable();
    var nroItem = itemIndex + 1;
    table.row.add([
            nroItem,
            compraItem.Producto.Nombre,
            compraItem.Cantidad,
            formatCurrency(compraItem.PrecioCostoComprado),
            formatCurrency(compraItem.MontoItemComprado),
            modificar + " " + eliminar
    ]).draw();

    var hiddenProductoId = "<input type='hidden' id='Items[" + itemIndex + "].ProductoId' class='hiddenProductoId' name='Items[" + itemIndex + "].ProductoId' value='" + compraItem.ProductoId + "' />";
    var hiddenCantidad = "<input type='hidden'  id='Items[" + itemIndex + "].Cantidad' class='hiddenCantidad' name='Items[" + itemIndex + "].Cantidad' value='" + compraItem.Cantidad + "' />";
    var hiddenPrecioCostoComprado = "<input type='hidden' id='Items[" + itemIndex + "].PrecioCostoComprado' class='hiddenPrecioCostoComprado' name='Items[" + itemIndex + "].PrecioCostoComprado' value='" + formatFloat(compraItem.PrecioCostoComprado) + "' />";
    var hiddenMontoItemComprado = "<input type='hidden' id='Items[" + itemIndex + "].MontoItemComprado' class='hiddenMontoItemComprado' name='Items[" + itemIndex + "].MontoItemComprado' value='" + formatFloat(compraItem.MontoItemComprado) + "' />";
    $("#formCompra").append(hiddenProductoId + hiddenCantidad + hiddenPrecioCostoComprado + hiddenMontoItemComprado);
    $("#cantidadItems").val(itemIndex + 1);
}

function getHtmlBotonModificar(itemIndex) {
    return "<button class='btn btn-warning btn-sm btnModificar' type='button' onclick='javascript:abrirModalCompraItem(\"/CompraItem/ModificarItem\", " + itemIndex + ");' title='Modificar item'>" +
        "<span class='glyphicon glyphicon-pencil' aria-hidden='true'></span>" +
        "</button>";
}

function getHtmlBotonEliminar(itemIndex) {
    return "<button class='btn btn-danger btn-sm btnEliminar' type='button' data-toggle='modal' data-target='#modalEliminarCompraItem' onclick='javascript:setearIdCompraItem(" + itemIndex + ");' title='Eliminar item'>" +
        "<span class='glyphicon glyphicon-trash' aria-hidden='true'></span>" +
        "</button>";
}

function eliminarCompraItem(itemIndex) {
    $("#Items\\[" + itemIndex + "\\]\\.ProductoId").remove();
    $("#Items\\[" + itemIndex + "\\]\\.Cantidad").remove();
    $("#Items\\[" + itemIndex + "\\]\\.PrecioCostoComprado").remove();
    $("#Items\\[" + itemIndex + "\\]\\.MontoItemComprado").remove();
    var table = $("#compraDetalleTable").DataTable();
    table.row(itemIndex).remove().draw();
    actualizarHiddens();
    $('#modalEliminarCompraItem').modal('toggle');
    mensajeSuccess("Se eliminó el item Nº " + (parseInt(itemIndex) + 1) + " exitosamente");
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
                mensajeSuccess("Se modificó el item Nº " + compraItem.Orden + " exitosamente");
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
    var itemIndex = 0;
    var table = $("#compraDetalleTable").DataTable();
    $(".hiddenProductoId").each(function () {
        table.cell(itemIndex, 0).data(itemIndex + 1).draw();
        var oldPreFix = "#Items\\[" + $(this).attr("id").substring($(this).attr("id").indexOf("[") + 1, $(this).attr("id").indexOf("]")) + "\\]\\.";

        var preFix = "Items[" + itemIndex + "].";
        // ProductoId
        $(this).attr("id", preFix + "ProductoId");
        $(this).attr("name", preFix + "ProductoId");
        // Cantidad
        $(oldPreFix + "Cantidad").attr("id", preFix + "Cantidad").attr("name", preFix + "Cantidad");
        // PrecioCostoComprado
        $(oldPreFix + "PrecioCostoComprado").attr("id", preFix + "PrecioCostoComprado").attr("name", preFix + "PrecioCostoComprado");
        // MontoItemComprado 
        $(oldPreFix + "MontoItemComprado").attr("id", preFix + "MontoItemComprado").attr("name", preFix + "MontoItemComprado");
        itemIndex++;
    });

    $("#cantidadItems").val(itemIndex);

    itemIndex = 0;
    $(".btnEliminar").each(function () {
        $(this).attr("onclick", "setearIdCompraItem(" +itemIndex + ")");
    });

    itemIndex = 0;
    $(".btnModificar").each(function () {
        $(this).attr("onclick", "abrirModalCompraItem(\"/VentaItem/ModificarItem\"," + itemIndex + ")");
    });
}

function calcularMontoItemComprado(precioCostoComprado) {
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && precioCostoComprado >= 0) {
        var monto = precioCostoComprado * cantidad;
        $("#MontoItemComprado").val(formatFloat(monto));
    } else {
        $("#MontoItemComprado").val("");
    }
}

function actualizarCompraItemRow(compraItem) {
    var itemIndex = compraItem.Orden - 1;

    // Actualizar hiddens del  item modificado
    $("#Items\\[" + itemIndex + "\\]\\.ProductoId").val(compraItem.ProductoId);
    $("#Items\\[" + itemIndex + "\\]\\.Cantidad").val(compraItem.Cantidad);
    $("#Items\\[" + itemIndex + "\\]\\.PrecioCostoComprado").val(formatFloat(compraItem.PrecioCostoComprado));
    $("#Items\\[" + itemIndex + "\\]\\.MontoItemComprado").val(formatFloat(compraItem.MontoItemComprado));

    // Actualizar la fila del DataTable correspondiente al item
    var table = $("#compraDetalleTable").DataTable();
    table.cell(itemIndex, 1)
        .data(compraItem.Producto.Nombre);
    table.cell(itemIndex, 2)
        .data(compraItem.Cantidad);
    table.cell(itemIndex, 3)
        .data(formatCurrency(compraItem.PrecioCostoComprado));
    table.cell(itemIndex, 4)
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

/* AJAX Calls */
function getProducto(url) {
    var idProducto = $("#ProductoId :selected").attr("value");
    if (idProducto > 0) {
        var request = getProductoRequest(url, idProducto);
        request.done(function (data) {
            $("#CodigoBarra").val(data.CodigoBarra);
            var precioCosto = parseFloat(data.PrecioCosto);
            calcularMontoItemComprado(precioCosto);
            $("#precioCostoAnterior").text(formatFloat(precioCosto));
            $("#PrecioCostoComprado").val(formatFloat(precioCosto));
        });
    } else {
        $("#PrecioCostoComprado, #MontoItemComprado").val("");
        $("#precioCostoAnterior").text("-");
    }
}

function getProductoByCodigoBarra(url, codigoBarra) {
    if (isCodigoBarraValid(codigoBarra)) {
        var getProductoRequest = getProductoByCodigoBarraRequest(url, codigoBarra);
        getProductoRequest.done(function (data) {
            if (data.Id > 0) {
                $("#ProductoId").val(data.Id);
                if ($("#ProductoId").val() == undefined) {
                    mensajeWarn("El producto ingresado ya se encuentra en el detalle de la compra");
                    $("#CodigoBarra").focus().select();
                } else {
                    $("#ProductoId").trigger("change");
                    $("#Cantidad").focus();
                }
            } else {
                mensajeWarn("Código de barra inexistente");
                $("#CodigoBarra").focus().select();
            }
        });
    } else {
        $("#PrecioCostoComprado, #MontoItemComprado").val("");
        $("#precioCostoAnterior").text("-");
    }
}