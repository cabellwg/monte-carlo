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
    console.log("Histogram Chart is Building")
    let ctx = (document.getElementById(this.histogramId) as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
     type: 'bar',
     labels:["Income Range 1", "Income Range 2", "Income Range 3"],
     data:{
       datasets:[{
         label: "Stocks Histogram",
         data: [this.data.stocksReturnRateFrequencies, this.data.stocksFrequencyPeak, this.data.stocksFrequencyScale],
         backgroundColor: "rgba(84, 111, 140, 0.74)",
       }], 
       /*
       {
         label: 'Stocks Line',
        data: [this.data.stocksReturnRateFrequencies, this.data.stocksFrequencyPeak, this.data.stocksFrequencyScale],
         backgroundColor: "rgba(95, 2, 31, 0.47)",
         type: 'line'
       }],*/
     },
     options:{
      scales:{
        yAxis:[{
          ticks:{
            beginAtZero: true,
            stacked: true,
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



  







