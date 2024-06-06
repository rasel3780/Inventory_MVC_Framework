$(document).ready(function () {
    loadDataTable();

    // Add to Cart button click handler
    $('#productTable').on('click', '.addToCartBtn', function () {
        var productId = $(this).data('id');
        $('#productId').val(productId); // Set the productId value
        $('#btnSubmit').val('AddToCart'); // Set the btnSubmit value
        $('#productForm').submit(); // Submit the form
    });
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
                "data": 'ProductID',
                "orderable": false,
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <button type="button" class="btn btn-primary mx-2 addToCartBtn" data-id="${data}">Add to Cart</button>
                            <a href="#" class="btn btn-warning mx-2"><i class="bi bi-ticket-detailed"></i>Details</a>
                            <a href="#" class="btn btn-info mx-2"><i class="bi bi-pencil-square"></i>Edit</a>
                            <a href="#" class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i>Delete</a>
                        </div>
                    `;
                }
            }
        ]
    });
}
