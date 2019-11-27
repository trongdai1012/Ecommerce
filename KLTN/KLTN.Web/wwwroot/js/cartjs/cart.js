var cart = {
    init: function () {
        cart.registerEvents();
    },
    registerEvents: function () {
        $(document).on('keyup', '.inputqty', function (e) {
            e.preventDefault();
            var inp = $(this);
            var id = inp.data('id');
            var quantity = parseInt(document.getElementById(id).value);
            var inven = parseInt(document.getElementById('inventory-' + id).textContent);
            if (parseInt(quantity) <= inven) {
                $.ajax({
                    url: "/Cart/Update",
                    data: { id: id, quantity: quantity },
                    dataType: "json",
                    type: "PUT",
                    success: function (response) {
                        console.log(response);
                        var byId = '#total-' + id;
                        var tolalPrice = response.total.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                        var subTotalPrice = response.subTotal.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                        $(byId).html(tolalPrice);
                        $('#subtotal').html(subTotalPrice);
                        $(id).focus();
                    }
                });
            } else {
                alert("Số lượng nhập vào không hợp lệ vui lòng nhập lại");
            }
            
        });
    }
};
cart.init();