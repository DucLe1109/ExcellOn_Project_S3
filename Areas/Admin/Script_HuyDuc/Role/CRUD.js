
function _Add() {
    $('.block_loader').show();
    var roleObj = {
        Role_Name: $('#Role_Name').val(),
        Role_Description: $('#Role_Description').val()
    }
    $.ajax({
        url: "/Admin/RoleManagement/Add",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(roleObj),
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

function _GetById(RoleId) {
    $.ajax({
        url: "/Admin/RoleManagement/GetById?RoleId=" + RoleId,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            $('#Role_Name_edit').val(result.Role_Name);
            $('#Role_Description_edit').val(result.Role_Description);
            $('#Id_edit').val(result.Id);
        },
        error: function (erro) {
            alert(erro);
        }
    });
}

function _Update() {
    $('.block_loader').show();
    var roleObj = {
        Id: $('#Id_edit').val(),
        Role_Name: $('#Role_Name_edit').val(),
        Role_Description: $('#Role_Description_edit').val()
    }
    $.ajax({
        url: "/Admin/RoleManagement/Update",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(roleObj),
        type: "POST",
        dataType: "json",
        success: function (data) {
            window.location.href = data;
        },
        error: function (errormessage) {
            alert("Update Unsuccessfully !");
        }

    });
}

function _Delete(RoleId) {
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
                url: "/Admin/RoleManagement/Delete?RoleId=" + RoleId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    $('#' + RoleId).parent().parent().parent().parent().remove();
                    Swal.fire(
                        'Deleted!',
                        'Your file has been deleted.',
                        'success'
                    )
                },
                error: function () {
                    alert("Error !");
                }
            });

        }
    });
}