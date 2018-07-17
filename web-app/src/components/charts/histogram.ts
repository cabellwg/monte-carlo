import { Chart } from 'chart.js';

export class Histogram {

  attached() {
    this.buildChart();
  }
   

  buildChart() {
    let ctx = (document.getElementById("histogram1") as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
     type: 'bar',
     data:{
       datasets:[{
         label: 'Histogram Label',
         data: [0,10,40,80,40,10,0],
         backgroundColor: "rgba(84, 111, 140, 0.74)",
       },
       {
         label: 'Line Label',
         data: [0,10,40,70,40,10,0],
         backgroundColor: "rgba(95, 2, 31, 0.47)",
         type: 'line'
       }],
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



  







