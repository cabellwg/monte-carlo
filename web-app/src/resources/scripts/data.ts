import { Inputs } from "./inputs";

export class Data {
  static instance: Data;
  historical: Result;
  projected: Result;
  inputs: Inputs;
}

export class Result {
  successRate: number;

  //Line Graph
  portfolioPercentiles: [[number]];

  //Histogram
  stocksReturnRateFrequencies: [number];
  bondsReturnRateFrequencies: [number];

  //Stacked bar
  stocksRetirementAmounts: [number];
  stocksEndAmounts: [number];

  bondsRetirementAmounts: [number];
  bondsEndAmount: [number];
}
