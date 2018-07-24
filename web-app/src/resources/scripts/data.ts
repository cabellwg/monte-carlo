export class Data {
  static instance: Data;
  historical: Result;
  projected: Result;
}

export class Result {
  successRate: number;

  //Line Graph
  portfolioPercentiles: [[number]];

  //Histogram
  stocksReturnRateFrequencies: [number];
  stocksFrequencyPeak: number;
  stocksFrequencyScale: number;

  bondsReturnRateFrequencies: [number];
  bondsFrequencyPeak: number;
  bondsFrequencyScale: number;

  //Stacked bar
  stocksRetirementAmounts: [number];
  stocksEndAmounts: [number];

  bondsRetirementAmounts: [number];
  bondsEndAmount: [number];
}
