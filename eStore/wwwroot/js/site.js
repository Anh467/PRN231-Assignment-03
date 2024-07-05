// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function DisplayText(msg) {
    Toastify({

        text: msg,

        duration: 3000

    }).showToast();
}

function getProduct() {
    debugger
    let apiUrl = 'https://localhost:7221/api/Products'
    $.ajax({
        url: 'https://localhost:7221/api/Products',
        type: 'GET',
        error: function (response) {
            DisplayText('Load lại không thành công: ' + response.statusText);
        },
        success: function (response) {
            debugger;
            DisplayText('thành công');
            // Gán dữ liệu vào biến products (nếu cần)
            var products = response;
        }
    });
}