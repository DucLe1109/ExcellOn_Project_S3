var count = 0;
function _remove(ID) {
    $.ajax({
        url: "/Admin/OrderManagement/RemoveItem?OrderDetail_ID=" + ID,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            $('#' + ID).parent().parent().remove();
            $('._total_cost').text(parseInt($('._total_cost').text()) - result);
            
            $('.total_cost').text(parseInt($('.total_cost').text()) - result);
        },
        error: function () {

        }
    });
}
function _GiveCode() {
    var a = ($('#code').val() == "huyducdeptrai");
    if (a && count < 2 && count != 1) {
        var total_cost = parseInt($('._total_cost').text());
        total_cost = total_cost - total_cost * 0.01;
        $('.total_cost').text(total_cost);
        $('#discount').show();
        count += 1;
    }
    var b = ($('#code').val() == "huyducratdeptrai");
    if (b && count < 2 && count != 1) {
        var total_cost = parseInt($('._total_cost').text());
        total_cost = total_cost - total_cost * 0.03;
        $('.total_cost').text(total_cost);
        $('#discount2').show();
        count += 1;
    }
    //if () {
    //
    //}
}
function _CheckOut() {
    var gender_selected = $('select option:selected');
    var Customer_Gender = parseInt(gender_selected.val());
    var data = {
        total_cost: parseInt($('.total_cost').text()),
        Customer_Description: $('#Customer_Description').val(),
        Company_Description: $('#Company_Description').val(),
        Company_Name: $('#Company_Name').val(),
        Company_Phone: $('#Company_Phone').val(),
        Company_Email: $('#Company_Email').val(),
        Company_Address: $('#Company_Address').val(),
        Customer_Name: $('#Customer_Name').val(),
        Customer_Phone: $('#Customer_Phone').val(),
        Customer_Email: $('#Customer_Email').val(),
        Customer_Gender: Customer_Gender
    };
    console.log(data);
    $.ajax({
        url: "/Admin/OrderManagement/SubmitOrder",
        contentType: "application/json;charset=utf-8",
        type: "GET",
        data: data,
        dataType: "json",
        success: function (result) {
            alert("Create Successful !")
            history.go(-1);
        },
        error: function (e) {
            console.log(e);
        }
    });
}