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
    $(".dataTableCustom").addClass("table table-striped table-hover hover table-bordered order-column");
    //$(".dataTableCustom .columnaAcciones").attr("data-orderable", "false");
    var table = $(".dataTableCustom").DataTable();
    $('.dataTableCustom tbody').on('click', 'tr', function () {
        if ($(this).hasClass('active')) {
            //$(this).removeClass('active');
        }
        else {
            $('.dataTableCustom tbody tr').removeClass('active');
            table.$('tr.selected').removeClass('active');
            $(this).addClass('active');
        }
    });
    $('.dataTableCustom tbody').on('dblclick', 'tr', function () {
        window.location = $(".btnModificar").attr("href");
    });
    //$("#DataTables_Table_0_length").("<a class='btn btn-success btnNuevo'>Nuevo</a>");
});