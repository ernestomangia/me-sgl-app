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
                mensajeSuccess("Se agrego el producto " + ventaItem.Producto.Nombre + " al detalle de la venta");
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
    return "<button class='btn btn-danger btn-sm btnEliminar' type='button' data-toggle='modal' data-target='#modalEliminarVentaItem' onclick='javascript:setearIdVentaItem(" + indexItem + ");' title='Eliminar item'>" +
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
    $('#modalEliminarVentaItem').modal('toggle');
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
        $(this).attr("onclick", "setearIdVentaItem(" + indexItem + ")");
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
                $("#CodigoBarra").val(data.CodigoBarra);
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

function modificarVentaItem(form) {
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
                actualizarVentaItemRow(ventaItem);
                $('#modalVentaItem').modal('toggle');
                mensajeSuccess("Se modifico el producto " + ventaItem.Producto.Nombre + " exitosamente");
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

function actualizarVentaItemRow(ventaItem) {
    var hiddenProductoId = $(".hiddenProductoId[value=" + ventaItem.ProductoId + "]");
    var subtringStart = hiddenProductoId.attr("id").indexOf("[") + 1;
    var subtringEnd = hiddenProductoId.attr("id").indexOf("]");
    var indexItem = parseInt(hiddenProductoId.attr("id").substring(subtringStart, subtringEnd));

    // Actualizar hiddens del  item modificado
    $("#Items\\[" + indexItem + "\\]\\.Cantidad").val(ventaItem.Cantidad);
    $("#Items\\[" + indexItem + "\\]\\.PrecioVentaVendido").val(formatFloat(ventaItem.PrecioVentaVendido));
    $("#Items\\[" + indexItem + "\\]\\.MontoItemVendido").val(formatFloat(ventaItem.MontoItemVendido));

    // Actualizar la fila del DataTable correspondiente al item
    var table = $("#ventaDetalleTable").DataTable();
    table.cell(indexItem, 2)
        .data(ventaItem.Cantidad);
    table.cell(indexItem, 3)
        .data(formatCurrency(ventaItem.PrecioVentaVendido));
    table.cell(indexItem, 4)
        .data(formatCurrency(ventaItem.MontoItemVendido));

    // Redibujar para recalcular footer
    table.draw();
}

$(document).on('click', '#ventaCuotaTable tbody tr td button.details-control', function () {
    var dt = $('#ventaCuotaTable').DataTable();
    var tr = $(this).closest('tr');
    var row = dt.row(tr);
    var detailsControl = $(this);
    var icon = detailsControl.find("span");

    if (row.child.isShown()) {
        // This row is already open - close it
        $('div.slider', row.child()).slideUp(function () {
            row.child.hide();
            tr.removeClass('shown');
            $(detailsControl).trigger("focusout");
        });

        detailsControl.toggleClass("btn-info").toggleClass("btn-default");
        icon.toggleClass("glyphicon-collapse-up").toggleClass("glyphicon-collapse-down");
    }
    else {
        // Open this row
        var promise = GetCobrosByCuota(row.data());
        promise.done(function (data) {
            detailsControl.toggleClass("btn-default").toggleClass("btn-info");
            icon.toggleClass("glyphicon-collapse-down").toggleClass("glyphicon-collapse-up");
            row.child(data, 'no-padding').show();
            tr.addClass('shown');
            $('div.slider', row.child()).slideDown();
        });
    }
});

function GetCobrosByCuota(cuota, tr) {
    return $.ajax({
        method: "POST",
        url: "/Cobro/VerCobros",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({ cuotaId: cuota.cuotaId }),
        dataType: "html",
        error: function (jqXhr, status, error) {
            mensajeError("Ha ocurrido un error");
        },
        timeout: 10000,
        cache: false
    });
}

function loadVentaDetalleDataTable() {
    $("#ventaDetalleTable").addClass("table table-striped table-hover hover table-bordered order-column KeyTable");
    $('#ventaDetalleTable').dataTable({
        "order": [],
        "footerCallback": function (row, data, start, end, display) {
            var totales = calcularTotalColumna(this, [4]);
            if ($("#Id").length == 0) {
                $("#MontoVendido, #MontoCalculado").val(formatFloat(totales[0].total));
            }
        },
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(2),td:eq(3),td:eq(4)', nRow).addClass("text-right");
        },
        lengthMenu: [5, 10]
    });
}

function loadCuotasDataTable() {
    $("#ventaCuotaTable").addClass("table table-striped table-hover table-bordered order-column KeyTable");
    $('#ventaCuotaTable').dataTable({
        "columns": [
            {
                "data": "cuotaId",
                "visible": false,
                "searchable": false
            },
            {
                "orderable": false
            },
            { "data": "nro" },
            { "data": "monto" },
            { "data": "vencim" },
            { "data": "estado" },
            { "data": "interes" },
            { "data": "cobrado" },
            { "data": "fechaCobro" },
            { "data": "atraso" },
            { "data": "saldo" }
        ]
    });
}