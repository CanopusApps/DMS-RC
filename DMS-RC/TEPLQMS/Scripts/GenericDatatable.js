function initializeDataTable(tableId, ajaxUrl, additionalData, columns) {
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
        "columns": columns
    });
}
