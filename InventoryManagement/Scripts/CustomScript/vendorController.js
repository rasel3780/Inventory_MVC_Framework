$(document).ready(function () {

    $('#vendorTable').DataTable({
        "ajax": {
            "url": '/Vendor/GetVendorList',
            "dataSrc": ''
        },
        "columns": [
            { "data": 'VendorID' },
            { "data": 'VendorName' },
            { "data": 'ContactNumber' },
            { "data": 'Address' }
        ]
    })

});