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
    this.bondsEnd = this.data.bondsEndAmounts

    this.buildChart();
  }


  buildChart(){
    console.log("Building Bar Chart")
    let ctx = (document.getElementById(this.barId) as HTMLCanvasElement).getContext("2d");
    new Chart(ctx, {
      type: 'bar',
      data:{
        labels: ['25%', '50%', '75'],
        datasets: [
          {
            label: 'Retirement',
            backgroundColor: 'red',
            data: this.data.stocksRetirementAmounts,
            stack: ' Stocks Stack'
          },
          {
            label: 'Retirement',
            backgroundColor: 'pink',
            data: this.data.bondsRetirementAmounts,
            stack: 'Bond Stack'
          },
          
          {
            label: 'End',
            data: this.data.stocksEndAmounts,
            backgroundColor: 'red',
            stack: 'Stocks Stack'
          },
          {
            label: 'End',
            backgroundColor: 'pink',
            data: this.data.bondsEndAmounts,
            stack: 'Bond Stack'
          },
          
        ]
      },
      options:{
        scales:{
          xAxes: [{
            stacked: true,
          }],
          yAxes:[{
            ticks:{
              beginAtZero: true,
            },
           stacked: true,
          }]
        },
        legend: {position: 'bottom'},
      }
    });
  }
 }

