var ord_id = $('#ord_id').val();
$(document).ready(function () {
    _CountChecked();
});
function _LoadMore() {
    var list_staff_was_append_count = parseInt($('#list_staff_was_append_count').val());
    $.ajax({
        url: "/Admin/ServiceManagement/LoadMore?ord_id=" + ord_id + "&list_staff_was_append_count=" + list_staff_was_append_count,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            console.log(result);
            var count = parseInt(result.list_staff_append.length) + parseInt($('#list_staff_was_append_count').val());
            $('#list_staff_was_append_count').val(count);
            var html = '';
            if (result.isLoadMore == false) {
                $('.loadmore').hide();
                result.list_staff_append.forEach(value => {
                    html +=
                        ' <label class="select__item">                           ' +
                        '     <input onclick="return _CountChecked()" type="checkbox" value="' + value.Id + '" class="checkitem"> ' +
                        '     <span class="circle-image">                        ' +
                        '         <img src="' + value.Staff_Avatar + '" />                                 ' +
                        '     </span>                                            ' +
                        '     <span style="margin-left: 10px" class="info_staff"> ' + value.Staff_FullName + (value.Id == 0 ? '<span style="margin-left:10px" class="badge badge-dot"><i class="bg-warning"></i><span>Training</span></span><span> - Project (' + value.Staff_OrderDetail + ')</span>' : '<span style="margin-left:10px" class="badge badge-dot"><i class="bg-info"></i><span>Pro</span></span><span> - Project (' + value.Staff_OrderDetail + ')</span>') +
                        ' </label>                                               ';
                });
                $('.select').append(html);
            } else {
                result.list_staff_append.forEach(value => {
                    html +=
                        ' <label class="select__item">                           ' +
                        '     <input onclick="return _CountChecked()" type="checkbox" value="' + value.Id + '" class="checkitem"> ' +
                        '     <span class="circle-image">                        ' +
                        '         <img src="' + value.Staff_Avatar + '" />                                 ' +
                        '     </span>                                            ' +
                        '     <span style="margin-left: 10px" class="info_staff"> ' + value.Staff_FullName + (value.Id == 0 ? '<span style="margin-left:10px" class="badge badge-dot"><i class="bg-warning"></i><span>Training</span></span><span> - Project (' + value.Staff_OrderDetail + ')</span>' : '<span style="margin-left:10px" class="badge badge-dot"><i class="bg-info"></i><span>Pro</span></span><span> - Project (' + value.Staff_OrderDetail + ')</span>') +
                        ' </label>                                               ';
                });
                $('.select').append(html);
            }
        },
        error: function (e) {
            alert(e);
        }
    });
}
function _CountChecked() {
    var input_checked = $('input[type="checkbox"][class="checkitem"]:checked');
    $('.Count_Check').text("(" + input_checked.length + ")");
}
function _SubmitAssignment() {
    var ord_id = $('#ord_id').val();
    var input_checked = $('input[type="checkbox"][class="checkitem"]:checked');
    var input_checked_count = $('input[type="checkbox"][class="checkitem"]:checked').length;
    var list_staff_id = [];
    for (var i = 0; i < input_checked.length; i++) {
        list_staff_id.push(parseInt($(input_checked[i]).val()));
    }
    var number_of_employee = parseInt($('#number_of_employee').val());
    if (input_checked_count < number_of_employee) {
        alert("The staff is not sufficient for this order");
    } else if (input_checked_count == number_of_employee) {
        var objSubmit = {
            list_staff_id: list_staff_id,
            orderdetail_id: ord_id
        }
        $.ajax({
            url: "/Admin/ServiceManagement/SubmitAssignment",
            contentType: "application/json;charset=utf-8",
            type: "POST",
            data: JSON.stringify(objSubmit),
            dataType: "json",
            success: function (return_url) {
                alert("Update successfully");
                window.location.href = return_url;
            },
            error: function () {

            }
        });
    }
    else {
        alert("The number of staff is greater than necessary number of employee !");
    }
}
function _search() {
    var key_search = $('#key_search').val().toLowerCase().trim();
    var info_staff = $('.info_staff');
    $.each(info_staff, function () {
        var info = $(this).text().toLowerCase();
        if (info.includes(key_search) == false) {
            $(this).parent().hide();
        } else {
            $(this).parent().show();
        }
    });
    if (key_search == "") {
        $('.info_staff').parent().show();
    }
}
function _Close() {
    history.go(-1);
}