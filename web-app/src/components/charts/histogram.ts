import { Chart } from 'chart.js';
import { Result } from './../../resources/scripts/data';
import { bindable } from 'aurelia-framework';

export class Histogram {

  //Stocks Histogram
  stocksReturnRate: [number];
  stocksPeak: number;
  stocksScale: number;

  //Bonds Histogram
  bondsReturnRate: [number];
  bondsPeak: number;
  bondsScale: number;

  @bindable data: Result;
  @bindable histogramId: string

  
  attached() {
    this.stocksReturnRate = this.data.stocksReturnRateFrequencies;
    this.stocksPeak = this.data.stocksFrequencyPeak;
    this.stocksScale = this.data.stocksFrequencyScale;

    this.bondsReturnRate = this.data.stocksReturnRateFrequencies;
    this.bondsPeak = this.data.stocksFrequencyPeak;
    this.bondsScale = this.data.stocksFrequencyScale;


    this.buildChart();
  }
   

  buildChart() {
    let ctx = (document.getElementById(this.histogramId) as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
     type: 'bar',
     data:{
       datasets:[{
         label: 'Histogram Label',
         data: [this.stocksReturnRate, this.stocksPeak, this.stocksScale, this.bondsReturnRate, this.bondsPeak, this.bondsScale],
         backgroundColor: "rgba(84, 111, 140, 0.74)",
       },
       {
         label: 'Line Label',
         data: [10,56,78,56,54,78],
         backgroundColor: "rgba(95, 2, 31, 0.47)",
         type: 'line'
       }],
       labels:["1", "2", "3", "4", "5", "6"]
     },
     options:{
      scales:{
        yAxis:[{
          ticks:{
            beginAtZero: true,
            min: 0,
            max: 25000
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



  







