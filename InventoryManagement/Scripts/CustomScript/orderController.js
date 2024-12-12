$(document).ready(function () {
    $('#orderHistoryTable').DataTable({
        "ajax": {
            "url": '/Order/GetOrderHistory',
            "dataSrc": ''
        },
        "columns": [
            { "data": 'OrderID' },
            { "data": 'CustomerName' },
            { "data": 'CustomerMobile' },
            { "data": 'SerialNumber' },
            { "data": 'ProductName' },
            { "data": 'VendorName' },
            { "data": 'OrderDate' },
            { "data": 'Amount', "render": $.fn.dataTable.render.number(',', '.', 2) },
            { "data": 'SoldBy' }
        ]
    });
});