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
    $(".dataTableCustom").addClass("table table-striped table-hover hover table-bordered order-column KeyTable");
    $(".dataTableCustom .columnaAcciones").attr("data-orderable", "false");

    var table = $('.dataTableCustom').dataTable();

    if ($(".dataTables_empty").length == 0) {
        $('.dataTableCustom tbody').on('click', 'tr', function() {
            if (!$(this).hasClass('active')) {
                $('.dataTableCustom tbody tr').removeClass('active');
                $(this).addClass('active');
            }
        });

        $('.dataTableCustom tbody').on('dblclick', 'tr', function() {
            window.location = $(this).find(".btnModificar").attr("href");
        });

        var keys = new $.fn.dataTable.KeyTable(table);

        /* Action */
        keys.event.action(null, null, function(node) {
            window.location = $(node).parent("tr").find(".btnModificar").attr("href");
        });

        keys.event.focus(null, null, function(node) {
            $('.dataTableCustom tbody tr').removeClass('active');
            $(node).parent("tr").addClass('active');
        });

        $(document).bind("keydown", function(event) {
            if (event.keyCode == 46) {
                var rowSelected = $(".dataTableCustom").find("tr.active");
                if (rowSelected.length == 1) {
                    $(".dataTableCustom").find("tr.active").find(".btnEliminar").click();
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