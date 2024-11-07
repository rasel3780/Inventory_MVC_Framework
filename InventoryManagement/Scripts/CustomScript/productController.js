$(document).ready(function () {
   
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    let adjustedQuantities = JSON.parse(localStorage.getItem('adjustedQuantities')) || {};

    const saveAdjustedQuantitiesToLocalStorage = () => {
        localStorage.setItem('adjustedQuantities', JSON.stringify(adjustedQuantities));
    };

    const showNotification = (message, type = 'success') => {
        Swal.fire({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 1500,
            icon: type,
            title: message
        });
    };


    const restoreQuantitiesInDataTable = () => {
        $('#productTable').DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
            const row = this.data();
            const productId = row.ProductID;

            if (adjustedQuantities[productId] !== undefined) {
                row.Quantity = adjustedQuantities[productId];
            }
        }).invalidate().draw(false);
    };

    const updateCartBadge = () => {
        const totalItemCount = cart.reduce((total, item) => total + item.Quantity, 0);
        $("#cartBadge").text(totalItemCount);
    };

    const saveCartToLocalStorage = () => {
        localStorage.setItem('cart', JSON.stringify(cart));
    };

    const loadDataTable = () => {
        $('#productTable').DataTable({
            "ajax": {
                "url": '/Home/LstProduct',
                "dataSrc": ''
            },
            "columns": [
                
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
                    "render": function (data, type, row) {
                        const isOutOfStock = row.Quantity <= 0;
                        return `
                            <div class="w-75 btn-group" role="group">
                                <button type="button" class="btn ${isOutOfStock ? 'btn-danger' : 'btn-primary'} mx-2 addToCartBtn" data-id="${data}" ${isOutOfStock ? 'disabled' : ''}>
                                    ${isOutOfStock ? 'Out of Stock' : 'add to cart'}
                                </button>
                            </div>
                        `;
                    }
                }
            ],
            
        }).on('xhr.dt', function () {
            restoreQuantitiesInDataTable();
        });
    };

    loadDataTable();

    const updateCartSidebar = () => {

        let totalAmount = cart.reduce((sum, item) => sum + (item.Quantity * item.Price), 0).toFixed(2);
        let cartHtml = cart.map(item => `
        <div class="cart-item row mb-3 border-bottom pb-2" style="background-color:#ffffff">
            <div class="col-8">
                <!-- First Column -->
                <div class="row">
                    <div class="col-12">
                        <strong>${item.Name}</strong>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        ${item.Quantity} x $${item.Price.toFixed(2)}
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        Total: ${(item.Quantity * item.Price).toFixed(2)} BDT
                    </div>
                </div>
            </div>
            <div class="col-4 d-flex flex-column justify-content-center align-items-center">
                <!-- Second Column -->
                <div class="d-flex mb-2 align-items-center">
                    <button class="btn btn-sm btn-secondary increaseQty" data-id="${item.ProductID}">+</button>
                    <input type="number" min="1" class="quantityInput mx-1" data-id="${item.ProductID}" value="${item.Quantity}" style="width: 50px; text-align: center;">
                    <button class="btn btn-sm btn-secondary decreaseQty" data-id="${item.ProductID}">-</button>
                </div>
                <button class="btn btn-outline-danger btn-sm removeFromCart mt-2" data-id="${item.ProductID}"><i class="fa-solid fa-trash"></i></button>
            </div>
        </div>
        `).join('');

        let footerHtml = `
            <h5 class="d-flex justify-content-between">
                <span>Total Amount:</span>
                <span>${totalAmount} BDT</span>
            </h5>
            ${cart.length > 0 ? `
            <div class="d-flex justify-content-between mt-3">
                <button id="clearCart" class="btn btn-danger btn-sm">Clear Cart</button>
                <button id="checkoutBtn" class="btn btn-success btn-sm" >Checkout</button>
            </div>` : ''}
        `;

        $('#cartSidebar .cartContent').html(cartHtml);
        $('#cartSidebar .cartFooter').html(footerHtml);
    };

    $(document).on('click', '#checkoutBtn', function () {
        window.location.href = "/Home/Invoice";
    });

    $(document).on('click', '#clearCart', function () {
        Swal.fire({
            title: 'Are you sure?',
            text: "You are about to clear all items from your cart!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, clear it!',
            cancelButtonText: 'No, keep it'
        }).then((result) => {
            if (result.isConfirmed) {
                cart.forEach(item => {
                    if (adjustedQuantities[item.ProductID] !== undefined) {
                        adjustedQuantities[item.ProductID] += item.Quantity;
                    } else {
                        adjustedQuantities[item.ProductID] = item.Quantity;
                    }
                });

                cart = []; 
                saveCartToLocalStorage(); 
                saveAdjustedQuantitiesToLocalStorage(); 
                updateCartSidebar();
                updateCartBadge(); 
                restoreQuantitiesInDataTable(); 

                Swal.fire('Cleared!', 'Your cart has been cleared.', 'success');
            }
        });
    });

    $(document).on('click', '.increaseQty', function () {
        const productId = $(this).data('id');
        let cartItem = cart.find(item => item.ProductID === productId);
        if (cartItem) {
            if (adjustedQuantities[productId] > 0) {
                cartItem.Quantity += 1;
                adjustedQuantities[productId] -= 1;
                saveAdjustedQuantitiesToLocalStorage();
                saveCartToLocalStorage();
                updateCartSidebar();
                restoreQuantitiesInDataTable();
                showNotification('Item added to cart!', 'success');
                updateCartBadge();
            } else {
                Swal.fire('Error', 'No more stock available for this product.', 'error');
            }
        }
    });

    $(document).on('click', '.decreaseQty', function () {
        const productId = $(this).data('id');
        let cartItem = cart.find(item => item.ProductID === productId);
        if (cartItem && cartItem.Quantity > 1) {
            cartItem.Quantity -= 1;
            adjustedQuantities[productId] += 1;
            saveAdjustedQuantitiesToLocalStorage();
            saveCartToLocalStorage();
            updateCartSidebar();
            restoreQuantitiesInDataTable();
            showNotification('Item removed from cart!', 'info');
            updateCartBadge();
        }
    });

    $(document).on('change', '.quantityInput', function () {
        const productId = $(this).data('id');
        const newQuantity = parseInt($(this).val());
        let cartItem = cart.find(item => item.ProductID === productId);

        if (cartItem && newQuantity > 0) {
            const difference = newQuantity - cartItem.Quantity;
            if (difference > 0 && adjustedQuantities[productId] >= difference) {
                cartItem.Quantity = newQuantity;
                adjustedQuantities[productId] -= difference;
            } else if (difference < 0) {
                adjustedQuantities[productId] += Math.abs(difference);
                cartItem.Quantity = newQuantity;
            }
            saveAdjustedQuantitiesToLocalStorage();
            saveCartToLocalStorage();
            updateCartSidebar();
            restoreQuantitiesInDataTable();
            showNotification('Cart updated!', 'info');
        } else {
            Swal.fire('Error', 'Invalid quantity value.', 'error');
        }
    });

    $(document).on('click', '.removeFromCart', function () {

        Swal.fire({
            title: 'Are you sure?',
            text: "You are sure you want to remove the items from your cart!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, clear it!',
            cancelButtonText: 'No, keep it'
        }).then((result) => {
            if (result.isConfirmed) {


                const productId = $(this).data('id');
                let cartItem = cart.find(item => item.ProductID === productId);


                if (cartItem) {
                    adjustedQuantities[productId] += cartItem.Quantity;
                    cart = cart.filter(item => item.ProductID !== productId);
                    saveCartToLocalStorage();
                    saveAdjustedQuantitiesToLocalStorage();
                    updateCartSidebar();
                    updateCartBadge();
                    restoreQuantitiesInDataTable();
                    showNotification('Item removed from the cart!', 'info');
                }
            }

         });
    });

    $(document).on('click', '.addToCartBtn', function () {
        const productId = $(this).data('id');
        const table = $('#productTable').DataTable();
        const productData = table.row($(this).closest('tr')).data();

        if (adjustedQuantities[productId] === undefined) {
            adjustedQuantities[productId] = productData.Quantity;
        }

        if (adjustedQuantities[productId] > 0) {
            let existingCartItem = cart.find(item => item.ProductID === productId);

            if (existingCartItem) {
                existingCartItem.Quantity += 1;
            } else {
                cart.push({
                    ProductID: productId,
                    SerialNumber: productData.SerialNumber,
                    Name: productData.Name,
                    Price: productData.Price,
                    Vendor: productData.VendorName,
                    WarrantyDays : productData.WarrantyDays,
                    Quantity: 1
                });
            }

            adjustedQuantities[productId] -= 1;
            saveAdjustedQuantitiesToLocalStorage();
            saveCartToLocalStorage();
            updateCartBadge();
            updateCartSidebar();
            restoreQuantitiesInDataTable();
            showNotification('Item added to cart!', 'success');
        } else {
            Swal.fire('Error', 'This product is out of stock.', 'error');
        }
    });



    let isCartOpen = false;

    const toggleCart = () => {
        isCartOpen = !isCartOpen;
        $(".cartSidebar").toggleClass("active");
        $(".main-content").toggleClass("with-cart");
        localStorage.setItem('cartOpen', isCartOpen);
    }
    const savedCartState = localStorage.getItem('cartOpen');
    if (savedCartState === 'true') {
        toggleCart();
        updateCartSidebar();
    }
    $("#btnCartIcon").click(function (e) {
        e.preventDefault();
        toggleCart();
        updateCartSidebar();
    });

    $("#btnCloseCartSidebar").click(function () {
        toggleCart();

    });

    updateCartBadge();

   
});
