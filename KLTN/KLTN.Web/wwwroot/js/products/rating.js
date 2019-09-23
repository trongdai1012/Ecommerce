var rate = {
    init: function () {
        rate.registerEvents();
    },
    registerEvents: function () {
        $(document).on('click', '.inputqty', function (e) {
            e.preventDefault();
            var inp = $(this);
            var id = inp.data('id');
            var quantity = document.getElementById(id).value;
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
        });
    }
};
rate.init();