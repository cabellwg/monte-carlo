import { Chart } from 'chart.js';
import { bindable } from 'aurelia-framework';

export class LineChart {
  chart: Chart;
  canBuildNewChart = false;

  @bindable max: number;
  @bindable currentAge: number;
  @bindable percentiles: [[number]];

  @bindable localId: string;

  attached(){
    this.buildChart();

    this.canBuildNewChart = true;
  }

  percentilesChanged() {
    if (this.canBuildNewChart) {
      this.buildChart();
    }
  }

  buildChart() {
    if (this.chart) {
      this.chart.destroy();
    }
    
    let ctx = (document.getElementById(this.localId) as HTMLCanvasElement).getContext("2d");
    this.chart = new Chart(ctx, {
     type: 'line',
     data:{
       datasets:[{
         label: '10',
         data: this.percentiles[0],
         lineTension: 0,
         fill: false,
         borderColor: '#1067FD',
         backgroundColor: 'transparent',
         pointBackgroundColor: '#1067FD',
         pointBorderWidth: 0,
       },
       {
         label: '20',
         data: this.percentiles[1],
         lineTension: 0,
         fill: false,
         borderColor: '#231200',
         backgroundColor: 'transparent',
         pointBorderColor: '#231200',
         pointBackgroundColor: '#231200',
         pointBorderWidth: 0,
         type: 'line'
       },
       {
         label: '30',
         data:  this.percentiles[2],
         lineTension: 0,
         fill: false,
         borderColor: '#0B0033',
         backgroundColor: 'transparent',
         pointBorderColor: '#0B0033',
         pointBackgroundColor: '#0B0033',
         pointBorderWidth: 2,
         type: 'line'
       },
       {
        label: '40',
        data:  this.percentiles[3],
        lineTension: 0,
        fill: false,
        borderColor: '#7A5B16',
        backgroundColor: 'transparent',
        pointBorderColor: '#7A5B16',
        pointBackgroundColor: '#7A5B16',
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '50',
        data:  this.percentiles[4],
        lineTension: 0,
        fill: false,
        borderColor: '#FD2600',
        backgroundColor: 'transparent',
        pointBorderColor: '#FD2600',
        pointBackgroundColor: '#FD2600',
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '60',
        data:  this.percentiles[5],
        lineTension: 0,
        fill: false,
        borderColor: '#01276F',
        backgroundColor: 'transparent',
        pointBorderColor: '#01276F',
        pointBackgroundColor: '#01276F',
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '70',
        data:  this.percentiles[6],
        lineTension: 0,
        fill: false,
        borderColor: '#7A1D5B',
        backgroundColor: 'transparent',
        pointBorderColor: '#7A1D5B',
        pointBackgroundColor: '#7A1D5B',
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '80',
        data:  this.percentiles[7],
        lineTension: 0,
        fill: false,
        borderColor: '#033A10',
        backgroundColor: 'transparent',
        pointBorderColor: '#033A10',
        pointBackgroundColor: '#033A10',
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '90',
        data:  this.percentiles[8],
        lineTension: 0,
        fill: false,
        borderColor: '#7A2A16',
        backgroundColor: 'transparent',
        pointBorderColor: '#7A2A16',
        pointBackgroundColor: '#7A2A16',
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '100',
        data:  this.percentiles[9],
        lineTension: 0,
        fill: false,
        borderColor: '#E30045',
        backgroundColor: 'transparent',
        pointBorderColor: '#E30045',
        pointBackgroundColor: '#E30045',
        pointBorderWidth: 2,
        type: 'line',
      }],
       labels:  this.percentiles[5].map((_,index)=>index + this.currentAge),
     },
     options:{
      scales:{
        yAxes:[{
          ticks:{
            beginAtZero: true,
            max: this.max
          }
        }]
      },
      layout:{
        padding: 0
      },
      legend:{
        position: 'bottom',
      }
    }
  });
 }

}



  







