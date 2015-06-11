$.extend($.fn.dataTable.defaults, {
    //columnDefs: [
    //    { "width": "50px", "targets": "columnaCodigo" },
    //    { "width": "50px", "targets": "columnaAcciones" },
    //    { "width": "150px", "targets": "numero" }
    //    //{ "autoWidth": true, "targets": "numero" }
    //],
    //autoWidth: false,
    searching: true,
    ordering: true,
    order: [],
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
        },
        "decimal": ",",
        "thousands": "."
    },
    lengthMenu: [10, 25, 50],
    pagingType: "full_numbers",
    selecteable: true
});
