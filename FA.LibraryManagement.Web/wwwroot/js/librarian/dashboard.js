setInterval(function () {
    let currentDate = moment().format('MMMM D, YYYY | dddd, HH:mm:ss A');
    $('#datetime-now').text(currentDate);
}, 1000);

function loadTotalBooksReport() {
    $.ajax({
        url: 'http://localhost:5055/api/DashBoard/books-report',
        type: 'GET',
        success: function (response) {
            createPieChart(response);   
        }
    });
}

function createPieChart(data) {
    let options = {
        chart: {
            type: 'donut'
        },
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }],
        series: Object.values(data),
        labels: Object.keys(data),
        colors: ['#007bff', '#ff073a', '#ffc107', '#28a745']
    };

    let chart = new ApexCharts(document.querySelector("#total-books-report"), options);
    chart.render();
}

loadTotalBooksReport();
      