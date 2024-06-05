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
            { "data": 'Category' },
            { "data": 'VendorName' },
            {
                "data": 'id',
                "orderable": false,
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="#" class="btn btn-primary mx-2"><i class="bi bi-cart4"></i>Add To Cart</a>
                        <a href="#" class="btn btn-warning mx-2"><i class="bi bi-ticket-detailed"></i>Details</a>
                            <a href="#" class="btn btn-info mx-2"><i class="bi bi-pencil-square"></i>Edit</a>
                            <a href="#" class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i>Delete</a>
                        </div>
                    
                    `
                }
            }
        ]
    });
}
