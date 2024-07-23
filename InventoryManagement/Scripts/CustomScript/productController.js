function updateCartView(cart) {
    console.log("updateCartView: ", cart);
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
        cartItems += `
        <div class="cart-item" data-id="${item.ProductID}">
            <p>Name: ${item.Name}</p>
            <p>Price: ${item.Price}</p>
            <p>Quantity: ${item.Quantity}</p>
            <button class="btn btn-danger removeFromCartBtn" data-id="${item.ProductID}">Remove</button>
            <button class="btn btn-secondary incrementQtyBtn" data-id="${item.ProductID}">+</button>
            <button class="btn btn-secondary decrementQtyBtn" data-id="${item.ProductID}">-</button>
        </div>
    `;
    });

    $cartItems.html(cartItems);
    $('#cart-count').text(totalQuantity);
}

function getCart() {
    var cartString = localStorage.getItem('cart');
    console.log("getCart raw: ", cartString);
    var cart = cartString ? JSON.parse(cartString) : [];
    console.log("getCart parsed: ", cart);
    return cart;
}



$(document).ready(function () {
    loadDataTable();
    updateCartView(getCart());
    updateProductTableQuantitiesFromLocalStorage();

    $('#productTable').on('click', '.addToCartBtn', function () {
        var productId = $(this).data('id');
        console.log("Add to cart clicked for product ID:", productId);
        addToCart(productId);
    });

    $('#cart-items').on('click', '.removeFromCartBtn', function () {
        var productId = $(this).data('id');
        console.log("Remove from cart clicked for product ID:", productId);
        removeFromCart(productId);
    });

    $('#cart-items').on('click', '.incrementQtyBtn', function () {
        var productId = $(this).data('id');
        addToCart(productId);
    });

    $('#cart-items').on('click', '.decrementQtyBtn', function () {
        var productId = $(this).data('id');
        console.log("Decrement quantity clicked for product ID:", productId);
        updateCartQuantity(productId, -1);
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
            ],
            "initComplete": function (settings, json) {
                // Update product table quantities after table initialization
                updateProductTableQuantitiesFromLocalStorage();
            }
        });
    }

    function updateProductTableQuantity(productId, change) {
        var table = $('#productTable').DataTable();
        var row = table.row(function (idx, data, node) {
            return data.ProductID === productId;
        });

        if (row.length) {
            var data = row.data();
            var newQuantity = data.Quantity - change;

            if (newQuantity < 0) {
                newQuantity = 0;
            }

            data.Quantity = newQuantity;
            table.row(row.index()).data(data);
            table.draw(false);

            saveProductQuantitiesToLocalStorage();
        }
    }

    function updateProductTableQuantitiesFromLocalStorage() {
        var quantities = getProductQuantitiesFromLocalStorage();
        var table = $('#productTable').DataTable();

        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            if (quantities[data.ProductID] !== undefined) {
                data.Quantity = quantities[data.ProductID];
                this.data(data);
            }
        });

        table.draw(false);
    }

    function saveProductQuantitiesToLocalStorage() {
        var table = $('#productTable').DataTable();
        var quantities = {};

        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
            var data = this.data();
            quantities[data.ProductID] = data.Quantity;
        });

        localStorage.setItem('productQuantities', JSON.stringify(quantities));
    }

    function getProductQuantitiesFromLocalStorage() {
        var quantities = localStorage.getItem('productQuantities');
        return quantities ? JSON.parse(quantities) : {};
    }

    function addToCart(productId) {
        console.log("Adding to cart: ", productId);
        var cart = getCart();
        console.log("Current cart: ", cart);
        var existingItem = cart.find(item => item.ProductID === productId);

        var table = $('#productTable').DataTable();
        var row = table.row(function (idx, data, node) {
            return data.ProductID === productId;
        });

        if (row.length) {
            var data = row.data();
            if (data.Quantity > 0) {
                if (existingItem) {
                    existingItem.Quantity++;
                    updateProductTableQuantity(productId, 1);
                } else {
                    productService.getProductById(productId, function (product) {
                        if (product) {
                            cart.push({
                                ProductID: productId,
                                SerialNumber: product.SerialNumber,
                                Name: product.Name,
                                WarrantyDays: product.WarrantyDays,
                                Quantity: 1,
                                Price: product.Price,
                                VendorName: product.VendorName
                            });
                            updateProductTableQuantity(productId, 1);
                        } else {
                            showErrorAlert('Product not found.');
                        }
                    });
                }
                saveCart(cart);
                console.log("Cart immediately after saving: ", getCart());

                updateCartView(cart);
                console.log("Updated cart: ", getCart());
                showSuccessAlert('Product added to cart.');
            } else {
                showOutOfStockAlert(productId);
            }
        }
    }

    function removeFromCart(productId) {
        var cart = getCart();
        var item = cart.find(item => item.ProductID === productId);

        if (item) {
            updateProductTableQuantity(productId, -item.Quantity);
            cart = cart.filter(item => item.ProductID !== productId);
            saveCart(cart);
            updateCartView(cart);
        }
    }

    function updateCartQuantity(productId, change) {
        var cart = getCart();
        var existingItem = cart.find(item => item.ProductID === productId);

        if (existingItem) {
            var newQuantity = existingItem.Quantity + change;
            var table = $('#productTable').DataTable();
            var row = table.row(function (idx, data, node) {
                return data.ProductID === productId;
            });

            if (row.length) {
                var data = row.data();
                var availableStock = data.Quantity + existingItem.Quantity;

                if (change > 0 && newQuantity <= availableStock) {
                    existingItem.Quantity = newQuantity;
                    updateProductTableQuantity(productId, change);
                } else if (change < 0) {
                    if (newQuantity > 0) {
                        existingItem.Quantity = newQuantity;
                        updateProductTableQuantity(productId, change);
                    } else {
                        updateProductTableQuantity(productId, existingItem.Quantity);
                        cart = cart.filter(item => item.ProductID !== productId);
                    }
                }

                saveCart(cart);
                updateCartView(cart);
            }
        }
    }

  
    function saveCart(cart) {
        console.log("saveCart: ", cart);
        localStorage.setItem('cart', JSON.stringify(cart));
        console.log("Cart after saving: ", localStorage.getItem('cart'));
    }

    

    $(document).on('click', '#proceed-to-checkout', function () {
        productService.proceedToCheckout(getCart(), function (success, response) {
            if (success) {
                localStorage.removeItem('cart');
                window.location.href = '/Home/Invoice';
            } else {
                showErrorAlert(response.message);
            }
        });
    });


    function showOutOfStockAlert(productId) {
        Swal.fire('Out of Stock', 'Product with ID ' + productId + ' is out of stock.', 'warning');
    }

    function showSuccessAlert(message) {
        Swal.fire({
            title: 'Success!',
            text: message,
            icon: 'success',
            timer: 1000, 
            showConfirmButton: false 
        });
    }

    function showErrorAlert(message) {
        Swal.fire('Error!', message, 'error');
    }
});
