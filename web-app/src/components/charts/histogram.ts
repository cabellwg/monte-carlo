import { Chart } from 'chart.js';
import { bindable } from 'aurelia-framework';

export class Histogram {

  idealDistribution: [number];
  chart: Chart;
  canBuildNewChart = false;

  @bindable returnRates: [number];
  @bindable histogramId: string
  

  attached() {
    this.generateIdeal();

    this.buildChart();
    this.canBuildNewChart = true;
  }

  returnRatesChanged() {
    if (this.canBuildNewChart) {
      this.buildChart();
    }
  }

  generateIdeal() {
    this.idealDistribution = new Array() as [number];
    for (let i = 0; i < this.returnRates.length; i++) {
      let normal = this.returnRates[Math.floor(this.returnRates.length / 2)] * Math.exp(-Math.pow(i - Math.floor(this.returnRates.length / 2), 2) / 18);
      this.idealDistribution.push(normal);
    }
  }

  buildChart() {
    if (this.chart) {
      this.chart.destroy();
    }
    
    let ctx = (document.getElementById(this.histogramId) as HTMLCanvasElement).getContext("2d");
    this.chart = new Chart(ctx, {
     type: 'bar',
     data:{
       labels: this.returnRates.map((_,index)=>index),
       datasets:[{
         label: "Bar",
         data: this.returnRates,   
         backgroundColor: "#26A65B",
      },
      {
        label: 'Bell Curve',
        data: this.idealDistribution,
        lineTenstion: 0,
        fill: false,
        borderColor: "#212224",
        backgroundColor: 'transparent',
        pointBackgroundColor: "#212422",
        pointBorderWidth: 0,
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



  







