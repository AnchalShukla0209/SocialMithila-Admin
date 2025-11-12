/* profit-bar start */
const chartData = [2, 4, 3, 4, 5, 4, 5, 3, 4];

  const options = {
    series: [
    {
      name: "Sales",
      data:[2, 4, 3, 4, 5, 4, 5, 3, 4],
    }
  ],
    chart: {
      type: 'bar',
      height: 30,
      width:80,
      toolbar: {
        show: false, 
      },
      sparkline: {
      enabled: true
    },
    },
    dataLabels: {
      enabled: false,
    },
    xaxis: {
      labels: {
        show: false, 
      },
      axisBorder: {
        show: false, 
      },
    },
    yaxis: {
      labels: {
        show: false, 
      },
      axisBorder: {
        show: false,
      },
    },
    grid: {
      show: false,
    },
    colors: ['#eb5da4'],
  };

  const chart1 = new ApexCharts(document.querySelector('#profit-bar'), options);
  chart1.render();
  /* profit-bar End */

  /* profit-bar1 start */
  const chartData1 = [2, 4, 3, 4, 5, 4, 5, 3, 4];

  const options41 = {
    series: [
    {
      name: "Sales",
      data:[2, 4, 3, 4, 5, 4, 5, 3, 4],
    }
  ],
    chart: {
      type: 'bar',
      height: 30,
      width:80,
      toolbar: {
        show: false,
      },
    sparkline: {
      enabled: true
    },
    },
    plotOptions: {
      bar: {
        horizontal: false,
        columnWidth: '50%',
        colors: {
          ranges: [{
            from: 9,
            to: 9,
            color: '#00D1A2', 
          }],
          backgroundBarColors: [],
          backgroundBarOpacity: 1,
        },
      },
    },
    dataLabels: {
      enabled: false,
    },
    xaxis: {
      labels: {
        show: false, 
      },
      axisBorder: {
        show: false, 
      },
    },
    yaxis: {
      labels: {
        show: false, 
      },
      axisBorder: {
        show: false,
      },
    },
    grid: {
      show: false, 
    },
    colors: ['#00D1A2'], 
  };

  const chart11 = new ApexCharts(document.querySelector('#profit-bar1'), options41);
  chart11.render();
  /* profit-bar1 End */