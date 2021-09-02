function _Activate(Id) {
    $.ajax({
        url: "/Admin/ServiceManagement/Activate?ord_id=" + Id,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (data) {
            $('#ORD_' + Id).text("Status: Started");
        },
        error: function () {
            alert("Unsuccessfully !");
        }
    });
}
function _Complete(Id) {
    $.ajax({
        url: "/Admin/ServiceManagement/Complete?ord_id=" + Id,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (data) {
            $('#ORD_' + Id).text("Status: Completed");
        },
        error: function () {
            alert("Unsuccessfully !");
        }
    });
}
function _Delete(Id) {
    $.ajax({
        url: "/Admin/ServiceManagement/Delete_ord?ord_id=" + Id,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (data) {
            $('#ORD_' + Id).text("Status: Deleted");
        },
        error: function () {
            alert("Unsuccessfully !");
        }
    });
}
function _Reset(Id) {
    $.ajax({
        url: "/Admin/ServiceManagement/Reset?ord_id=" + Id,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (data) {
            $('#ORD_' + Id).text("Status: Pending");
        },
        error: function () {
            alert("Unsuccessfully !");
        }
    });
}
function _ResetOrder(Id) {
    $.ajax({
        url: "/Admin/OrderManagement/Reset?OrderId=" + Id,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $('#ORD_' + value.Id).text("Status: Pending");
            });
            $('#Order_' + Id).text("Order Code : " + Id + " - Pending");
        },
        error: function () {
            alert("Unsuccessfully !");
        }
    });
}
function _ActivateOrder(Id) {
    $.ajax({
        url: "/Admin/OrderManagement/Activate?OrderId=" + Id,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $('#ORD_' + value.Id).text("Status: Activated");
            });
            $('#Order_' + Id).text("Order Code : " + Id + " - Activated");
        },
        error: function () {
            alert("Unsuccessfully !");
        }
    });
}
function _CompleteOrder(Id) {
    $.ajax({
        url: "/Admin/OrderManagement/Complete?OrderId=" + Id,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $('#ORD_' + value.Id).text("Status: Completed");
            });
            $('#Order_' + Id).text("Order Code : " + Id + " - Completed");
        },
        error: function () {
            alert("Unsuccessfully !");
        }
    });
}
function _DeleteOrder(Id) {
    $.ajax({
        url: "/Admin/OrderManagement/Delete?OrderId=" + Id,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (data) {
            data.forEach(value => {
                $('#ORD_' + value.Id).text("Status: Deleted");
            });
            $('#Order_' + Id).text("Order Code : " + Id + " - Deleted");
        },
        error: function () {
            alert("Unsuccessfully !");
        }
    });
}
function _search() {
    var key_search = $('#key_search').val().toLowerCase().trim();
    var all_info = $('.all_info');
    $.each(all_info, function () {
        var info = $(this).text().toLowerCase();
        if (info.includes(key_search) == false) {
            $(this).parent().hide();
        } else {
            $(this).parent().show();
        }
    });
    if (key_search == "") {
        $('.all_info').parent().show();
    }
}