// Add one item to cart
function AddToCart(id, showAddedToCartMessage = true) {

    fetch('https://localhost:44397/Cart/Add/' + id)
        .then((response) => {

            if (response.ok) {
                RefreshCartContent();

                if (showAddedToCartMessage)
                    DisplayCartMessageModal()
            }
        });

}

// Remove one item from cart
function RemoveFromCart(id) {

    fetch('https://localhost:44397/Cart/Remove/' + id)
        .then((response) => {

            if (response.ok) {
                RefreshCartContent();
            }
        });
}

// Remove one item from cart
function DeleteFromCart(id) {

    fetch('https://localhost:44397/Cart/Delete/' + id)
        .then((response) => {

            if (response.ok) {

                $('.product-' + id).slideUp(300, function () {
                    RefreshCartContent();
                });

            }

        });
}

// Fetch cart content
function RefreshCartContent() {
    fetch('https://localhost:44397/Cart')
        .then((response) => {
            if (response.ok) {
                StartSessionTimer(); // Start timer
                return response.text();
            }
        })
        .then((data) => {
            DisplayCartData('cart-content', data);
            UpdateCartItemCounter();
        });
}

// Item counter
function UpdateCartItemCounter() {

    // Update cart item counter
    let totalItemsInCart = GetNrOfItemsInCart();
    document.getElementById('cart-item-count').innerHTML = totalItemsInCart;

    // Update cart total cost
    let totalCost = document.getElementById('totalItemsCost').innerHTML;
    DisplayCartData('total-items-cost', totalCost);

    // Display Checkout button if cart contains > 0 items
    let checkoutButton = document.getElementById('checkout-button');
    checkoutButton.style.display = (totalItemsInCart > 0) ? "block" : "none";

    // If user is on checkout page and removed all items in cart, reload order page.
    let onOrderPage = document.getElementById('user-order-visible');

    if (onOrderPage !== null && totalItemsInCart === 0) {
        location.reload();
    }

}

function GetNrOfItemsInCart() {
    return parseInt(document.getElementById('totalItemsInCart').innerHTML);
}

function DisplayCartData(selector, data) {
    let elements = document.getElementsByClassName(selector)

    for (var i = 0; i < elements.length; i++)
        elements[i].innerHTML = data;
}

function DisplayCartDropdown() {

    //RefreshCartContent();
    $('#dimmer').fadeIn(200);
    $('#dropdown-cart').show(200)

}

function CloseCartDrodown() {

    $('#dimmer').fadeOut(200);
    $('#dropdown-cart').hide(200);
}


function DisplayCartMessageModal() {
    let itemsInCart = GetNrOfItemsInCart() + 1;

    document.getElementById("cartPopupItemCount").innerHTML = itemsInCart;
    $('#addedToCartMessage').modal('show');
}


function ScrollTopViewCart() {

    // Hide modal
    $('#addedToCartMessage').modal('hide');

    // Scroll to top after modal close
    $('#addedToCartMessage').on('hidden.bs.modal', function () {

        $('html').animate({ scrollTop: 0 }, function () {
            DisplayCartDropdown();

            // Dispose to disable scroll feater!
            $('#addedToCartMessage').modal('dispose');
        });

    });

}

// CartSession timer. Session cookie expires in 5 minutes. Run RefreshCartContent 1 minute before the session expires to keep it alive.
var CartSessionTimer;
function StartSessionTimer() {

    if (CartSessionTimer != null)
        window.clearTimeout(CartSessionTimer);

    CartSessionTimer = window.setTimeout(RefreshCartContent, 4 * (60 * 1000));
}

// Refresh cart when page loads
window.onload = (event) => {

    // Get content in cart. Function exist in cart.js
    RefreshCartContent();

    // display cart when user click cart button in menu
    document.getElementById("cart-open").addEventListener("click", DisplayCartDropdown);

    // If user click x (close) button in cart
    document.getElementById("cart-close").addEventListener("click", CloseCartDrodown);

    // Close cart if user click outside cart
    document.getElementById("dimmer").addEventListener("click", CloseCartDrodown);

    // Scroll to top and display cart-dropdown
    let productModal = document.getElementById("modalViewCart");

    if (productModal != null)
        productModal.addEventListener("click", ScrollTopViewCart);

}