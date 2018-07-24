export class Data {
  static instance: Data;
  historical: Result;
  projected: Result;
}

export class Result {
  successRate: number;

  portfolioPercentiles: [[number]];

  stocksReturnRateFrequencies: [number];
  stocksFrequencyPeak: number;
  stocksFrequencyScale: number;

  bondsReturnRateFrequencies: [number];
  bondsFrequencyPeak: number;
  bondsFrequencyScale: number;

  stocksRetirementAmounts: [number];
  stocksEndAmounts: [number];

  bondsRetirementAmounts: [number];
  bondsEndAmount: [number];
}
