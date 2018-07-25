import { Result } from './../../resources/scripts/data';
import { Chart } from 'chart.js';
import { bindable } from 'aurelia-framework';

export class LineChart {

  percentiles: [[number]];
  max: number;
  @bindable data: Result;
  @bindable localId: string;

  attached(){
    this.percentiles = this.data.portfolioPercentiles;
    this.max = (this.data.bondsRetirementAmounts[2] + this.data.stocksRetirementAmounts[2]) * 2.5;
    this.buildChart();
  }

  buildChart() {
    console.log("Building chart");
    let ctx = (document.getElementById(this.localId) as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
     type: 'line',
     data:{
       datasets:[{
         label: '10',
         data: this.percentiles[0],
         lineTension: 0,
         fill: false,
         borderColor: 'red',
         backgroundColor: 'transparent',
         pointBorderColor: 'red',
         pointBackgroundColor: 'red',
         pointHitRadius: 30,
         pointBorderWidth: 2,
       },
       {
         label: '20',
         data: this.percentiles[1],
         lineTension: 0,
         fill: false,
         borderColor: 'purple',
         backgroundColor: 'transparent',
         pointBorderColor: 'purple',
         pointBackgroundColor: 'purple',
         pointHitRadius: 30,
         pointBorderWidth: 2,
         type: 'line'
       },
       {
         label: '30',
         data:  this.percentiles[2],
         lineTension: 0,
         fill: false,
         borderColor: 'orange',
         backgroundColor: 'transparent',
         pointBorderColor: 'orange',
         pointBackgroundColor: 'orange',
         pointHitRadius: 30,
         pointBorderWidth: 2,
         type: 'line'
       },
       {
        label: '40',
        data:  this.percentiles[3],
        lineTension: 0,
        fill: false,
        borderColor: 'green',
        backgroundColor: 'transparent',
        pointBorderColor: 'green',
        pointBackgroundColor: 'green',
        pointHitRadius: 30,
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '50',
        data:  this.percentiles[4],
        lineTension: 0,
        fill: false,
        borderColor: 'pink',
        backgroundColor: 'transparent',
        pointBorderColor: 'green',
        pointBackgroundColor: 'pink',
        pointHitRadius: 30,
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '60',
        data:  this.percentiles[5],
        lineTension: 0,
        fill: false,
        borderColor: 'light green',
        backgroundColor: 'transparent',
        pointBorderColor: 'light green',
        pointBackgroundColor: 'light green',
        pointHitRadius: 30,
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '70',
        data:  this.percentiles[6],
        lineTension: 0,
        fill: false,
        borderColor: 'blue',
        backgroundColor: 'transparent',
        pointBorderColor: 'blue',
        pointBackgroundColor: 'blue',
        pointHitRadius: 30,
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '80',
        data:  this.percentiles[7],
        lineTension: 0,
        fill: false,
        borderColor: 'cyan',
        backgroundColor: 'transparent',
        pointBorderColor: 'cyan',
        pointBackgroundColor: 'cyan',
        pointHitRadius: 30,
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '90',
        data:  this.percentiles[8],
        lineTension: 0,
        fill: false,
        borderColor: 'yellow',
        backgroundColor: 'transparent',
        pointBorderColor: 'yellow',
        pointBackgroundColor: 'yellow',
        pointHitRadius: 30,
        pointBorderWidth: 2,
        type: 'line'
      },

      {
        label: '100',
        data:  this.percentiles[9],
        lineTension: 0,
        fill: false,
        borderColor: 'black',
        backgroundColor: 'transparent',
        pointBorderColor: 'black',
        pointBackgroundColor: 'black',
        pointHitRadius: 30,
        pointBorderWidth: 2,
        type: 'line',
      }],
       labels:  this.percentiles[5].map((_,index)=>index),
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
      }

    }
  });
 }

}



  







