$(function () {
    var startDate = new Date();
    startDate.setFullYear(startDate.getFullYear() - 1);
    var endDate = new Date();
    var startPara = getParameterFromUrl('start');
    if (startPara != null) {
        startDate = new Date(startPara);
    }
    var endPara = getParameterFromUrl('end');
    if (endPara != null) {
        endDate = new Date(endPara);
    }
    var studentId = getParameterFromUrl('studentId');
    var classId = getParameterFromUrl('classId');

    $('input[name="datefilter"]').daterangepicker({
        autoUpdateInput: true,
        startDate: startDate,
        endDate: endDate,
        locale: {
            cancelLabel: 'Clear'
        },
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    });

    $("#filterValueSubmit").on('click', function (e) {
        e.preventDefault();
        var dateValue = $('#datefilter').val();
        var studentId = $("#studentList option:selected").val();
        var classId = $("#classList option:selected").val();

        var start = dateValue.split(" - ")[0];
        var end = dateValue.split(" - ")[1];
        window.location.href = window.location.href.split('?')[0] + '?studentId=' + studentId + '&classId=' + classId + '&start=' + start + '&end=' + end;
    });

    $.ajax({
        url: `/LateEvents/GetChartData?start=${startPara}&end=${endPara}&studentId=${studentId}&classId=${classId}`,
        type: 'GET',
        success: function (responseData) {
            google.charts.load('current', { 'packages': ['corechart'] });
            google.charts.setOnLoadCallback(drawChart);
            google.charts.setOnLoadCallback(drawChartPushCount);

            function drawChart() {
                var data = new google.visualization.DataTable();
                data.addColumn('date', 'Time');
                data.addColumn('number', 'Money');
                for (var i = 0; i < responseData.length; i++) {
                    data.addRow([new Date(responseData[i].Date), responseData[i].LateMoney]);
                }
                var options = {
                    title: 'Thống kê số tiền của sinh viên theo thời gian',
                    curveType: 'function',
                    legend: { position: 'bottom' }
                };
                var chart = new google.visualization.LineChart(document.getElementById('curve_chart'));
                chart.draw(data, options);
            }

            function drawChartPushCount() {
                var data = new google.visualization.DataTable();
                data.addColumn('date', 'Time');
                data.addColumn('number', 'PushCount');
                for (var i = 0; i < responseData.length; i++) {
                    data.addRow([new Date(responseData[i].Date), responseData[i].PushCount]);
                }
                var options = {
                    title: 'Thống kê số chống đẩy của sinh viên theo thời gian',
                    curveType: 'function',
                    legend: { position: 'bottom' }
                };
                var chart = new google.visualization.LineChart(document.getElementById('curve_chart_push'));
                chart.draw(data, options);
            }
        }
    });
});
function getParameterFromUrl(parameter) {
    var url_string = window.location.href;
    var url = new URL(url_string);
    return url.searchParams.get(parameter);
}