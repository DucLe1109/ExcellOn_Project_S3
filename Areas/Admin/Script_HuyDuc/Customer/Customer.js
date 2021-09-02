function _send() {
    if ($('#Subject').val() != "" && $('#Body').val() != "" && $('#To').val() != "") {
        $('.loader').show();
    } else {
        alert("May be some information is null !");
    }

}
function check_uncheck_checkbox(isChecked) {
    if (isChecked) {
        $('input[name="sendEmail"]').each(function () {
            this.checked = true;
        });
    } else {
        $('input[name="sendEmail"]').each(function () {
            this.checked = false;
        });
    }
    checkItem();
}

function checkItem() {
    //var isCheck = $(el).is(':checked');
    var lstEmail = "";
    var lstChecked = document.querySelectorAll('#lstCustomer input:checked');
    [...lstChecked].map((elm, idx) => {
        lstEmail += `${$(elm).val()},`;
    })
    $('#toEmail').val(lstEmail.slice(0, -1));

    $("#popupsend").click(function () {
        $("#To").val(lstEmail.slice(0, -1));
    });
}

function _Add() {
    var gender = $('#Customer_Gender option:selected');

    $('.block_loader').show();
    var customerObj = {
        Customer_Name: $('#Customer_Name').val(),
        Customer_Gender: gender.val(),
        Customer_Comment: $('#Customer_Comment').val(),
        Customer_Phone: $('#Customer_Phone').val(),
        Customer_Email: $('#Customer_Email').val()
    }
    $.ajax({
        url: "/Admin/CustomerManagement/Add",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(customerObj),
        type: "POST",
        dataType: "json",
        success: function (data) {
            window.location.href = data;
        },
        error: function (errormessage) {
            alert("Add Unsuccessfully !");
        }

    });
}

function _GetById(CustomerId) {
    $.ajax({
        url: "/Admin/CustomerManagement/GetById?CustomerId=" + CustomerId,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            $('#Customer_Name_edit').val(result.Customer_Name);
            $('#Customer_Phone_edit').val(result.Customer_Phone);
            $('#Customer_Email_edit').val(result.Customer_Email);
            $('#Customer_Gender_edit').val(result.Customer_Gender).change();
            $('#Customer_Comment_edit').val(result.Customer_Comment);
            $('#Id_edit').val(result.Id);
        },
        error: function () {
            alert("Error");
        }
    });
}

function _Update() {
    var gender = $('#Customer_Gender_edit option:selected');
    $('.block_loader').show();
    var customerObj = {
        Id: $('#Id_edit').val(),
        Customer_Name: $('#Customer_Name_edit').val(),
        Customer_Phone: $('#Customer_Phone_edit').val(),
        Customer_Email: $('#Customer_Email_edit').val(),
        Customer_Gender: gender.val(),
        Customer_Comment: $('#Customer_Comment_edit').val()
    }
    $.ajax({
        url: "/Admin/CustomerManagement/Update",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(customerObj),
        type: "POST",
        dataType: "json",
        success: function (data) {
            var Id = data.Id;
            $('#Customer_Name_' + Id).text(data.Customer_Name);
            $('#Customer_Phone_' + Id).text(data.Customer_Phone);
            $('#Customer_Email_' + Id).text(data.Customer_Email);
            $('#Customer_Comment_' + Id).text(data.Customer_Comment);
            var gender_id = data.Customer_Gender;
            if (gender_id == 1) {
                $('#Customer_Gender_' + Id).text("Female");
            } else {
                $('#Customer_Gender_' + Id).text("Male");
            }
            
            $('#myModal2').modal('hide');
            $('#Edit_Row_' + $('#Id_edit').val()).parent().parent().parent().parent().css({ "backgroundColor": "#3c4d69", "color": "white" });
        },
        error: function (errormessage) {
            alert("Update Unsuccessfully !");
        }

    });
}

function _Delete(CustomerId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/CustomerManagement/Delete?CustomerId=" + CustomerId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    if (result != 404) {
                        $('#' + CustomerId).parent().parent().parent().parent().remove();
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted. Please Reload for Update',
                            'success'
                        )
                    } else {
                        alert(" Error !");
                    }
                },
                error: function (e) {
                    alert("Error !");
                }
            });

        }
    });
}

function _Reset(CustomerId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/CustomerManagement/Reset?CustomerId=" + CustomerId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    if (result != 404) {
                        $('#' + CustomerId).parent().parent().remove();
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted. Please Reload for Update',
                            'success'
                        )
                    } else {
                        alert(" Error !");
                    }
                },
                error: function (e) {
                    alert("Error !");
                }
            });

        }
    });
}