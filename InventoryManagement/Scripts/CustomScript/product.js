$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#productTable').DataTable({
        "ajax": {
            "url": '/Home/LstProduct',
            "dataSrc": ''
        },
        "columns": [
            { "data": 'ProductID' },
            { "data": 'SerialNumber' },
            { "data": 'Name' },
            { "data": 'Quantity' },
            { "data": 'EntryDate' },
            { "data": 'Price' },
            { "data": 'WarrantyDays' },
            { "data": 'Category' }
        ]
    });
}
