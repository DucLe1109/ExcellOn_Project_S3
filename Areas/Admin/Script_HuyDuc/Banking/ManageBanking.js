$(document).ready(function () {
    $('#Start').datepicker({
        dateFormat: "dd/M/yy"
    });
    $('#End').datepicker({
        dateFormat: "dd/M/yy"
    });
    $('#table_banking').DataTable({
        "lengthMenu": [10, 15, 20, 30],
        "pagingType": "full_numbers",
        language: {
            paginate: {
                next: '<i class="fa fa-fw fa-long-arrow-right">',
                previous: '<i class="fa fa-fw fa-long-arrow-left">'
            }
        },
        order: [[1, "desc"]]
    });
});

function _Reset(BankingId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Reset it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Admin/Banking/Reset?BankingId=" + BankingId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    if (result != 404) {
                        $('#' + BankingId).parent().parent().remove();
                        Swal.fire(
                            'Deleted!',
                            'Your file has been reseted. Please Reload for Update',
                            'success'
                        )
                    } else {
                        alert("The banking is not valid !");
                    }
                },
                error: function (e) {
                    alert("Error !");
                }
            });

        }
    });
}

function _Delete(BankingId) {
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
                url: "/Admin/Banking/Delete?BankingId=" + BankingId,
                contentType: "application/json;charset=utf-8",
                type: "GET",
                dataType: "json",
                success: function (result) {
                    if (result != 404) {
                        $('#' + BankingId).parent().parent().remove();
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted. Please Reload for Update',
                            'success'
                        )
                    } else {
                        alert("The banking is not valid !");
                    }
                },
                error: function (e) {
                    alert("Error !");
                }
            });

        }
    });
}