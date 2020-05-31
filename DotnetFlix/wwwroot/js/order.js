
// Post payment-form using AJAX
let orderSubmit = function(event) {

    let form = document.forms.paymentForm;
    let formData = new FormData(form);

    // Post back 
    fetch("https://localhost:44397/order", {
        method: "POST",
        body: formData
    })

        .then(response => {

            if (response.ok)
                return response.text();
            else
                return;

        })

        .then(data => {
            window.location = 'https://localhost:44397/order/Success/?orderId=' + data;
        });

    event.preventDefault();
}


window.addEventListener('DOMContentLoaded', function () {

    // Name of cardholder
    let paymentNameInput = document.getElementById("OwnerName");
    if (paymentNameInput != null) {
        paymentNameInput.addEventListener("input", function () {

            let regexp = /^\p{L}{2,}([-]?\p{L}{2,})\s\p{L}{2,}([-\s]?\p{L}{2,}){0,3}$/gu;
            InputError("OwnerName", regexp.test(this.value), "Invalid Name");

        });
    }

    // Cardnumber
    let paymentCardInput = document.getElementById("CardNumber");
    if (paymentCardInput != null) {
        paymentCardInput.addEventListener("input", function () {

            this.value = this.value.replace(/[^\d]/g, '').replace(/(.{4})/g, '$1 ').trim();

            let regexp = /^(?:\d{4}\s){3}\d\d\d\d/;
            InputError("CardNumber", regexp.test(this.value), "Invalid Cardnumber");

        });
    }

    // CVV
    let paymentCVVInput = document.getElementById("CVV");
    if (paymentCVVInput != null) {
        paymentCVVInput.addEventListener("input", function () {

            this.value = this.value.replace(/[^\d]/g, '');

            let regexp = /^\d{3}$/gu;
            InputError("CVV", regexp.test(this.value), "Invalid");

        });
    }

    // Listen for form submit
    let paymentSubmit = document.getElementById("paymentForm");
    if (paymentSubmit != null)
        paymentSubmit.addEventListener("submit", orderSubmit, true);

});

function InputError(elementId, status, message) {
    document.getElementById(elementId + '-error').innerHTML = (!status) ? message : "";
}