$(document).ready(function () {
    loadDataTable();
    $('#productTable').on('click', '.addToCartBtn', function () {
        var productId = $(this).data('id');
        addToCart(productId);
    });

    $('#cart-items').on('click', '.removeFromCartBtn', function () {
        var productId = $(this).data('id');
        removeFromCart(productId);
    });

    $('#cart-items').on('click', '.incrementQtyBtn', function () {
        var productId = $(this).data('id');
        updateCartQuantity(productId, 1);
    });

    $('#cart-items').on('click', '.decrementQtyBtn', function () {
        var productId = $(this).data('id');
        updateCartQuantity(productId, -1);
    });

    $('#checkoutBtn').click(function () {
        proceedToCheckout();
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
                            </div>
                        `;
                    }
                }
            ]
        });
    }

    function addToCart(productId) {
        productService.addToCart(productId, function (success, response) {
            if (success) {
                updateCartView(response.cart);
                updateProductQuantityInTable(productId, -1);
                showSuccessAlert('Product added to cart.');
            } else {
                showErrorAlert(response.message);
            }
        });
    }

    function removeFromCart(productId) {
        productService.removeFromCart(productId, function (success, response) {
            if (success) {
                updateCartView(response.cart);
                updateProductQuantityInTable(productId, 1);
                showSuccessAlert('Product removed from cart.');
            } else {
                showErrorAlert(response.message);
            }
        });
    }

    function updateCartQuantity(productId, change) {
        productService.updateCartQuantity(productId, change, function (success, response) {
            if (success) {
                updateCartView(response.cart);
                updateProductQuantityInTable(productId, -change);
                showSuccessAlert('Product quantity updated in cart.');
            } else {
                showErrorAlert(response.message);
            }
        });
    }

    function updateProductQuantityInTable(productId, change) {
        var table = $('#productTable').DataTable();
        var data = table.rows().data();
        data.each(function (value, index) {
            if (value.ProductID === productId) {
                value.Quantity += change;
                table.row(index).data(value).draw();
                return false;
            }
        });
    }

    function updateCartView(cart) {
        var $cartItems = $('#cart-items');
        $cartItems.empty();

        if (cart.length === 0) {
            $cartItems.append('<p>Your cart is empty.</p>');
            $('#cart-count').text(0);
            return;
        }

        var cartItems = '';
        var totalQuantity = 0;
        $.each(cart, function (index, item) {
            totalQuantity += item.Quantity;
            var existingItem = $cartItems.find(`.cart-item[data-id="${item.ProductID}"]`);
            if (existingItem.length) {
                existingItem.find('.cart-item-quantity').text(item.Quantity);
            } else {
                cartItems += `
                    <div class="cart-item" data-id="${item.ProductID}">
                        <p>Product ID: ${item.ProductID}</p>
                        <p>Name: ${item.Name}</p>
                        <p>Price: ${item.Price}</p>
                        <p>Quantity: <span class="cart-item-quantity">${item.Quantity}</span></p>
                        <button class="btn btn-danger removeFromCartBtn" data-id="${item.ProductID}">Remove</button>
                        <button class="btn btn-secondary incrementQtyBtn" data-id="${item.ProductID}">+</button>
                        <button class="btn btn-secondary decrementQtyBtn" data-id="${item.ProductID}">-</button>
                    </div>
                `;
            }
        });
        cartItems += `<button id="proceed-to-checkout" class="btn btn-success mt-3">Proceed to Checkout</button>`;
        $cartItems.html(cartItems);
        $('#cart-count').text(totalQuantity);
    }

    function showSuccessAlert(message) {
        Swal.fire('Success!', message, 'success');
    }

    function showErrorAlert(message) {
        Swal.fire('Error!', message, 'error');
    }

    function proceedToCheckout() {
        productService.proceedToCheckout(function (success, response) {
            if (success) {
                Swal.fire(
                    'Success!',
                    'Checkout completed successfully.',
                    'success'
                ).then(function () {
                    window.location.reload();
                });
            } else {
                showErrorAlert(response.message);
            }
        });
    }

    $(document).on('click', '#proceed-to-checkout', function () {
        window.location.href = '/Home/Invoice';
    });
});