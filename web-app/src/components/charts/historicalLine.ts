import { Data } from './../../resources/scripts/data';
import { Chart } from 'chart.js';

export class HistoricalLine {

  attached(){
    this.buildChart();
  }
   

  buildChart() {
    let ctx = (document.getElementById("historicalLine") as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
     type: 'line',
     data:{
       datasets:[{
         label: 'Label 1',
         data: Data.portfolioPercentiles[0],
         lineTension: 0.3,
         fill: false,
         borderColor: 'red',
         backgroundColor: 'transparent',
         pointBorderColor: 'red',
         pointBackgroundColor: 'red',
         pointHitRadius: 30,
         pointBorderWidth: 2,
       },
       {
         label: 'Label 2',
         data: Data.portfolioPercentiles[1],
         lineTension: 0.3,
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
         label: 'Label 3',
         data: Data.portfolioPercentiles[2],
         lineTension: 0.3,
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
        label: 'Label 4',
        data: Data.portfolioPercentiles[3],
        lineTension: 0.3,
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
        label: 'Label 5',
        data: Data.portfolioPercentiles[4],
        lineTension: 0.3,
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
        label: 'Label 6',
        data: Data.portfolioPercentiles[5],
        lineTension: 0.3,
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
        label: 'Label 7',
        data: Data.portfolioPercentiles[6],
        lineTension: 0.3,
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
        label: 'Label 8',
        data: Data.portfolioPercentiles[7],
        lineTension: 0.3,
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
        label: 'Label 9',
        data: Data.portfolioPercentiles[8],
        lineTension: 0.3,
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
        label: 'Label 10',
        data: Data.portfolioPercentiles[9],
        lineTension: 0.3,
        fill: false,
        borderColor: 'black',
        backgroundColor: 'transparent',
        pointBorderColor: 'black',
        pointBackgroundColor: 'black',
        pointHitRadius: 30,
        pointBorderWidth: 2,
        type: 'line'
      }],
       labels:["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"]
     },
     options:{
      scales:{
        yAxis:[{
          ticks:{
            beginAtZero: true,
            max: 50000
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



  







