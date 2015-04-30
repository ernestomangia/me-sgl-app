$(document).ready(function () {
    if ($(".dataTableCustom").length > 0) {
        $(".dataTableCustom").addClass("table table-striped table-hover hover table-bordered order-column KeyTable");
        $(".dataTableCustom .columnaAcciones").attr("data-orderable", "false");

        var table = $('.dataTableCustom').dataTable();

        if ($(".dataTables_empty").length == 0) {
            $('.dataTableCustom tbody').on('click', 'tr', function () {
                if (!$(this).hasClass('active')) {
                    $('.dataTableCustom tbody tr').removeClass('active');
                    $(this).addClass('active');
                }
            });

            $('.dataTableCustom tbody').on('dblclick', 'tr', function () {
                window.location = $(this).find(".btnModificar").attr("href");
            });

            var keys = new $.fn.dataTable.KeyTable(table);

            /* Action */
            keys.event.action(null, null, function (node) {
                window.location = $(node).parent("tr").find(".btnModificar").attr("href");
            });

            keys.event.focus(null, null, function (node) {
                $('.dataTableCustom tbody tr').removeClass('active');
                $(node).parent("tr").addClass('active');
            });

            $(document).bind("keydown", function (event) {
                if (event.keyCode == 46) {
                    var rowSelected = $(".dataTableCustom").find("tr.active");
                    if (rowSelected.length == 1) {
                        $(".dataTableCustom").find("tr.active").find(".btnEliminar").click();
                    }
                }
            });
        }
    }
});