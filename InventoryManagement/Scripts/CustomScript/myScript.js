document.addEventListener("DOMContentLoaded", function () {
    //Graph
    var monthlySalesData = [3000, 4500, 5000, 5800, 5500, 5000, 6800, 7200, 7500, 8800, 9000, 9200];
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    var ctx1 = document.getElementById('line-chart');
    if (ctx1) {
        var myChart = new Chart(ctx1.getContext('2d'), {
            type: 'line',
            data: {
                labels: months,
                datasets: [{
                    label: 'Monthly Sales',
                    data: monthlySalesData,
                    borderColor: 'rgb(75, 192, 192)',
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                width: 600,
                height: 300,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }


    //pie chart
    var topFiveCategoriesData = {
        labels: ['Medicine', 'Cosmetic', 'Groceries', 'Cloth', 'Baby Food'],
        datasets: [{
            data: [300, 200, 150, 100, 50],
            backgroundColor: ['#000ce8', '#0fe800', '#fff700', '#f2700c', '#c002e6'],
        }]
    };

    var ctx2 = document.getElementById('pie-chart');
    if (ctx2) {
        var pieChart = new Chart(ctx2.getContext('2d'), {
            type: 'pie',
            data: topFiveCategoriesData,
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: true,
                        position: 'right',
                    },
                    title: {
                        display: true,
                        text: 'Top Five Selling Categories',
                        font: {
                            size: 18
                        },
                        padding: 20
                    }
                }
            }
        });
    }
});


//Password toggle 
$(document).ready(function () {
    // Using event delegation for dynamically added elements
    $(document).on("click", "#togglePassword", function () {
        var passwordField = $("#password");
        var icon = $(this);

        // Toggle password visibility
        if (passwordField.attr("type") === "password") {
            passwordField.attr("type", "text");
            icon.removeClass("fa-eye").addClass("fa-eye-slash");
        } else {
            passwordField.attr("type", "password");
            icon.removeClass("fa-eye-slash").addClass("fa-eye");
        }
    });
});
