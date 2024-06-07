var productService = (function () {
    function addToCart(productId, callback) {
        $.ajax({
            url: '/Home/AddToCart',
            type: 'POST',
            data: { productId: productId },
            success: function (response) {
                callback(true, response);
            },
            error: function (xhr, status, error) {
                callback(false, { message: 'An error occurred while adding the product to the cart.' });
            }
        });
    }

    function removeFromCart(productId, callback) {
        $.ajax({
            url: '/Home/RemoveFromCart',
            type: 'POST',
            data: { productId: productId },
            success: function (response) {
                callback(true, response);
            },
            error: function (xhr, status, error) {
                callback(false, { message: 'An error occurred while removing the product from the cart.' });
            }
        });
    }

    function updateCartQuantity(productId, change, callback) {
        $.ajax({
            url: '/Home/UpdateCartQuantity',
            type: 'POST',
            data: { productId: productId, change: change },
            success: function (response) {
                callback(true, response);
            },
            error: function (xhr, status, error) {
                callback(false, { message: 'An error occurred while updating the product quantity in the cart.' });
            }
        });
    }

    function proceedToCheckout(callback) {
        $.ajax({
            url: '/Home/ProceedToCheckout',
            type: 'POST',
            success: function (response) {
                callback(true, response);
            },
            error: function (xhr, status, error) {
                callback(false, { message: 'An error occurred during checkout.' });
            }
        });
    }

    return {
        addToCart: addToCart,
        removeFromCart: removeFromCart,
        updateCartQuantity: updateCartQuantity,
        proceedToCheckout: proceedToCheckout
    };
})();