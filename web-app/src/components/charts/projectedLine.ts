import { Chart } from 'chart.js';

export class ProjectedLine {

  attached() {
    this.buildChart();
  }
   

  buildChart() {
    let ctx = (document.getElementById("projectedLine") as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
     type: 'line',
     data:{
       datasets:[{
         label: 'Label 1',
         data: [0, 59, 75, 20, 20, 55, 40],
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
         data: [20, 15, 60, 60, 65, 30, 70],
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
         data: [56, 78, 12, 30, 45, 15, 65],
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
        data: [60, 35, 20, 15, 90, 55, 40],
        lineTension: 0.3,
        fill: false,
        borderColor: 'green',
        backgroundColor: 'transparent',
        pointBorderColor: 'green',
        pointBackgroundColor: 'green',
        pointHitRadius: 30,
        pointBorderWidth: 2,
        type: 'line'
      }
      ],
       labels:["1", "2", "3", "4", "5", "6", "7"]
     },
     options:{
      scales:{
        yAxis:[{
          ticks:{
            beginAtZero: true
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



  







