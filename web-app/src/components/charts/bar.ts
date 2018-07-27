import { bindable } from "aurelia-framework";
import { Chart } from 'chart.js';

export class Bar {
  //stocks
  @bindable stocksRetirement: [number];
  @bindable stocksEnd: [number];

  //bonds
  @bindable bondsRetirement: [number];
  @bindable bondsEnd: [number];
  
  @bindable barId: string;
  @bindable max: number;

  attached(){
    this.buildChart();
  }


  buildChart(){
    let ctx = (document.getElementById(this.barId) as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
      type: 'bar',
      data:{
        labels: ["Stocks", "Bonds"],
        datasets: [
          {
            label: '25th percentile at Retirement',
            backgroundColor: '#1067FD',
            data: [this.stocksRetirement[0], this.bondsRetirement[0]],
            stack: 'Retirement'
          },
          {
            label: '50th percentile at Retirement',
            backgroundColor: '#FD2600',
            data: [this.stocksRetirement[1], this.bondsRetirement[1]],
            stack: 'Retirement'
          },
          {
            label: '75th percentile at Retirement',
            data: [this.stocksRetirement[2], this.bondsRetirement[2]],
            backgroundColor: '#008577',
            stack: 'Retirement'
          },
          {
            label: '25th percentile at End',
            backgroundColor: '#1067FD',
            data: [this.stocksEnd[0], this.bondsEnd[0]],
            stack: 'End'
          },
          {
            label: '50th percentile at End',
            backgroundColor: '#FD2600',
            data: [this.stocksEnd[1], this.bondsEnd[1]],
            stack: 'End'
          },
          {
            label: '75th percentile at End',
            data: [this.stocksEnd[2], this.bondsEnd[2]],
            backgroundColor: '#008577',
            stack: 'End'
          }
        ]
      },
      options:{
        scales:{
          yAxes:[{
            type: 'logarithmic',
            ticks:{
              max: this.max * 10,
              beginAtZero: true,
              callback: function(value, index, values) {
                return String(value).charAt(0) == "1" || String(value).charAt(0) == "5" ? value : "";
              }
            },
           stacked: true,
          }]
        },
        legend: {position: 'bottom'},
      }
    });
  }
 }

