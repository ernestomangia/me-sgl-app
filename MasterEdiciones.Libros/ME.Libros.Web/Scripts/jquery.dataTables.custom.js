$.extend($.fn.dataTable.defaults, {
    columnDefs: [
        { "width": "50px", "targets": "columnaCodigo" },
        { "width": "50px", "targets": "columnaAcciones" }
    ],
    searching: true,
    ordering: true,
    paging: true,
    language: {
        "sProcessing": "Procesando...",
        "sLengthMenu": "Mostrar _MENU_",
        "sZeroRecords": "No se encontraron resultados que coincidan con la búsqueda",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando registros del _START_ al _END_. Total: _TOTAL_",
        "sInfoEmpty": "",
        "sInfoFiltered": "",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ".",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "<<",
            "sLast": ">>",
            "sNext": ">",
            "sPrevious": "<"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    },
    lengthMenu: [10, 25, 50],
    pagingType: "full_numbers",
    selecteable: true
});

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