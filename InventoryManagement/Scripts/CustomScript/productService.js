var productService = {
    getProductById: function (productId, callback) {
        $.ajax({
            url: '/Home/GetProductById',
            method: 'GET',
            data: { productId: productId },
            success: function (data) {
                callback(data);
            },
            error: function () {
                callback(null);
            }
        });
    },
    proceedToCheckout: function (cart, callback) {
        $.ajax({
            url: '/Home/ProceedToCheckout',
            method: 'POST',
            data: JSON.stringify(cart),
            contentType: 'application/json',
            success: function (data) {
                callback(true, data);
            },
            error: function (response) {
                callback(false, response);
            }
        });
    }
};
