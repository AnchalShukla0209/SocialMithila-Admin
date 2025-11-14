
function communitygrowth() {
    var options = {
        series: [{
            name: 'Users',
            data: [0, 150, 65, 160, 70, 130, 70, 120]
        }, {
            name: 'Businesses',
            data: [50, 90, 210, 90, 150, 75, 200, 70]
        }, {
            name: 'Events',
            data: [50, 90, 210, 90, 150, 75, 200, 70]
        }],
        chart: {
            height: 350,
            type: 'area',
            toolbar: {
                show: false,
            },
        },
        colors: ["rgba(251, 73, 102,.8)", "#b6025a"],
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        grid: {
            borderColor: '#f2f5f7',
        },
        xaxis: {
            type: 'month',
            categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "July", "Aug"],
            labels: {
                show: true,
                style: {
                    colors: "#8c9097",
                    fontSize: '11px',
                    fontWeight: 600,
                    cssClass: 'apexcharts-xaxis-label',
                },
            }
        },
        yaxis: {
            labels: {
                show: false,
                style: {
                    colors: "#8c9097",
                    fontSize: '11px',
                    fontWeight: 600,
                    cssClass: 'apexcharts-xaxis-label',
                },
            }
        },
        tooltip: {
            x: {
                format: 'dd/MM/yy HH:mm'
            },
        },
        legend: {
            show: false,
        },
        colors: ["rgba(" + myVarVal + ", 0.95)", '#b6025a'],
    };
    document.getElementById('communitygrowth').innerHTML = '';
    var chart10 = new ApexCharts(document.querySelector("#communitygrowth"), options);
    chart10.render();
}

// projectworkloadchart //
function projectworkloadchart() {
    var options = {
        series: [1554, 1234, 1854],
        labels: ["Extenal", "Internal", "other"],
        chart: {
            height: 210,
            type: 'donut'
        },
        dataLabels: {
            enabled: false,
        },

        legend: {
            show: false,
        },
        stroke: {
            show: true,
            curve: 'smooth',
            lineCap: 'round',
            colors: "#fff",
            width: 0,
            dashArray: 0,
        },
        plotOptions: {

            pie: {
                expandOnClick: false,
                donut: {
                    size: '75%',
                    background: 'transparent',
                    labels: {
                        show: false,
                        name: {
                            show: false,
                            fontSize: '20px',
                            color: '#495057',
                            offsetY: -4
                        },
                        value: {
                            show: false,
                            fontSize: '18px',
                            color: undefined,
                            offsetY: 8,
                            formatter: function (val) {
                                return val + "%"
                            }
                        },
                        total: {
                            show: false,
                            showAlways: true,
                            label: 'Total',
                            fontSize: '22px',
                            fontWeight: 600,
                            color: '#495057',
                        }

                    }
                }
            }
        },
        colors: ["#fd9900", "#f09819", "#3cba92"],

        colors: ["rgba(" + myVarVal + ", 0.95)", '#f09819', "#3cba92"],

    };
    document.querySelector("#projectworkloadchart").innerHTML = " ";
    var chart1 = new ApexCharts(document.querySelector("#projectworkloadchart"), options);
    chart1.render();
}

function trendingBusinessChart() {
    var options = {
        series: [40, 35, 25], // Trending percentages
        labels: ["Restaurants", "Shops", "Services"],
        chart: {
            height: 210,
            type: 'donut',
        },
        dataLabels: {
            enabled: false,
        },
        legend: {
            show: false,
        },
        stroke: {
            show: true,
            colors: "#fff",
            width: 0,
        },
        plotOptions: {
            pie: {
                expandOnClick: false,
                donut: {
                    size: '75%',
                    background: 'transparent',
                    labels: {
                        show: true,
                        name: {
                            show: true,
                            fontSize: '16px',
                            color: '#495057',
                            offsetY: -10
                        },
                        value: {
                            show: true,
                            fontSize: '18px',
                            color: '#000',
                            offsetY: 8,
                            formatter: function (val) {
                                return val + "%";
                            }
                        },
                        total: {
                            show: true,
                            showAlways: true,
                            label: 'Total',
                            fontSize: '20px',
                            fontWeight: 600,
                            color: '#495057',
                            formatter: function (w) {
                                return w.globals.seriesTotals.reduce((a, b) => a + b, 0) + "%";
                            }
                        }
                    }
                }
            }
        },
        colors: ["#b5025c", "#dc3545", "#28a745"], // Blue, Red, Green gradients
    };

    document.querySelector("#trendingchart").innerHTML = "";
    var chart = new ApexCharts(document.querySelector("#trendingchart"), options);
    chart.render();
}

