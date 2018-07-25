import { Chart } from 'chart.js';
import { bindable } from 'aurelia-framework';

export class Histogram {

  idealDistribution: [number];

  @bindable returnRates: [number];
  @bindable histogramId: string

  
  attached() {

    this.generateIdeal();

    this.buildChart();
  }

  generateIdeal() {
    this.idealDistribution = new Array() as [number];
    for (let i = 0; i < this.returnRates.length; i++) {
      let normal = this.returnRates[Math.floor(this.returnRates.length / 2)] * Math.exp(-Math.pow(i - Math.floor(this.returnRates.length / 2), 2) / 18);
      this.idealDistribution.push(normal);
    }
  }

  buildChart() {
    console.log("Histogram Chart is Building")
    let ctx = (document.getElementById(this.histogramId) as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
     type: 'bar',
     data:{
       labels: this.returnRates.map((_,index)=>index),
       datasets:[{
         label: "Stocks Histogram",
         data: this.returnRates,
         backgroundColor: "rgba(84, 111, 140, 0.74)",
      },
      {
        label: 'Stocks Line',
        data: this.idealDistribution,
        backgroundColor: "rgba(95, 2, 31, 0.47)",
        type: 'line'
      }],
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



  







