var pro = {
    init: function () {
        pro.registerEvents();
    },
    registerEvents: function () {
        $(document).on('click', '.btn-active', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            var confirmResult = confirm("Bạn muốn thay đổi trạng thái sản phẩm có id = " + id + "?");
            if (confirmResult) {
                $.ajax({
                    url: "/Admin/Product/ChangeStatus",
                    data: { id: id },
                    dataType: "json",
                    type: "POST",
                    success: function (response) {
                        console.log(response);
                        if (response.status === true) {
                            btn.html('<strong>Kích hoạt<span class="glyphicon glyphicon-ok" style="color:green"></strong>');
                        }
                        else {
                            btn.html('<strong>Kích hoạt<span class="glyphicon glyphicon-remove" style="color:red"></strong>');
                        }
                        var table = $('#laptop-index').DataTable();
                        table.ajax.reload();
                    }
                });
            }
        });
    }
};
pro.init();