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
        ventaItem.PrecioVenta,
        ventaItem.Monto,
        modificar + eliminar
    ]).draw();

    var hiddenProductoId = "<input type='hidden' id='Items[" + indexItems + "].ProductoId' class='hiddenProductoId' name='Items[" + indexItems + "].ProductoId' value='" + ventaItem.ProductoId + "' />";
    var hiddenCantidad = "<input type='hidden'  id='Items[" + indexItems + "].Cantidad' class='hiddenCantidad' name='Items[" + indexItems + "].Cantidad' value='" + ventaItem.Cantidad + "' />";
    var hiddenPrecioVenta = "<input type='hidden' id='Items[" + indexItems + "].PrecioVenta' class='hiddenPrecioVenta' name='Items[" + indexItems + "].PrecioVenta' value='" + ventaItem.PrecioVenta + "' />";
    $("#formVenta").append(hiddenProductoId + hiddenCantidad + hiddenPrecioVenta);
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
    $("#Items\\[" + indexItem + "\\]\\.PrecioVenta").remove();
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
        $(this).next(".hiddenPrecioVenta").attr("id", "Items[" + indexItem + "].PrecioVenta");
        $(this).next(".hiddenPrecioVenta").attr("name", "Items[" + indexItem + "].PrecioVenta");
        indexItem++;
    });

    $("#cantidadItems").val(indexItem);
}