// external chart //
function externalchart() {
    var spark4 = {
        chart: {
            type: 'line',
            height: 30,
            width: 120,
            sparkline: {
                enabled: true
            },
            dropShadow: {
                enabled: true,
                enabledOnSeries: undefined,
                top: 0,
                left: 0,
                blur: 3,
                color: '#000',
                opacity: 0.1
            }
        },
        stroke: {
            show: true,
            curve: 'smooth',
            lineCap: 'butt',
            colors: undefined,
            width: 1.5,
            dashArray: 0,
        },
        fill: {
            gradient: {
                enabled: false
            }
        },
        tooltip: {
            enabled: false,
        },
        series: [{
            name: 'Value',
            // data: [0, 45, 54, 38, 56, 24, 65, 31, 37, 39, 62, 51, 35, 41, 35, 27, 93, 53, 61, 27, 54, 43, 19, 46]
            data: [1, 3, 7, 8, 4, 5, 4, 8, 6]
        }],
        yaxis: {
            min: 0,
            show: false
        },
        xaxis: {
            axisBorder: {
                show: false
            },
        },
        yaxis: {
            axisBorder: {
                show: false
            },
        },
        colors: ['#fd9900'],

        colors: ["rgba(" + myVarVal + ", 0.95)"],
    }
    document.getElementById('externalchart').innerHTML = '';
    var spark4 = new ApexCharts(document.querySelector("#externalchart"), spark4);
    spark4.render();
}

(function () {
    "use strict";

    // internal chart //
    var spark5 = {
        chart: {
            type: 'line',
            height: 30,
            width: 120,
            sparkline: {
                enabled: true
            },
            dropShadow: {
                enabled: true,
                enabledOnSeries: undefined,
                top: 0,
                left: 0,
                blur: 3,
                color: '#000',
                opacity: 0.1
            }
        },
        stroke: {
            show: true,
            curve: 'smooth',
            lineCap: 'butt',
            colors: undefined,
            width: 1.5,
            dashArray: 0,
        },
        fill: {
            gradient: {
                enabled: false
            }
        },
        tooltip: {
            enabled: false,
        },
        series: [{
            name: 'Value',
            // data: [0, 45, 54, 38, 56, 24, 65, 31, 37, 39, 62, 51, 35, 41, 35, 27, 93, 53, 61, 27, 54, 43, 19, 46]
            data: [2, 5, 6, 4, 8, 6, 5, 9, 6]
        }],
        yaxis: {
            min: 0,
            show: false
        },
        xaxis: {
            axisBorder: {
                show: false
            },
        },
        yaxis: {
            axisBorder: {
                show: false
            },
        },
        colors: ['#f09819'],

    }
    document.getElementById('internalchart').innerHTML = '';
    var spark5 = new ApexCharts(document.querySelector("#internalchart"), spark5);
    spark5.render();


    // other chart //
    var spark6 = {
        chart: {
            type: 'line',
            height: 30,
            width: 120,
            sparkline: {
                enabled: true
            },
            dropShadow: {
                enabled: true,
                enabledOnSeries: undefined,
                top: 0,
                left: 0,
                blur: 3,
                color: '#000',
                opacity: 0.1
            }
        },
        stroke: {
            show: true,
            curve: 'smooth',
            lineCap: 'butt',
            colors: undefined,
            width: 1.5,
            dashArray: 0,
        },
        fill: {
            gradient: {
                enabled: false
            }
        },
        tooltip: {
            enabled: false,
        },
        series: [{
            name: 'Value',
            // data: [0, 45, 54, 38, 56, 24, 65, 31, 37, 39, 62, 51, 35, 41, 35, 27, 93, 53, 61, 27, 54, 43, 19, 46]
            data: [2, 5, 6, 4, 8, 6, 5, 9, 6]
        }],
        yaxis: {
            min: 0,
            show: false
        },
        xaxis: {
            axisBorder: {
                show: false
            },
        },
        yaxis: {
            axisBorder: {
                show: false
            },
        },
        colors: ['#3cba92'],

    }
    document.getElementById('otherchart').innerHTML = '';
    var spark6 = new ApexCharts(document.querySelector("#otherchart"), spark6);
    spark6.render();

})();