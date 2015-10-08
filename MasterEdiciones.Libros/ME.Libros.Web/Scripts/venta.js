function abrirModalVentaItem(url, itemIndex) {
    var ventaItemViewModels = new Array();
    var i = 0;
    $(".hiddenProductoId").each(function () {
        var preFix = "#Items\\[" + i + "\\]\\.";
        var item = {
            Orden: i + 1,
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
        data: JSON.stringify({ ventaItemViewModels: ventaItemViewModels, itemIndex: itemIndex }),
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
                mensajeSuccess("Se agregó el producto \"" + ventaItem.Producto.Nombre + "\" al detalle de la venta");
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
    var itemIndex = parseInt($("#cantidadItems").val());
    var modificar = getHtmlBotonModificar(itemIndex);
    var eliminar = getHtmlBotonEliminar(itemIndex);
    var table = $("#ventaDetalleTable").DataTable();
    var nroItem = itemIndex + 1;
    table.row.add([
            nroItem,
            ventaItem.Producto.Nombre,
            ventaItem.Cantidad,
            formatCurrency(ventaItem.PrecioVentaVendido),
            formatCurrency(ventaItem.MontoItemVendido),
            modificar + " " + eliminar
    ]).draw();

    var hiddenProductoId = "<input type='hidden' id='Items[" + itemIndex + "].ProductoId' class='hiddenProductoId' name='Items[" + itemIndex + "].ProductoId' value='" + ventaItem.ProductoId + "' />";
    var hiddenCantidad = "<input type='hidden'  id='Items[" + itemIndex + "].Cantidad' class='hiddenCantidad' name='Items[" + itemIndex + "].Cantidad' value='" + ventaItem.Cantidad + "' />";
    var hiddenPrecioVentaVendido = "<input type='hidden' id='Items[" + itemIndex + "].PrecioVentaVendido' class='hiddenPrecioVentaVendido' name='Items[" + itemIndex + "].PrecioVentaVendido' value='" + formatFloat(ventaItem.PrecioVentaVendido) + "' />";
    var hiddenMontoItemVendido = "<input type='hidden' id='Items[" + itemIndex + "].MontoItemVendido' class='hiddenMontoItemVendido' name='Items[" + itemIndex + "].MontoItemVendido' value='" + formatFloat(ventaItem.MontoItemVendido) + "' />";
    $("#formVenta").append(hiddenProductoId + hiddenCantidad + hiddenPrecioVentaVendido + hiddenMontoItemVendido);
    $("#cantidadItems").val(itemIndex + 1);
}

function getHtmlBotonModificar(itemIndex) {
    return "<button class='btn btn-warning btn-sm btnModificar' type='button' onclick='javascript:abrirModalVentaItem(\"/VentaItem/ModificarItem\", " + itemIndex + ");' title='Modificar item'>" +
        "<span class='glyphicon glyphicon-pencil' aria-hidden='true'></span>" +
        "</button>";
}

function getHtmlBotonEliminar(itemIndex) {
    return "<button class='btn btn-danger btn-sm btnEliminar' type='button' data-toggle='modal' data-target='#modalEliminarVentaItem' onclick='javascript:setearIdVentaItem(" + itemIndex + ");' title='Eliminar item'>" +
        "<span class='glyphicon glyphicon-trash' aria-hidden='true'></span>" +
        "</button>";
}

function eliminarVentaItem(itemIndex) {
    $("#Items\\[" + itemIndex + "\\]\\.ProductoId").remove();
    $("#Items\\[" + itemIndex + "\\]\\.Cantidad").remove();
    $("#Items\\[" + itemIndex + "\\]\\.PrecioVentaVendido").remove();
    $("#Items\\[" + itemIndex + "\\]\\.MontoItemVendido").remove();
    var table = $("#ventaDetalleTable").DataTable();
    table.row(itemIndex).remove().draw();
    actualizarHiddens();
    $('#modalEliminarVentaItem').modal('toggle');
    mensajeSuccess("Se eliminó el item Nº " + (parseInt(itemIndex) + 1) + " exitosamente");
}

function actualizarHiddens() {
    var itemIndex = 0;
    var table = $("#ventaDetalleTable").DataTable();
    $(".hiddenProductoId").each(function () {
        table.cell(itemIndex, 0).data(itemIndex + 1).draw();
        var oldPreFix = "#Items\\[" + $(this).attr("id").substring($(this).attr("id").indexOf("[") + 1, $(this).attr("id").indexOf("]")) + "\\]\\.";

        var preFix = "Items[" + itemIndex + "].";
        // ProductoId
        $(this).attr("id", preFix + "ProductoId");
        $(this).attr("name", preFix + "ProductoId");
        // Cantidad
        $(oldPreFix + "Cantidad").attr("id", preFix + "Cantidad").attr("name", preFix + "Cantidad");
        // PrecioVentaVendido
        $(oldPreFix + "PrecioVentaVendido").attr("id", preFix + "PrecioVentaVendido").attr("name", preFix + "PrecioVentaVendido");
        // MontoItemVendido 
        $(oldPreFix + "MontoItemVendido").attr("id", preFix + "MontoItemVendido").attr("name", preFix + "MontoItemVendido");
        itemIndex++;
    });

    $("#cantidadItems").val(itemIndex);

    itemIndex = 0;
    $(".btnEliminar").each(function () {
        $(this).attr("onclick", "setearIdVentaItem(" + itemIndex + ")");
    });

    itemIndex = 0;
    $(".btnModificar").each(function () {
        $(this).attr("onclick", "abrirModalVentaItem(\"/VentaItem/ModificarItem\"," + itemIndex + ")");
    });
}

function calcularMontosItem(precioVentaCalculado, precioVentaVendido) {
    if (precioVentaVendido === undefined) {
        precioVentaVendido = precioVentaCalculado;
    }
    calcularMontoItemVendido(precioVentaVendido);
    calcularMontoItemCalculado(precioVentaCalculado);
    calcularDiferenciaVentaItem();
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

function calcularDiferenciaVentaItem() {
    var montoCalculado = Globalize.parseFloat($("#subtotalSugerido").text());
    var montoVendido = Globalize.parseFloat($("#MontoItemVendido").val());
    var cantidad = parseInt($("#Cantidad").val());
    if (cantidad > 0 && montoCalculado >= 0 && montoVendido >= 0) {
        var difAbsoluta = montoVendido - montoCalculado;
        // Si montoCalculado == 0, no calcular diferencia porcentual. Solo mostrar la diferencia absoluta.
        var mensaje = formatFloat(difAbsoluta);
        if (montoCalculado > 0) {
            var difRelativaPorcentual = (difAbsoluta / montoCalculado) * 100;
            mensaje += " | " + formatFloat(difRelativaPorcentual) + "%";
        }

        if (difAbsoluta < 0) {
            $("#diferencia").parent().switchClass("label-success label-default", "label-danger");
        } else if (difAbsoluta > 0) {
            $("#diferencia").parent().switchClass("label-danger label-default", "label-success");
        } else {
            $("#diferencia").parent().switchClass("label-danger label-success", "label-default");
        }

        $("#diferencia").text(mensaje);
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
                mensajeSuccess("Se modificó el item Nº  " + ventaItem.Orden + " exitosamente");
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
    var itemIndex = ventaItem.Orden - 1;

    // Actualizar hiddens del  item modificado
    $("#Items\\[" + itemIndex + "\\]\\.ProductoId").val(ventaItem.ProductoId);
    $("#Items\\[" + itemIndex + "\\]\\.Cantidad").val(ventaItem.Cantidad);
    $("#Items\\[" + itemIndex + "\\]\\.PrecioVentaVendido").val(formatFloat(ventaItem.PrecioVentaVendido));
    $("#Items\\[" + itemIndex + "\\]\\.MontoItemVendido").val(formatFloat(ventaItem.MontoItemVendido));

    // Actualizar la fila del DataTable correspondiente al item
    var table = $("#ventaDetalleTable").DataTable();
    table.cell(itemIndex, 1)
        .data(ventaItem.Producto.Nombre);
    table.cell(itemIndex, 2)
        .data(ventaItem.Cantidad);
    table.cell(itemIndex, 3)
        .data(formatCurrency(ventaItem.PrecioVentaVendido));
    table.cell(itemIndex, 4)
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
        var promise = getCobrosByCuotaRequest(row.data());
        promise.done(function (data) {
            detailsControl.toggleClass("btn-default").toggleClass("btn-info");
            icon.toggleClass("glyphicon-collapse-down").toggleClass("glyphicon-collapse-up");
            row.child(data, 'no-padding').show();
            tr.addClass('shown');
            $('div.slider', row.child()).slideDown();
        });
    }
});

function loadVentaDetalleDataTable() {
    $("#ventaDetalleTable").addClass("table table-striped table-hover hover table-bordered order-column KeyTable");
    $('#ventaDetalleTable').dataTable({
        "order": [],
        "footerCallback": function (row, data, start, end, display) {
            var totales = calcularTotalColumna(this, [4]);
            if ($("#Id").length == 0) {
                var total = formatFloat(totales[0].total);
                $("#MontoCalculado").val(total);
                if (!$("#MontoVendido").prop("disabled")) {
                    // Actualizar MontoVendido solo si esta habilitado
                    $("#MontoVendido").val(total);
                }
                $("#MontoVendido").trigger("change");
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
            {
                "data": "nro"
            },
            {
                "data": "monto"
            },
            {
                "data": "vencim"
            },
            {
                "data": "estado"
            },
            {
                "data": "interes"
            },
            {
                "data": "cobrado"
            },
            {
                "data": "fechaCobro"
            },
            {
                "data": "atraso"
            },
            {
                "data": "saldo"
            }
        ]
    });
}

/* AJAX Calls */
function getProducto(url) {
    var idProducto = $("#ProductoId :selected").attr("value");
    if (idProducto > 0) {
        var request = getProductoRequest(url, idProducto);
        request.done(function (data) {
            $("#CodigoBarra").val(data.CodigoBarra);
            var precioVenta = parseFloat(data.PrecioVenta);
            calcularMontosItem(precioVenta);
            $("#precioSugerido").text(formatFloat(precioVenta));
            $("#PrecioVentaVendido").val(formatFloat(precioVenta));
        });
    } else {
        $("#PrecioVentaVendido, #MontoItemVendido").val("");
        $("#precioSugerido").text("-");
    }
}

function getProductoByCodigoBarra(url, codigoBarra) {
    if (isCodigoBarraValid(codigoBarra)) {
        var getProductoRequest = getProductoByCodigoBarraRequest(url, codigoBarra);
        getProductoRequest.done(function (data) {
            if (data.Id > 0) {
                $("#ProductoId").val(data.Id);
                if ($("#ProductoId").val() == undefined) {
                    mensajeWarn("El producto ingresado ya se encuentra en el detalle de la venta");
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
        $("#PrecioVentaVendido, #MontoItemVendido").val("");
        $("#precioSugerido").text("-");
    }
}

function getCobrosByCuotaRequest(cuota) {
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

function getComisionVendedor(url) {
    var idVendedor = $("#VendedorId :selected").attr("value");
    if (idVendedor > 0) {
        var request = getVendedorRequest(url, idVendedor);
        request.done(function (data) {
            $("#Comision").val(formatFloat(data.PorcentajeComision));
            $("#Comision").trigger("change");
        });
    } else {
        $("#Comision").val("");
        $("#Comision").trigger("change");
    }
}

function getMontoPlanPago(url) {
    var idPlanPago = $("#PlanPagoId :selected").attr("value");
    if (idPlanPago > 0) {
        var request = getPlanPagoRequest(url, idPlanPago);
        request.done(function (data) {
            if (data.Tipo == "1") {
                // Cuando es Financiado actualizar Monto Vendido
                $("#MontoVendido").val(formatFloat(data.Monto));
                $("#MontoVendido").attr("disabled", "disabled");
            } else {
                resetMontoVendido();
            }
            $("#MontoVendido").trigger("change");
        });
    } else {
        resetMontoVendido();
        $("#MontoVendido").trigger("change");
    }
}

function resetMontoVendido() {
    var montoCalculado = $("#MontoCalculado").val();
    $("#MontoVendido").val(formatFloat(montoCalculado));
    $("#MontoVendido").removeAttr("disabled");
}

///* Funciones: Forma de pago */
function calcularMontoComisionVenta() {
    var montoVendido = Globalize.parseFloat($("#MontoVendido").val());
    var comision = Globalize.parseFloat($("#Comision").val());
    if (montoVendido >= 0 && comision >= 0) {
        var montoComision = calcularMontoComision(montoVendido, comision);
        $("#MontoComision").val(formatFloat(montoComision));
        calcularMontoNeto(montoVendido, montoComision);
    }
    else {
        $("#MontoComision").val("");
        $("#MontoNetoVendido").val("");
    }
}

function calcularMontoNeto(montoVendido, montoComision) {
    var montoNeto = montoVendido - montoComision;
    $("#MontoNetoVendido").val(formatFloat(montoNeto));
}

function calcularDiferenciaVenta() {
    var montoCalculado = Globalize.parseFloat($("#MontoCalculado").val());
    var montoVendido = Globalize.parseFloat($("#MontoVendido").val());
    if (montoCalculado >= 0 && montoVendido >= 0) {
        var difAbsoluta = montoVendido - montoCalculado;
        // Si montoCalculado == 0, no calcular diferencia porcentual. Solo mostrar la diferencia absoluta.
        var mensaje = formatFloat(difAbsoluta);
        if (montoCalculado > 0) {
            var difRelativaPorcentual = (difAbsoluta / montoCalculado) * 100;
            mensaje += " | " + formatFloat(difRelativaPorcentual) + "%";
        }

        if (difAbsoluta < 0) {
            $("#diferenciaMontosVenta").parent().switchClass("label-success label-default", "label-danger");
        } else if (difAbsoluta > 0) {
            $("#diferenciaMontosVenta").parent().switchClass("label-danger label-default", "label-success");
        } else {
            $("#diferenciaMontosVenta").parent().switchClass("label-danger label-success", "label-default");
        }

        $("#diferenciaMontosVenta").text(mensaje);
    } else {
        $("#diferenciaMontosVenta").text("-");
        $("#diferenciaMontosVenta").parent().switchClass("label-success label-danger", "label-default");
    }
}