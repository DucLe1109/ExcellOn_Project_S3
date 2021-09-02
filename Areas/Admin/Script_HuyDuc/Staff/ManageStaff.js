function _Delete(StaffId) {
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
                url: "/Admin/StaffManagement/Delete?StaffId=" + StaffId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    $('#' + StaffId).parent().parent().parent().parent().remove();
                    Swal.fire(
                        'Deleted!',
                        'Your file has been deleted. Please Reload for Update',
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
function _ChangeStatus(StaffId) {
    $.ajax({
        url: "/Admin/StaffManagement/ChangeStatus?StaffId=" + StaffId,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            if (result == "status_1") {
                $('#status_' + StaffId).html("Profession");
            } else {
                $('#status_' + StaffId).html("Training");
            }

        },
        error: function () {
            alert("Error !");
        }
    });
}
function _Reset(StaffId) {
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
                url: "/Admin/StaffManagement/Reset?StaffId=" + StaffId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    if (result != 404) {
                        $('#' + StaffId).parent().parent().remove();
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted. Please Reload for Update',
                            'success'
                        )
                    } else {
                        alert(result);
                    }
                },
                error: function (e) {
                    alert("Error !");
                }
            });

        }
    });
}