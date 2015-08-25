$(document).ready(function () {
    if ($(".dataTableCustom").length > 0) {
        loadDataTable();
    }
});

function calcularTotalColumna(dt, columnArray) {
    var api = dt.api();

    for (var i = 0; i < columnArray.length; i++) {
        var column = columnArray[i];

        // Total over all pages
        var total = api
            .column(column)
            .data()
            .reduce(function (a, b) {
                return getFloatVal(a) + getFloatVal(b);
            }, 0);

        // Total over this page
        var pageTotal = api
        .column(column, { page: 'current' })
        .data()
        .reduce(function (a, b) {
            return getFloatVal(a) + getFloatVal(b);
        }, 0);

        // Update footer
        $(api.column(column).footer()).html(formatCurrency(pageTotal) + ' (' + formatCurrency(total) + ')');
    }
}

function getFloatVal(i) {
    return typeof i === 'string'
            ? Globalize.parseFloat(i)
            : typeof i === 'number'
                ? i
                : 0;
}

function loadDataTable() {
    $(".dataTableCustom").addClass("table table-striped table-hover table-bordered order-column");
    $(".dataTableCustom .columnaAcciones").attr("data-orderable", "false");

    var table = $('.dataTableCustom').DataTable({
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        keys: {
            className: ''
        }
    });

    table.buttons()
        .container()
        .insertBefore('#DataTables_Table_0_filter');

    if ($(".dataTables_empty").length == 0) {
        $('.dataTableCustom tbody').on('click', 'tr', function () {
            if (!$(this).hasClass('rowActive')) {
                $('.dataTableCustom tbody tr').removeClass('rowActive');
                $(this).addClass('rowActive');
            }
        });

        $('.dataTableCustom tbody').on('dblclick', 'tr', function () {
            window.location = $(this).find(".btnModificar").attr("href");
        });

        table.on('key', function (e, datatable, key, cell, originalEvent) {
            if (key === 13) {
                // Enter sobre un registro
                window.location = $(cell.node()).parent("tr").find(".btnModificar").attr("href");
            }
        });

        table.on('key-focus', function (e, datatable, cell) {
            // Focus sobre una celda
            $('.dataTableCustom tbody tr').removeClass('rowActive');
            $(cell.node()).parent("tr").addClass('rowActive');
        });

        $(document).bind("keydown", function (event) {
            if (event.keyCode == 46) {
                // Delete sobre un registro
                var rowSelected = $(".dataTableCustom").find("tr.rowActive");
                if (rowSelected.length == 1) {
                    $(".dataTableCustom").find("tr.rowActive").find(".btnEliminar").click();
                }
            }
        });
    }
}

function loadDataTable2() {
    $(".dataTableCustom").addClass("table table-striped table-hover hover table-bordered order-column");
    $(".dataTableCustom .columnaAcciones").attr("data-orderable", "false");

    var table = $('.dataTableCustom').dataTable();

    if ($(".dataTables_empty").length == 0) {
        //$('.dataTableCustom tbody').on('click', 'tr', function () {
        //    if (!$(this).hasClass('active')) {
        //        $('.dataTableCustom tbody tr').removeClass('active');
        //        $(this).addClass('active');
        //    }
        //});

        //$('.dataTableCustom tbody').on('dblclick', 'tr', function () {
        //    window.location = $(this).find(".btnModificar").attr("href");
        //});

        //var keys = new $.fn.dataTable.KeyTable(table);

        ///* Action */
        //keys.event.action(null, null, function (node) {
        //    window.location = $(node).parent("tr").find(".btnModificar").attr("href");
        //});

        //keys.event.focus(null, null, function (node) {
        //    $('.dataTableCustom tbody tr').removeClass('active');
        //    $(node).parent("tr").addClass('active');
        //});

        //$(document).bind("keydown", function (event) {
        //    if (event.keyCode == 46) {
        //        var rowSelected = $(".dataTableCustom").find("tr.active");
        //        if (rowSelected.length == 1) {
        //            $(".dataTableCustom").find("tr.active").find(".btnEliminar").click();
        //        }
        //    }
        //});
    }
}