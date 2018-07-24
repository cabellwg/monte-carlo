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
        labels: ['Stocks', 'Bonds', 'Savings'],
        datasets: [
          {
            label: 'Retirement Amount',
            data:[this.data.stocksRetirementAmounts, this.data.bondsRetirementAmounts],
            backgroundColor: 'red',
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
          xAxis:[{stacked: true}],
          yAxis:[{
            stacked:true,
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
