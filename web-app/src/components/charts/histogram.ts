import {GoogleCharts} from 'google-charts';
import { Chart} from 'chart.js';
import {inject, bindable} from 'aurelia-framework';
import { EventAggregator } from "aurelia-event-aggregator";

export class histogram {

   ctx = (document.getElementById("myChart") as HTMLCanvasElement).getContext("2d");
   myChart = new Chart(this.ctx,{
     type: 'bar',
     data:{
       labels:["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
       datasets:[{
        label: '# of Votes',
        data: [12, 19, 3, 5, 2, 3],
        backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(255, 206, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(153, 102, 255, 0.2)',
            'rgba(255, 159, 64, 0.2)'
        ],
        borderColor: [
          'rgba(255,99,132,1)',
          'rgba(54, 162, 235, 1)',
          'rgba(255, 206, 86, 1)',
          'rgba(75, 192, 192, 1)',
          'rgba(153, 102, 255, 1)',
          'rgba(255, 159, 64, 1)'
      ],
      borderWidth: 1
       }]
     },
     options:{
       scales:{
         yAxis:[{
           ticks:{
             beginAtZero: true
           }
         }]
       }
     }
   });


}




  







