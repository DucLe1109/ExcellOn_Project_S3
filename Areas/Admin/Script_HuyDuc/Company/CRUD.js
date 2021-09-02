function _Add() {
    $('.block_loader').show();
    var companyObj = {
        Company_Name: $('#Company_Name').val(),
        Company_Address: $('#Company_Address').val(),
        Company_Phone: $('#Company_Phone').val(),
        Company_Email: $('#Company_Email').val(),
        Company_Description: $('#Company_Description').val()
    }
    $.ajax({
        url: "/Admin/CompanyManagement/Add",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(companyObj),
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

function _GetById(CompanyId) {
    $.ajax({
        url: "/Admin/CompanyManagement/GetById?CompanyId=" + CompanyId,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            $('#Company_Name_edit').val(result.Company_Name);
            $('#Company_Address_edit').val(result.Company_Address);
            $('#Company_Phone_edit').val(result.Company_Phone);
            $('#Company_Email_edit').val(result.Company_Email);
            $('#Company_Description_edit').val(result.Company_Description);
            $('#Id_edit').val(result.Id);
        },
        error: function () {
            alert("Error");
        }
    });
}

function _Update() {
    $('.block_loader').show();
    var companyObj = {
        Id: $('#Id_edit').val(),
        Company_Name: $('#Company_Name_edit').val(),
        Company_Address: $('#Company_Address_edit').val(),
        Company_Phone: $('#Company_Phone_edit').val(),
        Company_Email: $('#Company_Email_edit').val(),
        Company_Description: $('#Company_Description_edit').val()
    }
    $.ajax({
        url: "/Admin/CompanyManagement/Update",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(companyObj),
        type: "POST",
        dataType: "json",
        success: function (data) {
            var Id = data.Id;
            $('#Company_Name_' + Id).text(data.Company_Name);
            $('#Company_Address_' + Id).text(data.Company_Address);
            $('#Company_Phone_' + Id).text(data.Company_Phone);
            $('#Company_Email_' + Id).text(data.Company_Email);
            $('#Company_Description_' + Id).text(data.Company_Description);
            $('#myModal2').modal('hide');
            $('#Edit_Row_' + $('#Id_edit').val()).parent().parent().parent().parent().css({ "backgroundColor": "#3c4d69", "color": "white"});
        },
        error: function (errormessage) {
            alert("Update Unsuccessfully !");
        }

    });
}

function _Delete(CompanyId) {
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
                url: "/Admin/CompanyManagement/Delete?CompanyId=" + CompanyId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    if (result != 404) {
                        $('#' + CompanyId).parent().parent().parent().parent().remove();
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted. Please Reload for Update',
                            'success'
                        )
                    } else {
                        alert("The company is reference to some customer. Cannot delete !");
                    }
                },
                error: function (e) {
                    alert("Error !");
                }
            });

        }
    });
}
function _Reset(CompanyId) {
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
                url: "/Admin/CompanyManagement/Reset?CompanyId=" + CompanyId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    if (result != 404) {
                        $('#' + CompanyId).parent().parent().remove();
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted. Please Reload for Update',
                            'success'
                        )
                    } else {
                        alert("The company is reference to some customer. Cannot delete !");
                    }
                },
                error: function (e) {
                    alert("Error !");
                }
            });

        }
    });
}