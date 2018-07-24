import { Chart } from 'chart.js';
import { Result } from './../../resources/scripts/data';
import { bindable } from "aurelia-framework";


export class HistogramBonds{

  bondsReturnRate: [number];
  bondsPeak: number;
  bondsScale: number;

  @bindable data: Result;
  @bindable bondsId: string

  attached(){
    this.bondsReturnRate = this.data.bondsReturnRateFrequencies;
    this.bondsPeak = this.data.bondsFrequencyPeak;
    this.bondsScale = this.data.bondsFrequencyScale;
    this.buildChart();
  }

  buildChart(){
    console.log("Bonds Histogram is loading")
    let ctx = (document.getElementById(this.bondsId) as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
      type: 'bar',
      labels:["Income Range 1", "Income Range 2", "Income Range 3"],
      data:{
        datasets:[{
          label: "Bonds Histogram",
          data: [this.data.bondsReturnRateFrequencies, this.data.bondsFrequencyPeak, this.data.bondsFrequencyScale],
          backgroundColor: "rgba(84, 111, 140, 0.74)",
        }],
        /*
      {
        label: 'Bonds Line',
        data: [this.data.bondsReturnRateFrequencies, this.data.bondsFrequencyPeak, this.data.bondsFrequencyScale],
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
