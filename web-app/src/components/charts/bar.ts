import { Result } from './../../resources/scripts/data';
import { bindable } from "aurelia-framework";
import { Chart } from 'chart.js';

export class BarChart {
  //stocks
  stocksRetirement: [number];
  stocksEnd: [number];

  //bonds
  bondsRetirement: [number];
  bondsEnd: [number];
  
  @bindable data: Result;
  @bindable barId: string;

  attached(){
    //stocks
    this.stocksRetirement = this.data.stocksRetirementAmounts
    this.stocksEnd = this.data.stocksEndAmounts

    //bonds
    this.bondsRetirement = this.data.bondsRetirementAmounts
    this.bondsEnd = this.data.bondsEndAmount

    this.buildChart();
  }

  buildChart(){
    console.log("Building Bar Chart")
    let ctx = (document.getElementById(this.barId) as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
      type: 'bar',
      data:{
        labels: ['Stocks', 'Bonds'],
        datasets: [
          {
            label: 'Retirement Amount',
            backgroundColor: 'red',
            data:[this.data.stocksRetirementAmounts, this.data.bondsRetirementAmounts],
          },
          {
            label: 'End Amount',
            data: [this.data.stocksEndAmounts, this.data.bondsEndAmount],
            backgroundColor: 'green',
          }
        ]
      },
      options:{
        scales:{
          yAxis:[{
            ticks:{
              beginAtZero: true,
            },
           type: 'linear',
          }]
        },
        legend: {position: 'bottom'},
      }
    });
  }
}
