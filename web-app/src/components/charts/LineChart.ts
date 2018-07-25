import { Result } from './../../resources/scripts/data';
import { Chart } from 'chart.js';
import { bindable } from 'aurelia-framework';

export class LineChart {

  percentiles: [[number]];
  //max: number;
  @bindable data: Result;
  @bindable localId: string;

  attached(){
    this.percentiles = this.data.portfolioPercentiles;
    //this.max = (this.data.bondsRetirementAmounts[2] + this.data.stocksRetirementAmounts[2]) * 2.5;
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
         borderColor: '#DC08E3',
         backgroundColor: 'transparent',
         pointBorderColor: '#DC08E3',
         pointBackgroundColor: '#DC08E3',
         pointBorderWidth: 0,
         type: 'line'
       },
       {
         label: '30',
         data:  this.percentiles[2],
         lineTension: 0,
         fill: false,
         borderColor: '#FA6700',
         backgroundColor: 'transparent',
         pointBorderColor: '#FA6700',
         pointBackgroundColor: '#FA6700',
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
        borderColor: '#B711E3',
        backgroundColor: 'transparent',
        pointBorderColor: '#B711E3',
        pointBackgroundColor: '#B711E3',
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
        borderColor: '#FDB600',
        backgroundColor: 'transparent',
        pointBorderColor: '#FDB600',
        pointBackgroundColor: '#FDB600',
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
       labels:  this.percentiles[5].map((_,index)=>index),
     },
     options:{
      scales:{
        yAxes:[{
          ticks:{
            beginAtZero: true,
            //max: this.max
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



  







