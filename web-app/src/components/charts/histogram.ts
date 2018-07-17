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
         backgroundColor: "rgb(4, 147, 114)",
       },
       {
         label: 'Line Label',
         data: [0,10,40,70,40,10,0],
         backgroundColor: "rgba(255, 99, 132, 0.2)",
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



  







