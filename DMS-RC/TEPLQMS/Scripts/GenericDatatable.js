function initializeDataTable(tableId, ajaxUrl, additionalData, columns, pageLength) {
    // Destroy existing DataTable if it exists
    if ($.fn.DataTable.isDataTable(tableId)) {
        $(tableId).DataTable().destroy();
    }

    // Initialize DataTable
    $(tableId).DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": ajaxUrl,
            "type": "POST",
            "data": function (d) {
                // Extend default parameters with additional data
                $.extend(d, additionalData());
            }
        },
        "columns": columns,
        "pageLength": pageLength, // Adjust the default page length
        "lengthChange": false, // This hides the "Records per page" dropdown
        "responsive": true,
        "searching": false, 
        "searchDelay": 500,
    });
}
