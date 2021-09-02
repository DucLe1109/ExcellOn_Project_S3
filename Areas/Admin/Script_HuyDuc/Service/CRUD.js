function _setServiceId(ServiceId) {
    $("#Service_Id").val(ServiceId)
}
function _Submit() {
    var data = {
        Service_Id: $("#Service_Id").val(),
        Number_Of_Employee: $("#Number_Of_Employee").val(),
        Date_Start: $("#Date_Start").val(),
        Date_End: $("#Date_End").val(),
        Description: $("#Description").val()
    }
    $.ajax({
        url: "/Admin/OrderManagement/CreateOrder",
        contentType: "application/json;charset=utf-8",
        type: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        success: function (result) {
            if (result == "The Employee is greater than our Staff") {
                alert(result);
            } else if (result == 404) {
                alert("Date time is not valid");
            }
            else {
                window.location.href = result;
            }
        },
        error: function (e) {
            alert(e);
        }

    });
}
function _Delete(ServiceId) {
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
                url: "/Admin/ServiceManagement/Delete?ServiceId=" + ServiceId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    $('#' + ServiceId).parent().parent().parent().parent().remove();
                    Swal.fire(
                        'Deleted!',
                        'Your file has been deleted. Please Reload to update data',
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