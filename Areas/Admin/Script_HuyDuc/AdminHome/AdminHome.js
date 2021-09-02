
// plugin màu nền cho chart
const plugin = {
    id: 'custom_canvas_background_color',
    beforeDraw: (chart) => {
        const ctx = chart.canvas.getContext('2d');
        ctx.save();
        ctx.globalCompositeOperation = 'destination-over';
        ctx.fillStyle = '#32325d';
        ctx.fillRect(0, 0, chart.width, chart.height);
        ctx.restore();
    }
};

// Khởi tạo các biến toàn cục.
var tunover = null;
var Order_DateCreate = null;
var ctx1 = $('#Chart_Order_Tunover');
var Chart_Order_Tunover = new Chart(ctx1);

// Load dữ liệu vào biến toàn cục khi khởi động trang
$(document).ready(function () {
    $.ajax({
        url: "/Admin/AdminHome/GetJsonOrder?num_of_days_ago=15",
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            tunover = result.map(function (element) {
                return element.Order_TotalCost;
            });
            Order_DateCreate = result.map(function (element) {
                return element.Order_DateCreate;
            });
            Chart_Order_Tunover.destroy();
            Chart_Order_Tunover = new Chart(ctx1, {
                type: 'line',
                data: {
                    labels: Order_DateCreate,
                    datasets: [{
                        label: 'Tunover',
                        data: tunover,
                        borderColor: [
                            '#e5e5f0'
                        ],
                        borderWidth: 4,
                        backgroundColor: 'rgba(255, 99, 132, 0.2)'
                    }]
                },
                options: {
                    plugins: {
                        legend: {
                            labels: {
                                color: 'white',
                                font: {
                                    size: 14
                                }
                            }
                        }
                    },
                    layout: {
                        padding: {
                            left: 20,
                            right: 60,
                            top: 20,
                            bottom: 20
                        }
                    },
                    scales: {
                        x: {
                            grid: {
                                display: false
                            },
                            ticks: {
                                color: 'rgba(86, 194, 205, 0.82)',
                                font: {
                                    size: 13
                                }
                            }
                        },
                        y: {
                            beginAtZero: true,
                            ticks: {
                                color: 'rgba(86, 194, 205, 0.82)',
                                font: {
                                    size: 14
                                }
                            }
                        }
                    }
                },
                plugins: [plugin]
            });
        },
        error: function (erro) {
            alert(erro);
        }
    });
    $.ajax({
        url: "/Admin/AdminHome/GetJsonOrderDetail",
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            var Service_Name = result.map(function (element) {
                return element.Service_Name;
            });
            var Number_Of_OrderDetail = result.map(function (element) {
                return element.Number_Of_OrderDetail;
            });
            var ctx = $('#Chart_Service_OrderDetail');
            var myChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: Service_Name,
                    datasets: [{
                        label: 'My First Dataset',
                        data: Number_Of_OrderDetail,
                        backgroundColor: [
                            'rgb(255, 99, 132)',
                            'rgb(54, 162, 235)',
                            'rgb(255, 205, 86)'
                        ],
                        hoverOffset: 4
                    }]
                }
                //options: {
                //    scales: {
                //        y: {
                //            beginAtZero: true
                //        }
                //    }
                //}
            });
        },
        error: function (erro) {
            alert(erro);
        }
    });
});


// Thay đổi dữ liệu chart bằng cách xóa chart cũ bằng hàm destroy().
$('select').on('change', function (e) {
    var optionSelected = $("option:selected", this);
    var valueSelected = this.value;
    $.ajax({
        url: "/Admin/AdminHome/GetJsonOrder?num_of_days_ago=" + valueSelected,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var tunover = result.map(function (element) {
                return element.Order_TotalCost;
            });

            var Order_DateCreate = result.map(function (element) {
                return element.Order_DateCreate;
            });

            Chart_Order_Tunover.destroy();
            Chart_Order_Tunover = new Chart(ctx1, {
                type: 'line',
                data: {
                    labels: Order_DateCreate,
                    datasets: [{
                        label: 'Tunover',
                        data: tunover,
                        borderColor: [
                            '#e5e5f0'
                        ],
                        borderWidth: 4,
                        backgroundColor: 'rgba(255, 99, 132, 0.2)'
                    }]
                },
                options: {
                    plugins: {
                        legend: {
                            labels: {
                                color: 'white',
                                font: {
                                    size: 14
                                }
                            }
                        }
                    },
                    layout: {
                        padding: {
                            left: 20,
                            right: 60,
                            top: 20,
                            bottom: 20
                        }
                    },
                    scales: {
                        x: {
                            grid: {
                                display: false
                            },
                            ticks: {
                                color: 'rgba(86, 194, 205, 0.82)',
                                font: {
                                    size: 13
                                }
                            }
                        },
                        y: {
                            beginAtZero: true,
                            ticks: {
                                color: 'rgba(86, 194, 205, 0.82)',
                                font: {
                                    size: 14
                                }
                            }
                        }
                    }
                },
                plugins: [plugin]
            });
        },
        error: function (erro) {
            alert(erro);
        }
    });
});

//function scroll to element (example: to -> Chart_Order_Tunover)
function _GoToChart_Order_Tunover() {
    $('html, body').animate({
        scrollTop: $("#Chart_Order_Tunover").offset().top
    }, 1000);
}