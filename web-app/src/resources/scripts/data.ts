import { Inputs } from "./inputs";

export class Data {
  static instance: Data = new Data();
  historical: Result;
  projected: Result;
  inputs: Inputs = new Inputs();
}

export class Result {
  successRate: number;

  //Line Graph
  portfolioPercentiles: [[number]];

  //Histogram
  stocksReturnRateFrequencies: [number];
  stocksReturnRateXLabels: [number];
  bondsReturnRateFrequencies: [number];
  bondsReturnRateXLabels: [number];

  //Stacked bar
  stocksRetirementAmounts: [number];
  stocksEndAmounts: [number];

  bondsRetirementAmounts: [number];
  bondsEndAmounts: [number];
}
